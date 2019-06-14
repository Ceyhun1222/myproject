using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;
using Aran.Aim.Metadata;

namespace Aran.Aim.AixmMessage
{
    class AranAixmWriter
    {
        static AranAixmWriter ()
        {
            _gmlIdCounter = 0;
        }

        public AranAixmWriter(XmlWriter xmlWriter, NamespaceInfo caw, NamespaceInfo cae, bool write3DIfExists, SrsNameType srsType)
        {
            Writer = xmlWriter;
            SerializeExtension = true;
            CAW = caw;
            CAE = cae;
            Write3DIfExists = write3DIfExists;
            SrsType = srsType;
        }

        public static string GenerateNewGmlId ()
        {
            _gmlIdCounter++;
            return "gmlAranID" + _gmlIdCounter;
        }

        public NamespaceInfo CAW { get; private set; }

        public NamespaceInfo CAE { get; private set; }

        public bool Write3DIfExists { get; private set; }

        public SrsNameType SrsType { get; private set; }

        public XmlWriter Writer { get; set; }

        public bool SerializeExtension { get; set; }

        public int WriteAranObject2(IAimProperty aranProperty, int propStartIndex, bool isDateTime = false)
        {
            switch (aranProperty.PropertyType)
            {
                case AimPropertyType.AranField:
                    propStartIndex = -1;
                    WriteAranField2((AimField)aranProperty, isDateTime);
                    break;
                case AimPropertyType.DataType:
                    propStartIndex = WriteDataType((ADataType)aranProperty, propStartIndex);
                    break;
                case AimPropertyType.Object:
                    propStartIndex = WriteObject((AObject)aranProperty, propStartIndex);
                    break;
            }

            if (propStartIndex == -1)
                return propStartIndex;

            WriteProperties((IAimObject)aranProperty, propStartIndex);
            return -1;
        }

        public void WriteFeatureTimeSlice (Feature feature)
        {
            string featureName = feature.FeatureType.ToString ();

            Writer.WriteStartElement (AimDbNamespaces.AIXM51, "timeSlice");
            {
                Writer.WriteStartElement (AimDbNamespaces.AIXM51, featureName + "TimeSlice");
                {
                    Writer.WriteAttributeString (AimDbNamespaces.GML, "id", GenerateNewGmlId ());

                    if (feature.TimeSlice != null)
                    {
                        if (feature.TimeSlice.ValidTime != null)
                        {
                            Writer.WriteStartElement (AimDbNamespaces.GML, "validTime");
                            WriteTimePeriod (feature.TimeSlice.ValidTime);
                            Writer.WriteEndElement ();
                        }

                        Writer.WriteStartElement (AimDbNamespaces.AIXM51, "interpretation");
                        PropertySerialize (feature.TimeSlice, (int) PropertyTimeSlice.Interpretation, Writer, 0);
                        Writer.WriteEndElement ();

                        Writer.WriteStartElement (AimDbNamespaces.AIXM51, "sequenceNumber");
                        PropertySerialize (feature.TimeSlice, (int) PropertyTimeSlice.SequenceNumber, Writer, 0);
                        Writer.WriteEndElement ();

                        if (feature.TimeSlice.CorrectionNumber >= 0)
                        {
                            Writer.WriteStartElement (AimDbNamespaces.AIXM51, "correctionNumber");
                            PropertySerialize (feature.TimeSlice, (int) PropertyTimeSlice.CorrectionNumber, Writer, 0);
                            Writer.WriteEndElement ();
                        }

                        if (feature.TimeSliceMetadata != null)
                        {
                            Writer.WriteStartElement (AimDbNamespaces.AIXM51, "timeSliceMetadata");
                            WriteTimeSliceMetadata (feature.TimeSliceMetadata, Writer);
                            Writer.WriteEndElement ();
                        }

                        if (feature.TimeSlice.FeatureLifetime != null)
                        {
                            Writer.WriteStartElement (AimDbNamespaces.AIXM51, "featureLifetime");
                            WriteTimePeriod (feature.TimeSlice.FeatureLifetime);
                            Writer.WriteEndElement ();
                        }
                    }

                    WriteProperties (feature, (int) PropertyFeature.TimeSlice /* + 1*/);

                    //WriteExtension (feature);
                }
                Writer.WriteEndElement ();
            }

            Writer.WriteEndElement ();
        }

        /// <summary>
        /// Write XML for AIP DataSet.
        /// Require for custom xml writing.
        /// </summary>
        /// <param name="feature"></param>
        public void WriteDataSetFeatureTimeSlice(Feature feature, Dictionary<FeatureType, List<string>> IgnoredProperties = null)
        {
            string featureName = feature.FeatureType.ToString();

            Writer.WriteStartElement(AimDbNamespaces.AIXM51, "timeSlice");
            {
                Writer.WriteStartElement(AimDbNamespaces.AIXM51, featureName + "TimeSlice");
                {
                    Writer.WriteAttributeString(AimDbNamespaces.GML, "id", GenerateNewGmlId());

                    if (feature.TimeSlice != null)
                    {
                        if (feature.TimeSlice.ValidTime != null)
                        {
                            Writer.WriteStartElement(AimDbNamespaces.GML, "validTime");
                            WriteTimePeriod(feature.TimeSlice.ValidTime);
                            Writer.WriteEndElement();
                        }

                        Writer.WriteStartElement(AimDbNamespaces.AIXM51, "interpretation");
                        PropertySerialize(feature.TimeSlice, (int)PropertyTimeSlice.Interpretation, Writer, 0);
                        Writer.WriteEndElement();

                        Writer.WriteStartElement(AimDbNamespaces.AIXM51, "sequenceNumber");
                        PropertySerialize(feature.TimeSlice, (int)PropertyTimeSlice.SequenceNumber, Writer, 0);
                        Writer.WriteEndElement();

                        if (feature.TimeSlice.CorrectionNumber >= 0)
                        {
                            Writer.WriteStartElement(AimDbNamespaces.AIXM51, "correctionNumber");
                            PropertySerialize(feature.TimeSlice, (int)PropertyTimeSlice.CorrectionNumber, Writer, 0);
                            Writer.WriteEndElement();
                        }

                        if (feature.TimeSliceMetadata != null)
                        {
                            Writer.WriteStartElement(AimDbNamespaces.AIXM51, "timeSliceMetadata");
                            WriteTimeSliceMetadata(feature.TimeSliceMetadata, Writer);
                            Writer.WriteEndElement();
                        }

                        if (feature.TimeSlice.FeatureLifetime != null)
                        {
                            Writer.WriteStartElement(AimDbNamespaces.AIXM51, "featureLifetime");
                            WriteTimePeriod(feature.TimeSlice.FeatureLifetime);
                            Writer.WriteEndElement();
                        }
                    }

                    WriteDataSetProperties(feature, (int)PropertyFeature.TimeSlice, null, IgnoredProperties);

                    //WriteExtension (feature);
                }
                Writer.WriteEndElement();
            }

            Writer.WriteEndElement();
        }

        private void WriteExtension (Feature feature)
        {
            bool isNew = (feature.Id == -1);

            Writer.WriteStartElement (AimDbNamespaces.AIXM51, "extension");
            {
                Writer.WriteStartElement (CAE, feature.FeatureType + "Extension");
                {
                    Writer.WriteAttributeString (AimDbNamespaces.GML, "id", GenerateNewGmlId ());
                    Writer.WriteElementString (CAE, "aipClassifier", "AMDT");
                    Writer.WriteElementString (CAE, "featureCode", feature.FeatureType + " " + feature.Identifier);

                    if (isNew)
                    {
                        Writer.WriteStartElement (CAE, "authority");
                        Writer.WriteAttributeString ("role", "AIP-PUBLICATION");
                        Writer.WriteString ("Estonia");
                        Writer.WriteEndElement ();

                        Writer.WriteStartElement (CAE, "authority");
                        Writer.WriteAttributeString ("role", "STRUCTURAL");
                        Writer.WriteString ("Estonia");
                        Writer.WriteEndElement ();
                    }
                }
                Writer.WriteEndElement ();
            }
            Writer.WriteEndElement ();
        }

        private void WriteProperties (IAimObject aranObject, int propStartIndex, NamespaceInfo nsInfo = null)
        {
            if (nsInfo == null)
                nsInfo = AimDbNamespaces.AIXM51;

            AimPropInfo [] propInfoArr = AimMetadata.GetAimPropInfos (aranObject);

            for (int i = propStartIndex; i < propInfoArr.Length; i++)
            {
                AimPropInfo propInfo = propInfoArr [i];

                if (propInfo.Name == "Metadata" && aranObject is Feature)
                    continue;

                IAimProperty propValue = aranObject.GetValue (propInfo.Index);

                if (propValue == null)
                {
                    if (propInfo.PropType.SubClassType != AimSubClassType.Choice)
                        WriteNullProperty(nsInfo, aranObject, propInfo);
                }
                else
                {
                    #region Detect DateTimeProperty 
                    ///*** Temporary   detect dateTime property. For the present I dont change core PropInfo info.
                    bool isPropertyDateTime = false;
                    if (aranObject is SurfaceContamination)
                    {
                        if (propInfo.AixmName == "observationTime" || propInfo.AixmName == "nextObservationTime")
                            isPropertyDateTime = true;
                    }
                    #endregion

                    if (propValue.PropertyType == AimPropertyType.AranField) {
                        var editAimField = propValue as IEditAimField;
                        if (editAimField.FieldValue is string && string.IsNullOrWhiteSpace(editAimField.FieldValue.ToString()))
                            continue;
                    }

                    if (propValue.PropertyType == AimPropertyType.List)
                    {
                        IList list = (IList) propValue;
						if (propInfo.IsFeatureReference)
                        {
	                        foreach (FeatureRefObject fro in list)
                            {
                                Writer.WriteStartElement (nsInfo, propInfo.AixmName);
                                WriteFeatureRef (fro.Feature, propInfo.ReferenceFeature);
                                Writer.WriteEndElement ();
                            }
                        }
                        else
                        {
                            foreach (AObject aObject in list)
                            {
								if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
								{
									WriteChoiceClass ( ( IEditChoiceClass ) aObject, propInfo.AixmName );
								}
								else if ( AimMetadata.IsAbstractFeatureRefObject ( propInfo.TypeIndex ))
	                            {
									Writer.WriteStartElement ( nsInfo, propInfo.AixmName );
		                            dynamic dynamicObj = aObject;
		                            var featId = dynamicObj.Feature.Identifier;
		                            Writer.WriteAttributeString(AimDbNamespaces.XLINK, "href",
			                            "urn:uuid:" + CommonXmlWriter.GetString((Guid) featId));
									Writer.WriteEndElement ( );
								}
								else 
                                {
                                    var aObjectItem = aObject;

                                    if (aObject.ObjectType == ObjectType.Note) { 
                                        var note = aObject.Clone() as Note;
                                        if (IsNoteEmpty(note))
                                            continue;
                                        else
                                            aObjectItem = note;
                                    }

                                    Writer.WriteStartElement (nsInfo, propInfo.AixmName);
                                    WriteAranObject2(aObjectItem, 0);
                                    Writer.WriteEndElement ();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (propInfo.AixmName != string.Empty)
                        {
                            if (AimMetadata.IsChoice(propInfo.TypeIndex))
                            {
                                WriteChoiceClass((IEditChoiceClass) propValue, propInfo.AixmName);
                            }
                            else
                            {
                                if (propInfo.IsExtension && !SerializeExtension)
                                    continue;

                                Writer.WriteStartElement(nsInfo, propInfo.AixmName);
                                if (propInfo.IsFeatureReference && propInfo.ReferenceFeature != 0)
                                {
                                    WriteFeatureRef((FeatureRef) propValue, propInfo.ReferenceFeature);
                                }
                                else
                                {
                                    if (aranObject is RulesProcedures && propInfo.AixmName == "content")
                                        WriteXHtml((propValue as IEditAimField).FieldValue as string);
                                    else
                                        WriteAranObject2(propValue, 0, isPropertyDateTime);
                                }
                                Writer.WriteEndElement();
                            }
                        }
                        else
                        {
                            if (propInfo.Name != "Id")
                                WriteAranObject2(propValue, 0, isPropertyDateTime);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write XML for AIP DataSet.
        /// Require for custom xml writing.
        /// </summary>
        /// <param name="aranObject"></param>
        /// <param name="propStartIndex"></param>
        /// <param name="nsInfo"></param>
        private void WriteDataSetProperties(IAimObject aranObject, int propStartIndex, NamespaceInfo nsInfo = null, Dictionary<FeatureType, List<string>> IgnoredProperties = null)
        {
            if (nsInfo == null)
                nsInfo = AimDbNamespaces.AIXM51;

            AimPropInfo[] propInfoArr = AimMetadata.GetAimPropInfos(aranObject);

            for (int i = propStartIndex; i < propInfoArr.Length; i++)
            {
                AimPropInfo propInfo = propInfoArr[i];
                IAimProperty propValue = aranObject.GetValue(propInfo.Index);
                
                //
                
                if (propValue == null)
                {
                    if (propInfo.PropType.SubClassType != AimSubClassType.Choice)
                        WriteNullProperty(nsInfo, aranObject, propInfo);
                }
                else if (IgnoredProperties != null && aranObject.AimObjectType == AimObjectType.Feature &&
                         IgnoredProperties.ContainsKey(((Feature)aranObject).FeatureType))
                {
                    if (IgnoredProperties[((Feature)aranObject).FeatureType].Contains(propInfo.Name))
                    {
                        WriteNullProperty(nsInfo, aranObject, propInfo);
                    }
                }
                else
                {
                    #region Detect DateTimeProperty 
                    ///*** Temporary   detect dateTime property. For the present I dont change core PropInfo info.
                    bool isPropertyDateTime = false;
                    if (aranObject is SurfaceContamination)
                    {
                        if (propInfo.AixmName == "observationTime" || propInfo.AixmName == "nextObservationTime")
                            isPropertyDateTime = true;
                    }
                    #endregion

                    if (propValue.PropertyType == AimPropertyType.AranField)
                    {
                        var editAimField = propValue as IEditAimField;
                        if (editAimField.FieldValue is string && string.IsNullOrWhiteSpace(editAimField.FieldValue.ToString()))
                            continue;
                    }

                    if (propValue.PropertyType == AimPropertyType.List)
                    {
                        IList list = (IList)propValue;
                        if (propInfo.IsFeatureReference)
                        {
                            foreach (FeatureRefObject fro in list)
                            {
                                Writer.WriteStartElement(nsInfo, propInfo.AixmName);
                                WriteFeatureRef(fro.Feature, propInfo.ReferenceFeature);
                                Writer.WriteEndElement();
                            }
                        }
                        else
                        {
                            foreach (AObject aObject in list)
                            {
                                if (AimMetadata.IsChoice(propInfo.TypeIndex))
                                {
                                    WriteChoiceClass((IEditChoiceClass)aObject, propInfo.AixmName);
                                }
                                else if (AimMetadata.IsAbstractFeatureRefObject(propInfo.TypeIndex))
                                {
                                    Writer.WriteStartElement(nsInfo, propInfo.AixmName);
                                    dynamic dynamicObj = aObject;
                                    var featId = dynamicObj.Feature.Identifier;
                                    Writer.WriteAttributeString(AimDbNamespaces.XLINK, "href",
                                        "urn:uuid:" + CommonXmlWriter.GetString((Guid)featId));
                                    Writer.WriteEndElement();
                                }
                                else
                                {
                                    var aObjectItem = aObject;

                                    if (aObject.ObjectType == ObjectType.Note)
                                    {
                                        var note = aObject.Clone() as Note;
                                        if (IsNoteEmpty(note))
                                            continue;
                                        else
                                            aObjectItem = note;
                                    }

                                    Writer.WriteStartElement(nsInfo, propInfo.AixmName);
                                    WriteAranObject2(aObjectItem, 0);
                                    Writer.WriteEndElement();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (propInfo.AixmName != string.Empty)
                        {
                            if (AimMetadata.IsChoice(propInfo.TypeIndex))
                            {
                                WriteChoiceClass((IEditChoiceClass)propValue, propInfo.AixmName);
                            }
                            else
                            {
                                if (propInfo.IsExtension && !SerializeExtension)
                                    continue;

                                Writer.WriteStartElement(nsInfo, propInfo.AixmName);
                                if (propInfo.IsFeatureReference && propInfo.ReferenceFeature != 0)
                                {
                                    WriteFeatureRef((FeatureRef)propValue, propInfo.ReferenceFeature);
                                }
                                else
                                {
                                    WriteAranObject2(propValue, 0, isPropertyDateTime);
                                }
                                Writer.WriteEndElement();
                            }
                        }
                        else
                        {
                            if (propInfo.Name != "Id")
                                WriteAranObject2(propValue, 0, isPropertyDateTime);
                        }
                    }
                }
            }
        }

        private void WriteNullProperty(NamespaceInfo nsInfo, IAimObject aranObject, AimPropInfo propInfo)
        {
            if (aranObject.AimObjectType != AimObjectType.Feature)
                return;

            var nilReasonText = "unknown";
            var editFeature = aranObject as IEditFeature;
            var nr = editFeature.GetNilReasonProp(propInfo.Index);
            if (nr != null)
                nilReasonText = nr.Value.ToString();

            Writer.WriteStartElement(nsInfo, propInfo.AixmName);
            Writer.WriteAttributeString("nilReason", nilReasonText);
            Writer.WriteAttributeString(AimDbNamespaces.XSI, "nil", "true");
            Writer.WriteEndElement();
        }

        private void WriteChoiceClass (IEditChoiceClass choiceClass, string propAixmName)
        {
            AimPropInfo [] propInfoArr = AimMetadata.GetAimPropInfos ((IAimObject) choiceClass);
            
            foreach (AimPropInfo propInfo in propInfoArr)
            {
                if (propInfo.IsFeatureReference)
                {
                    if ((int) propInfo.ReferenceFeature == choiceClass.RefType)
                    {
                        Writer.WriteStartElement (AimDbNamespaces.AIXM51, propAixmName + "_" + propInfo.AixmName);
                        WriteFeatureRef ((FeatureRef) choiceClass.RefValue, propInfo.ReferenceFeature);
                        Writer.WriteEndElement ();
                        break;
                    }
                }
                else if (propInfo.TypeIndex == choiceClass.RefType)
                {
                    Writer.WriteStartElement (AimDbNamespaces.AIXM51, propAixmName + "_" + propInfo.AixmName);
                    WriteAranObject2 (choiceClass.RefValue, 0);
                    Writer.WriteEndElement ();
                    break;
                }
            }
        }

        private int WriteObject (AObject aObject, int propStartIndex)
        {
            string objectName = aObject.ObjectType.ToString();
            
            if (aObject.ObjectType == ObjectType.AixmPoint)
                objectName = "Point";

            Writer.WriteStartElement (AimDbNamespaces.AIXM51, objectName);
            Writer.WriteAttributeString (AimDbNamespaces.GML, "id", GenerateNewGmlId ());
            WriteProperties (aObject, 0);
            Writer.WriteEndElement ();
            return -1;
        }

        private bool IsNoteEmpty(Note note)
        {
            if (note.TranslatedNote.Count > 0)
            {
                var emptyCount = 0;
                foreach (var lgNote in note.TranslatedNote)
                {
                    if (lgNote.Note == null || string.IsNullOrEmpty(lgNote.Note.Value))
                        emptyCount++;
                }

                if (emptyCount == note.TranslatedNote.Count)
                    note.TranslatedNote.Clear();
                else
                {
                    for (int i = 0; i < note.TranslatedNote.Count; i++)
                    {
                        var lgNote = note.TranslatedNote[i];

                        if (lgNote.Note == null || string.IsNullOrEmpty(lgNote.Note.Value))
                        {
                            note.TranslatedNote.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return (note.TranslatedNote.Count == 0) && (string.IsNullOrEmpty(note.PropertyName) || (note.Purpose == null));
                
        }

        private int WriteDataType (ADataType dataType, int propStartIndex)
        {
            if (dataType.DataType == DataType.TextNote)
            {
                WriteTextBox ((TextNote) dataType);
                return -1;
            }

            if (dataType is IEditValClass)
                WriteValClass ((IEditValClass) dataType);
            else if (dataType is IAbstractFeatureRef)
                WriteAbstractFeatureRef ((IAbstractFeatureRef) dataType);
            else
                WriteProperties (dataType, propStartIndex, AimDbNamespaces.GML);

            return -1;
        }

        private void WriteTextBox (TextNote textNote)
        {
            if (!string.IsNullOrEmpty(textNote.Value))
            {
                Writer.WriteAttributeString("lang", textNote.Lang.ToString().ToLower());
                Writer.WriteString(textNote.Value);
            }
        }

        private void WriteTimePeriod (TimePeriod timePeriod)
        {
            Writer.WriteStartElement (AimDbNamespaces.GML, "TimePeriod");
            Writer.WriteAttributeString (AimDbNamespaces.GML, "id", GenerateNewGmlId ());
            Writer.WriteElementString (AimDbNamespaces.GML, "beginPosition", CommonXmlWriter.GetString (timePeriod.BeginPosition));
            Writer.WriteElementString (AimDbNamespaces.GML, "endPosition",
                (timePeriod.EndPosition == null ? "" : CommonXmlWriter.GetString (timePeriod.EndPosition.Value)));
            Writer.WriteEndElement ();
        }

        private void WriteAbstractFeatureRef (IAbstractFeatureRef abstractFeatureRef)
        {
            WriteFeatureRef ((FeatureRef) abstractFeatureRef, (FeatureType) abstractFeatureRef.FeatureTypeIndex);
        }

        private void WriteFeatureRef (FeatureRef featureRef, FeatureType featureType)
        {
#if URN_UUID || EUROCONTROL_NAMESPACE
            Writer.WriteAttributeString (AimDbNamespaces.XLINK, "href", "urn:uuid:" + CommonXmlWriter.GetString (featureRef.Identifier));
#else
            Writer.WriteAttributeString (AimDbNamespaces.XLINK, "href",
                string.Format (CAW.Namespace + "#xpointer(//{0}[gml:identifier='{1}'])",
                featureType,
                CommonXmlWriter.GetString (featureRef.Identifier)));
#endif
        }

        private void WriteValClass (IEditValClass editValClass)
        {
            ValClassBase vcb = editValClass as ValClassBase;
            if (vcb.DataType == DataType.ValDistanceVertical)
            {
                ValDistanceVertical vdv = editValClass as ValDistanceVertical;
                if (vdv.OtherValue != null)
                {
                    Writer.WriteValue (vdv.OtherValue.Value.ToString ());
                }
                else
                {
                    Writer.WriteAttributeString ("uom", vdv.Uom.ToString ());
                    Writer.WriteValue (vdv.Value.ToString ("0.####"));
                }
                return;
            }

            AimPropInfo [] aranPropInfoArr = AimMetadata.GetAimPropInfos (editValClass as IAimObject);

            string uomValueText = AimMetadata.GetEnumValueAsString (
                editValClass.Uom, aranPropInfoArr [1].TypeIndex);

            Writer.WriteAttributeString ("uom", uomValueText);
            Writer.WriteValue (Convert.ToDecimal (editValClass.Value));
        }

        private void PropertySerialize (IAimObject aranObject, int propIndex, XmlWriter writer, int propStartIndex)
        {
            IAimProperty aranProp = aranObject.GetValue (propIndex);
            if (aranProp != null)
                WriteAranObject2 (aranProp, propIndex);
        }

        private int WriteAranField2(AimField aranField, bool isDateTime = false)
        {
            IEditAimField editAranField = aranField;

            switch (aranField.FieldType)
            {
                case AimFieldType.SysGuid:
                    Writer.WriteString(CommonXmlWriter.GetString((Guid)editAranField.FieldValue));
                    break;
                case AimFieldType.SysDateTime:
                    Writer.WriteString(CommonXmlWriter.GetString((DateTime)editAranField.FieldValue, isDateTime));
                    break;
                case AimFieldType.SysBool:
                    if (editAranField.FieldValue.Equals(true))
                        Writer.WriteString("YES");
                    else
                        Writer.WriteString("NO");
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    CommonXmlWriter.WriteElement((Aran.Geometries.Geometry)editAranField.FieldValue, Writer, false, Write3DIfExists, SrsType);
                    break;
                case AimFieldType.SysEnum:
                    Writer.WriteString(CommonXmlWriter.GetEnumString(editAranField.FieldValue));
                    break;
                default:
                    Writer.WriteString(editAranField.FieldValue.ToString());
                    break;
            }
            return -1;
        }

        private void WriteTimeSliceMetadata (FeatureTimeSliceMetadata md, XmlWriter writer)
        {
        }
        
        private void WriteXHtml(string xml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                doc.DocumentElement?.WriteTo(Writer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int _gmlIdCounter;
    }
}
