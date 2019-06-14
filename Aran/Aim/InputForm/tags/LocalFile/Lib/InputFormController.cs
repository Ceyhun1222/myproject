using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.CAWProvider;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Queries.Common;
using Aran.Queries.Viewer;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.InputFormLib
{
    public class InputFormController
    {
        public FormClosingEventHandler ClosedEventHandler;
        public FeatureEventHandler SavedEventHandler;
        public FeatureEventHandler OpenedEventHandler;

        public InputFormController ()
        {
            _metadata = UIMetadata.Instance;

            foreach (AimClassInfo classInfo in _metadata.ClassInfoList)
            {
                UIClassInfo uiClassInfo = classInfo.UiInfo ();
                if (uiClassInfo.DependsFeature != null &&
                    (uiClassInfo.DependsFeature == "" ||
                    uiClassInfo.DependsFeature == " "))
                {
                    uiClassInfo.DependsFeature = null;
                }
            }

            _cawService = null;
        }

        public string [] GetFeaturesByDepends (string dependsFeature)
        {
            List<string> list = new List<string> ();

            foreach (AimClassInfo classInfo in _metadata.ClassInfoList)
            {
                UIClassInfo uiInfo = classInfo.UiInfo ();

                if (classInfo.AimObjectType == AimObjectType.Feature)
                {
                    if (dependsFeature == uiInfo.DependsFeature)
                    {
                        list.Add (classInfo.Name);
                    }
                }
            }
            return list.ToArray ();
        }

        public Feature [] GetFeatures (FeatureType featureType,
            InterpretationType interpretationType,
            DateTime dateTime,
            Feature dependsFeature)
        {
            LoadCawService ();

            AbstractRequest absRequest;
            SimpleQuery simpleQuery = new SimpleQuery ();
            simpleQuery.TemproalTimeslice = new TemporalTimeslice (dateTime);
            simpleQuery.FeatureType = featureType;
            simpleQuery.Interpretation = interpretationType;

            absRequest = simpleQuery;
            
            if (dependsFeature != null)
            {
                LinkQuery linkQuery = new LinkQuery ();
                linkQuery.FeatureTypeList.Add (featureType);
                linkQuery.TraverseTimeslicePropertyName = GetFeatureRelationPropName (featureType, dependsFeature.FeatureType);

                if (linkQuery.TraverseTimeslicePropertyName == null)
                {
                    return new Feature [] { };
                }

                linkQuery.SimpleQuery = simpleQuery;
                linkQuery.SimpleQuery.FeatureType = dependsFeature.FeatureType;
                linkQuery.SimpleQuery.IdentifierList.Add (dependsFeature.Identifier);

                absRequest = linkQuery;
            }

            try
            {
                return _cawService.GetFeature (absRequest);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith ("Property name '"))
                    return new Feature [] {};

                throw ex;
            }
        }

        public void FillColumns (FeatureType featureType, DataGridView dgv)
        {
            UIUtilities.FillColumns (_metadata.GetClassInfo ((int) featureType), dgv);
        }

        public void SetRow (DataGridView dgv, Feature feature, int rowIndex = -1)
        {
            UIUtilities.SetRow (dgv, feature, rowIndex);
        }

        public string GetFeatureDescription (Feature feature)
        {
            AimClassInfo classInfo = _metadata.ClassInfoList.Where (
                ci => ci.Index == (int) feature.FeatureType).First ();

            UIClassInfo uiClassInfo = classInfo.UiInfo ();
            string s = uiClassInfo.DescriptionFormat;

            if (string.IsNullOrEmpty (s))
                return null;

            string resultStr = "";

            int startIndex = -1;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s [i];

                if (startIndex == -1)
                {
                    if (c == '<')
                    {
                        startIndex = i;
                    }
                    else
                    {
                        resultStr += c;
                    }
                }
                else
                {
                    if (c == '>')
                    {
                        string propName = s.Substring (startIndex + 1, i - startIndex - 1);

                        #region GetValue

                        foreach (AimPropInfo propInfo in classInfo.Properties)
                        {
                            if (propInfo.Name == propName)
                            {
                                IAimProperty aimProp = (feature as IAimObject).GetValue (propInfo.Index);
                                if (aimProp != null)
                                {
                                    if (aimProp.PropertyType == AimPropertyType.AranField)
                                    {
                                        AimField aimField = (AimField) aimProp;

                                        if (aimField.FieldType == AimFieldType.SysEnum)
                                        {
                                            string enumText = AimMetadata.GetEnumValueAsString (
                                                (int) ((IEditAimField) aimField).FieldValue, propInfo.TypeIndex);

                                            resultStr += enumText;
                                        }
                                        else
                                        {
                                            resultStr += ((IEditAimField) aimField).FieldValue.ToString ();
                                        }
                                    }
                                }
                                break;
                            }
                        }

                        #endregion

                        startIndex = -1;
                    }
                }
            }

            return resultStr;
        }

        public Control GetPropertiesControl (Feature feature)
        {
            AimClassInfo classInfo = _metadata.ClassInfoList.Where (ci => ci.Index == (int) feature.FeatureType).First ();

            FeatureControl featureControl = new FeatureControl ();
            featureControl.Closed += ClosedEventHandler;
            featureControl.Saved += SavedEventHandler;
            featureControl.Opened += OpenedEventHandler;
            featureControl.GetFeature += new GetFeatureHandler (FeatureControl_GetFeature);
            featureControl.GetFeatureList += new GetFeatureListHandler (FeatureControl_GetFeatureList);
            featureControl.DataGridColumnsFilled = new FillDataGridColumnsHandler (UIUtilities.FillColumns);
            featureControl.DataGridRowSetted = new SetDataGridRowHandler (SetRow);

            featureControl.LoadFeature (feature, classInfo);

            return featureControl;
        }

        public string GetDependsFeature (Feature feature)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) feature.FeatureType);
            UIClassInfo uiClassInfo = classInfo.UiInfo ();
            return uiClassInfo.DependsFeature;
        }

        public Feature CreateNewFeature (FeatureType featureType, Feature dependsFeature)
        {
            Feature newFeature = AimObjectFactory.CreateFeature (featureType);
            newFeature.Id = -1;
            newFeature.Identifier = Guid.NewGuid ();
            newFeature.TimeSlice = new Aran.Aim.DataTypes.TimeSlice ();
            newFeature.TimeSlice.Interpretation = Aran.Aim.Enums.TimeSliceInterpretationType.PERMDELTA;
            newFeature.TimeSlice.ValidTime = new Aran.Aim.DataTypes.TimePeriod (DateTime.Now);
            newFeature.TimeSlice.SequenceNumber = 1;
            newFeature.TimeSlice.CorrectionNumber = 0;
            newFeature.TimeSlice.FeatureLifetime = newFeature.TimeSlice.ValidTime;


            #region Set depends value
            if (dependsFeature != null)
            {
                AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
                AimPropInfo propInfo = classInfo.Properties.Where (pi => pi.ReferenceFeature == dependsFeature.FeatureType).FirstOrDefault ();
                if (propInfo != null)
                {
                    if (propInfo.IsList)
                    {
                        FeatureRefObject featureRefObj = (FeatureRefObject) AimObjectFactory.Create (propInfo.TypeIndex);
                        featureRefObj.Feature = new FeatureRef ();
                        featureRefObj.Feature.Identifier = dependsFeature.Identifier;

                        IAimProperty listPropVal = ((IAimObject) newFeature).GetValue (propInfo.Index);
                        if (listPropVal == null)
                        {
                            AObjectList<FeatureRefObject> objList = new AObjectList<FeatureRefObject> ();
                            objList.Add (featureRefObj);
                            ((IAimObject) newFeature).SetValue (propInfo.Index, objList);
                        }
                    }
                    else
                    {
                        FeatureRef featureRef = (FeatureRef) AimObjectFactory.Create (propInfo.TypeIndex);
                        featureRef.Identifier = dependsFeature.Identifier;
                        ((IAimObject) newFeature).SetValue (propInfo.Index, featureRef);
                    }
                }
            }
            #endregion

            return newFeature;
        }

        public bool SaveFeature (Feature newFeature)
        {
            return _cawService.InsertFeature (newFeature, null);
        }

        public string ExportToXml (string xmlFileName)
        {
            if (_cawService.ConnectionInfo.Server.IsFile)
            {
                LocalDbExporter exporter = new LocalDbExporter ();
                exporter.Export (_cawService.ConnectionInfo.Server.LocalPath, xmlFileName);
                return null;
            }

            return "Exporter not supported";
        }


        private IEnumerable<Feature> FeatureControl_GetFeatureList (FeatureType featureType)
        {
            SimpleQuery sq = new SimpleQuery ();
            sq.FeatureType = featureType;

            return _cawService.GetFeature (sq);
        }

        private Feature FeatureControl_GetFeature (FeatureType featureType, Guid identifier)
        {
            SimpleQuery sq = new SimpleQuery ();
            sq.FeatureType = featureType;
            sq.IdentifierList.Add (identifier);

            Feature [] featArr = _cawService.GetFeature (sq);
            if (featArr.Length > 0)
                return featArr [0];
            return null;
        }
        
        private string GetFeatureRelationPropName (FeatureType featureType, FeatureType propertyFeatureType)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
            AimPropInfo propInfo = classInfo.Properties.Where (pi => pi.ReferenceFeature == propertyFeatureType).FirstOrDefault ();
            if (propInfo != null)
                return propInfo.AixmName;

            return null;
        }

        private void LoadCawService ()
        {
            if (_cawService == null)
            {
                _cawService = CawProviderFactory.CreateService (CawProviderType.FileBase);
                _cawService.ConnectionInfo = new ConnectionInfo
                {
                    //Server = new Uri ("http://91.221.120.150:17871/cadas-aimdb-risk/caw"),
                    //Server = new Uri ("http://91.221.120.150:17871/cadas-aimdb-risk-v3-4-snapshot/caw"),
                    Server = new Uri (@"C:\Program Files\R.I.S.K. AirNavLab\ProcViewer\Data"),
                    UserName = "risk",
                    Password = "CSaimdb"
                };
            }
        }

        private UIMetadata _metadata;
        private ICawService _cawService;
    }
}
