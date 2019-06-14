using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    public class Marking:PDMObject
    {
        public CodeMarkingConditionType Condition { get; set; }

        [Description("MarkingElement")]
        [Browsable(false)]
        [PropertyOrder(70)]
        public List<MarkingElement> MarkingElementList { get; set; }
    }
}
