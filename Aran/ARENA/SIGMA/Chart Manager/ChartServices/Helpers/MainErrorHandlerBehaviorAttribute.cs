using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ChartServices.Helpers
{
    class MainErrorHandlerBehaviorAttribute :Attribute, IServiceBehavior
    {
        private readonly Type _errorHandlerType;

        public MainErrorHandlerBehaviorAttribute(Type errorHandlerType)
        {
            _errorHandlerType = errorHandlerType;
        }
        
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            IErrorHandler handler = (IErrorHandler)Activator.CreateInstance(_errorHandlerType);
            foreach (var item in serviceHostBase.ChannelDispatchers)
            {
                var chDisp = item as ChannelDispatcher;

                chDisp?.ErrorHandlers.Add(handler);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //Проверка корректности параметров сервиса
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //Параметры binding-a
        }

    }
}
