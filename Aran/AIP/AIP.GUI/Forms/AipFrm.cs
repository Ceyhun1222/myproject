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
    public partial class AipFrm : Telerik.WinControls.UI.RadForm
    {
        private int? ObjectId = null;
        eAIPContext db = new eAIPContext();
        internal DB.eAIP eaip = new DB.eAIP();
        Dictionary<string, object> ComplPropList = new Dictionary<string, object>();
        bool FirstRun = true;

        public AipFrm()
        {
            InitializeComponent();
            DesignForm();
        }
        public AipFrm(eAIP t) : this()
        {
            try
            {
                ObjectId = -1;
                this.radDataEntry1.DataSource = t;
                this.Name = t.Effectivedate.ToString();
                this.Name += ", Status: " + t.AIPStatus;
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
            this.radDataEntry1.ItemDefaultSize = new Size(520, 25);
            this.radDataEntry1.ItemSpace = 8;
        }

        

        private void UniFrm_Load(object sender, EventArgs e)
        {
            if (ObjectId == null)
            {
                this.radDataEntry1.DataSource = eaip;
            }
            
        }

        void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            if (e.DataMember.ToLowerInvariant().EndsWith("date"))
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
            e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
        }

        void radDataEntry1_EditorInitializing(object sender, Telerik.WinControls.UI.EditorInitializingEventArgs e)
        {
            try
            {                
                if (e.Property.Name.EndsWith("date"))
                {
                    BaseLib.Airac.AiracCycleDateTime control = new BaseLib.Airac.AiracCycleDateTime();
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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            ((RadButton)sender).Enabled = false;
            eaip = (DB.eAIP)radDataEntry1.DataSource;
            //db.SaveChanges();
            ((RadButton)sender).Enabled = true;
        }

        private void radDataEntry1_BindingCreating(object sender, BindingCreatingEventArgs e)
        {
            if (e.Control is RadDropDownList)
            {
                e.PropertyName = "SelectedValue";
            }
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
        
    }
}
