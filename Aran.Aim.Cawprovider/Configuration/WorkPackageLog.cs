using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageLog : AbstractResponse
    {
        public WorkPackageLog ()
        {
            LogMessages = new List<WorkPackageLogMessage> ();
        }

        public DateTime OperationTime { get; set; }
        public string UserName { get; set; }
        public string StatusOld { get; set; }
        public string StatusNew { get; set; }
        public string Operation { get; set; }
        public List<WorkPackageLogMessage> LogMessages { get; private set; }
        public long MappingReportId { get; set; }
        public WorkPackageLog CancelBy { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            ReaderHelper rh = new ReaderHelper (reader, 2);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        private bool ElementReading (XmlReader reader, int depth)
        {
            if (depth == 1)
            {
                return reader.LocalName == "WorkPackageLog";
            }
            else if (depth == 2)
            {
                switch (reader.LocalName)
                {
                    case "operationTime":
                        OperationTime = CommonXmlReader.GetDateTime (reader.ReadString ());
                        break;
                    case "username":
                        UserName = reader.ReadString ();
                        break;
                    case "statusOld":
                        StatusOld = reader.ReadString ();
                        break;
                    case "statusNew":
                        StatusNew = reader.ReadString ();
                        break;
                    case "operation":
                        Operation = reader.ReadString ();
                        break;
                    case "logMessages":
                        LogMessages.Clear ();
                        CommonXmlReader.ParseContentList<WorkPackageLogMessage> (reader, LogMessages, "logMessages");
                        break;
                    case "mappingReportId":
                        MappingReportId = long.Parse (reader.ReadString ());
                        break;
                    case "cancelBy":
                        CancelBy = new WorkPackageLog ();
                        CancelBy.ReadXml (reader);
                        break;
                }
            }
            return true;
        }
    }
}
