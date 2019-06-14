using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Geometries;
using System.Collections;
using Aran.Aim.Objects;
using System.Text.RegularExpressions;

namespace KFileParser
{
	public class DbWriter
	{
		public DbWriter ( )
		{
			DbProvider = DbProviderFactory.Create ( "Aran.Aim.Data.PgDbProviderComplex" );
			//string connectionString = "Server=172.30.31.18; Port=6401; Database=kaz-ter-3-20141014; User Id=aran; Password=airnav2012;";
			//string connectionString = "Server=172.30.31.18; Port=6400; Database=kaz; User Id=aran; Password=airnav2012;";
            string connectionString = "Server=localhost; Port=5432; Database=kaz; User Id=aran; Password=airnav2012;";
			DbProvider.Open ( connectionString );
			bool connected = DbProvider.Login ( "administrator", Aran.Aim.Data.DbUtility.GetMd5Hash ( "aim_administrator" ) );
			if ( !connected )
				throw new Exception ( "Couldn't connect to database" );

			_rwyIdentifierList = new List<Guid> ( );
		}

		internal void InsertRwyCntrPoints ( List<LineData> rwyCntPntLnDataList, int transaction )
		{
			List<Runway> rwyList = GetRunwayList ( _arpGuid );
			foreach ( Runway rwy in rwyList )
			{
				_rwyIdentifierList.Add ( rwy.Identifier );
				List<RunwayDirection> rwyDirList = GetRunwayDirectionList ( rwy.Identifier );
				RunwayDirection rwyDir1 = rwyDirList[ 0 ];
				RunwayDirection rwyDir2 = rwyDirList[ 1 ];
				string rwyDirDsg;
				Match match = Regex.Match ( rwyCntPntLnDataList[ 0 ].Id, @"\d+" );
				if ( match.Index != 2 )
				{
					rwyDirDsg = match.Value;
					double tmp = double.Parse ( rwyDirDsg.Substring ( 0, 3 ) );
					if ( tmp > 36 )
						rwyDirDsg = rwyDirDsg.Substring ( 0, 2 );
					else
						rwyDirDsg = tmp.ToString ( );
				}
				else
					rwyDirDsg = match.Value;
				if ( rwyDirDsg.Length == 3 && rwyDirDsg[ 1 ] != '0' )
					rwyDirDsg = double.Parse ( rwyDirDsg ).ToString ( );
				else if ( (rwyDirDsg.Length == 3 && rwyDirDsg[ 1 ] == '0') || ( rwyDirDsg.Length == 2 && rwyDirDsg[ 1 ] == '0' ) )
					rwyDirDsg = rwyDirDsg.Substring ( rwyDirDsg.Length - 2 );

				rwyCntPntLnDataList[ 0 ].Id = rwyDirDsg;
				if ( !rwyDir1.Designator.Contains ( rwyDirDsg ) )
				{
					RunwayDirection rwyDirTemp = rwyDir1;
					rwyDir1 = rwyDir2;
					rwyDir2 = rwyDirTemp;
				}

				List<RunwayCentrelinePoint> rwyCentPntListDir1 = InsertRwyCntPntsDir1 ( rwyCntPntLnDataList, rwyDir1.Identifier, transaction );
				InsertRwyCntPntsDir2 ( rwyCentPntListDir1, rwyDir2.Identifier, transaction );
			}
		}

		internal string SetObsAccuracy ( )
		{
			ComparisonOps compOperName = new ComparisonOps ( ComparisonOpType.Like, "Name", Airport.Designator.Substring(2,2) + "0%" );
			OperationChoice operChoiceName = new OperationChoice ( compOperName );

			ComparisonOps compOperType = new ComparisonOps ( ComparisonOpType.NotLike, "Name", "%U" );
			OperationChoice operChoiceType = new OperationChoice ( compOperType );

			BinaryLogicOp logiOper = new BinaryLogicOp ( );
			logiOper.Type = BinaryLogicOpType.And;
			logiOper.OperationList.Add ( operChoiceName );
			logiOper.OperationList.Add ( operChoiceType );

			OperationChoice operChoiceMain = new OperationChoice ( logiOper );
			Filter filter = new Filter ( operChoiceMain );

			int transaction = DbProvider.BeginTransaction ( );
			double verticalAccuracy = 0.061;
			double horizontalAccuracy = 0.068;
			try
			{
				
				List<VerticalStructure> vertStructList = ( List<VerticalStructure> ) GetFeatsViaFilter ( filter, FeatureType.VerticalStructure );
				foreach ( var item in vertStructList )
				{					
					switch ( item.Part[ 0 ].HorizontalProjection.Choice )
					{
						case VerticalStructurePartGeometryChoice.ElevatedPoint:
							//if ( item.Part[ 0 ].HorizontalProjection.Location.VerticalAccuracy != null && item.Part[ 0 ].HorizontalProjection.Location.HorizontalAccuracy != null )
							{

								item.Part[ 0 ].HorizontalProjection.Location.VerticalAccuracy = new Aran.Aim.DataTypes.ValDistance ( verticalAccuracy, UomDistance.M );
								item.Part[ 0 ].HorizontalProjection.Location.HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance ( horizontalAccuracy, UomDistance.M );
							}
							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							//if ( item.Part[ 0 ].HorizontalProjection.LinearExtent.VerticalAccuracy != null && item.Part[ 0 ].HorizontalProjection.LinearExtent.HorizontalAccuracy != null )
							{

								item.Part[ 0 ].HorizontalProjection.LinearExtent.VerticalAccuracy = new Aran.Aim.DataTypes.ValDistance ( verticalAccuracy, UomDistance.M );
								item.Part[ 0 ].HorizontalProjection.LinearExtent.HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance ( horizontalAccuracy, UomDistance.M );
							}
							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							//if ( item.Part[ 0 ].HorizontalProjection.SurfaceExtent.VerticalAccuracy != null && item.Part[ 0 ].HorizontalProjection.SurfaceExtent.HorizontalAccuracy != null )
							{

								item.Part[ 0 ].HorizontalProjection.SurfaceExtent.VerticalAccuracy = new Aran.Aim.DataTypes.ValDistance ( verticalAccuracy, UomDistance.M );
								item.Part[ 0 ].HorizontalProjection.SurfaceExtent.HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance ( horizontalAccuracy, UomDistance.M );
							}
							break;
						default:
							break;
					}
					UpdateFeat ( item, transaction );
				}
				DbProvider.Commit ( transaction );
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
			return "";
		}

		internal string SetObstacleTypes ( List<LineData> lnDataList, Filter filter, bool isIdFull )
		{
			try
			{
				int transactionId = DbProvider.BeginTransaction ( );				
				List<VerticalStructure> vertStructList = ( List<VerticalStructure> ) GetFeatsViaFilter ( filter, FeatureType.VerticalStructure );	
				string name;
				int id = 0;
				LineData lnData;
				VerticalStructure item;
				for ( int i = 0; i < vertStructList.Count; i++ )
				{
					item = vertStructList[ i ];
					if ( isIdFull )
						lnData = lnDataList.Find ( lineData => lineData.Id == item.Name );
					else
					{
						name = item.Name.Substring ( 2 );
						name = new string ( name.TakeWhile( ch => char.IsDigit ( ch ) ).ToArray ( ) );
						id = int.Parse ( name );
						lnData = lnDataList.Find ( lineData => int.Parse ( lineData.Id ) == id );
					}					
					if ( lnData == null)
						continue;
					if ( lnData.Code != "" )
						item.Type = ( CodeVerticalStructure ) Enum.Parse ( typeof ( CodeVerticalStructure ), lnData.Code.ToUpper ( ) );
					item.Part[ 0 ].Frangible = lnData.Frangible;
					if ( lnData.Colour != null )
					{
						LightElement lightElement = new LightElement ( );
						if ( lnData.Colour == "R" )
							lightElement.Colour = CodeColour.RED;
						else if ( lnData.Colour == "W" )
							lightElement.Colour = CodeColour.WHITE;
						item.Part[ 0 ].Lighting.Clear ( );
						item.Part[ 0 ].Lighting.Add ( lightElement );
					}
					item.MarkingICAOStandard = lnData.MarkingICAOStandard;
					UpdateFeat ( item, transactionId );
				}
				DbProvider.Commit ( transactionId );
			}
			catch ( Exception ex)
			{
				return ex.Message;				
			}
			return "";
		}

		internal List<string> InsertSurveyControlPoints ( List<LineData> lnDataList )
		{
			List<AirportHeliport> featList = ( List<AirportHeliport> ) GetFeats ( null, FeatureType.AirportHeliport );
			AirportHeliport arp;
			List<string> notFoundArps = new List<string> ( );
			int transaction = DbProvider.BeginTransaction ( );
			foreach ( var lnData in lnDataList )
			{
				SurveyControlPoint surveyCntrlPnt = ( SurveyControlPoint ) Global.CreateFeature ( FeatureType.SurveyControlPoint );
				surveyCntrlPnt.Designator = lnData.Id;
				arp = featList.Find ( feat => feat.Designator == lnData.Id.Substring ( 0, 4 ) );
				if ( arp != null )
					surveyCntrlPnt.AssociatedAirportHeliport = new Aran.Aim.DataTypes.FeatureRef ( arp.Identifier );
				else
				{
					notFoundArps.Add ( lnData.Id.Substring ( 0, 4 ) );
				}
				surveyCntrlPnt.Location = Global.GetELevatedPoint ( lnData );
				InsertFeat ( surveyCntrlPnt, transaction );

				VerticalStructure vertStruct = ( VerticalStructure ) Global.CreateFeature ( FeatureType.VerticalStructure );
				
				VerticalStructurePart vertStructPart = new VerticalStructurePart ( );
				vertStruct.Name = lnData.Id;
				vertStructPart.HorizontalProjection = new VerticalStructurePartGeometry ( );
				vertStructPart.HorizontalProjection.Location = Global.GetELevatedPoint ( lnData );
				vertStruct.Part.Add ( vertStructPart );
				InsertFeat ( vertStruct, transaction );
			}
			DbProvider.Commit ( transaction );
			return notFoundArps;
		}

		internal void InsertAircraftStands ( List<LineData> allAircraftStands )
		{
			AircraftStand aircraftStand;
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "Designator", "" );
			IList list;
			int transaction = DbProvider.BeginTransaction ( );
			Guid apronGuid = GetApronElementGuid ( );
			foreach ( LineData lnData in allAircraftStands )
			{
				compOper.Value = lnData.Id;
				list = GetFeats ( compOper, FeatureType.AircraftStand );
				if ( list.Count == 0 )
				{
					aircraftStand = ( AircraftStand ) Global.CreateFeature ( FeatureType.AircraftStand );
					aircraftStand.Designator = lnData.Id;
					aircraftStand.Location = Global.GetELevatedPoint ( lnData );
					aircraftStand.ApronLocation = new Aran.Aim.DataTypes.FeatureRef ( apronGuid );
					InsertFeat ( aircraftStand, transaction );
				}
				else
				{
					aircraftStand = ( AircraftStand ) list[ 0 ];
					aircraftStand.Location = Global.GetELevatedPoint ( lnData );
					UpdateFeat ( aircraftStand, transaction );
				}
			}
			DbProvider.Commit ( transaction );
		}

		internal void InsertAirspaces ( List<Airspace> airspaceList )
		{
			int transaction = DbProvider.BeginTransaction ( );
			foreach ( var airspace in airspaceList )
			{
				InsertFeat ( airspace, transaction );
			}
			DbProvider.Commit ( transaction );
		}


		internal void ConnectObstacles2Organisation ( )
		{
			GettingResult getResult = DbProvider.GetVersionsOf ( FeatureType.VerticalStructure, TimeSliceInterpretationType.BASELINE );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			List<VerticalStructure> verticalStructList = getResult.GetListAs<VerticalStructure> ( );
			getResult = DbProvider.GetVersionsOf ( FeatureType.OrganisationAuthority, TimeSliceInterpretationType.BASELINE );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			OrganisationAuthority organistion = ( OrganisationAuthority ) getResult.List[ 0 ];
			foreach ( VerticalStructure vertStruct in verticalStructList )
			{
				vertStruct.HostedOrganisation.Add ( new FeatureRefObject ( organistion.Identifier ) );
			}
			InsertingResult insertResult;
			int transactionId = DbProvider.BeginTransaction ( );
			foreach ( VerticalStructure vertStruct in verticalStructList )
			{
				insertResult = DbProvider.Update ( vertStruct, transactionId );
				if ( !insertResult.IsSucceed )
				{
					DbProvider.Rollback ( transactionId );
					throw new Exception ( insertResult.Message );
				}
			}
			DbProvider.Commit ( transactionId );
		}

		internal void InsertGuidanceLineApron ( Dictionary<string, Dictionary<string, List<LineData>>> multiPartGuidances )
		{
			List<GuidanceLine> guidanceLineList = new List<GuidanceLine> ( );
			foreach ( KeyValuePair<string, Dictionary<string, List<LineData>>> gd in multiPartGuidances )
			{
				GuidanceLine guidanceLine = Global.CreateGuidanceLine ( gd.Key );
				ElevatedCurve elevCurve = Global.Create ( gd.Value );
				if ( elevCurve.Geo.Count > 1 )
				{
					guidanceLine.Extent = elevCurve;
					guidanceLineList.Add ( guidanceLine );
				}
			}
			InsertGuidanceLineApron ( guidanceLineList );
		}

		internal void InsertGuidanceLineApron ( List<GuidanceLine> apronGuidanceLineList )
		{
			List<Apron> apronList = GetApronsOf ( );
			//Guid apronIdentifier = apronList.Where ( apr => apr.Name == "B" ).First ( ).Identifier;
			Guid apronIdentifier = apronList.First ( ).Identifier;
			int transaction = DbProvider.BeginTransaction ( );
			foreach ( GuidanceLine item in apronGuidanceLineList )
			{
				GuidanceLine guidanceLine = GetGuidanceLine ( item.Designator );
				if ( guidanceLine == null )
				{
					item.ConnectedApron.Add ( Global.CreateFeatRefObject ( apronIdentifier ) );
					InsertFeat ( item, transaction );
				}
				else
				{
					guidanceLine.ConnectedApron.Clear ( );
					guidanceLine.ConnectedApron.AddRange ( item.ConnectedApron );
					guidanceLine.Extent = item.Extent;
					UpdateFeat ( guidanceLine, transaction );
				}
			}
			DbProvider.Commit ( transaction );
		}

		internal void InsertGuidanceLineTaxiway ( Dictionary<string, Dictionary<string, List<LineData>>> result )
		{
			List<GuidanceLine> guidanceLineList = new List<GuidanceLine> ( );
			foreach ( KeyValuePair<string, Dictionary<string, List<LineData>>> keyValuePair in result )
			{
				GuidanceLine guidanceLineTaxiway = Global.CreateGuidanceLine ( keyValuePair.Key );
				guidanceLineTaxiway.Extent = Global.Create ( keyValuePair.Value );
				guidanceLineList.Add ( guidanceLineTaxiway );
			}
			InsertGuidanceLineTaxiway ( guidanceLineList );
		}

		internal void InsertGuidanceLineTaxiway ( List<GuidanceLine> guidanceLineList )
		{
			ComparisonOps compOper;
			IList list;
			Taxiway taxiway;
			GuidanceLine guidanceLineTaxiway;
			int transaction = DbProvider.BeginTransaction ( );
			foreach ( var item in guidanceLineList )
			{
				// Before inserting guidanceline checks whether db contains appropriate taxiway
				compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "Designator", item.Designator );
				list = ( List<Taxiway> ) GetFeats ( compOper, FeatureType.Taxiway );
				if ( list.Count == 0 )
				{
					taxiway = Global.CreateTaxiway ( item.Designator, _arpGuid );
					InsertFeat ( taxiway, transaction );
				}
				else
					taxiway = ( Taxiway ) list[ 0 ];

				// 
				guidanceLineTaxiway = GetGuidanceLine ( item.Designator );

				if ( guidanceLineTaxiway == null )
				{
					item.ConnectedTaxiway.Add ( new FeatureRefObject ( taxiway.Identifier ) );
					InsertFeat ( item, transaction );
				}
				else
				{
					guidanceLineTaxiway.Extent = item.Extent;
					guidanceLineTaxiway.ConnectedTaxiway.Clear ( );
					guidanceLineTaxiway.ConnectedTaxiway.Add ( new FeatureRefObject ( taxiway.Identifier ) );
					UpdateFeat ( guidanceLineTaxiway, transaction );
				}
			}
			DbProvider.Commit ( transaction );
		}
		//internal void InsertGuidanceLineTaxiway ( List<LineData> allTwGuidanceLines, string startMultiString, string endMultiString )
		//{
		//    LineData lnData;
		//    int index = 0;
		//    List<TaxiHoldingPosition> taxiHoldingPosList = new List<TaxiHoldingPosition> ( );
		//    while ( index < allTwGuidanceLines.Count )
		//    {
		//        lnData = allTwGuidanceLines[ index ];
		//        GuidanceLine guidanceLine = new GuidanceLine ( );
		//        guidanceLine.Identifier = Guid.NewGuid ( );
		//        if ( lnData.Id.Equals ( startMultiString ) )
		//        {
		//            guidanceLine.Extent = new ElevatedCurve ( );
		//            index++;
		//            lnData = allTwGuidanceLines[ index ];

		//            string designator = lnData.Id.Substring ( 4, 1 );

		//            if ( designator.Equals ( "P" ) )
		//                designator = "MAIN_PAPA";
		//            guidanceLine.Designator = designator;

		//            ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "Designator", designator );
		//            Guid taxiwayIdentifier = ( ( Feature ) GetFeats ( compOper, FeatureType.Taxiway )[ 0 ] ).Identifier;
		//            FeatureRefObject featRefObj = new FeatureRefObject ( );
		//            featRefObj.Feature = new Aran.Aim.DataTypes.FeatureRef ( taxiwayIdentifier );
		//            guidanceLine.ConnectedTaxiway.Add ( featRefObj );

		//            LineString lnString = new LineString ( );
		//            while ( !lnData.Id.Equals ( endMultiString ) )
		//            {
		//                lnString.Add ( new Point ( lnData.X, lnData.Y ) );
		//                index++;
		//                lnData = allTwGuidanceLines[ index ];

		//                if ( lnData.Id.EndsWith ( "SL" ) )
		//                {
		//                    TaxiHoldingPosition taxiHoldingPos = ( TaxiHoldingPosition ) Global.CreateFeature ( FeatureType.TaxiHoldingPosition );
		//                    taxiHoldingPos.Location = Global.GetELevatedPoint ( lnData );
		//                    taxiHoldingPos.AssociatedGuidanceLine = new Aran.Aim.DataTypes.FeatureRef ( guidanceLine.Identifier );
		//                    taxiHoldingPosList.Add ( taxiHoldingPos );
		//                }

		//                if ( lnData.Id.Equals ( "part" ) )
		//                {
		//                    guidanceLine.Extent.Geo.Add ( lnString );
		//                    lnString = new LineString ( );
		//                    index++;
		//                    lnData = allTwGuidanceLines[ index ];
		//                }
		//            }
		//            guidanceLine.Extent.Geo.Add ( lnString );
		//        }
		//        index++;
		//        InsertFeat ( guidanceLine );
		//    }
		//    foreach ( TaxiHoldingPosition feat in taxiHoldingPosList )
		//    {
		//        InsertFeat ( feat );
		//    }
		//}
		//internal void InsertGuidanceLineApron ( List<LineData> apronGuidanceLines, string startMultiString, string endMultiString )
		//{
		//    //DeleteGuidanceLines ( );

		//    LineData lnData;
		//    int index = 0;
		//    List<Apron> apronList = GetApronsOf (  );
		//    Guid apronIdentifier = apronList[ 0 ].Identifier;
		//    string designator, designatorWithZero;
		//    while ( index < apronGuidanceLines.Count )
		//    {
		//        lnData = apronGuidanceLines[ index ];
		//        GuidanceLine guidanceLine = new GuidanceLine ( );
		//        guidanceLine.Identifier = Guid.NewGuid ( );
		//        guidanceLine.ConnectedApron.Add (Global.CreateFeatRefObject ( apronIdentifier ) );

		//        if ( lnData.Id.Equals ( startMultiString ) )
		//        {
		//            guidanceLine.Extent = new ElevatedCurve ( );
		//            index++;
		//            lnData = apronGuidanceLines[ index ];
		//            guidanceLine.Designator = lnData.Id.Substring ( 0, 4 );
		//            designator = guidanceLine.Designator;
		//            if ( !char.IsDigit ( designator[ 3 ] ) )
		//                designatorWithZero = guidanceLine.Designator.Substring ( 0, 2 ) + "0" + guidanceLine.Designator.Substring ( 3, 1 );
		//            else
		//                designatorWithZero = designator;
		//            designator = designator.Substring ( 0, 2 ) + designator.Substring ( 3 ) + "S";

		//            #region Get AircraftStandRefObjects and adds to GuidanceLine.ConnectedStand
		//            ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.Like, "designator", designatorWithZero );
		//            OperationChoice operChoice = new OperationChoice ( compOper );

		//            ComparisonOps compOper2 = new ComparisonOps ( ComparisonOpType.Like, "designator", designator );
		//            OperationChoice operChoice2 = new OperationChoice ( compOper2 );

		//            BinaryLogicOp binLogicOper = new BinaryLogicOp ( );
		//            binLogicOper.Type = BinaryLogicOpType.Or;
		//            binLogicOper.OperationList.Add ( operChoice );
		//            binLogicOper.OperationList.Add ( operChoice2 );
		//            OperationChoice mainOpChoice = new OperationChoice ( binLogicOper );

		//            Filter filter = new Filter ( mainOpChoice );

		//            List<AircraftStand> aircraftStandList = ( List<AircraftStand> ) GetFeatsViaFilter ( filter, FeatureType.AircraftStand );
		//            foreach ( AircraftStand aircraftStand in aircraftStandList )
		//            {
		//                guidanceLine.ConnectedStand.Add ( Global.CreateFeatRefObject ( aircraftStand.Identifier ) );
		//            }
		//            #endregion

		//            LineString lnString = new LineString ( );
		//            while ( !lnData.Id.Equals ( endMultiString ) )
		//            {
		//                lnString.Add ( new Point ( lnData.X, lnData.Y ) );
		//                index++;
		//                lnData = apronGuidanceLines[ index ];
		//                if ( lnData.Id.Equals ( "part" ) )
		//                {
		//                    guidanceLine.Extent.Geo.Add ( lnString );
		//                    lnString = new LineString ( );
		//                    index++;
		//                    lnData = apronGuidanceLines[ index ];
		//                }
		//            }
		//            guidanceLine.Extent.Geo.Add ( lnString );
		//        }
		//        index++;
		//        InsertFeat ( guidanceLine );
		//    }
		//}

		internal InsertingResult InsertVerticalStructures ( List<VerticalStructure> vertStructList, bool isUpdate = false )
		{
			int transaction = DbProvider.BeginTransaction ( );

			InsertingResult insertResult;
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "Name", "" );
			VerticalStructure vertStruct;
			int i = 0;
			foreach ( var item in vertStructList )
			{
				if ( isUpdate )
				{
					compOper.Value = item.Name;
					vertStruct = ( VerticalStructure ) GetFeats ( compOper, FeatureType.VerticalStructure )[ 0 ];
					vertStruct.Part[ 0 ].VerticalExtent = item.Part[ 0 ].VerticalExtent;
					UpdateFeat ( vertStruct, transaction );
				}
				else
				{
					if ( i == 83 )
					{
					}
					insertResult = DbProvider.Insert ( item, transaction );
					if ( !insertResult.IsSucceed )
					{
						DbProvider.Rollback ( transaction );
						return insertResult;
					}
				}
				i++;
			}
			return DbProvider.Commit ( transaction );
		}
		
		internal void InsertTaxiHoldingPosition ( List<LineData> taxiHoldingPosList )
		{
			int transaction = DbProvider.BeginTransaction ( );
			foreach ( var item in taxiHoldingPosList )
			{
				TaxiHoldingPosition txholdPos = ( TaxiHoldingPosition ) Global.CreateFeature ( FeatureType.TaxiHoldingPosition );
				txholdPos.Location = Global.GetELevatedPoint ( item );
				//List<Runway> rwyList = GetRunwayList ( _arpGuid );
				//txholdPos.ProtectedRunway.Add ( new FeatureRefObject ( rwyList[ 0 ].Identifier ) );
				InsertFeat ( txholdPos, transaction );
			}
			DbProvider.Commit ( transaction );
		}

		//internal void InsertApronElements (List<LineData> apronElementPnts )
		//{
		//    List<Apron> apronList = GetApronsOf ( );
		//    if ( apronList.Count > 0 )
		//    {
		//        foreach ( Apron apron in apronList )
		//        {
		//            ApronElement apronElement = ( ApronElement ) CreateFeature ( FeatureType.ApronElement );

		//            apronElement.Extent = new ElevatedSurface ( );
		//            apronElement.Extent.Geo.Add ( CreatePolygon ( apronElementPnts ) );
		//            apronElement.AssociatedApron = new Aran.Aim.DataTypes.FeatureRef ( apron.Identifier );
		//            //UpdateFeat ( apronElement );
		//            InsertFeat ( apronElement );
		//            //_apronElementGuid = apronElement.Identifier;

		//            //List<ApronElement> apronElementList = GetApronElements ( );
		//            //if ( apronElementList.Count > 0 )
		//            //{
		//            //    foreach ( ApronElement apronElement in apronElementList )
		//            //    {
		//            //        if ( apronElement.AssociatedApron == null )
		//            //        {
		//            //            apronElement.AssociatedApron = new Aran.Aim.DataTypes.FeatureRef ( apron.Identifier );
		//            //            InsertFeat ( apronElement );
		//            //        }
		//            //    }
		//            //}
		//        }
		//        return;
		//    }
		//    throw new Exception ( "Apron should be inserted first !" );
		//}

		internal void InsertApronElements ( Dictionary<string, List<LineData>> apronElments )
		{
			IList apronlist = GetApronsOf ( );
			if ( apronlist.Count > 1 )
				throw new NotImplementedException ( "Many apron is not implemented" );

			Apron apron;
			int transaction = DbProvider.BeginTransaction ( );
			if ( apronlist.Count == 0 )
			{
				apron = ( Apron ) Global.CreateFeature ( FeatureType.Apron );
				apron.AssociatedAirportHeliport = new Aran.Aim.DataTypes.FeatureRef ( _arpGuid );
				apron.Name = Airport.Name;
				InsertFeat ( apron, transaction );
			}
			else
				apron = ( Apron ) apronlist[ 0 ];

			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "AssociatedApron", apron.Identifier );
			IList apronElementlist = GetFeats ( compOper, FeatureType.ApronElement );
			if ( apronElementlist.Count < 2 )
			{
				//if ( apronElementlist.Count > 0 )
				//    InsertApronElementsByApron ( apronElments, apron, transaction, apronElementlist[ 0 ] as ApronElement );
				//else
					InsertApronElementsByApron ( apronElments, apron, transaction );
			}
			else
			{
				foreach ( var item in apronElementlist )
				{
					Feature feat = ( Feature ) item;
					DeleteFeat ( feat, transaction );
				}
				InsertApronElementsByApron ( apronElments, apron, transaction );
			}
			DbProvider.Commit ( transaction );
		}

		private void InsertApronElementsByApron (Dictionary<string, List<LineData>> apronElments, Apron apron, int transaction, ApronElement apronElementInDb = null )
		{
			if ( apronElementInDb != null )
			{
				apronElementInDb.Extent = new ElevatedSurface ( );
				apronElementInDb.Extent.Geo.Add ( CreatePolygon ( apronElments.ElementAt ( 0 ).Value ) );
				UpdateFeat ( apronElementInDb, transaction );
			}
			else
			{
				ApronElement apronElementNew;
				foreach ( var item in apronElments )
				{
					apronElementNew = ( ApronElement ) Global.CreateFeature ( FeatureType.ApronElement );
					apronElementNew.AssociatedApron = new Aran.Aim.DataTypes.FeatureRef ( apron.Identifier );
					apronElementNew.Extent = new ElevatedSurface ( );
					apronElementNew.Extent.Geo.Add ( CreatePolygon ( item.Value ) );
					InsertFeat ( apronElementNew, transaction );
				}
			}	
		}

		internal void InsertNavComponents ( Dictionary<NavaidComponent, LineData> dictionary )
		{
			int transaction = DbProvider.BeginTransaction ( );
			foreach ( KeyValuePair<NavaidComponent,LineData> keyValuePair in dictionary )
			{
				NavaidEquipment navEquipment = ( NavaidEquipment ) GetFeature ( ( FeatureType ) keyValuePair.Key.TheNavaidEquipment.Type, keyValuePair.Key.TheNavaidEquipment.Identifier );
				if ( keyValuePair.Value != null )
				{
					navEquipment.Location = Global.GetELevatedPoint ( keyValuePair.Value );
					UpdateFeat ( navEquipment , transaction);
				}			
			}
			DbProvider.Commit ( transaction );
		}

		internal void InsertArp ( LineData lineData, int transaction )
		{			
			Airport.ARP = Global.GetELevatedPoint ( lineData );
			UpdateFeat ( Airport, transaction );
		}

		internal void InsertRunwayElement ( List<LineData> rwyElementLnDataList, int transaction )
		{
			List<Runway> rwyList = GetRunwayList ( _arpGuid );
			_rwyIdentifierList.Add ( rwyList[ 0 ].Identifier );
			
			foreach ( Guid rwyIdentifier in _rwyIdentifierList )
			{
				List<RunwayElement> rwyElementList = GetRunwayElementList ( rwyIdentifier );
				if ( rwyElementList.Count == 0 )
				{
					RunwayElement rwyElement = ( RunwayElement ) Global.CreateFeature ( FeatureType.RunwayElement );

					rwyElement.AssociatedRunway.Add ( new FeatureRefObject ( rwyIdentifier ) );
					rwyElement.Extent = Global.GetElevatedSurface ( rwyElementLnDataList );
					InsertFeat ( rwyElement, transaction );
				}
				else
				{
					foreach ( RunwayElement rwyElement in rwyElementList )
					{
						rwyElement.Extent = Global.GetElevatedSurface ( rwyElementLnDataList );
						UpdateFeat ( rwyElement, transaction );
					}
				}
			}
		}

		private List<RunwayCentrelinePoint> InsertRwyCntPntsDir1 ( List<LineData> rwyCntPntLnDataList, Guid rwyDir1Identifier, int transaction )
		{
			List<RunwayCentrelinePoint> rwyCentPntList = new List<RunwayCentrelinePoint> ( );			
			RunwayCentrelinePoint rwyCntPoint;
			foreach ( LineData lnData in rwyCntPntLnDataList )
			{
				if ( lnData.CodeRunway == CodeRunwayPointRole.THR )
					lnData.Id = Regex.Match ( lnData.Id, @"\d+" ).Value;
				rwyCntPoint = GetRwyCntPoint ( rwyDir1Identifier, lnData.Id );
				if ( rwyCntPoint == null )
				{
					rwyCntPoint = ( RunwayCentrelinePoint ) Global.CreateFeature ( FeatureType.RunwayCentrelinePoint );
					rwyCntPoint.Role = lnData.CodeRunway;
					rwyCntPoint.OnRunway = new Aran.Aim.DataTypes.FeatureRef ( rwyDir1Identifier );
					rwyCntPoint.Designator = lnData.Id;
					rwyCntPoint.Location = Global.GetELevatedPoint ( lnData );
					InsertFeat ( rwyCntPoint, transaction );
				}
				else
				{
					rwyCntPoint.Location = Global.GetELevatedPoint ( lnData );
					UpdateFeat ( rwyCntPoint, transaction );
				}
				rwyCentPntList.Add ( rwyCntPoint );
			}
			return rwyCentPntList;
		}

		private void InsertRwyCntPntsDir2 ( List<RunwayCentrelinePoint> rwyCentPntListDir1, Guid rwyDir2Identifier, int transaction  )
		{
			rwyCentPntListDir1.Reverse ( );

			foreach ( RunwayCentrelinePoint rwyCntPnt in rwyCentPntListDir1 )
			{
				if ( rwyCntPnt.Role == CodeRunwayPointRole.THR )
					continue;

				RunwayCentrelinePoint rwyCntPntInDb = GetRwyCntPoint ( rwyDir2Identifier, rwyCntPnt.Designator );
				if ( rwyCntPntInDb != null )
				{
					rwyCntPntInDb.Location = rwyCntPnt.Location;					
					UpdateFeat ( rwyCntPntInDb, transaction );
				}
				else
				{
					if ( rwyCntPnt.Role == CodeRunwayPointRole.START )
						rwyCntPnt.Role = CodeRunwayPointRole.END;
					else if ( rwyCntPnt.Role == CodeRunwayPointRole.END )
					{
						double tmp = double.Parse ( rwyCentPntListDir1.Find ( thrCntPnt => thrCntPnt.Role == CodeRunwayPointRole.THR ).Designator );
						string thrDsg;
						if ( tmp > 18 )
						{
							tmp = tmp - 18;
							thrDsg = tmp.ToString ( );
							if ( tmp < 10 )
								thrDsg = "0" + tmp.ToString ( );
						}
						else
						{
							tmp = tmp + 18;
							thrDsg = tmp.ToString ( );
						}
						rwyCntPntInDb = GetRwyCntPoint ( rwyDir2Identifier, thrDsg );

						if ( rwyCntPntInDb == null )
							throw new NotImplementedException ( thrDsg + " THR should exist in DB" );
						rwyCntPntInDb.Location = rwyCntPnt.Location;
						rwyCntPntInDb.OnRunway = new Aran.Aim.DataTypes.FeatureRef ( rwyDir2Identifier );
						UpdateFeat ( rwyCntPntInDb, transaction );
						
						rwyCntPnt.Role = CodeRunwayPointRole.START;
					}
					rwyCntPnt.Id = -1;
					rwyCntPnt.Identifier = Guid.NewGuid ( );
					rwyCntPnt.OnRunway = new Aran.Aim.DataTypes.FeatureRef ( rwyDir2Identifier );
					InsertFeat ( rwyCntPnt, transaction );
				}
				//}
			}
		}

		private GuidanceLine GetGuidanceLine ( string p )
		{
			string designator;
			//int index = p.ToLower ( ).IndexOf ( "at" );
			//if ( index == -1 )
			//    index = p.ToLower ( ).IndexOf ( "main" ) + 4;
			//designator = p.Substring ( 0, index );

			designator = p;
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.Like, "designator", designator );
			IList list = GetFeats ( compOper, FeatureType.GuidanceLine );
			if ( list.Count > 0 )
			{
				List<GuidanceLine> guidanceLineList = ( List<GuidanceLine> ) list;
				return guidanceLineList[ 0 ];
			}
			return null;
		}

		private GuidanceLine CreateGuidanceLineTaxiway ( string designator, Guid twyGuid )
		{
			GuidanceLine guidanceLine = ( GuidanceLine ) Global.CreateFeature ( FeatureType.GuidanceLine );
			guidanceLine.Designator = designator;			
			return guidanceLine;
		}
		
		private Guid GetArpGuid (string arpCode )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "Designator", arpCode );
			Airport = ( AirportHeliport ) GetFeats ( compOper, FeatureType.AirportHeliport )[ 0 ];
			return Airport.Identifier;
		}

		private void InsertFeat ( Feature feat, int transaction )
		{
			InsertingResult insertResult = DbProvider.Insert ( feat, transaction );
			if ( !insertResult.IsSucceed )
			{
				DbProvider.Rollback ( transaction );
				throw new Exception ( insertResult.Message );
			}
		}

		private void UpdateFeat ( Feature feat, int transaction = 0)
		{			
			InsertingResult insertResult = DbProvider.Update ( feat, transaction );
			if ( !insertResult.IsSucceed )
			{
				DbProvider.Rollback ( transaction );
				throw new Exception ( insertResult.Message );
			}
		}

		private void DeleteFeat ( Feature feat, int transaction )
		{
			InsertingResult insertResult = DbProvider.Delete ( feat );
			if ( !insertResult.IsSucceed )
			{
				DbProvider.Rollback ( transaction );
				throw new Exception ( insertResult.Message );
			}
		}

		private Polygon CreatePolygon ( List<LineData> apronElementPnts )
		{
			Polygon result = new Polygon ( );
			foreach ( LineData lnData in apronElementPnts )
			{
				Point pnt = new Point ( );
				pnt.X = lnData.X;
				pnt.Y = lnData.Y;
				result.ExteriorRing.Add ( pnt );
			}
			return result;
		}

		internal void SetArpCode ( string arpCode )
		{
			_arpGuid = GetArpGuid ( arpCode );
		}

		#region Reads database

		internal IList GetFeats ( ComparisonOps compOper, FeatureType featType )
		{
			Filter filter = null;
			if ( compOper != null )
			{
				OperationChoice operChoice = new OperationChoice ( compOper );
				filter = new Filter ( operChoice );
			}
			return GetFeatsViaFilter ( filter, featType );
		}

		private IList GetFeatsViaFilter ( Filter filter, FeatureType featType )
		{
			GettingResult getResult = DbProvider.GetVersionsOf ( featType, TimeSliceInterpretationType.BASELINE, default ( Guid ), false, null, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			return getResult.List;
		}

		private Feature GetFeature ( FeatureType featureType, Guid guid )
		{
			GettingResult getResult = DbProvider.GetVersionsOf ( featureType, TimeSliceInterpretationType.BASELINE, guid );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			if ( getResult.List.Count > 0 )
				return ( Feature ) getResult.List[ 0 ];
			else
				return null;
		}

		private List<RunwayElement> GetRunwayElementList ( Guid rwyIdentifier )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "associatedRunway", rwyIdentifier );
			return ( List<RunwayElement> ) GetFeats ( compOper, FeatureType.RunwayElement );
		}

		private List<Runway> GetRunwayList ( Guid arpIdentifier )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "associatedAirportHeliport", arpIdentifier );
			return ( List<Runway> ) GetFeats ( compOper, FeatureType.Runway );
		}

		private RunwayCentrelinePoint GetRwyCntPoint ( Guid rwyDirIdentifier, string designator = "" )
		{
			ComparisonOps compOperDsg = new ComparisonOps ( ComparisonOpType.EqualTo, "Designator", designator );
			OperationChoice operChoiceDsg = new OperationChoice ( compOperDsg );
			BinaryLogicOp binLogicOper = new BinaryLogicOp ( );
			binLogicOper.Type = BinaryLogicOpType.And;
			binLogicOper.OperationList.Add ( operChoiceDsg );

			ComparisonOps compOperOnRwy = new ComparisonOps ( ComparisonOpType.EqualTo, "onRunway", rwyDirIdentifier );
			OperationChoice operChoiceOnRwy = new OperationChoice ( compOperOnRwy );
			binLogicOper.OperationList.Add ( operChoiceOnRwy );

			OperationChoice operChoice = new OperationChoice ( binLogicOper );
			Filter filter = new Filter ( operChoice );
			GettingResult getResult = DbProvider.GetVersionsOf ( FeatureType.RunwayCentrelinePoint, TimeSliceInterpretationType.BASELINE, default ( Guid ), false, null, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			if ( getResult.List.Count > 0 )
				return ( RunwayCentrelinePoint ) getResult.List[ 0 ];
			return null;
		}

		private List<Apron> GetApronsOf ( )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "associatedAirportHeliport", _arpGuid );
			return ( List<Apron> ) GetFeats ( compOper, FeatureType.Apron );
		}

		private List<ApronElement> GetApronElements ( Guid apronIdentifier )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "associatedApron", apronIdentifier );
			return ( List<ApronElement> ) GetFeats ( compOper, FeatureType.ApronElement );
		}

		private Guid GetApronElementGuid ( )
		{
			List<Apron> apronList = GetApronsOf ( );
			if ( apronList.Count == 0 )
				throw new KeyNotFoundException ( "Has no Apron associated with this Airport" );
			List<ApronElement> apronElementList = GetApronElements ( apronList[ 0 ].Identifier );
			if ( apronElementList.Count == 0 )
				throw new KeyNotFoundException ( "Has no Apron Element associated with this Apron" );
			//return apronElementList.Where ( apr => apr.Id == 21 ).First ( ).Identifier;
			return apronElementList[ 0 ].Identifier;
		}

		private List<RunwayDirection> GetRunwayDirectionList ( Guid rwyIdentifier )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "usedRunway", rwyIdentifier );
			return ( List<RunwayDirection> ) GetFeats ( compOper, FeatureType.RunwayDirection );
		}

		private AircraftStand GetAircraftStand ( string designator )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.Like, "designator", designator );
			IList list = GetFeats ( compOper, FeatureType.AircraftStand );
			if ( list.Count > 0 )
			{
				List<AircraftStand> aircraftStandList = ( List<AircraftStand> ) list;
				return aircraftStandList[ 0 ];
			}
			return null;
		}

		#endregion

		private Guid _arpGuid;
		private List<Guid> _rwyIdentifierList;
		internal DbProvider DbProvider;
		public AirportHeliport Airport;
	}
}