using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            MainTest();
            return;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AranUpdService() 
            };
            ServiceBase.Run(ServicesToRun);
        }

        static void MainTest()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AranUpdService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
