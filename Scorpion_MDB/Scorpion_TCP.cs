using System;
using System.Net;
using SimpleTCP;

namespace Scorpion_MDB
{
    public class Scorpion_TCP
    {
        SimpleTcpServer tcp = new SimpleTcpServer();
        SimpleTcpClient tcp_cl = new SimpleTcpClient();
        Scorpion do_on;

        public Scorpion_TCP(string url, int port, Scorpion fm1)
        {
            //Alaways uses non localhost IP
            do_on = fm1;
            Console.WriteLine("Connected: {0} on {1}:{2}", start_client(ref url, ref port), url, port);
            return;
        }

        public bool start_client(ref string url, ref int port)
        {
            tcp_cl.Connect(url, port);
            tcp_cl.Delimiter = 0x13;
            tcp_cl.DataReceived += Tcp_Cl_DataReceived;
            return tcp_cl.TcpClient.Connected;
        }

        public void client_broadcast(string message)
        {
            tcp_cl.WriteLine("output::{&var}{&quot}" + message + "{&quot}");
            return;
        }

        void Tcp_Cl_DataReceived(object sender, Message e)
        {
            IPEndPoint ipep = (IPEndPoint)e.TcpClient.Client.RemoteEndPoint;
            IPAddress ipa = ipep.Address;
            Console.ForegroundColor = ConsoleColor.Blue;
            EngineFunctions ef__ = new EngineFunctions();
            string command = ef__.replace_fakes(ef__.replace_telnet(e.MessageString));
            Console.WriteLine("[NETWORK:{1}] {0}", command.TrimEnd(new char[] { Convert.ToChar(0x13) }), ipa);
            do_on.execute_command(command.TrimEnd(new char[] { Convert.ToChar(0x13) }));
            return;
        }

        //Depreciated. Only a client will be used now
        /*public bool start_server(ref int port)
        {
            tcp.Start(port, false);
            tcp.ClientConnected += Tcp_ClientConnected;
            tcp.DataReceived += Tcp_DataReceived;
            return tcp.IsStarted;
        }

        public bool stop_server()
        {
            tcp.Stop();
            return tcp.IsStarted;
        }

        void Tcp_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            //Executes mongo
            Console.WriteLine("Client Connected {0}");
            return;
        }

        void Tcp_DataReceived(object sender, Message e)
        {
            IPEndPoint ipep = (IPEndPoint)e.TcpClient.Client.RemoteEndPoint;
            IPAddress ipa = ipep.Address;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Got string: {0} from {1}", e.MessageString, ipa);
            do_on.execute_command(e.MessageString);
        }*/
    }
}
