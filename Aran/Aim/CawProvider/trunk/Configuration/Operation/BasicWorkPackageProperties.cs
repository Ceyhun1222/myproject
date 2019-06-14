using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class BasicWorkPackageProperties : AbstractRequest
    {
        public string Description { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, GetType().Name);
            WriteProperties (writer);
            writer.WriteEndElement ();
        }

        public void WriteProperties (XmlWriter writer)
        {
            if (Description != null)
                writer.WriteElementString(CadasNamespaces.SDP, "description", Description);
        }
    }
}
