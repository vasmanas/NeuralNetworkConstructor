using System;

namespace NeuralNetworkConstructor.Core.Dependency
{
    public interface IServiceLocator
    {
        object GetService(Type type);
    }
}
