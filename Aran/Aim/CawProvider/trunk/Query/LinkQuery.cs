using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class LinkQuery : AbstractAixmQuery
    {
        public LinkQuery ()
        {
            FeatureTypeList = new List<FeatureType> ();
            RetrieveSourceFeature = false;
        }

        public bool RetrieveSourceFeature { get; set; }

        public List<FeatureType> FeatureTypeList { get; private set; }

        public string TraverseTimeslicePropertyName { get; set; }

        public SimpleQuery SimpleQuery { get; set; }

        public override void ReadXml (XmlReader reader)
        {
        }

        public override void WriteXml (XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.CAW, "LinkQuery");    //--- Begin LinkQuery.
            writer.WriteAttributeString ("outputFormat", "xml/aixm");
            writer.WriteAttributeString ("retrieveSourceFeature", RetrieveSourceFeature.ToString ().ToLower ());
            
            foreach (FeatureType featureType in FeatureTypeList)
            {
                writer.WriteElementString(CadasNamespaces.CAW, "featureType", featureType.ToString());
            }

            writer.WriteElementString(CadasNamespaces.CAW, "traverseTimeslicePropertyName", TraverseTimeslicePropertyName);

            if (SimpleQuery != null)
                SimpleQuery.WriteXml (writer);

            writer.WriteEndElement ();  //--- End LinkQuery.
        }

        public override XmlSchema GetSchema ()
        {
            return null;
        }
    }
}
