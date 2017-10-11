using NeuralNetworkConstructor.Diagrams.Common;

namespace NeuralNetworkConstructor.Diagrams
{
    public class GraphEdge : ValueObject
    {
        public GraphEdge(double startX, double startY, double endX, double endY, Point leftSite, Point rightSite)
            : this(new Point(startX, startY), new Point(endX, endY), leftSite, rightSite)
        {
        }

        public GraphEdge(Point start, Point end, Point leftSite, Point rightSite)
        {
            this.Start = start;
            this.End = end;
            this.LeftSite = leftSite;
            this.RightSite = rightSite;
        }

        public Point Start { get; private set; }

        public Point End { get; private set; }

        public Point LeftSite { get; private set; }

        public Point RightSite { get; private set; }
    }
}
