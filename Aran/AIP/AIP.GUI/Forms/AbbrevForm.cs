using AIP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using AIP.BaseLib;
using AIP.BaseLib.Airac;
using AIP.GUI.Classes;
using Telerik.WinControls.RichTextEditor.UI;
using Telerik.WinForms.Documents.RichTextBoxCommands;
using Telerik.WinForms.RichTextEditor;
using Telerik.WinForms.RichTextEditor.RichTextBoxUI.Menus;
using DataEntryOption = AIP.DB.DataEntryOption;
using Telerik.WinForms.Documents.FormatProviders.Html;
using Telerik.WinForms.Documents.Layout;
using Telerik.WinForms.Documents.Model;
using XHTML_WPF;
using FontStyle = System.Drawing.FontStyle;
using InsertSymbolDialog = Telerik.WinForms.RichTextEditor.RichTextBoxUI.Dialogs.InsertSymbolDialog;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace AIP.GUI.Forms
{
    public partial class AbbrevForm : Telerik.WinControls.UI.RadForm
    {
        private int? ObjectId = null;
        public eAIPContext db;
        internal DB.Abbreviation formSource = new DB.Abbreviation();
        internal bool isNewDocument = true;
        private int correctiveLine = -1;
        private int lineHeight = 25;
        private bool RT_loaded = false;
        private string xhtmlValue = "";
        private RadRichTextEditor Editor;
        private FontFamily fnt;

        public AbbrevForm()
        {
            InitializeComponent();
            DesignForm();
        }
        public AbbrevForm(Abbreviation t) : this()
        {
            try
            {
                formSource = t;
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
                this.radDataEntry1.DataSource = formSource;
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
                if (e.DataMember.Contains("date") || e.DataMember.ToLowerInvariant().Contains("language"))
                {
                    e.Binding.FormattingEnabled = true;
                    e.Binding.Format += Binding_Format;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);

            }
        }

        void radDataEntry1_ItemInitialized(object sender, Telerik.WinControls.UI.ItemInitializedEventArgs e)
        {
            try
            {
                e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
                e.Panel.Location = new Point(e.Panel.Location.X, e.Panel.Location.Y - lineHeight * correctiveLine);
                if (e.Panel.Controls[0].Tag is DataEntryOption)
                {
                    e.Panel.Visible = ((DataEntryOption)e.Panel.Controls[0].Tag).Visible;
                    if (!e.Panel.Visible) correctiveLine++;
                    e.Panel.Enabled = !((DataEntryOption)e.Panel.Controls[0].Tag).ReadOnly;
                    int units = ((DataEntryOption)e.Panel.Controls[0].Tag).RowSpan;
                    if (e.Panel.Visible && units > 1)
                    {
                        e.Panel.Size = new Size(520, lineHeight * units);
                        correctiveLine = correctiveLine - units + 1;
                    }
                }
                if (isNewDocument && e.Panel.Controls[0].Name == "Lang" && e.Panel.Controls[0] is RadDropDownList && ((RadDropDownList)e.Panel.Controls[0]).Items.Count > 0)
                {
                    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 1;
                    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 0;
                }
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
                else if (e.Property?.Name.Contains("Lang") == true)
                {
                    RadDropDownList control = new RadDropDownList();
                    control.Name = "Lang";
                    control.DisplayMember = "Value";
                    control.ValueMember = "Key";
                    control.DropDownStyle = RadDropDownStyle.DropDownList;
                    db?.LanguageReference?.Load();
                    List<KeyValuePair<int?, string>> cnList = db?.LanguageReference?.Local?.ToBindingList()?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                    cnList?.Insert(0, new KeyValuePair<int?, string>(null, " "));
                    control.DataSource = cnList;

                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property?.Name.Contains("ChartNumberId") == true)
                {
                    RadDropDownList control = new RadDropDownList();
                    control.Name = "ChartNumber";
                    control.DisplayMember = "Value";
                    control.ValueMember = "Key";
                    control.DropDownStyle = RadDropDownStyle.DropDownList;
                    db?.ChartNumber?.Load();
                    List<KeyValuePair<int?, string>> cnList = db?.ChartNumber?.Local?.ToBindingList()?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                    cnList?.Insert(0, new KeyValuePair<int?, string>(null, " "));
                    control.DataSource = cnList;
                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property?.Name.Contains("Details") == true)
                {
                    //RadRichTextEditor control = InitRichTextEditor();
                    //XHTML_WPF.View.XhtmlViewer control = new XHTML_WPF.View.XhtmlViewer();
                    BaseLib.XHtmlEditor control = new BaseLib.XHtmlEditor();
                    control.Name = "Details";
                    control.Tag = deo;
                    e.Editor = control;
                    e.Editor.Width = 500;
                    //Editor = control;
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
                    e.PropertyName = "SelectedValue";
                }
                else if (e.Control is AiracCycleDateTime)
                {
                    e.PropertyName = "Value";
                }
                else if (e.Control is RadRichTextEditor)
                {
                    e.PropertyName = "Document";
                }
                else if (e.Control is XHtmlEditor)
                {
                    e.PropertyName = "Value";
                }
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
            }
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                formSource = (DB.Abbreviation)radDataEntry1.DataSource;
                DialogResult = DialogResult.OK;
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
    }
}
