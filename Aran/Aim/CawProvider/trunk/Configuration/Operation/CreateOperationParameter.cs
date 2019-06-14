using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class CreateOperationParameter : AbstractOperationParameter
    {
        public int? EadPublicSlotId { get; set; }
        public WorkPackageProperties Properties { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, "CreateOperationParameter");
            {
                if (EadPublicSlotId != null)
                    writer.WriteElementString(CadasNamespaces.SDP, "eadPublicSlotId", EadPublicSlotId.Value.ToString());

                if (Properties != null)
                {
                    writer.WriteStartElement(CadasNamespaces.SDP, "properties");
                    Properties.WriteXml (writer);
                    writer.WriteEndElement ();
                }
            }
            writer.WriteEndElement ();
        }
    }
}
