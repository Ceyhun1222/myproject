using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using AranUpdater;
using AranUpdManager;

namespace AranUpd
{
    public class AUServerStarter
    {
        private static ServiceHost _updaterHost;

        public static void StartUpdateServer()
        {
            _updaterHost = CreateService(typeof(AUServer), typeof(IAUServer), 4433);
            _updaterHost.Open();
        }

        public static void StopUpdateServer()
        {
            if (_updaterHost != null)
                _updaterHost.Abort();
        }
        
        private static ServiceHost CreateService(Type serviceType, Type implementedContract, int port)
        {
            var host = new ServiceHost(serviceType, new Uri("http://localhost:" + port));

            var binding = new WebHttpBinding
            {
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = 1024 * 1014 * 100,
                MaxBufferSize = 1024 * 1014 * 100,
                MaxBufferPoolSize = 1024 * 1014 * 100,
                SendTimeout = new TimeSpan(0, 4, 0),
                ReceiveTimeout = new TimeSpan(0, 4, 0)
            };

            var endpoint = host.AddServiceEndpoint(implementedContract, binding, "");
            endpoint.Behaviors.Add(new WebHttpBehavior());

            var behavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (behavior != null)
                behavior.IncludeExceptionDetailInFaults = false;
            else
                host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

            host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
            return host;
        }
    }
}
