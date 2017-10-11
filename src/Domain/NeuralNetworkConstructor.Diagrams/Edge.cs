using NeuralNetworkConstructor.Diagrams.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Edge : ValueObject
    {
        public Edge(double a, double b, double c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        public double A { get; set; }

        public double B { get; set; }

        public double C { get; set; }

        public Point[] Ep { get; set; } = new Point[2];

        public Point[] Reg { get; set; } = new Point[2];
    }
}
