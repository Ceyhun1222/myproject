using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class SEGMENT_AIRTRACK : Object_AIRTRACK
    {
        private Object_AIRTRACK _startPnt;
        //[System.ComponentModel.Browsable(false)]
        public Object_AIRTRACK StartPnt
        {
            get { return _startPnt; }
            set { _startPnt = value; }
        }

        private Object_AIRTRACK _endPnt;
        //[System.ComponentModel.Browsable(false)]
        public Object_AIRTRACK EndPnt
        {
            get { return _endPnt; }
            set { _endPnt = value; }
        }

        public SEGMENT_AIRTRACK()
        {
        }

        public SEGMENT_AIRTRACK(Object_AIRTRACK start, Object_AIRTRACK end)
        {
            this.StartPnt = start;
            this.EndPnt = end;

            //ESRI.ArcGIS.Geometry.ILine ln = new ESRI.ArcGIS.Geometry.LineClass();
            IPolyline ln = new PolylineClass();
            ln.FromPoint = start.Shape.Geometry as IPoint;
            ln.ToPoint = end.Shape.Geometry as IPoint;

            IZAware zAware = ln as IZAware;
            zAware.ZAware = true;

            IMAware mAware = ln as IMAware;
            mAware.MAware = true;

            //zAware = ln.FromPoint as IZAware;
            //zAware.ZAware = true;
            //ln.FromPoint.Z =0;

            //zAware = ln.ToPoint as IZAware;
            //zAware.ZAware = true;
            //ln.ToPoint.Z=0;
            ////ln.SetConstantZ(0);


            //mAware = ln.FromPoint as IMAware;
            //mAware.MAware = true;
            //ln.FromPoint.M = 0;

            //mAware = ln.ToPoint as IMAware;
            //mAware.MAware = true;
            //ln.ToPoint.M = 0;

            this.Shape = new Shape_AIRTRACK();

            this.Shape.Geometry = ln;

        }

        
    }
}
