using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Aran.Aim.Enums;
using Aran.PANDA.Common;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.Aim.Data;
using System.Data;
using System.Data.OleDb;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using Aran.Converters;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections;
using Aran.Aim.Data.Filters;
using Aran.Aim;
using System.Text.RegularExpressions;
using System.Diagnostics;
using ESRI.ArcGIS.esriSystem;

namespace KFileParser
{
	public class Controller
	{
		public Controller ( )
		{
			_featConv = new FeatureConverter ( );
			_dbWriter = new DbWriter ( );
			_utmValues = new Dictionary<string, double> ( );
			_loadedUtmFileNames = new List<string> ( );
			_report = new List<string> ( );
		}

		internal string InsertRwyCntLinePoints ( bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				List<LineData> rwyElementLnDataList;
				List<LineData> rwyCentPoints = ParseFolder ( System.IO.Path.Combine ( FolderPath, "RWY_CenterLinePoint" ), false, replaceDecSeparatorWithDot );
#if !EXPERT_DATA
			for ( int i = 0; i < rwyCentPoints.Count; i++ )
			{
				SetElev_MSL ( rwyCentPoints[ i ] );
			}
#endif
				LineData arpLnData;
				List<LineData> result = GetRwyCntPoints ( rwyCentPoints, out rwyElementLnDataList, out arpLnData );
				if ( Insert2DB )
				{
					int transaction = _dbWriter.DbProvider.BeginTransaction ( );
					if ( arpLnData != null )
						_dbWriter.InsertArp ( arpLnData, transaction );
					_dbWriter.InsertRwyCntrPoints ( result, transaction );
					_dbWriter.InsertRunwayElement ( rwyElementLnDataList, transaction );
					_dbWriter.DbProvider.Commit ( transaction );
				}
				Dictionary<string, List<LineData>> dict = new Dictionary<string, List<LineData>> ( );
				dict.Add ( "RWY Element", rwyElementLnDataList );
				_featConv.AddApronElement ( dict, false );
				_featConv.AddRunwayCenterLinePoint ( result );
				return string.Empty;

			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal void ConvertSimData ( )
		{
			//IWorkspace workspace;
			//IWorkspaceFactory2 workspaceFactory;
			//IFeatureWorkspace featureWorkspace;
			IFeatureClass featureClass;
			//// Open the directory as a shapefile workspace.
			//workspaceFactory = ( IWorkspaceFactory2 ) new AccessWorkspaceFactoryClass ( );
			////workspace = workspaceFactory.OpenFromFile ( System.IO.Path.Combine ( FolderPath, "Obstacle" ), 0 );
			//workspace = workspaceFactory.OpenFromFile ( System.IO.Path.Combine ( FolderPath, "sim_to_adm.mdb" ), 0 );
			//featureWorkspace = ( IFeatureWorkspace ) workspace;

			IAoInitialize ao = new AoInitialize ( );
			ao.Initialize ( esriLicenseProductCode.esriLicenseProductCodeBasic );// .esriLicenseProductCodeArcView );
			IWorkspaceFactory wf = new AccessWorkspaceFactory ( );
			IFeatureWorkspace featureWorkspace = wf.OpenFromFile ( System.IO.Path.Combine ( FolderPath, "sim_to_adm.mdb" ), 0 ) as IFeatureWorkspace;


			featureClass = featureWorkspace.OpenFeatureClass ( "taxiwaypath_pln" );
			int handleIndex = featureClass.FindField ( "Handle" );
			int field50Index = featureClass.FindField ( "Field50" );
			IFeatureCursor featCursor = featureClass.Search ( null, true );
			IFeature feat = featCursor.NextFeature ( );
			int transaction = _dbWriter.DbProvider.BeginTransaction ( );
			InsertGuidanceLine ( handleIndex, field50Index, featCursor, feat, transaction );


			featureClass = featureWorkspace.OpenFeatureClass ( "apron_pgn" );
			int objIdIndex = featCursor.FindField ( "OBJECTID" );
			featCursor = featureClass.Search ( null, true );
			feat = featCursor.NextFeature ( );
			InsertApronElements ( featCursor, feat, objIdIndex, transaction );


			featureClass = featureWorkspace.OpenFeatureClass ( "runway_pgn" );
			featCursor = featureClass.Search ( null, true );
			feat = featCursor.NextFeature ( );
			if ( feat != null )
			{
				RunwayElement runwayElement = ( RunwayElement ) Global.CreateFeature ( FeatureType.RunwayElement );
				runwayElement.AssociatedRunway.Add ( new Aran.Aim.Objects.FeatureRefObject ( new Guid ("0556d520-7b59-4b38-a334-f24d2493ad47" ) ) );
				runwayElement.Extent = new ElevatedSurface ( );
				IPolygon polygon = ( IPolygon ) feat.Shape;
				polygon = _featConv.SimplifyPolygon ( polygon );
				Geometry geom = ConvertFromEsriGeom.ToPolygonGeo ( polygon );
				if ( geom.Type == GeometryType.Polygon )
					runwayElement.Extent.Geo.Add ( ( Aran.Geometries.Polygon ) geom );
				else
				{
					foreach ( Aran.Geometries.Polygon plygn in ( MultiPolygon ) geom )
						runwayElement.Extent.Geo.Add ( plygn );
				}
				_dbWriter.DbProvider.Insert ( runwayElement, transaction );
			}

			_dbWriter.DbProvider.Commit ( transaction );
		}

		private void InsertApronElements ( IFeatureCursor featCursor, IFeature feat, int objIdIndex, int transaction )
		{
			InsertingResult dbResult;
			Dictionary<ApronElement, Guid> apronList = new Dictionary<ApronElement, Guid> ( );
			while ( feat != null )
			{
				Apron apron = ( Apron ) Global.CreateFeature ( FeatureType.Apron );
				object obj = feat.get_Value ( objIdIndex );
				apron.Name = obj.ToString ( );
				dbResult = _dbWriter.DbProvider.Insert ( apron, transaction );
				if ( !dbResult.IsSucceed )
					throw new Exception ( dbResult.Message );

				ApronElement apronElement = ( ApronElement ) Global.CreateFeature ( FeatureType.ApronElement );
				apronElement.Extent = new ElevatedSurface ( );
				IPolygon polygon = ( IPolygon ) feat.Shape;
				polygon = _featConv.SimplifyPolygon ( polygon );
				Geometry geom = ConvertFromEsriGeom.ToPolygonGeo ( polygon );
				if ( geom.Type == GeometryType.Polygon )
					apronElement.Extent.Geo.Add ( ( Aran.Geometries.Polygon ) geom );
				else
				{
					foreach ( Aran.Geometries.Polygon plygn in ( MultiPolygon ) geom )
						apronElement.Extent.Geo.Add ( plygn );
				}

				dbResult = _dbWriter.DbProvider.Insert ( apronElement, transaction );
				if ( !dbResult.IsSucceed )
					throw new Exception ( dbResult.Message );

				apronList.Add ( apronElement, apron.Identifier );

				feat = featCursor.NextFeature ( );
			}

			foreach ( KeyValuePair<ApronElement,Guid> keyVal in apronList )
			{
				keyVal.Key.AssociatedApron = new Aran.Aim.DataTypes.FeatureRef ( keyVal.Value );
				dbResult = _dbWriter.DbProvider.Update ( keyVal.Key, transaction );
				if ( !dbResult.IsSucceed )
					throw new Exception ( dbResult.Message );

			}
		}

		private void InsertGuidanceLine ( int handleIndex, int field50Index, IFeatureCursor featCursor, IFeature feat, int transaction )
		{
			InsertingResult dbResult;
			Dictionary<GuidanceLine, Guid> guidanceLineList = new Dictionary<GuidanceLine, Guid> ( );
			while ( feat != null )
			{
				GuidanceLine guidanceLine = ( GuidanceLine ) Global.CreateFeature ( FeatureType.GuidanceLine );
				guidanceLine.Designator = Get_eTodFeatValueAsString ( feat, handleIndex );

				Aran.Geometries.MultiLineString mltLnString = ConvertFromEsriGeom.ToPolyline ( ( IPolyline ) feat.Shape, true );
				ElevatedCurve curve = new ElevatedCurve ( );
				foreach ( LineString part in mltLnString )
					curve.Geo.Add ( part );
				guidanceLine.Extent = curve;

				dbResult = _dbWriter.DbProvider.Insert ( guidanceLine, transaction );
				if ( !dbResult.IsSucceed )
					throw new Exception ( dbResult.Message );

				Taxiway taxiway = ( Taxiway ) Global.CreateFeature ( FeatureType.Taxiway );
				taxiway.Designator = guidanceLine.Designator;
				taxiway.Width = new Aran.Aim.DataTypes.ValDistance ( Get_eTodFeatValueAsDouble ( feat, field50Index ), UomDistance.M );
				taxiway.AssociatedAirportHeliport = new Aran.Aim.DataTypes.FeatureRef ( new Guid ( "e1424c7e-58a0-48dc-b657-25474f361d17" ) );
				dbResult = _dbWriter.DbProvider.Insert ( taxiway, transaction );
				if ( !dbResult.IsSucceed )
					throw new Exception ( dbResult.Message );

				guidanceLineList.Add ( guidanceLine, taxiway.Identifier );

				feat = featCursor.NextFeature ( );
			}

			foreach ( KeyValuePair<GuidanceLine, Guid> keyVal in guidanceLineList )
			{
				keyVal.Key.ConnectedTaxiway.Add ( new Aran.Aim.Objects.FeatureRefObject ( keyVal.Value ) );
				dbResult = _dbWriter.DbProvider.Update ( keyVal.Key, transaction );
				if ( !dbResult.IsSucceed )
					throw new Exception ( dbResult.Message );
			}
		}

		internal string SetArea1ObstacleTypes ( )
		{
			var filteredFiles = Directory.EnumerateFiles ( System.IO.Path.Combine ( FolderPath, "Obstacle", "Area 1" ), "*.xls", SearchOption.AllDirectories );
			string commandText = "SELECT * FROM [Sheet1$]";
			string connectionString;
			DataTable dataTable;
			List<LineData> lnDataList = new List<LineData> ( );
			foreach ( var item in filteredFiles )
			{
				connectionString = string.Format ( @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=		{0};Extended Properties={1}Excel 12.0;HDR=NO{2}", item, Convert.ToChar ( 34 ), Convert.ToChar ( 34 ) );
				dataTable = new DataTable ( );
				using ( OleDbConnection connection = new OleDbConnection ( connectionString ) )
				{
					try
					{
						connection.Open ( );
						using ( OleDbDataAdapter adapter = new OleDbDataAdapter ( commandText, connection ) )
						{
							adapter.Fill ( dataTable );
							foreach ( DataRow row in dataTable.Rows )
							{
								if ( row[ 0 ].ToString ( ) != "" )
								{
									LineData lnData = new LineData ( );
									lnData.Id = row[ 0 ].ToString ( ).Trim ( );
									lnData.Code = row[ 1 ].ToString ( );
									lnDataList.Add ( lnData );
								}
							}
						}
					}
					catch ( Exception ex )
					{
						return ( item + " file contains error: " + ex.Message );
					}
					finally
					{
						connection.Dispose ( );
						connection.Close ( );
					}
				}
			}
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.Like, "Name", ArpCode.Substring ( 2, 2 ) + "%U" );
			OperationChoice operChoice = new OperationChoice ( compOper );
			Filter filter = new Filter ( operChoice );
			string result = _dbWriter.SetObstacleTypes ( lnDataList, filter, true );
			return result;
		}

		internal string SetObsAccuracy ( )
		{
			return _dbWriter.SetObsAccuracy ( );
		}

		internal string SetArea2_3ObstacleTypes ( string fileName )
		{
			DataTable dataTable = ImportDataTable ( fileName );
			List<LineData> lnDataList = new List<LineData> ( );
			string type;
			DataRow row;
			string frangible;
			string marking;
			for ( int i = 1; i < dataTable.Rows.Count; i++ )
			{
				row = dataTable.Rows[ i ];
				if ( row[ 0 ].ToString ( ) != "" )
				{
					LineData lnData = new LineData ( );
					lnData.Id = row[ 0 ].ToString ( ).Trim ( );
					type = row[ 2 ].ToString ( );
					if ( type == "" )
						lnData.Code = "";
					else
						lnData.Code = _codeVerticalStructures[ type ].ToString ( );
					if ( row[ 3 ] != DBNull.Value )
					{
						frangible = row[ 3 ].ToString ( );
						if ( frangible == "NO" )
							lnData.Frangible = false;
						else if ( frangible == "YES" )
							lnData.Frangible = true;
					}
					if ( row[ 4 ] != DBNull.Value )
					{
						lnData.Colour = row[ 4 ].ToString ( );
					}
					if ( row[ 5 ] != DBNull.Value )
					{
						marking = row[ 5 ].ToString ( );
						if ( marking == "Y" )
							lnData.MarkingICAOStandard = true;
					}
					lnDataList.Add ( lnData );
				}
			}
			//lnDataList.RemoveAt ( 0 );
			ComparisonOps compOperName = new ComparisonOps ( ComparisonOpType.Like, "Name", ArpCode.Substring ( 2, 2 ) + "%" );
			OperationChoice operChoiceName = new OperationChoice ( compOperName );

			ComparisonOps compOperType = new ComparisonOps ( ComparisonOpType.NotLike, "Name", "%U" );
			OperationChoice operChoiceType = new OperationChoice ( compOperType );

			BinaryLogicOp logiOper = new BinaryLogicOp ( );
			logiOper.Type = BinaryLogicOpType.And;
			logiOper.OperationList.Add ( operChoiceName );
			logiOper.OperationList.Add ( operChoiceType );

			OperationChoice operChoiceMain = new OperationChoice ( logiOper );
			Filter filter = new Filter ( operChoiceMain );

			string result = _dbWriter.SetObstacleTypes ( lnDataList, filter, false );
			return result;
		}

		internal string InsertNavaidComponentPoints ( bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				List<LineData> result = new List<LineData> ( );
				List<LineData> vor_dmeList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Navaid", "VOR_DME" ), false, replaceDecSeparatorWithDot );

				List<LineData> ndb_MrkList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Navaid", "NDB" ), false, replaceDecSeparatorWithDot );
				List<LineData> ils_dmeList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Navaid", "ILS_DME" ), false, replaceDecSeparatorWithDot );
				result.AddRange ( vor_dmeList );
				result.AddRange ( ndb_MrkList );
				result.AddRange ( ils_dmeList );

#if !EXPERT_DATA
			foreach ( var item in result )
				SetElev_MSL ( item );
#endif
				if ( Insert2DB )
				{
					Dictionary<string, List<LineData>> dict = new Dictionary<string, List<LineData>> ( );
					foreach ( var item in result )
					{
						dict.Add ( item.Id, new List<LineData> ( ) { item } );
					}
					//List<VerticalStructure> vertStructList = new List<VerticalStructure> ( );
					//_featConv.AddVS_Point ( dict, ref vertStructList );
					//_dbWriter.InsertVerticalStructures ( vertStructList );
					FormNavComponent frmNavComp = new FormNavComponent ( _dbWriter.DbProvider, _dbWriter.Airport.Name );
					if ( frmNavComp.ShowDialog ( vor_dmeList, ils_dmeList, ndb_MrkList ) == System.Windows.Forms.DialogResult.OK )
					{
						_dbWriter.InsertNavComponents ( frmNavComp.NavComponentPath );
					}
				}
				//_featConv.AddNavaid ( result );
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal string InsertVerticalStructures ( )
		{
			try
			{
				List<VerticalStructure> vertStructList = new List<VerticalStructure> ( );

				Dictionary<string, List<LineData>> pointDict = GetVertStuctsType ( "Point" );
				List<VerticalStructure> pointVertStructList = new List<VerticalStructure> ( );
				_featConv.AddVS_Point ( pointDict, ref pointVertStructList );
				vertStructList.AddRange ( pointVertStructList );


				Dictionary<string, List<LineData>> lineDict = GetVertStuctsType ( "Line" );
				List<VerticalStructure> lineVertStructList = new List<VerticalStructure> ( );
				_featConv.AddVS_Line ( lineDict, ref lineVertStructList );
				vertStructList.AddRange ( lineVertStructList );

				Dictionary<string, List<LineData>> polygonDict = GetVertStuctsType ( "Polygon" );
				List<VerticalStructure> polygonVertStructList = new List<VerticalStructure> ( );
				_featConv.AddVS_Polygon ( polygonDict, ref polygonVertStructList );
				vertStructList.AddRange ( polygonVertStructList );

				if ( Insert2DB )
					_dbWriter.InsertVerticalStructures ( vertStructList );
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal void ConnectObstacles2Organisation ( )
		{
			_dbWriter.ConnectObstacles2Organisation ( );
		}

		//internal string InsertArea2_3Obstacles ( )
		//{
		//	//string fileName = System.IO.Path.Combine ( FolderPath, "Obstacle", "Area 2&3", ArpCode + "_Obstacle.xlsx" );
  //          string fileName = System.IO.Path.Combine(FolderPath, "abu-dhabi obstacles.xlsx");
		//	if ( !File.Exists ( fileName ) )
		//		throw new Exception ( "File is not exists :/n" + fileName );
		//	//string commandText = "SELECT * FROM [Лист1$]";
		//	string connectionString;
		//	DataTable dataTable;
		//	List<LineData> lnDataList = new List<LineData> ( );
		//	connectionString = string.Format ( @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties={1}Excel 12.0;HDR=NO{2}", fileName, Convert.ToChar ( 34 ), Convert.ToChar ( 34 ) );
		//	dataTable = new DataTable ( );

  //          Microsoft.Office.Interop.Excel.Application ExcelObj = new Microsoft.Office.Interop.Excel.Application ( );
		//	Microsoft.Office.Interop.Excel.Workbook theWorkbook = null;
		//	theWorkbook = ExcelObj.Workbooks.Open ( fileName);
		//	Microsoft.Office.Interop.Excel.Sheets sheets = theWorkbook.Worksheets;
		//	Microsoft.Office.Interop.Excel.Worksheet worksheet = ( Microsoft.Office.Interop.Excel.Worksheet ) sheets.get_Item ( 1 );//Get the reference of second worksheet
		//	string strWorksheetName = worksheet.Name;//Get the name of worksheet.

		//	string commandText = "SELECT * FROM [" + strWorksheetName + "$]";
		//	string id;
  //          string x, y;
		//	using ( OleDbConnection connection = new OleDbConnection ( connectionString ) )
		//	{
		//		try
		//		{
		//			connection.Open ( );
		//			using ( OleDbDataAdapter adapter = new OleDbDataAdapter ( commandText, connection ) )
		//			{
		//				adapter.Fill ( dataTable );
  //                      string obs_type;
  //                      bool isFirstRow = true;
		//				foreach ( DataRow row in dataTable.Rows )
		//				{
  //                          if (isFirstRow)
  //                          {
  //                              isFirstRow = false;
  //                              continue; 
  //                          }
                                
		//					id = row[ 0 ].ToString ( );
		//					if ( id.Trim() != "" && !id.ToLower().StartsWith("id"))
		//					{
		//						LineData lnData = new LineData ( );
		//						lnData.Id = id;
  //                              //lnData.Y = CoordStrToDouble ( row[ 1 ].ToString ( ) + " " + row[ 2 ].ToString ( ) + " " + row[ 3 ].ToString ( ) );
  //                              //lnData.X = CoordStrToDouble ( row[ 4 ].ToString ( ) + " " + row[ 5 ].ToString ( ) + " " + row[ 6 ].ToString ( ) );
		//						//lnData.Z = double.Parse ( row[ 7 ].ToString ( ) );
  //                              //x = row[1].ToString().Replace("°", "");
  //                              //x = x.Replace("'", "");
  //                              //x = x.Replace(@"""", "");
  //                              //x = x.Replace("E", "");
  //                              //x.TrimEnd();

  //                              //x = row[2].ToString().Insert(3, " ");
  //                              //x = x.Insert(6, " ");
  //                              //x = x.Replace("E", "");
  //                              //x.TrimEnd();

  //                              //y = row[1].ToString().Insert(2, " ");
  //                              //y = y.Insert(5, " ");
  //                              //y = y.Replace("N", "");
  //                              //y.TrimEnd();
  //                              x = row[19].ToString() + " " + row[20].ToString() + " " + row[21].ToString();
  //                              y = row[15].ToString() + " " + row[16].ToString() + " " + row[17].ToString();
  //                              lnData.Y = CoordStrToDouble(y);
  //                              lnData.X = CoordStrToDouble(x);
  //                              lnData.Z = double.Parse(row[7].ToString()) * 0.3048;
  //                              lnData.Z_MSL = double.Parse(row[6].ToString()) * 0.3048;
  //                              lnData.Code = "POINT";
  //                              //lnData.ZoneCode = row[1].ToString().ToUpper();
  //                              //column8 = row[ 8 ].ToString ( );
  //                              //lnData.Code = column8;
  //                              //obs_type = row[9].ToString();
  //                              //if (obs_type != "")
  //                              //    lnData.ZoneCode = obs_type;

		//						//column9 = row[ 9 ].ToString ( );
		//						//if ( column9 == "D" )
		//						//{
		//						//	lnData.IsUp = false;							
		//						//	lnData.Code = row[ 8 ].ToString ( );
		//						//}
		//						//else if ( column9 == "U" )
		//						//{
		//						//	lnData.IsUp = true;
		//						//	lnData.Code = row[ 8 ].ToString ( );
		//						//}
		//						//else
		//						//{
		//						//	lnData.Code = column9;
		//						//	//lnData.IsUp = true;
		//						//}
		//						lnDataList.Add ( lnData );
		//					}
		//				}
		//			}
		//		}
		//		catch ( Exception ex )
		//		{
		//			return ( fileName + " file contains error: " + ex.Message );
		//		}
		//		finally
		//		{
		//			connection.Dispose ( );
		//			connection.Close ( );
		//		}
		//	}
		//	List<string> nonInsertKeys = new List<string> ( );
		//	List<VerticalStructure> vertStructList = new List<VerticalStructure> ( );
		//	List<string> tmpReportList;

  //          //Dictionary<string, List<LineData>> lineDict = CreateDictionary(lnDataList.FindAll(lineData => lineData.Code == "LINE"));
  //          //tmpReportList = _featConv.AddVS_Line(lineDict, ref vertStructList, false, true);
  //          //if (tmpReportList.Count > 0)
  //          //{
  //          //    nonInsertKeys.Add("Below are obstacles that defined as polyline but contains a point");
  //          //    nonInsertKeys.AddRange(tmpReportList);
  //          //}

		//	Dictionary<string, List<LineData>> pntDict = CreateDictionary ( lnDataList.FindAll ( lineData => lineData.Code == "POINT" ) );
		//	nonInsertKeys.AddRange (  _featConv.AddVS_Point ( pntDict, ref vertStructList, true ));

  //          //Dictionary<string, List<LineData>> polygonDict = CreateDictionary ( lnDataList.FindAll ( lineData => lineData.Code == "POLIGON" ) );
  //          //tmpReportList = _featConv.AddVS_Polygon ( polygonDict, ref vertStructList, false, true );
  //          //if ( tmpReportList.Count > 0 )
  //          //{
  //          //    nonInsertKeys.Add ( "Below are obstacles that defined as polygon but contains fewer than 3 points" );
  //          //    nonInsertKeys.AddRange ( tmpReportList );
  //          //}

		//	CreateReport ( nonInsertKeys );
		//	//CreateSqlCommand4DeleteObstacles ( vertStructList );
		//	if ( Insert2DB )
		//	{
		//		InsertingResult insertResult = _dbWriter.InsertVerticalStructures ( vertStructList );

		//		if ( !insertResult.IsSucceed )
		//			return insertResult.Message;
		//	}
		//	return string.Empty;

		//}

		internal string ExportToArcMap ( )
		{
			ComparisonOps comopOper = new ComparisonOps ( ComparisonOpType.Like, "Name", "II0%" );
			List<VerticalStructure> vertStructList = ( List<VerticalStructure> ) _dbWriter.GetFeats ( comopOper, FeatureType.VerticalStructure );
			foreach ( var vertStruct in vertStructList )
			{

			}
			return string.Empty;
		}


		private void CreateSqlCommand4DeleteObstacles ( List<VerticalStructure> vertStructList )
		{
			StringBuilder strBuilder = new StringBuilder ( );
			int i = 0;
			foreach ( var item in vertStructList )
			{
				strBuilder.Append ( "'" + item.Name + "'," );
				i++;
				if ( i == 4 )
				{
					strBuilder.Append ( "\r\n" );
					i = 0;
				}
			}
			string names = strBuilder.ToString ( );
			names = names.Remove ( names.LastIndexOf ( ',' ) );
			string sqlCommand = string.Format ( "delete from features_transaction_list where feat_identifier in" +
								"\r\n\t(select \"Identifier\" from features where \"Id\" in " +
				"\r\n\t\t(select feat_id from \"bl_VerticalStructure\" where \"Name\" in ({0})));" +
				"\r\ndelete from features where \"Id\" in " +
				"\r\n\t(select feat_id from \"bl_VerticalStructure\" where \"Name\" in ({0}));", names );
			CreateReport ( new List<string> ( ) { sqlCommand }, "deleteObstaclesCommand.txt" );
		}

		private Dictionary<string, List<LineData>> CreateDictionary ( List<LineData> lnDataList )
		{
			string key;
			Dictionary<string, List<LineData>> result = new Dictionary<string, List<LineData>> ( );
			List<string> keyList = new List<string> ( );
			int indexOfOB;
            int i = 0;
			foreach ( var lnData in lnDataList )
			{
				indexOfOB = lnData.Id.IndexOf ( "OB" );
				if ( indexOfOB != -1 )
				{
					key = lnData.Id.Substring ( 0, indexOfOB + 2 );
					if ( !result.Keys.Any ( str => str.StartsWith ( key ) ) )
						result.Add ( key, new List<LineData> ( ) );
				}
				else
				{
					key = lnData.Id;
                    if (result.ContainsKey(key))
                    {
                        i++;
                        if (result[key][0].X == lnData.X && result[key][0].Y == lnData.Y)
                            continue;
                        else
                            key = GenerateRandomKey (key, result);
                    }
                    result.Add ( key, new List<LineData> ( ) );
				}
				result[ key ].Add ( lnData );
			}
			return result;
		}

        private string GenerateRandomKey(string key, Dictionary<string, List<LineData>> dict)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            Random r = new Random();
            int randomCharIndex = r.Next(chars.Length);
            string result = chars[randomCharIndex] + key;
            if (dict.ContainsKey(result))
                result = GenerateRandomKey(result, dict);
            return result;
        }

		internal string InserteTodObstacles ( bool isGDB )
		{
			try
			{
				IWorkspace workspace;
				IWorkspaceFactory2 workspaceFactory;
				IFeatureWorkspace featureWorkspace;
				IFeatureClass featureClass;
				if ( isGDB )
				{
					workspaceFactory = ( IWorkspaceFactory2 ) new FileGDBWorkspaceFactory ( );
					workspace = workspaceFactory.OpenFromFile ( System.IO.Path.Combine ( FolderPath, "Obstacle", "Lidar.gdb" ), 0 );
				}
				else
				{
					// Open the directory as a shapefile workspace.
					workspaceFactory = ( IWorkspaceFactory2 ) new ShapefileWorkspaceFactory ( );
					//workspace = workspaceFactory.OpenFromFile ( System.IO.Path.Combine ( FolderPath, "Obstacle", "Last_OBST", "Shape" ), 0 );
					workspace = workspaceFactory.OpenFromFile ( @"D:\AirNav\ArcMap projects\SHP", 0 );
				}
				featureWorkspace = ( IFeatureWorkspace ) workspace;
				featureClass = featureWorkspace.OpenFeatureClass ( "Point" );

				Dictionary<string, int> featIndices = new Dictionary<string, int> ( );
				FindFieldsOfFeatureClass ( featureClass, featIndices );


				List<eTodObstacle> eTodObsList = new List<eTodObstacle> ( );
				eTodObsList.AddRange ( InserteTODVertStructs ( featureClass, featIndices, GeometryType.Point ) );

				featureClass = featureWorkspace.OpenFeatureClass ( "Line" );
				featIndices.Clear ( );
				FindFieldsOfFeatureClass ( featureClass, featIndices );
				eTodObsList.AddRange ( InserteTODVertStructs ( featureClass, featIndices, GeometryType.MultiLineString ) );

				featureClass = featureWorkspace.OpenFeatureClass ( "Polygon" );
				featIndices.Clear ( );
				FindFieldsOfFeatureClass ( featureClass, featIndices );
				eTodObsList.AddRange ( InserteTODVertStructs ( featureClass, featIndices, GeometryType.MultiPolygon ) );
				
				CreateReport ( _report );

				List<VerticalStructure> vertStructList = CreateVerticalStructureeList ( eTodObsList );
				InsertingResult insertResult = _dbWriter.InsertVerticalStructures ( vertStructList );

				if ( !insertResult.IsSucceed )
					return insertResult.Message;
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal void InserAirspaces ( )
		{
			IWorkspace workspace;
			IWorkspaceFactory2 workspaceFactory;
			IFeatureWorkspace featureWorkspace;
			IFeatureClass featureClass;
			workspaceFactory = ( IWorkspaceFactory2 ) new FileGDBWorkspaceFactory ( );
			workspace = workspaceFactory.OpenFromFile ( @"C:\Users\AbuzarH\Documents\ArcGIS\Default.gdb", 0 );
			featureWorkspace = ( IFeatureWorkspace ) workspace;
			//featureClass = featureWorkspace.OpenFeatureClass ( "Point" );

			//Dictionary<string, int> featIndices = new Dictionary<string, int> ( );
			//FindFieldsOfFeatureClass ( featureClass, featIndices );


			//List<eTodObstacle> eTodObsList = new List<eTodObstacle> ( );
			//eTodObsList.AddRange ( InserteTODVertStructs ( featureClass, featIndices, GeometryType.Point ) );

			//featureClass = featureWorkspace.OpenFeatureClass ( "Line" );
			//featIndices.Clear ( );
			//FindFieldsOfFeatureClass ( featureClass, featIndices );
			//eTodObsList.AddRange ( InserteTODVertStructs ( featureClass, featIndices, GeometryType.MultiLineString ) );

			featureClass = featureWorkspace.OpenFeatureClass ( "converted" );
			//featIndices.Clear ( );
			//FindFieldsOfFeatureClass ( featureClass, featIndices );
			List<MultiPolygon> geomList = GetPolygons ( featureClass );

			List<Airspace> airspaceList = new List<Airspace>();
			foreach ( var item in geomList )
			{
				Airspace airspace = (Airspace) Global.CreateFeature ( FeatureType.Airspace );
				AirspaceGeometryComponent airspGeomComp = new AirspaceGeometryComponent();
				
				airspGeomComp.TheAirspaceVolume = new AirspaceVolume();

				airspGeomComp.TheAirspaceVolume.HorizontalProjection = new Surface ( );
				foreach ( Aran.Geometries.Polygon polygon in item)
				{
					airspGeomComp.TheAirspaceVolume.HorizontalProjection.Geo.Add ( polygon );
				}

				airspace.GeometryComponent.Add ( airspGeomComp );
				airspaceList.Add ( airspace );
			}
			_dbWriter.InsertAirspaces ( airspaceList );
		}

		private List<MultiPolygon> GetPolygons( IFeatureClass featureClass )
		{
			List<MultiPolygon> geomList = new List<MultiPolygon> ( );
			IFeatureCursor featCursor = featureClass.Search ( null, true );
			IFeature feat = featCursor.NextFeature ( );
			try
			{
				while ( feat != null )
				{
					Geometry geom = null;
					try
					{
						IPolygon polygon = ( IPolygon ) feat.Shape;
						polygon = _featConv.SimplifyPolygon ( polygon );
						geom = ConvertFromEsriGeom.ToPolygonGeo ( polygon );
					}
					catch ( Exception ex )
					{
						feat = featCursor.NextFeature ( );
						continue;
					}
					Aran.Geometries.MultiPolygon mltPolygon = new MultiPolygon ( );
					if ( geom.Type == GeometryType.Polygon )
						mltPolygon.Add ( ( Aran.Geometries.Polygon ) geom );
					else
						mltPolygon = ( Aran.Geometries.MultiPolygon ) geom;
					geomList.Add ( mltPolygon );
					feat = featCursor.NextFeature ( );
				}
			}
			catch ( Exception ex )
			{
				throw ex;
			}
			return geomList;
		}


		private static void FindFieldsOfFeatureClass ( IFeatureClass featureClass, Dictionary<string, int> featIndices )
		{
			featIndices.Add ( "OBJECTID", featureClass.FindField ( "OBJECTID" ) );
			featIndices.Add ( "Area", featureClass.FindField ( "Area" ) );
			featIndices.Add ( "Orginator", featureClass.FindField ( "Orginator" ) );
			featIndices.Add ( "Href", featureClass.FindField ( "Href" ) );
			featIndices.Add ( "Type", featureClass.FindField ( "Type" ) );
			featIndices.Add ( "Elevation", featureClass.FindField ( "Elevation" ) );
			featIndices.Add ( "GROUP_NUMB", featureClass.FindField ( "GROUP_NUMB" ) );
			featIndices.Add ( "Part_elev", featureClass.FindField ( "Part_elev" ) );
			featIndices.Add ( "Part_Type", featureClass.FindField ( "Part_Type" ) );
			featIndices.Add ( "Haccu", featureClass.FindField ( "Haccu" ) );
			featIndices.Add ( "Haccu_u", featureClass.FindField ( "Haccu_u" ) );
			featIndices.Add ( "Hconf", featureClass.FindField ( "Hconf" ) );
			featIndices.Add ( "Hres", featureClass.FindField ( "Hres" ) );
			featIndices.Add ( "Height", featureClass.FindField ( "Height" ) );
			featIndices.Add ( "Vaccu", featureClass.FindField ( "Vaccu" ) );
			featIndices.Add ( "Vconf", featureClass.FindField ( "Vconf" ) );
			featIndices.Add ( "Vres", featureClass.FindField ( "Vres" ) );
			featIndices.Add ( "Vref", featureClass.FindField ( "Vref" ) );
			featIndices.Add ( "Integr", featureClass.FindField ( "Integr" ) );
			featIndices.Add ( "Date", featureClass.FindField ( "Date" ) );
			featIndices.Add ( "UoM", featureClass.FindField ( "UOM" ) );
			featIndices.Add ( "Operat", featureClass.FindField ( "Operat" ) );
			featIndices.Add ( "Effect", featureClass.FindField ( "Effect" ) );
			featIndices.Add ( "Light", featureClass.FindField ( "Light" ) );
			featIndices.Add ( "Mark", featureClass.FindField ( "Mark" ) );
		}

		internal string InsertArea1Obstacles ( bool replaceDecSeparatorWithDot = false, bool fromExcelFile = false )
		{
			List<VerticalStructure> verticalStructList = new List<VerticalStructure> ( );
			List<LineData> result;
			if ( fromExcelFile )
			{
				var filteredFiles = Directory.EnumerateFiles ( System.IO.Path.Combine ( FolderPath, "Obstacle", "Area 1" ), "*.xlsx", SearchOption.AllDirectories );
				string commandText = "SELECT * FROM [Лист1$]";
				string connectionString;
				DataTable dataTable;
				result = new List<LineData> ( );
				foreach ( var item in filteredFiles )
				{
					connectionString = string.Format ( @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=		{0};Extended Properties={1}Excel 12.0;HDR=YES{2}", item, Convert.ToChar ( 34 ), Convert.ToChar ( 34 ) );
					dataTable = new DataTable ( );
					using ( OleDbConnection connection = new OleDbConnection ( connectionString ) )
					{
						try
						{
							connection.Open ( );
							using ( OleDbDataAdapter adapter = new OleDbDataAdapter ( commandText, connection ) )
							{
								adapter.Fill ( dataTable );
								string startCode = ArpCode.Substring ( 2 );
								foreach ( DataRow row in dataTable.Rows )
								{
									if ( row[ 0 ].ToString ( ) != "" && row[0].ToString().StartsWith(startCode))
									{
										LineData lnData = new LineData ( );
										lnData.Id = row[ 0 ].ToString ( );
										lnData.Y = CoordStrToDouble ( row[ 1 ].ToString ( ) + " " + row[ 2 ].ToString ( ) + " " + row[ 3 ].ToString ( ) );
										lnData.X = CoordStrToDouble ( row[ 4 ].ToString ( ) + " " + row[ 5 ].ToString ( ) + " " + row[ 6 ].ToString ( ) );
										if ( lnData.Id.EndsWith ( "U" ) )
											lnData.IsUp = true;
										lnData.Z = double.Parse ( row[ 7 ].ToString ( ) );
										result.Add ( lnData );
									}
								}
							}
						}
						catch ( Exception ex )
						{
							return ( item + " file contains error: " + ex.Message );
						}
						finally
						{
							connection.Dispose ( );
							connection.Close ( );
						}
					}
				}
			}
			else
			{
				result = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Obstacle", "Area 1" ), false, replaceDecSeparatorWithDot );
			}
			Dictionary<string, List<LineData>> dict = CreateDictionary ( result );
			_featConv.AddVS_Point ( dict, ref verticalStructList );
			//CreateSqlCommand4DeleteObstacles ( verticalStructList );
			if ( Insert2DB )
			{
				InsertingResult insertResult = _dbWriter.InsertVerticalStructures ( verticalStructList );

				if ( !insertResult.IsSucceed )
					return insertResult.Message;
			}
			return string.Empty;
		}

		internal string InsertRawPoints ( bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				FormFolderSelection frmFolderSelection = new FormFolderSelection ( );
				List<LineData> lineDataList = null;
				if ( frmFolderSelection.ShowDialog ( ) == DialogResult.OK )
				{
					lineDataList = ParseFolder ( System.IO.Path.Combine ( FolderPath, frmFolderSelection.SelectedFolder ), false, replaceDecSeparatorWithDot );
#if !EXPERT_DATA
				foreach ( var item in lineDataList )
					SetElev_MSL ( item );
#endif
					_featConv.AddRawPoints ( lineDataList );
				}
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal string InsertApronElements ( bool sort, bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				//List<LineData> oldLnDataList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Apron_Element_old" ), true, false );
				List<LineData> result = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Apron_Element" ), false, replaceDecSeparatorWithDot );
#if !EXPERT_DATA
			foreach ( LineData lnData in result )
				SetElev_MSL ( lnData );
#endif
				List<LineData> sortedResult = new List<LineData> ( );
				//if ( sort )
				//{
				//    for ( int i = 0; i < oldLnDataList.Count; i++ )
				//    {
				//        sortedResult.Add ( result.Find ( lnData => lnData.Id == oldLnDataList[ i ].Id ) );
				//    }
				//}
				//result = sortedResult;
				string name;
				string idNumber;
				Dictionary<string, List<LineData>> apronElements = new Dictionary<string, List<LineData>> ( );
				foreach ( LineData lnData in result )
				{
					idNumber = "";
					foreach ( var item in lnData.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
					{
						idNumber = item + idNumber;
					}
#if !EXPERT_DATA
				int directorySeparatorIndex = lnData.Path.LastIndexOf ( System.IO.Path.DirectorySeparatorChar );
				int indexWgs84 = lnData.Path.ToLower ( ).IndexOf ( "wgs" );
				int lengthWGS84 = lnData.Path.Substring ( indexWgs84 ).Length;
				string fileName = lnData.Path.Substring ( directorySeparatorIndex + 1, lnData.Path.Length - lengthWGS84 - directorySeparatorIndex - 1 );
				name = fileName + " " + lnData.Id.Substring ( 0, lnData.Id.IndexOf ( idNumber ) );
#else
					name = lnData.Id.Substring ( 0, lnData.Id.IndexOf ( idNumber ) );
#endif
					if ( !apronElements.ContainsKey ( name ) )
						apronElements.Add ( name, new List<LineData> ( ) );
					else
					{
						if ( !apronElements.ContainsKey ( name ) )
						{
							apronElements.Add ( name, new List<LineData> ( ) );
						}
					}
					apronElements[ name ].Add ( lnData );
				}

				if ( Insert2DB )
					_dbWriter.InsertApronElements ( apronElements );
				_featConv.AddApronElement ( apronElements, sort );
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal string InsertAircraftStands ( bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				List<LineData> result = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Aircraft_Stand" ), false, replaceDecSeparatorWithDot );
//#if !EXPERT_DATA
			//foreach ( LineData lnData in result )
			{
				//SetElev_MSL ( lnData );
			}
//#endif
				if ( Insert2DB )
					_dbWriter.InsertAircraftStands ( result );
				_featConv.AddAircraftStand ( result );
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal string InsertControlPoints ( )
		{
			string dirPath = @"D:\Work\Kaz\Control Points";
			var filteredFiles = Directory.EnumerateFiles ( dirPath, "*.xlsx", SearchOption.AllDirectories );
			string commandText = "SELECT * FROM [Лист1$]";
			string connectionString;
			DataTable dataTable;
			List<LineData> lnDataList = new List<LineData> ( );
			foreach ( var item in filteredFiles )
			{
				connectionString = string.Format ( @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=		{0};Extended Properties={1}Excel 12.0;HDR=YES{2}", item, Convert.ToChar ( 34 ), Convert.ToChar ( 34 ) );
				dataTable = new DataTable ( );
				using ( OleDbConnection connection = new OleDbConnection ( connectionString ) )
				{
					try
					{
						connection.Open ( );
						using ( OleDbDataAdapter adapter = new OleDbDataAdapter ( commandText, connection ) )
						{
							adapter.Fill ( dataTable );
							foreach ( DataRow row in dataTable.Rows )
							{
								if ( row[ 0 ].ToString ( ) != "" )
								{
									LineData lnData = new LineData ( );
									lnData.Id = row[ 0 ].ToString ( );
									lnData.Y = CoordStrToDouble ( row[ 1 ].ToString ( ) + " " + row[ 2 ].ToString ( ) + " " + row[ 3 ].ToString ( ) );
									lnData.X = CoordStrToDouble ( row[ 4 ].ToString ( ) + " " + row[ 5 ].ToString ( ) + " " + row[ 6 ].ToString ( ) );
									//lnData.Z = double.Parse ( row[ 7 ].ToString ( ) );
									//lnData.Z_MSL = double.Parse ( row[ 8 ].ToString ( ) );
									lnData.Z = double.Parse ( row[ 8 ].ToString ( ) );
									lnData.Z_MSL = double.Parse ( row[ 7 ].ToString ( ) ) - lnData.Z;
									lnDataList.Add ( lnData );
								}
							}
						}
					}
					catch ( Exception ex )
					{
						return ( item + " file contains error: " + ex.Message );
					}
					finally
					{
						connection.Dispose ( );
						connection.Close ( );
					}
				}
			}
			if ( Insert2DB )
				_dbWriter.InsertSurveyControlPoints ( lnDataList );
			_featConv.AddCntrlPoints ( lnDataList, System.IO.Path.Combine ( dirPath, "Network.mdb" ) );
			return string.Empty;
		}

		internal string InsertApronGuidanceLinesWithPart ( bool sort, bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				if ( _readFromMdb )
				{
					List<GuidanceLine> apronGuidanceLineList = _featConv.GetApronGuidanceLines ( );
					_dbWriter.InsertGuidanceLineApron ( apronGuidanceLineList );
				}
				else
				{
					List<LineData> allApronGuidanceLines ;
					//allApronGuidanceLines = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Apron_Guidance_Line_old" ), true, false );
#if !EXPERT_DATA
			foreach ( var item in allApronGuidanceLines )
			{
				SetElev_MSL ( item );
			}			
#else
					List<LineData> expertLnDataList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Apron_Guidance_Line" ), false, replaceDecSeparatorWithDot );
					//SetGuidanceLineCodes ( allApronGuidanceLines, expertLnDataList );
					allApronGuidanceLines = expertLnDataList;
#endif
					Dictionary<string, Dictionary<string, List<LineData>>> result = CollapseDataStructure ( allApronGuidanceLines, sort );
					if ( Insert2DB )
						_dbWriter.InsertGuidanceLineApron ( result );

					_featConv.AddGuidanceLine_Apron ( result );
				}
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal string InsertGuidanceLineTaxiway ( bool sort, bool replaceDecSeparatorWithDot = false )
		{
			//List<string> fileNamePrefixList = new List<string> ( );
			//fileNamePrefixList.Add ( "TW" );
			//List<LineData> allTaxiwayGuidanceLines = ParseFolder ( fileNamePrefixList, FolderPath, true ) [ fileNamePrefixList [ 0 ] ];
			//Dictionary<string, Dictionary<string, List<LineData>>> result = CollapseDataStructure ( allTaxiwayGuidanceLines );
			//foreach ( LineData lnData in allTaxiwayGuidanceLines )
			//{
			//    if ( lnData.Id.Equals ( "part" ) || lnData.Id.Equals ( StartMulti ) || lnData.Id.Equals ( EndMulti ) )
			//        continue;
			//    SetElev_MSL ( lnData );
			//}
			//_dbWriter.InsertGuidanceLineTaxiway ( result );
			//_dbWriter.InsertTaxiwayGuidanceLines ( result, startMulti, endMulti );
			//return result;

			try
			{
				if ( _readFromMdb )
				{
					List<GuidanceLine> guidanceLineList = _featConv.GetTaxiwayGuidanceLines ( );
					_dbWriter.InsertGuidanceLineTaxiway ( guidanceLineList );
				}
				else
				{
					List<LineData> allTaxiwayGuidanceLines;
					//List<LineData> allTaxiwayGuidanceLines = ParseFolder ( System.IO.Path.Combine ( FolderPath, "TWY_Guidance_Line_old" ), true, false );
#if !EXPERT_DATA
			string tmp;
			foreach ( var item in allTaxiwayGuidanceLines )
			{
				tmp = SetElev_MSL ( item );
				if ( tmp != "" )
					reportList.Add ( "TWY_Guidance_Line : " + tmp );
			}
#else
					List<LineData> expertLnDataList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "TWY_Guidance_Line" ), false, replaceDecSeparatorWithDot );
					//SetGuidanceLineCodes ( allTaxiwayGuidanceLines, expertLnDataList );
					allTaxiwayGuidanceLines = expertLnDataList;
#endif
					//_featConv.AddRawPoints ( allTaxiwayGuidanceLines.Where ( lnData => lnData.Code == null ).ToList ( ) );
					Dictionary<string, Dictionary<string, List<LineData>>> result = CollapseDataStructure ( allTaxiwayGuidanceLines, sort );
					if ( Insert2DB )
						_dbWriter.InsertGuidanceLineTaxiway ( result );
					_featConv.AddGuidanceLine_Taxiway ( result );
				}
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		internal string GetTaxiHoldingPositions ( bool replaceDecSeparatorWithDot = false )
		{
			try
			{
				List<LineData> taxiHoldingPosList = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Taxi_Holding_Position" ), true, replaceDecSeparatorWithDot );
//#if !EXPERT_DATA
			foreach ( var item in taxiHoldingPosList )
				SetElev_MSL ( item );
//#endif
				if ( Insert2DB )
					_dbWriter.InsertTaxiHoldingPosition ( taxiHoldingPosList );
				_featConv.AddTaxiHoldingPositions ( taxiHoldingPosList );
				return string.Empty;
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		private List<VerticalStructure> CreateVerticalStructureeList ( List<eTodObstacle> eTodObsList )
		{
			List<VerticalStructure> vertStructList = new List<VerticalStructure> ( );

			Dictionary<string, List<eTodObstacle>> sortedEtodObsList = SorteTodObstacles ( eTodObsList );
			foreach ( var item in sortedEtodObsList )
			{
				VerticalStructure vertStruct = Global.CreateVerticalStructure ( item.Key );
				vertStruct.Group = false;
				vertStruct.Part.AddRange ( CreateVertStructPart ( item.Value ) );
				var eTodObsType = item.Value.Where ( eTodObs => eTodObs.GroupNumber != "" && eTodObs.Type.HasValue ).FirstOrDefault ( );
				if ( eTodObsType != null )
					vertStruct.Type = eTodObsType.Type;
				if ( item.Value[ 0 ].GroupNumber != "" )
				{
					if ( item.Value[ 0 ].Type == CodeVerticalStructure.TREE )
						vertStruct.Group = true;
				}
				else
				{
					vertStruct.Type = item.Value[ 0 ].Type;
				}
				if ( item.Value.Where ( eTodObs => eTodObs.Light.HasValue && eTodObs.Light.Value ).FirstOrDefault ( ) != null )
					vertStruct.Lighted = true;
				vertStructList.Add ( vertStruct );
			}

			return vertStructList;
		}

		private static List<VerticalStructurePart> CreateVertStructPart ( List<eTodObstacle> eTodObsList )
		{
			List<VerticalStructurePart> result = new List<VerticalStructurePart> ( );
			foreach ( var item in eTodObsList )
			{
				VerticalStructurePart vertStructPart = new VerticalStructurePart ( );
				vertStructPart.Designator = item.ObjectID;
				if ( item.Type.HasValue )
					vertStructPart.Type = item.Type;
				else
					vertStructPart.Type = item.PartType;

				vertStructPart.Mobile = item.Mobile;
				vertStructPart.HorizontalProjection = new VerticalStructurePartGeometry ( );
				if ( item.Geom.Type == GeometryType.Point )
				{
					vertStructPart.HorizontalProjection.Location = new ElevatedPoint ( );
					if ( item.Elevation != null )
						vertStructPart.HorizontalProjection.Location.Elevation = item.Elevation;
					else
						vertStructPart.HorizontalProjection.Location.Elevation = item.PartElevation;

					vertStructPart.HorizontalProjection.Location.Geo.Assign ( item.Geom );
					vertStructPart.HorizontalProjection.Location.HorizontalAccuracy = item.HorizontalAccuracy;
					vertStructPart.HorizontalProjection.Location.VerticalAccuracy = item.VerticalAccuracy;
					vertStructPart.HorizontalProjection.Location.VerticalDatum = item.VerticalDatumType;
				}
				else if ( item.Geom.Type == GeometryType.MultiLineString )
				{
					vertStructPart.HorizontalProjection.LinearExtent = new ElevatedCurve ( );
					foreach ( LineString lnString in ( MultiLineString ) item.Geom )
					{
						vertStructPart.HorizontalProjection.LinearExtent.Geo.Add ( lnString );
					}
					if ( item.Elevation != null )
						vertStructPart.HorizontalProjection.LinearExtent.Elevation = item.Elevation;
					else
						vertStructPart.HorizontalProjection.LinearExtent.Elevation = item.PartElevation;
					vertStructPart.HorizontalProjection.LinearExtent.HorizontalAccuracy = item.HorizontalAccuracy;
					vertStructPart.HorizontalProjection.LinearExtent.VerticalAccuracy = item.VerticalAccuracy;
					vertStructPart.HorizontalProjection.LinearExtent.VerticalDatum = item.VerticalDatumType;
				}
				else if ( item.Geom.Type == GeometryType.MultiPolygon )
				{
					vertStructPart.HorizontalProjection.SurfaceExtent = new ElevatedSurface ( );
					foreach ( Aran.Geometries.Polygon polygon in ( MultiPolygon ) item.Geom )
					{
						vertStructPart.HorizontalProjection.SurfaceExtent.Geo.Add ( polygon );
					}
					if ( item.Elevation != null )
						vertStructPart.HorizontalProjection.SurfaceExtent.Elevation = item.Elevation;
					else
						vertStructPart.HorizontalProjection.SurfaceExtent.Elevation = item.PartElevation;
					vertStructPart.HorizontalProjection.SurfaceExtent.HorizontalAccuracy = item.HorizontalAccuracy;
					vertStructPart.HorizontalProjection.SurfaceExtent.VerticalAccuracy = item.VerticalAccuracy;
					vertStructPart.HorizontalProjection.SurfaceExtent.VerticalDatum = item.VerticalDatumType;
				}
				result.Add ( vertStructPart );
			}
			return result;
		}

		private Dictionary<string, List<eTodObstacle>> SorteTodObstacles ( List<eTodObstacle> eTodObsList )
		{
			Dictionary<string, List<eTodObstacle>> result = new Dictionary<string, List<eTodObstacle>> ( );
			string name;
			foreach ( eTodObstacle eTodObs in eTodObsList )
			{
				name = eTodObs.GroupNumber.Trim ( );
				if ( name != "" )
				{
					if ( !result.ContainsKey ( name ) )
						result.Add ( name, new List<eTodObstacle> ( ) );
					result[ name ].Add ( eTodObs );
				}
				else
				{
					name = eTodObs.ObjectID.Trim ( );
					result.Add ( name, new List<eTodObstacle> ( ) );
					result[ name ].Add ( eTodObs );
				}
			}

			List<eTodObstacle> mltPolygonList = new List<eTodObstacle> ( ), mltLnStringList = new List<eTodObstacle> ( ), pntList = new List<eTodObstacle> ( );
			foreach ( string key in result.Keys )
			{
				if ( result[ key ].Count > 1 )
				{
					mltPolygonList.Clear ( );
					mltLnStringList.Clear ( );
					pntList.Clear ( );
					foreach ( var item in result[ key ] )
					{
						if ( item.Geom.Type == GeometryType.Point )
							pntList.Add ( item );
						else if ( item.Geom.Type == GeometryType.MultiLineString )
							mltLnStringList.Add ( item );
						else if ( item.Geom.Type == GeometryType.MultiPolygon )
							mltPolygonList.Add ( item );
					}
					result[ key ].Clear ( );
					result[ key ].AddRange ( mltPolygonList );
					result[ key ].AddRange ( mltLnStringList );
					result[ key ].AddRange ( pntList );
				}
			}
			return result;
		}

		private List<eTodObstacle> InserteTODVertStructs ( IFeatureClass featureClass, Dictionary<string, int> featIndices, GeometryType geometryType )
		{
			List<eTodObstacle> eTodObsList = new List<eTodObstacle> ( );
			IFeatureCursor featCursor = featureClass.Search ( null, true );
			IFeature feat = featCursor.NextFeature ( );
			string type;
			string uomString;
			UomDistanceVertical? uomDistVert;
			UomDistance? uomDist;
			int i = 0;
			int index = 0;
			try
			{
				while ( feat != null )
				{
					index++;
					eTodObstacle eTodObs = new eTodObstacle ( );

					eTodObs.ObjectID = Get_eTodFeatValueAsString ( feat, featIndices[ "OBJECTID" ] );
					eTodObs.GroupNumber = Get_eTodFeatValueAsString ( feat, featIndices[ "GROUP_NUMB" ] );
					CodeVerticalStructure codeVertsStruct;

					type = Get_eTodFeatValueAsString ( feat, featIndices[ "Type" ] ).ToUpper ( ).Trim ( );
					if ( Enum.TryParse<CodeVerticalStructure> ( type, out codeVertsStruct ) )
						eTodObs.Type = codeVertsStruct;
					else
					{
						if ( type == "CHIMNEY" )
							eTodObs.Type = CodeVerticalStructure.STACK;
						else if ( type == "BUILD UP AREA" || type == "CONSTRUCTION" || type == "CONSTRUCTION SITE" || type == "CONSTRUCTION AREA" || type == "SITE" || type == "BUILDING SITE" )
							eTodObs.Type = CodeVerticalStructure.BUILDING;
						else if ( type == "TREE ROW" )
						{
							eTodObs.Type = CodeVerticalStructure.TREE;
							eTodObs.GroupNumber = eTodObs.ObjectID;
						}
						else if ( type == "CEMETERY" || type == "CEMETERY OVERGROWN WITH TREES" || type == "FIELD" || type == "GREENHOUSE" || type == "PARK" )
							eTodObs.Type = CodeVerticalStructure.VEGETATION;
						else if ( type == "ROAD SIGN" || type == "TRAFFIC SIGN" || type == "DISPLAY SIGN" )
							eTodObs.Type = CodeVerticalStructure.SIGN;
						else if ( type == "CHURCH" )
							eTodObs.Type = CodeVerticalStructure.DOME;
						else if ( type == "HEAP" )
							eTodObs.Type = CodeVerticalStructure.INDUSTRIAL_SYSTEM;
						else if ( type == "MOBILE CRANE" )
						{
							eTodObs.Type = CodeVerticalStructure.CRANE;
							eTodObs.Mobile = true;
						}
						else if ( type == "EPL" || type == "COLUMN" || type == "PAIR OF POLES" || type == "EPL POLE" || type == "POLES" || type == "LAMPPOST" )
							eTodObs.Type = CodeVerticalStructure.POLE;
						else if ( type == "BRICKS" )
							eTodObs.Type = CodeVerticalStructure.FENCE;
						else if ( type == "TRANSFORMER" )
							eTodObs.Type = CodeVerticalStructure.ELECTRICAL_SYSTEM;
						else if ( type == "NAVIGATION LIGHTS" )
							eTodObs.Type = CodeVerticalStructure.ELECTRICAL_EXIT_LIGHT;
						else if ( type == "POWER PLANT" )
							eTodObs.Type = CodeVerticalStructure.POWER_PLANT;
						else if ( type == "RAILWAY" || type == "VEHICLE" || type == "VEHICLE STATION" )
							eTodObs.Type = CodeVerticalStructure.TRAMWAY;
						else if ( type == "SURVEYING" )
							eTodObs.Type = CodeVerticalStructure.URBAN;
						else if ( type == "ROAD" )
							eTodObs.Type = CodeVerticalStructure.GENERAL_UTILITY;
						else if ( type == "OIL TANK" )
							eTodObs.Type = CodeVerticalStructure.TANK;
						else if ( type == "GIRDER" )
							eTodObs.Type = CodeVerticalStructure.SPIRE;
						else if ( type == "OBSERVATION MAST" || type == "COMMUNICATION TOWER" )
							eTodObs.Type = CodeVerticalStructure.TOWER;
						else if ( type == "WATER TOWER" )
							eTodObs.Type = CodeVerticalStructure.WATER_TOWER;
						else if ( type == "GREEN AREA" || type == "GREEN PARK" || type == "GREEN PARK WITH WATERPOOL" || type == "VEGETATION" )
						{
							eTodObs.Type = CodeVerticalStructure.VEGETATION;
							eTodObs.GroupNumber = eTodObs.ObjectID;
						}
						else if ( type == "POWER TRANSMISSION LINE" || type == "POWER TRANSMISSION PYLON" )
							eTodObs.Type = CodeVerticalStructure.TRANSMISSION_LINE;
						else if ( type == "AMUSEMENT PARK STRUCTURE" )
							eTodObs.Type = CodeVerticalStructure.VEGETATION;
						else if ( type == "" )
							eTodObs.Type = null;
						else
						{
							if ( !_report.Contains ( type + " type is not implemented !" ) )
								_report.Add ( type + " type is not implemented !" );
						}							
					}
					uomString = "";
					uomDistVert = null;
					uomDist = null;
					uomString = Get_eTodFeatValueAsString ( feat, featIndices[ "UoM" ] ).ToUpper ( );
					if ( uomString == "METERS" || uomString == "M" )
					{
						uomDistVert = UomDistanceVertical.M;
						uomDist = UomDistance.M;
					}
					else
						throw new NotImplementedException ( "Not implemented (" + uomString + ") unit" );



					double tmp = Get_eTodFeatValueAsDouble ( feat, featIndices[ "Elevation" ] );
					if ( !double.IsNaN ( tmp ) )
					{
						if ( uomDistVert.HasValue )
							eTodObs.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical ( tmp, uomDistVert.Value );
					}
					if ( eTodObs.GroupNumber != "" && eTodObs.Type.HasValue )
						eTodObs.PartType = eTodObs.Type;
					else if ( Enum.TryParse<CodeVerticalStructure> ( Get_eTodFeatValueAsString ( feat, featIndices[ "Part_Type" ] ).ToUpper ( ), out codeVertsStruct ) )
						eTodObs.PartType = codeVertsStruct;
					if ( type == "" && eTodObs.PartType == null)
						i++;
					tmp = Get_eTodFeatValueAsDouble ( feat, featIndices[ "Part_elev" ] );
					if ( !double.IsNaN ( tmp ) )
					{
						if ( uomDistVert.HasValue != null )
							eTodObs.PartElevation = new Aran.Aim.DataTypes.ValDistanceVertical ( tmp, uomDistVert.Value );
					}

					tmp = Get_eTodFeatValueAsDouble ( feat, featIndices[ "Haccu" ] );
					if ( !double.IsNaN ( tmp ) )
					{
						if ( uomDist.HasValue != null )
							eTodObs.HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance ( tmp, uomDist.Value );
					}

					tmp = Get_eTodFeatValueAsDouble ( feat, featIndices[ "Vaccu" ] );
					if ( !double.IsNaN ( tmp ) )
					{
						if ( uomDist.HasValue != null )
							eTodObs.VerticalAccuracy = new Aran.Aim.DataTypes.ValDistance ( tmp, uomDist.Value );
					}

					string verticalDatum = Get_eTodFeatValueAsString ( feat, featIndices[ "Vref" ] );
					if ( verticalDatum != "" )
					{
						verticalDatum = verticalDatum.ToLower ( );
						if ( verticalDatum == "egm2008" )
							eTodObs.VerticalDatumType = CodeVerticalDatum.OTHER_EGM_08;
						else if ( verticalDatum == "egm96" )
							eTodObs.VerticalDatumType = CodeVerticalDatum.EGM_96;
					}

					string light = Get_eTodFeatValueAsString ( feat, featIndices[ "Light" ] );
					if ( light != "" )
					{
						light = light.ToLower ( );
						if ( light == "y" )
							eTodObs.Light = true;
						else if ( light == "n" )
							eTodObs.Light = false;
					}

					if ( geometryType == GeometryType.Point )
					{
						Aran.Geometries.Point pnt = ConvertFromEsriGeom.ToPoint ( ( IPoint ) feat.Shape );
						eTodObs.Geom = pnt;
					}
					else if ( geometryType == GeometryType.MultiLineString )
					{
						Aran.Geometries.MultiLineString mltLnString = ConvertFromEsriGeom.ToPolyline ( ( IPolyline ) feat.Shape, true );
						eTodObs.Geom = ( Aran.Geometries.Geometry ) mltLnString;
					}
					else if ( geometryType == GeometryType.MultiPolygon )
					{
						Geometry geom = null;
						//try
						{
							IPolygon polygon = ( IPolygon ) feat.Shape;
							polygon = _featConv.SimplifyPolygon ( polygon );
							geom = ConvertFromEsriGeom.ToPolygonGeo ( polygon );
						}
						//catch ( Exception ex )
						//{
						//feat = featCursor.NextFeature ( );
						//continue;
						//}
						Aran.Geometries.MultiPolygon mltPolygon = new MultiPolygon ( );
						if ( geom.Type == GeometryType.Polygon )
							mltPolygon.Add ( ( Aran.Geometries.Polygon ) geom );
						else
							mltPolygon = ( Aran.Geometries.MultiPolygon ) geom;
						eTodObs.Geom = mltPolygon;
					}
					eTodObsList.Add ( eTodObs );
					feat = featCursor.NextFeature ( );
				}
				//_report.Add ( "Empty type count (in " + geometryType + "): " + i );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
			return eTodObsList;
		}

		private string Get_eTodFeatValueAsString ( IFeature feat, int index )
		{
			string tmp = feat.get_Value ( index );
			if ( DBNull.Value.Equals ( tmp ) )
				return "";
			return tmp.Trim ( ).ToString ( );
		}

		private double Get_eTodFeatValueAsDouble ( IFeature feat, int index )
		{
			string tmp = Get_eTodFeatValueAsString ( feat, index );
			double result;
			tmp = tmp.Replace ( ',', '.' );
			var ci = CultureInfo.InvariantCulture.Clone ( ) as CultureInfo;
			ci.NumberFormat.NumberDecimalSeparator = ".";
			if ( double.TryParse ( tmp, NumberStyles.Any, ci, out result ) )
				return result;
			return double.NaN;
		}

		private Dictionary<string, List<LineData>> GetVertStuctsType ( string verticalStructureTypePath )
		{
			string zoneName = "Other";
			List<LineData> lineDataListOther = FillVertStruct ( verticalStructureTypePath, zoneName );

			zoneName = "Area3";
			List<LineData> lineDataListArea3 = FillVertStruct ( verticalStructureTypePath, zoneName );

			zoneName = "Area4";
			List<LineData> lineDataListArea4 = FillVertStruct ( verticalStructureTypePath, zoneName );

			zoneName = "Strip";
			List<LineData> lineDataListStrip = FillVertStruct ( verticalStructureTypePath, zoneName );

			List<LineData> lnDataListAll = new List<LineData> ( );
			lnDataListAll.AddRange ( lineDataListOther );
			lnDataListAll.AddRange ( lineDataListArea3 );
			lnDataListAll.AddRange ( lineDataListArea4 );
			lnDataListAll.AddRange ( lineDataListStrip );

			Dictionary<string, List<LineData>> verticalStructures = GetVertStructDictionaryOf ( lnDataListAll );

			return verticalStructures;
		}

		private List<LineData> FillVertStruct ( string verticalStructureTypePath, string zoneName )
		{
			List<LineData> lineDataListOther = ParseFolder ( System.IO.Path.Combine ( FolderPath, "Obstacle", zoneName, verticalStructureTypePath ), false, false );
			//reportList.AddRange ( SetElev_MSL ( lineDataListOther, "Vertical Structure (" + verticalStructureTypePath + ") in " + zoneName + " folder:" ) );
			foreach ( var item in lineDataListOther )
			{
				item.ZoneCode = zoneName;
			}
			return lineDataListOther;
		}

		private static Dictionary<string, List<LineData>> GetVertStructDictionaryOf ( List<LineData> lineDataList )
		{
			Dictionary<string, List<LineData>> verticalStructures = new Dictionary<string, List<LineData>> ( );
			string idNumber, name;
			foreach ( LineData lnData in lineDataList )
			{
				idNumber = "";
				foreach ( var item in lnData.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
				{
					idNumber = item + idNumber;
				}
				int directorySeparatorIndex = lnData.Path.LastIndexOf ( System.IO.Path.DirectorySeparatorChar );
				int indexWgs84 = lnData.Path.ToLower ( ).IndexOf ( "wgs" );
				int lengthWGS84 = lnData.Path.Substring ( indexWgs84 ).Length;
				string fileName = lnData.Path.Substring ( directorySeparatorIndex + 1, lnData.Path.Length - lengthWGS84 - directorySeparatorIndex - 1 );
				name = fileName + " " + lnData.Id.Substring ( 0, lnData.Id.LastIndexOf ( idNumber ) );

				if ( !verticalStructures.ContainsKey ( name ) )
					verticalStructures.Add ( name, new List<LineData> ( ) );
				else
				{
					if ( !verticalStructures.ContainsKey ( name ) )
					{
						verticalStructures.Add ( name, new List<LineData> ( ) );
					}
				}
				verticalStructures[ name ].Add ( lnData );
			}
			return verticalStructures;
		}

		private static void SetGuidanceLineCodes ( List<LineData> allApronGuidanceLines, List<LineData> expertLnDataList )
		{
			string id;
			//List<LineData> notExpertedList = new List<LineData> ( );
			foreach ( var item in allApronGuidanceLines )
			{
				id = item.Id;
				if ( id.Contains ( "_" ) )
				{
					id = id.Substring ( 0, id.IndexOf ( "_" ) );
					LineData lineData = expertLnDataList.Find ( lnData => lnData.Id == id );
					if ( lineData != null )
					{
						LineData newLnData = new LineData ( );
						newLnData.Id = item.Id;
						newLnData.X = item.X;
						newLnData.Y = item.Y;
						newLnData.Z = item.Z;
						newLnData.Z_MSL = item.Z_MSL;
						newLnData.Code = item.Code;
						expertLnDataList.Add ( newLnData );
					}
						expertLnDataList.Add ( item );
					//else
					//{
					//    notExpertedList.Add ( item );
					//}
				}
				else
				{
					List<LineData> foundList = expertLnDataList.FindAll ( lnData => lnData.Id == id );
					foreach ( var foundItem in foundList )
					{
						if ( foundItem.Code == null )
							foundItem.Code = item.Code;
						else
						{
						}
					}
				}
			}
		}

		private Dictionary<string, Dictionary<string, List<LineData>>> CollapseDataStructure ( List<LineData> lineDataList, bool sort )
		{
			//int index = 0;
			//string guidanceLineName, guidanceLinePartName;

			//First key determines guidanceline name
			//Second Dictionary determines part and its LineDataList appropriate to guidanceLine
			Dictionary<string, Dictionary<string, List<LineData>>> result = new Dictionary<string, Dictionary<string, List<LineData>>> ( );
			//Dictionary<string, List<LineData>> partDict = null;
			//string fileName;
			//LineData lineData;

			string name;
			//int directorySeparatorIndex, indexWgs84, lengthWGS84;
			string idNumber;
			foreach ( LineData lnData in lineDataList )
			{
				idNumber = "";
				if ( lnData.Code == null || lnData.Code == "" )
					continue;

				foreach ( var item in lnData.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
				{
					idNumber = item + idNumber;
				}
				name = lnData.Id.Substring ( 0, lnData.Id.IndexOf ( idNumber ) );
				if ( !result.ContainsKey ( name ) )
					result.Add ( name, new Dictionary<string, List<LineData>> ( ) );
				if ( !result[ name ].ContainsKey ( lnData.Code ) )
					result[ name ].Add ( lnData.Code, new List<LineData> ( ) );
				result[ name ][ lnData.Code ].Add ( lnData );
			}
			if ( sort )
			{
				foreach ( KeyValuePair<string, Dictionary<string, List<LineData>>> keyValuePair in result )
				{
					foreach ( KeyValuePair<string, List<LineData>> partKeyValuePair in keyValuePair.Value )
					{
						if ( partKeyValuePair.Value.Count < 2 )
							continue;
						partKeyValuePair.Value.Sort (
							delegate ( LineData ld1, LineData ld2 )
							{
								string idNumber1 = "", idNumber2 = "";
								foreach ( var item in ld1.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
								{
									idNumber1 = item + idNumber1;
								}
								idNumber1 = idNumber1.Substring ( 2 );

								foreach ( var item in ld2.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
								{
									idNumber2 = item + idNumber2;
								}
								idNumber2 = idNumber2.Substring ( 2 );
								if ( idNumber1.Contains ( "_" ) || idNumber2.Contains ( "_" ) )
									return idNumber1.CompareTo ( idNumber2 );
								double id1 = double.Parse ( idNumber1 );
								double id2 = double.Parse ( idNumber2 );
								return id1.CompareTo ( id2 );
							} );
					}
				}
			}
			return result;

			//int partIndex;
			//while ( index < lineDataList.Count )
			//{
			//    lineData = lineDataList[ index ];
			//    if ( lineData.Id.Equals ( StartMulti ) )
			//    {
			//        index++;
			//        partIndex = 1;
			//        lineData = lineDataList[ index ];
			//        fileName = lineData.FileName.Substring ( lineData.FileName.LastIndexOf ( @"\" ) + 1 );
			//        guidanceLineName = fileName.Substring ( 0, fileName.IndexOf ( "_adjusted" ) );

			//        partDict = new Dictionary<string, List<LineData>> ( );
			//        result.Add ( guidanceLineName, partDict );

			//        guidanceLinePartName = "Part " + partIndex.ToString ( );
			//        partDict.Add ( guidanceLinePartName, new List<LineData> ( ) );

			//        while ( !lineData.Id.Equals ( EndMulti ) )
			//        {
			//            if ( lineData.Id.Equals ( "part" ) )
			//            {
			//                partIndex++;
			//                guidanceLinePartName = "Part " + partIndex.ToString ( );
			//                partDict.Add ( guidanceLinePartName, new List<LineData> ( ) );
			//                index++;
			//                lineData = lineDataList[ index ];
			//                continue;
			//            }
			//            SetElev_MSL ( lineData );
			//            partDict[ guidanceLinePartName ].Add ( lineData );
			//            index++;
			//            lineData = lineDataList[ index ];
			//        }
			//    }
			//    index++;
			//}

			//for ( int i = 0; i < apronGuidanceLines.Count; i++ )
			//{
			//    lineData = apronGuidanceLines[ i ];
			//    fileName = lineData.FileName.Substring ( lineData.FileName.LastIndexOf ( @"\" ) + 1 );
			//    index = fileName.IndexOf ( "_part" );
			//    if ( index != -1 )
			//    {
			//        guidanceLineName = fileName.Substring ( 0, index );
			//        if ( multiPartGuidances.ContainsKey ( guidanceLineName ) )
			//        {
			//            guidanceLinePartName = fileName.Substring ( index + 1, 6 );
			//            if ( !partDict.ContainsKey ( guidanceLinePartName ) )
			//                partDict.Add ( guidanceLinePartName, new List<LineData> ( ) );
			//        }
			//        else
			//        {
			//            partDict = new Dictionary<string, List<LineData>> ( );
			//            guidanceLinePartName = fileName.Substring ( index + 1, 6 );
			//            partDict.Add ( guidanceLinePartName, new List<LineData> ( ) );
			//            multiPartGuidances.Add ( guidanceLineName, partDict );
			//        }
			//        SetElev_MSL ( lineData );
			//        multiPartGuidances[ guidanceLineName ][ guidanceLinePartName ].Add ( lineData );
			//    }
			//    else
			//    {
			//        if ( lineData.Id.Equals ( StartMulti ) )
			//        {
			//            partDict = new Dictionary<string, List<LineData>> ( );
			//            guidanceLinePartName = "Part_1";
			//            partDict.Add ( guidanceLinePartName, new List<LineData> ( ) );
			//            index = fileName.ToLower ( ).IndexOf ( "_adjusted" );
			//            guidanceLineName = fileName.Substring ( 0, index );
			//            multiPartGuidances.Add ( guidanceLineName, partDict );
			//            i++;
			//            lineData = apronGuidanceLines[ i ];
			//            while ( !lineData.Id.Equals ( EndMulti ) )
			//            {
			//                SetElev_MSL ( lineData );
			//                multiPartGuidances[ guidanceLineName ][ guidanceLinePartName ].Add ( lineData );
			//                i++;
			//                lineData = apronGuidanceLines[ i ];
			//            }
			//        }
			//    }
			//}

			//List<string> keyList = result.Keys.ToList ( );
			//List<string> guidanceLineNameList = new List<string> ( );
			//foreach ( var item in keyList )
			//{
			//    index = item.IndexOf ( "_part" );
			//    if ( index != -1 )
			//    {
			//        guidanceLineName = item.Substring ( 0, index );
			//        if ( !guidanceLineNameList.Contains ( guidanceLineName ) )
			//            guidanceLineNameList.Add ( guidanceLineName );
			//        if ( item.EndsWith ( "1" ) )
			//        {
			//            result.Add ( guidanceLineName, new Dictionary<string, List<LineData>> ( ) );
			//        }
			//        foreach ( var item2 in result[ item ].Keys )
			//        {
			//            result[ guidanceLineName ].Add ( "Part " + ( result[ guidanceLineName ].Keys.Count + 1 ).ToString ( ), result[ item ][ item2 ] );
			//        }
			//        result.Remove ( item );
			//    }
			//}

			//return result;
		}

		private List<LineData> GetRwyCntPoints ( List<LineData> allRwyPnts, out List<LineData> rwyElementLnDatas, out LineData arpLineData )
		{
			allRwyPnts.Sort ( delegate ( LineData ld1, LineData ld2 )
			{
				return ld1.Id.CompareTo ( ld2.Id );
			} );


			int count = allRwyPnts.Count;
			int rem;
			int quotient = Math.DivRem ( count, 3, out rem );

			arpLineData = allRwyPnts.Where ( lnData => lnData.Id.Contains ( "RP" ) ).FirstOrDefault ( );
			int removeCount = 0;
			if ( arpLineData != null )
				removeCount = 1;

			List<LineData> thrLnDataList = allRwyPnts.Where ( lnData => lnData.Id.Contains ( "TH" ) ).ToList ( );
			removeCount += thrLnDataList.Count;

			List<LineData> result = null;

			if ( rem != 0 )
			{
				//quotient -= 1;
				result = allRwyPnts.GetRange ( quotient, quotient - removeCount + 1 );
				//result = allRwyPnts.GetRange ( quotient, quotient -1);
			}
			else
			{
				if ( thrLnDataList.Count == 2 && arpLineData != null )
					quotient -= 1;
				result = allRwyPnts.GetRange ( quotient, quotient );
				//result = allRwyPnts.GetRange ( quotient, quotient - removeCount );
			}

			//If airport is not
			//List<LineData> result = allRwyPnts.GetRange(quotient, quotient-removeCount);

			rwyElementLnDatas = allRwyPnts.GetRange ( 0, quotient );
			List<LineData> reverseLnDataList = allRwyPnts.GetRange ( quotient + result.Count, quotient );
			reverseLnDataList.Reverse ( );
			rwyElementLnDatas.AddRange ( reverseLnDataList );

			if ( thrLnDataList.Count != 0 )
			{
				ARANFunctions.InitEllipsoid ( );
				double angleIndDegree, angleReverse;
				ARANFunctions.ReturnGeodesicAzimuth ( new Aran.Geometries.Point ( result[ 0 ].X, result[ 0 ].Y ), new Aran.Geometries.Point ( result[ 1 ].X, result[ 1 ].Y ), out angleIndDegree, out angleReverse );
				double angleThr;
				try
				{
					//get after third char
					angleThr = double.Parse ( thrLnDataList[ 0 ].Id.Substring ( 3, 2 ) + "0" );
				}
				catch ( Exception )
				{
					try
					{
						//if error occured get after second char
						angleThr = double.Parse ( thrLnDataList[ 0 ].Id.Substring ( 2, 2 ) + "0" );
					}
					catch ( Exception )
					{
						//if error again occured get after fourth char
						angleThr = double.Parse ( thrLnDataList[ 0 ].Id.Substring ( 4, 2 ) + "0" );
					}


				}

				LineData startLnData, endLnData;
				if ( ARANMath.Modulus ( angleIndDegree, angleThr ) < 10 )
				{
					startLnData = thrLnDataList[ 0 ];
					endLnData = thrLnDataList[ 1 ];
				}
				else
				{
					startLnData = thrLnDataList[ 1 ];
					endLnData = thrLnDataList[ 0 ];
				}
				startLnData.CodeRunway = CodeRunwayPointRole.START;
				endLnData.CodeRunway = CodeRunwayPointRole.END;

				//string rwyDirText = result[ 0 ].Id.Substring ( result[ 0 ].Id.Length - 4, 2 );
				//startLnData = thrLnDataList.Where ( lnData => lnData.Id.EndsWith ( rwyDirText + "01" ) ).FirstOrDefault ( );
				//endLnData = thrLnDataList[ 1 - thrLnDataList.IndexOf ( startLnData ) ];

				LineData thrLnData = new LineData ( )
				{
					Id = startLnData.Id,
					X = startLnData.X,
					Y = startLnData.Y,
					Z = startLnData.Z,
					Z_MSL = startLnData.Z_MSL,
					CodeRunway = CodeRunwayPointRole.THR,
					Path = startLnData.Path
				};

				result.Insert ( 0, thrLnData );
				result.Insert ( 1, startLnData );
				result.Add ( endLnData );
			}

			if ( arpLineData != null )
				result.Add ( arpLineData );
			return result;
		}

		private string SetElev_MSL ( LineData lnData, bool loadsAll = true )
		{
			string utmFileName = "";
			string replacableText = "";
			if ( _loadedUtmFileNames.Contains ( lnData.Path ) )
			{
				if ( !_utmValues.ContainsKey ( lnData.Id ) )
				{
					return lnData.Id + " (located in " + lnData.Path + ") doesn't exist in UTM file";
				}
				lnData.Z_MSL = _utmValues[ lnData.Id ];
				return "";
			}

			if ( lnData.Path.Contains ( "wgs-84" ) )
				replacableText = "wgs-84";
			else if ( lnData.Path.Contains ( "wgs_84" ) )
				replacableText = "wgs_84";
			else if ( lnData.Path.Contains ( "wgs84" ) )
				replacableText = "wgs84";
			else if (lnData.Path.Contains("WGS84"))
				replacableText = "WGS84";
			else if ( lnData.Path.Contains ( "wgs" ) )
				replacableText = "wgs";
			else
				throw new Exception ( "It is not wgs 84 file" );

			utmFileName = lnData.Path.Replace ( replacableText, "utm" );
			if ( !File.Exists ( utmFileName ) )
			{
				utmFileName = lnData.Path.Replace ( replacableText, "utm" + _utmCoord );
				if ( !File.Exists ( utmFileName ) )
					throw new NotImplementedException ( "Not found utm file of \'" + lnData.Path + "\'" );
			}

			string lowerLine;
			bool found = false;
			using ( StreamReader sr = File.OpenText ( utmFileName ) )
			{
				string line;
				while ( ( line = sr.ReadLine ( ) ) != null )
				{
					lowerLine = line.ToLower ( );
					if ( lowerLine.Length == 0 || lowerLine.Contains ( ArpCode ) || lowerLine.Contains ( "sn" ) || lowerLine.Contains ( "sv" ))
						continue;
					string[] sa = line.Split ( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
					if ( sa.Length < 4 )
						throw new NotImplementedException ( "Not implemented this format in \'" + utmFileName + "\' UTM file " );
					if ( sa[ 0 ] != lnData.Id )
					{
						if ( _utmValues.ContainsKey ( sa[ 0 ] ) )
							_utmValues[ sa[ 0 ] ] = double.Parse ( sa[ 3 ] );
						else
							_utmValues.Add ( sa[ 0 ], double.Parse ( sa[ 3 ] ) );
					}
					else
					{
						lnData.Z_MSL = double.Parse ( sa[ 3 ] );
						if ( loadsAll )
							_utmValues.Add ( sa[ 0 ], double.Parse ( sa[ 3 ] ) );
						found = true;
					}
					if ( !loadsAll )
						break;
				}
			}
			if ( !found )
				return lnData.Id + " (located in " + lnData.Path + ") doesn't exist in UTM file";
			if ( loadsAll )
				_loadedUtmFileNames.Add ( lnData.Path );
			return "";
		}

		private List<string> SetElev_MSL ( List<LineData> lnDataList, string beginningReportstr )
		{
			List<string> reportList = new List<string> ( );
			List<int> deleteIndices = new List<int> ( );
			string tmp;
			for ( int i = 0; i < lnDataList.Count; i++ )
			{
				tmp = SetElev_MSL ( lnDataList[ i ] );
				if ( tmp != "" )
				{
					reportList.Add ( beginningReportstr + tmp );
					deleteIndices.Add ( i );
				}
			}
			foreach ( int index in deleteIndices )
			{
				lnDataList.RemoveAt ( index );
			}
			return reportList;

			//string wgs84Text = "wgs-84";
			//string wgs84_Text = "wgs_84";
			//string utmFileName = "";
			//if ( lnDataList [ 0 ].FileName.Contains ( wgs84Text ) || lnDataList [ 0 ].FileName.Contains ( wgs84_Text ) )
			//    utmFileName = lnDataList [ 0 ].FileName.Replace ( wgs84Text, "utm" );
			//else
			//    throw new Exception ( "It is not wgs 84 file" );

			//using ( StreamReader sr = File.OpenText ( utmFileName ) )
			//{
			//    string line;
			//    while ( ( line = sr.ReadLine ( ) ) != null )
			//    {
			//        if ( line.Length > 0 && !line.ToLower ( ).StartsWith ( "iisn" ) )
			//        {
			//            string [] sa = line.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			//            if ( sa.Length < 4)
			//                continue;
			//            LineData lineData = lnDataList.Where ( lnData => lnData.Id == sa [ 0 ] ).FirstOrDefault ( );
			//            lineData.Z_MSL = double.Parse ( sa [ 3 ] );
			//        }
			//    }
			//}
		}

		private List<LineData> ParseFolder ( string folderPath, bool onlyWgs84Files, bool replaceDecSeparatorWithDot )
		{
			var filteredFiles = Directory.EnumerateFiles ( folderPath, "*.txt", SearchOption.AllDirectories );
			List<LineData> result = new List<LineData> ( );
			string s;
			foreach ( string file in filteredFiles )
			{
				s = file.ToLower ( );
				if ( onlyWgs84Files )
				{
					if ( s.Contains ( "wgs" ) || s.Contains ( "wgs84" ) || s.Contains ( "wgs-84" ) || s.Contains ( "wgs_84" ) )
					{
						result.AddRange ( ParseFile ( s, replaceDecSeparatorWithDot ) );
					}
				}
				else
				{
					result.AddRange ( ParseFile ( s, replaceDecSeparatorWithDot ) );
				}
			}
			result.RemoveAll ( ( lnData ) =>
			{

				return ( ArpCode != null && lnData.Id.Contains ( ArpCode ) ) || lnData.Id.Contains ( "SN" ) || lnData.Id.Contains ( "SV" );
				//|| lnData.Id.Contains ( "SB" )
			} );
			return result;
		}

		private List<LineData> ParseFile ( string fileName, bool replaceDecSeparatorWithDot )
		{
			List<LineData> lineList = new List<LineData> ( );
			string lowerLine;
			using ( StreamReader sr = File.OpenText ( fileName ) )
			{
				string line;
				LineData lnData;
				//sr.ReadLine ( );
				while ( ( line = sr.ReadLine ( ) ) != null )
				{
					lowerLine = line.ToLower ( ).TrimStart ( '\t' );
					if ( lowerLine.StartsWith ( "point" ) || lowerLine.StartsWith ( "id point" ) || lowerLine == "" )
						continue;
					if ( replaceDecSeparatorWithDot )
						line = line.Replace ( ',', '.' );
					if ( line.Length > 0 )
					{
						lnData = ToLineData ( line );
						lnData.Path = fileName;
						lineList.Add ( lnData );
					}
				}
			}
			return lineList;
		}

		private void CreateReport ( List<string> reportList, string fileName = "Report.txt" )
		{
			if ( reportList.Count == 0 )
				return;

			using ( StreamWriter streamWriter = File.CreateText ( Application.StartupPath + @"\" + fileName ) )
			{
				foreach ( string report in reportList )
				{
					streamWriter.WriteLine ( report );
				}
			}
			Process.Start ( Application.StartupPath + @"\" + fileName );
		}

		private LineData ToLineData ( string lineText )
		{
			LineData ld = new LineData ( );
			string[] sa = lineText.Split ( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			if ( sa.Length < 4 )
			{
				sa = lineText.Split ( new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries );
				ld.Id = sa[ 0 ];
				ld.Y = CoordStrToDouble ( sa[ 1 ] + ' ' + sa[ 2 ] + ' ' + sa[ 3 ] ); //--- LAT
				ld.X = CoordStrToDouble ( sa[ 4 ] + ' ' + sa[ 5 ] + ' ' + sa[ 6 ] ); //--- LON
				if ( sa.Length > 7 )
				{
					ld.Z = double.Parse ( sa[ 7 ] );
					//if ( sa.Length > 8 )
					//	ld.Z_MSL = double.Parse ( sa[ 8 ] );
					if ( sa.Length > 8 )
						ld.Code = sa[ 8 ];
				}
			}
			else
			{
				ld.Id = sa[ 0 ];
				if ( sa.Length == 4 || sa.Length == 5 )
				{
					ld.Y = CoordStrToDouble ( sa[ 1 ] ); //--- LAT
					ld.X = CoordStrToDouble ( sa[ 2 ] ); //--- LON
					ld.Z = double.Parse ( sa[ 3 ] );
					if ( sa.Length > 4 )
						ld.Code = sa[ 4 ];
				}
				else
				{
					//Decimal seperator is comma( , ) symbol					
					ld.Y = CoordStrToDouble ( sa[ 1 ] + "." + sa[ 2 ] );
					ld.X = CoordStrToDouble ( sa[ 3 ] + "." + sa[ 4 ] );
					ld.Z = double.Parse ( sa[ 5 ] + "." + sa[ 6 ] );
					if ( sa.Length > 7 )
						ld.Code = sa[ 7 ];
				}
			}
			return ld;
		}

		private double CoordStrToDouble ( string lat )
		{
			string[] sa = lat.Split ( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

			double degree = double.NaN, minute = double.NaN, second = double.NaN;

			if ( sa.Length == 3 )
			{
				degree = double.Parse ( sa[ 0 ] );
				minute = double.Parse ( sa[ 1 ] );
				second = double.Parse ( sa[ 2 ] );
			}
			else
			{
				bool degreeFound = false, minuteFound = false, secondFound = false;
				int index = 0;
				string tmp = "";
				while ( !( degreeFound && minuteFound && secondFound ) )
				{
					if ( char.IsDigit ( lat[ index ] ) || lat[ index ] == '.' || lat[ index ] == ',' )
					{
						tmp += lat[ index ];
					}
					else
					{
						if ( !degreeFound )
						{
							degree = Double.Parse ( tmp );
							degreeFound = true;
						}
						else if ( !minuteFound )
						{
							minute = double.Parse ( tmp );
							minuteFound = true;
						}
						else
						{
							second = double.Parse ( tmp );
							secondFound = true;
						}
						tmp = "";
					}
					index++;
				}

				//int degreeIndex = lat.IndexOf ( "°" ) ;
				//int minutIndex = lat.IndexOf ( "'" );
				//degree = double.Parse ( lat.Substring ( 0, degreeIndex) );
				//minute = double.Parse ( lat.Substring ( degreeIndex + 1, minutIndex - degreeIndex -1) );
				//second = double.Parse ( lat.Substring ( minutIndex + 1, lat.IndexOf ( "\"" ) - minutIndex - 1 ) );
			}

			return degree + ( minute / 60 ) + ( second / 3600 );
		}

		public string FolderPath
		{
			get
			{
				return _folderPath;
			}

			set
			{
				_folderPath = value;
				_featConv.OpenFeatureClass ( System.IO.Path.Combine ( _folderPath, "KAZLayers.mdb" ) );
				string fileName = _folderPath.Substring ( _folderPath.LastIndexOf ( System.IO.Path.DirectorySeparatorChar ) + 1 );
				if ( !fileName.Contains ( "(" ) )
					ArpCode = fileName.Substring ( 0, 4 );
				else
					ArpCode = fileName.Substring ( fileName.LastIndexOf ( "(" ) - 4, 4 );
				if ( ArpCode.Substring ( 0, 2 ).ToUpper ( ) != "UA" )
					throw new NotImplementedException ( ArpCode + "  code is not implmemented " );
				_dbWriter.SetArpCode ( ArpCode );
				//switch ( ArpCode )
				//{
				//	// ALMATY
				//	case "UACC":
				//	case "UAAA":
				//	case "UAUU":
				//		_utmCoord = "";
				//		break;
				//	// TARAZ
				//	case "UADD":
				//	case "UAKD":
				//		_utmCoord = "42";
				//		break;
				//	// SHYMKENT
				//	case "UAII":
				//		_utmCoord = "";
				//		break;
				//	// URALSK
				//	case "UARR":
				//		_utmCoord = "39";
				//		break;

				//	// AKTAU
				//	case "UATE":
				//		_utmCoord = "39";
				//		break;
				//	// ATYRAU
				//	case "UATG":
				//		_utmCoord = "39";
				//		break;
				//	// AKTOBE
				//	case "UATT":
				//		_utmCoord = "40";
				//		break;
				//	case "UAOO":
				//		_utmCoord = "41";
				//		break;
				//	case "UAAH":
				//		_utmCoord = "43";
				//		break;

				//	default:
				//		throw new NotImplementedException ( "UTM file coord of Airport of " + ArpCode + " is not implemented" );
				//}
			}
		}

		public string ArpCode
		{
			get;
			set;
		}

		public bool Insert2DB
		{
			get;
			set;
		}

		internal void SetFromMDB ( bool fromMdb )
		{
			_readFromMdb = fromMdb;
		}

		private DataTable ImportDataTable ( string fileName )
		{
			string commandText = "SELECT * FROM [Sheet1$]";
			string connectionString = string.Format ( @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties={1}Excel 12.0;HDR=No{2}", fileName, Convert.ToChar ( 34 ), Convert.ToChar ( 34 ) );
			DataTable dataTable = new DataTable ( );

			using ( OleDbConnection connection = new OleDbConnection ( connectionString ) )
			{
				try
				{
					connection.Open ( );
					LoadCodeVerticalStructure ( connection );
					using ( OleDbDataAdapter adapter = new OleDbDataAdapter ( commandText, connection ) )
					{
						adapter.Fill ( dataTable );
					}
				}
				catch ( Exception ex )
				{
					MessageBox.Show ( ex.Message );
				}
				finally
				{
					connection.Dispose ( );
					connection.Close ( );
				}
			}
			return dataTable;
		}

		private void LoadCodeVerticalStructure ( OleDbConnection connection )
		{
			string commandText = "SELECT * FROM [Sheet2$]";
			DataTable dataTable = new DataTable ( );
			using ( OleDbDataAdapter adapter = new OleDbDataAdapter ( commandText, connection ) )
			{
				adapter.Fill ( dataTable );
			}
			_codeVerticalStructures = new Dictionary<string, CodeVerticalStructure> ( );
			int codeIndex = 0;
			CodeVerticalStructure[] codeVertStructIndices = ( CodeVerticalStructure[] ) Enum.GetValues ( typeof ( CodeVerticalStructure ) );
			foreach ( DataRow row in dataTable.Rows )
			{
				if ( row[ 0 ] != DBNull.Value && codeIndex != 54 )
				{
					_codeVerticalStructures.Add ( row[ 0 ].ToString ( ), codeVertStructIndices[ codeIndex ] );
					codeIndex++;
				}
			}
		}

		public string StartMulti = "startMulti";
		public string EndMulti = "endMulti";

		private Dictionary<string, double> _utmValues;
		private List<string> _loadedUtmFileNames;
		private FeatureConverter _featConv;
		private DbWriter _dbWriter;
		private string _folderPath;
		private string _utmCoord;
		private Dictionary<string, CodeVerticalStructure> _codeVerticalStructures;
		private bool _readFromMdb;
		private List<string> _report;
	}
}