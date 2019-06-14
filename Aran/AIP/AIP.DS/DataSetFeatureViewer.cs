using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class DataSetFeatureViewer : Telerik.WinControls.UI.RadForm
    {
        public Feature feature;

        private int correctiveLine = 0;
        private int lineHeight = 25;

        public DataSetFeatureViewer()
        {
            InitializeComponent();
            DesignForm();
        }

        private void DataSetFeatureViewer_Load(object sender, EventArgs e)
        {
            radDataEntry1.DataSource = feature;
        }

        private void DesignForm()
        {
            try
            {
                this.radDataEntry1.ShowValidationPanel = true;
                this.radDataEntry1.ItemDefaultSize = new Size(520, lineHeight);
                this.radDataEntry1.ItemSpace = 8;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radDataEntry1_ItemInitialized(object sender, ItemInitializedEventArgs e)
        {
            try
            {
                e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
                e.Panel.Enabled = false;
                
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            try
            {
                if (e.DataMember.Contains("Slice"))
                {
                    e.Binding.FormattingEnabled = true;
                    e.Binding.Format += Collection_Format;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);

            }
        }

        private void Collection_Format(object sender, ConvertEventArgs e)
        {
            try
            {
                e.Value = null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                //return null;
            }
        }
    }
}

