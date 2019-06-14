using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.ComponentModel;
using System.Runtime.InteropServices;
using PDM.PropertyExtension;

namespace PDM
{
    [Serializable()]
    public class HoldingPattern : PDMObject
    {
        private string _ID_Transition;
        [Browsable(false)]
        public string ID_Transition
        {
            get { return _ID_Transition; }
            set { _ID_Transition = value; }
        }

        private string _ProcedureLegID;
        [Browsable(false)]
        public string ProcedureLegID
        {
            get { return _ProcedureLegID; }
            set { _ProcedureLegID = value; }
        }

        private SegmentPoint _holdingPoint;
        public SegmentPoint HoldingPoint
        {
            get { return _holdingPoint; }
            set { _holdingPoint = value; }
        }

        private CodeHoldingUsage _type;
        public CodeHoldingUsage Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private double? _outboundCourse = null;
        public double? OutboundCourse
        {
            get { return _outboundCourse; }
            set { _outboundCourse = value; }
        }

        private CodeCourse _outboundCourseType;
        public CodeCourse OutboundCourseType
        {
            get { return _outboundCourseType; }
            set { _outboundCourseType = value; }
        }

        private double? _inboundCourse = null;
        public double? InboundCourse
        {
            get { return _inboundCourse; }
            set { _inboundCourse = value; }
        }

        private DirectionTurnType turnDirection;
        public DirectionTurnType TurnDirection
        {
            get { return turnDirection; }
            set { turnDirection = value; }
        }

        private double? _upperLimit = null;
        public double? UpperLimit
        {
            get { return _upperLimit; }
            set { _upperLimit = value; }
        }

        private UOM_DIST_VERT _upperLimitUOM;
        public UOM_DIST_VERT UpperLimitUOM
        {
            get { return _upperLimitUOM; }
            set { _upperLimitUOM = value; }
        }

        private CODE_DIST_VER _upperLimitReference;
        public CODE_DIST_VER UpperLimitReference
        {
            get { return _upperLimitReference; }
            set { _upperLimitReference = value; }
        }

        private double? _lowerLimit = null;
        public double? LowerLimit
        {
            get { return _lowerLimit; }
            set { _lowerLimit = value; }
        }

        private UOM_DIST_VERT _lowerLimitUOM;
        public UOM_DIST_VERT LowerLimitUOM
        {
            get { return _lowerLimitUOM; }
            set { _lowerLimitUOM = value; }
        }

        private CODE_DIST_VER _lowerLimitReference;
        public CODE_DIST_VER LowerLimitReference
        {
            get { return _lowerLimitReference; }
            set { _lowerLimitReference = value; }
        }


        private double? _speedLimit = null;
        public double? SpeedLimit
        {
            get { return _speedLimit; }
            set { _speedLimit = value; }
        }

        private SpeedType _speedLimitUOM;
        public SpeedType SpeedLimitUOM
        {
            get { return _speedLimitUOM; }
            set { _speedLimitUOM = value; }
        }

        private string _instruction;
        public string Instruction
        {
            get { return _instruction; }
            set { _instruction = value; }
        }

        private bool _nonStandardHolding;
        public bool NonStandardHolding
        {
            get { return _nonStandardHolding; }
            set { _nonStandardHolding = value; }
        }

        private double? _duration_Distance = null;
        public double? Duration_Distance
        {
            get { return _duration_Distance; }
            set { _duration_Distance = value; }
        }

        private string _duration_Distance_UOM;
        public string Duration_Distance_UOM
        {
            get { return _duration_Distance_UOM; }
            set { _duration_Distance_UOM = value; }
        }

        private byte[] _hodingBorder;
        [Browsable(false)]
        public byte[] HodingBorder
        {
            get { return _hodingBorder; }
            set { _hodingBorder = value; }
        }

        [Browsable(false)]
        private SegmentPoint _endPoint;
        public SegmentPoint EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }


        [Browsable(false)]
        public override double? Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }

        [Browsable(false)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }

        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }

        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.HoldingPattern;
            }
        } 

        public HoldingPattern()
        {
        }

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();

                if (this.HoldingPoint != null)
                {
                    this.HoldingPoint.StoreToDB(AIRTRACK_TableDic);
                }

                if (this.EndPoint != null)
                {
                   // this.EndPoint.StoreToDB(AIRTRACK_TableDic);
                }


            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }



            return res;
        }

        public override void CompileRow(ref ESRI.ArcGIS.Geodatabase.IRow row)
        {
            int findx = -1;


            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_Transition"); if (findx >= 0 && this.ID_Transition!=null) row.set_Value(findx, this.ID_Transition);
            findx = row.Fields.FindField("ProcedureLegID"); if (findx >= 0 && this.ProcedureLegID!=null) row.set_Value(findx, this.ProcedureLegID);
            //findx = row.Fields.FindField("StartWayPointDesignator"); if (findx >= 0) row.set_Value(findx, this.ProcedureLegID);
            findx = row.Fields.FindField("SegmentPointID"); if (findx >= 0 && this.HoldingPoint != null) row.set_Value(findx, this.HoldingPoint.SegmentPointDesignator);
            findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("outboundCourse"); if (findx >= 0 && this.OutboundCourse.HasValue) row.set_Value(findx, this.OutboundCourse.Value);
            findx = row.Fields.FindField("outboundCourseType"); if (findx >= 0 && this.OutboundCourse.HasValue) row.set_Value(findx, this.OutboundCourseType.ToString());
            findx = row.Fields.FindField("inboundCourse"); if (findx >= 0 && this.InboundCourse.HasValue) row.set_Value(findx, this.InboundCourse.Value);
            findx = row.Fields.FindField("turnDirection"); if (findx >= 0) row.set_Value(findx, this.turnDirection.ToString());
            findx = row.Fields.FindField("upperLimit"); if (findx >= 0 && this.UpperLimit.HasValue) row.set_Value(findx, this.UpperLimit.Value);
            findx = row.Fields.FindField("upperLimitUOM"); if (findx >= 0 && this.UpperLimit.HasValue) row.set_Value(findx, this.UpperLimitUOM.ToString());
            findx = row.Fields.FindField("upperLimitReference"); if (findx >= 0 && this.UpperLimit.HasValue) row.set_Value(findx, this.UpperLimitReference.ToString());
            findx = row.Fields.FindField("lowerLimit"); if (findx >= 0 && this.LowerLimit.HasValue) row.set_Value(findx, this.LowerLimit.Value);
            findx = row.Fields.FindField("lowerLimitUOM"); if (findx >= 0 && this.LowerLimit.HasValue) row.set_Value(findx, this.LowerLimitUOM.ToString());
            findx = row.Fields.FindField("lowerLimitReference"); if (findx >= 0 && this.LowerLimit.HasValue) row.set_Value(findx, this.LowerLimitReference.ToString());
            findx = row.Fields.FindField("speedLimit"); if (findx >= 0 && this.SpeedLimit.HasValue) row.set_Value(findx, this.SpeedLimit.Value);
            findx = row.Fields.FindField("speedLimitUOM"); if (findx >= 0 && this.SpeedLimit.HasValue) row.set_Value(findx, this.SpeedLimitUOM.ToString());
            //findx = row.Fields.FindField("instruction"); if (findx >= 0) row.set_Value(findx, this.Instruction.ToString());
            findx = row.Fields.FindField("nonStandardHolding"); if (findx >= 0) row.set_Value(findx, this.NonStandardHolding);
            findx = row.Fields.FindField("duration_Time"); if (findx >= 0 && this.Duration_Distance.HasValue) row.set_Value(findx, this.Duration_Distance.Value);
            findx = row.Fields.FindField("duration_Time_UOM"); if (findx >= 0 && this.Duration_Distance_UOM !=null) row.set_Value(findx, this.Duration_Distance_UOM.ToString());
            if (this.EndPoint != null)
            {
                //findx = row.Fields.FindField("endPoint_SegmentPointID");
                //if (findx >= 0) row.set_Value(findx, this.EndPoint.ID);
            }

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }
        }

        public override string GetObjectLabel()
        {
            return this.HoldingPoint != null ? "Holding  " + this.HoldingPoint.SegmentPointDesignator : "Holding";
        }

        public override void RebuildGeo()
        {
            if (this.HodingBorder == null) return;
            //string[] words = ((string)this.HodingBorder).Split(':');

            //byte[] bytes = new byte[words.Length];

            //for (int i = 0; i <= words.Length - 2; i++) bytes[i] = Convert.ToByte(words[i]);

            byte[] bytes = (byte[])this.HodingBorder;
            // сконвертируем его в геометрию 
            IMemoryBlobStream memBlobStream = new MemoryBlobStream();

            IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

            varBlobStream.ImportFromVariant(bytes);

            IObjectStream anObjectStream = new ObjectStreamClass();
            anObjectStream.Stream = memBlobStream;

            IPropertySet aPropSet = new PropertySetClass();

            IPersistStream aPersistStream = (IPersistStream)aPropSet;
            aPersistStream.Load(anObjectStream);

            this.Geo = aPropSet.GetProperty("Border") as IGeometry;
        }


        public void RebuildGeo2()
        {
            RebuildGeo();
        }


        //public override List<string> GetIdList()
        //{
        //    List<string> res = base.GetIdList();

        //    if (this.HoldingPoint != null)
        //    {
        //        res.Add(HoldingPoint.ID);
        //        res.Add(HoldingPoint.PointChoiceID);
        //    }

        //    return res;
        //}

    }
}
