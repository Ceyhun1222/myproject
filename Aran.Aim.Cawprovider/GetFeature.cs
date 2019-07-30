using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class GetFeature : BaseRequest
    {
        public GetFeature ()
        {
            OutputFormat = OutputFormatType.TextXmlAixm;
            QueryList = new List<AbstractRequest> ();
        }

        public OutputFormatType OutputFormat { get; set; }

        public long? Workpackage { get; set; }

        public List<AbstractRequest> QueryList { get; private set; }

        public override XmlSchema GetSchema ()
        {
            return null;
        }

        public override void ReadXml (XmlReader reader)
        {
        }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.CAW, "GetFeature");

            if (Workpackage != null)
                writer.WriteAttributeString("workpackage", Workpackage.ToString());

            foreach (AbstractRequest query in QueryList)
                query.WriteXml (writer);

            writer.WriteEndElement ();
        }
    }
}
