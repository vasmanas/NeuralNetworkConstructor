using NeuralNetworkConstructor.Core.Logging;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Diagrams;
using System.Collections.Generic;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class MakeConvexHull : Request<List<LineSegment>>
    {
        public MakeConvexHull(List<Diagrams.Point> points, Dispatcher dispatcher, ILog log)
        {
            this.Points = points;
            this.Dispatcher = dispatcher;
            this.Log = log;
        }

        public List<Diagrams.Point> Points { get; private set; }

        public Dispatcher Dispatcher { get; private set; }

        public ILog Log { get; private set; }
    }
}
