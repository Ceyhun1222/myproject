using System;
using System.Collections.Generic;
using System.Xml;
using Aran;
using Aran.Aixm;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.CAWProvider
{
    public class AixmFeatureWriter
    {
        public void SetFeatures (Feature [] features)
        {
            if (features == null ||
                features [0] == null)
                return;

            _featureName = features [0].GetType ().Name;

            _features = new Feature [features.Length];
            features.CopyTo (_features, 0);
        }

        public void WriteXml (XmlWriter writer, bool serializeExtension)
        {
            var aranWriter = new AranAixmWriter (writer);
            aranWriter.SerializeExtension = serializeExtension;

            Dictionary<Guid, List<Feature>> featureGroup = GetFeatureGroupByIdentifier ();

            foreach (KeyValuePair<Guid, List<Feature>> pair in featureGroup)
            {
                writer.WriteStartElement (AimdbNamespaces.AIXM51, _featureName);
                {
                    writer.WriteAttributeString (AimdbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ());

                    writer.WriteStartElement (AimdbNamespaces.GML, "identifier");
                    {
                        writer.WriteAttributeString ("codeSpace", AimdbNamespaces.IdentifierCodeSpace);
                        writer.WriteString (CommonXmlWriter.GetString (pair.Key));
                    }
                    writer.WriteEndElement ();

                    foreach (Feature feature in pair.Value)
                    {
                        if (feature != null)
                        {
                            aranWriter.WriteFeatureTimeSlice (feature);
                        }
                    }
                }
                writer.WriteEndElement ();
            }
        }

        private Dictionary<Guid, List<Feature>> GetFeatureGroupByIdentifier ()
        {
            Dictionary<Guid, List<Feature>> featureGroup = new Dictionary<Guid, List<Feature>> ();

            foreach (Feature feature in _features)
            {
                if (feature != null)
                {
                    if (featureGroup.ContainsKey (feature.Identifier))
                    {
                        featureGroup [feature.Identifier].Add (feature);
                    }
                    else
                    {
                        List<Feature> featureList = new List<Feature> ();
                        featureList.Add (feature);
                        featureGroup.Add (feature.Identifier, featureList);
                    }
                }
            }

            return featureGroup;
        }

        private Feature [] _features;
        private string _featureName;
    }
}
