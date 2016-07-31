using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ClientLibrary
{
    public interface IChatClient
    {
        bool Connect(string ip, int portNo, string myName);
        void SendMessage(MessageBase message);
        event Action<MessageBase> MessageReceived;
    }
}
