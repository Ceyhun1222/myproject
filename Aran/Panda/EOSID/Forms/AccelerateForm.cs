using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EOSID
{
	public partial class AccelerateForm : Form
	{
		private TrakceLeg[] _LegList;
		private int _LegCount;
		private int _massIndex;

		private double _maxAccelerationHeight;
		private double _maxNetGrad2;
		private double _maxNetGrad4;
		private double _xRet3;
		private double _xRet4;
		private double _phase3Length;
		private double totalPathLeng;
		private double transitionH;
		private ObstacleData det;

		public AccelerateForm()
		{
			InitializeComponent();
			transitionH = GlobalVars.minAccelerationHeight;
			textBox01.Text = UnitConverter.HeightToDisplayUnits(transitionH, eRoundMode.CEIL).ToString();
			label02.Text = UnitConverter.HeightUnit;
			label04.Text = UnitConverter.DistanceUnit;
		}

		private void AccelerateForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void AccelerateForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
				//e.Handled = true;
			}
		}

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void textBox01_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox01_Validating(textBox01, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox01.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox01_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox01.Text, out fTmp))
			{
				if (double.TryParse((string)textBox01.Tag, out fTmp))
					textBox01.Text = (string)textBox01.Tag;
				else
					textBox01.Text = UnitConverter.DistanceToDisplayUnits(GlobalVars.heightAboveDER, eRoundMode.CEIL).ToString();

				return;
			}

			if (textBox01.Tag != null && textBox01.Tag.ToString() == textBox01.Text)
				return;

			double NewHeight = UnitConverter.HeightToInternalUnits(fTmp);

			if (NewHeight < GlobalVars.minAccelerationHeight)
				NewHeight = GlobalVars.minAccelerationHeight;
			if (NewHeight > _maxAccelerationHeight)
				NewHeight = _maxAccelerationHeight;
			
			transitionH = NewHeight;

			_xRet3 = (transitionH - GlobalVars.heightAboveDER) / _maxNetGrad2;

			textBox01.Text = UnitConverter.HeightToDisplayUnits(transitionH, eRoundMode.CEIL).ToString();
			textBox01.Tag = textBox01.Text;
			Calculate();
		}

		private void Calculate()
		{
			double xMin = (transitionH + GlobalVars.NetMOC) / _maxNetGrad4;

			_xRet4 = totalPathLeng;				// double.MaxValue;	//xMin;
			int l = -1, o = -1;

			for (int j = 0; j < _LegCount; j++)
			{
				for (int i = 0; i < _LegList[j].ObstacleList.Length; i++)
				{
					double x1 = (_LegList[j].ObstacleList[i].MOCH - transitionH) / _maxNetGrad4;
					if (x1 > 0.0)
					{
						double x = _LegList[j].ObstacleList[i].TotalDist - x1;
						if (x < _xRet4)
						{
							l = j;
							o = i;
							_xRet4 = x;
						}
					}
				}
			}

			if (l >= 0 && o >= 0)
				det = _LegList[l].ObstacleList[o];
			else
				det = new ObstacleData();

			textBox04.Text = det.ID;
 
			_xRet4 = Math.Max(_xRet4, xMin);

			_phase3Length = _xRet4 - _xRet3;
			textBox02.Text = UnitConverter.DistanceToDisplayUnits(_phase3Length, eRoundMode.NERAEST).ToString();

			double fTAS = Functions.IAS2TAS(GlobalVars.v2[_massIndex], transitionH, GlobalVars.m_CurrADHP.ISAtC);
			double phase3time = 0.06 * _phase3Length / fTAS;
			textBox03.Text = Math.Round (phase3time, 2).ToString();
		}

		public DialogResult DoDialog(ref TrakceLeg[] LegList, int cnt, int massIndex, double maxAccelerationHeight, double PDG2, double PDG4)
		{
			totalPathLeng = LegList[cnt - 1].WorstCase.PrevTotalLength + LegList[cnt - 1].WorstCase.Length;

			_LegList = LegList;
			_LegCount = cnt;
			_maxAccelerationHeight = maxAccelerationHeight;
			_maxNetGrad2 = PDG2;
			_maxNetGrad4 = PDG4;
			_massIndex = massIndex;
			textBox01.Tag = "";
			textBox01_Validating(textBox01, new CancelEventArgs());

			return ShowDialog();
		}

	}
}
