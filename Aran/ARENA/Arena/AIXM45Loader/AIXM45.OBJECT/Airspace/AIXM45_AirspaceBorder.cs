using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_AirspaceBorderItem
    {
        private AIXM45_AirspaceVertex[] _Gbr;
        public AIXM45_AirspaceVertex[] Gbr
        {
            get { return _Gbr; }
            set { _Gbr = value; }
        }

        private BorderItemCodeType _codeType;
        public BorderItemCodeType CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private AIXM45_AirspaceVertex _FinalPoint;
        public AIXM45_AirspaceVertex FinalPoint
        {
            get { return _FinalPoint; }
            set { _FinalPoint = value; }
        }

        private AIXM45_AirspaceVertex _CenterPoint;
        public AIXM45_AirspaceVertex CenterPoint
        {
            get { return _CenterPoint; }
            set { _CenterPoint = value; }
        }

        private double _radius;
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }


        private string _uomRadius;
        public string UomRadius
        {
            get { return _uomRadius; }
            set { _uomRadius = value; }
        }


        public AIXM45_AirspaceBorderItem()
        {
        }
    }

    public enum BorderItemCodeType
    {
        FNT = 0,
        CWA = 1,
        CCA = 2,
        PNT= 3,
        CRCL =4,
    }

    public class AIXM45_AirspaceVertex
    {
        private WKSPoint _vrtx;

        public WKSPoint Vrtx
        {
            get { return _vrtx; }
            set { _vrtx = value; }
        }
        private string _crcCode;

        public string CrcCode
        {
            get { return _crcCode; }
            set { _crcCode = value; }
        }

        public AIXM45_AirspaceVertex()
        {
        }

    }
   
}
