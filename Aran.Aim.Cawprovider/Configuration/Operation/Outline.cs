using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class Outline : BaseRequest
    {
        public OutlineReference Reference { get; set; }

        public OutlineFilter Filter { get; set; }

        public bool? RetrieveEADPublicSlots { get; set; }

        public bool? RetrieveWorkPackages { get; set; }

        public bool? RetrieveTimeSlices { get; set; }

        public uint? StartIndex { get; set; }

        public uint? Count { get; set; }

        public bool? EadReload { get; set; }

        public uint? EadTimeout { get; set; }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, "Outline");
            {
                if (RetrieveEADPublicSlots != null)
                    writer.WriteAttributeString("retrieveEADPublicSlots", CommonXmlWriter.GetString(RetrieveEADPublicSlots.Value));

                if (RetrieveWorkPackages != null)
                    writer.WriteAttributeString("retrieveWorkPackages", CommonXmlWriter.GetString(RetrieveWorkPackages.Value));

                if (RetrieveTimeSlices != null)
                    writer.WriteAttributeString("retrieveTimeSlices", CommonXmlWriter.GetString(RetrieveTimeSlices.Value));

                if (StartIndex != null)
                    writer.WriteAttributeString("startIndex", StartIndex.ToString());

                if (Count != null)
                    writer.WriteAttributeString("count", Count.ToString());

                if (EadReload != null)
                    writer.WriteAttributeString("eadReload", CommonXmlWriter.GetString(EadReload.Value));

                if (EadTimeout != null)
                    writer.WriteAttributeString("eadTimeout", EadTimeout.ToString());

                if (Reference != null) {
                    writer.WriteStartElement(CadasNamespaces.SDP, "reference");
                    {
                        Reference.WriteXml(writer);
                    }
                    writer.WriteEndElement();
                }

                if (Filter != null) {
                    writer.WriteStartElement(CadasNamespaces.SDP, "filter");
                    {
                        Filter.WriteXml(writer);
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }
    }
}
