using System;
using System.Collections;
using System.Xml;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;
using Aran.Aim.Metadata;

namespace Aran.Aim.CAWProvider
{
    public class AranAixmWriter
    {
        static AranAixmWriter ()
        {
            _gmlIdCounter = 0;
        }

        public AranAixmWriter (XmlWriter xmlWriter)
        {
            Writer = xmlWriter;
            SerializeExtension = true;
        }

        public static string GenerateNewGmlId ()
        {
            _gmlIdCounter++;
            return "gmlAranID" + _gmlIdCounter;
        }

        public XmlWriter Writer { get; set; }

        public bool SerializeExtension { get; set; }

        public int WriteAranObject (IAimProperty aranProperty, int propStartIndex)
        {
            switch (aranProperty.PropertyType)
            {
                case AimPropertyType.AranField:
                    propStartIndex = WriteAranField ((AimField) aranProperty, propStartIndex);
                    break;
                case AimPropertyType.DataType:
                    propStartIndex = WriteDataType ((ADataType) aranProperty, propStartIndex);
                    break;
                case AimPropertyType.Object:
                    propStartIndex = WriteObject ((AObject) aranProperty, propStartIndex);
                    break;
            }

            if (propStartIndex == -1)
                return propStartIndex;

            WriteProperties ((IAimObject) aranProperty, propStartIndex);
            return -1;
        }

        public void WriteFeatureTimeSlice (Feature feature)
        {
            string featureName = feature.FeatureType.ToString ();

            Writer.WriteStartElement (AimdbNamespaces.AIXM51, "timeSlice");
            {
                Writer.WriteStartElement (AimdbNamespaces.AIXM51, featureName + "TimeSlice");
                {
                    Writer.WriteAttributeString (AimdbNamespaces.GML, "id", GenerateNewGmlId ());

                    if (feature.TimeSlice != null)
                    {
                        if (feature.TimeSlice.ValidTime != null)
                        {
                            Writer.WriteStartElement (AimdbNamespaces.GML, "validTime");
                            WriteTimePeriod (feature.TimeSlice.ValidTime);
                            Writer.WriteEndElement ();
                        }

                        Writer.WriteStartElement (AimdbNamespaces.AIXM51, "interpretation");
                        PropertySerialize (feature.TimeSlice, (int) PropertyTimeSlice.Interpretation, Writer, 0);
                        Writer.WriteEndElement ();

                        Writer.WriteStartElement (AimdbNamespaces.AIXM51, "sequenceNumber");
                        PropertySerialize (feature.TimeSlice, (int) PropertyTimeSlice.SequenceNumber, Writer, 0);
                        Writer.WriteEndElement ();

                        if (feature.TimeSlice.CorrectionNumber >= 0)
                        {
                            Writer.WriteStartElement (AimdbNamespaces.AIXM51, "correctionNumber");
                            PropertySerialize (feature.TimeSlice, (int) PropertyTimeSlice.CorrectionNumber, Writer, 0);
                            Writer.WriteEndElement ();
                        }

                        if (feature.TimeSliceMetadata != null)
                        {
                            Writer.WriteStartElement (AimdbNamespaces.AIXM51, "timeSliceMetadata");
                            WriteTimeSliceMetadata (feature.TimeSliceMetadata, Writer);
                            Writer.WriteEndElement ();
                        }

                        if (feature.TimeSlice.FeatureLifetime != null)
                        {
                            Writer.WriteStartElement (AimdbNamespaces.AIXM51, "featureLifetime");
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

        
        private void WriteExtension (Feature feature)
        {
            bool isNew = (feature.Id == -1);

            Writer.WriteStartElement (AimdbNamespaces.AIXM51, "extension");
            {
                Writer.WriteStartElement (AimdbNamespaces.CAE, feature.FeatureType + "Extension");
                {
                    Writer.WriteAttributeString (AimdbNamespaces.GML, "id", GenerateNewGmlId ());
                    Writer.WriteElementString (AimdbNamespaces.CAE, "aipClassifier", "AMDT");
                    Writer.WriteElementString (AimdbNamespaces.CAE, "featureCode", feature.FeatureType + " " + feature.Identifier);

                    if (isNew)
                    {
                        Writer.WriteStartElement (AimdbNamespaces.CAE, "authority");
                        Writer.WriteAttributeString ("role", "AIP-PUBLICATION");
                        Writer.WriteString ("Estonia");
                        Writer.WriteEndElement ();

                        Writer.WriteStartElement (AimdbNamespaces.CAE, "authority");
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
                nsInfo = AimdbNamespaces.AIXM51;

            AimPropInfo [] propInfoArr = AimMetadata.GetAimPropInfos (aranObject);

            for (int i = propStartIndex; i < propInfoArr.Length; i++)
            {
                AimPropInfo propInfo = propInfoArr [i];
                IAimProperty propValue = aranObject.GetValue (propInfo.Index);

                if (propValue != null)
                {
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
                                if (AimMetadata.IsChoice (propInfo.TypeIndex))
                                {
                                    WriteChoiceClass ((IEditChoiceClass) aObject, propInfo.AixmName);
                                }
                                else
                                {
                                    Writer.WriteStartElement (nsInfo, propInfo.AixmName);
                                    WriteAranObject (aObject, 0);
                                    Writer.WriteEndElement ();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (propInfo.AixmName != string.Empty)
                        {
                            if (AimMetadata.IsChoice (propInfo.TypeIndex))
                            {
                                WriteChoiceClass ((IEditChoiceClass) propValue, propInfo.AixmName);
                            }
                            else
                            {
                                if (propInfo.IsExtension && !SerializeExtension)
                                    continue;

                                Writer.WriteStartElement (nsInfo, propInfo.AixmName);
                                if (propInfo.IsFeatureReference && propInfo.ReferenceFeature != 0)
                                {
                                    WriteFeatureRef ((FeatureRef) propValue, propInfo.ReferenceFeature);
                                }
                                else
                                {
                                    WriteAranObject (propValue, 0);
                                }
                                Writer.WriteEndElement ();
                            }
                        }
                        else
                        {
                            if (propInfo.Name != "Id")
                                WriteAranObject (propValue, 0);
                        }
                    }
                }
            }
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
                        Writer.WriteStartElement (AimdbNamespaces.AIXM51, propAixmName + "_" + propInfo.AixmName);
                        WriteFeatureRef ((FeatureRef) choiceClass.RefValue, propInfo.ReferenceFeature);
                        Writer.WriteEndElement ();
                        break;
                    }
                }
                else if (propInfo.TypeIndex == choiceClass.RefType)
                {
                    Writer.WriteStartElement (AimdbNamespaces.AIXM51, propAixmName + "_" + propInfo.AixmName);
                    WriteAranObject (choiceClass.RefValue, 0);
                    Writer.WriteEndElement ();
                    break;
                }
            }
        }

        private int WriteObject (AObject aObject, int propStartIndex)
        {
            string objectName;
            if (aObject.ObjectType == ObjectType.AixmPoint)
                objectName = "Point";
            else
                objectName = aObject.ObjectType.ToString ();

            Writer.WriteStartElement (AimdbNamespaces.AIXM51, objectName);
            Writer.WriteAttributeString (AimdbNamespaces.GML, "id", GenerateNewGmlId ());
            WriteProperties (aObject, 0);
            Writer.WriteEndElement ();
            return -1;
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
                WriteProperties (dataType, propStartIndex, AimdbNamespaces.GML);

            return -1;
        }

        private void WriteTextBox (TextNote textNote)
        {
            Writer.WriteAttributeString ("lang", textNote.Lang.ToString ().ToLower ());
            Writer.WriteString (textNote.Value);
        }

        private void WriteTimePeriod (TimePeriod timePeriod)
        {
            Writer.WriteStartElement (AimdbNamespaces.GML, "TimePeriod");
            Writer.WriteAttributeString (AimdbNamespaces.GML, "id", GenerateNewGmlId ());
            Writer.WriteElementString (AimdbNamespaces.GML, "beginPosition", CommonXmlWriter.GetString (timePeriod.BeginPosition));
            Writer.WriteElementString (AimdbNamespaces.GML, "endPosition",
                (timePeriod.EndPosition == null ? "" : CommonXmlWriter.GetString (timePeriod.EndPosition.Value)));
            Writer.WriteEndElement ();
        }

        private void WriteAbstractFeatureRef (IAbstractFeatureRef abstractFeatureRef)
        {
            WriteFeatureRef ((FeatureRef) abstractFeatureRef, (FeatureType) abstractFeatureRef.FeatureTypeIndex);
        }

        private void WriteFeatureRef (FeatureRef featureRef, FeatureType featureType)
        {
#if EUROCONTROL_NAMESPACE
            Writer.WriteAttributeString (AimdbNamespaces.XLINK, "href", "urn:uuid:" + CommonXmlWriter.GetString (featureRef.Identifier));
#else
            Writer.WriteAttributeString (AimdbNamespaces.XLINK, "href",
                string.Format (AimdbNamespaces.CAW.Namespace + "#xpointer(//{0}[gml:identifier='{1}'])",
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
                WriteAranObject (aranProp, propIndex);
        }

        private int WriteAranField (AimField aranField, int propStartIndex)
        {
            IEditAimField editAranField = aranField;

            switch (aranField.FieldType)
            {
                case AimFieldType.SysGuid:
                    Writer.WriteString (CommonXmlWriter.GetString ((Guid) editAranField.FieldValue));
                    break;
                case AimFieldType.SysDateTime:
                    Writer.WriteString (CommonXmlWriter.GetString ((DateTime) editAranField.FieldValue));
                    break;
                case AimFieldType.SysBool:
                    if (editAranField.FieldValue.Equals (true))
                        Writer.WriteString ("YES");
                    else
                        Writer.WriteString ("NO");
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    CommonXmlWriter.WriteElement ((Aran.Geometries.Geometry) editAranField.FieldValue, Writer, false);
                    break;
                case AimFieldType.SysEnum:
                    Writer.WriteString (CommonXmlWriter.GetEnumString (editAranField.FieldValue));
                    break;
                default:
                    Writer.WriteString (editAranField.FieldValue.ToString ());
                    break;
            }
            return -1;
        }

        private void WriteTimeSliceMetadata (FeatureTimeSliceMetadata md, XmlWriter writer)
        {
        }

        private static int _gmlIdCounter;
    }
}
