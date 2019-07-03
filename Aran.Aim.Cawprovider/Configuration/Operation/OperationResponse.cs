using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class OperationResponse : BaseResponse
    {
        public OperationResponse ()
        {
            OperationLog = new List<WorkPackageLog> ();
            FaultMessages = new List<FaultMessage> ();
        }

        public ResponseStatus Status { get; set; }
        public WorkPackageBasics WorkPackage { get; set; }
        public List<WorkPackageLog> OperationLog { get; private set; }
        public List<FaultMessage> FaultMessages { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            base.ReadXml (reader);

            ReaderHelper rh = new ReaderHelper (reader, 1);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        private bool ElementReading (XmlReader reader, int depth)
        {
            switch (reader.LocalName)
            {
                case "status":
                    Status = CommonXmlReader.ParseEnum<ResponseStatus> (reader.ReadString ());
                    break;
                case "workPackage":
                    WorkPackage = new WorkPackageBasics ();
                    WorkPackage.ReadXml (reader);
                    break;
                case "operationLog":
                    CommonXmlReader.ParseContentList<WorkPackageLog> (reader, OperationLog, "operationLog");
                    break;
                case "faultMessage":
                    CommonXmlReader.ParseContentList<FaultMessage> (reader, FaultMessages, "faultMessage");
                    break;
            }
            return true;
        }
    }

    public enum ResponseStatus { SUCCESS, FAILURE, PENDING }
}
