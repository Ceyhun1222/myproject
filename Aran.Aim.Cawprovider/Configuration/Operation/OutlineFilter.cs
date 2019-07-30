using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class OutlineFilter : AbstractRequest
    {
        public OutlineFilter()
        {
            WorkPackageStatusList = new List<string>();
            EadPublicSlotStatusList = new List<string>();
        }

        public TimeInstantPeriodChoice TimeInstantPeriodChoice { get; set; }

        public List<string> WorkPackageStatusList { get; private set; }

        public List<string> EadPublicSlotStatusList { get; private set; }

        public SortOrderType? TimeSortOrder { get; set; }

        public SortOrderType? StatusSortOrder { get; set; }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(CadasNamespaces.SDP, "OutlineFilter");
            {
                if (TimeInstantPeriodChoice != null)
                    TimeInstantPeriodChoice.WriteXml(writer);

                #region workPackage

                foreach (var item in WorkPackageStatusList)
                    writer.WriteElementString(CadasNamespaces.SDP, "workPackageStatus", item);

                #endregion

                #region eadPublicSlotStatus

                foreach (var item in EadPublicSlotStatusList)
                    writer.WriteElementString(CadasNamespaces.SDP, "eadPublicSlotStatus", item);

                #endregion

                #region timeSortOrder

                if (TimeSortOrder != null)
                    writer.WriteElementString(CadasNamespaces.SDP, "timeSortOrder", TimeSortOrder.ToString());

                #endregion

                #region statusSortOrder
                
                if (StatusSortOrder != null)
                    writer.WriteElementString(CadasNamespaces.SDP, "statusSortOrder", StatusSortOrder.ToString());

                #endregion
            }
            writer.WriteEndElement();
        }
    }

    public class TimeInstantPeriodChoice : AbstractRequest
    {
        public TimeInstantPeriodChoice(List<TimeInstant> timeInstant)
        {
            TimeInstantList = timeInstant;
        }

        public TimeInstantPeriodChoice(List<TimePeriod> timePeriod)
        {
            TimePeriodList = timePeriod;
        }

        public List<TimeInstant> TimeInstantList { get; private set; }

        public List<TimePeriod> TimePeriodList { get; private set; }

        public override void WriteXml(XmlWriter writer)
        {
            if (TimeInstantList != null) {
                foreach (var item in TimeInstantList) {
                    writer.WriteStartElement(CadasNamespaces.SDP, "timeInstant");
                    {
                        item.WriteXml(writer);
                    }
                    writer.WriteEndElement();
                }
            }
            else if (TimePeriodList != null) {
                foreach (var item in TimePeriodList) {
                    writer.WriteStartElement(CadasNamespaces.SDP, "timePeriod");
                    {
                        item.WriteXml(writer);
                    }
                    writer.WriteEndElement();
                }
            }
        }
    }
}
