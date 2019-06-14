using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Aran.Geometries.Operators;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using ESRI.ArcGIS;
using Aran.Geometries.SpatialReferences;

namespace KFileParser
{
	public class FeatureConverter
	{
		private ISpatialReference _spatRefWGS84;

		public FeatureConverter ( )
		{
			_spatRefWGS84 = CreateWGS84SR ( );
		}

		public void OpenFeatureClass ( string fileName )
		{
			if ( fileName != _fileName )
				_fileName = fileName;
			else
				return;

			//IAoInitialize ao = new AoInitialize ( );
			try
			{
				RuntimeManager.BindLicense ( ProductCode.Desktop );
				//ao.Initialize ( esriLicenseProductCode. );// .esriLicenseProductCodeArcView );				
				IWorkspaceFactory wf = new AccessWorkspaceFactory ( );
				_featureWorkspace = wf.OpenFromFile ( fileName, 0 ) as IFeatureWorkspace;
			}
			catch ( Exception ex )
			{
				throw ex;
			}


			//IsOpen = true;
		}

		//public bool IsOpen
		//{
		//    get;
		//    private set;
		//}

		internal void AddRunwayCenterLinePoint ( List<LineData> lineDataList )
		{
			if ( _rwyFeatClass == null )
			{
				_rwyFeatClass = _featureWorkspace.OpenFeatureClass ( "Runway_CenterLinePoint" );

				_rwyIndices = new int[ 5 ];
				_rwyIndices[ 0 ] = _rwyFeatClass.FindField ( "ID" );
				_rwyIndices[ 1 ] = _rwyFeatClass.FindField ( "Elev" );
				_rwyIndices[ 2 ] = _rwyFeatClass.FindField ( "Elev_MSL" );
				_rwyIndices[ 3 ] = _rwyFeatClass.FindField ( "Role" );
			}

			foreach ( LineData data in lineDataList )
			{
				IFeature feature = _rwyFeatClass.CreateFeature ( );
				try
				{
					IPoint point = CreatePoint ( data );

					feature.Shape = point;
					feature.set_Value ( _rwyIndices[ 0 ], data.Id );
					feature.set_Value ( _rwyIndices[ 1 ], data.Z );
					feature.set_Value ( _rwyIndices[ 2 ], data.Z_MSL );
					feature.set_Value ( _rwyIndices[ 3 ], data.CodeRunway.ToString ( ) );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}

		internal void AddTaxiHoldingPositions ( List<LineData> lineDataList )
		{
			if ( _taxiHoldingPositon == null )
			{
				_taxiHoldingPositon = _featureWorkspace.OpenFeatureClass ( "TaxiHoldingPosition" );

				_taxiHoldingPosIndices = new int[ 5 ];
				_taxiHoldingPosIndices[ 0 ] = _taxiHoldingPositon.FindField ( "ID" );
				_taxiHoldingPosIndices[ 1 ] = _taxiHoldingPositon.FindField ( "Geoid_Undulation" );
				//_taxiHoldingPosIndices[ 1 ] = _taxiHoldingPositon.FindField ( "Elev" );
				//_taxiHoldingPosIndices[ 2 ] = _taxiHoldingPositon.FindField ( "Elev_MSL" );
			}

			foreach ( LineData data in lineDataList )
			{
				IFeature feature = _taxiHoldingPositon.CreateFeature ( );
				try
				{
					IPoint point = CreatePoint ( data, true );

					feature.Shape = point;
					feature.set_Value ( _taxiHoldingPosIndices[ 0 ], data.Id );
					//feature.set_Value ( _taxiHoldingPosIndices[ 1 ], data.Z );
					feature.set_Value ( _taxiHoldingPosIndices[ 1 ], data.Z_MSL );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}
		//internal List<string> AddVS_Polygon ( List<LineData> lineDataList, ref List<VerticalStructure> polygonVertStructList, bool sort = true )
		//{
		//    Dictionary<string, List<LineData>> dict = CreateVSDict ( lineDataList );
		//    List<string> resultReport = new List<string> ( );
		//    if ( _vsPolygonFeatClass == null )
		//    {
		//        _vsPolygonFeatClass = _featureWorkspace.OpenFeatureClass ( "VS_Polygon" );

		//        _vsIndexes = new int[ 11 ];
		//        _vsIndexes[ 0 ] = _vsPolygonFeatClass.FindField ( "ID" );
		//        _vsIndexes[ 1 ] = _vsPolygonFeatClass.FindField ( "Elevation" );
		//        _vsIndexes[ 2 ] = _vsPolygonFeatClass.FindField ( "Height" );
		//        _vsIndexes[ 3 ] = _vsPolygonFeatClass.FindField ( "Point_Count" );
		//        _vsIndexes[ 4 ] = _vsPolygonFeatClass.FindField ( "Up_Down" );
		//        _vsIndexes[ 5 ] = _vsPolygonFeatClass.FindField ( "Type" );

		//        _vsIndexes[ 6 ] = _vsPolygonFeatClass.FindField ( "Frangible" );
		//        _vsIndexes[ 7 ] = _vsPolygonFeatClass.FindField ( "Lighted" );
		//        _vsIndexes[ 8 ] = _vsPolygonFeatClass.FindField ( "Color" );
		//        _vsIndexes[ 9 ] = _vsPolygonFeatClass.FindField ( "Intensivity" );
		//        _vsIndexes[ 10 ] = _vsPolygonFeatClass.FindField ( "Marked" );
		//    }

		//    List<LineData> lineDataListUp, lineDataListDown;
		//    double z_msl, z_maxUp, z_minDown;
		//    string up_downReport;
		//    int pointCount;
		//    ITopologicalOperator2 topoOper2;
		//    IGeometry convexGeom;
		//    string tmp;
		//    foreach ( string key in dict.Keys )
		//    {
		//        List<LineData> ldList = dict[ key ];
		//        VerticalStructure vertStruct = Global.CreateVerticalStructure ( key );

		//        lineDataListUp = ldList.Where ( lineData => lineData.Path.Contains ( "_u_" ) ).ToList ( );
		//        if ( lineDataListUp == null || lineDataListUp.Count == 0 )
		//        {
		//            resultReport.Add ( key + " has no Up Vertical Structure Line" );
		//            continue;
		//        }

		//        lineDataListDown = ldList.Where ( lineData => lineData.Path.Contains ( "_d_" ) ).ToList ( );
		//        if ( lineDataListDown == null || lineDataListDown.Count == 0 )
		//        {
		//            resultReport.Add ( key + " has no Down Vertical Structure Line" );
		//            continue;
		//        }
		//        IPolygon polygon;
		//        if ( key.Contains ( "117" ) || key.Contains ( "146" ) || key.Contains ( "201" ) || key.Contains ( "213" ) )
		//        {

		//        }
		//        if ( lineDataListUp.Count <= lineDataListDown.Count )
		//        {
		//            polygon = CreatePolygon ( lineDataListDown, false, sort );
		//            //					= Global.GetElevatedSurface ( lineDataListDown );
		//            up_downReport = "Down";
		//            pointCount = lineDataListDown.Count;
		//        }
		//        else
		//        {
		//            polygon = CreatePolygon ( lineDataListUp, false, sort );
		//            //					vertStruct.Part[ 0 ].HorizontalProjection.SurfaceExtent = Global.GetElevatedSurface ( lineDataListUp );
		//            up_downReport = "Up";
		//            pointCount = lineDataListUp.Count;
		//        }
		//        IFeature feature = _vsPolygonFeatClass.CreateFeature ( );

		//        if ( key.Contains ( "117" ) || key.Contains ( "146" ) || key.Contains ( "201" ) || key.Contains ( "213" ) )
		//        {
		//            convexGeom = polygon;
		//        }
		//        else
		//        {

		//            topoOper2 = ( ITopologicalOperator2 ) polygon;
		//            convexGeom = topoOper2.ConvexHull ( );

		//            topoOper2 = ( ITopologicalOperator2 ) convexGeom;
		//            topoOper2.IsKnownSimple_2 = false;
		//            topoOper2.Simplify ( );
		//        }

		//        vertStruct.Part[ 0 ].HorizontalProjection.SurfaceExtent = new ElevatedSurface ( );
		//        MultiPolygon multiPolygon = ( Aran.Geometries.MultiPolygon ) ConvertFromEsriGeom.ToPolygonGeo ( ( IPolygon ) convexGeom );
		//        for ( int i = 0; i < multiPolygon.Count; i++ )
		//        {
		//            vertStruct.Part[ 0 ].HorizontalProjection.SurfaceExtent.Geo.Add ( multiPolygon[ i ] );
		//        }

		//        feature.Shape = convexGeom;
		//        feature.set_Value ( _vsIndexes[ 0 ], key );

		//        z_maxUp = lineDataListUp.Max ( lineData => lineData.Z_MSL );
		//        z_minDown = lineDataListDown.Min ( lineData => lineData.Z_MSL );
		//        vertStruct.Part[ 0 ].VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( z_maxUp - z_minDown, UomDistance.M );
		//        SetVertStructPropsFromExcel ( vertStruct );

		//        feature.set_Value ( _vsIndexes[ 1 ], z_maxUp );
		//        z_msl = 0;
		//        foreach ( var item in lineDataListDown )
		//        {
		//            z_msl += item.Z_MSL;
		//        }

		//        feature.set_Value ( _vsIndexes[ 2 ], z_maxUp - z_msl / lineDataListDown.Count );
		//        feature.set_Value ( _vsIndexes[ 3 ], pointCount );
		//        feature.set_Value ( _vsIndexes[ 4 ], up_downReport );
		//        tmp = vertStruct.Type.ToString ( );
		//        feature.set_Value ( _vsIndexes[ 5 ], tmp );

		//        if ( vertStruct.Part[ 0 ].Frangible.HasValue )
		//            feature.set_Value ( _vsIndexes[ 6 ], "Yes" );
		//        else
		//            feature.set_Value ( _vsIndexes[ 6 ], "No" );

		//        if ( vertStruct.Part[ 0 ].Lighting.Count > 0 )
		//        {
		//            feature.set_Value ( _vsIndexes[ 7 ], "Yes" );
		//            tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].Colour.ToString ( );
		//            feature.set_Value ( _vsIndexes[ 8 ], tmp );
		//            tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].IntensityLevel.ToString ( );
		//            feature.set_Value ( _vsIndexes[ 9 ], tmp );
		//        }
		//        else
		//            feature.set_Value ( _vsIndexes[ 7 ], "No" );

		//        if ( vertStruct.MarkingICAOStandard.HasValue )
		//            feature.set_Value ( _vsIndexes[ 10 ], "Yes" );
		//        else
		//            feature.set_Value ( _vsIndexes[ 10 ], "No" );

		//        polygonVertStructList.Add ( vertStruct );

		//        feature.Store ( );
		//    }
		//    return resultReport;
		//}

		//internal List<string> AddVS_Line ( List<LineData> lineDataList, ref List<VerticalStructure> lineVertStructList, bool sort = false )
		//{
		//    Dictionary<string, List<LineData>> dict = CreateVSDict ( lineDataList );
		//    List<string> resultReport = new List<string> ( );

		//    if ( _vsLineFeatClass == null )
		//    {
		//        _vsLineFeatClass = _featureWorkspace.OpenFeatureClass ( "VS_Line" );

		//        _vsIndexes = new int[ 11 ];
		//        _vsIndexes[ 0 ] = _vsLineFeatClass.FindField ( "ID" );
		//        _vsIndexes[ 1 ] = _vsLineFeatClass.FindField ( "Elevation" );
		//        _vsIndexes[ 2 ] = _vsLineFeatClass.FindField ( "Height" );
		//        _vsIndexes[ 3 ] = _vsLineFeatClass.FindField ( "Point_Count" );
		//        _vsIndexes[ 4 ] = _vsLineFeatClass.FindField ( "Up_Down" );
		//        _vsIndexes[ 5 ] = _vsLineFeatClass.FindField ( "Type" );

		//        _vsIndexes[ 6 ] = _vsLineFeatClass.FindField ( "Frangible" );
		//        _vsIndexes[ 7 ] = _vsLineFeatClass.FindField ( "Lighted" );
		//        _vsIndexes[ 8 ] = _vsLineFeatClass.FindField ( "Color" );
		//        _vsIndexes[ 9 ] = _vsLineFeatClass.FindField ( "Intensivity" );
		//        _vsIndexes[ 10 ] = _vsLineFeatClass.FindField ( "Marked" );

		//    }

		//    List<LineData> lineDataListUp, lineDataListDown;
		//    double z_msl, z_max, z_min;
		//    int pointCount;
		//    string up_DownReport;
		//    string tmp;
		//    string reportStr = "Vertical Structure (Line) :";
		//    foreach ( string key in dict.Keys )
		//    {
		//        List<LineData> ldList = dict[ key ];
		//        VerticalStructure vertStruct = Global.CreateVerticalStructure ( key );

		//        if ( key.Contains ( "070" ) || key.Contains ( "072" ) || key.Contains ( "215" ) )
		//        {
		//        }
		//        lineDataListUp = ldList.Where ( lineData => lineData.Path.Contains ( "_u_" ) ).ToList ( );
		//        if ( lineDataListUp == null || lineDataListUp.Count == 0 )
		//        {
		//            resultReport.Add ( key + " has no Up Vertical Structure Line" );
		//            continue;
		//        }

		//        lineDataListDown = ldList.Where ( lineData => lineData.Path.Contains ( "_d_" ) ).ToList ( );
		//        if ( lineDataListDown == null || lineDataListDown.Count == 0 )
		//        {
		//            resultReport.Add ( key + " has no Down Vertical Structure Line" );
		//            continue;
		//        }

		//        IPolyline polyLine;
		//        if ( lineDataListUp.Count < lineDataListDown.Count )
		//        {
		//            polyLine = CreatePolyline ( lineDataListDown, true );//, sort );
		//            vertStruct.Part[ 0 ].HorizontalProjection.LinearExtent = Global.GetElevatedCurve ( lineDataListDown );
		//            //resultLines.Add ( key, lineDataListDown );
		//            pointCount = lineDataListDown.Count;
		//            up_DownReport = "Down";
		//        }
		//        else
		//        {
		//            polyLine = CreatePolyline ( lineDataListUp, true );//, sort );
		//            vertStruct.Part[ 0 ].HorizontalProjection.LinearExtent = Global.GetElevatedCurve ( lineDataListUp );
		//            //resultLines.Add ( key, lineDataListUp );
		//            pointCount = lineDataListUp.Count;
		//            up_DownReport = "Up";
		//        }
		//        SetVertStructPropsFromExcel ( vertStruct );

		//        IFeature feature = _vsLineFeatClass.CreateFeature ( );
		//        feature.Shape = polyLine;
		//        feature.set_Value ( _vsIndexes[ 0 ], key );

		//        z_max = lineDataListUp.Max ( lineData => lineData.Z_MSL );
		//        z_min = lineDataListDown.Min ( lineData => lineData.Z_MSL );
		//        vertStruct.Part[ 0 ].VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( z_max - z_min, UomDistance.M );

		//        feature.set_Value ( _vsIndexes[ 1 ], z_max );
		//        z_msl = 0;
		//        foreach ( var item in lineDataListDown )
		//        {
		//            z_msl += item.Z_MSL;
		//        }

		//        feature.set_Value ( _vsIndexes[ 2 ], z_max - z_msl / lineDataListDown.Count );
		//        feature.set_Value ( _vsIndexes[ 3 ], pointCount );
		//        feature.set_Value ( _vsIndexes[ 4 ], up_DownReport );
		//        tmp = vertStruct.Type.ToString ( );
		//        feature.set_Value ( _vsIndexes[ 5 ], tmp );

		//        if ( vertStruct.Part[ 0 ].Frangible.HasValue )
		//            feature.set_Value ( _vsIndexes[ 6 ], "Yes" );
		//        else
		//            feature.set_Value ( _vsIndexes[ 6 ], "No" );

		//        if ( vertStruct.Part[ 0 ].Lighting.Count > 0 )
		//        {
		//            feature.set_Value ( _vsIndexes[ 7 ], "Yes" );
		//            tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].Colour.ToString ( );
		//            feature.set_Value ( _vsIndexes[ 8 ], tmp );
		//            tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].IntensityLevel.ToString ( );
		//            feature.set_Value ( _vsIndexes[ 9 ], tmp );
		//        }
		//        else
		//            feature.set_Value ( _vsIndexes[ 7 ], "No" );

		//        if ( vertStruct.MarkingICAOStandard.HasValue )
		//            feature.set_Value ( _vsIndexes[ 10 ], "Yes" );
		//        else
		//            feature.set_Value ( _vsIndexes[ 10 ], "No" );

		//        lineVertStructList.Add ( vertStruct );
		//        feature.Store ( );
		//    }
		//    return resultReport;
		//}

		//internal List<string> AddVS_Point ( List<LineData> lineDataList, ref List<VerticalStructure> pointVertStructList )
		//{
		//    Dictionary<string, List<LineData>> dict = CreateVSDict ( lineDataList );
		//    List<string> reportList = new List<string> ( );

		//    if ( _vsPointFeatClass == null )
		//    {
		//        _vsPointFeatClass = _featureWorkspace.OpenFeatureClass ( "VS_Point" );

		//        _vsIndexes = new int[ 9 ];
		//        _vsIndexes[ 0 ] = _vsPointFeatClass.FindField ( "ID" );
		//        _vsIndexes[ 1 ] = _vsPointFeatClass.FindField ( "Elevation" );
		//        _vsIndexes[ 2 ] = _vsPointFeatClass.FindField ( "Height" );
		//        _vsIndexes[ 3 ] = _vsPointFeatClass.FindField ( "Type" );
		//        _vsIndexes[ 4 ] = _vsPointFeatClass.FindField ( "Frangible" );
		//        _vsIndexes[ 5 ] = _vsPointFeatClass.FindField ( "Lighted" );
		//        _vsIndexes[ 6 ] = _vsPointFeatClass.FindField ( "Color" );
		//        _vsIndexes[ 7 ] = _vsPointFeatClass.FindField ( "Intensivity" );
		//        _vsIndexes[ 8 ] = _vsPointFeatClass.FindField ( "Marked" );
		//    }

		//    LineData lnDataUp, lnDataDown = new LineData ( );
		//    List<LineData> lnDataDownList;
		//    string tmp;
		//    string reportStr = "Vertical Structure (Point) :";
		//    foreach ( string key in dict.Keys )
		//    {
		//        List<LineData> ldList = dict[ key ];
		//        VerticalStructure vertStruct = Global.CreateVerticalStructure ( key );

		//        lnDataUp = ldList.Where ( lineData => lineData.Path.Contains ( "_u_" ) ).FirstOrDefault ( );
		//        if ( lnDataUp == null )
		//        {
		//            reportList.Add ( reportStr + key.ToString ( ) + " has no Up Vertical Structure Point" );
		//            continue;
		//        }
		//        IPoint point = CreatePoint ( lnDataUp );

		//        vertStruct.Part[ 0 ].HorizontalProjection.Location = Global.GetELevatedPoint ( lnDataUp );
		//        SetVertStructPropsFromExcel ( vertStruct );
		//        vertStruct.Part[ 0 ].VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( lnDataUp.Z - lnDataDown.Z, UomDistance.M );

		//        IFeature feature = _vsPointFeatClass.CreateFeature ( );
		//        feature.Shape = point;
		//        feature.set_Value ( _vsIndexes[ 0 ], key );
		//        feature.set_Value ( _vsIndexes[ 1 ], lnDataUp.Z_MSL );
		//        lnDataDownList = ldList.Where ( lineData => lineData.Path.Contains ( "_d_" ) ).ToList ( );
		//        if ( lnDataDownList == null || lnDataDownList.Count == 0 )
		//        {
		//            reportList.Add ( reportStr + key.ToString ( ) + " has no Down Vertical Structure Point" );
		//            continue;
		//        }
		//        if ( lnDataDownList.Count > 1 )
		//        {
		//            double z = 0;
		//            foreach ( LineData lnData in lnDataDownList )
		//            {
		//                z += lnData.Z;
		//            }
		//            lnDataDown.Z = z / lnDataDownList.Count;
		//        }
		//        else
		//            lnDataDown = lnDataDownList[ 0 ];
		//        feature.set_Value ( _vsIndexes[ 2 ], lnDataUp.Z - lnDataDown.Z );
		//        tmp = vertStruct.Type.ToString ( );
		//        feature.set_Value ( _vsIndexes[ 3 ], tmp );

		//        if ( vertStruct.Part[ 0 ].Frangible.HasValue )
		//            feature.set_Value ( _vsIndexes[ 4 ], "YesFrang" );
		//        else
		//            feature.set_Value ( _vsIndexes[ 4 ], "NoFrang" );

		//        if ( vertStruct.Part[ 0 ].Lighting.Count > 0 )
		//        {
		//            feature.set_Value ( _vsIndexes[ 5 ], "YesLight" );
		//            tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].Colour.ToString ( );
		//            feature.set_Value ( _vsIndexes[ 6 ], tmp );
		//            tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].IntensityLevel.ToString ( );
		//            feature.set_Value ( _vsIndexes[ 7 ], tmp );
		//        }
		//        else
		//            feature.set_Value ( _vsIndexes[ 5 ], "NoLight" );

		//        if ( vertStruct.MarkingICAOStandard.HasValue )
		//            feature.set_Value ( _vsIndexes[ 8 ], "YesMark" );
		//        else
		//            feature.set_Value ( _vsIndexes[ 8 ], "NoMark" );

		//        pointVertStructList.Add ( vertStruct );
		//        feature.Store ( );
		//    }
		//    return reportList;
		//}
		internal List<GuidanceLine> GetApronGuidanceLines ( )
		{
			IFeatureClass featureClass = _featureWorkspace.OpenFeatureClass ( "GuidanceLine_Apron" );
			return GetGuidanceLineList ( featureClass );
		}

		internal List<GuidanceLine> GetTaxiwayGuidanceLines ( )
		{
			IFeatureClass featureClass = _featureWorkspace.OpenFeatureClass ( "GuidanceLine_Taxiway" );
			return GetGuidanceLineList ( featureClass );
		}

		private List<GuidanceLine> GetGuidanceLineList ( IFeatureClass featureClass )
		{
			Dictionary<string, int> featIndices = new Dictionary<string, int> ( );
			featIndices.Add ( "ID", featureClass.FindField ( "ID" ) );

			IFeatureCursor featCursor = featureClass.Search ( null, true );
			IFeature feat = featCursor.NextFeature ( );
			string id, fullId;
			int partIndex;
			Dictionary<string, GuidanceLine> guidanceLineDict = new Dictionary<string, GuidanceLine> ( );
			while ( feat != null )
			{
				Aran.Geometries.MultiLineString mltLnString = ConvertFromEsriGeom.ToPolyline ( ( IPolyline ) feat.Shape, true );

				fullId = feat.get_Value ( featIndices[ "ID" ] );
				partIndex = fullId.ToLower ( ).IndexOf ( "part" );
				id = fullId;
				if ( partIndex > 0 )
					id = fullId.Substring ( 0, partIndex );
				// id = String.Concat ( fullId.TakeWhile ( ch => !char.IsWhiteSpace ( ch ) ) );
				if ( !guidanceLineDict.ContainsKey ( id ) )
				{
					GuidanceLine guidanceLine = Global.CreateGuidanceLine ( id );
					guidanceLineDict.Add ( id, guidanceLine );
				}
				if ( guidanceLineDict[ id ].Extent == null )
					guidanceLineDict[ id ].Extent = new ElevatedCurve ( );
				foreach ( LineString lnString in mltLnString )
					guidanceLineDict[ id ].Extent.Geo.Add ( lnString );
				feat = featCursor.NextFeature ( );
			}
			return guidanceLineDict.Values.ToList ( );
		}

		internal List<string> AddVS_Point ( Dictionary<string, List<LineData>> pointDict, ref List<VerticalStructure> pointVertStructList, bool isArea2 = false )
		{
			List<string> result = new List<string> ( );
			//SetPointFields ( );
			List<LineData> pntLnDataList;
			double elevation, height;
            CodeVerticalStructure obs_type;
			foreach ( string key in pointDict.Keys )
			{
				pntLnDataList = pointDict[ key ];
				elevation = double.NaN;
				height = double.NaN;
				List<LineData> up_or_downPnts;
                //elevation = pntLnDataList.Max(lnData => lnData.Z);
				//if ( pntLnDataList.Count > 1 )
				//    height = elevation - pntLnDataList.Min ( lnData => lnData.Z );

				if ( isArea2 )
					up_or_downPnts = GetUpOrDownPoints ( pntLnDataList, out elevation, out height );
				else
				{
					LineData upLnData = pntLnDataList.Find ( lnData => lnData.Id.EndsWith ( "U" ) );
					LineData downLnData = pntLnDataList.Find ( lnData => lnData.Id.EndsWith ( "D" ) );
					elevation = upLnData.Z;
					height = double.NaN;
					if ( downLnData != null )
						height = elevation - downLnData.Z;
					up_or_downPnts = new List<LineData> ( );
					up_or_downPnts.Add ( upLnData );
				}

				if ( pntLnDataList.Count > 2 )
				{
					LineData lnDataUp = pntLnDataList.Find ( lndata => lndata.Z == elevation );
					if ( lnDataUp == null )
					{
						result.Add ( key );
						continue;
					}
					pntLnDataList.RemoveAll ( lnData => lnData != lnDataUp );
					up_or_downPnts = pntLnDataList;
				}
				VerticalStructure vertStruct = Global.CreateVerticalStructure ( key );

                if (pntLnDataList[0].ZoneCode != null && Enum.TryParse<CodeVerticalStructure>(pntLnDataList[0].ZoneCode.ToUpper(), out obs_type))
                    vertStruct.Type = obs_type;

				IPoint point = CreatePoint ( up_or_downPnts[ 0 ] );

				VerticalStructurePart vertStructPart = new VerticalStructurePart ( );
				vertStructPart.HorizontalProjection = new VerticalStructurePartGeometry ( );
				vertStructPart.HorizontalProjection.Location = Global.GetELevatedPoint ( up_or_downPnts[ 0 ] );


                //IFeature feature = _vsPointFeatClass.CreateFeature ( );
                //feature.Shape = point;
                //feature.set_Value ( _vsIndices[ 0 ], key );
				if ( !double.IsNaN ( elevation ) )
				{
					//feature.set_Value ( _vsIndices[ 1 ], elevation );
					vertStructPart.HorizontalProjection.Location.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical ( elevation, UomDistanceVertical.M );
				}
				if ( !double.IsNaN ( height ) )
				{
					//feature.set_Value ( _vsIndices[ 2 ], height );
					vertStructPart.VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( height, UomDistance.M );
				}
				vertStruct.Part.Add ( vertStructPart );
				//SetVertStructPropsFromExcel ( vertStruct );				

				//if ( vertStruct.Part[ 0 ].Frangible.HasValue )
				//    feature.set_Value ( _vsIndexes[ 4 ], "YesFrang" );
				//else
				//    feature.set_Value ( _vsIndexes[ 4 ], "NoFrang" );

				//if ( vertStruct.Part[ 0 ].Lighting.Count > 0 )
				//{
				//    feature.set_Value ( _vsIndexes[ 5 ], "YesLight" );
				//    tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].Colour.ToString ( );
				//    feature.set_Value ( _vsIndexes[ 6 ], tmp );
				//    tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].IntensityLevel.ToString ( );
				//    feature.set_Value ( _vsIndexes[ 7 ], tmp );
				//}
				//else
				//    feature.set_Value ( _vsIndexes[ 5 ], "NoLight" );

				//if ( vertStruct.MarkingICAOStandard.HasValue )
				//    feature.set_Value ( _vsIndexes[ 8 ], "YesMark" );
				//else
				//    feature.set_Value ( _vsIndexes[ 8 ], "NoMark" );

				pointVertStructList.Add ( vertStruct );
				//feature.Store ( );
			}
			return result;
		}

		internal List<string> AddVS_Line ( Dictionary<string, List<LineData>> lineDict, ref List<VerticalStructure> lineVertStructList, bool sort = false, bool isArea2 = false )
		{
			//Dictionary<string, List<LineData>> dict = CreateVSDict ( lineDataList );
			//List<string> resultReport = new List<string> ( );

			SetLineFields ( );

			//LineData lineDatatUp;
			//LineData[] lineDataListDown;
			//string up_DownReport;
			List<LineData> lnDataList;
			double elevation, height;
			List<string> result = new List<string> ( );
			foreach ( string key in lineDict.Keys )
			{
				lnDataList = lineDict[ key ];
				elevation = double.NaN;
				height = double.NaN;
				//if ( key == "TG0059OB" )
				//{
				//}
				if ( isArea2 )
					lnDataList = GetUpOrDownPoints ( lnDataList, out elevation, out height);
				if ( lnDataList.Count < 2 )
				{
					result.Add ( key );
					continue;
				}
				VerticalStructure vertStruct = Global.CreateVerticalStructure ( key );
				IPolyline polyLine;
				polyLine = CreatePolyline ( lnDataList, true, true );

				VerticalStructurePart vertStructPart = new VerticalStructurePart ( );
				vertStructPart.HorizontalProjection = new VerticalStructurePartGeometry ( );
				vertStructPart.HorizontalProjection.LinearExtent = Global.GetElevatedCurve ( lnDataList );

				IFeature feature = _vsLineFeatClass.CreateFeature ( );
				feature.Shape = polyLine;
				feature.set_Value ( _vsIndices[ 0 ], key );

				if ( !double.IsNaN ( elevation ) )
				{
					vertStructPart.HorizontalProjection.LinearExtent.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical ( elevation, UomDistanceVertical.M );
					feature.set_Value ( _vsIndices[ 1 ], elevation );
				}

				if ( !double.IsNaN ( height ) )
				{
					vertStructPart.VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( height, UomDistance.M );
					feature.set_Value ( _vsIndices[ 2 ], vertStructPart.VerticalExtent.Value );
				}				
				vertStruct.Part.Add ( vertStructPart );

				//resultLines.Add ( key, lineDataListDown );
				//pointCount = lineDataListDown.Length;
				//up_DownReport = "Down";
				//SetVertStructPropsFromExcel ( vertStruct );


				//z_max = lineDatatUp.Z_MSL;
				//z_min = lineDataListDown.Min ( lineData => lineData.Z_MSL );
				//vertStruct.Part[ 0 ].VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( z_max - z_min, UomDistance.M );

				//z_msl = 0;
				//foreach ( var item in lineDataListDown )
				//{
				//    z_msl += item.Z_MSL;
				//}

				//feature.set_Value ( _vsIndexes[ 2 ], z_max - z_msl / lineDataListDown.Length );
				//feature.set_Value ( _vsIndexes[ 3 ], pointCount );
				//feature.set_Value ( _vsIndexes[ 4 ], up_DownReport );
				//tmp = vertStruct.Type.ToString ( );
				//feature.set_Value ( _vsIndexes[ 5 ], tmp );

				//if ( vertStruct.Part[ 0 ].Frangible.HasValue )
				//    feature.set_Value ( _vsIndexes[ 6 ], "Yes" );
				//else
				//    feature.set_Value ( _vsIndexes[ 6 ], "No" );

				//if ( vertStruct.Part[ 0 ].Lighting.Count > 0 )
				//{
				//    feature.set_Value ( _vsIndexes[ 7 ], "Yes" );
				//    tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].Colour.ToString ( );
				//    feature.set_Value ( _vsIndexes[ 8 ], tmp );
				//    tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].IntensityLevel.ToString ( );
				//    feature.set_Value ( _vsIndexes[ 9 ], tmp );
				//}
				//else
				//    feature.set_Value ( _vsIndexes[ 7 ], "No" );

				//if ( vertStruct.MarkingICAOStandard.HasValue )
				//    feature.set_Value ( _vsIndexes[ 10 ], "Yes" );
				//else
				//    feature.set_Value ( _vsIndexes[ 10 ], "No" );

				lineVertStructList.Add ( vertStruct );
				feature.Store ( );
			}
			return result;
		}

		internal List<string> AddVS_Polygon ( Dictionary<string, List<LineData>> polygonDict, ref List<VerticalStructure> polygonVertStructList, bool sort = true, bool isArea2 = false )
		{
			SetPolygonFields ( );

			List<LineData> lnDataList;
			List<string> result = new List<string> ( );
			double elevation, height;
			ISpatialReference spatialRefWGS84 = CreateWGS84SR ( );
			foreach ( string key in polygonDict.Keys )
			{
				lnDataList = polygonDict[ key ];
				elevation = double.NaN;
				height = double.NaN;
				if ( isArea2 )
					lnDataList = GetUpOrDownPoints ( lnDataList, out elevation, out height );
				if ( lnDataList.Count < 3 )
				{
					result.Add ( key );
					continue;
				}

				VerticalStructure vertStruct = Global.CreateVerticalStructure ( key );
				//AddRawPoints ( lnDataList );
				IPolygon polygon = CreatePolygon ( lnDataList, false, sort, true );

				IFeature feature = _vsPolygonFeatClass.CreateFeature ( );

				VerticalStructurePart vertStructPart = new VerticalStructurePart ( );
				vertStructPart.HorizontalProjection = new VerticalStructurePartGeometry ( );
				vertStructPart.HorizontalProjection.SurfaceExtent = new ElevatedSurface ( );
				if ( !double.IsNaN ( elevation ) )
				{
					vertStructPart.HorizontalProjection.SurfaceExtent.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical ( elevation, UomDistanceVertical.M );
					feature.set_Value ( _vsIndices[ 1 ], elevation );
				}

				if ( !double.IsNaN ( height ) )
				{
					vertStructPart.VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( height, UomDistance.M );
					feature.set_Value ( _vsIndices[ 2 ], height );
				}
				//else
				//    result.Add ( string.Format ( "Has no Z_Min of ", key ) );

				vertStruct.Part.Add ( vertStructPart );

				MultiPolygon multiPolygon;
				Geometry geometry = ConvertFromEsriGeom.ToPolygonGeo ( polygon );
				if ( geometry is Aran.Geometries.Polygon )
				{
					multiPolygon = new MultiPolygon ( );
					multiPolygon.Add ( ( Aran.Geometries.Polygon ) geometry );
				}
				else
					multiPolygon = ( MultiPolygon ) geometry;

				for ( int i = 0; i < multiPolygon.Count; i++ )
				{
					vertStruct.Part[ 0 ].HorizontalProjection.SurfaceExtent.Geo.Add ( multiPolygon[ i ] );
				}

				feature.Shape = polygon;
				feature.set_Value ( _vsIndices[ 0 ], key );

				//z_maxUp =  polygonDict[ key ] lineDataListUp.Z_MSL;
				//z_minDown = lineDataListDown.Min ( lineData => lineData.Z_MSL );
				//vertStruct.Part[ 0 ].VerticalExtent = new Aran.Aim.DataTypes.ValDistance ( z_maxUp - z_minDown, UomDistance.M );
				//SetVertStructPropsFromExcel ( vertStruct );

				//z_msl = 0;
				//foreach ( var item in lineDataListDown )
				//{
				//    z_msl += item.Z_MSL;
				//}

				//				feature.set_Value ( _vsIndexes[ 2 ], z_maxUp - z_msl / lineDataListDown.Length );
				//				feature.set_Value ( _vsIndexes[ 3 ], pointCount );
				//				feature.set_Value ( _vsIndexes[ 4 ], up_downReport );
				//tmp = vertStruct.Type.ToString ( );
				//feature.set_Value ( _vsIndexes[ 5 ], tmp );

				//if ( vertStruct.Part[ 0 ].Frangible.HasValue )
				//    feature.set_Value ( _vsIndexes[ 6 ], "Yes" );
				//else
				//    feature.set_Value ( _vsIndexes[ 6 ], "No" );

				//if ( vertStruct.Part[ 0 ].Lighting.Count > 0 )
				//{
				//    feature.set_Value ( _vsIndexes[ 7 ], "Yes" );
				//    tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].Colour.ToString ( );
				//    feature.set_Value ( _vsIndexes[ 8 ], tmp );
				//    tmp = vertStruct.Part[ 0 ].Lighting[ 0 ].IntensityLevel.ToString ( );
				//    feature.set_Value ( _vsIndexes[ 9 ], tmp );
				//}
				//else
				//    feature.set_Value ( _vsIndexes[ 7 ], "No" );

				//if ( vertStruct.MarkingICAOStandard.HasValue )
				//    feature.set_Value ( _vsIndexes[ 10 ], "Yes" );
				//else
				//    feature.set_Value ( _vsIndexes[ 10 ], "No" );

				polygonVertStructList.Add ( vertStruct );

				feature.Store ( );
			}
			return result;
		}

		private List<LineData> GetUpOrDownPoints ( List<LineData> lnDataList, out double elevation, out double height )
		{
			elevation = double.NaN;
			height = double.NaN;
			int countUpPnts = 0, countDownPnts = 0;
			bool hasUpOrDownFlag = ( lnDataList.Where ( lnData => lnData.IsUp.HasValue ).ToList ( ).Count > 0 );
			List<LineData> upLnDataList = lnDataList.Where ( lnData => lnData.IsUp.HasValue && lnData.IsUp.Value ).ToList ( );
			List<LineData> downLnDataList = lnDataList.Where ( lnData => lnData.IsUp.HasValue && !lnData.IsUp.Value ).ToList ( );
			countUpPnts = upLnDataList.Count;
			countDownPnts = downLnDataList.Count;
			if ( !hasUpOrDownFlag )
			{
				elevation = lnDataList.Max ( lnData => lnData.Z_MSL );
				height = lnDataList.Min ( lnData => lnData.Z );
				return lnDataList;
			}
			else if ( countUpPnts == 0 || countDownPnts == 0 )
			{
				elevation = lnDataList.Max ( lnData => lnData.Z );
				return lnDataList;
			}
			else
			{
				elevation = upLnDataList.Max ( lnData => lnData.Z );
				height = elevation - downLnDataList.Min ( lnData => lnData.Z );
				if ( countUpPnts >= countDownPnts )
					return upLnDataList;
				else
					return downLnDataList;
			}
		}

		private ISpatialReference CreateWGS84SR ( )
		{
			//ISpatialReferenceFactory spatialRefFatcory = new SpatialReferenceEnvironmentClass ( );
			//IGeographicCoordinateSystem geoCoordSys;
			//geoCoordSys = spatialRefFatcory.CreateGeographicCoordinateSystem ( ( int ) esriSRGeoCSType.esriSRGeoCS_WGS1984 );
			//geoCoordSys.SetFalseOriginAndUnits ( -180.0, -180.0, 5000000.0 );
			//geoCoordSys.SetZFalseOriginAndUnits ( 0.0, 100000.0 );
			//geoCoordSys.SetMFalseOriginAndUnits ( 0.0, 100000.0 );
			//return geoCoordSys as ISpatialReference;
			ESRI.ArcGIS.RuntimeManager.Bind ( ESRI.ArcGIS.ProductCode.Desktop );

			Type type = Type.GetTypeFromProgID ( "esriGeometry.SpatialReferenceEnvironment" );
			System.Object obj = Activator.CreateInstance ( type );
			ISpatialReferenceFactory2 spatRefFact = new SpatialReferenceEnvironment ( ) as ISpatialReferenceFactory2;
			//( ISpatialReferenceFactory2 ) obj;
			IGeographicCoordinateSystem geoCoordSys = spatRefFact.CreateGeographicCoordinateSystem ( ( int ) esriSRGeoCSType.esriSRGeoCS_WGS1984 ) as IGeographicCoordinateSystem;
			ISpatialReference result = geoCoordSys;
			result.SetZDomain ( -2000.0, 14000.0 );
			result.SetMDomain ( -2000.0, 14000.0 );
			result.SetDomain ( -360.0, 360.0, -360.0, 360.0 );
			return result;
		}

		internal void AddGuidanceLine_Apron ( Dictionary<string, Dictionary<string, List<LineData>>> multiPartGuidanceLine )
		{
			if ( _apronTxwFeatClass == null )
			{
				_apronTxwFeatClass = _featureWorkspace.OpenFeatureClass ( "GuidanceLine_Apron" );

				_aprTxwIndices = new int[ 3 ];
				_aprTxwIndices[ 0 ] = _apronTxwFeatClass.FindField ( "ID" );
				_aprTxwIndices[ 1 ] = _apronTxwFeatClass.FindField ( "Elevation" );
			}

			string id;
			double elevation;
			foreach ( KeyValuePair<string, Dictionary<string, List<LineData>>> keyValuePair in multiPartGuidanceLine )
			{
				foreach ( KeyValuePair<string, List<LineData>> partKeyValuePair in keyValuePair.Value )
				{
					//List<LineData> ldList = dict [ key ];

					IPolyline polyline = CreatePolyline ( partKeyValuePair.Value, true );
					elevation = partKeyValuePair.Value.Max ( lnData => lnData.Z );
					IFeature feature = _apronTxwFeatClass.CreateFeature ( );

					try
					{
						feature.Shape = polyline;

						if ( keyValuePair.Key.ToLower ( ).Contains ( "part" ) )
							id = keyValuePair.Key;
						else
							id = keyValuePair.Key + " " + partKeyValuePair.Key;
						//feature.set_Value ( _aprTxwIndices[ 1 ], elevation );
						feature.set_Value ( _aprTxwIndices[ 0 ], id );
						feature.Store ( );
					}
					catch ( Exception ex )
					{
						feature.Delete ( );
						MessageBox.Show ( ex.Message );
					}
				}
			}
		}

		internal void AddGuidanceLine_Taxiway ( List<LineData> lineDataList )
		{
			Dictionary<string, List<LineData>> dict = new Dictionary<string, List<LineData>> ( );
			foreach ( var data in lineDataList )
			{
				int cnt = ( data.Id.Length == 11 ? 3 : 2 );
				string key = data.Id.Substring ( 2, cnt );

				List<LineData> ldList;

				if ( dict.ContainsKey ( key ) )
				{
					ldList = dict[ key ];
				}
				else
				{
					ldList = new List<LineData> ( );
					dict.Add ( key, ldList );
				}

				ldList.Add ( data );
			}

			if ( _apronTxwFeatClass == null )
			{
				_apronTxwFeatClass = _featureWorkspace.OpenFeatureClass ( "GuidanceLine_Taxiway" );

				_aprTxwIndices = new int[ 3 ];
				_aprTxwIndices[ 0 ] = _apronTxwFeatClass.FindField ( "ID" );
			}

			foreach ( string key in dict.Keys )
			{
				List<LineData> ldList = dict[ key ];

				IPolyline polyline = CreatePolyline ( ldList, true );

				IFeature feature = _apronTxwFeatClass.CreateFeature ( );

				try
				{
					feature.Shape = polyline;
					feature.set_Value ( _aprTxwIndices[ 0 ], key );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}

		internal void AddGuidanceLine_Taxiway ( Dictionary<string, Dictionary<string, List<LineData>>> multiPartGuidanceLine )
		{
			if ( _slFeatClass == null )
			{
				_slFeatClass = _featureWorkspace.OpenFeatureClass ( "GuidanceLine_Taxiway" );
			}

			int fieldIndex = _slFeatClass.FindField ( "ID" );
			int elevationIndex = _slFeatClass.FindField ( "Elevation" );
			double elevation;

			Dictionary<string, List<LineData>> dict = new Dictionary<string, List<LineData>> ( );


			foreach ( KeyValuePair<string, Dictionary<string, List<LineData>>> keyValuePair in multiPartGuidanceLine )
			{
				foreach ( KeyValuePair<string, List<LineData>> partKeyValuePair in keyValuePair.Value )
				{
					//List<LineData> ldList = dict [ key ];

					IPolyline polyline = CreatePolyline ( partKeyValuePair.Value, true );
					elevation = partKeyValuePair.Value.Max ( lnData => lnData.Z );
					IFeature feature = _slFeatClass.CreateFeature ( );

					try
					{
						feature.Shape = polyline;
						feature.set_Value ( fieldIndex, keyValuePair.Key + " " + partKeyValuePair.Key );
						//feature.set_Value ( elevationIndex, elevation );
						feature.Store ( );
					}
					catch ( Exception ex )
					{
						feature.Delete ( );
						MessageBox.Show ( ex.Message );
					}
				}
			}
		}

		internal void AddRawPoints ( List<LineData> lineDataList )
		{
			if ( _rawPointFeatClass == null )
			{
				_rawPointFeatClass = _featureWorkspace.OpenFeatureClass ( "RawPoint" );
				_rawPointIdices = new int[ 3 ];
				_rawPointIdices[ 0 ] = _rawPointFeatClass.FindField ( "Id" );
				//_rawPointIdices[ 1 ] = _rawPointFeatClass.FindField ( "Height" );
				//_rawPointIdices[ 2 ] = _rawPointFeatClass.FindField ( "Code" );
			}

			foreach ( LineData ld in lineDataList )
			{
				IFeature feature = null;
				try
				{
					feature = _rawPointFeatClass.CreateFeature ( );

					IPoint point = CreatePoint ( ld, true );

					feature.Shape = point;
					feature.set_Value ( _rawPointIdices[ 0 ], ld.Id );
					//feature.set_Value ( _rawPointIdices[ 1 ], ld.Z );
					//feature.set_Value ( _rawPointIdices[ 2 ], ld.Code );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}

		internal void AddNavaid ( List<LineData> lineDataList )
		{
			if ( _navPointFeatClass == null )
			{
				_navPointFeatClass = _featureWorkspace.OpenFeatureClass ( "Navaid" );
				_navFeatIndices = new int[ 2 ];
				_navFeatIndices[ 0 ] = _navPointFeatClass.FindField ( "Name" );
			}

			foreach ( LineData ld in lineDataList )
			{
				IFeature feature = null;
				try
				{
					feature = _navPointFeatClass.CreateFeature ( );

					IPoint point = CreatePoint ( ld, true );

					feature.Shape = point;
					feature.set_Value ( _navFeatIndices[ 0 ], ld.Id );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}

		}

		internal void AddApronElement ( Dictionary<string, List<LineData>> lineDataList, bool sort )
		{
			if ( _apronDataFeatClass == null )
			{
				_apronDataFeatClass = _featureWorkspace.OpenFeatureClass ( "Apron_Element" );
				_aprDataIndices = new int[ 1 ];
				_aprDataIndices[ 0 ] = _apronDataFeatClass.FindField ( "ID" );
				//_aprDataIndices[ 1 ] = _apronDataFeatClass.FindField ( "Elevation" );
			}

			foreach ( var item in lineDataList )
			{
				IPolygon polygon = CreatePolygon ( item.Value, false, sort );
				IFeature feature = _apronDataFeatClass.CreateFeature ( );

				try
				{
					feature.Shape = polygon;
					feature.set_Value ( _aprDataIndices[ 0 ], item.Key );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}

		internal void AddAircraftStand ( List<LineData> lineDataList )
		{
			if ( _spFeatClass == null )
			{
				_spFeatClass = _featureWorkspace.OpenFeatureClass ( "Aircraft_Stand" );
				_spIndices = new int[ 2 ];
				_spIndices[ 0 ] = _spFeatClass.FindField ( "ID" );
				//_spIndices[ 1 ] = _spFeatClass.FindField ( "Geoid_Undulation" );
				//_spIndices[ 2 ] = _spFeatClass.FindField ( "Numb" );
				//_spIndices[ 3 ] = _spFeatClass.FindField ( "Lat" );
				//_spIndices[ 4 ] = _spFeatClass.FindField ( "Lon" );
				_spIndices[ 1 ] = _spFeatClass.FindField ( "Elev_MSL" );
			}
			string number;
			int index;
			foreach ( LineData ld in lineDataList )
			{
				IFeature feature = null;
				try
				{
					feature = _spFeatClass.CreateFeature ( );

					IPoint point = CreatePoint ( ld, true );

					string id = ld.Id;//.Substring ( 2, ld.Id.Length - 8 );
					feature.Shape = point;
					feature.set_Value ( _spIndices[ 0 ], id );
					feature.set_Value ( _spIndices[ 1 ], ld.Z );
					//index = ld.Id.IndexOf ( "SP" );
					//if ( index == -1 )
					//	index = ld.Id.IndexOf ( "HP" );
					//number = ld.Id.Substring ( 2, index - 2 );					
					//feature.set_Value ( _spIndices[ 2 ], number );
					//feature.set_Value ( _spIndices[ 3 ], ld.Z );
					//feature.set_Value ( _spIndices[ 4 ], id );

					//feature.set_Value ( _spIndices[ 2 ], ld.Z_MSL );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}

		//internal void AddSL ( List<LineData> lineDataList )
		//{
		//    if ( _slFeatClass == null )
		//    {
		//        _slFeatClass = _featureWorkspace.OpenFeatureClass ( "GuidanceLine_Taxiway" );
		//    }

		//    int fieldIndex = _slFeatClass.FindField ( "ID" );

		//    Dictionary<string, List<LineData>> dict = new Dictionary<string, List<LineData>> ( );

		//    foreach ( var ld in lineDataList )
		//    {
		//        if ( ld.Id.Contains ( "Multi" ) || ld.Id.Contains ( "part" ) )
		//            continue;
		//        string key = ld.Id.Substring ( 4, 1 );
		//        List<LineData> ldList;

		//        if ( dict.ContainsKey ( key ) )
		//        {
		//            ldList = dict[ key ];
		//        }
		//        else
		//        {
		//            ldList = new List<LineData> ( );
		//            dict.Add ( key, ldList );
		//        }

		//        ldList.Add ( ld );
		//    }

		//    foreach ( string key in dict.Keys )
		//    {
		//        List<LineData> ldList = dict[ key ];

		//        IFeature feature = null;

		//        try
		//        {
		//            feature = _slFeatClass.CreateFeature ( );

		//            LineData slLineData = null;
		//            string prevName = null;

		//            for ( int i = 0; i < ldList.Count; i++ )
		//            {
		//                if ( ldList[ i ].Id.EndsWith ( "SL" ) )
		//                {
		//                    slLineData = ldList[ i ];
		//                    prevName = ldList[ i - 1 ].Id;
		//                    ldList.RemoveAt ( i );
		//                    break;
		//                }
		//            }

		//            //lineDataList.Sort (
		//            //delegate ( LineData ld1, LineData ld2 )
		//            //{
		//            //    return ld1.Id.CompareTo ( ld2.Id );
		//            //} );

		//            for ( int i = 0; i < ldList.Count; i++ )
		//            {
		//                if ( ldList[ i ].Id == prevName )
		//                {
		//                    ldList.Insert ( i + 1, slLineData );
		//                    break;
		//                }
		//            }

		//            IPolyline polyline = CreatePolyline ( ldList, true, false );

		//            feature.Shape = polyline;
		//            feature.set_Value ( fieldIndex, key );
		//            feature.Store ( );
		//        }
		//        catch ( Exception ex )
		//        {
		//            feature.Delete ( );
		//            MessageBox.Show ( ex.Message );
		//        }
		//    }
		//}

		private void SetLineFields ( )
		{
			if ( _vsLineFeatClass == null )
			{
				_vsLineFeatClass = _featureWorkspace.OpenFeatureClass ( "VS_Line" );

				_vsIndices = new int[ 11 ];
				_vsIndices[ 0 ] = _vsLineFeatClass.FindField ( "ID" );
				_vsIndices[ 1 ] = _vsLineFeatClass.FindField ( "Elevation" );
				_vsIndices[ 2 ] = _vsLineFeatClass.FindField ( "Height" );
				_vsIndices[ 3 ] = _vsLineFeatClass.FindField ( "Point_Count" );
				_vsIndices[ 4 ] = _vsLineFeatClass.FindField ( "Up_Down" );
				_vsIndices[ 5 ] = _vsLineFeatClass.FindField ( "Type" );

				_vsIndices[ 6 ] = _vsLineFeatClass.FindField ( "Frangible" );
				_vsIndices[ 7 ] = _vsLineFeatClass.FindField ( "Lighted" );
				_vsIndices[ 8 ] = _vsLineFeatClass.FindField ( "Color" );
				_vsIndices[ 9 ] = _vsLineFeatClass.FindField ( "Intensivity" );
				_vsIndices[ 10 ] = _vsLineFeatClass.FindField ( "Marked" );

			}
		}

		private void SetPointFields ( )
		{
			if ( _vsPointFeatClass == null )
			{
				_vsPointFeatClass = _featureWorkspace.OpenFeatureClass ( "VS_Point" );

				_vsIndices = new int[ 9 ];
				_vsIndices[ 0 ] = _vsPointFeatClass.FindField ( "ID" );
				_vsIndices[ 1 ] = _vsPointFeatClass.FindField ( "Elevation" );
				_vsIndices[ 2 ] = _vsPointFeatClass.FindField ( "Height" );
				_vsIndices[ 3 ] = _vsPointFeatClass.FindField ( "Type" );
				_vsIndices[ 4 ] = _vsPointFeatClass.FindField ( "Frangible" );
				_vsIndices[ 5 ] = _vsPointFeatClass.FindField ( "Lighted" );
				_vsIndices[ 6 ] = _vsPointFeatClass.FindField ( "Color" );
				_vsIndices[ 7 ] = _vsPointFeatClass.FindField ( "Intensivity" );
				_vsIndices[ 8 ] = _vsPointFeatClass.FindField ( "Marked" );
			}
		}

		private void SetPolygonFields ( )
		{
			if ( _vsPolygonFeatClass == null )
			{
				_vsPolygonFeatClass = _featureWorkspace.OpenFeatureClass ( "VS_Polygon" );

				_vsIndices = new int[ 11 ];
				_vsIndices[ 0 ] = _vsPolygonFeatClass.FindField ( "ID" );
				_vsIndices[ 1 ] = _vsPolygonFeatClass.FindField ( "Elevation" );
				_vsIndices[ 2 ] = _vsPolygonFeatClass.FindField ( "Height" );
				_vsIndices[ 3 ] = _vsPolygonFeatClass.FindField ( "Point_Count" );
				_vsIndices[ 4 ] = _vsPolygonFeatClass.FindField ( "Up_Down" );
				_vsIndices[ 5 ] = _vsPolygonFeatClass.FindField ( "Type" );

				_vsIndices[ 6 ] = _vsPolygonFeatClass.FindField ( "Frangible" );
				_vsIndices[ 7 ] = _vsPolygonFeatClass.FindField ( "Lighted" );
				_vsIndices[ 8 ] = _vsPolygonFeatClass.FindField ( "Color" );
				_vsIndices[ 9 ] = _vsPolygonFeatClass.FindField ( "Intensivity" );
				_vsIndices[ 10 ] = _vsPolygonFeatClass.FindField ( "Marked" );
			}
		}

		private IPolygon CreatePolygon ( List<LineData> lineDataList, bool setZ = false, bool sort = true, bool convexHull = false )
		{
			if ( sort )
			{
				lineDataList.Sort (
					delegate ( LineData ld1, LineData ld2 )
					{
						return ld1.Id.CompareTo ( ld2.Id );
					} );
			}
			IMultipoint multiPnt = new Multipoint ( ) as IMultipoint;
			IPointCollection ptColl = ( IPointCollection ) multiPnt;

			IPoint point;

			foreach ( var ld in lineDataList )
			{
				point = new ESRI.ArcGIS.Geometry.Point ( );
				point.PutCoords ( ld.X, ld.Y );
				if ( setZ )
				{
					point.Z = ld.Z;
					( point as IZAware ).ZAware = true;
				}
				ptColl.AddPoint ( point );
			}
			multiPnt.SpatialReference = _spatRefWGS84;
			IPolygon result;
			if ( convexHull )
			{
				ITopologicalOperator2 topOper2 = ( ITopologicalOperator2 ) multiPnt;
				result = ( IPolygon ) topOper2.ConvexHull ( );
			}
			else
			{
				result = new ESRI.ArcGIS.Geometry.Polygon ( ) as IPolygon;
				IPointCollection resultPntColl = result as IPointCollection;
				resultPntColl.AddPointCollection ( ptColl );
				var pnt = new ESRI.ArcGIS.Geometry.Point ( );
				pnt.PutCoords ( lineDataList[ 0 ].X, lineDataList[ 0 ].Y );
				if ( setZ )
				{
					pnt.Z = lineDataList[ 0 ].Z;
					( pnt as IZAware ).ZAware = true;
				}
				resultPntColl.AddPoint ( pnt );
			}
			return SimplifyPolygon ( result );
		}

		public IPolygon SimplifyPolygon ( IPolygon polygon )
		{
			polygon.SpatialReference = _spatRefWGS84;
			ITopologicalOperator2 topOper2 = ( ITopologicalOperator2 ) polygon;
			topOper2.IsKnownSimple_2 = true;
			topOper2.Simplify ( );
			return polygon;
		}

		private IPolyline CreatePolyline ( List<LineData> lineDataList, bool setZ = false, bool sort = false )
		{
			if ( sort )
			{
				lineDataList.Sort (
					delegate ( LineData ld1, LineData ld2 )
					{
						return ld1.Id.CompareTo ( ld2.Id );
						//string idNumber1 = "", idNumber2 = "";
						//foreach ( var item in ld1.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
						//{
						//    idNumber1 = item + idNumber1;
						//}
						//idNumber1 = idNumber1.Substring ( 2 );

						//foreach ( var item in ld2.Id.Reverse ( ).TakeWhile ( ch => char.IsDigit ( ch ) || ch == '_' ) )
						//{
						//    idNumber2 = item + idNumber2;
						//}
						//idNumber2 = idNumber2.Substring ( 2 );
						//if ( idNumber1.Contains ( "_" ) || idNumber2.Contains ( "_" ) )
						//    return idNumber1.CompareTo ( idNumber2 );
						//double id1 = double.Parse ( idNumber1 );
						//double id2 = double.Parse ( idNumber2 );
						//return id1.CompareTo ( id2 );
					} );
			}

			IPolyline polyline = new Polyline ( ) as IPolyline;

			IPointCollection ptColl = polyline as IPointCollection;

			foreach ( var ld in lineDataList )
			{
				IPoint point = CreatePoint ( ld, setZ );
				ptColl.AddPoint ( point );
			}

			if ( setZ )
			{
				( polyline as IZAware ).ZAware = true;
			}

			//( polyline as ITopologicalOperator2 ).IsKnownSimple_2 = true;
			//( polyline as ITopologicalOperator ).Simplify ( );

			return polyline;
		}

		internal void AddCntrlPoints ( List<LineData> lnDataList, string fileName )
		{
			if ( _cntrlPntFeatClass == null )
			{
				IAoInitialize ao = new AoInitialize ( );
				ao.Initialize ( esriLicenseProductCode.esriLicenseProductCodeBasic );// .esriLicenseProductCodeArcView );

				IWorkspaceFactory wf = new AccessWorkspaceFactory ( );
				IFeatureWorkspace featureWorkspace = wf.OpenFromFile ( fileName, 0 ) as IFeatureWorkspace;

				_cntrlPntFeatClass = featureWorkspace.OpenFeatureClass ( "GeoNetwork" );
				_cntrlPntpIndexes = new int[ 3 ];
				_cntrlPntpIndexes[ 0 ] = _cntrlPntFeatClass.FindField ( "ID" );
				_cntrlPntpIndexes[ 1 ] = _cntrlPntFeatClass.FindField ( "Elev" );
				_cntrlPntpIndexes[ 2 ] = _cntrlPntFeatClass.FindField ( "Elev_MSL" );
			}

			foreach ( LineData ld in lnDataList )
			{
				IFeature feature = null;
				try
				{
					feature = _cntrlPntFeatClass.CreateFeature ( );

					IPoint point = CreatePoint ( ld, true );

					string id = ld.Id;

					feature.Shape = point;
					feature.set_Value ( _cntrlPntpIndexes[ 0 ], id );
					feature.set_Value ( _cntrlPntpIndexes[ 1 ], ld.Z );
					feature.set_Value ( _cntrlPntpIndexes[ 2 ], ld.Z_MSL );

					feature.Store ( );
				}
				catch ( Exception ex )
				{
					feature.Delete ( );
					MessageBox.Show ( ex.Message );
				}
			}
		}

		private static Dictionary<string, List<LineData>> CreateVSDict ( List<LineData> lineDataList )
		{
			Dictionary<string, List<LineData>> dict = new Dictionary<string, List<LineData>> ( );

			foreach ( LineData ld in lineDataList )
			{
				List<LineData> ldList;

				string key = ld.Id.Substring ( 0, 6 );

				if ( dict.ContainsKey ( key ) )
				{
					ldList = dict[ key ];
				}
				else
				{
					ldList = new List<LineData> ( );
					dict.Add ( key, ldList );
				}

				ldList.Add ( ld );
			}
			return dict;
		}

		//private void SetVertStructPropsFromExcel ( VerticalStructure vertStruct )
		//{
		//    if ( _dataTable == null )
		//        LoadVerticalStructData ( );
		//    string lighting;
		//    string color, intensivity;
		//    bool markNotNUllable, frangNotNullable;
		//    foreach ( DataRow row in _dataTable.Rows )
		//    {
		//        if ( row[ 1 ].ToString ( ).Contains ( vertStruct.Name ) )
		//        {
		//            if ( !DBNull.Value.Equals ( row[ 6 ] ) )
		//                vertStruct.Type = _codeVerticalStructures[ row[ 6 ].ToString ( ) ];

		//            vertStruct.Part[ 0 ].Type = vertStruct.Type;
		//            vertStruct.Part[ 0 ].Designator = vertStruct.Name;

		//            if ( !Boolean.TryParse ( row[ 3 ].ToString ( ), out frangNotNullable ) )
		//                vertStruct.Part[ 0 ].Frangible = null;
		//            else
		//                vertStruct.Part[ 0 ].Frangible = frangNotNullable;

		//            if ( !DBNull.Value.Equals ( row[ 4 ] ) )
		//            {
		//                lighting = row[ 4 ].ToString ( );
		//                if ( lighting == "D" || lighting == "" )
		//                    vertStruct.Lighted = false;
		//                else
		//                {
		//                    vertStruct.Lighted = true;
		//                    LightElement lightElement = new LightElement ( );
		//                    color = lighting.Substring ( 2, 1 );
		//                    if ( color == "R" )
		//                        lightElement.Colour = CodeColour.RED;
		//                    else if ( color == "W" )
		//                        lightElement.Colour = CodeColour.WHITE;
		//                    else
		//                        lightElement.Colour = null;

		//                    intensivity = lighting.Substring ( 4, 1 );
		//                    if ( intensivity == "M" )
		//                        lightElement.IntensityLevel = CodeLightIntensity.LIM;
		//                    else if ( intensivity == "L" )
		//                        lightElement.IntensityLevel = CodeLightIntensity.LIL;
		//                    else if ( intensivity == "H" )
		//                        lightElement.IntensityLevel = CodeLightIntensity.LIH;
		//                    else
		//                        lightElement.IntensityLevel = null;

		//                    vertStruct.Part[ 0 ].Lighting.Add ( lightElement );
		//                }
		//            }

		//            if ( !Boolean.TryParse ( row[ 5 ].ToString ( ), out markNotNUllable ) )
		//                vertStruct.MarkingICAOStandard = null;
		//            else
		//                vertStruct.MarkingICAOStandard = markNotNUllable;
		//            return;
		//        }
		//    }
		//}

		private IPoint CreatePoint ( LineData lnData, bool hasZ = false )
		{
			IPoint point = new ESRI.ArcGIS.Geometry.Point ( );
			point.PutCoords ( lnData.X, lnData.Y );
			if ( hasZ )
			{
				point.Z = lnData.Z;

				IZAware zw = point as IZAware;
				zw.ZAware = true;
			}
			return point;
		}

		private void LoadVerticalStructData ( )
		{
			if ( _dataTable == null )
			{
				string path = System.IO.Path.Combine ( @"D:\Work\Kaz", "Att_Obs (version 2).xls" );
				if ( !File.Exists ( path ) )
					throw new Exception ( "Not found \"Att_Obs (version 2).xls\" file" );
				_dataTable = ImportDataTable ( path );
			}
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
				_codeVerticalStructures.Add ( row[ 0 ].ToString ( ), codeVertStructIndices[ codeIndex ] );
				codeIndex++;
			}
		}

		private DataTable _dataTable;
		private IFeatureWorkspace _featureWorkspace;
		private IFeatureClass _rwyFeatClass;
		private IFeatureClass _taxiHoldingPositon;
		private IFeatureClass _vsPointFeatClass;
		private IFeatureClass _vsLineFeatClass;
		private IFeatureClass _vsPolygonFeatClass;
		private IFeatureClass _apronTxwFeatClass;
		private IFeatureClass _apronDataFeatClass;
		private IFeatureClass _spFeatClass;
		private IFeatureClass _rawPointFeatClass;
		private IFeatureClass _slFeatClass;
		private IFeatureClass _cntrlPntFeatClass;
		private IFeatureClass _navPointFeatClass;
		private int[] _rwyIndices;
		private int[] _vsIndices;
		private int[] _aprTxwIndices;
		private int[] _aprDataIndices;
		private int[] _spIndices;
		private int[] _rawPointIdices;
		private int[] _taxiHoldingPosIndices;
		private int[] _cntrlPntpIndexes;
		private int[] _navFeatIndices;
		private Dictionary<string, CodeVerticalStructure> _codeVerticalStructures;
		private string _fileName;
		private const double square_epislon = 0.0000000016;
	}
}