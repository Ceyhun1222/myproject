using System.ServiceProcess;

namespace ChartManagerWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ChartWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
