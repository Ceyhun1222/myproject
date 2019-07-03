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
    public abstract class SpatialOps : IXmlSerializable
    {
        #region IXmlSerializable Members

        public virtual XmlSchema GetSchema ()
        {
            return null;
        }

        public virtual void ReadXml (XmlReader reader)
        {
        }

        public abstract void WriteXml (XmlWriter writer);

        #endregion
    }

    public class BBox : SpatialOps
    {
        public string PropertyName { get; set; }
        public Aran.Geometries.Box Envelope { get; set; }

        public override XmlSchema GetSchema ()
        {
            return null;
        }

        public override void ReadXml (XmlReader reader)
        {
        }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.CAW, "BBOX");
            if (PropertyName != null)
                writer.WriteElementString(CadasNamespaces.CAW, "TimeslicePropertyName", PropertyName);

            CommonXmlWriter.WriteElement (Envelope, writer, true);
            
            writer.WriteEndElement ();
        }
    }

    public class DWithin : SpatialOps
    {
        public string PropertyName { get; set; }
        public Aran.Geometries.Geometry Geometry { get; set; }
        public DataTypes.ValDistance Distance { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.CAW, "DWithin");

            if (PropertyName != null)
                writer.WriteElementString(CadasNamespaces.CAW, "TimeslicePropertyName", PropertyName);

            CommonXmlWriter.WriteElement (Geometry, writer, true);
            CommonXmlWriter.WriteElement(Distance, writer, CadasNamespaces.CAW, "Distance");

            writer.WriteEndElement ();
        }
    }

    public class Within : SpatialOps
    {
        public string PropertyName { get; set; }
        public Aran.Geometries.Geometry Geometry { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.CAW, "Within");

            if (PropertyName != null)
                writer.WriteElementString(CadasNamespaces.CAW, "TimeslicePropertyName", PropertyName);

            CommonXmlWriter.WriteElement (Geometry, writer, true);

            writer.WriteEndElement ();
        }
    }
}
