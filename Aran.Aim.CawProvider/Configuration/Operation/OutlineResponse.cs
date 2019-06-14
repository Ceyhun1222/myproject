using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class OutlineResponse : BaseResponse
    {
        public OutlineResponse()
        {
            EadPublicSlots = new List<EADPublicSlotOutline>();
            WorkPackages = new List<WorkPackageOutline>();
            FaultMessages = new List<FaultMessage>();
        }

        public List<EADPublicSlotOutline> EadPublicSlots { get; private set; }

        public List<WorkPackageOutline> WorkPackages { get; private set; }

        public List<FaultMessage> FaultMessages { get; private set; }
        
        public uint? NoSkippedItems { get; set; }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

            var rh = new ReaderHelper(reader, 1);
            rh.ElementReading += ElementReading;
            rh.Read();
        }

        private bool ElementReading(XmlReader reader, int dpeth)
        {
            EadPublicSlots.Clear();
            CommonXmlReader.ParseContentList<EADPublicSlotOutline>(reader, EadPublicSlots, "eadPublicSlots");

            WorkPackages.Clear();
            CommonXmlReader.ParseContentList<WorkPackageOutline>(reader, WorkPackages, "workPackages");

            FaultMessages.Clear();
            CommonXmlReader.ParseContentList<FaultMessage>(reader, FaultMessages, "faultMessage");

            return true;
        }
    }
}
