using Aran.Aim.DataTypes;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;
using System;

namespace Aran.Aim.Objects
{
	public class FeatureRefObject : AObject
	{
        public FeatureRefObject ()
        {
        }

        public FeatureRefObject (Guid identifier)
        {
            Feature = new FeatureRef (identifier);
        }

		public override ObjectType ObjectType
		{
			get { return ObjectType.FeatureRefObject; }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyFeatureRefObject.Feature); }
			set { SetValue ((int) PropertyFeatureRefObject.Feature, value); }
		}

        protected override bool AixmDeserialize (XmlContext context)
        {
            FeatureRef featureRef = new FeatureRef ();
            bool isDeserialized = ((IAixmSerializable) featureRef).AixmDeserialize (context);
            
            if (!isDeserialized)
                return false;

            Feature = featureRef;
            return true;
        }
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFeatureRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS
	}

    public static class MetadataFeatureRefObject
    {
        public static AimPropInfoList PropInfoList;

        static MetadataFeatureRefObject ()
        {
            PropInfoList = MetadataAObject.PropInfoList.Clone ();

            PropInfoList.Add (PropertyFeatureRefObject.Feature, (int) DataType.FeatureRef);
        }
    }
}
