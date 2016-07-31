using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ClientLibrary
{
    public class ClientSender : IDisposable
    {
        private readonly ConcurrentQueue<MessageBase> SendQueue;
        private readonly StreamWriter internalWriter;
        private Task internalProcess;
        private bool isactive = true;

        public ClientSender(StreamWriter wr)
        {
            SendQueue = new ConcurrentQueue<MessageBase>();
            internalWriter = wr;
        }

        public void Start()
        {
            internalProcess = Task.Run(() => SendLoop());
        }

        private void SendLoop()
        {
            while (isactive)
            {
                MessageBase result;
                if (SendQueue.TryDequeue(out result))
                {
                    internalWriter.WriteLine(Serializer.Serialize(result));
                    internalWriter.Flush();
                }
            }
        }

        public void SendMessage(MessageBase msg)
        {
            SendQueue.Enqueue(msg);
        }

        public void Dispose()
        {
            isactive = false;
            internalProcess?.Wait();
        }
    }
}
