using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class TimeSliceOutline : AbstractResponse
    {
        public TimeSliceReference Content { get; set; }

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
                    Content = new TimeSliceBasics();
                    Content.ReadXml(reader);
                    break;
            }

            return true;
        }
    }
}
