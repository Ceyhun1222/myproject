using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;

namespace Aran.Queries.Common
{
	public partial class DocInfoForm : Form
	{
		private PropRestriction _restriction;

		public DocInfoForm()
		{
			InitializeComponent ();
		}

		public string Description
		{
			get { return ui_infoTB.Text; }
			private set 
			{
				ui_infoTB.Text = value.Replace ("\n", "\r\n");
				ui_infoTB.Select (0, 0);

				var gr = ui_infoTB.CreateGraphics ();
				var textSize = gr.MeasureString (value, ui_infoTB.Font, ui_infoTB.ClientRectangle.Width);
				gr.Dispose ();
				textSize.Height += 8;

				var isLonger = !((RectangleF) ui_infoTB.ClientRectangle).Contains ((PointF) textSize);

				Width = Math.Min ((int) textSize.Width + 10, Width);
				Height = (int) textSize.Height + 10;

				if (!isLonger)
					ui_infoTB.ScrollBars = ScrollBars.None;
			}
		}

		public PropRestriction Restriction
		{
			get { return _restriction; }
		}

		public void SetInfo(string description, PropRestriction restriction)
		{
			var rs = string.Empty;
			if (restriction.Min != null)
				rs += "Min: " + restriction.Min;
			if (restriction.Max != null)
				rs += (rs != string.Empty ? "\r\n" : "") + "Max: " + restriction.Max;

			rs = (rs != string.Empty ? description + "\r\n_______\r\n\r\n" + rs : description);

			Description = rs;
		}

		public void SetEnumItemInfo (AimClassInfo enumClassInfo)
		{

		}

		private void DocInfoForm_Deactivate(object sender, EventArgs e)
		{
			Close ();
		}		
	}
}
