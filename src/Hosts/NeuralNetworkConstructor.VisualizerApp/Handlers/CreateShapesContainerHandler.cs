using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.SingleImage;
using System.Threading.Tasks;
using System.Windows;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class CreateShapesContainerHandler : IRequestHandler<CreateShapesContainer, UIElement>
    {
        public Task<UIElement> Handle(CreateShapesContainer request)
        {
            var host = new PaintingComposite(request.Width, request.Height);
            //var host = new ShapesHost();

            return Task.FromResult<UIElement>(host);
        }
    }
}
