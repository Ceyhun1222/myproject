using AIP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq.Expressions;
using System.Linq;
using AIP.BaseLib.Airac;
using Aran.Temporality.CommonUtil.Context;

namespace AIP.GUI.Forms
{
    public partial class eAISFrm : Telerik.WinControls.UI.RadForm
    {
        private int? ObjectId = null;
        eAIPContext db = new eAIPContext();
        internal DB.eAISpackage eais = new DB.eAISpackage();
        Dictionary<string, object> ComplPropList = new Dictionary<string, object>();
        internal bool isNewDocument = true;
        private int correctiveLine = 0;
        private int lineHeight = 28;
        private int numberOfPublishDays = -42;

        public eAISFrm()
        {
            InitializeComponent();
            DesignForm();
        }
        public eAISFrm(eAISpackage t) : this()
        {
            try
            {
                ObjectId = -1;
                DataEntrySort<eAISpackage> dataEntrySort = new DataEntrySort<eAISpackage> { t };
                this.radDataEntry1.DataSource = dataEntrySort;
                this.Name = t.Effectivedate.ToString();
                this.Name += ", Status: " + t.Status;
                isNewDocument = !(t.id >= 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                    ex.Message);
            }


        }
        private void DesignForm()
        {
            this.radDataEntry1.ShowValidationPanel = true;
            this.radDataEntry1.ItemDefaultSize = new Size(520, lineHeight);
            this.radDataEntry1.ItemSpace = 8;
        }



        private void UniFrm_Load(object sender, EventArgs e)
        {
            //if (ObjectId == null)
            //{
            //    this.radDataEntry1.DataSource = eais;
            //}
            if (radDataEntry1.DataSource is eAISpackage)
            {
                btn_Status.Visible = ((eAISpackage) radDataEntry1.DataSource).Status != Status.Work && Classes.Permissions.Is_Admin();
            }
            
        }

        void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            if (e.DataMember.EndsWith("date"))
            {
                e.Binding.FormattingEnabled = true;
                e.Binding.Format += Binding_Format;
            }
            else if (e.DataMember.EndsWith("Date"))
            {
                e.Binding.FormattingEnabled = true;
                e.Binding.Format += Binding_Parse;
            }
        }


        void Binding_Parse(object sender, ConvertEventArgs e)
        {
            string dt = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            e.Value = dt;
        }

        void radDataEntry1_ItemInitialized(object sender, Telerik.WinControls.UI.ItemInitializedEventArgs e)
        {
            e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
            if (e.Panel.Controls[0].Tag is DataEntryOption)
            {
                if(!((DataEntryOption)e.Panel.Controls[0].Tag).Visible)
                {
                    foreach (Control ctrl in e.Panel.Controls)
                    {
                        ctrl.Visible = false;
                    }
                    correctiveLine++;
                }
                e.Panel.Enabled = !((DataEntryOption)e.Panel.Controls[0].Tag).ReadOnly;
            }
            e.Panel.Location = new Point(e.Panel.Location.X, e.Panel.Location.Y - lineHeight * correctiveLine);
            //if (isNewDocument && e.Panel.Controls[0].Name == "Lang" && e.Panel.Controls[0] is RadDropDownList && ((RadDropDownList)e.Panel.Controls[0]).Items.Count > 0)
            //{
            //    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 1;
            //    ((RadDropDownList)e.Panel.Controls[0]).SelectedIndex = 0;
            //}
            //if (e.Panel.Controls[0].Name.Contains("AiracCycleDateTime") &&
            //    e.Panel.Controls[0] is AiracCycleDateTime)
            //{
            //    AiracCycleDateTime control = e.Panel.Controls[0] as AiracCycleDateTime;
            //    //DateTime airacDate = BaseLib.Airac.AiracCycle.AiracCycleList?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))?.Airac ?? DateTime.UtcNow;
            //    //control.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            //    //control.DTValue = airacDate.ToString("yyyy - MM - dd");// "09.11.2017";
            //    control.SelectionMode = (BaseLib.Airac.AiracCycle.AiracCycleList?.Any(d => d.Airac == control.Value) == true) ? BaseLib.Airac.AiracSelectionMode.Airac : BaseLib.Airac.AiracSelectionMode.Custom;
            //    //control.Value = airacDate;
            //    //control.Dock = DockStyle.Fill;
            //}
        }

        void radDataEntry1_EditorInitializing(object sender, Telerik.WinControls.UI.EditorInitializingEventArgs e)
        {
            try
            {
                DataEntryOption deo = e.Property?.Attributes.OfType<DataEntryOption>().FirstOrDefault();
                e.Editor.Tag = deo;
                if (isNewDocument && e.Property?.Name.EndsWith("date") == true)
                {
                    BaseLib.Airac.AiracCycleDateTime control = new BaseLib.Airac.AiracCycleDateTime();
                    control.Tag = deo;
                    DateTime? ed = BaseLib.Airac.AiracCycle.AiracCycleList
                        ?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(84))?.Airac;
                    DateTime airacDate = 
                        e.Property?.Name.ToLowerInvariant().StartsWith("effective") == true ? 
                        ed ?? DateTime.UtcNow : 
                        ed?.AddDays(numberOfPublishDays) ?? DateTime.UtcNow;
                    control.DateTimeFormat = "yyyy - MM - dd  HH:mm";
                    control.DTValue = airacDate.ToString("yyyy - MM - dd");// "09.11.2017";
                    control.SelectionMode = (BaseLib.Airac.AiracCycle.AiracCycleList?.Any(d => d.Airac == airacDate) == true) ? BaseLib.Airac.AiracSelectionMode.Airac : BaseLib.Airac.AiracSelectionMode.Custom;
                    control.Value = airacDate;
                    control.Dock = DockStyle.Fill;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property?.Name.EndsWith("Date") == true)
                {
                    TextBox control = new TextBox();
                    control.Tag = deo;
                    control.Dock = DockStyle.Fill;
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                else if (e.Property?.Name.EndsWith("lang") == true)
                {
                    ComboBox control = new ComboBox();
                    e.Editor = control;
                    control.Tag = deo;
                    e.Editor.Width = 300;
                    List<String> SysLangs = Lib.DBOptions?.LanguageReferences?.Select(n => n.Value).ToList();
                    if (SysLangs != null)
                    {
                        CultureInfo[] lng = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(n => SysLangs.Contains(n.Name)).ToArray();
                        //String[] lng = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(n => SysLangs.Contains(n.Name)).ToArray();
                        control.DataSource = lng;
                        control.DisplayMember = "Name";
                        control.ValueMember = "Name";
                        control.DropDownStyle = ComboBoxStyle.DropDownList;
                        //control.Font = new System.Drawing.Font(control.Font.Name, 12, FontStyle.Regular);
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                    ex.Message);
            }

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            ((RadButton)sender).Enabled = false;
            eais = ((DataEntrySort<eAISpackage>)radDataEntry1.DataSource).FirstOrDefault();// (DB.eAISpackage)radDataEntry1.DataSource;
            //db.SaveChanges();
            if (eais?.Publicationdate == null && eais != null) eais.Publicationdate = eais.Effectivedate.AddDays(numberOfPublishDays);
            ((RadButton)sender).Enabled = true;
            DialogResult = DialogResult.OK;

        }

        private void radDataEntry1_BindingCreating(object sender, BindingCreatingEventArgs e)
        {
            if (e.Control is RadDropDownList || e.Control is ComboBox)
            {
                e.PropertyName = "SelectedValue";
            }
            else if (e.Control is AiracCycleDateTime)
            {
                e.PropertyName = "Value";
            }
            else if (e.Control is TextBox)
            {
                e.PropertyName = "Text";
            }
        }


        void Binding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value != null && e.Value.Equals(DBNull.Value))
            {
                e.Value = null;
            }
        }

        private void btn_Status_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    $@"Are you sure you want to change current eAIP package status to Work?",
                    @"Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DialogResult = DialogResult.OK;
                ((RadButton) sender).Enabled = false;
                if ((radDataEntry1.DataSource as DataEntrySort<eAISpackage>)?.FirstOrDefault() != null)
                {
                        ((DataEntrySort<eAISpackage>)radDataEntry1.DataSource).FirstOrDefault().Status = Status.Work;
                }
                eais = ((DataEntrySort<eAISpackage>)radDataEntry1.DataSource).FirstOrDefault();
                ((RadButton) sender).Enabled = true;
            }
        }
    }
}
