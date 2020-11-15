using System;

namespace Scorpion_MDB
{
    public static class scmain
    {
        public static void Main(string[] args)
        {
            Scorpion sc = new Scorpion();
            return;
        }
    }

    public class Scorpion
    {
        Scorpion_TCP tcp;
        static Scorpion_MONGO scmdb;

        public Scorpion()
        {
            //Start TCP
            write_cui("Scorpion MONGODB hub v0.1b 2020+ <Oscar Arjun Singh Tark>");
            write_cui("Please enter a valid IP address to bind to and a valid PORT:");
            try
            {
                tcp = new Scorpion_TCP(Console.ReadLine(), Convert.ToInt32(Console.ReadLine()), this);
                write_network("Scorpion MONGODB hub v0.1b 2020+ <Oscar Arjun Singh Tark>: Started successfully on PORT 8554");
            }
            catch(Exception erty) { write_error("Unable to establish a network server. ERROR: " + erty.Message); }
            scmdb = new Scorpion_MONGO(this);
            while (true)
                execute_command(Console.ReadLine());
        }

        public void execute_command(string command)
        {
            //function::mongocommand
            Console.ForegroundColor = ConsoleColor.White;
            //string[] command_vars = split_command(ref command);
            //scmdb.do_mongo(command_vars);
            //return;
            scmdb.scorpion(command);
            return;
        }

        static string[] unwanted = { "::", "*" };
        public string[] split_command(ref object command)
        {
            return ((string)command).Replace("\n", "").Replace(" ", "").Replace(",", "").Split(unwanted, StringSplitOptions.RemoveEmptyEntries);
        }

        public void write_success(string STR_)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}", STR_);
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        public void write_cui(string STR_)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}", STR_);
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        public void write_error(string STR_)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}", STR_);
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        public void write_network(string STR_)
        {
            tcp.client_broadcast(STR_);
            return;
        }
    }

    class EngineFunctions
    {
        public string replace_fakes(string Scorp_Line)
        {
            return Scorp_Line.Replace("{&var}", "*").Replace("{&quot}", "'");
        }

        public string replace_telnet(string Scorp_Line)
        {
            return Scorp_Line.Replace("\n", "").Replace("\r", "").Replace("959;1R", "").Replace("U+0013", "");
        }
    }
 }