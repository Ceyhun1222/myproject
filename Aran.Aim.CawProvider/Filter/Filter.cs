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
    public class Filter : IXmlSerializable
    {
        public OperationChoice Operation { get; set; }

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
            writer.WriteStartElement(CadasNamespaces.CAW, "Filter");
            Operation.WriteXml (writer);
            writer.WriteEndElement ();
        }

        #endregion
    }
}
