using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CDOTMA
{
	partial class SelectLayersBox : Form
	{
		public DataType datatypes
		{
			get
			{
				DataType result = DataType.NoData;

				if (checkBox1.Checked)
					result |= DataType.Airspace;

				if (checkBox2.Checked)
					result |= DataType.AirportHeliport;

				if (checkBox4.Checked)
					result |= DataType.Route;

				if (checkBox5.Checked)
					result |= DataType.STAR;

				if (checkBox6.Checked)
					result |= DataType.IAP;

				if (checkBox7.Checked)
					result |= DataType.SID;

				return result;
			}
		}

		public bool LoadAirspaces
		{
			get { return checkBox1.Checked; }
		}

		public bool LoadAirportHeliports
		{
			get { return checkBox2.Checked; }
		}

		public bool LoadRouts
		{
			get { return checkBox4.Checked; }
		}

		public bool LoadSTARs
		{
			get { return checkBox5.Checked; }
		}

		public bool LoadIAPs
		{
			get { return checkBox6.Checked; }
		}

		public bool LoadSIDs
		{
			get { return checkBox7.Checked; }
		}
	}
}
