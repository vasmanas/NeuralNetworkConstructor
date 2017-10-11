using System.Windows.Media;

namespace NeuralNetworkConstructor.Drawing.SingleDraw
{
    public class LineSegment : Shape
    {
        public LineSegment(Brush color, double ax, double ay, double bx, double by)
        {
            this.Color = color;
            this.Ax = ax;
            this.Ay = ay;
            this.Bx = bx;
            this.By = by;
        }

        public Brush Color { get; set; }

        public double Ax { get; set; }

        public double Ay { get; set; }

        public double Bx { get; set; }

        public double By { get; set; }
    }
}
