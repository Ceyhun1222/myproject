using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class TimeInstant : AbstractGMLType
    {
        public DateTime Position { get; set; }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(AimDbNamespaces.GML, "TimeInstant");
            {
                base.WriteXml(writer);
                
                writer.WriteStartElement(AimDbNamespaces.GML, "timePosition");
                {
                    writer.WriteString(CommonXmlWriter.GetString(Position));
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
