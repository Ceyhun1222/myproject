using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace VisualizerCommon.Remote
{
    public sealed class VisualizerClient : IVisualizerServer
    {
        private IVisualizerServer _proxy;
        private ChannelFactory<IVisualizerServer> _pipeFactory;

        public void Close()
        {
            _proxy = null;
            if (_pipeFactory != null)
            {
                _pipeFactory.Close();
                _pipeFactory = null;
            }
        }

        public VisualizerClient()
        {
            _pipeFactory = new ChannelFactory<IVisualizerServer>(new NetNamedPipeBinding(){MaxReceivedMessageSize = int.MaxValue}, 
                new EndpointAddress("net.pipe://localhost/" + VisualizerServer.PipeName));
            _pipeFactory.Faulted += TcpFactoryFaulted;
            _proxy = _pipeFactory.CreateChannel();
            ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
        }

        private void TcpFactoryFaulted(object sender, EventArgs e)
        {
            _proxy = _pipeFactory.CreateChannel();
            ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
        }

        private void ClientFaulted(object sender, EventArgs e)
        {
            _proxy = _pipeFactory.CreateChannel();
            ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
        }

        public void SetSelection(List<GeometrySelection> selection)
        {
            int g = 0;
            try
            {
                _proxy = _pipeFactory.CreateChannel();
                ((ICommunicationObject)_proxy).Faulted += ClientFaulted;
                _proxy.SetSelection(selection);
            }
            catch (Exception exception)
            {
            
            }
            
        }

        public void ClearSelection()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.ClearSelection();
                }
            }
            catch (Exception exception)
            {
                int g = 0;
            }
           
        }
    }
}
