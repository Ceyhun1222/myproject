namespace PVT.Engine.Common.Converters
{
    class ConvertersManager
    {
        private static IConverters _converters;
        public static IConverters Converters
        {
            get
            {
                if (_converters != null) return _converters;
                if (Environment.Current == null)
                    return null;

                if (Environment.Current.Value == Environments.IAIM)
                    _converters = new IAIM.Converters.Converters();
                if (Environment.Current.Value == Environments.CDOTMA)
                    _converters = new CDOTMA.Converters.Converters();
                return _converters;
            }
        }
    }
}
