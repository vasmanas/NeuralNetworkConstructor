using System;
using System.Runtime.Serialization;

namespace NeuralNetworkConstructor.Core.Messaging
{
    [DataContract]
    public class Event : Message
    {
        public Event() : base()
        {
        }

        public Event(Guid conversationId) : base(conversationId)
        {
        }
    }
}
