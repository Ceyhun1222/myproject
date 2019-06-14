#region

using System.Dynamic;
using Aran.Temporality.Internal.Remote.ClientServer;
using Aran.Temporality.Internal.Remote.Protocol;

#endregion

namespace Aran.Temporality.Aim.Storage
{
    internal class AimRemoteStorage : DynamicObject
    {
        private readonly TemporalityClient _client;
        private readonly string _path;
        
        public AimRemoteStorage(string path, string username, string password, string serviceAddress = "localhost:8523")
        {
            _path = path;
            _client = new TemporalityClient();
            _client.Open(username, password, serviceAddress);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var request = new CommunicationRequest {MethodName = binder.Name, Params = args, Path = _path};
            result = _client.ProcessBinaryMessage(request);
            return true;
        }
    }
}