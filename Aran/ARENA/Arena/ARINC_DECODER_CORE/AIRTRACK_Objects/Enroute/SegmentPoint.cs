using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class SEGMENT_POINT_AIRTRACK : Object_AIRTRACK
    {
        private int _Sequence_Number;
        [System.ComponentModel.Browsable(false)]
        public int Sequence_Number
        {
            get { return _Sequence_Number; }
            set { _Sequence_Number = value; }
        }


        private string _Route_Identifier;
        [System.ComponentModel.Browsable(false)]
        public string Route_Identifier
        {
            get { return _Route_Identifier; }
            set { _Route_Identifier = value; }
        }

        private string _PointType;
        [System.ComponentModel.Browsable(false)]
        public string PointType
        {
            get { return _PointType; }
            set { _PointType = value; }
        }

        private Object_AIRTRACK _segmentStartEndPoint;
        [System.ComponentModel.Browsable(false)]
        public Object_AIRTRACK SegmentStartEndPoint
        {
            get { return _segmentStartEndPoint; }
            set { _segmentStartEndPoint = value; }
        }

        private string _segmentPoint_designator;

        public string SegmentPoint_Designator
        {
            get { return _segmentPoint_designator; }
            set { _segmentPoint_designator = value; }
        }

        public SEGMENT_POINT_AIRTRACK()
        {
        }

        public SEGMENT_POINT_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;

            ARINC_Enroute_Airways_Primary_Record ARINC_RoteSegment = (ARINC_Enroute_Airways_Primary_Record)this.ARINC_OBJ;
            #region

            int num = 0;

            Int32.TryParse(ARINC_RoteSegment.Sequence_Number, out num);
            this.Sequence_Number = num;

            this.Route_Identifier = ARINC_RoteSegment.Route_Identifier;
            this.PointType = ARINC_RoteSegment.Section_Code + ARINC_RoteSegment.Subsection;
            this.SegmentPoint_Designator = ARINC_RoteSegment.Fix_Identifier;
            
            this.INFO_AIRTRACK = ARINC_RoteSegment.Route_Identifier.TrimEnd();
            #endregion
        }
    }
}
