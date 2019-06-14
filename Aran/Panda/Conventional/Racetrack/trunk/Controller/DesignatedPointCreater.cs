using Aran.Geometries;
using System;
using Aran.Panda.Common;

namespace Aran.Panda.Conventional.Racetrack
{
	public class DesignatedPointCreater
	{
		public DesignatedPointCreater ( InboundTrack inboundTrack )
		{
			_inboundTrack = inboundTrack;
		}

		#region Add EventHandlers

		//public void AddIntersectionDirectionChangedEvent ( DirectionEventHandler OnDirChanged )
		//{
		//    _intersectionDirectionChanged += OnDirChanged;
		//}

		public void AddDistanceChangedEvent ( DistanceEventHandler OnDistChanged )
		{
			_distanceChanged += OnDistChanged;
		}

		public void AddSelectedDesignatedPointChangedEvent ( DsgPntEventHandler OnSelectedDsgPntChanged )
		{
			_selectedDsgPntChanged += OnSelectedDsgPntChanged;
		}

		#endregion

		#region Set Values

		public void SetAltitude ( double altitude )
		{
			_altitude = GlobalParams.UnitConverter.HeightToInternalUnits ( altitude );
			NominalDistance = Math.Sqrt ( NominalDistanceInPrj * NominalDistanceInPrj + _altitude * _altitude );
		}

		public void SetVorPnts ( Point vorPntGeo, Point vorPntPrj, bool doSetDsgPnt )
		{
			_vorPntGeo = vorPntGeo;
			_vorPntPrj = vorPntPrj;
			_inboundTrack.SetVorPntPrj ( _vorPntPrj, _vorPntGeo );
			if ( doSetDsgPnt )
				SetDesignatedPoint ( );
		}

		//internal void SetIntersectingVorPnts ( Point IntersectingVorPntGeo, Point IntersectingVorPntPrj)
		//{
		//    _intersectingVorPntGeo = IntersectingVorPntGeo;
		//    _intersectingVorPntPrj = IntersectingVorPntPrj;
		//    //CalculateRadial ( );
		//}

		//private void CalculateRadial ( )
		//{
		//    if ( _intersectionDirectionChanged != null )
		//    {
		//        DirectionEventArg arg = new DirectionEventArg ( double.NaN, ARANFunctions.ReturnAngleInDegrees ( _intersectingVorPntPrj, ARANFunctions.LocalToPrj ( VorPntPrj, _inboundTrack.Direction, NominalDistanceInPrj, 0 ) ) );
		//        _intersectionDirectionChanged ( null, arg );
		//    }
		//}

		public void SetNominalDistance ( double distance, bool isCalculated, bool callEvent4GUI )
		{
			if ( !isCalculated )
				_nominalDistanceInPrj = GlobalParams.UnitConverter.DistanceToInternalUnits ( distance );
			else
				_nominalDistanceInPrj = distance;
			NominalDistance = Math.Sqrt ( _nominalDistanceInPrj * _nominalDistanceInPrj + _altitude * _altitude );
			if ( _distanceChanged != null)
			{
				NomDistanceEventArg argDist = new NomDistanceEventArg ( _nominalDistanceInPrj, GlobalParams.UnitConverter.DistanceToDisplayUnits ( _nominalDistanceInPrj, eRoundMode.NERAEST ), NominalDistance );
				_distanceChanged ( callEvent4GUI, argDist );
			}
			if ( !isCalculated )
				SetDesignatedPoint ( );
		}

		public void SetDirection ( double direction, bool isCalculated )
		{
			_inboundTrack.SetDirection ( direction, isCalculated );
			//if ( isCalculated )
			//    /* _directionInRadInDir */
			//    _inboundTrack.Direction = direction;
			//else
			//    /*_directionInRadInDir*/
			//    _inboundTrack.Direction = ARANMath.Modulus ( ARANFunctions.AztToDirection ( _vorPntGeo, ARANFunctions.DegToRad ( ( double ) direction ), GlobalParams.SpatialRefOperation.GeoSp, GlobalParams.SpatialRefOperation.PrjSp ), 2*ARANMath.C_PI );

			//if ( _directionChanged != null )
			//{
			//    EventArgDirection argDir = new EventArgDirection ( /*_directionInRadInDir*/_inboundTrack.Direction, ARANFunctions.RadToDeg ( ARANFunctions.DirToAzimuth ( VorPntPrj, /*_directionInRadInDir*/_inboundTrack.Direction, GlobalParams.SpatialRefOperation.PrjSp, GlobalParams.SpatialRefOperation.GeoSp ) ) );
			//    _directionChanged ( null, argDir );
			//}
			if ( !isCalculated )
				SetDesignatedPoint ( );
		}

		internal void SetIntersectingVorRadial ( double direction )
		{
			
		}

		public void SetDesignatedPoint ( )
		{
			if ( NominalDistanceInPrj != 0 && _inboundTrack.Direction != 0 )
			{
				if ( _selectedDsgPntChanged != null )
				{
					DsgPntEventArg argDsgPnt;
					if ( VorPntPrj != null )
						argDsgPnt = new DsgPntEventArg ( ARANFunctions.LocalToPrj ( VorPntPrj, _inboundTrack.Direction, NominalDistanceInPrj, 0 ) );
					else
						argDsgPnt = new DsgPntEventArg ( null );
					_selectedDsgPntChanged ( null, argDsgPnt );
				}
			}
		}

		internal void SetDesignatedPoint ( double x, double y )
		{
			Point pntPrj = new Point ( x, y );
			SetDesignatedPoint ( pntPrj );
		}

		internal void SetDesignatedPoint ( Point dsgPntPrj )
		{
			SetDirection ( ARANMath.Modulus ( ARANFunctions.ReturnAngleInRadians ( VorPntPrj, dsgPntPrj ), ARANMath.C_2xPI ), true );
			SetNominalDistance ( ARANFunctions.ReturnDistanceInMeters ( VorPntPrj, dsgPntPrj ), true, true );
			SetDesignatedPoint ( );
		}

		#endregion

		#region Properties

		public double Direction
		{
			get
			{
				return _inboundTrack.Direction;
			}
		}

		public Point VorPntPrj
		{
			get
			{
				return _vorPntPrj;
			}
			private set
			{
				_vorPntPrj = value;
				SetDesignatedPoint ( );
			}
		}

		internal Point VorPntGeo
		{
			get
			{
				return _vorPntGeo;
			}
		}

		public double NominalDistanceInPrj
		{
			get
			{
				return _nominalDistanceInPrj;
			}
		}

		public double NominalDistance
		{
			get
			{
				return _nominalDistance;
			}

			private set
			{
				_nominalDistance = value;
			}
		}

		#endregion

		//private DirectionEventHandler _intersectionDirectionChanged;
		private DistanceEventHandler _distanceChanged;
		private DsgPntEventHandler _selectedDsgPntChanged;

		private double _altitude;
		private double _nominalDistanceInPrj, _nominalDistance;
		//private double _directionInRadInDir;
		private Point _vorPntGeo, _vorPntPrj;//, _intersectingVorPntGeo, _intersectingVorPntPrj;
		private InboundTrack _inboundTrack;
	}
}
