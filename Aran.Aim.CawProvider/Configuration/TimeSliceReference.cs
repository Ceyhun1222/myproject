using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class TimeSliceReference : AbstractResponse
    {
        public TimeSliceReference ()
        {
            TimeSlice = new TimeSlice ();
        }

        public FeatureType? FeatureType { get; set; }
        public Guid Identifier { get; set; }
        public TimeSlice TimeSlice { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            ReaderHelper rh = new ReaderHelper (reader, 2);
            rh.ElementReading += new ElementReadingHandle (ElementReading);
            rh.Read ();
        }

        protected virtual bool ElementReading (XmlReader reader, int depth)
        {
            if (depth == 1)
            {
                return reader.LocalName == GetType ().Name;
            }
            else if (depth == 2)
            {
                string text = null;

                switch (reader.LocalName)
                {
                    case "featureType":
                        FeatureType = CommonXmlReader.TryParseEnum<FeatureType> (reader.ReadString ());
                        break;
                    case "identifier":
                        Identifier = CommonXmlReader.GetGuid (reader.ReadString ());
                        break;
                    case "interpretation":
                        TimeSlice.Interpretation = CommonXmlReader.ParseEnum<TimeSliceInterpretationType> (reader.ReadString ());
                        break;
                    case "sequenceNumber":
                        text = reader.ReadString ();
                        TimeSlice.SequenceNumber = int.Parse (text);
                        break;
                    case "correctionNumber":
                        text = reader.ReadString ();
                        TimeSlice.CorrectionNumber = int.Parse (text);
                        break;
                }
            }
            return true;
        }
    }
}
