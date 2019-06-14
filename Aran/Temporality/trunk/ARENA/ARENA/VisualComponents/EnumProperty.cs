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
    public partial class EnumProperty : UserControl
    {
        public string PropertyName
        {
            get { return this.Property_Name.Text; }
            set { this.Property_Name.Text = value; }
        }

        public string PropertyType
        {
            get { return this.Property_Type; }
            set { this.Property_Type = value; }
        }

        public EnumProperty()
        {
            InitializeComponent();
        }

        public EnumProperty(bool MandatoryGlag)
        {
            InitializeComponent(MandatoryGlag);
        }

        public EnumProperty(bool MandatoryGlag, string tooolTipTextStr)
        {
            InitializeComponent(MandatoryGlag,tooolTipTextStr);
        }

        private void PropertyValueCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
