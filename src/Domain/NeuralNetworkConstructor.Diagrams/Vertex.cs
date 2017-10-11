using NeuralNetworkConstructor.Diagrams.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Vertex : Aggregate
    {
        public Vertex()
            : this(0, 0)
        {
        }

        public Vertex(double x, double y)
        {
            this.Point = new Point(x, y);
        }

        public Point Point { get; set; }
    }
}
