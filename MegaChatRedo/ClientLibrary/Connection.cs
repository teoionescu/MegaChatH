﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void Disconnect()
        {
            Dispose();
        }

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

        public string Connect(string ip, int port, string myName)
        {
            string greet;
            try
            {
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                Reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);
                cnListener = new ClientListener(Reader);
                cnSender = new ClientSender(Writer);

                Writer.WriteLine(myName);
                Writer.Flush();
                greet = Reader.ReadLine();
            }
            catch (Exception e)
            {
                greet = e.Message;
            }

            if (greet != "AC")
            {
                Dispose();
                Trace.WriteLine(greet);
                return greet;
            }
            Start();
            return null;
        }

        public void SendMessage(MessageBase message)
        {
            cnSender.SendMessage(message);
        }

        public void Dispose()
        {
            cnSender?.Dispose();
            cnSender = null;
            stream?.Close();
            cnListener?.Dispose();
            cnListener = null;
            stream?.Dispose();
            stream = null;
            client?.Close();
            client = null;
        }
    }
}
