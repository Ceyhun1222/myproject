using AerodromeServices.Services_Contract;
using System;
using System.Linq;
using AerodromeServices.Helpers;
using AerodromeServices.Logging;
using System.ServiceModel;

namespace AerodromeServices.Security
{
    public class CustomServiceAuthorizationManager : ServiceAuthorizationManager
    {


        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            OperationContext oc = OperationContext.Current;

            oc.Channel.Closed += Channel_Closed;
            var callback = operationContext.GetCallbackChannel<IAmdbManagerServiceCallback>();

            var name = operationContext.ServiceSecurityContext.PrimaryIdentity.Name;
            if (!SessionStorage.IsExist(operationContext.SessionId))
            {
                SessionStorage.Add(new SessionData() { SessionId = operationContext.SessionId, UserName = name },
                    callback);
                LogManager.GetLogger(GetType().Name).Info($"{name} | {operationContext.SessionId} | ---------- New session is created ---------- ");
            }

            return true;

        }

        private void Channel_Closed(object sender, EventArgs e)
        {
            var sessionId = ((IClientChannel)sender).SessionId;
            
            if (SessionStorage.IsExist(sessionId))
            {
                var name = SessionStorage.Sessions.FirstOrDefault(c => c.Key.SessionId == sessionId).Key.UserName;
                LogManager.GetLogger(GetType().Name).Info($"{name} | {sessionId} | ---------- Session closed ---------- ");
                SessionStorage.Remove(sessionId);
            }
        }
    }
}
