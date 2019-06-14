using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using System.Collections;
using System.ComponentModel;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.FeatureInfo
{
    internal abstract class BindingInfo
    {
        public BindingInfo (AimPropInfo propInfo, IAimProperty aimProperty)
        {
            _propInfo = propInfo;
            _aimProperty = aimProperty;
            _name = _propInfo.UiPropInfo().Caption;

            if (string.IsNullOrWhiteSpace (_name))
                _name = Global.MakeSentence(_propInfo.Name);

            if (_name == "Geo")
                _name = "Geo (WGS84)";

            SetValue ();
        }

        public abstract BindingInfoType InfoType { get; }

        public string Name
        {
            get { return _name; }
        }
        
        public bool IsNameLonger
        {
            get { return false; }
        }

        public object Value
        {
            get { return _propValue; }
        }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
        }

        public IAimProperty AimProperty
        {
            get { return _aimProperty; }
        }

        
		protected abstract void SetValue ();

        public static BindingInfo CreateInfo (AimPropInfo propInfo, IAimProperty aimProperty)
        {
            AimClassInfo prClassInfo = propInfo.PropType;

            if (prClassInfo.AimObjectType == AimObjectType.Field)
                return new PrimitiveInfo (propInfo, aimProperty);

            if (propInfo.IsFeatureReference || AimMetadata.IsAbstractFeatureRefObject (prClassInfo.Index))
                return new ReferenceInfo (propInfo, aimProperty);

            if (prClassInfo.AimObjectType == AimObjectType.DataType)
            {
                if (prClassInfo.SubClassType == AimSubClassType.ValClass)
                {
                    return new ValClassInfo (propInfo, aimProperty);
                }
                else if (prClassInfo.Index == (int) DataType.TextNote ||
                   prClassInfo.Index == (int) DataType.TimeSlice)
                {
                    return new ComplexInfo (propInfo, aimProperty);
                }
                else if (prClassInfo.Index == (int) DataType.TimePeriod)
                {
                    return new PrimitiveInfo (propInfo, aimProperty);
                }
            }
            else if (prClassInfo.AimObjectType == AimObjectType.Object)
            {
                if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
                {
                    if (aimProperty.PropertyType == AimPropertyType.List)
                        throw new Exception ("Choice List");

                    IEditChoiceClass editChoice = aimProperty as IEditChoiceClass;
                    AimPropInfo choicePropInfo = Aran.Aim.Utilities.AimMetadataUtility.GetChoicePropInfo (editChoice);

                    BindingInfo bi = null;

                    if (choicePropInfo.IsFeatureReference)
                        bi = new ReferenceInfo (choicePropInfo, editChoice.RefValue);
                    else
                        bi = new ComplexInfo (choicePropInfo, editChoice.RefValue);

                    bi._name = Global.MakeSentence (propInfo.Name + " -> " + choicePropInfo.Name);

                    return bi;
                }
                else
                {
                    return new ComplexInfo (propInfo, aimProperty);
                }
            }

            return null;
        }

        
		protected AimPropInfo _propInfo;
        protected IAimProperty _aimProperty;
        protected object _propValue;
        protected string _name;
    }

    internal class PrimitiveInfo : BindingInfo
    {
        public PrimitiveInfo (AimPropInfo propInfo, IAimProperty aimProperty)
            : base (propInfo, aimProperty)
        {
        }

        public override BindingInfoType InfoType
        {
            get { return BindingInfoType.Primitive; }
        }

        protected override void SetValue ()
        {
            IsPoint = false;
            IsSimpleField = false;
            PolyModel = null;

            if (_aimProperty is IEditAimField)
            {
                IEditAimField editAimField = _aimProperty as IEditAimField;
                _propValue = editAimField.FieldValue;
                IsSimpleField = true;

                if (_propValue is Aran.Geometries.Point)
                {
                    IsPoint = true;
                    IsSimpleField = false;
                    Geo = (_propValue as Aran.Geometries.Point);
                }
                else if (_propValue is MultiLineString)
                {
                    PolyModel = new MultiPolyModel(_propValue as Geometry);
                    IsSimpleField = false;
                    var mls = _propValue as MultiLineString;
                    var mp = mls.ToMultiPoint ();
                    _propValue = string.Format ("Curve (Point count: {0})", mp.Count);
                }
                else if (_propValue is MultiPolygon)
                {
                    PolyModel = new MultiPolyModel(_propValue as Geometry);
                    IsSimpleField = false;
                    var mplg = _propValue as MultiPolygon;
                    var mp = mplg.ToMultiPoint ();
                    _propValue = string.Format ("Surface (Point count: {0})", mp.Count);
                }
                else if (_propValue is bool)
                {
                    _propValue = ((bool) _propValue ? "YES" : "NO");
                }
                else if (_propValue is DateTime)
                {
                    _propValue = Global.DateTimeToString ((DateTime) _propValue);
                }
                else if (_propValue is double)
                {
                    _propValue = ((double) _propValue).ToString ("#.####");
                }
            }
            else if (_aimProperty is TimePeriod)
            {
                TimePeriod tp = _aimProperty as TimePeriod;
                _propValue = string.Format ("From: {0}\nTo: {1}",
                    Global.DateTimeToString (tp.BeginPosition),
                    (tp.EndPosition == null ? string.Empty : Global.DateTimeToString (tp.EndPosition.Value)));
            }
        }

        public bool IsSimpleField { get; private set; }

        public bool IsPoint { get; private set; }

        public bool IsPoly { get { return (PolyModel != null); } }

        public MultiPolyModel PolyModel { get; private set; }

        public Aran.Geometries.Point Geo { get; private set; }
    }

    internal class ValClassInfo : BindingInfo
    {
        public ValClassInfo (AimPropInfo propInfo, IAimProperty aimProperty)
            : base (propInfo, aimProperty)
        {
        }

        public override BindingInfoType InfoType
        {
            get { return BindingInfoType.ValClass; }
        }

        protected override void SetValue ()
        {
            IEditValClass editVC = _aimProperty as IEditValClass;
            _propValue = editVC.Value;

            AimClassInfo valClassInfo = _propInfo.PropType;
            AimPropInfo uomPropInfo = valClassInfo.Properties ["Uom"];

            Uom = AimMetadata.GetEnumValueAsString (editVC.Uom, uomPropInfo.TypeIndex);
        }

        public string Uom { get; private set; }
    }

    internal class ComplexInfo : BindingInfo
    {
        public ComplexInfo (AimPropInfo propInfo, IAimProperty aimProperty)
            : base (propInfo, aimProperty)
        {
        }

        public override BindingInfoType InfoType
        {
            get { return BindingInfoType.Complex; }
        }

        public bool IsList { get; private set; }

        public int Count { get; private set; }

        public bool IsNote { get; private set; }

        public string NoteText { get; private set; }

        protected override void SetValue ()
        {
            IsList = false;
            Count = 0;
            IsNote = false;

            _propValue = _propInfo.PropType.UiInfo().Caption;

            if (_aimProperty.PropertyType == AimPropertyType.List)
            {
                IList list = _aimProperty as IList;

                if (_propInfo.PropType.Index == (int) ObjectType.LinguisticNote)
                {
                    #region Note

                    IsNote = true;
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < list.Count; i++)
                    {
                        LinguisticNote ln = list [i] as LinguisticNote;
                        TextNote tn = ln.Note;

                        string numberText = "";
                        if (list.Count > 1)
                            numberText = (i + 1) + ".\n";

                        if (tn == null)
                            sb.AppendLine (numberText + "<NULL>");
                        else
                            sb.AppendLine (string.Format ("{0}Language: {1}\n{2}\n", numberText, tn.Lang, tn.Value));
                    }
                    NoteText = sb.ToString ();

                    #endregion
                }
                else
                {
                    IsList = true;
                    Count = list.Count;
                }
            }
        }
    }

    internal class ReferenceInfo : BindingInfo
    {
        public ReferenceInfo (AimPropInfo propInfo, IAimProperty aimProperty)
            : base (propInfo, aimProperty)
        {
        }

        public override BindingInfoType InfoType
        {
            get { return BindingInfoType.Reference; }
        }

        protected override void SetValue ()
        {
            IsList = (_aimProperty is IList);
            RefList = new List<RefItem> ();
			IsMoreButtonVisible = false;

            if (_propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef ||
                AimMetadata.IsAbstractFeatureRefObject (_propInfo.TypeIndex))
            {
                if (_propInfo.IsList)
                {
                    IList list = _aimProperty as IList;
                    foreach (IAimObject absFeatRefObject in list)
                    {
                        IAimProperty featRefValue = absFeatRefObject.GetValue (
                            (int) Aran.Aim.PropertyEnum.PropertyAbstractFeatureRefObject.Feature);

                        if (featRefValue != null)
                        {
                            IAbstractFeatureRef absFeatRef = featRefValue as IAbstractFeatureRef;
                            AddToRefList ( (FeatureType)absFeatRef.FeatureTypeIndex, absFeatRef.Identifier);
                        }
                    }
                }
                else
                {
                    IAbstractFeatureRef absFeatRef = _aimProperty as IAbstractFeatureRef;
                    AddToRefList ((FeatureType) absFeatRef.FeatureTypeIndex, absFeatRef.Identifier);
                }
            }
            else
            {
                if (_aimProperty is FeatureRef)
                {
                    AddToRefList (_propInfo.ReferenceFeature, (_aimProperty as FeatureRef).Identifier);
                }
                else if (_aimProperty.PropertyType == AimPropertyType.List)
                {
                    IList list = _aimProperty as IList;
					IsMoreButtonVisible = list.Count > 10;
					MoreFeatureCount = list.Count - 5;

					var n = 0;
					foreach (FeatureRefObject fro in list)
					{
						if (IsMoreButtonVisible && n > 4)
							break;
						AddToRefList (_propInfo.ReferenceFeature, fro.Feature.Identifier);
						n++;
					}
                }
            }

			if (!IsMoreButtonVisible)
				RefList [RefList.Count - 1].IsLast = true;
        }

        private void AddToRefList (FeatureType featureType, Guid identifier)
        {
            RefList.Add (new RefItem ()
            {
                Index = RefList.Count + 1,
                FeatureType = featureType,
                Identifier = identifier,
                IsLast = false
            });
        }

        public bool IsList { get; private set; }

        public List<RefItem> RefList { get; private set; }

		public bool IsMoreButtonVisible { get; private set; }

		public int MoreFeatureCount { get; private set; }
    }

    internal class RefItem
    {
        public int Index { get; set; }
        public FeatureType FeatureType { get; set; }
        public Guid Identifier { get; set; }
        public bool IsLast { get; set; }
    }

    internal enum BindingInfoType { Primitive, ValClass, Complex, Reference }
}
