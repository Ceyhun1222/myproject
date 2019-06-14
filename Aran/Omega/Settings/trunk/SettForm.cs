using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.AranEnvironment;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using System.Threading;

namespace Aran.Omega.SettingsUI
{
    public partial class SettingsForm : UserControl, ISettingsPage
    {
        #region Declaration

      
        #endregion

        public SettingsForm ()
        {
            InitializeComponent ();
        }

        public string Title
        {
            get { return "Omega"; }
        }

        public Control Page
        {
            get { return this; }
        }

        public void OnLoad ()
        {
            Globals.Settings = new OmegaSettings();
            Globals.Settings.Load(SettingsGlobals.AranEnvironment);
            SettingsControl control = elementHost2.Child as SettingsControl;
            control.LoadAll();
        }

        public bool OnSave ()
        {
            SaveSettings ();
            return true;
        }

        private void SaveSettings ()
        {
            Globals.Settings.Store(SettingsGlobals.AranEnvironment);
        }
    }
}
