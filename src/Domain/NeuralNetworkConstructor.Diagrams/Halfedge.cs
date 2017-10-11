using NeuralNetworkConstructor.Diagrams.Common;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Halfedge : ValueObject
    {
        public Halfedge ELleft { get; set; }

        public Halfedge ELright { get; set; }

        public Edge ELedge { get; set; }

        public bool Deleted { get; set; }

        public int ELpm { get; set; }

        public Point Vertex { get; set; }

        public double YStar { get; set; }

        public Halfedge PQnext { get; set; }
    }
}
