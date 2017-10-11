using NeuralNetworkConstructor.Diagrams;
using System;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// http://www.geeksforgeeks.org/convex-hull-set-1-jarviss-algorithm-or-wrapping/.
    /// </summary>
    public class GiftWrappingAlgorithm : IConvexHullAlgorithm
    {
        public Dictionary<int, Point> Calculate(List<Point> points)
        {
            if (points == null || points.Count < 3)
            {
                throw new ArgumentException("At least 3 points reqired", "points");
            }

            /// Get leftmost point
            var vPointOnHull = this.FindMinimum(points);

            var hull = new Dictionary<int, Point>();

            Point vEndpoint;
            do
            {
                hull.Add(hull.Count, vPointOnHull);
                vEndpoint = points[0];

                for (int i = points.Count - 1; i >= 0; i--)
                {
                    if ((vPointOnHull == vEndpoint)
                        || (GeometryUtils.Orientation(vPointOnHull, vEndpoint, points[i]) == GeometryUtils.TURN_CLOCKWISE))
                    {
                        vEndpoint = points[i];
                    }
                }

                vPointOnHull = vEndpoint;
            }
            while (vEndpoint != hull[0]);

            return hull;
        }

        /// <summary>
        /// Finds point that has minimum X, if X is equal then by minimum Y.
        /// </summary>
        private Point FindMinimum(List<Point> points)
        {
            //var minimum = points.Where(p => p.X == points.Min(min => min.X)).First();
            var minimum = points[points.Count - 1];
            for (int i = points.Count - 2; i >= 0; i--)
            {
                var point = points[i];

                if (((minimum.X == point.X) && (minimum.Y > point.Y)) || (minimum.X > point.X))
                {
                    minimum = point;
                }
            }

            return minimum;
        }
    }
}
