using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParseMDL;

namespace UMLInfo
{
    public partial class ObjectInfoParser
    {
        public ObjectInfoParser (List<UmlClass> umlClassList, List<Association> assocList)
        {
            _umlClassList = umlClassList;
            _assocList = assocList;

            _replaceNames = new List<string []> ();
            _replaceNames.Add (new string [] { "CodeYesNoBase", "boolean" });

            _replaceObjectInfo = new List<string []> ();
            _replacedPropertyNameList = new List<string> ();
        }

        public List<ObjectInfo> GetObjectInfoList ()
        {
            _objInfoList = new List<ObjectInfo> ();

            foreach (UmlClass umlClass in _umlClassList)
            {
                if (umlClass.StereoType != null)
                {
                    _objInfoList.Add (ToObjectInfo (umlClass));
                }
            }

            for (int i = 0; i < _objInfoList.Count; i++)
            {
                foreach (var replaceName in _replaceNames)
                {
                    if (_objInfoList [i].Name == replaceName [0])
                    {
                        var item = _objInfoList.Where (oi => oi.Name == replaceName [1]).FirstOrDefault ();
                        if (item != null)
                        {
                            string [] val = new string [] { _objInfoList [i].Id, item.Id };
                            _replaceObjectInfo.Add (val);
                        }
                    }
                }
            }

            for (int i = 0; i < _objInfoList.Count; i++)
            {
                _objInfoList [i].Base = _objInfoList.Where (
                    oi => oi.Id == (_objInfoList [i].Tag as UmlClass).SuperClassId).FirstOrDefault ();
            }

            for (int i = 0; i < _objInfoList.Count; i++)
            {
                FillProperties (_objInfoList [i], _objInfoList);
            }

            for (int i = 0; i < _objInfoList.Count; i++)
            {
                ModifyProperties (_objInfoList [i], _objInfoList);
            }

            _stringObjInfo = _objInfoList.Where (oi => oi.Name == "string").First ();

            TakeOutDerivedFromSystemType (_objInfoList);

            AddSingleParentPropsToChild (_objInfoList);

            SetAssociationBetween (_objInfoList);

            AddCustomTypes (_objInfoList);

            SetIsUsed (_objInfoList);

            foreach (var item in _objInfoList)
            {
                if (item.IsChoice)
                    item.IsAbstract = false;
            }

            _objInfoList.Where (oi => oi.Name == "Point").First ().Name = "AixmPoint";

            ChangeOtherListPropertyToObjectList (_objInfoList);

            CorrectNameIfBase (_objInfoList);

            CheckPropertyName (_objInfoList);

            #region Do Some changes.
            ObjectInfo aStringType = _objInfoList.Where (oi => oi.Name == "string").First ();
            ObjectInfo objInfo = _objInfoList.Where (oi => oi.Name == "RulesProcedures").First ();
            PropInfo propInfo = objInfo.Properties.Where (pi => pi.Name == "Content").First ();
            propInfo.PropType = aStringType;

            objInfo = _objInfoList.Where (oi => oi.Name == "Note").First ();
            propInfo = objInfo.Properties.Where (pi => pi.Name == "PropertyName").First ();
            propInfo.PropType = aStringType;
            #endregion

            ChangeIfPropNameEqualsObjectName (_objInfoList);

            CreateAbstractFeatureAsDataType (_objInfoList);

            _objInfoList.Where (oi => oi.Name == "XHTML").First ().IsUsed = false;

            RemovePropertyWithEmptyEnum (_objInfoList);

            return _objInfoList;
        }

        public List<string> ReplacedPropertyNameList
        {
            get { return _replacedPropertyNameList; }
        }

        private void RemovePropertyWithEmptyEnum (List<ObjectInfo> objInfoList)
        {
            foreach (ObjectInfo objInfo in objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Codelist)
                    continue;

                for(int i = 0; i <objInfo.Properties.Count; i++)
                {
                    PropInfo propInfo = objInfo.Properties [i];

                    if (propInfo.PropType.Type == ObjectInfoType.Codelist && propInfo.PropType.Properties.Count == 0)
                    {
                        objInfo.Properties.RemoveAt (i);
                        i--;
                    }
                }
            }
        }

        private void CreateAbstractFeatureAsDataType (List<ObjectInfo> objInfoList)
        {
            List<ObjectInfo> absFeatList = new List<ObjectInfo> ();

            foreach (ObjectInfo objInfo in objInfoList)
            {
                if (objInfo.IsUsed &&
                    objInfo.Type == ObjectInfoType.Feature &&
                    objInfo.IsAbstract)
                {
                    absFeatList.Add (objInfo);
                }
            }

            ObjectInfo typeObjInfo = objInfoList.Where (oi => oi.Name == "int").First ();
            ObjectInfo featureObjInfo = objInfoList.Where (oi => oi.Name == "FeatureRef").First ();

            foreach (ObjectInfo objInfo in absFeatList)
            {
                ObjectInfo objInf_AbsFeatRef = new ObjectInfo ();
                objInf_AbsFeatRef.Type = ObjectInfoType.Datatype;
                objInf_AbsFeatRef.Name = "Abstract" + objInfo.Name + "Ref";
                objInf_AbsFeatRef.Namespace = "Aran.Aim.DataTypes";
                objInf_AbsFeatRef.IsUsed = true;

                objInf_AbsFeatRef.Properties.Add (new PropInfo ("Type", typeObjInfo));
                objInf_AbsFeatRef.Properties.Add (new PropInfo ("Feature", featureObjInfo));

                objInfoList.Add (objInf_AbsFeatRef);


                ObjectInfo objInf_AbsFeatRefObject = new ObjectInfo ();
                objInf_AbsFeatRefObject.Type = ObjectInfoType.Object;
                objInf_AbsFeatRefObject.Name = "Abstract" + objInfo.Name + "RefObject";
                objInf_AbsFeatRefObject.Namespace = "Aran.Aim.Objects";
                objInf_AbsFeatRefObject.IsUsed = true;

                objInf_AbsFeatRefObject.Properties.Add (new PropInfo ("Feature", objInf_AbsFeatRef));

                objInfoList.Add (objInf_AbsFeatRefObject);


                foreach (ObjectInfo item in objInfoList)
                {
                    foreach (PropInfo pi in item.Properties)
                    {
                        if (pi.PropType == objInfo)
                            pi.PropType = (pi.IsList ? objInf_AbsFeatRefObject : objInf_AbsFeatRef);
                    }
                }
            }
        }

        private void ChangeIfPropNameEqualsObjectName (List<ObjectInfo> objInfoList)
        {
            foreach (var objInfo in objInfoList)
            {
                foreach (var propInfo in objInfo.Properties)
                {
                    if (propInfo.Name == objInfo.Name)
                    {
                        propInfo.Name = objInfo.Name + "P";
                    }
                }
            }
        }

        private void CheckPropertyName (List<ObjectInfo> objInfoList)
        {
            foreach (var objInfo in objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Codelist)
                    continue;

                foreach (var propInfo in objInfo.Properties)
                {
                    int ind;
                    if ((ind = propInfo.Name.IndexOf ('-')) >= 0)
                    {
                        _replacedPropertyNameList.Add (propInfo.Name);

                        string s = propInfo.Name;
                        if (ind < s.Length - 1)
                        {
                            char c = s [ind + 1];
                            s = s.Remove (ind, 2).Insert (ind, char.ToUpper (c).ToString ());
                        }
                        else
                            s = s.Remove (ind, 1);
                        propInfo.Name = s;
                    }
                }
            }
        }

        private void CorrectNameIfBase (List<ObjectInfo> objInfoList)
        {
            foreach (var objInfo in objInfoList)
            {
                if (objInfo.IsUsed &&
                    objInfo.Type == ObjectInfoType.Codelist && 
                    objInfo.Name.EndsWith ("Base"))
                {
                    string newName = objInfo.Name.Remove (objInfo.Name.Length - 4);
                    foreach (var item in objInfoList)
                    {
                        if (item.Name == newName)
                        {
                            item.Name = item.Name + "_";
                            break;
                        }
                    }
                    objInfo.Name = newName;
                }
            }
        }

        private void ChangeOtherListPropertyToObjectList (List<ObjectInfo> objInfoList)
        {
            for (int i = 0; i < objInfoList.Count; i++)
            {
                ObjectInfo objInfo = objInfoList [i];

                foreach (PropInfo propInfo in objInfo.Properties)
                {
                    if (propInfo.IsList &&
                        propInfo.PropType.Type != ObjectInfoType.Feature &&
                        propInfo.PropType.Type != ObjectInfoType.Object)
                    {
                        ObjectInfo newObjInfo = new ObjectInfo ();
                        newObjInfo.Type = ObjectInfoType.Object;
                        newObjInfo.Name = propInfo.PropType.Name + "Object";
                        newObjInfo.Namespace = "Aran.Aim.Objects";
                        newObjInfo.IsUsed = true;

                        newObjInfo.Properties.Add (new PropInfo ("Value", propInfo.PropType, false));

                        propInfo.PropType = newObjInfo;

                        objInfoList.Add (newObjInfo);
                    }
                }
            }
        }

        private void AddCustomTypes (List<ObjectInfo> objInfoList)
        {
            ObjectInfo aStringType = objInfoList.Where (oi => oi.Name == "string").FirstOrDefault ();
            ObjectInfo aDateTimeType = objInfoList.Where (oi => oi.Name == "dateTime").FirstOrDefault ();
            ObjectInfo aIntegerType = objInfoList.Where (oi => oi.Name == "integer").FirstOrDefault ();
            ObjectInfo aLongType = objInfoList.Where (oi => oi.Name == "long").FirstOrDefault ();
            ObjectInfo aTimePeriodType;
            ObjectInfo aTimeSliceInterpretationType;
            ObjectInfo aTimeSliceType;
            ObjectInfo aFeatureType;
            //ObjectInfo aAnnotationType;
            ObjectInfo aFeatureRef;
            ObjectInfo aFeatureRefObject;

            #region TimeSliceInterpretationType
            ObjectInfo newObjInfo = new ObjectInfo ();
            newObjInfo.Type = ObjectInfoType.Codelist;
            newObjInfo.Name = "TimeSliceInterpretationType";
            newObjInfo.Namespace = "Aran.Aim.Enums";
            newObjInfo.IsUsed = true;

            newObjInfo.Properties.Add (new PropInfo ("BASELINE", aStringType));
            newObjInfo.Properties.Add (new PropInfo ("SNAPSHOT", aStringType));
            newObjInfo.Properties.Add (new PropInfo ("TEMPDELTA", aStringType));
            newObjInfo.Properties.Add (new PropInfo ("PERMDELTA", aStringType));

            objInfoList.Add (newObjInfo);
            aTimeSliceInterpretationType = newObjInfo;
            #endregion

            #region TimePeriod
            newObjInfo = new ObjectInfo ();
            newObjInfo.Type = ObjectInfoType.Datatype;
            newObjInfo.Name = "TimePeriod";
            newObjInfo.Namespace = "Aran.Aim.DataTypes";
            newObjInfo.IsUsed = true;

            newObjInfo.Properties.Add (new PropInfo ("BeginPosition", aDateTimeType));
            newObjInfo.Properties.Add (new PropInfo ("EndPosition", aDateTimeType, true));

            objInfoList.Add (newObjInfo);
            aTimePeriodType = newObjInfo;
            #endregion

            #region Metadata
            
            CreateMessageMetadata ();
            var aMetadata = Create_FeatureTimeSliceMetadata ();

            CreateIsoMetadataObjects();

            #endregion

            #region TimeSlice
            newObjInfo = new ObjectInfo ();
            newObjInfo.Type = ObjectInfoType.Datatype;
            newObjInfo.Name = "TimeSlice";
            newObjInfo.Namespace = "Aran.Aim.DataTypes";
            newObjInfo.IsUsed = true;

            newObjInfo.Properties.Add (new PropInfo ("ValidTime", aTimePeriodType, true));
            newObjInfo.Properties.Add (new PropInfo ("Interpretation", aTimeSliceInterpretationType));
            newObjInfo.Properties.Add (new PropInfo ("SequenceNumber", aIntegerType));
            newObjInfo.Properties.Add (new PropInfo ("CorrectionNumber", aIntegerType));

            if (aMetadata != null)
                newObjInfo.Properties.Add (new PropInfo ("TimeSliceMetadata", aMetadata));

            newObjInfo.Properties.Add (new PropInfo ("FeatureLifetime", aTimePeriodType, true));

            objInfoList.Add (newObjInfo);
            aTimeSliceType = newObjInfo;
            #endregion

            #region Feature
            newObjInfo = new ObjectInfo ();
            newObjInfo.Type = ObjectInfoType.Feature;
            newObjInfo.IsAbstract = true;
            newObjInfo.Name = "Feature";
            newObjInfo.Namespace = "Aran.Aim.Features";
            newObjInfo.IsUsed = false;

            newObjInfo.Properties.Add (new PropInfo ("Identifier", aStringType));
            newObjInfo.Properties.Add (new PropInfo ("TimeSlice", aTimeSliceType, true));
            newObjInfo.Properties.Add (new PropInfo ("Metadata", GetByName("MdMetadata"), true));

            aFeatureType = newObjInfo;
            objInfoList.Add (aFeatureType);
            #endregion

            #region FeatureRef
            aFeatureRef = new ObjectInfo ();
            aFeatureRef.Type = ObjectInfoType.Datatype;
            aFeatureRef.Name = "FeatureRef";
            aFeatureRef.Namespace = "Aran.Aim.DataTypes";
            aFeatureRef.IsUsed = true;

            aFeatureRef.Properties.Add (new PropInfo ("Id", aLongType));

            objInfoList.Add (aFeatureRef);
            #endregion

            #region FeatureRefObject
            aFeatureRefObject = new ObjectInfo ();
            aFeatureRefObject.Type = ObjectInfoType.Object;
            aFeatureRefObject.Name = "FeatureRefObject";
            aFeatureRefObject.Namespace = "Aran.Aim.Objects";
            aFeatureRefObject.IsUsed = true;

            aFeatureRefObject.Properties.Add (new PropInfo ("Feature", aFeatureRef));

            objInfoList.Add (aFeatureRefObject);
            #endregion
        }

        private void AddSingleParentPropsToChild (List<ObjectInfo> objInfoList)
        {
            foreach (var objInfo in objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Object ||
                    objInfo.Type == ObjectInfoType.Datatype)
                {
                    if (objInfo.Name == "Point" ||
                        objInfo.Name == "Curve" ||
                        objInfo.Name == "Surface")
                    {
                        continue;
                    }

                    List<ObjectInfo> childs = objInfoList.Where (oi => oi.Base == objInfo).ToList ();
                    if (childs.Count == 1)
                    {
                        childs [0].Properties.InsertRange (0, objInfo.Properties);
                        childs [0].RestrictionList.InsertRange (0, objInfo.RestrictionList);
                        childs [0].Base = objInfo.Base;
                    }
                    else if ((objInfo.Base == null || objInfo.Base.Type != ObjectInfoType.Codelist) &&
                        childs.Count > 1 &&
                        objInfo.Type == ObjectInfoType.Datatype)
                    {
                        foreach (ObjectInfo chilObj in childs)
                        {
                            chilObj.Properties.InsertRange (0, objInfo.Properties);
                            chilObj.RestrictionList.InsertRange (0, objInfo.RestrictionList);
                            chilObj.Base = objInfo.Base;
                        }
                    }
                }
            }
        }

        private void TakeOutDerivedFromSystemType (List<ObjectInfo> objInfoList)
        {
            ObjectInfo intObjInfo = objInfoList.Where (oi => oi.Name == "integer").First ();

            foreach (ObjectInfo objInfo in objInfoList)
            {
                if (objInfo.Name == "Point" ||
                    objInfo.Name == "Curve" ||
                    objInfo.Name == "Surface")
                {
                    PropInfo propInfo = new PropInfo ();
                    propInfo.Name = "Geo";
                    propInfo.PropType = intObjInfo;
                    objInfo.Properties.Insert (0, propInfo);
                }
                else if (objInfo.Type != ObjectInfoType.XSDsimpleType &&
                    objInfo.Type != ObjectInfoType.Codelist &&
                    objInfo.Base != null &&
                    objInfo.Base.Type == ObjectInfoType.XSDsimpleType)
                {
                    PropInfo propInfo = new PropInfo ();
                    propInfo.Name = "Value";
                    propInfo.PropType = objInfo.Base;
                    objInfo.Properties.Insert (0, propInfo);

                    objInfo.Base = null;
                }
            }
        }

        private void SetAssociationBetween (List<ObjectInfo> objInfoList)
        {
            foreach (var assoc in _assocList)
            {
                if (assoc.AssociationClass != null)
                {
                    foreach (var objInfo in objInfoList)
                    {
                        if (objInfo.Id == assoc.AssociationClass)
                        {
                            objInfo.AssocBetween = new AssocBetween ();
                            ObjectInfo role1ObjInfo = (from oi in objInfoList
                                                       where (oi.Id == assoc.Role1.SupplierId)
                                                       select oi).FirstOrDefault ();
                            ObjectInfo role2ObjInfo = (from oi in objInfoList
                                                       where (oi.Id == assoc.Role2.SupplierId)
                                                       select oi).FirstOrDefault ();

                            if (assoc.Role1.Label == null)
                            {
                                objInfo.AssocBetween.ObjectInfoFrom = role1ObjInfo;
                                objInfo.AssocBetween.ObjectInfoTo = role2ObjInfo;
                            }
                            else
                            {
                                objInfo.AssocBetween.ObjectInfoFrom = role2ObjInfo;
                                objInfo.AssocBetween.ObjectInfoTo = role1ObjInfo;
                            }

                            PropInfo propInfo = new PropInfo ();
                            propInfo.Name = "The" + objInfo.AssocBetween.ObjectInfoTo.Name;
                            propInfo.PropType = objInfo.AssocBetween.ObjectInfoTo;
                            propInfo.IsList = false;
                            propInfo.Nullable = true;
                            propInfo.IsAssociation = true;

                            CardinalityInterval cardInt = new CardinalityInterval ();
                            cardInt.Min = 0;
                            cardInt.Max = 1;
                            propInfo.Cardinality = cardInt;

                            foreach (var pr in objInfo.AssocBetween.ObjectInfoFrom.Properties)
                            {
                                if (pr.PropType == objInfo.AssocBetween.ObjectInfoTo)
                                {
                                    pr.PropType = objInfo;
                                    break;
                                }
                            }

                            objInfo.Properties.Add (propInfo);
                        }
                    }
                }
            }
        }

        private void SetIsUsed (ObjectInfo objInfo)
        {
            if (objInfo.IsUsed)
                return;

            if (objInfo.Name == "Character2")
            {
            }

            objInfo.IsUsed = true;
            if (objInfo.Base != null)
            {
                SetIsUsed (objInfo.Base);
            }

            if (objInfo.Type == ObjectInfoType.Codelist)
                return;

            foreach (var propInfo in objInfo.Properties)
            {
                if (propInfo.PropType.Type != ObjectInfoType.Feature)
                {
                    SetIsUsed (propInfo.PropType);
                }
            }
        }

        private void SetIsUsed (List<ObjectInfo> objInfoList)
        {
            foreach (var objInfo in objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Feature)
                {
                    foreach (var propInfo in objInfo.Properties)
                    {
                        if (propInfo.PropType.Type != ObjectInfoType.Feature)
                        {
                            SetIsUsed (propInfo.PropType);
                        }
                    }
                }
            }
        }

        private void ModifyProperties (ObjectInfo objInfo, List<ObjectInfo> objInfoList)
        {
            foreach (PropInfo propInfo in objInfo.Properties)
            {
                if (!propInfo.IsAssociation)
                {
                    ObjectInfo propObjInfo = propInfo.PropType;
                    List<string []> restrictionValues = new List<string []> ();

                    while (propObjInfo != null)
                    {
                        restrictionValues.AddRange (propObjInfo.RestrictionList);

                        if (propObjInfo.Type != ObjectInfoType.Datatype ||
                            propObjInfo.Properties.Count > 0 ||
                            (propObjInfo.Name == "XHTML"))
                        {
                            break;
                        }

                        propObjInfo = propObjInfo.Base;
                    }

                    bool isNullable = false;

                    for (int i = 0; i < restrictionValues.Count; i++)
                    {
                        if (restrictionValues [i] [0] == "nilReason")
                        {
                            restrictionValues.RemoveAt (i);
                            isNullable = true;
                            break;
                        }
                    }

                    propInfo.Nullable = isNullable;
                    propInfo.PropType = propObjInfo;
                    propInfo.Restriction = ValueRestriction.Parse (restrictionValues);

                    if (propInfo.PropType != null)
                    {
                        foreach (string [] sa in _replaceObjectInfo)
                        {
                            if (sa [0] == propInfo.PropType.Id)
                            {
                                propInfo.PropType = (from oi in objInfoList
                                                     where (oi.Id == sa [1])
                                                     select oi).FirstOrDefault ();
                            }
                        }
                    }
                }
            }
        }

        private void FillProperties (ObjectInfo objInfo, List<ObjectInfo> objInfoList)
        {
            UmlClass umlClass = objInfo.Tag as UmlClass;

            foreach (ClassAttribute ca in umlClass.Attributes)
            {
                if (objInfo.Type == ObjectInfoType.Codelist && ca.Name == "OTHER")
                    continue;

                if (ca.InitialValue != null || ca.Name == "nilReason")
                {
                    string [] restrictionVal = new string [] { ca.Name, ca.InitialValue };
                    objInfo.RestrictionList.Add (restrictionVal);
                }
                else
                {
                    PropInfo pi = new PropInfo ();
                    pi.Tag = ca;
                    pi.Id = ca.Id;
                    pi.Name = ReplacePropertyName (ca.Name);
                    pi.Documentation = ca.Documentation;
                    pi.IsAssociation = false;

                    pi.PropType = (from oi in objInfoList
                                   where (oi.Id == ca.TypeId)
                                   select oi).FirstOrDefault ();

                    objInfo.Properties.Add (pi);
                }
            }

            Role mainRole = null;
            Role otherRole = null;

            foreach (Association assoc in _assocList)
            {
                bool b1 = (assoc.Role1.Label != null &&
                    assoc.Role2.SupplierId == umlClass.Id);
                bool b2 = (assoc.Role2.Label != null &&
                    assoc.Role1.SupplierId == umlClass.Id);

                if (b1 || b2)
                {
                    if (b1)
                    {
                        mainRole = assoc.Role1;
                        otherRole = assoc.Role2;
                    }
                    else
                    {
                        mainRole = assoc.Role2;
                        otherRole = assoc.Role1;
                    }

                    PropInfo pi = new PropInfo ();
                    pi.Id = assoc.Id;
                    pi.Name = ReplacePropertyName (mainRole.Name);
                    pi.IsAssociation = true;

                    {
                        pi.Documentation = (assoc.Documentation != null ? assoc.Documentation :
                            (assoc.Role1.Documentation != null ? assoc.Role1.Documentation :
                                (assoc.Role2.Documentation != null ? assoc.Role2.Documentation : "")));
                    }

                    {
                        pi.PropType = objInfoList.Where (oi => oi.Id == mainRole.SupplierId).FirstOrDefault ();
                    }

                    {
                        if (mainRole.Containment == "REF")
                            pi.Containment = PropInfoContainment.REF;
                        else if (mainRole.Containment == "VAL")
                            pi.Containment = PropInfoContainment.VAL;
                        else
                            pi.Containment = PropInfoContainment.Unspecified;
                    }

                    pi.Cardinality = CardinalityInterval.Parse (mainRole.ClientCardinality);
                    pi.OtherCardinality = CardinalityInterval.Parse (otherRole.ClientCardinality);
                    pi.Nullable = (pi.Cardinality.IsEmpty || pi.Cardinality.Min == 0);
                    pi.IsList = (!pi.Cardinality.IsEmpty && pi.Cardinality.Max == short.MaxValue);

                    pi.Restriction = null;

                    objInfo.Properties.Add (pi);
                }
            }
        }

        private string ReplacePropertyName (string propName)
        {
            if (!char.IsLower (propName, 0))
                return propName;

            char [] ca = propName.ToCharArray ();
            ca [0] = char.ToUpper (ca [0]);
            return new string (ca);
        }

        private void FillPropertyRestriction (PropInfo pi)
        {
            //ObjectInfo objInfo = pi.PropType;

            //while (objInfo != null)
            //{
            //    foreach (var item in objInfo.Properties)
            //    {
            //        item.is
            //    }

            //    objInfo = objInfo.Base;
            //}

            //if (pi.PropType != null)
            //{
            //    var nilReasonObject = (from pr in pi.PropType.Properties
            //                           where pr.Name == "nilReason"
            //                           select pr).FirstOrDefault ();

            //    pi.PropType = pi.PropType.Base;

            //    pi.Nullable = (nilReasonObject != null);
            //}
        }

        private ObjectInfo ToObjectInfo (UmlClass umlClass)
        {
            ObjectInfo objInfo = new ObjectInfo ();

            objInfo.Tag = umlClass;
            objInfo.Id = umlClass.Id;
            objInfo.Name = RenameObjectName (umlClass.Name);
            objInfo.Documentation = umlClass.Documentation;

            if (umlClass.StereoType.ToLower () == "choice")
            {
                objInfo.Type = ObjectInfoType.Object;
                objInfo.IsChoice = true;
            }
            else
                objInfo.Type = (ObjectInfoType) Enum.Parse (typeof (ObjectInfoType), umlClass.StereoType, true);
            
            if (objInfo.Type == ObjectInfoType.Feature)
                objInfo.IsUsed = true;

            if (objInfo.Type == ObjectInfoType.Codelist)
                objInfo.Namespace = "Aran.Aim.Enums";
            else
                objInfo.Namespace = RenameNamespace (umlClass.Namespace);

            objInfo.IsAbstract = umlClass.IsAbstract;

            return objInfo;
        }

        private string RenameNamespace (string text)
        {
            text = text.Replace ("Logical View.", "Aran.").
                Replace ("AIXM.AIXM ", "Aim.").
                Replace ("AIXM Data Types", "DataTypes").
                Replace ("Data Types", "DataTypes").
                Replace ("AIXM Features", "Features");

            if (text.StartsWith ("Aran.Aim.Features."))
                text = text.Substring (0, "Aran.Aim.Features".Length);
            return text;
        }

        private string RenameObjectName (string text)
        {
            if (text.EndsWith ("Type"))
                text = text.Substring (0, text.Length - 4);

            //if (text.EndsWith ("Base"))
            //    text = text.Substring (0, text.Length - 4);

            return text;
        }

        private ObjectInfo GetByName(string name)
        {
            return _objInfoList.Where(oi => oi.Name == name).FirstOrDefault();
        }

        private List<UmlClass> _umlClassList;
        private List<Association> _assocList;
        private List<string []> _replaceObjectInfo;
        private List<string []> _replaceNames;
        private List<string> _replacedPropertyNameList;
        private List<ObjectInfo> _objInfoList;
        private ObjectInfo _stringObjInfo;

        private ObjectInfo _guidObjectInfo = new ObjectInfo
        {
            Type = ObjectInfoType.XSDsimpleType,
            Name = "guid",
            Namespace = "Aran.XMLSchemaDatatypes"
        };
    }

    public static class Global
    {
        public static List<int> SearchProperty (List<ObjectInfo> objInfoList, string property /*, SearchCondition sc */)
        {
            List<int> indexList = new List<int> ();

            for (int i = 0; i < objInfoList.Count; i++)
            {
                if (IsPropertyExists (objInfoList [i], property))
                {
                    indexList.Add (i);
                }
            }

            return indexList;
        }

        public static bool IsPropertyExists (ObjectInfo objInfo, string property /*, SearchCondition sc */)
        {
            foreach (PropInfo pi in objInfo.Properties)
            {
                if (IsEqual (pi.Name.ToLower (), property))
                    return true;
            }
            return false;
        }

        public static bool IsEqual (string str1, string str2 /*, SearchCondition sc */)
        {
            return str1.Equals (str2);
        }

        public static List<ObjectInfo> MetadataObjects = new List<ObjectInfo> ();
    }

    public static class MyExtensions
    {
        public static void AddProperty (this ObjectInfo thisValue,
            string name, ObjectInfo propType, bool nullable = true, bool isList = false, string documentation = null)
        {
            thisValue.Properties.Add (new PropInfo (
                name, propType, nullable, isList, documentation));
        }

        public static void AddListProperty (this ObjectInfo thisValue,
            string name, ObjectInfo propType, string documentation = null)
        {
            thisValue.Properties.Add (new PropInfo (
                name, propType, true, true, documentation));
        }
    }
}
