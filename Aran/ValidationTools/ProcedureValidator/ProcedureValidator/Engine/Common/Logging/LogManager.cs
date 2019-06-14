namespace PVT.Engine.Common.Logging
{
    class LogManager
    {
        private static ILogger _logger;
        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    if (Environment.Current == null)
                        return null;

                    if (Environment.Current.Value == Environments.IAIM)
                        _logger = new IAIM.Logging.Logger();
                    if (Environment.Current.Value == Environments.CDOTMA)
                        _logger = new CDOTMA.Logging.Logger();
                }
                return _logger;
            }
        }
    }
}
