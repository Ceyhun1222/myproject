using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Arrival.Properties;

namespace Aran.PANDA.RNAV.Arrival
{
	public partial class ReportsForm : Form
	{
		private ObstacleContainer _obstaclesPage1;
		private ObstacleContainer _obstaclesPage2;

		//LegSBAS _currLeg;
		List<LegArrival> _legs;

		private int _page2Count;
		private int _page2Index;

		private int _selectedTab;
		private int _pointElem;
		private int _geomElem;

		private CheckBox _reportBtn;
		private PointSymbol _exactVertexSymbol;

		#region FORM

		public ReportsForm()
		{
			InitializeComponent();

			_exactVertexSymbol = new PointSymbol
			{
				Color = 255,
				Style = ePointStyle.smsCross,
				Size = 16
			};

			//Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			this.Text = Resources.str02003;// + "" + " v:" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Revision.ToString();

			saveButton.Text = Resources.str02001;
			closeButton.Text = Resources.str02002;
			tabControl1.TabPages[0].Text = Resources.str02004;
			tabControl1.TabPages[1].Text = Resources.str02005;

			dataGridView01.Columns[0].HeaderText = Resources.str02018;                                                      //	Name
			dataGridView01.Columns[1].HeaderText = Resources.str02019;                                                      //	ID
			dataGridView01.Columns[2].HeaderText = Resources.str02020 + " (" + GlobalVars.unitConverter.DistanceUnit + ")"; //	X
			dataGridView01.Columns[3].HeaderText = Resources.str02021 + " (" + GlobalVars.unitConverter.DistanceUnit + ")"; //	Y
			dataGridView01.Columns[4].HeaderText = Resources.str02022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	Elevation
			dataGridView01.Columns[5].HeaderText = Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	MOC
			dataGridView01.Columns[6].HeaderText = Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	MOCA
			dataGridView01.Columns[7].HeaderText = Resources.str02025 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	Hor. accuracy
			dataGridView01.Columns[8].HeaderText = Resources.str02026 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	Ver. accuracy
			dataGridView01.Columns[9].HeaderText = Resources.str02027;                                                      //	Area

			dataGridView02.Columns[0].HeaderText = Resources.str02018;                                                      //	Name
			dataGridView02.Columns[1].HeaderText = Resources.str02019;                                                      //	ID
			dataGridView02.Columns[2].HeaderText = Resources.str02020 + " (" + GlobalVars.unitConverter.DistanceUnit + ")"; //	X
			dataGridView02.Columns[3].HeaderText = Resources.str02021 + " (" + GlobalVars.unitConverter.DistanceUnit + ")"; //	Y
			dataGridView02.Columns[4].HeaderText = Resources.str02022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	Elevation
			dataGridView02.Columns[5].HeaderText = Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	MOC
			dataGridView02.Columns[6].HeaderText = Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	MOCA
			dataGridView02.Columns[7].HeaderText = Resources.str02025 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	Hor. accuracy
			dataGridView02.Columns[8].HeaderText = Resources.str02026 + " (" + GlobalVars.unitConverter.HeightUnit + ")";   //	Ver. accuracy
			dataGridView02.Columns[9].HeaderText = Resources.str02027;                                                      //	Area
		}

		private void ReportsForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
				e.Handled = true;
			}
		}

		private void ReportsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			if (e.CloseReason == CloseReason.UserClosing)
			{
				Hide();
				e.Cancel = true;
				_reportBtn.Checked = false;
			}

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
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

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;

			if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
				return;

			WriteTab(RepFileName, RepFileTitle);
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		internal void Init(CheckBox reportBtn)          //, int HelpContext = 0
		{
			_reportBtn = reportBtn;             //_helpContextID = HelpContext;
		}

		public void WriteTab(string RepFileName, string comment)
		{
			switch (tabControl1.SelectedIndex)
			{
				case 0:
					CReportFile.WriteTabToHTML(dataGridView01, RepFileName, comment);   //RepFileTitle
					break;
				case 1:
					CReportFile.WriteTabToHTML(dataGridView02, RepFileName, comment);       //RepFileTitle
					break;
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int Selected = tabControl1.SelectedIndex;
			if (Selected == _selectedTab || tabControl1.TabCount == 0)
				return;

			numericUpDown1.Visible = Selected == 1;
			lblCount.Visible = Selected == 0;

			if (this.Visible)
			{
				switch (Selected)
				{
					case 0:
						if (dataGridView01.SelectedRows.Count > 0)
							dataGridView01_RowEnter(dataGridView01, new DataGridViewCellEventArgs(0, dataGridView01.SelectedRows[0].Index));
						break;
					case 1:
						if (dataGridView02.SelectedRows.Count > 0)
							dataGridView02_RowEnter(dataGridView02, new DataGridViewCellEventArgs(0, dataGridView02.SelectedRows[0].Index));

						break;
				}
			}

			_selectedTab = Selected;
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
		#endregion

		public void Update(List<LegArrival> Legs)
		{
			_legs = Legs;
			int n = _legs.Count;

			_page2Count = n;
			if (_page2Index >= _page2Count)
				_page2Index = _page2Count - 1;

			int val = (int)numericUpDown1.Value;
			numericUpDown1.Maximum = n - 1;

			if (val == (int)numericUpDown1.Value)
			{
				_page2Index = val;
				FillPage2();
			}
		}

		public void SetCurrentPage(int page)
		{
			if (page == 3)
			{
				tabControl1.TabPages.Remove(tabPage1);
				numericUpDown1.Visible = true;
				lblCount.Visible = false;
			}
			else
			{
				tabControl1.TabPages.Insert(0, tabPage1);
				numericUpDown1.Visible = false;
				lblCount.Visible = true;
			}
		}

		//public void SetCurrentLeg(LegSBAS currLeg)
		//{
		//	_currLeg = currLeg;
		//	FillCurrentLeg(_currLeg);
		//}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			_page2Index = (int)numericUpDown1.Value;
			FillPage2();
		}

		public void FillCurrentLeg(LegArrival currLeg, LegBase currLegB = null)
		{
			//Stopwatch st = new Stopwatch();
			//st.Start();

			if (_selectedTab == 0)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
			}

			dataGridView01.RowCount = 0;
			ObstacleContainer Obstacles = currLeg.Obstacles;

			if (currLegB != null)
				Functions.mergeObstacleLists(currLeg, currLegB, out Obstacles);

			int n = Obstacles.Parts.Length;
			_obstaclesPage1.Parts = new ObstacleData[n];

			lblCount.Text = "Count : " + n;

			if (n <= 0)
				return;

			int m = Obstacles.Obstacles.Length;
			_obstaclesPage1.Obstacles = new Obstacle[m];
			Array.Copy(Obstacles.Obstacles, _obstaclesPage1.Obstacles, m);

			for (int i = 0; i < n; i++)
			{
				_obstaclesPage1.Parts[i] = Obstacles.Parts[i];

				double VertAccuracy = Obstacles.Obstacles[Obstacles.Parts[i].Owner].VertAccuracy;

				string strArea;
				if (_obstaclesPage1.Parts[i].Prima)
					strArea = Resources.str02030;
				else
					strArea = Resources.str02031;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage1.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].Dist, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].CLShift, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height - VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strArea;

				row.Cells.Add(new DataGridViewTextBoxCell());

				var obstacleGeoType = Obstacles.Obstacles[Obstacles.Parts[i].Owner].pGeomPrj.Type;
				if (obstacleGeoType == Geometries.GeometryType.Point)
					row.Cells[10].Value = "Point";
				else if (obstacleGeoType == Geometries.GeometryType.LineString || obstacleGeoType == Geometries.GeometryType.MultiLineString)
					row.Cells[10].Value = "Line";
				else
					row.Cells[10].Value = "Polygon";

				dataGridView01.Rows.Add(row);

				//if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].ID == _page1DetID)
				//{
				//	Page1Det = i;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				//row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
			}

			//	st.Stop();
			//	MessageBox.Show(st.Elapsed.ToString());

			//SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		private void FillPage2()
		{
			if (_selectedTab == 2)
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
			}

			dataGridView02.RowCount = 0;
			if (_page2Index < 0 || _page2Index >= _page2Count || _page2Count < 1)
				return;

			LegArrival currLeg = _legs[_page2Index];
			ObstacleContainer Obstacles = currLeg.Obstacles;

			int n = Obstacles.Parts.Length;

			_obstaclesPage2.Parts = new ObstacleData[n];

			if (n <= 0)
				return;

			int m = Obstacles.Obstacles.Length;
			_obstaclesPage2.Obstacles = new Obstacle[m];
			Array.Copy(Obstacles.Obstacles, _obstaclesPage2.Obstacles, m);

			//int Page3Det = -1;

			for (int i = 0; i < n; i++)
			{
				_obstaclesPage2.Parts[i] = Obstacles.Parts[i];

				double VertAccuracy = Obstacles.Obstacles[Obstacles.Parts[i].Owner].VertAccuracy;

				string strArea;
				if (_obstaclesPage2.Parts[i].Prima)
					strArea = Resources.str02030;
				else
					strArea = Resources.str02031;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage2.Parts[i];

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].TypeName;
				row.Cells.Add(cell);

				cell = new DataGridViewTextBoxCell();
				cell.Value = Obstacles.Obstacles[Obstacles.Parts[i].Owner].UnicalName;
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].Dist, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(Obstacles.Parts[i].CLShift, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height - VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strArea;

				dataGridView02.Rows.Add(row);

				//if (Obstacles.Obstacles[Obstacles.Parts[i].Owner].ID == _page1DetID)
				//{
				//	Page1Det = i;
				//	row.Cells[0].Style.ForeColor = System.Drawing.Color.FromArgb(0XFF0000);
				//	row.Cells[0].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

				//	for (int j = 1; j < row.Cells.Count; j++)
				//	{
				//		row.Cells[j].Style.ForeColor = row.Cells[0].Style.ForeColor;
				//		row.Cells[j].Style.Font = row.Cells[0].Style.Font;
				//	}
				//}

				//row.Cells[3].Style.Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);
			}

			//SetTabVisible(0, true);

			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		private void dataGridView01_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _obstaclesPage1.Parts.Length == 0 || !Visible)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obsData = (ObstacleData)dataGridView01.Rows[e.RowIndex].Tag;
			Obstacle owner = _obstaclesPage1.Obstacles[obsData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.Point)
				_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, 255);
			else
			{
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

				if (!obsData.Prima)
					_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, _exactVertexSymbol);
				else
					_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, 255);
			}
		}

		private void dataGridView02_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _obstaclesPage2.Parts.Length == 0 || !Visible)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obsData = (ObstacleData)dataGridView02.Rows[e.RowIndex].Tag;
			Obstacle owner = _obstaclesPage2.Obstacles[obsData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == Geometries.GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == Geometries.GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == Geometries.GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, 255);
		}
	}
}
