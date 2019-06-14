#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;

#endregion

namespace VisualizerCommon.Remote
{
    public sealed class VisualizerServer : IVisualizerServer
    {
        public static string PipeName = "VisualizerServer";
        private static ServiceHost _host;
        public static void StartServer()
        {
            _host = new ServiceHost(typeof(VisualizerServer), new Uri("net.pipe://localhost/"));
            _host.AddServiceEndpoint(typeof(IVisualizerServer), new NetNamedPipeBinding(){MaxReceivedMessageSize = int.MaxValue}, PipeName);
            _host.Open();

            Console.WriteLine("Service started. Available in following endpoints");
            foreach (var serviceEndpoint in _host.Description.Endpoints)
            {
                Console.WriteLine(serviceEndpoint.ListenUri.AbsoluteUri);
            }
        }
        public static Action<List<GeometrySelection>> SetSelectionHandler { get; set; }
        public static Action ClearSelectionHandler { get; set; }
        public void SetSelection(List<GeometrySelection> selection)
        {
            try
            {
                if (SetSelectionHandler != null)
                {
                    SetSelectionHandler(selection);
                }
            }
            catch (Exception exception)
            {
                
            }
            
        }
        public void ClearSelection()
        {
            try
            {
                if (ClearSelectionHandler != null)
                {
                    ClearSelectionHandler();
                }
            }
            catch (Exception exception)
            {
                
            }
           
        }
    }
}