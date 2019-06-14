using PVT.Engine.Common.Logging;
using System;

namespace PVT.Engine.IAIM.Logging
{
    class Logger : ILogger
    {
        private readonly Aran.AranEnvironment.ILogger _logger;

        public Logger()
        {
            var aranEnv = ((IAIMEnvironment)Environment.Current).AranEnv;
            _logger = aranEnv.GetLogger(Environment.Name);
        }

        public void Debug(string message, object argument)
        {
            _logger.Debug(message, argument);
        }

        public void Error(string message, object argument)
        {
            _logger.Error(message, argument);
        }

        public void Error(Exception ex, string message)
        {
            _logger.Error(ex, message);
        }

        public void Fatal(string message, object argument)
        {
            _logger.Fatal(message, argument);
        }

        public void Info(string message, object argument)
        {
            _logger.Info(message, argument);
        }

        public void Trace(string message, object argument)
        {
            _logger.Trace(message, argument);
        }

        public void Warn(string message, object argument)
        {
            _logger.Warn(message, argument);
        }
    }
}
