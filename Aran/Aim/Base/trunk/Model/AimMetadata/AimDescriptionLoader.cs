using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Aran.Aim.Enums;
using Aran.Aim.Model.AimMetadata;

namespace Aran.Aim
{
	public class AimDescriptionLoader
	{
		public bool Load (string fileName)
		{
			var xmlDoc = new XmlDocument ();

			if (!System.IO.File.Exists (fileName))
				return false;

			try
			{
				xmlDoc.Load (fileName);
			}
			catch
			{
				return false;
			}

			var classInfiList = AimMetadata.AimClassInfoList;

			foreach (XmlNode node in xmlDoc.DocumentElement)
			{
				if (node.NodeType == XmlNodeType.Element)
				{
					var elem = node as XmlElement;
					///--- F: Feature; O: Object; E: Enum
					var name = elem.Attributes[0].Value;
					var aimTypeIndex = -1;
					
					if (elem.Name == "F")
					{
						FeatureType featType;
						if (Enum.TryParse<FeatureType> (name, out featType))
							aimTypeIndex = (int) featType;
					}
					else if (elem.Name == "O")
					{
						ObjectType objType;
						if (Enum.TryParse<ObjectType> (name, out objType))
							aimTypeIndex = (int) objType;
					}
                    else if (elem.Name == "E")
                    {
                        EnumType enumType;
                        if (Enum.TryParse<EnumType>(name, out enumType))
                        {
                            var classInfo = AimMetadata.GetClassInfoByIndex((int)enumType);
                            classInfo.Documentation = WebUtility.HtmlDecode(elem.Attributes[1].Value);
                            FillEnumDocs(elem, classInfo);
                        }

                        continue;
                    }
                    else
					{

					}

					if (aimTypeIndex > 0)
					{
						var classInfo = AimMetadata.GetClassInfoByIndex (aimTypeIndex);
						classInfo.Documentation = WebUtility.HtmlDecode(elem.Attributes[1].Value);

						FillPropDocs (elem, classInfo);
					}
				}
			}

			return true;
		}

	    private void FillEnumDocs(XmlElement elem, AimClassInfo classInfo)
	    {
            if (classInfo.EnumValues.Count>0) return;//init only once

            var enumType = AimMetadata.GetEnumType(classInfo.Index);

	        foreach (XmlNode node in elem.ChildNodes)
			{
			    if (node.NodeType != XmlNodeType.Element) continue;
			    var propElem = node as XmlElement;
			    if (propElem == null) continue;
			    
                var enumName = propElem.Attributes["Name"].Value;
			    var enumDoc = WebUtility.HtmlDecode(propElem.Attributes["Doc"].Value);

                //correct enum name
			    if ("0123456789".IndexOf(enumName.Substring(0,1))>-1)
                {
                      enumName="_"+enumName;
                }
			    enumName=enumName.Replace("-", "_minus_");
                enumName=enumName.Replace("+", "_plus_");
                enumName=enumName.Replace(" ", "");

                if (!Enum.IsDefined(enumType, enumName))
                {
                   continue;
                }

                var enumValue = Enum.Parse(enumType, enumName);
			    var enumIndex = (int)enumValue;
                classInfo.EnumValues.Add(new AimEnumInfo
                {
                    Documentation = enumDoc,
                    EnumIndex = enumIndex
                });
			}
	    }

	    private void FillPropDocs (XmlElement elem, AimClassInfo classInfo)
		{
			foreach (XmlNode node in elem.ChildNodes)
			{
				if (node.NodeType == XmlNodeType.Element)
				{
					var propElem = node as XmlElement;
					var propName = propElem.Attributes[0].Value;
					var propInfo = classInfo.Properties[propName];
					propInfo.Documentation = WebUtility.HtmlDecode(propElem.Attributes[1].Value);

					if (propElem.ChildNodes.Count > 0)
					{
						var restElem = propElem.ChildNodes[0] as XmlElement;
						foreach (XmlAttribute attr in restElem.Attributes)
						{
							if (attr.Name == "Min")
                                propInfo.Restriction.Min = Convert.ToDouble(attr.Value, CultureInfo.InvariantCulture.NumberFormat);
							else if (attr.Name == "Max")
                                propInfo.Restriction.Max = Convert.ToDouble(attr.Value, CultureInfo.InvariantCulture.NumberFormat);
                            else if (attr.Name == "Pattern")
							{
							    propInfo.Restriction.Pattern =  WebUtility.HtmlDecode(attr.Value).Replace("\\\\","\\");
							}
						}
					}
				}
			}
		}
	}
}
