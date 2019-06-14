using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Holding
{
    public  class DmeCoverageOperation
    {
        #region :>Fields
        const double zoneCircle = 5556;
        private const double ST = 463;
        private FteConstantList _fteConstants;
        private List<TwoDMEParam> _twoDMEParamList;
        #endregion
       
        #region :>Ctor
        public DmeCoverageOperation()
        {
            _fteConstants = GlobalParams.Constant_G.Fte_ConstantsList;
            DmeCoverageParamList = new List<DmeCoverageParam>();
            _twoDMEParamList = new List<TwoDMEParam>();
        }
        #endregion

        #region :>Property
        public List<DmeCoverageParam> DmeCoverageParamList { get; set; }
        #endregion

        #region :>Methods

        public double ATT(DmeCoverageType dmeCovType, double doc, string spesificNavigation)
        {
            double dtt = DTT(dmeCovType, doc);
            return Math.Sqrt(dtt * dtt + ST * ST);
        }

        public double XTT(DmeCoverageType dmeCovType, double doc, string spesificNavigation)
        {
            double dtt = DTT(dmeCovType, doc);
            double fte = _fteConstants[spesificNavigation].Value;
            return Math.Sqrt(dtt * dtt + fte * fte + ST * ST);
        }
        
        public void CreateDMECoverage(List<Navaid> dmeList, DmeCoverageType dmeCovType, Point segmentPoint, double doc, ref MultiPolygon result)
        {
            DmeCoverageParam tmpCoverageParam =
                new DmeCoverageParam
                {
                    DmeCovType = dmeCovType,
                    NavList = dmeList,
                    RefPoint = segmentPoint,
                    Doc = doc
                };


            DmeCoverageParam dmeParam = DmeCoverageParamList.Find(
                delegate(DmeCoverageParam dmecoverageParam)
                {
                    if (dmecoverageParam.Equals(dmecoverageParam, tmpCoverageParam))
                        return true;
                    else
                        return false;
                });

            if (dmeParam != null)
            {
                if (dmeParam.DmeCovType == tmpCoverageParam.DmeCovType)
                {
                    if (tmpCoverageParam.DmeCovType == DmeCoverageType.twoDme)
                        result = dmeParam.TwoDmeGeom;
                    else
                        result = dmeParam.ThreeDmeGeom;
                }
                else
                {
                    Aran.Geometries.Polygon tmpPolygon = new Polygon();
                    if (tmpCoverageParam.DmeCovType == DmeCoverageType.twoDme)
                    {
                        result = dmeParam.TwoDmeGeom;
                    }
                    else
                    {
                        ThreeDmeCoverageList(dmeParam.DmeCoverageList, ref result);
                        dmeParam.ThreeDmeGeom = result;
                        dmeParam.DmeCovType = DmeCoverageType.threeDme;
                    }
                }
            }
            else
            {
                MultiPolygon twoDmeGeom = new MultiPolygon();
                tmpCoverageParam.DmeCoverageList = TwoDmeCoverageList(tmpCoverageParam.NavList, segmentPoint, doc, ref twoDmeGeom);
                tmpCoverageParam.TwoDmeGeom = twoDmeGeom;
                if (tmpCoverageParam.DmeCovType == DmeCoverageType.threeDme)
                {
                    ThreeDmeCoverageList(tmpCoverageParam.DmeCoverageList, ref result);
                    tmpCoverageParam.ThreeDmeGeom = result;
                }
                else
                    result = twoDmeGeom;
                DmeCoverageParamList.Add(tmpCoverageParam);
            }


        }
       
        private void ThreeDmeCoverageList(List<DmeCoverage> twoDmeCoverageList, ref MultiPolygon result)
        {
            Polygon twoCoveragePolgon = new Polygon();
            //List<DmeCoverage> twoDmeCoverageList = TwoDmeCoverageList(dmeHoldingList, segmentPoint, doc,ref twoCoveragePolgon);
            for (int i = 0; i < twoDmeCoverageList.Count - 1; i++)
            {
                for (int j = i + 1; j < twoDmeCoverageList.Count; j++)
                {
                    MultiPolygon tmpPolygon =(MultiPolygon) GlobalParams.GeomOperators.Intersect(twoDmeCoverageList[i].geom, twoDmeCoverageList[j].geom);
                    result =(MultiPolygon)GlobalParams.GeomOperators.UnionGeometry(result, tmpPolygon);
                }
            }
            //_dmeCoverageParamList[_dmeCoverageParamList.Count - 1].DmeCovType = DmeCoverageType.threeDme;
            //_dmeCoverageParamList[_dmeCoverageParamList.Count - 1].ThreeDmeGeom = result ;

        }

        private List<DmeCoverage> TwoDmeCoverageList(List<Navaid> dmeList, Point segmentPoint, double doc,ref MultiPolygon result)
        {
            bool isInside;
            List<DmeCoverage> dmeCoverageList = new List<DmeCoverage>();
            for (int i = 0; i < dmeList.Count - 1; i++)
            {
                var navEquipment1 = dmeList[i].NavaidEquipment.Find(navComp => navComp.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.DME);
                if (navEquipment1 == null)
                    continue;
                var dme1 = GlobalParams.Database.HoldingQpi.GetDme(navEquipment1.TheNavaidEquipment.Identifier);

                for (int j = i + 1; j < dmeList.Count; j++)
                {
                    var navEquipment2 = dmeList[j].NavaidEquipment.Find(navComp => navComp.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.DME);
                    if (navEquipment2 == null)
                        continue;
                    var dme2 = GlobalParams.Database.HoldingQpi.GetDme(navEquipment2.TheNavaidEquipment.Identifier);

                    MultiPolygon tmpPolygon = TwoDmeCoverage(new TwoDMEParam(GeomFunctions.AssignToPrj(dme1.Location),GeomFunctions.AssignToPrj(dme2.Location), doc));
                    result =(MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(tmpPolygon, result);
                    isInside = GlobalParams.GeomOperators.Contains(tmpPolygon, segmentPoint);
                    List<Navaid> covDmeList = new List<Navaid> { dmeList[i], dmeList[j] };
                    DmeCoverage dmeCoverage = new DmeCoverage { geom = tmpPolygon, CovDmeList = covDmeList, IsValidated = isInside };
                    dmeCoverageList.Add(dmeCoverage);
                }
        
            }
            return dmeCoverageList;
        }

        private double DTT(DmeCoverageType dmeCovType, double doc)
        {
            double alphaAir, DTT, alphaSis = 92.6;
            double docMin = doc * 0.00125;
            double alphaAirMin = 157.42;
            alphaAir = docMin < alphaAirMin ? alphaAirMin : docMin;
            DTT = 2 * Math.Sqrt(2 * (alphaAir * alphaAir + alphaSis * alphaSis));
            if (dmeCovType == DmeCoverageType.twoDme)
                DTT *= 2;
            return DTT;
        }
		
        private MultiPolygon TwoDmeCoverage(TwoDMEParam twoDmeParam)
        {				
            double radius,angledme1Todme2;
            Polygon circle1 = new Polygon(), circle2 = new Polygon(),coverageDmeA = new Polygon(),coverageDmeB = new Polygon(),
                circleA = new Polygon(),circleB = new Polygon();
            Geometry tmpGeo;
            Geometry geomTop = null,geomBottom= null;
            Aran.Geometries.Point ptCenter1,ptCenter2;

            GeometryOperators geomOperators  = GlobalParams.GeomOperators;

            TwoDMEParam dmeParam = _twoDMEParamList.Find(delegate(TwoDMEParam dMEParam)
            {
                if (dMEParam.Equals(twoDmeParam, dMEParam))
                    return true;
                else
                    return false;
            });
            if (dmeParam != null)
                return dmeParam.TwoDMEGeom;
            radius = ARANFunctions.ReturnDistanceInMeters(twoDmeParam.PtDME1, twoDmeParam.PtDME2);
            angledme1Todme2 = ARANFunctions.ReturnAngleInRadians(twoDmeParam.PtDME1,twoDmeParam.PtDME2);
			
            ptCenter1 =  ARANFunctions.LocalToPrj(twoDmeParam.PtDME1,angledme1Todme2,radius/2,radius/2 * Math.Tan(Math.PI/3));
            ptCenter2 = ARANFunctions.LocalToPrj(twoDmeParam.PtDME1, angledme1Todme2, radius / 2, -radius / 2 * Math.Tan(Math.PI / 3));

            circle1.ExteriorRing = ARANFunctions.CreateCirclePrj(ptCenter1, radius);
            circle2.ExteriorRing = ARANFunctions.CreateCirclePrj(ptCenter2, radius);
			
            geomTop = geomOperators.Difference(circle1, circle2);
            geomBottom = geomOperators.Difference(circle2,circle1);

            coverageDmeA.ExteriorRing = ARANFunctions.CreateCirclePrj(twoDmeParam.PtDME1, twoDmeParam.Doc);
            coverageDmeB.ExteriorRing = ARANFunctions.CreateCirclePrj(twoDmeParam.PtDME2, twoDmeParam.Doc);
            circleA.ExteriorRing = ARANFunctions.CreateCirclePrj(twoDmeParam.PtDME1, zoneCircle);
            circleB.ExteriorRing = ARANFunctions.CreateCirclePrj(twoDmeParam.PtDME2, zoneCircle);
			
            tmpGeo = geomOperators.Intersect(coverageDmeA, geomTop);
            tmpGeo = geomOperators.Intersect(coverageDmeB, tmpGeo) ;
            tmpGeo = geomOperators.Difference(tmpGeo, circleA);
            Geometry left = geomOperators.Difference(tmpGeo, circleB);

            tmpGeo = geomOperators.Intersect(coverageDmeA, geomBottom);
            tmpGeo = geomOperators.Intersect(coverageDmeB, tmpGeo);
            tmpGeo = geomOperators.Difference(tmpGeo, circleA);
            Geometry right = geomOperators.Difference(tmpGeo, circleB);
            twoDmeParam.TwoDMEGeom =(MultiPolygon) geomOperators.UnionGeometry(left, right);
            _twoDMEParamList.Add(twoDmeParam);

            return twoDmeParam.TwoDMEGeom;
            #region draw
            //ui.DrawPointWithText(twoDmeParam.PtDME1, 1, "PtDme1");
            //ui.DrawPointWithText(ptDme2, 2, "PtDme2");
            //ui.DrawPointWithText(ptCenter1, 2, "ptCenter1");
            //ui.DrawPointWithText(ptCenter2, 2, "ptCenter2");

            //ui.DrawPolygon(circle1, 1, eFillStyle.sfsNull);
            //ui.DrawPolygon(circle2, 1, eFillStyle.sfsNull);
            //ui.DrawPolygon(geomTop as Polygon, 1, eFillStyle.sfsCross);
            //ui.DrawPolygon(geomBottom as Polygon, 1, eFillStyle.sfsHorizontal);
            //ui.DrawPolygon(right, 1, eFillStyle.sfsHollow);
            //ui.DrawPolygon(left, 1, eFillStyle.sfsCross);
            ////ui.DrawPolygon(coverageDmeA, 1, eFillStyle.sfsNull);
            //ui.DrawPolygon(coverageDmeB, 1, eFillStyle.sfsVertical);
            # endregion
        }

        #endregion
    }
}