using NeuralNetworkConstructor.Core.Logging;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System.Collections.Generic;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class MakeTriangulation : Command
    {
        public MakeTriangulation(List<Diagrams.Point> points, IShapeComposite composite, Dispatcher dispatcher, ILog log)
        {
            this.Points = points;
            this.Dispatcher = dispatcher;
            this.Composite = composite;
            this.Log = log;
        }

        public List<Diagrams.Point> Points { get; private set; }

        public IShapeComposite Composite { get; private set; }

        public Dispatcher Dispatcher { get; private set; }

        public ILog Log { get; private set; }
    }
}
