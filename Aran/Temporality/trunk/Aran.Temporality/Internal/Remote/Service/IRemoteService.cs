#region

using System.ServiceModel;

#endregion

namespace Aran.Temporality.Internal.Remote.Service
{
    [ServiceContract]
    internal interface IRemoteService
    {
        [OperationContract]
        //should process CommunicationRequest and return CommunicationResult
        byte[] ProcessBinaryMessage(byte[] binary);
    }
}