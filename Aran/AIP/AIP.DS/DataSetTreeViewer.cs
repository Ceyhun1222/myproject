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
    public partial class DataSetTreeViewer : Telerik.WinControls.UI.RadForm
    {
        public Feature feature;
        

        public DataSetTreeViewer()
        {
            InitializeComponent();
            DesignForm();
        }

        private void DataSetFeatureViewer_Load(object sender, EventArgs e)
        {
            radTreeView1.DataSource = feature;
        }

        private void DesignForm()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
    }
}

