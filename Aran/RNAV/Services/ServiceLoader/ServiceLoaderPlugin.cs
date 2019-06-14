using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;
using ARAN.Contracts.Registry;
using System.Runtime.InteropServices;

namespace ServiceLoader
{
    public class ServiceLoaderPlugin : IAranPlugin
    {
        public ServiceLoaderPlugin ()
        {
            _testMapEvnHandle = -1;
            _approachHandle = -1;
            _departureHandle = -1;
            _uiService = new UIService ();
        }

        public void Startup (IAranEnvironment aranEnv)
        {
            Globals.Env = aranEnv;

            Registry_Contract.RegisterClass ("UIService", 0, _uiService.UIServiceEntyPoint);

            ToolStripMenuItem tsmi = new ToolStripMenuItem ();
            tsmi.Text = "TestMapEnv";
            tsmi.Click += new EventHandler (TestMapEnv_Click);
            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, tsmi);

            ToolStripMenuItem tsmiApproach = new ToolStripMenuItem();
            tsmiApproach.Text = "Approach";
            tsmiApproach.Click += new EventHandler(Approch_Click);
            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, tsmiApproach);

            ToolStripMenuItem tsmiDeparture = new ToolStripMenuItem();
            tsmiDeparture.Text = "Departure";
            tsmiDeparture.Click += new EventHandler(Departure_Click);
            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, tsmiDeparture);

            //  Registry_Contract.GetInstance("Constants");
        }

        private void Departure_Click(object sender, EventArgs e)
        {
            if (_departureHandle == -1)
                _departureHandle = Registry_Contract.GetInstance("Departure");
            else
            {
                Registry_Contract.BeginMessage(_departureHandle, Registry_Contract.svcGetInstance);
                Registry_Contract.EndMessage(_departureHandle);
            }
        }
        
        private void Approch_Click(object sender, EventArgs e)
        {
            if (_approachHandle == -1)
                _approachHandle = Registry_Contract.GetInstance("Approach");
            else
            {
                Registry_Contract.BeginMessage(_approachHandle, Registry_Contract.svcGetInstance);
                Registry_Contract.EndMessage(_approachHandle);
            }
        }

        private void TestMapEnv_Click (object sender, EventArgs e)
        {
            if (_testMapEvnHandle == -1)
                _testMapEvnHandle = Registry_Contract.GetInstance ("TestMapEnv");

            Registry_Contract.BeginMessage (_testMapEvnHandle, Registry_Contract.svcGetInstance);
            Registry_Contract.EndMessage (_testMapEvnHandle);
        }

        private int _testMapEvnHandle,_approachHandle,_departureHandle;
        private UIService _uiService;


        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public void AddChildSubMenu(List<string> hierarcy)
        {
            throw new NotImplementedException();
        }
    }
}
