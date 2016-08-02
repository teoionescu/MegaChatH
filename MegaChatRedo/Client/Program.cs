using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientLibrary;
using CommonLibrary;

namespace Client
{
    class Program
    {
        private static IChatClient ClientConnection = new Connection();
        
        static void Main(string[] args)
        {
            ClientConnection.MessageReceived += OnMessageReceived;
            while (true)
            {
                string result = null;
                do
                {
                    if(result != null) Console.WriteLine(result);
                    Console.WriteLine("Insert name:");
                    var name = Console.ReadLine();
                    result = ClientConnection.Connect("192.168.0.5", 8888, name);
                }
                while (result != null);



                var connected = true;
                while (connected)
                {
                    var body = Console.ReadLine();
                    var words = body.Split(' ');
                    if (words.Length > 1)
                    {
                        ChatMessage msg = new ChatMessage();
                        msg.Source = null;
                        msg.Destination = words[0];
                        msg.Body = string.Join(" ", words.Where((s, i) => i > 0));
                        ClientConnection.SendMessage(msg);
                    }
                    else
                    {
                        if (words[0] == "list")
                        {
                            ListMessage msg = new ListMessage();
                            ClientConnection.SendMessage(msg);
                        }
                        if (words[0] == "logout")
                        {
                            ClientConnection.Disconnect();
                            connected = false;
                        }
                    }
                }
                Console.WriteLine("Disconnected");
            }
        }

        private static void OnMessageReceived(MessageBase msg)
        {
            Console.WriteLine(msg.ToString());
        }
    }
}
