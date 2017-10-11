using NeuralNetworkConstructor.Core.Messaging;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawPolygonHandler : IMessageHandler<DrawPolygon>
    {
        public Task Handle(DrawPolygon message)
        {
            var polygon = message.Polygon;

            var head = polygon.NextVertex();

            while (true)
            {
                var tail = polygon.NextVertex(head);

                if (tail == null)
                {
                    tail = polygon.NextVertex();
                    message.Composite.AddLine(message.Brush, tail.X, tail.Y, head.X, head.Y);

                    break;
                }

                message.Composite.AddLine(message.Brush, head.X, head.Y, tail.X, tail.Y);

                head = tail;
            }

            return Task.Delay(0);
        }
    }
}
