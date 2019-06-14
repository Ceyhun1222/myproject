using System;
using System.ServiceModel;
using System.Xml;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Internal.Remote.Service;

namespace Aran.Temporality.Internal.Remote.ClientServer
{
    public class HelperClient
    {
        #region Common client

        private ChannelFactory<IRemoteHelperService> _tcpFactory;

        public IRemoteHelperService Proxy { get; private set; }

        public void Close()
        {
            Proxy = null;

            if (_tcpFactory != null)
            {
                _tcpFactory.Close();
                _tcpFactory = null;
            }
        }

        public void Open(string serviceAddress = null)
        {
            if (serviceAddress == null)
            {
                serviceAddress = ConfigUtil.HelperServerAddress;
            }

            var binding = new NetTcpBinding
                              {
                                  Security =
                                      {
                                          Mode = SecurityMode.None
                                      },
                                  ReaderQuotas = XmlDictionaryReaderQuotas.Max,
                                  MaxReceivedMessageSize = 1024*1014*100,
                                  MaxBufferSize = 1024*1014*100,
                                  MaxBufferPoolSize = 1024*1014*100,
                                  SendTimeout = new TimeSpan(0, 10, 0),
                                  ReceiveTimeout = new TimeSpan(0, 10, 0)
                              };


            var address = new EndpointAddress(new Uri("net.tcp://" + serviceAddress + "/" + ConfigUtil.HelperServerEndPoint));
            _tcpFactory = new ChannelFactory<IRemoteHelperService>(binding, address);

            _tcpFactory.Faulted += TcpFactoryFaulted;

            Proxy = _tcpFactory.CreateChannel();
            ((ICommunicationObject)Proxy).Faulted += ClientFaulted;
        }

     
        private void TcpFactoryFaulted(object sender, EventArgs e)
        {
            Proxy = _tcpFactory.CreateChannel();
            ((ICommunicationObject)Proxy).Faulted += ClientFaulted;
        }

        private void ClientFaulted(object sender, EventArgs e)
        {
            Proxy = _tcpFactory.CreateChannel();
            ((ICommunicationObject)Proxy).Faulted += ClientFaulted;
        }


        #endregion

    }
}
