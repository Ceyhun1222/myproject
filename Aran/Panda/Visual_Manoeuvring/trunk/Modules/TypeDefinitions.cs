using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Aim.Features;
using Aran.Queries;

namespace Aran.Panda.VisualManoeuvring
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ObstacleMSA
    {
        public Point pPtGeo;
        public Point pPtPrj;
        public string Name;
        public string ID;
        public double Height;
        public int Index;
        public int trackStepNumber;
    }

    [System.Runtime.InteropServices.ComVisible(false)]
    public class ObstacleType
    {
        public Point pPtGeo;
        public Point pPtPrj;
        public string Name;
        public Guid Identifier;
        public string ID;
        public double Height;
        public int Index;
        public double HorAccuracy;
        public double VertAccuracy;        
        //public double EffectiveHeight;
        //public double ReqH;
        //public double Dist;
        //public double CLDist;
        //public double DistStar;
        //public double MOC;
        //public double ReqOCH;
        //public double hPent;
        //public double TurnDistL;
        //public double TurnDistR;
        //public double TurnAngleL;
        //public double TurnAngleR;
        //public double Kmin;
        //public double Kmax;
        //public double Rmin;
        //public double Rmax;
        //public double dMin15;
        //public double dMax15;
        //public double dNom15;
        //public double fSort;
        //public double sSort;
        //public int Flags;
        //public double fTmp;
        //public int stepNumber;
    }

    [System.Runtime.InteropServices.ComVisible(false)]
    public struct LowHigh
    {
        public double Low;
        public double High;
        public int Tag;
    }

    [System.Runtime.InteropServices.ComVisible(false)]
    public struct InSectorNav
    {
        public Point pPtGeo;
        public Point pPtPrj;
        public string Name;
        public Guid Identifier;
        public string CallSign;
        public double MagVar;
        public eNavaidType TypeCode;
        public double Range;
        public int Index;
        public int PairNavaidIndex;
        public double GPAngle;
        public double GP_RDH;
        public double Course;
        public double LLZ_THR;
        public double SecWidth;
        public double AngleWidth;
		public Feature pFeature;
        //public eIntersectionType IntersectionType;
        public int Tag;
        public double Distance;
        public double FromAngle;
        public double ToAngle;

        public SignificantPoint GetSignificantPoint()
		{
			SignificantPoint sp = new SignificantPoint();
            sp.NavaidSystem = pFeature.GetFeatureRef();
			return sp;
		}
    }

    public enum eOAS
    {
        ZeroPlane = 0,
        WPlane = 1,
        XlPlane = 2,
        YlPlane = 3,
        ZPlane = 4,
        YrPlane = 5,
        XrPlane = 6,
        WsPlane = 7,
        CommonPlane = 8,
        NonPrec = 9
    }

    public struct D3DPolygone
    {
        public Polygon Poly;
        public D3DPlane Plane;
    }

    public struct D3DPlane
    {
        public Point pPt;
        public double X;
        public double Y;
        public double Z;
        public double A;
        public double B;
        public double C;
        public double D;
    }

    public enum BestCircuit
    {
        Righthand = 1,
        Lefthand = -1,
        Both = 0
    }

    public enum CircuitSide
    {
        Righthand = 1,
        Lefthand = -1
    }

    //public class VM_VisualFeature
    //{
    //    public Point gShape { get; set; }        
    //    public Point pShape { get; set; }       
    //    public string name { get; set; }        
    //    public string type { get; set; }        
    //    public string description { get; set; }       

    //    //public VisualReferencePoint (Point gShapeValue, Point pShapeValue, string nameValue, string typeValue, string descriptionValue)
    //    //{
    //    //    gShape = gShapeValue;
    //    //    pShape = pShapeValue;
    //    //    name = nameValue;
    //    //    type = typeValue;
    //    //    description = descriptionValue;
    //    //}
    //}

    /*public class TrackStep
    {
        public Point startPointPrj;
        public double startPointDirPrj;
        public Point targetPointPrj;
        public double targetPointDirPrj;

        public double divergenceAngle;
        public double convergenceAngle;
        public double distanceBetweenManoeuvers;

        public VM_VisualFeature divergenceVF;
        public VM_VisualFeature convergenceVF;
        public int divergenceVFElement;
        public int convergenceVFElement;

        public LineString stepCentreline;
        public Polygon stepBufferArea;

        public int stepCentrelineElement;
        public int stepBufferAreaElement;

        public bool isFinalStep;
    }*/

    public class ParameterObject
    {
        public string property;
        public double doubleValue;
        public string stringValue;
        public UoM uom;
        public string message;
        public MessageType messageType;

        public ParameterObject(string pName, double val, UoM mUoM, string strVal = "", string msg = "", MessageType msgType = MessageType.infoMsg)
        {
            property = pName;
            doubleValue = val;
            stringValue = strVal;
            uom = mUoM;
            message = msg;
            messageType = msgType;
        }
    }

    public enum UoM
    {
        Distance = 0,
        Speed = 1,
        Height = 2,
        Angle = 3,
        RateOfDescent = 4,
        Time = 5,
        Gradient = 6,
        RateOfTurn = 7,
        Empty = 8        
    }

    public enum MessageType
    {
        infoMsg = 0,
        errorMsg = 1
    }

    public class Interval
    {
        double min;
        double max;

        public double MIN
        {
            get { return min; }
            set { min = value; }
        }

        public double MAX
        {
            get { return max; }
            set { min = value; }
        }

        public Interval(double min, double max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
