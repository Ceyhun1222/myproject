using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class InsertResults : AbstractResponse
    {
        public InsertResults ()
        {
            InsertedFeatureList = new List<InsertedFeature> ();
        }

        public List<InsertedFeature> InsertedFeatureList { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    CommonXmlReader.ParseContentList<InsertedFeature> (reader, InsertedFeatureList, "InsertedFeature");
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
