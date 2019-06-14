using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;

namespace PDM
{
    public class StandardInstrumentArrival : Procedure
    {
        private string _ID_MasterProc;
        [Browsable(false)]
        public string ID_MasterProc
        {
            get { return _ID_MasterProc; }
            set { _ID_MasterProc = value; }
        }

        private string _designator;

        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.StandardInstrumentArrival.ToString();
            }
        } 

        public StandardInstrumentArrival()
        {
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("MasterProcID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);

            row.Store();
        }

        public override string ToString()
        {
            return this.Airport_ICAO_Code + " " + this.ProcedureIdentifier;
        }
    }
}
