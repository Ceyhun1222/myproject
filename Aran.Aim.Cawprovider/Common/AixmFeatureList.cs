using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Aran.Aim.Features;

namespace Aran.Aim.CAWProvider
{
    public class AixmFeatureList :
        List<Feature>,
        IXmlSerializable
    {
        public Guid Identifier { get; set; }

        #region IXmlSerializable Members

        public XmlSchema GetSchema ()
        {
            return null;
        }

        public void ReadXml (XmlReader reader)
        {
            string featureName = reader.LocalName;
            Guid identifier;

			if (CommonXmlReader.MoveInnerElement (reader) && reader.LocalName == "identifier")
			{
				identifier = CommonXmlReader.GetGuid (reader.ReadString ());
			}
			else
				return;

            AranAixmReader aranReader = new AranAixmReader ();

            while (CommonXmlReader.MoveInnerElement (reader) && reader.LocalName == "timeSlice")
            {
                if (CommonXmlReader.MoveInnerElement (reader) && reader.LocalName == featureName + "TimeSlice")
                {
                    try
                    {

                        Feature feature = CreateFeature (featureName);
                        feature.Identifier = identifier;

                        aranReader.Reader = reader;
                        aranReader.ReadFeatureTimeSlice (feature);

                        this.Add (feature);
                    }
                    catch (Exception ex)
                    {
                        string log = string.Format (
                            "Error on Read {0} - Identifier: {1} - {2}\n",
                            featureName, identifier, ex.Message);
                        Global.Errors.Add (log);

                        //throw ex;
                    }
                }
                else
                    return;
            }
        }

        public void WriteXml (XmlWriter writer)
        {
            WriteXml (writer, true);
        }

        public void WriteXml (XmlWriter writer, bool serializeExtension)
        {
            var featureWriter = new AixmFeatureWriter ();
            featureWriter.SetFeatures (this.ToArray ());
            featureWriter.WriteXml (writer, serializeExtension);
        }

        private Feature CreateFeature (string featureName)
        {
            FeatureType featureType = (FeatureType) Enum.Parse (typeof (FeatureType), featureName);
            return AimObjectFactory.CreateFeature (featureType);
        }

        #endregion
    }
}
