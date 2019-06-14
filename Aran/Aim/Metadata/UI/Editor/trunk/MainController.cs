using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using System.Xml;
using System.IO;
using Aran.Aim.DataTypes;
using Aran.Aim.Metadata.UI;

namespace UIMetadatEditor
{
    public class MainController
    {
        public MainController ()
        {
            _uiMetadata = UIMetadata.Instance;
            IsChanged = false;
            _xmlDoc = new XmlDocument ();
        }

        public List<AimClassInfo> ClassInfoList
        {
            get { return _uiMetadata.ClassInfoList; }
        }

        public void SetCaption (AimClassInfo classInfo, string caption)
        {
            classInfo.UiInfo ().Caption = caption;
            IsChanged = true;
        }

        public void SetDependsFeature (AimClassInfo classInfo, string dependsFeature)
        {
            classInfo.UiInfo ().DependsFeature = dependsFeature;
            IsChanged = true;
        }

        public void Save (string fileName)
        {
            _xmlDoc.LoadXml ("<?xml version=\"1.0\" encoding=\"utf-8\"?><UIClassInfoList></UIClassInfoList>");

            foreach (AimClassInfo classInfo in _uiMetadata.ClassInfoList)
            {
                XmlElement xmlElem = _xmlDoc.CreateElement ("UIClassInfo");
                _xmlDoc.DocumentElement.AppendChild (xmlElem);

                FillUIClassInfoElement (classInfo, xmlElem);
            }

            if (File.Exists (fileName))
                File.Delete (fileName);

            _xmlDoc.Save (fileName);

            IsChanged = false;
        }

        public bool IsChanged { get; private set; }

        public void SetPropInfoShowGridView (AimPropInfo propInfo, bool value)
        {
            propInfo.UiPropInfo ().ShowGridView = value;
            IsChanged = true;
        }

        public void Test ()
        {
            //foreach (var classInfo in _uiMetadata.ClassInfoList)
            //{
            //    string s = "";
            //    int n = 0; ;

            //    for (int i = 0; i < classInfo.Properties.Count; i++)
            //    {
            //        var propInfo = classInfo.Properties [i];

            //        if (propInfo.UiPropInfo ().ShowGridView)
            //        {
            //            if (s.Length > 0)
            //                s += ", ";
            //            s += "<" + propInfo.Name + ">";
            //            n++;

            //            if (n == 2)
            //                break;
            //        }
            //    }

            //    if (s.Length > 0)
            //        classInfo.UiInfo ().DescriptionFormat = s;
            //}

            foreach (var classInfo in _uiMetadata.ClassInfoList)
            {
                if (classInfo.Parent != null)
                {
                    for (int i = 0; i < classInfo.Properties.Count; i++)
                    {
                        var propInfo = classInfo.Properties [i];
                        UIPropInfo uiPropInfo = propInfo.UiPropInfo ();

                        if (uiPropInfo.Caption == string.Empty && 
                            propInfo.Index != (int) Aran.Aim.PropertyEnum.PropertyDBEntity.Id &&
                            propInfo.Index != (int) Aran.Aim.PropertyEnum.PropertyFeature.Identifier &&
                            propInfo.Index != (int) Aran.Aim.PropertyEnum.PropertyFeature.TimeSlice)
                        {
                            uiPropInfo.Caption = ForDebug_MakeSentence (propInfo.Name);
                        }
                    }
                }
            }
        }

        private string ForDebug_MakeSentence (string propName)
        {
            if (propName == null)
                return null;

            if (propName.Length > 0 && char.IsLower (propName [0]))
            {
                propName = char.ToUpper (propName [0]) + propName.Substring (1);
            }

            for (int i = 1; i < propName.Length - 1; i++)
            {
                if (char.IsUpper (propName [i]) &&
                    (char.IsLower (propName [i - 1]) ||
                    char.IsLower (propName [i + 1])))
                {
                    propName = propName.Insert (i, " ");
                    i++;
                }
            }

            return propName;
        }

        private void FillUIClassInfoElement (AimClassInfo classInfo, XmlElement xmlElem)
        {
            UIClassInfo uiClassInfo = classInfo.UiInfo ();
            xmlElem.SetAttribute ("Name", classInfo.Name);
            xmlElem.SetAttribute ("AixmNamespace", uiClassInfo.AixmNamespace);
            xmlElem.SetAttribute ("DependFeature", uiClassInfo.DependsFeature);
            xmlElem.SetAttribute ("Caption", uiClassInfo.Caption);
            xmlElem.SetAttribute ("DescriptionFormat", uiClassInfo.DescriptionFormat);

            foreach (AimPropInfo propInfo in classInfo.Properties)
            {
                XmlElement propInfoElem = _xmlDoc.CreateElement ("UIPropInfo");
                xmlElem.AppendChild (propInfoElem);

                FillUIPropInfoElement (propInfo, propInfoElem);
            }
        }

        private void FillUIPropInfoElement (AimPropInfo propInfo, XmlElement propInfoElem)
        {
            UIPropInfo uiPropInfo = propInfo.UiPropInfo ();
            propInfoElem.SetAttribute ("Name", propInfo.Name);
            propInfoElem.SetAttribute ("Caption", uiPropInfo.Caption);
            propInfoElem.SetAttribute ("ShowGridView", uiPropInfo.ShowGridView.ToString ());
        }

        private UIMetadata _uiMetadata;
        private XmlDocument _xmlDoc;
    }

    public class DependsFeatureFinder
    {
        public DependsFeatureFinder (List<AimClassInfo> classInfoList)
        {
            _aimClassInfoList = classInfoList;
        }

        public string [] GetDependFeatureNames (AimClassInfo aimClassInfo, out int index)
        {
            List<string> strList = new List<string> ();

            if (aimClassInfo.AimObjectType == AimObjectType.Feature)
            {
                AddFeatureRef (aimClassInfo, strList, new List<AimClassInfo> ());
            }

            List<AimClassInfo> hasRefList = GetHasFeatureRefList (aimClassInfo);

            foreach (var item in hasRefList)
                strList.Add (item.Name);

            string [] strArr = strList.ToArray ();

            index = IsMatchingNamespace (strArr, aimClassInfo.UiInfo().AixmNamespace);

            return strArr;
        }

        private void AddFeatureRef (AimClassInfo aimClassInfo, List<string> featureRefPropList, List<AimClassInfo> addedList)
        {
            if (addedList.Contains (aimClassInfo))
                return;

            addedList.Add (aimClassInfo);

            foreach (var propInfo in aimClassInfo.Properties)
            {
                if (propInfo.IsFeatureReference)
                {
                    string featName = propInfo.ReferenceFeature.ToString ();

                    if (propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef)
                    {
                        featName = propInfo.PropType.Name;
                        featName = featName.Substring ("Abstract".Length, featName.Length - "AbstractRef".Length);
                    }

                    if (!featureRefPropList.Contains (featName))
                        featureRefPropList.Add (featName);
                }
                else if (propInfo.PropType.AimObjectType == AimObjectType.Object)
                {
                    AddFeatureRef (propInfo.PropType, featureRefPropList, addedList);
                }
            }
        }

        private List<AimClassInfo> GetHasFeatureRefList (AimClassInfo featureClasstInfo)
        {
            _searchedList = new Dictionary<AimClassInfo, bool> ();
            _featureClassInfo = featureClasstInfo;

            List<AimClassInfo> list = new List<AimClassInfo> ();

            foreach (var classInfo in _aimClassInfoList)
            {
                if (classInfo.Equals (featureClasstInfo) ||
                    classInfo.AimObjectType != AimObjectType.Feature)
                {
                    continue;
                }

                bool hasFRL = HasFeatureRefList (classInfo);
                if (hasFRL)
                    list.Add (classInfo);
            }

            return list;
        }

        private bool HasFeatureRefList (AimClassInfo objInfo, bool checkIsList = true)
        {
            if (_searchedList.ContainsKey (objInfo))
                return _searchedList [objInfo];

            _searchedList.Add (objInfo, false);

            bool rv = false;

            foreach (var propInfo in objInfo.Properties)
            {
                if (!propInfo.IsList && checkIsList)
                    continue;

                if (propInfo.IsFeatureReference)
                {
                    if ((int) propInfo.ReferenceFeature == 0)
                    {
                        string s = "Abstract" + AimMetadata.GetAimTypeName (_featureClassInfo.Index) + "Ref";

                        if (((DataType) propInfo.TypeIndex).ToString () == s)
                        {
                            rv = true;
                            break;
                        }
                    }
                    else if ((int) propInfo.ReferenceFeature == _featureClassInfo.Index)
                    {
                        rv = true;
                        break;
                    }
                }
                else if (propInfo.PropType.AimObjectType == AimObjectType.Object)
                {
                    bool hasFRL = HasFeatureRefList (propInfo.PropType, false);
                    if (hasFRL)
                    {
                        rv = true;
                        break;
                    }
                }
            }

            if (rv)
                _searchedList [objInfo] = true;

            return rv;
        }

        private int IsMatchingNamespace (string [] depedsFeatures, string aixmNamespace)
        {
            if (aixmNamespace == null)
                return -1;

            string [] nsArr = aixmNamespace.Split ('.');

            for (int i = nsArr.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < depedsFeatures.Length; j++)
                {
                    if (nsArr [i].StartsWith (depedsFeatures [j]))
                        return j;
                }
            }

            return -1;
        }

        private List<AimClassInfo> _aimClassInfoList;
        private Dictionary<AimClassInfo, bool> _searchedList;
        private AimClassInfo _featureClassInfo;
    }
}
