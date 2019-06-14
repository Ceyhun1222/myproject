using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace ChartElementEditor.Elements
{
    public class MapTextMapElement : MapElementBase
    {
        [Browsable(false)]
        public TextElementClass InternalElement { get; set; }

        public double XOffset
        {
            get { return InternalElement == null ? 0 : InternalElement.XOffset; }
            set
            {
                if (InternalElement == null) return;
                InternalElement.XOffset = value;
            }
        }
       

    }
}
