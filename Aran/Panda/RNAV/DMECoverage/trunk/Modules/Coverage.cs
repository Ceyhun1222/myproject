using System;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.DMECoverage
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class CDMECoverage
	{
		public const double Catet = 0.5 * ARANMath.C_SQRT3;
		public const double DMENoUpdateZoneRadius = 5556;		//1852.0;	//4130.0;

		public const double DMEMaxOperationalCoverage = 296320.0;
		public const double NavSignalReception = 4130.0;
		public const double DMEMaxOperationalAltitude = DMEMaxOperationalCoverage * DMEMaxOperationalCoverage / (NavSignalReception * NavSignalReception);

		public NavaidType DME1
		{
			get { return _DME1; }

			set
			{
				_DME1 = value;
				CreateAvailableZones();
			}
		}

		public NavaidType DME2
		{
			get { return _DME2; }

			set
			{
				_DME2 = value;
				CreateAvailableZones();
			}
		}

		public double Altitude
		{
			get { return _altitude; }

			set
			{
				if (value > DMEMaxOperationalAltitude)
					value = DMEMaxOperationalAltitude;

				if (value != _altitude && value <= DMEMaxOperationalAltitude)
				{
					_altitude = value;
					_maxDist = Math.Sqrt(_altitude) * NavSignalReception;
					if (_maxDist > DMEMaxOperationalCoverage)
						_maxDist = DMEMaxOperationalCoverage;

					CreateAvailableZones();
				}
			}
		}

	    public double Distance { get; set; }

		public MultiPolygon AvailableZone { get { return _availableZone; } }

		private Point _center1;
		private Point _center2;
		private MultiPolygon _availableZone;
		private NavaidType _DME1;
		private NavaidType _DME2;

		private double _altitude;
		private double _maxDist;
		private double _dist;
		private double _dir;

		private int _elemDME1;
		private int _elemDME2;
		private int _elemInterceptCircle1;
		private int _elemInterceptCircle2;
		private int _elemOperationalCoverage1;
		private int _elemOperationalCoverage2;
		private int _elemAvailableZone;

		public CDMECoverage()
		{
			_altitude = 100.0;
			_dist = 0.0;
			_dir = 0.0;
			_maxDist = Math.Sqrt(_altitude) * NavSignalReception;

			_availableZone = new MultiPolygon();
			_center1 = new Point();
			_center2 = new Point();

			_elemDME1 = -1;
			_elemDME2 = -1;
			_elemInterceptCircle1 = -1;
			_elemInterceptCircle2 = -1;
			_elemOperationalCoverage1 = -1;
			_elemOperationalCoverage2 = -1;
			_elemAvailableZone = -1;
		}

		public CDMECoverage(double altitude, NavaidType dme1, NavaidType dme2)
		{
			_altitude = altitude + 100.0;
			_dist = 0.0;
			_dir = 0.0;
			_maxDist = Math.Sqrt(_altitude) * NavSignalReception;

			_availableZone = new MultiPolygon();
			_center1 = new Point();
			_center2 = new Point();

			_elemDME1 = -1;
			_elemDME2 = -1;
			_elemInterceptCircle1 = -1;
			_elemInterceptCircle2 = -1;
			_elemOperationalCoverage1 = -1;
			_elemOperationalCoverage2 = -1;
			_elemAvailableZone = -1;

			_DME1 = dme1;
			_DME2 = dme2;

			Altitude = altitude;
		}

		~CDMECoverage()
		{
			ClearImages();
		}

		private void CreateAvailableZones()
		{
			if (!(_DME1.pPtPrj != null) || !(_DME2.pPtPrj != null))
				return;

			_dist = ARANMath.Hypot(_DME1.pPtPrj.Y - _DME2.pPtPrj.Y, _DME1.pPtPrj.X - _DME2.pPtPrj.X);

			if (_dist > 2.0 * _maxDist)
			{
				_availableZone.Clear();
				return;
			}

			Polygon OperationalCoverage1 = new Polygon();
			Polygon OperationalCoverage2 = new Polygon();

			Polygon InterceptCircle1 = new Polygon();
			Polygon InterceptCircle2 = new Polygon();

			Polygon DMENoUpdateZone1 = new Polygon();
			Polygon DMENoUpdateZone2 = new Polygon();

			Ring ring;

			_dir = Math.Atan2(_DME2.pPtPrj.Y - _DME1.pPtPrj.Y, _DME2.pPtPrj.X - _DME1.pPtPrj.X);
			_center1 = ARANFunctions.LocalToPrj(_DME1.pPtPrj, _dir, 0.5 * _dist, Catet * _dist);
			_center2 = ARANFunctions.LocalToPrj(_DME1.pPtPrj, _dir, 0.5 * _dist, -Catet * _dist);
			ring = ARANFunctions.CreateCirclePrj(_DME1.pPtPrj, _maxDist);
			OperationalCoverage1.ExteriorRing = ring;

			ring = ARANFunctions.CreateCirclePrj(_center1, _dist);
			InterceptCircle1.ExteriorRing = ring;

			ring = ARANFunctions.CreateCirclePrj(_DME2.pPtPrj, _maxDist);
			OperationalCoverage2.ExteriorRing = ring;


			ring = ARANFunctions.CreateCirclePrj(_center2, _dist);
			InterceptCircle2.ExteriorRing = ring;

			ring = ARANFunctions.CreateCirclePrj(_DME1.pPtPrj, DMENoUpdateZoneRadius);
			DMENoUpdateZone1.ExteriorRing = ring;

			ring = ARANFunctions.CreateCirclePrj(_DME2.pPtPrj, DMENoUpdateZoneRadius);
			DMENoUpdateZone2.ExteriorRing = ring;
			// ==============================================================================
			GeometryOperators geoop = new GeometryOperators();

			//GlobalVars.gAranGraphics.DrawPolygon(InterceptCircle1, -1, eFillStyle.sfsVertical);
			//GlobalVars.gAranGraphics.DrawPolygon(OperationalCoverage1, -1, eFillStyle.sfsHorizontal);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(TmpPolygon1, -1, eFillStyle.sfsBackwardDiagonal);
			//while(true)
			//System.Windows.Forms.Application.DoEvents();

			MultiPolygon tmpPolygon0 = (MultiPolygon)geoop.Intersect(InterceptCircle1, OperationalCoverage1);

			//GlobalVars.gAranGraphics.DrawMultiPolygon(TmpPolygon0, -1, eFillStyle.sfsBackwardDiagonal);
			//GlobalVars.gAranGraphics.DrawPolygon(OperationalCoverage2, -1, eFillStyle.sfsForwardDiagonal);
			//while(true)
			//System.Windows.Forms.Application.DoEvents();

			MultiPolygon TmpPolygon1 = (MultiPolygon)geoop.Intersect(tmpPolygon0, OperationalCoverage2);

			//GlobalVars.gAranGraphics.DrawPolygon(OperationalCoverage2, -1, eFillStyle.sfsSolid);
			//GlobalVars.gAranGraphics.DrawMultiPolygon(TmpPolygon1, -1, eFillStyle.sfsSolid);
			//while(true)
			//System.Windows.Forms.Application.DoEvents();


			tmpPolygon0 = (MultiPolygon)geoop.Difference(TmpPolygon1, InterceptCircle2);
			if (tmpPolygon0 == null)
			{
				_availableZone.Clear();
				return;
			}

			TmpPolygon1 = (MultiPolygon)geoop.Difference(tmpPolygon0, DMENoUpdateZone1);

			if (TmpPolygon1 == null)
			{
				_availableZone.Clear();
				return;
			}

			MultiPolygon AvailableZone1 = (MultiPolygon)geoop.Difference(TmpPolygon1, DMENoUpdateZone2);
			// ==============================================================================
			tmpPolygon0 = (MultiPolygon)geoop.Intersect(InterceptCircle2, OperationalCoverage1);
			TmpPolygon1 = (MultiPolygon)geoop.Intersect(tmpPolygon0, OperationalCoverage2);
			tmpPolygon0 = (MultiPolygon)geoop.Difference(TmpPolygon1, InterceptCircle1);
			TmpPolygon1 = (MultiPolygon)geoop.Difference(tmpPolygon0, DMENoUpdateZone1);
			tmpPolygon0 = (MultiPolygon)geoop.Difference(TmpPolygon1, DMENoUpdateZone2);
			TmpPolygon1 = (MultiPolygon)geoop.UnionGeometry(tmpPolygon0, AvailableZone1);
			_availableZone.Assign(TmpPolygon1);
			// ==============================================================================
		}

		public void DrawPolygons()
		{
			Ring Ring;
			Polygon InterceptCircle1;
			Polygon InterceptCircle2;
			Polygon OperationalCoverage1;
			Polygon OperationalCoverage2;

			OperationalCoverage1 = new Polygon();
			OperationalCoverage2 = new Polygon();
			InterceptCircle1 = new Polygon();
			InterceptCircle2 = new Polygon();
			Ring = ARANFunctions.CreateCirclePrj(_DME1.pPtPrj, _maxDist);
			OperationalCoverage1.ExteriorRing = Ring;

			Ring = ARANFunctions.CreateCirclePrj(_DME2.pPtPrj, _maxDist);
			OperationalCoverage2.ExteriorRing = Ring;

			Ring = ARANFunctions.CreateCirclePrj(_center1, _dist);
			InterceptCircle1.ExteriorRing = Ring;

			Ring = ARANFunctions.CreateCirclePrj(_center2, _dist);
			InterceptCircle2.ExteriorRing = Ring;

			ClearImages();

			_elemAvailableZone = GlobalVars.gAranGraphics.DrawMultiPolygon(_availableZone, eFillStyle.sfsCross, ARANFunctions.RGB(0, 255, 0));
			_elemDME1 = GlobalVars.gAranGraphics.DrawPointWithText(DME1.pPtPrj, DME1.CallSign, 255);
			_elemDME2 = GlobalVars.gAranGraphics.DrawPointWithText(DME2.pPtPrj, DME2.CallSign, 255);
			_elemOperationalCoverage1 = GlobalVars.gAranGraphics.DrawPolygon(OperationalCoverage1, eFillStyle.sfsNull, 255);
			_elemOperationalCoverage2 = GlobalVars.gAranGraphics.DrawPolygon(OperationalCoverage2, eFillStyle.sfsNull, 255);
			_elemInterceptCircle1 = GlobalVars.gAranGraphics.DrawPolygon(InterceptCircle1, eFillStyle.sfsNull, 0);
			_elemInterceptCircle2 = GlobalVars.gAranGraphics.DrawPolygon(InterceptCircle2, eFillStyle.sfsNull, 0);
		}

		public void ClearImages()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemDME1);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemDME2);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemInterceptCircle1);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemInterceptCircle2);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemOperationalCoverage1);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemOperationalCoverage2);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemAvailableZone);
		}

	    public void DrawOnlyAvailableZone()
	    {
	        ClearImages();

	        _elemAvailableZone = GlobalVars.gAranGraphics.DrawMultiPolygon(_availableZone, eFillStyle.sfsNull, ARANFunctions.RGB(255, 0, 0));
        }

	    public int Tag { get; set; }

	} // end TDMECoverage

}

/*
 Soz: Ilqar Fehmi ,
 Suleyman Lokbatanli ve Aydin Saninin ifasinda ...

Meni axtar darixanda,
 Seni qem-qusse sixanda,
* Yorulub bezsen heyatdan
 Yorulub sersem heyatdan
 Gozunun yasi axanda
 Meni axtar darixanda.

Meni axtar bos otagda,
 Yoluna kolge cixanda
 Soyumus guzguden bir qiz,
 Sene hiddetle baxanda
 Meni axtar darixanda.

Cixmaram birce sozunden,
 Seni allam yer uzunden.
 Cekerem mavi semaya
 Operem nemli gozunden.

Meni axtar gece yari
 Yolumu taksi kesende.
 Dayanib yol kenarinda
 Buzusub tir-tir esende
 Meni axtar darixanda.

Kucede yolda yagisda
 Sevisende, gulusende,
 Masinin penceresinden
 Uzune damci dusende
 Meni axtar darixanda.

Cixmaram birce sozunden,
 Seni allam yer uzunden.
 Cekerem mavi semaya,
 Operem nemli gozunden.

 */