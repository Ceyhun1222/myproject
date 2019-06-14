using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Geometries.Operators;
using Aran.AranEnvironment.Symbols;
using System.Windows.Forms;
using Aran.Aim.Enums;

namespace Aran.PANDA.RNAV.SGBAS
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public delegate void OnInitNewFIXHandler(object sender, WayPoint ReferenceFIX, WayPoint AddedFIX);
	public delegate void OnFIXUpdatedHandler(object sender, WayPoint CurrFIX);
	//public delegate void OnTransitionOver56(object sender, bool transfers);

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Transitions
	{
		public const double MaxAltitude = 12800.0;

		static int FN = 0;
		protected static Aran.PANDA.Constants.Constants constants = null;
		protected static SpatialReferenceOperation pSpatialReferenceOperation = null;
		protected AranEnvironment.IAranEnvironment _aranEnvironment;

		public System.EventHandler OnUpdateDirList;
		public System.EventHandler OnUpdateDistList;
		//public OnTransitionOver56 OnTransitOver56;

		public OnInitNewFIXHandler OnInitNewFIX;
		public OnFIXUpdatedHandler OnFIXUpdated;

		public DoubleValEventHandler OnDistanceChanged;
		public DoubleValEventHandler OnDirectionChanged;
		public DoubleValEventHandler OnAltitudeChanged;

		//Segment1Term _term;

		WayPoint _ReferenceFIX;
		public WayPoint ReferenceFIX { get { return _ReferenceFIX; } }

		aircraftCategory _aircraftCategory;
		List<WayPoint> _SignificantCollection;

		FIX _CurrFIX;
		LegApch _PrevLeg;
		LegApch _CurrLeg;

		#region Simple Props

		ADHPType _Adhp;
		public ADHPType ADHP { get { return _Adhp; } }

		RWYType _RWY;
		public RWYType RWY { get { return _RWY; } }

		Point _CourseInConvertionPoint;
		public Point CourseInConvertionPoint { get { return _CourseInConvertionPoint; } }

		Point _CourseOutConvertionPoint;
		public Point CourseOutConvertionPoint { get { return _CourseOutConvertionPoint; } }

		List<WayPoint> _LegPoints;
		public List<WayPoint> LegPoints { get { return _LegPoints; } }

		List<LegApch> _Legs;
		public List<LegApch> Legs { get { return _Legs; } }

		List<WayPoint> _CourseSgfPoint;
		public List<WayPoint> CourseSgfPoint { get { return _CourseSgfPoint; } }

		List<WayPoint> _DistanceSgfPoint;
		public List<WayPoint> DistanceSgfPoint { get { return _DistanceSgfPoint; } }

		public string FIXName
		{
			get { return _CurrFIX.Name; }
			set { _CurrFIX.Name = value; }
		}

		public double InDirection { get { return _ReferenceFIX.EntryDirection; } }

		public double EntryDirection { get { return _ReferenceFIX.EntryDirection; } }

		Interval _GradientRange;
		Interval GradientRange { get { return _GradientRange; } }

		Interval _OutDirRange;
		public Interval OutDirRange { get { return _OutDirRange; } }

		Interval _DistanceRange;
		Interval DistanceRange { get { return _DistanceRange; } }

		ObstacleData _CurrentDetObs;
		public ObstacleData CurrentDetObs { get { return _CurrentDetObs; } }

		ObstacleContainer _CurrentObstacleList;
		public ObstacleContainer CurrentObstacleList { get { return _CurrentObstacleList; } }

		Interval _IASRange;
		Interval IASRange { get { return _IASRange; } }

		Interval _AltitudeRange;
		Interval AltitudeRange { get { return _AltitudeRange; } }

		public double Gradient
		{
			get
			{
				return _CurrFIX.ConstructionGradient;
			}

			//private 
			set
			{
				if (value < _GradientRange.Min)
					value = _GradientRange.Min;

				if (value > _GradientRange.Max)
					value = _GradientRange.Max;

				_CurrFIX.ConstructionGradient = value;
				_CurrLeg.Gradient = value;
			}
		}

		//int _prevTransitions;
		//int _transferCounter;
		public bool transferedOver56 { get { return _CurrLeg.transferedOver56; } }
		#endregion

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
						_DistanceRange.Min -= 0.0125;
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		public bool MultiCoverage
		{
			get { return _CurrFIX.MultiCoverage; }
			set
			{
				if (_CurrFIX.MultiCoverage != value)
				{
					_CurrFIX.MultiCoverage = value;
					if (_updateEnabled && _CurrFIX.SensorType == eSensorType.DME_DME)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		double _BankAngle;
		public double BankAngle
		{
			get { return _BankAngle; }
			set
			{
				if (_BankAngle != value)
				{
					_BankAngle = value;
					_CurrFIX.BankAngle = value;

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
			get { return _CurrFIX.PBNType; }
			set
			{
				if (_CurrFIX.PBNType != value)
				{
					_CurrFIX.PBNType = value;

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
			get { return _CurrFIX.SensorType; }
			set
			{
				if (_CurrFIX.SensorType != value)
				{
					_CurrFIX.SensorType = value;

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
			get { return _CurrFIX.FlyMode; }
			set
			{
				if (_CurrFIX.FlyMode != value)
				{
					_CurrFIX.FlyMode = value;

					if (_updateEnabled)
					{
						UpdateFixParams();
						UpdateCurrFIX();
					}
				}
			}
		}

		double _DistanceValue;
		public double Distance
		{
			get
			{
				return _DistanceValue;
			}

			set
			{
				if (ChangeDistance(value) && _updateEnabled)
					UpdateCurrFIX();
			}
		}

		string _OldCallSign;
		string _OldName;
		Guid _OldID;

		int _DistanceIndex;
		public int DistanceIndex
		{
			get { return _DistanceIndex; }

			set
			{
				if (_DistanceIndex < 0)
				{
					if (value >= 0)
					{
						_OldCallSign = _CurrFIX.CallSign;
						_OldName = _CurrFIX.Name;
						_OldID = _CurrFIX.Id;
					}
				}

				_DistanceIndex = value;
				if (_DistanceIndex >= 0)
				{
					WayPoint sigPoint = _DistanceSgfPoint[_DistanceIndex];
					_CurrFIX.CallSign = sigPoint.CallSign;
					_CurrFIX.Name = sigPoint.Name;
					_CurrFIX.Id = sigPoint.Id;
				}
				else
				{
					_CurrFIX.CallSign = _OldCallSign;
					_CurrFIX.Name = _OldName;
					_CurrFIX.Id = _OldID;
				}
			}
		}

		static public Point CalcDRTurnParams(WayPoint StartFIX, WayPoint EndFIX, out double TurnAngle, out double NomDir)
		{
			double fTurnDirection = -(int)(StartFIX.TurnDirection);
			double r = StartFIX.ConstructTurnRadius;
			Point ptCenter = ARANFunctions.LocalToPrj(StartFIX.PrjPt, StartFIX.EntryDirection, 0, -r * fTurnDirection);

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

		int _DirectionIndex;
		public int DirectionIndex
		{
			get { return _DirectionIndex; }
			set
			{
				_DirectionIndex = value;

				if (_DirectionIndex != -1)
				{
					double nomdir, turnAngle;
					Point ptCenter = CalcDRTurnParams(_ReferenceFIX, _CurrFIX, out turnAngle, out nomdir);
				}
			}
		}

		public double OutDirection
		{
			get { return _CurrFIX.EntryDirection; }
			set
			{
				value = _OutDirRange.CheckValue(value);
				if (_CurrFIX.EntryDirection != value)
				{
					ChangeCourse(value);
				}
			}
		}

		double _MOCLimit;
		public double MOCLimit
		{
			get
			{
				return _MOCLimit;
			}
			set
			{
				if (_MOCLimit != value)
				{
					_MOCLimit = value;
					if (_updateEnabled)
						GetOstacles();
				}
			}
		}

		public double IAS
		{
			get
			{
				return _CurrFIX.IAS;
			}

			set
			{
				//if (value < _IASRange.Min)
				//    value = _IASRange.Min;

				if (value < _ReferenceFIX.IAS)
					value = _ReferenceFIX.IAS;

				if (value > _IASRange.Max)
					value = _IASRange.Max;

				if (Math.Abs(value - _CurrFIX.IAS) < ARANMath.EpsilonDistance)
					return;

				_CurrFIX.IAS = value;
				_CurrLeg.IAS = value;

				if (_updateEnabled)
				{
					UpdateFixParams();
					UpdateCurrFIX();
				}
			}
		}

		public double Altitude
		{
			get
			{
				return _CurrFIX.NomLineAltitude;
			}

			set
			{
				if (value > _AltitudeRange.Max)
					value = _AltitudeRange.Max;

				if (value < _AltitudeRange.Min)
					value = _AltitudeRange.Min;

				Gradient = (value - _AltitudeRange.Min) / (_DistanceValue);

				_CurrFIX.NomLineAltitude = value;

				if (_updateEnabled)
				{
					UpdateFixParams();
					UpdateCurrFIX();

					if (OnAltitudeChanged != null)
						OnAltitudeChanged(this, _CurrFIX.NomLineAltitude);
				}
			}
		}

		Interval _plannedTurnLimits;

		double _plannedTurnAngle;
		public double plannedTurnAngle
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

					if (_PathAndTermination == CodeSegmentPath.DF)
					{
						_plannedTurnLimits.Max = GlobalVars.f270;
						double min, max;

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

						_OutDirRange.Min = min;
						_OutDirRange.Max = max;

						double NewCourse = _OutDirRange.CheckValue(_CurrFIX.EntryDirection);
						//if (_CurrFIX.EntryDirection != NewCourse)
						//{
						//	_CurrFIX.TurnDirection = _turnDirection;
                        _ReferenceFIX.OutDirection = NewCourse;
						_ReferenceFIX.TurnDirection = _turnDirection;
						ChangeCourse(NewCourse);
						//FillCourseSgfPoints();		//??????
						//}
					}
					else
						_plannedTurnLimits.Max = GlobalVars.f120;
				}
			}
		}

		CodeSegmentPath _PathAndTermination;
		public CodeSegmentPath PathAndTermination
        {
            get { return _PathAndTermination; }
            set
            {
                if (_PathAndTermination != value)
                {
                    _PathAndTermination = value;

                    bool OldDFTarget = _CurrFIX.IsDFTarget;
					_CurrFIX.IsDFTarget = value == CodeSegmentPath.DF;

                    double min, max;
					if (_PathAndTermination == CodeSegmentPath.DF)
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
                        FillCourseSgfPoints();		//??????
                    }
                    else
                    {
                        _plannedTurnLimits.Max = GlobalVars.f120;
                        min = ARANMath.Modulus(_ReferenceFIX.EntryDirection - _plannedTurnAngle, ARANMath.C_2xPI);	//GlobalVars.f120
                        max = ARANMath.Modulus(_ReferenceFIX.EntryDirection + _plannedTurnAngle, ARANMath.C_2xPI);	//GlobalVars.f120

                        _OutDirRange.Min = min;
                        _OutDirRange.Max = max;

                        ChangeCourse(_CurrFIX.EntryDirection);		//OutDirection = _CurrFIX.EntryDirection;	//??????
                    }

                    //if (OldDFTarget != _CurrFIX.DFTarget)						UpdateCurrFIX();
                }
            }
        }

		//=================================================================================================================
		double _PrevSumMinLegLenght;
		double _SumMinLegLenght;
		double _MACG;
		Point _ptSOC;

		public Transitions(List<WayPoint> SignificantCollection, ADHPType ADHP, RWYType RWY, Point ptSOC, LegApch prevLeg, double MACG, double plannedMaxTurnAngle,
			double MaxDist, aircraftCategory category, AranEnvironment.IAranEnvironment aranEnvironment, OnInitNewFIXHandler OnInitPointHandler)
		{
			if (constants == null)
				constants = new Aran.PANDA.Constants.Constants();
			if (pSpatialReferenceOperation == null)
				pSpatialReferenceOperation = new SpatialReferenceOperation(aranEnvironment);

			_aranEnvironment = aranEnvironment;
			_DistanceIndex = -1;
			DirectionIndex = -1;
			_MACG = MACG;
			FN = 0;

			//_term = term;
			_Adhp = ADHP;
			_PrevLeg = prevLeg;
			_PrevLeg.transferedOver56 = false;

			OnInitNewFIX = OnInitPointHandler;
			_aircraftCategory = category;
			_plannedTurnLimits.Min = 0;
			_plannedTurnLimits.Max = GlobalVars.f120;		//ARANMath.C_PI_2;	//constants.Pansops[ePANSOPSData.rnvIFMaxTurnAngl].Value;

			_plannedTurnAngle = plannedMaxTurnAngle;

			//_PrevLeg.EndFIX.TurnAltitude = _term.TerminationAltitude;

			_PrevLeg.CreateKKLine(null);

			_RWY = RWY;
			_ptSOC = ptSOC;

			_SignificantCollection = SignificantCollection;

			WayPoint prevFix = (WayPoint)_PrevLeg.EndFIX.Clone();

			_OutDirRange.Circular = true;

			_OutDirRange.Min = prevFix.EntryDirection - _plannedTurnAngle + ARANMath.DegToRad(0.49);
			_OutDirRange.Max = prevFix.EntryDirection + _plannedTurnAngle - ARANMath.DegToRad(0.49);

			_CourseInConvertionPoint = prevFix.GeoPt.Clone() as Point;	// (Point)ReferenceFIX.AngleParameter.InConvertionPoint.Clone();
			_CourseOutConvertionPoint = prevFix.PrjPt.Clone() as Point;	// (Point)ReferenceFIX.AngleParameter.OutConvertionPoint.Clone();

			_IASRange.Min = 1.1 * constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[_aircraftCategory];
			_IASRange.Max = 1.1 * constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[_aircraftCategory];

			_GradientRange.Min = 0;// constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			_GradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;// 16.6;	//GradientLimits

			_LegPoints = new List<WayPoint>();
			_Legs = new List<LegApch>();
			_Legs.Add(_PrevLeg);

			//_transferCounter = _prevTransitions = 0;

			prevFix.OutDirection = prevFix.EntryDirection;

			_CourseSgfPoint = new List<WayPoint>();
			_DistanceSgfPoint = new List<WayPoint>();
			//term.leg.StartFIX.Altitude =term.leg.StartFIX.PrjPt.Z+5;
			_ReferenceFIX = _PrevLeg.StartFIX;//	term.leg.StartFIX;

			//double fTmp = ARANFunctions.ReturnDistanceInMeters(_PrevLeg.StartFIX.PrjPt, term.leg.StartFIX.PrjPt);

			_AltitudeRange.Circular = false;
			_AltitudeRange.Min = prevFix.NomLineAltitude;

			//==================================================================
			_DistanceRange.Circular = false;
			_DistanceRange.Max = MaxDist;

			//double MinStabFrom = prevFix.ATT;
			double MinStabFrom = prevFix.CalcConstructionFromMinStablizationDistance(_plannedTurnAngle);

			//_aranEnvironment.Graphics.DrawPointWithText(_PrevLeg.EndFIX.PrjPt, -1, "ptEndFIX");
			//Leg.ProcessMessages();

			double MinStabIn = _PrevLeg.EndFIX.CalcConstructionInToMinStablizationDistance(_plannedTurnAngle);

			//double MinStabIn = prevFix.CalcInToMinStablizationDistance(_plannedTurnAngle);

			double minDist = MinStabIn + MinStabFrom;

			if (prevFix.FlyMode == eFlyMode.Flyby)
			{
				double kTurn = Math.Tan(0.5 * _plannedTurnAngle);
				int i = 100;

				while (i >= 0)
				{
					double hTurn = Math.Min(prevFix.NomLineAltitude + _GradientRange.Max * minDist, MaxAltitude);
					double RTurn = ARANMath.BankToRadiusForRnav(prevFix.BankAngle, prevFix.IAS, hTurn, constants.Pansops[ePANSOPSData.ISA].Value);	//prevFix.ISA
					double TAS = ARANMath.IASToTASForRnav(prevFix.IAS, hTurn, constants.Pansops[ePANSOPSData.ISA].Value);							//prevFix.ISA

					MinStabIn = Math.Max(RTurn * kTurn + constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value * TAS, prevFix.ATT);

					double minDistNew = MinStabIn + MinStabFrom;
					double delta = minDistNew - minDist;

					minDist = minDistNew;
					if (Math.Abs(delta) < ARANMath.EpsilonDistance)
						break;
					i--;
				}
			}

			_DistanceRange.Min = minDist;

			_DistanceValue = _DistanceRange.Min;
			_AltitudeRange.Max = Math.Min(MaxAltitude, _AltitudeRange.Min + _GradientRange.Max * _DistanceValue);
			_PrevSumMinLegLenght = _SumMinLegLenght = 0.0;

			InitNewPoint(prevFix);
			Gradient = _GradientRange.Max;
			Altitude = _AltitudeRange.Max;
		}

		public void Clean()
		{
			_PrevLeg.DeleteGraphics();
			_CurrLeg.DeleteGraphics();

			for (int i = 0; i < _Legs.Count; i++)
				_Legs[i].DeleteGraphics();
		}

		bool CheckName(string NewName)
		{
			NewName = NewName.ToUpper();

			foreach (WayPoint fix in _LegPoints)
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
			if (_Legs.Count < 2)
				return false;

			_Legs.Remove(_PrevLeg);
			if (!Closed)
			{
				_CurrLeg.DeleteGraphics();	///////////////////
				_LegPoints.RemoveAt(_LegPoints.Count - 1);
			}

			//Application.DoEvents();

			_CurrLeg = _PrevLeg;
			_CurrLeg.Active = false;

			_SumMinLegLenght -= _CurrLeg.MinLegLength;
			_ReferenceFIX = _CurrLeg.StartFIX;

			_CurrFIX = (FIX)_CurrLeg.EndFIX;
			_PrevLeg = _Legs[_Legs.Count - 1];

			//FN--;
			string sNumP = FN.ToString();

			if (FN < 10)
				sNumP = '0' + sNumP;
			sNumP = "TP" + sNumP;

			_CurrFIX.Name = sNumP;
			_PrevSumMinLegLenght -= _PrevLeg.MinLegLength;

			_CourseOutConvertionPoint = (Point)_CurrFIX.PrjPt.Clone();
			_CourseInConvertionPoint = (Point)_CurrFIX.GeoPt.Clone();

			_AltitudeRange.Min = _CurrFIX.NomLineAltitude;
			_BankAngle = _CurrFIX.BankAngle;
			//_CurrLeg.Gradient = Gradient;
			//_CurrLeg.IAS = IAS;

			if (_CurrFIX.IsDFTarget)
				_PathAndTermination = CodeSegmentPath.DF;
			else
				_PathAndTermination = CodeSegmentPath.CF;

			if (_CurrFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _ReferenceFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _ReferenceFIX.ConstructTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_ReferenceFIX.PrjPt, _ReferenceFIX.EntryDirection, 0.0, r * fTurnSide);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, _CurrFIX.EntryDirection, 0.0, -r * fTurnSide);

				_DistanceValue = ARANFunctions.ReturnDistanceInMeters(ptFrom, _CurrFIX.PrjPt);
			}
			else
				_DistanceValue = ARANFunctions.ReturnDistanceInMeters(_ReferenceFIX.PrjPt, _CurrFIX.PrjPt);

			if (Closed)
				return true;

			//CalcMaxAltitude(gRADIENT);
			//UpdateCurrFIX();
			//InitNewPoint(_CurrFIX);
			//============================================================================================================================
			double min, max;
			if (_PathAndTermination == CodeSegmentPath.DF)
			{
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
			}
			else
			{
				_plannedTurnLimits.Max = GlobalVars.f120;
				min = ARANMath.Modulus(_ReferenceFIX.EntryDirection - _plannedTurnAngle, ARANMath.C_2xPI);	//GlobalVars.f120
				max = ARANMath.Modulus(_ReferenceFIX.EntryDirection + _plannedTurnAngle, ARANMath.C_2xPI);	//GlobalVars.f120
			}

			_OutDirRange.Min = min;
			_OutDirRange.Max = max;

            double NewCourse = _OutDirRange.CheckValue(_CurrFIX.EntryDirection);
            _ReferenceFIX.OutDirection = NewCourse;
            _ReferenceFIX.TurnDirection = _turnDirection;

            FillCourseSgfPoints();		//??????

            ChangeCourse(NewCourse);		//OutDirection = _CurrFIX.EntryDirection;

			return true;
		}

		public bool Add(bool Close = false)
		{
			Boolean bExisting = false;

			if (_DistanceIndex >= 0)
				bExisting = true;

			//_CurrLeg.Gradient = Gradient;
			//_UI.DrawMultiPolygon(PrevLeg.PrimaryArea, 0, eFillStyle.sfsCross);
			//ProcessMessages(true);

			if (!bExisting && !CheckName(_CurrFIX.Name))
			{
				MessageBox.Show("Invalid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return false;
			}

			_PrevSumMinLegLenght = _SumMinLegLenght;
			_SumMinLegLenght += _CurrLeg.MinLegLength;

			_CourseOutConvertionPoint = (Point)_CurrFIX.PrjPt.Clone();
			_CourseInConvertionPoint = (Point)_CurrFIX.GeoPt.Clone();

			_AltitudeRange.Min = _CurrFIX.NomLineAltitude;

			CalcMaxAltitude(_GradientRange.Max);

			_CurrLeg.Gradient = Gradient;
			_CurrLeg.IAS = IAS;

			_CurrLeg.Obstacles = _CurrentObstacleList;
			_CurrLeg.Active = true;
			//_CurrLeg.transfersCount = _transferCounter;
			_Legs.Add(_CurrLeg);
			_PrevLeg = _CurrLeg;

			//_prevTransitions = _transferCounter;

			if (!Close)
				InitNewPoint(_CurrFIX);

			return true;
		}

		void GetOstacles()
		{
			double MOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_FinalMOC].Value;

			if (_Legs.Count == 1)
			{
				//_SumMinLegLenght = 0.0;
				if (_PrevLeg.EndFIX.FlyMode == eFlyMode.Flyby && ARANMath.RadToDeg(_PrevLeg.EndFIX.TurnAngle) <= 15)
					MOC = GlobalVars.constants.Pansops[ePANSOPSData.arMA_InterMOC].Value;
			}

			int inx = Functions.GetLegAreaObstacles(GlobalVars.ObstacleList, out _CurrentObstacleList, _CurrLeg, _SumMinLegLenght,
							_RWY.pPtPrj[eRWY.ptTHR].Z, _CurrLeg.Altitude, MOC, _MACG);

			if (inx >= 0)
				_CurrentDetObs = _CurrentObstacleList.Parts[inx];
			else
				_CurrentDetObs = new ObstacleData ();
			_CurrLeg.DetObstacle_2 = _CurrentDetObs;
		}

		void InitNewPoint(WayPoint PrevWPT)
		{
			string sNumP;

			do
			{
				sNumP = FN.ToString();
				if (FN < 10)
					sNumP = '0' + sNumP;
				sNumP = "TP" + sNumP;

				FN++;
			} while (!CheckName(sNumP) || (_CurrFIX != null && _CurrFIX.Name == sNumP));

			//if (FN > 1)
			//{
				//Prev.Role = eFIXRole.TP_;
				//Prev.RefreshGraphics();
			//}
			//WayPoint ReferenceFIX = Prev;
			_CurrFIX = new FIX(eFIXRole.TP_, _aranEnvironment);

			_CurrFIX.FlightPhase = PrevWPT.FlightPhase;
			_CurrFIX.PBNType = PrevWPT.PBNType;

			if (PrevWPT.FlyMode == eFlyMode.Atheight)
				_CurrFIX.FlyMode = eFlyMode.Flyby;
			else
				_CurrFIX.FlyMode = PrevWPT.FlyMode;

			_CurrFIX.SensorType = PrevWPT.SensorType;
			_CurrFIX.Role = PrevWPT.Role;

			_CurrFIX.BankAngle = PrevWPT.BankAngle;
			_BankAngle = PrevWPT.BankAngle;

			_CurrFIX.IAS = PrevWPT.IAS;
			_CurrFIX.ISAtC = PrevWPT.ISAtC;
			_CurrFIX.NomLineAltitude = PrevWPT.NomLineAltitude;

			_CurrFIX.OutDirection = _CurrFIX.EntryDirection = PrevWPT.OutDirection = PrevWPT.EntryDirection;
			_CurrFIX.Name = sNumP;
			//=================================================================================

			_LegPoints.Add(PrevWPT);

			_CurrLeg = new LegApch(PrevWPT, _CurrFIX, _aranEnvironment, _PrevLeg);

			//_CurrLeg.transferedOver56 = _PrevLeg.transferedOver56;
			//_CurrLeg.Active = false;
			Gradient = _GradientRange.Max;

			//_CurrLeg.Gradient = Gradient;
			_CurrLeg.IAS = IAS;
			//_CurrLeg.RefreshGraphics();

			double min, max;

			if (_PathAndTermination == CodeSegmentPath.DF)
			{
				_plannedTurnLimits.Max = GlobalVars.f270;
				min = ARANMath.Modulus(PrevWPT.EntryDirection - GlobalVars.f270, ARANMath.C_2xPI);
				max = ARANMath.Modulus(PrevWPT.EntryDirection + GlobalVars.f270, ARANMath.C_2xPI);
			}
			else
			{
				_plannedTurnLimits.Max = GlobalVars.f120;
				min = ARANMath.Modulus(PrevWPT.EntryDirection - _plannedTurnAngle, ARANMath.C_2xPI);		//GlobalVars.f120
				max = ARANMath.Modulus(PrevWPT.EntryDirection + _plannedTurnAngle, ARANMath.C_2xPI);		//GlobalVars.f120
			}

			_OutDirRange.Min = min;
			_OutDirRange.Max = max;

			if (OnInitNewFIX != null)
				OnInitNewFIX(this, _ReferenceFIX, PrevWPT);

			//=================================================================================
			_ReferenceFIX = PrevWPT;

			UpdateCurrFIX();
			FillCourseSgfPoints();		//??????
		}

		void FillCourseSgfPoints()
		{
			_CourseSgfPoint.Clear();

			if (_PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = -(int)(_ReferenceFIX.TurnDirection);
				double r = _ReferenceFIX.ConstructTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_ReferenceFIX.PrjPt, _ReferenceFIX.EntryDirection, 0, -r * fTurnDirection);

				//_aranEnvironment.Graphics.DrawPointWithText(ptCenter, -1, "Center");
				//Application.DoEvents();
				//Leg.ProcessMessages(true);

				for (int i = 0; i < _SignificantCollection.Count; i++)
				{
					WayPoint SigPoint = _SignificantCollection[i];
					double dX = SigPoint.PrjPt.X - ptCenter.X;
					double dY = SigPoint.PrjPt.Y - ptCenter.Y;

					double dirDest = Math.Atan2(dY, dX);
					double distDest = ARANMath.Hypot(dX, dY);

					double TurnAngle = (_ReferenceFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);
					double nomDir = ARANMath.Modulus(_ReferenceFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);

					double MinStabPrev, MinStabCurr;
					MinStabPrev = _ReferenceFIX.CalcConstructionFromMinStablizationDistance(TurnAngle, true);
					MinStabCurr = _CurrFIX.CalcConstructionInToMinStablizationDistance(_plannedTurnAngle);
					double mindist = MinStabPrev + MinStabCurr;

					if (_OutDirRange.CheckValue(nomDir) == nomDir)
						if (distDest > r && distDest >= mindist && distDest <= _DistanceRange.Max)
							_CourseSgfPoint.Add(SigPoint);
				}
			}
			else for (int i = 0; i < _SignificantCollection.Count; i++)
				{
					WayPoint SigPoint = _SignificantCollection[i];
					double dX = SigPoint.PrjPt.X - _ReferenceFIX.PrjPt.X;
					double dY = SigPoint.PrjPt.Y - _ReferenceFIX.PrjPt.Y;
					double distDest = ARANMath.Hypot(dX, dY);

					double nomDir = ARANMath.Modulus(Math.Atan2(dY, dX), ARANMath.C_2xPI);
					double TurnAngle = ARANMath.Modulus((nomDir - _ReferenceFIX.EntryDirection) * ((int)ARANMath.SideFrom2Angle(nomDir, _ReferenceFIX.EntryDirection)), 2 * Math.PI);

					//_aranEnvironment.Graphics.DrawPointWithText(SigPoint.PrjPt, -1, "DD100");
					//_aranEnvironment.Graphics.DrawPointWithText(_ReferenceFIX.PrjPt, -1, "ReferenceFIX");
					//Leg.ProcessMessages();

					double MinStabPrev, MinStabCurr;
					MinStabPrev = _ReferenceFIX.CalcConstructionFromMinStablizationDistance(TurnAngle);
					MinStabCurr = _CurrFIX.CalcConstructionInToMinStablizationDistance(_plannedTurnAngle);
					double mindist = MinStabPrev + MinStabCurr;

					if (distDest >= mindist && distDest <= _DistanceRange.Max)
					{
						if (_OutDirRange.CheckValue(nomDir) == nomDir)
							_CourseSgfPoint.Add(SigPoint);
					}
				}

			if (OnUpdateDirList != null)
				OnUpdateDirList(this, new EventArgs());
		}

		void FillDistanceSgfPoints()
		{
			double MaxDist = _DistanceRange.Max;
			double MinDist = _DistanceRange.Min;

			Point LocalSigPoint, Local1, ptFrom;
			_DistanceSgfPoint.Clear();

			if (_CurrFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _ReferenceFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _ReferenceFIX.ConstructTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_ReferenceFIX.PrjPt, _ReferenceFIX.EntryDirection, 0.0, r * fTurnSide);
				ptFrom = ARANFunctions.LocalToPrj(ptCnt, _CurrFIX.EntryDirection, 0.0, -r * fTurnSide);
				//_aranEnvironment.Graphics.DrawPointWithText(ptCnt, -1, "ptCnt");
				//Leg.ProcessMessages();
				//MinDist = Math.Max(r, _DistanceRange.Min);
			}
			else
				ptFrom = _ReferenceFIX.PrjPt;

			//_aranEnvironment.Graphics.DrawPointWithText(ptFrom, -1,"ptFrom");
			//Leg.ProcessMessages();

			foreach (WayPoint SigPoint in _CourseSgfPoint)
			{
				LocalSigPoint = ARANFunctions.PrjToLocal(_ReferenceFIX.PrjPt, _CurrFIX.EntryDirection, SigPoint.PrjPt);
				Local1 = ARANFunctions.PrjToLocal(ptFrom, _CurrFIX.EntryDirection, SigPoint.PrjPt);

				if (LocalSigPoint.X > 0 && Math.Abs(Local1.Y) <= 0.25 * _ReferenceFIX.XTT)
					if (LocalSigPoint.X >= MinDist && LocalSigPoint.X <= MaxDist)
						_DistanceSgfPoint.Add(SigPoint);
			}

			if (OnUpdateDistList != null)
				OnUpdateDistList(this, new EventArgs());
		}

		double CalcMaxAltitude(double grd)
		{
			_AltitudeRange.Max = Math.Min(_ReferenceFIX.NomLineAltitude + grd * _DistanceValue, MaxAltitude);
			return _AltitudeRange.Max;

			double MinStabFrom = _ReferenceFIX.CalcConstructionFromMinStablizationDistance(_ReferenceFIX.TurnAngle, _PathAndTermination == CodeSegmentPath.DF);
			if (_CurrFIX.FlyMode != eFlyMode.Flyby)
			{
				double MinStabIn = _CurrFIX.CalcConstructionInToMinStablizationDistance(_plannedTurnAngle);
				double minDist = MinStabIn + MinStabFrom;

				_AltitudeRange.Max = Math.Min(_ReferenceFIX.NomLineAltitude + grd * minDist, MaxAltitude);
				return _AltitudeRange.Max;
			}

			double TurnAngle = _plannedTurnAngle;
			if (ARANMath.RadToDeg(_plannedTurnAngle) < 50.0)
				TurnAngle = ARANMath.DegToRad(50.0);

			double kTurn = Math.Tan(0.5 * TurnAngle);
			double MaxAlt, mindistOld = _DistanceRange.Min;

			int i = 100;

			do
			{
				MaxAlt = Math.Min(_AltitudeRange.Min + grd * mindistOld, MaxAltitude);
				double RTurn = ARANMath.BankToRadiusForRnav(_CurrFIX.BankAngle, _CurrFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);
				double TAS = ARANMath.IASToTASForRnav(_CurrFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);

				double MinStabIn = Math.Max(RTurn * kTurn + constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value * TAS, _CurrFIX.ATT);

				double minDist = MinStabFrom + MinStabIn;
				double delta = minDist - mindistOld;

				if (Math.Abs(delta) < ARANMath.EpsilonDistance)
					break;

				mindistOld = minDist;			// +delta;
				//if (MaxAlt < MaxAltitude)		mindistOld += delta;

			} while (--i >= 0);

			if (i < 0)
				throw new Exception("IFA ERROR: Invalid function argument!");

			_AltitudeRange.Max = MaxAlt;//Math.Min(MaxAlt, MaxAltitude);
			return MaxAlt;
		}

		double CalcMinDistance(double grd)
		{
			double MinStabFrom = _ReferenceFIX.CalcConstructionFromMinStablizationDistance(_ReferenceFIX.TurnAngle, _PathAndTermination == CodeSegmentPath.DF);

			if (_CurrFIX.FlyMode != eFlyMode.Flyby)
			{
				double MinStabIn = _CurrFIX.CalcConstructionInToMinStablizationDistance(_plannedTurnAngle);
				_DistanceRange.Min = MinStabIn + MinStabFrom;
				return _DistanceRange.Min;
			}


			double TurnAngle = _plannedTurnAngle;
			if (ARANMath.RadToDeg(_plannedTurnAngle) < 50.0)
				TurnAngle = ARANMath.DegToRad(50.0);

			double kTurn = Math.Tan(0.5 * TurnAngle);
			double MaxAlt, mindist, mindistOld = _DistanceRange.Min;

			int i = 100;

			do
			{
				MaxAlt = Math.Min(_ReferenceFIX.NomLineAltitude + grd * mindistOld, MaxAltitude);

				double RTurn = ARANMath.BankToRadiusForRnav(_CurrFIX.BankAngle, _CurrFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);
				double TAS = ARANMath.IASToTASForRnav(_CurrFIX.IAS, MaxAlt, constants.Pansops[ePANSOPSData.ISA].Value);
				double MinStabIn = Math.Max(RTurn * kTurn + constants.Pansops[ePANSOPSData.rnvFlyByTechTol].Value * TAS, _CurrFIX.ATT);

				mindist = MinStabIn + MinStabFrom;
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

			_DistanceRange.Min = mindistOld;
			return _DistanceRange.Min;
		}

		bool ChangeDistance(double newVal)
		{
			if (newVal < _DistanceRange.Min)
				newVal = _DistanceRange.Min;

			if (newVal > _DistanceRange.Max)
				newVal = _DistanceRange.Max;

			if (Math.Abs(_DistanceValue - newVal) > ARANMath.EpsilonDistance)
			{
				_DistanceValue = newVal;
				double CurrAltitue = _CurrFIX.NomLineAltitude;

				_GradientRange.Max = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
				CalcMaxAltitude(_GradientRange.Max);

				if (_AltitudeRange.Max == MaxAltitude)
					_GradientRange.Max = (_AltitudeRange.Max - _AltitudeRange.Min) / _DistanceValue;

				if (_CurrFIX.NomLineAltitude > _AltitudeRange.Max)
					_CurrFIX.NomLineAltitude = _AltitudeRange.Max;

				Gradient = (_CurrFIX.NomLineAltitude - _AltitudeRange.Min) / _DistanceValue;

				if (OnDistanceChanged != null)
					OnDistanceChanged(this, _DistanceValue);

				if (CurrAltitue != _CurrFIX.NomLineAltitude && OnAltitudeChanged != null)
					OnAltitudeChanged(this, _CurrFIX.NomLineAltitude);

				return true;
			}

			return false;
		}

		void ChangeCourse(double newVal)
		{
			double OldTurnAngle = _ReferenceFIX.TurnAngle;

			_CurrFIX.EntryDirection = newVal;
			_ReferenceFIX.OutDirection = newVal;

			if (!_CurrFIX.IsDFTarget)
			{
				double fAngle = ARANMath.Modulus(_ReferenceFIX.EntryDirection - _CurrFIX.EntryDirection, ARANMath.C_2xPI);
				if (Math.Abs(fAngle) < ARANMath.EpsilonRadian)
					_ReferenceFIX.TurnDirection = TurnDirection.NONE;
				else if (fAngle < Math.PI)
					_ReferenceFIX.TurnDirection = TurnDirection.CW;
				else
					_ReferenceFIX.TurnDirection = TurnDirection.CCW;
			}

			if (_updateEnabled)
			{
				UpdateFixParams(false);
				UpdateCurrFIX();

				if (OnDirectionChanged != null)
					OnDirectionChanged(this, newVal);

				FillDistanceSgfPoints();
			}
		}

		//==============================================================================
		bool UpdateFixParams(bool updateCourseList = true)		//updateDistanceList
		{
			bool result = false;
			double minDist, OldMinDist = _DistanceRange.Min;

			minDist = CalcMinDistance(Gradient);

			if (Math.Abs(OldMinDist - _DistanceRange.Min) > ARANMath.EpsilonDistance  )
			{
				FillCourseSgfPoints();

				if (_DistanceValue < minDist)
					result = ChangeDistance(minDist);

				if (updateCourseList)
					FillDistanceSgfPoints();
			}

			return result;
		}

		void UpdateCurrFIX()
		{
			Point FIXPoint, ptInOutBase, ptOutOutBase;

			if (_CurrFIX.IsDFTarget)
			{
				TurnDirection TurnDir = _ReferenceFIX.EffectiveTurnDirection;
				double fTurnSide = (int)TurnDir;
				double r = _ReferenceFIX.ConstructTurnRadius;
				Point ptCnt = ARANFunctions.LocalToPrj(_ReferenceFIX.PrjPt, _ReferenceFIX.EntryDirection, 0.0, r * fTurnSide);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCnt, _CurrFIX.EntryDirection, 0.0, -r * fTurnSide);

				if (_PathAndTermination == CodeSegmentPath.DF)
					FIXPoint = ARANFunctions.LocalToPrj(ptFrom, _CurrFIX.EntryDirection, _DistanceValue, 0.0);
				else
					FIXPoint = ARANFunctions.CircleVectorIntersect(_ReferenceFIX.PrjPt, _DistanceValue, ptFrom, _CurrFIX.EntryDirection);

				_CurrFIX.PrjPt = FIXPoint;

				if (TurnDir == TurnDirection.CW)
				{
					_CurrFIX.OutDirection = _CurrFIX.EntryDirection + _plannedTurnLimits.Max;	// ARANMath.C_PI
					_CurrFIX.EntryDirection_L = _ReferenceFIX.CalcDFOuterDirection(_CurrFIX, out ptOutOutBase);
					_CurrFIX.EntryDirection_R = _ReferenceFIX.CalcDFInnerDirection(_CurrFIX, out ptInOutBase, _PrevLeg);
				}
				else
				{
					_CurrFIX.OutDirection = _CurrFIX.EntryDirection - _plannedTurnLimits.Max;	// ARANMath.C_PI
					_CurrFIX.EntryDirection_R = _ReferenceFIX.CalcDFOuterDirection(_CurrFIX, out ptOutOutBase);
					_CurrFIX.EntryDirection_L = _ReferenceFIX.CalcDFInnerDirection(_CurrFIX, out ptInOutBase, _PrevLeg);
				}
			}
			else
			{
				FIXPoint = ARANFunctions.LocalToPrj(_ReferenceFIX.PrjPt, _CurrFIX.EntryDirection, _DistanceValue, 0.0);
				_CurrFIX.PrjPt = FIXPoint;
				if (_CurrFIX.FlyMode == eFlyMode.Flyover)
					_CurrFIX.OutDirection = _CurrFIX.EntryDirection + _plannedTurnLimits.Max;	// ARANMath.C_PI
				else
					_CurrFIX.OutDirection = _CurrFIX.EntryDirection;	// ARANMath.C_PI
			}

			_CurrFIX.BankAngle = _BankAngle;
			//double minDist = CalcMinDistance(Gradient);

			double fDist = ARANMath.Hypot(FIXPoint.X - _Adhp.pPtPrj.X, FIXPoint.Y - _Adhp.pPtPrj.Y);

			//if (_ReferenceFIX.FlightPhase >= eFlightPhase.FAFApch)			//SIDGE28

			if (_ReferenceFIX.FlightPhase == eFlightPhase.FAFApch)  //|| _ReferenceFIX.FlightPhase == eFlightPhase.IIAP	//|| _ReferenceFIX.FlightPhase == eFlightPhase.MApGE28
			{
				if (fDist >= PANSOPSConstantList.PBNTerminalTriggerDistance)
				{
					//_CurrFIX.PBNType = Prev.PBNType;

					//if (_ReferenceFIX.FlightPhase < eFlightPhase.MApGE28)
						_ReferenceFIX.FlightPhase = eFlightPhase.STARGE56;

					if (_CurrFIX.PBNType == ePBNClass.RNP_APCH)
						_CurrFIX.PBNType = ePBNClass.RNAV1;

					_CurrLeg.transferedOver56 = true;
				}
				else if (!_PrevLeg.transferedOver56)
				{
					if (fDist <= PANSOPSConstantList.PBNInternalTriggerDistance)
						_CurrFIX.FlightPhase = eFlightPhase.MApLT28;
					else
						_CurrFIX.FlightPhase = eFlightPhase.MApGE28;

					_CurrFIX.PBNType = ePBNClass.RNP_APCH;
					_CurrLeg.transferedOver56 = false;
				}
			}

			//_CurrFIX.BankAngle = _BankAngle;

			if (_Legs.Count == 1)
			{
				_PrevLeg.EndFIX.OutDirection = _CurrFIX.EntryDirection;
				_PrevLeg.EndFIX.TurnDirection = _ReferenceFIX.EffectiveTurnDirection;// ARANMath.SideFrom2Angle(_CurrFIX.EntryDirection, _PrevLeg.EndFIX.EntryDirection);
			}

			_CurrLeg.StartFIX.Assign(_PrevLeg.EndFIX);
			_CurrLeg.EndFIX.Assign(_CurrFIX);
			_CurrLeg.CreateGeometry(_PrevLeg, _Adhp);

			GeometryOperators go = new GeometryOperators();

			double hKK = _PrevLeg.Altitude + _PrevLeg.MinLegLength * _MACG;

			if (_Legs.Count == 1)
			{
				//_SumMinLegLenght = go.GetDistance(_PrevLeg.StartFIX.PrjPt, _CurrLeg.KKLine);
				//_CurrLeg.MinLegLength = _SumMinLegLenght;
				//_CurrLeg.KKLine = _PrevLeg.FullAssesmentArea;// ARANFunctions.PolygonToPolyLine(_PrevLeg.FullAssesmentArea[0])[0];

				_CurrLeg.MinLegLength = go.GetDistance(_PrevLeg.KKLine, _CurrLeg.KKLine);
				//_CurrLeg.MinLegLength = 0.0;
				double socDist = go.GetDistance(_ptSOC, _CurrLeg.KKLine);
				hKK = _ptSOC.Z + socDist * _MACG - _RWY.pPtPrj[eRWY.ptTHR].Z;
			}
			else
				_CurrLeg.MinLegLength = go.GetDistance(_PrevLeg.KKLine, _CurrLeg.KKLine);

			_CurrLeg.Altitude = hKK;

			_PrevLeg.RefreshGraphics();
			_CurrLeg.RefreshGraphics();

			GetOstacles();

			if (OnFIXUpdated != null)
				OnFIXUpdated(this, _CurrFIX);
		}

	}
}
