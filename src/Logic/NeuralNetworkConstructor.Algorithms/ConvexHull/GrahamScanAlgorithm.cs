using NeuralNetworkConstructor.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// http://www.geeksforgeeks.org/convex-hull-set-2-graham-scan/.
    /// </summary>
    public class GrahamScanAlgorithm : IConvexHullAlgorithm
    {
        private const int TURN_COUNTERCLOCKWISE = 1;
        private const int TURN_CLOCKWISE = -1;
        private const int TURN_NONE = 0;

        public Dictionary<int, Point> Calculate(List<Point> points)
        {
            if (points == null || points.Count < 3)
            {
                throw new ArgumentException("At least 3 points reqired", "points");
            }

            /// Get leftmost point
            var p0 = this.FindMinimum(points);

            var remainder = points.Where(p => p != p0).ToList();

            var orderedPoints = Sort(p0, remainder);

            var result = new Dictionary<int, Point>
            {
                { 0, p0 },
                { 1, orderedPoints.Values[0] },
                { 2, orderedPoints.Values[1] }
            };

            foreach (var point in orderedPoints)
            {
                KeepLeft(result, point.Value);
            }

            return result;
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

        private SortedList<double, Point> Sort(Point p0, List<Point> points)
        {
            var sortedPoints = new SortedList<double, Point>();

            for (var i = points.Count - 1; i >= 0; i--)
            {
                var point = points[i];

                var angle = GeometryUtils.AngleRad(p0, point);

                if (sortedPoints.ContainsKey(angle))
                {
                    // Calc distance, take the one that is further;
                    var oldPoint = sortedPoints[angle];

                    if (GeometryUtils.Distance(p0, oldPoint) < GeometryUtils.Distance(p0, point))
                    {
                        sortedPoints[angle] = point;
                    }
                }
                else
                {
                    sortedPoints.Add(angle, point);
                }
            }

            return sortedPoints;
        }

        private SortedList<double, Point> MergeSort(Point p0, List<Point> points)
        {
            if (points.Count == 1)
            {
                return new SortedList<double, Point>
                {
                    { GeometryUtils.AngleRad(p0, points[0]), points[0] }
                };
            }

            var middle = points.Count / 2;

            var leftPoints = points.GetRange(0, middle);
            var rightPoints = points.GetRange(middle, points.Count - middle);

            var leftSortedPoints = MergeSort(p0, leftPoints);
            var rightSortedPoints = MergeSort(p0, rightPoints);

            var leftptr = leftSortedPoints.Count - 1;
            var rightptr = rightSortedPoints.Count - 1;

            var sortedPoints = new SortedList<double, Point>();

            for (var i = leftSortedPoints.Count + rightSortedPoints.Count; i > 0; i--)
            {
                if (leftptr == -1)
                {
                    var el = rightSortedPoints.ElementAt(rightptr);

                    // TODO: measure distance if same key exists
                    sortedPoints.Add(el.Key, el.Value);
                    rightptr--;
                }
                else if (rightptr == -1)
                {
                    var el = leftSortedPoints.ElementAt(leftptr);

                    // TODO: measure distance if same key exists
                    sortedPoints.Add(el.Key, el.Value);
                    leftptr--;
                }
                else if (leftSortedPoints.ElementAt(leftptr).Key < rightSortedPoints.ElementAt(rightptr).Key)
                {
                    var el = leftSortedPoints.ElementAt(leftptr);

                    // TODO: measure distance if same key exists
                    sortedPoints.Add(el.Key, el.Value);
                    leftptr--;
                }
                else
                {
                    var el = rightSortedPoints.ElementAt(rightptr);

                    // TODO: measure distance if same key exists
                    sortedPoints.Add(el.Key, el.Value);
                    rightptr--;
                }
            }

            return sortedPoints;
        }

        private void KeepLeft(Dictionary<int, Point> hull, Point r)
        {
            while (hull.Count > 1 && GeometryUtils.Orientation(hull[hull.Count - 2], hull[hull.Count - 1], r) != TURN_COUNTERCLOCKWISE)
            {
                hull.Remove(hull.Count - 1);
            }

            if (hull.Count == 0 || hull[hull.Count - 1] != r)
            {
                hull.Add(hull.Count, r);
            }
        }
    }
}
