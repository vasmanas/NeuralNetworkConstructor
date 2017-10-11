using NeuralNetworkConstructor.Core.Messaging;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawEdgesHandler : IMessageHandler<DrawEdges>
    {
        public Task Handle(DrawEdges message)
        {
            foreach (var edge in message.Edges)
            {
                message.Composite.AddLine(Brushes.DarkBlue, edge.Start.X, edge.Start.Y, edge.End.X, edge.End.Y);
            }

            return Task.Delay(0);
        }
    }
}
