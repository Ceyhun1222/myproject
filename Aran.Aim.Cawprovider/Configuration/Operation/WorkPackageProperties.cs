using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageProperties : BasicWorkPackageProperties
    {
        public WorkPackageProperties ()
        {
            attachments = new List<WorkPackageAttachmentUpload> ();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public List<WorkPackageAttachmentUpload> attachments { get; private set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, GetType().Name);
            base.WriteProperties (writer);

            if (Name != null)
                writer.WriteElementString(CadasNamespaces.SDP, "name", Name);

            if (Type != null)
                writer.WriteElementString(CadasNamespaces.SDP, "type", Type);

            if (EffectiveDate != null)
                writer.WriteElementString(CadasNamespaces.SDP, "effectiveDate", CommonXmlWriter.GetString(EffectiveDate.Value));

            foreach (WorkPackageAttachmentUpload wpau in attachments)
            {
                writer.WriteStartElement(CadasNamespaces.SDP, "attachments");
                wpau.WriteXml (writer);
                writer.WriteEndElement ();
            }
            writer.WriteEndElement ();
        }
    }
}
