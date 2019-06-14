using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class MyCallbackClient : IChartManagerServiceCallback
    {
        public void AllChartVersionsDeleted(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public void ChartChanced(Chart chart, ChartCallBackType type)
        {
            Console.WriteLine("Hi from client!");
        }

        public void ChartChanged(Chart chart, ChartCallBackType type)
        {
            throw new NotImplementedException();
        }

        public void ChartsByEffectiveDateDeleted(Guid identifier, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public void UserChanced(UserCallbackType type)
        {
            Console.WriteLine("Hi from client!");
        }

        public void UserChanged(UserCallbackType type)
        {
            throw new NotImplementedException();
        }
    }
    public class ServiceConnectionStart
    {
        public static ChartManagerServiceClient connected;
        public ChartManagerServiceClient ConnectionLoad()
        {
            var callback = new MyCallbackClient();
            var instanceContext = new InstanceContext(callback);
            ChartManagerServiceClient client = new ChartManagerServiceClient(instanceContext, "netTcp_ChartService");
            client.Endpoint.Address = new EndpointAddress(client.Endpoint.Address.Uri,
                EndpointIdentity.CreateDnsIdentity("ChartManagerTempCert"));
            return client;
        }
    }
}