using System;
using System.Collections.Generic;
using System.Xml;
using Aran.Aixm;
using Aran.Package;
using System.ComponentModel;
using System.Collections;
using System.Collections.Concurrent;
using Aran.Aim.Objects;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Aim.DataTypes;

namespace Aran.Aim
{
    public abstract class AimObject :
        AranObject,
        IAimObject,
        IPackable,
        IAranCloneable,
        IAixmSerializable,
        IAimNotifyPropertyChanged
    {
        protected AimObject()
        {
            _values = new ConcurrentDictionary<int, IAimProperty>();
        }

        protected abstract AimObjectType AimObjectType { get; }

		#region GetValue

		protected IAimProperty GetValue ( int propertyIndex )
		{
			if ( _values.ContainsKey ( propertyIndex ) )
			{
				if ( _values[ propertyIndex ] is PointExtension)
				{
					Point pnt = null;
					string elev = string.Empty;
					foreach ( var key in _values.Keys )
					{
						if ( _values[ key ] is AimField<Point> )
							pnt = ( _values[ key ] as AimField<Point> ).Value;
						if ( _values[ key ] is ValDistanceVertical )
						{
							var valDist = _values[ key ] as ValDistanceVertical;
							if ( !double.IsNaN ( valDist.Value ) )
								elev += valDist.Value.ToString ( ) + valDist.Uom;
						}
					}
					if ( pnt != null )
						return CalculateExtension ( pnt, elev );
				}
				return _values[ propertyIndex ];
			}
			else
				return null;
        }

        protected T GetFieldValue<T>(int propertyIndex)
        {
            object value = GetValue(propertyIndex);

            if (value != null)
                return (T)((dynamic)value).Value;

            T t = default(T);
            if (t != null)
            {
                AimField<T> aranField = new AimField<T>(t);
                SetValue(propertyIndex, aranField);
                return aranField.Value;
            }
            return t;
        }

        protected Nullable<T> GetNullableFieldValue<T>(int propertyIndex) where T : struct
        {
            object value = GetValue(propertyIndex);
            if (value == null)
                return null;
            return (Nullable<T>)((AimField<T>)value).Value;
        }

        #endregion

        #region SetValue

        protected void SetFieldValue<T>(int propertyIndex, T value)
        {
            if (value == null)
            {
                SetValuePrivate(propertyIndex, null);
            }
            else
            {
                object storeValue = GetValue(propertyIndex);
                if (storeValue == null)
                {
                    SetValuePrivate(propertyIndex, new AimField<T>(value));
                }
                else
                {
                    ((AimField<T>)storeValue).Value = value;
                    OnPropertyChanged(propertyIndex);
                }
            }
        }

        protected void SetNullableFieldValue<T>(int propertyIndex, Nullable<T> value) where T : struct
        {
            if (value == null)
            {
                SetValuePrivate(propertyIndex, null);
            }
            else
            {
                object storeValue = GetValue(propertyIndex);
                if (storeValue == null)
                {
                    SetValuePrivate(propertyIndex, new AimField<T>(value.Value));
                }
                else
                {
                    ((AimField<T>)storeValue).Value = value.Value;
                    OnPropertyChanged(propertyIndex);
                }
            }
        }

        protected void SetValue(int propertyIndex, IAimProperty value)
        {
            SetValuePrivate(propertyIndex, value);
        }

        protected void SetValuePrivate(int propertyIndex, IAimProperty value)
        {
            if (_values.ContainsKey(propertyIndex))
            {
                if (_values[propertyIndex] != value)
                {
                    _values[propertyIndex] = value;
                    OnPropertyChanged(propertyIndex);
                }
            }
            else
            {
                // _values.Add (propertyIndex, value);
                _values[propertyIndex] = value;
                OnPropertyChanged(propertyIndex);
            }
        }

        #endregion

        #region IAimObject Members

        AimObjectType IAimObject.AimObjectType
        {
            get { return this.AimObjectType; }
        }

        IAimProperty IAimObject.GetValue(int propertyIndex)
        {
            return this.GetValue(propertyIndex);
        }

        void IAimObject.SetValue(int propertyIndex, IAimProperty value)
        {
            this.SetValue(propertyIndex, value);
        }

        int[] IAimObject.GetPropertyIndexes()
        {
            List<int> indexList = new List<int>();
            foreach (int key in _values.Keys)
            {
                IAimProperty propVal = _values[key];
                if (propVal != null && (propVal.PropertyType != AimPropertyType.List || ((IList)propVal).Count > 0))
                {
                    indexList.Add(key);
                }
            }
            indexList.Sort();
            return indexList.ToArray();
        }

        #endregion

        #region IPackable Members

        void IPackable.Pack(PackageWriter writer)
        {
            this.Pack(writer);
        }

        void IPackable.Unpack(PackageReader reader)
        {
            this.Unpack(reader);
        }

        protected virtual void Pack(PackageWriter writer)
        {
            int[] propIndexes = (this as IAimObject).GetPropertyIndexes();

            writer.PutInt32(propIndexes.Length);
            foreach (var item in propIndexes)
                writer.PutInt32(item);

            AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos(this);

            for (int i = 0; i < propIndexes.Length; i++)
            {

                IAimProperty aimPropVal = GetValue(propIndexes[i]);

                if (aimPropVal.PropertyType == AimPropertyType.Object)
                {

                    AimPropInfo propInfo = null;
                    foreach (var item in propInfos)
                    {
                        if (item.Index == propIndexes[i])
                        {
                            propInfo = item;
                            break;
                        }
                    }

                    if (propInfo.PropType.IsAbstract)
                        writer.PutInt32((int)(aimPropVal as AObject).ObjectType);
                }

                (aimPropVal as IPackable).Pack(writer);
            }
        }

        protected virtual void Unpack(PackageReader reader)
        {
            int propIndexCount = reader.GetInt32();
            int[] propIndexes = new int[propIndexCount];
            for (int i = 0; i < propIndexes.Length; i++)
            {
                propIndexes[i] = reader.GetInt32();
            }

            AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos(this);

            for (int i = 0; i < propIndexes.Length; i++)
            {
                AimPropInfo propInfo = null;
                foreach (var item in propInfos)
                {
                    if (item.Index == propIndexes[i])
                    {
                        propInfo = item;
                        break;
                    }
                }

                IPackable packable;

                if (propInfo.IsList)
                {
                    IList list = AimObjectFactory.CreateList(propInfo.TypeIndex);
                    packable = (IPackable)list;
                }
                else
                {
                    var typeIndex = propInfo.TypeIndex;

                    if (propInfo.PropType.AimObjectType == Aim.AimObjectType.Object && propInfo.PropType.IsAbstract)
                        typeIndex = reader.GetInt32();

                    packable = AimObjectFactory.Create(typeIndex) as IPackable;
                }

                packable.Unpack(reader);
                this.SetValue(propIndexes[i], (IAimProperty)packable);
            }
        }

        #endregion

        #region IAranClonable Members

        public virtual void Assign(AranObject source)
        {
            AimObject aimSource = (AimObject)source;
            int thisIndex = AimMetadata.GetAimTypeIndex(this);
            int sourceIndex = AimMetadata.GetAimTypeIndex(aimSource);

            _values.Clear();

            if (thisIndex == sourceIndex)
            {
                int[] propIndexes = (aimSource as IAimObject).GetPropertyIndexes();
                foreach (int propIndex in propIndexes)
                {
                    IAimProperty propVal = aimSource._values[propIndex];
                    IAranCloneable cloneable = propVal as IAranCloneable;
                    if (cloneable != null)
                    {
                        IAimProperty newPropVal = cloneable.Clone() as IAimProperty;
                        if (newPropVal != null)
                        {
                            if (newPropVal.PropertyType == AimPropertyType.List)
                            {
                                IList list = newPropVal as IList;
                                for (int i = 0; i < list.Count; i++)
                                    (list[i] as DBEntity).Id = 0;
                            }

                            _values.Add(propIndex, newPropVal);
                        }
                    }
                }
            }
            else    //--- Find Common Parent.
            {
                AimClassInfo thisClassInfo = AimMetadata.GetClassInfoByIndex(thisIndex);
                AimClassInfo sourceClassInfo = AimMetadata.GetClassInfoByIndex(sourceIndex);

                List<AimClassInfo> tciList = new List<AimClassInfo>();
                tciList.Add(thisClassInfo);
                AimClassInfo aci = thisClassInfo;
                while (aci.Parent != null)
                {
                    tciList.Add(aci.Parent);
                    aci = aci.Parent;
                }

                List<AimClassInfo> sciList = new List<AimClassInfo>();
                sciList.Add(sourceClassInfo);
                aci = sourceClassInfo;
                while (aci.Parent != null)
                {
                    sciList.Add(aci.Parent);
                    aci = aci.Parent;
                }

                tciList.Reverse();
                sciList.Reverse();
                int count = Math.Min(tciList.Count, sciList.Count);

                AimClassInfo commonClassInfo = null;

                for (int i = 0; i < count - 1; i++)
                {
                    if (tciList[i] == sciList[i] &&
                        tciList[i + 1] != sciList[i + 1])
                    {
                        commonClassInfo = tciList[i];
                        break;
                    }
                }

                if (commonClassInfo != null)
                {
                    foreach (AimPropInfo propInfo in commonClassInfo.Properties)
                    {
                        if (aimSource._values.ContainsKey(propInfo.Index))
                        {
                            IAimProperty propVal = aimSource._values[propInfo.Index];
                            IAranCloneable cloneable = propVal as IAranCloneable;
                            if (cloneable != null)
                            {
                                IAimProperty newPropVal = cloneable.Clone() as IAimProperty;

                                if (newPropVal.PropertyType == AimPropertyType.List)
                                {
                                    IList list = newPropVal as IList;
                                    for (int i = 0; i < list.Count; i++)
                                        (list[i] as DBEntity).Id = 0;
                                }

                                if (newPropVal != null)
                                    _values.Add(propInfo.Index, newPropVal);
                            }
                        }
                    }
                }
            }

            if (Assigned != null)
                Assigned(this, new AimPropertyChangedEventArgs(-1));
        }

        public virtual AranObject Clone()
        {
            AimObject aranObject = AimObjectFactory.Create(AimMetadata.GetAimTypeIndex(this));
            aranObject.Assign(this);
            return aranObject;
        }

        #endregion

        #region IAixmSerializable Members

        bool IAixmSerializable.AixmDeserialize(XmlContext context)
        {
            return this.AixmDeserialize(context);
        }

        protected virtual bool AixmDeserialize(XmlContext context)
        {
            XmlElement xmlElement = context.Element;
            int elementIndex = context.ElementIndex.Start;
            int elementEndIndex = (context.ElementIndex.End == -1 ?
                xmlElement.ChildNodes.Count : context.ElementIndex.End);

            if (elementIndex > elementEndIndex)
                return false;

            XmlNode childNode = xmlElement.ChildNodes[elementIndex];
            if (childNode == null)
                return true;

            int propertyIndex = context.PropertyIndex.Start;
            int thisAimTypeIndex = AimMetadata.GetAimTypeIndex(this);
            AimPropInfo[] aranPropInfoArr = AimMetadata.GetAimPropInfos(thisAimTypeIndex);
            if (elementIndex > elementEndIndex)
                return false;

            
            bool isTrafficSeparationService = (AimObjectType == Aim.AimObjectType.Feature && (
                (this as Features.Feature).FeatureType == FeatureType.AirTrafficControlService || 
                (this as Features.Feature).FeatureType == FeatureType.GroundTrafficControlService));

            
            XmlContext defaultContext = new XmlContext();
            defaultContext.ElementIndex.End = -1;
            defaultContext.PropertyIndex.End = -1;

            DeserializeLastException.Exception = null;

            if (childNode.NodeType == XmlNodeType.Element)
                DeserializeLastException.AddPropName(childNode.LocalName);

            while (true)
            {
                if (childNode.NodeType != XmlNodeType.Element)
                {
                    elementIndex++;
                    if (elementIndex >= elementEndIndex)
                        break;
                    childNode = xmlElement.ChildNodes[elementIndex];

                    if (childNode.NodeType == XmlNodeType.Element)
                        DeserializeLastException.ReplaceLastPropName(childNode.LocalName);

                    continue;
                }

                var childElem = childNode as XmlElement;
                AimPropInfo propInfo = aranPropInfoArr[propertyIndex];

                if (isTrafficSeparationService && childElem.LocalName == "dataLinkChannel")
                {
                    elementIndex++;
                    if (elementIndex >= elementEndIndex)
                        break;
                    childNode = xmlElement.ChildNodes[elementIndex];

                    if (childNode.NodeType == XmlNodeType.Element)
                        DeserializeLastException.ReplaceLastPropName(childNode.LocalName);

                    continue;
                }

                try
                {
                    if (AimMetadata.IsChoice(propInfo.TypeIndex))
                    {
                        if (childElem.LocalName.StartsWith(propInfo.Name + "_", StringComparison.CurrentCultureIgnoreCase))
                        {
                            IAimProperty tmpObj = AimObjectFactory.CreateAimProperty(propInfo);
                            IAixmSerializable tmpObjSerializable = tmpObj.GetAixmSerializable();
                            defaultContext.Element = childElem;
                            bool isDeserialized = tmpObjSerializable.AixmDeserialize(defaultContext);

                            if (isDeserialized)
                                this.SetValue(propInfo.Index, tmpObj);
                            else if (DeserializeLastException.HasError)
                                DeserializeLastException.AddErrorInfo();

                            elementIndex++;
                            if (elementIndex >= elementEndIndex)
                                break;
                            childNode = xmlElement.ChildNodes[elementIndex];
                        }
                    }
                    else if (propInfo.AixmName == childElem.LocalName)
                    {
                        XmlElement contextElement = null;

                        if ((propInfo.TypeCharacter & PropertyTypeCharacter.List) == PropertyTypeCharacter.List)
                        {
                            XmlContext xmlContext = new XmlContext(xmlElement);
                            xmlContext.ElementIndex.Start = elementIndex;

                            int listElementEndIndex = elementIndex + 1;
                            while (listElementEndIndex < xmlElement.ChildNodes.Count &&
                                propInfo.AixmName == xmlElement.ChildNodes[listElementEndIndex].LocalName)
                            {
                                listElementEndIndex++;
                            }

                            xmlContext.ElementIndex.End = listElementEndIndex;

                            IAimProperty tmpObj = AimObjectFactory.CreateAimProperty(propInfo);
                            IAixmSerializable tmpObjSerializable = tmpObj.GetAixmSerializable();
                            bool isDesirialized = tmpObjSerializable.AixmDeserialize(xmlContext);
                            if (isDesirialized)
                                this.SetValue(propInfo.Index, tmpObj);
                            else if (DeserializeLastException.HasError)
                                DeserializeLastException.AddErrorInfo();

                            elementIndex = listElementEndIndex - 1;
                        }
                        else
                        {
                            bool isAbstract;
                            bool dataTypeContextIsCurrentElement;

                            AimObjectType propAranObjectType = CommonXmlFunctions.GetAranObjectType(
                                propInfo.TypeIndex,
                                out isAbstract,
                                out dataTypeContextIsCurrentElement);

                            if (isAbstract)
                            {
                                contextElement = CommonXmlFunctions.GetChildElement(childElem);
                                if (contextElement != null)
                                    propInfo.TypeIndex = (int)Enum.Parse(typeof(ObjectType), contextElement.LocalName);
                            }
                            else
                            {
                                switch (propAranObjectType)
                                {
                                    case AimObjectType.Object:
                                        contextElement = CommonXmlFunctions.GetChildElement(childElem);
                                        break;
                                    case AimObjectType.DataType:
                                        {
                                            if (dataTypeContextIsCurrentElement)
                                                contextElement = childElem;
                                            else
                                                contextElement = CommonXmlFunctions.GetChildElement(childElem);

                                            break;
                                        }
                                    default:
                                        contextElement = childElem;
                                        break;
                                }
                            }

                            if (contextElement != null)
                            {
                                var nillAtrr = contextElement.Attributes["nil", "http://www.w3.org/2001/XMLSchema-instance"];
                                if (nillAtrr == null || nillAtrr.Value != "true")
                                {
                                    IAimProperty tmpObj = AimObjectFactory.CreateAimProperty(propInfo);

#warning Error occured, AimObjectFactory.CreateEnum created liar AimField for ValBaseClass.
                                    if (thisAimTypeIndex == (int)FeatureType.StandardLevelColumn && propInfo.Name == "UnitOfMeasurement")
                                        tmpObj = new AimField<Aim.Enums.UomDistanceVertical>();

                                    bool isDesirialized = false;

                                    if (thisAimTypeIndex == (int) FeatureType.RulesProcedures &&
                                        propInfo.Name == "Content")
                                    {
                                        (tmpObj as IEditAimField).FieldValue = contextElement.InnerXml;
                                        isDesirialized = true;
                                    }
                                    else
                                    {
                                        IAixmSerializable tmpObjSerializable = tmpObj.GetAixmSerializable();
                                        defaultContext.Element = contextElement;
                                        isDesirialized = tmpObjSerializable.AixmDeserialize(defaultContext);

                                        if (isDesirialized && propInfo.TypeIndex == (int) AimFieldType.SysString &&
                                            propInfo.Restriction != null)
                                        {
                                            string tmp = ((IEditAimField) tmpObj).FieldValue.ToString();
                                            if (propInfo.Restriction.Max.HasValue &&
                                                propInfo.Restriction.Max <= tmp.Length)
                                            {
                                                ((IEditAimField) tmpObj).FieldValue =
                                                    tmp.Substring(0, (int) propInfo.Restriction.Max.Value);
                                                DeserializeLastException.AddWarningInfo("Text cutted");
                                            }
                                        }
                                    }

                                    if (isDesirialized)
                                        this.SetValue(propInfo.Index, tmpObj);
                                    else if (DeserializeLastException.HasError)
                                        DeserializeLastException.AddErrorInfo();
                                }
                            }
                        }

                        elementIndex++;
                        if (elementIndex >= elementEndIndex)
                            break;

                        childNode = xmlElement.ChildNodes[elementIndex];

                        if (childNode.NodeType == XmlNodeType.Element)
                            DeserializeLastException.ReplaceLastPropName(childNode.LocalName);
                    }
                }
                catch (Exception ex)
                {
                    DeserializeLastException.Exception = ex;
                    return false;
                }

                propertyIndex++;
                if (propertyIndex >= aranPropInfoArr.Length)
                    break;
            }

            DeserializeLastException.RemoveLastPropName();

            return true;
        }

        #endregion

        public event AimPropertyChangedEventHandler AimPropertyChanged;
        public event AimPropertyChangedEventHandler Assigned;

        public override bool Equals(object obj)
        {
            if (obj is AimObject)
            {
                return AimObject.IsEquals(this, ((AimObject)obj));
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool IsEquals(AimObject aimObj1, AimObject aimObj2, bool excludeId = false)
        {
            IAimObject ao1 = aimObj1;
            IAimObject ao2 = aimObj2;

            if (ao1.AimObjectType != ao2.AimObjectType)
                return false;


            if (AimMetadata.GetAimTypeIndex(ao1) != AimMetadata.GetAimTypeIndex(ao2))
                return false;

            int[] propInd1 = ao1.GetPropertyIndexes();
            int[] propInd2 = ao2.GetPropertyIndexes();

            DeleteExcludeIndices(excludeId, ref propInd1, ref propInd2, ao1);


            if (propInd1.Length != propInd2.Length)
                return false;

            for (int i = 0; i < propInd1.Length; i++)
            {
                if (propInd1[i] != propInd2[i])
                    return false;
            }

            for (int i = 0; i < propInd1.Length; i++)
            {
                IAimProperty propVal1 = ao1.GetValue(propInd1[i]);
                IAimProperty propVal2 = ao2.GetValue(propInd2[i]);

                if (propVal1.PropertyType != propVal2.PropertyType)
                    return false;

                switch (propVal1.PropertyType)
                {
                    case AimPropertyType.AranField:
                        {
                            if (!((AimField)propVal1).Equals((AimField)propVal2))
                                return false;
                            break;
                        }
                    case AimPropertyType.DataType:
                    case AimPropertyType.Object:
                        {
                            if (!AimObject.IsEquals((AimObject)propVal1, (AimObject)propVal2, excludeId))
                                return false;
                            break;
                        }
                    case AimPropertyType.List:
                        {
                            IList list1 = (IList)propVal1;
                            IList list2 = (IList)propVal2;
                            if (list1.Count != list2.Count)
                                return false;
                            for (int j = 0; j < list1.Count; j++)
                            {
                                if (!AimObject.IsEquals((AimObject)list1[j], (AimObject)list2[j], excludeId))
                                    return false;
                            }
                            break;
                        }
                }
            }

            return true;
        }

        ///// <summary>
        ///// It checks differences between 2 same type objects.
        ///// If this and destAimObj types are different then Exception will be throw
        ///// else return difference Property Index List
        ///// </summary>
        ///// <param name="destAimObj"></param>
        ///// <returns></returns>
        public List<int> GetDifferences(IAimObject destAimObj, bool excludeId = false)
        {
            List<int> result = new List<int>();
            if (AimMetadata.GetAimTypeIndex(this) != AimMetadata.GetAimTypeIndex(destAimObj))
                throw new Exception("These are not same type to check differences !");

            int[] propIndSource = ((IAimObject)this).GetPropertyIndexes();
            int[] propIndDest = destAimObj.GetPropertyIndexes();

            //if ( excludeId )
            //{
            //    AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos ( ( IAimObject ) this );
            //    foreach ( var propInfo in propInfos )
            //    {
            //        if ( propInfo.Index == 0 )
            //        {
            //            if ( propInfo.Name.ToLower ( ) == "id" )
            //                excludeId = true;
            //            else
            //                excludeId = false;
            //            break;
            //        }
            //    }
            //}
            DeleteExcludeIndices(excludeId, ref propIndSource, ref propIndDest, (IAimObject)this);

            List<int> sourcePropIndexList = new List<int>(propIndSource);
            List<int> destPropIndexList = new List<int>(propIndDest);

            IAimProperty sourcePropVal;
            IAimProperty destPropVal;

            foreach (int sourcePropIndex in sourcePropIndexList)
            {
                if (!destPropIndexList.Contains(sourcePropIndex))
                {
                    result.Add(sourcePropIndex);
                }
                else
                {
                    sourcePropVal = this.GetValue(sourcePropIndex);
                    destPropVal = destAimObj.GetValue(sourcePropIndex);
                    if (IsDifference(sourcePropVal, destPropVal, excludeId))
                        result.Add(sourcePropIndex);
                }
            }

            foreach (int destPropIndex in destPropIndexList)
            {
                if (!sourcePropIndexList.Contains(destPropIndex))
                {
                    result.Add(destPropIndex);
                }
            }


            return result;
        }

        private static void DeleteExcludeIndices(bool excludeId, ref int[] propIndSource, ref int[] propIndDest, IAimObject aimObj)
        {
            if (excludeId)
            {

                AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos(aimObj);
                int index = -1;
                foreach (var propInfo in propInfos)
                {
                    //					if ( propInfo.Index == 0 )
                    {
                        if (propInfo.Name.ToLower() == "id")
                        {
                            index = propInfo.Index;
                            break;
                        }
                    }
                }

                List<int> sourcePropIndexList = new List<int>(propIndSource);
                List<int> destPropIndexList = new List<int>(propIndDest);

                //if ( index > -1 )
                {
                    sourcePropIndexList.Remove(index);
                    destPropIndexList.Remove(index);
                }

                propIndSource = sourcePropIndexList.ToArray();
                propIndDest = destPropIndexList.ToArray();
            }
        }

        private void OnPropertyChanged(int propertyIndex)
        {
            if (AimPropertyChanged != null)
                AimPropertyChanged(this, new AimPropertyChangedEventArgs(propertyIndex));
        }

        private bool IsDifference(IAimProperty sourceProp, IAimProperty destProp, bool excludeId = false)
        {
            if (sourceProp.PropertyType != destProp.PropertyType)
                return true;

            switch (sourceProp.PropertyType)
            {
                case AimPropertyType.AranField:
                    {
                        if (!((AimField)sourceProp).Equals((AimField)destProp))
                            return true;
                        break;
                    }
                case AimPropertyType.DataType:
                case AimPropertyType.Object:
                    {
                        if (!AimObject.IsEquals((AimObject)sourceProp, (AimObject)destProp, excludeId))
                            return true;
                        break;
                    }
                case AimPropertyType.List:
                    {
                        IList list1 = (IList)sourceProp;
                        IList list2 = (IList)destProp;
                        if (list1.Count != list2.Count)
                            return true;
                        for (int j = 0; j < list1.Count; j++)
                        {
                            if (!AimObject.IsEquals((AimObject)list1[j], (AimObject)list2[j], excludeId))
                                return true;
                        }
                        break;
                    }
            }
            return false;
        }

        private IDictionary<int, IAimProperty> _values;

		// CRC 
		public PointExtension CalculateExtension ( Aran.Geometries.Point pnt, string elev )
		{
			PointExtension extension = new PointExtension ( );
			string crcItems = "Lat/Lon";
			string crcValue = pnt.X + "" + pnt.Y;
			if ( !string.IsNullOrEmpty ( elev ) )
			{
				crcItems += "/Elev Value/Uom";
				crcValue += elev;
			}
			( extension as ICRCExtension ).SetCRCValue ( CalcCRC32 ( crcValue ) );
			( extension as ICRCExtension ).SetCRCItems ( crcItems );
			return extension;
		}

		#region Tables

		private static UInt32[] CRCTable =
		{
			0x00000000, 0x814141AB, 0x83C3C2FD, 0x02828356,
			0x86C6C451, 0x078785FA, 0x050506AC, 0x84444707,
			0x8CCCC909, 0x0D8D88A2, 0x0F0F0BF4, 0x8E4E4A5F,
			0x0A0A0D58, 0x8B4B4CF3, 0x89C9CFA5, 0x08888E0E,
			0x98D8D3B9, 0x19999212, 0x1B1B1144, 0x9A5A50EF,
			0x1E1E17E8, 0x9F5F5643, 0x9DDDD515, 0x1C9C94BE,
			0x14141AB0, 0x95555B1B, 0x97D7D84D, 0x169699E6,
			0x92D2DEE1, 0x13939F4A, 0x11111C1C, 0x90505DB7,
			0xB0F0E6D9, 0x31B1A772, 0x33332424, 0xB272658F,
			0x36362288, 0xB7776323, 0xB5F5E075, 0x34B4A1DE,
			0x3C3C2FD0, 0xBD7D6E7B, 0xBFFFED2D, 0x3EBEAC86,
			0xBAFAEB81, 0x3BBBAA2A, 0x3939297C, 0xB87868D7,
			0x28283560, 0xA96974CB, 0xABEBF79D, 0x2AAAB636,
			0xAEEEF131, 0x2FAFB09A, 0x2D2D33CC, 0xAC6C7267,
			0xA4E4FC69, 0x25A5BDC2, 0x27273E94, 0xA6667F3F,
			0x22223838, 0xA3637993, 0xA1E1FAC5, 0x20A0BB6E,

			0xE0A08C19, 0x61E1CDB2, 0x63634EE4, 0xE2220F4F,
			0x66664848, 0xE72709E3, 0xE5A58AB5, 0x64E4CB1E,
			0x6C6C4510, 0xED2D04BB, 0xEFAF87ED, 0x6EEEC646,
			0xEAAA8141, 0x6BEBC0EA, 0x696943BC, 0xE8280217,
			0x78785FA0, 0xF9391E0B, 0xFBBB9D5D, 0x7AFADCF6,
			0xFEBE9BF1, 0x7FFFDA5A, 0x7D7D590C, 0xFC3C18A7,
			0xF4B496A9, 0x75F5D702, 0x77775454, 0xF63615FF,
			0x727252F8, 0xF3331353, 0xF1B19005, 0x70F0D1AE,
			0x50506AC0, 0xD1112B6B, 0xD393A83D, 0x52D2E996,
			0xD696AE91, 0x57D7EF3A, 0x55556C6C, 0xD4142DC7,
			0xDC9CA3C9, 0x5DDDE262, 0x5F5F6134, 0xDE1E209F,
			0x5A5A6798, 0xDB1B2633, 0xD999A565, 0x58D8E4CE,
			0xC888B979, 0x49C9F8D2, 0x4B4B7B84, 0xCA0A3A2F,
			0x4E4E7D28, 0xCF0F3C83, 0xCD8DBFD5, 0x4CCCFE7E,
			0x44447070, 0xC50531DB, 0xC787B28D, 0x46C6F326,
			0xC282B421, 0x43C3F58A, 0x414176DC, 0xC0003777,

			0x40005999, 0xC1411832, 0xC3C39B64, 0x4282DACF,
			0xC6C69DC8, 0x4787DC63, 0x45055F35, 0xC4441E9E,
			0xCCCC9090, 0x4D8DD13B, 0x4F0F526D, 0xCE4E13C6,
			0x4A0A54C1, 0xCB4B156A, 0xC9C9963C, 0x4888D797,
			0xD8D88A20, 0x5999CB8B, 0x5B1B48DD, 0xDA5A0976,
			0x5E1E4E71, 0xDF5F0FDA, 0xDDDD8C8C, 0x5C9CCD27,
			0x54144329, 0xD5550282, 0xD7D781D4, 0x5696C07F,
			0xD2D28778, 0x5393C6D3, 0x51114585, 0xD050042E,
			0xF0F0BF40, 0x71B1FEEB, 0x73337DBD, 0xF2723C16,
			0x76367B11, 0xF7773ABA, 0xF5F5B9EC, 0x74B4F847,
			0x7C3C7649, 0xFD7D37E2, 0xFFFFB4B4, 0x7EBEF51F,
			0xFAFAB218, 0x7BBBF3B3, 0x793970E5, 0xF878314E,
			0x68286CF9, 0xE9692D52, 0xEBEBAE04, 0x6AAAEFAF,
			0xEEEEA8A8, 0x6FAFE903, 0x6D2D6A55, 0xEC6C2BFE,
			0xE4E4A5F0, 0x65A5E45B, 0x6727670D, 0xE66626A6,
			0x622261A1, 0xE363200A, 0xE1E1A35C, 0x60A0E2F7,

			0xA0A0D580, 0x21E1942B, 0x2363177D, 0xA22256D6,
			0x266611D1, 0xA727507A, 0xA5A5D32C, 0x24E49287,
			0x2C6C1C89, 0xAD2D5D22, 0xAFAFDE74, 0x2EEE9FDF,
			0xAAAAD8D8, 0x2BEB9973, 0x29691A25, 0xA8285B8E,
			0x38780639, 0xB9394792, 0xBBBBC4C4, 0x3AFA856F,
			0xBEBEC268, 0x3FFF83C3, 0x3D7D0095, 0xBC3C413E,
			0xB4B4CF30, 0x35F58E9B, 0x37770DCD, 0xB6364C66,
			0x32720B61, 0xB3334ACA, 0xB1B1C99C, 0x30F08837,
			0x10503359, 0x911172F2, 0x9393F1A4, 0x12D2B00F,
			0x9696F708, 0x17D7B6A3, 0x155535F5, 0x9414745E,
			0x9C9CFA50, 0x1DDDBBFB, 0x1F5F38AD, 0x9E1E7906,
			0x1A5A3E01, 0x9B1B7FAA, 0x9999FCFC, 0x18D8BD57,
			0x8888E0E0, 0x09C9A14B, 0x0B4B221D, 0x8A0A63B6,
			0x0E4E24B1, 0x8F0F651A, 0x8D8DE64C, 0x0CCCA7E7,
			0x044429E9, 0x85056842, 0x8787EB14, 0x06C6AABF,
			0x8282EDB8, 0x03C3AC13, 0x01412F45, 0x80006EEE
		};

		private static char[] CharTab = {   '0', '1', '2', '3', '4', '5', '6', '7',
											'8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

		#endregion

		private string CalcCRC32 ( string str )
		{
			int i, l = str.Length;

			// Calc CRC value
			UInt32 CRCValue = 0;

			for ( i = 0; i < l; i++ )
			{
				byte b = ( byte ) str[ i ];
				CRCValue = ( CRCValue << 8 ) ^ CRCTable[ ( ( CRCValue >> 24 ) ^ b ) & 0xFF ];
			}

			// Convert CRC value to string
			string res = "";

			for ( i = 0; i < 8; i++ )
			{
				res = CharTab[ CRCValue & 0xF ] + res;
				CRCValue >>= 4;
			}
			return res;
		}
		//

	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataAimObject
    {
        public static AimPropInfoList PropInfoList;

        static MetadataAimObject ()
        {
            PropInfoList = new AimPropInfoList ();
        }
    }
}
