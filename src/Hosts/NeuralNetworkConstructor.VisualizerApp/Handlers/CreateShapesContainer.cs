using NeuralNetworkConstructor.Core.Messaging;
using System.Windows;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class CreateShapesContainer : Request<UIElement>
    {
        public CreateShapesContainer(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width { get; private set; }

        public double Height { get; private set; }
    }
}
