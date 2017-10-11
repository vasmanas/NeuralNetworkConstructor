using NeuralNetworkConstructor.Core.Messaging;
using NeuralNetworkConstructor.VisualizerApp.Handlers;

namespace NeuralNetworkConstructor.VisualizerApp.Configuration
{
    public class BusConfig
    {
        /// <summary>
        /// Registers the handlers.
        /// </summary>
        public static void RegisterHandlers()
        {
            Bus.RegisterHandler<CalculateVoronoiEdgesHandler>();
            Bus.RegisterHandler<CreateShapesContainerHandler>();
            Bus.RegisterHandler<FilterEdgesTouchingSameCategoryHandler>();
            Bus.RegisterHandler<DrawPointsHandler>();
            Bus.RegisterHandler<DrawLineSegmentsHandler>();
            Bus.RegisterHandler<DrawEdgesHandler>();
            Bus.RegisterHandler<DrawPolygonHandler>();

            Bus.RegisterHandler<MakeConvexHullHandler>();
            Bus.RegisterHandler<MakeTriangulationHandler>();
            Bus.RegisterHandler<MakeMultiColorTriangulationHandler>();
            Bus.RegisterHandler<GenerateRandomPointsHandler>();
        }
    }
}
