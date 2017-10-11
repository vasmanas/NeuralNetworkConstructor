using NeuralNetworkConstructor.Core.Messaging;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class FilterEdgesTouchingSameCategory : Request<List<Diagrams.GraphEdge>>
    {
        public FilterEdgesTouchingSameCategory(List<Diagrams.GraphEdge> edges, List<List<Diagrams.Point>> categories)
        {
            this.Edges = edges;
            this.Categories = categories;
        }

        public List<Diagrams.GraphEdge> Edges { get; private set; }

        public List<List<Diagrams.Point>> Categories { get; private set; }
    }
}
