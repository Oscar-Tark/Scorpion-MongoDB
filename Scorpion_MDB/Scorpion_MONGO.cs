using System;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Libmongocrypt;

namespace Scorpion_MDB
{
    public class Scorpion_MONGO
    {
        struct Mongo_settings
        { 
        
        };

        public void do_mongo(ref string[] command)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">>> Executing: {0}", command[1]);
            this.GetType().GetMethod(command[1], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(this, new object[] { command });
            return;
        }

        //MONGO DB
        public void mongodbset(ref string[] command)
        {
            //*database, *criteria
            var client = new MongoClient();
            var db = client.GetDatabase("test");
            var collection = db.GetCollection<BsonDocument>("cars");

            BsonElement[] bs_elem;
            //bs_elem.SetValue("BMW", 1);

            BsonDocument bs = new BsonDocument();
            //System.Console.WriteLine(collection.Indexes);
            //var document = collection.Find(new BsonDocument()).FirstOrDefault();
            //System.Console.WriteLine(document.ToString());

            return;
        }

        public void mongodbfind(ref string[] command)
        {
            //*database, *criteria
            var client = new MongoClient();
            var db = client.GetDatabase(command[2]);
            var collection = db.GetCollection<BsonDocument>(command[3]);

            System.Console.WriteLine(collection.Indexes);

            var document = collection.Find(new BsonDocument()).FirstOrDefault();
            System.Console.WriteLine(document.ToString());

            return;
        }

        public void mongotest(ref string[] command)
        {
            Console.WriteLine("OK test {0} {1} {2}", command[0], command[1], command[2]);
            return;
        }
    }
}
