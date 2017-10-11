using NeuralNetworkConstructor.Diagrams;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// Performs the Delauney triangulation on a set of points.
    /// </summary>
    public class BowyerWatsonAlgorithm : ITriangulationAlgorithm
    {
        /// <summary>
        /// input : vertex list
        /// output : triangle list
        ///    initialize the triangle list
        ///    determine the supertriangle
        ///    add supertriangle vertices to the end of the vertex list
        ///    add the supertriangle to the triangle list
        ///    for each sample point in the vertex list
        ///       initialize the edge buffer
        ///       for each triangle currently in the triangle list
        ///          calculate the triangle circumcircle center and radius
        ///          if the point lies in the triangle circumcircle then
        ///             add the three triangle edges to the edge buffer
        ///             remove the triangle from the triangle list
        ///          endif
        ///       endfor
        ///       delete all doubly specified edges from the edge buffer
        ///          this leaves the edges of the enclosing polygon only
        ///       add to the triangle list all triangles formed between the point 
        ///          and the edges of the enclosing polygon
        ///    endfor
        ///    remove any triangles from the triangle list that use the supertriangle vertices
        ///    remove the supertriangle vertices from the vertex list
        /// end.
        /// </summary>
        public List<Triangle> Calculate(List<Point> points)
        {
            var triangles = new List<Triangle>();
            if (points.Count < 3)
            {
                /// Need at least three vertices for triangulation.
                return triangles;
            }

            /// Make supertriangle, that contains all points.
            var superTriangle = MakeSuperTriangle(points);

            /// The supertriangle is the first triangle in the triangle list.
            /// SuperTriangle placed at index 0.
            triangles.Add(superTriangle);

            var circumcircles = new Dictionary<Triangle, Circle>();;

            /// Include each point one at a time into the existing mesh.
            foreach (var vertex in points)
			{
                /// Set up the edge buffer.
                var edges = new List<LineSegment>();

                /// If the point (Vertex.x,Vertex.y) lies inside the circumcircle then the
                /// three edges of that triangle are added to the edge buffer and the triangle is removed from list.
                for (int j = triangles.Count - 1; j >= 0; j--)
				{
                    var triangle = triangles[j];

                    if (!circumcircles.TryGetValue(triangle, out Circle circumcircle))
                    {
                        circumcircle = GeometryUtils.MakeCircle(triangle.P1, triangle.P2, triangle.P3);
                        circumcircles.Add(triangle, circumcircle);
                    }

                    //if (GeometryUtils.InCircle(vertice, triangle.P1, triangle.P2, triangle.P3))
                    if (GeometryUtils.InCircle(vertex, circumcircle))
                    {
                        edges.Add(new LineSegment(triangle.P1, triangle.P2));
                        edges.Add(new LineSegment(triangle.P2, triangle.P3));
                        edges.Add(new LineSegment(triangle.P3, triangle.P1));

                        triangles.RemoveAt(j);

                        circumcircles.Remove(triangle);
                    }
                }

                /// Remove duplicate edges.
                /// Note: if all triangles are specified anticlockwise then all
                /// interior edges are opposite pointing in direction.
                for (int j = edges.Count - 2; j >= 0; j--)
				{
                    var edge = edges[j];

                    for (int k = edges.Count - 1; k >= j + 1; k--)
					{
						if (edge.Equals(edges[k]))
						{
							edges.RemoveAt(k);
							edges.RemoveAt(j);
							k--;

                            continue;
						}
					}
                }

                /// Form new triangles for the current point. Skipping over any tagged edges.
                /// All edges are arranged in clockwise order.
                edges.ForEach(edge => triangles.Add(new Triangle(edge.P1, edge.P2, vertex)));
            }

            /// Remove triangles with supertriangle vertices.
            /// These are triangles which have a vertex number greater than nv.
            for (int i = triangles.Count - 1; i >= 0; i--)
			{
                var triangle = triangles[i];

                if (triangle.P1 == superTriangle.P1 || triangle.P1 == superTriangle.P2 || triangle.P1 == superTriangle.P3
                    || triangle.P2 == superTriangle.P1 || triangle.P2 == superTriangle.P2 || triangle.P2 == superTriangle.P3
                    || triangle.P3 == superTriangle.P1 || triangle.P3 == superTriangle.P2 || triangle.P3 == superTriangle.P3)
                {
                    triangles.RemoveAt(i);
                }
			}

            triangles.TrimExcess();

            return triangles;
		}

        private Triangle MakeSuperTriangle(List<Point> points)
        {
            /// Find the maximum and minimum vertex bounds.
            /// This is to allow calculation of the bounding supertriangle.
            var xmin = points.Min(p => p.X);
            var xmax = points.Max(p => p.X);
            var ymin = points.Min(p => p.Y);
            var ymax = points.Max(p => p.Y);

            var dx = xmax - xmin;
            var dy = ymax - ymin;
            var dmax = dx > dy ? dx : dy;

            var xmid = (xmin + xmax) * 0.5;
            var ymid = (ymin + ymax) * 0.5;

            /// Set up the supertriangle.
            /// This is a triangle which encompasses all the sample points.
            return new Triangle(
                new Point(xmid - 2 * dmax, ymid - dmax),
                new Point(xmid, ymid + 2 * dmax),
                new Point(xmid + 2 * dmax, ymid - dmax));
        }
    }
}
