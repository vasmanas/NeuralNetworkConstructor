using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawLineSegments : Command
    {
        public DrawLineSegments(Dictionary<Brush, List<Diagrams.LineSegment>> lineSegments, IShapeComposite composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            this.LineSegments = lineSegments;
            this.Composite = composite;
        }

        public Dictionary<Brush, List<Diagrams.LineSegment>> LineSegments { get; private set; }

        public IShapeComposite Composite { get; private set; }
    }
}
