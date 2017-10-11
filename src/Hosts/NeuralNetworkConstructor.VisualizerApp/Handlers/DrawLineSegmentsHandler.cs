using NeuralNetworkConstructor.Core.Messaging;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawLineSegmentsHandler : IMessageHandler<DrawLineSegments>
    {
        public Task Handle(DrawLineSegments message)
        {
            var host = message.Composite;

            foreach (var category in message.LineSegments)
            {
                foreach (var lineSegment in category.Value)
                {
                    host.AddLine(category.Key, lineSegment.P1.X, lineSegment.P1.Y, lineSegment.P2.X, lineSegment.P2.Y);

                    //await dispatcher.BeginInvoke(new Action(() => { composite.AddLine(Brushes.Blue, a.X, a.Y, b.X, b.Y); }), DispatcherPriority.ContextIdle);
                }
            }

            return Task.Delay(0);
        }
    }
}
