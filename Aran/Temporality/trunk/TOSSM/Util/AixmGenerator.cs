using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Aran.Aim.AixmMessage;
using Aran.Aim.Features;
using TOSSM.ViewModel.Tool;

namespace TOSSM.ViewModel.Pane
{
    public class AixmGenerator
    {
        private static readonly Object _lock = new object();
        private static AixmGenerator _instance;
        public static AixmGenerator Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AixmGenerator();
                }
                return _instance;
            }
        }

        private AixmGenerator()
        {
        }

        public XmlWriter CreateXmlWriter(out StringWriterWithEncoding stringBuilder)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            stringBuilder = new StringWriterWithEncoding(Encoding.UTF8);

            //stringBuilder = new StringBuilder();
            //var writer = XmlWriter.Create(fileName, settings);
            var writer = XmlWriter.Create(stringBuilder, settings);

            writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "AIXMBasicMessage");
            writer.WriteAttributeString(AimDbNamespaces.XSI, "schemaLocation",
                "http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd");
            writer.WriteAttributeString(AimDbNamespaces.GML, "id", CommonXmlWriter.GenerateNewGmlId());
            writer.WriteAttributeString("xmlns", AimDbNamespaces.AIXM51.Prefix, null, AimDbNamespaces.AIXM51.Namespace);
            if (false) //TODO:add metadata
            {
                writer.WriteStartElement(AimDbNamespaces.AIXM51, "messageMetadata");
                {
                }
                writer.WriteEndElement();
            }
            return writer;
        }

        public void CloseXmlWriter(XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        public void WriteFeature(XmlWriter writer, List<Feature> features)
        {
            try
            {
                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                {
                    var afl = new AixmFeatureList();
                    afl.AddRange(features);
                    afl.WriteXml(writer,
                        false,//WriteExtension
                        false,//Write3DCoordinateIfExists, 
                        SrsNameType.CRS84);
                }
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteFeature(XmlWriter writer, Feature feature)
        {
            try
            {
                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                {
                    var afl = new AixmFeatureList { feature };
                    afl.WriteXml(writer,
                        false,//WriteExtension
                        false,//Write3DCoordinateIfExists, 
                        SrsNameType.CRS84);
                }
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}