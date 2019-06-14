using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.EnRoute.Modules;
using Aran.PANDA.RNAV.EnRoute.Properties;

namespace Aran.PANDA.RNAV.EnRoute
{
	public partial class ReportForm : Form
	{
		//private ObstacleContainer _obstaclesPage2;
		private ObstacleContainer _obstaclesPage3;

		//Segment _currLeg;
		List<Segment> _legs;

		private int _page3Count;
		private int _page3Index;

		//private int _selectedTab;
		private int _pointElem;
		private int _geomElem;

		private CheckBox _reportBtn;
        private List<ObstacleReport> _obstacles;

        #region FORM
        public ReportForm()
		{
			InitializeComponent();

			Text = Resources.str02000 ;
			//Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			//this.Text = Resources.str02000 + "" + " v:" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Revision.ToString();

			saveButton.Text = Resources.str02001;
			closeButton.Text = Resources.str02002;
			chbForward.Text = Resources.str02006;
			chbBackward.Text = Resources.str02007;

			//tabControl1.TabPages[0].Text = Resources.str02003;
			//tabControl1.TabPages[1].Text = Resources.str02005;

			//dataGridView02.Columns[0].HeaderText = Resources.str02018;										 				//	Name
			//dataGridView02.Columns[1].HeaderText = Resources.str02019;														//	ID
			//dataGridView02.Columns[2].HeaderText = Resources.str02020 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";	//	X
			//dataGridView02.Columns[3].HeaderText = Resources.str02021 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";	//	Y
			//dataGridView02.Columns[4].HeaderText = Resources.str02022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	Elevation
			//dataGridView02.Columns[5].HeaderText = Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	MOC
			//dataGridView02.Columns[6].HeaderText = Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	MOCA
			//dataGridView02.Columns[7].HeaderText = Resources.str02025 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	Hor. accuracy
			//dataGridView02.Columns[8].HeaderText = Resources.str02026 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	Ver. accuracy
			//dataGridView02.Columns[9].HeaderText = Resources.str02027;														//	Area

			dataGridView03.Columns[0].HeaderText = Resources.str02018;										 				//	Name
			dataGridView03.Columns[1].HeaderText = Resources.str02019;														//	ID
			dataGridView03.Columns[2].HeaderText = Resources.str02020 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";	//	X
			dataGridView03.Columns[3].HeaderText = Resources.str02021 + " (" + GlobalVars.unitConverter.DistanceUnit + ")";	//	Y
			dataGridView03.Columns[4].HeaderText = Resources.str02022 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	Elevation
			dataGridView03.Columns[5].HeaderText = Resources.str02023 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	MOC
			dataGridView03.Columns[6].HeaderText = Resources.str02024 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	MOCA
			dataGridView03.Columns[7].HeaderText = Resources.str02025 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	Hor. accuracy
			dataGridView03.Columns[8].HeaderText = Resources.str02026 + " (" + GlobalVars.unitConverter.HeightUnit + ")";	//	Ver. accuracy
			dataGridView03.Columns[9].HeaderText = Resources.str02027;														//	Area

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
		    GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
		    GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

            if (e.CloseReason == CloseReason.UserClosing)
			{
				Hide();
				e.Cancel = true;
				_reportBtn.Checked = false;
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = CommonFunctions.GetSystemMenu(this.Handle, false);
			// Add a separator
			CommonFunctions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			CommonFunctions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
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

		private void ReportForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;

			if (saveDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			RepFileName = saveDialog.FileName;
			RepFileTitle = RepFileName;

			int pos = RepFileName.LastIndexOf('.');
			if (pos > 0)
				RepFileTitle = RepFileName.Substring(0, pos);

			int pos2 = RepFileTitle.LastIndexOf('\\');
			if (pos2 > 0)
				RepFileTitle = RepFileTitle.Substring(pos2 + 1);

			CReportFile.WriteTabToHTML(dataGridView03, RepFileName, RepFileTitle);
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
		  Close();
		}

		public void Init(CheckBox reportBtn)		//, int HelpContext
		{
			_reportBtn = reportBtn;					//_helpContextID = HelpContext;
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

		public void Update(List<Segment> Legs)
		{
			_legs = Legs;
			int n = _legs.Count;

			_page3Count = n;
			if (_page3Index >= _page3Count)
				_page3Index = _page3Count - 1;

			int val = (int)numericUpDown1.Value;
			numericUpDown1.Maximum = n;

			if (val == (int)numericUpDown1.Value)
			{
				_page3Index = val - 1;
				FillGrid();
			}
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			_page3Index = (int)numericUpDown1.Value - 1;
			FillGrid();
		}

		private void chbForward_CheckedChanged(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid_old()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			dataGridView03.RowCount = 0;
			if (_page3Index < 0 || _page3Index >= _page3Count || _page3Count < 1)
				return;

			Segment currLeg = _legs[_page3Index];
			ObstacleContainer Obstacles;

			chbForward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.FORWARD;
			chbBackward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.BACKWARD;

			bool bReturn = false;
			if (!chbForward.Enabled)
			{
				bReturn = chbForward.Checked;
				chbForward.Checked = false;
				if (bReturn)
					return;
			}

			if (!chbBackward.Enabled)
			{
				bReturn = chbBackward.Checked;
				chbBackward.Checked = false;

				if (bReturn)
					return;
			}

			if (chbForward.Checked && chbBackward.Checked)
				Obstacles = currLeg.MergedObstacles;
			else if (chbForward.Checked && (chbForward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.FORWARD))
				Obstacles = currLeg.Forwardleg.Obstacles_2;
			else if (chbBackward.Checked && (chbBackward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.BACKWARD))
				Obstacles = currLeg.Backwardleg.Obstacles_2;
			else
				return;

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

				dataGridView03.Rows.Add(row);

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

			dataGridView03.Sort(dataGridView03.Columns[6], ListSortDirection.Descending);

			//SetTabVisible(0, true);
			if (_reportBtn.Checked && !Visible)
				Show(GlobalVars.Win32Window);
		}

	    private void FillGrid()
	    {
	        GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
	        GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

	        dataGridView03.RowCount = 0;
	        if (_page3Index < 0 || _page3Index >= _page3Count || _page3Count < 1)
	            return;

	        Segment currLeg = _legs[_page3Index];

	        chbForward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.FORWARD;
	        chbBackward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.BACKWARD;

	        bool bReturn = false;
	        if (!chbForward.Enabled)
	        {
	            bReturn = chbForward.Checked;
	            chbForward.Checked = false;
	            if (bReturn)
	                return;
	        }

	        if (!chbBackward.Enabled)
	        {
	            bReturn = chbBackward.Checked;
	            chbBackward.Checked = false;

	            if (bReturn)
	                return;
	        }

	        _obstacles = currLeg.ObstacleList.OrderBy(obs=>obs.ReqH).ToList<ObstacleReport>();

	        if (chbForward.Checked && chbBackward.Checked)
	            _obstacles = currLeg.ObstacleList.GroupBy(obs => obs.Obstacle.Identifier)
	                .Select(grp => grp.FirstOrDefault(obsGr => Math.Abs(obsGr.ReqH - grp.Max(y => y.ReqH)) < 0.0001)).ToList();
	        else if (chbForward.Checked && (chbForward.Enabled = currLeg.Direction == CodeDirection.BOTH ||
	                                                        currLeg.Direction == CodeDirection.FORWARD))
	            _obstacles = _obstacles.Where(obs => obs.SideType == Side.Forward).ToList();
	        else if (chbBackward.Checked && (chbBackward.Enabled = currLeg.Direction == CodeDirection.BOTH || currLeg.Direction == CodeDirection.BACKWARD))
	            _obstacles = _obstacles.Where(obs => obs.SideType == Side.BackWard).ToList();
            else
	            return;

	        lblObsCount.Text = _obstacles.Count.ToString();
	        //var resul = currLeg.ObstacleList.FindAll(
	        //    obsd => _obstacles.Find(obsf => obsf.Obstacle.Identifier != obsd.Obstacle.Identifier) == null).ToList();

	        foreach(var obstacle in _obstacles)
	        {

	            string strArea;

	            strArea = obstacle.Prima ? Resources.str02030 : Resources.str02031;

	            DataGridViewRow row = new DataGridViewRow {Tag = obstacle };

	            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell {Value = obstacle.TypeName};
	            row.Cells.Add(cell);

	            cell = new DataGridViewTextBoxCell {Value = obstacle.UnicalName};
	            row.Cells.Add(cell);

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[2].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstacle.Dist, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[3].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(obstacle.CLShift, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[4].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.Height, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[5].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.Moc, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[6].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.ReqH, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[7].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.HorAccuracy, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[8].Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.VerAccuracy, eRoundMode.NEAREST).ToString();

	            row.Cells.Add(new DataGridViewTextBoxCell());
	            row.Cells[9].Value = strArea;

	            dataGridView03.Rows.Add(row);
	        }

	        dataGridView03.Sort(dataGridView03.Columns[6], ListSortDirection.Descending);

	        //SetTabVisible(0, true);
	        if (_reportBtn.Checked && !Visible)
	            Show(GlobalVars.Win32Window);
	    }

        private void dataGridView03_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || _obstacles.Count== 0 || !Visible)
				return;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_pointElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_geomElem);

			ObstacleReport obsData = (ObstacleReport)dataGridView03.Rows[e.RowIndex].Tag;

			Geometry pGeometry = obsData.Geo;

			if (pGeometry.Type == GeometryType.LineString)
				_geomElem = GlobalVars.gAranGraphics.DrawLineString((LineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.MultiLineString)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)pGeometry, 2, 255);
			else if (pGeometry.Type == GeometryType.Polygon)
				_geomElem = GlobalVars.gAranGraphics.DrawPolygon((Polygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);
			else if (pGeometry.Type == GeometryType.MultiPolygon)
				_geomElem = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)pGeometry, AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross, 255);

			_pointElem = GlobalVars.gAranGraphics.DrawPointWithText(obsData.Vertex, obsData.UnicalName, 255);
		}

		private void dataGridView03_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{

		}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int i = 0;
            foreach (System.Windows.Forms.DataGridViewRow r in dataGridView03.Rows)
            {
                if ((r.Cells[1].Value).ToString().ToUpper().Contains(txtSearch.Text.ToUpper()))
                {
                    dataGridView03.Rows[r.Index].Visible = true;
                    dataGridView03.Rows[r.Index].Selected = true;
                    i++;
                }
                else
                {
                    dataGridView03.CurrentCell = null;
                    dataGridView03.Rows[r.Index].Visible = false;
                }
            }
            lblObsCount.Text = i.ToString();
            this.Cursor = Cursors.Arrow;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
    }
}

