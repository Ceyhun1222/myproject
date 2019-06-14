using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class MultiQuery : AbstractAixmQuery
    {
        public MultiQuery ()
        {
            FeatureTypeList = new List<FeatureType> ();
        }

        public List<FeatureType> FeatureTypeList { get; private set; }

        public InterpretationType Interpretation { get; set; }

        public MQTemproalTimeclie TemproalTimeslice { get; set; }

        public Filter Filter { get; set; }

        public override void ReadXml (XmlReader reader)
        {
        }

        public override void WriteXml (XmlWriter writer)
        {
            //--- Begin MultiQuery.
            writer.WriteStartElement(CadasNamespaces.CAW, "MultiQuery");
            writer.WriteAttributeString ("outputFormat", "xml/aixm");

            foreach (FeatureType featureType in FeatureTypeList)
            {
                writer.WriteElementString(CadasNamespaces.CAW, "featureType", featureType.ToString());
            }
            writer.WriteElementString (AimDbNamespaces.AIXM51, "interpretation", Interpretation.ToString ());

            if (TemproalTimeslice != null)
                TemproalTimeslice.WriteXml (writer);

            if (Filter != null)
                Filter.WriteXml (writer);

            //--- End MultiQuery.
            writer.WriteEndElement ();
        }

        public override XmlSchema GetSchema ()
        {
            return null;
        }
    }
}
