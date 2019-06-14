using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.SGBAS.Forms
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			double arMinISlen = (double)numericUpDown1.Value;

			TextBox0106.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(arMinISlen).ToString();
		}
	}
}
