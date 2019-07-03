using System;

namespace Aran.Aim.Data.Filters
{
    public class OperationChoice
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

        private SpatialOps _spatialOps;
        private ComparisonOps _comparisonOps;
        private LogicOps _logicOps;
    }

    public enum OperationChoiceType
    {
        Spatial, 
        Comparison, 
        Logic
    }
}
