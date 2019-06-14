#region

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Xml;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Remote.Protocol;
using Aran.Temporality.Internal.Remote.Service;
using Aran.Temporality.Common.Logging;

#endregion

namespace Aran.Temporality.Internal.Remote.ClientServer
{
    internal class TemporalityClient
    {
        #region Common client

        private IRemoteService _proxy;
        private ChannelFactory<IRemoteService> _tcpFactory;

        public void Close()
        {
            _proxy = null;

            if (_tcpFactory != null)
            {
                _tcpFactory.Close();
                _tcpFactory = null;
            }
        }

        public void Open(string userId,
            string userPassword, 
            string serviceAddress = null)
        {
            if (serviceAddress == null)
            {
                serviceAddress = ConfigUtil.TemporalityServerAddress;
            }

            var binding = new NetTcpBinding
                              {
                                  Security =
                                      {
                                          Mode = SecurityMode.Message,
                                          Message = {ClientCredentialType = MessageCredentialType.UserName}
                                      }
                              };

            var readerQuotas = XmlDictionaryReaderQuotas.Max;
            binding.ReaderQuotas = readerQuotas;
            binding.MaxReceivedMessageSize = 1024 * 1014 * 100;
            binding.MaxBufferSize = 1024 * 1014 * 100;
            binding.MaxBufferPoolSize = 1024 * 1014 * 100;
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
       
            var address =
                new EndpointAddress(new Uri("net.tcp://" + serviceAddress + "/" + ConfigUtil.TemporalityServerEndPoint),
                                    EndpointIdentity.CreateDnsIdentity(ConfigUtil.TemporalityServerCertificateSubject));
            _tcpFactory = new ChannelFactory<IRemoteService>(binding, address);

            _tcpFactory.Faulted+=TcpFactoryFaulted;

            _tcpFactory.Credentials?.ClientCertificate.SetCertificate(StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindByIssuerName,//.FindBySubjectName,
                ConfigUtil.TemporalityServerCertificateIssuer);

            SetUser(userId);
            SetPassword(userPassword);

            _proxy = _tcpFactory.CreateChannel();
            ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
        }

        public bool SetUser(string userId)
        {
            if (_tcpFactory?.Credentials == null) return false;

            _tcpFactory.Credentials.UserName.UserName = userId;

            //TODO: reset proxy?
            return true;
        }

        public bool SetPassword(string password)
        {
            if (_tcpFactory?.Credentials == null) return false;

            _tcpFactory.Credentials.UserName.Password = password;


            //TODO: reset proxy?
            return true;
        }

        private void TcpFactoryFaulted(object sender, EventArgs e)
        {
            _proxy = _tcpFactory.CreateChannel();
            ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
        }

        private void ClientFaulted(object sender, EventArgs e)
        {
            _proxy = _tcpFactory.CreateChannel();
            ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
        }


        #endregion

        #region Logic

        public object ProcessBinaryMessage(CommunicationRequest request)
        {
            byte[] binaryRequest = FormatterUtil.ObjectToBytes(request);

            CommunicationResponse response;
            try
            {
                byte[] binaryResponse = _proxy.ProcessBinaryMessage(binaryRequest);
                response = FormatterUtil.ObjectFromBytes<CommunicationResponse>(binaryResponse);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(TemporalityClient)).Error(ex);
                throw new Exception("Server exception: " + ex.Message);
            }

            if (response == null)
            {
                throw new Exception("Server returned bad response");
            }

            if (!response.OperationResult.IsOk)
            {
                throw new Exception("Server exception: " + response.OperationResult.ErrorMessage);
            }

            return response.ResultObject;
        }

        #endregion
    }
}