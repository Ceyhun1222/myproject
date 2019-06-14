using System;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Departure.Properties;
using Aran.PANDA.Constants;
using System.Reflection;
using System.IO;
using Aran.Geometries;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class CReports : Form
	{
		public const int MaxLegsCount = 20;

		#region declerations

		private ObstacleContainer _page01Obstacles;
		private ObstacleContainer _page02Obstacles;
		private ObstacleContainer _page03Obstacles;
		private ObstacleContainer[] _page04Obstacles;
		private ObstacleContainer[] _DetObsPage4;

		private ObstacleContainer[] _page05Obstacles;
		private ObstacleContainer[] _DetObsPage5;

		private CheckBox _reportBtn;
		private bool[] _pageIsVisible;
		private TabPage[] _tabPages;

		private int _page4Count = 0;
		private int _page4Index = -1;

		private int _page5Count = 0;
		private int _page5Index = -1;

		private int _previousTab = -1;
		private int _helpContextID;
		private int _pointElem;
		private int _geomElem;

		public ObstacleContainer[] ObstaclesP4
		{
			get { return _page04Obstacles; }
		}

		public ObstacleContainer[] ObstaclesP5
		{
			get { return _page05Obstacles; }
		}

		public int ObstacleLists
		{
			get { return _page4Count; }
		}

		#endregion

		public CReports()
		{
			InitializeComponent();

			_tabPages = new TabPage[mainTabControl.TabPages.Count];
			for (int i = 0; i < mainTabControl.TabPages.Count; i++)
				_tabPages[i] = mainTabControl.TabPages[i];

			_pageIsVisible = new bool[mainTabControl.TabPages.Count];
			SetTabVisible(-1, false);

			_page04Obstacles = new ObstacleContainer[MaxLegsCount];
			_DetObsPage4 = new ObstacleContainer[MaxLegsCount];

			_page05Obstacles = new ObstacleContainer[MaxLegsCount];
			_DetObsPage5 = new ObstacleContainer[MaxLegsCount];

			Text = Resources.str40105;
			//this.Text = Resources.str40105 + "  " + Resources.str00034; // +"   " + MultiPage1.TabPages[0].Text;

			Label111.Text = Resources.str15087 + (GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value * 100.0).ToString() + Resources.str15088;

			//Label11.Text = GlobalVars.GetMapFileName();
			Label12.Text = Resources.str02001;

			_tabPages[0].Text = Resources.str15075; // Resources.str400;
			_tabPages[1].Text = Resources.str00410;
			_tabPages[2].Text = Resources.str00420;
			_tabPages[3].Text = Resources.str00430;
			_tabPages[4].Text = Resources.str00440;

			//_tabPages[5].Text = Resources.str00450;
			//dataGridView1.Columns[5].HeaderText = Resources.str40007;

			dataGridView1.Columns[0].HeaderText = Resources.str40000;
			dataGridView1.Columns[1].HeaderText = Resources.str40001;
			dataGridView1.Columns[2].HeaderText = Resources.str40002 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView1.Columns[3].HeaderText = Resources.str40003 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView1.Columns[4].HeaderText = Resources.str40005 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[5].HeaderText = Resources.str40011;
			dataGridView1.Columns[6].HeaderText = Resources.str40012;
			dataGridView1.Columns[7].HeaderText = Resources.str40031 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[8].HeaderText = Resources.str40008 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[9].HeaderText = Resources.str40009 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[10].HeaderText = Resources.str40013 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView1.Columns[11].HeaderText = Resources.str40014 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[12].HeaderText = Resources.str40030;
			dataGridView1.Columns[13].HeaderText = Resources.str40004 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[14].HeaderText = Resources.str40006 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView1.Columns[15].HeaderText = Resources.str15072;

			dataGridView2.Columns[0].HeaderText = Resources.str40000;
			dataGridView2.Columns[1].HeaderText = Resources.str40001;
			dataGridView2.Columns[2].HeaderText = Resources.str40002 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView2.Columns[3].HeaderText = Resources.str40003 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView2.Columns[4].HeaderText = Resources.str40005 + ", " + GlobalVars.unitConverter.HeightUnit;
			//dataGridView2.Columns[5].HeaderText = Resources.str40007;
			dataGridView2.Columns[5].HeaderText = Resources.str40011;
			dataGridView2.Columns[6].HeaderText = Resources.str40012;
			dataGridView2.Columns[7].HeaderText = Resources.str40031 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView2.Columns[8].HeaderText = Resources.str40008 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView2.Columns[9].HeaderText = Resources.str40009 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView2.Columns[10].HeaderText = Resources.str40013 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView2.Columns[11].HeaderText = Resources.str40014 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView2.Columns[12].HeaderText = Resources.str40030;
			dataGridView2.Columns[13].HeaderText = Resources.str40004 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView2.Columns[14].HeaderText = Resources.str40006 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView2.Columns[15].HeaderText = Resources.str15072;

			//============================================================================
			dataGridView3.Columns[0].HeaderText = Resources.str40000;
			dataGridView3.Columns[1].HeaderText = Resources.str40001;
			dataGridView3.Columns[2].HeaderText = Resources.str40032 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView3.Columns[3].HeaderText = Resources.str40035 + GlobalVars.unitConverter.DistanceUnit;
			dataGridView3.Columns[4].HeaderText = Resources.str40005 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView3.Columns[5].HeaderText = Resources.str40015 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView3.Columns[6].HeaderText = Resources.str40008 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView3.Columns[7].HeaderText = Resources.str40009 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView3.Columns[8].HeaderText = Resources.str40012;
			dataGridView3.Columns[9].HeaderText = Resources.str15072;

			dataGridView4.Columns[0].HeaderText = Resources.str40000;
			dataGridView4.Columns[1].HeaderText = Resources.str40001;
			dataGridView4.Columns[2].HeaderText = Resources.str40032 + ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView4.Columns[3].HeaderText = Resources.str40035 + GlobalVars.unitConverter.DistanceUnit;
			dataGridView4.Columns[4].HeaderText = Resources.str40005 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView4.Columns[5].HeaderText = Resources.str40015 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView4.Columns[6].HeaderText = Resources.str40008 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView4.Columns[7].HeaderText = Resources.str40009 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView4.Columns[8].HeaderText = Resources.str40012;
			dataGridView4.Columns[9].HeaderText = Resources.str40030;
			dataGridView4.Columns[10].HeaderText = Resources.str15072;

			dataGridView5.Columns[0].HeaderText = Resources.str40000;
			dataGridView5.Columns[1].HeaderText = Resources.str40001;
			dataGridView5.Columns[2].HeaderText = Resources.str40035 + GlobalVars.unitConverter.DistanceUnit;
			dataGridView5.Columns[3].HeaderText = Resources.str40005 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView5.Columns[4].HeaderText = Resources.str40015 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView5.Columns[5].HeaderText = Resources.str40008 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView5.Columns[6].HeaderText = Resources.str40009 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView5.Columns[7].HeaderText = Resources.str40016 + ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView5.Columns[8].HeaderText = Resources.str15072;

			saveButton.Text = Resources.str00007;
			closeButton.Text = Resources.str00008;

			mainTabControl.SelectedIndex = 0;
		}

		private void CReportsFrm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
				e.Handled = true;
			}
		}

		private void CReportsFrm_FormClosing(Object eventSender, FormClosingEventArgs eventArgs)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);

			if (eventArgs.CloseReason == CloseReason.UserClosing)
			{
				Hide();
				eventArgs.Cancel = true;
				_reportBtn.Checked = false;
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

		public void Init(CheckBox Btn, int HelpContext)
		{
			_reportBtn = Btn;
			_helpContextID = HelpContext;
		}

		public void SetTabVisible(int index, bool visible)
		{
			//int n = _tabPages.Length;

			//if (index < 0)
			//{
			//    if (visible)
			//    {
			//        for (int i = 0; i < n; i++)
			//        {
			//            if (mainTabControl.TabPages.IndexOf(_tabPages[i]) < 0)
			//                mainTabControl.TabPages.Add(_tabPages[i]);

			//            _pageIsVisible[i] = visible;
			//            //_pageButtons[i].Visible = visible;
			//        }
			//    }
			//    else
			//    {
			//        for (int i = 0; i < n; i++)
			//        {
			//            if (mainTabControl.TabPages.IndexOf(_tabPages[i]) >= 0)
			//                mainTabControl.TabPages.Remove(_tabPages[i]);

			//            _pageIsVisible[i] = visible;
			//            //_pageButtons[i].Visible = visible;
			//        }
			//        //mainTabControl.TabPages
			//    }
			//}
			//else if (index < n)
			//{
			//    if (visible)
			//    {
			//        if (mainTabControl.TabPages.IndexOf(_tabPages[index]) < 0)
			//            mainTabControl.TabPages.Add(_tabPages[index]);

			//        mainTabControl.SelectedIndex = mainTabControl.TabPages.IndexOf(_tabPages[index]);
			//    }
			//    else
			//    {
			//        if (mainTabControl.TabPages.IndexOf(_tabPages[index]) >= 0)
			//            mainTabControl.TabPages.Remove(_tabPages[index]);

			//    }
			//    _pageIsVisible[index] = visible;
			//    //_pageButtons[index].Visible = visible;
			//}

			//numericUpDown1.Visible = mainTabControl.SelectedIndex == 3;
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

		public void FillPage1(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page01Obstacles.Obstacles = new Obstacle[m];
			_page01Obstacles.Parts = new ObstacleData[n];

			dataGridView1.RowCount = 0;
			if (n <= 0)
				return;

			double NomPDG = 100.0 * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			int i;

			for (i = 0; i < m; i++)
				_page01Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
			{
				_page01Obstacles.Parts[i] = Obstacles.Parts[i];

				double tmpPDG = Math.Round(_page01Obstacles.Parts[i].PDG * 100.0 + 0.04999, 1);
				if (tmpPDG < NomPDG)
					tmpPDG = NomPDG;

				//string strIgnor = Resources.str39014;
				//if (_page01Obstacles.Parts[i].PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
				//{
				//	if (_page01Obstacles.Parts[i].Ignored)
				//		strIgnor = Resources.str40030;
				//	else
				//		strIgnor = Resources.str39015;
				//}

				string strCloseIn = "";		// Resources.str39014;
				if (_page01Obstacles.Parts[i].Ignored && _page01Obstacles.Parts[i].PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
					strCloseIn = Resources.str39015;

				string strPrima;
				if (_page01Obstacles.Parts[i].Prima)
					strPrima = Resources.str15080;
				else
					strPrima = Resources.str15081;

				//int ix = dataGridView1.Rows.Add();
				//dataGridView1.Rows[ix].Cells[0].Value = _obstaclesPage1[i].Name;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _page01Obstacles.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_page01Obstacles.Parts[i].Dist, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_page01Obstacles.Parts[i].CLShift, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				//=========================

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = Math.Round(_page01Obstacles.Parts[i].PDG * 100.0, 3).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = tmpPDG.ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].ReqTNH, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].ReqH, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(_page01Obstacles.Parts[i].NomPDGDist, eRoundMode.NONE), 1).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Parts[i].NomPDGHeight, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[12].Value = strCloseIn;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[13].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[14].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[15].Value = strPrima;

				if (_page01Obstacles.Obstacles[_page01Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
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

				dataGridView1.Rows.Add(row);
			}

			SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillPage2(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page02Obstacles.Obstacles = new Obstacle[m];
			_page02Obstacles.Parts = new ObstacleData[n];

			dataGridView2.RowCount = 0;
			if (n <= 0)
				return;

			double NomPDG = 100.0 * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			int i;

			for (i = 0; i < m; i++)
				_page02Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
			{
				_page02Obstacles.Parts[i] = Obstacles.Parts[i];

				double tmpPDG = Math.Round(_page02Obstacles.Parts[i].PDG * 100.0 + 0.04999, 1);
				if (tmpPDG < NomPDG)
					tmpPDG = NomPDG;

				//string strIgnor = Resources.str39014;
				//if (_page02Obstacles.Parts[i].PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
				//{
				//	if (_page02Obstacles.Parts[i].Ignored)
				//		strIgnor = Resources.str40030;
				//	else
				//		strIgnor = Resources.str39015;
				//}

				//string strCloseIn = Resources.str39014;
				string strCloseIn = "";
				if (_page02Obstacles.Parts[i].Ignored && _page02Obstacles.Parts[i].PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
					strCloseIn = Resources.str39015;

				string strPrima = Resources.str15081;

				if (_page02Obstacles.Parts[i].Prima)
					strPrima = Resources.str15080;

				//int ix = dataGridView2.Rows.Add();
				//dataGridView2.Rows[ix].Cells[0].Value = _obstaclesPage2[i].Name;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _page02Obstacles.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_page02Obstacles.Parts[i].Dist, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_page02Obstacles.Parts[i].CLShift, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				//=========================

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = Math.Round(_page02Obstacles.Parts[i].PDG * 100.0, 3).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = tmpPDG.ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].ReqTNH, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].ReqH, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(_page02Obstacles.Parts[i].NomPDGDist, eRoundMode.NONE), 1).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Parts[i].NomPDGHeight, eRoundMode.CEIL).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[12].Value = strCloseIn;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[13].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[14].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[15].Value = strPrima;

				if (_page02Obstacles.Obstacles[_page02Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
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

				dataGridView2.Rows.Add(row);
			}

			SetTabVisible(1, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		public void FillPage3(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page03Obstacles.Obstacles = new Obstacle[m];
			_page03Obstacles.Parts = new ObstacleData[n];

			dataGridView3.RowCount = 0;
			if (n <= 0)
				return;

			double NomPDG = 100.0 * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			int i;

			if (_page4Count > 0)
				for (i = 0; i < _page04Obstacles[_page4Count - 1].Parts.Length; i++)
					_page04Obstacles[_page4Count - 1].Parts[i].IsExcluded = GetIndex(Obstacles.Parts, _page04Obstacles[_page4Count - 1].Parts[i]) >= 0;

			for (i = 0; i < m; i++)
				_page03Obstacles.Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
			{
				_page03Obstacles.Parts[i] = Obstacles.Parts[i];
				_page03Obstacles.Parts[i].IsExcluded = false;

				string strPrima;
				if (_page03Obstacles.Parts[i].Prima)
					strPrima = Resources.str15080;
				else
					strPrima = Resources.str15081;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _page03Obstacles.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = _page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_page03Obstacles.Parts[i].d0, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(_page03Obstacles.Parts[i].DistStar, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(_page03Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.GradientToDisplayUnits(_page03Obstacles.Parts[i].PDG, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strPrima;

				if (_page03Obstacles.Obstacles[_page03Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
				{
					row.Cells[0].Style.ForeColor = System.Drawing.Color.Red;			// System.Drawing.Color.FromArgb(0XFF0000);
					row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

					for (int j = 1; j < row.Cells.Count; j++)
					{
						row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
						row.Cells[j].Style.Font = row.Cells[0].Style.Font;
					}
				}

				row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
				dataGridView3.Rows.Add(row);
			}

			SetTabVisible(2, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);

			//if(numericUpDown1.Value == _page4Index + 1)
			//	FillPage4((int)numericUpDown1.Value - 1);
			//if (_page4Index < 0)	_page4Index = 0;

			if (numericUpDown1.Value - 1 == _page4Index)
				FillPage4(_page4Index);
		}

		public void AddPage4(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			if (_page4Count > MaxLegsCount)
				throw new Exception("Maximum legs count exceeded.");

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page04Obstacles[_page4Count].Obstacles = new Obstacle[m];
			_page04Obstacles[_page4Count].Parts = new ObstacleData[n];
			_DetObsPage4[_page4Count] = DetObs;

			int i;

			for (i = 0; i < m; i++)
				_page04Obstacles[_page4Count].Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
				_page04Obstacles[_page4Count].Parts[i] = Obstacles.Parts[i];

			_page4Count++;
			numericUpDown1.Maximum = _page4Count;

			if (_page4Index < 0)
				_page4Index = 0;

			FillPage4(_page4Index);

			SetTabVisible(3, true);
		}

		public void RemoveLastLegP4()
		{
			//if (_page4Index < 0)
			//    throw new Exception("Legs index out of bounds.");
			_page4Index = 0;        //????
			_page4Count--;
			numericUpDown1.Maximum = _page4Count;

			if (_page4Count <= 0)
				SetTabVisible(3, false);
		}

		public void SavePage4Obstacles(int LegNum, string TabComment, ReportFile protRep)
		{
			protRep.SaveObstacles(TabComment, dataGridView4, _page04Obstacles[LegNum], LegNum);
		}

		public void FillPage4(int LegNum)
		{
			_page4Index = LegNum;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (LegNum < 0)
				return;

			ObstacleContainer Obstacles = _page04Obstacles[LegNum];
			ObstacleContainer DetObs = _DetObsPage4[LegNum];

			if (Obstacles.Obstacles == null)
				return;

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			dataGridView4.RowCount = 0;
			if (n <= 0)
				return;

			for (int i = 0; i < n; i++)
			{
				if (Obstacles.Parts[i].IsExcluded)
					continue;

				//string strCloseIn = Resources.str39014;
				string strCloseIn = "";
				if (LegNum == 0 && Obstacles.Parts[i].Ignored && Obstacles.Parts[i].PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
					strCloseIn = Resources.str39015;

				string strPrima;
				if (Obstacles.Parts[i].Prima)
					strPrima = Resources.str15080;
				else
					strPrima = Resources.str15081;

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

				if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
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
				dataGridView4.Rows.Add(row);
			}

			//SetTabVisible(4, true);
			//if (_reportBtn.Checked && !Visible)
			//	Show(GlobalVars.Win32Window);
		}

		public void ClearPage5Obstacles()
		{
			_page5Count = 0;
			_page5Index = -1;
		}

		public void AddPage5(ObstacleContainer Obstacles, ObstacleContainer DetObs)
		{
			if (_page5Count > MaxLegsCount)
				throw new Exception("Maximum legs count exceeded.");

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			_page05Obstacles[_page5Count].Obstacles = new Obstacle[m];
			_page05Obstacles[_page5Count].Parts = new ObstacleData[n];
			_DetObsPage5[_page5Count] = DetObs;

			int i;

			for (i = 0; i < m; i++)
				_page05Obstacles[_page5Count].Obstacles[i] = Obstacles.Obstacles[i];

			for (i = 0; i < n; i++)
				_page05Obstacles[_page5Count].Parts[i] = Obstacles.Parts[i];

			_page5Count++;

			if (_page5Index < 0)
				_page5Index = 0;

			if (_page5Count == _page4Count)
			{
				FillPage5(_page5Index);
				SetTabVisible(4, true);
			}
		}

		public void FillPage5(int LegNum)
		{
			_page5Index = LegNum;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			if (LegNum < 0)
				return;

			ObstacleContainer Obstacles = _page05Obstacles[LegNum];
			ObstacleContainer DetObs = _DetObsPage5[LegNum];

			if (Obstacles.Obstacles == null)
				return;

			int m = Obstacles.Obstacles.Length;
			int n = Obstacles.Parts.Length;

			dataGridView5.RowCount = 0;
			if (n < 0)
				return;

			for (int i = 0; i < n; i++)
			{

			//if (Obstacles[i].IsExcluded)	continue;

				string strPrima;
				if (Obstacles.Parts [i].Prima)
					strPrima = Resources.str15080;
				else
					strPrima = Resources.str15081;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = Obstacles.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].DistStar, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Elev, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].EffectiveHeight, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = strPrima;

				if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].ID == DetObs.Obstacles[0].ID)
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
				dataGridView5.Rows.Add(row);
			}

			//SetTabVisible(4, true);
			//if (_reportBtn.Checked && !Visible)
			//	Show(GlobalVars.Win32Window);
		}

		public void SavePage5Obstacles(int LegNum, string TabComment, ReportFile protRep)
		{
			protRep.SaveObstacles5(TabComment, dataGridView5, _page05Obstacles[LegNum]);
		}

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _page01Obstacles.Parts.Length == 0)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obs = (ObstacleData)dataGridView1.Rows[e.RowIndex].Tag;
			int Index = obs.Owner;
		
			Geometry pGeometry = _page01Obstacles.Obstacles[Index].pGeomPrj;
			Point pPtTmp = obs.pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, _page01Obstacles.Obstacles[Index].UnicalName, 255);
		}

		private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _page02Obstacles.Parts.Length == 0)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obs = (ObstacleData)dataGridView2.Rows[e.RowIndex].Tag;
			int Index = obs.Owner;

			Geometry pGeometry = _page02Obstacles.Obstacles[Index].pGeomPrj;
			Point pPtTmp = obs.pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, _page02Obstacles.Obstacles[Index].UnicalName, 255);
		}

		private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _page03Obstacles.Parts.Length == 0)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obs = (ObstacleData)dataGridView3.Rows[e.RowIndex].Tag;
			int Index = obs.Owner;

			Geometry pGeometry = _page03Obstacles.Obstacles[Index].pGeomPrj;
			Point pPtTmp = obs.pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, _page03Obstacles.Obstacles[Index].UnicalName, 255);
		}

		private void dataGridView4_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _page4Count < 0)//	|| _obstaclesPage4[_page54Index].Length == 0)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obs = (ObstacleData)dataGridView4.Rows[e.RowIndex].Tag;
			int Index = obs.Owner;

			Geometry pGeometry = _page04Obstacles[_page4Index].Obstacles[Index].pGeomPrj;
			Point pPtTmp = obs.pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, _page04Obstacles[_page4Index].Obstacles[Index].UnicalName, 255);
		}

		private void dataGridView5_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _page5Count < 0)//	|| _obstaclesPage4[_page5Index].Length == 0)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obs = (ObstacleData)dataGridView5.Rows[e.RowIndex].Tag;
			int Index = obs.Owner;

			Geometry pGeometry = _page05Obstacles[_page5Index].Obstacles[Index].pGeomPrj;
			Point pPtTmp = obs.pPtPrj;

			if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(pPtTmp, _page05Obstacles[_page5Index].Obstacles[Index].UnicalName, 255);
		}

		private void mainTabControl_SelectedIndexChanged(Object eventSender, EventArgs eventArgs)
		{
			int SelectedTab = mainTabControl.SelectedIndex;

			numericUpDown1.Visible = SelectedTab >= 3;

			//if (!_pageIsVisible[SelectedTab])
			//{
			//    if (_previousTab > -1)
			//        mainTabControl.SelectedIndex = _previousTab;
			//    else
			//        mainTabControl.SelectedIndex = 0;
			//    return;
			//}

			//if(SelectedTab == 3)
			//	numericUpDown1.Value = _page4Index;
			//else if (SelectedTab == 4)
			//	numericUpDown1.Value = _page5Index;

			//if (SelectedTab == _previousTab)
			//	return;

			if (this.Visible)
			{
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
						if (dataGridView3.SelectedRows.Count > 0)
							dataGridView3_RowEnter(dataGridView3, new DataGridViewCellEventArgs(0, dataGridView3.SelectedRows[0].Index));
						break;
					case 3:
						if (dataGridView4.SelectedRows.Count > 0)
							dataGridView4_RowEnter(dataGridView4, new DataGridViewCellEventArgs(0, dataGridView4.SelectedRows[0].Index));
						numericUpDown1.Value = _page4Index + 1;
						break;
					case 4:
						if (dataGridView5.SelectedRows.Count > 0)
						{
							dataGridView5_RowEnter(dataGridView5, new DataGridViewCellEventArgs(0, dataGridView5.SelectedRows[0].Index));
							numericUpDown1.Value = _page5Index + 1;
						}
						break;
				}
			}

			_previousTab = SelectedTab;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (mainTabControl.SelectedIndex == 3)
				FillPage4((int)numericUpDown1.Value - 1);
			else
				FillPage5((int)numericUpDown1.Value - 1);
		}

		public void WriteTabToHTML(System.Windows.Forms.DataGridView gridView, string FileName)
		{
			if (gridView == null)
				return;

			int n, m, i, j;

			StreamWriter sw = File.CreateText(FileName);

			//(char)9;

			sw.WriteLine("<html>");
			sw.WriteLine("<head>");
			sw.WriteLine("<title>PANDA - " + Convert.ToChar(9) + mainTabControl.TabPages[mainTabControl.SelectedIndex].Text + "</title>");
			sw.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
			sw.WriteLine("<style>");
			sw.WriteLine("body {font-family: Arial, Sans-Serif; font-size:12;}");
			sw.WriteLine("table {font-family: Arial, Sans-Serif; font-size:10;}");
			sw.WriteLine("</style>");
			sw.WriteLine("</head>");
			sw.WriteLine("<body>");

			sw.WriteLine(System.Net.WebUtility.HtmlEncode(Text) + "<br>");
			sw.WriteLine(System.Net.WebUtility.HtmlEncode(Convert.ToChar(9) + mainTabControl.TabPages[mainTabControl.SelectedIndex].Text) + "<br>");
			sw.WriteLine("<br>");

			sw.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>");

			n = gridView.Columns.Count;
			m = gridView.Rows.Count;

			sw.WriteLine("<tr>");
			for (i = 0; i < n; i++)
				sw.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(gridView.Columns[i].HeaderText) + "</b></td>");

			sw.WriteLine("</tr>");

			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.DataGridViewRow row = gridView.Rows[i];

				for (j = 0; j < n; j++)
					sw.WriteLine("<td>" + System.Net.WebUtility.HtmlEncode(row.Cells[j].Value.ToString()) + "</td>");

				sw.WriteLine("</tr>");
			}

			sw.WriteLine("</table>");
			sw.WriteLine("</body>");
			sw.WriteLine("</html>");

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		public void WriteTabToTXT(System.Windows.Forms.DataGridView gridView, string FileName)
		{
			if (gridView == null)
				return;

			int i, n = gridView.Columns.Count, m, maxLen = 0;

			string[] headersText = new string[n];
			int[] headersLen = new int[n];

			StreamWriter sw = File.CreateText(FileName);

			sw.WriteLine(Text);
			sw.WriteLine(Convert.ToChar(9) + mainTabControl.TabPages[mainTabControl.SelectedIndex].Text);
			sw.WriteLine();


			for (i = 0; i < n; i++)
			{
				System.Windows.Forms.DataGridViewColumn column = gridView.Columns[i];
				headersText[i] = @"""" + column.HeaderText + @"""";
				headersLen[i] = headersText[i].Length;
				if (headersLen[i] > maxLen)
					maxLen = headersLen[i];
			}

			string strOut = "", tmpStr;

			for (i = 0; i < n; i++)
			{
				int j = maxLen - headersLen[i];
				m = Convert.ToInt32(j / 2);
				tmpStr = "";

				if (i < n)
					tmpStr = String.Empty.PadLeft(m) + "|";

				strOut = strOut + String.Empty.PadLeft(j - m) + headersText[i] + tmpStr;
			}

			sw.WriteLine(strOut);
			strOut = "";

			tmpStr = new string('-', maxLen) + "+";
			for (i = 1; i < n; i++)
				strOut = strOut + tmpStr;

			strOut = strOut + new string('-', maxLen);

			sw.WriteLine(strOut);

			m = gridView.RowCount;
			for (i = 0; i < m; i++)
			{
				System.Windows.Forms.DataGridViewRow row = gridView.Rows[i];

				tmpStr = row.Cells[0].Value.ToString();
				int tmpLen = tmpStr.Length;

				if (tmpLen > maxLen)
					strOut = tmpStr.Substring(0, maxLen - 1) + "*";
				else
					strOut = String.Empty.PadLeft(maxLen - tmpLen) + tmpStr;

				for (int j = 1; j < n; j++)
				{
					tmpStr = row.Cells[j].Value.ToString();

					tmpLen = tmpStr.Length;

					if (tmpLen > maxLen)
						tmpStr = tmpStr.Substring(0, maxLen - 1) + "*";
					else if (j < n - 1 || tmpLen > 0)
						tmpStr = String.Empty.PadLeft(maxLen - tmpLen) + tmpStr;

					strOut = strOut + "|" + tmpStr;
				}
				sw.WriteLine(strOut);
			}

			sw.Flush();
			sw.Dispose();
			sw = null;
		}

		private void SaveBtn_Click(Object eventSender, EventArgs eventArgs)
		{
			if (SaveDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			DataGridView gridView = null;
			switch (mainTabControl.SelectedIndex)
			{
				case 0:
					gridView = dataGridView1;
					break;
				case 1:
					gridView = dataGridView2;
					break;
				case 2:
					gridView = dataGridView3;
					break;
				case 3:
					gridView = dataGridView4;
					break;
				case 4:
					gridView = dataGridView5;
					break;
			}

			WriteTabToHTML(gridView, SaveDialog1.FileName);
		}

		private void CloseBtn_Click(Object eventSender, EventArgs eventArgs)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			//GlobalVars.gAranGraphics.Refresh();
			//GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

			Hide();
			_reportBtn.Checked = false;
		}

		private void HelpBtn_Click(Object eventSender, EventArgs eventArgs)
		{
			NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
		}

	}
}
