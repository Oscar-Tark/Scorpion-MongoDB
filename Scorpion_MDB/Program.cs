using System;

namespace Scorpion_MDB
{
    public static class scmain
    {
        public static void Main(string[] args)
        {
            Scorpion sc = new Scorpion();
        }
    }

    public class Scorpion
    {
        Scorpion_TCP tcp;
        static Scorpion_MONGO scmdb = new Scorpion_MONGO();

        public Scorpion()
        {
            //Start TCP
            write_cui("Scorpion MONGODB hub v0.1b 2020+ <Oscar Arjun Singh Tark>");
            write_cui("Please enter a valid TCP port to bind to:");
            try
            {
                tcp = new Scorpion_TCP(Convert.ToInt32(Console.ReadLine()), this);
            }
            catch(Exception erty) { write_error("Unable to establish a network server due to: " + erty.Message); }
            //scmdb = new Scorpion_MONGO();
            //scmdb.mongodbfind();
            while (true)
                execute_command(Console.ReadLine());
        }

        public void execute_command(string command)
        {
            //mongo::function::mongocommand
            Console.ForegroundColor = ConsoleColor.White;
            if (command == "exit")
                Console.WriteLine("Server is running: {0}", tcp.stop_server());
            else if (command.StartsWith("mongo::", StringComparison.CurrentCulture))
            {
                string[] command_vars = split_command(ref command);
                scmdb.do_mongo(ref command_vars);
            }
        }

        static string[] unwanted = { "::", "*" };
        public static string[] split_command(ref string command)
        {
            return command.Replace("\n", "").Replace(" ", "").Replace(",", "").Split(unwanted, StringSplitOptions.RemoveEmptyEntries);
        }

        private void write_cui(string STR_)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}", STR_);
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        private void write_error(string STR_)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}", STR_);
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
    }
}
