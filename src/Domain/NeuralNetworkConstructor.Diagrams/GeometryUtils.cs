using System;

namespace NeuralNetworkConstructor.Diagrams
{
    public static class GeometryUtils
    {
        public const int TURN_COUNTERCLOCKWISE = 1;
        public const int TURN_CLOCKWISE = -1;
        public const int TURN_NONE = 0;

        /// <summary>
        /// To find orientation of ordered triplet (p, q, r).
        /// The function returns following values
        /// 0 --> p, q and r are colinear
        /// -1 --> Clockwise
        /// 1 --> Counterclockwise
        /// </summary>
        public static double Orientation(Point p, Point q, Point r)
        {
            /// Determinant
            var orin = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (orin > 0)
            {
                /// Orientation is to the left-hand side, clockwise
                return TURN_CLOCKWISE;
            }

            if (orin < 0)
            {
                /// Orientation is to the right-hand side, counterclockwise
                return TURN_COUNTERCLOCKWISE;
            }

            /// Orientation is neutral aka collinear
            return TURN_NONE;
        }

        public static double Distance(Point a, Point b)
        {
            // Greiciausiai galima sumazinti operaciju skaiciu nedauginant kvardratu, bet naudojant sign, taip pat nuimant sakni
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates angle in radians.
        /// </summary>
        public static double AngleRad(Point a, Point b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;

            return Math.Atan2(dy, dx);
        }

        /// <summary>
        /// Calculates angle in degrees.
        /// </summary>
        public static double Angle(Point a, Point b)
        {
            return AngleRad(a, b) * 180.0 / Math.PI;
        }

        public static Circle MakeCircle(Point a, Point b, Point c)
        {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            var x1 = (b.X + a.X) / 2;
            var y1 = (b.Y + a.Y) / 2;
            var dy1 = b.X - a.X;
            var dx1 = a.Y - b.Y;

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            var x2 = (c.X + b.X) / 2;
            var y2 = (c.Y + b.Y) / 2;
            var dy2 = c.X - b.X;
            var dx2 = b.Y - c.Y;

            // See where the lines intersect.
            var center = Intersection(
                new Point(x1, y1), new Point(x1 + dx1, y1 + dy1),
                new Point(x2, y2), new Point(x2 + dx2, y2 + dy2));

            if (double.IsNaN(center.X))
            {
                return null;
            }

            var dx = center.X - a.X;
            var dy = center.Y - a.Y;
            var radius = Math.Sqrt(dx * dx + dy * dy);

            return new Circle(center, radius);
        }

        /// <summary>
        /// Find the point of intersection between
        /// the lines a1 --> a2 and b1 --> b2.
        /// </summary>
        private static Point Intersection(Point a1, Point a2, Point b1, Point b2)
        {
            /// Get the segments' parameters.
            var dxa = a2.X - a1.X;
            var dya = a2.Y - a1.Y;
            var dxb = b2.X - b1.X;
            var dyb = b2.Y - b1.Y;

            var denominator = (dya * dxb - dxa * dyb);

            var t1 = ((a1.X - b1.X) * dyb + (b1.Y - a1.Y) * dxb) / denominator;

            if (double.IsInfinity(t1))
            {
                /// The lines are parallel (or close enough to it).
                return new Point(double.NaN, double.NaN);
            }

            /// Point of intersection.
            return new Point(a1.X + dxa * t1, a1.Y + dya * t1);
        }

        /// <summary>
        /// Returns true if the point (p) lies inside the circumcircle made up by points (a,b,c).
        /// NOTE: A point on the edge is inside the circumcircle.
        /// </summary>
        /// <param name="p"> Point to check. </param>
        /// <param name="a"> First point on circle. </param>
        /// <param name="b"> Second point on circle. </param>
        /// <param name="c"> Third point on circle. </param>
        /// <returns> true if p is inside circle. </returns>
        public static bool InCircle(Point p, Point a, Point b, Point c)
        {
            var cr = MakeCircle(a, b, c);

            return InCircle(p, cr);
        }

        /// <summary>
        /// Returns true if the point (p) lies inside the circle.
        /// NOTE: A point on the edge is inside the circle.
        /// </summary>
        /// <param name="p"> Point to check. </param>
        /// <param name="cr"> Circle to check in. </param>
        /// <returns> true if p is inside circle. </returns>
        public static bool InCircle(Point p, Circle cr)
        {
            if (cr == null)
            {
                return false;
            }

            var dx = p.X - cr.Center.X;
            var dy = p.Y - cr.Center.Y;

            var drsqr = Math.Sqrt(dx * dx + dy * dy);

            return drsqr <= cr.Radius;
        }
    }
}
