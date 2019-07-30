#define COMSOFT

using System;
using System.Collections.Generic;
using System.Xml;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;

namespace Aran.Aim.DataTypes
{
    public class FeatureRef :
        ADataType
    {
        #region NON_GUID_IDENTIFIER
        public static Dictionary<string, Guid> GuidAssociteList = new Dictionary<string, Guid>();
        public static Dictionary<string, List<FeatureRef>> GuidWaitingList = new Dictionary<string, List<FeatureRef>>();
#if NON_GUID_IDENTIFIER
        public static bool NonGuidIdentifier = true;
#else
        public static bool NonGuidIdentifier = false;
#endif
        #endregion

        public FeatureRef ()
        {
        }

        public FeatureRef (long id)
        {
            Id = id;
        }

        public FeatureRef (Guid identifier)
        {
            Identifier = identifier;
        }

        public override DataType DataType
        {
            get { return DataType.FeatureRef; }
        }

        public long Id
        {
            get { return GetFieldValue<long> ((int) PropertyFeatureRef.Id); }
            set { SetFieldValue<long> ((int) PropertyFeatureRef.Id, value); }
        }

        public Guid Identifier
        {
            get { return GetFieldValue<Guid> ((int) PropertyFeatureRef.Identifier); }
            set { SetFieldValue<Guid> ((int) PropertyFeatureRef.Identifier, value); }
        }

        protected override bool AixmDeserialize (XmlContext xmlContext)
        {
            XmlElement xmlElement = xmlContext.Element;
            string attrValue = null;
            for (int i = 0; i < xmlElement.Attributes.Count; i++)
            {
                if (xmlElement.Attributes [i].LocalName == "href")
                {
                    attrValue = xmlElement.Attributes [i].Value;
                    break;
                }
            }

            if (attrValue == null)
                return false;

            #region NON_GUID_IDENTIFIER
#if NON_GUID_IDENTIFIER
            if (attrValue.StartsWith("#")){
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
#endif
            #endregion

            int indStart = attrValue.IndexOf ("identifier='");
            int indEnd = attrValue.IndexOf ('\'', indStart + 12);

            if (indStart != -1 && indEnd != -1)
            {
                string s = attrValue.Substring (indStart + 12, indEnd - indStart - 12);
                Identifier = CommonXmlFunctions.ParseAixmGuid (s);


                //*** Added in Georgia.
                //*** Parse featureType for Extension 'GetFeature' method in Queries.Common.
                //*** Example: urn:uuid:#xpointer(//aixm:OrganisationAuthority[gml:identifier='64bde3c2-807f-4026-a25e-01d84ab55963'])
#if COMSOFT
                indStart = attrValue.IndexOf("#xpointer(");
                indEnd = attrValue.IndexOf('[', indStart + 17);
                var featTypeStr = attrValue.Substring(indStart + 17, indEnd - indStart - 17);

                FeatureType ft;
                if (Enum.TryParse<FeatureType>(featTypeStr, true, out ft))
                    FeatureType = ft;
#endif

            }
            else if (attrValue.StartsWith ("urn:uuid:"))
            {
                string s = attrValue.Substring (9);
                Identifier = CommonXmlFunctions.ParseAixmGuid (s);
            }
            else if (attrValue.StartsWith("#uuid."))
            {
                var s = attrValue.Substring(6);
                Identifier = CommonXmlFunctions.ParseAixmGuid(s);
            }

            return true;
        }

#if COMSOFT
        public override void Assign(AranObject source)
        {
            if (source is FeatureRef featureRef)
            {
                Id = featureRef.Id;
                Identifier = featureRef.Identifier;
                FeatureType = featureRef.FeatureType;
            }
        }

        public FeatureType? FeatureType { get; set; }
#endif
    }
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFeatureRef
	{
		Id,
		Identifier,
        NEXT_CLASS
	}

    public static class MetadataFeatureRef
    {
        public static AimPropInfoList PropInfoList;

        static MetadataFeatureRef ()
        {
            PropInfoList = MetadataADataType.PropInfoList.Clone ();

            PropInfoList.Add (PropertyFeatureRef.Id, (int) AimFieldType.SysInt64);
            PropInfoList.Add (PropertyFeatureRef.Identifier, (int) AimFieldType.SysGuid);
        }
    }
}
