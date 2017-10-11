using NeuralNetworkConstructor.Diagrams;
using NeuralNetworkConstructor.Diagrams.Common;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Circle : ValueObject
    {
        public Circle(double cx, double cy, double radius)
            : this(new Point(cx, cy), radius)
        {
        }

        public Circle(Point center, double radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Point Center { get; private set; }

        public double Radius { get; private set; }
    }
}
