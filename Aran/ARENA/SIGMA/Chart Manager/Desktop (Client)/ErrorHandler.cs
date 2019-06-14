using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using ChartManager.Helper;
using ChartManager.Logging;

namespace ChartManager
{
    public static class ErrorHandler
    {
        public static void Handle(object obj, Exception ex, INotifyService notifyService, string message = "", [CallerMemberName]string memberName="")
        {
            string dialogMsg = ex.Message;
            LogManager.GetLogger(obj).Error(ex, $"{memberName}|{message}");
            if (ex is FaultException)
            {
                
            }
            else if (ex is MessageSecurityException)
            {
                if (ex.InnerException?.Message == "An error occurred when verifying security for the message")
                {
                    dialogMsg = "Authorization failed due to invalid username or password.Please, contact your administrator for further information";
                }
            }
            else
            {
                
            }
            notifyService.ShowMessage(dialogMsg, MessageType.Error);
        }
    }
}