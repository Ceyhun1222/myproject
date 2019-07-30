using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class InsertedFeature : AbstractResponse
    {
        public InsertedFeature ()
        {
            FaultMessageList = new List<FaultMessage> ();
        }

        public FeatureType FeatureType { get; set; }
        public Guid Identifier { get; set; }
        public InterpretationType Interpretation { get; set; }
        public uint SequenceNumber { get; set; }
        public uint CorrectionNumber { get; set; }
        public bool Successful { get; set; }
        public List<FaultMessage> FaultMessageList { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "featureType")
                    {
                         FeatureType = CommonXmlReader.ParseEnum <FeatureType> (reader.ReadString ());
                    }
                    else if (reader.LocalName == "identifier")
                    {
                        Identifier = CommonXmlReader.GetGuid (reader.ReadString ());
                    }
                    else if (reader.LocalName == "interpretation")
                    {
                        Interpretation = CommonXmlReader.ParseEnum <InterpretationType> (reader.ReadString ());
                    }
                    else if (reader.LocalName == "sequenceNumber")
                    {
                        SequenceNumber = uint.Parse (reader.ReadString ());
                    }
                    else if (reader.LocalName == "correctionNumber")
                    {
                        CorrectionNumber = uint.Parse (reader.ReadString ());
                    }
                    else if (reader.LocalName == "extension")
                    {
                        int extensionDepth = reader.Depth;
                        while (reader.Read ())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement &&
                                extensionDepth == reader.Depth)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.LocalName == "DeviationExtensionPropertyGroup")
                    {
                        int deviationDepth = reader.Depth;
                        while (reader.Read ())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement &&
                                deviationDepth == reader.Depth)
                            {
                                break;
                            }
                        }
                    }
                    else if (reader.LocalName == "successful")
                    {
                        Successful = bool.Parse (reader.ReadString ());
                    }
                    else
                    {
                        FaultMessageList.Clear ();
                        CommonXmlReader.ParseContentList<FaultMessage> (reader, FaultMessageList, "faultMessage");
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Depth == depth)
                {
                    break;
                }
            }
        }
    }
}
