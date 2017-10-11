using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Core.Dependency
{
    public class SimpleResolver : IServiceLocator
    {
        public object GetService(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
