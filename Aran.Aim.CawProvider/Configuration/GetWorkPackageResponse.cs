using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class GetWorkPackageResponse : BaseResponse
    {
        public GetWorkPackageResponse ()
        {
            WorkPackages = new List<WorkPackage> ();
            FaultMessages = new List<FaultMessage> ();
        }

        public List<WorkPackage> WorkPackages { get; private set; }
        public List<FaultMessage> FaultMessages { get; private set; }

        public override void ReadXml (XmlReader reader)
        {
            base.ReadXml (reader);

            ReaderHelper rh = new ReaderHelper (reader, 1);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        private bool ElementReading (XmlReader reader, int dpeth)
        {
            WorkPackages.Clear ();
            CommonXmlReader.ParseContentList<WorkPackage> (reader, WorkPackages, "workPackage");

            FaultMessages.Clear ();
            CommonXmlReader.ParseContentList<FaultMessage> (reader, FaultMessages, "faultMessage");

            return true;
        }
    }
}
