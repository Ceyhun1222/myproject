using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Accent.MapCore;
using System.Drawing.Design;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

namespace Accent.MapElements
{

    [XmlType]
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphicsChartElement : AbstractChartElement
    {
        private AcntPoint _position;
        [XmlElement]
        [ReadOnly(false)]
        [Browsable(false)]
        public AcntPoint Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public GraphicsChartElement()
        {
            _position = new AcntPoint(2, 26);
        }

        //public override object ConvertToIElement()
        //{
        //    return base.ConvertToIElement();
        //}
    }
}
