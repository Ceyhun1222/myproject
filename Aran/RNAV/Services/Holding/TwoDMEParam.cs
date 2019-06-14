using System;
using System.Collections.Generic;
using Aran.Geometries;

namespace Holding
{
    public class TwoDMEParam : IEqualityComparer<TwoDMEParam>
    {
        public TwoDMEParam(Aran.Geometries.Point pt1,Aran.Geometries.Point pt2,double doc)
        {
            PtDME1 = pt1;
            PtDME2 = pt2;
            Doc = doc;
        }
        
        public Aran.Geometries.Point  PtDME1 { get; set; }
        public Aran.Geometries.Point PtDME2 { get; set; }
        public double Doc { get; set; }
        public MultiPolygon TwoDMEGeom { get; set; }


        public bool Equals(TwoDMEParam x, TwoDMEParam y)
        {
            if (((x.PtDME1.X == y.PtDME1.X) && (x.PtDME1.Y == y.PtDME1.Y)) && ((x.PtDME2.X== y.PtDME2.X) && (x.PtDME2.Y== y.PtDME2.Y)) && x.Doc==y.Doc)
                return true;
            else 
            if (((x.PtDME1.X== y.PtDME2.X)&& (x.PtDME1.Y == y.PtDME2.Y)) && ((x.PtDME2.X== y.PtDME1.X) && (x.PtDME2.Y== y.PtDME1.Y)) &&x.Doc== y.Doc)
                return true;
            return false;
            
        }

        public int GetHashCode(TwoDMEParam obj)
        {
            return base.GetHashCode();
        }
    }
}