using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ServerLibrary
{
    public class Person : IDisposable
    {
        public static Dictionary<string, Person> Map = new Dictionary<string, Person>();

        private TcpClient client;
        private NetworkStream stream;
        public Sender PersonalSender;
        public Listener PersonalListener;
        public StreamWriter Writer { get; }
        public StreamReader Reader { get; }
        public string Name { get; private set; }

        public Person(TcpClient client)
        {
            this.client = client;
            stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
            Task.Run(GetName).ContinueWith( t => StartConversations(), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        void StartConversations()
        {
            PersonalSender = new Sender(this);
            PersonalSender.Start();
            PersonalListener = new Listener(this);
            PersonalListener.Start();
        }

        private bool NameChecker(string s)
        {
            if (s == null) return false;
            if (Map.ContainsKey(s)) return false;
            if (s.Any(char.IsWhiteSpace)) return false;
            return true;
        }

        private async Task GetName()
        {
            Name = null;
            while (!NameChecker(Name))
            {
                try
                {
                    Name = await Reader.ReadLineAsync();
                    Console.WriteLine(Name);
                }
                catch (Exception e)
                {
                    Dispose();
                    return;
                }
                if (NameChecker(Name) == false)
                {
                    Writer.WriteLine("Invalid name!");
                    Dispose();
                    return;
                }
            }
            Map.Add(Name, this);
            Console.WriteLine("User name set: " + "   " + Name);
            Writer.WriteLine("AC");
            Writer.Flush();
        }

        public void Dispose()
        {
            if (Name!=null && Map.ContainsKey(Name)) Map.Remove(Name);
            PersonalSender?.Dispose();
            PersonalListener?.Dispose();
            stream.Close();
            stream.Dispose();
            stream = null;
            client.Close();
            client = null;
            Console.WriteLine( "User disconnected!      name: " + (Name ?? "Invalid name") );
        }
    }
}
