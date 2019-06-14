using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARENA.VisualComponents
{


    public partial class StringProperty : UserControl
    {

        public string PropertyName
        {
            get { return this.Property_Name.Text; }
            set { this.Property_Name.Text = value; }
        }

        //public string PropertyValue
        //{
        //    get { return this.Property_Value.Text; }
        //    set { this.Property_Value.Text = value; }
        //}

        public StringProperty()
        {
            InitializeComponent();
        }

        public StringProperty(bool mandatoryFlag)
        {
            InitializeComponent(mandatoryFlag);
        }

        public StringProperty(bool mandatoryFlag, string tooolTipTextStr)
        {
            InitializeComponent(mandatoryFlag, tooolTipTextStr);
        }

        private void Property_Value_Validated(object sender, EventArgs e)
        {
            //this.PropertyValue = Property_Value.Text;
        }

        private void StringProperty_Load(object sender, EventArgs e)
        {

        }

        private void Property_Name_Click(object sender, EventArgs e)
        {

        }

        private void PropertyValueTxtBx_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
