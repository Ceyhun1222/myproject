using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageAttachmentUpload : AbstractRequest
    {
        public string Description { get; set; }
        public string Content { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, "WorkPackageAttachmentUpload");
            {
                if (Description != null)
                    writer.WriteElementString(CadasNamespaces.SDP, "description", Description);

                if (Content != null)
                    writer.WriteElementString(CadasNamespaces.SDP, "content", Content);
            }
            writer.WriteEndElement ();
        }
    }
}
