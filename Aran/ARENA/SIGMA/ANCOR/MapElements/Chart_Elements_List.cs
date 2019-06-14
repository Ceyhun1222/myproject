using ANCOR.MapElements;
using ANCOR.MapCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace ANCOR
{
   // [XmlInclude(typeof(AbstractChartElement))]
    [XmlInclude(typeof(ChartElement_AerspaceBufer))]
    [XmlInclude(typeof(ChartElement_BorderedText))]
    [XmlInclude(typeof(ChartElement_BorderedText_Collout))]
    [XmlInclude(typeof(ChartElement_BorderedText_Collout_CaptionBottom))]
    [XmlInclude(typeof(ChartElement_GlidePath))]
    [XmlInclude(typeof(ChartElement_RouteDesignator))]
    [XmlInclude(typeof(GraphicsChartElement_NorthArrow))]
    [XmlInclude(typeof(GraphicsChartElement_SafeArea))]
    [XmlInclude(typeof(GraphicsChartElement_ScaleBar))]
    [XmlInclude(typeof(GraphicsChartElement_TAA))]
    [XmlInclude(typeof(ChartElement_TextArrow))]
    [XmlInclude(typeof(ChartElement_BorderedText_Collout_CaptionBottom))]
    [XmlInclude(typeof(GraphicsChartElement))]
    [XmlInclude(typeof(AncorFrameMargins))]
    [XmlInclude(typeof(AncorArrowMarker))]
    [XmlInclude(typeof(AncorChartElementWord))]
    [XmlInclude(typeof(AncorColor))]
    [XmlInclude(typeof(AncorDataSource))]
    [XmlInclude(typeof(AncorFont))]
    [XmlInclude(typeof(AncorFrame))]
    [XmlInclude(typeof(AncorLeaderLine))]
    [XmlInclude(typeof(AncorMarkerBackGround))]
    [XmlInclude(typeof(AncorPoint))]
    [XmlInclude(typeof(AncorSymbol))]
    [XmlInclude(typeof(ChartElement_MarkerSymbol))]
    [XmlInclude(typeof(ChartElement_Radial))]
    [XmlInclude(typeof(ChartElement_SigmaCollout))]
    [XmlInclude(typeof(ChartElement_SigmaCollout_Navaid))]
    [XmlInclude(typeof(ChartElement_SigmaCollout_Airspace))]
    [XmlInclude(typeof(ChartElement_SigmaCollout_Designatedpoint))]
    [XmlInclude(typeof(ChartElement_SigmaCollout_AccentBar))]
    [XmlInclude(typeof(ChartElement_ILSCollout))]



    [XmlType]
    [Serializable()]
    public class Chart_ObjectsList
    {


        private List<AbstractChartElement> _list;
        [XmlElement]
        public List<AbstractChartElement> List
        {
            get { return _list; }
            set { _list = value; }
        }

        private string _chartType;
        [XmlElement]
        public string ChartType
        {
            get { return _chartType; }
            set { _chartType = value; }
        }

 

        public Chart_ObjectsList()
        {
            this.List = new List<AbstractChartElement>();
            this.ChartType = "ARENA";
        }

   

    }


}
