using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class AREA_AIRTRACK :Object_AIRTRACK
    {
        public AREA_AIRTRACK()
        {
        }

        public AREA_AIRTRACK(IGeometry areaPoly, string areaName)
        {
            this.Shape = new Shape_AIRTRACK();
            this.Shape.Geometry = areaPoly;
            this.AreaName = areaName;
            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.INFO_AIRTRACK = areaName;
        }

        private string _areaName;

        public string AreaName
        {
            get { return _areaName; }
            set { _areaName = value; }
        }
    }
}
