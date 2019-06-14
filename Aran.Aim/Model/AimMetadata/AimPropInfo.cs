using System.Collections.Generic;

namespace Aran.Aim
{
    #region DebuggerDisplay
    [System.Diagnostics.DebuggerDisplay ("Name = {Name}, AixmName = {AixmName}")]
    #endregion
    public class AimPropInfo
    {
		public AimPropInfo()
		{
			Restriction = new PropRestriction ();
		}

        public string Name { get; set; }
        public int Index { get; set; }
        public int TypeIndex { get; set; }
        public PropertyTypeCharacter TypeCharacter { get; set; }
        public string AixmName { get; set; }
        public bool IsFeatureReference { get; set; }
        public FeatureType ReferenceFeature { get; set; }
        public AimClassInfo PropType { get; set; }
        public object Tag { get; set; }
		public string Documentation { get; set; }
		public PropRestriction Restriction { get; private set; }
        public int ItemIndex {get; set;}

        public bool IsList
        {
            get { return ((TypeCharacter & PropertyTypeCharacter.List) == PropertyTypeCharacter.List); }
        }

        public bool IsNullable
        {
            get { return ((TypeCharacter & PropertyTypeCharacter.Nullable) == PropertyTypeCharacter.Nullable); }
        }

        public bool IsExtension
        {
            get { return (Name == "Extension"); }
        }

        /// <summary>
        /// PropType not Cloned...
        /// </summary>
        /// <returns></returns>
        public AimPropInfo Clone ()
        {
            var propInfo = new AimPropInfo ();
            propInfo.Assign (this);
            return propInfo;
        }

        /// <summary>
        /// PropType not Assigned...
        /// </summary>
        /// <param name="source"></param>
		public void Assign(AimPropInfo source)
		{
			Name = source.Name;
			Index = source.Index;
			TypeIndex = source.TypeIndex;
			TypeCharacter = source.TypeCharacter;
			AixmName = source.AixmName;
			IsFeatureReference = source.IsFeatureReference;
			ReferenceFeature = source.ReferenceFeature;
			PropType = source.PropType;
			Documentation = source.Documentation;
			
			Restriction.Min = source.Restriction.Min;
			Restriction.Max = source.Restriction.Max;
			Restriction.Pattern = source.Restriction.Pattern;
		}
    }

	public class PropRestriction
	{
		public double? Min { get; set; }
		public double? Max { get; set; }
		public string Pattern { get; set; }
	}

    public class AimPropInfoList : List<AimPropInfo>
    {
        public void Add (object propertyEnumItem, int typeIndex)
        {
            Add (propertyEnumItem, typeIndex, (PropertyTypeCharacter) 0);
        }

        public void Add (object propertyEnumItem, int typeIndex, PropertyTypeCharacter typeCharacter)
        {
            string name = propertyEnumItem.ToString ();
            int index = (int) propertyEnumItem;

            Add (name, index, typeIndex, typeCharacter);
        }

        public void Add (object propertyEnumItem, int typeIndex, string aixmName)
        {
            Add (propertyEnumItem, typeIndex, (PropertyTypeCharacter) 0, aixmName);
        }

        public void Add (object propertyEnumItem, int typeIndex, PropertyTypeCharacter typeCharacter, string aixmName)
        {
            string name = propertyEnumItem.ToString ();
            int index = (int) propertyEnumItem;

            Add (name, index, typeIndex, typeCharacter, aixmName);
        }


        public void Add (string name, int index, int typeIndex)
        {
            Add (name, index, typeIndex, (PropertyTypeCharacter) 0);
        }

        public void Add (string name, int index, int typeIndex, PropertyTypeCharacter typeCharacter)
        {
            string aixmName = MakeFirstLatterLower (name);
            Add (name, index, typeIndex, typeCharacter, aixmName);
        }

        public void Add (string name, int index, int typeIndex, string aixmName)
        {
            Add (name, index, typeIndex, (PropertyTypeCharacter) 0, aixmName);
        }

        public void Add(string name, int index, int typeIndex, PropertyTypeCharacter typeCharacter, string aixmName)
        {
            var propInfo = new AimPropInfo {
                Name = name,
                Index = index,
                TypeCharacter = typeCharacter,
                AixmName = aixmName
            };

            AimObjectType aranObjectType = AimMetadata.GetAimObjectType(typeIndex);

            if (aranObjectType == AimObjectType.Feature) {
                if ((typeCharacter & PropertyTypeCharacter.List) != PropertyTypeCharacter.None)
                    propInfo.TypeIndex = (int)ObjectType.FeatureRefObject;
                else
                    propInfo.TypeIndex = (int)DataType.FeatureRef;

                propInfo.IsFeatureReference = true;
                propInfo.ReferenceFeature = (FeatureType)typeIndex;
            }
            else {
                propInfo.TypeIndex = typeIndex;
                propInfo.IsFeatureReference = false;

                if (AimMetadata.IsAbstractFeatureRef(typeIndex)) {
                    propInfo.IsFeatureReference = true;
                    propInfo.ReferenceFeature = 0;
                }
            }

            propInfo.ItemIndex = Count;

            Add(propInfo);
        }


        public AimPropInfo this [string name]
        {
            get
            {
                foreach (var item in this)
                {
                    if (string.Equals (item.Name, name, System.StringComparison.CurrentCultureIgnoreCase))
                        return item;
                }
                return null;
            }
        }

        public AimPropInfoList Clone ()
        {
            AimPropInfoList propInfoList = new AimPropInfoList ();
            propInfoList.AddRange (this);
            return propInfoList;
        }

        private static string MakeFirstLatterLower (string text)
        {
            if (text.Length > 1 && char.IsUpper (text [0]) && char.IsLower (text [1]))
                return char.ToLower (text [0]) + text.Substring (1);

            return text;
        }
    }
}