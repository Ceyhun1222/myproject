using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class TransactionResponse : BaseResponse
    {
        public TransactionResponse ()
        {
            Version = "5.1";
            InsertResultsList = new List<InsertResults> ();
            FaultMessageList = new List<FaultMessage> ();
        }

        public string Version { get; set; }

        public TransactionSummary TransactionSummary { get; set; }

        public List<InsertResults> InsertResultsList { get; private set; }

        public List<FaultMessage> FaultMessageList { get; private set; }

        public string Test { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            //string s = reader.ReadOuterXml ();

            Version = reader.GetAttribute ("version");

            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "TransactionSummary")
                    {
                        TransactionSummary = new TransactionSummary ();
                        TransactionSummary.ReadXml (reader);
                    }
                    else
                    {
                        InsertResultsList.Clear ();
                        CommonXmlReader.ParseContentList<InsertResults> (reader, InsertResultsList, "InsertResults");
                        
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

            Test = reader.ReadOuterXml ();
        }
    }
}
