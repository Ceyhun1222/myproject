using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Aran.Aim.DBService
{
    public partial class MainForm : Form
    {
        private ServiceHost _host;
        public static MainForm This;

        public MainForm ()
        {
            InitializeComponent ();

            This = this;
        }

        private void exitToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Stop ();

            exitToolStripMenuItem.Tag = true;
            Close ();
        }

        private void MainForm_Load (object sender, EventArgs e)
        {
            Start ();

            //Thread thread = new Thread (new ThreadStart (ThreadFunction));
            //thread.Start ();
        }

        private void ThreadFunction ()
        {
            Visible = false;
        }

        private void showWindowToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            Visible = true;
        }

        private void MainForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (exitToolStripMenuItem.Tag == null)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void startToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Start ();
        }

        private void stopToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Stop ();
        }

        private void Start ()
        {
            if (_host == null)
            {
                BasicHttpBinding binding = new BasicHttpBinding ();
                binding.Name = "binding1";
                binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                binding.Security.Mode = BasicHttpSecurityMode.None;
                binding.MaxBufferSize = 65536000;
                binding.MaxBufferPoolSize = 524288000;
                binding.MaxReceivedMessageSize = 65536000;

                binding.ReaderQuotas.MaxStringContentLength = 8192000;

                Uri baseAddress = new Uri ("http://localhost:8080/AranDbService");
                Uri address = new Uri ("http://localhost:8080/AranDbService/test");

                // Create a ServiceHost for the CalculatorService type and provide the base address.
                ServiceHost serviceHost = new ServiceHost (typeof (DbService), baseAddress);

                serviceHost.AddServiceEndpoint (typeof (IDbService), binding, address);

                _host = serviceHost;





                //Uri baseAddress = new Uri ("http://localhost:8080/AranDbService");

                //_host = new ServiceHost (typeof (DbService), baseAddress);
                
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior ();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                _host.Description.Behaviors.Add (smb);
            }
            else
            {
                return;
            }

            _host.Open ();

            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
        }

        public void DBService_Responsed (object sender, EventArgs e)
        {
            string text = (string) sender;
            ui_logTextBon.AppendText ("\r\nResponsed [" + DateTime.Now + "] : " + text);

            ui_logTextBon.AppendText ("\r\n*******************************************************************");
            ui_logTextBon.AppendText ("\r\n*******************************************************************");
            ui_logTextBon.AppendText ("\r\n*******************************************************************");
        }

        public void DBService_Requested (object sender, EventArgs e)
        {
            string text = (string) sender;
            ui_logTextBon.AppendText ("\r\nRequested [" + DateTime.Now + "] : " + text);

            ui_logTextBon.AppendText ("\r\n*******************************************************************");
            ui_logTextBon.AppendText ("\r\n*******************************************************************");
            ui_logTextBon.AppendText ("\r\n*******************************************************************");
        }

        private void Stop ()
        {
            if (_host != null)
            {
                _host.Close ();

                stopToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;

                _host = null;
            }
        }

    }
}
