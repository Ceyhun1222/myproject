using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Common;
using Aran.PANDA.Rnav.Holding.Properties;

namespace Holding.Forms
{
    public partial class frmHoldingReport : Form
    {
        private int _allObstacleSortColIndex, _penetratedObstacleSortColIndex = -1, _vsHandle;
        private HoldingReport _hReport;
        private SortOrder sortOrder;
        private PointSymbol _ptSymbol;
        private FillSymbol _polygonFillSymbol;
        private List<Report> _allObstacleList;
        private List<Report> _penetratedObstacleList;

        public frmHoldingReport()
        {
            InitializeComponent();
            Closed = true;
        }

        public frmHoldingReport(HoldingReport hReport)
            : this()
        {
            _hReport = hReport;
            _allObstacleList = _hReport.ObstacleReport.ToList<Report>();
            _penetratedObstacleList = _hReport.PenetratedObstacleList.ToList<Report>();
            holdingReportBindingSource.DataSource = hReport;
            lblReportCnt.Text = _hReport.ReportCount.ToString();
            lblReportAltitude.Text = Math.Round(Common.ConvertHeight(hReport.Altitude, roundType.toNearest), 0).ToString();
            tlStripLblUnitText.Text = InitHolding.HeightConverter.Unit;
            tabControl1.TabPages[0].Text = Resources.All_obstacles;
            tabControl1.TabPages[1].Text = Resources.Penetrated_obstacles;
            _ptSymbol = new PointSymbol(ePointStyle.smsCircle, Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 8);

            _polygonFillSymbol = new FillSymbol();
            _polygonFillSymbol.Color = 242424;
            _polygonFillSymbol.Outline = new LineSymbol(eLineStyle.slsDash,
                Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 4);
            _polygonFillSymbol.Style = eFillStyle.sfsNull;
        }

        public void     UptadeReport(HoldingReport hReport)
        {
            _hReport = hReport;
            holdingReportBindingSource.DataSource = hReport;
            holdingReportBindingSource.ResetBindings(true);
        }

        public bool Closed { get; private set; }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<Report> tmpReportList;
            try
            {
                PropertyInfo pInfo = typeof(Report).GetProperty(dataGridReport.Columns[e.ColumnIndex].DataPropertyName);

                if (_allObstacleSortColIndex == e.ColumnIndex && sortOrder == SortOrder.Ascending)
                {
                    tmpReportList = _hReport.ObstacleReport.OrderByDescending(report => pInfo.GetValue(report, null)).ToList<Report>();
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    tmpReportList = _hReport.ObstacleReport.OrderBy(report => pInfo.GetValue(report, null)).ToList<Report>();
                    sortOrder = SortOrder.Ascending;
                }
            }
            catch (Exception excep)
            {
                throw excep;
            }

            _hReport.ObstacleReport.Clear();
            foreach (var report in tmpReportList)
            {
                _hReport.ObstacleReport.Add(report);
            }
            _allObstacleSortColIndex = e.ColumnIndex;
            dataGridReport.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void dataGridReport_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (_hReport.ObstacleReport[e.RowIndex].Penetrate >= 0)
                dataGridReport.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
        }

        private void dataGridReport_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(_vsHandle);
            if (e.RowIndex != -1 && _hReport != null && _hReport.ObstacleReport[e.RowIndex] != null)
            {
                VerticalStructure vs = _hReport.ObstacleReport[e.RowIndex].Obstacle;
                var _selectedObstacle = _hReport.ObstacleReport[e.RowIndex];

				if (_selectedObstacle.GeomPrj is Aran.Geometries.Point)
					_vsHandle = GlobalParams.UI.DrawPointWithText(_selectedObstacle.GeomPrj as Aran.Geometries.Point, vs.Name, _ptSymbol);

				else if (_selectedObstacle.GeomPrj is MultiLineString)
					_vsHandle = GlobalParams.UI.DrawMultiLineString(_selectedObstacle.GeomPrj as MultiLineString, 4, ARANFunctions.RGB(255, 0, 0));

				else if (_selectedObstacle.GeomPrj is Aran.Geometries.MultiPolygon)
				{
					_vsHandle = GlobalParams.UI.DrawMultiPolygon(
						_selectedObstacle.GeomPrj as MultiPolygon, _polygonFillSymbol, true, false);
				}
            }
        }

        private void frmHoldingReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(_vsHandle);
            Closed = true;
        }

        private void frmHoldingReport_Shown(object sender, EventArgs e)
        {
            Closed = false;
        }

        private void dataGridView1_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<Report> tmpReportList;
            try
            {
                PropertyInfo pInfo = typeof(Report).GetProperty(dataGridView1.Columns[e.ColumnIndex].DataPropertyName);

                if (_penetratedObstacleSortColIndex == e.ColumnIndex && sortOrder == SortOrder.Ascending)
                {
                    tmpReportList = _hReport.PenetratedObstacleList.OrderByDescending(report => pInfo.GetValue(report, null)).ToList<Report>();
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    tmpReportList = _hReport.PenetratedObstacleList.OrderBy(report => pInfo.GetValue(report, null)).ToList<Report>();
                    sortOrder = SortOrder.Ascending;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _hReport.PenetratedObstacleList.Clear();
            foreach (var report in tmpReportList)
            {
                _hReport.PenetratedObstacleList.Add(report);
            }
            _penetratedObstacleSortColIndex = e.ColumnIndex;
            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(_vsHandle);
            if (e.RowIndex != -1 && _hReport != null && _hReport.PenetratedObstacleList[e.RowIndex] != null)
            {
                var vs = _hReport.PenetratedObstacleList[e.RowIndex].Obstacle;
                var ptSymbol = new PointSymbol(ePointStyle.smsCircle, Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 8);
                _vsHandle = GlobalParams.UI.DrawPointWithText(GeomFunctions.AssignToPrj(vs.Part[0].HorizontalProjection.Location), vs.Name, ptSymbol);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                lblReportCnt.Text = _hReport.ObstacleReport.Count.ToString();
            else
                lblReportCnt.Text = _hReport.PenetratedObstacleList.Count.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.ToLower();
         
            var searchResult = _allObstacleList.Where(
                    c => (c.Name!=null) &&(c.Name.ToLower().Contains(query)) ||
                    c.Id.ToString().Contains(query) ||
                    c.GeomType.ToString().ToLower().Contains(query) ||
                    c.Elevation.ToString().Contains(query) ||
                    c.Area.ToLower().Contains(query) ||
                    c.Moc.ToString().Contains(query)
                ).ToList<Report>();

            _hReport.ObstacleReport.Clear();
            foreach (var report in searchResult)
            {
                _hReport.ObstacleReport.Add(report);
            }
        }

        private void txtSearchPenetrate_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearchPenetrate.Text.ToLower();

            var searchResult = _penetratedObstacleList.Where(
                    c => (c.Name != null) && (c.Name.ToLower().Contains(query)) ||
                    c.Id.ToString().Contains(query) ||
                    c.GeomType.ToString().ToLower().Contains(query) ||
                    c.Elevation.ToString().Contains(query) ||
                    c.Area.ToLower().Contains(query) ||
                    c.Moc.ToString().Contains(query)
                ).ToList<Report>();

            _hReport.PenetratedObstacleList.Clear();
            foreach (var report in searchResult)
            {
                _hReport.PenetratedObstacleList.Add(report);
            }
        }

    }
}
