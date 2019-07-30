using System;
using System.Xml;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;
using System.Collections.Generic;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Features
{
	public interface IEditFeature
	{
		NilReason? GetNilReasonProp (int propIndex);
		void SetNilReasonProp (int propIndex, NilReason? nr);
	}

    public abstract class Feature : DBEntity, IEditFeature
    {
		public Feature ()
		{
			_nillReasonProps = new Dictionary<int, NilReason> ();
		}

        protected override AimObjectType AimObjectType
        {
            get { return AimObjectType.Feature; }
        }

        public abstract FeatureType FeatureType { get; }

        public Guid Identifier
        {
            get { return GetFieldValue<Guid> ((int) PropertyFeature.Identifier); }
            set { SetFieldValue<Guid> ((int) PropertyFeature.Identifier, value); }
        }

        public TimeSlice TimeSlice
        {
            get { return (TimeSlice)GetValue((int)PropertyFeature.TimeSlice); }
            set { SetValue((int)PropertyFeature.TimeSlice, value); }
        }

        public MdMetadata Metadata
        {
            get { return GetObject<MdMetadata>((int) PropertyFeature.Metadata); }
            set { SetValue((int) PropertyFeature.Metadata, value); }
        }

        public Metadata.FeatureTimeSliceMetadata TimeSliceMetadata { get; set; }

        public List<string> Extension
        {
            get
            {
                if (_extension == null)
                    _extension = new List<string> ();

                return _extension;
            }
        }

		public long WorksPackageId { get; set; }


        protected override bool AixmDeserialize (XmlContext context)
        {
            XmlElement xmlElement = context.Element;
            int elementIndex = context.ElementIndex.Start;
            if (elementIndex >= xmlElement.ChildNodes.Count)
                return false;
            int enumIndex = context.PropertyIndex.Start;
            
            IAimProperty tmpPropValue;

            bool isDeserialized;

            XmlElement childElement = null;
            XmlContext tmpContext = new XmlContext ();
            tmpContext.ElementIndex.End = -1;
            tmpContext.PropertyIndex.End = -1;
            
            this.TimeSlice = new TimeSlice ();

            #region [gml:validTime]

            childElement = CommonXmlFunctions.GetChildElement (xmlElement, ref elementIndex);
            if (childElement == null)
                return false;

            if (childElement.LocalName == "validTime")
            {
                childElement = CommonXmlFunctions.GetChildElement (childElement);

                if (childElement != null)
                {
                    tmpContext.Element = childElement;
                    tmpPropValue = new TimePeriod ();
                    isDeserialized = tmpPropValue.GetAixmSerializable ().AixmDeserialize (tmpContext);
                    if (isDeserialized)
                        this.TimeSlice.ValidTime = (TimePeriod) tmpPropValue;
                }

                elementIndex++;
                if (elementIndex >= xmlElement.ChildNodes.Count)
                    return false;

                childElement = CommonXmlFunctions.GetChildElement (xmlElement, ref elementIndex);
                if (childElement == null)
                    return false;
            }

            #endregion

            #region [aixm-5.1:interpretation]

            if (childElement.LocalName == "interpretation")
            {
                tmpPropValue = new AimField<TimeSliceInterpretationType> ();
                tmpContext.Element = childElement;
                isDeserialized = tmpPropValue.GetAixmSerializable ().AixmDeserialize (tmpContext);
                if (isDeserialized)
                    (this.TimeSlice as IAimObject).SetValue ((int) PropertyTimeSlice.Interpretation, tmpPropValue);

                elementIndex++;
                if (elementIndex >= xmlElement.ChildNodes.Count)
                    return false;

                childElement = CommonXmlFunctions.GetChildElement (xmlElement, ref elementIndex);
                if (childElement == null)
                    return false;
            }

            #endregion

            #region [aixm-5.1:sequenceNumber]

            if (childElement.LocalName == "sequenceNumber")
            {
                tmpPropValue = new AimField<Int32> ();
                tmpContext.Element = childElement;
                isDeserialized = tmpPropValue.GetAixmSerializable ().AixmDeserialize (tmpContext);
                if (isDeserialized)
                    (this.TimeSlice as IAimObject).SetValue ((int) PropertyTimeSlice.SequenceNumber, tmpPropValue);

                elementIndex++;
                if (elementIndex >= xmlElement.ChildNodes.Count)
                    return false;

                childElement = CommonXmlFunctions.GetChildElement (xmlElement, ref elementIndex);
                if (childElement == null)
                    return false;
            }

            #endregion

            #region [aixm-5.1:correctionNumber]

            if (childElement.LocalName == "correctionNumber")
            {
                tmpPropValue = new AimField<Int32> ();
                tmpContext.Element = childElement;
                isDeserialized = tmpPropValue.GetAixmSerializable ().AixmDeserialize (tmpContext);
                if (isDeserialized)
                    (this.TimeSlice as IAimObject).SetValue ((int) PropertyTimeSlice.CorrectionNumber, tmpPropValue);

                elementIndex++;
                if (elementIndex >= xmlElement.ChildNodes.Count)
                    return false;

                childElement = CommonXmlFunctions.GetChildElement (xmlElement, ref elementIndex);
                if (childElement == null)
                    return false;
            }

            #endregion

            #region [timeSliceMetadata]

            if (childElement.LocalName == "timeSliceMetadata")
            {
                TimeSliceMetadata = new Metadata.FeatureTimeSliceMetadata ();
                TimeSliceMetadata.XmlText = childElement.InnerXml;

                elementIndex++;
                if (elementIndex >= xmlElement.ChildNodes.Count)
                    return false;

                childElement = CommonXmlFunctions.GetChildElement (xmlElement, ref elementIndex);
                if (childElement == null)
                    return false;
            }

            #endregion

            #region [aixm-5.1:featureLifetime]

            if (childElement.LocalName == "featureLifetime")
            {
                childElement = CommonXmlFunctions.GetChildElement (childElement);

                if (childElement != null)
                {
                    tmpPropValue = new TimePeriod ();
                    tmpContext.Element = childElement;
                    isDeserialized = tmpPropValue.GetAixmSerializable ().AixmDeserialize (tmpContext);
                    if (isDeserialized)
                        this.TimeSlice.FeatureLifetime = (TimePeriod) tmpPropValue;
                }

                elementIndex++;
                if (elementIndex > xmlElement.ChildNodes.Count)
                    return false;
            }

            #endregion

            enumIndex += 2;

            tmpContext.Element = xmlElement;
            tmpContext.ElementIndex.Start = elementIndex;
            tmpContext.PropertyIndex.Start = enumIndex;

            try
            {
                return base.AixmDeserialize(tmpContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Assign (AranObject source)
        {
            base.Assign (source);
			
			var src = source as Feature;

			var tsmd = src.TimeSliceMetadata;
            if (tsmd != null)
            {
				TimeSliceMetadata = tsmd.Clone () as Metadata.FeatureTimeSliceMetadata;
				//    new Metadata.FeatureTimeSliceMetadata ();
				//TimeSliceMetadata.XmlText = ((Feature) source).TimeSliceMetadata.XmlText;
            }

			_nillReasonProps.Clear ();
			foreach (var pair in src._nillReasonProps)
				_nillReasonProps.Add (pair.Key, pair.Value);
        }

		protected override void Pack (Aran.Package.PackageWriter writer)
		{
			writer.PutInt32 (_nillReasonProps.Count);
			foreach (var pair in _nillReasonProps)
			{
				writer.PutInt32 (pair.Key);
				writer.PutEnum<NilReason> (pair.Value);
			}

			writer.PutInt64 (WorksPackageId);

			base.Pack (writer);
		}

		protected override void Unpack (Aran.Package.PackageReader reader)
		{
			_nillReasonProps.Clear ();
			var count = reader.GetInt32 ();
			for (int i = 0; i < count; i++)
			{
				var propIndex = reader.GetInt32 ();
				var nr = reader.GetEnum<NilReason> ();
				_nillReasonProps.Add (propIndex, nr);
			}

			WorksPackageId = reader.GetInt64 ();

			base.Unpack (reader);
		}

		public NilReason? GetNilReason<T> (T propItem) where T : struct
		{
			var propIndex = (int) (object) propItem;
			return (this as IEditFeature).GetNilReasonProp (propIndex);
		}

		/// <summary>
		/// Set NilReasion null to remove.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propItem"></param>
		/// <param name="nr"></param>
		public void SetNilReason<T> (T propItem, NilReason? nr) where T : struct
		{
			//--- Check type of T to proper this PropEnum type of this object
			//... implement it.

			var propIndex = (int) (object) propItem;
			(this as IEditFeature).SetNilReasonProp (propIndex, nr);
		}

		NilReason? IEditFeature.GetNilReasonProp (int propIndex)
		{
			if (_nillReasonProps.ContainsKey (propIndex))
				return (NilReason) _nillReasonProps [propIndex];
			return null;
		}

		void IEditFeature.SetNilReasonProp (int propIndex, NilReason? nr)
		{
			if (nr == null)
			{
				if (_nillReasonProps.ContainsKey (propIndex))
					_nillReasonProps.Remove (propIndex);
				return;
			}

			if (_nillReasonProps.ContainsKey (propIndex))
				_nillReasonProps [propIndex] = nr.Value;
			else
				_nillReasonProps.Add (propIndex, nr.Value);
		}

		private List<string> _extension;
		private Dictionary<int, NilReason> _nillReasonProps;
	}
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyFeature
    {
        Identifier = PropertyDBEntity.NEXT_CLASS,
        TimeSlice,
        NEXT_CLASS,
        Metadata = 10000
    }

    public static class MetadataFeature
    {
        public static AimPropInfoList PropInfoList;

        static MetadataFeature ()
        {
            PropInfoList = MetadataDBEntity.PropInfoList.Clone ();
            PropInfoList.Add (PropertyFeature.Identifier, (int) AimFieldType.SysGuid);
            PropInfoList.Add (PropertyFeature.TimeSlice, (int) DataType.TimeSlice);
        }
    }
}