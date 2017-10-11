using NeuralNetworkConstructor.Diagrams;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Voronoi_diagram.
    /// </summary>
    public interface IVoronoiDiagramAlgorithm
    {
        List<GraphEdge> Calculate(List<Point> points, double minX, double maxX, double minY, double maxY);
    }
}
