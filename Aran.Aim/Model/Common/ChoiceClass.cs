using System;
using System.Xml;
using Aran.Aixm;
using Aran.Aim.DataTypes;
using Aran.Aim.PropertyEnum;
using Aran.Package;

namespace Aran.Aim.Objects
{
    public interface IEditChoiceClass
    {
        int RefType { get; set; }
        IAimProperty RefValue { get; set; }
    }

    public abstract class ChoiceClass : 
        AObject,
        IEditChoiceClass
    {
        protected int RefType
        {
            get { return GetFieldValue<int> ((int) PropertyChoiceClass.RefType); }
            set { SetFieldValue<int> ((int) PropertyChoiceClass.RefType, value); }
        }

        protected FeatureRef RefFeature
        {
            get { return (FeatureRef) RefValue; }
            set { RefValue = value; }
        }

        protected IAbstractFeatureRef RefAbstractFeature
        {
            get { return (IAbstractFeatureRef) RefValue; }
            set { RefValue = value as IAimProperty; }
        }

        protected AObject GetChoiceAbstractObject (AbstractType abstractType)
        {
            return GetAbstractObject ((int) PropertyChoiceClass.RefValue, abstractType);
        }

        protected IAimProperty RefValue
        {
            get { return GetValue ((int) PropertyChoiceClass.RefValue); }
            set { SetValue ((int) PropertyChoiceClass.RefValue, value); }
        }

        protected override bool AixmDeserialize (XmlContext context)
        {
            XmlElement xmlElement = context.Element;

            int ind = xmlElement.LocalName.IndexOf ("_");
            if (ind == -1)
                return false;

            string refTypeText = xmlElement.LocalName.Substring (ind + 1);

            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex (this);
            AimPropInfo refTypePropInfo = classInfo.Properties [refTypeText];

            bool isAbstract;
            bool dataTypeContextIsCurrentElement;
            AimObjectType aranObjectType = CommonXmlFunctions.GetAranObjectType (refTypePropInfo.TypeIndex,
                       out isAbstract,
                       out dataTypeContextIsCurrentElement);

            XmlElement contextElement;
            if (dataTypeContextIsCurrentElement)
                contextElement = xmlElement;
            else
                contextElement = CommonXmlFunctions.GetChildElement (xmlElement);

            var typeIndex = refTypePropInfo.TypeIndex;
            if (refTypePropInfo.PropType.IsAbstract)
                typeIndex = (int)Enum.Parse(typeof(ObjectType), contextElement.LocalName);

            IAixmSerializable tmpObj = AimObjectFactory.Create(typeIndex);
            context.Element = contextElement;
            bool isDeserialized = tmpObj.AixmDeserialize (context);
            if (isDeserialized)
            {
                RefValue = tmpObj as IAimProperty;
                if (refTypePropInfo.IsFeatureReference)
                    RefType = (int) refTypePropInfo.ReferenceFeature;
                else
                    RefType = refTypePropInfo.TypeIndex;

                return true;
            }
            return false;
        }

        #region IEditChoiceClass Members

        int IEditChoiceClass.RefType
        {
            get { return this.RefType; }
            set { this.RefType = value; }
        }

        IAimProperty IEditChoiceClass.RefValue
        {
            get { return this.RefValue; }
            set { this.RefValue = value; }
        }

        #endregion

        protected override void Pack (PackageWriter writer)
        {
            writer.PutInt32 (RefType);
            IAimObject refValueObject = RefValue as IAimObject;
            writer.PutInt32 (AimMetadata.GetAimTypeIndex (refValueObject));
            (refValueObject as IPackable).Pack (writer);
        }
        protected override void Unpack (PackageReader reader)
        {
            RefType = reader.GetInt32 ();
            int aimTypeIndex = reader.GetInt32 ();
            IAimObject aimObject = AimObjectFactory.Create (aimTypeIndex);
            (aimObject as IPackable).Unpack (reader);
            RefValue = aimObject as IAimProperty;
        }
    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyChoiceClass
    {
        RefType = PropertyAObject.NEXT_CLASS,
        RefValue,
        NEXT_CLASS
    }
}
