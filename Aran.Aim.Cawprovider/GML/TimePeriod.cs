using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    //*** #warning Not implemented...
    public class TimePeriod : AbstractGMLType
    {
        public override void WriteXml(XmlWriter writer)
        {
            //*** #warning Not implemented...
            writer.WriteStartElement(AimDbNamespaces.GML, "TimePeriod");
            {
                base.WriteXml(writer);
            }
            writer.WriteEndElement();
        }
    }

    //*** #warning Not implemented...
    public class TimePositionChoice : AbstractRequest
    {
        public TimePositionChoice(DateTime beginPosition)
        {
            BeginPosition = beginPosition;
        }

        public DateTime? BeginPosition { get; private set; }

        public override void WriteXml(XmlWriter writer)
        {
            //*** #warning Not implemented...
        }
    }
}
