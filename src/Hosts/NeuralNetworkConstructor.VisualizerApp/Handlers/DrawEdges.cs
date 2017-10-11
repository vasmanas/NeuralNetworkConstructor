using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class DrawEdges : Command
    {
        public DrawEdges(List<Diagrams.GraphEdge> edges, IShapeComposite composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            this.Edges = edges;
            this.Composite = composite;
        }

        public List<Diagrams.GraphEdge> Edges { get; private set; }

        public IShapeComposite Composite { get; private set; }
    }
}
