using System;

namespace NeuralNetworkConstructor.Core.Messaging
{
    /// <summary>
    /// Request.
    /// </summary>
    /// <typeparam name="TResponse"> Response type. </typeparam>
    public class Request<TResponse> : Message
    {
        public Request() : base()
        {
        }

        public Request(Guid conversationId) : base(conversationId)
        {
        }
    }
}
