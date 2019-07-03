using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class WorkPackage : WorkPackageBasics
    {
        public WorkPackage ()
        {
            Attachments = new List<WorkPackageAttachmentBasics> ();
            Log = new List<WorkPackageLog> ();
            TimeSlices = new List<TimeSliceBasics> ();
        }

        public List<WorkPackageAttachmentBasics> Attachments { get; private set; }
        public string Description { get; set; }
        public List<WorkPackageLog> Log { get; private set; }
        public List<TimeSliceBasics> TimeSlices { get; private set; }
        public EADPublicSlotBasics EadPublicSlot { get; set; }
        public EADPrivateSlotBasics EadPrivateSlot { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            Attachments.Clear ();

            ReaderHelper rh = new ReaderHelper (reader, 2);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        protected override bool ElementReading (XmlReader reader, int depth)
        {
            if (depth == 1)
            {
                if (reader.LocalName == "WorkPackage")
                {
                    return true;
                }
                else
                    return false;
            }
            else if (depth == 2)
            {
                bool r = base.ElementReading (reader, depth);
                if (!r)
                    return false;

                switch (reader.LocalName)
                {
                    case "attachments":
                        WorkPackageAttachmentBasics attachment = new WorkPackageAttachmentBasics ();
                        attachment.ReadXml (reader);
                        Attachments.Add (attachment);
                        break;
                    case "description":
                        Description = reader.ReadString ();
                        break;
                    case "log":
                        WorkPackageLog wpl = new WorkPackageLog ();
                        wpl.ReadXml (reader);
                        Log.Add (wpl);
                        break;
                    case "timeSlices":
                        TimeSliceBasics tsb = new TimeSliceBasics ();
                        tsb.ReadXml (reader);
                        TimeSlices.Add (tsb);
                        break;
                    case "eadPublicSlot":
                        EadPublicSlot = new EADPublicSlotBasics ();
                        EadPublicSlot.ReadXml (reader);
                        break;
                    case "eadPrivateSlot":
                        EadPrivateSlot = new EADPrivateSlotBasics ();
                        EadPrivateSlot.ReadXml (reader);
                        break;
                }
            }

            return true;
        }
    }

    public class EADPublicSlotBasics : AbstractResponse
    {
        public override void ReadXml (XmlReader reader)
        {
            string xmlText = reader.ReadInnerXml ();
        }
    }

    public class EADPrivateSlotBasics : AbstractResponse
    {
        public override void ReadXml (XmlReader reader)
        {
            string xmlText = reader.ReadInnerXml ();
        }
    }
}
