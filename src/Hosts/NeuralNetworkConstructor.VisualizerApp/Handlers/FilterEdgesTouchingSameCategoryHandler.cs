using NeuralNetworkConstructor.Core.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeuralNetworkConstructor.Diagrams;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class FilterEdgesTouchingSameCategoryHandler : IRequestHandler<FilterEdgesTouchingSameCategory, List<Diagrams.GraphEdge>>
    {
        public Task<List<GraphEdge>> Handle(FilterEdgesTouchingSameCategory request)
        {
            var frontEdges = new List<Diagrams.GraphEdge>();

            // Remove edges that belong to same category
            foreach (var edge in request.Edges)
            {
                foreach (var category in request.Categories)
                {
                    var a = category.Any(f => f.X == edge.LeftSite.X && f.Y == edge.LeftSite.Y);
                    var b = category.Any(f => f.X == edge.RightSite.X && f.Y == edge.RightSite.Y);

                    if (a || b)
                    {
                        if (!a || !b)
                        {
                            frontEdges.Add(edge);
                        }

                        break;
                    }
                }
            }

            return Task.FromResult(frontEdges);
        }
    }
}
