using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Features;
using Aran.Aim.Data;
using ESRI.ArcGIS.Geodatabase;
using Aran.Aim;

namespace KFileParser
{
	public partial class MainForm : Form
	{
		private Controller _controller;
		private List<int> _checkedList;

		public MainForm ( )
		{
			InitializeComponent ( );
			_controller = new Controller ( );
			_checkedList = new List<int> ( );
		}

		private void MainForm_Load ( object sender, EventArgs e )
		{
			FeatureType[] featTypes = ( FeatureType[] ) Enum.GetValues ( typeof ( FeatureType ) );
			List<AimPropInfo> propInfoList = new List<AimPropInfo> ( );
			foreach ( var featType in featTypes )
			{
				Aran.Aim.Features.Feature feature = AimObjectFactory.CreateFeature ( ( FeatureType ) featType );
				if ( AimMetadata.IsAbstract ( feature ) )
					continue;
				if ( featType == FeatureType.HoldingAssessment )
				{
				}
				propInfoList.AddRange ( CheckFeat ( ( IAimObject ) feature ) );
			}
			_controller.FolderPath = ui_folderTB.Text;
		}

		private List<AimPropInfo> CheckFeat ( IAimObject aimObj )
		{
			List<AimPropInfo> result = new List<AimPropInfo> ( );
				AimPropInfo[] propInfoList = AimMetadata.GetAimPropInfos ( aimObj );
				foreach ( var propInfo in propInfoList )
				{
					if ( AimMetadata.IsAbstract ( propInfo.TypeIndex ) )
						continue;
					//if ( propInfo.TypeIndex == 2052 )
					//	continue;
					AimObject propObj;
					try
					{
						propObj = AimObjectFactory.Create ( propInfo.TypeIndex );
					}
					catch ( Exception ex )
					{
						string msg = ex.Message;
						throw;
					}
					IAimProperty aimProp = ( propObj as IAimProperty );
					if ( aimProp.PropertyType == AimPropertyType.AranField || aimProp.PropertyType == AimPropertyType.DataType )
						continue;
					if ( aimProp.PropertyType == AimPropertyType.Object )
					{
						if ( !_checkedList.Contains ( propInfo.TypeIndex ) )
						{
							_checkedList.Add ( propInfo.TypeIndex );
							result.AddRange ( CheckFeat ( propObj ) );
						}
					}
					else if ( aimProp.PropertyType == AimPropertyType.List )
					{
						if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) || AimMetadata.IsAbstractFeatureRefObject ( propInfo.TypeIndex ) )
						{
						}
					}
				}
			return result;
		}

		private void SelectFolder_Click ( object sender, EventArgs e )
		{
			FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog ( );
			folderBrowserDlg.RootFolder = Environment.SpecialFolder.MyComputer;
			folderBrowserDlg.SelectedPath = @"D:\Work\Kaz\";			
			if ( folderBrowserDlg.ShowDialog ( ) != DialogResult.OK )
				return;
			ui_folderTB.Text = folderBrowserDlg.SelectedPath;
		}

		private void ParseRwy_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertRwyCntLinePoints (chckBxReplaceDecimalWithDot.Checked  );
			ShowMessage ( message );
		}

		private void ParseVS_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertVerticalStructures ( );
			ShowMessage(message);
		}
		

		private void ParseApronData_Click ( object sender, EventArgs e )
		{
			ShowMessage ( _controller.InsertApronElements ( chckBxSortApronElement.Checked, chckBxReplaceDecimalWithDot.Checked ) );
		}

		private void ParseSP_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertAircraftStands (chckBxReplaceDecimalWithDot.Checked  );
			ShowMessage ( message );
		}

		private void ParseGuidanceLine_Apron_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertApronGuidanceLinesWithPart ( chckBxSortGuidanceLineApron.Checked, chckBxReplaceDecimalWithDot.Checked );
			ShowMessage ( message );
		}

		private void ParseTaxiwayGuidanceLine_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertGuidanceLineTaxiway ( chckBxSortGuidanceLineTaxiway.Checked, chckBxReplaceDecimalWithDot.Checked );
			ShowMessage ( message );
		}

		private void btnNavaid_Click ( object sender, EventArgs e )
		{
			ShowMessage ( _controller.InsertNavaidComponentPoints ( chckBxReplaceDecimalWithDot.Checked ) );
		}

		private void btnRawPoint_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertRawPoints (chckBxReplaceDecimalWithDot.Checked );
			ShowMessage ( message );
		}

		private void ui_folderTB_Leave ( object sender, EventArgs e )
		{
			//_featConv.OpenFeatureClass ( System.IO.Path.Combine ( ui_folderTB.Text, "KAZLayers.mdb" ) );
			_controller.FolderPath = ui_folderTB.Text;
		}

		private void btnCntrlPnt_Click ( object sender, EventArgs e )
		{			
			string message = _controller.InsertControlPoints ( );
			ShowMessage ( message );
		}

		private void btnTaxiHoldingPos_Click ( object sender, EventArgs e )
		{
			string message = _controller.GetTaxiHoldingPositions (chckBxReplaceDecimalWithDot.Checked  );
			ShowMessage ( message );
		}

		private void chckBxInsert2DB_CheckedChanged ( object sender, EventArgs e )
		{
			_controller.Insert2DB = chckBxInsert2DB.Checked;
		}

		private void ui_folderTB_TextChanged(object sender, EventArgs e)
		{
			_controller.FolderPath = ui_folderTB.Text;
		}

		private void btn_Click ( object sender, EventArgs e )
		{
			string message = _controller.InserteTodObstacles ( chckBxIsGDB.Checked );
			ShowMessage ( message );
		}

		private void btnArea2VertStructs_Click ( object sender, EventArgs e )
		{
			//string message = _controller.InsertArea2_3Obstacles ( );
			//ShowMessage ( message );
		}

		private void btnArea1_Click ( object sender, EventArgs e )
		{
			string message = _controller.InsertArea1Obstacles ( chckBxReplaceDecimalWithDot.Checked, chckBxArea1ObsFromExcel.Checked );
			ShowMessage ( message );
		}

		private void ShowMessage ( string message )
		{
			if ( message == string.Empty )
				message = "Successfully inserted !";
			MessageBox.Show ( message );
		}

		private void btnSetArea1Types_Click ( object sender, EventArgs e )
		{
			ShowMessage ( _controller.SetArea1ObstacleTypes ( ) );
		}

		private void btnSetArea2_3ObsTypes_Click ( object sender, EventArgs e )
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog ( );
			openFileDialog1.InitialDirectory = System.IO.Path.Combine ( _controller.FolderPath, "Obstacle", "Area 2&3" );
			openFileDialog1.Filter = "Excel files (*.xlsx)|*.kde";			
			//if ( openFileDialog1.ShowDialog ( ) == DialogResult.OK )
			openFileDialog1.FileName = System.IO.Path.Combine ( openFileDialog1.InitialDirectory, "Types.xls" );
			ShowMessage ( _controller.SetArea2_3ObstacleTypes ( openFileDialog1.FileName ) );
		}

		private void btnSetObsAccuracy_Click ( object sender, EventArgs e )
		{
			ShowMessage ( _controller.SetObsAccuracy ( ) );
		}

		private void btnExportToMdb_Click ( object sender, EventArgs e )
		{
			ShowMessage ( _controller.ExportToArcMap ( ) );
		}

		private void chckBxFromMdb_CheckedChanged ( object sender, EventArgs e )
		{
			_controller.SetFromMDB ( chckBxFromMdb.Checked );
		}

		private void btnConvert_Click ( object sender, EventArgs e )
		{
			_controller.ConvertSimData ( );
		}

		private void btnObsRelateOrg_Click ( object sender, EventArgs e )
		{
			_controller.ConnectObstacles2Organisation ( );
		}

		private void btnAirspace_Click ( object sender, EventArgs e )
		{
			_controller.InserAirspaces ( );
		}
	}
}