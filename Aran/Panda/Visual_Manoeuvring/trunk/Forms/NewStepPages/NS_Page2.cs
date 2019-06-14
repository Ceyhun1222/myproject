using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class NS_Page2 : UserControl
    {
        public NS_Page2()
        {
            InitializeComponent();
        }

        private void NS_Page2_Load(object sender, EventArgs e)
        {
            txtBox_maxDivergenceAngle.Text = "45";
            txtBox_maxConvergenceAngle.Text = "90";
        }

        private void txtBox_maxDivergenceAngle_TextChanged(object sender, EventArgs e)
        {
            if (txtBox_maxDivergenceAngle.Text.Equals(""))
                txtBox_maxDivergenceAngle.Text = "0";
            else
                VMManager.Instance.MaxDivergenceAngle = ARANMath.DegToRad(double.Parse(txtBox_maxDivergenceAngle.Text));            
        }

        private void txtBox_maxConvergenceAngle_TextChanged(object sender, EventArgs e)
        {
            if (txtBox_maxConvergenceAngle.Text.Equals(""))
                txtBox_maxConvergenceAngle.Text = "0";
            else
                VMManager.Instance.MaxConvergenceAngle = ARANMath.DegToRad(double.Parse(txtBox_maxConvergenceAngle.Text));
        }

        private void txtBox_maxDivergenceAngle_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }

        private void txtBox_maxConvergenceAngle_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }        
    }
}
