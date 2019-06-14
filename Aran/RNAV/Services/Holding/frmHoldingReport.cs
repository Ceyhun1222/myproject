using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Delib.Classes.Features.Obstacle;
using Delib.Classes;

namespace Holding
{
    public partial class frmHoldingReport : Form
    {
        public frmHoldingReport()
        {
            InitializeComponent();
        }


        public frmHoldingReport(HoldingReport hReport):base()
        {
           InitializeComponent();
           _hReport = hReport;
            holdingReportBindingSource.DataSource = hReport;
            txtCount.Text = _hReport.ReportCount.ToString();
            dataGridReport.Columns["Elevation"].HeaderText = "Elevation("+InitHolding.HeightConverter.Unit+")";
            dataGridReport.Columns["Altitude"].HeaderText = "Altitude("+InitHolding.HeightConverter.Unit+")";
            dataGridReport.Columns["MOC"].HeaderText = "MOC(" + InitHolding.HeightConverter.Unit + ")";
            dataGridReport.Columns["RequriedH"].HeaderText = "Req_H(" + InitHolding.HeightConverter.Unit + ")";
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<Report> tmpReportList;
            try 
	        {
                 PropertyInfo pInfo = typeof(Report).GetProperty(dataGridReport.Columns[e.ColumnIndex].DataPropertyName);
                
                 if (oldColumnIndex ==e.ColumnIndex && sortOrder== SortOrder.Ascending)
                 {
                    tmpReportList =  _hReport.ObstacleReport.OrderByDescending(report=>pInfo.GetValue(report,null)).ToList<Report>();
                    sortOrder = SortOrder.Descending;
                 }
                 else 
                 {
                    tmpReportList = _hReport.ObstacleReport.OrderBy(report=>pInfo.GetValue(report,null)).ToList<Report>();
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
            oldColumnIndex = e.ColumnIndex;
            dataGridReport.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private int oldColumnIndex = -1,_vsHandle;
        private HoldingReport _hReport;
        private SortOrder sortOrder;

        private void dataGridReport_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (_hReport.ObstacleReport[e.RowIndex].Req_H <= 0)
                dataGridReport.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
        }

        private void dataGridReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void dataGridReport_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(ref _vsHandle);
            if (e.RowIndex != -1)
            {
                VerticalStructure vs = _hReport.VerticalStructureList[e.RowIndex];
                ARAN.Contracts.UI.PointSymbol ptSymbol = new ARAN.Contracts.UI.PointSymbol(ARAN.Contracts.UI.ePointStyle.smsCircle,2,6);
                _vsHandle = GlobalParams.UI.DrawPointWithText(GeomFunctions.GmlToAranPointPrj(vs as Feature),ptSymbol,vs.name);

            }
        }

        private void frmHoldingReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(ref _vsHandle);
        }
    }
}
