using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawPoints: Command
    {
        public DrawPoints(Dictionary<Brush, List<Diagrams.Point>> points, IShapeComposite composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            this.Points = points;
            this.Composite = composite;
        }

        public Dictionary<Brush, List<Diagrams.Point>> Points { get; private set; }

        public IShapeComposite Composite { get; private set; }
    }
}
