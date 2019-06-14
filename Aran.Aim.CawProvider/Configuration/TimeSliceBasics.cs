using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aixm;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider.Configuration
{
    public class TimeSliceBasics : TimeSliceReference
    {
        public string FeatureCode { get; set; }
        public AipClassifier AipClassifier { get; set; }

        protected override bool ElementReading (XmlReader reader, int depth)
        {
            if (depth == 1)
            {
                return reader.LocalName == GetType ().Name;
            }
            else if (depth == 2)
            {
                if (! base.ElementReading (reader, depth))
                    return false;

                switch (reader.LocalName)
                {
                    case "validTime":
                        {
#warning implement is...
                            if (reader.Read ())
                            {
                                string xmlText = reader.ReadInnerXml ();
                            }

                            //XmlContext xmlContext = new XmlContext (
                            //TimeSlice.ValidTime
                        }
                        break;
                    case "featureCode":
                        FeatureCode = reader.ReadString ();
                        break;
                    case "aipClassifier":
                        AipClassifier = CommonXmlReader.ParseEnum<AipClassifier> (reader.ReadString ());
                        break;
                }
            }
            return true;
        }
    }

    public enum AipClassifier { AMDT, NIL, NOTAM, SUP }
}
