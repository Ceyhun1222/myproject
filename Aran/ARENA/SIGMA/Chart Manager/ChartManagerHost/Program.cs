using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using ChartServices.Service;
using ChartServices;
using System.IdentityModel.Policy;
using ChartServices.Security;
using ChartServices.Logging;

namespace ChartManagerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;
            
            try
            {
                host = new ServiceHost(typeof(ChartManagerService));

                host.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager();
                List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy> { new AuthorizationPolicy() };
                host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

                LogManager.Configure("ChartManagerService.log", "ChartManagerService_Errorlogs.log", LogLevel.Info);

                host.Open();
                Console.WriteLine($"Service started at {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
                Console.WriteLine($"Press any key to stop");
                Console.ReadKey();                
            }
            catch (Exception e)
            {
                host?.Close();
                Console.WriteLine(e);
                Console.Read();
                
            }
        }
    }
}
