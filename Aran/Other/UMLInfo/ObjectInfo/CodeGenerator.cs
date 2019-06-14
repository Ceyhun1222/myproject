using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UMLInfo.Classes;

namespace UMLInfo
{
    public class CodeGenerator
    {
        private List<CustomEnumFields> customEnumFields;

        public CodeGenerator (List<ObjectInfo> objInfoList)
        {
            _errors = new List<string> ();
            _objInfoList = objInfoList;
            _featureRefObjInfo = objInfoList.Where (oi => oi.Name == "FeatureRef").First ();
            _featureRefObjectObjInfo = objInfoList.Where (oi => oi.Name == "FeatureRefObject").First ();
            GenerateDataTypes = true;
        }

        public List<string> Errors
        {
            get { return _errors; }
        }

        public string CodeFolder { get; set; }

        public bool GenerateDataTypes { get; set; }

        public void GenerateEnums ()
        {
            List<ObjectInfo> usedEnums = GetUsedEnums ();

            var deserializer = new DeserializerBuilder()
             .WithNamingConvention(new CamelCaseNamingConvention())
             .Build();

            customEnumFields = deserializer.Deserialize<List<CustomEnumFields>>(File.OpenText(@"../../Files/CustomFields.yaml"));

            List<string> csLines = new List<string> ();
            csLines.Add ("namespace Aran.Aim.Enums");
            csLines.Add ("{");
            foreach (ObjectInfo objInfo in usedEnums)
            {
                csLines.AddRange (GenerateEnum (objInfo));
            }
            csLines.Add ("}");

            WriteFile (csLines, "Enum", "Enums.cs");
        }

        public void CreateAddFill_aran_ENUMS ()
        {
            List<ObjectInfo> usedEnums = GetUsedEnums ();

            List<string> dbLines = new List<string> ();
            dbLines.Add ("DROP TABLE IF EXISTS _aran_ENUMS");
            dbLines.Add ("");
            dbLines.Add ("CREATE TABLE _aran_enums (enum_name text, value int, name text, alias text, documentation text)");
            dbLines.Add ("");

            foreach (ObjectInfo objInfo in usedEnums)
            {
                dbLines.AddRange (GenerateInsertIntoEnum (objInfo));
            }

            string dbCode = "";
            foreach (string dbLine in dbLines)
                dbCode += dbLine + ";\r\n";
        }

        public void CreateClasses ()
        {
            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (objInfo.IsUsed &&
                    (objInfo.Type == ObjectInfoType.Feature ||
                    objInfo.Type == ObjectInfoType.Object ||
                    (GenerateDataTypes && objInfo.Type == ObjectInfoType.Datatype)))
                {

                    try
                    {
                        List<string> lines = CreateClass (objInfo);
                        WriteFile (lines, objInfo);
                    }
                    catch (Exception ex)
                    {
                        _errors.Add (ex.Message);
                    }
                }
            }
        }

        public List<string> CreateClass (ObjectInfo objInfo)
        {
            List<string> lines = new List<string> ();

            if (!objInfo.IsUsed)
                return lines;

            List<string> namespaceList = new List<string> ();

            namespaceList.Add ("System");
            namespaceList.Add ("System.Collections.Generic");
            namespaceList.Add ("Aran.Aim");
            namespaceList.Add ("Aran.Aim.PropertyEnum");

            if (objInfo.Name == "TextNote")
                namespaceList.Add ("Aran.Aim.Enums");

            lines.Add ("namespace " + objInfo.Namespace);

            //--- namespace

            lines.Add ("{");
            {
                string baseEnumType;
                string namespaceText;
                string parentClassText = GetParentClassText (objInfo, out baseEnumType, out namespaceText);

                bool isPartial = false;

                if (objInfo.Name.StartsWith ("Elevated") ||
                    objInfo.Name == "AixmPoint" ||
                    objInfo.Name == "Curve" ||
                    objInfo.Name == "Surface")
                {
                    isPartial = true;
                }

                lines.Add (string.Format ("public {0}{3}class {1}{2}",
                    (objInfo.IsAbstract ? "abstract " : ""),
                    objInfo.Name,
                    parentClassText,
                    (isPartial ? "partial " : "")));

                AddNamespace (namespaceList, namespaceText);

                //--- class

                lines.Add ("{");
                {
                    if (objInfo.IsAbstract)
                    {
                        lines.Add (string.Format ("public virtual {0}Type {0}Type ", objInfo.Name));
                        lines.Add ("{");
                        {
                            lines.Add (string.Format ("get {{ return ({0}Type) {1}; }}", objInfo.Name,
                                (objInfo.Type == ObjectInfoType.Feature ? "FeatureType" : "ObjectType")));
                        }
                        lines.Add ("}");
                        lines.Add ("");
                    }
                    else if (baseEnumType != null)
                    {
                        #region Abstract<Feature> Constructor
                        if (objInfo.Type == ObjectInfoType.Datatype && objInfo.Name.StartsWith ("Abstract"))
                        {
                            string absType = objInfo.Name.Substring (8);
                            char c = absType [0];
                            string absTypeParamName = char.ToLower (c) + absType.Substring (1);
                            lines.Add (string.Format ("public {0} ()", objInfo.Name));
                            lines.Add ("{");
                            lines.Add ("}");
                            lines.Add ("");

                            lines.Add (string.Format ("public {0} ({1}Type {2}, FeatureRef feature)",
                                objInfo.Name, absType, absTypeParamName));
                            lines.Add (" : base (" + absTypeParamName + ", feature)");
                            lines.Add ("{");
                            lines.Add ("}");
                            lines.Add ("");
                        }
                        #endregion

                        lines.Add (string.Format ("public override {0} {0}", baseEnumType));
                        lines.Add ("{");
                        {
                            lines.Add (string.Format ("get {{ return {0}.{1}; }}", baseEnumType, objInfo.Name));
                        }
                        lines.Add ("}");
                        lines.Add ("");
                    }


                    if (objInfo.IsChoice)
                    {
                        lines.Add (string.Format ("public {0}Choice Choice", objInfo.Name));
                        lines.Add ("{");
                        {
                            lines.Add (string.Format ("get {{ return ({0}Choice) RefType; }}", objInfo.Name));
                        }
                        lines.Add ("}");
                        lines.Add ("");

                        CreateChoicePropertyLines (objInfo, lines, namespaceList);
                    }
                    else
                        CreatePropertyLines (objInfo, lines, namespaceList);
                }

                lines.Add ("}");
            }
            lines.Add ("}");


            namespaceList.Remove ("Aran.XMLSchemaDatatypes");

            for (int i = 0; i < namespaceList.Count; i++)
                namespaceList [i] = "using " + namespaceList [i] + ";";


            namespaceList.Add ("");
            lines.InsertRange (0, namespaceList);

            if (objInfo.Name != "AixmPoint" &&
                objInfo.Name != "Curve" &&
                objInfo.Name != "Surface")
            {
                lines.Add ("");
                lines.Add ("namespace Aran.Aim.PropertyEnum");
                lines.Add ("{");
                {
                    lines.AddRange (CreatePropertyEnum (objInfo));
                    lines.Add ("");
                    lines.AddRange (CreateMetadataClass (objInfo));
                }
                lines.Add ("}");
            }

            return lines;
        }

        private List<string> CreateMetadataClass (ObjectInfo objInfo)
        {
            List<string> lines = new List<string> ();

            string baseClassName = "";

            if (objInfo.Base != null)
                baseClassName = objInfo.Base.Name;
            else if (objInfo.Type == ObjectInfoType.Object)
                baseClassName = "AObject";
            else if (objInfo.Type == ObjectInfoType.Feature)
                baseClassName = "Feature";
            else
                baseClassName = "AimObject";

            string className = "Metadata" + objInfo.Name;
            baseClassName = "Metadata" + baseClassName;

            lines.Add ("public static class " + className);
            lines.Add ("{");
            lines.Add ("public static AimPropInfoList PropInfoList;");
            lines.Add ("");
            lines.Add ("static " + className + " ()");
            lines.Add ("{");
            lines.Add ("PropInfoList = " + baseClassName + ".PropInfoList.Clone ();");
            lines.Add ("");

            foreach (PropInfo propInfo in objInfo.Properties)
            {
                if (propInfo.Name == "Annotation")
                {
                    lines.Add ("PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);");
                }
                else
                {
                    string propTypeCharacter;
                    string aixmName;

                    string propTypeIndexEnumName = GetPropertyTypeIndexEnumName (propInfo, out propTypeCharacter, out aixmName);

                    if (propTypeCharacter != null)
                        propTypeCharacter = ", " + propTypeCharacter;

                    if (aixmName != null)
                        aixmName = ", \"" + aixmName + "\"";

                    lines.Add (string.Format ("PropInfoList.Add (Property{1}.{0}, (int) {2}{3}{4});",
                        propInfo.Name,
                        objInfo.Name,
                        propTypeIndexEnumName,
                        propTypeCharacter,
                        aixmName));
                }
            }

            if (objInfo.Type == ObjectInfoType.Feature && !objInfo.IsAbstract)
                lines.Add("PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);");

            lines.Add ("}");
            lines.Add ("}");
            return lines;
        }

        public void WriteFile (List<string> lines, ObjectInfo objInfo)
        {
            string subDir = objInfo.SubDir;

            if (string.IsNullOrEmpty(subDir))
            {

                if (Global.MetadataObjects.Contains(objInfo))
                {
                    subDir = "Metadata";
                }
                else
                {
                    switch (objInfo.Type)
                    {
                        case ObjectInfoType.Feature:
                            subDir = "Feature";
                            break;
                        case ObjectInfoType.Object:
                            subDir = "Object";
                            break;
                        case ObjectInfoType.Codelist:
                            subDir = "Enum";
                            break;
                        case ObjectInfoType.Datatype:
                            subDir = "DataType";
                            break;
                        default:
                            throw new Exception(objInfo.Type + " not supproted to create code file.");
                    }
                }
            }

            WriteFile (lines, subDir, objInfo.Name + ".cs");
        }

        public List<string> CreateEnum_AllObjectType ()
        {
            List<string> lines = new List<string> ();

            List<string> dataTypeLines = new List<string> ();
            List<string> choiceObjectsLines = new List<string> ();
            List<string> nonChoiceObjectsLines = new List<string> ();
            List<string> featuresLines = new List<string> ();
            List<string> abstractFeatureLines = new List<string> ();
            List<string> abstractObjectLines = new List<string> ();
            List<string> enumLines = new List<string> ();

            List<string> tempLines;

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (!objInfo.IsUsed)
                    continue;

                if (objInfo.IsAbstract)
                {
                    if (objInfo.Type == ObjectInfoType.Feature)
                        abstractFeatureLines.Add (objInfo.Name);
                    else
                        abstractObjectLines.Add (objInfo.Name);
                }
                else
                {
                    switch (objInfo.Type)
                    {
                        case ObjectInfoType.Datatype:
                            dataTypeLines.Add (objInfo.Name);
                            break;
                        case ObjectInfoType.Object:
                            {
                                if (objInfo.IsChoice)
                                    choiceObjectsLines.Add (objInfo.Name);
                                else
                                    nonChoiceObjectsLines.Add (objInfo.Name);
                            }
                            break;
                        case ObjectInfoType.Feature:
                            featuresLines.Add (objInfo.Name);
                            break;
                        case ObjectInfoType.Codelist:
                            enumLines.Add (objInfo.Name);
                            break;
                        default:
                            break;
                    }
                }
            }

            enumLines.Add ("Language");

            //#region Add _VALCLASS BLOCK
            #region Add _VALCLASS and _ABSTRACTCLASS BLOCK
            dataTypeLines.Sort ();
            dataTypeLines.Reverse ();
            bool isBegin = false;

            for (int i = 0; i < dataTypeLines.Count; i++)
            {
                if (!isBegin && dataTypeLines [i].StartsWith ("Val"))
                {
                    dataTypeLines.Insert (i, "_2_1_VALCLASS_BEGIN");
                    isBegin = true;
                }
                else if (isBegin && !dataTypeLines [i].StartsWith ("Val"))
                {
                    dataTypeLines.Insert (i, "_2_1_VALCLASS_END");
                    break;
                }

            }

            isBegin = false;
            for (int i = 0; i < dataTypeLines.Count; i++)
            {
                if (!isBegin && dataTypeLines [i].StartsWith ("Abstract"))
                {
                    dataTypeLines.Insert (i, "_2_2_ABSTRACTCLASS_BEGIN");
                    isBegin = true;
                }
                else if (isBegin && !dataTypeLines [i].StartsWith ("Abstract"))
                {
                    dataTypeLines.Insert (i, "_2_2_ABSTRACTCLASS_END");
                    break;
                }
            }

            if (isBegin)
                dataTypeLines.Add ("_2_2_ABSTRACTCLASS_END");
            #endregion

            #region EnumType

            foreach (var objInfo in _objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Datatype &&
                    objInfo.Name.StartsWith ("Abstract") &&
                    objInfo.Name.EndsWith ("Ref"))
                {
                    string enumItemName = objInfo.Name.Substring (8, objInfo.Name.Length - 11);
                    enumItemName += "Type";
                    enumLines.Add (enumItemName);
                }
            }

            tempLines = new List<string> ();
            tempLines.Add ("public enum EnumType");
            tempLines.Add ("{");
            foreach (string line in enumLines)
            {
                tempLines.Add (line + " = AllAimObjectType." + line);
            }
            tempLines.Add ("}");

            for (int i = 2; i < tempLines.Count - 2; i++)
            {
                tempLines [i] = tempLines [i] + ",";
            }
            List<string> lines_Enum = tempLines;

            #endregion

            #region AllObjectType

            string seperaterLine = "//****************************   {0}  **********************************";

            List<string> lines_AllObjectType = new List<string> ();
            lines_AllObjectType.Add ("internal enum AllAimObjectType");
            lines_AllObjectType.Add ("{");
            {
                #region Custom lines

                int n = 1;
                lines_AllObjectType.Add ("#region FieldType");
                lines_AllObjectType.Add (string.Format (seperaterLine, "FIELDTYPE"));
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("_1_FIELDTYPE = " + (n++));
                lines_AllObjectType.Add ("SysGuid = " + (n++));
                lines_AllObjectType.Add ("SysDouble = " + (n++));
                lines_AllObjectType.Add ("SysString = " + (n++));
                lines_AllObjectType.Add ("SysUInt32 = " + (n++));
                lines_AllObjectType.Add ("SysInt32 = " + (n++));
                lines_AllObjectType.Add ("SysInt64 = " + (n++));
                lines_AllObjectType.Add ("SysBool = " + (n++));
                lines_AllObjectType.Add ("SysDateTime = " + (n++));
                lines_AllObjectType.Add ("SysEnum = " + (n++));
                n = 128;
                //--- Geo
                lines_AllObjectType.Add ("GeoPoint = " + (n++));
                lines_AllObjectType.Add ("GeoPolyline = " + (n++));
                lines_AllObjectType.Add ("GeoPolygon = " + (n++));
                lines_AllObjectType.Add ("");

                #endregion

                lines_AllObjectType.Add ("#endregion");
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("#region DataType");
                lines_AllObjectType.Add (string.Format (seperaterLine, "DATATYPES"));
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("_2_DATATYPE = 512");
                for (int i = 0; i < dataTypeLines.Count; i++)
                    lines_AllObjectType.Add (dataTypeLines [i] + " = " + (i + 512 + 1));

                lines_AllObjectType.Add ("#endregion");
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("#region Object");
                lines_AllObjectType.Add (string.Format (seperaterLine, "OBJECTS"));
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("_3_OBJECT = 1024");
                for (int i = 0; i < nonChoiceObjectsLines.Count; i++)
                    lines_AllObjectType.Add (nonChoiceObjectsLines [i] + " = " + (i + 1024 + 1));
                lines_AllObjectType.Add ("_3_1_CHOICE_OBJECT = 1408");
                for (int i = 0; i < choiceObjectsLines.Count; i++)
                    lines_AllObjectType.Add (choiceObjectsLines [i] + " = " + (i + 1408 + 1));


                lines_AllObjectType.Add ("#endregion");
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("#region Feature");
                lines_AllObjectType.Add (string.Format (seperaterLine, "FEATURE"));
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("_4_FEATURE = 1536");
                for (int i = 0; i < featuresLines.Count; i++)
                    lines_AllObjectType.Add (featuresLines [i] + " = " + (i + 1536 + 1));

                lines_AllObjectType.Add ("#endregion");
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("#region Abstract");
                lines_AllObjectType.Add (string.Format (seperaterLine, "ABSTRACT"));
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("_5_ABSTRACT = 2048");
                for (int i = 0; i < abstractObjectLines.Count; i++)
                    lines_AllObjectType.Add (abstractObjectLines [i] + " = " + (i + 2048 + 1));
                lines_AllObjectType.Add ("_5_1_ABSTRACT_FEATURE = 2304");
                for (int i = 0; i < abstractFeatureLines.Count; i++)
                    lines_AllObjectType.Add (abstractFeatureLines [i] + " = " + (i + 2304 + 1));

                lines_AllObjectType.Add ("#endregion");
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("#region Enum");
                lines_AllObjectType.Add (string.Format (seperaterLine, "ENUMS"));
                lines_AllObjectType.Add ("");
                lines_AllObjectType.Add ("_6_ENUM = 2560");
                for (int i = 0; i < enumLines.Count; i++)
                    lines_AllObjectType.Add (enumLines [i] + " = " + (i + 2560 + 1));
                lines_AllObjectType.Add ("#endregion");
            }
            lines_AllObjectType.Add ("}");

            tempLines = lines_AllObjectType;
            for (int i = 2; i < tempLines.Count - 2; i++)
            {
                if (tempLines [i].Trim ().Length > 0 &&
                    !tempLines [i].StartsWith ("//"))
                {
                    tempLines [i] = tempLines [i] + ",";
                }
            }

            #endregion

            #region DataType

            tempLines = new List<string> ();
            tempLines.Add ("public enum DataType");
            tempLines.Add ("{");
            foreach (string line in dataTypeLines)
            {
                if (line [0] != '_')
                    tempLines.Add (line + " = AllAimObjectType." + line);
            }
            tempLines.Add ("}");

            for (int i = 2; i < tempLines.Count - 2; i++)
            {
                tempLines [i] = tempLines [i] + ",";
            }
            List<string> lines_DateType = tempLines;

            #endregion

            #region ObjectType

            tempLines = new List<string> ();
            tempLines.Add ("public enum ObjectType");
            tempLines.Add ("{");
            foreach (string line in nonChoiceObjectsLines)
            {
                tempLines.Add (line + " = AllAimObjectType." + line);
            }
            foreach (string line in choiceObjectsLines)
            {
                tempLines.Add (line + " = AllAimObjectType." + line);
            }
            tempLines.Add ("}");

            for (int i = 2; i < tempLines.Count - 2; i++)
            {
                tempLines [i] = tempLines [i] + ",";
            }
            List<string> lines_ObjectType = tempLines;

            #endregion

            #region FeatureType

            tempLines = new List<string> ();
            tempLines.Add ("public enum FeatureType");
            tempLines.Add ("{");
            foreach (string line in featuresLines)
            {
                tempLines.Add (line + " = AllAimObjectType." + line);
            }
            tempLines.Add ("}");

            for (int i = 2; i < tempLines.Count - 2; i++)
            {
                tempLines [i] = tempLines [i] + ",";
            }
            List<string> lines_FeatureType = tempLines;

            #endregion

            #region AbstractType

            tempLines = new List<string> ();
            tempLines.Add ("public enum AbstractType");
            tempLines.Add ("{");
            foreach (string line in abstractObjectLines)
            {
                tempLines.Add (line + " = AllAimObjectType." + line);
            }
            foreach (string line in abstractFeatureLines)
            {
                tempLines.Add (line + " = AllAimObjectType." + line);
            }
            tempLines.Add ("}");

            for (int i = 2; i < tempLines.Count - 2; i++)
            {
                tempLines [i] = tempLines [i] + ",";
            }
            List<string> lines_AbstractType = tempLines;

            #endregion


            lines.Add ("using System;");
            lines.Add ("");
            lines.Add ("namespace Aran.Aim");
            lines.Add ("{");
            {
                lines.AddRange (lines_AllObjectType);
                lines.Add ("");
                lines.AddRange (lines_DateType);
                lines.Add ("");
                lines.AddRange (lines_ObjectType);
                lines.Add ("");
                lines.AddRange (lines_FeatureType);
                lines.Add ("");
                lines.AddRange (lines_AbstractType);
                lines.Add ("");
                lines.AddRange (lines_Enum);

                #region Create Absract Types Enum

                foreach (var objInfo in _objInfoList)
                {
                    if ((objInfo.IsAbstract || objInfo.IsChoice) &&
                        objInfo.Name != "Feature")
                    {
                        string text_TypeOrChoice;
                        List<string> childNames = new List<string> ();

                        if (objInfo.IsAbstract)
                        {
                            text_TypeOrChoice = "Type";

                            childNames.AddRange (GetChildNames (objInfo));

                            ////foreach (ObjectInfo oi in _objInfoList)
                            ////{
                            ////    if (oi.Base == objInfo && oi.IsUsed)
                            ////    {
                            ////        childNames.Add (oi.Name);
                            ////    }
                            ////}

                            //childNames.AddRange (
                            //    _objInfoList.Where (oi => oi.Base == objInfo && oi.IsUsed).Select (oi => oi.Name));
                        }
                        else
                        {
                            text_TypeOrChoice = "Choice";
                            foreach (PropInfo propInfo in objInfo.Properties)
                            {
                                string s = propInfo.PropType.Name;
                                childNames.Add (s);
                            }
                        }

                        List<string> abstractTypesLines = new List<string> ();
                        abstractTypesLines.Add ("public enum " + objInfo.Name + text_TypeOrChoice);
                        abstractTypesLines.Add ("{");
                        for (int i = 0; i < childNames.Count; i++)
                        {
                            abstractTypesLines.Add (string.Format ("{0} = AllAimObjectType.{0}{1}",
                                childNames [i],
                                (i < childNames.Count - 1 ? "," : "")));
                        }
                        abstractTypesLines.Add ("}");

                        lines.Add ("");
                        lines.AddRange (abstractTypesLines);
                    }
                }

                #endregion

            }
            lines.Add ("}");


            WriteFile (lines, "Enum", "AllAimObjectType.cs");

            return lines;
        }

        public List<string> CreateFactoryFunc ()
        {
            List<string> lines = new List<string> ();

            lines.Add ("using System;");
            lines.Add ("using System.Collections;");
            lines.Add ("using System.Collections.Generic;");
            lines.Add ("using Aran.Aim.DataTypes;");
            lines.Add ("using Aran.Aim.Features;");
            lines.Add ("using Aran.Aim.Objects;");
            lines.Add ("using Aran.Package;");
            lines.Add ("using Aran.Aim.Enums;");
            lines.Add ("using Aran.Aim.Metadata;");
            lines.Add ("using Aran.Aim.Metadata.ISO;");
            lines.Add ("");
            lines.Add ("namespace Aran.Aim");
            lines.Add ("{");
            lines.Add ("public static partial class AimObjectFactory");
            lines.Add ("{");

            string [] enumTypeName = new string [] { "DataType", "ObjectType", "FeatureType", "AbstractType", "EnumType" };
            string [] typeNames = new string [] { "ADataType", "AObject", "Feature", "Abstract", "EnumType" };
            string [] returnTextFormat = new string [] { "{0}", "{0}", "{0}", "{0}", "AimField <{0}>" };

            List<string> [] funcListArr = new List<string> [enumTypeName.Length];
            List<string> [] listFuncListArr = new List<string> [enumTypeName.Length];

            for (int i = 0; i < enumTypeName.Length; i++)
            {
                funcListArr [i] = new List<string> ();

                if (i != 3)
                {
                    funcListArr [i] = new List<string> ();
                    funcListArr [i].Add (string.Format ("public static {0} Create{2} ({1} a{1})",
                        (i != 4 ? typeNames [i] : "AimField"), enumTypeName [i], typeNames [i]));
                    funcListArr [i].Add ("{");
                    funcListArr [i].Add (string.Format ("switch (a{0})", enumTypeName [i]));
                    funcListArr [i].Add ("{");
                }

                listFuncListArr [i] = new List<string> ();
                listFuncListArr [i].Add (string.Format ("public static IList Create{1}List ({0} a{0})",
                    enumTypeName [i], typeNames [i]));
                listFuncListArr [i].Add ("{");
                listFuncListArr [i].Add (string.Format ("switch (a{0})", enumTypeName [i]));
                listFuncListArr [i].Add ("{");
            }

            string temp;
            int index;
            string listClassName;

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (!objInfo.IsUsed)
                    continue;

                if (objInfo.IsAbstract)
                    index = 3;
                else if (objInfo.Type == ObjectInfoType.Datatype)
                    index = 0;
                else if (objInfo.Type == ObjectInfoType.Object)
                    index = 1;
                else if (objInfo.Type == ObjectInfoType.Feature)
                    index = 2;
                else if (objInfo.Type == ObjectInfoType.Codelist)
                    index = 4;
                else
                    index = -1;

                if (index != -1)
                {
                    temp = string.Format ("case {0}." + objInfo.Name + ":", enumTypeName [index]);

                    string objiInfoName = objInfo.Name;

                    if (index != 3)
                    {
                        funcListArr [index].Add (temp);

                        int k = -1;
                        if ((k = objiInfoName.IndexOf ('_')) != -1)
                        {
                            objiInfoName = string.Format ("{0}<{1}>", objiInfoName.Substring (0, k), objiInfoName.Substring (k + 1));
                        }

                        funcListArr [index].Add ("return new " + string.Format (returnTextFormat [index], objiInfoName) + " ();");
                    }

                    listClassName = (index == 1 ? "AObjectList" : "List");

                    listFuncListArr [index].Add (temp);
                    listFuncListArr [index].Add ("return new " + listClassName +
                        " <" + string.Format (returnTextFormat [index], objiInfoName) + "> ();");
                }
            }

            foreach (var objInfo in _objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Datatype &&
                    objInfo.Name.StartsWith ("Abstract") &&
                    objInfo.Name.EndsWith ("Ref"))
                {
                    string s = objInfo.Name.Substring (8, objInfo.Name.Length - 11);
                    s += "Type";

                    funcListArr [4].Add ("case EnumType." + s + ":");
                    funcListArr [4].Add ("return new AimField<int> ();");
                }
            }

            //funcListArr [4]

            for (int i = 0; i < enumTypeName.Length; i++)
            {
                if (i != 3)
                {
                    funcListArr [i].Add ("default:");
                    funcListArr [i].Add (string.Format (
                        "throw new Exception (\"Create {0} is not supported for type: \" + a{0});", enumTypeName [i]));
                    funcListArr [i].Add ("}");
                    funcListArr [i].Add ("}");
                }

                listFuncListArr [i].Add ("default:");
                listFuncListArr [i].Add (string.Format (
                    "throw new Exception (\"Create {0} List is not supported for type: \" + a{0});", enumTypeName [i]));
                listFuncListArr [i].Add ("}");
                listFuncListArr [i].Add ("}");
            }

            for (int i = 0; i < enumTypeName.Length; i++)
            {
                lines.AddRange (funcListArr [i]);
                lines.Add ("");
                lines.AddRange (listFuncListArr [i]);
                lines.Add ("");
            }

            lines.Add ("}");
            lines.Add ("}");

            WriteFile (lines, "Common", "AimObjectFactory_gen.cs");

            return lines;
        }

        public void CreateExtensionClass ()
        {
            string temp = CodeFolder;
            CodeFolder += "\\Extension";

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (objInfo.IsUsed &&
                    (objInfo.Type == ObjectInfoType.Object ||
                    objInfo.Type == ObjectInfoType.Feature))
                {
                    List<string> lines = CreateExtensionClass (objInfo);

                    if (lines.Count > 0)
                    {
                        WriteFile (lines, objInfo.Type.ToString (), objInfo.Name + "Extension.cs");
                    }
                }
            }

            CodeFolder = temp;
        }


        #region CreateTable

        public List<string> Create_RelationTables ()
        {
            List<string> lines = new List<string> ();

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (objInfo.IsUsed)
                {
                    foreach (var propInfo in objInfo.Properties)
                    {
                        if (propInfo.IsList &&
                            propInfo.PropType.Type == ObjectInfoType.Feature &&
                            !propInfo.Cardinality.IsEmpty &&
                            propInfo.Cardinality.Max == short.MaxValue &&
                            !propInfo.OtherCardinality.IsEmpty &&
                            propInfo.OtherCardinality.Max == short.MaxValue)
                        {
                            string dataType1 = (objInfo.IsAbstract ? "\"_d_AbstractType\"" : "bigint");
                            string dataType2 = (propInfo.PropType.IsAbstract ? "\"_d_AbstractType\"" : "bigint");

                            lines.Add (string.Format ("CREATE TABLE \"_r_{0}_{1}\" (",
                                objInfo.Name, propInfo.PropType.Name));
                            lines.Add ("\"" + objInfo.Name + "Id\" " + dataType1 + " NOT NULL,");
                            lines.Add ("\"" + propInfo.PropType.Name + "Id\" " + dataType2 + " NOT NULL");
                            lines.Add (");");
                            lines.Add ("");
                        }
                    }
                }
            }

            return lines;
        }

        public List<string> CreateTable_FeatureOrObject ()
        {
            Dictionary<ObjectInfo, List<string []>> tables = new Dictionary<ObjectInfo, List<string []>> ();

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (objInfo.Name != "Runway")
                    continue;

                if (objInfo.IsUsed &&
                    (objInfo.Type == ObjectInfoType.Feature ||
                    objInfo.Type == ObjectInfoType.Object))
                {
                    List<string []> fieldsList = new List<string []> ();
                    GetTableFields (objInfo, fieldsList);

                    tables.Add (objInfo, fieldsList);
                }
            }

            List<string> lines = new List<string> ();

            foreach (var item in tables)
            {
                string tableName;
                if (item.Key.Type == ObjectInfoType.Feature)
                    tableName = item.Key.Name;
                else
                    tableName = "_" + item.Key.Name;

                lines.Add ("CREATE TABLE \"" + tableName + "\"");
                lines.Add ("(");
                lines.Add ("\"Id\" bigserial PRIMARY KEY,");

                foreach (string [] fieldText in item.Value)
                {
                    lines.Add ("\"" + fieldText [0] + "\" " + fieldText [1] + ",");
                }

                lines.Add ("\"Annotation\" text,");

                if (item.Key.Type == ObjectInfoType.Feature)
                {
                    lines.Add ("\"TimeSlice\" \"_d_TimeSlice\"");
                }
                else
                {
                    lines.Add ("ref_id bigint,");
                    lines.Add ("ref_type integer,");
                    lines.Add ("ref_property integer");
                }

                lines.Add (");");
                lines.Add ("");
            }

            return lines;
        }

        private void GetTableFields (ObjectInfo objInfo, List<string []> list)
        {
            foreach (var propInfo in objInfo.GetAllProperties ())
            {
                if (propInfo.IsList)
                {
                }
                else
                {
                    ObjectInfo propObjInfo = propInfo.PropType;
                    string propName = propInfo.Name;
                    string dataType;

                    switch (propObjInfo.Type)
                    {
                        case ObjectInfoType.Feature:
                            {
                                if (propObjInfo.IsAbstract)
                                    dataType = "\"_d_AbstractType\"";
                                else
                                {
                                    dataType = "bigint";
                                    //propName = AddIdToText (propName);
                                }
                            }
                            break;
                        case ObjectInfoType.Codelist:
                            dataType = "int";
                            break;
                        case ObjectInfoType.Datatype:
                            {
                                dataType = "\"_d_ValClass\"";
                                if (!propObjInfo.Name.StartsWith ("Val"))
                                {
                                }
                            }
                            break;
                        case ObjectInfoType.XSDsimpleType:
                            dataType = XsdTypeToDbType (propObjInfo.Name, propInfo);
                            break;
                        default:
                            continue;
                    }
                    list.Add (new string [] { propName, dataType });
                }
            }
        }

        //private string AddIdToText (string propName)
        //{
        //    if (char.IsUpper (propName, propName.Length - 1))
        //        return propName + "_Id";

        //    return propName + "Id";
        //}

        private string XsdTypeToDbType (string xsdType, PropInfo propInfo)
        {
            switch (xsdType)
            {
                case "float":
                case "double":
                case "decimal":
                    return "float8";
                case "int":
                case "short":
                case "unsignedShort":
                case "integer":
                case "unsignedInt":
                    return "int";
                case "long":
                case "unsignedLong":
                    return "bigint";
                case "time":
                case "date":
                case "dateTime":
                    return "timestamp";
                case "boolean":
                    return "boolean";
                case "string":
                    {
                        if (!double.IsNaN (propInfo.Restriction.Max))
                            return "varchar (" + (int) propInfo.Restriction.Max + ")";

                        return "text";
                    }
                default:
                    throw new Exception ("XSDsimpleType not supported: " + xsdType);
            }
        }

        #endregion


        private List<string> GetChildNames (ObjectInfo objInfo)
        {
            List<string> childNames = new List<string> ();

            foreach (ObjectInfo oi in _objInfoList)
            {
                if (oi.Base == objInfo && oi.IsUsed)
                {
                    if (oi.IsAbstract)
                        childNames.AddRange (GetChildNames (oi));
                    else
                        childNames.Add (oi.Name);
                }
            }

            return childNames;
        }

        private List<string> CreateExtensionClass (ObjectInfo objInfo)
        {
            List<string> innerLines = new List<string> ();

            foreach (PropInfo propInfo in objInfo.Properties)
            {
                if (propInfo.PropType.Type == ObjectInfoType.Feature)
                {
                    if (propInfo.IsList)
                    {
                        innerLines.Add (string.Format (
                            "public static ReadOnlyCollection<{0}> Get{1} (this {2} thisValue)",
                            propInfo.PropType.Name,
                            propInfo.Name,
                            objInfo.Name));

                        innerLines.Add ("{");
                        innerLines.Add ("return null;");
                        innerLines.Add ("}");
                    }
                    else
                    {
                        innerLines.Add (string.Format (
                            "public static {0} Get{1} (this {2} thisValue)",
                            propInfo.PropType.Name,
                            propInfo.Name,
                            objInfo.Name));

                        innerLines.Add ("{");
                        innerLines.Add ("return null;");
                        innerLines.Add ("}");
                    }
                }
            }

            if (innerLines.Count == 0)
                return innerLines;

            List<string> lines = new List<string> ();
            lines.Add ("using System.Collections.ObjectModel;");
            lines.Add ("");
            lines.Add ("namespace " + objInfo.Namespace);
            lines.Add ("{");
            lines.Add ("public static class " + objInfo.Name + "Extension");
            lines.Add ("{");

            lines.AddRange (innerLines);

            lines.Add ("}");
            lines.Add ("}");

            return lines;
        }

        private void AddNamespace (List<string> namespaceList, string ns)
        {
            if (!namespaceList.Contains (ns))
                namespaceList.Add (ns);
        }

        private void WriteFile (List<string> lines, string dir, string fileName)
        {
            FormatCodeLines (lines);

            string fileFullName = CodeFolder + "\\" + dir;
            if (!Directory.Exists (fileFullName))
                Directory.CreateDirectory (fileFullName);

            fileFullName += "\\" + fileName;

            FileStream stream = new FileStream (fileFullName, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter (stream);

            foreach (string line in lines)
                writer.WriteLine (line);

            writer.Close ();
            stream.Close ();
        }

        private void FormatCodeLines (List<string> lines)
        {
            string tabText = "";

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines [i].Trim () == "{")
                {
                    lines [i] = tabText + lines [i];
                    tabText = tabText.Insert (0, "\t");
                }
                else if (lines [i].Trim () == "}")
                {
                    tabText = tabText.Remove (0, 1);
                    lines [i] = tabText + lines [i];
                }
                else
                {
                    lines [i] = tabText + lines [i];
                }
            }
        }


        int count = 0;

        private List<string> CreatePropertyEnum (ObjectInfo objInfo)
        {
            string enumName = "Property" + objInfo.Name;

            string baseClassName = "";

            if (objInfo.Base != null)
            {
                baseClassName = objInfo.Base.Name;
            }
            else if (objInfo.Type == ObjectInfoType.Object)
                baseClassName = "AObject";
            else if (objInfo.Type == ObjectInfoType.Feature)
                baseClassName = "Feature";

            if (baseClassName != "")
                baseClassName = " = Property" + baseClassName + ".NEXT_CLASS";

            List<string> lines = new List<string> ();
            lines.Add ("public enum " + enumName);
            lines.Add ("{");

            //if (objInfo.IsChoice)
            //{
            //    for (int i = 0; i < objInfo.Properties.Count; i++)
            //    {
            //        lines.Add (objInfo.Properties [i].Name + " = PropertyChoiceClass.RefValue,");
            //    }

            //    lines.Add ("NEXT_CLASS = PropertyChoiceClass.NEXT_CLASS");
            //}
            //else
            //{

            if (objInfo.Properties.Count > 0 && objInfo.Properties[0].Name != "Annotation")
            {
                lines.Add(objInfo.Properties[0].Name + baseClassName + ",");

                for (int i = 1; i < objInfo.Properties.Count; i++)
                {
                    if (objInfo.Properties[i].Name == "Annotation")
                        continue;

                    lines.Add(objInfo.Properties[i].Name + ",");
                }

                lines.Add("NEXT_CLASS");
            }
            else
            {
                lines.Add("NEXT_CLASS" + baseClassName);
                count++;
            }

            lines.Add ("}");

            return lines;
        }

        private void CreateChoicePropertyLines (ObjectInfo objInfo, List<string> lines, List<string> namespaceList)
        {
            foreach (PropInfo propInfo in objInfo.Properties)
            {
                if (propInfo.PropType.Type == ObjectInfoType.Feature && propInfo.IsList)
                {
                    continue;
                }

                string getFuncName;
                string setFuncName;
                string namespaceText;
                string propInfoName;
                string getArgName;

                string propTypeName = GetPropertyTypeName (propInfo,
                    out getFuncName, out setFuncName, out namespaceText, out propInfoName, out getArgName);

                if (propInfo.PropType.Type == ObjectInfoType.Feature && propInfo.Nullable)
                    propTypeName = propTypeName.Replace ("?", "");

                if (namespaceText.StartsWith ("Aran.") &&
                    !namespaceList.Contains (namespaceText))
                {
                    namespaceList.Add (namespaceText);
                }

                lines.Add (string.Format ("public {0} {1}", propTypeName, propInfoName));
                lines.Add ("{");
                {
                    if (propInfo.PropType.Type == ObjectInfoType.Object)
                    {
                        if (propInfo.PropType.IsAbstract)
                        {
                            lines.Add (string.Format ("get {{ return ({0}) GetChoiceAbstractObject ({1}); }}",
                                propTypeName, getArgName));
                        }
                        else
                        {
                            lines.Add (string.Format ("get {{ return ({0}) RefValue; }}", propTypeName));
                        }
                    }
                    else
                    {
                        if (propInfo.PropType.Type == ObjectInfoType.Datatype &&
                            propInfo.PropType.Name.StartsWith ("Abstract"))
                        {
                            lines.Add (string.Format (
                                "get {{ return ({0}) RefAbstractFeature; }}", propTypeName));
                        }
                        else
                        {
                            lines.Add ("get { return RefFeature; }");
                        }
                    }

                    lines.Add ("set");
                    lines.Add ("{");
                    {
                        if (propInfo.PropType.Type == ObjectInfoType.Object)
                        {
                            lines.Add ("RefValue = value;");
                        }
                        else if (propInfo.PropType.Type == ObjectInfoType.Datatype &&
                            propInfo.PropType.Name.StartsWith ("Abstract"))
                        {
                            lines.Add ("RefAbstractFeature = value;");
                        }
                        else
                        {
                            lines.Add ("RefFeature = value;");
                        }
                    }
                    lines.Add (string.Format ("RefType = (int) {0}Choice.{1};",
                                objInfo.Name, propInfo.PropType.Name));
                    lines.Add ("}");

                }
                lines.Add ("}");
                lines.Add ("");
            }
        }

        private void CreatePropertyLines (ObjectInfo objInfo, List<string> lines, List<string> namespaceList)
        {
            foreach (PropInfo propInfo in objInfo.Properties)
            {
                if (propInfo.Name == "Annotation")
                    continue;

                if (propInfo.Name == "Geo" && 
                    (objInfo.Name == "AixmPoint" ||
                    objInfo.Name == "Curve" ||
                    objInfo.Name == "Surface"))
                    continue;

                string getFuncName;
                string setFuncName;
                string namespaceText;
                string propInfoName;
                string getArgName;
                string propTypeName = GetPropertyTypeName (propInfo,
                    out getFuncName, out setFuncName, out namespaceText, out propInfoName, out getArgName);

                if (getArgName.Length > 0)
                    getArgName = ", " + getArgName;

                if (namespaceText.StartsWith ("Aran.") &&
                    !namespaceList.Contains (namespaceText))
                {
                    namespaceList.Add (namespaceText);
                }

                lines.Add (string.Format ("public {0} {1}", propTypeName, propInfoName));
                lines.Add ("{");
                {
                    lines.Add (string.Format ("get {{ return {0} ((int) Property{1}.{2}{3}); }}",
                        getFuncName,
                        objInfo.Name,
                        propInfoName,
                        getArgName));

                    if (!propInfo.IsList)
                    {
                        lines.Add (string.Format ("set {{ {0} ((int) Property{1}.{2}, value); }}",
                            setFuncName,
                            objInfo.Name,
                            propInfoName));
                    }
                }
                lines.Add ("}");
                lines.Add ("");
            }
        }

        private string GetPropertyTypeName (PropInfo propInfo,
            out string getFuncName,
            out string setFuncName,
            out string namespaceText,
            out string propInfoName,
            out string getArgName)
        {
            ObjectInfo objInfo = propInfo.PropType;
            namespaceText = objInfo.Namespace;
            propInfoName = propInfo.Name;
            getArgName = "";
            setFuncName = "SetValue";

            if (objInfo.Type == ObjectInfoType.Feature)
            {
                if (objInfo.IsAbstract)
                {
                    string absObjName = "Abstract" + objInfo.Name;
                    if (propInfo.IsList)
                        absObjName += "Object";
                    absObjName += "Ref";
                    objInfo = _objInfoList.Where (oi => oi.Name == absObjName).First ();
                }
                else
                {
                    if (propInfo.IsList)
                        objInfo = _featureRefObjectObjInfo;
                    else
                        objInfo = _featureRefObjInfo;
                }

                namespaceText = objInfo.Namespace;
            }

            if (objInfo.Type == ObjectInfoType.XSDsimpleType ||
                objInfo.Type == ObjectInfoType.Codelist)
            {
                string systemType = objInfo.Name;

                if (objInfo.Type == ObjectInfoType.XSDsimpleType)
                {
                    switch (objInfo.Name)
                    {
                        case "boolean":
                            systemType = "bool";
                            break;
                        case "integer":
                            systemType = "int";
                            break;
                        case "unsignedInt":
                            systemType = "uint";
                            break;
                        case "date":
                        case "dateTime":
                            systemType = "DateTime";
                            break;
                        case "decimal":
                            systemType = "double";
                            break;
                        case "guid":
                            systemType = "Guid";
                            break;
                    }
                }

                if (propInfo.IsList)
                {
                    getFuncName = null;
                }
                else
                {

                    if (propInfo.Nullable)
                    {
                        if (systemType != "string")
                        {
                            getFuncName = "GetNullableFieldValue <" + systemType + ">";
                            setFuncName = "SetNullableFieldValue <" + systemType + ">";
                            systemType += "?";
                        }
                        else
                        {
                            getFuncName = "GetFieldValue <" + systemType + ">";
                            setFuncName = "SetFieldValue <" + systemType + ">";
                        }
                    }
                    else
                    {
                        getFuncName = "GetFieldValue <" + systemType + ">";
                        setFuncName = "SetFieldValue <" + systemType + ">";
                    }
                }

                return systemType;
            }
            else if (objInfo.Type == ObjectInfoType.Datatype)
            {
                if (propInfo.IsList)
                    getFuncName = null;
                else
                    getFuncName = "(" + objInfo.Name + " ) GetValue";
            }
            else if (objInfo.Type == ObjectInfoType.Object)
            {
                if (objInfo.IsAbstract)
                {
                    getArgName = "AbstractType." + objInfo.Name;

                    if (propInfo.IsList)
                        getFuncName = "(List <" + objInfo.Name + ">) GetAbstractList";
                    else
                        getFuncName = "(" + objInfo.Name + " ) GetAbstractObject";
                }
                else
                {
                    if (propInfo.IsList)
                        getFuncName = "GetObjectList <" + objInfo.Name + ">";
                    else
                        getFuncName = "GetObject <" + objInfo.Name + ">";
                }
            }
            else
            {
                getFuncName = null;
            }

            string returnTypeText;
            if (propInfo.IsList)
                returnTypeText = "List <" + objInfo.Name + ">";
            else
                returnTypeText = objInfo.Name;

            return returnTypeText;
        }

        private string GetParentClassText (ObjectInfo objInfo, out string baseEnumType, out string aNamespace)
        {
            string parentName = null;
            aNamespace = null;

            switch (objInfo.Type)
            {
                case ObjectInfoType.Feature:
                    parentName = "Feature";
                    baseEnumType = "FeatureType";
                    aNamespace = "Aran.Aim.Features";
                    break;
                case ObjectInfoType.Object:
                    {
                        if (objInfo.IsChoice)
                            parentName = "ChoiceClass";
                        else
                            parentName = "AObject";
                        baseEnumType = "ObjectType";
                        aNamespace = "Aran.Aim.Objects";
                    }
                    break;
                case ObjectInfoType.Datatype:
                    if (objInfo.Name.StartsWith ("Abstract") && objInfo.Name.EndsWith ("Ref"))
                        parentName = "AbstractFeatureRef <" + objInfo.Name.Substring (8, objInfo.Name.Length - 11) + "Type>";
                    else
                        parentName = "ADataType";
                    baseEnumType = "DataType";
                    aNamespace = "Aran.Aim.DataTypes";
                    break;
                default:
                    parentName = null;
                    baseEnumType = null;
                    break;
            }

            if (objInfo.Base != null)
            {
                parentName = objInfo.Base.Name;
                aNamespace = objInfo.Base.Namespace;
            }

            return " : " + parentName;
        }

        private string GetPropertyTypeIndexEnumName (PropInfo propInfo,
            out string propTypeCharacter,
            out string aixmName)
        {
            string rv = "";

            ObjectInfo objInfo = propInfo.PropType;

            if (objInfo.IsAbstract)
            {
                rv = "AbstractType." + objInfo.Name;

                if (objInfo.Type != ObjectInfoType.Object)
                    _errors.Add ("PropertyType is abstract\r\n\tproperty name: " + propInfo.Name + ", type: " + objInfo.Name);
            }
            else
            {
                switch (objInfo.Type)
                {
                    case ObjectInfoType.XSDsimpleType:
                        {
                            string s = objInfo.Name;

                            switch (objInfo.Name)
                            {
                                case "boolean":
                                    s = "Bool";
                                    break;
                                case "integer":
                                    s = "Int32";
                                    break;
                                case "unsignedInt":
                                    s = "UInt32";
                                    break;
                                case "date":
                                case "dateTime":
                                    s = "DateTime";
                                    break;
                                case "decimal":
                                    s = "Double";
                                    break;
                                case "string":
                                    s = "String";
                                    break;
                                case "long":
                                    s = "Int64";
                                    break;
                                case "guid":
                                    s = "Guid";
                                    break;
                                case "language":
                                    rv = "EnumType.Language";
                                    break;
                            }

                            if (rv == "")
                                rv = "AimFieldType.Sys" + s;

                            break;
                        }
                    case ObjectInfoType.Feature:
                        rv = "FeatureType." + objInfo.Name;
                        break;
                    case ObjectInfoType.Object:
                        rv = "ObjectType." + objInfo.Name;
                        break;
                    case ObjectInfoType.Datatype:
                        rv = "DataType." + objInfo.Name;
                        break;
                    case ObjectInfoType.Codelist:
                        rv = "EnumType." + objInfo.Name;
                        break;
                }
            }

            propTypeCharacter = null;

            if (propInfo.IsList)
                propTypeCharacter = "PropertyTypeCharacter.List";

            if (propInfo.Nullable)
            {
                if (propTypeCharacter != null)
                    propTypeCharacter += " | PropertyTypeCharacter.Nullable";
                else
                    propTypeCharacter = "PropertyTypeCharacter.Nullable";
            }

            if (propInfo.Name == "CallSign")
                aixmName = "call-sign";
            else
                aixmName = null;

            return rv;
        }

        private List<string> GenerateInsertIntoEnum (ObjectInfo objInfo)
        {
            List<string> list = new List<string> ();

            for (int i = 0; i < objInfo.Properties.Count; i++)
            {
                PropInfo propInfo = objInfo.Properties [i];

                string docText = (propInfo.Documentation == null ? "NULL" :
                    "'" + propInfo.Documentation.Replace ("'", "''") + "'");

                list.Add (string.Format (
                    "INSERT INTO _aran_enums (enum_name, value, name, alias, documentation) VALUES " +
                    "('{0}', {1}, '{2}', '{3}', {4})",
                    objInfo.Name,
                    i,
                    ModifyEnumItem (propInfo.Name),
                    propInfo.Name,
                    docText));
            }

            return list;
        }

        private List<ObjectInfo> GetUsedEnums ()
        {
            List<ObjectInfo> list = new List<ObjectInfo> ();

            foreach (ObjectInfo objInfo in _objInfoList)
            {
                if (objInfo.Type == ObjectInfoType.Codelist ||
                    (objInfo.Namespace != "Aran.Aim.Features" &&
                    objInfo.Namespace != "Aran.Aim.DataTypes" &&
                    objInfo.Namespace != "Aran.Aim.Metadata.ISO" &&
                    objInfo.Namespace != "Aran.Aim.Metadata"))
                {
                    continue;
                }

                foreach (PropInfo propInfo in objInfo.Properties)
                {
                    if (propInfo.PropType.Type == ObjectInfoType.Codelist)
                    {
                        if (!list.Contains (propInfo.PropType))
                            list.Add (propInfo.PropType);
                    }
                }
            }

            return list;
        }

        private List<string> GenerateEnum (ObjectInfo objInfo)
        {
            List<string> lines = new List<string> ();

            if (objInfo.Properties.Count == 0 ||
                (objInfo.Properties.Count == 1 && objInfo.Properties [0].Name == "OTHER"))
                return lines;

            lines.Add ("public enum " + objInfo.Name);
            lines.Add ("{");

            for (int i = 0; i < objInfo.Properties.Count; i++)
            {
                lines.Add (
                    ModifyEnumItem (objInfo.Properties [i].Name) +
                    (i < objInfo.Properties.Count - 1 ? "," : ""));
            }

            var customEnumField = customEnumFields.Where(p => p.Name == objInfo.Name).FirstOrDefault();

            if(customEnumField != null)
            {
                if(objInfo.Properties.Count > 0) {
                    var lastIndex = lines.Count - 1;
                    lines[lastIndex] = lines[lastIndex] + ",";
                }

                for (int i = 0; i < customEnumField.Fields.Count; i++)
                {
                    if (customEnumField.Fields[i].Description != null)
                        lines.Add($"[Description(\"{customEnumField.Fields[i].Description}\")]");

                    if (customEnumField.Fields[i].Name != null)
                        lines.Add(customEnumField.Fields[i].Name +
                            (i < customEnumField.Fields.Count - 1 ? "," : ""));
                }
            }

            lines.Add ("}");
            return lines;
        }

        private string ModifyEnumItem (string enumItem)
        {
            if (char.IsDigit (enumItem, 0))
                enumItem = "_" + enumItem;

            int ind;
            if ((ind = enumItem.IndexOf (' ')) >= 0)
                enumItem = enumItem.Remove (ind, 1);

            if (enumItem.IndexOf ('-') >= 0)
                enumItem = enumItem.Replace ("-", "_minus_");
            else if (enumItem.IndexOf ('+') >= 0)
                enumItem = enumItem.Replace ("+", "_plus_");

            return enumItem;
        }

        private List<ObjectInfo> _objInfoList;
        private ObjectInfo _featureRefObjInfo;
        private ObjectInfo _featureRefObjectObjInfo;
        private List<string> _errors;
    }
}
