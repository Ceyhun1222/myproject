using AIP.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace AIP.GUI.Forms
{
    public partial class ChartNumber : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;
        private BindingList<DB.ChartNumber> query;

        public ChartNumber()
        {
            InitializeComponent();
        }

        private void LanguageManager_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {
            try
            {
                db?.ChartNumber?.Load();
                query = db?.ChartNumber?.Local?.ToBindingList();
                fm_rgv.DataSource = query;
                fm_rgv.Refresh();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
        }

        private void fm_rgv_CellValidating(object sender, CellValidatingEventArgs e)
        {
            try
            {
                if (e.Row != null)
                {
                    e.Row.ErrorText = string.Empty;
                    RadTextBoxEditor tbEditor = e.ActiveEditor as RadTextBoxEditor;
                    if (tbEditor != null && e.Column.Name == "Name" &&
                        e.Row.Tag == null)
                    {
                        string cellValue = tbEditor.Value + "";
                        // check for empty
                        if (cellValue == string.Empty)
                        {
                            e.Row.ErrorText = "Empty value is not allowed!";
                            e.Cancel = true;
                        }
                        // check for duplicates
                        else if (query?.Any(x => x.Name.ToLowerInvariant() == cellValue.ToLowerInvariant()) == true)
                        {
                            bool? ff = query?.Any(x => x.Name.ToLowerInvariant() == cellValue.ToLowerInvariant());
                            e.Row.ErrorText = "Duplicate values are not allowed!";
                            e.Cancel = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }

}
