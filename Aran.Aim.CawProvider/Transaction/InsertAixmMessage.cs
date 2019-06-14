using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class InsertAixmMessage : AbstractRequest
    {
        public AixmBasicMessage AixmBasicMessage { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement (CadasNamespaces.CAW, "InsertAixmMessage");
            {
                if (AixmBasicMessage != null)
                {
                    AixmBasicMessage.WriteXml (writer);
                }
            }
            writer.WriteEndElement ();
        }
    }
}
