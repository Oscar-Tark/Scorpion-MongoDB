using System;
using System.Net;
using System.Security;
using System.Text;
using SimpleTCP;

namespace Scorpion_MDB
{
    public class Scorpion_TCP
    {
        SimpleTcpServer tcp = new SimpleTcpServer();
        SimpleTcpClient tcp_cl = new SimpleTcpClient();
        Scorpion do_on;
        private static readonly string rsa_private_key = "/home/" + Environment.UserName + "/.scorpionharvester/rsa/keys/private";
        private static readonly string rsa_public_key = "/home/" + Environment.UserName + "/.scorpionharvester/rsa/keys/public";

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
            tcp_cl.StringEncoder = Encoding.UTF8;
            tcp_cl.DataReceived += Tcp_Cl_DataReceived;
            return tcp_cl.TcpClient.Connected;
        }

        public void client_broadcast(string message)
        {
            message = "output::{&var}{&quot}" + message + "{&quot}";
            byte[] data = Scorpion_RSA.Scorpion_RSA.encrypt_data(message, rsa_public_key);
            tcp_cl.Write(data);
            return;
        }

        void Tcp_Cl_DataReceived(object sender, Message e)
        {
            IPEndPoint ipep = (IPEndPoint)e.TcpClient.Client.RemoteEndPoint;
            IPAddress ipa = ipep.Address;
            Console.ForegroundColor = ConsoleColor.Blue;

            SecureString key = Scorpion_RSA.Scorpion_RSA.get_private_key_file(rsa_private_key);
            byte[] data = Scorpion_RSA.Scorpion_RSA.decrypt_data(e.Data, key);
            string s_data = To_String(data);

            EngineFunctions ef__ = new EngineFunctions();
            string command = ef__.replace_fakes(ef__.replace_telnet(e.MessageString));
            Console.WriteLine("[NETWORK:{1}] {0}", command.TrimEnd(new char[] { Convert.ToChar(0x13) }), ipa);
            do_on.execute_command(command.TrimEnd(new char[] { Convert.ToChar(0x13) }));
            return;
        }

        public string To_String(byte[] byt)
        {
            return Encoding.Default.GetString(byt);
        }
    }
}
