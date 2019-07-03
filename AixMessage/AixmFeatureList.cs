using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Aran.Aim.Features;

namespace Aran.Aim.AixmMessage
{
    public class AixmFeatureList :
        List<Feature>,
        IXmlSerializable
    {
        public AixmFeatureList()
        {
            ErrorInfoList = new List<DeserializedErrorInfo>();
            IdentifierCodeSpace = "urn:uuid:";
        }

        public Guid Identifier { get; set; }

        public NamespaceInfo CAW { get; set; }
        
        public NamespaceInfo CAE { get; set; }

        public List<DeserializedErrorInfo> ErrorInfoList { get; private set; }

        public string IdentifierCodeSpace { get; set; }

        #region IXmlSerializable Members

        public XmlSchema GetSchema ()
        {
            return null;
        }


        private readonly Dictionary<Feature,string> _xmlDictionary=new Dictionary<Feature, string>();

        public string Xml(Feature feature)
        {
            string result=null;
            if (feature != null)
            {
                _xmlDictionary.TryGetValue(feature, out result);
            }
            return result;
        }

        public void ReadXml (XmlReader reader)
        {
            ErrorInfoList.Clear();

            var featureTypeName = reader.LocalName;

            FeatureType featureType;

            if (!Enum.TryParse<FeatureType>(featureTypeName, out featureType))
            {
                ErrorInfoList.Add(new DeserializedErrorInfo
                {
                    ErrorMessage = "Expected Feature Name, but name is [" + featureTypeName + "]",
                    ErrorType = CodeErrorType.UnknownFeature
                });

                return;
            }
           

			if (CommonXmlReader.MoveInnerElement (reader) && reader.LocalName == "identifier")
			{
				Identifier = CommonXmlReader.GetGuid (reader.ReadString ());
			}
			else
				return;

            if (CommonXmlReader.MoveInnerElement(reader) && reader.LocalName == "featureMetadata")
                reader.ReadInnerXml();

            AranAixmReader aranReader = new AranAixmReader ();

            while (reader.LocalName != "timeSlice" && CommonXmlReader.MoveInnerElement(reader))
            {
                ;//wait for timeslice
            }

            while (reader.LocalName == "timeSlice")
            {
                if (CommonXmlReader.MoveInnerElement(reader) && reader.LocalName == featureTypeName + "TimeSlice")
                {
                    var feature = AimObjectFactory.CreateFeature(featureType);
                    feature.Identifier = Identifier;

                    aranReader.Reader = reader;
                    var xmlText = reader.ReadOuterXml();

                    DeserializeLastException.LastErrorInfo = new DeserializedErrorInfo
                    {
                        XmlMessage = xmlText,
                        Identifier = Identifier,
                        FeatureType = featureType
                    };
                    DeserializeLastException.ClearPropertyPath();

                    try
                    {
                        var isDeserialized = aranReader.ReadFeatureTimeSlice(feature, xmlText);
                        if (isDeserialized)
                            Add(feature);
                    }
                    catch (Exception eex)
                    {
                        ErrorInfoList.Add(new DeserializedErrorInfo
                        {
                            FeatureType = featureType,
                            Identifier = Identifier,
                            ErrorMessage = eex.Message,
                            XmlMessage = xmlText
                        });
                    }

                    _xmlDictionary[feature] = xmlText;
                }
                else
                {
                    if (reader.NodeType == XmlNodeType.EndElement)
                        return;

                    ErrorInfoList.Add(new DeserializedErrorInfo
                    {
                        FeatureType = featureType,
                        Identifier = Identifier,
                        ErrorMessage = "Expected " + featureTypeName + "TimeSlice",
                        ErrorType=CodeErrorType.UnknownFeature
                    });

                    return;
                }

                CommonXmlReader.MoveInnerElement(reader);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            WriteXml (writer, true, false, SrsNameType.EPSG_4326);
        }

        public void WriteXml(XmlWriter writer, bool serializeExtension, bool write3DIfExists, SrsNameType srsType)
        {
            var featureWriter = new AixmFeatureWriter();
            featureWriter.SetFeatures(this.ToArray());
            featureWriter.WriteXml(writer, serializeExtension, CAW, CAE, IdentifierCodeSpace, write3DIfExists, srsType);
        }

        /// <summary>
        /// Write XML for AIP DataSet.
        /// Require for custom xml writing.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteDataSetXml(XmlWriter writer, Dictionary<FeatureType, List<string>> IgnoredProperties = null)
        {
            var featureWriter = new AixmFeatureWriter();
            featureWriter.SetFeatures(this.ToArray());

            featureWriter.WriteDataSetXml(writer, false, CAW, CAE, IdentifierCodeSpace, false, SrsNameType.CRS84, IgnoredProperties);
        }
        #endregion
    }
}
