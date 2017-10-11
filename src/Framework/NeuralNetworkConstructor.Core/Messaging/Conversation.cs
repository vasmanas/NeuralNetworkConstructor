using System;

namespace NeuralNetworkConstructor.Core.Messaging
{
    public static class Conversation
    {
        public static Guid MakeId()
        {
            return Guid.NewGuid();
        }
    }
}
