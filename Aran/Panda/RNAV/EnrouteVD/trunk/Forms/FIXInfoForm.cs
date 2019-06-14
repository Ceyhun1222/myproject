using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.PANDA.RNAV.Enroute.VD.Properties;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	public partial class FIXInfoForm : Form
	{
		public FIXInfoForm()
		{
			InitializeComponent();

			label04.Text = GlobalVars.unitConverter.DistanceUnit;
			label06.Text = GlobalVars.unitConverter.DistanceUnit;
			label08.Text = GlobalVars.unitConverter.DistanceUnit;
			label10.Text = GlobalVars.unitConverter.DistanceUnit;
			label12.Text = GlobalVars.unitConverter.DistanceUnit;
			label14.Text = GlobalVars.unitConverter.DistanceUnit;

			label01.Text = Resources.str01211;
			label03.Text = Resources.str01204;
			label05.Text = Resources.str01205;
			label07.Text = Resources.str01206;
			label09.Text = Resources.str01208;
			label11.Text = Resources.str01207;
			label13.Text = Resources.str01209;
		}

		public void ShowInfo(Segment segment, int fix, Point position)
		{
			if (fix == 0)
			{
				textBox01.Text = segment.NavDirStart.ToString("0.0");
				textBox02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Dstart).ToString();
				textBox03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D1start).ToString();
				textBox04.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D2start).ToString();
				textBox05.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Start.ATT).ToString();
				textBox06.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Start.XTT).ToString();
				textBox07.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Start.ASW_L).ToString();
			}
			else
			{
				textBox01.Text = segment.NavDirEnd.ToString("0.0");
				textBox02.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.Dend).ToString();
				textBox03.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D1end).ToString();
				textBox04.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.D2end).ToString();
				textBox05.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.End.ATT).ToString();
				textBox06.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.End.XTT).ToString();
				textBox07.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment.End.ASW_L).ToString();
			}

			Left = position.X;
			Top = position.Y;
			Show();
		}

		private void FIXInfoForm_Deactivate(object sender, EventArgs e)
		{
			Hide();
		}
	}
}
