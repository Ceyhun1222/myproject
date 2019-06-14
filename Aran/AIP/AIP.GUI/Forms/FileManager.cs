using AIP.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using AIP.BaseLib;
using EntityFramework.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace AIP.GUI.Forms
{
    public partial class FileManager : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;
        private bool FormLoaded = false;

        public FileManager()
        {
            InitializeComponent();
        }

        private void LanguageManager_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            FillCbx();
            UpdateDataGridView();
            FormLoaded = true;
        }

        private void FillCbx()
        {
            try
            {
                cbx_Section.DataSource = Enum.GetValues(typeof(SectionName));
                cbx_Section.SelectedIndex = -1;
                
                cbx_Lang.DisplayMember = "Value";
                cbx_Lang.ValueMember = "Key";
                //cbx_Lang.DropDownStyle = RadDropDownStyle.DropDownList;
                //cbx_Lang.DataSource = db.LanguageReference.AsEnumerable().Select(x => new KeyValuePair<string, string>(x.id.ToString(), x.Name)).ToList();

                db?.LanguageReference?.Load();
                List<KeyValuePair<int?, string>> cnList = db?.LanguageReference?.Local?.ToBindingList()?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                cnList?.Insert(0, new KeyValuePair<int?, string>(null, " "));
                cbx_Lang.DataSource = cnList;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void FillGridView()
        {
            try
            {
                UpdateDataGridView();

                //lm_rgv.GroupDescriptors.Add(new GridGroupByExpression("LanguageCategory as LanguageCategory format \"{0}: {1}\" Group By LanguageCategory"));
                //lm_rgv.MasterTemplate.ExpandAllGroups();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void UpdateDataGridView()
        {
            try
            {
                SectionName? sectionName = null;
                if (cbx_Section.SelectedValue is SectionName) sectionName = (SectionName)cbx_Section.SelectedValue;
                int? lang = int.TryParse(cbx_Lang?.SelectedValue?.ToString(), out int temp) ? temp : default(int?);

                List<AIPFile> query;

                //Expression<Func<AIPFile, bool>> selectSection = _ => true;
                //if (sectionName != null) selectSection = x => x.SectionName == sectionName;

                //Expression<Func<AIPFile, bool>> selectLang = _ => true;
                //if (lang != null) selectLang = x => x.LanguageReferenceId == lang;

                Expression<Func<AIPFile, bool>> selectQuery = _ => true;
                if (lang != null) selectQuery = x => x.LanguageReferenceId == lang;
                if (sectionName != null) selectQuery = selectQuery.And(x => x.SectionName == sectionName);

                query = db.AIPFile
                    .Where(x => x.IsCanceled == false)?
                    //.Include(x=>x.User)
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .Where(selectQuery)
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


        private void cbx_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //FillGridView((SectionName)cbx_Section.SelectedItem);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            try
            {
                FileManagerForm frm = new FileManagerForm();
                frm.db = db;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AddAIPFile(frm.formSource, frm.isNewDocument);
                    UpdateDataGridView();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void AddAIPFile(AIPFile file, bool isNew)
        {
            try
            {
                //db.Database.Log = Console.Write;
                string path = file.FileName;
                bool? newAttachmentAdded = path?.Contains(":"); // C:\blabla
                AIPFileData AIPFileData = new AIPFileData();
                AIPFileDataHash AIPFileDataHash = new AIPFileDataHash();

                if (isNew)
                {
                    file.Identifier = Guid.NewGuid();
                    if (!File.Exists(path))
                    {
                        ErrorLog.ShowMessage("No file selected");
                        return;
                    }
                    
                    AIPFileData.AIPFile.Add(file);
                    AIPFileData.Data = File.ReadAllBytes(path);
                    AIPFileDataHash.Hash = Lib.SHA1(path);
                    file.FileName = Path.GetFileName(path);
                    AIPFileDataHash.AIPFileData = AIPFileData;

                    db.AIPFileData.Add(AIPFileData);
                    db.AIPFileDataHash.Add(AIPFileDataHash);
                    db.AIPFile.Add(file);

                }
                else
                {
                    if (file.IsCanceled == true)
                    {
                        db.Entry(file).State = EntityState.Modified;
                        db.SaveChanges();
                        return;
                    }
                    if (newAttachmentAdded == true && !File.Exists(path)) // C:\...
                    {
                        ErrorLog.ShowMessage("No file exist");
                        return;
                    }
                    // Require to compute next version
                    // Current version number may be not last, if last one is Canceled
                    int? ver = db.AIPFile?
                        .AsNoTracking()
                        .Where(x => x.Identifier == file.Identifier)?
                        .OrderByDescending(x => x.Version)
                        .FirstOrDefault()
                        ?.Version;
                    file.Version = ver+1 ?? 1; 

                    if (newAttachmentAdded == true)
                    {
                        AIPFileData.AIPFile.Add(file);
                        AIPFileData.Data = File.ReadAllBytes(path);
                        AIPFileDataHash.Hash = Lib.SHA1(path);
                        file.FileName = Path.GetFileName(path);

                        db.Entry(file).State = EntityState.Added;
                        db.Entry(AIPFileData).State = EntityState.Added;
                        db.Entry(AIPFileDataHash).State = EntityState.Added;

                        db.AIPFileData.Add(AIPFileData);
                        db.AIPFileDataHash.Add(AIPFileDataHash);
                        db.AIPFile.Add(file);
                    }
                    else
                    {
                        db.Entry(file).State = EntityState.Modified;
                        db.AIPFile.Add(file);
                    }
                    
                }
                file.Created = Lib.GetServerDate() ?? DateTime.UtcNow;
                file.UserId = Globals.CurrentUser.id;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Filter_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateDataGridView();
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
                    var selectedItem = fm_rgv.CurrentRow;
                    int id = ((AIPFile)fm_rgv.CurrentRow.DataBoundItem).id;
                    AIPFile file = db.AIPFile.Where(x=>x.id == id)
                        .Include(x=>x.AIPFileData.AIPFileDataHash)
                        .Include(x => x.ChartNumber)
                        .FirstOrDefault();
                    FileManagerForm frm = new FileManagerForm(file);
                    //FileManagerForm frm = new FileManagerForm(id);
                    frm.db = db;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        AddAIPFile(frm.formSource, frm.isNewDocument);
                        UpdateDataGridView();
                    }
                    //else
                    //{
                    //    //db.Entry(file).Reload();
                    //    db.AIPFile.GetContext().Refresh(RefreshMode.StoreWins, file);
                    //    fm_rgv.Refresh();
                    //}
                }
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

        private void btn_History_Click(object sender, EventArgs e)
        {
            try
            {
                if (fm_rgv.CurrentRow == null || fm_rgv.CurrentRow.DataBoundItem == null)
                    return;
                if (fm_rgv.CurrentRow.DataBoundItem is AIPFile)
                {
                    FileManagerVersion frm = new FileManagerVersion();
                    frm.db = db;
                    frm.Identifier = ((AIPFile)fm_rgv.CurrentRow.DataBoundItem).Identifier;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_ChartNumbers_Click(object sender, EventArgs e)
        {
            try
            {
                ChartNumber frm = new ChartNumber();
                frm.db = db;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Filter_Click(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if(FormLoaded) UpdateDataGridView();
        }
    }

}
