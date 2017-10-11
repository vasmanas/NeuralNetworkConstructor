using NeuralNetworkConstructor.Core.Logging;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class MakeMultiColorTriangulation : Request<UIElement>
    {
        public MakeMultiColorTriangulation(Dictionary<Brush, List<Diagrams.Point>> categories, IShapeComposite composite, Dispatcher dispatcher, ILog log)
        {
            this.Categories = categories;
            this.Composite = composite;
            this.Dispatcher = dispatcher;
            this.Log = log;
        }

        public Dictionary<Brush, List<Diagrams.Point>> Categories { get; private set; }

        public IShapeComposite Composite { get; private set; }

        public Dispatcher Dispatcher { get; private set; }

        public ILog Log { get; private set; }
    }
}
