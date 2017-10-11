using NeuralNetworkConstructor.Core.Dependency;
using NeuralNetworkConstructor.Core.Logging;
using NeuralNetworkConstructor.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.VisualizerApp.Configuration
{
    public static class Bootstrap
    {
        public static void Configure()
        {
            IServiceLocator locator = new SimpleResolver();

            ILog<InMemoryBus> log = new EmptyLog<InMemoryBus>();

            var bus = new InMemoryBus(locator, log);//container.Resolve<IBus>();
            Bus.SetBus(bus);

            BusConfig.RegisterHandlers();
        }
    }
}
