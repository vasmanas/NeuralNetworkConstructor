using NeuralNetworkConstructor.Diagrams.Common;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Triangle : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of a triangle
        /// </summary>
        /// <param name="point1">Vertex 1</param>
        /// <param name="point2">Vertex 2</param>
        /// <param name="point3">Vertex 3</param>
        public Triangle(Point point1, Point point2, Point point3)
        {
            this.P1 = point1;
            this.P2 = point2;
            this.P3 = point3;
        }

        /// <summary>
        /// First vertex index in triangle
        /// </summary>
        public Point P1 { get; private set; }

        /// <summary>
        /// Second vertex index in triangle
        /// </summary>
        public Point P2 { get; private set; }

        /// <summary>
        /// Third vertex index in triangle
        /// </summary>
        public Point P3 { get; private set; }
    }
}