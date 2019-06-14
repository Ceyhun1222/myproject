using System;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.RNAV.Approach.Properties;
using System.Collections.Generic;
using Aran.PANDA.Constants;

namespace Aran.PANDA.RNAV.Approach
{
	public partial class ReportFrm : Form
	{
		//public const int MaxLegsCount = 20;

		//const int _helpContextID = 3700;
		#region declerations

		public ObstacleContainer obstaclesPage01;
		public ObstacleContainer obstaclesPage02;
		public ObstacleContainer obstaclesPage03;
		public ObstacleContainer obstaclesPage04;
		public ObstacleContainer obstaclesPage05;
		public ObstacleContainer obstaclesPage06;
		public ObstacleContainer obstaclesPage07;
		public ObstacleContainer obstaclesPage08;
		public ObstacleContainer obstaclesPage09;
		public ObstacleContainer obstaclesPage10;
		public ObstacleContainer obstaclesPage11;
		public ObstacleContainer obstaclesPage12;

		//private int _page4Count = 0;
		private int _page4Index = -1;

		public List<ObstacleContainer> obstaclesPage13;
		private List<ObstacleContainer> _DetObsPage13;

		private int _pointElem;
		private int _geomElem;
		private double _refElevation;
		private TabPage _previousTab;

		//private bool[] _pageVisible;
		//private TabPage[] _tapPages;

		private CheckBox _reportBtn;

		//private double MOC01;
		//private long Ix01ID;
		//private sbyte SortF01;

		public List<ObstacleContainer> ObstaclesP4
		{
			get { return obstaclesPage13; }
		}

		public int ObstacleLists
		{
			get { return obstaclesPage13.Count; }
		}
		#endregion


		#region Form
		public ReportFrm()
		{
			InitializeComponent();

			this.Text = Resources.str10000 + " " + GlobalVars.thisAssemName.Version.ToString();
			saveButton.Text = Resources.str00007;
			closeButton.Text = Resources.str00008;
			//_tapPages = new TabPage[] { tsCircklingArea, tsFinalApproach, tsIntermSignifi, tsFinalApproach };

			tsCircklingArea.Text = Resources.str10010;
			tsFSDF1.Text = Resources.str10014;
			tsFSDF2.Text = Resources.str10015;
			tsFinalApproach.Text = Resources.str10013;
			tsIntermSignifi.Text = Resources.str10012;
			tsISDF1.Text = Resources.str10016;
			tsISDF2.Text = Resources.str10017;
			tsIntermediateApproach.Text = Resources.str10011;
			tsStraightMissedApproach.Text = Resources.str10018;
			tsMA_TIA.Text = Resources.str10019;
			tsMAturnArea.Text = Resources.str10007;			//MA Turn area
			tsCurrSeg.Text = Resources.str10008;            //Current segment
			tsSegments.Text = Resources.str10009;			//Segments
			//Tab 1 =========================================================================
			dataGridView01.Columns[0].HeaderText = Resources.str10020;
			dataGridView01.Columns[1].HeaderText = Resources.str10021;
			dataGridView01.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView01.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView01.Columns[4].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView01.Columns[5].HeaderText = Resources.str10025;
			//Tab 2-4 =========================================================================
			dataGridView02.Columns[0].HeaderText = Resources.str10020;
			dataGridView02.Columns[1].HeaderText = Resources.str10021;
			dataGridView02.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView02.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView02.Columns[4].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView02.Columns[5].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView02.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView02.Columns[7].HeaderText = Resources.str10029;
			dataGridView02.Columns[8].HeaderText = Resources.str10030;

			dataGridView03.Columns[0].HeaderText = Resources.str10020;
			dataGridView03.Columns[1].HeaderText = Resources.str10021;
			dataGridView03.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView03.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView03.Columns[4].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView03.Columns[5].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView03.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView03.Columns[7].HeaderText = Resources.str10029;
			dataGridView03.Columns[8].HeaderText = Resources.str10030;

			dataGridView04.Columns[0].HeaderText = Resources.str10020;
			dataGridView04.Columns[1].HeaderText = Resources.str10021;
			dataGridView04.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView04.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView04.Columns[4].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView04.Columns[5].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView04.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView04.Columns[7].HeaderText = Resources.str10029;
			dataGridView04.Columns[8].HeaderText = Resources.str10030;
			//Tab 5-8 =======================================================================
			dataGridView05.Columns[0].HeaderText = Resources.str10020;
			dataGridView05.Columns[1].HeaderText = Resources.str10021;
			dataGridView05.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView05.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView05.Columns[4].HeaderText = Resources.str10038 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView05.Columns[5].HeaderText = Resources.str10026 + " (" + GlobalVars.unitConverter.GradientUnit + ")";
			dataGridView05.Columns[6].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView05.Columns[7].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView05.Columns[8].HeaderText = Resources.str10029;

			dataGridView06.Columns[0].HeaderText = Resources.str10020;
			dataGridView06.Columns[1].HeaderText = Resources.str10021;
			dataGridView06.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView06.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView06.Columns[4].HeaderText = Resources.str10038 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView06.Columns[5].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView06.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView06.Columns[7].HeaderText = Resources.str10029;
			dataGridView06.Columns[8].HeaderText = Resources.str10030;

			dataGridView07.Columns[0].HeaderText = Resources.str10020;
			dataGridView07.Columns[1].HeaderText = Resources.str10021;
			dataGridView07.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView07.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView07.Columns[4].HeaderText = Resources.str10038 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView07.Columns[5].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView07.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView07.Columns[7].HeaderText = Resources.str10029;
			dataGridView07.Columns[8].HeaderText = Resources.str10030;

			dataGridView08.Columns[0].HeaderText = Resources.str10020;
			dataGridView08.Columns[1].HeaderText = Resources.str10021;
			dataGridView08.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView08.Columns[3].HeaderText = Resources.str10023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView08.Columns[4].HeaderText = Resources.str10038 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView08.Columns[5].HeaderText = Resources.str10027 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView08.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView08.Columns[7].HeaderText = Resources.str10029;
			dataGridView08.Columns[8].HeaderText = Resources.str10030;
			//Tab 9 ===================================================================================================================
			dataGridView09.Columns[0].HeaderText = Resources.str10020;
			dataGridView09.Columns[1].HeaderText = Resources.str10021;
			dataGridView09.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView09.Columns[3].HeaderText = Resources.str10031 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView09.Columns[4].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView09.Columns[5].HeaderText = Resources.str10036 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			//dataGridView09.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView09.Columns[6].HeaderText = Resources.str10029;
			//dataGridView09.Columns[8].HeaderText = Resources.str10030;

			//Tab 10 ===================================================================================================================
			dataGridView10.Columns[0].HeaderText = Resources.str10020;
			dataGridView10.Columns[1].HeaderText = Resources.str10021;
			dataGridView10.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView10.Columns[3].HeaderText = Resources.str10035 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView10.Columns[4].HeaderText = Resources.str10032 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView10.Columns[5].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView10.Columns[6].HeaderText = Resources.str10033 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView10.Columns[7].HeaderText = Resources.str10036 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			//dataGridView10.Columns[8].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView10.Columns[8].HeaderText = Resources.str10029;
			//dataGridView10.Columns[10].HeaderText = Resources.str10030;

			//Tab 11 ===================================================================================================================
			dataGridView11.Columns[0].HeaderText = Resources.str10020;
			dataGridView11.Columns[1].HeaderText = Resources.str10021;
			dataGridView11.Columns[2].HeaderText = Resources.str10022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView11.Columns[3].HeaderText = Resources.str10032 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView11.Columns[4].HeaderText = Resources.str10024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			dataGridView11.Columns[5].HeaderText = Resources.str10037 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			//dataGridView11.Columns[6].HeaderText = Resources.str10028 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";
			dataGridView11.Columns[6].HeaderText = Resources.str10029;
			//dataGridView11.Columns[8].HeaderText = Resources.str10030;
			//=======================================================================================================================
			dataGridView12.Columns[0].HeaderText = Resources.str10020;
			dataGridView12.Columns[1].HeaderText = Resources.str10021;
			dataGridView12.Columns[2].HeaderText = Resources.str10042 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView12.Columns[3].HeaderText = Resources.str10043 + GlobalVars.unitConverter.DistanceUnit;
			dataGridView12.Columns[4].HeaderText = Resources.str03025 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView12.Columns[5].HeaderText = Resources.str10022 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView12.Columns[6].HeaderText = Resources.str10023 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView12.Columns[7].HeaderText = Resources.str10044 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView12.Columns[8].HeaderText = Resources.str10045 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView12.Columns[9].HeaderText = Resources.str10029;

			dataGridView13.Columns[0].HeaderText = Resources.str10020;
			dataGridView13.Columns[1].HeaderText = Resources.str10021;
			dataGridView13.Columns[2].HeaderText = Resources.str10042 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView13.Columns[3].HeaderText = Resources.str10043 + GlobalVars.unitConverter.DistanceUnit;
			dataGridView13.Columns[4].HeaderText = Resources.str03025 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView13.Columns[5].HeaderText = Resources.str10022 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView13.Columns[6].HeaderText = Resources.str10023 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView13.Columns[7].HeaderText = Resources.str10044 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView13.Columns[8].HeaderText = Resources.str10045 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView13.Columns[9].HeaderText = Resources.str10029;

			//============================================================================

			_pointElem = -1;
			_geomElem = -1;

			mainTabControl.Controls.Remove(tsFSDF1);
			mainTabControl.Controls.Remove(tsFSDF2);
			mainTabControl.Controls.Remove(tsISDF1);
			mainTabControl.Controls.Remove(tsISDF2);


			obstaclesPage13 = new List< ObstacleContainer>();
			_DetObsPage13 = new List<ObstacleContainer>();

			/** /
			mainTabControl.Controls.Clear();
			_tapPages[0].Text = Resources.str11000;
			_tapPages[1].Text = Resources.str12000;
			/**/

			//_pageVisible = new bool[_tapPages.Length];
		}

		private void Report_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
		}

		private void Report_FormClosing(object sender, FormClosingEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
				_reportBtn.Checked = false;
			}
		}

		private void Report_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
				e.Handled = true;
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Functions.GetSystemMenu(this.Handle, false);
			// Add a separator
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		#endregion

		#region Routines
		internal void Init(CheckBox reportBtn)          //, int HelpContext = 0
		{
			_reportBtn = reportBtn;                     //_helpContextID = HelpContext;
		}

		internal void SetRefElev(double refElev)
		{
			_refElevation = refElev;
		}

		//public void SetTabVisible(int index, bool visible)
		//{
		//	int x;

		//	if (visible)
		//		mainTabControl.Controls.Clear();

		//	if (index < 0)
		//	{
		//		for (int i = 0; i < _tapPages.Length; i++)
		//		{
		//			_pageVisible[i] = visible;

		//			if (visible)
		//				mainTabControl.Controls.Add(_tapPages[i]);
		//			else if (mainTabControl.Controls.Contains(_tapPages[i]))
		//				mainTabControl.Controls.Remove(_tapPages[i]);
		//		}
		//	}
		//	else if (index < _tapPages.Length)
		//	{
		//		_pageVisible[index] = visible;

		//		if (visible)
		//		{
		//			for (int i = 0; i < _tapPages.Length; i++)
		//				if (_pageVisible[i])
		//					mainTabControl.Controls.Add(_tapPages[i]);

		//			x = mainTabControl.Controls.GetChildIndex(_tapPages[index]);
		//			mainTabControl.SelectedIndex = x;
		//		}
		//		else if (mainTabControl.Controls.Contains(_tapPages[index]))
		//			mainTabControl.Controls.Remove(_tapPages[index]);
		//	}
		//}

		static IDictionary<int, int> GetUnicalObstales(ObstacleContainer obstacleList, bool skipExludes = false)
		{
			IDictionary<int, int> result = new Dictionary<int, int>();

			int i, n = obstacleList.Parts.Length;

			for (i = 0; i < n; i++)
			{
				if (skipExludes && obstacleList.Parts[i].IsExcluded)
					continue;

				int owner = obstacleList.Parts[i].Owner;

				if (!result.ContainsKey(owner))
					result.Add(owner, i);
			}

			return result;
		}

		private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
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

			e.Handled = true;
		}

		public string GetTabPageText(int index)
		{
			return mainTabControl.TabPages[index].Text;
		}

		int GetIndex(ObstacleData[] Obstacles, ObstacleData TestObs)
		{
			int i, n = Obstacles.Length;

			for (i = 0; i < n; i++)
			{
				double dist = Math.Abs(Obstacles[i].pPtPrj.X - TestObs.pPtPrj.X) + Math.Abs(Obstacles[i].pPtPrj.Y - TestObs.pPtPrj.Y);
				if (dist <= ARANMath.Epsilon_2Distance)
					return i;
			}

			return -1;
		}

		#endregion

		#region Controls events
		private void HelpBtn_Click(object sender, EventArgs e)
		{
			//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
		}

		private void saveButton_Click(object sender, EventArgs e)
		{

		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
			//GlobalVars.gAranGraphics.Refresh();

			_reportBtn.Checked = false;
			Hide();
		}

		private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPage SelectedTab = mainTabControl.SelectedTab;

			if (SelectedTab == _previousTab)
				return;

			_previousTab = SelectedTab;

			int cnt = 0;

			if (this.Visible)
			{
				numericUpDown1.Visible = SelectedTab == tsSegments;

				int rowindex;
				if (SelectedTab == tsCircklingArea)
				{
					if (obstaclesPage01.Obstacles != null)
						cnt = obstaclesPage01.Obstacles.Length;

					if (dataGridView01.SelectedRows.Count > 0)
					{
						rowindex = dataGridView01.SelectedRows[0].Index;
						dataGridView01_RowEnter(dataGridView01, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsFSDF1)
				{
					if (obstaclesPage02.Obstacles != null)
						cnt = obstaclesPage02.Obstacles.Length;

					if (dataGridView02.SelectedRows.Count > 0)
					{
						rowindex = dataGridView02.SelectedRows[0].Index;
						dataGridView02_RowEnter(dataGridView02, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsFSDF2)
				{
					if (obstaclesPage03.Obstacles != null)
						cnt = obstaclesPage03.Obstacles.Length;

					if (dataGridView03.SelectedRows.Count > 0)
					{
						rowindex = dataGridView03.SelectedRows[0].Index;
						dataGridView03_RowEnter(dataGridView03, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsFinalApproach)
				{
					if (obstaclesPage04.Obstacles != null)
						cnt = obstaclesPage04.Obstacles.Length;

					if (dataGridView04.SelectedRows.Count > 0)
					{
						rowindex = dataGridView04.SelectedRows[0].Index;
						dataGridView04_RowEnter(dataGridView04, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsIntermSignifi)
				{
					if (obstaclesPage05.Obstacles != null)
						cnt = obstaclesPage05.Obstacles.Length;

					if (dataGridView05.SelectedRows.Count > 0)
					{
						rowindex = dataGridView05.SelectedRows[0].Index;
						dataGridView05_RowEnter(dataGridView05, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsISDF1)
				{
					if (obstaclesPage06.Obstacles != null)
						cnt = obstaclesPage06.Obstacles.Length;

					if (dataGridView06.SelectedRows.Count > 0)
					{
						rowindex = dataGridView06.SelectedRows[0].Index;
						dataGridView06_RowEnter(dataGridView06, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsISDF2)
				{
					if (obstaclesPage07.Obstacles != null)
						cnt = obstaclesPage07.Obstacles.Length;

					if (dataGridView07.SelectedRows.Count > 0)
					{
						rowindex = dataGridView07.SelectedRows[0].Index;
						dataGridView07_RowEnter(dataGridView07, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsIntermediateApproach)
				{
					if (obstaclesPage08.Obstacles != null)
						cnt = obstaclesPage08.Obstacles.Length;

					if (dataGridView08.SelectedRows.Count > 0)
					{
						rowindex = dataGridView08.SelectedRows[0].Index;
						dataGridView08_RowEnter(dataGridView08, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsStraightMissedApproach)
				{
					if (obstaclesPage09.Obstacles != null)
						cnt = obstaclesPage09.Obstacles.Length;

					if (dataGridView09.SelectedRows.Count > 0)
					{
						rowindex = dataGridView09.SelectedRows[0].Index;
						dataGridView09_RowEnter(dataGridView09, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsMA_TIA)
				{
					if (obstaclesPage10.Obstacles != null)
						cnt = obstaclesPage10.Obstacles.Length;

					if (dataGridView10.SelectedRows.Count > 0)
					{
						rowindex = dataGridView10.SelectedRows[0].Index;
						dataGridView10_RowEnter(dataGridView10, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsMAturnArea)
				{
					if (obstaclesPage11.Obstacles != null)
						cnt = obstaclesPage11.Obstacles.Length;

					if (dataGridView11.SelectedRows.Count > 0)
					{
						rowindex = dataGridView11.SelectedRows[0].Index;
						dataGridView11_RowEnter(dataGridView11, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsCurrSeg)
				{
					if (obstaclesPage12.Obstacles != null)
						cnt = obstaclesPage12.Obstacles.Length;

					if (dataGridView12.SelectedRows.Count > 0)
					{
						rowindex = dataGridView12.SelectedRows[0].Index;
						dataGridView12_RowEnter(dataGridView12, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}
				else if (SelectedTab == tsSegments)
				{
					if (_page4Index>=0 && obstaclesPage13[_page4Index].Obstacles != null)
						cnt = obstaclesPage13[_page4Index].Obstacles.Length;

					if (dataGridView13.SelectedRows.Count > 0)
					{
						rowindex = dataGridView13.SelectedRows[0].Index;
						dataGridView13_RowEnter(dataGridView13, new DataGridViewCellEventArgs(1, rowindex));
					}
					else
					{
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
						GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
					}
				}

				//lblCount.Text = Resources.str10001 + cnt;
			}

			lblCount.Text = Resources.str10001 + cnt;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			FillPage4((int)numericUpDown1.Value - 1);
		}
		#endregion

		internal void FillPage01(ObstacleContainer Obstacles, double MOC, int Ix = -1)
		{
			int n = Obstacles.Obstacles.Length;
			if (mainTabControl.SelectedTab == tsCircklingArea)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + n;
			}

			dataGridView01.RowCount = 0;

			obstaclesPage01.Obstacles = new Obstacle[n];

			if (n == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage01.Obstacles, n);

			dataGridView01.SuspendLayout();

			for (int i = 0; i < n; i++)
			{
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage01.Obstacles[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[i].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[i].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(MOC, eRoundMode.NEAREST).ToString();

				double ReqOCH = Obstacles.Obstacles[i].Height + MOC - _refElevation;
				if (ReqOCH < MOC)
					ReqOCH = MOC;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(ReqOCH, eRoundMode.NEAREST).ToString();

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[i].HorAccuracy, eRoundMode.NEAREST).ToString();

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[i].VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				var obstacleGeoType = Obstacles.Obstacles[i].pGeomPrj.Type;
				if (obstacleGeoType == Geometries.GeometryType.Point)
					row.Cells[5].Value = Resources.str10002;
				else if (obstacleGeoType == Geometries.GeometryType.LineString || obstacleGeoType == Geometries.GeometryType.MultiLineString)
					row.Cells[5].Value = Resources.str10003;
				else
					row.Cells[5].Value = Resources.str10004;

				if (i == Ix)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
					row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}
				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				dataGridView01.Rows.Add(row);
			}

			dataGridView01.ResumeLayout();
			//st.Stop();
			//MessageBox.Show(st.Elapsed.ToString());
			//SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void RemovePage02()
		{
			mainTabControl.Controls.Remove(tsFSDF1);
		}

		internal void FillPage02(ObstacleContainer Obstacles)
		{
			if (!mainTabControl.Controls.Contains(tsFSDF1))
			{
				bool interSDF = mainTabControl.Controls.Contains(tsISDF1);
				mainTabControl.Controls.Remove(tsIntermediateApproach);
				if (interSDF)
					mainTabControl.Controls.Remove(tsISDF1);

				mainTabControl.Controls.Remove(tsIntermSignifi);
				mainTabControl.Controls.Remove(tsFinalApproach);
				mainTabControl.Controls.Add(tsFSDF1);
				mainTabControl.Controls.Add(tsFinalApproach);
				mainTabControl.Controls.Add(tsIntermSignifi);
				if (interSDF)
					mainTabControl.Controls.Add(tsISDF1);

				mainTabControl.Controls.Add(tsIntermediateApproach);
			}

			int m = 0;
			if (Obstacles.Parts != null)
				m = Obstacles.Parts.Length;

			if (mainTabControl.SelectedTab == tsFSDF1)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView02.RowCount = 0;

			int n = 0;
			if (Obstacles.Obstacles != null)
				n = Obstacles.Obstacles.Length;
			obstaclesPage02.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage02.Obstacles, n);

			obstaclesPage02.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage02.Parts, m);

			Array.Sort(obstaclesPage02.Parts, Comparers.PartComparerReqH);
			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage02);

			dataGridView02.SuspendLayout();

			if (mainTabControl.SelectedTab == tsFSDF1)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage02.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

				//cell.Value = _obstaclesPage3.Obstacles[_obstaclesPage3.Parts[i].Owner].TypeName;
				//row.Cells.Add(cell);
				//cell = new DataGridViewTextBoxCell();
				//cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				//row.Cells.Add(cell);

				cell.Value = obstaclesPage02.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage02.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage02.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage02.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage02.Parts[i].ReqH).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage02.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage02.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage02.Parts[i].Prima)
					row.Cells[7].Value = Resources.str10040;
				else
					row.Cells[7].Value = Resources.str10041;

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage02.Parts[i].Ignored)
					row.Cells[8].Value = Resources.str10046;
				else
					row.Cells[8].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView02.Rows.Add(row);
			}

			dataGridView02.ResumeLayout();
			//	st.Stop();
			//	MessageBox.Show(st.Elapsed.ToString());
			//SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void RemovePage03()
		{
			mainTabControl.Controls.Remove(tsFSDF2);
		}

		internal void FillPage03(ObstacleContainer Obstacles)
		{
			if (!mainTabControl.Controls.Contains(tsFSDF2))
			{
				bool interSDF = mainTabControl.Controls.Contains(tsISDF1);
				mainTabControl.Controls.Remove(tsIntermediateApproach);
				if (interSDF)
					mainTabControl.Controls.Remove(tsISDF1);

				mainTabControl.Controls.Remove(tsIntermSignifi);
				mainTabControl.Controls.Remove(tsFinalApproach);
				mainTabControl.Controls.Add(tsFSDF2);
				mainTabControl.Controls.Add(tsFinalApproach);
				mainTabControl.Controls.Add(tsIntermSignifi);

				if (interSDF)
					mainTabControl.Controls.Add(tsISDF1);

				mainTabControl.Controls.Add(tsIntermediateApproach);
			}

			int m = 0;
			if (Obstacles.Parts != null)
				m = Obstacles.Parts.Length;

			if (mainTabControl.SelectedTab == tsFSDF2)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView03.RowCount = 0;

			int n = 0;
			if (Obstacles.Obstacles != null)
				n = Obstacles.Obstacles.Length;

			obstaclesPage03.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage03.Obstacles, n);

			obstaclesPage03.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage03.Parts, m);

			Array.Sort(obstaclesPage03.Parts, Comparers.PartComparerReqH);
			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage03);

			dataGridView03.SuspendLayout();

			if (mainTabControl.SelectedTab == tsFSDF2)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage03.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

				cell.Value = obstaclesPage03.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage03.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage03.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage03.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage03.Parts[i].ReqH).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage03.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage03.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage03.Parts[i].Prima)
					row.Cells[7].Value = Resources.str10040;
				else
					row.Cells[7].Value = Resources.str10041;

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage03.Parts[i].Ignored)
					row.Cells[8].Value = Resources.str10046;
				else
					row.Cells[8].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView03.Rows.Add(row);
			}

			dataGridView03.ResumeLayout();

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void FillPage04(ObstacleContainer Obstacles)
		{
			//if (!mainTabControl.Controls.Contains(tsFinalApproach))
			//	mainTabControl.Controls.Add(tsFinalApproach);

			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsFinalApproach)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView04.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage04.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage04.Obstacles, n);

			obstaclesPage04.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage04.Parts, m);

			Array.Sort(obstaclesPage04.Parts, Comparers.PartComparerReqH);
			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage04);

			dataGridView04.SuspendLayout();

			if (mainTabControl.SelectedTab == tsFinalApproach)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage04.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

				cell.Value = obstaclesPage04.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage04.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage04.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage04.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage04.Parts[i].ReqH).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage04.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage04.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage04.Parts[i].Prima)
					row.Cells[7].Value = Resources.str10040;
				else
					row.Cells[7].Value = Resources.str10041;

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage04.Parts[i].Ignored)
					row.Cells[8].Value = Resources.str10046;
				else
					row.Cells[8].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView04.Rows.Add(row);
			}

			dataGridView04.ResumeLayout();

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void FillPage05(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsIntermSignifi)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView05.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage05.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage05.Obstacles, n);

			obstaclesPage05.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage05.Parts, m);

			//Array.Sort(_obstaclesPage2.Parts, Comparers.PartComparerReqH);
			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage05);

			dataGridView05.SuspendLayout();

			if (mainTabControl.SelectedTab == tsIntermSignifi)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage05.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage05.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage05.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage05.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage05.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage05.Parts[i].ReqH + _refElevation).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.GradientToDisplayUnits(obstaclesPage05.Parts[i].PDG).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage05.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage05.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage05.Parts[i].Prima)
					row.Cells[8].Value = Resources.str10040;
				else
					row.Cells[8].Value = Resources.str10041;

				if (firstItem)
				{
					firstItem = false;
					row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
					row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				dataGridView05.Rows.Add(row);
			}

			dataGridView05.ResumeLayout();
			//	st.Stop();
			//	MessageBox.Show(st.Elapsed.ToString());
			//SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void RemovePage06()
		{
			//if (mainTabControl.Controls.Contains(tsISDF1))
			mainTabControl.Controls.Remove(tsISDF1);
		}

		internal void FillPage06(ObstacleContainer Obstacles)
		{
			if (!mainTabControl.Controls.Contains(tsISDF1))
			{
				mainTabControl.Controls.Remove(tsIntermediateApproach);
				mainTabControl.Controls.Add(tsISDF1);
				mainTabControl.Controls.Add(tsIntermediateApproach);
			}

			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsISDF1)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView06.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage06.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage06.Obstacles, n);

			obstaclesPage06.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage06.Parts, m);

			Array.Sort(obstaclesPage06.Parts, Comparers.PartComparerReqH);
			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage06);

			dataGridView06.SuspendLayout();

			if (mainTabControl.SelectedTab == tsISDF1)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage06.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

				//cell.Value = _obstaclesPage6.Obstacles[_obstaclesPage6.Parts[i].Owner].TypeName;
				//row.Cells.Add(cell);
				//cell = new DataGridViewTextBoxCell();
				//cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				//row.Cells.Add(cell);

				cell.Value = obstaclesPage06.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage06.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage06.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage06.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage06.Parts[i].ReqH + _refElevation).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage06.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage06.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage06.Parts[i].Prima)
					row.Cells[7].Value = Resources.str10040;
				else
					row.Cells[7].Value = Resources.str10041;

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage06.Parts[i].Ignored)
					row.Cells[8].Value = Resources.str10046;
				else
					row.Cells[8].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView06.Rows.Add(row);
			}

			dataGridView06.ResumeLayout();
			//	st.Stop();
			//	MessageBox.Show(st.Elapsed.ToString());
			//SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void RemovePage07()
		{
			//if (mainTabControl.Controls.Contains(tsISDF2))
			mainTabControl.Controls.Remove(tsISDF2);
		}

		internal void FillPage07(ObstacleContainer Obstacles)
		{
			if (!mainTabControl.Controls.Contains(tsISDF2))
			{
				mainTabControl.Controls.Remove(tsIntermediateApproach);
				mainTabControl.Controls.Add(tsISDF2);
				mainTabControl.Controls.Add(tsIntermediateApproach);
			}

			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsISDF2)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView07.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage07.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage07.Obstacles, n);

			obstaclesPage07.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage07.Parts, m);

			Array.Sort(obstaclesPage07.Parts, Comparers.PartComparerReqH);
			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage07);

			dataGridView07.SuspendLayout();

			if (mainTabControl.SelectedTab == tsISDF2)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;
				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage07.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

				//cell.Value = _obstaclesPage7.Obstacles[_obstaclesPage7.Parts[i].Owner].TypeName;
				//row.Cells.Add(cell);
				//cell = new DataGridViewTextBoxCell();
				//cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				//row.Cells.Add(cell);

				cell.Value = obstaclesPage07.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage07.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage07.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage07.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage07.Parts[i].ReqH + _refElevation).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage07.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage07.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage07.Parts[i].Prima)
					row.Cells[7].Value = Resources.str10040;
				else
					row.Cells[7].Value = Resources.str10041;

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage07.Parts[i].Ignored)
					row.Cells[8].Value = Resources.str10046;
				else
					row.Cells[8].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView07.Rows.Add(row);
			}

			dataGridView07.ResumeLayout();
			//	st.Stop();
			//	MessageBox.Show(st.Elapsed.ToString());
			//SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		internal void FillPage08(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsIntermediateApproach)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView08.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage08.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage08.Obstacles, n);

			obstaclesPage08.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage08.Parts, m);

			Array.Sort(obstaclesPage08.Parts, Comparers.PartComparerReqH);

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage08);

			dataGridView08.SuspendLayout();

			if (mainTabControl.SelectedTab == tsIntermediateApproach)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			bool firstItem = true;
			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage08.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage08.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage08.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage08.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage08.Parts[i].MOC).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage08.Parts[i].ReqH + _refElevation).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage08.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage08.Parts[i].CLShift).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage08.Parts[i].Prima)
					row.Cells[7].Value = Resources.str10040;
				else
					row.Cells[7].Value = Resources.str10041;

				row.Cells.Add(new DataGridViewTextBoxCell());
				if (obstaclesPage08.Parts[i].Ignored)
					row.Cells[8].Value = Resources.str10046;
				else
					row.Cells[8].Value = Resources.str10047;

				if (firstItem)
				{
					firstItem = false;
					row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
					row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				dataGridView08.Rows.Add(row);
			}

			dataGridView08.ResumeLayout();
			// st.Stop();
			// MessageBox.Show(st.Elapsed.ToString());
			// SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void AddMissedApproach(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsStraightMissedApproach)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView09.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage09.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage09.Obstacles, n);

			obstaclesPage09.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage09.Parts, m);

			Array.Sort(obstaclesPage09.Parts, Comparers.PartComparerReqH);

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage09);

			dataGridView09.SuspendLayout();

			if (mainTabControl.SelectedTab == tsStraightMissedApproach)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage09.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage09.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage09.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage09.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(fMOC30 * obstaclesPage09.Parts[i].fSecCoeff).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				//row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage09.Parts[i].ReqH).ToString();
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage09.Parts[i].ReqOCH).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage09.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage09.Parts[i].Prima)
					row.Cells[6].Value = Resources.str10040;
				else
					row.Cells[6].Value = Resources.str10041;

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//if (_obstaclesPage09.Parts[i].Ignored)
				//	row.Cells[8].Value = Resources.str10046;
				//else
				//	row.Cells[8].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				//row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				dataGridView09.Rows.Add(row);
			}

			dataGridView09.ResumeLayout();
			// st.Stop();
			// MessageBox.Show(st.Elapsed.ToString());
			// SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillMA_TurnInitiationArea(ObstacleContainer Obstacles, string tabText)
		{
			tsMA_TIA.Text = tabText;

			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsMA_TIA)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView10.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage10.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage10.Obstacles, n);

			obstaclesPage10.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage10.Parts, m);

			Array.Sort(obstaclesPage10.Parts, Comparers.PartComparerReqH);

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage10);

			dataGridView10.SuspendLayout();

			if (mainTabControl.SelectedTab == tsMA_TIA)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;

				double ReqOCH = obstaclesPage10.Parts[i].ReqOCH;
				if (ReqOCH < 0)
					ReqOCH = 0.0;

				double obMOC50 = fMOC50 * obstaclesPage10.Parts[i].fSecCoeff;
				double fTA = obstaclesPage10.Parts[i].Elev + obMOC50;

				double fTmp = ReqOCH - obstaclesPage10.Parts[i].Height;
				if (fTmp < 0.0)
					fTmp = 0.0;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage10.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage10.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage10.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage10.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obMOC50).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(ReqOCH).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(fTA).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage10.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage10.Parts[i].Prima)
					row.Cells[8].Value = Resources.str10040;
				else
					row.Cells[8].Value = Resources.str10041;

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//if (_obstaclesPage10.Parts[i].Ignored)
				//	row.Cells[9].Value = Resources.str10046;
				//else
				//	row.Cells[9].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				//row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				dataGridView10.Rows.Add(row);
			}

			dataGridView10.ResumeLayout();
			// st.Stop();
			// MessageBox.Show(st.Elapsed.ToString());
			// SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillMA_TurnArea(ObstacleContainer Obstacles)
		{
			int m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsMAturnArea)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView11.RowCount = 0;

			int n = Obstacles.Obstacles.Length;
			obstaclesPage11.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage11.Obstacles, n);

			obstaclesPage11.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage11.Parts, m);

			Array.Sort(obstaclesPage11.Parts, Comparers.PartComparerReqH);

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage11);

			dataGridView11.SuspendLayout();

			if (mainTabControl.SelectedTab == tsMAturnArea)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;

				double ReqOCH = obstaclesPage11.Parts[i].ReqOCH;
				if (ReqOCH < 0)
					ReqOCH = 0.0;

				double obMOC50 = fMOC50 * obstaclesPage11.Parts[i].fSecCoeff;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage11.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage11.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage11.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage11.Parts[i].Elev).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obMOC50).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(ReqOCH).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage11.Parts[i].Dist).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());

				if (obstaclesPage11.Parts[i].Prima)
					row.Cells[6].Value = Resources.str10040;
				else
					row.Cells[6].Value = Resources.str10041;

				//row.Cells.Add(new DataGridViewTextBoxCell());
				//if (_obstaclesPage10.Parts[i].Ignored)
				//	row.Cells[9].Value = Resources.str10046;
				//else
				//	row.Cells[9].Value = Resources.str10047;

				//if (firstItem)
				//{
				//	firstItem = false;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				//row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				dataGridView11.Rows.Add(row);
			}

			dataGridView11.ResumeLayout();
			// st.Stop();
			// MessageBox.Show(st.Elapsed.ToString());
			// SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillPageCurrPage(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			int i, n, m;
			/*==========================================================================*/
			m = Obstacles.Parts.Length;
			if (mainTabControl.SelectedTab == tsCurrSeg)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + m;
			}

			dataGridView12.RowCount = 0;

			n = Obstacles.Obstacles.Length;
			obstaclesPage12.Obstacles = new Obstacle[n];

			if (n == 0 || m == 0)
				return;

			Array.Copy(Obstacles.Obstacles, obstaclesPage12.Obstacles, n);

			obstaclesPage12.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, obstaclesPage12.Parts, m);

			Array.Sort(obstaclesPage12.Parts, Comparers.PartComparerReqH);

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(obstaclesPage12);

			dataGridView12.SuspendLayout();

			if (mainTabControl.SelectedTab == tsCurrSeg)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			int page4Count = obstaclesPage13.Count;

			if (page4Count > 0)
				for (i = 0; i < obstaclesPage13[page4Count - 1].Parts.Length; i++)
					obstaclesPage13[page4Count - 1].Parts[i].IsExcluded = GetIndex(Obstacles.Parts, obstaclesPage13[page4Count - 1].Parts[i]) >= 0;

			//double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			//bool firstItem = true;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				i = item.Value;
				obstaclesPage12.Parts[i].IsExcluded = false;

				string strPrima;
				if (obstaclesPage12.Parts[i].Prima)
					strPrima = Resources.str10040;
				else
					strPrima = Resources.str10041;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = obstaclesPage12.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage12.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = obstaclesPage12.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage12.Parts[i].d0, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstaclesPage12.Parts[i].DistStar, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage12.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage12.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage12.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage12.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstaclesPage12.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strPrima;

				//if (_obstaclesPage12.Obstacles[item.Key].ID == DetObs.Obstacles[0].ID)
				//{
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;            // System.Drawing.Color.FromArgb(0XFF0000);
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView12.Rows.Add(row);
			}

			dataGridView12.ResumeLayout();

			//SetTabVisible(2, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);

			//if(numericUpDown1.Value == _page4Index + 1)
			//	FillPage4((int)numericUpDown1.Value - 1);
			//if (_page4Index < 0)	_page4Index = 0;

			if (numericUpDown1.Value - 1 == _page4Index)
				FillPage4(_page4Index);

			/*==========================================================================* /

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			m = Obstacles.Obstacles.Length;
			n = Obstacles.Parts.Length;

			_obstaclesPage12.Obstacles = new Obstacle[m];
			_obstaclesPage12.Parts = new ObstacleData[n];

			dataGridView12.RowCount = 0;
			if (n <= 0)
				return;

			int page4Count = _page04Obstacles.Count;

			if (page4Count > 0)
				for (i = 0; i < _page04Obstacles[page4Count - 1].Parts.Length; i++)
					_page04Obstacles[page4Count - 1].Parts[i].IsExcluded = GetIndex(Obstacles.Parts, _page04Obstacles[page4Count - 1].Parts[i]) >= 0;

			for (i = 0; i < m; i++)
				_obstaclesPage12.Obstacles[i] = Obstacles.Obstacles[i];

			dataGridView12.SuspendLayout();

			for (i = 0; i < n; i++)
			{
				_obstaclesPage12.Parts[i] = Obstacles.Parts[i];
				_obstaclesPage12.Parts[i].IsExcluded = false;

				string strPrima;
				if (_obstaclesPage12.Parts[i].Prima)
					strPrima = Resources.str10040;
				else
					strPrima = Resources.str10041;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage12.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage12.Obstacles[_obstaclesPage12.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage12.Obstacles[_obstaclesPage12.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_obstaclesPage12.Parts[i].d0, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_obstaclesPage12.Parts[i].DistStar, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage12.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage12.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage12.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage12.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.GradientToDisplayUnits(_obstaclesPage12.Parts[i].PDG, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strPrima;

				//if (_page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
				//{
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;            // System.Drawing.Color.FromArgb(0XFF0000);
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView12.Rows.Add(row);
			}

			dataGridView12.ResumeLayout();

			//SetTabVisible(2, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);

			//if(numericUpDown1.Value == _page4Index + 1)
			//	FillPage4((int)numericUpDown1.Value - 1);
			//if (_page4Index < 0)	_page4Index = 0;

			if (numericUpDown1.Value - 1 == _page4Index)
				FillPage4(_page4Index);
			/*===============================================================================*/
		}

		public void AddPage4(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			//if (_page4Count > MaxLegsCount)
			//	throw new Exception("Maximum legs count exceeded.");

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);		//??????
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);      //??????

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			ObstacleContainer newObst;
			newObst.Obstacles = new Obstacle[m];
			newObst.Parts = new ObstacleData[n];

			Array.Copy(Obstacles.Obstacles, newObst.Obstacles, m);
			Array.Copy(Obstacles.Parts, newObst.Parts, n);

			//_page04Obstacles[_page4Count].Obstacles = new Obstacle[m];
			//_page04Obstacles[_page4Count].Parts = new ObstacleData[n];

			//int i;
			//for (i = 0; i < m; i++)
			//	_page04Obstacles[_page4Count].Obstacles[i] = Obstacles.Obstacles[i];

			//for (i = 0; i < n; i++)
			//	_page04Obstacles[_page4Count].Parts[i] = Obstacles.Parts[i];

			obstaclesPage13.Add(newObst);
			_DetObsPage13.Add(DetObs);
			numericUpDown1.Maximum = obstaclesPage13.Count;

			if (_page4Index < 0)
				_page4Index = 0;

			FillPage4(_page4Index);

			//SetTabVisible(3, true);
		}

		public void RemoveLastLegP4()
		{
			//if (_page4Index < 0)
			//    throw new Exception("Legs index out of bounds.");
			//_page4Index = 0;        //????

			if (obstaclesPage13.Count > 0)
			{
				obstaclesPage13.RemoveAt(obstaclesPage13.Count - 1);
				_DetObsPage13.RemoveAt(_DetObsPage13.Count - 1);
			}

			if (_page4Index == obstaclesPage13.Count)
				_page4Index--;

			numericUpDown1.Maximum = obstaclesPage13.Count;

			//if (_page04Obstacles.Count == 0)		SetTabVisible(3, false);
		}

		public void SavePage4Obstacles(int LegNum, string TabComment, ReportFile protRep)
		{
			protRep.SaveObstacles(TabComment, dataGridView13, obstaclesPage13[LegNum], LegNum);
		}

		public void FillPage4(int LegNum)
		{
			_page4Index = LegNum;

			if (mainTabControl.SelectedTab == tsSegments)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
				lblCount.Text = Resources.str10048 + 0;
			}

			dataGridView13.RowCount = 0;

			if (_page4Index < 0)
				return;

			ObstacleContainer Obstacles = this.obstaclesPage13[_page4Index]; //_obstaclesPage13
			ObstacleContainer DetObs = _DetObsPage13[_page4Index];

			if (Obstacles.Obstacles == null)
				return;

			int m = Obstacles.Parts.Length;
			int n = Obstacles.Obstacles.Length;

			if (n == 0 || m == 0)
				return;
			/*========================================================*/

			ObstacleContainer _obstaclesPage13;
			_obstaclesPage13.Obstacles = new Obstacle[n];

			Array.Copy(Obstacles.Obstacles, _obstaclesPage13.Obstacles, n);

			_obstaclesPage13.Parts = new ObstacleData[m];
			Array.Copy(Obstacles.Parts, _obstaclesPage13.Parts, m);

			Array.Sort(_obstaclesPage13.Parts, Comparers.PartComparerReqH);

			IDictionary<int, int> unicalObstacleList = GetUnicalObstales(_obstaclesPage13, true);

			dataGridView13.SuspendLayout();

			if (mainTabControl.SelectedTab == tsSegments)
				lblCount.Text = Resources.str10048 + unicalObstacleList.Count;

			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

			foreach (KeyValuePair<int, int> item in unicalObstacleList)
			{
				int i = item.Value;
				//if (Obstacles.Parts[i].IsExcluded)					continue;

				string strPrima;
				if (Obstacles.Parts[i].Prima)
					strPrima = Resources.str10040;
				else
					strPrima = Resources.str10041;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage13.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage13.Obstacles[item.Key].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _obstaclesPage13.Obstacles[item.Key].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_obstaclesPage13.Parts[i].d0, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_obstaclesPage13.Parts[i].DistStar, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage13.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage13.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage13.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage13.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_obstaclesPage13.Parts[i].ReqOCH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strPrima;

				//if (_obstaclesPage13.Obstacles[item.Key].ID == DetObs.Obstacles[0].ID)
				//{
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView13.Rows.Add(row);
			}

			dataGridView13.ResumeLayout();

			/*========================================================* /

			dataGridView13.SuspendLayout();

			for (int i = 0; i < n; i++)
			{
				if (Obstacles.Parts[i].IsExcluded)
					continue;

				//string strCloseIn = Resources.str39014;
				string strCloseIn = "";
				if (LegNum == 0 && Obstacles.Parts[i].Ignored && Obstacles.Parts[i].PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
					strCloseIn = Resources.str10039;

				string strPrima;
				if (Obstacles.Parts[i].Prima)
					strPrima = Resources.str10040;
				else
					strPrima = Resources.str10041;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = Obstacles.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].d0, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].DistStar, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.GradientToDisplayUnits(Obstacles.Parts[i].PDG, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strCloseIn;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = strPrima;

				//if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
				//{
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView13.Rows.Add(row);
			}

			dataGridView13.ResumeLayout();
			/*=======================================================================================*/
			//SetTabVisible(4, true);
			//if (_reportBtn.Checked && !Visible)
			//	Show(GlobalVars.Win32Window);
		}



		private void dataGridView01_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage01.Obstacles.Length == 0 || !Visible)
				return;

			Obstacle owner = (Obstacle)dataGridView01.Rows[e.RowIndex].Tag;     // _obstaclesPage1.Obstacles[e.RowIndex];
			Geometry pGeometry = owner.pGeomPrj;
			Point pt = pGeometry.Centroid;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			AranEnvironment.Symbols.PointSymbol ps = new AranEnvironment.Symbols.PointSymbol(AranEnvironment.Symbols.ePointStyle.smsX, 255, 10);
			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pt, owner.UnicalName, ps);

			//_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pt, owner.UnicalName, 255);
		}

		private void dataGridView02_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage02.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView02.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage02.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView03_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage03.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView03.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage03.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView04_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage04.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView04.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage04.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView05_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage05.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView05.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage05.Obstacles[obstacleData.Owner];
			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView06_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage06.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView06.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage06.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView07_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage07.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView07.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage07.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView08_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage08.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView08.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage08.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView09_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage09.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView09.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage09.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView10_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage10.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView10.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage10.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView11_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage11.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView11.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage11.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView12_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (e.RowIndex < 0 || obstaclesPage12.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView12.Rows[e.RowIndex].Tag;
			Obstacle owner = obstaclesPage12.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}

		private void dataGridView13_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleContainer pageObstacles = obstaclesPage13[_page4Index]; //_obstaclesPage13

			if (e.RowIndex < 0 || pageObstacles.Obstacles.Length == 0 || !Visible)
				return;

			ObstacleData obstacleData = (ObstacleData)dataGridView13.Rows[e.RowIndex].Tag;
			Obstacle owner = pageObstacles.Obstacles[obstacleData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry,
					AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obstacleData.pPtPrj, owner.UnicalName, 255);
		}
	}
}
