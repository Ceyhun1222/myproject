using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class Operation : BaseRequest
    {
        public int? WorkPackageId { get; set; }
        public AbstractOperationParameter OperationParameter { get; set; }
        public string Name { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, "Operation");
            writer.WriteAttributeString ("name", Name);

            if (WorkPackageId != null)
                writer.WriteElementString(CadasNamespaces.SDP, "workPackageId", WorkPackageId.Value.ToString());

            if (OperationParameter != null)
            {
                writer.WriteStartElement(CadasNamespaces.SDP, "operationParameter");
                OperationParameter.WriteXml (writer);
                writer.WriteEndElement ();
            }
            writer.WriteEndElement ();
        }
    }
}
