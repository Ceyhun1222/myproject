using System;
using Aran.Aim.Features;
using Aran.PANDA.Common;

namespace Holding
{
	public class VORDMECoverage
	{
		//private double _vt, _dt;
		private string _pBN;
		private Aran.Geometries.Point _WYPoint;
		private Navaid _refFacility;
		private double _direction;

        public VORDMECoverage(Navaid vorDme, Aran.Geometries.Point prjPoint, string PBN, double direction)
		{
			_refFacility = vorDme;
			_WYPoint = prjPoint;
			_pBN = PBN;
			_direction = direction;
			CalculateATTXTT();
		}

        public Aran.Geometries.Point WyPoint
		{
			set { _WYPoint = value;CalculateATTXTT();}
		}								   										   	

		public Navaid RefFacility
		{
			set { _refFacility = value; CalculateATTXTT(); }
		}

		public double Direction
		{
			set { _direction = value; CalculateATTXTT(); }
		}
	
		public double ATT { get; private set; }
		public double XTT { get; private set; }
		public double D { get; private set; }
		public double D1 { get; private set; }
		public double D2 { get; private set; }

        private double CalculatDTT(double D)
        {

            double alphaAir, alphaSis = 92.6;
            double docMin = D * 0.00125;
            double alphaAirMin = 157.42;
            alphaAir = docMin < alphaAirMin ? alphaAirMin : docMin;
            return 2 * Math.Sqrt((alphaAir * alphaAir + alphaSis * alphaSis));
        }
				
		private void CalculateATTXTT()
		{
			double vorSystemAccuracy, sigma,
					VT, DT, ADT, AVT, FTT,DTT;
            var navEquipment = _refFacility.NavaidEquipment.Find(navComp => navComp.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.VOR);
            if (navEquipment == null)
                return;
            var vor = GlobalParams.Database.HoldingQpi.GetVor(navEquipment.TheNavaidEquipment.Identifier);
            Aran.Geometries.Point ptNav = GlobalParams.SpatialRefOperation.ToPrj(vor.Location.Geo);
			vorSystemAccuracy = 4.5 * Math.PI / 180;

			D = ARANFunctions.ReturnDistanceInMeters(ptNav, _WYPoint);
			D2 = ARANFunctions.Point2LineDistancePrj(ptNav, _WYPoint, _direction);
			D1 = Math.Sqrt(D * D - D2 * D2);
			sigma = Math.Atan(D2 / D1);
			DTT = CalculatDTT(D);
			VT = D1 - D * Math.Cos(sigma + vorSystemAccuracy);
			DT = DTT * Math.Cos(sigma);
			ADT = DTT * Math.Sin(sigma);
			AVT = D2 - D * Math.Sin(sigma- vorSystemAccuracy);
			ATT =  Math.Sqrt(AVT * AVT + ADT * ADT + InitHolding.SysCompTolerance * InitHolding.SysCompTolerance);
			FTT =GlobalParams.Constant_G.Fte_ConstantsList[_pBN].Value;
			XTT = Math.Sqrt(VT * VT + DT * DT + FTT * FTT + InitHolding.SysCompTolerance * InitHolding.SysCompTolerance);
		//	DrawParams(D, D1, D2);	
		}

		private void DrawParams(double D, double D1, double D2)
		{
			GlobalParams.UI.SafeDeleteGraphic(_toleranceHandle);
			GlobalParams.UI.SafeDeleteGraphic(_fixPointHandle);
			GlobalParams.UI.SafeDeleteGraphic(_refFacilityHandle);
			GlobalParams.UI.SafeDeleteGraphic(_dir);
			GlobalParams.UI.SafeDeleteGraphic(_D2Handle);
			
										  			
			Aran.Geometries.Ring tolerance = ARANFunctions.ToleranceArea(_WYPoint, ATT, XTT, _direction, SideDirection.sideRight);
			_toleranceHandle = GlobalParams.UI.DrawRing(tolerance,Aran.AranEnvironment.Symbols.eFillStyle.sfsNull, 1);
			_fixPointHandle = GlobalParams.UI.DrawPointWithText(_WYPoint, "FixPoint", 1);
			Aran.Geometries.Point ptDir1 = ARANFunctions.LocalToPrj(_WYPoint, _direction, 10000,0);
			Aran.Geometries.Point ptDir2 = ARANFunctions.LocalToPrj(_WYPoint, _direction, -1000.0,0);
			Aran.Geometries.LineString partDir = new Aran.Geometries.LineString();
			partDir.Add(ptDir1);
			partDir.Add(ptDir2);
			_dir = GlobalParams.UI.DrawLineString(partDir, 2, 1);
			Aran.Geometries.Point ptD2 = ARANFunctions.LocalToPrj(_WYPoint, _direction, D2, 0);
			_D2Handle =  GlobalParams.UI.DrawPointWithText(ptD2, "PtD2", 1);

		}

		private int _toleranceHandle,_fixPointHandle,_refFacilityHandle,_dir,_D2Handle;

		

	}
}
