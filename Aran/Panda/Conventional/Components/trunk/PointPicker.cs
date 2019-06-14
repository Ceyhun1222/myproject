using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChoosePointNS
{
	public partial class PointPicker : UserControl
	{
		public event EventHandler ByClickChanged;
		public event EventHandler LatitudeChanged;
		public event EventHandler LongitudeChanged;
		public event KeyPressEventHandler KeyPressChanged;

		public PointPicker()
		{
			InitializeComponent();

			DD_DMSLat.IsDD = true;
			DD_DMSLat.IsX = false;

			DD_DMSLon.IsDD = true;
			DD_DMSLon.IsX = true;

			DD_DMSLat.DDAccuracy = 4;
			DD_DMSLon.DDAccuracy = 4;

			DD_DMSLat.DMSAccuracy = 2;
			DD_DMSLon.DMSAccuracy = 2;

			DD_DMSLat.Refresh();
			DD_DMSLon.Refresh();

			DD_DMSLat.KeyDown += OnKeyDown_Changed;
			DD_DMSLon.KeyDown += OnKeyDown_Changed;

		}

		public double Latitude
		{
			get
			{
				return DD_DMSLat.Value;
			}
			set
			{
				DD_DMSLat.Value = value;
			}
		}

		public double Longitude
		{
			get
			{
				return DD_DMSLon.Value;
			}
			set
			{
				DD_DMSLon.Value = value;
			}
		}

		public bool ByClick
		{
			get
			{
				return chkByClick.Checked;
			}
			set
			{
				chkByClick.Checked = value;
			}
		}

		public bool IsDD
		{
			get { return DD_DMSLat.IsDD; }
			set
			{
				if (DD_DMSLat.IsDD != value)
				{
					//m_IsDD = value;
					rbDD.Checked = value;
					rbDMS.Checked = !value;
				}
			}
		}

		public int DDAccuracy
		{
			get { return DD_DMSLat.DDAccuracy; }
			set
			{
				if (DD_DMSLat.DDAccuracy != value)
				{
					DD_DMSLat.DDAccuracy = value;
					DD_DMSLon.DDAccuracy = value;
				}
			}
		}

		public int DMSAccuracy
		{
			get { return DD_DMSLat.DMSAccuracy; }
			set
			{
				if (DD_DMSLat.DMSAccuracy != value)
				{
					DD_DMSLat.DMSAccuracy = value;
					DD_DMSLon.DMSAccuracy = value;
				}
			}
		}

		public EarthDirection LatitudeDirection
		{
			get { return DD_DMSLat.Direction; }
		}

		public EarthDirection LongtitudeDirection
		{
			get { return DD_DMSLon.Direction; }
		}

		private void rbDD_CheckedChanged(object sender, EventArgs e)
		{
			DD_DMSLat.IsDD = rbDD.Checked;
			DD_DMSLon.IsDD = rbDD.Checked;

			DD_DMSLat.Enabled = !chkByClick.Checked;
			DD_DMSLon.Enabled = !chkByClick.Checked;

			//if (ByClickChanged != null)
			//    ByClickChanged(this, new EventArgs());
		}

		private void DD_DMSLat_ValueChanged(object sender, EventArgs e)
		{
			if (LatitudeChanged != null)
				LatitudeChanged(this, new EventArgs());
		}

		private void DD_DMSLon_ValueChanged(object sender, EventArgs e)
		{
			if (LongitudeChanged != null)
				LongitudeChanged(this, new EventArgs());
		}

		private void chkkByClick_CheckedChanged(object sender, EventArgs e)
		{
			DD_DMSLat.Enabled = !chkByClick.Checked;
			DD_DMSLon.Enabled = !chkByClick.Checked;

			if (ByClickChanged != null)
				ByClickChanged(this, new EventArgs());
		}

		private void OnKeyDown_Changed(object sender, KeyPressEventArgs e)
		{
			if (KeyPressChanged != null)
				KeyPressChanged(sender, e);
		}
	}
}
