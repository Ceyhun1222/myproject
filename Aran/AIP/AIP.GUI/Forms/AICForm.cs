using AIP.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq;
using System.Windows.Forms.Integration;
using AIP.BaseLib;
using AIP.BaseLib.Airac;
using Telerik.WinControls.RichTextEditor.UI;
using DataEntryOption = AIP.DB.DataEntryOption;
using XHTML_WPF;
using XHTML_WPF.ViewModel;
using FontStyle = System.Drawing.FontStyle;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace AIP.GUI.Forms
{
    public partial class AICForm : Telerik.WinControls.UI.RadForm
    {
        private int? ObjectId = null;
        public eAIPContext db;
        internal Circular formSource = new Circular();
        internal bool isNewDocument = true;
        private int correctiveLine = -1;
        private int lineHeight = 25;
        private bool RT_loaded = false;
        private string xhtmlValue = "";
        private RadRichTextEditor Editor;
        private FontFamily fnt;
         
        public AICForm()
        {
            InitializeComponent();
            DesignForm();
            // 
        }
        public AICForm(Circular t) : this()
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
                DataEntrySort<Circular> dataEntrySort = new DataEntrySort<Circular> { formSource };
                this.radDataEntry1.DataSource = dataEntrySort;
                //this.radDataEntry1.DataSource = formSource;
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
                if (e.DataMember.ToLowerInvariant().Contains("date") || e.DataMember.ToLowerInvariant().Contains("language"))
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
                if (e.Panel.Controls[0] is AiracCycleDateTime &&
                    ((AiracCycleDateTime)e.Panel.Controls[0]).Value == DateTime.MinValue)
                {
                    BaseLib.Airac.AiracCycleDateTime control = e.Panel.Controls[0] as BaseLib.Airac.AiracCycleDateTime;
                    DateTime airacDate = BaseLib.Airac.AiracCycle.AiracCycleList?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))?.Airac ?? DateTime.UtcNow;
                    control.DateTimeFormat = "yyyy - MM - dd  HH:mm";
                    control.DTValue = airacDate.ToString("yyyy - MM - dd");// "09.11.2017";
                    control.SelectionMode = (BaseLib.Airac.AiracCycle.AiracCycleList?.Any(d => d.Airac == airacDate) == true) ? BaseLib.Airac.AiracSelectionMode.Airac : BaseLib.Airac.AiracSelectionMode.Custom;
                    control.Value = airacDate;
                    control.Dock = DockStyle.Fill;
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
                if (isNewDocument && e.Property?.Name.ToLowerInvariant().Contains("date") == true)
                {
                    //BaseLib.Airac.AiracCycleDateTime control = new AiracCycleDateTime
                    //{
                    //    Tag = deo
                    //};
                    //e.Editor = control;
                    //e.Editor.Width = 300;

                    BaseLib.Airac.AiracCycleDateTime control = new BaseLib.Airac.AiracCycleDateTime();
                    control.Tag = deo;
                    DateTime? ed = BaseLib.Airac.AiracCycle.AiracCycleList
                        ?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))?.Airac;
                    DateTime airacDate =
                        e.Property?.Name.ToLowerInvariant().StartsWith("effective") == true ?
                            ed ?? DateTime.UtcNow :
                            ed?.AddDays(-42) ?? DateTime.UtcNow;
                    control.DateTimeFormat = "yyyy - MM - dd  HH:mm";
                    control.DTValue = airacDate.ToString("yyyy - MM - dd");// "09.11.2017";
                    control.SelectionMode = (BaseLib.Airac.AiracCycle.AiracCycleList?.Any(d => d.Airac == airacDate) == true) ? BaseLib.Airac.AiracSelectionMode.Airac : BaseLib.Airac.AiracSelectionMode.Custom;
                    control.Value = airacDate;
                    control.Dock = DockStyle.Fill;
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
                //else if (e.Property?.Name.Contains("Description") == true)
                //{
                //    //RadRichTextEditor control = InitRichTextEditor();
                //    //XHTML_WPF.View.XhtmlViewer control = new XHTML_WPF.View.XhtmlViewer();
                //    WebBrowser control = new WebBrowser();
                //    control.TextChanged += (o, args) =>
                //    {
                        
                //    };
                //    control.Name = "Description";
                //    control.Tag = deo;
                //    e.Editor = control;
                //    e.Editor.Width = 500;
                //    e.Editor.Height = 150;
                //    //Editor = control;
                //}
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
                formSource = ((DataEntrySort<Circular>)radDataEntry1.DataSource)?.FirstOrDefault();
                if (formSource?.Publicationdate > formSource?.EffectivedateFrom ||
                    formSource?.Publicationdate > formSource?.EffectivedateTo)
                {
                    ErrorLog.ShowWarning(
                        $@"Publication date can`t be later than Effective date. Please fix it and try again.");
                    return;
                }
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
        
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                Lib.LoadXhtmlControlResources();
                XhtmlEditor richTextDialog = new XhtmlEditor();
                var viewModel = richTextDialog.DataContext as XhtmlEditorViewModel;
                if (viewModel == null) return;
                viewModel.Editable = true;
                richTextDialog.DataContext = viewModel;
                viewModel.HtmlText = formSource.Description;
                ElementHost.EnableModelessKeyboardInterop(richTextDialog);
                richTextDialog.ShowDialog();
                if (viewModel.IsSaved)
                {
                    formSource.Description = viewModel.HtmlText;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
    }
}
