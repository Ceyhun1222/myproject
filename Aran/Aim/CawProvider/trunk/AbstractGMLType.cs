using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class AbstractGMLType : AbstractRequest
    {
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(AimDbNamespaces.GML, "id", CommonXmlWriter.GenerateNewGmlId());
        }
    }
}
