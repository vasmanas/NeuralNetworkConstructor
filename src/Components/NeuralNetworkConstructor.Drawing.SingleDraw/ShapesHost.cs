using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace NeuralNetworkConstructor.Drawing.SingleDraw
{
    public class ShapesHost : FrameworkElement, IShapeComposite
    {
        private Lazy<VisualCollection> children;

        private List<Shape> shapes;

        public ShapesHost()
        {
            this.children = new Lazy<VisualCollection>(() => this.Transform());
            this.shapes = new List<Shape>();
        }

        public void AddLine(Brush color, double p0x, double p0y, double p1x, double p1y)
        {
            this.shapes.Add(new LineSegment(color, p0x, p0y, p1x, p1y));
        }

        public void AddPoint(Brush color, double x, double y)
        {
            this.shapes.Add(new Point(color, x, y));
        }

        protected override int VisualChildrenCount
        {
            get { return this.children.Value.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= this.children.Value.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.children.Value[index];
        }

        private VisualCollection Transform()
        {
            var collection = new VisualCollection(this);

            var drawingVisual = new DrawingVisual();

            using (var ctx = drawingVisual.RenderOpen())
            {
                foreach (dynamic shape in this.shapes)
                {
                    this.Transform(ctx, shape);
                }
            }

            collection.Add(drawingVisual);

            return collection;
        }

        private void Transform(DrawingContext context, LineSegment segment)
        {
            context.DrawLine(new Pen(segment.Color, 0.1), new System.Windows.Point(segment.Ax, segment.Ay), new System.Windows.Point(segment.Bx, segment.By));
        }

        private void Transform(DrawingContext context, Point point)
        {
            context.DrawEllipse(point.Color, null, new System.Windows.Point(point.X, point.Y), 1, 1);
        }
    }
}
