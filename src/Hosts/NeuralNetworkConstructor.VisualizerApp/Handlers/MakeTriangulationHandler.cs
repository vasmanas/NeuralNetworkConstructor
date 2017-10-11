using NeuralNetworkConstructor.Algorithms;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Drawing.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class MakeTriangulationHandler : IMessageHandler<MakeTriangulation>
    {
        public async Task Handle(MakeTriangulation request)
        {
            var dispatcher = request.Dispatcher;
            var log = request.Log;
            var points = request.Points;

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
                await this.AddLines(
                    dispatcher,
                    composite,
                    Brushes.PaleVioletRed,
                    new double[3, 4]
                    {
                        { triangle.P1.X, triangle.P1.Y, triangle.P2.X, triangle.P2.Y },
                        { triangle.P2.X, triangle.P2.Y, triangle.P3.X, triangle.P3.Y },
                        { triangle.P3.X, triangle.P3.Y, triangle.P1.X, triangle.P1.Y }
                    }
                    );

                drawnCount++;

                if (drawnCount % 10 == 0)
                {
                    log.Info($"{drawnCount}/{totalCount}");
                }
            }

            log.Info($"{drawnCount}/{totalCount}");
        }

        private async Task AddLines(Dispatcher dispatcher, IShapeComposite composite, Brush brush, double[,] lines)
        {
            await dispatcher.BeginInvoke(new Action(() => {
                for (int i = lines.GetLength(0) - 1; i >= 0; i--)
                {
                    composite.AddLine(brush, lines[i, 0], lines[i, 1], lines[i, 2], lines[i, 3]);
                }
            }), DispatcherPriority.ContextIdle);
        }
    }
}
