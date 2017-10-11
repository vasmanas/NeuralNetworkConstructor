using NeuralNetworkConstructor.Core.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeuralNetworkConstructor.Diagrams;
using NeuralNetworkConstructor.Algorithms;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class CalculateVoronoiEdgesHandler : IRequestHandler<CalculateVoronoiEdges, List<GraphEdge>>
    {
        public Task<List<GraphEdge>> Handle(CalculateVoronoiEdges request)
        {
            IVoronoiDiagramAlgorithm algorythm = new FortunesAlgorithm();

            var edges = algorythm.Calculate(
                request.Points,
                0,
                request.Width,
                0,
                request.Height);

            return Task.FromResult(edges);
        }
    }
}
