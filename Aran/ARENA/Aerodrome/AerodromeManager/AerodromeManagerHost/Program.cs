using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.IdentityModel.Policy;
using AerodromeServices.Service;
using AerodromeServices.Logging;
using AerodromeServices.Security;

namespace AerodromeManagerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;
            
            try
            {
                host = new ServiceHost(typeof(AmdbManagerService));

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
