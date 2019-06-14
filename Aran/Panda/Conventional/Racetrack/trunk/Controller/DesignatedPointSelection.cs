using Aran.Geometries;
using Aran.Aim.Features;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;

namespace Aran.PANDA.Conventional.Racetrack
{
    public class DesignatedPntSelection
    {
        public DesignatedPntSelection (LimitingDistance limDist, ProcedureTypeConv procType)
        {
            _limitingDistance = limDist;
			_radius = 0;
			_dsgPntChanged += OnDsgPntChanged;
			_desgntPntList = new List<DesignatedPoint> ( );
			MinDist4DsgPnt = double.NaN;
			_procType = procType;
			_direction = double.NaN;

			HomingVorDistance = 700000;
			IntersectingVorDistance = HomingVorDistance;
         }

        #region Add EventHandlers

		public void AddDirectionChangedEvent ( DirectionEventHandler onDirChanged )
		{
			_directionChanged += onDirChanged;
		}

		public void AddIntersectDirectionIntervalChangedEvent ( IntervalEventHandler onIntervalChanged )
		{
			_intersectDirIntervalChanged += onIntervalChanged;
		}

		public void AddIntersectionDirectionChanged ( DirectionEventHandler onIntersectionDirectionChanged )
		{
			_intersectionDirectionChanged += onIntersectionDirectionChanged;
		}

        public void AddNominalDistanceChangedEvent ( DistanceEventHandler onDistChanged )
        {
            _nominalDistanceChanged += onDistChanged;
        }

		internal void AddIntersectVorDistanceChangedEvent ( DistanceEventHandler onIntersectVorDistChanged )
		{
			_intersectVorDistanceChanged += onIntersectVorDistChanged;
		}

        public void AddtDsgPntListChangedEvent ( DsgPntListEventHandler onDsgPntListChanged )
        {
            _dsgPntListChanged += onDsgPntListChanged;
        }

        public void AddDsgPntChangedEvent ( DsgPntEventHandler onDsgPntChanged )
        {
            _dsgPntChanged += onDsgPntChanged;
        }

        public void AddMagVarChangedEvent ( CommonEventHandler onMagVarChanged )
        {
			_magVariationChanged += onMagVarChanged;
        }
        #endregion

        #region Methods
       
        internal double[] SetDesignatedPoint ( DesignatedPoint dsgPnt)
        {
			SelectedDesignatedPoint = dsgPnt;
            _nomDistChanged = false;
            _dirChanged = false;
			double[] result = new double[ 2 ];
			if ( _procType == ProcedureTypeConv.Vorvor )
			{
				//SetNominalDistance ( ARANFunctions.ReturnDistanceInMeters ( VorPntPrj, SelectedDsgPntPrj ), true, true );
				SetIntersectVorDistance ( ARANFunctions.ReturnDistanceInMeters ( _selectedIntersectingVorPntPrj, SelectedDsgPntPrj ) );

				return Prior_PostFixToleranceDistance ( );
			}
			return result;
        }

		internal void SetPntDefinition4VorVor ( DsgPntDefinition dsgPntDefinition )
		{
			_dsgPntDefition4VorVor = dsgPntDefinition;
		}

		internal void SetFixToleranceDist ( double value )
		{
			MaxFixToleranceDist = GlobalParams.UnitConverter.DistanceToInternalUnits ( value );
		}

		internal void SetDesignatedPoint ( Point dsgPntPrj = null )
		{
			if ( dsgPntPrj != null )
			{
				SetDirection ( ARANMath.Modulus ( ARANFunctions.ReturnAngleInRadians ( VorPntPrj, dsgPntPrj ), ARANMath.C_2xPI ), true );
				SetNominalDistance ( ARANFunctions.ReturnDistanceInMeters ( VorPntPrj, dsgPntPrj ), true, true );
				if ( _procType == ProcedureTypeConv.Vorvor )
				{
					SelectedDsgPntPrj = dsgPntPrj;
					IntersectionDirection = ARANFunctions.ReturnAngleInRadians ( _selectedIntersectingVorPntPrj, SelectedDsgPntPrj );

					DirectionEventArg argDir = new DirectionEventArg ( IntersectionDirection, ARANFunctions.DirToAzimuth ( _selectedIntersectingVorPntPrj, IntersectionDirection, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar );
					_intersectionDirectionChanged ( null, argDir );

					CalculateIntersectionVorRadialInterval ( );
				}
			}

			if ( NominalDistanceInPrj != 0 && _direction != 0 )
			{
				var argDsgPnt = VorPntPrj != null ? new DsgPntEventArg ( ARANFunctions.LocalToPrj ( VorPntPrj, _direction, NominalDistanceInPrj, 0 ) ) : new DsgPntEventArg ( null );
				_dsgPntChanged ( null, argDsgPnt );
			}
		}

        internal void SetAltitude (double altitude, double elevationNavaid)
        {
            _altitude = altitude;
            _elevationNavaid = elevationNavaid;
			if ( NominalDistanceInPrj > 0 && _altitude > 0)
				NominalDistance = Math.Sqrt ( NominalDistanceInPrj * NominalDistanceInPrj + _altitude * _altitude );
        }

		//internal double[] SetNominalDistance ( double dist )
		internal void SetNominalDistance ( double dist )
        {
			//if ( _procType == ProcedureTypeConv.VORVOR )
			//{
			//    SetNominalDistance ( dist, false, false );
			//    SelectedDsgPntPrj = ARANFunctions.LocalToPrj ( VorPntPrj, _direction, NominalDistanceInPrj, 0 );
			//    IntersectionDirection = ARANFunctions.ReturnAngleInRadians ( _selectedIntersectingVorPntPrj, SelectedDsgPntPrj );

			//    DirectionEventArg argDir = new DirectionEventArg ( IntersectionDirection, ARANFunctions.DirToAzimuth ( _selectedIntersectingVorPntPrj, IntersectionDirection, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar );
			//    _intersectionDirectionChanged ( null, argDir );
	
			//    return Prior_PostFixToleranceDistance ( );
			//} else 
			if ( ChosenPntType == PointChoice.Create )
            {
				SetNominalDistance ( dist, false, false );
                _nomDistChanged = true;
            }
			//return null;
        }

		internal void SetDirection ( double directionInDeg )
        {
			if ( _procType == ProcedureTypeConv.Vordme && ChosenPntType == PointChoice.Create )
			{
				SetDirection ( directionInDeg, false, false );
				_dirChanged = true;
			}
			else
			{
				SetDirection ( directionInDeg, false, false );
				//SelectedDsgPntPrj = ARANFunctions.LocalToPrj ( VorPntPrj, Direction, NominalDistanceInPrj, 0 );
				if ( _procType == ProcedureTypeConv.Vorvor )
					CalculateIntersectionVorRadialInterval ( );
			}
        }

		internal void SetVorPnts ( Point vorPntGeo, Point vorPntPrj )
        {
			_vorPntGeo = vorPntGeo;
			_vorPntPrj = vorPntPrj;
			if (ChosenPntType == PointChoice.Create)			
				SetDesignatedPoint ( );
        }

		internal void SetIntersectingVorPnts ( Point intersectingVorPntPrj )
		{
			_selectedIntersectingVorPntPrj = intersectingVorPntPrj;
			CalculateIntersectionVorRadialInterval ( );
		}

        /// <summary>
        /// Returns false if magnetic variation of selected navaid is null else returs true
        /// </summary>
        /// <param name="navaid"></param>
        /// <param name="magneticValue"></param>
        /// <returns></returns>
        internal bool SetNavaid(Navaid navaid, double? magneticValue)
        {
            bool result = ChangeMagneticValue(magneticValue);
            if (!result)
            {
                return false;
            }
            else if (result)
            {

                if (ChosenPntType == PointChoice.Select && _procType == ProcedureTypeConv.Vordme)
                {
                    if (_selectedNavaid != navaid)
                    {
                        _selectedNavaid = navaid;
                        SetDesignatePointList();
                    }
                    else
                    {
                        SetDirAndDist(_dirChanged, _nomDistChanged);
                    }
                }
                else
                    _selectedNavaid = navaid;
                if (_procType == ProcedureTypeConv.Vorvor && SelectedDsgPntPrj != null)
                {
                    _direction = ARANFunctions.ReturnAngleInRadians(VorPntPrj, SelectedDsgPntPrj);

                    DirectionEventArg argDir = new DirectionEventArg(_direction, ARANFunctions.DirToAzimuth(VorPntPrj, _direction, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo) - MagVar);
                    _directionChanged(null, argDir);
                }
            }
            return true;
        }

        internal void SetNdbGeo(Point ndbPntGeo)
        {
            _ndbPntGeo = ndbPntGeo;
        }

		public bool ChangeMagneticValue ( double? magneticValue )
		{
            if (magneticValue != null)
            {
                MagVar = magneticValue.Value;
                _magVariationChanged(null, new CommonEventArg(magneticValue.Value));
                return true;
            }
            else
            {
                _magVariationChanged(null, null);
                return false;
            }
		}

		internal void SetIntersectVorMagVar ( double? intersectMagVar )
		{
			_intersectMagVar = 0;
			if ( intersectMagVar.HasValue )
				_intersectMagVar = intersectMagVar.Value;
		}

		internal void SetDirection ( double direction, bool isCalculated, bool raiseEvent = true )
		{
            if (isCalculated)
                _direction = direction;
            else
            {
                if(_procType == ProcedureTypeConv.VorNdb && _vorPntGeo == null)
                    _direction = ARANMath.Modulus(ARANFunctions.AztToDirection(_ndbPntGeo, direction + MagVar, GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj), ARANMath.C_2xPI);
                else
                    _direction = ARANMath.Modulus(ARANFunctions.AztToDirection(_vorPntGeo, direction + MagVar, GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj), ARANMath.C_2xPI);
            }
            if ( _procType != ProcedureTypeConv.VorNdb )
			{
				if ( raiseEvent )
				{
					DirectionEventArg argDir = new DirectionEventArg ( _direction, ARANFunctions.DirToAzimuth ( _vorPntPrj, _direction, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - MagVar );
					_directionChanged ( null, argDir );
				}
				if ( _procType == ProcedureTypeConv.Vordme && !isCalculated )
					SetDesignatedPoint ( );
			}
		}

        private void SetNominalDistance ( double distance, bool isCalculated, bool callEvent4Gui )
	    {
		    _nominalDistanceInPrj = isCalculated ? distance : GlobalParams.UnitConverter.DistanceToInternalUnits(distance);
			NominalDistance = Math.Sqrt ( _nominalDistanceInPrj * _nominalDistanceInPrj + _altitude * _altitude );

			DistanceEventArg argDist = new DistanceEventArg ( _nominalDistanceInPrj, GlobalParams.UnitConverter.DistanceToDisplayUnits ( _nominalDistanceInPrj, eRoundMode.NEAREST ), NominalDistance );
			_nominalDistanceChanged ( callEvent4Gui, argDist );

			if ( !isCalculated )
				SetDesignatedPoint ( );
		}

	    private void SetNavaid ( Navaid navaid, bool dirChanged, bool distChanged )
		{
			if ( _selectedNavaid != navaid )
			{
				_selectedNavaid = navaid;
				SetDesignatePointList ( );
			}
			else
			{
				SetDirAndDist ( dirChanged, distChanged );
			}
		}

		private void SetDirAndDist ( bool dirChanged, bool distChanged )
		{
			if ( dirChanged || distChanged )
			{
				if ( dirChanged )
					SetDirection ( ARANMath.Modulus ( ARANFunctions.ReturnAngleInRadians ( VorPntPrj, SelectedDsgPntPrj ), ARANMath.C_2xPI ), true );

				if ( distChanged )
					SetNominalDistance ( ARANFunctions.ReturnDistanceInMeters ( VorPntPrj, SelectedDsgPntPrj ), true, true );

				if ( _procType == ProcedureTypeConv.Vorvor )
				{
					IntersectionDirection = ARANFunctions.ReturnAngleInRadians ( _selectedIntersectingVorPntPrj, SelectedDsgPntPrj );

					DirectionEventArg argDir = new DirectionEventArg ( IntersectionDirection, ARANFunctions.DirToAzimuth ( _selectedIntersectingVorPntPrj, IntersectionDirection, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar );
					_intersectionDirectionChanged ( null, argDir );

					//if ( _dsgPntDefition4VorVor == DsgPntDefinition.ViaVorVorRadial )
					CalculateIntersectionVorRadialInterval ( );
				}

				if ( _selectedDsgPnt != null )
				{
					DsgPntEventArg argDsgPnt = new DsgPntEventArg ( SelectedDsgPntPrj );
					_dsgPntChanged ( null, argDsgPnt );
				}
			}
		}

	    private void SetDesignatePointList ( )
		{
			if ( _selectedNavaid != null && _radius > 0)
			{
			    Point pnt = null;
			    if (_selectedNavaid.Location != null)
			        pnt = _selectedNavaid.Location.Geo;
			    else
			    {
			        if (_vorPntGeo != null)
			            pnt = _vorPntGeo;
                    else if (_dmePntGeo != null)
			            pnt = _dmePntGeo;
			        else
			            throw new Exception($"{_selectedNavaid.Name} has no point");
			    }
				if ( double.IsNaN ( MinDist4DsgPnt ) )
					DsgntPntList = GlobalParams.Database.HoldingQpi.GetDesignatedPointList ( pnt, _radius );
				else
				{
					Polygon polygon = ARANFunctions.CreateTorGeo ( pnt, MinDist4DsgPnt, _radius );
					var tmpList = GlobalParams.Database.HoldingQpi.GetDesignatedPointList ( polygon );
                    DsgntPntList = (tmpList.Count > 0) ? tmpList : null;
				}
			}
			else
				DsgntPntList = null;
		}

		internal void SetDmePntPrj ( Point dmePntPrj )
        {
			_selectedDmePntPrj = dmePntPrj;
        }

		internal double[] SetIntersectingVorRadial ( double intersectDirection )
		{
			IntersectionDirection = ARANMath.Modulus ( ARANFunctions.AztToDirection ( _vorPntGeo, intersectDirection + _intersectMagVar, GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj ), ARANMath.C_2xPI );

			Geometry geom = ARANFunctions.LineLineIntersect ( VorPntPrj, Direction, _selectedIntersectingVorPntPrj, IntersectionDirection );
			SelectedDsgPntPrj = ( Point ) geom;
			SetNominalDistance ( ARANFunctions.ReturnDistanceInMeters ( VorPntPrj, SelectedDsgPntPrj ), true, true );
			SetIntersectVorDistance ( ARANFunctions.ReturnDistanceInMeters ( _selectedIntersectingVorPntPrj, SelectedDsgPntPrj ) );

			return Prior_PostFixToleranceDistance ( );
		}

		private void SetIntersectVorDistance ( double distance )
		{
			DistanceEventArg argDist = new DistanceEventArg ( double.NaN, GlobalParams.UnitConverter.DistanceToDisplayUnits ( distance, eRoundMode.NEAREST ), double.NaN );
			_intersectVorDistanceChanged ( null, argDist );
		}

		internal double CalculateIntersectionVorRadial4Gui ( out double fixToleranceDist4Gui )
		{
			fixToleranceDist4Gui = double.NaN;
			if ( SelectedDsgPntPrj == null )
				return double.NaN;
			IntersectionDirection = ARANFunctions.ReturnAngleInRadians ( _selectedIntersectingVorPntPrj, SelectedDsgPntPrj );
			Geometry geom = ARANFunctions.LineLineIntersect ( VorPntPrj, Direction, _selectedIntersectingVorPntPrj, IntersectionDirection );
			SelectedDsgPntPrj = ( Point ) geom;
			double result = ( ARANFunctions.DirToAzimuth ( _selectedIntersectingVorPntPrj, IntersectionDirection, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar );
			double[] priorPostDists = Prior_PostFixToleranceDistance ( );
			fixToleranceDist4Gui = priorPostDists[0];
			if ( priorPostDists[ 1 ] > fixToleranceDist4Gui )
				fixToleranceDist4Gui = priorPostDists[ 1 ];
			fixToleranceDist4Gui = GlobalParams.UnitConverter.DistanceToDisplayUnits ( fixToleranceDist4Gui, eRoundMode.NEAREST );
			return result;

		}

		private double[] Prior_PostFixToleranceDistance ( )
		{
			Geometry geom = ARANFunctions.LineLineIntersect ( VorPntPrj, Direction, _selectedIntersectingVorPntPrj, IntersectionDirection + GlobalParams.NavaidDatabase.Vor.IntersectingTolerance );
			double[] result = new double[ 2 ];
			Point intersectPnt;
			if ( geom is Point )
			{
				intersectPnt = ( Point ) geom;
				result[ 0 ] = ARANFunctions.ReturnDistanceInMeters ( intersectPnt, SelectedDsgPntPrj );
			}

			geom = ARANFunctions.LineLineIntersect ( VorPntPrj, Direction, _selectedIntersectingVorPntPrj, IntersectionDirection - GlobalParams.NavaidDatabase.Vor.IntersectingTolerance );
			if ( geom is Point )
			{
				intersectPnt = ( Point ) geom;
				result[ 1 ] = ARANFunctions.ReturnDistanceInMeters ( intersectPnt, SelectedDsgPntPrj );
			}
			return result;
		}

	    private void CalculateIntersectionVorRadialInterval ( )
		{
			if ( double.IsNaN ( _direction ) )
				return;

            double radius = (_altitude - _elevationNavaid) * Math.Tan(ARANMath.DegToRad(50));
			Point circlePnt = ARANFunctions.LocalToPrj ( VorPntPrj, _direction, radius, 0 );
			double angleVorVor = ARANMath.Modulus ( ARANFunctions.ReturnAngleInRadians ( _selectedIntersectingVorPntPrj, circlePnt ), ARANMath.C_2xPI );
			SideDirection sideDir = ARANMath.SideDef ( VorPntPrj, Direction, _selectedIntersectingVorPntPrj );
			double minDir, maxDir;
			if ( sideDir == SideDirection.sideRight )
			{
				maxDir = angleVorVor - GlobalParams.NavaidDatabase.Vor.IntersectingTolerance;
				minDir = Direction + GlobalParams.NavaidDatabase.Vor.IntersectingTolerance;
			}
			else
			{
				minDir = angleVorVor + GlobalParams.NavaidDatabase.Vor.IntersectingTolerance;
				maxDir = Direction - GlobalParams.NavaidDatabase.Vor.IntersectingTolerance;
			}
			minDir = ARANMath.Modulus ( minDir, ARANMath.C_2xPI );
			maxDir = ARANMath.Modulus ( maxDir, ARANMath.C_2xPI );
			_distIntersectVor2HomVorRadial = ARANFunctions.Point2LineDistancePrj ( _selectedIntersectingVorPntPrj, _vorPntPrj, _direction );
			//double perpendicularAngle = _direction - ( int ) sideDir * ARANMath.C_PI_2;
			//if ( _distIntersectVor2HomVorRadial * Math.Tan ( GlobalParams.Navaid_Database.VOR.IntersectingTolerance ) < _fixTolerance )
			//{

			//}
			double qamma = 0.5 * ( Math.Acos ( ( 2 * _distIntersectVor2HomVorRadial * Math.Sin ( GlobalParams.NavaidDatabase.Vor.IntersectingTolerance ) / MaxFixToleranceDist ) - Math.Cos ( GlobalParams.NavaidDatabase.Vor.IntersectingTolerance ) ) - GlobalParams.NavaidDatabase.Vor.IntersectingTolerance );
			double minDirResult = minDir;
			double maxDirResult = maxDir;
			bool intersectInterval;
			if ( !double.IsNaN ( qamma ) )
			{
				double qammaInDeg = ARANMath.RadToDeg ( qamma );
				//qammaInDeg = Math.Floor ( qammaInDeg );
				//qamma = ARANMath.DegToRad ( qammaInDeg );
				double minDir3700, maxDir3700;
				if ( sideDir == SideDirection.sideLeft )
				{
					minDir3700 = _direction - ARANMath.C_PI_2 - qamma;
					maxDir3700 = _direction - ARANMath.C_PI_2 + qamma;
				}
				else
				{
					minDir3700 = _direction + ARANMath.C_PI_2 - qamma;
					maxDir3700 = _direction + ARANMath.C_PI_2 + qamma;
				}
				minDir3700 = ARANMath.Modulus ( minDir3700, ARANMath.C_2xPI );
				maxDir3700 = ARANMath.Modulus ( maxDir3700, ARANMath.C_2xPI );

				double tmp1 = ARANFunctions.DirToAzimuth ( VorPntPrj, minDir, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo );
				double tmp2 = ARANFunctions.DirToAzimuth ( VorPntPrj, maxDir, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo );
				double minDir3700InDeg = ARANMath.RadToDeg ( minDir3700 );
				double maxDir3700InDeg = ARANMath.RadToDeg ( maxDir3700 );
				double minDirIndDeg = ARANMath.RadToDeg ( minDir );
				double maxDirInDeg = ARANMath.RadToDeg ( maxDir );
				double direction = ARANMath.RadToDeg ( _direction );

				double tmp3 = ARANFunctions.DirToAzimuth ( VorPntPrj, minDir3700, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo );
				double tmp4 = ARANFunctions.DirToAzimuth ( VorPntPrj, maxDir3700, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo );

				Interval interval1 = new Interval
				{
					Min = minDir,
					Max = maxDir
				};

				Interval interval2 = new Interval
				{
					Min = minDir3700,
					Max = maxDir3700
				};

				Interval resultInterval = ARANFunctions.IntervalIntersectInDir ( interval1, interval2);
				intersectInterval = true;
				if ( double.IsNaN ( resultInterval.Min ) && double.IsNaN ( resultInterval.Max ) )
				{
					minDirResult = minDir;
					maxDirResult = maxDir;
					intersectInterval = false;
				}
				else
				{
					minDirResult = resultInterval.Min;
					maxDirResult = resultInterval.Max;
				}
			}
			else
				intersectInterval = false;
			//if ( SelectedDsgPntPrj != null )
			//{
			//    maxAzimuth = ARANFunctions.DirToAzimuth ( SelectedDsgPntPrj, minDirResult, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar;
			//    minAzimuth = ARANFunctions.DirToAzimuth ( SelectedDsgPntPrj, maxDirResult, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar;
			//}
			//else
			//{
			var maxAzimuth = ARANFunctions.DirToAzimuth ( _vorPntPrj, minDirResult, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar;
			var minAzimuth = ARANFunctions.DirToAzimuth ( _vorPntPrj, maxDirResult, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) - _intersectMagVar;
			//}

			//if ( minAzimuth > maxAzimuth )
			//{
			//    double tmp = maxAzimuth;
			//    maxAzimuth = minAzimuth;
			//    minAzimuth = tmp;
			//}

			maxAzimuth = Math.Floor ( maxAzimuth );
			minAzimuth = Math.Ceiling ( minAzimuth );

			maxAzimuth = ARANMath.Modulus ( maxAzimuth, 360 );
			minAzimuth = ARANMath.Modulus ( minAzimuth, 360 );

			SpeedInterval intersectDirInterval = new SpeedInterval
			{
				Min = minAzimuth,
				Max = maxAzimuth
			};
			if(intersectInterval)
				_intersectDirIntervalChanged ( null, intersectDirInterval );
			else
				_intersectDirIntervalChanged ( false, intersectDirInterval );
		}

        #endregion

        #region Events

        private void OnDesignatedPointListChanged ( object sender, DsgPntListEventArg argDsgPntList )
        {
			_dsgPntListChanged ( sender, argDsgPntList );
        }

		private void OnIntersectingDirectionChanged ( object sender, DirectionEventArg argDir )
		{
			IntersectionDirection = argDir.Direction;

			_intersectionDirectionChanged ( sender, argDir );
		}

        private void OnDsgPntChanged ( object sender, DsgPntEventArg argDsgPnt )
        {
            SelectedDsgPntPrj = argDsgPnt.DsgPntPrj;
        }

        #endregion

		#region Properties 

		public Point SelectedDsgPntPrj
		{
			get;
			private set;
		}

        public double NominalDistanceInPrj 
        {
            get
            {
                return _nominalDistanceInPrj;
            }
            private set
            {
                _nominalDistanceInPrj = value;
            }
        }

        public double NominalDistanceInGeo 
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

		public PointChoice ChosenPntType
		{
			get
			{
				return _pntChoice;
			}
			set
			{
				if ( _pntChoice == value )
					return;
				_pntChoice = value;
				if ( _pntChoice == PointChoice.Select )
				{
					SetNavaid ( _selectedNavaid, _dirChanged, _nomDistChanged );
				}
				else
				{
					SetDesignatedPoint ( );
				}
			}
		}

		public double Direction => _direction;

        public double DirectionInAzimuth => ARANFunctions.DirToAzimuth(_vorPntPrj, _direction, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo);


        public double IntersectionDirection
		{
			get; private set;
		}

	    private Point VorPntPrj
		{
			get
			{
				return _vorPntPrj;
			}
			set
			{
				_vorPntPrj = value;
				SetDesignatedPoint ( );
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
				_limitingDistance.SetNominalDistance ( _nominalDistanceInPrj );
			}
		}

		public DesignatedPoint SelectedDesignatedPoint
		{
			get
			{
				return _selectedDsgPnt;
			}

			private set
			{
				_selectedDsgPnt = value;
				if ( _selectedDsgPnt == null )
				{
					_selectedDsgPntGeo = null;
					SelectedDsgPntPrj = null;
					SetDirAndDist ( false, false );
				}
				else
				{
					_selectedDsgPntGeo = _selectedDsgPnt.Location.Geo;
					SelectedDsgPntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( _selectedDsgPntGeo );
					SetDirAndDist ( true, true );
				}
			}
		}

		internal double Radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
				SetDesignatePointList ( );
			}
		}

		public List<DesignatedPoint> DsgntPntList
		{
			get
			{
				return _desgntPntList;
			}

			private set
			{
                if (value == null || value.Count == 0)
                {
	                _desgntPntList?.Clear();
                }
                else
                {
                    _desgntPntList = value;
                    _desgntPntList.RemoveAll(dsgPnt => dsgPnt.Designator == null);
                    _desgntPntList.Sort((dsgPnt1, dsgPnt2) =>
                        {
                            if (dsgPnt1.Designator == null || dsgPnt2.Designator == null)
                                return -1;
                            var firstCompare = String.CompareOrdinal(dsgPnt1.Designator, dsgPnt2.Designator);
                            return firstCompare != 0 ? firstCompare : dsgPnt1.Id.CompareTo(dsgPnt2.Id);
                        }
                        );
                }

				_dsgPntListChanged ( null, new DsgPntListEventArg ( _desgntPntList ) );
			}
		}

		public double HomingVorDistance
		{
			get;
		}

		public double IntersectingVorDistance
		{
			get; private set;
		}

		public double MinDist4DsgPnt
		{
			private get
			{
				return _minDist4DsgPnt;
			}

			set
			{
				_minDist4DsgPnt = value;
				if ( ChosenPntType == PointChoice.Select && _procType == ProcedureTypeConv.Vordme)
					SetDesignatePointList ( );
			}
		}

		#endregion

		#region Fields

		private readonly LimitingDistance _limitingDistance;
        private bool _nomDistChanged = false, _dirChanged = false;
		private Navaid _selectedNavaid;        
        private PointChoice _pntChoice;
        private double _altitude, _nominalDistanceInPrj, _nominalDistance, _radius;
        private double _elevationNavaid;
        private Point _selectedIntersectingVorPntPrj, _vorPntGeo, _vorPntPrj, _ndbPntGeo, 
			_selectedDsgPntGeo, _selectedDmePntPrj;
		private List<DesignatedPoint> _desgntPntList;
		private DesignatedPoint _selectedDsgPnt;

		public double MaxFixToleranceDist;
		private CommonEventHandler _magVariationChanged;
        private DsgPntListEventHandler _dsgPntListChanged;
        private DirectionEventHandler _directionChanged;
        private DistanceEventHandler _nominalDistanceChanged;
		private DistanceEventHandler _intersectVorDistanceChanged;
		private DsgPntEventHandler _dsgPntChanged;
		private DirectionEventHandler _intersectionDirectionChanged;
		private IntervalEventHandler _intersectDirIntervalChanged;
		private double _direction;
		public double MagVar;
		private readonly ProcedureTypeConv _procType;
		private double _intersectMagVar;
		private DsgPntDefinition _dsgPntDefition4VorVor;
		private double _distIntersectVor2HomVorRadial;
		private double _minDist4DsgPnt;
        private Point _dmePntGeo;

        #endregion

        public void SetDmePnts(Point dmePntGeo)
        {
            _dmePntGeo = dmePntGeo;
        }
    }
}