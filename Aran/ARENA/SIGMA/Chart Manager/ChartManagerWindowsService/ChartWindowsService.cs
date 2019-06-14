using ChartServices.Security;
using ChartServices.Service;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceProcess;
using System.IdentityModel.Policy;
using ChartServices;

namespace ChartManagerWindowsService
{
    public partial class ChartWindowsService : ServiceBase
    {
        ServiceHost host = null;
        public ChartWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            host = new ServiceHost(typeof(ChartManagerService));

            host.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager();
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy> { new AuthorizationPolicy() };
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            host.Open();
        }

        protected override void OnStop()
        {
            host?.Close();
        }
    }
}
