using System;
using System.Collections.Generic;
using System.Xml;
using Aran;
using Aran.Aixm;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.AixmMessage
{
    class AixmFeatureWriter
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

        public void WriteXml(XmlWriter writer, bool serializeExtension, NamespaceInfo caw, NamespaceInfo cae, string identifierCodeSpace, bool write3DIfExists, SrsNameType srsType)
        {
            var aranWriter = new AranAixmWriter(writer, caw, cae, write3DIfExists, srsType);
            aranWriter.SerializeExtension = serializeExtension;

            Dictionary<Guid, List<Feature>> featureGroup = GetFeatureGroupByIdentifier ();

            foreach (KeyValuePair<Guid, List<Feature>> pair in featureGroup)
            {
                if (pair.Value.Count > 0)
                {
                    var featureName = pair.Value[0].FeatureType.ToString();

                    writer.WriteStartElement(AimDbNamespaces.AIXM51, featureName);
                    {
                        //writer.WriteAttributeString(AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId());
                        writer.WriteAttributeString(AimDbNamespaces.GML, "id", string.Format("urn.uuid.{0}", CommonXmlWriter.GetString(pair.Key)));
                        writer.WriteStartElement(AimDbNamespaces.GML, "identifier");
                        {
                            writer.WriteAttributeString("codeSpace", identifierCodeSpace);
                            writer.WriteString(CommonXmlWriter.GetString(pair.Key));
                        }
                        writer.WriteEndElement();

                        foreach (Feature feature in pair.Value)
                        {
                            if (feature != null)
                            {
                                aranWriter.WriteFeatureTimeSlice(feature);
                            }
                        }
                    }
                    writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Write XML for AIP DataSet.
        /// Require for custom xml writing.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="serializeExtension"></param>
        /// <param name="caw"></param>
        /// <param name="cae"></param>
        /// <param name="identifierCodeSpace"></param>
        /// <param name="write3DIfExists"></param>
        /// <param name="srsType"></param>
        public void WriteDataSetXml(XmlWriter writer, bool serializeExtension, NamespaceInfo caw, NamespaceInfo cae, string identifierCodeSpace, bool write3DIfExists, SrsNameType srsType, Dictionary<FeatureType, List<string>> IgnoredProperties = null)
        {
            var aranWriter = new AranAixmWriter(writer, caw, cae, write3DIfExists, srsType);
            aranWriter.SerializeExtension = serializeExtension;

            Dictionary<Guid, List<Feature>> featureGroup = GetFeatureGroupByIdentifier();

            foreach (KeyValuePair<Guid, List<Feature>> pair in featureGroup)
            {
                if (pair.Value.Count > 0)
                {
                    var featureName = pair.Value[0].FeatureType.ToString();

                    writer.WriteStartElement(AimDbNamespaces.AIXM51, featureName);
                    {
                        //writer.WriteAttributeString(AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId());
                        writer.WriteAttributeString(AimDbNamespaces.GML, "id", string.Format("urn.uuid.{0}", CommonXmlWriter.GetString(pair.Key)));
                        writer.WriteStartElement(AimDbNamespaces.GML, "identifier");
                        {
                            writer.WriteAttributeString("codeSpace", identifierCodeSpace);
                            writer.WriteString(CommonXmlWriter.GetString(pair.Key));
                        }
                        writer.WriteEndElement();

                        foreach (Feature feature in pair.Value)
                        {
                            if (feature != null)
                            {
                                aranWriter.WriteDataSetFeatureTimeSlice(feature, IgnoredProperties);
                            }
                        }
                    }
                    writer.WriteEndElement();
                }
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
