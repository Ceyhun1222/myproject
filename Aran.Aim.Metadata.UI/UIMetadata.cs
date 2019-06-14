#define NEW_2013_09

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.Metadata.UI
{
    public class UIMetadata
    {
        public static UIMetadata Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIMetadata ();
                }
                return _instance;
            }
        }

        private UIMetadata ()
        {
            _xmlElemDic = new Dictionary<AimClassInfo, XmlElement> ();
            _classInfoNameDict = new Dictionary<string, AimClassInfo> ();
        }

        public List<AimClassInfo> ClassInfoList
        {
            get
            {
                if (_classInfoList == null)
                {
                    LoadUIClassInfo ();
                }
                return _classInfoList;
            }
        }

        public AimClassInfo GetClassInfo (int aimTypeIndex)
        {
            return ClassInfoList.Where (ci => ci.Index == aimTypeIndex).FirstOrDefault ();
        }

        public void Save (bool reset = false)
        {
            if (_classInfoList == null)
                return;

            if (_modelXmlDoc == null)
            {
                if (reset)
                {
                    _modelXmlDoc = new XmlDocument ();
                    _modelXmlDoc.LoadXml ("<UIClassInfoList></UIClassInfoList>");
                }
                else
                {
                    return;
                }
            }

            foreach (AimClassInfo classInfo in _classInfoList)
            {
#if NEW_2013_09
                if (reset)
                    ClassInfoToXmlElement_Reset (classInfo);
                else
                    ClassInfoToXmlElement_new (classInfo);
#else
                ClassInfoToXmlElement (classInfo);
#endif
            }

            var fileName = GetFileName ();
            _modelXmlDoc.Save (fileName);
        }


        public static string GetGeoViewerApplicationDataDir ()
        {
            string dir = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
            dir = dir + "\\RISK\\GeoViewer";

            if (!System.IO.Directory.Exists (dir))
                System.IO.Directory.CreateDirectory (dir);

            return dir;
        }



        private string GetFileName ()
        {
            //string dir = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
            //dir = dir + "\\GeoViewer";

            //if (!System.IO.Directory.Exists (dir))
            //    System.IO.Directory.CreateDirectory (dir);

            return GetGeoViewerApplicationDataDir () + "\\AimUIModel.xml";
        }
        
        private void LoadUIClassInfo ()
        {
            _classInfoList = AimMetadata.AimClassInfoList;
            foreach (var ci in _classInfoList)
                _classInfoNameDict.Add (ci.Name, ci);

            _modelXmlDoc = new XmlDocument ();

            string fileName = GetFileName ();

            if (!System.IO.File.Exists (fileName))
            {
                string assemblyName = GlobalFunctions.GetAssebmlyFileName ();
                string dir = System.IO.Path.GetDirectoryName (assemblyName);
                fileName = dir + "\\AimUIModel.xml";

                if (!System.IO.File.Exists (fileName))
                {
                    throw new Exception ("Model config file not exists");
                }
            }

            try
            {
                _modelXmlDoc.Load (fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (XmlElement classInfoElem in _modelXmlDoc.DocumentElement)
            {
                var ciName = classInfoElem.Attributes [0].Value;
                AimClassInfo classInfo = GetClassInfo (ciName);
                if (classInfo != null)
                    _xmlElemDic.Add (classInfo, classInfoElem);
            }

            foreach (var item in _classInfoList)
            {
#if NEW_2013_09
                SetUIClassInfo_new (item);
#else
                SetUIClassInfo (item);
#endif
            }
        }

        private void ClassInfoToXmlElement (AimClassInfo aimClassInfo)
        {
            UIClassInfo uiClassInfo = aimClassInfo.UiInfo ();

            XmlElement xmlElem;
            if (_xmlElemDic != null && _xmlElemDic.TryGetValue (aimClassInfo, out xmlElem))
            {
            }
            else
            {
                xmlElem = _modelXmlDoc.CreateElement ("UIClassInfo");
                _modelXmlDoc.DocumentElement.AppendChild (xmlElem);
                xmlElem.SetAttribute ("Name", aimClassInfo.Name);
            }

            xmlElem.SetAttribute ("AixmNamespace", uiClassInfo.AixmNamespace);
            xmlElem.SetAttribute ("DependFeature", uiClassInfo.DependsFeature);
            xmlElem.SetAttribute ("Caption", uiClassInfo.Caption);
            xmlElem.SetAttribute ("DescriptionFormat", uiClassInfo.DescriptionFormat);

            foreach (AimPropInfo propInfo in aimClassInfo.Properties)
            {
                UIPropInfo uiPropInfo = propInfo.UiPropInfo ();

                XmlElement propInfoElement = null;

                for (int i = 0; i < xmlElem.ChildNodes.Count; i++)
                {
                    if (xmlElem.ChildNodes [i].Attributes [0].Value == propInfo.Name)
                    {
                        propInfoElement = xmlElem.ChildNodes [i] as XmlElement;
                        break;
                    }
                }

                if (propInfoElement == null)
                {
                    propInfoElement = _modelXmlDoc.CreateElement ("UIPropInfo");
                    xmlElem.AppendChild (propInfoElement);
                    propInfoElement.SetAttribute ("Name", propInfo.Name);
                }

                propInfoElement.SetAttribute ("Caption", uiPropInfo.Caption);
                propInfoElement.SetAttribute ("ShowGridView", uiPropInfo.ShowGridView.ToString ());

                propInfoElement.SetAttribute("Position", uiPropInfo.Position.ToString());
            }

        }


        private void ClassInfoToXmlElement_new (AimClassInfo aimClassInfo)
        {
            UIClassInfo uiClassInfo = aimClassInfo.UiInfo ();

            XmlElement classInfoElem;
            if (_xmlElemDic == null || !_xmlElemDic.TryGetValue (aimClassInfo, out classInfoElem))
            {
                return;
            }

            var propInfoListElem = classInfoElem ["PropInfoList"];
            if (propInfoListElem == null)
            {
                return;
            }

            foreach (AimPropInfo propInfo in aimClassInfo.Properties)
            {
                foreach (XmlElement elem in propInfoListElem.ChildNodes)
                {
                    if (elem.Attributes [0].Value == propInfo.Name)
                    {
                        UIPropInfo uiPropInfo = propInfo.UiPropInfo ();
                        elem.SetAttribute ("ShowGridView", uiPropInfo.ShowGridView.ToString ());
                        elem.SetAttribute("Position", uiPropInfo.Position.ToString());
                        break;
                    }
                }
            }
        }

        private void ClassInfoToXmlElement_Reset (AimClassInfo aimClassInfo)
        {
            UIClassInfo uiClassInfo = aimClassInfo.UiInfo ();

            XmlElement classInfoElem;
            if (_xmlElemDic != null && _xmlElemDic.TryGetValue (aimClassInfo, out classInfoElem))
            {
                classInfoElem.RemoveAll ();
            }
            else
            {
                classInfoElem = _modelXmlDoc.CreateElement ("UIClassInfo");
                _modelXmlDoc.DocumentElement.AppendChild (classInfoElem);
            }

            classInfoElem.SetAttribute ("Name", aimClassInfo.Name);
            classInfoElem.SetAttribute ("AixmNamespace", uiClassInfo.AixmNamespace);
            classInfoElem.SetAttribute ("DependFeature", uiClassInfo.DependsFeature);
            classInfoElem.SetAttribute ("Caption", uiClassInfo.Caption);
            classInfoElem.SetAttribute ("DescriptionFormat", uiClassInfo.DescriptionFormat);

            var propInfoListElem = classInfoElem ["PropInfoList"];
            if (propInfoListElem == null)
            {
                propInfoListElem = _modelXmlDoc.CreateElement ("PropInfoList");
                classInfoElem.AppendChild (propInfoListElem);
            }
            else
            {
                propInfoListElem.RemoveAll ();
            }

            foreach (AimPropInfo propInfo in aimClassInfo.Properties)
            {
                UIPropInfo uiPropInfo = propInfo.UiPropInfo ();
                XmlElement propInfoElem = _modelXmlDoc.CreateElement ("UIPropInfo");
                propInfoListElem.AppendChild (propInfoElem);
                propInfoElem.SetAttribute ("Name", propInfo.Name);
                propInfoElem.SetAttribute ("Caption", uiPropInfo.Caption);
                propInfoElem.SetAttribute ("ShowGridView", uiPropInfo.ShowGridView.ToString ());
                propInfoElem.SetAttribute("Position", uiPropInfo.Position.ToString());
            }

            if (aimClassInfo.AimObjectType == AimObjectType.Feature)
            {
                var refInfoListElem = classInfoElem ["RefInfoList"] as XmlElement;
                if (refInfoListElem == null)
                {
                    refInfoListElem = _modelXmlDoc.CreateElement ("RefInfoList");
                    classInfoElem.AppendChild (refInfoListElem);

                    foreach (var uiRefInfo in uiClassInfo.RefInfoList)
                    {
                        var refInfoElem = _modelXmlDoc.CreateElement ("RefInfo");
                        refInfoListElem.AppendChild (refInfoElem);

                        refInfoElem.SetAttribute ("ClassInfo", uiRefInfo.ClassInfo.Name);
                        refInfoElem.SetAttribute ("Directioin", uiRefInfo.Direction.ToString ());

                        foreach (var prPathInfo in uiRefInfo.PropertyPath)
                        {
                            var pathInfoElem = _modelXmlDoc.CreateElement ("Path");
                            refInfoElem.AppendChild (pathInfoElem);
                            pathInfoElem.SetAttribute ("Name", prPathInfo.Name);
                            pathInfoElem.SetAttribute ("IsList", prPathInfo.IsList.ToString ());
                            pathInfoElem.SetAttribute ("Status", prPathInfo.Status.ToString ());
                        }
                    }
                }
            }

        }

        private void SetUIClassInfo (AimClassInfo aimClassInfo)
        {
            UIClassInfo uiClassInfo = new UIClassInfo ();

            XmlElement xmlElem;
            XmlNodeList childNodes = null;

            if (_xmlElemDic != null)
            {
                if (_xmlElemDic.TryGetValue (aimClassInfo, out xmlElem))
                {
                    XmlAttribute attr = xmlElem.Attributes ["AixmNamespace"];
                    if (attr != null && attr.Value != null)
                        uiClassInfo.AixmNamespace = attr.Value;
                    else
                        uiClassInfo.AixmNamespace = "";

                    attr = xmlElem.Attributes ["DependFeature"];
                    if (attr != null && attr.Value != null)
                        uiClassInfo.DependsFeature = attr.Value;

                    attr = xmlElem.Attributes ["Caption"];
                    if (attr != null)
                    {
                        if (string.IsNullOrWhiteSpace (attr.Value))
                            uiClassInfo.Caption = MakeSentence (aimClassInfo.Name);
                        else
                            uiClassInfo.Caption = attr.Value;
                    }

                    attr = xmlElem.Attributes ["DescriptionFormat"];
                    if (attr != null && attr.Value != null)
                        uiClassInfo.DescriptionFormat = attr.Value;

                    childNodes = xmlElem.ChildNodes;
                }
            }

            aimClassInfo.Tag = uiClassInfo;

            if (childNodes == null)
            {
                foreach (AimPropInfo aimPropInfo in aimClassInfo.Properties)
                    aimPropInfo.Tag = new UIPropInfo ();
            }
            else
            {
                foreach (AimPropInfo aimPropInfo in aimClassInfo.Properties)
                    SetUIPropInfo (aimPropInfo, childNodes);
            }
        }

        private void SetUIClassInfo_new (AimClassInfo aimClassInfo)
        {
            UIClassInfo uiClassInfo = new UIClassInfo ();

            XmlElement classInfoElem = null;
            XmlNodeList propInfoListChildNodes = null;

            if (_xmlElemDic != null)
            {
                if (_xmlElemDic.TryGetValue (aimClassInfo, out classInfoElem))
                {
                    XmlAttribute attr = classInfoElem.Attributes ["AixmNamespace"];
                    if (attr != null && attr.Value != null)
                        uiClassInfo.AixmNamespace = attr.Value;
                    else
                        uiClassInfo.AixmNamespace = "";

                    attr = classInfoElem.Attributes ["DependFeature"];
                    if (attr != null && attr.Value != null)
                        uiClassInfo.DependsFeature = attr.Value;

                    attr = classInfoElem.Attributes ["Caption"];
                    if (attr != null)
                    {
                        if (string.IsNullOrWhiteSpace (attr.Value))
                            uiClassInfo.Caption = MakeSentence (aimClassInfo.Name);
                        else
                            uiClassInfo.Caption = attr.Value;
                    }

                    attr = classInfoElem.Attributes ["DescriptionFormat"];
                    if (attr != null && attr.Value != null)
                        uiClassInfo.DescriptionFormat = attr.Value;

                    var tmpElem = classInfoElem ["PropInfoList"];
                    if (tmpElem != null)
                        propInfoListChildNodes = tmpElem.ChildNodes;
                }
            }

            aimClassInfo.Tag = uiClassInfo;

            if (propInfoListChildNodes == null)
            {
                foreach (AimPropInfo aimPropInfo in aimClassInfo.Properties)
                    aimPropInfo.Tag = new UIPropInfo ();
            }
            else
            {
                foreach (AimPropInfo aimPropInfo in aimClassInfo.Properties)
                    SetUIPropInfo (aimPropInfo, propInfoListChildNodes);
            }

            string tmp;

            if (classInfoElem != null)
            {
                var refInfoListElem = classInfoElem ["RefInfoList"];
                if (refInfoListElem != null)
                {
                    foreach (XmlElement refInfoElem in refInfoListElem.ChildNodes)
                    {
                        var uiRefInfo = new UIReferenceInfo ();
                        var attr = refInfoElem.Attributes [0];
                        uiRefInfo.ClassInfo = GetClassInfo (attr.Value);

                        tmp = refInfoElem.Attributes [1].Value;
                        var dir = PropertyDirection.Sub;
                        if (Enum.TryParse<PropertyDirection> (tmp, out dir))
                            uiRefInfo.Direction = dir;
                        else
                            throw new Exception ("RefInfo is not correct: (Directioin Attribute)" + aimClassInfo.Name);

                        foreach (XmlElement pathInfoElem in refInfoElem.ChildNodes)
                        {
                            var uiPathInfo = new PropertyPathInfo ();
                            uiPathInfo.Name = pathInfoElem.Attributes [0].Value;
                            uiPathInfo.IsList = bool.Parse (pathInfoElem.Attributes [1].Value);
                            tmp = pathInfoElem.Attributes [2].Value;
                            var status = PropertyPathStatus.Normal;
                            if (Enum.TryParse<PropertyPathStatus> (tmp, out status))
                                uiPathInfo.Status = status;

                            uiRefInfo.PropertyPath.Add (uiPathInfo);
                        }

                        uiClassInfo.RefInfoList.Add (uiRefInfo);
                    }
                }
            }
        }

        private void SetUIPropInfo (AimPropInfo aimPropInfo, XmlNodeList nodeList)
        {
            UIPropInfo uiPropInfo = new UIPropInfo ();

            foreach (XmlElement xmlElem in nodeList)
            {
                if (xmlElem.Attributes [0].Value == aimPropInfo.Name)
                {
                    XmlAttribute attr = xmlElem.Attributes [1];
                    if (string.IsNullOrWhiteSpace (attr.Value))
                        uiPropInfo.Caption = MakeSentence (aimPropInfo.AixmName);
                    else
                        uiPropInfo.Caption = attr.Value;

                    attr = xmlElem.Attributes [2];
                    if (attr.Value != null)
                        uiPropInfo.ShowGridView = bool.Parse (attr.Value);

                    attr = xmlElem.Attributes["Position"];
                    if (attr != null && attr.Value!=null)
                    {
                        uiPropInfo.Position = int.Parse(attr.Value);
                    }
                }
            }

            aimPropInfo.Tag = uiPropInfo;
        }

        private string MakeSentence (string propName)
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

        private AimClassInfo GetClassInfo (string name)
        {
            return _classInfoNameDict [name];
        }

        private List<AimClassInfo> _classInfoList;
        private Dictionary<string, AimClassInfo> _classInfoNameDict;
        private XmlDocument _modelXmlDoc;
        private Dictionary<AimClassInfo, XmlElement> _xmlElemDic;
        private static UIMetadata _instance;
    }
}
