using AIP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq.Expressions;
using System.Linq;
using AIP.BaseLib.Airac;
using AIP.GUI.Classes;
using DataEntryOption = AIP.DB.DataEntryOption;

namespace AIP.GUI.Forms
{
    public partial class FileManagerForm : Telerik.WinControls.UI.RadForm
    {
        private int? ObjectId = null;
        public eAIPContext db;
        internal DB.AIPFile formSource = new DB.AIPFile();
        Dictionary<string, object> ComplPropList = new Dictionary<string, object>();
        internal bool isNewDocument = true;
        private int correctiveLine = 0;
        private int lineHeight = 25;

        public FileManagerForm()
        {
            InitializeComponent();
            DesignForm();
        }
        public FileManagerForm(AIPFile t) : this()
        {
            try
            {
                formSource = t;
                
                //this.Name = t.Effectivedate.ToString();
                //this.Name += ", Status: " + t.Status;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                    ex.Message);
            }


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



        private void UniFrm_Load(object sender, EventArgs e)
        {
            try
            {
                isNewDocument = formSource.id == 0;
                btn_DownloadFile.Visible = !isNewDocument;
                this.radDataEntry1.DataSource = formSource;
                //btn_CancelFile.Visible = Permissions.Is_Admin();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
             }
        }

        void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            try
            {
                if (e.DataMember.Contains("date") || e.DataMember.ToLowerInvariant().Contains("chartnumber") || e.DataMember.ToLowerInvariant().Contains("language"))
                {
                    e.Binding.FormattingEnabled = true;
                    e.Binding.Format += Binding_Format;
                }
                else if (e.DataMember.ToLowerInvariant().Contains("filename"))
                {
                    e.Binding.FormattingEnabled = true;
                    e.Binding.Format += Binding_Format2;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
             
            }
        }

        void Binding_Parse(object sender, ConvertEventArgs e)
        {
            string obj = e.Value as string;
            e.Value = obj;
        }

        void radDataEntry1_ItemInitialized(object sender, Telerik.WinControls.UI.ItemInitializedEventArgs e)
        {
            try
            {
                e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
                if (e.Panel.Controls[0].Tag is DataEntryOption)
                {
                    e.Panel.Visible = ((DataEntryOption) e.Panel.Controls[0].Tag).Visible;
                    if (!e.Panel.Visible) correctiveLine++;
                    e.Panel.Enabled = !((DataEntryOption) e.Panel.Controls[0].Tag).ReadOnly;
                }
                e.Panel.Location = new Point(e.Panel.Location.X, e.Panel.Location.Y - lineHeight * correctiveLine);
                if (isNewDocument && e.Panel.Controls[0].Name == "Lang" && e.Panel.Controls[0] is RadDropDownList && ((RadDropDownList)e.Panel.Controls[0]).Items.Count > 0)
                {
                    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 1;
                    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 0;
                }
                //if (isNewDocument && e.Panel.Controls[0].Name == "ChartNumber" && e.Panel.Controls[0] is RadDropDownList && ((RadDropDownList)e.Panel.Controls[0]).Items.Count > 0)
                //{
                //    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 1;
                //    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        void radDataEntry1_EditorInitializing(object sender, Telerik.WinControls.UI.EditorInitializingEventArgs e)
        {
            try
            {
                DataEntryOption deo = e.Property?.Attributes.OfType<DataEntryOption>().FirstOrDefault();
                e.Editor.Tag = deo;
                if (e.Property?.Name.Contains("date") == true)
                {
                    BaseLib.Airac.AiracCycleDateTime control = new AiracCycleDateTime
                    {
                        Tag = deo
                    };
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property?.Name.Contains("FileName") == true)
                {
                    RadBrowseEditor control = new RadBrowseEditor();
                    control.DialogType = BrowseEditorDialogType.OpenFileDialog;
                    //control.ValueChanging += (o, x) =>
                    //{
                    //    System.Diagnostics.Debugger.Launch();
                    //    x.Cancel = !File.Exists(x.NewValue.ToString());
                    //};
                    //control.ValueChanged += (o, x) =>
                    //{
                    //    System.Diagnostics.Debugger.Launch();
                    //    //if (o is RadBrowseEditorElement && ((RadBrowseEditorElement) o).Value == null)
                    //};
                    control.ReadOnly = true;
                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property.Name.Contains("Lang"))
                {
                    RadDropDownList control = new RadDropDownList();
                    control.Name = "Lang";
                    control.DisplayMember = "Value";
                    control.ValueMember = "Key";
                    control.DropDownStyle = RadDropDownStyle.DropDownList;
                    //List<LanguageReference> l = db.LanguageReference?.AsNoTracking()?.AsEnumerable().ToList();
                    //control.DataSource = l?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();

                    db?.LanguageReference?.Load();
                    List<KeyValuePair<int?, string>> cnList = db?.LanguageReference?.Local?.ToBindingList()?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                    cnList?.Insert(0, new KeyValuePair<int?, string>(null, " "));
                    control.DataSource = cnList;

                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                //else if (e.Property.Name.Contains("ChartNumber"))
                //{
                //    RadDropDownList control = new RadDropDownList();
                //    control.Name = "ChartNumber";
                //    //control.DisplayMember = "Value";
                //    //control.ValueMember = "Key";
                //    control.DisplayMember = "Name";
                //    control.ValueMember = "id";
                //    control.DropDownStyle = RadDropDownStyle.DropDownList;
                //    //List<DB.ChartNumber> cn = db.ChartNumber?.AsNoTracking()?.ToList();
                //    db?.ChartNumber?.Load();
                //    BindingList<DB.ChartNumber> query = db?.ChartNumber?.Local?.ToBindingList();
                //    query?.AddNew();
                //    control.DataSource = query?.OrderBy(x => x.Name);
                //    //List<KeyValuePair<int?, string>> cnList = cn?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                //    //cnList?.Insert(0, new KeyValuePair<int?, string>(null," "));
                //    //control.DataSource = cnList;
                //    control.Tag = deo;
                //    e.Editor = control;
                //    e.Editor.Width = 300;
                //}
                else if (e.Property.Name.Contains("ChartNumberId"))
                {
                    RadDropDownList control = new RadDropDownList();
                    control.Name = "ChartNumber";
                    control.DisplayMember = "Value";
                    control.ValueMember = "Key";
                    control.DropDownStyle = RadDropDownStyle.DropDownList;
                    //List<DB.ChartNumber> cn = db.ChartNumber?.AsNoTracking()?.AsEnumerable()?.ToList();
                    db?.ChartNumber?.Load();
                    //BindingList<DB.ChartNumber> query = db?.ChartNumber?.Local?.ToBindingList();
                    //control.DataSource = query?.OrderBy(x => x.Name);
                    List<KeyValuePair<int?, string>> cnList = db?.ChartNumber?.Local?.ToBindingList()?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                    cnList?.Insert(0, new KeyValuePair<int?, string>(null," "));
                    control.DataSource = cnList;
                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property.Name.Contains("AirportHeliport"))
                {
                    RadDropDownList control = new RadDropDownList();
                    control.Name = "AirportHeliport";
                    control.DropDownStyle = RadDropDownStyle.DropDownList;
                    control.DataSource = Lib.GetAIXMAirportHeliport()?.Select(x => x.LocationIndicatorICAO).ToList();
                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                    ex.Message);
            }
        }

        private void radDataEntry1_BindingCreating(object sender, BindingCreatingEventArgs e)
        {
            try
            {
                if (e.Control is RadDropDownList || e.Control is ComboBox)
                {
                    if (e.Control.Name == "AirportHeliport")
                        e.PropertyName = "Text";
                    else if (e.Control.Name == "Lang" || e.Control.Name == "ChartNumber")
                        e.PropertyName = "SelectedValue";
                    else
                        e.PropertyName = "SelectedValue";
                }
                else if (e.Control is RadBrowseEditor)
                {
                    e.PropertyName = "Value";
                }
                else if (e.Control is AiracCycleDateTime)
                {
                    e.PropertyName = "Value";
                }
                //else if (e.DataMember.ToLowerInvariant().Contains("date"))
                //{
                //    e.PropertyName = "Value";
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
             
            }
        }


        void Binding_Format(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value != null && e.Value.Equals(DBNull.Value))
                {
                    e.Value = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                //return null;
            }
        }

        void Binding_Format2(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null || e.Value.Equals(DBNull.Value))
                {
                    e.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                //return null;
            }
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                formSource = (DB.AIPFile)radDataEntry1.DataSource;
                DialogResult = DialogResult.OK;
                //((RadButton)sender).Enabled = false;
                //db.SaveChanges();
                //((RadButton)sender).Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
        private void btn_DownloadFile_Click(object sender, EventArgs e)
        {
            try
            {
                Lib.SaveAIPFile(formSource, false);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void FileManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // If nothing changed, refreshing data
                if (DialogResult != DialogResult.OK && !isNewDocument) db.Entry(formSource).Reload();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_CancelFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(
                        $@"Are you sure you want to change current File status status to Canceled? This file will not be active anymore.",
                        @"Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var file = (DB.AIPFile) radDataEntry1.DataSource;
                    file.IsCanceled = true;
                    formSource = file;
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }
}
