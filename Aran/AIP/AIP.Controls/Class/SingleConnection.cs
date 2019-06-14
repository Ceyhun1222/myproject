using System;
using System.Configuration;

namespace AIP.BaseLib.Class
{
    /// <summary>
    /// Class to get ConnectionString from eAIP.exe.config
    /// </summary>
    public class SingleConnection
    {
        static SingleConnection() { }
        private static SingleConnection _ConsString = null;
        private String _String = null;

        public static string String
        {
            get
            {
                if (_ConsString == null)
                {
                    _ConsString = new SingleConnection { _String = SingleConnection.Connect() };
                    return _ConsString._String;
                }
                else
                    return _ConsString._String;
            }
        }

        public static string Connect()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration("AIP.BaseLib.dll");
            var fo = config?.ConnectionStrings?.ConnectionStrings["MongoDBConnection"]?.ConnectionString;
            return (fo != null) ?  fo : "";
        }

}
}
