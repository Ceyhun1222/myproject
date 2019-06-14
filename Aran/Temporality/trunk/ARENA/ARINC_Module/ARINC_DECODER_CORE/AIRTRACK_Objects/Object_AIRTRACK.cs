using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;
using ARINC_Types;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    [XmlType]
    [Serializable()]
    public class Object_AIRTRACK
    {
        private Shape_AIRTRACK _shape;
        [System.ComponentModel.Browsable(false)]
        [XmlIgnore]
        public Shape_AIRTRACK Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        private string _ID_AIRTRACK;
        [System.ComponentModel.Browsable(false)]
        [XmlElement]
        public string ID_AIRTRACK
        {
            get { return _ID_AIRTRACK; }
            set { _ID_AIRTRACK = value; }
        }

        private string _INFO_AIRTRACK;
        [System.ComponentModel.Browsable(false)]
        [XmlElement]
        public string INFO_AIRTRACK
        {
            get { return _INFO_AIRTRACK; }
            set { _INFO_AIRTRACK = value; }
        }


        private ARINC_OBJECT _ARINC_OBJ;
        [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        public ARINC_OBJECT ARINC_OBJ
        {
            get { return _ARINC_OBJ; }
            set { _ARINC_OBJ = value; }
        }

        public Object_AIRTRACK()
        {
            this.ID_AIRTRACK = new Guid().ToString();
        }
    }

    public class Shape_AIRTRACK
    {

        private IGeometry _geometry;
        [System.ComponentModel.Browsable(false)]
        public IGeometry Geometry
        {
            get { return _geometry; }
            set { _geometry = value; }
        }

        private string _geoLat;
        [System.ComponentModel.Browsable(false)]

        public string GeoLat
        {
            get { return _geoLat; }
            set { _geoLat = value; }
        }

        private string _geoLong;
        [System.ComponentModel.Browsable(false)]

        public string GeoLong
        {
            get { return _geoLong; }
            set { _geoLong = value; }
        }

        public Shape_AIRTRACK()
        {
        }

        public Shape_AIRTRACK(string Lat, string Long)
        {
            this.GeoLat = Lat;
            this.GeoLong = Long;
        }
    }
}
