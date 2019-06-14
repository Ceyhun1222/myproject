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
    public class NavigationSystemCheckpoint : PDMObject
    {
        public string ID_AirportHeliport { get; set; }

        public CodeCommunicationDirectionType? Category { get; set; }

        public double? UpperLimit { get; set; }

        public UOM_DIST_VERT UpperLimit_UOM { get; set; }

        public CodeVerticalReference? UpperLimitReference { get; set; }

        public double? LowerLimit { get; set; }

        public UOM_DIST_VERT LowerLimit_UOM { get; set; }

        public CodeVerticalReference? LowerLimitReference { get; set; }

        public AltitudeUseType AltitudeInterpretation { get; set; }

        public double? Distance { get; set; }

        public UOM_DIST_HORZ Distance_UOM { get; set; }

        public double? Angle { get; set; }

        public string Annotation { get; set; }

    }
}
