using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_Object
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private IGeometry _geometry;
        public IGeometry Geometry
        {
            get { return _geometry; }
            set { _geometry = value; }
        }


       // private string _AIXM45_Object_type;

        public string AIXM45_Object_type
        {
            get { return this.GetType().Name; }
            //set { _AIXM45_Object_type = value; }
        }
      

        public AIXM45_Object()
        {
            
        }
    }
}
