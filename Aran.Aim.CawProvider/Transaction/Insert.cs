using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class Insert : AbstractRequest
    {
        public Insert ()
        {
            AllowDeviation = true;
            InsertAixmMessages = new List<InsertAixmMessage> ();
        }

        public bool AllowDeviation { get; set; }

        public List<InsertAixmMessage> InsertAixmMessages { get; private set; }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement (CadasNamespaces.CAW, "Insert");
            {
                writer.WriteAttributeString ("allowDeviation", AllowDeviation.ToString ().ToLower ());

                foreach (InsertAixmMessage iam in this.InsertAixmMessages)
                {
                    iam.WriteXml (writer);
                }
            }
            writer.WriteEndElement ();
            
        }
    }
}
