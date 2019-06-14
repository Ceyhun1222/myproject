using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using AIP.DataSet.Properties;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.CommonUtil.Util;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace AIP.DataSet
{
    public partial class Settings : Telerik.WinControls.UI.RadForm
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void DataSetManager_Load(object sender, EventArgs e)
        {
            try
            {
                chkSuppressErrors.Checked = Properties.Settings.Default.SuppressErrors;
                chkUseLinks.Checked = Properties.Settings.Default.UseLinks;
                tbxOrganization.Text = Properties.Settings.Default.Organization;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SuppressErrors = chkSuppressErrors.Checked;
            Properties.Settings.Default.UseLinks = chkUseLinks.Checked;
            Properties.Settings.Default.Organization = tbxOrganization.Text;
            Properties.Settings.Default.Save();
        }
    }
}

