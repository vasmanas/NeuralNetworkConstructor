using NeuralNetworkConstructor.Diagrams.Common;
using System;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Point : ValueObject, IEquatable<Point>, IEqualityComparer<Point>
    {
        public Point(Point p) : this(p.X, p.Y)
        {
        }


        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Y { get; private set; }

        public double X { get; private set; }

        public bool Equals(Point other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return Math.Round(X, 5).Equals(Math.Round(other.X, 5))
                && Math.Round(Y, 5).Equals(Math.Round(other.Y, 5));
        }

        public bool Equals(Point x, Point y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Point obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}; {1})", this.X, this.Y);
        }
    }
}
