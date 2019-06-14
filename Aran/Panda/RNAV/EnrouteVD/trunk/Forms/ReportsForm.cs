using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Enroute.VD.Properties;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	public partial class ReportsForm : Form
	{
		private ObstacleContainer _obstaclesPage3;

		private List<Segment> _legs;

		private int _page3Count;
		private int _page3Index;

		private int _pointElem;
		private int _geomElem;

		private CheckBox _reportBtn;

		#region FORM
		public ReportsForm()
		{
			InitializeComponent();
			Text = Resources.str02000;

			saveButton.Text = Resources.str02001;
			closeButton.Text = Resources.str02002;

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

			//_currLeg.Direction = SegmentDirection.Empty;
			_page3Count = 0;
			_page3Index = -1;
		}

		private void ReportForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				//NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, _helpContextID);
				e.Handled = true;
			}
		}

		private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			if (e.CloseReason == CloseReason.UserClosing)
			{
				Hide();
				e.Cancel = true;
				_reportBtn.Checked = false;
			}
		}

		private void ReportForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Utility.GetSystemMenu(this.Handle, false);
			// Add a separator
			Utility.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Utility.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
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

			if (!Utility.ShowSaveDialog(out RepFileName, out RepFileTitle))
				return;

			CReportFile.WriteTabToHTML(dataGridView01, RepFileName, RepFileTitle);
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		internal void Init(CheckBox reportBtn)          //, int HelpContext = 0
		{
			_reportBtn = reportBtn;             //_helpContextID = HelpContext;
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

				default:
					e.SortResult = System.String.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
					break;
			}

			e.Handled = true;
		}

#endregion

		public void Update(List<Segment> Legs)
		{
			_legs = Legs;
			int n = _legs.Count;

			_page3Count = n;
			if (_page3Index >= _page3Count)
				_page3Index = _page3Count - 1;

			int val = (int)numericUpDown01.Value;
			numericUpDown01.Maximum = n;

			if (val == (int)numericUpDown01.Value)
			{
				_page3Index = val - 1;
				FillGrid();
			}
		}

		private void numericUpDown01_ValueChanged(object sender, EventArgs e)
		{
			_page3Index = (int)numericUpDown01.Value - 1;
			FillGrid();
		}

		private void FillGrid()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			dataGridView01.RowCount = 0;
			if (_page3Index < 0 || _page3Index >= _page3Count || _page3Count < 1)
				return;

			Segment currLeg = _legs[_page3Index];

			ObstacleContainer Obstacles = currLeg.Obstacles;

			int n = Obstacles.Parts.Length;

			_obstaclesPage3.Parts = new ObstacleData[n];

			if (n <= 0)
				return;

			int m = Obstacles.Obstacles.Length;
			_obstaclesPage3.Obstacles = new Obstacle[m];
			Array.Copy(Obstacles.Obstacles, _obstaclesPage3.Obstacles, m);

			//int Page3Det = -1;

			for (int i = 0; i < n; i++)
			{
				_obstaclesPage3.Parts[i] = Obstacles.Parts[i];

				string strArea;

				if (_obstaclesPage3.Parts[i].Prima)
					strArea = Resources.str02030;
				else
					strArea = Resources.str02031;

				DataGridViewRow row = new DataGridViewRow();
				row.Tag = _obstaclesPage3.Parts[i];

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
				row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].Height, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].MOC, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Parts[i].ReqH, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].HorAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(Obstacles.Obstacles[Obstacles.Parts[i].Owner].VertAccuracy, eRoundMode.NEAREST).ToString();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[9].Value = strArea;

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

			//SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

		private void dataGridView01_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _obstaclesPage3.Parts.Length == 0 || !Visible)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleData obsData = (ObstacleData)dataGridView01.Rows[e.RowIndex].Tag;
			Obstacle owner = _obstaclesPage3.Obstacles[obsData.Owner];

			Geometry pGeometry = owner.pGeomPrj;

			if (pGeometry.Type == GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.pPtPrj, owner.UnicalName, 255);
		}
	}
}
