using Aran.Package;
using System;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public class OperationChoice : IPackable
    {
        private SpatialOps _spatialOps;
        private ComparisonOps _comparisonOps;
        private LogicOps _logicOps;


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

        private OperationChoice()
        {
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

        public string PropertyName
        {
            get
            {
                if (Choice == OperationChoiceType.Comparison)
                    return ComparisonOps.PropertyName;
                if (Choice == OperationChoiceType.Spatial)
                    return SpatialOps.PropertyName;
                return string.Empty;
            }
        }

        

        public void Pack(PackageWriter writer)
        {
            writer.PutEnum<OperationChoiceType>(Choice);

            switch (Choice) {
                case OperationChoiceType.Comparison:
                    ComparisonOps.Pack(writer);
                    break;
                case OperationChoiceType.Logic:
                    LogicOps.Pack(writer);
                    break;
                case OperationChoiceType.Spatial:
                    writer.PutString(SpatialOps.GetType().Name);
                    SpatialOps.Pack(writer);
                    break;
            }
        }

        public void Unpack(PackageReader reader)
        {
            var oct = (OperationChoiceType)reader.GetInt32();

            switch (oct) {
                case OperationChoiceType.Comparison:
                    ComparisonOps = new ComparisonOps();
                    ComparisonOps.Unpack(reader);
                    break;

                case OperationChoiceType.Logic:
                    var binLogicOp = new BinaryLogicOp();
                    binLogicOp.Unpack(reader);
                    LogicOps = binLogicOp;
                    break;

                case OperationChoiceType.Spatial:

                    var className = reader.GetString();

                    if (className == "DWithin")
                        SpatialOps = new DWithin();
                    else if (className == "Within")
                        SpatialOps = new Within();

                    if (SpatialOps != null)
                        SpatialOps.Unpack(reader);

                    break;
            }
        }

        public static OperationChoice UnpackOperationChoice(PackageReader reader)
        {
            var oc = new OperationChoice();
            oc.Unpack(reader);
            return oc;
        }
    }

    public enum OperationChoiceType
    {
        Spatial, 
        Comparison, 
        Logic
    }
}
