using NeuralNetworkConstructor.Diagrams;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Delaunay_triangulation.
    /// </summary>
    public interface ITriangulationAlgorithm
    {
        List<Triangle> Calculate(List<Point> points);
    }
}
