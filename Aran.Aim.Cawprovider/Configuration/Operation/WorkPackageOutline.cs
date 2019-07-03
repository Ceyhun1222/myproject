using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackageOutline : AbstractResponse
    {
        public WorkPackageOutline()
        {
            TimeSlices = new List<TimeSliceOutline>();
        }

        public WorkPackageBasics Content { get; set; }

        public WorkPackageLog OperationLog { get; set; }

        public WorkPackageLog CanceledOperationLog { get; set; }

        public List<TimeSliceOutline> TimeSlices { get; private set; }

        public override void ReadXml(XmlReader reader)
        {
            var rh = new ReaderHelper(reader, 2);
            rh.ElementReading += ElementReading;
            rh.Read();
        }

        private bool ElementReading(XmlReader reader, int depth)
        {
            if (depth == 1)
                return (reader.LocalName == GetType().Name);

            switch (reader.LocalName) {
                case "content":
                    Content = new WorkPackageBasics();
                    Content.ReadXml(reader);
                    break;
                case "operationLog":
                    OperationLog = new WorkPackageLog();
                    OperationLog.ReadXml(reader);
                    break;
                case "canceledOperationLog":
                    CanceledOperationLog = new WorkPackageLog();
                    CanceledOperationLog.ReadXml(reader);
                    break;
                case "timeSlices":
                    TimeSlices.Clear();
                    CommonXmlReader.ParseContentList<TimeSliceOutline>(reader, TimeSlices, "timeSlices");
                    break;
            }
            return true;
        }
    }
}
