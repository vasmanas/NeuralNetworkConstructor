using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Diagrams;
using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Windows.Media;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawPolygon : Command
    {
        public DrawPolygon(Brush brush, Polygon polygon, IShapeComposite composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            this.Brush = brush;
            this.Polygon = polygon;
            this.Composite = composite;
        }

        public Brush Brush { get; private set; }

        public Polygon Polygon { get; private set; }

        public IShapeComposite Composite { get; private set; }
    }
}
