using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Aran.Aim.Env2.Layers;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using Aran.Aim;
using Aran.Aim.Utilities;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;

namespace MapEnv
{
	public static class AimFLComplexLoader
	{
		public static void AddComplexLayer ()
		{
			var compLayerBuilder = new ComplexLayer.ComplexLayerBuilderForm ();

			if (compLayerBuilder.ShowDialog () != DialogResult.OK)
				return;

			var qi = compLayerBuilder.GetQueryInfo ();
			var layerInfoList = new List<FeatLayerInfo> ();

			var compTable = LoadQueryInfo (qi, layerInfoList);

			var aimFL = new AimFeatureLayer ();
			aimFL.SetQueryInfo (qi);
			aimFL.ComplexTable = compTable;
			aimFL.DbSpatialReference = AimFLGlobal.GeoWGS84_SpatialRef;
			aimFL.SpatialReference = AimFLGlobal.GeoWGS84_SpatialRef;
			aimFL.LayerInfoList.AddRange (layerInfoList);

			var aimLayer = new Toc.AimLayer (aimFL);
			Globals.MainForm.AddAimSimpleShapefileLayer (aimLayer, true);
		}

		public static AimComplexTable LoadQueryInfo (QueryInfo qi, List<FeatLayerInfo> layerInfoList)
		{
			var featList = GetFeatures (qi.FeatureType, qi.Filter);
			var compTable = CreateComplexTable (qi.FeatureType, featList);
			var compTableList = new List<AimComplexTable> ();
			compTableList.Add (compTable);

			var qiFeatDict = new QIFeatDict ();
			qiFeatDict.Add (qi, featList);

			var featAssocCompTableList = featList.Select (f => (int) 0).ToList ();

			LoadFeatQueryInfo (qi, featList, featAssocCompTableList, compTableList, qiFeatDict);

			foreach (var qiKey in qiFeatDict.Keys)
			{
				if (qiKey.ShapeInfoList.Count > 0)
				{
					var features = qiFeatDict [qiKey];

					AimFLGlobal.GetFeatureClasses (
						qiKey.FeatureType,
						features,
						qiKey.ShapeInfoList,
						layerInfoList);
				}
			}

			return compTable;
		}

		public static void LoadFeatQueryInfo (QueryInfo qi, List<Feature> featList, List<int> featAssocCompTableList,
			List<AimComplexTable> compTableList, QIFeatDict qiFeatDict)
		{
			if (featList.Count == 0)
				return;

			#region Sub Query

			foreach (SubQueryInfo sqi in qi.SubQueries)
			{
				var featIdensPairList = featList.Select (f =>
					new AimSubFeatIdensPair () {
						Feature = f,
						Identifiers = GetPropGuidValues (f, sqi.PropertyPath)
					}).ToList ();


				LoadSubQueryInfo (sqi.QueryInfo, featIdensPairList);

				var subfCompTableList = new List<AimComplexTable> ();
				var subFeatList = new List<Feature> ();
				var subFeatAssocCompTableList = new List<int> ();

				for (int i = 0; i <featIdensPairList.Count; i++)
				{
					var pair = featIdensPairList [i];

					var refComp = new AimRefComplex ();
					refComp.PropertyPath = sqi.PropertyPath;
					refComp.FeatureType = sqi.QueryInfo.FeatureType;
					refComp.ComplexTable = CreateComplexTable (sqi.QueryInfo.FeatureType, pair.RefFeatures);

					var assocCompTable = compTableList [featAssocCompTableList [i]];
					var row = FindComplexTable (pair.Feature, assocCompTable);
					row.RefQueryList.Add (refComp);

					subfCompTableList.Add (refComp.ComplexTable);
					subFeatList.AddRange (pair.RefFeatures);

					int n = 0;
					while ((n++) < pair.RefFeatures.Count)
						subFeatAssocCompTableList.Add (subfCompTableList.Count - 1);
				}

				qiFeatDict.AddQueryInfoFeatures (sqi.QueryInfo, subFeatList);

				LoadFeatQueryInfo (sqi.QueryInfo, subFeatList, subFeatAssocCompTableList, subfCompTableList, qiFeatDict);
			}

			#endregion

			#region Ref Query

			foreach (var rqi in qi.RefQueries)
			{
				var featIdensPairList = featList.Select (f => new AimRefFeatIdensPair () { Feature = f }).ToList ();

				LoadRefQueryInfo (rqi.QueryInfo, featIdensPairList, rqi.PropertyPath);

				var refCompTableList = new List<AimComplexTable> ();
				var refFeatList = new List<Feature> ();
				var refAssocFeatCompTableList = new List<int> ();

				for (int i = 0; i <featIdensPairList.Count; i++)
				{
					var pair = featIdensPairList [i];
					var refComp = new AimRefComplex ();
					refComp.PropertyPath = rqi.PropertyPath;
					refComp.FeatureType = rqi.QueryInfo.FeatureType;
					refComp.ComplexTable = CreateComplexTable (rqi.QueryInfo.FeatureType, pair.RefFeatures);

					var assocCompTable = compTableList [featAssocCompTableList [i]];
					var row = FindComplexTable (pair.Feature, assocCompTable);
					row.RefQueryList.Add (refComp);

					refCompTableList.Add (refComp.ComplexTable);
					refFeatList.AddRange (pair.RefFeatures);

					int n = 0;
					while ((n++) < pair.RefFeatures.Count)
						refAssocFeatCompTableList.Add (refCompTableList.Count - 1);
				}

				qiFeatDict.AddQueryInfoFeatures (rqi.QueryInfo, refFeatList);

				LoadFeatQueryInfo (rqi.QueryInfo, refFeatList, refAssocFeatCompTableList, refCompTableList, qiFeatDict);
			}

			#endregion
		}

		public static void LoadSubQueryInfo (QueryInfo qi, List<AimSubFeatIdensPair> featIdensPairList)
		{
			Filter filter = null;
			List<Feature> featList = null;

			var identifierList = new List<Guid> ();
			foreach (var item in featIdensPairList)
				identifierList.AddRange (item.Identifiers);

			if (identifierList.Count == 0)
			{
				featList = new List<Feature> ();
			}
			else
			{
				ComparisonOps compOp = new ComparisonOps ();
				compOp.OperationType = ComparisonOpType.In;
				compOp.PropertyName = "Identifier";
				compOp.Value = identifierList;

				filter = new Filter (new OperationChoice (compOp));

				if (qi.Filter != null)
				{
					BinaryLogicOp blo = new BinaryLogicOp ();
					blo.Type = BinaryLogicOpType.And;
					blo.OperationList.Add (new OperationChoice (compOp));
					blo.OperationList.Add (qi.Filter.Operation);
					filter = new Filter (new OperationChoice (blo));
				}
			}

			if (featList == null)
				featList = GetFeatures (qi.FeatureType, filter);

			foreach (var pair in featIdensPairList)
			{
				foreach (var f in featList)
				{
					if (pair.Identifiers.Contains (f.Identifier))
						pair.AddRefFeature (f);
				}
			}
		}

		public static void LoadRefQueryInfo (QueryInfo qi, List<AimRefFeatIdensPair> featIdensPairList, string propertyPath)
		{
			Filter filter = null;
			List<Feature> featList = null;

			var identifierList = new List<Guid> ();
			foreach (var item in featIdensPairList)
				identifierList.Add (item.Feature.Identifier);

			if (identifierList.Count == 0)
			{
				featList = new List<Feature> ();
			}
			else
			{
				ComparisonOps compOp = new ComparisonOps ();
				compOp.OperationType = ComparisonOpType.In;
				compOp.PropertyName = propertyPath;
				compOp.Value = identifierList;

				filter = new Filter (new OperationChoice (compOp));

				if (qi.Filter != null)
				{
					BinaryLogicOp blo = new BinaryLogicOp ();
					blo.Type = BinaryLogicOpType.And;
					blo.OperationList.Add (new OperationChoice (compOp));
					blo.OperationList.Add (qi.Filter.Operation);
					filter = new Filter (new OperationChoice (blo));
				}
			}

			if (featList == null)
				featList = GetFeatures (qi.FeatureType, filter);

			var innerPropInfos = AimMetadataUtility.GetInnerProps ((int) qi.FeatureType, propertyPath);

			foreach (var pair in featIdensPairList)
			{
				foreach (var feat in featList)
				{
					if (IsRefPropertyEqual (feat, innerPropInfos, pair.Feature.Identifier))
					{
						pair.AddRefFeature (feat);
					}
				}
			}
		}


		public static void LoadComplexShapefile (QueryInfo queryInfo)
		{
			var acl = new AimComplexLayer ();
			acl.Name = queryInfo.Name;
			acl.LayerDescription = acl.Name;
			acl.BaseOnQueryInfo = queryInfo;

			LayerLoader.ReLoadComplex (acl, false);

			var layerInfoList = new List<FeatLayerInfo> ();
			LoadComplexTable (acl.ComplexTable, layerInfoList);
		}

		private static void LoadComplexTable (AimComplexTable compTable, List<FeatLayerInfo> layerInfoList)
		{
			var features = compTable.Rows.Select (row => row.Row.AimFeature);

			AimFLGlobal.GetFeatureClasses (
				compTable.FeatureType,
				features,
				compTable.ShapeInfoList,
				layerInfoList);

			foreach (var compRow in compTable.Rows)
			{
				foreach (var item in compRow.SubQueryList)
				{
					LoadComplexTable (item.ComplexTable, layerInfoList);
				}

				foreach (var item in compRow.RefQueryList)
				{
					LoadComplexTable (item.ComplexTable, layerInfoList);
				}
			}
		}


		private static List<Feature> GetFeatures (FeatureType featureType, Filter filter)
		{
			lock (Globals.Environment.DbProvider)
			{
				var dbPro = Globals.Environment.DbProvider as Aran.Aim.Data.IDbProvider;
				var gr = dbPro.GetVersionsOf (
					featureType,
					Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
					Guid.Empty,
					true,
					null,
					null,
					filter);

				if (!gr.IsSucceed)
				{
					throw new Exception (gr.Message);
				}

				return gr.GetListAs<Feature> ();
			}
		}

		private static List<Guid> GetPropGuidValues (Feature feature, string propertyPath)
		{
			AimPropInfo [] pathPropInfos = AimMetadataUtility.GetInnerProps ((int) feature.FeatureType, propertyPath);

			List<Guid> guidList = new List<Guid> ();

			var aimPropValList = AimMetadataUtility.GetInnerPropertyValue (feature, pathPropInfos);

			foreach (IAimProperty aimProp in aimPropValList)
			{
				Guid guid = Guid.Empty;

				if (aimProp is IAbstractFeatureRef)
					guid = (aimProp as IAbstractFeatureRef).Identifier;
				else if (aimProp is FeatureRef)
					guid = (aimProp as FeatureRef).Identifier;

				else if (aimProp.PropertyType == AimPropertyType.List)
				{
					foreach (AObject obj in (aimProp as IList))
					{
						if (obj is FeatureRefObject)
						{
							guidList.Add ((obj as FeatureRefObject).Feature.Identifier);
						}
						else
						{
							IAimProperty featProp = (obj as IAimObject).GetValue ((int) Aran.Aim.PropertyEnum.PropertyAbstractFeatureRefObject.Feature);
							FeatureRef fr = featProp as FeatureRef;
							if (fr != null)
							{
								guidList.Add (fr.Identifier);
							}
						}
					}
				}

				else if (aimProp is AbstractFeatureRefBase)
					guid = (aimProp as AbstractFeatureRefBase).Identifier;

				if (guid != Guid.Empty && !guidList.Contains (guid))
					guidList.Add (guid);
			}

			return guidList;
		}

		private static bool IsRefPropertyEqual (Feature feat, AimPropInfo [] innerPropInfos, Guid refIdentifier)
		{
			var aimPropValList = AimMetadataUtility.GetInnerPropertyValue (feat, innerPropInfos, true);

			for (int i = 0; i < aimPropValList.Count; i++)
			{
				if (aimPropValList [0] is FeatureRef)
				{
					var fr = aimPropValList [0] as FeatureRef;
					if (fr.Identifier == refIdentifier)
						return true;
				}
			}

			return false;
		}

		private static AimComplexRow FindComplexTable (Feature feat, AimComplexTable compTable)
		{
			foreach (var row in compTable.Rows)
			{
				if (row.Row.AimFeature == feat)
					return row;
			}

			return null;
		}

		private static AimComplexTable CreateComplexTable (FeatureType featType, List<Feature> featList)
		{
			var compTable = new AimComplexTable ();
			compTable.FeatureType = featType;

			foreach (var feat in featList)
			{
				var compRow = new AimComplexRow ();
				compRow.Row = new AimRow ();
				compRow.Row.AimFeature = feat;
				compTable.Rows.Add (compRow);
			}

			return compTable;
		}

	}

	public class AimRefFeatIdensPair
	{
		public AimRefFeatIdensPair ()
		{
			RefFeatures = new List<Feature> ();
		}

		public Feature Feature { get; set; }

		public List<Feature> RefFeatures { get; private set; }

		public void AddRefFeature (Feature feat)
		{
			var existsCount = RefFeatures.Count (f => f.Identifier == feat.Identifier);
			if (existsCount == 0)
				RefFeatures.Add (feat);
		}
	}

	public class AimSubFeatIdensPair : AimRefFeatIdensPair
	{
		public List<Guid> Identifiers { get; set; }
	}

	public class QIFeatDict : Dictionary<QueryInfo, List<Feature>>
	{
		public void AddQueryInfoFeatures (QueryInfo qi, List<Feature> featList)
		{
			List<Feature> fl;
			if (!TryGetValue (qi, out fl))
			{
				fl = new List<Feature> ();
				Add (qi, fl);
			}
			fl.AddRange (featList);
		}
	}

	public class FeatClassLoaderInfo
	{
		public FeatClassLoaderInfo ()
		{
			FeatureList = new List<Feature> ();
		}

		public FeatureType FeatureType { get; set; }

		public List<Feature> FeatureList { get; private set; }

		public List<TableShapeInfo> ShapeInfos { get; set; }
	}
}
