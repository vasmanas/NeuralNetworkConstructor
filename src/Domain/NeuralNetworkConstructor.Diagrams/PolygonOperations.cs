using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkConstructor.Diagrams
{
    public static class PolygonOperations
    {
        public static List<Point> ToPoints(this Polygon poly)
        {
            var results = new List<Point>();
            var tail = poly.NextVertex();

            while (tail != null)
            {
                results.Add(tail);
                tail = poly.NextVertex(tail);
            }

            return results;
        }

        /// <summary>
        /// Difference: var1 - var2.
        /// </summary>
        public static Polygon[] Difference(Polygon poly1, Polygon poly2)
        {
            if (poly1 == null)
            {
                throw new ArgumentNullException(nameof(poly1));
            }

            if (poly2 == null)
            {
                throw new ArgumentNullException(nameof(poly2));
            }

            var head1 = poly1.NextVertex();
            var head2 = poly2.NextVertex();

            while (!head1.Equals(head2))
            {
                head2 = poly2.NextVertex(head2);

                if (head2 == null)
                {
                    throw new Exception("Polygons dont have same minimum point");
                }
            }

            var cutOuts = new List<Polygon>();
            var last = false;

            while (true)
            {
                var tail1 = poly1.NextVertex(head1);
                if (tail1 == null)
                {
                    tail1 = poly1.NextVertex();

                    last = true;
                }

                var tail2 = poly2.NextVertex(head2, true);
                
                var lines = new List<LineSegment>
                {
                    new LineSegment(new Point(head1), new Point(tail1)),
                    new LineSegment(new Point(head2), new Point(tail2))
                };

                while (!tail1.Equals(tail2))
                {
                    head2 = tail2;
                    tail2 = poly2.NextVertex(tail2, true);

                    lines.Add(new LineSegment(new Point(head2), new Point(tail2)));
                }

                if (lines.Count > 2)
                {
                    cutOuts.Add(new Polygon(lines));
                }

                head1 = tail1;
                head2 = tail2;

                if (last)
                {
                    break;
                }
            }

            return cutOuts.ToArray();
        }
    }
}
