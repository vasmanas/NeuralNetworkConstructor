using System;

namespace NeuralNetworkConstructor.Core.Logging
{
    public class EmptyLog : EmptyLog<object>
    {
    }
    
    /// <summary>
    /// Empty logger.
    /// </summary>
    public class EmptyLog<T> : ILog<T>
    {
        public void Debug(string message)
        {
        }

        public void DebugFormat(string message, params object[] parameters)
        {
        }

        public void Info(string message)
        {
        }

        public void InfoFormat(string message, params object[] parameters)
        {
        }

        public void Warn(string message)
        {
        }

        public void WarnFormat(string message, params object[] parameters)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception exception)
        {
        }

        public void ErrorFormat(string message, params object[] parameters)
        {
        }

        public void ErrorFormat(string message, Exception ex, params object[] parameters)
        {
        }

        public void Test(string message)
        {            
        }

        public void TestFormat(string message, params object[] parameters)
        {
            
        }
    }
}
