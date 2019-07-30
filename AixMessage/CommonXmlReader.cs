using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.AixmMessage
{
    public static class CommonXmlReader
    {
        public static DateTime GetDateTime (string value)
        {
            return DateTime.Parse (value);
        }

        public static Guid GetGuid (string value)
        {
            Guid guid;
            if (Guid.TryParse(value, out guid))
                return guid;

            #region NON_GUID_IDENTIFIER
            if (Aran.Aim.DataTypes.FeatureRef.NonGuidIdentifier)
            {
                if (Aran.Aim.DataTypes.FeatureRef.GuidAssociteList.ContainsKey(value))
                    return Aran.Aim.DataTypes.FeatureRef.GuidAssociteList[value];

                guid = Guid.NewGuid();
                Aran.Aim.DataTypes.FeatureRef.GuidAssociteList.Add(value, guid);

                List<Aran.Aim.DataTypes.FeatureRef> featRefList;
                if (Aran.Aim.DataTypes.FeatureRef.GuidWaitingList.TryGetValue(value, out featRefList))
                {
                    foreach (var featRef in featRefList)
                        featRef.Identifier = guid;
                }
            }
            #endregion

            return guid;
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

        public static TEnum? TryParseEnum<TEnum> (string enumText) where TEnum : struct
        {
            return (Enum.TryParse<TEnum>(enumText, out TEnum tenum)) ? (TEnum?)tenum : null;
        }
    }
}
