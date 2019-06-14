using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;

namespace Aran.Delta.Settings
{
    public partial class SettingsPage : UserControl, ISettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return "Delta"; }
        }

        public Control Page
        {
            get { return this; }
        }

        public void OnLoad()
        {
            Globals.Settings = new DeltaSettings();
            Globals.Settings.Load(SettingsGlobals.AranEnvironment);
            SettingsForm control = elementHost1.Child as SettingsForm;
            control.LoadAll();
        }

        private void SaveSettings()
        {
            Globals.Settings.Store(SettingsGlobals.AranEnvironment);
        }

        public bool OnSave()
        {
            SaveSettings();
            return true;
        }
    }
}
