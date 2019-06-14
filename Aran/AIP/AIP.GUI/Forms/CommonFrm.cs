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
    public partial class CommonFrm : Telerik.WinControls.UI.RadForm
    {
        private dynamic GlobalObject;
        Dictionary<string, object> ComplPropList = new Dictionary<string, object>();

        public CommonFrm()
        {
            InitializeComponent();
            DesignForm();
        }

        public CommonFrm(string Title, dynamic Object) : this()
        {
            try
            {
                if (Object?.id != null)
                {
                    Text = Title;
                    GlobalObject = Object;
                    this.radDataEntry1.DataSource = Object;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
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
            //if (ObjectId == null)
            //{
            //    this.radDataEntry1.DataSource = GlobalType;
            //}
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
        }


        void Binding_Parse(object sender, ConvertEventArgs e)
        {
            string obj = e.Value as string;
            e.Value = obj;
        }

        void radDataEntry1_ItemInitialized(object sender, Telerik.WinControls.UI.ItemInitializedEventArgs e)
        {
            
            if (e.Panel.Controls[1].Text == "ENR")
            {
                e.Panel.Controls[1].BackColor = Color.Wheat;
                e.Panel.Controls[1].Click += UniFrm_Click;
                e.Panel.Controls[0].BackColor = Color.Green;

                e.Panel.Controls[0].Text = " test 0 ";
                e.Panel.Controls[0].Click += UniFrm_Click1;
            }

            e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
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
               
                if (e.Property.Name.EndsWith("date")
                    || e.Property.Name.EndsWith("datefrom")
                    || e.Property.Name.EndsWith("dateto")
                    )
                {
                    BaseLib.Airac.AiracCycleDateTime control = new BaseLib.Airac.AiracCycleDateTime();
                    e.Editor = control;
                    e.Editor.Width = 300;
                }
                
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
            try
            {
                ((RadButton)sender).Enabled = false;
                GlobalObject = radDataEntry1.DataSource;
                DialogResult = DialogResult.OK;
                ((RadButton)sender).Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radDataEntry1_BindingCreating(object sender, BindingCreatingEventArgs e)
        {
            if (e.DataMember.ToLowerInvariant().EndsWith("date"))
            {
                e.PropertyName = "Value";
            }
        }


        void Binding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.Equals(DBNull.Value))
            {
                e.Value = null;

            }
        }

        private void CommonFrm_FormClosing(object sender, FormClosingEventArgs e)
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
