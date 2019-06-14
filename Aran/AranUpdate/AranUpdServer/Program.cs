using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using AranUpd;
using AranUpdater;
using AranUpdManager;

namespace AranUpdServer
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Aran Updater and Manager Server started.");

            try
            {
                AUServerStarter.StartUpdateServer();

                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Global.WriteLog("Error: " + ex.Message);
            }
        }

        
    }
}
