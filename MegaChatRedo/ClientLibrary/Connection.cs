using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;

namespace ClientLibrary
{
    public class Connection : IDisposable,IChatClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private ClientListener cnListener;
        private ClientSender cnSender;
        private StreamWriter Writer;
        private StreamReader Reader;
        public event Action<MessageBase> MessageReceived;

        private void Start()
        {
            cnListener.OnMessageReceived += OnBounceMessage;
            cnListener.Start();
            cnSender.Start();
        }

        private void OnBounceMessage(MessageBase obj)
        {
            MessageReceived?.Invoke(obj);
        }

        public bool Connect(string ip, int port, string myName)
        {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
            cnListener = new ClientListener(Reader);
            cnSender = new ClientSender(Writer);

            Writer.WriteLine(myName);
            Writer.Flush();
            var greet = Reader.ReadLine();

            if (greet != "AC")
            {
                Dispose();
                return false;
            }
            Start();
            return true;
        }

        public void SendMessage(MessageBase message)
        {
            cnSender.SendMessage(message);
        }

        public void Dispose()
        {
            cnListener.Dispose();
            cnListener = null;
            cnSender.Dispose();
            cnSender = null;
            stream.Close();
            stream.Dispose();
            stream = null;
            client.Close();
            client = null;
            Console.WriteLine("Connection aborted!");
        }
    }
}
