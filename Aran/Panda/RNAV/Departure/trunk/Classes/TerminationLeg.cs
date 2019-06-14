using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;
using Aran.Geometries;
using Aran.PANDA.Constants;
using Aran.Geometries.Operators;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class TerminationLeg : Straight
	{
		//Point _ptHt;
		//int _ptHtElem;

		public TerminationLeg(Straight Base)
			: base(Base.RWY, Base.InitialTrack, Base.MOCLimit, Base.TermDistance, Base.TurnBeforeDer, Base.ADHP, Base.Environment)
		{
			WPT_FIXType fix = new WPT_FIXType();

			fix.Name = "TP";
			fix.pPtPrj = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, Base.TermDistance, 0);
			fix.pPtGeo = _RWY.pPtGeo[eRWY.ptDER];

			TerminationFIX _WPT_TP;
			_WPT_TP = new TerminationFIX(eFIXRole.TP_, fix, _aranEnvironment);
			_WPT_TP.DrawingEnabled = false;
			_WPT_TP.EntryDirection = _depDir;
			_WPT_TP.OutDirection = _depDir;
			_WPT_TP.TurnDirection = TurnDirection.CCW;

			_WPT_TP.FlightPhase = eFlightPhase.SID;
			//_WPT_TP.FlyMode = eFlyMode.Atheight;	//_WPT_TP.FlyMode = eFlyMode.Flyby;
			_WPT_TP.ISAtC = 15;

			Leg = new LegDep(_WPT_der, _WPT_TP, Base.Environment);

			//IAS = Base.IAS;
			_IAS = Base.IAS;
			_WPT_der.IAS = _IAS;
			_WPT_15NM.IAS = _IAS;
			_WPT_30NM.IAS = _IAS;
			Leg.IAS = _IAS;
		}

		TerminationType _terminationType;
		public TerminationType terminationType
		{
			get { return _terminationType; }
			set
			{
				if (_terminationType != value)
				{
					_terminationType = value;

					if (_terminationType == TerminationType.AtHeight)
					{
						Leg.EndFIX.NomLineAltitude = Leg.EndFIX.ConstructAltitude;

						_nomDistance = (Leg.EndFIX.NomLineAltitude - _RWY.pPtPrj[eRWY.ptDER].Z - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _nomLineGradient;
						Leg.EndFIX.PrjPtH = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, _nomDistance, 0);

						//_aranEnvironment.Graphics.DrawPointWithText(Leg.EndFIX.PrjPtH, -1, "PrjPtH");
						//LegBase.ProcessMessages();

						//double dh = (ConstructAltitude - _RWY.pPtPrj[eRWY.ptDER].Z - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _PDG;
						//Leg.EndFIX.PrjPtH = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, dh, 0);
					}
					else
						Leg.EndFIX.PrjPtH = Leg.EndFIX.PrjPt;

					((TerminationFIX)Leg.EndFIX).terminationType = value;
					ApplayWPTParams();
					//ReDraw();
				}
			}
		}

		eFlyMode _flyMode;
		public eFlyMode FlyMode
		{
			get { return _flyMode; }
			set
			{
				if (_flyMode != value)
				{
					_flyMode = value;
					Leg.EndFIX.FlyMode = value;
					ApplayWPTParams();
				}
			}
		}

		override public ePBNClass PBNtype
		{
			get { return base.PBNtype; }
			set
			{
				if (_pbnType != value)
				{
					base.PBNtype = value;
					ApplayWPTParams();
				}
			}
		}

		override public eSensorType sensor
		{
			get { return base.sensor; }
			set
			{
				base.sensor = value;
				Leg.EndFIX.SensorType = value;

				if (_sensor != value)
					ApplayWPTParams();
			}
		}

		public override double NomLineGradient
		{
			set
			{
				_nomLineGradient = value;
				Leg.EndFIX.NomLineGradient = value;

				if (terminationType == TerminationType.AtHeight)
				{
					Point PtEnd = _RWY.pPtPrj[eRWY.ptDER];
					//Leg.EndFIX.NomLineAltitude
					_nomDistance = (ConstructAltitude - PtEnd.Z - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _nomLineGradient;
					Leg.EndFIX.PrjPtH = ARANFunctions.LocalToPrj(PtEnd, _depDir, _nomDistance, 0);
					ReDraw();
				}
			}
		}


		double _nomLineAltitude;
		public double NomLineAltitude
		{
			get			{ return _nomLineAltitude; }
			set
			{
				_nomLineAltitude = value;
				Leg.EndFIX.NomLineAltitude = value;

				if (terminationType == TerminationType.AtHeight)
				{
					_nomDistance = (_nomLineAltitude - _RWY.pPtPrj[eRWY.ptDER].Z - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _nomLineGradient;
					Leg.EndFIX.PrjPtH = ARANFunctions.LocalToPrj(_RWY.pPtPrj[eRWY.ptDER], _depDir, _nomDistance, 0);
					ReDraw();
				}
			}
		}

		protected override void setNewDistance(double newDistance)
		{
			_termDistance = newDistance;
			Point PtEnd = _RWY.pPtPrj[eRWY.ptDER];

			Leg.EndFIX.PrjPt = ARANFunctions.LocalToPrj(PtEnd, _depDir, newDistance, 0);

			if (_terminationType == TerminationType.AtHeight)
			{	//?????
				_nomDistance  = (ConstructAltitude - PtEnd.Z - constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _nomLineGradient;
				Leg.EndFIX.PrjPtH = ARANFunctions.LocalToPrj(PtEnd, _depDir, _nomDistance , 0);
			}
			else
			{
				Leg.EndFIX.ConstructAltitude = PtEnd.Z + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _termDistance * constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
				Leg.EndFIX.NomLineAltitude = PtEnd.Z + constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _nomLineGradient * _termDistance;
				Leg.EndFIX.PrjPtH = Leg.EndFIX.PrjPt;
			}

			double BankAngle = Leg.EndFIX.BankAngle;
			double dist = ARANFunctions.ReturnDistanceInMeters(_ADHP.pPtPrj, Leg.EndFIX.PrjPt);

			if (dist < PANSOPSConstantList.PBNInternalTriggerDistance)
				Leg.EndFIX.FlightPhase = eFlightPhase.SID;
			else if (dist < PANSOPSConstantList.PBNTerminalTriggerDistance)
				Leg.EndFIX.FlightPhase = eFlightPhase.SIDGE28;
			else
				Leg.EndFIX.FlightPhase = eFlightPhase.SIDGE56;

			Leg.EndFIX.BankAngle = BankAngle;
			Leg.EndFIX.ReCreateArea();
			Leg.EndFIX.DrawingEnabled = true;
			Leg.EndFIX.RefreshGraphics();
		}

		protected override void createDrawingPolygons(out MultiPolygon pFullArea, out MultiPolygon pPrimArea)
		{
			GeometryOperators geometryOperators = new GeometryOperators();

			LineString pLineStr = new LineString();
			Point PtEnd = _RWY.pPtPrj[eRWY.ptDER];

			double lptF = Leg.EndFIX.FlyMode != eFlyMode.Flyby ? Leg.EndFIX.LPT : Leg.EndFIX.ATT + Leg.EndFIX.PilotTime * Leg.EndFIX.ConstructTAS;
			double calcDist = _termDistance + lptF;

			//ptTmp = ARANFunctions.LocalToPrj(_StartFIX.PrjPt, EntryDir, LPT - 1.0, 0);

			pLineStr.Add(ARANFunctions.LocalToPrj(PtEnd, _depDir, calcDist, 50000.0));
			pLineStr.Add(ARANFunctions.LocalToPrj(PtEnd, _depDir, calcDist, -50000.0));

			//_aranEnvironment.Graphics.DrawMultiPolygon(_fullAreaPolygon, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal);
			//_aranEnvironment.Graphics.DrawMultiPolygon(_primatyAreaPolygon, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
			//_aranEnvironment.Graphics.DrawLineString(pLineStr, 2);
			//LegBase.ProcessMessages();

			Geometry geomLeft, geomRight;
			geometryOperators.Cut(_fullAreaPolygon, pLineStr, out geomLeft, out geomRight);

			//_aranEnvironment.Graphics.DrawMultiPolygon(_fullAreaPolygon, -1, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal);
			//_aranEnvironment.Graphics.DrawLineString(pLineStr, -1, 2);
			//LegBase.ProcessMessages(true);

			Leg.FullArea = (MultiPolygon)geomRight;
			Leg.PrimaryArea = (MultiPolygon)geometryOperators.Intersect(Leg.FullArea, _primatyAreaPolygon);

			//_aranEnvironment.Graphics.DrawMultiPolygon(Leg.FullArea, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal, 0);
			//_aranEnvironment.Graphics.DrawMultiPolygon(Leg.PrimaryArea, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal, 255);
			//_aranEnvironment.Graphics.DrawLineString(pLineStr, -1, 2);
			//LegBase.ProcessMessages();

			//=================================================================================
			if (_terminationType == TerminationType.AtHeight)
				calcDist = _termDistance - Leg.EndFIX.EPT;

			pLineStr.Clear();
			pLineStr.Add(ARANFunctions.LocalToPrj(PtEnd, _depDir, calcDist, 50000.0));
			pLineStr.Add(ARANFunctions.LocalToPrj(PtEnd, _depDir, calcDist, -50000.0));

			//_aranEnvironment.Graphics.DrawMultiPolygon(_FullAreaPolygon, 0, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
			//_aranEnvironment.Graphics.DrawLineString(pLineStr, 255, 2);
			//Leg.ProcessMessages();
			//_aranEnvironment.Graphics.DrawPointWithText(PtEnd, 255, "PtEnd");
			//Leg.ProcessMessages();

			//geomLeft = geomRight = null;
			geometryOperators.Cut(_fullAreaPolygon, pLineStr, out geomLeft, out geomRight);

			pFullArea = (MultiPolygon)geomRight;
			pPrimArea = (MultiPolygon)geometryOperators.Intersect(pFullArea, _primatyAreaPolygon);

			//_aranEnvironment.Graphics.DrawMultiPolygon(pFullArea, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal, 0);
			//_aranEnvironment.Graphics.DrawMultiPolygon(pPrimArea, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal, 255);
			//LegBase.ProcessMessages();

			Leg.PrimaryAssesmentArea = pPrimArea;
			Leg.FullAssesmentArea = pFullArea;
		}

		public override void Clean()
		{
			base.Clean();
			//_aranEnvironment.Graphics.SafeDeleteGraphic(_ptHtElem);
		}

		public override void ReDraw()
		{
			base.ReDraw();
			//if (_terminationType == TerminationType.AtHeight)
			//{
			//	_aranEnvironment.Graphics.SafeDeleteGraphic(_ptHtElem);
			//	_ptHtElem = _aranEnvironment.Graphics.DrawPointWithText(_ptHt, ARANFunctions.RGB(127, 255, 0), "HT");
			//	//_ptHt = 
			//}
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