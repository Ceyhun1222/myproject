using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Aran.Aim.AixmMessage;
using Aran.Aim.Features;

namespace Aran.Omega.VSImporter
{
    public class WriteToXml
    {
        public static void WriteAllFeatureToXML(List<Feature> featureList, string xmlFileName, bool writeExtension, bool write3DifExists, DateTime? effectiveDate, SrsNameType srsType)
        {
            var writerSettings = new XmlWriterSettings
            {
                NewLineOnAttributes = false,
                Indent = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };

            var xmlWriter = XmlWriter.Create(xmlFileName, writerSettings);

            var aixmBasicMessage =
                new AixmBasicMessage(MessageReceiverType.Panda, srsType)
                {
                    Write3DCoordinateIfExists = write3DifExists,
                    WriteExtension = writeExtension,
                    EffectiveDate = effectiveDate
                };

            foreach (Feature feature in featureList)
            {
                var afl = new AixmFeatureList {feature};
                aixmBasicMessage.HasMember.Add(afl);
            }

            featureList.Clear();

            aixmBasicMessage.WriteXml(xmlWriter);
            xmlWriter.Close();
        }
        public static byte[] WriteAllFeatureToStream(List<Feature> featureList,  bool writeExtension, bool write3DifExists, DateTime? effectiveDate, SrsNameType srsType)
        {
            var writerSettings = new XmlWriterSettings
            {
                NewLineOnAttributes = false,
                Indent = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };

            byte[] result;
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, writerSettings))
                {
                    var aixmBasicMessage =
                        new AixmBasicMessage(MessageReceiverType.Panda, srsType)
                        {
                            Write3DCoordinateIfExists = write3DifExists,
                            WriteExtension = writeExtension,
                            EffectiveDate = effectiveDate
                        };

                    foreach (Feature feature in featureList)
                    {
                        var afl = new AixmFeatureList { feature };
                        aixmBasicMessage.HasMember.Add(afl);
                    }

                    featureList.Clear();

                    aixmBasicMessage.WriteXml(xmlWriter);
                }
                result = stream.ToArray();
            }

            return result;
        }
    }
}
