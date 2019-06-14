namespace PVT.Engine.Common.Utils
{

    class UtilsManager
    {
        private static IUtils _utils;
        public static IUtils Utils
        {
            get
            {
                if (_utils == null)
                {
                    if (Environment.Current == null)
                        return null;

                    if (Environment.Current.Value == Environments.IAIM)
                        _utils = new IAIM.Utils.Utils();
                }
                return _utils;
            }
        }
    }

}
