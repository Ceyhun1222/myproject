using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class GetConfigurationResponse : BaseResponse
    {
        public GetConfigurationResponse ()
        {
            FaultMessages = new List<FaultMessage> ();
        }

        public WorkPackageConfiguration Configuration { get; set; }
        public List<FaultMessage> FaultMessages { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            base.ReadXml (reader);

            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "configuration")
                    {
                        if (CommonXmlReader.MoveInnerElement (reader))
                        {
                            if (reader.LocalName == "WorkPackageConfiguration")
                            {
                                Configuration = new WorkPackageConfiguration ();
                                Configuration.ReadXml (reader);
                            }
                        }
                    }

                    FaultMessages.Clear ();
                    CommonXmlReader.ParseContentList<FaultMessage> (reader, FaultMessages, "faultMessage");
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    depth == reader.Depth)
                {
                    break;
                }
            }
        }
    }
}
