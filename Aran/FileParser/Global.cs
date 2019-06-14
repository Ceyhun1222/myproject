using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.Objects;

namespace KFileParser
{
	public static class Global
	{
		internal static ElevatedPoint GetELevatedPoint ( LineData lineData )
		{
			ElevatedPoint result = new ElevatedPoint ( );
			result.Geo.X = lineData.X;
			result.Geo.Y = lineData.Y;
#if (!EXPERT_DATA)
			result.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical ( lineData.Z_MSL, UomDistanceVertical.M );
			result.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned ( lineData.Z - lineData.Z_MSL, UomDistance.M );
#else
            //if ( !double.IsNaN ( lineData.Z ) )
            //    result.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical ( lineData.Z, UomDistanceVertical.M );
            //if ( !double.IsNaN ( lineData.Z_MSL ) )
            //    result.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned ( lineData.Z_MSL, UomDistance.M );
#endif
			return result;
		}

		internal static ElevatedSurface GetElevatedSurface ( List<LineData> lineDataList )
		{
			ElevatedSurface result = new ElevatedSurface ( );
			Polygon polygon = new Polygon ( );
			foreach ( LineData lnData in lineDataList )
			{
				polygon.ExteriorRing.Add ( GetPoint ( lnData ) );
			}
			result.Geo.Add ( polygon );
			return result;
		}

		internal static Point GetPoint ( LineData lnData )
		{
			Point pnt = new Point ( );
			pnt.X = lnData.X;
			pnt.Y = lnData.Y;
			pnt.Z = lnData.Z;
			return pnt;
		}

		internal static VerticalStructure CreateVerticalStructure ( string name )
		{
			VerticalStructure vertStructure = new VerticalStructure ( );
			vertStructure.TimeSlice = new Aran.Aim.DataTypes.TimeSlice ( );
			vertStructure.TimeSlice.FeatureLifetime = new Aran.Aim.DataTypes.TimePeriod ( DateTime.Now );
			vertStructure.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
			vertStructure.TimeSlice.SequenceNumber = 1;
            vertStructure.TimeSlice.ValidTime = vertStructure.TimeSlice.FeatureLifetime;

			vertStructure.Name = name;
			vertStructure.Identifier = Guid.NewGuid ( );
			//VerticalStructurePart vertStructPart = new VerticalStructurePart ( );
			//vertStructPart.HorizontalProjection = new VerticalStructurePartGeometry ( );
			//vertStructure.Part.Add ( vertStructPart );
			return vertStructure;
		}

		internal static ElevatedCurve GetElevatedCurve ( List<LineData> lineDataList )
		{
			ElevatedCurve result = new ElevatedCurve ( );
			LineString lnString = new LineString ( );
			foreach ( var item in lineDataList )
			{
				lnString.Add ( GetPoint ( item ) );
			}
			result.Geo.Add ( lnString );
			return result;
		}

		internal static Taxiway CreateTaxiway ( string designator, Guid arpGuid )
		{
			Taxiway taxiway = ( Taxiway ) Global.CreateFeature ( FeatureType.Taxiway );
			taxiway.Designator = designator;
			taxiway.AssociatedAirportHeliport = new Aran.Aim.DataTypes.FeatureRef ( arpGuid );
			return taxiway;
		}

		internal static GuidanceLine CreateGuidanceLine ( string id )
		{
			GuidanceLine guidanceLine = ( GuidanceLine ) CreateFeature ( FeatureType.GuidanceLine );
			//int index = id.ToLower ( ).IndexOf ( "part" );
			//if ( index == -1 )
			//    index = id.ToLower ( ).IndexOf ( "main" ) + 4;
			//guidanceLine.Designator = id.Substring ( 0, index - 1);

			guidanceLine.Designator = id;
			//string designatorWithZero;
			string designator = guidanceLine.Designator;
			//if ( !char.IsDigit ( designator[ 3 ] ) )
			//    designatorWithZero = guidanceLine.Designator.Substring ( 0, 2 ) + "0" + guidanceLine.Designator.Substring ( 3, 1 );
			//else
			//    designatorWithZero = designator;
			//designator = designator.Substring ( 0, 2 ) + designator.Substring ( 3 ) + "S";

			#region Get AircraftStandRefObjects and adds to GuidanceLine.ConnectedStand
			//ComparisonOps compOper = new ComparisonOps ( ComparisonOpType.Like, "designator", designatorWithZero );
			//OperationChoice operChoice = new OperationChoice ( compOper );

			ComparisonOps compOper2 = new ComparisonOps ( ComparisonOpType.Like, "designator", designator );
			OperationChoice operChoice2 = new OperationChoice ( compOper2 );

			//BinaryLogicOp binLogicOper = new BinaryLogicOp ( );
			//binLogicOper.Type = BinaryLogicOpType.Or;
			//binLogicOper.OperationList.Add ( operChoice );
			//binLogicOper.OperationList.Add ( operChoice2 );
			//OperationChoice mainOpChoice = new OperationChoice ( binLogicOper );
			OperationChoice mainOpChoice = new OperationChoice ( compOper2 );

			//Filter filter = new Filter ( mainOpChoice );

			//List<AircraftStand> aircraftStandList = ( List<AircraftStand> ) GetFeatsViaFilter ( filter, FeatureType.AircraftStand );
			//foreach ( AircraftStand aircraftStand in aircraftStandList )
			//{
			//    guidanceLine.ConnectedStand.Add ( CreateFeatRefObject ( aircraftStand.Identifier ) );
			//}
			#endregion

			return guidanceLine;
		}		

		internal static FeatureRefObject CreateFeatRefObject ( Guid identifier )
		{
			FeatureRefObject aircraftStandRefObj = new FeatureRefObject ( );
			aircraftStandRefObj.Feature = new Aran.Aim.DataTypes.FeatureRef ( identifier );
			return aircraftStandRefObj;
		}

		internal static Feature CreateFeature ( FeatureType featType )
		{
			Feature feat = ( Feature ) AimObjectFactory.Create ( ( int ) featType );
			feat.TimeSlice = new Aran.Aim.DataTypes.TimeSlice ( );
			feat.TimeSlice.FeatureLifetime = new Aran.Aim.DataTypes.TimePeriod ( DateTime.Now );
			feat.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
			feat.TimeSlice.SequenceNumber = 1;
			feat.TimeSlice.ValidTime = feat.TimeSlice.FeatureLifetime;
			feat.Identifier = Guid.NewGuid ( );
			return feat;
		}

		internal static ElevatedCurve Create ( Dictionary<string, List<LineData>> parts )
		{
			ElevatedCurve result = new ElevatedCurve ( );
			foreach ( var part in parts )
			{
				LineString lnString = CreatLineString ( part.Value );
				if ( lnString.Count > 1 )
					result.Geo.Add ( lnString );
			}
			return result;
		}

		internal static LineString CreatLineString ( List<LineData> list )
		{
			LineString lnString = new LineString ( );
			foreach ( LineData lineData in list )
			{
				lnString.Add ( new Point ( lineData.X, lineData.Y, lineData.Z_MSL ) );
			}
			return lnString;
		}
	}
}