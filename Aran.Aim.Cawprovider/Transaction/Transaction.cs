using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class Transaction : BaseRequest
    {
        public int? Workpackage { get; set; }
        public Insert Insert { get; set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.CAW, "Transaction");
            {
                if (Workpackage != null)
                    writer.WriteAttributeString ("workpackage", Workpackage.Value.ToString ());

                if (Insert != null)
                {
                    Insert.WriteXml (writer);
                }
            }
            writer.WriteEndElement ();
        }
    }
}
