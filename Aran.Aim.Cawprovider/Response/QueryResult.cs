using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class QueryResult : AbstractResponse
    {
        public QueryResultChoice [] ChoiceValues
        {
            get
            {
                return _choiceValues;
            }
            set
            {
                if (value != null && value.Length > 2)
                {
                    throw new Exception ("the maximum number of queryResult choice values is 2");
                }
                _choiceValues = value;
            }
        }

        public List<FaultMessage> FaultMessages { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            if (!CommonXmlReader.MoveInnerElement (reader))
                return;

            List<QueryResultChoice> qrcList = new List<QueryResultChoice> ();

            QueryResultChoice qrc = QueryResultChoice.CreateFromXml (reader);
            if (qrc != null)
                qrcList.Add (qrc);

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                qrc = QueryResultChoice.CreateFromXml (reader);
                if (qrc != null)
                    qrcList.Add (qrc);
            }

            if (qrcList.Count > 0)
                ChoiceValues = qrcList.ToArray ();

            List<FaultMessage> fmList = new List<FaultMessage> ();
            bool result = CommonXmlReader.ParseContentList<FaultMessage> (reader, fmList, "faultMessage");

            if (result)
            {
                FaultMessages = fmList;
            }
        }

        private QueryResultChoice [] _choiceValues;
    }
}
