using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class ChatServer
    {
        protected TcpListener PortListener;

        public ChatServer(int port)
        {
            PortListener = new TcpListener(IPAddress.Parse("127.0.0.1"),port);
            PortListener.Start();
            Console.WriteLine("Starting to listen on port " + port + "...");
            Run();
        }

        public async void Run()
        {
            while (true)
            {
                var client = await PortListener.AcceptTcpClientAsync();
                new Person(client);
                Console.WriteLine("New user connected!");
            }
        }
    }
}
