using System;
using System.Runtime.Serialization;

namespace NeuralNetworkConstructor.Core.Messaging
{
    [DataContract]
    public abstract class Message
    {
        protected Message() : this(Guid.Empty)
        {
        }

        protected Message(Guid conversationId)
        {
            this.Id = Guid.NewGuid();
            this.ConversationId = conversationId;
        }

        [DataMember]
        public Guid Id { get; private set; }


        [DataMember]
        public Guid ConversationId { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}:Id({1});ConversationId({2})", this.GetType().Name, this.Id, this.ConversationId);
        }
    }
}
