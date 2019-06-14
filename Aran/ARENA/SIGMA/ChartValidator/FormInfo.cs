using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartValidator
{
	public partial class FormInfo : Form
	{
        public FormInfo(ChartType chartType)
        {
            InitializeComponent();
            label3.Visible = (chartType == ChartType.Enroute);
            label4.Visible = (chartType == ChartType.Enroute);
            label5.Visible = (chartType == ChartType.Enroute);
            label6.Visible = (chartType == ChartType.Enroute);
            label7.Visible = (chartType == ChartType.Enroute);
            label8.Visible = (chartType == ChartType.Enroute);
        }

		private void button1_Click (object sender, EventArgs e)
		{
			this.Close ();
		}
    }
}
