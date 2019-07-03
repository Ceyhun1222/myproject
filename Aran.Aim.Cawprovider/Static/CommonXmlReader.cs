using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public static class CommonXmlReader
    {
        public static DateTime GetDateTime (string value)
        {
            return DateTime.Parse (value);
        }

        public static Guid GetGuid (string value)
        {
            return new Guid (value);

//////            Guid result = Guid.Empty;

//////            try
//////            {
//////                return new Guid (value);
//////            }
//////            catch (FormatException fex)
//////            {
//////#warning "2011.11.21 --- added exception for Latvian's data"
//////                //--- Check for Latvian's data...
//////                if (value.StartsWith ("id"))
//////                {
//////                    value = value.Substring (2);
//////                    return new Guid (value);
//////                }
//////                else
//////                    throw fex;
//////            }
//////            //return Guid.ParseExact (value, "D");
        }

        public static bool ParseContentList<T> (XmlReader reader, List<T> list, string localName)
            where T : AbstractXmlSerializable, new ()
        {
            bool result = false;

            while (reader.NodeType == XmlNodeType.Element &&
                reader.LocalName == localName)
            {
                T t = new T ();
                t.ReadXml (reader);
                list.Add (t);
                result = true;

                while (reader.Read ())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                        break;
                    else if (reader.NodeType == XmlNodeType.EndElement)
                        return result;
                }
            }

            return result;
        }

        public static bool MoveInnerElement (XmlReader reader)
        {
            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                    return true;
                else if (reader.NodeType == XmlNodeType.EndElement)
                    return false;
            }
            return false;
        }

        public static TEnum ParseEnum<TEnum> (string enumText) where TEnum : struct
        {
            return (TEnum) Enum.Parse (typeof (TEnum), enumText, true);
        }
    }
}
