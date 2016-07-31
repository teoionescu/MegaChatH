using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ClientLibrary
{
    public class ClientListener : IDisposable
    {
        private readonly StreamReader internalReader;
        private Task internalProcess;
        private bool isactive = true;

        public ClientListener(StreamReader wr)
        {
            internalReader = wr;
        }

        public void Start()
        {
            internalProcess = Task.Run(ListenLoop);
        }

        private async Task ListenLoop()
        {
            while (isactive)
            {
                var rd = await internalReader.ReadLineAsync();
                var receivedMessage = Serializer.Deserialize(rd);
                OnMessageReceived?.Invoke(receivedMessage);
            }
        }

        public event Action<MessageBase> OnMessageReceived;

        public void Dispose()
        {
            isactive = false;
            internalProcess?.Wait();
        }
    }
}
