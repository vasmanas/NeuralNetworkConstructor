using NeuralNetworkConstructor.Algorithms;
using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.Diagrams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class MakeConvexHullHandler : IRequestHandler<MakeConvexHull, List<LineSegment>>
    {
        public async Task<List<LineSegment>> Handle(MakeConvexHull request)
        {
            var dispatcher = request.Dispatcher;
            var log = request.Log;
            var points = request.Points;

            log.Info("Starting");

            IConvexHullAlgorithm algorythm = new GiftWrappingAlgorithm();
            //IConvexHullCalculationAlgorithm algorythm = new GrahamScanAlgorithm();

            var hullPoints = algorythm.Calculate(points);
            var lines = new List<LineSegment>();

            for (var i = hullPoints.Count - 1; i >= 0; i--)
            {
                request.Log.Info($"Drawing line {i}");

                var a = hullPoints[i];
                var b = i == 0 ? hullPoints[hullPoints.Count - 1] : hullPoints[i - 1];

                lines.Add(new LineSegment(a, b));
            }

            return lines;
        }
    }
}
