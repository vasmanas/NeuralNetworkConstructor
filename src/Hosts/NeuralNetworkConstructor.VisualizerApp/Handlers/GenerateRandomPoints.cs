using NeuralNetworkConstructor.Core.Logging;
using NeuralNetworkConstructor.Core.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class GenerateRandomPoints : Request<List<Diagrams.Point>>
    {
        public GenerateRandomPoints(int count, double width, double height, List<Diagrams.Point> points, Dispatcher dispatcher, ILog log)
        {
            this.Count = count;
            this.Width = width;
            this.Height = height;
            this.PlacedPoints = points.AsReadOnly();
            this.Dispatcher = dispatcher;
            this.Log = log;
        }

        public int Count { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public ReadOnlyCollection<Diagrams.Point> PlacedPoints { get; private set; }

        public Dispatcher Dispatcher { get; private set; }

        public ILog Log { get; private set; }
    }
}
