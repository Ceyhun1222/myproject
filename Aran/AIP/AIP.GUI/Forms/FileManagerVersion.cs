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
    public partial class FileManagerVersion : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;
        public Guid Identifier;

        public FileManagerVersion()
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
                List<AIPFile> query = db.AIPFile
                        //.AsNoTracking()
                        .Where(x => x.Identifier == Identifier)
                        .OrderBy(x=>x.Version)
                        .ToList();
                Dictionary<string, GridViewOption> fileProp = Lib.GridViewAttributes<AIPFile>();
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

        

        private async void SaveClick(object sender, EventArgs e)
        {
            try
            {
                await db.SaveChangesAsync();
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
                if (fm_rgv.CurrentRow == null || fm_rgv.CurrentRow.DataBoundItem == null)
                    return;
                if (sender is GridDataCellElement && ((GridDataCellElement)sender).ColumnInfo.Name == "Order")
                {
                    return;
                }
                if (fm_rgv.CurrentRow.DataBoundItem is AIPFile)
                {
                    int id = ((AIPFile)fm_rgv.CurrentRow.DataBoundItem).id;
                    AIPFile file = db.AIPFile.Where(x=>x.id == id)
                        .Include(x=>x.AIPFileData.AIPFileDataHash)
                        .FirstOrDefault();
                    FileManagerForm frm = new FileManagerForm(file);
                    frm.db = db;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        //AddAIPFile(frm.formSource, frm.isNewDocument);
                        if (file?.IsCanceled == true)
                        {
                            db.Entry(file).State = EntityState.Modified;
                            db.SaveChanges();
                            return;
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
