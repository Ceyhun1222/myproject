using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class TransactionSummary : AbstractResponse
    {
        public TransactionSummary ()
        {
            FaultMessages = new List<FaultMessage> ();
        }

        public uint? Successfully { get; set; }
        public uint? Warnings { get; set; }
        public uint? Errors { get; set; }
        public uint? Critical { get; set; }
        public uint? NotInserted { get; set; }
        public List<FaultMessage> FaultMessages { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "successfully")
                    {
                        Successfully = uint.Parse (reader.ReadString ());
                    }
                    else if (reader.LocalName == "warnings")
                    {
                        Warnings = uint.Parse (reader.ReadString ());
                    }
                    else if (reader.LocalName == "errors")
                    {
                        Errors = uint.Parse (reader.ReadString ());
                    }
                    else if (reader.LocalName == "critical")
                    {
                        Critical = uint.Parse (reader.ReadString ());
                    }
                    else if (reader.LocalName == "notInserted")
                    {
                        NotInserted = uint.Parse (reader.ReadString ());
                    }
                    else
                    {
                        FaultMessages.Clear ();
                        CommonXmlReader.ParseContentList<FaultMessage> (reader, FaultMessages, "faultMessage");
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
