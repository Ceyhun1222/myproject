using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class ComparisonOps : IXmlSerializable
    {
        public ComparisonOps ()
        {
        }

        public ComparisonOps (ComparisonOpType operationType, 
            string propertyName, object value)
        {
            OperationType = operationType;
            PropertyName = propertyName;
            Value = value;
        }

        public ComparisonOpType OperationType { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }

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
            //&& OperationType != ComparisonOpType.Isnotnu
            if (OperationType != ComparisonOpType.IsNull && OperationType != ComparisonOpType.ContainsProperty && Value == null)
                throw new Exception ("ComparisonOps Value is null and Operation is not 'PropertyIsNull'");

            string opName;

            if (OperationType == ComparisonOpType.ContainsProperty)
                opName = "ContainsProperty";
            else
                opName = "Property" + OperationType;
            
            writer.WriteStartElement(CadasNamespaces.CAW, opName);
            
            //writer.WriteElementString (AimdbNamespaces.CAW, "TimeslicePropertyName", PropertyName);
            writer.WriteElementString(CadasNamespaces.CAW, "PropertyName", PropertyName);

            if (Value != null)
            {
                if (Value is IXmlSerializable)
                    (Value as IXmlSerializable).WriteXml (writer);
                else
                    writer.WriteElementString(CadasNamespaces.CAW, "Literal", Value.ToString());
            }
            
            writer.WriteEndElement ();
        }

        #endregion
    }

    public enum ComparisonOpType
    {
        IsEqualTo,
        IsNotEqualTo,
        IsLessThan,
        IsGreaterThan,
        IsLessThanOrEqualTo,
        IsGreaterThanOrEqualTo,
        IsNull,
        ContainsProperty,
        IsLike
    }
}
