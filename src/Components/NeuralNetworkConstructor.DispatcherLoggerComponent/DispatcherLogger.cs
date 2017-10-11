using NeuralNetworkConstructor.Core.Logging;
using System;
using System.Windows.Threading;

namespace NeuralNetworkConstructor.DispatcherLoggerComponent
{
    public class DispatcherLogger : ILog
    {
        private readonly Action<string> action;
        private readonly Dispatcher dispatcher;

        public DispatcherLogger(Dispatcher dispatcher, Action<string> action)
        {
            this.action = action;
            this.dispatcher = dispatcher;
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string message, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string message, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            dispatcher.BeginInvoke(action, DispatcherPriority.ContextIdle, $"Info: {message}");
        }

        public void InfoFormat(string message, params object[] parameters)
        {
            this.Info(string.Format(message, parameters));
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string message, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
