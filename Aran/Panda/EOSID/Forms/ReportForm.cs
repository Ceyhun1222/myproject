using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace EOSID
{
	public partial class ReportForm : Form
	{
		#region declerations

		private int maxLegs;
		private int LegsCnt;
		private int _previousTab;
		private int _helpContextID;

		private TrackLeg[] _legs3;
		private TrackLeg[] _legs4;

		private int Page3Index, Page4Index;

		private ObstacleData[] _obstaclesPage1;
		private ObstacleData[] _obstaclesPage2;
		private ObstacleData[] _obstaclesPage3;
		private ObstacleData[] _obstaclesPage4;
		private ObstacleData[] _obstaclesPage5;

		private IElement _pointElem;
		private CheckBox _reportBtn;

		#endregion

		public ReportForm()
		{
			InitializeComponent();

			_previousTab = -1;

			maxLegs = 256;
			LegsCnt = 0;
			_legs3 = new TrackLeg[maxLegs];
			Page3Index = Page4Index = -1;
		}

		private void ReportForm_Load(object sender, EventArgs e)
		{
			dataGridView1.Columns[2].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView1.Columns[3].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView1.Columns[4].HeaderText += " (" + UnitConverter.HeightUnit + ")";


			dataGridView2.Columns[2].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView2.Columns[3].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView2.Columns[4].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView2.Columns[5].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView2.Columns[6].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView2.Columns[7].HeaderText += " (" + UnitConverter.DistanceUnit + ")";

			dataGridView3.Columns[2].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView3.Columns[3].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView3.Columns[4].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView3.Columns[5].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView3.Columns[6].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView3.Columns[7].HeaderText += " (" + UnitConverter.DistanceUnit + ")";

			dataGridView4.Columns[2].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView4.Columns[3].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView4.Columns[4].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView4.Columns[5].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView4.Columns[6].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView4.Columns[7].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView4.Columns[8].HeaderText += " (" + UnitConverter.HeightUnit + ")";

			dataGridView5.Columns[2].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView5.Columns[3].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView5.Columns[4].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView5.Columns[5].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView5.Columns[6].HeaderText += " (" + UnitConverter.HeightUnit + ")";
			dataGridView5.Columns[7].HeaderText += " (" + UnitConverter.HeightUnit + ")";

			dataGridView5.Columns[9].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView5.Columns[10].HeaderText += " (" + UnitConverter.DistanceUnit + ")";
			dataGridView5.Columns[11].HeaderText += " (" + UnitConverter.HeightUnit + ")";

			Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; // assemName.Version;
			this.Text = this.Text + "" + " v:" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Revision.ToString();
		}

		private void ReportForm_KeyUp(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F1)
			//{
			//    NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
			//    e.Handled = true;
			//}
		}

		private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Graphics.DeleteElement(_pointElem);

			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
				_reportBtn.Checked = false;
			}
		}

		public void Init(CheckBox Btn, int HelpContext)
		{
			_reportBtn = Btn;
			_helpContextID = HelpContext;
		}

		//int GetIndex(ObstacleType[] Obstacles, ObstacleType TestObs)
		//{
		//    int i, n = Obstacles.Length;

		//    for (i = 0; i < n; i++)
		//        if (Obstacles[i].index == TestObs.index)
		//            return i;

		//    return -1;
		//}

		private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			e.Handled = true;
			if (e.CellValue2 == null && e.CellValue1 == null)
			{
				e.SortResult = 0;
				return;
			}
			if (e.CellValue2 == null && e.CellValue1 != null)
			{
				e.SortResult = 1;
				return;
			}
			if (e.CellValue2 != null && e.CellValue1 == null)
			{
				e.SortResult = -1;
				return;
			}

			switch (e.Column.Name[0])
			{
				case 'f':
					double f1 = double.Parse(e.CellValue1.ToString()), f2 = double.Parse(e.CellValue2.ToString());
					if (f1 > f2)
						e.SortResult = 1;
					else if (f1 < f2)
						e.SortResult = -1;
					else
						e.SortResult = 0;
					break;
				case 'i':
					int i1 = int.Parse(e.CellValue1.ToString()), i2 = int.Parse(e.CellValue2.ToString());
					if (i1 > i2)
						e.SortResult = 1;
					else if (i1 < i2)
						e.SortResult = -1;
					else
						e.SortResult = 0;
					break;
				//case 't':
				default:
					e.SortResult = System.String.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
					break;
			}
		}

		public void FillPage1(TrackLeg leg)
		{
			int n = leg.ObstacleList.Length;
			_obstaclesPage1 = leg.ObstacleList;// new ObstacleType[n];

			dataGridView1.RowCount = 0;

			ObstacleData DetObs;
			DetObs.ID = "";

			if (leg.ObsMaxNetGrd >= 0)
				DetObs = leg.ObstacleList[leg.ObsMaxNetGrd];

			for (int i = 0; i < n; i++)
			{
				//_obstaclesPage1[i] = Obstacles[i];
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage1[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage1[i].Name;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage1[i].ID;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = UnitConverter.DistanceToDisplayUnits(_obstaclesPage1[i].X, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = UnitConverter.DistanceToDisplayUnits(_obstaclesPage1[i].Y, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = UnitConverter.HeightToDisplayUnits(_obstaclesPage1[i].Height, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = UnitConverter.GradientToDisplayUnits(_obstaclesPage1[i].ReqNetGradient, eRoundMode.CEIL).ToString();

				dataGridView1.Rows.Add(row);

				if (_obstaclesPage1[i].ID == DetObs.ID)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
					row.Cells[0].Style.Font = new Font(Font, FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}

				row.Cells[3].Style.Font = new Font(Font, FontStyle.Bold);
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.win32Window);
		}

		public void FillCurrentLeg(TrackLeg leg)
		{
			int n = leg.ObstacleList.Length;
			_obstaclesPage2 = leg.ObstacleList;// new ObstacleType[n];

			dataGridView2.RowCount = 0;

			ObstacleData DetObs;
			DetObs.ID = "";

			if (leg.ObsMaxNetGrd >= 0)
				DetObs = leg.ObstacleList[leg.ObsMaxNetGrd];

			for (int i = 0; i < n; i++)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage2[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage2[i].Name;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage2[i].ID;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = UnitConverter.DistanceToDisplayUnits(_obstaclesPage2[i].TotalDist, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = UnitConverter.DistanceToDisplayUnits(_obstaclesPage2[i].X, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = UnitConverter.DistanceToDisplayUnits(_obstaclesPage2[i].Y, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = UnitConverter.HeightToDisplayUnits(_obstaclesPage2[i].Height, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = UnitConverter.HeightToDisplayUnits(_obstaclesPage2[i].MOC, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = UnitConverter.DistanceToDisplayUnits(_obstaclesPage2[i].AcceleStartDist, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = UnitConverter.GradientToDisplayUnits(_obstaclesPage2[i].ReqNetGradient, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = UnitConverter.GradientToDisplayUnits(_obstaclesPage2[i].ApplNetGradient2, eRoundMode.CEIL).ToString();

				dataGridView2.Rows.Add(row);

				//if (_obstaclesPage2[i].ID == DetObs.ID)
				if (_obstaclesPage2[i].ReqNetGradient > _obstaclesPage2[i].ApplNetGradient2)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
					row.Cells[0].Style.Font = new Font(Font, FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}

				row.Cells[3].Style.Font = new Font(Font, FontStyle.Bold);
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.win32Window);
		}

		public void AddLeg(TrackLeg leg)
		{
			if (LegsCnt >= maxLegs)
			{
				maxLegs += 256;
				Array.Resize<TrackLeg>(ref _legs3, maxLegs);
			}

			_legs3[LegsCnt++] = leg;

			int OldMax = (int)numericUpDown1.Maximum;
			numericUpDown1.Maximum = LegsCnt;
			numericUpDown1.Value = numericUpDown1.Maximum;

			if (OldMax == LegsCnt)
				FillPage3(LegsCnt - 1);
		}

		public void RemoveLastLeg()
		{
			LegsCnt--;
			if (LegsCnt > 0)
			{
				bool bRefresh = numericUpDown1.Value > LegsCnt;
				if (bRefresh)
					numericUpDown1.Value = LegsCnt;

				numericUpDown1.Maximum = LegsCnt;

				if (!bRefresh)
					FillPage3(LegsCnt - 1);
			}
			else
			{
				dataGridView2.RowCount = 0;
				dataGridView3.RowCount = 0;
				LegsCnt = 0;
			}
		}

		public void DeleteNLegs(int N)
		{
			LegsCnt -= N;
			if (LegsCnt > 0)
			{
				bool bRefresh = numericUpDown1.Value > LegsCnt;
				if (bRefresh)
					numericUpDown1.Value = LegsCnt;

				numericUpDown1.Maximum = LegsCnt;

				if (!bRefresh)
					FillPage3(LegsCnt - 1);
			}
			else
			{
				dataGridView2.RowCount = 0;
				dataGridView3.RowCount = 0;
				dataGridView4.RowCount = 0;
				LegsCnt = 0;
			}
		}

		public void RemoveAllLegs()
		{
			DeleteNLegs(LegsCnt);
		}

		public void FillPage3(int LegIndex)
		{
			Page3Index = LegIndex;

			dataGridView3.RowCount = 0;
			if (LegIndex < 0)
				return;

			_obstaclesPage3 = _legs3[LegIndex].ObstacleList;
			if (_obstaclesPage3 == null)
				return;
			ObstacleData DetObs;
			DetObs.ID = "";

			if (_legs3[LegIndex].ObsMaxNetGrd >= 0)
				DetObs = _obstaclesPage3[_legs3[LegIndex].ObsMaxNetGrd];

			int n = _obstaclesPage3.Length;

			for (int i = 0; i < n; i++)
			{
				ObstacleData Obstacle = _obstaclesPage3[i];
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage3[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacle.Name;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacle.ID.ToString();
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.TotalDist, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.X, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.Y, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = UnitConverter.HeightToDisplayUnits(Obstacle.Height, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = UnitConverter.HeightToDisplayUnits(Obstacle.MOC, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.AcceleStartDist, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = UnitConverter.GradientToDisplayUnits(Obstacle.ReqNetGradient, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = UnitConverter.GradientToDisplayUnits(Obstacle.ApplNetGradient2, eRoundMode.CEIL).ToString();

				dataGridView3.Rows.Add(row);

				if (Obstacle.ID == DetObs.ID)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
					row.Cells[0].Style.Font = new Font(Font, FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}
				row.Cells[3].Style.Font = new Font(Font, FontStyle.Bold);
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.win32Window);
		}

		public void FillPage4Legs(TrackLeg[] legs)
		{
			_legs4 = legs;

			if (numericUpDown1.Value > 1)
				FillPage4((int)numericUpDown1.Value - 1);
			else
				FillPage4(0);
		}

		public void FillPage4(int LegIndex)
		{
			Page4Index = LegIndex;

			dataGridView4.RowCount = 0;
			if (LegIndex < 0)
				return;

			_obstaclesPage4 = _legs4[LegIndex].ObstacleList;
			if (_obstaclesPage4 == null)
				return;

			string[] phaseNames = new string[3] { "Phase 2", "Accelerate phase", "Phase 4" };
			ObstacleData DetObs;
			DetObs.ID = "";

			if (_legs4[LegIndex].ObsMaxNetGrd >= 0)
				DetObs = _obstaclesPage4[_legs4[LegIndex].ObsMaxNetGrd];

			int n = _obstaclesPage4.Length;

			for (int i = 0; i < n; i++)
			{
				ObstacleData Obstacle = _obstaclesPage4[i];
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage4[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacle.Name;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacle.ID.ToString();
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.TotalDist, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.X, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.Y, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = UnitConverter.HeightToDisplayUnits(Obstacle.Height, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = UnitConverter.HeightToDisplayUnits(Obstacle.MOC, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = UnitConverter.HeightToDisplayUnits(Obstacle.MOCH, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = UnitConverter.HeightToDisplayUnits(Obstacle.ActualHeight, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = UnitConverter.GradientToDisplayUnits(Obstacle.ApplNetGradient2, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = UnitConverter.GradientToDisplayUnits(Obstacle.ApplNetGradient4, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = phaseNames[Obstacle.Phase];

				dataGridView4.Rows.Add(row);

				if (Obstacle.ID == DetObs.ID)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
					row.Cells[0].Style.Font = new Font(Font, FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}
				row.Cells[3].Style.Font = new Font(Font, FontStyle.Bold);
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.win32Window);
		}

		public void FillPage5(ObstacleData[] ObstacleList, ObstacleData DetObs, double AccelHeight)
		{
			int n = ObstacleList.Length;
			_obstaclesPage5 = ObstacleList;

			dataGridView5.RowCount = 0;

			//ObstacleData DetObs;
			//DetObs.ID = "";

			//if (n > 0)
			//    DetObs = ObstacleList[n - 1];

			for (int i = 0; i < n; i++)
			{
				DataGridViewRow row = new DataGridViewRow();
				ObstacleData Obstacle = _obstaclesPage5[i];
				row.Tag = Obstacle;

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacle.Name;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacle.ID;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.TotalDist, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.X, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.Y, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = UnitConverter.HeightToDisplayUnits(Obstacle.Height, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = UnitConverter.HeightToDisplayUnits(Obstacle.MOC, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = UnitConverter.HeightToDisplayUnits(Obstacle.MOCH, eRoundMode.NERAEST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = UnitConverter.GradientToDisplayUnits(Obstacle.ApplNetGradient2, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.AcceleStartDist, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = UnitConverter.DistanceToDisplayUnits(Obstacle.AcceleEndDist, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = UnitConverter.HeightToDisplayUnits(AccelHeight, eRoundMode.CEIL).ToString();


				dataGridView5.Rows.Add(row);

				if (_obstaclesPage5[i].ID == DetObs.ID)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
					row.Cells[0].Style.Font = new Font(Font, FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}

				row.Cells[3].Style.Font = new Font(Font, FontStyle.Bold);
			}

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.win32Window);
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (mainTabControl.SelectedIndex == 2)
				FillPage3((int)numericUpDown1.Value - 1);
			else if (_obstaclesPage4 != null && mainTabControl.SelectedIndex == 3)
				FillPage4((int)numericUpDown1.Value - 1);
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Graphics.DeleteElement(_pointElem);

			//Graphic.Refresh();
			//GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

			_reportBtn.Checked = false;
			Hide();
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{
			//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
		}

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_obstaclesPage1 == null)
				return;
			if (e.RowIndex < 0 || _obstaclesPage1.Length == 0)
				return;

			Graphics.DeleteElement(_pointElem);
			ObstacleData obs = (ObstacleData)dataGridView1.Rows[e.RowIndex].Tag;
			_pointElem = Graphics.DrawPointWithText(obs.pPtPrj, obs.ID.ToString(), 255);	// RGB(0, 0, 255)
			_pointElem.Locked = true;
		}

		private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_obstaclesPage2 == null)
				return;
			if (e.RowIndex < 0 || _obstaclesPage2.Length == 0)
				return;

			Graphics.DeleteElement(_pointElem);
			ObstacleData obs = (ObstacleData)dataGridView2.Rows[e.RowIndex].Tag;
			_pointElem = Graphics.DrawPointWithText(obs.pPtPrj, obs.ID.ToString(), 255);	// RGB(0, 0, 255)
			_pointElem.Locked = true;
		}

		private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_obstaclesPage3 == null)
				return;

			if (e.RowIndex < 0 || _obstaclesPage3.Length == 0)
				return;

			Graphics.DeleteElement(_pointElem);
			ObstacleData obs = (ObstacleData)dataGridView3.Rows[e.RowIndex].Tag;
			_pointElem = Graphics.DrawPointWithText(obs.pPtPrj, obs.ID.ToString(), 255);	// RGB(0, 0, 255)
			_pointElem.Locked = true;
		}

		private void dataGridView4_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_obstaclesPage4 == null)
				return;

			if (e.RowIndex < 0 || _obstaclesPage4.Length == 0)
				return;

			Graphics.DeleteElement(_pointElem);
			ObstacleData obs = (ObstacleData)dataGridView4.Rows[e.RowIndex].Tag;
			_pointElem = Graphics.DrawPointWithText(obs.pPtPrj, obs.ID.ToString(), 255);	// RGB(0, 0, 255)
			_pointElem.Locked = true;
		}

		private void dataGridView5_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_obstaclesPage5 == null)
				return;

			if (e.RowIndex < 0 || _obstaclesPage5.Length == 0)
				return;

			Graphics.DeleteElement(_pointElem);
			ObstacleData obs = (ObstacleData)dataGridView5.Rows[e.RowIndex].Tag;
			_pointElem = Graphics.DrawPointWithText(obs.pPtPrj, obs.ID.ToString(), 255);	// RGB(0, 0, 255)
			_pointElem.Locked = true;
		}

		private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int SelectedTab = mainTabControl.SelectedIndex;

			if (SelectedTab == _previousTab)
				return;

			if (this.Visible)
			{
				numericUpDown1.Visible = SelectedTab == 2 || SelectedTab == 3;
				label30.Visible = numericUpDown1.Visible;

				switch (SelectedTab)
				{
					case 0:
						if (dataGridView1.SelectedRows.Count > 0)
							dataGridView1_RowEnter(dataGridView1, new DataGridViewCellEventArgs(0, dataGridView1.SelectedRows[0].Index));
						break;
					case 1:
						if (dataGridView2.SelectedRows.Count > 0)
							dataGridView2_RowEnter(dataGridView2, new DataGridViewCellEventArgs(0, dataGridView2.SelectedRows[0].Index));
						break;
					case 2:
						if (numericUpDown1.Value != Page3Index + 1)
							FillPage3((int)numericUpDown1.Value - 1);
						/*else */if (dataGridView3.SelectedRows.Count > 0)
							dataGridView3_RowEnter(dataGridView3, new DataGridViewCellEventArgs(0, dataGridView3.SelectedRows[0].Index));
						break;
					case 3:
						if (numericUpDown1.Value != Page4Index + 1)
							FillPage4((int)numericUpDown1.Value - 1);
						/*else */if (dataGridView4.SelectedRows.Count > 0)
							dataGridView4_RowEnter(dataGridView4, new DataGridViewCellEventArgs(0, dataGridView4.SelectedRows[0].Index));
						break;
				}
			}
			_previousTab = SelectedTab;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{

		}
	}
}
