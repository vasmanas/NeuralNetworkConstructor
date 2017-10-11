using System;
using System.Runtime.Serialization;

namespace NeuralNetworkConstructor.Core.Messaging
{
    [DataContract]
    public abstract class Command : Message
    {
        public Command() : base()
        {
        }

        public Command(Guid conversationId) : base(conversationId)
        {
        }
    }
}
