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

namespace AIP.GUI.Forms
{
    public partial class UniFrm : Telerik.WinControls.UI.RadForm
    {
        private int? ObjectId = null;
        eAIPContext db = new eAIPContext();
        AIPFile obj = new AIPFile();
        Dictionary<string, object> ComplPropList = new Dictionary<string, object>();
        bool FirstRun = true;

        public UniFrm()
        {
            InitializeComponent();
            DesignForm();
        }
        public UniFrm(Type t, int? Id = null) : this()
        {
            try
            {
                if (Id != null)
                {
                    ObjectId = Id;
                    if (t.Name.ToLowerInvariant().Contains("aip"))
                    {
                        this.radDataEntry1.DataSource = db.eAIP.Find(Id);
                        //db.Entry((Dataset.BaseEntity)radDataEntry1.DataSource).State = EntityState.Modified;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public UniFrm(AIPSection t) : this()
        {
            try
            {
                string propType = "";
                foreach (var prop in t.GetType().GetProperties())
                {
                    propType = prop.GetValue(t, null)?.GetType()?.ToString();
                    if (propType != null && propType.ToLowerInvariant().Contains("system.collections.generic.list`1[aip.db"))
                    {
                        ComplPropList.Add(prop.Name, prop.GetValue(t, null));
                    }
                    //Console.WriteLine("{0} / {1} / {2}", prop.Name, prop.GetValue(t, null), prop.GetValue(t, null).GetType());
                }

                ObjectId = -1;
                this.radDataEntry1.DataSource = t;
                PV1.Text = t.SectionName.ToString();
                this.Name = t.SectionName.ToString();
                this.Name += ", ID: " + t.id;


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
                this.radDataEntry1.ItemDefaultSize = new Size(520, 25);
                this.radDataEntry1.ItemSpace = 8;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void UniFrm_Load(object sender, EventArgs e)
        {
            if (ObjectId == null)
            {
                this.radDataEntry1.DataSource = obj;
            }
        }

        void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            if (e.DataMember.ToLowerInvariant().EndsWith("date")
                || e.DataMember.ToLowerInvariant().EndsWith("datefrom")
                || e.DataMember.ToLowerInvariant().EndsWith("dateto"))
            {
                e.Binding.FormattingEnabled = true;
                e.Binding.Format += Binding_Format;
            }
            //else if (e.DataMember.ToLowerInvariant().EndsWith("cation"))
            //{
            //    e.Binding.FormattingEnabled = true;
            //    e.Binding.Format += Binding_Format2;
            //}
        }


        void Binding_Parse(object sender, ConvertEventArgs e)
        {
            string obj = e.Value as string;
            e.Value = obj;
        }

        void radDataEntry1_ItemInitialized(object sender, Telerik.WinControls.UI.ItemInitializedEventArgs e)
        {
            //if (e.Panel.Controls[1].Text == "First Name")
            //{
            //    e.Panel.Size = new Size(300, 25);
            //    e.Panel.Controls[1].Text = "Name";
            //}
            //else if (e.Panel.Controls[1].Text == "Last Name")
            //{
            //    e.Panel.Size = new Size(160, 25);
            //    e.Panel.Controls[1].Visible = false;
            //    e.Panel.Location = new Point(310, 10);
            //}
            //else
            //{
            //    e.Panel.Location = new Point(e.Panel.Location.X, e.Panel.Location.Y);
            //}

            //if (e.Panel.Controls[0] is RadDateTimePicker)
            //{
            //    e.Panel.Controls[0].ForeColor = Color.MediumVioletRed;
            //}

            if (e.Panel.Controls[1].Text == "ENR")
            {
                e.Panel.Controls[1].BackColor = Color.Wheat;
                e.Panel.Controls[1].Click += UniFrm_Click;
                e.Panel.Controls[0].BackColor = Color.Green;

                e.Panel.Controls[0].Text = " test 0 ";
                e.Panel.Controls[0].Click += UniFrm_Click1;
                //e.Panel.Size = new Size(e.Panel.Size.Width, 200);
                //(e.Panel.Controls[0] as RadTextBox).Multiline = true;
            }

            e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
            //e.Panel.Controls[1].ForeColor = Color.Red;
        }

        private void UniFrm_Click1(object sender, EventArgs e)
        {

            MessageBox.Show(((Control)sender).Text + ((Control)sender).AccessibleName + "-" + ((Control)sender).Name);
        }

        private void UniFrm_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(((Control)sender).Text);
            //((Control)sender).Text += " > " + eaip.ENR.BaseEntityType + " > " + eaip.ENR.ENR3.BaseEntityType;
        }

        void radDataEntry1_EditorInitializing(object sender, Telerik.WinControls.UI.EditorInitializingEventArgs e)
        {
            try
            {
                //if (FirstRun)
                //{
                //    string propType = "";
                //    foreach (var prop in this.radDataEntry1.DataSource.GetType().GetProperties())
                //    {
                //        propType = prop.GetValue(this.radDataEntry1.DataSource, null)?.GetType()?.ToString();
                //        if (propType != null && propType.ToLowerInvariant().Contains("system.collections.generic.list`1[aip.dataset"))
                //        {
                //            ComplPropList.Add(prop.Name, prop.GetValue(this.radDataEntry1.DataSource, null));
                //        }
                //        //Console.WriteLine("{0} / {1} / {2}", prop.Name, prop.GetValue(t, null), prop.GetValue(t, null).GetType());
                //    }

                //    FirstRun = false;
                //}

                //if (e.Property.Name == "Salary")
                //{
                //    RadMaskedEditBox radMaskedEditBox = new RadMaskedEditBox();
                //    radMaskedEditBox.MaskType = MaskType.Numeric;
                //    radMaskedEditBox.MaskedEditBoxElement.StretchVertically = true;

                //    e.Editor = radMaskedEditBox;
                //}
                if (e.Property.Name.EndsWith("date")
                    || e.Property.Name.EndsWith("datefrom")
                    || e.Property.Name.EndsWith("dateto")
                    )
                {
                    BaseLib.Airac.AiracCycleDateTime control = new BaseLib.Airac.AiracCycleDateTime();
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                //if (e.Property.PropertyType.Name.ToLowerInvariant().Contains("icollection"))
                //{
                //    //e.Cancel = true;
                //    LinkLabel control = new LinkLabel();

                //    control.Click += Control_Click;
                //    e.Editor = control;
                //    e.Editor.Width = 300;
                //    e.Editor.Height = 300;
                //}
                if (ComplPropList.ContainsKey(e.Property.Name))
                {
                    LinkLabel control = new LinkLabel();
                    control.Click += Control_Click;
                    control.Tag = ComplPropList[e.Property.Name];
                    control.Name = e.Property.Name;
                    e.Editor = control;
                    e.Editor.Width = 300;
                    e.Editor.Height = 300;
                }
                //if (e.Property.Name == "PassWord")
                //{
                //    (e.Editor as RadTextBox).PasswordChar = '*';
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                    ex.Message);
            }

        }

        private void Control_Click(object sender, EventArgs e)
        {
            foreach (RadPageViewPage page in MainTabControl.Pages)
            {
                if (page.Name == ((Control)sender).Name) // Already created, just switch to it
                {
                    MainTabControl.SelectedPage = page;
                    return;
                }
            }

            RadGridView rde = new RadGridView();
            rde.DataSource = ((Control)sender).Tag;
            rde.Dock = DockStyle.Fill;
            rde.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rde.ReadOnly = true;
            rde.ThemeName = "Office2013Light";

            RadPageViewPage rpvp = new RadPageViewPage();
            rpvp.Dock = DockStyle.Fill;
            rpvp.Controls.Add(rde);
            rpvp.Text = "Navaidcollection";
            rpvp.Name = ((Control)sender).Name;
            MainTabControl.Pages.Add(rpvp);
            MainTabControl.SelectedPage = rpvp;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            ((RadButton)sender).Enabled = false;
            //if (((Dataset.eAIP)radDataEntry1.DataSource).id != 0)
            //{
            //    db.Entry((Dataset.eAIP)radDataEntry1.DataSource).State = EntityState.Modified;
            //    ((Dataset.eAIP)radDataEntry1.DataSource).MetaData.DataVersion++;

            //}
            //else
            //    db.eAIP.Add((Dataset.eAIP)radDataEntry1.DataSource);
            db.SaveChanges();
            ((RadButton)sender).Enabled = true;
        }

        private void radDataEntry1_BindingCreating(object sender, BindingCreatingEventArgs e)
        {
            if (e.DataMember.ToLowerInvariant().EndsWith("date"))
            {
                e.PropertyName = "Value";

            }
            //if (e.DataMember.ToLowerInvariant().Contains("navaid"))
            //{
            //    e.PropertyName = "DataSource";
            //}
        }


        void Binding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.Equals(DBNull.Value))
            {
                e.Value = null;

            }
        }

        void Binding_Format2(object sender, ConvertEventArgs e)
        {
            if (!e.Value.Equals(DBNull.Value))
            {

                e.Value = null;

            }
        }
    }
}
