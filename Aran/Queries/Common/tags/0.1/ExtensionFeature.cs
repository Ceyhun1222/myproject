using System;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Aim.Objects;

namespace Aran.Queries
{
	public static class ExtensionFeature
	{
		public static ICommonQPI CommonQPI;

		public static TFeature GetFeature<TFeature>(this FeatureRef featureRef) where TFeature : Feature
		{
			if (AimMetadata.IsAbstractFeatureRef(featureRef))
			{
				int featureTypeIndex = ((IAbstractFeatureRef)featureRef).FeatureTypeIndex;
				return (TFeature)CommonQPI.GetFeature((FeatureType)featureTypeIndex, featureRef.Identifier);
			}

			return CommonQPI.GetFeature<TFeature>(featureRef.Identifier);

			//return null;
		}

		public static FeatureRef GetFeatureRef( this Feature feature)
		{
			FeatureRef featureRef = new FeatureRef();
			featureRef.Identifier = feature.Identifier;

			if (AimMetadata.IsAbstract(AimMetadata.GetAimTypeIndex(featureRef)))
			{
				((IAbstractFeatureRef)featureRef).FeatureTypeIndex = (int)feature.FeatureType;
			}

			return featureRef;
		}

		public static TFeatureRef GetAbstractFeatureRef<TFeatureRef>(this Feature feature) where TFeatureRef : AbstractFeatureRefBase, new()
		{
			TFeatureRef featureRef = new TFeatureRef();
			featureRef.Identifier = feature.Identifier;
			((IAbstractFeatureRef)featureRef).FeatureTypeIndex = (int)feature.FeatureType;
			return featureRef;
		}

		public static FeatureRef GetFeatureRef(this ChoiceClass choiceClass)
		{
			IEditChoiceClass editChoice = choiceClass as IEditChoiceClass;
			if (editChoice.RefValue.PropertyType == AimPropertyType.DataType)
				return (FeatureRef)editChoice.RefValue;

			return null;
		}

		public static AObject GetObject(this ChoiceClass choiceClass)
		{
			IEditChoiceClass editChoice = choiceClass as IEditChoiceClass;
			if (editChoice.RefValue.PropertyType == AimPropertyType.Object)
				return (AObject)editChoice.RefValue;

			return null;
		}

		public static FeatureRefObject GetFeatureRefObject(this Feature feature)
		{
			FeatureRefObject fro = new FeatureRefObject();
			fro.Feature = feature.GetFeatureRef();
			return fro;
		}
	}
}
