using NeuralNetworkConstructor.Core.Messaging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NeuralNetworkConstructor.Algorithms;

namespace NeuralNetworkConstructor.VisualizerApp.Handlers
{
    public class GenerateRandomPointsHandler : IRequestHandler<GenerateRandomPoints, List<Diagrams.Point>>
    {
        public Task<List<Diagrams.Point>> Handle(GenerateRandomPoints request)
        {
            var dispatcher = request.Dispatcher;
            var log = request.Log;
            var count = request.Count;

            log.Info($"Generating {count} points");

            var generatedPoints = new List<Diagrams.Point>();

            while (count > 0)
            {
                var points = PointGenerator.GeneratePoints(count, 0, request.Width, 0, request.Height);

                foreach (var point in points)
                {
                    if (request.PlacedPoints.All(p => p.X != point.X || p.Y != point.Y)
                        && generatedPoints.All(p => p.X != point.X || p.Y != point.Y))
                    {
                        generatedPoints.Add(point);

                        count--;
                    }
                }
            }

            return Task.FromResult(generatedPoints);
        }
    }
}
