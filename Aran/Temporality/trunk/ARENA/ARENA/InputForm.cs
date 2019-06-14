using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PDM;
using ARINC_DECODER_CORE;
using PDM.PropertyExtension;
using System.Collections;

namespace ARENA
{
    public partial class InputForm : Form
    {
        private object _PdmObj;
        public object LinkedObject
        {
            get { return _PdmObj; }
            set { _PdmObj = value; }
        }

        public InputForm()
        {
            InitializeComponent();
        }


        private void _InpurForm_Load(object sender, EventArgs e)
        {
            this.panel2.SuspendLayout();
            this.SuspendLayout();

            if (LinkedObject is PDMObject)
            {


                this.Text = LinkedObject.GetType().Name;

                int cntrlCntr = CreateComponentsPanel(ref this.panel2, LinkedObject as PDMObject);
                cntrlCntr+=1;
                this.Size = new Size(this.Width, 40 * cntrlCntr + panel1.Height);
                this.panel2.ResumeLayout(false);


            }

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void InpurForm_Load(object sender, EventArgs e)
        {
            if (LinkedObject is PDMObject)
            {
                propertyGrid1.SelectedObject = LinkedObject;
            }
            if (LinkedObject is Settings.ArenaSettings)
            {
                propertyGrid1.SelectedObject = LinkedObject;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void PropertyValueCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {

            object obj = System.Enum.Parse(((ComboBox)sender).Tag.GetType(), ((ComboBox)sender).Text);
            Static_Proc.SetObjectValue(LinkedObject, ((ComboBox)sender).DataBindings[0].BindingMemberInfo.BindingField, obj);
        }

        private int CreateComponentsPanel(ref Panel panel, PDMObject Obj)
        {
            int cntrlCntr = 0;
            bool flagMandatory = false;

            #region Sort Properties by PropertyOrderAttribute

            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(Obj);
            ArrayList orderedProperties = new ArrayList();

            foreach (PropertyDescriptor pd in pdc)
            {

                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];

                if (attribute != null)
                {
                    // атрибут есть - используем номер п/н из него
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    // атрибута нет – считаем, что 0
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }

            orderedProperties.Sort();

            // формируем список имен свойств
            ArrayList propertyNames = new ArrayList();

            foreach (PropertyOrderPair pop in orderedProperties)
                propertyNames.Add(pop.Name);

            PropertyDescriptorCollection pdcsort = pdc.Sort((string[])propertyNames.ToArray(typeof(string)));


            #endregion

            for (int i = pdcsort.Count - 1; i >= 0; i--)
            {

                flagMandatory = false;

                PropertyDescriptor pd = pdcsort[i];
                Attribute BrowsableFlag = pd.Attributes[typeof(BrowsableAttribute)];
                Attribute MandatoryFalg = pd.Attributes[typeof(Mandatory)];
                //string DescriptionString = pd.Description;
                if (MandatoryFalg != null)
                {
                        flagMandatory = ((Mandatory)MandatoryFalg).MandatoryFlag;
                }

                if (BrowsableFlag != null) 
                {
                    if (!((BrowsableAttribute)BrowsableFlag).Browsable)
                        continue;
                }

                if (pd.PropertyType.IsEnum)
                {
                    #region

                    string[] itms = Enum.GetNames(pd.PropertyType);

                    VisualComponents.EnumProperty cntrl = new VisualComponents.EnumProperty(flagMandatory);

                    cntrl.Dock = System.Windows.Forms.DockStyle.Top;
                    //cntrl.Location = new System.Drawing.Point(0, 0);
                    cntrl.PropertyName = pd.Name;
                    cntrl.PropertyValueCmbBx.DataBindings.Add("Text", Obj, pd.Name);
                    cntrl.PropertyValueCmbBx.Items.AddRange(itms);


                    cntrl.PropertyValueCmbBx.Tag = Activator.CreateInstance(pd.PropertyType);
                    cntrl.PropertyValueCmbBx.SelectedIndexChanged += new EventHandler(PropertyValueCmbBx_SelectedIndexChanged);

                    panel.Controls.Add(cntrl);

                    #endregion
                }
                else
                {
                    #region

                    VisualComponents.StringProperty cntrl = new VisualComponents.StringProperty(flagMandatory, pd.Description);

                    cntrl.Dock = System.Windows.Forms.DockStyle.Top;
                    //cntrl.Location = new System.Drawing.Point(0, 0);
                    cntrl.PropertyName = pd.Name;
                    cntrl.PropertyValueTxtBx.DataBindings.Add("Text", Obj, pd.Name);
                    cntrl.Leave += new EventHandler(cntrl_Leave);
                    panel.Controls.Add(cntrl);
                    //this.Controls.Add(cntrl);
                    #endregion

                }

               

                cntrlCntr++;

            }

            return cntrlCntr;
        }


        void cntrl_Leave(object sender, EventArgs e)
        {
            if ((sender as VisualComponents.StringProperty).PropertyName.CompareTo("Lat") == 0)
            {
                AranSupport.Utilitys ut = new AranSupport.Utilitys();
                double lat = ut.GetLATITUDEFromAIXMString((sender as VisualComponents.StringProperty).PropertyValueTxtBx.Text);
                if (lat == 0) (sender as VisualComponents.StringProperty).PropertyValueTxtBx.Text = "00.00N";
            }

            if ((sender as VisualComponents.StringProperty).PropertyName.CompareTo("Lon") == 0)
            {
                AranSupport.Utilitys ut = new AranSupport.Utilitys();
                double lon = ut.GetLONGITUDEFromAIXMString((sender as VisualComponents.StringProperty).PropertyValueTxtBx.Text);
                if (lon == 0) (sender as VisualComponents.StringProperty).PropertyValueTxtBx.Text = "000.00N";
            }
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
           
        }

        private void propertyGrid1_PropertySortChanged(object sender, EventArgs e)
        {
            
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            
        }

        private void propertyGrid1_Validating(object sender, CancelEventArgs e)
        { 

        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

            if (e.ChangedItem.PropertyDescriptor.Name.CompareTo("Lat") == 0)
            {
                AranSupport.Utilitys ut = new AranSupport.Utilitys();
                double lat = ut.GetLATITUDEFromAIXMString(e.ChangedItem.Value.ToString());
                if (lat == 0) Static_Proc.SetObjectValue(LinkedObject, e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
            }

            if (e.ChangedItem.PropertyDescriptor.Name.CompareTo("Lon") == 0)
            {
                AranSupport.Utilitys ut = new AranSupport.Utilitys();
                double lon = ut.GetLONGITUDEFromAIXMString(e.ChangedItem.Value.ToString());
                if (lon == 0) Static_Proc.SetObjectValue(LinkedObject, e.ChangedItem.PropertyDescriptor.Name, e.OldValue);
            }
        }


    }
}
