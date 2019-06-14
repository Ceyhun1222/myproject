using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class OperationChoice : IXmlSerializable
    {
        public OperationChoice (SpatialOps spatialOps)
        {
            SpatialOps = spatialOps;
        }

        public OperationChoice (ComparisonOps comparisonOps)
        {
            ComparisonOps = comparisonOps;
        }

        public OperationChoice (LogicOps logicOps)
        {
            LogicOps = logicOps;
        }

        public OperationChoiceType Choice { get; private set; }

        public SpatialOps SpatialOps
        {
            get { return _spatialOps; }
            set
            {
                _spatialOps = value;
                Choice = OperationChoiceType.Spatial;
            }
        }

        public ComparisonOps ComparisonOps
        {
            get { return _comparisonOps; }
            set
            {
                _comparisonOps = value;
                Choice = OperationChoiceType.Comparison;
            }
        }

        public LogicOps LogicOps
        {
            get { return _logicOps; }
            set
            {
                _logicOps = value;
                Choice = OperationChoiceType.Logic;
            }
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
        }

        public void WriteXml (XmlWriter writer)
        {
            switch (Choice)
            {
                case OperationChoiceType.Spatial:
                    SpatialOps.WriteXml (writer);
                    break;
                case OperationChoiceType.Comparison:
                    ComparisonOps.WriteXml (writer);
                    break;
                case OperationChoiceType.Logic:
                    LogicOps.WriteXml (writer);
                    break;
            }
        }

        #endregion

        private SpatialOps _spatialOps;
        private ComparisonOps _comparisonOps;
        private LogicOps _logicOps;
    }

    public enum OperationChoiceType
    {
        Spatial, Comparison, Logic
    }
}
