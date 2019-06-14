using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.Departure
{

	[System.Runtime.InteropServices.ComVisible(false)]
	public class Segment1Term
	{
		public Segment1Term(Straight straight)
		{
			_straightLeg = straight;

			_IAS = _straightLeg.IAS;
			_nomLinePDG = GlobalVars.NomLineGrd;
			_constructPDG = GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;

			_nomLineAltitude = straight.NomPDGHeight + _straightLeg.RWY.pPtPrj[eRWY.ptDER].Z + GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpOIS_abv_DER].Value;     //straight.Leg.Length * _nomLinePDG;
			_constructAltitude = _nomLineAltitude;      //straight.Leg.Length * _constructPDG;// + straight.NomPDGHeight;
			_appliedAltitude = _nomLineAltitude;// 

			_terminationLeg = new TerminationLeg(straight);
			_terminationLeg.sensor = _straightLeg.sensor;
			_terminationLeg.PBNtype = _straightLeg.PBNtype;
			_terminationLeg.PDGChanged += OnPDGChanged;
			_terminationLeg.NomPDGDistChanged += OnNomPDGDistChanged;
		}

		//private static Aran.PANDA.Constants.Constants constants = null;

		private Straight _straightLeg;
		private TerminationLeg _terminationLeg;

		public event DoubleValEventHandler PDGChanged;
		public event DoubleValEventHandler NomPDGDistChanged;

		private TerminationType _terminationType;
		public TerminationType terminationType
		{
			get { return _terminationType; }

			set
			{
				this._terminationType = value;

				switch (_terminationType)
				{
					case TerminationType.AtHeight:
						_terminationLeg.ConstructAltitude = _nomLineAltitude = _constructAltitude = _appliedAltitude;

						_termDistance = (_constructAltitude - _straightLeg.RWY.pPtPrj[eRWY.ptDER].Z - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpOIS_abv_DER].Value) / _appliedPDG;
						//leg.EndFIX.PrjPtH = ARANFunctions.LocalToPrj(_straightLeg.RWY.pPtPrj[eRWY.ptDER], leg.EndFIX.OutDirection, _termDistance);
						//_terminationLeg.nom = _appliedAltitude;

						break;
					case TerminationType.AtWPT:
						//_nomLinePDG = GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;
						_nomLineAltitude = _constructAltitude;
						//leg.EndFIX.ISA
						//private double _EPT;

						//_EPT = Functions.MinFlybyDist(GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.ISA].Value, _straightLeg.RWY.pPtPrj[eRWY.ptDER].Z, _appliedPDG, _plannedMaxTurnAngle, Leg.EndFIX, _constructAltitude);

						_termDistance = (_constructAltitude - _straightLeg.RWY.pPtPrj[eRWY.ptDER].Z - GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpOIS_abv_DER].Value) / _appliedPDG;
						break;
					case TerminationType.Intercept:
						_terminationLeg.Clean();
						break;
				}

				_terminationLeg.terminationType = value;
			}
		}

		private double _constructPDG;
		public double ConstructionPDG
		{
			get { return _constructPDG; }

			set
			{
				_constructPDG = value;
				Leg.EndFIX.ConstructionGradient = value;
				_terminationLeg.ConstructionGradient = value;
			}
		}

		private double _constructAltitude;			//protected area
		public double ConstructAltitude
		{
			get { return _constructAltitude; }

			set
			{
				_constructAltitude = value;
				_terminationLeg.ConstructAltitude = _constructAltitude;

				if (_terminationType == TerminationType.AtHeight)
				{
					_nomLineAltitude = value;
					_terminationLeg.NomLineAltitude = _nomLineAltitude;
					//Leg.EndFIX.NomLineAltitude = _nomLineAltitude;
				}
			}
		}

		double _nomLinePDG;
		public double NomLinePDG
		{
			get { return _nomLinePDG; }

			set
			{
				if (value < _appliedPDG)
					value = _appliedPDG;

				if (value > GlobalVars.NomLineGrd)
					value = GlobalVars.NomLineGrd;

				if (_nomLinePDG == value)
					return;

				_nomLinePDG = value;
				_terminationLeg.NomLineGradient = value;

				//if (_terminationType == TerminationType.AtHeight)
				//{

				//}
			}
		}

		double _nomLineAltitude;
		public double NomLineAltitude					//stabilisation distances
		{
			set
			{
				if (_terminationType == TerminationType.AtWPT)
				{
					_nomLinePDG = (value - _terminationLeg.RWY.pPtPrj[eRWY.ptDER].Z -
											GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpOIS_abv_DER].Value) / _termDistance;
					if (_nomLinePDG > GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value)
						_nomLinePDG = GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;
					else if (_nomLinePDG < _appliedPDG)
						_nomLinePDG = _appliedPDG;

					_terminationLeg.NomLineGradient = _nomLinePDG;
					_nomLineAltitude = _termDistance * _nomLinePDG + _terminationLeg.RWY.pPtPrj[eRWY.ptDER].Z +
										GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpOIS_abv_DER].Value;	//??????????
				}
				else
				{
					_nomLinePDG = GlobalVars.NomLineGrd;// GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;	//_constructPDG;//
					_terminationLeg.NomLineGradient = _nomLinePDG;
					_nomLineAltitude = _constructAltitude;
				}

				_terminationLeg.NomLineAltitude = _nomLineAltitude;
			}

			get
			{
				if (_terminationType == TerminationType.AtWPT)
					_nomLineAltitude = _termDistance * _nomLinePDG + _terminationLeg.RWY.pPtPrj[eRWY.ptDER].Z +
										GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpOIS_abv_DER].Value;
				else
					_nomLineAltitude = _constructAltitude;

				_terminationLeg.NomLineAltitude = _nomLineAltitude;
				//Leg.EndFIX.NomLineAltitude = _nomLineAltitude;

				return _nomLineAltitude;
			}
		}

		double _appliedAltitude;
		public double AppliedAltitude					//stabilisation distances
		{
			set
			{
				_appliedAltitude = value;
				if (_terminationType == TerminationType.AtHeight)
				{
					_constructAltitude = _appliedAltitude;
					_terminationLeg.ConstructAltitude = _constructAltitude;

					_nomLineAltitude = _appliedAltitude;
					_terminationLeg.NomLineAltitude = _appliedAltitude;

					//Leg.EndFIX.NomLineAltitude = _nomLineAltitude;
				}
			}
			get { return _appliedAltitude; }
		}

		private double _appliedPDG;
		public double AppliedPDG
		{
			get { return _appliedPDG; }

			set
			{
				_appliedPDG = value;
				//leg.EndFIX.AppliedGradient = value;
				_terminationLeg.AppliedGradient = _appliedPDG;
			}
		}

		private double _termDistance;
		public double TermDistance
		{
			get { return _termDistance; }

			set
			{
				_termDistance = value;
				//_terminationLeg.ConstructAltitude
				_terminationLeg.TermDistance = _termDistance;
			}
		}

		public ADHPType ADHP
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.ADHP;

				return new ADHPType();
			}
		}

		public WayPoint TermWPT
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.WPT_TP;

				return null;
			}
		}

		public eFlyMode FlyMode
		{
			get
			{
				if (_terminationType == TerminationType.AtWPT)
					return _terminationLeg.FlyMode;
				return (eFlyMode)(-1);
			}

			set { _terminationLeg.FlyMode = value; }
		}

		double _plannedMaxTurnAngle;
		public double PlannedMaxTurnAngle
		{
			get { return _plannedMaxTurnAngle; }

			set
			{
				_plannedMaxTurnAngle = value;
				_terminationLeg.PlannedMaxTurnAngle = value;
			}
		}

		double _IAS;
		public double IAS
		{
			get { return _IAS; }

			set
			{
				_IAS = value;
				_terminationLeg.IAS = value;
			}
		}

		double _bankAngle;
		public double BankAngle
		{
			get { return _bankAngle; }

			set
			{
				_bankAngle = value;
				_terminationLeg.BankAngle = value;
			}
		}

		public double MinPDG
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.MinPDG;
				return 0;
			}
		}

		public double NomPDGHeight
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.NomPDGHeight;

				return 0;
			}
		}

		bool _updateEnabled = false;
		public bool UpdateEnabled
		{
			set
			{
				_updateEnabled = value;
				_terminationLeg.UpdateEnabled = _updateEnabled;
			}

			get { return _updateEnabled; }
		}

		public bool TurnBeforeDer
		{
			set { _terminationLeg.TurnBeforeDer = value; }

			get { return _terminationLeg.TurnBeforeDer; }
		}

		public ObstacleContainer DetObs
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.DetObs;

				ObstacleContainer result;
				result.Obstacles = new Obstacle[1];
				result.Parts = new ObstacleData[1];
				result.Obstacles[0].ID = -1;
				result.Parts[0].Index = -1;

				return result;
			}
		}

		public ObstacleContainer InnerObstacleList
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.InnerObstacleList;

				ObstacleContainer result;
				result.Obstacles = new Obstacle[1];
				result.Parts = new ObstacleData[1];
				result.Obstacles[0].ID = -1;
				result.Parts[0].Index = -1;

				return result;
			}
		}

		public LegDep Leg
		{
			get
			{
				if (_terminationType != TerminationType.Intercept)
					return _terminationLeg.Leg;

				return null;
			}
		}

		private void OnPDGChanged(object sender, double newPDG)
		{
			if (PDGChanged != null)
				PDGChanged(this, newPDG);
		}

		private void OnNomPDGDistChanged(object sender, double newNomPDG)
		{
			if (NomPDGDistChanged != null)
				NomPDGDistChanged(this, _terminationLeg.NomPDGHeight);
		}

		public void Clean()
		{
			_terminationLeg.Clean();
		}

		public void ReDraw()
		{
			_terminationLeg.ReDraw();
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