using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIP.DB
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

            Configuration config = ConfigurationManager.OpenExeConfiguration("eAIP.exe");
            var fo = config?.ConnectionStrings?.ConnectionStrings[eAIPContext.DefaultConnectionName]
                ?.ConnectionString;
            return fo ?? "";
        }

    }
}
