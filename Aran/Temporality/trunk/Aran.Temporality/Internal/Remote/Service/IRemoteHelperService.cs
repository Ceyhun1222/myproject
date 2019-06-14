using System;
using System.ServiceModel;

namespace Aran.Temporality.Internal.Remote.Service
{
    [ServiceContract]
    public interface IRemoteHelperService
    {
        [OperationContract]
        //should process CommunicationRequest and return CommunicationResult
        DateTime GetServerTime(int userId);

        [OperationContract]
        string GetUserName(int userId);

        [OperationContract]
        bool IsUserSecured(int userId);
    }
}
