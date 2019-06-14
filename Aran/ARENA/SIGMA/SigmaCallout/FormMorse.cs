using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaCallout
{
	public partial class FormMorse : Form
	{
		public FormMorse ( double shiftOnX, double shiftOnY, double leading)
		{
			InitializeComponent ( );
			this.SuspendLayout ( );
			nmrcUpDwnLeading.Value = ( decimal ) leading;
			nmrcUpDwnShiftOnX.Value = ( decimal ) shiftOnX;
			nmrcUpDwnShiftOnY.Value = ( decimal ) shiftOnY;
			this.ResumeLayout ( );
			this.PerformLayout ( );
		}

		private void btnClose_Click ( object sender, EventArgs e )
		{
			this.Close ( );
		}

		internal void AddShiftOnXChangedHandler ( EventHandler eventHandler )
		{
			nmrcUpDwnShiftOnX.ValueChanged += eventHandler;
		}

		internal void AddShiftOnYChangedHandler ( EventHandler eventHandler )
		{
			nmrcUpDwnShiftOnY.ValueChanged += eventHandler;
		}

		internal void AddLeadingChangedHandler ( EventHandler eventHandler )
		{
			nmrcUpDwnLeading.ValueChanged += eventHandler;
		}
	}
}