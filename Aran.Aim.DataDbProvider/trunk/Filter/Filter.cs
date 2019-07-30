
using System;
using System.Collections.Generic;
using Aran.Package;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public class Filter : IPackable
    {
        public Filter(OperationChoice operationChoice)
        {
            Operation = operationChoice;
        }

        public OperationChoice Operation
        {
            get;
            set;
        }

        public static Filter CreateComparision(ComparisonOpType operationType, string propertyName, object value)
        {
            var co = new ComparisonOps(operationType, propertyName, value);
            var operCoice = new OperationChoice(co);
            return new Filter(operCoice);
        }

        public HashSet<string> GetPaths()
        {
            var paths = new HashSet<string>();

            FindPaths(Operation, paths);

            return paths;
        }

        private static void FindPaths(OperationChoice operationChoice, HashSet<string> paths)
        {
            if (paths == null)
                paths = new HashSet<string>();

            switch (operationChoice.Choice)
            {
                case OperationChoiceType.Comparison:
                    paths.Add(operationChoice.ComparisonOps.PropertyName);
                    break;
                case OperationChoiceType.Spatial:
                    paths.Add(operationChoice.SpatialOps.PropertyName);
                    break;
                case OperationChoiceType.Logic:
                    if (operationChoice.LogicOps is BinaryLogicOp binaryLogicOp)
                    {
                        foreach (var subOperationChoice in binaryLogicOp.OperationList)
                        {
                            FindPaths(subOperationChoice, paths);
                        }
                    }
                    break;
            }
        }

        public void Pack(PackageWriter writer)
        {
            Operation.Pack(writer);
        }

        public void Unpack(PackageReader reader)
        {
            Operation = OperationChoice.UnpackOperationChoice(reader);
        }
    }
}
