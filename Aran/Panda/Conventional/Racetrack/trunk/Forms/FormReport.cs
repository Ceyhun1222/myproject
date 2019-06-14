using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using DoddleReport;
using DoddleReport.Writers;
using Aran.Geometries;
using Aran.PANDA.Common;
using System.Globalization;
using Aran.PANDA.Conventional.Racetrack.Properties;

namespace Aran.PANDA.Conventional.Racetrack
{
	public partial class FormReport : Form
	{
		private int _oldColumnIndex = -1, _vsHandle;
		private Report _report;
		private SortOrder _sortOrder;
		private readonly MainController _controller;
		private readonly FillSymbol _polygonFillSymbol;
		//private PointSymbol _ptSymbol;

		private FormReport ( )
		{
			InitializeComponent ( );
			SetColumnHeaders ( );

			//_ptSymbol = new PointSymbol ( ePointStyle.smsCircle, Aran.PANDA.Common.ARANFunctions.RGB ( 255, 0, 0 ), 8 );

			_polygonFillSymbol = new FillSymbol
			{
				Color = 242424,
				Outline = new LineSymbol(eLineStyle.slsDash,
					ARANFunctions.RGB(255, 0, 0), 4),
				Style = eFillStyle.sfsNull
			};

		}

		private void SetColumnHeaders ( )
		{
            //dataGridReport.Columns[ "dgvTxtBxClmnId" ].Tag = "Id";
            //dataGridReport.Columns[ "dgvTxtBxClmnId" ].HeaderText = "Id";

			dataGridReport.Columns[ "dgvTxtBxClmnName" ].Tag = "Name";
			dataGridReport.Columns[ "dgvTxtBxClmnName" ].HeaderText = "Name";

			dataGridReport.Columns[ "dgvTxtBxClmnGeomType" ].Tag = "GeomType";
			dataGridReport.Columns[ "dgvTxtBxClmnGeomType" ].HeaderText = "Geometry Type";

			dataGridReport.Columns[ "dgvTxtBxClmnArea" ].Tag = "Area";
			dataGridReport.Columns[ "dgvTxtBxClmnArea" ].HeaderText = "Area";

			dataGridReport.Columns[ "dgvTxtBxClmnHAcc" ].Tag = "HorAccuracy";
			dataGridReport.Columns[ "dgvTxtBxClmnHAcc" ].HeaderText = "HAcc";

			dataGridReport.Columns[ "dgvTxtBxClmnVAcc" ].Tag = "VerAccuracy";
			dataGridReport.Columns[ "dgvTxtBxClmnVAcc" ].HeaderText = "VAcc";

			dataGridReport.Columns[ "dgvTxtBxClmnElevation" ].Tag = "Elevation";
			dataGridReport.Columns[ "dgvTxtBxClmnElevation" ].HeaderText = "Elevation (" + GlobalParams.Settings.HeightUnit + ")";

			dataGridReport.Columns[ "dgvTxtBxClmnMOC" ].Tag = "Moc";
			dataGridReport.Columns[ "dgvTxtBxClmnMOC" ].HeaderText = "MOC (" + GlobalParams.Settings.HeightUnit + ")";

			dataGridReport.Columns[ "dgvTxtBxClmnReq_H" ].Tag = "Req_H";
			dataGridReport.Columns[ "dgvTxtBxClmnReq_H" ].HeaderText = "Req_Altitude (" + GlobalParams.Settings.HeightUnit + ")";


			dataGridReport.Columns[ "dgvTxtBxClmnPenetrate" ].Tag = "Penetrate";
			dataGridReport.Columns[ "dgvTxtBxClmnPenetrate" ].HeaderText = "Penetrate";
		}

		public FormReport ( Report report, string altitudeText, MainController controller )
			: this ( )
		{
			_controller = controller;
			_report = report;
			UpdateDataGridView ( report.ObstacleReport );
			tsStLblAltitude.Text = altitudeText;
			tsStLblCnt.Text = _report.ReportCount.ToString ( );
		}

		private void UpdateDataGridView ( List<UserReport> obstacleReport )
		{
			SetColumnHeaders ( );

			dataGridReport.Rows.Clear ( );
			foreach ( var item in obstacleReport )
			{
				var index = dataGridReport.Rows.Add ( );
				var row = dataGridReport.Rows[ index ];

                row.Tag = item;
				//row.Cells[ 0 ].Value = item.Id;
				row.Cells[ 0 ].Value = item.Name;
				row.Cells[ 1 ].Value = item.GeomType;
				row.Cells[ 2 ].Value = item.Area;
				row.Cells[ 3 ].Value = item.HorAccuracy;
				row.Cells[ 4 ].Value = item.VerAccuracy;
				row.Cells[ 5 ].Value = item.Elevation;
				row.Cells[ 6 ].Value = item.Moc;
				row.Cells[ 7 ].Value = item.Req_H;
				row.Cells[ 8 ].Value = item.Penetrate;
				if ( item.Penetrate > 0 )
				{
					row.DefaultCellStyle.BackColor = Color.Red;
					row.DefaultCellStyle.SelectionBackColor = Color.Red;
				}
			}

			tsStLblCnt.Text = obstacleReport.Count.ToString ( );
			btnSave.Enabled = obstacleReport.Count > 0;
		}

		public void UptadeReport ( Report report, string altitdueText )
		{
			_report = report;
			UpdateDataGridView ( report.ObstacleReport );
			tsStLblAltitude.Text = altitdueText;

		}

		private void dataGridView1_ColumnHeaderMouseClick ( object sender, DataGridViewCellMouseEventArgs e )
		{
			List<UserReport> tmpReportList;
			try
			{
				PropertyInfo pInfo = typeof ( UserReport ).GetProperty ( dataGridReport.Columns[ e.ColumnIndex ].Tag.ToString ( ) );

				if ( _oldColumnIndex == e.ColumnIndex && _sortOrder == SortOrder.Ascending )
				{
					tmpReportList = _report.ObstacleReport.OrderByDescending ( report => pInfo.GetValue ( report, null ) ).ToList<UserReport> ( );
					_sortOrder = SortOrder.Descending;
				}
				else
				{
					tmpReportList = _report.ObstacleReport.OrderBy ( report => pInfo.GetValue ( report, null ) ).ToList<UserReport> ( );
					_sortOrder = SortOrder.Ascending;
				}
			}
			catch ( Exception excep )
			{
				throw excep;
			}

			_report.ObstacleReport.Clear ( );
			foreach ( var report in tmpReportList )
			{
				_report.ObstacleReport.Add ( report );
			}
			_oldColumnIndex = e.ColumnIndex;
			dataGridReport.Columns[ e.ColumnIndex ].HeaderCell.SortGlyphDirection = _sortOrder;
		}

		private void dataGridReport_RowPrePaint ( object sender, DataGridViewRowPrePaintEventArgs e )
		{
			if ( _report.ObstacleReport[ e.RowIndex ].Req_H <= 0 )
				dataGridReport.Rows[ e.RowIndex ].DefaultCellStyle.BackColor = Color.Red;
		}

		private void dataGridReport_RowEnter ( object sender, DataGridViewCellEventArgs e )
		{
			_controller.SafeDeleteGraphic ( _vsHandle );
			if ( e.RowIndex != -1 && dataGridReport.Rows[e.RowIndex]?.Tag != null)
			{
				//VerticalStructure vs = _report.VerticalStructureList.ElementAt ( e.RowIndex ).Key;
                var selectedObstacle = (UserReport) dataGridReport.Rows[e.RowIndex].Tag; // _report.ObstacleReport[ e.RowIndex ];
				if ( selectedObstacle.GeomPrj is Geometries.Point )
					_vsHandle = GlobalParams.AranEnvironment.Graphics.DrawPoint ( selectedObstacle.GeomPrj as Geometries.Point, ARANFunctions.RGB ( 255, 0, 0 ) );

				else if ( selectedObstacle.GeomPrj is MultiLineString )
					_vsHandle = GlobalParams.AranEnvironment.Graphics.DrawMultiLineString ( selectedObstacle.GeomPrj as MultiLineString, 4, ARANFunctions.RGB ( 255, 0, 0 ));

				else if ( selectedObstacle.GeomPrj is MultiPolygon )
				{
					_vsHandle = GlobalParams.AranEnvironment.Graphics.DrawMultiPolygon (
						selectedObstacle.GeomPrj as MultiPolygon, _polygonFillSymbol, true, false );
				}
			}
		}

		private void dataGridReport_ColumnRemoved ( object sender, DataGridViewColumnEventArgs e )
		{

		}

		private void FormReport_FormClosing ( object sender, FormClosingEventArgs e )
		{
			_controller.SafeDeleteGraphic ( _vsHandle );
			Hide ( );
			e.Cancel = true;
		}

		private void report_RenderingRow ( object sender, ReportRowEventArgs e )
		{
			if ( e.Row.DataItem == null )
				return;
			var penetrate = ( double ) e.Row[ "Penetrate" ];
			if ( penetrate > 0 )
			{
				//e.Row.Fields[ "Id" ].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields[ "Name" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "Elevation" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "Moc" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "Req_H" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "Penetrate" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "Area" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "GeomType" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "HorAccuracy" ].DataStyle.ForeColor = Color.Red;
				e.Row.Fields[ "VerAccuracy" ].DataStyle.ForeColor = Color.Red;
			}
		}

		public void btnSave_Click ( object sender, EventArgs e )
		{
		    SaveReport();
		}

	    public void SaveReport()
	    {
            try
            {
                var doddleReport = new DoddleReport.Report(_report.ObstacleReport.ToReportSource());
                doddleReport.RenderingRow += new EventHandler<ReportRowEventArgs>(report_RenderingRow);

                doddleReport.TextFields.Title = "Conventional " + _controller.SelectedModul;

                doddleReport.TextFields.SubTitle = "";
                doddleReport.TextFields.Footer = "Copyright 2017 &copy; R.I.S.K Company";

                string altitudeText = GlobalParams.UnitConverter.HeightToDisplayUnits(_controller.AltitudeAboveNavaid, eRoundMode.NEAREST) +
                                      " " + GlobalParams.Settings.HeightUnit;
                string speedUnit = "Kt";
                if (GlobalParams.Settings.SpeedUnit == HorizantalSpeedType.KMInHour)
                    speedUnit = "Km/h";
                string iasText = GlobalParams.UnitConverter.SpeedToDisplayUnits(_controller.Speed.Ias, eRoundMode.NEAREST) + " " +
                                 speedUnit;
                string tasText = GlobalParams.UnitConverter.SpeedToDisplayUnits(_controller.Speed.Tas, eRoundMode.NEAREST) + " " +
                                 speedUnit;
                string legLength =
                    GlobalParams.UnitConverter.DistanceToDisplayUnits(_controller.LimDist.LegLength, eRoundMode.NEAREST) + " " +
                    GlobalParams.Settings.DistanceUnit;
                string procType = Resources.MainController_Startup_VOR_VOR;
                if (_controller.ProcType == ProcedureTypeConv.Vordme)
                    procType = Resources.MainController_Startup_VOR_DME;
                else if (_controller.ProcType == ProcedureTypeConv.VorNdb)
                    procType = Resources.MainController_Startup_Overhead;
                string mocText = GlobalParams.UnitConverter.HeightToDisplayUnits(_controller.Moc, eRoundMode.NEAREST) + " " +
                                 GlobalParams.Settings.HeightUnit;

                //string longtitude, latitude;
                //Common.Dd2Dms(_controller.SelectedNavPntsPrj[0].Value.X, _controller.SelectedNavPntsPrj[0].Value.Y, ".", "E", "N", 1,
                //    out longtitude, out latitude);

                
                double navaidElevation = _controller.Faclts.GetNavaidElevation();
                string navElevStr =
                    GlobalParams.UnitConverter.HeightToDisplayUnits(navaidElevation, eRoundMode.NEAREST) + " " +
                    GlobalParams.Settings.HeightUnit;
                string radialDescrpt;
                string dsgPntDescription = "";

                if (_controller.ProcType == ProcedureTypeConv.Vordme)
                {
                    string nominalDist =
                        GlobalParams.UnitConverter.DistanceToDisplayUnits(_controller.DsgPntSelection.NominalDistanceInPrj,
                            eRoundMode.NEAREST) + " " + GlobalParams.Settings.DistanceUnit;
                    radialDescrpt = Math.Round(_controller.DirectionInDeg) + " °";
                    if (_controller.DsgPntSelection.ChosenPntType == PointChoice.Select)
                        dsgPntDescription =
                            $@"Selected Designated Point : {_controller.DsgPntSelection.SelectedDesignatedPoint.Designator} 
							Distance : {nominalDist}
							Radial (Mag.) : {radialDescrpt}";

                    else
                    {
                        dsgPntDescription = $@"Distance : {nominalDist}
							Radial (Mag.) : {radialDescrpt}";
                    }
                    doddleReport.TextFields.Header = string.Format(@"
						Report Generated : {0}
						Aerodrome : {1}
						Procedure type : {2}
						Turn : {3}
						Direction : {4}
						Navaid : {5}
						Elevation of Navaid : {6}
						Length of leg : {7}
						Altitude (from MSL) : {8}
						Limiting distance : {9} {18}
						Aircraft category : {10}
						IAS : {11}
						TAS : {12}
						Time : {13} min
						MOC : {14}
						{15}
						Magnetic value : {16}
						Elevations and heights are in : {17}
						Distances are in : {18}
						",
                        DateTime.Now,
                        _controller.AirportName,
                        procType,
                        (_controller.Side == SideDirection.sideLeft) ? "Left" : "Right",
                        (_controller.EntryDirection == EntryDirection.Toward) ? "Toward" : "Away",
                        _controller.SelectedNavaid.Designator,
                        navElevStr,
                        GlobalParams.UnitConverter.DistanceToDisplayUnits(_controller.LimDist.LegLength, eRoundMode.NEAREST) + " " +
                        GlobalParams.Settings.DistanceUnit,
                        altitudeText,
                        GlobalParams.UnitConverter.DistanceToDisplayUnits(_controller.LimDist.ValueInPrj, eRoundMode.NEAREST),
                        _controller.Speed.SelectedCategory,
                        iasText,
                        tasText,
                        _controller.LimDist.TimeInMin,
                        mocText,
                        dsgPntDescription,
                        _controller.DsgPntSelection.MagVar,
                        GlobalParams.Settings.HeightUnit,
                        GlobalParams.Settings.DistanceUnit);
                }
                else if (_controller.ProcType == ProcedureTypeConv.VorNdb)
                {
                    radialDescrpt = Math.Round(_controller.DirectionInDeg) + " °";
                    dsgPntDescription = $@"Radial (Mag.) : {radialDescrpt}";
                    doddleReport.TextFields.Header = string.Format(@"
						Report Generated : {0}
						Aerodrome : {1}
						Procedure type : {2}
						Turn : {3}
						Navaid : {4}
						Elevation of Navaid : {5}
						Length of leg : {6}
						Altitude (from MSL) : {7}
						Aircraft category : {8}
						IAS : {9}
						TAS : {10}
						Time : {11} min
						MOC : {12}
						{13}
						Elevations and heights are in : {14}
						Distances are in : {15}
						",
                        DateTime.Now,
                        _controller.AirportName,
                        procType,
                        (_controller.Side == SideDirection.sideLeft) ? "Left" : "Right",
                        _controller.SelectedNavaid.Designator,
                        navElevStr,
                        GlobalParams.UnitConverter.DistanceToDisplayUnits(_controller.LimDist.LegLength, eRoundMode.NEAREST) + " " +
                        GlobalParams.Settings.DistanceUnit,
                        altitudeText,
                        _controller.Speed.SelectedCategory,
                        iasText,
                        tasText,
                        _controller.LimDist.TimeInMin,
                        mocText,
                        dsgPntDescription,
                        GlobalParams.Settings.HeightUnit,
                        GlobalParams.Settings.DistanceUnit);
                }
                else
                {
                    doddleReport.TextFields.Header = string.Format(@"
						Report Generated : {0}
						Aerodrome : {1}
						Procedure type : {2}
						Turn : {3}
						Direction : {4}
						Homing VOR : {5}
						Intersecting VOR : {6}
						Length of leg : {7}
						Altitude (from MSL) : {8}
						Aircraft category : {9}
						IAS : {10}
						TAS : {11}
						Time : {12} min
						MOC : {13}
						Elevations and heights are in : {14}
						Distances are in : {15}
						",
                        DateTime.Now,
                        _controller.AirportName,
                        procType,
                        (_controller.Side == SideDirection.sideLeft) ? "Left" : "Right",
                        (_controller.EntryDirection == EntryDirection.Toward) ? "Toward" : "Away",
                        _controller.SelectedNavaid.Designator,
                        _controller.IntersectingVorDsg,
                        GlobalParams.UnitConverter.DistanceToDisplayUnits(_controller.LimDist.LegLength, eRoundMode.NEAREST) + " " +
                        GlobalParams.Settings.DistanceUnit,
                        altitudeText,
                        _controller.Speed.SelectedCategory,
                        iasText,
                        tasText,
                        _controller.LimDist.TimeInMin,
                        mocText,
                        GlobalParams.Settings.HeightUnit,
                        GlobalParams.Settings.DistanceUnit);
                }
                //                report.TextFields.Header = string.Format ( @"
                //                Report Generated : {0}
                //                Aerodrome : {1}
                //                Procedure type : {2}
                //                Altitude(from MSL) : {3}
                //                IAS : {4}
                //                TAS : {5}    
                //                Time(min) : {6}
                //                MOC : {7}
                //                Elevations and heights are in : {8}
                //                Distances are in : {9}
                //                ", DateTime.Now, _controller.airportName, procType, altitudeText, iasText, tasText, _controller.LimDist.TimeInMin, mocText, GlobalParams.Settings.HeightUnit, GlobalParams.Settings.DistanceUnit );

                // Customize the data fields
                //report.DataFields[ "Altitude" ].Hidden = true;
                doddleReport.DataFields["Validation"].Hidden = true;
                doddleReport.DataFields["Obstacle"].Hidden = true;
                doddleReport.DataFields["Location"].Hidden = true;
                doddleReport.DataFields["AreaNumber"].Hidden = true;
                doddleReport.DataFields["GeomPrj"].Hidden = true;
                //doddleReport.DataFields["SurfaceType"].Hidden = true;

                doddleReport.DataFields["Elevation"].HeaderText = "Abs.H";
                doddleReport.DataFields["Moc"].HeaderText = "MOC";
                doddleReport.DataFields["Req_H"].HeaderText = "Req.H";
                doddleReport.DataFields["Penetrate"].HeaderText = "dh Penetrate";


                double d = GlobalParams.UnitConverter.HeightToDisplayUnits(_controller.Moc, eRoundMode.CEIL);
                double guiMoc = Math.Round(d / 100, 0) * 100;

                if (_report.ObstacleReport == null || _report.ObstacleReport.Count == 0)
                {
                    foreach (ReportField reportField in doddleReport.DataFields)
                    {
                        reportField.Hidden = true;
                    }
                    doddleReport.TextFields.Header += "OCA : " + guiMoc + " " + GlobalParams.Settings.HeightUnit;
                    doddleReport.TextFields.Header += "\r\n\r\n\r\n\r\n\r\nNo obstacle found in this holding area";
                }
                else
                {
                    var maxVs = _report.ObstacleReport.OrderByDescending(rep => (rep.Elevation + rep.Moc))
                        .FirstOrDefault();
                    string ocaText;

                    if ((maxVs.Elevation + maxVs.Moc) > guiMoc)
                    {
                        ocaText = (maxVs.Elevation + maxVs.Moc) + " " + GlobalParams.Settings.HeightUnit;
                    }
                    else
                        ocaText = guiMoc + " " + GlobalParams.Settings.HeightUnit;
                    doddleReport.TextFields.Header += "OCA : " + ocaText + " " + GlobalParams.Settings.HeightUnit;
                    doddleReport.TextFields.Header += "\r\nObstacle : " + maxVs.Obstacle.Name;
                }

                var dlg = new SaveFileDialog
                {
                    FileName = "Conventional " + _controller.SelectedModul + " report",
                    DefaultExt = ".text",
                    Title =
                        Resources.MainController_SaveReport_Save_Conventional_ + _controller.SelectedModul +
                        Resources.MainController_SaveReport__Report,
                    Filter = "Html documents|*.htm" +
                             //"|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xlsx"
                };
                // Default file name
                // Default file extension
                // Show save file dialog box

                var result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == DialogResult.OK)
                {
                    System.IO.Stream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);
                    if (dlg.FilterIndex == 1)
                    {
                        var writer = new HtmlReportWriter();
                        writer.WriteReport(doddleReport, stream);
                    }
                    //else if (dlg.FilterIndex == 2)
                    //{
                    //    var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                    //    writer.WriteReport(report, stream);
                    //}
                    else if (dlg.FilterIndex == 2)
                    {
                        var writer = new DoddleReport.OpenXml.ExcelReportWriter();
                        writer.WriteReport(doddleReport, stream);
                    }
                    MessageBox.Show("Document saved successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was error while saving report file");
            }
        }

	    private void txtSearch_TextChanged ( object sender, EventArgs e )
		{
			string query = txtSearch.Text.ToLower ( );

			List<UserReport> searchResult = _report.ObstacleReport.Where (
					c => ( c.Name != null ) && ( c.Name.ToLower ( ).Contains ( query ) ) ||
					//c.Id.ToString ( ).Contains ( query ) ||
					c.Elevation.ToString (CultureInfo.InvariantCulture).Contains ( query ) ||
					c.Moc.ToString (CultureInfo.InvariantCulture).Contains ( query ) ||
					c.Req_H.ToString (CultureInfo.InvariantCulture).Contains ( query ) ||
					c.Penetrate.ToString (CultureInfo.InvariantCulture).Contains ( query ) ||
					c.Area.Contains ( query ) ||
					c.GeomType.ToString ( ).ToLower ( ).Contains ( query ) ||
					c.HorAccuracy.ToString (CultureInfo.InvariantCulture).Contains ( query ) ||
					c.VerAccuracy.ToString (CultureInfo.InvariantCulture).Contains ( query )
				).ToList<UserReport> ( );

			UpdateDataGridView ( searchResult );
		}
	}
}