using NeuralNetworkConstructor.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkConstructor.Algorithms
{
    public static class PointGenerator
    {
        public static List<Point> GeneratePoints(int count, double minx, double maxx, double miny, double maxy)
        {
            var r = new Random();
            var points = new List<Point>();

            for (int i = 0; i < count; i++)
            {
                int x;
                int y;

                do
                {
                    x = r.Next((int)minx, (int)maxx + 1);
                    y = r.Next((int)miny, (int)maxy + 1);
                }
                while (points.Any(e => e.X == x && e.Y == y));                

                points.Add(new Point(x, y));
            }

            return points;
        }
    }
}
