using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;

namespace Aran.Aim.AixmMessage
{
    class AranAixmReader
    {
        static AranAixmReader ()
        {
        }

        public XmlReader Reader { get; set; }

        public bool ReadFeatureTimeSlice(Feature feature, string xmlText)
        {
            var xmlDoc = new XmlDocument ();
            xmlDoc.LoadXml (xmlText);
            var xmlContext = new XmlContext (xmlDoc.DocumentElement);
            var isDeserialized = (feature as IAixmSerializable).AixmDeserialize (xmlContext);

            if (isDeserialized)
                ReadFeatureExtension (feature, xmlContext);

            return isDeserialized;
        }

        public void ReadFeatureTimeSlice2 (Feature feature)
        {
            string [] localNames = new string [] {
                "validTime",
                "interpretation",
                "sequenceNumber", 
                "correctionNumber",
                "featureLifetime" };

            int n = 0;

            feature.TimeSlice = new TimeSlice ();

            while (Reader.Read ())
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {
                    if (Reader.LocalName == localNames [n])
                    {
                        switch (n)
                        {
                            case 0:
                                feature.TimeSlice.ValidTime = ReadTimePeriod ();
                                break;
                            case 1:
                                feature.TimeSlice.Interpretation = CommonXmlReader.ParseEnum<TimeSliceInterpretationType> (Reader.ReadElementContentAsString ());
                                break;
                            case 2:
                                feature.TimeSlice.SequenceNumber = Reader.ReadElementContentAsInt ();
                                break;
                            case 3:
                                feature.TimeSlice.CorrectionNumber = Reader.ReadElementContentAsInt ();
                                break;
                            case 4:
                                feature.TimeSlice.FeatureLifetime = ReadTimePeriod ();
                                break;
                        }
                    }
                    n++;
                }
                else if (Reader.NodeType == XmlNodeType.EndElement)
                {
                    return;
                }

                if (n == localNames.Length)
                    break;
            }

            ReadProperties (feature, (int) PropertyFeature.TimeSlice);
        }

        
        private void ReadProperties (IAimObject aimObject, int propStartIndex)
        {
            int depth = Reader.Depth - 1;

            while (Reader.Read ())
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {

                }
                else if (Reader.NodeType == XmlNodeType.EndElement && depth == Reader.Depth)
                {
                    break;
                }
            }
        }

        private TimePeriod ReadTimePeriod ()
        {
            string beginText = null;
            string endText = null;

            while (CommonXmlReader.MoveInnerElement (Reader))
            {
                while (CommonXmlReader.MoveInnerElement (Reader))
                {
                    if (Reader.LocalName == "beginPosition")
                        beginText = Reader.ReadElementContentAsString ();
                    else if (Reader.LocalName == "endPosition")
                        endText = Reader.ReadElementContentAsString ();
                    else
                        break;
                }
            }

            if (!string.IsNullOrEmpty(beginText))
            {
                TimePeriod tp = new TimePeriod ();
                tp.BeginPosition = CommonXmlReader.GetDateTime (beginText);

                if (!string.IsNullOrEmpty (endText))
                    tp.EndPosition = CommonXmlReader.GetDateTime (endText);

                return tp;
            }

            return null;
        }

        private void ReadFeatureExtension (Feature feature, XmlContext xmlContext)
        {
            //return;

            XmlElement elem = xmlContext.Element;

            for (int i = 0; i < elem.ChildNodes.Count; i++)
            {
                XmlNode node = elem.ChildNodes [i];
                if (node.NodeType == XmlNodeType.Element &&
                    node.LocalName == "extension")
                {
                    feature.Extension.Add (node.InnerXml);
                }
            }

        }
    }
}
