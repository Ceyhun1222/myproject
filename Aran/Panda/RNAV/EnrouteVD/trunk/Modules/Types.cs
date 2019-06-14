using System;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.Enroute.VD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportHeader
	{
		public string Aerodrome;
		public string Procedure;
		public string Category;
		public string Database;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportPoint
	{
		public string Description;
		public double Lat;
		public double Lon;

		public double TrueTrack;        //out
		public double MagnTrack;        //out
		public double Altitude;

		public double MOC;
		public double MOCA;

		public double DistToNext;
	}


	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Procedure
	{
		public Route pFeature;

		override public string ToString()
		{
			if (pFeature == null)
				return "";

			if (pFeature.Name == null)
				return "";
			return pFeature.Name;
			//return string.Format("{0}", pFeature.Name);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Segment
	{
		public RouteSegment pFeature;
		public Route proc;
		//public ePBNClass PBNType;

		public FIX Start;
		public FIX End;
		public ObstacleContainer Obstacles;
		public MultiLineString NominalTrack;
		public LineString EndLPTLine;
		public LineString EndKKLine;
		//public Polygon PrimaryProtectionArea;
		//public Polygon SecondaryProtectionArea;

		public EnRouteLeg leg;
		public CodeDirection Direction;

		public NavaidType StartVOR;
		public NavaidType StartDME
		{
			get { return Start.VOR_DME; }

			set
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(startTolerElem.toleranceElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(startTolerElem.nominalElem);

				//GlobalVars.gAranGraphics.SafeDeleteGraphic(SecondaryProtectionAreaElem);
				//GlobalVars.gAranGraphics.SafeDeleteGraphic(PrimaryProtectionAreaElem);

				Start.VOR_DME = value;
				Start.ReCreateArea();
				Start.RefreshGraphics();

				leg.StartFIX.VOR_DME = value;
				leg.StartFIX.ReCreateArea();

				//CreateLegGeometry();
				//============================================================================================================================================
				double navDir = ARANFunctions.ReturnAngleInRadians(Start.VOR_DME.pPtPrj, Start.PrjPt);
				Dstart = ARANFunctions.ReturnDistanceInMeters(Start.VOR_DME.pPtPrj, Start.PrjPt);
				double D1, D2;
				ARANFunctions.PrjToLocal(Start.PrjPt, Dir, Start.VOR_DME.pPtPrj, out D2, out D1);

				D1start = Math.Abs(D1);
				D2start = Math.Abs(D2);
				NavDirStart = NativeMethods.Modulus(ARANFunctions.DirToAzimuth(Start.VOR_DME.pPtPrj, navDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) - Start.VOR_DME.MagVar);
				//=========================================================================
				double SigmaAir = Math.Max(0.085 * 1852.0, 0.00125 * Dstart);
				double rNear = Dstart - SigmaAir;
				double rFar = Dstart + SigmaAir;

				startTolerElem.tolerance = new MultiLineString();
				startTolerElem.nominal = new MultiLineString();

				Point ptLeft = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance, rFar + 50);
				Point ptRight = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance, rFar + 50);
				Point ptCenter = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir, rFar + 50);

				LineString ls = new LineString();
				ls.Add(ptLeft);
				ls.Add(Start.VOR_DME.pPtPrj);
				ls.Add(ptRight);
				startTolerElem.tolerance.Add(ls);

				ptLeft = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance + ARANMath.DegToRad(1.0), rFar);
				ptRight = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance - ARANMath.DegToRad(1.0), rFar);
				ls = ARANFunctions.CreateArcAsPartPrj(Start.VOR_DME.pPtPrj, ptLeft, ptRight, TurnDirection.CW);
				startTolerElem.tolerance.Add(ls);

				ptLeft = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance + ARANMath.DegToRad(1.0), rNear);
				ptRight = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance - ARANMath.DegToRad(1.0), rNear);
				ls = ARANFunctions.CreateArcAsPartPrj(Start.VOR_DME.pPtPrj, ptLeft, ptRight, TurnDirection.CW);
				startTolerElem.tolerance.Add(ls);

				//_ptTolerElem.toleranceElem = GlobalVars.gAranGraphics.DrawMultiLineString(_ptTolerElem.tolerance, ARANFunctions.RGB(32, 255, 255), 1);
				LineSymbol lineSymbol = new LineSymbol();
				lineSymbol.Color = ARANFunctions.RGB(32, 255, 255);
				lineSymbol.Style = eLineStyle.slsDash;
				lineSymbol.Width = 1;

				startTolerElem.toleranceElem = GlobalVars.gAranGraphics.DrawMultiLineString(startTolerElem.tolerance, lineSymbol);

				ptLeft = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance + ARANMath.DegToRad(1.0), Dstart);
				ptRight = ARANFunctions.LocalToPrj(Start.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance - ARANMath.DegToRad(1.0), Dstart);
				ls = ARANFunctions.CreateArcAsPartPrj(Start.VOR_DME.pPtPrj, ptLeft, ptRight, TurnDirection.CW);
				startTolerElem.nominal.Add(ls);

				ls = new LineString();
				ls.Add(Start.VOR_DME.pPtPrj);
				ls.Add(ptCenter);
				startTolerElem.nominal.Add(ls);

				startTolerElem.nominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(startTolerElem.nominal, 1, 0);
				//========================================================================================================
				//return;

				//SecondaryProtectionAreaElem = GlobalVars.gAranGraphics.DrawPolygon(SecondaryProtectionArea, ARANFunctions.RGB(192, 192, 0), eFillStyle.sfsHollow);
				//PrimaryProtectionAreaElem = GlobalVars.gAranGraphics.DrawPolygon(PrimaryProtectionArea, ARANFunctions.RGB(0, 192, 0), eFillStyle.sfsHollow);

				//Point ptTmp1 = ARANFunctions.LocalToPrj(Start.PrjPt, Start.OutDirection, -Start.EPT, -Start.ASW_L);
				//Point ptTmp2 = ARANFunctions.LocalToPrj(Start.PrjPt, Start.OutDirection, -Start.EPT, Start.ASW_R);

				////GlobalVars.gAranGraphics.DrawPointWithText(ptTmp1, -1, "ptTmp1");
				////GlobalVars.gAranGraphics.DrawPointWithText(ptTmp2, -1, "ptTmp2");
				////System.Windows.Forms.Application.DoEvents();

				//StartKKLine = new LineString();
				//StartKKLine.Add(ptTmp1);
				//StartKKLine.Add(ptTmp2);

				//StartKKLineElem = GlobalVars.gAranGraphics.DrawLineString(StartKKLine, ARANFunctions.RGB(0, 198, 0), 1);
			}
		}

		public NavaidType EndVOR;
		public NavaidType EndDME
		{
			get { return End.VOR_DME; }

			set
			{
				GlobalVars.gAranGraphics.SafeDeleteGraphic(endTolerElem.toleranceElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(endTolerElem.nominalElem);

				//GlobalVars.gAranGraphics.SafeDeleteGraphic(SecondaryProtectionAreaElem);
				//GlobalVars.gAranGraphics.SafeDeleteGraphic(PrimaryProtectionAreaElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(EndKKLineElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(EndLPTLineElem);

				End.VOR_DME = value;
				End.ReCreateArea();
				End.RefreshGraphics();

				leg.EndFIX.VOR_DME = value;
				leg.EndFIX.ReCreateArea();

				//CreateLegGeometry();
				//=========================================================================
				double navDir = ARANFunctions.ReturnAngleInRadians(End.VOR_DME.pPtPrj, End.PrjPt);
				Dend = ARANFunctions.ReturnDistanceInMeters(End.VOR_DME.pPtPrj, End.PrjPt);
				double D1, D2;
				ARANFunctions.PrjToLocal(End.PrjPt, Dir, End.VOR_DME.pPtPrj, out D2, out D1);
				D1end = Math.Abs(D1);
				D2end = Math.Abs(D2);

				NavDirEnd = NativeMethods.Modulus(ARANFunctions.DirToAzimuth(End.VOR_DME.pPtPrj, navDir, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo) - End.VOR_DME.MagVar);
				//=========================================================================
				double SigmaAir = Math.Max(0.085 * 1852.0, 0.00125 * Dend);
				double rNear = Dend - SigmaAir;
				double rFar = Dend + SigmaAir;

				endTolerElem.tolerance = new MultiLineString();
				endTolerElem.nominal = new MultiLineString();

				Point ptLeft = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance, rFar + 50);
				Point ptRight = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance, rFar + 50);
				Point ptCenter = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir, rFar + 50);

				LineString ls = new LineString();
				ls.Add(ptLeft);
				ls.Add(End.VOR_DME.pPtPrj);
				ls.Add(ptRight);
				endTolerElem.tolerance.Add(ls);

				ptLeft = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance + ARANMath.DegToRad(1.0), rFar);
				ptRight = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance - ARANMath.DegToRad(1.0), rFar);
				ls = ARANFunctions.CreateArcAsPartPrj(End.VOR_DME.pPtPrj, ptLeft, ptRight, TurnDirection.CW);
				endTolerElem.tolerance.Add(ls);

				ptLeft = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance + ARANMath.DegToRad(1.0), rNear);
				ptRight = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance - ARANMath.DegToRad(1.0), rNear);
				ls = ARANFunctions.CreateArcAsPartPrj(End.VOR_DME.pPtPrj, ptLeft, ptRight, TurnDirection.CW);
				endTolerElem.tolerance.Add(ls);

				//_ptTolerElem.toleranceElem = GlobalVars.gAranGraphics.DrawMultiLineString(_ptTolerElem.tolerance, ARANFunctions.RGB(32, 255, 255), 1);
				LineSymbol lineSymbol = new LineSymbol();
				lineSymbol.Color = ARANFunctions.RGB(32, 255, 255);
				lineSymbol.Style = eLineStyle.slsDash;
				lineSymbol.Width = 1;

				endTolerElem.toleranceElem = GlobalVars.gAranGraphics.DrawMultiLineString(endTolerElem.tolerance, lineSymbol);

				ptLeft = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir + GlobalVars.navaidsConstants.VOR.IntersectingTolerance + ARANMath.DegToRad(1.0), Dend);
				ptRight = ARANFunctions.LocalToPrj(End.VOR_DME.pPtPrj, navDir - GlobalVars.navaidsConstants.VOR.IntersectingTolerance - ARANMath.DegToRad(1.0), Dend);
				ls = ARANFunctions.CreateArcAsPartPrj(End.VOR_DME.pPtPrj, ptLeft, ptRight, TurnDirection.CW);
				endTolerElem.nominal.Add(ls);

				ls = new LineString();
				ls.Add(End.VOR_DME.pPtPrj);
				ls.Add(ptCenter);
				endTolerElem.nominal.Add(ls);

				endTolerElem.nominalElem = GlobalVars.gAranGraphics.DrawMultiLineString(endTolerElem.nominal, 1, 0);
				//========================================================================================================
				//return;

				//SecondaryProtectionAreaElem = GlobalVars.gAranGraphics.DrawPolygon(SecondaryProtectionArea, ARANFunctions.RGB(192, 192, 0), eFillStyle.sfsHollow);
				//PrimaryProtectionAreaElem = GlobalVars.gAranGraphics.DrawPolygon(PrimaryProtectionArea, ARANFunctions.RGB(0, 192, 0), eFillStyle.sfsHollow);

				Point ptTmp1 = ARANFunctions.LocalToPrj(End.PrjPt, End.EntryDirection, -End.EPT, -End.ASW_L);
				Point ptTmp2 = ARANFunctions.LocalToPrj(End.PrjPt, End.EntryDirection, -End.EPT, End.ASW_R);

				EndKKLine = new LineString();
				EndKKLine.Add(ptTmp1);
				EndKKLine.Add(ptTmp2);

				EndKKLineElem = GlobalVars.gAranGraphics.DrawLineString(EndKKLine, 2, ARANFunctions.RGB(0, 198, 0));

				ptTmp1 = ARANFunctions.LocalToPrj(End.PrjPt, End.EntryDirection, -End.LPT, -End.ASW_L);
				ptTmp2 = ARANFunctions.LocalToPrj(End.PrjPt, End.EntryDirection, -End.LPT, End.ASW_R);

				EndLPTLine = new LineString();
				EndLPTLine.Add(ptTmp1);
				EndLPTLine.Add(ptTmp2);

				EndLPTLineElem = GlobalVars.gAranGraphics.DrawLineString(EndLPTLine, 2, ARANFunctions.RGB(0, 198, 198));
			}
		}

		public double Altitude;
		//public double TAS;
		//public double IAS;
		public double MOC;
		public double MOCA;

		//public double WindSpeed;
		public double Dir;
		//public double MaxTurnAtStart;
		//public double MaxTurnAtEnd;
		public double TrueTrack;
		public double MagnTrack;

		public double Length;

		public double Dstart;
		public double D1start, D2start;
		public double NavDirStart;

		public double Dend;
		public double D1end, D2end;
		public double NavDirEnd;

		public int NominalTracktElem;
		public int EndLPTLineElem;
		public int EndKKLineElem;

		//public int PrimaryProtectionAreaElem;
		//public int SecondaryProtectionAreaElem;

		public WPTToler startTolerElem;
		public WPTToler endTolerElem;

		public void DeleteGraphics()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(NominalTracktElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(startTolerElem.toleranceElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(startTolerElem.nominalElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(endTolerElem.toleranceElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(endTolerElem.nominalElem);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(EndKKLineElem);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(EndLPTLineElem);

			//GlobalVars.gAranGraphics.SafeDeleteGraphic(SecondaryProtectionAreaElem);
			//GlobalVars.gAranGraphics.SafeDeleteGraphic(PrimaryProtectionAreaElem);

			if (leg != null)
				leg.DeleteGraphics();

			if (Start != null)
				Start.DeleteGraphics();

			if (End != null)
				End.DeleteGraphics();
		}

		//public void CreateLegGeometry()
		//{
		//	double dirAngle = ARANFunctions.ReturnAngleInRadians(Start.PrjPt, End.PrjPt);
		//	double distance = ARANFunctions.ReturnDistanceInMeters(Start.PrjPt, End.PrjPt);

		//	Ring ring = new Ring();
		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, -Start.ATT, Start.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, 0, Start.ASW_2_L));

		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, 0, End.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, End.ATT, End.ASW_2_L));

		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, End.ATT, -End.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, 0, -End.ASW_2_L));

		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, 0, -Start.ASW_2_L));
		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, -Start.ATT, -Start.ASW_2_L));

		//	//PrimaryProtectionArea = new Polygon();
		//	//PrimaryProtectionArea.ExteriorRing = ring;
		//	//==================================================================
		//	ring = new Ring();
		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, -Start.ATT, Start.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, 0, Start.ASW_L));

		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, 0, End.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, End.ATT, End.ASW_L));

		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, End.ATT, -End.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(End.PrjPt, dirAngle, 0, -End.ASW_L));

		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, 0, -Start.ASW_L));
		//	ring.Add(ARANFunctions.LocalToPrj(Start.PrjPt, dirAngle, -Start.ATT, -Start.ASW_L));

		//	//SecondaryProtectionArea = new Polygon();
		//	//SecondaryProtectionArea.ExteriorRing = ring;
		//}

		override public string ToString()
		{
			string result = "";

			if (Start != null && Start.Name != null)
			{
				result = Start.Name;

				if (End != null && End.Name != null)
					result = Start.Name + " - " + End.Name;
			}
			else if (End != null || End.Name != null)
				result = End.Name;

			return result;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct WPTToler
	{
		public MultiLineString tolerance;
		public MultiLineString nominal;

		public int toleranceElem;
		public int nominalElem;
	}

}
