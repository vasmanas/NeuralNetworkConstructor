using NeuralNetworkConstructor.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkConstructor.Algorithms
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Fortune%27s_algorithm.
    /// </summary>
    public class FortunesAlgorithm : IVoronoiDiagramAlgorithm
    {
        private const int LE = 0;
        private const int RE = 1;

        private const double MinDistanceBetweenSites = 0.1;

        private Stack<Point> sites = new Stack<Point>();

        private double borderMinX, borderMaxX, borderMinY, borderMaxY;
        private double xmin, xmax, ymin, ymax, deltax, deltay;
        private int nsites;

        private Point bottomsite;
        private int sqrt_nsites;

        /// <summary>
        /// PQ - PriorityQueue.
        /// </summary>
		private int PQcount;
        private int PQmin;
        private int PQhashsize;
        private Halfedge[] PQhash;

        /// <summary>
        /// EL - EdgeList.
        /// </summary>
        private int ELhashsize;
        private Halfedge[] ELhash;
        private Halfedge ELleftend, ELrightend;

        /// <summary>
        /// Starting pont of calculations;
        /// </summary>
        /// <param name="xValuesIn"> X values for each site. </param>
        /// <param name="yValuesIn"> Y values for each site. Must be identical length to yValuesIn. </param>
        /// <param name="minX"> The minimum X of the bounding box around the voronoi. </param>
        /// <param name="maxX"> The maximum X of the bounding box around the voronoi. </param>
        /// <param name="minY"> The minimum Y of the bounding box around the voronoi. </param>
        /// <param name="maxY"> The maximum Y of the bounding box around the voronoi. </param>
        /// <returns> List of graph edges. </returns>
        public List<GraphEdge> Calculate(List<Point> points, double minX, double maxX, double minY, double maxY)
        {
            // Check bounding box inputs - if mins are bigger than maxes, swap them
            double temp = 0;

            if (minX > maxX)
            {
                temp = minX;
                minX = maxX;
                maxX = temp;
            }

            if (minY > maxY)
            {
                temp = minY;
                minY = maxY;
                maxY = temp;
            }

            borderMinX = minX;
            borderMinY = minY;
            borderMaxX = maxX;
            borderMaxY = maxY;

            nsites = points.Count;
            sqrt_nsites = (int)Math.Sqrt((double)nsites + 4);

            xmin = points.Min(p => p.X);
            xmax = points.Max(p => p.X);
            ymin = points.Min(p => p.Y);
            ymax = points.Max(p => p.Y);
            deltax = xmax - xmin;
            deltay = ymax - ymin;

            this.sites.Clear();
            foreach (var point in points
                .Distinct()
                .OrderByDescending(p => p.Y)
                .ThenByDescending(p => p.X))
            {
                sites.Push(point);
            }

            return Voronoi_bd();
        }

        private void PQinitialize()
        {
            PQcount = 0;
            PQmin = 0;
            PQhashsize = 4 * sqrt_nsites;
            PQhash = new Halfedge[PQhashsize];

            for (int i = 0; i < PQhashsize; i++)
            {
                PQhash[i] = new Halfedge();
            }
        }

        private void ELinitialize()
        {
            ELhashsize = 2 * sqrt_nsites;
            ELhash = new Halfedge[ELhashsize];

            for (int i = 0; i < ELhashsize; i++)
            {
                ELhash[i] = null;
            }

            ELleftend = new Halfedge();
            ELrightend = new Halfedge();
            ELleftend.ELleft = null;
            ELleftend.ELright = ELrightend;
            ELrightend.ELleft = ELleftend;
            ELrightend.ELright = null;
            ELhash[0] = ELleftend;
            ELhash[ELhashsize - 1] = ELrightend;
        }

        private Point NextOne()
        {
            return this.sites.Count == 0 ? null : this.sites.Pop();
        }

        private Halfedge ELleftbnd(Point p)
        {
            /// Use hash table to get close to desired halfedge
            /// use the hash function to find the place in the hash map that this
            /// HalfEdge should be.
            var bucket = (int)((p.X - xmin) / deltax * ELhashsize);

            /// Make sure that the bucket position is within the range of the hash
            /// array.
            if (bucket < 0) bucket = 0;
            if (bucket >= ELhashsize) bucket = ELhashsize - 1;

            var he = ELgethash(bucket);

            /// if the HE isn't found, search backwards and forwards in the hash map
            /// for the first non-null entry.
            if (he == null)
            {
                for (int i = 1; i < ELhashsize; i++)
                {
                    if ((he = ELgethash(bucket - i)) != null)
                        break;
                    if ((he = ELgethash(bucket + i)) != null)
                        break;
                }
            }

            /// Now search linear list of halfedges for the correct one.
            if (he == ELleftend || (he != ELrightend && RightOf(he, p)))
            {
                /// Keep going right on the list until either the end is reached, or
                /// you find the 1st edge which the point isn't to the right of.
                do
                {
                    he = he.ELright;
                }
                while (he != ELrightend && RightOf(he, p));
                he = he.ELleft;
            }
            else
            /// If the point is to the left of the HalfEdge, then search left for
            /// the HE just to the left of the point.
            {
                do
                {
                    he = he.ELleft;
                }
                while (he != ELleftend && !RightOf(he, p));
            }

            /// Update hash table and reference counts.
            if (bucket > 0 && bucket < ELhashsize - 1)
            {
                ELhash[bucket] = he;
            }

            return he;
        }

        private Edge Bisect(Point s1, Point s2)
        {
            var dx = s2.X - s1.X;
            var dy = s2.Y - s1.Y;

            var newedge = Math.Abs(dx) > Math.Abs(dy)
                ? new Edge(1.0, dy / dx, (s1.X * dx + s1.Y * dy + (dx * dx + dy * dy) * 0.5) / dx)
                : new Edge(dx / dy, 1.0, (s1.X * dx + s1.Y * dy + (dx * dx + dy * dy) * 0.5) / dy);

            newedge.Reg[0] = s1;
            newedge.Reg[1] = s2;

            return newedge;
        }

        private bool PQempty()
        {
            return (PQcount == 0);
        }

        private Point PQ_min()
        {
            while (PQhash[PQmin].PQnext == null)
            {
                PQmin++;
            }

            return new Point(PQhash[PQmin].PQnext.Vertex.X, PQhash[PQmin].PQnext.YStar);
        }

        private int PQbucket(Halfedge he)
        {
            var bucket = (int)((he.YStar - ymin) / deltay * PQhashsize);
            if (bucket < 0)
                bucket = 0;
            if (bucket >= PQhashsize)
                bucket = PQhashsize - 1;
            if (bucket < PQmin)
                PQmin = bucket;

            return bucket;
        }

        /// Push the HalfEdge into the ordered linked list of vertices.
        private void PQinsert(Halfedge he, Point v, double offset)
        {
            he.Vertex = v;
            he.YStar = v.Y + offset;
            var last = PQhash[PQbucket(he)];

            Halfedge next;

            while ((next = last.PQnext) != null
                && (he.YStar > next.YStar || (he.YStar == next.YStar && v.X > next.Vertex.X)))
            {
                last = next;
            }

            he.PQnext = last.PQnext;
            last.PQnext = he;
            PQcount++;
        }

        /// Remove the HalfEdge from the list of vertices.
        private void PQdelete(Halfedge he)
        {
            if (he.Vertex == null)
            {
                return;
            }

            var last = PQhash[PQbucket(he)];
            while (last.PQnext != he)
            {
                last = last.PQnext;
            }

            last.PQnext = he.PQnext;
            PQcount--;
            he.Vertex = null;
        }

        private Halfedge PQextractMin()
        {
            var curr = PQhash[PQmin].PQnext;
            PQhash[PQmin].PQnext = curr.PQnext;
            PQcount--;

            return curr;
        }

        private void ELinsert(Halfedge lb, Halfedge newHe)
        {
            newHe.ELleft = lb;
            newHe.ELright = lb.ELright;
            (lb.ELright).ELleft = newHe;
            lb.ELright = newHe;
        }

        /// <summary>
        /// This delete routine can't reclaim node, since pointers from hash table
        /// may be present.
        /// </summary>
        private void ELdelete(Halfedge he)
        {
            he.ELleft.ELright = he.ELright;
            he.ELright.ELleft = he.ELleft;
            he.Deleted = true;
        }

        /// <summary>
        /// Get entry from hash table, pruning any deleted nodes.
        /// </summary>
        private Halfedge ELgethash(int b)
        {
            if (0 < b || b >= ELhashsize)
            {
                return null;
            }

            var he = ELhash[b];
            if (he == null || !he.Deleted)
            {
                return he;
            }

            // Hash table points to deleted half edge. Patch as necessary.
            ELhash[b] = null;

            return null;
        }

        private GraphEdge Clip_line(Edge e)
        {
            var x1 = e.Reg[0].X;
            var y1 = e.Reg[0].Y;
            var x2 = e.Reg[1].X;
            var y2 = e.Reg[1].Y;
            var x = x2 - x1;
            var y = y2 - y1;

            // if the distance between the two points this line was created from is
            // less than the square root of 2, then ignore it
            if (Math.Sqrt((x * x) + (y * y)) < MinDistanceBetweenSites)
            {
                return null;
            }

            var pxmin = borderMinX;
            var pymin = borderMinY;
            var pxmax = borderMaxX;
            var pymax = borderMaxY;

            Point s1, s2;

            if (e.A == 1.0 && e.B >= 0.0)
            {
                s1 = e.Ep[1];
                s2 = e.Ep[0];
            }
            else
            {
                s1 = e.Ep[0];
                s2 = e.Ep[1];
            }

            if (e.A == 1.0)
            {
                y1 = pymin;

                if (s1 != null && s1.Y > pymin)
                    y1 = s1.Y;
                if (y1 > pymax)
                    y1 = pymax;
                x1 = e.C - e.B * y1;
                y2 = pymax;

                if (s2 != null && s2.Y < pymax)
                    y2 = s2.Y;
                if (y2 < pymin)
                    y2 = pymin;
                x2 = e.C - e.B * y2;
                if (((x1 > pxmax) & (x2 > pxmax)) | ((x1 < pxmin) & (x2 < pxmin)))
                {
                    return null;
                }

                if (x1 > pxmax)
                {
                    x1 = pxmax;
                    y1 = (e.C - x1) / e.B;
                }
                else if (x1 < pxmin)
                {
                    x1 = pxmin;
                    y1 = (e.C - x1) / e.B;
                }
                if (x2 > pxmax)
                {
                    x2 = pxmax;
                    y2 = (e.C - x2) / e.B;
                }
                else if (x2 < pxmin)
                {
                    x2 = pxmin;
                    y2 = (e.C - x2) / e.B;
                }
            }
            else
            {
                x1 = pxmin;
                if (s1 != null && s1.X > pxmin)
                    x1 = s1.X;
                if (x1 > pxmax)
                    x1 = pxmax;
                y1 = e.C - e.A * x1;

                x2 = pxmax;
                if (s2 != null && s2.X < pxmax)
                    x2 = s2.X;
                if (x2 < pxmin)
                    x2 = pxmin;
                y2 = e.C - e.A * x2;

                if (((y1 > pymax) & (y2 > pymax)) | ((y1 < pymin) & (y2 < pymin)))
                {
                    return null;
                }

                if (y1 > pymax)
                {
                    y1 = pymax;
                    x1 = (e.C - y1) / e.A;
                }
                else if (y1 < pymin)
                {
                    y1 = pymin;
                    x1 = (e.C - y1) / e.A;
                }
                if (y2 > pymax)
                {
                    y2 = pymax;
                    x2 = (e.C - y2) / e.A;
                }
                else if (y2 < pymin)
                {
                    y2 = pymin;
                    x2 = (e.C - y2) / e.A;
                }
            }

            return new GraphEdge(new Point(x1, y1), new Point(x2, y2), e.Reg[0], e.Reg[1]);
        }

        private GraphEdge Endpoint(Edge e, int lr, Point s)
        {
            e.Ep[lr] = s;

            if (e.Ep[RE - lr] == null)
            {
                return null;
            }

            return Clip_line(e);
        }

        /// <summary>
        /// returns true if p is to right of halfedge e.
        /// </summary>
        private bool RightOf(Halfedge el, Point p)
        {
            var e = el.ELedge;
            var topsite = e.Reg[1];
            var right_of_site = p.X > topsite.X;

            if (right_of_site && el.ELpm == LE)
                return true;
            if (!right_of_site && el.ELpm == RE)
                return false;

            bool above;

            if (e.A == 1.0)
            {
                var dxp = p.X - topsite.X;
                var dyp = p.Y - topsite.Y;
                var fast = false;

                if ((!right_of_site & (e.B < 0.0)) | (right_of_site & (e.B >= 0.0)))
                {
                    above = dyp >= e.B * dxp;
                    fast = above;
                }
                else
                {
                    above = p.X + p.Y * e.B > e.C;
                    if (e.B < 0.0)
                        above = !above;
                    if (!above)
                        fast = true;
                }

                if (!fast)
                {
                    var dxs = topsite.X - (e.Reg[0]).X;
                    above = e.B * (dxp * dxp - dyp * dyp) < dxs * dyp * (1.0 + 2.0 * dxp / dxs + e.B * e.B);

                    if (e.B < 0)
                        above = !above;
                }
            }
            else // e.b == 1.0
            {
                var yl = e.C - e.A * p.X;
                var t1 = p.Y - yl;
                var t2 = p.X - topsite.X;
                var t3 = yl - topsite.Y;
                above = t1 * t1 > t2 * t2 + t3 * t3;
            }

            return (el.ELpm == LE ? above : !above);
        }

        private Point RightReg(Halfedge he)
        {
            // if this halfedge has no edge, return the bottom site (whatever that is)
            // if the ELpm field is zero, return the site 0 that this edge bisects,
            // otherwise return site number 1
            return he.ELedge == null ? bottomsite : (he.ELpm == LE ? he.ELedge.Reg[RE] : he.ELedge.Reg[LE]);
        }

        // create a new site where the HalfEdges el1 and el2 intersect - note that
        // the Point in the argument list is not used, don't know why it's there
        private Point Intersect(Halfedge el1, Halfedge el2)
        {
            var e1 = el1.ELedge;
            var e2 = el2.ELedge;

            if (e1 == null || e2 == null)
            {
                return null;
            }

            // if the two edges bisect the same parent, return null
            if (e1.Reg[1] == e2.Reg[1])
            {
                return null;
            }

            var d = e1.A * e2.B - e1.B * e2.A;
            if (-1.0e-10 < d && d < 1.0e-10)
            {
                return null;
            }

            Edge e;
            Halfedge el;

            if ((e1.Reg[1].Y < e2.Reg[1].Y)
                || (e1.Reg[1].Y == e2.Reg[1].Y && e1.Reg[1].X < e2.Reg[1].X))
            {
                el = el1;
                e = e1;
            }
            else
            {
                el = el2;
                e = e2;
            }

            var xint = (e1.C * e2.B - e2.C * e1.B) / d;
            var yint = (e2.C * e1.A - e1.C * e2.A) / d;

            var right_of_site = xint >= e.Reg[1].X;
            if ((right_of_site && el.ELpm == LE)
                || (!right_of_site && el.ELpm == RE))
            {
                return null;
            }

            // create a new site at the point of intersection - this is a new vector
            // event waiting to happen
            return new Point(xint, yint);
        }

        /// <summary>
        /// Implicit parameters: nsites, sqrt_nsites, xmin, xmax, ymin, ymax, deltax,
        /// deltay (can all be estimates). Performance suffers if they are wrong;
        /// better to make nsites, deltax, and deltay too big than too small. (?)
        /// </summary>
        private List<GraphEdge> Voronoi_bd()
        {
            var allEdges = new List<GraphEdge>();

            Point bot, top, temp, p;
            Point newintstar = null;
            Halfedge lbnd, rbnd, llbnd, rrbnd, bisector;
            Edge e;

            PQinitialize();
            ELinitialize();

            bottomsite = NextOne();
            var newsite = NextOne();

            while (true)
            {
                if (!PQempty())
                {
                    newintstar = PQ_min();
                }

                /// If the lowest site has a smaller y value than the lowest vector
                /// intersection, process the site otherwise process the vector intersection.
                if (newsite != null
                    && (PQempty()
                    || newsite.Y < newintstar.Y
                    || (newsite.Y == newintstar.Y && newsite.X < newintstar.X)))
                {
                    /// New site is smallest -this is a site event
                    /// get the first HalfEdge to the LEFT of the new site.
                    lbnd = ELleftbnd(newsite);

                    /// Get the first HalfEdge to the RIGHT of the new site.
                    rbnd = lbnd.ELright;

                    /// If this halfedge has no edge,bot =bottom site (whatever that is).
                    bot = RightReg(lbnd);

                    /// Create a new edge that bisects.
                    e = Bisect(bot, newsite);

                    /// Create a new HalfEdge, setting its ELpm field to 0.
                    bisector = new Halfedge { ELedge = e, ELpm = LE };

                    /// Insert this new bisector edge between the left and right
                    /// vectors in a linked list.
                    ELinsert(lbnd, bisector);

                    /// If the new bisector intersects with the left edge,
                    /// remove the left edge's vertex, and put in the new one.
                    if ((p = Intersect(lbnd, bisector)) != null)
                    {
                        PQdelete(lbnd);
                        PQinsert(lbnd, p, GeometryUtils.Distance(p, newsite));
                    }

                    lbnd = bisector;

                    /// Create a new HalfEdge, setting its ELpm field to 1.
                    bisector = new Halfedge { ELedge = e, ELpm = RE };

                    /// Insert the new HE to the right of the original bisector
                    /// earlier in the IF stmt.
                    ELinsert(lbnd, bisector);

                    /// If this new bisector intersects with the new HalfEdge.
                    if ((p = Intersect(bisector, rbnd)) != null)
                    {
                        /// Push the HE into the ordered linked list of vertices.
                        PQinsert(bisector, p, GeometryUtils.Distance(p, newsite));
                    }

                    newsite = NextOne();
                }
                /// intersection is smallest - this is a vector event
                else if (!PQempty())
                {
                    /// Pop the HalfEdge with the lowest vector off the ordered list
                    /// of vectors.
                    lbnd = PQextractMin();

                    /// Get the HalfEdge to the left of the above HE.
                    llbnd = lbnd.ELleft;

                    /// Get the HalfEdge to the right of the above HE.
                    rbnd = lbnd.ELright;

                    /// Get the HalfEdge to the right of the HE to the right of the
                    /// lowest HE.
                    rrbnd = rbnd.ELright;

                    /// Get the Site to the left of the left HE which it bisects.
                    bot = lbnd.ELedge == null ? bottomsite : (lbnd.ELpm == LE ? lbnd.ELedge.Reg[LE] : lbnd.ELedge.Reg[RE]);

                    /// Get the Site to the right of the right HE which it bisects.
                    top = RightReg(rbnd);

                    /// Get the vertex that caused this event
                    /// earlier since we didn't know when it would be processed.
                    var v = lbnd.Vertex;

                    // Set the endpoint of the left HalfEdge to be this vector.
                    this.AddEdge(Endpoint(lbnd.ELedge, lbnd.ELpm, v), allEdges);

                    // Set the endpoint of the right HalfEdge to be this vector.
                    this.AddEdge(Endpoint(rbnd.ELedge, rbnd.ELpm, v), allEdges);

                    /// mark the lowest HE for deletion - can't delete yet because
                    /// there might be pointers to it in Hash Map.
                    ELdelete(lbnd);

                    /// Remove all vertex events to do with the right HE.
                    PQdelete(rbnd);

                    /// Mark the right HE for deletion - can't delete yet because
                    /// there might be pointers to it in Hash Map.
                    ELdelete(rbnd);

                    /// set the pm variable to zero.
                    var pm = LE;

                    /// If the site to the left of the event is higher than the Site.
                    if (bot.Y > top.Y)
                    {
                        /// To the right of it, then swap them and set the 'pm'
						/// variable to 1.
						temp = bot;
                        bot = top;
                        top = temp;
                        pm = RE;
                    }

                    /// Create an Edge (or line) that is between the two Sites.
                    /// This creates the formula of the line, and assigns a line number to it.
                    e = Bisect(bot, top);

                    /// Create a HE from the Edge 'e', and make it point to that edge
                    /// with its ELedge field.
                    bisector = new Halfedge { ELedge = e, ELpm = pm };

                    /// Insert the new bisector to the right of the left HE.
                    ELinsert(llbnd, bisector);

                    /// Set one endpoint to the new edge to be the vector point 'v'.
                    /// If the site to the left of this bisector is higher than the
                    /// right Site, then this endpoint is put in position 0; otherwise in pos 1.
                    this.AddEdge(Endpoint(e, RE - pm, v), allEdges);

                    /// If left HE and the new bisector intersect, then delete
                    /// the left HE, and reinsert it.
                    if ((p = Intersect(llbnd, bisector)) != null)
                    {
                        PQdelete(llbnd);
                        PQinsert(llbnd, p, GeometryUtils.Distance(p, bot));
                    }

                    /// If right HE and the new bisector intersect, then
                    /// reinsert it.
                    if ((p = Intersect(bisector, rrbnd)) != null)
                    {
                        PQinsert(bisector, p, GeometryUtils.Distance(p, bot));
                    }
                }
                else
                {
                    break;
                }
            }

            for (lbnd = ELleftend.ELright; lbnd != ELrightend; lbnd = lbnd.ELright)
            {
                e = lbnd.ELedge;

                this.AddEdge(Clip_line(e), allEdges);
            }

            return allEdges;
        }

        private bool AddEdge(GraphEdge newEdge, List<GraphEdge> edges)
        {
            if (newEdge == null)
            {
                return false;
            }

            edges.Add(newEdge);

            return true;
        }
    }
}
