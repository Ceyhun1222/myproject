using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Panda.Common;
using Aran.Geometries;
using Aran.Converters;

namespace AIM_Data_Validator
{
	public class ValidatorController
	{
		public ValidatorController ( DatabaseAttributes database = null )
		{
			LoadFeatrues ( );
			if ( database == null )
				Database = new DatabaseAttributes ( );
			else
				Database = database;
			_dbProvider = PgProviderFactory.Create ( );
			ARANFunctions.InitEllipsoid ( );
		}

		public void RunApp ( )
		{
			CalledVia = true;
			_formValidator = new FormValidationList ( );
		}

		internal DatabaseAttributes Database
		{
			get;
			set;
		}

		internal bool CalledVia
		{
			get;
			private set;
		}

		internal string SelectedFeature
		{
			get;
			set;
		}

		internal Dictionary<string, Dictionary<string, bool>> Features
		{
			get;
			private set;
		}

		internal void LoadFeatrues ( )
		{
			Features = new Dictionary<string, Dictionary<string, bool>> ( );

			Dictionary<string, bool> rwyCentLnPnt = new Dictionary<string, bool> ( );
			rwyCentLnPnt.Add ( "role", false );
			Features.Add ( "RunwayCenterLinePoint", rwyCentLnPnt );

			Dictionary<string, bool> rwyDirProps = new Dictionary<string, bool> ( );
			rwyDirProps.Add ( "trueBearing", false );
			rwyDirProps.Add ( "slopeTDZ", false );
			rwyDirProps.Add ( "elevationTDZ", false );
			Features.Add ( "RunwayDirection", rwyDirProps );

			Dictionary<string, bool> runwayProps = new Dictionary<string, bool> ( );
			runwayProps.Add ( "nominalLength", false );
			runwayProps.Add ( "nominalWidth", false );
			Features.Add ( "Runway", runwayProps );
		}

		internal bool SetupDbConnection ( string server, int port, string userName, string password, string dbName )
		{
			bool connectionChanged = false;
			if ( Database.Server != server || Database.Port != port || Database.Username != userName || Database.Password != password || Database.Name != dbName )
			{
				Database.Server = server;
				Database.Port = port;
				Database.Username = userName;
				Database.Password = password;
				Database.Name = dbName;
				connectionChanged = true;
			}
			if ( connectionChanged || _dbProvider.State != System.Data.ConnectionState.Open )
			{
				string connectionString = string.Format ( "Server={0}; Port={1}; Database={2}; User Id={3}; Password={4};", Database.Server, Database.Port, Database.Name, Database.Username, Database.Password );
				string errorString;
				_dbProvider.Open ( connectionString, out errorString );
				if ( errorString != "" )
					return false;
			}
			return true;
		}

		internal void Check ()
		{
			ValidateAirportList ( );
		}

		private void ValidateAirportList ( )
		{
			GettingResult getResult = _dbProvider.GetFeat ( FeatureType.AirportHeliport );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			foreach ( AirportHeliport arp in getResult.List )
			{
				if ( arp.Designator == "UAII" )
				{
					ValidateAirport ( arp );
				}
			}
		}

		private void ValidateAirport ( AirportHeliport airportHeliport )
		{
			Runway runway = GetRunwayOf ( airportHeliport.Identifier );
			RunwayDirection rwyDir1;
			RunwayDirection rwyDir2;
			GetRunwayDirOf ( runway.Identifier, out rwyDir1, out rwyDir2 );
			List<RunwayCentrelinePoint> rwyCntPntList = GetRunwayCntPointList ( rwyDir1.Identifier );
			List<RunwayCentrelinePoint> rwyCntPntList2 = GetRunwayCntPointList ( rwyDir2.Identifier );

			RunwayCentrelinePoint rwyCentPntThr1 = rwyCntPntList.Where ( rwyCntPnt => rwyCntPnt.Role == CodeRunwayPointRole.THR ).First ( );
			if ( rwyCentPntThr1 == null )
				throw new Exception ( "There is no RunwayCentrelinePoint whose role is THR of " + rwyDir1.Designator );

			RunwayCentrelinePoint rwyCentPntThr2 = rwyCntPntList2.Where ( rwyCntPnt => rwyCntPnt.Role == CodeRunwayPointRole.THR ).First ( );
			if ( rwyCentPntThr2 == null )
				throw new Exception ( "There is no RunwayCentrelinePoint whose role is THR of " + rwyDir2.Designator );

			double rwyDir1TrueBearing, rwyDir2TrueBearing;
			double rwyNominalDist = ARANFunctions.CalculateInverseProblem ( rwyCentPntThr1.Location.Geo.X, rwyCentPntThr1.Location.Geo.Y, rwyCentPntThr2.Location.Geo.X, rwyCentPntThr2.Location.Geo.Y, out rwyDir1TrueBearing, out rwyDir2TrueBearing );
			runway.NominalLength = new Aran.Aim.DataTypes.ValDistance ( rwyNominalDist, UomDistance.M );
			List<RunwayElement> rwyElementList = GetRunwayElementsOf ( runway.Identifier );
			try
			{
				Ring extRing = rwyElementList [ 0 ].Extent.Geo [ 0 ].ExteriorRing;
				int count = extRing.Count;
				double widthStart = ARANFunctions.ReturnGeodesicDistance ( extRing [ 0 ], extRing [ count - 1 ] );
				double widthEnd = ARANFunctions.ReturnGeodesicDistance ( extRing [ count / 2 - 1 ], extRing [ count / 2 ] );
				runway.NominalWidth = new Aran.Aim.DataTypes.ValDistance ( ( widthStart + widthEnd ) / 2, UomDistance.M );
				if ( rwyNominalDist > 1200 )
					runway.WidthStrip = new Aran.Aim.DataTypes.ValDistance ( 300, UomDistance.M );
				else
					runway.WidthStrip = new Aran.Aim.DataTypes.ValDistance ( 150, UomDistance.M );
			}
			catch ( Exception )
			{
				throw new Exception ( "There is no RunwayElement associated with Runway(" + runway.Designator + ")" );
			}
			if ( rwyNominalDist > 1200 )
				runway.WidthStrip = new Aran.Aim.DataTypes.ValDistance ( 300, UomDistance.M );
			else
				runway.WidthStrip = new Aran.Aim.DataTypes.ValDistance ( 150, UomDistance.M );

			RunwayDeclaredDistance asdaDeclDist = rwyCentPntThr1.AssociatedDeclaredDistance.Where ( declDist => declDist.Type == CodeDeclaredDistance.ASDA ).First ( );
			RunwayDeclaredDistance toraDeclDist = rwyCentPntThr1.AssociatedDeclaredDistance.Where ( declDist => declDist.Type == CodeDeclaredDistance.TORA ).First ( );
			double l = 0, m = 0;
			if ( asdaDeclDist.DeclaredValue != null && asdaDeclDist.DeclaredValue.Count > 0 )
			{
				l = ConverterToSI.Convert ( asdaDeclDist.DeclaredValue [ 0 ].Distance, double.NaN );
			}
			if ( toraDeclDist.DeclaredValue != null && toraDeclDist.DeclaredValue.Count > 0 )
			{
				m = ConverterToSI.Convert ( toraDeclDist.DeclaredValue [ 0 ].Distance, double.NaN );
			}
			runway.LengthStrip = new Aran.Aim.DataTypes.ValDistance ( rwyNominalDist + l + m, UomDistance.M );

//lengthStrip =  nominalLength + l+m
//l = Runway Directionxx ------ RunwayCentrelinePoint role = THRxx ------- RunwayDeclaredDistance type = ASDA value - Runway Directionxx ------ RunwayCentrelinePoint role = THRxx ------- RunwayDeclaredDistance type = TORA value
//if l<60 then l=60

//m = Runway Direction(xx+18) ------ RunwayCentrelinePoint role = THRxx ------- RunwayDeclaredDistance type = ASDA value - Runway Directionxx ------ RunwayCentrelinePoint role = THRxx ------- RunwayDeclaredDistance type = TORA value
//if m<60 then m=60

//widthStrip = d
			double d;
			if ( rwyNominalDist > 1200 )
				d = 300;
			else
				d = 150;
			runway.WidthStrip = new Aran.Aim.DataTypes.ValDistance ( d, UomDistance.M );

			rwyDir1.TrueBearing = rwyDir1TrueBearing;
			rwyDir1.MagneticBearing = rwyDir1.TrueBearing + airportHeliport.MagneticVariation;

			rwyDir2.TrueBearing = rwyDir2TrueBearing;
			rwyDir2.MagneticBearing = rwyDir2.TrueBearing + airportHeliport.MagneticVariation;

			RunwayCentrelinePoint rwyCntPntTDZ1 = ValidateRunwayDirection ( rwyDir1, rwyCntPntList, rwyCentPntThr1, rwyNominalDist );
			RunwayCentrelinePoint rwyCntPntTDZ2 =  ValidateRunwayDirection ( rwyDir2, rwyCntPntList2, rwyCentPntThr2, rwyNominalDist );
			airportHeliport.FieldElevation = CalculateMaxHeight ( rwyCntPntList );

			_dbProvider.Insert ( runway );
			_dbProvider.Insert ( rwyDir1 );
			_dbProvider.Insert ( rwyDir2 );
			_dbProvider.Insert ( rwyCntPntTDZ1 );
			_dbProvider.Insert ( rwyCntPntTDZ2 );
		}

		private Aran.Aim.DataTypes.ValDistanceVertical CalculateMaxHeight ( List<RunwayCentrelinePoint> rwyCntPntList )
		{
			double maxElev = 0;
			double currElev = 0;
			foreach ( RunwayCentrelinePoint rwyCntPnt in rwyCntPntList )
			{
				currElev = ConverterToSI.Convert ( rwyCntPnt.Location.Elevation, double.NaN );
				if ( currElev > maxElev )
					maxElev = currElev;
			}
			return new Aran.Aim.DataTypes.ValDistanceVertical ( maxElev, UomDistanceVertical.M );
		}

		private List<RunwayElement> GetRunwayElementsOf ( Guid rwyGuid )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "associatedRunway", rwyGuid );
			OperationChoice operChoice = new OperationChoice ( compOper );
			Filter filter = new Filter ( operChoice );
			GettingResult getResult = _dbProvider.GetFeat ( FeatureType.RunwayElement, default ( Guid ), false, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			return ( List<RunwayElement> ) getResult.List;
		}

		private List<RunwayCentrelinePoint> GetRunwayCntPointList ( Guid rwyDirGuid)
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "onRunway", rwyDirGuid );
			OperationChoice operChoice = new OperationChoice ( compOper );
			Filter filter = new Filter ( operChoice );
			GettingResult getResult = _dbProvider.GetFeat ( FeatureType.RunwayCentrelinePoint, default ( Guid ), false, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			return ( List<RunwayCentrelinePoint> ) getResult.List;
		}

		private void GetRunwayDirOf ( Guid rwyGuid, out RunwayDirection rwyDir1, out RunwayDirection rwyDir2 )
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "usedRunway", rwyGuid );
			OperationChoice operChoice = new OperationChoice ( compOper );
			Filter filter = new Filter ( operChoice );
			GettingResult getResult = _dbProvider.GetFeat ( FeatureType.Runway, default ( Guid ), false, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			rwyDir1 = ( RunwayDirection ) getResult.List [ 0 ];
			rwyDir2 = ( RunwayDirection ) getResult.List [ 1 ];
		}

		private Runway GetRunwayOf ( Guid arpGuid)
		{
			ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.EqualTo, "associatedAirportHeliport", arpGuid );
			OperationChoice operChoice = new OperationChoice ( compOper );
			Filter filter = new Filter ( operChoice );
			GettingResult getResult = _dbProvider.GetFeat ( FeatureType.Runway, default ( Guid ), false, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( getResult.Message );
			return ( Runway ) getResult.List [ 0 ];
		}

		/// <summary>
		/// Returns TDZ RunwayCenterLinePoint and validate ElevationTDZ and SlopeTDZ properties of RunwayDirection
		/// </summary>
		private RunwayCentrelinePoint ValidateRunwayDirection ( RunwayDirection rwyDir1, List<RunwayCentrelinePoint> rwyCntPntList, RunwayCentrelinePoint rwyCntPntThr, double rwyNominalDist )
		{
			double tdzLength = rwyNominalDist / 3;
			if ( tdzLength < 900 )
				tdzLength = 900;
			int maxIndex = 0;
			Point pnt;
			Point pntThr = rwyCntPntThr.Location.Geo;
			Point pntStart = rwyCntPntList.Where ( rwyCntPnt => rwyCntPnt.Role == CodeRunwayPointRole.START ).First ( ).Location.Geo;
			double distFromStartToThr = ARANFunctions.ReturnGeodesicDistance ( pntStart, pntThr );
			double distFromStartToTDZ= distFromStartToThr + tdzLength;

			double maxElev = ConverterToSI.Convert ( rwyCntPntThr.Location.Elevation, double.NaN );
			double minElev = maxElev;
			double currElevValue;
			double currDist;
			for ( int i = 0; i <= rwyCntPntList.Count - 1; i++ )
			{
				if ( rwyCntPntList [ i ].Role == CodeRunwayPointRole.START || rwyCntPntList [ i ].Role == CodeRunwayPointRole.END ||
					rwyCntPntList [ i ].Role == CodeRunwayPointRole.THR )
					continue;
				pnt = new Point ( rwyCntPntList [ i ].Location.Geo.X, rwyCntPntList [ i ].Location.Geo.Y );
				currDist = ARANFunctions.ReturnGeodesicDistance ( pnt, pntStart );
				if ( currDist > distFromStartToThr && currDist < distFromStartToTDZ)
				{
					currElevValue = ConverterToSI.Convert ( rwyCntPntList [ i ].Location.Elevation, double.NaN );
					if ( currElevValue > maxElev )
					{
						maxElev = currElevValue;
						maxIndex = i;
					}
					else if ( currElevValue < minElev )
						minElev = currElevValue;
				}
			}
			RunwayCentrelinePoint rwyCntPntTDZ = rwyCntPntList [ maxIndex ];
			rwyCntPntTDZ.Role = CodeRunwayPointRole.TDZ;
			rwyDir1.ElevationTDZ = new Aran.Aim.DataTypes.ValDistanceVertical ( maxElev, UomDistanceVertical.M );
			rwyDir1.SlopeTDZ = 100 * Math.Abs ( ( maxElev - minElev ) / tdzLength );
			return rwyCntPntTDZ;
		}

		private FormValidationList _formValidator;
		private IDbProvider _dbProvider;
	}
}