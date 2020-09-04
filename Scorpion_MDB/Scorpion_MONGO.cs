using System;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Libmongocrypt;

namespace Scorpion_MDB
{
    public class Scorpion_MONGO
    {
        Scorpion Do_on;
        struct Mongo_settings
        { 
            
        };

        public Scorpion_MONGO(Scorpion fm1)
        {
            Do_on = fm1;
            return;
        }

        public void do_mongo(ref string[] command)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">>> Executing: {0}", command[0]);
            this.GetType().GetMethod(command[0], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(this, new object[] { command });
            return;
        }

        //MONGO DB
        public void mongodbsetone(ref string[] command)
        {
            //*database, *collection, *value
            Console.WriteLine(command[0]);
            Console.WriteLine(command[1]);
            Console.WriteLine(command[2]);
            Console.WriteLine(command[3]);

            var client = new MongoClient();
            var db = client.GetDatabase(command[1]);
            var collection = db.GetCollection<BsonDocument>(command[2]);
            var document = new BsonDocument { { "user", "f" } };

            collection.InsertOne(document);

            return;
        }

        public void mongodbfind(ref string[] command)
        {
            //*database, *collection, *value
            var client = new MongoClient();
            var db = client.GetDatabase(command[1]);
            var collection = db.GetCollection<BsonDocument>(command[2]);
            var filter = Builders<BsonDocument>.Filter.Eq("", 10);
            var document = collection.Find(filter).FirstOrDefault();
            System.Console.WriteLine(document.ToString());

            return;
        }

        public void mongolist(ref string[] command)
        {
            //::
            MongoClient dbClient = new MongoClient();
            var dbList = dbClient.ListDatabases().ToList();

            Do_on.write_cui("The list of databases on this server is: ");
            foreach (var db in dbList)
                Do_on.write_cui(db.ToJson());

            return;
        }
    }
}
