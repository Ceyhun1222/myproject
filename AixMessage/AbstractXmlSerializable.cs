using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Aran.Aim.AixmMessage
{
    public abstract class AbstractXmlSerializable : IXmlSerializable
    {
        public virtual XmlSchema GetSchema ()
        {
            return null;
        }

        public abstract void ReadXml (XmlReader reader);

        public abstract void WriteXml (XmlWriter writer);
    }
}
