using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkConstructor.Diagrams
{
    public class Polygon
    {
        private PolygonPoint head = null;

        public Polygon(IEnumerable<LineSegment> lines)
        {
            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            var polygonLines = lines.Select(l => new LineSegment(new Point(l.P1.X, l.P1.Y), new Point(l.P2.X, l.P2.Y))).ToList();

            if (!polygonLines.Any())
            {
                throw new ArgumentException("No lines for polygon", nameof(lines));
            }

            var minimum = this.FindMinimum(polygonLines);
            this.head = this.MakeHead(minimum, polygonLines);
            this.AttachLines(this.head, polygonLines);
        }
        
        public Point NextVertex(Point point = null, bool loop = false)
        {
            if (point == null)
            {
                return new Point(this.head);
            }

            var position = head;
            while (!position.Equals(point))
            {
                position = position.Next;

                if (position == null)
                {
                    throw new ArgumentException("Point is not polygons vertex", nameof(point));
                }
            }

            if (position.Next == null)
            {
                if (loop)
                {
                    return new Point(this.head);
                }
                else
                {
                    return null;
                }
            }

            return new Point(position.Next);
        }

        private void AttachLines(PolygonPoint head, List<LineSegment> lines)
        {
            var tail = this.FindTail(head);
            
            while (lines.Count > 1)
            {
                LineSegment removeLine = null;

                foreach (var l in lines)
                {
                    if (tail.Equals(l.P1))
                    {
                        tail.Next = new PolygonPoint(l.P2.X, l.P2.Y);
                    }
                    else if (tail.Equals(l.P2))
                    {
                        tail.Next = new PolygonPoint(l.P1.X, l.P1.Y);
                    }

                    if (tail.Next == null)
                    {
                        continue;
                    }

                    tail = tail.Next;

                    removeLine = l;

                    break;
                }

                if (removeLine == null)
                {
                    throw new Exception("Shape is not polygon");
                }

                lines.Remove(removeLine);
            }

            var line = lines[0];

            if (!((head.Equals(line.P1) && tail.Equals(line.P2))
                || (head.Equals(line.P2) && tail.Equals(line.P1))))
            {
                throw new Exception("Shape is not connecting");
            }

            lines.RemoveAt(0);
        }

        private PolygonPoint FindTail(PolygonPoint head)
        {
            var tail = head;

            while (tail.Next != null)
            {
                tail = tail.Next;

                if (object.ReferenceEquals(tail, head))
                {
                    return null;
                }
            }

            return tail;
        }

        private PolygonPoint MakeHead(Point min, List<LineSegment> lines)
        {
            LineSegment l1 = null;
            LineSegment l2 = null;

            foreach (var line in lines)
            {
                if (!line.P1.Equals(min) && !line.P2.Equals(min))
                {
                    continue;
                }

                if (l1 == null)
                {
                    l1 = line;

                    continue;
                }
                
                l2 = line;

                break;
            }

            if (l1 == null || l2 == null)
            {
                throw new Exception("Lines not connected in to polygon");
            }

            lines.Remove(l1);
            lines.Remove(l2);

            var a = l1.P1.Equals(min) ? l1.P2: l1.P1;
            var b = l2.P1.Equals(min) ? l2.P2 : l2.P1;

            if (GeometryUtils.Orientation(min, a, b) == GeometryUtils.TURN_CLOCKWISE)
            {
                return new PolygonPoint(b.X, b.Y)
                {
                    Next = new PolygonPoint(min.X, min.Y)
                    {
                        Next = new PolygonPoint(a.X, a.Y)
                    }
                };
            }
            else
            {
                return new PolygonPoint(a.X, a.Y)
                {
                    Next = new PolygonPoint(min.X, min.Y)
                    {
                        Next = new PolygonPoint(b.X, b.Y)
                    }
                };
            }
        }

        private Point FindMinimum(List<LineSegment> lines)
        {
            Point min = null;

            foreach (var line in lines)
            {
                var m = LessThan(line.P1, line.P2) ? line.P1 : line.P2;

                if (min == null || LessThan(m, min))
                {
                    min = m;
                }
            }

            return min;
        }

        private bool LessThan(Point p1, Point p2)
        {
            return p1.X < p2.X || (p1.X == p2.X && p1.Y < p2.Y);
        }

        private class PolygonPoint : Point
        {
            public PolygonPoint(double x, double y) : base(x, y)
            {
            }

            public PolygonPoint Next { get; set; }
        }
    }
}
