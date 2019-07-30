using Aran.Package;
using System;
using System.Collections.Generic;

namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public abstract class LogicOps : IPackable
    {
        public abstract void Pack(PackageWriter writer);

        public abstract void Unpack(PackageReader reader);
    }

    [Serializable]
    public class BinaryLogicOp : LogicOps
    {
        public BinaryLogicOp ()
        {
            OperationList = new List<OperationChoice> ();
        }

        public BinaryLogicOpType Type { get; set; }

        public List<OperationChoice> OperationList { get; private set; }

        public override void Pack(PackageWriter writer)
        {
            writer.PutEnum<BinaryLogicOpType>(Type);
            writer.PutInt32(OperationList.Count);
            foreach (var operChoice in OperationList)
                operChoice.Pack(writer);
        }

        public override void Unpack(PackageReader reader)
        {
            Type = reader.GetEnum<BinaryLogicOpType>();
            int count = reader.GetInt32();
            for (int i = 0; i < count; i++) {
                var operChoice = OperationChoice.UnpackOperationChoice(reader);
                OperationList.Add(operChoice);
            }
        }
    }

    public enum BinaryLogicOpType
    {
        And, 
        Or
    }
}
