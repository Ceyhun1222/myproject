using System;
using System.Linq;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Geometries.Operators;
using Aran.AranEnvironment.Symbols;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class Straight
	{
		protected static Aran.PANDA.Constants.Constants constants = null;
		protected AranEnvironment.IAranEnvironment _aranEnvironment;
		protected MultiLineString _p120Line;

		protected bool _bTurnBeforeDer;
		protected double _termDistance, _nomDistance, _depDir, _CLDir, _trackAdjust, _PDG, _MOCLimit;
		protected double _additionR;

		protected RWYType _RWY;
		protected ePBNClass _pbnType;
		protected eSensorType _sensor;
		protected Point _ptCenter, _ptArea1End;
		protected MultiPolygon _circle;

		protected ObstacleContainer _obstacleList;

		protected ADHPType _ADHP;
		//protected ObstacleType _detObs;
		protected ObstacleContainer _detObs;
		protected WayPoint _WPT_der, _WPT_15NM, _WPT_30NM;

		protected double _drPDGMax;
		protected int _idPDGMax;

		protected MultiPolygon _fullAreaPolygon, _primatyAreaPolygon;

		public event DoubleValEventHandler PDGChanged;
		public event DoubleValEventHandler NomPDGDistChanged;

		protected bool  _updated;
		protected int _updateEnabled;

		public bool UpdateEnabled
		{
			set
			{
				if (value)
					_updateEnabled++;
				else
					_updateEnabled--;

				if (_updateEnabled > 0)
				{
					_PDG = 0.0;
					ApplyChanges();
				}
			}

			get { return _updateEnabled > 0; }
		}

		public ObstacleContainer DetObs
		{
			get { return _detObs; }
		}

		public ObstacleContainer InnerObstacleList
		{
			get { return _obstacleList; }
		}

		public bool TurnBeforeDer
		{
			get { return _bTurnBeforeDer; }

			set
			{
				if (_bTurnBeforeDer != value)
				{
					_bTurnBeforeDer = value;
					_updated = false;

					if (_updateEnabled > 0)
					{
						_PDG = 0.0;		// constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
						ApplyChanges();
					}
				}
			}
		}

		protected double _IAS;
		public double IAS
		{
			get { return _IAS; }

			set
			{
				_IAS = value;
				_WPT_der.IAS = _IAS;
				_WPT_15NM.IAS = _IAS;
				_WPT_30NM.IAS = _IAS;
				Leg.IAS = _IAS;

				ApplayWPTParams();
			}
		}

		protected double _bankAngle;
		public double BankAngle
		{
			get { return _bankAngle; }
	
			set
			{
				_bankAngle = value;
				Leg.EndFIX.BankAngle = _bankAngle;
				ApplayWPTParams();
			}
		}

		private double _constructGradient;
		public double ConstructionGradient
		{
			get { return _constructGradient; }

			set
			{
				_constructGradient = value;
				Leg.EndFIX.ConstructionGradient = value;
			}
		}

		private double _constructAltitude;
		public double ConstructAltitude
		{
			get { return _constructAltitude; }

			set
			{
				_constructAltitude = value;
				Leg.EndFIX.ConstructAltitude = value;
				//if (terminationType == TerminationType.AtHeight)
				//	Leg.EndFIX.NomLineAltitude = value;
				//ConstructAltitudeChanged();
			}
		}

		protected double _nomLineGradient;
		public virtual double NomLineGradient
		{
			get { return _nomLineGradient; }
	
			set
			{
				_nomLineGradient = value;
				Leg.EndFIX.NomLineGradient = value;
			}
		}

		private double _appliedGradient;
		public double AppliedGradient
		{
			get { return _appliedGradient; }

			set
			{
				_appliedGradient = value;
				Leg.EndFIX.AppliedGradient = value;
			}
		}

		public WayPoint WPT_TP
		{
			get { return Leg.EndFIX; }
		}

		double _plannedMaxTurnAngle;
		public double PlannedMaxTurnAngle
		{
			get { return _plannedMaxTurnAngle; }

			set
			{
				_plannedMaxTurnAngle = value;
				Leg.EndFIX.OutDirection = Leg.EndFIX.EntryDirection + value;
				ApplayWPTParams();
			}
		}

		protected void ApplayWPTParams()
		{
			Leg.StartFIX.ReCreateArea();
			Leg.EndFIX.ReCreateArea();
			Leg.EndFIX.RefreshGraphics();

			_updated = false;
			if (_updateEnabled > 0)
			{
				_PDG = 0.0;
				ApplyChanges();
			}
		}

		public double InitialTrack
		{
			get { return _depDir; }
	
			set
			{
				if (_depDir != value)
				{
					_depDir = value;
					_trackAdjust = ARANMath.SubtractAngles(_CLDir, _depDir);

					_WPT_15NM.PrjPt = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
					_WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, -_WPT_15NM.ATT, 0);

					_WPT_30NM.PrjPt = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
					_WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, -_WPT_30NM.ATT, 0);

					Leg.StartFIX.EntryDirection = _depDir;
					Leg.StartFIX.OutDirection = _depDir;

					Leg.EndFIX.EntryDirection = _depDir;
					Leg.EndFIX.OutDirection = _depDir;

					//leg.EndFIX.PrjPt = ARANFunctions.LocalToPrj(_ptCenter, _DepDir, _TermDistance + Math.Abs(_additionR), 0.0);
					Leg.EndFIX.PrjPt = ARANFunctions.CircleVectorIntersect(_ptCenter, _termDistance + Math.Abs(_additionR), _RWY.pPtPrj[eRWY.ptDER], _depDir);

					double dist = ARANFunctions.ReturnDistanceInMeters(_ADHP.pPtPrj, Leg.EndFIX.PrjPt);
					if (dist < PANSOPSConstantList.PBNInternalTriggerDistance)
						Leg.EndFIX.FlightPhase = eFlightPhase.SID;
					else if (dist < PANSOPSConstantList.PBNTerminalTriggerDistance)
						Leg.EndFIX.FlightPhase = eFlightPhase.SIDGE28;
					else
						Leg.EndFIX.FlightPhase = eFlightPhase.SIDGE56;

					_updated = false;

					if (_updateEnabled > 0)
					{
						_PDG = 0;					// constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
						ApplyChanges();
					}
				}
			}
		}

		virtual public ePBNClass PBNtype
		{
			get { return _pbnType; }

			set
			{
				if (_pbnType != value)
				{
					_pbnType = value;

					_WPT_der.PBNType = _pbnType;
					_WPT_15NM.PBNType = _pbnType;
					_WPT_30NM.PBNType = _pbnType;

					_WPT_15NM.PrjPt = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
					_WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, -_WPT_15NM.ATT, 0);

					_WPT_30NM.PrjPt = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
					_WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, -_WPT_30NM.ATT, 0);

					Leg.StartFIX.PBNType = _pbnType;
					Leg.EndFIX.PBNType = _pbnType;

					_updated = false;
					if (_updateEnabled > 0)
					{
						_PDG = 0;// constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
						ApplyChanges();
					}
				}
			}
		}

		virtual public eSensorType sensor
		{
			get { return _sensor; }

			set
			{
				if (_sensor != value)
				{
					_sensor = value;

					_WPT_der.SensorType = _sensor;
					_WPT_15NM.SensorType = _sensor;
					_WPT_30NM.SensorType = _sensor;

					_WPT_15NM.PrjPt = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
					_WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, -_WPT_15NM.ATT, 0);

					_WPT_30NM.PrjPt = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
					_WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, -_WPT_30NM.ATT, 0);

					Leg.StartFIX.SensorType = _sensor;
					Leg.EndFIX.SensorType = _sensor;

					_updated = false;

					if (_updateEnabled > 0)
					{
						_PDG = 0.0;// constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
						
						ApplyChanges();
					}
				}
			}
		}

		public double MOCLimit
		{
			get { return _MOCLimit; }
		}

		public double PDG
		{
			get { return _PDG; }

			set
			{
				if (_PDG != value)
				{
					_PDG = value;
					_updated = false;
					Leg.EndFIX.ConstructionGradient = value;
					Leg.Gradient = value;

					//leg.EndFIX.TurnAltitude = leg.StartFIX.TurnAltitude + leg.EndFIX.Gradient * _TermDistance;

					if (_updateEnabled > 0)
						ApplyChanges();

					if (PDGChanged != null)
						PDGChanged(this, _PDG);
				}
			}
		}

		public double MinPDG
		{
			get;
			private set;
		}

		public LegDep Leg
		{
			get;
			protected set;
		}

		protected virtual void setNewDistance(double newDistance)
		{
			_termDistance = newDistance;
			//_nomDistance = newDistance;

			Polygon pCirclePolygon = new Polygon();
			Ring pCircleRing = ARANFunctions.CreateCirclePrj(_ptCenter, _termDistance + Math.Abs(_additionR));

			_circle = new MultiPolygon();
			pCirclePolygon.ExteriorRing = pCircleRing;
			_circle.Add(pCirclePolygon);

			Leg.EndFIX.PrjPt = ARANFunctions.LocalToPrj(_ptCenter, _depDir, _termDistance + Math.Abs(_additionR), 0.0);

			double BankAngle = Leg.EndFIX.BankAngle;
			double dist = ARANFunctions.ReturnDistanceInMeters(_ADHP.pPtPrj, Leg.EndFIX.PrjPt);
			if (dist < PANSOPSConstantList.PBNInternalTriggerDistance)
				Leg.EndFIX.FlightPhase = eFlightPhase.SID;
			else if (dist < PANSOPSConstantList.PBNTerminalTriggerDistance)
				Leg.EndFIX.FlightPhase = eFlightPhase.SIDGE28;
			else
				Leg.EndFIX.FlightPhase = eFlightPhase.SIDGE56;

			Leg.EndFIX.BankAngle = BankAngle;
		}

		public double TermDistance
		{
			get { return _termDistance; }

			set
			{
				if (Math.Abs( _termDistance - value) > ARANMath.EpsilonDistance)
				{
					//_termDistance = value;
					//_nomDistance = value;
					//leg.EndFIX.NomLineAltitude = leg.StartFIX.NomLineAltitude + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + GlobalVars.NomLineGrd * _TermDistance;
					//leg.EndFIX.TurnAltitude = leg.StartFIX.TurnAltitude + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value * _TermDistance;

					setNewDistance(value);
					_updated = false;
					if (_updateEnabled > 0)
					{
						_PDG = 0.0;
						ApplyChanges();
					}
				}
			}
		}

		public double NomPDGDistance
		{
			get { return _drPDGMax; }

			private set
			{
				if (_drPDGMax != value)
				{
					_drPDGMax = value;

					if (NomPDGDistChanged != null)
						NomPDGDistChanged(this, _drPDGMax);
				}
			}
		}

		public double NomPDGHeight
		{
			get;
			private set;
		}

		public RWYType RWY
		{
			get { return _RWY; }
		}

		public ADHPType ADHP
		{
			get { return _ADHP; }
		}

		public AranEnvironment.IAranEnvironment Environment
		{
			get { return _aranEnvironment; }
		}

		public Straight(RWYType Rwy, double initialTrack, double MocLimit, double Radius, bool bTurnBeforeDer, ADHPType Adhp, AranEnvironment.IAranEnvironment aranEnvironment)
		{
			if (constants == null)
				constants = new Aran.PANDA.Constants.Constants();
			_aranEnvironment = aranEnvironment;

			_updateEnabled = 0;
			_updated = true;
			_p120Line = null;

			_pbnType = (ePBNClass)(-1);     //.BASIC_RNP1;	// PbnType;
			_sensor = (eSensorType)(-1);    //.GNSS;			// sensortype;

			_bTurnBeforeDer = bTurnBeforeDer;
			_RWY = Rwy;
			_CLDir = _RWY.pPtPrj[eRWY.ptDER].M;

			_MOCLimit = MocLimit;
			_depDir = initialTrack;
			_trackAdjust = ARANMath.SubtractAngles(_CLDir, _depDir);
			_termDistance = Radius;
			_PDG = constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			_ADHP = Adhp;

			WPT_FIXType fix = new WPT_FIXType();

			fix.Name = null; //"FICTIV";
			fix.pPtPrj = _RWY.pPtPrj[eRWY.ptDER];
			fix.pPtGeo = _RWY.pPtGeo[eRWY.ptDER];
			_WPT_der = new WayPoint(eFIXRole.DEP_ST, fix, _aranEnvironment);
			_WPT_der.FlightPhase = eFlightPhase.SID;

			_WPT_der.EntryDirection = _depDir;
			_WPT_der.OutDirection = _depDir;
			_WPT_der.NomLineAltitude = _WPT_der.ConstructAltitude = _RWY.pPtPrj[eRWY.ptDER].Z + GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;

			fix.pPtPrj = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);

			_WPT_15NM = new WayPoint(eFIXRole.DEP_ST, fix, _aranEnvironment);
			_WPT_15NM.FlightPhase = eFlightPhase.SIDGE28;
			_WPT_15NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, -_WPT_15NM.ATT, 0);
			_WPT_15NM.EntryDirection = _depDir;
			_WPT_15NM.OutDirection = _depDir;
			double dist = ARANFunctions.ReturnDistanceInMeters(_WPT_15NM.PrjPt, _WPT_der.PrjPt);
			_WPT_15NM.ConstructAltitude = _WPT_der.ConstructAltitude + dist * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			//fix.Name = "";
			fix.pPtPrj = ARANFunctions.CircleVectorIntersect(_ADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance, _RWY.pPtPrj[eRWY.ptDER], _depDir);
			_WPT_30NM = new WayPoint(eFIXRole.DEP_ST, fix, _aranEnvironment);
			_WPT_30NM.FlightPhase = eFlightPhase.SIDGE56;
			_WPT_30NM.PrjPt = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, -_WPT_30NM.ATT, 0.0);
			_WPT_30NM.EntryDirection = _depDir;
			_WPT_30NM.OutDirection = _depDir;

			dist = ARANFunctions.ReturnDistanceInMeters(_WPT_15NM.PrjPt, _WPT_30NM.PrjPt);
			_WPT_30NM.ConstructAltitude = _WPT_15NM.ConstructAltitude + dist * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			//WPT_above30NM = new WayPoint(eFIXRole.DEP_ST, fix, _aranEnvironment);
			//WPT_above30NM.PrjPt = ARANFunctions.LocalToPrj(WPT_above30NM.PrjPt, _RWY.pPtPrj[eRWY.PtEnd].M, -WPT_above30NM.ATT, 0.0);
			//WPT_above30NM.FlightPhase = eFlightPhase.SIDGE56;
			_additionR = GlobalVars.constants.Pansops[ePANSOPSData.dpT_Init].Value - _RWY.TODA;
			_ptCenter = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, _additionR);
			_nomLineGradient = GlobalVars.NomLineGrd;
			double radius = _termDistance + Math.Abs(_additionR);

			fix.Name = "";
			fix.pPtPrj = ARANFunctions.LocalToPrj(_ptCenter, _depDir, radius);
			WayPoint EndFIX = new WayPoint(eFIXRole.DEP_ST, fix, _aranEnvironment);

			dist = ARANFunctions.ReturnDistanceInMeters(_ADHP.pPtPrj, fix.pPtPrj);
			if (dist < PANSOPSConstantList.PBNInternalTriggerDistance)
				EndFIX.FlightPhase = eFlightPhase.SID;
			else if (dist < PANSOPSConstantList.PBNTerminalTriggerDistance)
				EndFIX.FlightPhase = eFlightPhase.SIDGE28;
			else
				EndFIX.FlightPhase = eFlightPhase.SIDGE56;


			EndFIX.EntryDirection = _depDir;
			EndFIX.OutDirection = _depDir;
			dist = ARANFunctions.ReturnDistanceInMeters(EndFIX.PrjPt, _WPT_der.PrjPt);

			//EndFIX.ConstructAltitude = fix.pPtPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;
			EndFIX.ConstructAltitude = _WPT_der.ConstructAltitude + dist * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			EndFIX.NomLineAltitude = EndFIX.ConstructAltitude;

			Leg = new LegDep(_WPT_der, EndFIX, aranEnvironment);

			Ring pCircleRing = ARANFunctions.CreateCirclePrj(_ptCenter, radius);
			Polygon pCirclePolygon = new Polygon();

			_circle = new MultiPolygon();
			pCirclePolygon.ExteriorRing = pCircleRing;
			_circle.Add(pCirclePolygon);
		}

		public void ApplyChanges()
		{
			MultiPolygon pFullArea, pPrimArea;

			double newPDG = PDG;
			if (newPDG < constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
				newPDG = constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			CreateDeparturePolygon(newPDG, out _fullAreaPolygon, out _primatyAreaPolygon, out _p120Line);
			createDrawingPolygons(out pFullArea, out pPrimArea);

			Functions.GetStrightAreaObstacles(out _obstacleList, pFullArea, pPrimArea, _RWY, _depDir);
			Functions.SortByDist(ref _obstacleList);

			_detObs = CalcPDGToTop(_obstacleList);

			if (_detObs.Obstacles[0].ID >= 0)
				MinPDG = Math.Min(DetObs.Parts[0].PDG, DetObs.Parts[0].PDGAvoid);
			else
				MinPDG = constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			newPDG = Math.Max(newPDG, MinPDG);

			double tmpPDG = Math.Round(newPDG + 0.0004999, 3);

			if (_PDG != tmpPDG)
			{
				_PDG = tmpPDG;
				PDGChanged?.Invoke(this, _PDG);
			}
			//else
			//{
			CalcObstaclesReqTNAH(_obstacleList, _PDG, DetObs.Parts[0].Index);

			double tmpNomDistance = dPDGMax(_obstacleList, _PDG, out _idPDGMax);
			NomPDGHeight = tmpNomDistance * _PDG + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;
			NomPDGDistance = tmpNomDistance;

			bool DrawFlag = true;

			Leg.CreateNomTrack(null);

			if (DrawFlag)
			{
				CreateDeparturePolygon(_PDG, out _fullAreaPolygon, out _primatyAreaPolygon, out _p120Line);
				//createDrawingPolygons(out pFullArea, out pPrimArea);

				_aranEnvironment.Graphics.SafeDeleteGraphic(GlobalVars.p120LineElem);

				GeometryOperators geometryOperators = new GeometryOperators();
				MultiLineString mls = (MultiLineString)geometryOperators.Intersect(_circle, Leg.NominalTrack);

				if (mls.Length > ARANMath.EpsilonDistance)
					Leg.NominalTrack = mls[0];
				else
					Leg.NominalTrack = new LineString();

				ReDraw();
			}
			//}

			_updated = true;
		}

		double dPDGMax(ObstacleContainer ObsList, double newPDG, out int index)
		{
			index = -1;
			int n = ObsList.Parts.Count();

			if (newPDG == constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
			{
				for (int i = 0; i < n; i++)
				{
					ObsList.Parts[i].NomPDGDist = 0.0;
					ObsList.Parts[i].NomPDGHeight = constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;
				}
				return (constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / newPDG;
			}

			double NomDist = (constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / newPDG;
			double NomHeight = NomDist * newPDG + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;
			double result = NomDist;
			double NomPDG = constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			for (int i = 0; i < n; i++)
			{
				ObsList.Parts[i].NomPDGDist = NomDist;
				ObsList.Parts[i].NomPDGHeight = NomHeight;

				if (!ObsList.Parts[i].Ignored)
				{
					double rH = ObsList.Parts[i].Height + ObsList.Parts[i].MOC - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;
					ObsList.Parts[i].NomPDGDist = (rH - NomPDG * ObsList.Parts[i].Dist) / (newPDG - NomPDG);
					ObsList.Parts[i].NomPDGHeight = ObsList.Parts[i].NomPDGDist * newPDG + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;
					if (ObsList.Parts[i].NomPDGHeight < 0)
						ObsList.Parts[i].NomPDGHeight = constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;

					if (result < ObsList.Parts[i].NomPDGDist)
					{
						result = ObsList.Parts[i].NomPDGDist;
						index = i;
					}
				}
			}
			return result;
		}

		ObstacleContainer CalcPDGToTop(ObstacleContainer ObsList)
		{
			ObstacleContainer result;

			TurnDirection sA = ARANMath.SideFrom2Angle(_depDir, _CLDir);
			double fWd = 0.5 * constants.Pansops[ePANSOPSData.dpNGui_Ar1_Wd].Value;

			int index = -1;
			
			int n = ObsList.Parts.Count();

			double sinDa = Math.Sin(_trackAdjust);

			double lPDG = constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			double nomPDG = lPDG;
			double tan15 = Math.Tan(constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
			double tan15_Tr = Math.Tan(constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value - _trackAdjust);
			double tan15pTr = Math.Tan(constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value + _trackAdjust);

			double f115 = constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value - constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;

			for (int i = 0; i < n; i++)
			{
				Point ptObsCurr = ObsList.Parts[i].pPtPrj;

				if (ObsList.Parts[i].Dist <= 0.0)
				{
					ObsList.Parts[i].MOC = 0.0;
					ObsList.Parts[i].PDGToTop = 0.0;
					ObsList.Parts[i].PDG = 0.0;
					ObsList.Parts[i].Ignored = true;
					//ObsList.Parts[i].IsExcluded = true;
					ObsList.Parts[i].PDGAvoid = 9999.0;
				}
				else
				{
					double tmpMOC = ObsList.Parts[i].Dist * constants.Pansops[ePANSOPSData.dpMOC].Value;

					if (tmpMOC > _MOCLimit)
						tmpMOC = _MOCLimit;

					ObsList.Parts[i].MOC = tmpMOC * ObsList.Parts[i].fSecCoeff;
					ObsList.Parts[i].ReqH = ObsList.Parts[i].MOC + ObsList.Parts[i].Height;

					double rH = ObsList.Parts[i].Height + ObsList.Parts[i].MOC - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;

					ObsList.Parts[i].PDGToTop = (rH - ObsList.Parts[i].MOC) / ObsList.Parts[i].Dist;
					ObsList.Parts[i].PDG = rH / ObsList.Parts[i].Dist;
					//ObsList.Parts[i].Ignored = rH + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value <= constants.Pansops[ePANSOPSData.dpPDG_60].Value;
					ObsList.Parts[i].Ignored = rH + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value <= constants.Pansops[ePANSOPSData.dpPDG_60].Value && ObsList.Parts[i].PDG > nomPDG;

					if (sA == TurnDirection.NONE || ObsList.Parts[i].Ignored || ObsList.Parts[i].PDG <= nomPDG)
						ObsList.Parts[i].PDGAvoid = 9999.0;
					else
					{
						double x, y;

						//ARANFunctions.PrjToLocal(_RWY.pPtPrj[eRWY.PtEnd],   _CLDir,    ptObsCurr, out x, out y);
						ARANFunctions.PrjToLocal(_RWY.pPtPrj[eRWY.ptDER], _depDir, ptObsCurr, out x, out y);

						if (Math.Sign(y) == (int)sA)
							ObsList.Parts[i].PDGAvoid = 9999.0;
						else
						{
							double fTmp, denom;
							//denom = x * tan15_Tr - Math.Abs(y) + fWd;
							denom = x * tan15 + tan15pTr * fWd * sinDa - Math.Abs(y);

							if (denom != 0.0)
								ObsList.Parts[i].PDGAvoid = 9999.0;
							else
							{
								//fTmp = -f115 * (tan15 - tan15_Tr) / denom;
								fTmp = -f115 * (tan15pTr - tan15) / denom;

								if (fTmp < 0.0)
									ObsList.Parts[i].PDGAvoid = 9999.0;
								else
								{
									fTmp = System.Math.Round(fTmp + 0.0004999, 3);

									if (fTmp <= nomPDG)
										ObsList.Parts[i].PDGAvoid = nomPDG;
									else
										ObsList.Parts[i].PDGAvoid = fTmp;
								}
							}
						}
					}

					double obsPDG = Math.Min(ObsList.Parts[i].PDG, ObsList.Parts[i].PDGAvoid);

					if (obsPDG > lPDG && !ObsList.Parts[i].Ignored)
					{
						lPDG = obsPDG;
						index = i;
					}
				}
			}

			result.Obstacles = new Obstacle[1];
			result.Parts = new ObstacleData[1]; 
			if (index < 0)
				result.Obstacles[0].ID = result.Parts[0].Owner = result.Parts[0].Index = -1;
			else
			{
				result.Parts[0] = ObsList.Parts[index];
				result.Obstacles[0] = ObsList.Obstacles[ObsList.Parts[index].Owner];
				result.Parts[0].Index = index;
			}

			return result;
		}

		void CalcObstaclesReqTNAH(ObstacleContainer Obstacles, double newPDG, int obstIndex)
		{
			int n = Obstacles.Parts.Length;
			if (n == 0)
				return;

			double TNIA_MOC_Bound = constants.Pansops[ePANSOPSData.dpObsClr].Value / constants.Pansops[ePANSOPSData.dpMOC].Value;
			double f120 = constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;
			double nomPDG = constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			for (int i = 0; i < n; i++)
			{
				Obstacles.Parts[i].ReqH = Obstacles.Parts[i].MOC + Obstacles.Parts[i].Height;
				Obstacles.Parts[i].IsInteresting = i >= obstIndex;

				if (Obstacles.Parts[i].Dist > TNIA_MOC_Bound)
					Obstacles.Parts[i].ReqTNH = f120;
				else
				{
					double fTmp;

					if (newPDG == nomPDG)
					{
						fTmp = Obstacles.Parts[i].Height + constants.Pansops[ePANSOPSData.dpObsClr].Value;
						if (constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + newPDG * Obstacles.Parts[i].Dist >= fTmp)
							fTmp = f120;
					}
					else
					{
						double X = (constants.Pansops[ePANSOPSData.dpObsClr].Value - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + Obstacles.Parts[i].Height - nomPDG * Obstacles.Parts[i].Dist) / (newPDG - nomPDG);

						if (X >= Obstacles.Parts[i].Dist)
							fTmp = constants.Pansops[ePANSOPSData.dpObsClr].Value + Obstacles.Parts[i].Height;
						else
							fTmp = constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + newPDG * X;
					}

					Obstacles.Parts[i].ReqTNH = Math.Max(fTmp, f120);
				}
			}
		}

		void CreateDeparturePolygon(double newPDG, out MultiPolygon fullArea, out MultiPolygon primArea, out MultiLineString p120Line)
		{
			Point ptrm00 = null, ptr00, ptr01;
			Point ptlm00 = null, ptl00, ptl01;

			Point ptrm10 = null, ptr10;
			Point ptlm10 = null, ptl10;
			Geometry pGeom;

			double dpAr1 = constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;
			double fWd = 0.5 * constants.Pansops[ePANSOPSData.dpNGui_Ar1_Wd].Value;
			double lExpAng, rExpAng;
			//double d0 = constants.Pansops[ePANSOPSData.dpT_Init].Value - _RWY.Length - _RWY.ClearWay;
			double d0 = _additionR;

			double localX, localY;

			if (_bTurnBeforeDer)
			{
				ptrm00 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, d0, -fWd);
				ptlm00 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, d0, fWd);
			}

			ptr00 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, 0.0, -fWd);
			ptl00 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, 0.0, fWd);

			fWd = 0.5 * _WPT_der.SemiWidth;
			ptrm10 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, d0, -fWd);
			ptlm10 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _CLDir, d0, fWd);

			ptr10 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, -fWd);
			ptl10 = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, fWd);

			TurnDirection adjustDirection = ARANMath.SideFrom2Angle(_depDir, _CLDir);

			double fDist = (constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / newPDG;
			double fDir = _depDir;		//= _CLDir;			//= _DepDir;

			_ptArea1End = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], fDir, fDist);

			if (adjustDirection == TurnDirection.CW)
			{ // Right adjust
				pGeom = ARANFunctions.LineLineIntersect(_ptArea1End, fDir - ARANMath.C_PI_2, ptr00, _depDir - constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
				if (pGeom.Type != GeometryType.Point)
					throw new Exception("Invalid geometry type.");
				ptr01 = (Point)pGeom;
				rExpAng = _depDir - constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value;

				pGeom = ARANFunctions.LineLineIntersect(_ptArea1End, fDir - ARANMath.C_PI_2, ptl00, _CLDir + constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
				if (pGeom.Type != GeometryType.Point)
					throw new Exception("Invalid geometry type.");
				ptl01 = (Point)pGeom;

				lExpAng = _depDir + constants.Pansops[ePANSOPSData.dpAr1_IB_TrAdj].Value;
			}
			else if (adjustDirection == TurnDirection.CCW)
			{ // Left adjust
				pGeom = ARANFunctions.LineLineIntersect(_ptArea1End, fDir - ARANMath.C_PI_2, ptr00, _CLDir - constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
				if (pGeom.Type != GeometryType.Point)
					throw new Exception("Invalid geometry type.");
				ptr01 = (Point)pGeom;
				rExpAng = _depDir - constants.Pansops[ePANSOPSData.dpAr1_IB_TrAdj].Value;

				pGeom = ARANFunctions.LineLineIntersect(_ptArea1End, fDir - ARANMath.C_PI_2, ptl00, _depDir + constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
				if (pGeom.Type != GeometryType.Point)
					throw new Exception("Invalid geometry type.");
				ptl01 = (Point)pGeom;
				lExpAng = _depDir + constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value;
			}
			else
			{
				pGeom = ARANFunctions.LineLineIntersect(_ptArea1End, fDir - ARANMath.C_PI_2, ptr00, _CLDir - constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
				if (pGeom.Type != GeometryType.Point)
					throw new Exception("Invalid geometry type.");
				ptr01 = (Point)pGeom;

				pGeom = ARANFunctions.LineLineIntersect(_ptArea1End, fDir - ARANMath.C_PI_2, ptl00, _CLDir + constants.Pansops[ePANSOPSData.dpAr1_OB_TrAdj].Value);
				if (pGeom.Type != GeometryType.Point)
					throw new Exception("Invalid geometry type.");
				ptl01 = (Point)pGeom;

				rExpAng = _depDir - constants.Pansops[ePANSOPSData.dpAr1_IB_TrAdj].Value;
				lExpAng = _depDir + constants.Pansops[ePANSOPSData.dpAr1_IB_TrAdj].Value;
			}

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			Point ptBr, ptr02, ptr03, ptr04, ptr05;
			Point ptBl, ptl02, ptl03, ptl04, ptl05;

			Point ptr13, ptr14, ptr15;
			Point ptl13, ptl14, ptl15;

			ptBr = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, -_WPT_der.SemiWidth);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptBr, "ptr07");
			//LegBase.ProcessMessages();

			pGeom = ARANFunctions.LineLineIntersect(ptBr, _depDir, ptr01, rExpAng);
			if (pGeom.Type != GeometryType.Point)
				throw new Exception("Invalid geometry type.");
			ptr02 = (Point)pGeom;
			ptr03 = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, 0.0, -_WPT_der.SemiWidth);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr02, "ptr02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr03, "ptr03");
			//LegBase.ProcessMessages();

			ptBl = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, _WPT_der.SemiWidth);
			pGeom = ARANFunctions.LineLineIntersect(ptBl, _depDir, ptl01, lExpAng);
			if (pGeom.Type != GeometryType.Point)
				throw new Exception("Invalid geometry type.");
			ptl02 = (Point)pGeom;
			ptl03 = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, 0.0, _WPT_der.SemiWidth);

			ptr13 = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, 0.0, -fWd);
			ptl13 = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, 0.0, fWd);

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			ptBr = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, -_WPT_15NM.SemiWidth);
			pGeom = ARANFunctions.LineLineIntersect(ptBr, _depDir, ptr03, rExpAng);
			if (pGeom.Type != GeometryType.Point)
				throw new Exception("Invalid geometry type.");
			ptr04 = (Point)pGeom;
			ptr05 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 0.0, -_WPT_15NM.SemiWidth);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr04, "ptr04");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr05, "ptr05");
			//LegBase.ProcessMessages();

			ARANFunctions.PrjToLocal(_WPT_15NM.PrjPt, _depDir, ptr04, out localX, out localY);
			ptr14 = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, localX, -0.5 * _WPT_15NM.SemiWidth);
			ptr15 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 0.0, -0.5 * _WPT_15NM.SemiWidth);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr14, "ptr14");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr15, "ptr15");
			//LegBase.ProcessMessages();

			ptBl = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, _WPT_15NM.SemiWidth);
			pGeom = ARANFunctions.LineLineIntersect(ptBl, _depDir, ptl03, lExpAng);
			if (pGeom.Type != GeometryType.Point)
				throw new Exception("Invalid geometry type.");
			ptl04 = (Point)pGeom;
			ptl05 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 0.0, _WPT_15NM.SemiWidth);

			ARANFunctions.PrjToLocal(_WPT_15NM.PrjPt, _depDir, ptl04, out localX, out localY);
			ptl14 = ARANFunctions.LocalToPrj(_WPT_15NM.PrjPt, _depDir, localX, 0.5 * _WPT_15NM.SemiWidth);
			ptl15 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 0.0, 0.5 * _WPT_15NM.SemiWidth);

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			Point ptr06, ptr07;
			Point ptl06, ptl07;
			Point ptr16, ptr17;
			Point ptl16, ptl17;

			ptBr = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, -_WPT_30NM.SemiWidth);
			pGeom = ARANFunctions.LineLineIntersect(ptBr, _depDir, ptr05, rExpAng);
			if (pGeom.Type != GeometryType.Point)
				throw new Exception("Invalid geometry type.");
			ptr06 = (Point)pGeom;

			if (_WPT_30NM.SensorType == eSensorType.GNSS || _termDistance < PANSOPSConstantList.PBNTerminalTriggerDistance)
				ptr07 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 2 * _termDistance, -_WPT_30NM.SemiWidth);
			else
				ptr07 = ARANFunctions.LocalToPrj(_WPT_der.PrjPt, _depDir, _termDistance, -Leg.EndFIX.SemiWidth);

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr04, "ptr04");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr05, "ptr05");

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr06, "ptr06");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr07, "ptr07");
			//LegBase.ProcessMessages();

			ARANFunctions.PrjToLocal(_WPT_30NM.PrjPt, _depDir, ptr06, out localX, out localY);
			ptr16 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, localX, -0.5 * _WPT_30NM.SemiWidth);

			if (_WPT_30NM.SensorType == eSensorType.GNSS || _termDistance < PANSOPSConstantList.PBNTerminalTriggerDistance)
				ptr17 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 2 * _termDistance, -0.5 * _WPT_30NM.SemiWidth);
			else
				ptr17 = ARANFunctions.LocalToPrj(_WPT_der.PrjPt, _depDir, _termDistance, -0.5 * Leg.EndFIX.SemiWidth);

			ptBl = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, 0.0, _WPT_30NM.SemiWidth);
			pGeom = ARANFunctions.LineLineIntersect(ptBl, _depDir, ptl05, lExpAng);
			if (pGeom.Type != GeometryType.Point)
				throw new Exception("Invalid geometry type.");
			ptl06 = (Point)pGeom;

			if (_WPT_30NM.SensorType == eSensorType.GNSS || _termDistance < PANSOPSConstantList.PBNTerminalTriggerDistance)
				ptl07 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 2 * _termDistance, _WPT_30NM.SemiWidth);
			else
				ptl07 = ARANFunctions.LocalToPrj(_WPT_der.PrjPt, _depDir, _termDistance, Leg.EndFIX.SemiWidth);

			ARANFunctions.PrjToLocal(_WPT_30NM.PrjPt, _depDir, ptl06, out localX, out localY);
			ptl16 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, localX, 0.5 * _WPT_30NM.SemiWidth);

			if (_WPT_30NM.SensorType == eSensorType.GNSS || _termDistance < PANSOPSConstantList.PBNTerminalTriggerDistance)
				ptl17 = ARANFunctions.LocalToPrj(_WPT_30NM.PrjPt, _depDir, 2 * _termDistance, 0.5 * _WPT_30NM.SemiWidth);
			else
				ptl17 = ARANFunctions.LocalToPrj(_WPT_der.PrjPt, _depDir, _termDistance, 0.5 * Leg.EndFIX.SemiWidth);

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			p120Line = new MultiLineString();
			LineString pLineStr = new LineString();
			pLineStr.Add(ptr01);
			pLineStr.Add(ptl01);
			p120Line.Add(pLineStr);

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			//Aran.AranEnvironment.Symbols.PointSymbol ptSym = new AranEnvironment.Symbols.PointSymbol();
			//ptSym.Size = 8;
			//ptSym.Style = AranEnvironment.Symbols.ePointStyle.smsCircle;
			//ptSym.Color = 255;

			//_aranEnvironment.Graphics.DrawPointWithText(ptr00, ptSym, "ptr00");
			//_aranEnvironment.Graphics.DrawPointWithText(ptr01, ptSym, "ptr01");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr02, ptSym, "ptr02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr03, ptSym, "ptr03");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr04, ptSym, "ptr04");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr05, ptSym, "ptr05");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr06, ptSym, "ptr06");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr07, ptSym, "ptr07");
			////Leg.ProcessMessages();

			//ptSym.Color = ARANFunctions.RGB(0, 0, 255);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl07, ptSym, "ptl07");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl06, ptSym, "ptl06");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl05, ptSym, "ptl05");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl04, ptSym, "ptl04");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl03, ptSym, "ptl03");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl02, ptSym, "ptl02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl01, ptSym, "ptl01");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl00, ptSym, "ptl00");
			//Leg.ProcessMessages();

			//GlobalVars.gAranGraphics.DrawPointWithText(ptArea1End, ptSym, "ptArea1End");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptBr, ptSym, "ptBr");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptBl, ptSym, "ptBl");
			//GlobalVars.gAranGraphics.DrawPointWithText(pPrjPt, ptSym, "PrjPt");

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr02, ptSym, "ptr02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl02, ptSym, "ptl02");

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//Aran.AranEnvironment.Symbols.LineSymbol pLineSym = new Aran.AranEnvironment.Symbols.LineSymbol();
			//pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
			//pLineSym.Width = 2;
			//////=====
			//Aran.AranEnvironment.Symbols.FillSymbol pFullAreaSym = new Aran.AranEnvironment.Symbols.FillSymbol();
			//pFullAreaSym.Style = eFillStyle.sfsVertical;	//Aran.AranEnvironment.Symbols.eFillStyle.sfsNull;
			//pFullAreaSym.Color = 255;
			//pFullAreaSym.Outline = pLineSym;	//	pRedLineSymbol
			//GlobalVars.gAranGraphics.DrawMultiPolygon(primArea, pFullAreaSym);
			//Leg.ProcessMessages();

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//Aran.AranEnvironment.Symbols.FillSymbol pSecondAreaSym = new Aran.AranEnvironment.Symbols.FillSymbol();
			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			//MultiPolygon pFullCircle = new MultiPolygon();
			//Polygon pCirclePolygon = new Polygon();

			//pFullAreaSym.Style = eFillStyle.sfsBackwardDiagonal;
			//pFullAreaSym.Color = ARANFunctions.RGB(0, 255, 0);
			//Ring pCircleRing = ARANFunctions.CreateCirclePrj(_ADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance);
			//pCirclePolygon.ExteriorRing = pCircleRing;
			//pFullCircle.Add(pCirclePolygon);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(pFullCircle, pFullAreaSym);

			//pFullAreaSym.Style = eFillStyle.sfsForwardDiagonal;
			//pFullAreaSym.Color = ARANFunctions.RGB(0, 0, 255);
			//pCircleRing = ARANFunctions.CreateCirclePrj(_ADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance);
			//pCirclePolygon.ExteriorRing = pCircleRing;
			//pFullCircle.Add(pCirclePolygon);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(pFullCircle, pFullAreaSym);


			fullArea = new MultiPolygon();
			Polygon tmpPoly = new Polygon();
			Ring extRing = new Ring();

			if (_bTurnBeforeDer)
				extRing.Add(ptrm00);

			//ptr07 = ARANFunctions.LocalToPrj(_WPT_der.PrjPt, _depDir, _termDistance, -Leg.EndFIX.SemiWidth);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr07, "ptr07");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr00, "ptr00");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr01, "ptr01");
			//LegBase.ProcessMessages();


			extRing.Add(ptr00);
			extRing.Add(ptr01);
			extRing.Add(ptr02);
			extRing.Add(ptr03);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptr04);
			extRing.Add(ptr05);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptr06);
			extRing.Add(ptr07);

			extRing.Add(ptl07);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptl06);
			extRing.Add(ptl05);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptl04);
			extRing.Add(ptl03);
			extRing.Add(ptl02);
			extRing.Add(ptl01);
			extRing.Add(ptl00);

			if (_bTurnBeforeDer)
				extRing.Add(ptlm00);

			tmpPoly.ExteriorRing = extRing;
			fullArea.Add(tmpPoly);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(fullArea, eFillStyle.sfsForwardDiagonal);
			//LegBase.ProcessMessages();


			//Aran.AranEnvironment.Symbols.LineSymbol pLineSym = new Aran.AranEnvironment.Symbols.LineSymbol();
			//pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
			//pLineSym.Width = 2;
			////=====
			//Aran.AranEnvironment.Symbols.FillSymbol pFullAreaSym = new Aran.AranEnvironment.Symbols.FillSymbol();
			//pFullAreaSym.Style = eFillStyle.sfsVertical;	//Aran.AranEnvironment.Symbols.eFillStyle.sfsNull;
			//pFullAreaSym.Color = 255;
			//pFullAreaSym.Outline = pLineSym;	//	pRedLineSymbol
			//_aranEnvironment.Graphics.DrawMultiPolygon(fullArea, pFullAreaSym);
			//_aranEnvironment.Graphics.DrawMultiPolygon(fullArea,255,eFillStyle.sfsDiagonalCross);
			//Leg.ProcessMessages();

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			primArea = new MultiPolygon();
			tmpPoly = new Polygon();
			extRing = new Ring();

			extRing.Add(ptrm10);

			extRing.Add(ptr10);
			extRing.Add(ptr13);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptr14);
			extRing.Add(ptr15);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptr16);
			extRing.Add(ptr17);

			extRing.Add(ptl17);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptl16);
			extRing.Add(ptl15);
			if (_WPT_30NM.SensorType == eSensorType.GNSS)
				extRing.Add(ptl14);
			extRing.Add(ptl13);
			extRing.Add(ptl10);

			extRing.Add(ptlm10);

			tmpPoly.ExteriorRing = extRing;
			primArea.Add(tmpPoly);

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			//Aran.AranEnvironment.Symbols.PointSymbol ptSym = new AranEnvironment.Symbols.PointSymbol();
			//ptSym.Size = 8;
			//ptSym.Style = AranEnvironment.Symbols.ePointStyle.smsCircle;

			//ptSym.Color = 255;
			//_aranEnvironment.Graphics.DrawPointWithText(ptr00, ptSym, "ptr00");
			//_aranEnvironment.Graphics.DrawPointWithText(ptr01, ptSym, "ptr01");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr02, ptSym, "ptr02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr03, ptSym, "ptr03");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr04, ptSym, "ptr04");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr05, ptSym, "ptr05");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr06, ptSym, "ptr06");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptr07, ptSym, "ptr07");

			//ptSym.Color = ARANFunctions.RGB(0, 0, 255);
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl07, ptSym, "ptl07");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl06, ptSym, "ptl06");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl05, ptSym, "ptl05");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl04, ptSym, "ptl04");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl03, ptSym, "ptl03");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl02, ptSym, "ptl02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl01, ptSym, "ptl01");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl00, ptSym, "ptl00");

			////GlobalVars.gAranGraphics.DrawPointWithText(ptArea1End, ptSym, "ptArea1End");
			////GlobalVars.gAranGraphics.DrawPointWithText(ptBr, ptSym, "ptBr");
			////GlobalVars.gAranGraphics.DrawPointWithText(ptBl, ptSym, "ptBl");
			////GlobalVars.gAranGraphics.DrawPointWithText(pPrjPt, ptSym, "PrjPt");

			//GlobalVars.gAranGraphics.DrawPointWithText(ptr02, ptSym, "ptr02");
			//GlobalVars.gAranGraphics.DrawPointWithText(ptl02, ptSym, "ptl02");


			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//Aran.AranEnvironment.Symbols.LineSymbol pLineSym = new Aran.AranEnvironment.Symbols.LineSymbol();
			//pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
			//pLineSym.Width = 2;
			//////=====
			//Aran.AranEnvironment.Symbols.FillSymbol pFullAreaSym = new Aran.AranEnvironment.Symbols.FillSymbol();
			//pFullAreaSym.Style = eFillStyle.sfsVertical;	//Aran.AranEnvironment.Symbols.eFillStyle.sfsNull;
			//pFullAreaSym.Color = 255;
			//pFullAreaSym.Outline = pLineSym;	//	pRedLineSymbol
			//GlobalVars.gAranGraphics.DrawMultiPolygon(primArea, pFullAreaSym);
			//Leg.ProcessMessages();

			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//Aran.AranEnvironment.Symbols.FillSymbol pSecondAreaSym = new Aran.AranEnvironment.Symbols.FillSymbol();
			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			//MultiPolygon pFullCircle = new MultiPolygon();
			//Polygon pCirclePolygon = new Polygon();

			//pFullAreaSym.Style = eFillStyle.sfsBackwardDiagonal;
			//pFullAreaSym.Color = ARANFunctions.RGB(0, 255, 0);
			//Ring pCircleRing = ARANFunctions.CreateCirclePrj(_ADHP.pPtPrj, PANSOPSConstantList.PBNInternalTriggerDistance);
			//pCirclePolygon.ExteriorRing = pCircleRing;
			//pFullCircle.Add(pCirclePolygon);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(pFullCircle, pFullAreaSym);

			//pFullAreaSym.Style = eFillStyle.sfsForwardDiagonal;
			//pFullAreaSym.Color = ARANFunctions.RGB(0, 0, 255);
			//pCircleRing = ARANFunctions.CreateCirclePrj(_ADHP.pPtPrj, PANSOPSConstantList.PBNTerminalTriggerDistance);
			//pCirclePolygon.ExteriorRing = pCircleRing;
			//pFullCircle.Add(pCirclePolygon);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(pFullCircle, pFullAreaSym);
		}

		protected virtual void createDrawingPolygons(out MultiPolygon pFullArea, out MultiPolygon pPrimArea)
		{
			GeometryOperators geometryOperators = new GeometryOperators();

			pFullArea = (MultiPolygon)geometryOperators.Intersect(_circle, _fullAreaPolygon);
			pPrimArea = (MultiPolygon)geometryOperators.Intersect(pFullArea, _primatyAreaPolygon);

			Leg.PrimaryArea = pPrimArea;
			Leg.FullArea = pFullArea;
		}

		public virtual void Clean()
		{
			Leg.DeleteGraphics();
			_aranEnvironment.Graphics.SafeDeleteGraphic(GlobalVars.p120LineElem);
		}

		public virtual void ReDraw()
		{
			bool recreate = true;
			if (recreate)
			{
				MultiPolygon pFullArea, pPrimArea;
				createDrawingPolygons(out pFullArea, out pPrimArea);
			}

			Leg.RefreshGraphics();
			_aranEnvironment.Graphics.SafeDeleteGraphic(GlobalVars.p120LineElem);

			if (_p120Line != null)
			{
				LineSymbol pLineSym = new LineSymbol();
				pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
				pLineSym.Style = eLineStyle.slsSolid;
				pLineSym.Width = 1;
				GlobalVars.p120LineElem = _aranEnvironment.Graphics.DrawMultiLineString(_p120Line, pLineSym, GlobalVars.ButtonControl7State);
			}
		}
	}
}

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/