using ChartServices.Logging;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace ChartServices.Helpers
{
    class MainErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            LogManager.GetLogger(error.TargetSite.Name).Error(error, $"{error.Message}");
            //Здесь можно делать логирование
            //Выполняется в отдельном потоке
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is FaultException)
                return;

            var flt = new FaultException(error.Message, new FaultCode("ServiceInnerException"));
            var msg = flt.CreateMessageFault();
            fault = Message.CreateMessage(version, msg, flt.Action);
        }
    }
}