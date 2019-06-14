using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aran.PANDA.RNAV.Approach
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public delegate void OnInitNewFIXHandler(object sender, WayPoint ReferenceFIX, WayPoint AddedFIX);
	public delegate void OnFIXUpdatedHandler(object sender, WayPoint CurrFIX);

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Transitions
	{
		static int _FN = 0;
		protected static Aran.PANDA.Constants.Constants constants = null;
		protected static SpatialReferenceOperation pSpatialReferenceOperation = null;
		protected AranEnvironment.IAranEnvironment _aranEnvironment;

		public System.EventHandler OnUpdateDirList;
		public System.EventHandler OnUpdateDistList;

		public OnInitNewFIXHandler OnInitNewFIX;
		public OnFIXUpdatedHandler OnFIXUpdated;

		public DoubleValEventHandler OnDistanceChanged;
		public DoubleValEventHandler OnDirectionChanged;
		//public DoubleValEventHandler OnConstructAltitudeChanged;
		public DoubleValEventHandler OnNomlineAltitudeChanged;

		//Segment1Term _term;

		WayPoint _referenceFIX;
		public WayPoint ReferenceFIX { get { return _referenceFIX; } }

		aircraftCategory _aircraftCategory;
		List<WayPoint> _significantCollection;

		FIX _currFIX;
		LegBase _prevLeg;
		LegBase _currLeg;

		#region Simple Props

		ADHPType _ADHP;
		public ADHPType ADHP { get { return _ADHP; } }

		double _refElevation;
		public double RefElevation { get { return _refElevation; } }

		Point _courseInConvertionPoint;
		public Point CourseInConvertionPoint { get { return _courseInConvertionPoint; } }

		Point _courseOutConvertionPoint;
		public Point CourseOutConvertionPoint { get { return _courseOutConvertionPoint; } }

		List<WayPoint> _legPoints;
		public List<WayPoint> LegPoints { get { return _legPoints; } }

		List<LegBase> _legs;
		public List<LegBase> Legs { get { return _legs; } }

		List<WayPoint> _courseSgfPoint;
		public List<WayPoint> CourseSgfPoint { get { return _courseSgfPoint; } }

		List<WayPoint> _distanceSgfPoint;
		public List<WayPoint> DistanceSgfPoint { get { return _distanceSgfPoint; } }

		public string FIXName
		{
			get { return _currFIX.Name; }
			set
			{
				_currFIX.Name = value;
				_currFIX.CallSign = value;
				_currFIX.Id = Guid.Empty;
			}
		}

		public double InDirection { get { return _referenceFIX.EntryDirection; } }
		public double EntryDirection { get { return _referenceFIX.EntryDirection; } }

		//Interval _nomLineGradientRange;
		//Interval NomLineGradientRange { get { return _nomLineGradientRange; } }

		//Interval _constructGradientRange;
		//Interval ConstructGradientRange { get { return _constructGradientRange; } }

		Interval _outDirRange;
		public Interval OutDirRange { get { return _outDirRange; } }

		Interval _distanceRange;
		Interval DistanceRange { get { return _distanceRange; } }

		ObstacleContainer _currentDetObs;
		public ObstacleContainer CurrentDetObs { get { return _currentDetObs; } }

		ObstacleContainer _currentObstacleList;
		public ObstacleContainer CurrentObstacleList { get { return _currentObstacleList; } }

		Interval _IASRange;
		Interval IASRange { get { return _IASRange; } }

		Interval _nominalAltitudeRange;
		Interval NominalAltitudeRange { get { return _nominalAltitudeRange; } }

		//Interval _constructAltitudeRange;
		//Interval ConstructAltitudeRange { get { return _constructAltitudeRange; } }

		//public double AccelerationAltitude { get; set; }

		//double _accelerationAltitude;
		//public double AccelerationAltitude
		//{
		//	set { _accelerationAltitude = value; }
		//	get { return _accelerationAltitude; }
		//}

		#endregion

		double _MAClimb;

		//public double ConstructGradient
		//{
		//	get
		//	{
		//		return _currFIX.ConstructionGradient;
		//	}

		//	//private 
		//	set
		//	{
		//		if ((_referenceFIX.NomLineAltitude < _accelerationAltitude && _referenceFIX.ConstructAltitude < _accelerationAltitude) ||
		//			(_referenceFIX.NomLineAltitude > _accelerationAltitude && _referenceFIX.ConstructAltitude > _accelerationAltitude))
		//		{
		//			if (value < NomLineGradient)
		//				value = NomLineGradient;
		//		}
		//		else if (value < _constructGradientRange.Min)
		//			value = _constructGradientRange.Min;

		//		if (value > _constructGradientRange.Max)
		//			value = _constructGradientRange.Max;

		//		_currFIX.ConstructionGradient = value;
		//		_currLeg.Gradient = value;

		//		double fAltitude = _referenceFIX.ConstructAltitude + (_distanceValue * ConstructGradient);

		//		if (_referenceFIX.ConstructAltitude < _accelerationAltitude && fAltitude > _accelerationAltitude)
		//		{
		//			double dd = (_accelerationAltitude - _referenceFIX.ConstructAltitude) / value;
		//			double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
		//			fAltitude = Math.Min(hh, GlobalVars.MaxAltitude);
		//		}

		//		if (_currFIX.ConstructAltitude != fAltitude)
		//		{
		//			_currFIX.ConstructAltitude = fAltitude;

		//			if (_updateEnabled)
		//			{
		//				UpdateFixParams();
		//				UpdateCurrFIX();

		//				if (OnConstructAltitudeChanged != null)
		//					OnConstructAltitudeChanged(this, _currFIX.ConstructAltitude);
		//			}
		//		}
		//	}
		//}

		//double _NomLineGradient;
		//public double NomLineGradient
		//{
		//	get
		//	{
		//		return _NomLineGradient;
		//	}

		//	set
		//	{
		//		_NomLineGradient = value;

		//		//double fAltitude = _referenceFIX.NomLineAltitude + (_distanceValue * NomLineGradient);

		//		//if (_currFIX.NomLineAltitude != fAltitude)
		//		//{
		//		//	_currFIX.ConstructAltitude = _currFIX.NomLineAltitude = fAltitude;

		//		//	if (_updateEnabled)
		//		//	{
		//		//		UpdateFixParams();
		//		//		UpdateCurrFIX();

		//		//		if (OnNomlineAltitudeChanged != null)
		//		//			OnNomlineAltitudeChanged(this, _currFIX.NomLineAltitude);
		//		//	}
		//		//}
		//	}
		//}

		bool _updateEnabled = false;
		public bool UpdateEnabled
		{
			get { return _updateEnabled; }

			set
			{
				if (_updateEnabled != value)
				{
					_updateEnabled = value;
					if (_updateEnabled)
					{
						_distanceRange.Min -= 0.0125;
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		public bool MultiCoverage
		{
			get { return _currFIX.MultiCoverage; }
			set
			{
				if (_currFIX.MultiCoverage != value)
				{
					_currFIX.MultiCoverage = value;
					if (_updateEnabled && _currFIX.SensorType == eSensorType.DME_DME)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		double _bankAngle;
		public double BankAngle
		{
			get { return _bankAngle; }

			set
			{
				if (_bankAngle != value)
				{
					_bankAngle = value;
					_currFIX.BankAngle = value;

					if (_updateEnabled)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		public ePBNClass PBNClass
		{
			get { return _currFIX.PBNType; }
			set
			{
				if (_currFIX.PBNType != value)
				{
					_currFIX.PBNType = value;

					if (_updateEnabled)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		public eSensorType SensorType
		{
			get { return _currFIX.SensorType; }
			set
			{
				if (_currFIX.SensorType != value)
				{
					_currFIX.SensorType = value;

					if (_updateEnabled)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		public eFlyMode FlyMode
		{
			get { return _currFIX.FlyMode; }
			set
			{
				if (_currFIX.FlyMode != value)
				{
					_currFIX.FlyMode = value;

					if (_updateEnabled)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		double _distanceValue;
		public double Distance
		{
			get
			{
				return _distanceValue;
			}

			set
			{
				if (ChangeDistance(value) && _updateEnabled)
					UpdateCurrFIX();
			}
		}

		int _distanceIndex;
		public int DistanceIndex
		{
			get { return _distanceIndex; }

			set
			{
				_distanceIndex = value;
				if (_distanceIndex >= 0)
				{
					//_currFIX.Assign(_distanceSgfPoint[_distanceIndex]);

					WayPoint sigPoint = _distanceSgfPoint[_distanceIndex];
					_currFIX.CallSign = sigPoint.CallSign;
					_currFIX.Name = sigPoint.Name;
					_currFIX.Id = sigPoint.Id;

					_currFIX.PrjPt = sigPoint.PrjPt;
					_currFIX.HorAccuracy = sigPoint.HorAccuracy;
				}
				else
				{
					string sNumP = _FN.ToString();

					if (_FN < 10)
						sNumP = '0' + sNumP;
					sNumP = "TP" + sNumP;

					_currFIX.Name = sNumP;
					_currFIX.CallSign = sNumP;
					_currFIX.Id = Guid.Empty;
					_currFIX.HorAccuracy = 0.0;
				}
			}
		}

		int _directionIndex;
		public int DirectionIndex
		{
			get { return _directionIndex; }
			set
			{
				_directionIndex = value;

				if (_directionIndex != -1)
				{
					double nomdir, turnAngle;

					WayPoint dirPoint = _courseSgfPoint[_directionIndex];
					CalcDRTurnParams(_referenceFIX, dirPoint, out turnAngle, out nomdir);

					//Point ptCenter = 
					//CalcDRTurnParams(_referenceFIX, _currFIX, out turnAngle, out nomdir);
					//resetDir = true;

					//_aranEnvironment.Graphics.DrawPointWithText(ptCenter, "ptCenter-1");
					//LegBase.ProcessMessages();

				}
			}
		}

		public double OutDirection
		{
			get { return _currFIX.EntryDirection; }

			set
			{
				value = _outDirRange.CheckValue(value);

				if (_currFIX.EntryDirection != value)
					ChangeCourse(value);
			}
		}

		//public double OutDirection_N
		//{
		//	get { return _CurrFIX.EntryDirection_N; }
		//	set
		//	{
		//		value = _OutDirRange.CheckValue(value);
		//		if (_CurrFIX.EntryDirection_N != value)
		//		{
		//			RedrawNominal(value);
		//		}
		//	}
		//}

		int _mocChanged;
		double _MOCLimit, _prevMOC;
		public double LegMOC
		{
			get
			{
				return _prevMOC;
			}
			//set
			//{
			//	if (_MOCLimit != value)
			//	{
			//		_MOCLimit = value;
			//		if (_updateEnabled)
			//			GetOstacles();
			//	}
			//}
		}

		public double IAS
		{
			get
			{
				return _currFIX.IAS;
			}

			set
			{
				if (value < _IASRange.Min)
					value = _IASRange.Min;

				//if (value < _referenceFIX.IAS)
				//	value = _referenceFIX.IAS;

				if (value > _IASRange.Max)
					value = _IASRange.Max;

				if (Math.Abs(value - _currFIX.IAS) < ARANMath.EpsilonDistance)
					return;

				_currFIX.IAS = value;
				_currLeg.IAS = value;

				if (_updateEnabled)
				{
					UpdateFixParams();
					UpdateCurrFIX();
				}
			}
		}

		//public double ConstructAltitude
		//{
		//	get
		//	{
		//		return _currFIX.ConstructAltitude;
		//	}

		//	set
		//	{
		//		if (value > _constructAltitudeRange.Max)
		//			value = _constructAltitudeRange.Max;

		//		if (value < _constructAltitudeRange.Min)
		//			value = _constructAltitudeRange.Min;

		//		ConstructGradient = (value - _constructAltitudeRange.Min) / (_distanceValue);

		//		_currFIX.ConstructAltitude = value;

		//		if (_updateEnabled)
		//		{
		//			UpdateFixParams();
		//			UpdateCurrFIX();

		//			if (OnConstructAltitudeChanged != null)
		//				OnConstructAltitudeChanged(this, _currFIX.ConstructAltitude);
		//		}
		//	}
		//}

		public double NomLineAltitude
		{
			get
			{
				return _currFIX.NomLineAltitude;
			}

			set
			{
				if (value > _nominalAltitudeRange.Max)
					value = _nominalAltitudeRange.Max;

				if (value < _nominalAltitudeRange.Min)
					value = _nominalAltitudeRange.Min;

				//_currFIX.NomLineGradient = (value - _referenceFIX.NomLineAltitude) / (_distanceValue);

				_currFIX.ConstructAltitude = _currFIX.NomLineAltitude = value;

				//_CurrLeg.CreateNomTrack(_PrevLeg);
				//_CurrLeg.RefreshGraphics();
				if (_updateEnabled)
				{
					UpdateFixParams();
					UpdateCurrFIX();

					OnNomlineAltitudeChanged?.Invoke(this, _currFIX.NomLineAltitude);
				}
			}
		}

		Interval _plannedTurnLimits;
		double _plannedTurnAngle;

		public double PlannedTurnAngle
		{
			get { return _plannedTurnAngle; }
			set
			{
				if (value < _plannedTurnLimits.Min) value = _plannedTurnLimits.Min;
				if (value > _plannedTurnLimits.Max) value = _plannedTurnLimits.Max;
				_plannedTurnAngle = value;

				if (_updateEnabled)
				{
					UpdateFixParams();
					UpdateCurrFIX();
				}
			}
		}

		TurnDirection _turnDirection;
		public TurnDirection turnDirection
		{
			get { return _turnDirection; }

			set
			{
				if (_turnDirection != value)
				{
					_turnDirection = value;

					if (_pathAndTermination == CodeSegmentPath.DF)
					{
						_plannedTurnLimits.Max = GlobalVars.f270;
						double min, max;

						if (_turnDirection == TurnDirection.CCW)
						{
							min = ARANMath.Modulus(_referenceFIX.EntryDirection + GlobalVars.f05, ARANMath.C_2xPI);
							max = ARANMath.Modulus(_referenceFIX.EntryDirection + GlobalVars.f270, ARANMath.C_2xPI);
						}
						else
						{
							min = ARANMath.Modulus(_referenceFIX.EntryDirection - GlobalVars.f270, ARANMath.C_2xPI);
							max = ARANMath.Modulus(_referenceFIX.EntryDirection - GlobalVars.f05, ARANMath.C_2xPI);
						}

						_outDirRange.Min = min;
						_outDirRange.Max = max;

						double NewCourse = _outDirRange.CheckValue(_currFIX.EntryDirection);
						//if (_CurrFIX.EntryDirection != NewCourse)
						//{
						//	_CurrFIX.TurnDirection = _turnDirection;
						_referenceFIX.OutDirection = NewCourse;
						_referenceFIX.TurnDirection = _turnDirection;
						ChangeCourse(NewCourse);
						FillCourseSgfPoints();      //??????
													//}
					}
					else
						_plannedTurnLimits.Max = GlobalVars.f120;
				}
			}
		}

		CodeSegmentPath _pathAndTermination;

		public CodeSegmentPath PathAndTermination
		{
			get { return _pathAndTermination; }
			set
			{
				if (_pathAndTermination != value)
				{
					bool OldDFTarget = _currFIX.IsDFTarget;
					_pathAndTermination = value;
					_currLeg.PathAndTermination = value;
					_currFIX.IsDFTarget = value == CodeSegmentPath.DF;

					double minDist = CalcMinDistance();

					double min, max;
					if (_pathAndTermination == CodeSegmentPath.DF)
					{
						/*
                            _plannedTurnLimits.Max = GlobalVars.f270;
                            if (_turnDirection == TurnDirection.CCW)
                            {
                                min = ARANMath.Modulus(_ReferenceFIX.EntryDirection + GlobalVars.f05, ARANMath.C_2xPI);
                                max = ARANMath.Modulus(_ReferenceFIX.EntryDirection + GlobalVars.f270, ARANMath.C_2xPI);
                            }
                            else
                            {
                                min = ARANMath.Modulus(_ReferenceFIX.EntryDirection - GlobalVars.f270, ARANMath.C_2xPI);
                                max = ARANMath.Modulus(_ReferenceFIX.EntryDirection - GlobalVars.f05, ARANMath.C_2xPI);
                            }
                        */
						TurnDirection td = _turnDirection;
						_turnDirection = _turnDirection == TurnDirection.CW ? TurnDirection.CCW : TurnDirection.CW;
						turnDirection = td;
						FillCourseSgfPoints();
					}
					else
					{
						_plannedTurnLimits.Max = GlobalVars.f120;
						min = ARANMath.Modulus(_referenceFIX.EntryDirection - _plannedTurnAngle, ARANMath.C_2xPI);  //GlobalVars.f120
						max = ARANMath.Modulus(_referenceFIX.EntryDirection + _plannedTurnAngle, ARANMath.C_2xPI);  //GlobalVars.f120

						_outDirRange.Min = min;
						_outDirRange.Max = max;

						FillCourseSgfPoints();
						ChangeCourse(_currFIX.EntryDirection);      //OutDirection = _CurrFIX.EntryDirection;	//??????
					}

					Distance = _distanceValue;
					//UpdateFixParams();
					//if (OldDFTarget != _CurrFIX.DFTarget)						UpdateCurrFIX();
				}
			}
		}

		public double TotalDuration
		{
			// TO DO
			get;
			set;
		}

		//=================================================================================================================
		double _prevSumMinLegLenght;
		double _sumMinLegLenght;

		public Transitions(List<WayPoint> SignificantCollection, ADHPType ADHP, LegBase prevLeg , double refElevation, double MAClimb, double PlannedMaxTurnAngle, double MaxDist,
			aircraftCategory category, AranEnvironment.IAranEnvironment aranEnvironment, OnInitNewFIXHandler OnInitPointHandler)
		{
			if (constants == null)
				constants = new Aran.PANDA.Constants.Constants();
			if (pSpatialReferenceOperation == null)
				pSpatialReferenceOperation = new SpatialReferenceOperation(aranEnvironment);

			_aranEnvironment = aranEnvironment;
			_distanceIndex = -1;
			_directionIndex = -1;
			_refElevation = refElevation;

			_MAClimb = MAClimb;
			_FN = 0;

			_ADHP = ADHP;
			_prevLeg = prevLeg;

			OnInitNewFIX = OnInitPointHandler;
			_aircraftCategory = category;
			_plannedTurnLimits.Min = 0;
			_plannedTurnLimits.Max = GlobalVars.f120;       //0.5 * Math.PI;	//constants.Pansops[ePANSOPSData.rnvIFMaxTurnAngl].Value;
			_plannedTurnAngle = PlannedMaxTurnAngle;

			//_accelerationAltitude = 12500.0;
			//_PrevLeg.EndFIX.TurnAltitude = _term.TerminationAltitude;

			_prevLeg.CreateKKLine(null);
			//=============================================================
			double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

			_MOCLimit = fMOC30;

			_mocChanged = -1;

			if (_prevLeg.StartFIX.TurnAngle <= GlobalVars.constants.Pansops[ePANSOPSData.arMATurnTrshAngl].Value)
				_prevMOC = fMOC30;
			else
			{
				_prevMOC = fMOC50;
				_mocChanged = 0;
			}

			//=============================================================

			//_RWY = RWY;

			_significantCollection = SignificantCollection;

			WayPoint prevFix = (WayPoint)_prevLeg.EndFIX.Clone();
			prevFix.EntryDirection = NativeMethods.Modulus(prevFix.EntryDirection, 2 * Math.PI);

			_outDirRange.Circular = true;
			_outDirRange.Min = prevFix.EntryDirection - _plannedTurnAngle + ARANMath.DegToRad(0.49);
			_outDirRange.Max = prevFix.EntryDirection + _plannedTurnAngle - ARANMath.DegToRad(0.49);

			_courseInConvertionPoint = prevFix.GeoPt.Clone() as Point;  // (Point)ReferenceFIX.AngleParameter.InConvertionPoint.Clone();
			_courseOutConvertionPoint = prevFix.PrjPt.Clone() as Point; // (Point)ReferenceFIX.AngleParameter.OutConvertionPoint.Clone();

			//GlobalVars.gAranGraphics.DrawPointWithText(prevFix.PrjPt, "prevFix.PrjPt");
			//LegBase.ProcessMessages(true);


			_IASRange.Min = 1.1 * constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[_aircraftCategory];
			_IASRange.Max = 1.1 * constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[_aircraftCategory];

			//_nomLineGradientRange.Min = 0.02;	//GlobalVars.NomLineGrd;
			//_nomLineGradientRange.Max = 0.07;	// GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			//_constructGradientRange.Min = _nomLineGradientRange.Min;
			//_constructGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			_legPoints = new List<WayPoint>();
			_legs = new List<LegBase>();
			_legs.Add(_prevLeg);

			prevFix.OutDirection = prevFix.EntryDirection;

			_courseSgfPoint = new List<WayPoint>();
			_distanceSgfPoint = new List<WayPoint>();
			//term.leg.StartFIX.Altitude =term.leg.StartFIX.PrjPt.Z+5;
			_referenceFIX = prevFix;// _prevLeg.StartFIX;//	term.leg.StartFIX;

			//double fTmp = ARANFunctions.ReturnDistanceInMeters(_PrevLeg.StartFIX.PrjPt, term.leg.StartFIX.PrjPt);

			_nominalAltitudeRange.Circular = false;
			_nominalAltitudeRange.Min = prevFix.NomLineAltitude;

			//_constructAltitudeRange.Circular = false;
			//_constructAltitudeRange.Min = prevFix.ConstructAltitude;

			//==================================================================
			_distanceRange.Circular = false;
			_distanceRange.Max = MaxDist;

			//double MinStabFrom = prevFix.ATT;
			double MinStabFrom = prevFix.CalcNomLineFromMinStablizationDistance(_plannedTurnAngle);

			//_aranEnvironment.Graphics.DrawPointWithText(_PrevLeg.EndFIX.PrjPt, -1, "ptEndFIX");
			//Leg.ProcessMessages();

			double MinStabIn = _prevLeg.EndFIX.CalcNomLineInToMinStablizationDistance(_plannedTurnAngle);

			//double MinStabIn = prevFix.CalcInToMinStablizationDistance(_plannedTurnAngle);

			double minDist = MinStabIn + MinStabFrom;

			if (prevFix.FlyMode == eFlyMode.Flyby)
			{
				double kTurn = Math.Tan(0.5 * _plannedTurnAngle);
				int i = 100;

				while (i >= 0)
				{
					double hTurn = Math.Min(prevFix.NomLineAltitude + _MAClimb * minDist, GlobalVars.MaxAltitude);
					double RTurn = ARANMath.BankToRadiusForRnav(prevFix.BankAngle, prevFix.IAS, hTurn, constants.Pansops[ePANSOPSData.ISA].Value);  //prevFix.ISA
					double TAS = ARANMath.IASToTASForRnav(prevFix.IAS, hTurn, constants.Pansops[ePANSOPSData.ISA].Value);                           //prevFix.ISA

					MinStabIn = Math.Max(RTurn * kTurn + constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value * TAS, prevFix.ATT);

					double minDistNew = MinStabIn + MinStabFrom;
					double delta = minDistNew - minDist;

					minDist = minDistNew;
					if (Math.Abs(delta) < ARANMath.EpsilonDistance)
						break;
					i--;
				}
			}

			_distanceRange.Min = minDist;
			_distanceValue = _distanceRange.Min;

			_nominalAltitudeRange.Max = Math.Min(GlobalVars.MaxAltitude, _nominalAltitudeRange.Min + _MAClimb * _distanceValue);
			//_constructAltitudeRange.Max = Math.Min(GlobalVars.MaxAltitude, _constructAltitudeRange.Min + _constructGradientRange.Max * _distanceValue);

			InitNewPoint(prevFix);

			NomLineAltitude = _nominalAltitudeRange.Max;
			//ConstructAltitude = _constructAltitudeRange.Max;

			_prevSumMinLegLenght = _sumMinLegLenght = 0.0;
		}

		static public Point CalcDRTurnParams(WayPoint StartFIX, WayPoint EndFIX, out double TurnAngle, out double NomDir)
		{
			double fTurnDirection = -(int)(StartFIX.TurnDirection);
			double r = StartFIX.NomLineTurnRadius;
			Point sigPt;
			if (StartFIX.FlyMode == eFlyMode.Atheight)
				sigPt = StartFIX.PrjPtH;
			else
				sigPt = StartFIX.PrjPt;

			Point ptCenter = ARANFunctions.LocalToPrj(sigPt, StartFIX.EntryDirection, 0, -r * fTurnDirection);

			double dx = EndFIX.PrjPt.X - ptCenter.X;
			double dy = EndFIX.PrjPt.Y - ptCenter.Y;

			double distDest = ARANMath.Hypot(dy, dx);
			if (r > distDest)
			{
				TurnAngle = NomDir = 0;
				return null;
			}

			double dirDest = Math.Atan2(dy, dx);

			TurnAngle = (StartFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);

			NomDir = ARANMath.Modulus(StartFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);
			return ptCenter;
		}

		public void Clean()
		{
			_prevLeg.DeleteGraphics();
			_currLeg.DeleteGraphics();

			for (int i = 0; i < _legs.Count; i++)
				_legs[i].DeleteGraphics();
		}

		bool CheckName(string NewName)
		{
			NewName = NewName.ToUpper();

			foreach (WayPoint fix in _legPoints)
				if (fix.Name.ToUpper() == NewName)
				{
					//double dist = ARANFunctions.ReturnDistanceInMeters(fix.PrjPt, _CurrFIX.PrjPt);
					//if (dist > 10.0)
					return false;
				}

			return true;
		}

		public bool Remove(bool Closed = false)
		{
			if (_legs.Count < 2)
				return false;

			_legs.Remove(_prevLeg);
			if (!Closed)
			{
				_currLeg.DeleteGraphics();  ///////////////////
				_legPoints.RemoveAt(_legPoints.Count - 1);
			}

			//Application.DoEvents();

			_currLeg = _prevLeg;
			_currLeg.Active = false;

			_sumMinLegLenght -= _currLeg.MinLegLength;
			_referenceFIX = _currLeg.StartFIX;

			_currFIX = (FIX)_currLeg.EndFIX;
			_prevLeg = _legs[_legs.Count - 1];

			//FN--;
			string sNumP = _FN.ToString();

			if (_FN < 10)
				sNumP = '0' + sNumP;
			sNumP = "TP" + sNumP;

			_currFIX.Name = sNumP;
			_currFIX.CallSign = sNumP;
			_currFIX.Id = Guid.Empty;

			_prevSumMinLegLenght -= _prevLeg.MinLegLength;

			_courseOutConvertionPoint = (Point)_currFIX.PrjPt.Clone();
			_courseInConvertionPoint = (Point)_currFIX.GeoPt.Clone();

			_nominalAltitudeRange.Min = _currFIX.NomLineAltitude;
			//_constructAltitudeRange.Min = _currFIX.ConstructAltitude;

			//if (_referenceFIX.NomLineAltitude >= _accelerationAltitude)
			//{
			//	_nomLineGradientRange.Min = 0.0;
			//	_nomLineGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//}
			//else
			//{
			//	_nomLineGradientRange.Min = GlobalVars.NomLineGrd;											//????
			//	_nomLineGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			//}

			//if (_currFIX.NomLineGradient > _nomLineGradientRange.Max)
			//	_currFIX.NomLineGradient = _nomLineGradientRange.Max;

			//if (_currFIX.NomLineGradient < _nomLineGradientRange.Min)
			//	_currFIX.NomLineGradient = _nomLineGradientRange.Min;

			_currLeg.Gradient = _currFIX.NomLineGradient;

			//_currFIX.ConstructAltitude

			//if (_referenceFIX.ConstructAltitude >= _accelerationAltitude)
			//{
			//	_constructGradientRange.Min = 0.0;
			//	_constructGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//}
			//else
			//{
			//	_constructGradientRange.Min = _nomLineGradientRange.Min;
			//	_constructGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			//}

			//if (_currFIX.ConstructionGradient > _constructGradientRange.Max)
			//{
			//	_currFIX.ConstructionGradient = _constructGradientRange.Max;
			//	_currLeg.Gradient = _constructGradientRange.Max;
			//}

			//if (_currFIX.ConstructionGradient < _constructGradientRange.Min)
			//{
			//	_currFIX.ConstructionGradient = _constructGradientRange.Min;
			//	_currLeg.Gradient = _constructGradientRange.Min;
			//}

			_bankAngle = _currFIX.BankAngle;
			//_CurrLeg.Gradient = Gradient;
			//_CurrLeg.IAS = IAS;

			if (_currFIX.IsDFTarget)
				_pathAndTermination = CodeSegmentPath.DF;
			else
				_pathAndTermination = CodeSegmentPath.CF;

			if (_currFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _referenceFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _referenceFIX.NomLineTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_referenceFIX.NomLinePrjPt, _referenceFIX.EntryDirection, 0.0, r * fTurnSide);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, _currFIX.EntryDirection, 0.0, -r * fTurnSide);

				_distanceValue = ARANFunctions.ReturnDistanceInMeters(ptFrom, _currFIX.PrjPt);
			}
			else
				_distanceValue = ARANFunctions.ReturnDistanceInMeters(_referenceFIX.NomLinePrjPt, _currFIX.PrjPt);

			if (Closed)
				return true;

			//CalcMaxAltitude(gRADIENT);
			//UpdateCurrFIX();
			//InitNewPoint(_CurrFIX);
			//============================================================================================================================
			double min, max;
			if (_pathAndTermination == CodeSegmentPath.DF)
			{
				_plannedTurnLimits.Max = GlobalVars.f270;
				if (_turnDirection == TurnDirection.CCW)
				{
					min = ARANMath.Modulus(_referenceFIX.EntryDirection + GlobalVars.f05, ARANMath.C_2xPI);
					max = ARANMath.Modulus(_referenceFIX.EntryDirection + GlobalVars.f270, ARANMath.C_2xPI);
				}
				else
				{
					min = ARANMath.Modulus(_referenceFIX.EntryDirection - GlobalVars.f270, ARANMath.C_2xPI);
					max = ARANMath.Modulus(_referenceFIX.EntryDirection - GlobalVars.f05, ARANMath.C_2xPI);
				}
			}
			else
			{
				_plannedTurnLimits.Max = GlobalVars.f120;
				min = ARANMath.Modulus(_referenceFIX.EntryDirection - _plannedTurnAngle, ARANMath.C_2xPI);  //GlobalVars.f120
				max = ARANMath.Modulus(_referenceFIX.EntryDirection + _plannedTurnAngle, ARANMath.C_2xPI);  //GlobalVars.f120
			}

			_outDirRange.Min = min;
			_outDirRange.Max = max;

			double NewCourse = _outDirRange.CheckValue(_currFIX.EntryDirection);
			_referenceFIX.OutDirection = NewCourse;
			_referenceFIX.TurnDirection = _turnDirection;

			_distanceIndex = -1;
			_directionIndex = -1;

			FillCourseSgfPoints();      //??????

			ChangeCourse(NewCourse);        //OutDirection = _CurrFIX.EntryDirection;

			return true;
		}

		public bool Add(bool Close = false)
		{
			Boolean bExisting = false;

			if (_distanceIndex >= 0)
				bExisting = true;

			//_CurrLeg.Gradient = Gradient;
			//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, 0, eFillStyle.sfsCross);
			//LegBase.ProcessMessages();

			if (!bExisting && !CheckName(_currFIX.Name))
			{
				MessageBox.Show("Invalid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return false;
			}

			_prevSumMinLegLenght = _sumMinLegLenght;
			_sumMinLegLenght += _currLeg.MinLegLength;

			_courseOutConvertionPoint = (Point)_currFIX.PrjPt.Clone();
			_courseInConvertionPoint = (Point)_currFIX.GeoPt.Clone();

			//_nominalAltitudeRange.Min = _currFIX.NomLineAltitude;
			////CalcMaxAltitude(NomLineGradient);
			//_nominalAltitudeRange.Max = Math.Min(_referenceFIX.NomLineAltitude + NomLineGradient * _distanceValue, GlobalVars.MaxAltitude);
			//if (_referenceFIX.NomLineAltitude < _accelerationAltitude && _nominalAltitudeRange.Max > _accelerationAltitude)
			//{
			//	double dd = (_accelerationAltitude - _referenceFIX.NomLineAltitude) / NomLineGradient;
			//	double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//	_nominalAltitudeRange.Max = Math.Min(hh, GlobalVars.MaxAltitude);
			//}

			//_constructAltitudeRange.Min = _currFIX.ConstructAltitude;
			//_constructAltitudeRange.Max = Math.Min(_referenceFIX.ConstructAltitude + ConstructGradient * _distanceValue, GlobalVars.MaxAltitude);
			//if (_referenceFIX.ConstructAltitude < _accelerationAltitude && _constructAltitudeRange.Max > _accelerationAltitude)
			//{
			//	double dd = (_accelerationAltitude - _referenceFIX.ConstructAltitude) / ConstructGradient;
			//	double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//	_constructAltitudeRange.Max = Math.Min(hh, GlobalVars.MaxAltitude);
			//}

			_currLeg.Gradient = _MAClimb;
			_currLeg.IAS = IAS;
			_currLeg.PathAndTermination = PathAndTermination;
			_currLeg.Obstacles = _currentObstacleList;
			_currLeg.Active = true;

			_legs.Add(_currLeg);

			_prevLeg = _currLeg;
			if (!Close)
				InitNewPoint(_currFIX);
			else
			{
				//_currLeg.CreateGeometry(_prevLeg, _ADHP);
				//_currLeg.RefreshGraphics();
				//LegBase.ProcessMessages();
			}

			//LegBase.ProcessMessages();

			return true;
		}

		void GetOstacles()
		{
			double fMOC30 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			double fMOC50 = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;
			double fMOC;

			if (_currLeg.StartFIX.TurnAngle <= GlobalVars.constants.Pansops[ePANSOPSData.arMATurnTrshAngl].Value)
				fMOC = Math.Max(_prevMOC, fMOC30);
			else
				fMOC = fMOC50;

			if (_prevMOC < fMOC)
			{
				_prevMOC = fMOC;
				_mocChanged = _legs.Count;
			}
			else if(_mocChanged == _legs.Count)
			{
				_prevMOC = fMOC;
				_mocChanged = -1;
			}

			WayPoint PtPrevFIX = null;
			if (_legs.Count == 1)
				PtPrevFIX = _prevLeg.EndFIX; //	_prevLeg.StartFIX;//			

			//ObstacleContainer
			_currentDetObs = Functions.GetLegAreaObstacles(out _currentObstacleList, _currLeg, _sumMinLegLenght + _currLeg.MinLegLength, _refElevation, fMOC, _MAClimb, PtPrevFIX);

			//_currentObstacleList = Functions.GetLegObstList(_currLeg, GlobalVars.ObstacleList, _MOCLimit, out int maxReqHi);
			//_currentDetObs.Obstacles = new Obstacle[1];
			//_currentDetObs.Parts = new ObstacleData[1];

			//if (maxReqHi >= 0)
			//{
			//	_currentDetObs.Parts[0]	 = _currentObstacleList.Parts [maxReqHi];
			//	_currentDetObs.Obstacles[0] = _currentObstacleList.Obstacles[_currentObstacleList.Parts[maxReqHi].Owner];
			//}

			_currLeg.DetObstacle_1 = _currentDetObs;
		}

		void InitNewPoint(WayPoint Prev)
		{
			string sNumP;

			_distanceIndex = -1;
			_directionIndex = -1;

			do
			{
				sNumP = _FN.ToString();
				if (_FN < 10)
					sNumP = '0' + sNumP;
				sNumP = "TP" + sNumP;

				_FN++;
			} while (!CheckName(sNumP) || (_currFIX != null && _currFIX.Name == sNumP));

			if (_FN > 1)
			{
				//Prev.Role = eFIXRole.TP_;
				//Prev.RefreshGraphics();
			}

			//WayPoint ReferenceFIX = Prev;
			_currFIX = new FIX(eFIXRole.TP_, _aranEnvironment);

			_currFIX.FlightPhase = Prev.FlightPhase;
			_currFIX.PBNType = Prev.PBNType;

			if (Prev.FlyMode == eFlyMode.Atheight)
				_currFIX.FlyMode = eFlyMode.Flyby;
			else
				_currFIX.FlyMode = Prev.FlyMode;

			_currFIX.SensorType = Prev.SensorType;
			_currFIX.Role = Prev.Role;

			_currFIX.BankAngle = Prev.BankAngle;
			_bankAngle = Prev.BankAngle;

			_currFIX.IAS = Prev.IAS;
			_currFIX.ISAtC = Prev.ISAtC;
			_currFIX.ConstructAltitude = Prev.ConstructAltitude;
			_currFIX.NomLineAltitude = Prev.NomLineAltitude;

			//_currFIX.ConstructionGradient = Prev.ConstructionGradient;
			//_currFIX.NomLineGradient = Prev.NomLineGradient;

			_currFIX.OutDirection = _currFIX.EntryDirection = Prev.OutDirection = Prev.EntryDirection;
			_currFIX.Name = sNumP;
			_currFIX.CallSign = sNumP;
			//=================================================================================

			_legPoints.Add(Prev);

			_currLeg = new LegDep(Prev, _currFIX, _aranEnvironment, _prevLeg);
			//_CurrLeg.Active = false;
			//_CurrLeg.Gradient = Gradient;
			//ConstructGradient = _constructGradientRange.Max;

			_currLeg.IAS = IAS;
			_currLeg.PathAndTermination = PathAndTermination;

			//LegBase.ProcessMessages();
			//_currLeg.RefreshGraphics();
			//LegBase.ProcessMessages();

			double min, max;

			if (_pathAndTermination == CodeSegmentPath.DF)
			{
				_plannedTurnLimits.Max = GlobalVars.f270;
				min = ARANMath.Modulus(Prev.EntryDirection - GlobalVars.f270, ARANMath.C_2xPI);
				max = ARANMath.Modulus(Prev.EntryDirection + GlobalVars.f270, ARANMath.C_2xPI);
			}
			else
			{
				_plannedTurnLimits.Max = GlobalVars.f120;
				min = ARANMath.Modulus(Prev.EntryDirection - _plannedTurnAngle, ARANMath.C_2xPI);       //GlobalVars.f120
				max = ARANMath.Modulus(Prev.EntryDirection + _plannedTurnAngle, ARANMath.C_2xPI);       //GlobalVars.f120
			}

			_outDirRange.Min = min;
			_outDirRange.Max = max;

			if (OnInitNewFIX != null)
				OnInitNewFIX(this, _referenceFIX, Prev);

			//=================================================================================
			_referenceFIX = Prev;

			//if (_referenceFIX.NomLineAltitude >= _accelerationAltitude)
			//{
			//	_nomLineGradientRange.Min = 0.0;
			//	_nomLineGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//}
			//else
			//{
			//	_nomLineGradientRange.Min = GlobalVars.NomLineGrd;
			//	_nomLineGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			//}
			_currFIX.NomLineGradient = Prev.NomLineGradient;
			//if (_currFIX.NomLineGradient > _nomLineGradientRange.Max)
			//	_currFIX.NomLineGradient = _nomLineGradientRange.Max;
			//if (_currFIX.NomLineGradient < _nomLineGradientRange.Min)
			//	_currFIX.NomLineGradient = _nomLineGradientRange.Min;


			//if (_referenceFIX.ConstructAltitude >= _accelerationAltitude)
			//{
			//	_constructGradientRange.Min = 0.0;
			//	_constructGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//}
			//else
			//{
			//	_constructGradientRange.Min = _nomLineGradientRange.Min;
			//	_constructGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			//}

			//_currFIX.ConstructionGradient = Prev.ConstructionGradient;

			//if (_currFIX.ConstructionGradient > _constructGradientRange.Max)
			//	_currFIX.ConstructionGradient = _constructGradientRange.Max;
			//if (_currFIX.ConstructionGradient < _constructGradientRange.Min)
			//	_currFIX.ConstructionGradient = _constructGradientRange.Min;


			_nominalAltitudeRange.Min = _referenceFIX.NomLineAltitude;
			CalcMaxAltitude();
			//if (_currFIX.NomLineAltitude < _nominalAltitudeRange.Min)
			//	_currFIX.NomLineAltitude = _nominalAltitudeRange.Min;

			//_nominalAltitudeRange.Max = Math.Min(_referenceFIX.NomLineAltitude + NomLineGradient * _distanceValue, GlobalVars.MaxAltitude);
			//if (_referenceFIX.NomLineAltitude < _accelerationAltitude && _nominalAltitudeRange.Max > _accelerationAltitude)
			//{
			//	double dd = (_accelerationAltitude - _referenceFIX.NomLineAltitude) / NomLineGradient;
			//	double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//	_nominalAltitudeRange.Max = Math.Min(hh, GlobalVars.MaxAltitude);
			//}

			//_constructAltitudeRange.Min = _referenceFIX.ConstructAltitude;
			//_constructAltitudeRange.Max = Math.Min(_referenceFIX.ConstructAltitude + ConstructGradient * _distanceValue, GlobalVars.MaxAltitude);
			//if (_referenceFIX.ConstructAltitude < _accelerationAltitude && _constructAltitudeRange.Max > _accelerationAltitude)
			//{
			//	double dd = (_accelerationAltitude - _referenceFIX.ConstructAltitude) / ConstructGradient;
			//	double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//	_constructAltitudeRange.Max = Math.Min(hh, GlobalVars.MaxAltitude);
			//}

			//ConstructGradient = _referenceFIX.ConstructionGradient;

			UpdateCurrFIX();
			FillCourseSgfPoints();      //??????
		}

		void FillCourseSgfPoints()
		{
			_courseSgfPoint.Clear();

			if (_pathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = -(int)(_referenceFIX.TurnDirection);
				double r = _referenceFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_referenceFIX.NomLinePrjPt, _referenceFIX.EntryDirection, 0, -r * fTurnDirection);

				//_aranEnvironment.Graphics.DrawPointWithText(ptCenter, -1, "Center");
				//Application.DoEvents();
				//Leg.ProcessMessages(true);

				foreach (WayPoint SigPoint in _significantCollection)
				{
					double dX = SigPoint.PrjPt.X - ptCenter.X;
					double dY = SigPoint.PrjPt.Y - ptCenter.Y;

					double dirDest = Math.Atan2(dY, dX);
					double distDest = ARANMath.Hypot(dX, dY);

					double TurnAngle = (_referenceFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
					double nomDir = ARANMath.Modulus(_referenceFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);

					double MinStabPrev, MinStabCurr;
					MinStabPrev = _referenceFIX.CalcNomLineFromMinStablizationDistance(TurnAngle, true);
					MinStabCurr = _currFIX.CalcNomLineInToMinStablizationDistance(_plannedTurnAngle);
					double mindist = 1000.0;                    // MinStabPrev + MinStabCurr;

					if (_outDirRange.CheckValue(nomDir) == nomDir)
						if (distDest > r && distDest >= mindist && distDest <= _distanceRange.Max)
							_courseSgfPoint.Add(SigPoint);
				}
			}
			else
				foreach (WayPoint SigPoint in _significantCollection)
				{
					double dX = SigPoint.PrjPt.X - _referenceFIX.NomLinePrjPt.X;
					double dY = SigPoint.PrjPt.Y - _referenceFIX.NomLinePrjPt.Y;
					double distDest = ARANMath.Hypot(dX, dY);

					double nomDir = ARANMath.Modulus(Math.Atan2(dY, dX), ARANMath.C_2xPI);
					double TurnAngle = ARANMath.Modulus((nomDir - _referenceFIX.EntryDirection) * ((int)ARANMath.SideFrom2Angle(nomDir, _referenceFIX.EntryDirection)), 2 * Math.PI);

					//_aranEnvironment.Graphics.DrawPointWithText(SigPoint.PrjPt, -1, "DD100");
					//_aranEnvironment.Graphics.DrawPointWithText(_ReferenceFIX.PrjPt, -1, "ReferenceFIX");
					//Leg.ProcessMessages();

					double MinStabPrev, MinStabCurr;
					MinStabPrev = _referenceFIX.CalcNomLineFromMinStablizationDistance(TurnAngle);
					MinStabCurr = _currFIX.CalcNomLineInToMinStablizationDistance(_plannedTurnAngle);
					double mindist = 1000.0;        // MinStabPrev + MinStabCurr;

					if (distDest >= mindist && distDest <= _distanceRange.Max)
						if (_outDirRange.CheckValue(nomDir) == nomDir)
							_courseSgfPoint.Add(SigPoint);
				}

			if (OnUpdateDirList != null)
				OnUpdateDirList(this, new EventArgs());
		}

		void FillDistanceSgfPoints()
		{
			_distanceSgfPoint.Clear();

			double MaxDist = _distanceRange.Max;
			double MinDist = _distanceRange.Min;
			Point LocalSigPoint, ptFrom;

			ptFrom = _referenceFIX.NomLinePrjPt;

			if (_currFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _referenceFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _referenceFIX.NomLineTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_referenceFIX.NomLinePrjPt, _referenceFIX.EntryDirection, 0.0, r * fTurnSide);
				ptFrom = ARANFunctions.LocalToPrj(ptCnt, _currFIX.EntryDirection, 0.0, -r * fTurnSide);

				//LegBase.ProcessMessages(true);

				//_aranEnvironment.Graphics.DrawPointWithText(ptFrom, "ptFrom-1");
				//_aranEnvironment.Graphics.DrawPointWithText(ptCnt, "ptCnt-1");
				//LegBase.ProcessMessages();


				//MinDist = Math.Max(r, _DistanceRange.Min);
			}
			else if ((_referenceFIX.FlyMode == eFlyMode.Flyover || _referenceFIX.FlyMode == eFlyMode.Atheight) && _pathAndTermination == CodeSegmentPath.CF)
			{
				double fTurnDir = (int)_referenceFIX.EffectiveTurnDirection;            //SideDef(_ReferenceFIX.PrjPt, _ReferenceFIX.EntryDir, _CurrFIX.PrjPt);

				double DivergenceAngle30 = GlobalVars.constants.Pansops[ePANSOPSData.arSecAreaCutAngl].Value;
				double Bank15 = GlobalVars.constants.Pansops[ePANSOPSData.rnvFlyOInterBank].Value;
				double R1 = _referenceFIX.NomLineTurnRadius;
				double R2 = ARANMath.BankToRadius(Bank15, _referenceFIX.NomLineTAS);
				double fTmp0 = Math.Acos((1 + R1 * Math.Cos(_referenceFIX.TurnAngle) / R2) / (1 + R1 / R2)) - ARANMath.EpsilonRadian;
				double DivergenceAngle = Math.Min(DivergenceAngle30, fTmp0);

				Point ptCnt = ARANFunctions.LocalToPrj(_referenceFIX.NomLinePrjPt, _referenceFIX.EntryDirection + ARANMath.C_PI_2 * fTurnDir, R1, 0);
				fTmp0 = _referenceFIX.EntryDirection + (_referenceFIX.TurnAngle + DivergenceAngle) * fTurnDir;

				Point ptRollOut1 = ARANFunctions.LocalToPrj(ptCnt, fTmp0 - ARANMath.C_PI_2 * fTurnDir, R1);

				if (ARANMath.SubtractAngles(fTmp0, _currFIX.EntryDirection) > ARANMath.EpsilonRadian)
				{
					Point ptInter = (Point)ARANFunctions.LineLineIntersect(ptRollOut1, fTmp0, _currFIX.PrjPt, _currFIX.EntryDirection);
					double fTmp1 = R2 * Math.Tan(0.5 * DivergenceAngle);
					ptFrom = ARANFunctions.LocalToPrj(ptInter, _currFIX.EntryDirection, fTmp1);
				}
			}

			//double distCorrection = ARANFunctions.ReturnDistanceInMeters(_referenceFIX.NomLinePrjPt, ptFrom);
			//MinDist -= distCorrection;

			//_aranEnvironment.Graphics.DrawPointWithText(ptFrom, -1,"ptFrom");
			//_aranEnvironment.Graphics.DrawPointWithText(_currFIX.PrjPt, -1, "CurrFIX");
			//LegBase.ProcessMessages();

			foreach (WayPoint SigPoint in _courseSgfPoint)
			{
				LocalSigPoint = ARANFunctions.PrjToLocal(ptFrom, _currFIX.EntryDirection, SigPoint.PrjPt);

				//if (LocalSigPoint.X >= MinDist && LocalSigPoint.X <= MaxDist)
				//&& Math.Abs(LocalSigPoint.Y) <= 0.25 * _referenceFIX.XTT

				if (LocalSigPoint.X >= MinDist && LocalSigPoint.X <= MaxDist && Math.Abs(LocalSigPoint.Y) <= 1.0)
					_distanceSgfPoint.Add(SigPoint);
			}

			if (OnUpdateDistList != null)
				OnUpdateDistList(this, new EventArgs());
		}

		double CalcMaxAltitude()
		{
			_nominalAltitudeRange.Max = Math.Min(_referenceFIX.NomLineAltitude + _MAClimb * _distanceValue, GlobalVars.MaxAltitude);
			//if (_referenceFIX.NomLineAltitude < _accelerationAltitude && _nominalAltitudeRange.Max > _accelerationAltitude)
			//{
			//	double dd = (_accelerationAltitude - _referenceFIX.NomLineAltitude) / grd;
			//	double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//	_nominalAltitudeRange.Max = Math.Min(hh, GlobalVars.MaxAltitude);
			//}

			return _nominalAltitudeRange.Max;

			double MinStabFrom = _referenceFIX.CalcNomLineFromMinStablizationDistance(_referenceFIX.TurnAngle, _pathAndTermination == CodeSegmentPath.DF);
			if (_currFIX.FlyMode != eFlyMode.Flyby)
			{
				double MinStabIn = _currFIX.CalcNomLineInToMinStablizationDistance(_plannedTurnAngle);
				double minDist = MinStabIn + MinStabFrom;

				_nominalAltitudeRange.Max = Math.Min(_referenceFIX.NomLineAltitude + _MAClimb * minDist, GlobalVars.MaxAltitude);
				return _nominalAltitudeRange.Max;
			}

			double TurnAngle = _plannedTurnAngle;
			if (ARANMath.RadToDeg(_plannedTurnAngle) < 50.0)
				TurnAngle = ARANMath.DegToRad(50.0);

			double kTurn = Math.Tan(0.5 * TurnAngle);
			double MaxAlt, mindistOld = _distanceRange.Min;

			int i = 100;

			do
			{
				MaxAlt = Math.Min(_nominalAltitudeRange.Min + _MAClimb * mindistOld, GlobalVars.MaxAltitude);
				double RTurn = ARANMath.BankToRadiusForRnav(_currFIX.BankAngle, _currFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);
				double TAS = ARANMath.IASToTASForRnav(_currFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);

				double MinStabIn = Math.Max(RTurn * kTurn + constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value * TAS, _currFIX.ATT);

				double minDist = MinStabFrom + MinStabIn;
				double delta = minDist - mindistOld;

				if (Math.Abs(delta) < ARANMath.EpsilonDistance)
					break;

				mindistOld = minDist;           // +delta;
												//if (MaxAlt < MaxAltitude)		mindistOld += delta;

			} while (--i >= 0);

			if (i < 0)
				throw new Exception("IFA ERROR: Invalid function argument!");

			_nominalAltitudeRange.Max = MaxAlt;//Math.Min(MaxAlt, MaxAltitude);

			return MaxAlt;
		}

		double CalcMinDistance()
		{
			double MinStabFrom = _referenceFIX.CalcNomLineFromMinStablizationDistance(_referenceFIX.TurnAngle, _pathAndTermination == CodeSegmentPath.DF);

			if (_currFIX.FlyMode != eFlyMode.Flyby)
			{
				double MinStabIn = _currFIX.CalcNomLineInToMinStablizationDistance(_plannedTurnAngle);
				double minDist = MinStabIn + MinStabFrom;
				_distanceRange.Min = minDist;
				return _distanceRange.Min;
			}

			double mindistOld = _distanceRange.Min;

			double TurnAngle = _plannedTurnAngle;
			if (ARANMath.RadToDeg(_plannedTurnAngle) < 50.0)
				TurnAngle = ARANMath.DegToRad(50.0);
			double kTurn = Math.Tan(0.5 * TurnAngle);

			double MaxAlt, mindist;
			int i = 100;

			do
			{
				MaxAlt = Math.Min(_nominalAltitudeRange.Min + _MAClimb * mindistOld, GlobalVars.MaxAltitude);

				double RTurn = ARANMath.BankToRadiusForRnav(_currFIX.BankAngle, _currFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);
				double TAS = ARANMath.IASToTASForRnav(_currFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);
				double MinStabIn = Math.Max(RTurn * kTurn + constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value * TAS, _currFIX.ATT);

				mindist = Math.Max(MinStabIn, MinStabFrom);
				//mindist = MinStabIn + MinStabFrom;
				double delta = mindist - mindistOld;

				mindistOld = mindist;
				//mindistOld = mindist;	// +delta;
				//if (MaxAlt < MaxAltitude)					mindistOld += delta;
				//if (delta > 0.0)			mindistOld += delta;

				if (Math.Abs(delta) < ARANMath.EpsilonDistance)
					break;
			} while (--i >= 0);

			if (i < 0)
				throw new Exception("IFA ERROR: Invalid function argument!");

			_distanceRange.Min = mindistOld;
			return _distanceRange.Min;
		}

		bool ChangeDistance(double newVal)
		{
			if (newVal < _distanceRange.Min)
				newVal = _distanceRange.Min;

			if (newVal > _distanceRange.Max)
				newVal = _distanceRange.Max;

			if (Math.Abs(_distanceValue - newVal) > ARANMath.EpsilonDistance)
			{
				_distanceValue = newVal;
				double CurrAltitue = _currFIX.NomLineAltitude;

				//_NomLineGradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
				CalcMaxAltitude();

				//if (_nominalAltitudeRange.Max == GlobalVars.MaxAltitude)
				//	_nomLineGradientRange.Max = (_nominalAltitudeRange.Max - _nominalAltitudeRange.Min) / _distanceValue;

				//_currFIX.NomLineAltitude = _nominalAltitudeRange.Max;

				//if (_currFIX.NomLineAltitude > _nominalAltitudeRange.Max)		_currFIX.NomLineAltitude = _nominalAltitudeRange.Max;

				//if (_referenceFIX.NomLineAltitude > _accelerationAltitude || _nominalAltitudeRange.Max < _accelerationAltitude)
				//{
				//	double fTmp = (_currFIX.NomLineAltitude - _referenceFIX.NomLineAltitude) / _distanceValue;
				//	if (Math.Abs(fTmp - _currFIX.NomLineGradient) > ARANMath.EpsilonRadian)
				//		NomLineGradient = (_currFIX.NomLineAltitude - _nominalAltitudeRange.Min) / _distanceValue;
				//}
				//_currFIX.NomLineGradient = (_currFIX.NomLineAltitude - _nominalAltitudeRange.Min) / _distanceValue;

				double fAltitude = _referenceFIX.ConstructAltitude + _distanceValue * _MAClimb;
				//if (_referenceFIX.ConstructAltitude < _accelerationAltitude && fAltitude > _accelerationAltitude)
				//{
				//	double dd = (_accelerationAltitude - _referenceFIX.ConstructAltitude) / ConstructGradient;
				//	double hh = _accelerationAltitude + (_distanceValue - dd) * constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
				//	fAltitude = Math.Min(hh, GlobalVars.MaxAltitude);
				//}

				_nominalAltitudeRange.Max = _referenceFIX.ConstructAltitude + _distanceValue * _MAClimb;
				_currFIX.NomLineAltitude = _currFIX.ConstructAltitude = fAltitude;

				//if (_constructAltitudeRange.Max == GlobalVars.MaxAltitude)
				//	_constructGradientRange.Max = (_constructAltitudeRange.Max - _constructAltitudeRange.Min) / _distanceValue;

				//if (_currFIX.ConstructAltitude > _constructAltitudeRange.Max)
				//	_currFIX.ConstructAltitude = _constructAltitudeRange.Max;

				//if (_referenceFIX.ConstructAltitude > _accelerationAltitude || _constructAltitudeRange.Max < _accelerationAltitude)
				//{
				//	double fTmp = (_currFIX.ConstructAltitude - _constructAltitudeRange.Min) / _distanceValue;
				//	if (Math.Abs(fTmp - _currFIX.ConstructionGradient) > ARANMath.EpsilonRadian)
				//		//NomLineGradient = (_currFIX.NomLineAltitude - _nominalAltitudeRange.Min) / _distanceValue;
				//		ConstructGradient = (_currFIX.ConstructAltitude - _constructAltitudeRange.Min) / _distanceValue;
				//}

				if (OnDistanceChanged != null)
					OnDistanceChanged(this, _distanceValue);

				if (CurrAltitue != _currFIX.NomLineAltitude && OnNomlineAltitudeChanged != null)
					OnNomlineAltitudeChanged(this, _currFIX.NomLineAltitude);

				//_currFIX.NomLineAltitude = _nominalAltitudeRange.Max;

				return true;
			}

			return false;
		}

		//void RedrawNominal(double newVal)
		//{
		//	_CurrFIX.EntryDirection_N = newVal;
		//	_ReferenceFIX.OutDirection_N = newVal;
		//	_CurrLeg.CreateNomTrack(_PrevLeg);
		//	_PrevLeg.RefreshGraphics();
		//	_CurrLeg.RefreshGraphics();
		//}

		void ChangeCourse(double newVal)
		{
			double OldTurnAngle = _referenceFIX.TurnAngle;

			_currFIX.EntryDirection = newVal;
			_referenceFIX.OutDirection = newVal;

			//_referenceFIX.FlyMode = eFlyMode.Flyby;

			if (!_currFIX.IsDFTarget)
			{
				double fAngle = ARANMath.Modulus(_referenceFIX.EntryDirection - _currFIX.EntryDirection, ARANMath.C_2xPI);
				if (Math.Abs(fAngle) < ARANMath.EpsilonRadian)
					_referenceFIX.TurnDirection = TurnDirection.NONE;
				else if (fAngle < Math.PI)
					_referenceFIX.TurnDirection = TurnDirection.CW;
				else
					_referenceFIX.TurnDirection = TurnDirection.CCW;
			}

			if (_updateEnabled)
			{
				UpdateFixParams(false);
				CheckDistanceFIXes();

				bool isSame = false;
				if (_distanceIndex >= 0 && _directionIndex >= 0)
				{
					WayPoint dirPoint = _courseSgfPoint[_directionIndex];
					WayPoint sigPoint = _distanceSgfPoint[_distanceIndex];

					isSame = sigPoint.Id == dirPoint.Id;
				}

				UpdateCurrFIX();

				if (!isSame)
					FillDistanceSgfPoints();

				if (OnDirectionChanged != null)
					OnDirectionChanged(this, newVal);

				//FillDistanceSgfPoints();
			}
		}

		//==============================================================================
		void CheckDistanceFIXes()
		{
			if (_currFIX.IsDFTarget)
				_distanceIndex = -1;

			if (_distanceIndex < 0)
				return;


			if (_directionIndex < 0 || (_courseSgfPoint[_directionIndex].Id != _distanceSgfPoint[_distanceIndex].Id))
				_distanceIndex = -1;

			//Point LocalSigPoint = ARANFunctions.PrjToLocal(_ReferenceFIX.PrjPt, _CurrFIX.EntryDirection, _DistanceSgfPoint[_DistanceIndex].PrjPt);

			//if (LocalSigPoint.X > _DistanceRange.Min && LocalSigPoint.X <= _DistanceRange.Max && Math.Abs(LocalSigPoint.Y) <= 0.25 * _ReferenceFIX.XXT)
			//{

			//}
			//else
			//{
			//	_DistanceIndex = -1;
			//}
		}

		bool UpdateFixParams(bool updateCourseList = true)      //updateDistanceList
		{
			bool result = false;
			double minDist, OldMinDist = _distanceRange.Min;    //resetDirection

			minDist = CalcMinDistance();

			if (Math.Abs(OldMinDist - _distanceRange.Min) > ARANMath.EpsilonDistance)
			{
				if (updateCourseList)
					FillCourseSgfPoints();

				if (_distanceValue < minDist)
					result = ChangeDistance(minDist);

				FillDistanceSgfPoints();
			}

			return result;
		}

		void UpdateCurrFIX()
		{
			Point FIXPoint, ptInOutBase, ptOutOutBase;
			//	_accelerationAltitude
			if (_currFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _referenceFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _referenceFIX.NomLineTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_referenceFIX.NomLinePrjPt, _referenceFIX.EntryDirection, 0.0, r * fTurnSide);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, _currFIX.EntryDirection, 0.0, -r * fTurnSide);

				//if (_DistanceIndex < 0)
				//{
				//_aranEnvironment.Graphics.DrawPointWithText(ptFrom, -1, "ptFrom");
				//_aranEnvironment.Graphics.DrawPointWithText(ptCnt, -1, "ptCnt");
				//LegBase.ProcessMessages();

				if (_pathAndTermination == CodeSegmentPath.DF)
					FIXPoint = ARANFunctions.LocalToPrj(ptFrom, _currFIX.EntryDirection, _distanceValue, 0.0);
				else
					FIXPoint = ARANFunctions.CircleVectorIntersect(_referenceFIX.NomLinePrjPt, _distanceValue, ptFrom, _currFIX.EntryDirection);

				_currFIX.PrjPt = FIXPoint;

				//_aranEnvironment.Graphics.DrawPointWithText(FIXPoint, -1, "FIXPoint");
				//LegBase.ProcessMessages();
				//}
				//else
				//	FIXPoint = _CurrFIX.PrjPt;

				if (TurnDir == TurnDirection.CW)
				{
					_currFIX.OutDirection = _currFIX.EntryDirection + _plannedTurnLimits.Max;   // ARANMath.C_PI
					_currFIX.EntryDirection_L = _referenceFIX.CalcDFOuterDirection(_currFIX, out ptOutOutBase, _prevLeg.PrimaryArea);
					_currFIX.EntryDirection_R = _referenceFIX.CalcDFInnerDirection(_currFIX, out ptInOutBase, _prevLeg);
				}
				else
				{
					_currFIX.OutDirection = _currFIX.EntryDirection - _plannedTurnLimits.Max;   // ARANMath.C_PI
					_currFIX.EntryDirection_R = _referenceFIX.CalcDFOuterDirection(_currFIX, out ptOutOutBase, _prevLeg.PrimaryArea);
					_currFIX.EntryDirection_L = _referenceFIX.CalcDFInnerDirection(_currFIX, out ptInOutBase, _prevLeg);
				}
			}
			else
			{
				if (_distanceIndex < 0)
				{
					FIXPoint = ARANFunctions.LocalToPrj(_referenceFIX.NomLinePrjPt, _referenceFIX.OutDirection, _distanceValue);

					//_aranEnvironment.Graphics.DrawPointWithText(FIXPoint, -1, "FIXPoint");
					//LegBase.ProcessMessages();

					_currFIX.PrjPt = FIXPoint;
				}
				else
					FIXPoint = _currFIX.PrjPt;

				//_aranEnvironment.Graphics.DrawPointWithText(FIXPoint, -1, "FIXPoint");
				//LegBase.ProcessMessages();

				if (_currFIX.FlyMode == eFlyMode.Flyover)
					_currFIX.OutDirection = _currFIX.EntryDirection + _plannedTurnLimits.Max;   // ARANMath.C_PI
				else
					_currFIX.OutDirection = _currFIX.EntryDirection;    // ARANMath.C_PI
			}

			//double fDist = ARANMath.Hypot(FIXPoint.X - _ADHP.pPtPrj.X, FIXPoint.Y - _ADHP.pPtPrj.Y);
			double fDist = ARANFunctions.ReturnDistanceInMeters(FIXPoint, _ADHP.pPtPrj);
			//double minDist = CalcMinDistance(Gradient);

			_currFIX.FlightPhase = _referenceFIX.FlightPhase;

			if (_referenceFIX.FlightPhase == eFlightPhase.SID)
			{
				if (fDist >= PANSOPSConstantList.PBNInternalTriggerDistance)
					_currFIX.FlightPhase = eFlightPhase.SIDGE28;
			}
			else if (_referenceFIX.FlightPhase >= eFlightPhase.SIDGE28)
			{
				if (fDist >= PANSOPSConstantList.PBNTerminalTriggerDistance)
					_currFIX.FlightPhase = eFlightPhase.SIDGE56;
			}

			_currFIX.BankAngle = _bankAngle;

			if (_legs.Count == 1)
			{
				_prevLeg.EndFIX.OutDirection = _currFIX.EntryDirection;
				_prevLeg.EndFIX.TurnDirection = _referenceFIX.EffectiveTurnDirection;       // ARANMath.SideFrom2Angle(_CurrFIX.EntryDirection, _PrevLeg.EndFIX.EntryDirection);
			}

			//_aranEnvironment.Graphics.DrawPointWithText(_prevLeg.EndFIX.PrjPt, "_prevLeg.EndFIX");
			//LegBase.ProcessMessages();

			_currLeg.StartFIX.Assign(_prevLeg.EndFIX);
			_currLeg.EndFIX.Assign(_currFIX);

			_currLeg.CreateGeometry(_prevLeg, _ADHP);

			GeometryOperators go = new GeometryOperators();

			//_currLeg.RefreshGraphics();
			//LegBase.ProcessMessages();

			if (_legs.Count == 1)
			{
				//_SumMinLegLenght = go.GetDistance(_PrevLeg.StartFIX.PrjPt, _CurrLeg.KKLine);
				//_CurrLeg.MinLegLength = _SumMinLegLenght;
				_currLeg.MinLegLength = go.GetDistance(_prevLeg.StartFIX.PrjPt, _currLeg.KKLine);

				if (_currLeg.StartFIX.FlyMode == eFlyMode.Atheight)
					_currLeg.KKLine = _prevLeg.FullAssesmentArea;       // ARANFunctions.PolygonToPolyLine(_PrevLeg.FullAssesmentArea[0])[0];
			}
			else
				_currLeg.MinLegLength = go.GetDistance(_prevLeg.KKLine, _currLeg.KKLine);

			//_aranEnvironment.Graphics.DrawLineString((LineString)_CurrLeg.KKLine, -1, 2);
			//_aranEnvironment.Graphics.DrawMultiPolygon(_PrevLeg.FullAssesmentArea, -1, eFillStyle.sfsBackwardDiagonal);
			//LegBase.ProcessMessages();

			//_prevLeg.RefreshGraphics();
			_currLeg.RefreshGraphics();
			//LegBase.ProcessMessages(true);

			GetOstacles();

			OnFIXUpdated?.Invoke(this, _currFIX);
		}

	}
}
