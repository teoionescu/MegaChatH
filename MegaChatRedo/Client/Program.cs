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
            string name = null;
            do {
                Console.WriteLine("Insert name:");
                name = Console.ReadLine();
            }
            while (!ClientConnection.Connect("localhost", 8888, name));

            while (true)
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
            }
        }

        private static void OnMessageReceived(MessageBase msg)
        {
            Console.WriteLine(msg.ToString());
        }
    }
}
