using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class GetFeatureResponse : BaseResponse
    {
        public GetFeatureResponse ()
        {
            QueryResults = new List<QueryResult> ();
            FaultMessages = new List<FaultMessage> ();
        }

        public List<QueryResult> QueryResults { get; private set; }

        public List<FaultMessage> FaultMessages { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            base.ReadXml (reader);

            QueryResults.Clear ();
            FaultMessages.Clear ();

            if (CommonXmlReader.MoveInnerElement (reader))
            {
                CommonXmlReader.ParseContentList<QueryResult> (reader, QueryResults, "queryResult");
                CommonXmlReader.ParseContentList<FaultMessage> (reader, FaultMessages, "faultMessage");
            }
        }
    }
}
