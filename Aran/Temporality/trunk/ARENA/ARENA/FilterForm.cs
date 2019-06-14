using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using PDM;
using System.Collections;
using PDM.PropertyExtension;

namespace ARENA
{
    public partial class FilterForm : Form
    {
        private List<string> filterList;

        public List<string> FilterList
        {
            get { return filterList; }
            set { filterList = value; }
        }

        public FilterForm()
        {
            InitializeComponent();
            FilterList = new List<string>();
        }

        private void comboBoxObjectTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxObjectTypes.Text.Length <= 0) return;

            ObjectHandle handle = Activator.CreateInstance("PDM", "PDM."+comboBoxObjectTypes.Text);
            System.Object pdm = handle.Unwrap();

            comboBoxObjectProperty.Items.Clear();
            Load_comboBoxObjectProperty(pdm);

            if (comboBoxObjectProperty.Items.Count > 0) comboBoxObjectProperty.SelectedIndex = 0;

            this.Tag = pdm;
        }

        private void Load_comboBoxObjectProperty(System.Object Obj)
        {

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

            foreach (PropertyOrderPair pop in orderedProperties)  propertyNames.Add(pop.Name);

            PropertyDescriptorCollection pdcsort = pdc.Sort((string[])propertyNames.ToArray(typeof(string)));


            #endregion

            for (int i = 0; i <= pdcsort.Count - 1; i++)
            {

                PropertyDescriptor pd = pdcsort[i];
                Attribute BrowsableFlag = pd.Attributes[typeof(BrowsableAttribute)];

                if (BrowsableFlag != null)
                {
                    if (!((BrowsableAttribute)BrowsableFlag).Browsable)
                        continue;
                }

                comboBoxObjectProperty.Items.Add(pd.Name);

            }

        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            comboBoxOperationType.SelectedIndex = 0;
            listBoxFilters.Items.Clear();

            if ((FilterList!=null) && (FilterList.Count >0)) 
                listBoxFilters.Items.AddRange(FilterList.ToArray());


            FilterList.Clear();

        }

        private void comboBoxObjectProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Tag == null) return;

            comboBoxFilterValue.Items.Clear();
            comboBoxFilterValue.Text = "";
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this.Tag);

            PropertyDescriptor pd = pdc[comboBoxObjectProperty.Text];
            if ((pd!=null) && (pd.PropertyType.IsEnum))
            {
                comboBoxFilterValue.Items.AddRange(Enum.GetNames(pd.PropertyType));
            }

            buttonApply.Enabled = listBoxFilters.Items.Count > 0;
        }

        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            string filterString="";
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this.Tag);

            PropertyDescriptor pd = pdc[comboBoxObjectProperty.Text];

            if (pd.PropertyType.Name.StartsWith("Double"))
            {
                double d;
                if (!Double.TryParse(comboBoxFilterValue.Text, out d)) comboBoxFilterValue.Text = "0";
                filterString = "SELECT OBJECTS FROM " + comboBoxObjectTypes.Text + " WHERE " + comboBoxObjectTypes.Text + "." + comboBoxObjectProperty.Text + " " + comboBoxOperationType.Text + " " + comboBoxFilterValue.Text;
            }
            else
            {
                filterString = "SELECT OBJECTS FROM " + comboBoxObjectTypes.Text + " WHERE " + comboBoxObjectTypes.Text + "." + comboBoxObjectProperty.Text + " " + comboBoxOperationType.Text + " '" + comboBoxFilterValue.Text + "'";
            }

            listBoxFilters.Items.Add(filterString);

            buttonApply.Enabled = listBoxFilters.Items.Count > 0;
        }

        private void buttonClearFilter_Click(object sender, EventArgs e)
        {
            listBoxFilters.Items.Clear();
            FilterList.Clear();
        }

        private void comboBoxOperationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxFilterValue.Items.Clear();
            comboBoxFilterValue.Text = "";
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            for(int i=0; i<= listBoxFilters.Items.Count-1;i++ )
            FilterList.Add(listBoxFilters.Items[i].ToString());
        }

        private void listBoxFilters_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void listBoxFilters_KeyUp(object sender, KeyEventArgs e)
        {
            if (listBoxFilters.Items.Count == 0) return;
            if (listBoxFilters.SelectedIndex == -1) return;

            if (e.KeyCode == Keys.Delete) listBoxFilters.Items.RemoveAt(listBoxFilters.SelectedIndex);
        }

    }
}
