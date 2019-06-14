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
    public partial class AbbrevVersion : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;
        public Guid Identifier;

        public AbbrevVersion()
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
                List<Abbreviation> query = db.Abbreviation
                        //.AsNoTracking()
                        .Where(x => x.Identifier == Identifier)
                        .OrderBy(x=>x.Version)
                        .ToList();
                Dictionary<string, GridViewOption> fileProp = Lib.GridViewAttributes<Abbreviation>();
                BindingSource bi = new BindingSource();
                bi.DataSource = query;
                fm_rgv.DataSource = query;
                foreach (GridViewDataColumn col in fm_rgv.Columns)
                {
                    if (fileProp.ContainsKey(col.Name))
                    {
                        col.IsVisible = fileProp[col.Name].Visible;
                        col.ReadOnly = fileProp[col.Name].ReadOnly;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                fm_rgv.Refresh();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
        private void fm_rgv_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (fm_rgv.CurrentRow?.DataBoundItem == null)return;
                if (fm_rgv.CurrentRow.DataBoundItem is Abbreviation)
                {
                    int id = ((Abbreviation)fm_rgv.CurrentRow.DataBoundItem).id;
                    Abbreviation abbrev = db.Abbreviation.FirstOrDefault(x => x.id == id);
                    AbbrevForm frm = new AbbrevForm(abbrev);
                    frm.db = db;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        //AddAbbreviation(frm.formSource, frm.isNewDocument);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void fm_rgv_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            try
            {
                Dictionary<string, GridViewOption> fileProp = Lib.GridViewAttributes<Abbreviation>();
                foreach (GridViewDataColumn col in fm_rgv.Columns)
                {
                    
                    if (fileProp.ContainsKey(col.Name))
                    {
                        col.IsVisible = fileProp[col.Name].Visible;
                        col.ReadOnly = fileProp[col.Name].ReadOnly;
                        col.DisableHTMLRendering = !fileProp[col.Name].RenderHTML;
                        if (fileProp[col.Name].MaxWidth > 0) col.MaxWidth = fileProp[col.Name].MaxWidth;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                GridViewDataColumn dataColumn = e.CellElement.ColumnInfo as GridViewDataColumn;
                if (dataColumn != null && dataColumn.DisableHTMLRendering == false)
                {
                    e.CellElement.Text = $@"<html>{e.CellElement.Value}</html>";
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }

}
