using AutoMapper;
using ChartServices.DataContract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ChartManagerServices.Helpers
{
    public class AutomapBootstrap
    {
        public static void InitializeMap()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ChartWithReference, Chart>();
            });
        }
    }

    public sealed class AutomapServiceBehavior : Attribute, IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            AutomapBootstrap.InitializeMap();
        }
    }
}
