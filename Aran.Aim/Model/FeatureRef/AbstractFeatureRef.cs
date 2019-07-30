using System;
using System.Collections.Generic;
using System.Xml;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;

namespace Aran.Aim.DataTypes
{
	public interface IAbstractFeatureRef
	{
		long Id { get; set; }
		Guid Identifier { get; set; }
		int FeatureTypeIndex { get; set; }
	}

	public abstract class AbstractFeatureRefBase : FeatureRef, IAbstractFeatureRef
	{
		protected int FeatureTypeIndex
		{
			get { return GetFieldValue<int>((int) PropertyAbstractFeatureRef.Type); }
			set { SetFieldValue<int>((int) PropertyAbstractFeatureRef.Type, value); }
		}

		#region IAbstractFeatureRef Members

		long IAbstractFeatureRef.Id
		{
			get { return base.Id; }
			set { base.Id = value; }
		}

		Guid IAbstractFeatureRef.Identifier
		{
			get { return base.Identifier; }
			set { base.Identifier = value; }
		}

		int IAbstractFeatureRef.FeatureTypeIndex
		{
			get { return FeatureTypeIndex; }
			set { this.FeatureTypeIndex = value; }
		}

        #endregion

	    public override void Assign(AranObject source)
	    {
	        if (source is AbstractFeatureRefBase featureRef)
	        {
                base.Assign(source);
	            FeatureTypeIndex = featureRef.FeatureTypeIndex;
	        }
	    }
    }

	public class AbstractFeatureRef<TEnum> : AbstractFeatureRefBase
		where TEnum : struct
	{
		public TEnum Type
		{
			get { return (TEnum) (object) FeatureTypeIndex; }
			set { FeatureTypeIndex = (int) (object) value; }
		}

		protected override bool AixmDeserialize(XmlContext xmlContext)
		{
			XmlElement xmlElement = xmlContext.Element;

			string attrValue = xmlElement.GetAttribute("href", "http://www.w3.org/1999/xlink");
			if (string.IsNullOrEmpty(attrValue))
				return false;

			#region NON_GUID_IDENTIFIER

			if (FeatureRef.NonGuidIdentifier)
			{
				if (attrValue.StartsWith("#"))
				{
					var keyStr = attrValue.Substring(1);
					Guid guid;
					if (GuidAssociteList.TryGetValue(keyStr, out guid))
					{
						Identifier = guid;
						return true;
					}
					else
					{
						List<FeatureRef> featRefList;

						if (!GuidWaitingList.TryGetValue(keyStr, out featRefList))
						{
							featRefList = new List<FeatureRef>();
							GuidWaitingList.Add(keyStr, featRefList);
						}
						featRefList.Add(this);

						Identifier = Guid.Empty;
						return true;
					}
				}
			}

			#endregion

			string featureName;
			string featureIdentifier;
			bool isParsed = CommonXmlFunctions.ParseHRef(attrValue, out featureName, out featureIdentifier);

			if (isParsed)
			{
				if (featureName != string.Empty)
					Type = (TEnum) Enum.Parse(typeof(TEnum), featureName, true);

				Identifier = CommonXmlFunctions.ParseAixmGuid(featureIdentifier);

				if (featureName == string.Empty &&
				    Aran.Aim.Other.AbstractFeatureRefTypeReadingHandle.Handle != null)
				{
					Aran.Aim.Other.AbstractFeatureRefTypeReadingHandle.Handle(this);
				}
			}
			else
			{
				if (attrValue.StartsWith("urn:uuid:"))
				{
					string s = attrValue.Substring(9);
					Identifier = CommonXmlFunctions.ParseAixmGuid(s);
				}
				else if (attrValue.StartsWith("#uuid."))
				{
					var s = attrValue.Substring(6);
					Identifier = CommonXmlFunctions.ParseAixmGuid(s);
				}
			}

			return true;
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyAbstractFeatureRef
    {
        Type = PropertyFeatureRef.NEXT_CLASS,
        NEXT_CLASS
    }

    public static class MetadataAbstractFeatureRef<TEnum> where TEnum : struct
    {
        public static AimPropInfoList PropInfoList;

        static MetadataAbstractFeatureRef ()
        {
            PropInfoList = MetadataFeatureRef.PropInfoList.Clone ();

            string enumTypeName = typeof (TEnum).Name;

            int typeIndex = (int) Enum.Parse (typeof (EnumType), enumTypeName);
            PropInfoList.Add (PropertyAbstractFeatureRef.Type, typeIndex);
        }
    }
}

namespace Aran.Aim.Other
{
    using Aran.Aim.DataTypes;

    public delegate void AbstractFeatureRefTypeReading (AbstractFeatureRefBase sender);

    public static class AbstractFeatureRefTypeReadingHandle
    {
        public static AbstractFeatureRefTypeReading Handle { get; set; }
    }
}