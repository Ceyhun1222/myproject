#region

using System;
using System.Dynamic;
using System.Windows;
using Aran.Temporality.Internal.Remote.ClientServer;
using Aran.Temporality.Internal.Remote.Protocol;
using System.Web;
using Aran.Temporality.Common.Logging;

#endregion

namespace Aran.Temporality.Internal.Service
{
    internal class AimRemoteService : DynamicObject
    {
        private readonly TemporalityClient _client;
        private readonly string _storage;

        public bool IsOpened;
        public bool VerboseErrors;

        public void Close()
        {
            _client?.Close();
        }

        public AimRemoteService(string storage, 
            string userId,
            string userPassword, 
            string serviceAddress = "localhost:8523")
        {
            try
            {
                _storage = storage;
                _client = new TemporalityClient();
                _client.Open(userId, userPassword, serviceAddress);
                IsOpened = true;
            }
            catch (Exception)
            {
                IsOpened = false;
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            if (binder.Name=="Dispose")
            {
                VerboseErrors = false;
                Close();
                return true;
            }

            try
            {
                var request = new CommunicationRequest
                {
                    MethodName = binder.Name,
                    Params = args,
                    Storage = _storage
                };
                result = _client.ProcessBinaryMessage(request);

                var list = result as System.Collections.IList;
                if (list!=null && list.Count==0)
                {
                //problem with list 
                }
               

                if (binder.Name == "Login")
                {
                    VerboseErrors = true;
                }

                return true;
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(this).Info("Request failed.", binder.Name, args);
                LogManager.GetLogger(this).ErrorRecursive(exception);
                if (VerboseErrors)
                {
                    if (HttpRuntime.AppDomainAppId == null)
                        MessageBox.Show(exception.Message, "Error on server side", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return true;
        }
    }
}