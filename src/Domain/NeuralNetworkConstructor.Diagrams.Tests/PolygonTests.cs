using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Diagrams.Tests
{
    [TestClass]
    public class PolygonTests
    {
        [TestMethod]
        [TestCategory("Polygon")]
        public void Create_Primitive_Shape()
        {
            var lines =
                new LineSegment[]
                {
                    new LineSegment(new Point(1, 2), new Point(2, 3)),
                    new LineSegment(new Point(2, 3), new Point(3, 1)),
                    new LineSegment(new Point(3, 1), new Point(1, 2))
                };

            var p = new Polygon(lines);

            Assert.IsNotNull(p);
        }

        [TestMethod]
        [TestCategory("Polygon")]
        public void Create_Complex_Shape()
        {
            var lines =
                new LineSegment[]
                {
                    new LineSegment(new Point(1, 3), new Point(3, 4)),
                    new LineSegment(new Point(3, 4), new Point(4, 5)),
                    new LineSegment(new Point(4, 5), new Point(5, 4)),
                    new LineSegment(new Point(5, 4), new Point(7, 3)),
                    new LineSegment(new Point(7, 3), new Point(5, 2)),
                    new LineSegment(new Point(5, 2), new Point(4, 1)),
                    new LineSegment(new Point(4, 1), new Point(3, 2)),
                    new LineSegment(new Point(3, 2), new Point(1, 3))
                };

            var p = new Polygon(lines);

            Assert.IsNotNull(p);
        }

        [TestMethod]
        [TestCategory("Polygon")]
        [ExpectedException(typeof(Exception))]
        public void Create_Not_Connecting_Shape()
        {
            var lines =
                new LineSegment[]
                {
                    new LineSegment(new Point(1, 2), new Point(2, 3)),
                    new LineSegment(new Point(2, 3), new Point(3, 1)),
                    new LineSegment(new Point(3, 1), new Point(1, 3))
                };

            var p = new Polygon(lines);
        }

        [TestMethod]
        [TestCategory("Polygon")]
        public void Get_Head()
        {
            var lines =
                new LineSegment[]
                {
                    new LineSegment(new Point(1, 2), new Point(2, 3)),
                    new LineSegment(new Point(2, 3), new Point(3, 1)),
                    new LineSegment(new Point(3, 1), new Point(1, 2))
                };

            var p = new Polygon(lines);

            var head = p.NextVertex();

            Assert.IsNotNull(head);
        }

        [TestMethod]
        [TestCategory("Polygon")]
        public void Get_Next()
        {
            var lines =
                new LineSegment[]
                {
                    new LineSegment(new Point(1, 2), new Point(2, 3)),
                    new LineSegment(new Point(2, 3), new Point(3, 1)),
                    new LineSegment(new Point(3, 1), new Point(1, 2))
                };

            var p = new Polygon(lines);

            var head = p.NextVertex(new Point(1, 2));

            Assert.IsNotNull(head);
        }
    }
}
