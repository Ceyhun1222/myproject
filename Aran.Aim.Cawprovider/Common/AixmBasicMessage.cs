using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.Features;

namespace Aran.Aim.CAWProvider
{
    public class AixmBasicMessage : AbstractXmlSerializable
    {
        public AixmBasicMessage ()
        {
            HasMember = new List<AixmFeatureList> ();
            WriteExtension = true;
        }

        public bool WriteExtension { get; set; }

        public Aran.Aim.Metadata.MessageMetadata MessageMetadata { get; set; }

        public List<AixmFeatureList> HasMember { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            int depth = reader.Depth;

            //--- must be derived from AbstractAIXMMessage class.

            bool firstTime = true;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (firstTime && reader.LocalName == "messageMetadata")
                    {
                        firstTime = false;

                        MessageMetadata = new Metadata.MessageMetadata ();
                        MessageMetadata_ReadXml (reader);
                    }
                    else if (reader.LocalName == "hasMember")
                    {
                        while (reader.Read ())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                AixmFeatureList afl = new AixmFeatureList ();
                                afl.ReadXml (reader);
                                HasMember.Add (afl);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    depth == reader.Depth)
                {
                    break;
                }
            }
        }

        public override void WriteXml (XmlWriter writer)
        {
            
            writer.WriteStartElement (AimdbNamespaces.AIXM51Message, "AIXMBasicMessage");
            {
                writer.WriteAttributeString(AimdbNamespaces.XSI, "schemaLocation",
                    "http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd");
                writer.WriteAttributeString (AimdbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ());
                writer.WriteAttributeString("xmlns", AimdbNamespaces.AIXM51.Prefix, null, AimdbNamespaces.AIXM51.Namespace);

                if (MessageMetadata != null)
                {
                    writer.WriteStartElement (AimdbNamespaces.AIXM51, "messageMetadata");
                    {
                        MessageMetadata_WriteXml (writer);
                    }
                    writer.WriteEndElement ();
                }

                try
                {
                    foreach (AixmFeatureList afl in this.HasMember)
                    {
                        writer.WriteStartElement (AimdbNamespaces.AIXM51Message, "hasMember");
                        {
                            afl.WriteXml (writer, WriteExtension);
                        }
                        writer.WriteEndElement ();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            writer.WriteEndElement ();
        }


		public static void ClearErrors ()
		{
			Global.Errors.Clear ();
		}

		public static List<string> GetLastErrors ()
		{
			return Global.Errors;
		}


        private void MessageMetadata_ReadXml (XmlReader reader)
        {
            var xmlText = reader.ReadInnerXml ();
        }

        private void MessageMetadata_WriteXml (XmlWriter writer)
        {

        }
    }
}
