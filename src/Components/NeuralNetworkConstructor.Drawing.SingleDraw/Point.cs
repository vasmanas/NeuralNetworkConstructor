using System.Windows.Media;

namespace NeuralNetworkConstructor.Drawing.SingleDraw
{
    public class Point : Shape
    {
        public Point(Brush color, double x, double y)
        {
            this.Color = color;
            this.X = x;
            this.Y = y;
        }

        public Brush Color { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
    }
}
