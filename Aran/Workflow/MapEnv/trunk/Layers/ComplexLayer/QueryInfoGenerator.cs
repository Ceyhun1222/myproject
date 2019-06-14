using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim;
using System.Collections;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using MapEnv.Layers;

namespace MapEnv.ComplexLayer
{
    public class QueryInfoGenerator
    {
        public QueryInfoGenerator ()
        {
            _loadedFeatures = new Dictionary<Guid, Feature> ();
        }

        public QueryInfo Load (Feature feature)
        {
            _loadedFeatures.Clear ();
            _loadedFeatures.Add (feature.Identifier, feature);

            var qi = CreateQueryInfo (feature.FeatureType);
            FillObject (feature, qi, new List<string> ());
            return qi;
        }

        
        private void FillObject (IAimObject aimObject, QueryInfo qi, List<string> propNameList)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex (aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                if (propInfo.PropType.AimObjectType == AimObjectType.Object)
                {
                    var aimPropVal = aimObject.GetValue (propInfo.Index);
                    if (aimPropVal == null)
                        continue;

                    var newPropNameList = new List<string> (propNameList);
                    newPropNameList.Add (propInfo.Name);

                    if (propInfo.IsList)
                    {
                        var listVal = aimPropVal as IList;

                        if (propInfo.TypeIndex == (int) ObjectType.FeatureRefObject)
                        {
                            foreach (FeatureRefObject fro in listVal)
                            {
                                var featVal = GetFeature (propInfo.ReferenceFeature, fro.Feature.Identifier);
                                if (featVal != null)
                                    SetSubQuery (featVal, qi, propNameList, propInfo.Name);
                            }
                        }
                        else
                        {
                            foreach (AObject objItem in listVal)
                                FillObject (objItem, qi, newPropNameList);
                        }
                    }
                    else if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
                    {
                        var editChoice = aimPropVal as IEditChoiceClass;
                        if (editChoice.RefValue is FeatureRef)
                        {
                            var fr = editChoice.RefValue as FeatureRef;
                            var featVal = GetFeature ((FeatureType) editChoice.RefType, fr.Identifier);
                            if (featVal != null)
                            {
                                var choicePropInfo = GetChoicePropName (editChoice);

                                var tmpPropNameList = new List<string> (propNameList);
                                tmpPropNameList.Add (propInfo.Name);
                                SetSubQuery (featVal, qi, tmpPropNameList, choicePropInfo.Name);
                            }
                        }
                    }
                    else
                    {
                        var objVal = aimPropVal as AObject;
                        FillObject (objVal, qi, newPropNameList);
                    }
                }
                else
                {
                    Feature featVal = null;

                    if (propInfo.IsFeatureReference && propInfo.ReferenceFeature != 0)
                    {
                        var featRef = aimObject.GetValue (propInfo.Index) as FeatureRef;
                        if (featRef != null)
                            featVal = GetFeature (propInfo.ReferenceFeature, featRef.Identifier);
                    }
                    else if (propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef)
                    {
                        var absFeatRef = aimObject.GetValue (propInfo.Index) as IAbstractFeatureRef;
                        if (absFeatRef != null)
                            featVal = GetFeature ((FeatureType) absFeatRef.FeatureTypeIndex, absFeatRef.Identifier);
                    }

                    if (featVal != null)
                        SetSubQuery (featVal, qi, propNameList, propInfo.Name);
                }
            }
        }

        private void SetSubQuery (Feature featVal, QueryInfo qi, List<string> propNameList, string propName)
        {
            SubQueryInfo sqi = null;
            var propPath = MakePropPath (propNameList, propName);

            foreach (var sqiItem in qi.SubQueries)
            {
                if (sqiItem.PropertyPath == propPath &&
                                sqiItem.QueryInfo.FeatureType == featVal.FeatureType)
                {
                    sqi = sqiItem;
                    break;
                }
            }

            if (sqi == null)
            {
                sqi = new SubQueryInfo ();
                sqi.PropertyPath = propPath;
                sqi.QueryInfo = CreateQueryInfo (featVal.FeatureType);
                qi.SubQueries.Add (sqi);
            }

            FillObject (featVal, sqi.QueryInfo, new List<string> ());
        }

        private string MakePropPath (List<string> propNameList, string propName)
        {
            string s = "";
            foreach (var item in propNameList)
                s += item + "/";
            return s + propName;
        }

        private Feature GetFeature (FeatureType featureType, Guid identifier)
        {
            if (_loadedFeatures.ContainsKey (identifier))
                return null;
                //return _loadedFeatures [identifier];

            var dbPro = Globals.Environment.DbProvider as DbProvider;
            var gr = dbPro.GetVersionsOf (featureType,
                Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, identifier);
            if (!gr.IsSucceed)
                throw new Exception (gr.Message);

            if (gr.List.Count == 0)
                return null;

            var feature = gr.List [0] as Feature;
            _loadedFeatures.Add (feature.Identifier, feature);
            return feature;
        }

        private AimPropInfo GetChoicePropName (IEditChoiceClass editChoice)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex (editChoice as IAimObject);

            foreach (var prInfo in classInfo.Properties)
            {
                if (prInfo.TypeIndex == editChoice.RefType || 
                    (prInfo.IsFeatureReference && (int) prInfo.ReferenceFeature == editChoice.RefType))
                    return prInfo;
            }

            return null;
        }

        private QueryInfo CreateQueryInfo (FeatureType featType)
        {
            var qi = new QueryInfo (featType);

            var shapeInfoList = DefaultStyleLoader.Instance.GetShapeInfo (featType);
            if (shapeInfoList != null && shapeInfoList.Count > 0)
                qi.ShapeInfoList.AddRange (shapeInfoList);

            return qi;
        }

        private Dictionary<Guid, Feature> _loadedFeatures;
    }
}
