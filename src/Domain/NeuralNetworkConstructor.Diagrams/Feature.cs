using NeuralNetworkConstructor.Diagrams.Common;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Feature : Aggregate
    {
        public Feature()
            : this(0, 0)
        {
        }

        public Feature(double x, double y)
        {
            this.Point = new Point(x, y);
        }

        public Point Point { get; set; }
    }
}
