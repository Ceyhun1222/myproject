using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIXM45Loader
{
    public class AIXM45_RouteSegment : AIXM45_Object
    {
        private string _codeType;
        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private string _codeRnp;
        public string CodeRnp
        {
            get { return _codeRnp; }
            set { _codeRnp = value; }
        }

        private string _codeLvl;
        public string CodeLvl
        {
            get { return _codeLvl; }
            set { _codeLvl = value; }
        }

        private string _codeClassAcft;
        public string CodeClassAcft
        {
            get { return _codeClassAcft; }
            set { _codeClassAcft = value; }
        }

        private string _codeIntl;
        public string CodeIntl
        {
            get { return _codeIntl; }
            set { _codeIntl = value; }
        }

        private string _codeTypeFltRule;
        public string CodeTypeFltRule
        {
            get { return _codeTypeFltRule; }
            set { _codeTypeFltRule = value; }
        }

        private string _codeCiv;
        public string CodeCiv
        {
            get { return _codeCiv; }
            set { _codeCiv = value; }
        }

        private double _valDistVerUpper;
        public double ValDistVerUpper
        {
            get { return _valDistVerUpper; }
            set { _valDistVerUpper = value; }
        }
        
        private string _uomDistVerUpper;
        public string UomDistVerUpper
        {
            get { return _uomDistVerUpper; }
            set { _uomDistVerUpper = value; }
        }
 
        private string _codeDistVerUpper;
        public string CodeDistVerUpper
        {
            get { return _codeDistVerUpper; }
            set { _codeDistVerUpper = value; }
        }

        private double _valDistVerLower;
        public double ValDistVerLower
        {
            get { return _valDistVerLower; }
            set { _valDistVerLower = value; }
        }

        private string _uomDistVerLower;
        public string UomDistVerLower
        {
            get { return _uomDistVerLower; }
            set { _uomDistVerLower = value; }
        }

        private string _codeDistVerLower;
        public string CodeDistVerLower
        {
            get { return _codeDistVerLower; }
            set { _codeDistVerLower = value; }
        }

        private double _valDistVerMnm;
        public double ValDistVerMnm
        {
            get { return _valDistVerMnm; }
            set { _valDistVerMnm = value; }
        }

        private string _uomDistVerMnm;
        public string UomDistVerMnm
        {
            get { return _uomDistVerMnm; }
            set { _uomDistVerMnm = value; }
        }

        private string _codeDistVerMnm;
        public string CodeDistVerMnm
        {
            get { return _codeDistVerMnm; }
            set { _codeDistVerMnm = value; }
        }

        private double _valDistVerLowerOvrde;
        public double ValDistVerLowerOvrde
        {
            get { return _valDistVerLowerOvrde; }
            set { _valDistVerLowerOvrde = value; }
        }

        private string _uomDistVerLowerOvrde;
        public string UomDistVerLowerOvrde
        {
            get { return _uomDistVerLowerOvrde; }
            set { _uomDistVerLowerOvrde = value; }
        }

        private string _codeDistVerLowerOvrde;
        public string CodeDistVerLowerOvrde
        {
            get { return _codeDistVerLowerOvrde; }
            set { _codeDistVerLowerOvrde = value; }
        }

        private double _valWid;
        public double ValWid
        {
            get { return _valWid; }
            set { _valWid = value; }
        }

        private string _uomWid;
        public string UomWid
        {
            get { return _uomWid; }
            set { _uomWid = value; }
        }

        private string _codeRepAtcStart;
        public string CodeRepAtcStart
        {
            get { return _codeRepAtcStart; }
            set { _codeRepAtcStart = value; }
        }

        private string _codeRepAtcEnd;
        public string CodeRepAtcEnd
        {
            get { return _codeRepAtcEnd; }
            set { _codeRepAtcEnd = value; }
        }

        private string _codeRvsmStart;
        public string CodeRvsmStart
        {
            get { return _codeRvsmStart; }
            set { _codeRvsmStart = value; }
        }

        private string _codeRvsmEnd;
        public string CodeRvsmEnd
        {
            get { return _codeRvsmEnd; }
            set { _codeRvsmEnd = value; }
        }

        private string _codeTypePath;
        public string CodeTypePath
        {
            get { return _codeTypePath; }
            set { _codeTypePath = value; }
        }

        private double _valTrueTrack;
        public double ValTrueTrack
        {
            get { return _valTrueTrack; }
            set { _valTrueTrack = value; }
        }

        private double _valMagTrack;
        public double ValMagTrack
        {
            get { return _valMagTrack; }
            set { _valMagTrack = value; }
        }

        private double _valReversTrueTrack;
        public double ValReversTrueTrack
        {
            get { return _valReversTrueTrack; }
            set { _valReversTrueTrack = value; }
        }

        private double _valReversMagTrack;
        public double ValReversMagTrack
        {
            get { return _valReversMagTrack; }
            set { _valReversMagTrack = value; }
        }

        private double _valLen;
        public double ValLen
        {
            get { return _valLen; }
            set { _valLen = value; }
        }

        private double _valCopDist;
        public double ValCopDist
        {
            get { return _valCopDist; }
            set { _valCopDist = value; }
        }

        private string _uomDist;
        public string UomDist
        {
            get { return _uomDist; }
            set { _uomDist = value; }
        }

        private string _txtRmk;
        public string TxtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }

        private string _R_RteMid;
        public string R_RteMid
        {
            get { return _R_RteMid; }
            set { _R_RteMid = value; }
        }


        private string _R_SignificantPointStaMid;
        public string R_SignificantPointStaMid
        {
            get { return _R_SignificantPointStaMid; }
            set { _R_SignificantPointStaMid = value; }
        }

        private string _R_SignificantPointStacode_Id;
        public string R_SignificantPointStacode_Id
        {
            get { return _R_SignificantPointStacode_Id; }
            set { _R_SignificantPointStacode_Id = value; }
        }

        private string _R_SignificantPointSta_LAT;
        public string R_SignificantPointSta_LAT
        {
            get { return _R_SignificantPointSta_LAT; }
            set { _R_SignificantPointSta_LAT = value; }
        }

        private string _R_SignificantPointSta_LONG;
        public string R_SignificantPointSta_LONG
        {
            get { return _R_SignificantPointSta_LONG; }
            set { _R_SignificantPointSta_LONG = value; }
        }

        private string _R_SignificantPointEndMid;
        public string R_SignificantPointEndMid
        {
            get { return _R_SignificantPointEndMid; }
            set { _R_SignificantPointEndMid = value; }
        }

        private string _R_SignificantPointEndcode_Id;
        public string R_SignificantPointEndcode_Id
        {
            get { return _R_SignificantPointEndcode_Id; }
            set { _R_SignificantPointEndcode_Id = value; }
        }

        private string _R_SignificantPointEnd_LAT;
        public string R_SignificantPointEnd_LAT
        {
            get { return _R_SignificantPointEnd_LAT; }
            set { _R_SignificantPointEnd_LAT = value; }
        }

        private string _R_SignificantPointEnd_LONG;
        public string R_SignificantPointEnd_LONG
        {
            get { return _R_SignificantPointEnd_LONG; }
            set { _R_SignificantPointEnd_LONG = value; }
        }

        private AIXM45_PointChoice _R_SignificantPointSta_PointChoice;
        public AIXM45_PointChoice R_SignificantPointSta_PointChoice
        {
            get { return _R_SignificantPointSta_PointChoice; }
            set { _R_SignificantPointSta_PointChoice = value; }
        }

        private AIXM45_PointChoice _R_SignificantPointEnd_PointChoice;
        public AIXM45_PointChoice R_SignificantPointEnd_PointChoice
        {
            get { return _R_SignificantPointEnd_PointChoice; }
            set { _R_SignificantPointEnd_PointChoice = value; }
        }


        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }

        public AIXM45_RouteSegment()
        {
        }

        public enum AIXM45_PointChoice
        {
            DesignatedPoint = 0,
            Navaid = 1,
            TouchDownLiftOff = 2,
            RunwayCenterlinePoint = 3,
            AirportHeliport = 4,
            NONE = 5,
            OTHER =6,
            VHFNavaid = 1,
        }
    }
}
