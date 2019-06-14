using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoddleReport;
using DoddleReport.Writers;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ARENA.TOCLayerView;
using EsriWorkEnvironment;
using PDM;

namespace ChartValidator
{
	public partial class MainForm : Form
	{
		private List<ResultLog> _logList;
		private List<PDMObject> _enrouteList;
		private IMap _map;
		private IApplication _esriApp;
		private List<DataGridView> dgViewList;
		private List<string> _headerList;
		private List<PDMObject> _airspaceList;
        private ChartType _chartType;
		private TOCLayerFilter _arenaToc;

		public MainForm (ChartType chartType)
		{
			InitializeComponent ();
            _chartType = chartType;
            this.Text += " - " + _chartType;
        }

		public void SetData (List<ResultLog> logList, List<PDMObject> dataList, IApplication app)
		{
			dgViewList = new List<DataGridView> ();
			dgViewList.Add (dgViewEnroute);
			dgViewList.Add (dgViewRouteSegment);
			dgViewList.Add (dgViewNavaid);
			dgViewList.Add (dgViewAirspace);
			dgViewList.Add (dgViewHolding);
			dgViewList.Add (dgViewWayPoint);
			dgViewList.Add (dgViewSID);
			dgViewList.Add (dgViewDepartureLeg);
			_esriApp = app;
			FindArenaToc ( );
			IMxDocument document = (IMxDocument) _esriApp.Document;
			_map = document.FocusMap;
			_logList = logList;
			_enrouteList = dataList.Where (pdm => pdm is Enroute).ToList ();
			_airspaceList = dataList.Where (pdm => pdm is Airspace).ToList ();
			_headerList = new List<string> ();
			FilDataGridView ();
		}

		private void FindArenaToc ( )
		{
			for ( int i = 0; i < ( ( IMxDocument ) _esriApp.Document ).ContentsViewCount; i++ )
			{
				var cntsView = ( ( IMxDocument ) _esriApp.Document ).get_ContentsView ( i );

				string cntxtName = cntsView.Name;

				if ( cntxtName.StartsWith ( "TOCLayerFilter" ) )
				{
					_arenaToc = cntsView as TOCLayerFilter;
					break;					
				}
			}
		}

		public ILayer FindLayer (string layerName)
		{
			ILayer result = null;
			for (int i = 0; i <= _map.LayerCount - 1; i++)
			{
				ILayer layer = _map.get_Layer (i);
				if (layer is ICompositeLayer)
				{
					ICompositeLayer Complayer = (ICompositeLayer) layer;
					for (int j = 0; j <= Complayer.Count - 1; j++)
					{
						ILayer layer2 = Complayer.get_Layer (j);
						if (layer2.Name.ToUpper ().CompareTo (layerName.ToUpper ()) == 0)
						{
							result = layer2;
							break;
						}
					}
				}
				else
				{
					if (layer.Name.CompareTo (layerName) == 0)
						result = layer;
				}
			}
			return result;
		}

		private void FilDataGridView ()
		{
			int index;
			try
			{
				for (int i = 0; i < _logList.Count; i++)
				{
					DataGridViewRow row = null;
					if (_logList[i].Feature is PDM.Enroute)
					{
						index = dgViewEnroute.Rows.Add ();
						row = dgViewEnroute.Rows[index];
					}
					else if (_logList[i].Feature is PDM.RouteSegment)
					{
						index = dgViewRouteSegment.Rows.Add ();
						row = dgViewRouteSegment.Rows[index];
					}
					else if (_logList[i].Feature is PDM.NavaidSystem)
					{
						index = dgViewNavaid.Rows.Add ();
						row = dgViewNavaid.Rows[index];
					}
					else if (_logList[i].Feature is PDM.Airspace || _logList[i].Feature is PDM.AirspaceVolume)
					{
						index = dgViewAirspace.Rows.Add ();
						row = dgViewAirspace.Rows[index];
					}
					else if (_logList[i].Feature is PDM.HoldingPattern)
					{
						index = dgViewHolding.Rows.Add ();
						row = dgViewHolding.Rows[index];
					}
					else if (_logList[i].Feature is PDM.WayPoint)
					{
						index = dgViewWayPoint.Rows.Add ();
						row = dgViewWayPoint.Rows[index];
					}
					else if (_logList[i].Feature is PDM.StandardInstrumentDeparture || _logList[i].Feature is PDM.StandardInstrumentArrival)
					{
						index = dgViewSID.Rows.Add ();
						row = dgViewSID.Rows[index];
					}
					else if (_logList[i].Feature is PDM.ProcedureLeg)
					{
						index = dgViewDepartureLeg.Rows.Add ();
						row = dgViewDepartureLeg.Rows[index];
					}
					if (row == null)
					{ 
					}
					row.Tag = _logList[i].Feature;

					if (row.Cells[0].OwningColumn is DataGridViewCheckBoxColumn)
					{
						row.Cells[1].Value = _logList[i].Name;
						row.Cells[2].Value = _logList[i].ReportText;
					}
					else
					{
						row.Cells[0].Value = _logList[i].Name;
						row.Cells[1].Value = _logList[i].ReportText;
						if(_logList[i].ReportText == "")
						{

						}
					}					
				}

				for (int i = 0; i < tbCntrlMain.TabPages.Count; i++)					
				//int k = 0;
				//while(k < tbCntrlMain.TabPages.Count)
				{
					if (dgViewList[i].Rows.Count == 0)
					{
						tbCntrlMain.TabPages.RemoveAt (i);
						dgViewList.RemoveAt (i);
						i--;
					}
					else
					{
						_headerList.Add (tbCntrlMain.TabPages[i].Text);
						//k++;
					}
				}
				tbCntrlMain_SelectedIndexChanged (null, null);
			}
			catch (Exception ex)
			{
				var t = ex.Message;
				throw;
			}


		}

		private void btnOk_Click (object sender, EventArgs e)
		{
			this.Close ();
		}

		private void ZoomTo (ILayer layer, List<string> idList)
		{
			try
			{
				_map.ClearSelection ();
				var pSelect = layer as IFeatureSelection;
				if (pSelect != null)
				{
					pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
					var s = "( ";
					for (var i = 0; i < idList.Count; i++)
						s += "\"" + idList[i] + "\",";
					s = s.Remove (s.Length - 1) + ")";

					IQueryFilter queryFilter = new QueryFilterClass ();
					queryFilter.WhereClause = "FeatureGUID in " + s;

					pSelect.SelectFeatures (queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

					UID menuID = new UIDClass ();

					menuID.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}";

					ICommandItem pCmdItem = _esriApp.Document.CommandBars.Find (menuID);
					pCmdItem.Execute ();
					Marshal.ReleaseComObject (pCmdItem);
					Marshal.ReleaseComObject (menuID);

					//if (pSelect.SelectionSet.Count > 0)
					//{
					//    var zoomToSelected = new ControlsZoomToSelectedCommandClass();
					//    zoomToSelected.OnCreate(m_application);
					//    zoomToSelected.OnClick();
					//}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine (ex.StackTrace + " ERROR " + ex.Message);
				throw;
			}
		}

		private void PanTo (ILayer layer, List<string> idList)
		{
			try
			{
				_map.ClearSelection ();
				if (idList.Count == 0)
					return;
				var pSelect = layer as IFeatureSelection;
				if (pSelect != null)
				{
					pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
					var s = "( ";
					for (var i = 0; i < idList.Count; i++)
						s += "\"" + idList[i] + "\",";
					s = s.Remove (s.Length - 1) + ")";

					IQueryFilter queryFilter = new QueryFilterClass ();
					queryFilter.WhereClause = "FeatureGUID in " + s;

					pSelect.SelectFeatures (queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

					UID menuID = new UIDClass ();

					menuID.Value = "{BF64319A-9062-11D2-AE71-080009EC732A}";

					ICommandItem pCmdItem = _esriApp.Document.CommandBars.Find (menuID);
					pCmdItem.Execute ();
					Marshal.ReleaseComObject (pCmdItem);
					Marshal.ReleaseComObject (menuID);

					//if (pSelect.SelectionSet.Count > 0)
					//{
					//    var zoomToSelected = new ControlsZoomToSelectedCommandClass();
					//    zoomToSelected.OnCreate(m_application);
					//    zoomToSelected.OnClick();
					//}

				}
				((IActiveView) _map).Refresh ();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine (ex.StackTrace + " ERROR " + ex.Message);
				throw;
			}
		}

		private void dgViewEnroute_RowEnter (object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow selectedRow = dgViewEnroute.Rows[e.RowIndex];
			ILayer layer = FindLayer("RouteSegment"); ;
            List<string> ids = ((Enroute)selectedRow.Tag).Routes.Select(route => route.ID).ToList();
			//ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
			PanTo (layer, ids);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
		}

        private void dgViewRouteSegment_RowEnter (object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow selectedRow = dgViewRouteSegment.Rows[e.RowIndex];
			var layer = FindLayer ("RouteSegment");
			//ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
			List<string> idList = new List<string> ();
			idList.Add ((selectedRow.Tag as PDM.RouteSegment).ID);
			PanTo (layer, idList);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
		}

		private void dgViewNavaid_RowEnter (object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow selectedRow = dgViewNavaid.Rows[e.RowIndex];
			var tmp = (selectedRow.Tag as PDM.NavaidSystem);
			var layer = FindLayer ("AirspaceVolume");
			//ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
			List<string> idList = new List<string> ();
			idList.Add ((selectedRow.Tag as PDM.NavaidSystem).ID);
			PanTo (layer, idList);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject).ID );
		}

		private void dgViewAirspace_RowEnter (object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow selectedRow = dgViewAirspace.Rows[e.RowIndex];
			var layer = FindLayer ("AirspaceVolume");
			//ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
			List<string> idList = (selectedRow.Tag as PDM.Airspace).AirspaceVolumeList.ConvertAll (pdm => pdm.ID).ToList ();
			PanTo (layer, idList);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
		}

		private void dgViewHolding_RowEnter (object sender, DataGridViewCellEventArgs e)
		{
			//DataGridViewRow selectedRow = dgViewAirspace.Rows[e.RowIndex];
			//var layer = FindLayer ("AirspaceVolume");
			////ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
			//List<string> idList = new List<string> ();
			//idList.Add ((selectedRow.Tag as PDM.Airspace).ID);
			//PanTo (layer, idList);
			//( _cntsView as TOCLayerFilter ).SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
		}

		private void dgViewWayPoint_RowEnter (object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow selectedRow = dgViewWayPoint.Rows[e.RowIndex];
			var layer = FindLayer ("WayPoint");
			//ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
			List<string> idList = new List<string> ();
			idList.Add ((selectedRow.Tag as PDM.WayPoint).ID);
			PanTo (layer, idList);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
		}

        private void dgViewSID_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dgViewSID.Rows[e.RowIndex];
            var layer = FindLayer("ProcedureLegs");
            List<string> idList = null;
            if (_chartType == ChartType.SID)
                idList = ((StandardInstrumentDeparture)selectedRow.Tag).Transitions.SelectMany(transition => transition.Legs).Select(leg => leg.ID).ToList();
            else
                idList = ((StandardInstrumentArrival)selectedRow.Tag).Transitions.SelectMany(transition => transition.Legs).Select(leg => leg.ID).ToList();
            PanTo(layer, idList);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
        }

        private void dgViewDepartureLeg_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dgViewDepartureLeg.Rows[e.RowIndex];
            var layer = FindLayer("ProcedureLegs");
            //ZoomTo (layer, (selectedRow.Tag as PDM.RouteSegment).ID);
            List<string> idList = new List<string>();
            idList.Add((selectedRow.Tag as PDMObject).ID);
            PanTo(layer, idList);
			_arenaToc.SelectNode ( ( selectedRow.Tag as PDMObject ).ID );
        }
        private void tbCntrlMain_SelectedIndexChanged (object sender, EventArgs e)
		{
			lblCount.Text = "Warning count : " + dgViewList[tbCntrlMain.SelectedIndex].Rows.Count;
			lblDescription.Visible = (tbCntrlMain.SelectedTab.Name == "tabPageRouteSegment");
			//tbCntrlMain.SelectedTab.Text = _headerList[tbCntrlMain.SelectedIndex];
			//for (int i = 0; i < tbCntrlMain.TabPages.Count; i++)
			//{
				//if (i != tbCntrlMain.SelectedIndex)
				//{
				//	if (dgViewList[i].Rows.Count != 0)
				//		tbCntrlMain.TabPages[i].Text = _headerList[i] + " - " + dgViewList[i].Rows.Count;
				//}
			//}
		}
		
		private void btnInfo_Click (object sender, EventArgs e)
		{
			FormInfo frmInfo = new FormInfo (_chartType);
			frmInfo.ShowDialog ();
		}

		private void btnSave_Click (object sender, EventArgs e)
		{
			try
			{
				var report = new DoddleReport.Report (_logList.ToReportSource ());

				report.TextFields.Title = "Validation Tool Report ";

				report.TextFields.SubTitle = "";
				report.TextFields.Footer = "Copyright 2015 &copy; R.I.S.K Company";


				report.TextFields.Header = string.Format (@"Report Generated : {0}", DateTime.Now.ToLocalTime ().ToShortDateString ());
				
				// Customize the data fields
				report.DataFields["Feature"].Hidden = true;
				report.DataFields["DescriptionList"].Hidden = true;
				report.DataFields["ReportType"].Hidden = true;
				report.DataFields["ConflictedRouteSegments"].Hidden = true;
				report.DataFields["Type"].Hidden = true;

				report.DataFields["FeatName"].HeaderText = "Feature type";
				report.DataFields["Name"].HeaderText = "Name";
				report.DataFields["ReportText"].HeaderText = "Detail";


				var dlg = new SaveFileDialog ();
				dlg.FileName = "Validation Report " + DateTime.Now.ToLocalTime().ToShortDateString().Replace('/',' '); // Default file name
				dlg.DefaultExt = ".text"; // Default file extension
				dlg.Title = "Save validation report ";
				dlg.Filter = "Html documents|*.htm" +
							 "|Pdf document|*.pdf" +
							 "|Excel Worksheets|*.xls";
				// Show save file dialog box

				var result = dlg.ShowDialog ();

				// Process save file dialog box results
				if (result == DialogResult.OK)
				{
					System.IO.Stream stream = new System.IO.FileStream (dlg.FileName, System.IO.FileMode.OpenOrCreate);
					if (dlg.FilterIndex == 1)
					{
						var writer = new HtmlReportWriter ();
						writer.WriteReport (report, stream);
					}
					else if (dlg.FilterIndex == 2)
					{
						var writer = new DoddleReport.iTextSharp.PdfReportWriter ();
						writer.WriteReport (report, stream);
					}
					else if (dlg.FilterIndex == 3)
					{
						var writer = new ExcelReportWriter ();
						writer.WriteReport (report, stream);
					}
					MessageBox.Show ("Document saved successfully");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show ("There was error while saving report file\r\n" + ex.ToString ());
			}
		}

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
    }
}
