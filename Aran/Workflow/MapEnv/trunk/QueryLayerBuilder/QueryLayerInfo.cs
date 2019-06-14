//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Aran.Aim.Env2.Layers;
//using System.Collections.ObjectModel;
//using Aran.Aim.Data.Filters;
//using System.Collections;
//using Aran.Aim;
//using Aran.Aim.Data;
//using Aran.Aim.Enums;
//using Aran.Aim.Utilities;
//using Aran.Aim.Features;
//using Aran.Aim.DataTypes;

//namespace MapEnv.QueryLayer
//{
//    public class QueryLayerInfo
//    {
//        public QueryLayerInfo (IDbProvider dbProvider)
//        {
//            _dbProvider = dbProvider;
//            LayerGroup = new AimFeatureLayerGroup ();
//            _layerDict = new Dictionary<QueryInfo_OLD, AimFeatureLayer> ();
//        }

//        public AimFeatureLayerGroup LayerGroup { get; private set; }

//        public QueryInfo_OLD QueryInfo
//        {
//            get { return _queryInfo; }
//            set
//            {
//                _queryInfo = value;

//                LoadQuery (value, null);
//                var layerList = ToAimFeatureLayer (value);

//                LayerGroup.Name = value.Name;
//                LayerGroup.Layers.AddRange (layerList);
//                LayerGroup.Tag = this;
//            }
//        }

//        public void Refresh ()
//        {
//            LoadQuery (_queryInfo, null);
//            FillLayerByQuery (_queryInfo);
//        }


//        private List<AimFeatureLayer> ToAimFeatureLayer (QueryInfo_OLD qi)
//        {
//            var list = new List<AimFeatureLayer> ();

//            var layer = new AimFeatureLayer ();
//            layer.Create (Globals.Environment);
//            layer.Name = qi.Name + " [" + qi.FeatureType + "]";
//            layer.AimTable.ShapeInfoList.AddRange (qi.ShapeInfoList);
//            layer.AimTable.Filter = qi.Filter;
//            layer.Open (qi.FeatureType, new List<Feature> (qi.FeatureList.Cast<Feature> ()));
//            layer.Visible = true;

//            _layerDict.Add (qi, layer);

//            list.Add (layer);

//            foreach (var sqi in qi.SubQueries)
//            {
//                var subList = ToAimFeatureLayer (sqi.QueryInfo);
//                list.AddRange (subList);
//            }

//            return list;
//        }

//        private void LoadQuery (QueryInfo_OLD qi, IEnumerable<Guid> identifiers)
//        {
//            Filter filter = null;

//            if (identifiers != null)
//            {
//                ComparisonOps compOp = new ComparisonOps ();
//                compOp.OperationType = ComparisonOpType.In;
//                compOp.PropertyName = "Identifier";
//                compOp.Value = identifiers;
//                filter = new Filter (new OperationChoice (compOp));
//            }
//            else
//            {
//                filter = qi.Filter;
//            }

//            IList featureList = GetFeatures (qi.FeatureType, filter);
//            qi.FeatureList = featureList;

//            foreach (SubQueryInfo_OLD si in qi.SubQueries)
//            {
//                AimPropInfo [] pathPropInfos = AimMetadataUtility.GetInnerProps ((int) qi.FeatureType, si.PropertyPath);

//                List<Guid> subGuidList = new List<Guid> ();

//                foreach (Feature feat in featureList)
//                {
//                    var aimPropValList = AimMetadataUtility.GetInnerPropertyValue (feat, pathPropInfos);

//                    foreach (IAimProperty aimProp in aimPropValList)
//                    {
//                        if (aimProp is IAbstractFeatureRef)
//                        {
//                            var afr = aimProp as IAbstractFeatureRef;
//                            if (afr.FeatureTypeIndex == (int) si.QueryInfo.FeatureType)
//                                subGuidList.Add (afr.Identifier);
//                        }
//                        else if (aimProp is FeatureRef)
//                        {
//                            var featRef = aimProp as FeatureRef;
//                            if (!subGuidList.Contains (featRef.Identifier))
//                                subGuidList.Add (featRef.Identifier);
//                        }
//                    }
//                }

//                LoadQuery (si.QueryInfo, subGuidList);
//            }
//        }

//        private IList GetFeatures (FeatureType featureType, Filter filter)
//        {
//            var gr = _dbProvider.GetVersionsOf (
//                featureType,
//                TimeSliceInterpretationType.BASELINE,
//                Guid.Empty,
//                true,
//                null,
//                null,
//                filter);

//            if (!gr.IsSucceed)
//            {
//                throw new Exception (gr.Message);
//            }

//            return gr.List;
//        }

//        private void FillLayerByQuery (QueryInfo_OLD qi)
//        {
//            var layer = _layerDict [qi];
//            layer.AimTable.Rows.Clear ();
//            layer.AimTable.AddFeatures (qi.FeatureList.Cast<Feature> ());

//            foreach (var sqi in qi.SubQueries)
//            {
//                FillLayerByQuery (sqi.QueryInfo);
//            }
//        }

//        private IDbProvider _dbProvider;
//        private QueryInfo_OLD _queryInfo;
//        private Dictionary<QueryInfo_OLD, AimFeatureLayer> _layerDict;
//    }
//}
