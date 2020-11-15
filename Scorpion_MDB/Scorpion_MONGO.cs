using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.Threading;

namespace Scorpion_MDB
{
    public class Scorpion_MONGO
    {
        Scorpion Do_on; private Retrievalengine re__;
        private Retrievalengine[] re_instance;
        private string[] re_ref;

        const int max_instances = 10;
        int instance_count = 0;

        public Scorpion_MONGO(Scorpion fm1)
        {
            Do_on = fm1;

            //Maximum supported instances is 10
            re_instance = new Retrievalengine[max_instances];
            re_ref = new string[max_instances];

            Do_on.write_cui("Use the start function in order to start the retrieval function:\n\nstart::*URL *remote_to_get_date *local_db *local_collection\n***************************************************\n\n");
            return;
        }

        public void scorpion(object Scorp_Line)
        {
            try
            {
                Thread ths = new Thread(new ParameterizedThreadStart(do_mongo));
                ths.IsBackground = true;
                ths.Start(Scorp_Line);
            }
            catch { Console.WriteLine("FATAL: COULD NOT START ENGINE THREAD"); }
            return;
        }

        public void do_mongo(object exec)
        {
            string[] command = (string[])Do_on.split_command(ref exec);
            Console.ForegroundColor = ConsoleColor.Yellow;
            try
            {
                this.GetType().GetMethod(command[0], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(this, new object[] { command });
                Do_on.write_success("Executed >> " + command[0]);
            }
            catch(Exception erty) { Do_on.write_error(erty.Message); }
            return;
        }

        //Contains basic functions in order to control the RETRIEVAL ENGINE
        /*
        Retrieval engine retrieves data from a specific service that supports JSON
        */
        public void start(ref string[] command)
        {
            //::*URL *interval, *db, *collection, *instance_name
            int this_instance = 0;
            try
            {
                if (instance_count == max_instances)
                    return;
                this_instance = instance_count++;
                re_instance[this_instance] = new Retrievalengine();
                Console.WriteLine("{0}, {1}, {2}, {3}", command[1], command[2], command[3], command[4]);
                re_instance[this_instance].__start(command[1], command[2], command[3], command[4], ref this_instance);
                re_ref[this_instance] = command[5];
            }
            catch(Exception e) { Console.WriteLine(e.Message); }
            return;
        }

        public void stop(ref string[] command)
        {
            //::*name
            for(int i = 0; i < max_instances; i++)
            {
                if (re_ref[i] == command[1])
                {
                    if (re_instance[i].__stop())
                    {
                        re_ref[i] = null;
                        re_instance[i].Dispose(true);
                        re_instance[i] = null;
                    }
                }
            }
            re__.__stop();
        }
    }

    class Retrievalengine
    {
        settings se; Thread th_eng; Thread th_monitor; Scorpion_JSON json; MONGODB mongodb;
        private struct settings
        {
            public int interval; //Interval in seconds
            public string URL; //Always ending with /
            public string db;
            public string collection;
            public int instance_index;
            public string date;
        };

        public void Dispose(bool v)
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
            GC.Collect();
            return;
        }

        public void __start(string URL, string date, string db, string collection, ref int instance_index)
        {
            Console.WriteLine("Building new instance [Instance: " + instance_index + "]");
            Console.WriteLine("Building settings...");

            se = new settings();
            se.instance_index = instance_index;
            se.URL = URL;
            se.db = db;
            se.date = date;
            se.collection = collection;

            json = new Scorpion_JSON();
            mongodb = new MONGODB();
            
            Console.WriteLine("Starting main thread...");
            ThreadStart ths_eng = new ThreadStart(ENG);
            th_eng = new Thread(ths_eng);
            th_eng.Priority = ThreadPriority.AboveNormal;
            th_eng.Start();
            Console.WriteLine("Main thread started...");
            return;
        }

        public bool __stop()
        {
            th_eng.Abort();
            th_monitor.Abort();

            if(th_eng.ThreadState == ThreadState.Aborted && th_monitor.ThreadState == ThreadState.Aborted)
                return true;
            return false;
        }

        //Retrieval functions
        private void ENG()
        {
            get_data(null);
            //Timer tms = new Timer(get_data);
            //tms.Change(0, se.interval * 1000);
            return;
        }

        private void get_data(object state)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Collecting data from [" + se.URL + "]");
            string JSON = json.JSON_post_auth(se.URL, "admin", "1234", se.date);

            //Do Mongo
            mongodb.setfromJSON(ref se.db, ref se.collection, ref JSON);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success >> Collecting data from [" + se.URL + "]; instance[" + se.instance_index + "]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            return;
        }
    }

    class MONGODB
    {
        //MONGO DB
        public void setfromJSON(ref string database, ref string collect, ref string JSON)
        {
            //*database, *collection, *ref, *value, *ref, *value
            //How to add values:
            /*
            *ref, *val, *ref, *val
            */
            var client = new MongoClient();
            var db = client.GetDatabase(database);
            var collection = db.GetCollection<BsonDocument>(collect);
            var BsonArray = BsonSerializer.Deserialize<BsonArray>(JSON);
            var document = new BsonDocument();

            Console.ForegroundColor = ConsoleColor.Green;
            var JSON__ = BsonArray.ToJson();
            Console.WriteLine("Got {0} characters as JSON", JSON__.Length);
            document.Add("Scorpion_Data", BsonArray);
            collection.InsertOneAsync(document);
            Console.WriteLine("Successfully inserted retrieved JSON into " + database + "." + collection);
            return;
        }

        public void find(ref string[] command)
        {
            //*database, *collection, *filter, *limit
            var client = new MongoClient();
            Console.WriteLine(command[1]);
            var db = client.GetDatabase(command[1]);
            var collection = db.GetCollection<BsonDocument>(command[2]);
            var filter = Builders<BsonDocument>.Filter.Eq("a", 10);
            var document = collection.Find(filter).FirstOrDefault();


            System.Console.WriteLine(document.ToString());

            return;
        }

        public void mongolist(ref string[] command)
        {
            //::
            MongoClient dbClient = new MongoClient();
            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
                Console.WriteLine(db.ToJson());

            return;
        }
    }
}
