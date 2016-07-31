using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ServerLibrary
{
    public class Sender : IDisposable
    {
        public ConcurrentQueue<MessageBase> SendQueue;
        private Task internalprocess;
        private bool isactive = true;
        private Person person;

        public Sender(Person person)
        {
            SendQueue = new ConcurrentQueue<MessageBase>();
            this.person = person;
        }
        public void Start()
        {
            internalprocess = Task.Run(() => SendLoop());
        }
        private void SendLoop()
        {
            while (isactive)
            {
                MessageBase result;
                if (SendQueue.TryDequeue(out result))
                {
                    person.Writer.WriteLine(Serializer.Serialize(result));
                    person.Writer.Flush();
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
            internalprocess.Wait();
        }
    }
}
