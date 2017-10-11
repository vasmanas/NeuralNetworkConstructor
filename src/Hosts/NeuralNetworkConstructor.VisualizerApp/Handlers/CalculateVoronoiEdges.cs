using NeuralNetworkConstructor.Core.Messaging;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class CalculateVoronoiEdges : Request<List<Diagrams.GraphEdge>>
    {
        public CalculateVoronoiEdges(List<Diagrams.Point> points, double width, double height)
        {
            this.Points = points;
            this.Width = width;
            this.Height = height;
        }

        public List<Diagrams.Point> Points { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }
    }
}
