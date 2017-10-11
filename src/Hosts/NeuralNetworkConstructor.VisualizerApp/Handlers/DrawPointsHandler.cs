using NeuralNetworkConstructor.Core.Messaging;
using System;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawPointsHandler : IMessageHandler<DrawPoints>
    {
        public Task Handle(DrawPoints message)
        {
            var host = message.Composite;

            foreach (var category in message.Points)
            {
                foreach (var point in category.Value)
                {
                    host.AddPoint(category.Key, point.X, point.Y);
                }
            }

            return Task.Delay(0);
        }
    }
}
