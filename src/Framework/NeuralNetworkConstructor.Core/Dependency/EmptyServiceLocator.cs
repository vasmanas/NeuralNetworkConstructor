using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkConstructor.Core.Dependency
{
    public class EmptyServiceLocator : IServiceLocator
    {
        public object GetService(Type type)
        {
            return null;
        }
    }
}
