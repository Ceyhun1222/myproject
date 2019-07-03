using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public abstract class LogicOps : IXmlSerializable
    {
        #region IXmlSerializable Members

        public abstract XmlSchema GetSchema ();

        public abstract void ReadXml (XmlReader reader);

        public abstract void WriteXml (XmlWriter writer);

        #endregion
    }

    public class BinaryLogicOp : LogicOps
    {
        public BinaryLogicOp ()
        {
            OperationList = new List<OperationChoice> ();
        }

        public BinaryLogicOpType Type { get; set; }

        public List<OperationChoice> OperationList { get; private set; }

        #region IXmlSerializable Members

        public override XmlSchema GetSchema ()
        {
            return null;
        }

        public override void ReadXml (XmlReader reader)
        {
        }

        public override void WriteXml (XmlWriter writer)
        {
            string elementName = (Type == BinaryLogicOpType.And ? "And" : "Or");

            writer.WriteStartElement(CadasNamespaces.CAW, elementName);
            
            foreach (OperationChoice operation in OperationList)
                operation.WriteXml (writer);

            writer.WriteEndElement ();
        }

        #endregion
    }

    public enum BinaryLogicOpType
    {
        And, Or
    }
}
