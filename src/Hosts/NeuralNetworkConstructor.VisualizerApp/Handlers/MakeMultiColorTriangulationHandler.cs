using NeuralNetworkConstructor.Algorithms;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class MakeMultiColorTriangulationHandler : IMessageHandler<MakeMultiColorTriangulation>
    {
        public async Task Handle(MakeMultiColorTriangulation request)
        {
            var dispatcher = request.Dispatcher;
            var log = request.Log;
            var points = request.Categories.SelectMany(e => e.Value).ToList();

            log.Info("Calculating");

            ITriangulationAlgorithm algorythm = new BowyerWatsonAlgorithm();

            var triangles = algorythm.Calculate(points);

            log.Info("Calculated");

            var composite = request.Composite;

            var totalCount = triangles.Count;
            var drawnCount = 0;

            log.Info($"{drawnCount}/{totalCount}");

            foreach (var triangle in triangles)
            {
                Brush f1b = null;
                Brush f2b = null;
                Brush f3b = null;
                Diagrams.Point p1 = null;
                Diagrams.Point p2 = null;
                Diagrams.Point p3 = null;

                foreach (var category in request.Categories)
                {
                    foreach (var point in category.Value)
                    {
                        if (point.X == triangle.P1.X && point.Y == triangle.P1.Y)
                        {
                            p1 = point;
                            f1b = category.Key;

                            if (f2b != null && f3b != null)
                            {
                                break;
                            }
                        }
                        else if (point.X == triangle.P2.X && point.Y == triangle.P2.Y)
                        {
                            p2 = point;
                            f2b = category.Key;

                            if (f1b != null && f3b != null)
                            {
                                break;
                            }
                        }
                        else if (point.X == triangle.P3.X && point.Y == triangle.P3.Y)
                        {
                            p3 = point;
                            f3b = category.Key;

                            if (f1b != null && f2b != null)
                            {
                                break;
                            }
                        }
                    }

                    if (f1b != null && f2b != null && f3b != null)
                    {
                        break;
                    }
                }

                await this.DrawEdge(dispatcher, composite, f1b, p1, f2b, p2);
                await this.DrawEdge(dispatcher, composite, f2b, p2, f3b, p3);
                await this.DrawEdge(dispatcher, composite, f1b, p1, f3b, p3);

                drawnCount++;

                if (drawnCount % 10 == 0)
                {
                    log.Info($"{drawnCount}/{totalCount}");
                }
            }

            log.Info($"{drawnCount}/{totalCount}");
        }

        private async Task DrawEdge(Dispatcher dispatcher, IShapeComposite composite, Brush b1, Diagrams.Point p1, Brush b2, Diagrams.Point p2)
        {
            if (b1 == b2)
            {
                await this.AddLine(dispatcher, composite, b1, p1.X, p1.Y, p2.X, p2.Y);
            }
            else
            {
                var xmidpoint = (p1.X + p2.X) / 2;
                var ymidpoint = (p1.Y + p2.Y) / 2;

                await this.AddLine(dispatcher, composite, b1, p1.X, p1.Y, xmidpoint, ymidpoint);
                await this.AddLine(dispatcher, composite, b2, xmidpoint, ymidpoint, p2.X, p2.Y);
            }
        }

        private async Task AddLine(Dispatcher dispatcher, IShapeComposite composite, Brush brush, double ax, double ay, double bx, double by)
        {
            await dispatcher.BeginInvoke(new Action(() => { composite.AddLine(brush, ax, ay, bx, by); }), DispatcherPriority.ContextIdle);
        }
    }
}
