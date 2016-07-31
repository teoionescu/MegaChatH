using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public abstract class MessageBase
    {
        public abstract override string ToString();
    }

    public class ChatMessage : MessageBase
    {
        public string Source;
        public string Destination;
        public string Body;
        public DateTime TimeSent { get; set; }

        public override string ToString()
        {
            return $"{Source}->{Destination}:{Body}";
        }
    }
}
