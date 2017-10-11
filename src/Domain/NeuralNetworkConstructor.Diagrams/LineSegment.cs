using NeuralNetworkConstructor.Diagrams.Common;
using System;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Diagrams
{
    /// <summary>
    /// Edge made from two point indexes
    /// </summary>
    public class LineSegment : ValueObject, IEquatable<LineSegment>, IEqualityComparer<LineSegment>
    {
        /// <summary>
        /// Initializes a new edge instance
        /// </summary>
        /// <param name="point1">Start edge vertex index</param>
        /// <param name="point2">End edge vertex index</param>
        public LineSegment(Point point1, Point point2)
        {
            this.P1 = point1;
            this.P2 = point2;
        }

        /// <summary>
        /// Start of edge index
        /// </summary>
        public Point P1 { get; private set; }

        /// <summary>
        /// End of edge index
        /// </summary>
        public Point P2 { get; private set; }

        /// <summary>
        /// Checks whether two edges are equal disregarding the direction of the edges.
        /// </summary>
        public bool Equals(LineSegment other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return (this.P1 == other.P1 && this.P2 == other.P2)
                || (this.P1 == other.P2 && this.P2 == other.P1);
        }

        public bool Equals(LineSegment x, LineSegment y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(LineSegment obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (this.P1.X < this.P2.X)
            {
                return this.P1.GetHashCode() ^ this.P2.GetHashCode();
            }
            else if (this.P1.X > this.P2.X)
            {
                return this.P2.GetHashCode() ^ this.P1.GetHashCode();
            }
            else if (this.P1.Y >= this.P2.Y)
            {
                return this.P1.GetHashCode() ^ this.P2.GetHashCode();
            }

            return this.P2.GetHashCode() ^ this.P1.GetHashCode();
        }
    }
}