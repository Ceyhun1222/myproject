using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using ANCOR.MapCore;
using System.Drawing.Design;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

namespace ANCOR.MapElements
{

    [XmlType]
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphicsChartElement : AbstractChartElement
    {
        private AncorPoint _position;
        [XmlElement]
        [ReadOnly(false)]
        [Browsable(false)]
        public AncorPoint Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string _GraphicsChartElementName;
        [XmlElement]
        [Browsable(false)]
        public string GraphicsChartElementName
        {
            get { return _GraphicsChartElementName; }
            set { _GraphicsChartElementName = value; }
        }

        public GraphicsChartElement()
        {
            _position = new AncorPoint(2, 26);
        }

        public override IDisplayFeedback GetFeedback()
        {

            IDisplayFeedback _feedback = new MovePolygonFeedback();
            IMovePolygonFeedback mvPtFeed = (IMovePolygonFeedback)_feedback;

            return mvPtFeed;
        }

        public override void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
        {
            GraphicsChartElement symbol = (GraphicsChartElement)this.Clone();
            IElement iEl = (IElement)symbol.ConvertToIElement();

            IElement C = (iEl as IGroupElement3).get_Element(0);
            IGeometry feedGeo = C.Geometry;
           
            ((MovePolygonFeedback)feedBack).Start((IPolygon)feedGeo, _position);


        }

        public override void MoveFeedback(IDisplayFeedback feedBack, IPoint _position, IGeometry LinkedGeometry, int Shift)
        {
            feedBack.MoveTo(_position);
        }

        public override IGeometry StopFeedback(IDisplayFeedback feedBack, int X, int Y, IGeometry LinkedGeometry, int Shift)
        {
            IGeometry gm  = ((MovePolygonFeedback)feedBack).Stop();
            return gm;
        }

    }
}
