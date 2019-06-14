using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PDM
{
    [Serializable()]
    public class GeoProperties
    {
        public GeoProperties()
        {

        }
        public double? VerticalAccuracy { get; set; }

        public UOM_DIST_HORZ VerticalAccuracy_UOM { get; set; }

        public double? HorizontalAccuracy { get; set; }

        public UOM_DIST_HORZ HorizontalAccuracy_UOM { get; set; }

        public double? GeoidUndulation { get; set; }

        public UOM_DIST_HORZ GeoidUndulation_UOM { get; set; }

        [XmlElement]
        [Browsable(false)]
        public CodeVerticalDatumType VerticalDatum { get; set; }
    }
}
