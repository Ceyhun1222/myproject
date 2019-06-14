using System;
using System.Runtime.Serialization;
using System.Xml;
using Aran.Aim.Package;
using Aran.Package;

namespace Aran.Aim
{
    internal static class CommonFunctions
    {
        public static string FixStringQuote (string text)
        {
            return text.Replace ("'", "''");
        }

        public static string RoundQuote (string text)
        {
            return "'" + text + "'";
        }

        public static void Serialize (AranPackageWriter writer, SerializationInfo info)
        {
            byte [] buffer = writer.GetBytes ();
            writer.Dispose ();

            char [] ca = new char [buffer.Length];
            for (int i = 0; i < ca.Length; i++)
                ca [i] = (char) buffer [i];
            string s = new string (ca);
            info.AddValue ("Value", s);
            writer.Dispose ();
        }

        public static AranPackageReader Desirialize (SerializationInfo info)
        {
            string s = info.GetString ("Value");
            char [] ca = s.ToCharArray ();
            byte [] buffer = new byte [ca.Length];
            for (int i = 0; i < ca.Length; i++)
                buffer [i] = (byte) ca [i];

            AranPackageReader reader = new AranPackageReader (buffer);
            return reader;
        }

        public static TEnum ParseEnum<TEnum> (string text) where TEnum : struct
        {
            Type enumType = typeof (TEnum);
            var retVal = default(TEnum);
            var notParsed = false;

            if (!Enum.TryParse<TEnum>(text, true, out retVal))
            {
                notParsed = true;

                if (text.StartsWith("OTHER:"))
                {
                    text = "OTHER_" + text.Substring(6);
                    if (Enum.TryParse<TEnum>(text, true, out retVal))
                        notParsed = false;
                }
            }

            if (notParsed)
            {
                AimSerializableErrorHandler.DoSerializableEvent(null,
                    new Exception("Enum [" + typeof(TEnum).Name + "] not supported values: " + text));
            }

            return retVal;
        }

        public static bool GeometryEquals (Geometries.Geometry geom1, Geometries.Geometry geom2)
        {
            if (geom1.Type != geom2.Type)
                return false;

            ///***
            ///*** Temporary solution. 20120110
            ///***

            AranPackageWriter pw = new AranPackageWriter ();
            geom1.Pack (pw);
            byte [] ba1 = pw.GetBytes ();

            pw = new AranPackageWriter ();
            geom2.Pack (pw);
            byte [] ba2 = pw.GetBytes ();

            if (ba1.Length != ba2.Length)
                return false;

            for (int i = 0; i < ba1.Length; i++)
            {
                if (ba1 [i] != ba2 [i])
                    return false;
            }

            return true;
        }
    }

    internal static class CommonXmlFunctions
    {
        public static bool ParseAixmBoolean (string value, out bool isOk)
        {
            isOk = true;

            if (value == "YES")
                return true;

            if (value == "NO")
                return false;

            if (value.StartsWith ("OTHER"))
            {
                AimSerializableErrorHandler.DoSerializableEvent (null,
                    new Exception ("CodeYesNoType not supported value: OTHER"));
                isOk = false;  
            }

            return false;
            //return bool.Parse (value);
        }

        public static Guid ParseAixmGuid (string value)
        {
            return new Guid (value);
            //return Guid.ParseExact (value, "D");
        }

        public static bool ParseHRef (string hrefValue, out string featureName, out string featureIdentifier)
        {
            int ind1 = hrefValue.IndexOf ("#xpointer(//");
            if (ind1 != -1)
            {
                int ind2 = hrefValue.IndexOf ("[gml:identifier='", ind1 + 12);
                int ind3 = hrefValue.IndexOf ("']", ind2 + 17);
                if (ind2 != -1 && ind3 != -1)
                {
                    featureName = hrefValue.Substring (ind1 + 12, ind2 - (ind1 + 12));

                    //ComSoft herden gijdiyir. aixm-5.1/<featureName>
                    var semiColon = featureName.IndexOf(':');
                    if (semiColon >= 0)
                        featureName = featureName.Substring(semiColon + 1);
                   
                    featureIdentifier = hrefValue.Substring (ind2 + 17, ind3 - (ind2 + 17));
                    return true;
                }
            }
            else
            {
                ind1 = hrefValue.IndexOf ("urn:uuid:");
                if (ind1 != -1)
                {
                    featureName = string.Empty;
                    featureIdentifier = hrefValue.Substring (ind1 + 9);
                    return true;
                }
                else if (hrefValue.StartsWith("#uuid."))
                {
                    featureName = string.Empty;
                    featureIdentifier = hrefValue.Substring(6);
                    return true;
                }
            }

            featureName = null;
            featureIdentifier = null;
            return false;
        }

        public static XmlElement GetChildElement (XmlElement xmlElement)
        {
            int startIndex = 0;
            return GetChildElement (xmlElement, ref startIndex);
        }

        public static XmlElement GetChildElement (XmlElement xmlElement, ref int startIndex)
        {
            XmlElement elem = null;

            while (startIndex < xmlElement.ChildNodes.Count)
            {
                XmlNode childNode = xmlElement.ChildNodes [startIndex];
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    elem = (XmlElement) childNode;
                    break;
                }
                startIndex++;
            }
            return elem;
        }

        public static AimObjectType GetAranObjectType (int aranTypeIndex,
            out bool isAbstract, 
            out bool dataTypeContextIsCurrentElement)
        {
            isAbstract = false;
            dataTypeContextIsCurrentElement = false;

            AllAimObjectType allType = (AllAimObjectType) aranTypeIndex;

            if (allType < AllAimObjectType._2_DATATYPE)
                return AimObjectType.Field;

            if (allType > AllAimObjectType._6_ENUM)
                return AimObjectType.Field;

            if (allType < AllAimObjectType._3_OBJECT)
            {
                if (allType == AllAimObjectType.FeatureRef ||
                    allType == AllAimObjectType.TextNote ||
                    (allType > AllAimObjectType._2_1_VALCLASS_BEGIN && allType < AllAimObjectType._2_1_VALCLASS_END) ||
                    (allType > AllAimObjectType._2_2_ABSTRACTCLASS_BEGIN && allType < AllAimObjectType._2_2_ABSTRACTCLASS_END))
                {
                    dataTypeContextIsCurrentElement = true;
                }
                return AimObjectType.DataType;
            }

            if (allType < AllAimObjectType._4_FEATURE)
                return AimObjectType.Object;

            if (allType < AllAimObjectType._5_ABSTRACT)
                return AimObjectType.Feature;

            isAbstract = true;
            return default (AimObjectType);
        }

        public static string GetElementAttribute (XmlElement xmlElement, string localName)
        {
            for (int i = 0; i < xmlElement.Attributes.Count; i++)
            {
                if (xmlElement.Attributes [i].LocalName == localName)
                    return xmlElement.Attributes [i].Value;
            }
            return null;
        }
    }
}
