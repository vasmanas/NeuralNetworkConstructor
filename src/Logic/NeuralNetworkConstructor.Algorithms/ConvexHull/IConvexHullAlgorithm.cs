using NeuralNetworkConstructor.Diagrams;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Convex_hull_algorithms.
    /// </summary>
    public interface IConvexHullAlgorithm
    {
        Dictionary<int, Point> Calculate(List<Point> points);
    }
}
