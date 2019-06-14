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
    public class InstrumentApproachProcedure : Procedure
    {

        private string _ID_MasterProc;
        [Browsable(false)]
        public string ID_MasterProc
        {
            get { return _ID_MasterProc; }
            set { _ID_MasterProc = value; }
        }

        private CodeApproachPrefix _approachPrefix;

        public CodeApproachPrefix ApproachPrefix
        {
            get { return _approachPrefix; }
            set { _approachPrefix = value; }
        }
        private ApproachType _approachType;

        public ApproachType ApproachType
        {
            get { return _approachType; }
            set { _approachType = value; }
        }
        private string _multipleIdentification;

        public string MultipleIdentification
        {
            get { return _multipleIdentification; }
            set { _multipleIdentification = value; }
        }
        private double _copterTrack;

        public double CopterTrack
        {
            get { return _copterTrack; }
            set { _copterTrack = value; }
        }
        private string _circlingIdentification;

        public string CirclingIdentification
        {
            get { return _circlingIdentification; }
            set { _circlingIdentification = value; }
        }
        private string _courseReversalInstruction;

        public string CourseReversalInstruction
        {
            get { return _courseReversalInstruction; }
            set { _courseReversalInstruction = value; }
        }
        private CodeApproachEquipmentAdditional _additionalEquipment;

        public CodeApproachEquipmentAdditional AdditionalEquipment
        {
            get { return _additionalEquipment; }
            set { _additionalEquipment = value; }
        }
        private double _channelGNSS;

        public double ChannelGNSS
        {
            get { return _channelGNSS; }
            set { _channelGNSS = value; }
        }
        private bool _WAASReliable;

        public bool WAASReliable
        {
            get { return _WAASReliable; }
            set { _WAASReliable = value; }
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.InstrumentApproachProcedure.ToString();
            }
        } 

        public InstrumentApproachProcedure()
        {
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("approachPrefix"); if (findx >= 0) row.set_Value(findx, this.ApproachPrefix.ToString());
            findx = row.Fields.FindField("approachType"); if (findx >= 0) row.set_Value(findx, this.ApproachType.ToString());
            findx = row.Fields.FindField("multipleIdentification"); if (findx >= 0) row.set_Value(findx, this.MultipleIdentification);
            findx = row.Fields.FindField("copterTrack"); if (findx >= 0) row.set_Value(findx, this.CopterTrack);
            findx = row.Fields.FindField("circlingIdentification"); if (findx >= 0) row.set_Value(findx, this.CirclingIdentification);
            findx = row.Fields.FindField("courseReversalInstruction"); if (findx >= 0) row.set_Value(findx, this.CourseReversalInstruction);
            findx = row.Fields.FindField("additionalEquipment"); if (findx >= 0) row.set_Value(findx, this.AdditionalEquipment);
            findx = row.Fields.FindField("channelGNSS"); if (findx >= 0) row.set_Value(findx, this.ChannelGNSS);
            findx = row.Fields.FindField("WAASReliable"); if (findx >= 0) row.set_Value(findx, this.WAASReliable);
            findx = row.Fields.FindField("MasterProcID"); if (findx >= 0) row.set_Value(findx, this.ID);

            //findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

            row.Store();
        }

        public override string ToString()
        {
            return this.Airport_ICAO_Code + " " + this.ProcedureIdentifier;
        }

    }
}
