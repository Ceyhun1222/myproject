using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace ARENA.VisualComponents
{
    public partial class DoubleType_Property : UserControl
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

        public DoubleType_Property()
        {
            InitializeComponent();
        }

        private void Property_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberFormatInfo nf_inf = CultureInfo.CurrentCulture.NumberFormat;
           

            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8) e.Handled = true;
            if (nf_inf.NumberDecimalSeparator.CompareTo(e.KeyChar.ToString()) == 0) e.Handled = false;
            if (nf_inf.NegativeSign.CompareTo(e.KeyChar.ToString()) == 0) e.Handled = false;
            
        }

        private void Property_Value_Validated(object sender, EventArgs e)
        {
            double dbl;
            if ((!Double.TryParse(this.PropertyValueTxtBx.Text,out dbl)) && this.PropertyValueTxtBx.Text.Length>0)
                this.PropertyValueTxtBx.Text = "ERROR";
        }
    }
}
