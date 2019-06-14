using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

using Aran.Aim.Data.Filters;
using AIM = Aran.Aim.Features;
using Aran.Package;
using MapEnv.Layers;

namespace MapEnv
{
	public class AimFeatureLayer : FeatureLayerClass, IPackable, IDisposable
	{
		private List<AIM.Feature> _aimFeatures;
		private Dictionary<Guid, AIM.Feature> _aimFeatureDict;
		private List<Guid> _selectedFeatureList;

		public event EventHandler UpdateStarted;
		public event EventHandler UpdateEnded;
		public event MyFLVisibleChangedEventHandler MyFLVisibleChanged;


		public AimFeatureLayer ()
		{
			_aimFeatures = new List<AIM.Feature> ();
			_aimFeatureDict = new Dictionary<Guid, AIM.Feature> ();
			_selectedFeatureList = new List<Guid> ();

			LayerInfoList = new List<FeatLayerInfo> ();
			ShapeInfoList = new List<TableShapeInfo> ();
			IsLoaded = false;

			DbSpatialReference = Globals.CreateWGS84SR ();

			var layerEvent = this as ILayerEvents_Event;
			if (layerEvent != null)
				layerEvent.VisibilityChanged += OnVisibilityChanged;
		}

		public Aran.Aim.FeatureType FeatureType { get; set; }

		public Filter AimFilter { get; set; }

		public List<TableShapeInfo> ShapeInfoList { get; private set; }

		public IEnumerable<AIM.Feature> AimFeatures
		{
			get { return _aimFeatures; }
			set
			{
				_aimFeatures.Clear ();
				_aimFeatures.AddRange (value);

				_aimFeatureDict.Clear ();
				foreach (var feat in value)
				{
					_aimFeatureDict.Add (feat.Identifier, feat);
				}
			}
		}

		public List<AIM.Feature> GetOverPoints (IEnvelope mouseEnv, Dictionary<AIM.Feature, IGeometry> featGeomDict)
		{
			ISpatialFilter spatialFilter = new SpatialFilterClass ();
			spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;//.esriSpatialRelEnvelopeIntersects;
			spatialFilter.Geometry = mouseEnv;

			var aimFeatList = new List<AIM.Feature> ();

			foreach (var item in LayerInfoList)
			{
				var featClass = item.Layer.FeatureClass;
				spatialFilter.GeometryField = featClass.ShapeFieldName;
				IFeatureCursor featureCursor = featClass.Search (spatialFilter, false);
				IFeature feature = featureCursor.NextFeature ();
				var identifierIndex = featureCursor.FindField ("identifier");

				while (feature != null)
				{
					var identifierText = (string) feature.get_Value (identifierIndex);
					var identifier = new Guid (identifierText);

					if (IsComplex)
					{
						var aimFeat = FindFeatureInComplex (ComplexTable, identifier);
						if (aimFeat != null)
						{
							aimFeatList.Add (aimFeat);

							if (!featGeomDict.ContainsKey (aimFeat))
								featGeomDict.Add (aimFeat, feature.Shape);
						}
					}
					else
					{
						AIM.Feature aimFeat = null;
						if (_aimFeatureDict.TryGetValue (identifier, out aimFeat))
						{
							aimFeatList.Add (aimFeat);

							if (!featGeomDict.ContainsKey (aimFeat))
								featGeomDict.Add (aimFeat, feature.Shape);
						}
					}

					feature = featureCursor.NextFeature ();
				}
			}

			return aimFeatList;
		}

		public List<FeatLayerInfo> LayerInfoList { get; private set; }

		public void RemoveSubLayersAndFiles ()
		{
			var map = Globals.MainForm.Map;

			foreach (var item in LayerInfoList)
			{
				map.DeleteLayer (item.Layer);
			}

			foreach (var item in LayerInfoList)
			{
				var fileInfo = item.ShapefileInfo;
				var dir = fileInfo.DirectoryName;
				var name = dir + "\\" + fileInfo.Name;

				try
				{
					File.Delete (name + ".dbf");
					File.Delete (name + ".prj");
					File.Delete (name + ".shp");
					File.Delete (name + ".shx");
				}
				catch { }
			}
		}

		public void BeginUpdate ()
		{
			if (UpdateStarted != null)
				UpdateStarted (this, new EventArgs ());
		}

		public void EndUpdate ()
		{
			IsLoaded = true;
			if (UpdateEnded != null)
				UpdateEnded (this, new EventArgs ());
		}

		public void RefreshLayers ()
		{
			foreach (var item in LayerInfoList)
			{
				var featLayer = item.Layer;
				Globals.MainForm.ActiveView.PartialRefresh (esriViewDrawPhase.esriViewGeography, featLayer, null);
			}
		}

        public IEnvelope GetAreaOfInterest()
        {
            var env = new Envelope() as IEnvelope;
            env.SpatialReference = Globals.MainForm.Map.SpatialReference;

            foreach (var item in LayerInfoList) {
                env.Union(item.Layer.AreaOfInterest);
            }

            //if (!env.IsEmpty)
            //    env.Expand (0.1, 0.1, false);

            return env;
        }

		public List<IGeometry> GetShapes (AIM.Feature feature)
		{
			var geomList = new List<IGeometry> ();
			var queryFilter = new QueryFilter ();
			queryFilter.WhereClause = "\"identifier\" = '" + feature.Identifier + "'";

			foreach (var item in LayerInfoList)
			{
				var featLayer = item.Layer;
				var featCursor = featLayer.Search (queryFilter, false);
				IFeature esriFeature;
				while ((esriFeature = featCursor.NextFeature ()) != null)
				{
					geomList.Add (esriFeature.Shape);
				}
			}

			return geomList;
		}

		public List<IGeometry> GetShapes (AIM.Feature feature, TableShapeInfo shapeInfo)
		{
			var shapeIndex = ShapeInfoList.IndexOf (shapeInfo);

			if (shapeIndex == -1 || LayerInfoList.Count <= shapeIndex)
				return null;

			var queryFilter = new QueryFilter ();
			queryFilter.WhereClause = "\"identifier\" = '" + feature.Identifier + "'";

			var layerInfo = LayerInfoList [shapeIndex];
			var featCursor = layerInfo.Layer.Search (queryFilter, false);

			var geomList = new List<IGeometry> ();
			IFeature esriFeature;

			while ((esriFeature = featCursor.NextFeature ()) != null)
			{
				geomList.Add (esriFeature.Shape);
			}

			return geomList;
		}

		public ISpatialReference DbSpatialReference { get; set; }

		public bool AllAimFeatureSelected { get; private set; }

		public void SelectAimFature (Guid identifier, bool isSelect)
		{
			bool isAll = (identifier == default (Guid));
			var queryFilter = new QueryFilter ();

			if (!isSelect)
				AllAimFeatureSelected = false;

			if (isAll)
			{
				queryFilter.WhereClause = (isSelect ? "" : "FID = -1");
				_selectedFeatureList.Clear ();
				if (isSelect)
				{
					_selectedFeatureList.AddRange (_aimFeatureDict.Keys);
					AllAimFeatureSelected = true;
				}
			}
			else
			{
				var isInList = _selectedFeatureList.Contains (identifier);

				if (!isSelect && isInList)
				{
					_selectedFeatureList.Remove (identifier);
				}
				else if (!isInList)
				{
					_selectedFeatureList.Add (identifier);
				}

				var s = "FID = -1";
				if (_selectedFeatureList.Count > 0)
				{
					s = "\"identifier\" IN ('" + _selectedFeatureList [0] + "'";
					for (int i = 1; i < _selectedFeatureList.Count; i++)
					{
						s += ",'" + _selectedFeatureList [i] + "'";
					}

					s += ")";
				}
				queryFilter.WhereClause = s;
			}

			foreach (var item in LayerInfoList)
			{
				var featLayer = item.Layer;

				var featSelection = featLayer as IFeatureSelection;
				featSelection.Clear ();
				featSelection.SelectFeatures (queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
			}
		}

		public void SelectAimFature (IEnumerable<Guid> identifiers, bool isSelect)
		{
			var queryFilter = new QueryFilter ();
			var newIdensList = new List<Guid> (identifiers);

			var s = "FID = -1";
			if (newIdensList.Count > 0)
			{
				s = "\"identifier\" IN ('" + newIdensList [0] + "'";
				for (int i = 1; i < newIdensList.Count; i++)
				{
					s += ",'" + newIdensList [i] + "'";
				}

				s += ")";
			}
			queryFilter.WhereClause = s;

			foreach (var item in LayerInfoList)
			{
				var featLayer = item.Layer;

				var featSelection = featLayer as IFeatureSelection;
				featSelection.Clear ();
				featSelection.SelectFeatures (queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
			}
		}

		public void ShowHideFeature (List<Guid> identifierList, bool isVisible)
		{
			if (LayerInfoList.Count == 0)
				return;

			var s = "FID = -1";
			if (identifierList.Count > 0)
			{
				s = "\"identifier\" IN ('" + identifierList [0] + "'";
				for (int i = 1; i < identifierList.Count; i++)
				{
					s += ",'" + identifierList [i] + "'";
				}

				s += ")";
			}

			foreach (var item in LayerInfoList)
			{
				var fld = item.Layer as IFeatureLayerDefinition;
				fld.DefinitionExpression = s;
			}
		}

		public ReadOnlyCollection<Guid> GetSelectedGuids ()
		{
			return new ReadOnlyCollection<Guid> (_selectedFeatureList);
		}

		public void RefreshComplex ()
		{
			if (ComplexTable == null)
				return;


			var selFeatList = new List<AIM.Feature> ();
			GetSelectedFeatures (ComplexTable, selFeatList, true);
			var guidList = selFeatList.Select (f => f.Identifier);
			SelectAimFature (guidList, true);


			var visibleFeatList = new List<AIM.Feature> ();
			GetSelectedFeatures (ComplexTable, visibleFeatList, false);
			guidList = visibleFeatList.Select (f => f.Identifier);
			ShowHideFeature (new List<Guid> (guidList), true);
		}

		public bool IsLoaded { get; set; }

		#region Complex Data

		public void SetQueryInfo (QueryInfo qi)
		{
			IsComplex = true;
			FeatureType = qi.FeatureType;
			BaseQueryInfo = qi;
			Name = qi.Name;
			Visible = qi.IsVisible;

			ShapeInfoList.Clear ();
			GetShapeInfosFromQI (qi, ShapeInfoList);
		}

		public bool IsComplex { get; private set; }

		public QueryInfo BaseQueryInfo { get; private set; }

		public AimComplexTable ComplexTable { get; set; }

		#endregion

        public void Dispose()
        {
            _aimFeatures.Clear();
            _aimFeatureDict.Clear();
            _selectedFeatureList.Clear();
        }


		private void OnVisibilityChanged (bool currentState)
		{
			foreach (var item in LayerInfoList)
				item.Layer.Visible = currentState;

			if (BaseQueryInfo != null)
				BaseQueryInfo.IsVisible = currentState;

			if (MyFLVisibleChanged != null)
				MyFLVisibleChanged (this, new MyFLVisibleChangedEventArgs (currentState));
		}

		private void GetShapeInfosFromQI (QueryInfo qi, List<TableShapeInfo> shapeInfoList)
		{
			shapeInfoList.AddRange (qi.ShapeInfoList);

			foreach (var sqi in qi.SubQueries)
				GetShapeInfosFromQI (sqi.QueryInfo, shapeInfoList);
			foreach (var rqi in qi.RefQueries)
				GetShapeInfosFromQI (rqi.QueryInfo, shapeInfoList);
		}

		private void GetSelectedFeatures (AimComplexTable compTable, List<AIM.Feature> featList, bool selectedOrVisible)
		{
			foreach (var row in compTable.Rows)
			{
				if (selectedOrVisible)
				{
					if (row.Row.IsSelected)
						featList.Add (row.Row.AimFeature);
				}
				else
				{
					if (row.Row.IsVisible)
						featList.Add (row.Row.AimFeature);
				}

				foreach (var item in row.RefQueryList)
					GetSelectedFeatures (item.ComplexTable, featList, selectedOrVisible);

				foreach (var item in row.SubQueryList)
					GetSelectedFeatures (item.ComplexTable, featList, selectedOrVisible);
			}
		}

		private AIM.Feature FindFeatureInComplex (AimComplexTable compTable, Guid identifier)
		{
			foreach (var row in compTable.Rows)
			{
				if (row.Row.AimFeature.Identifier == identifier)
					return row.Row.AimFeature;

				foreach (var item in row.SubQueryList)
				{
					var feat = FindFeatureInComplex (item.ComplexTable, identifier);
					if (feat != null)
						return feat;
				}

				foreach (var item in row.RefQueryList)
				{
					var feat = FindFeatureInComplex (item.ComplexTable, identifier);
					if (feat != null)
						return feat;
				}
			}

			return null;
		}

		#region IPackable

		void IPackable.Pack (PackageWriter writer)
		{
			writer.PutBool (IsComplex);
			if (IsComplex)
			{
				BaseQueryInfo.Pack (writer);
			}
			else
			{
				writer.PutInt32 ((int) FeatureType);
				writer.PutString (Name);
				writer.PutBool (Visible);

				writer.PutInt32 (ShapeInfoList.Count);
				foreach (TableShapeInfo shapeInfo in ShapeInfoList)
					(shapeInfo as IPackable).Pack (writer);

				bool filterNotNull = (AimFilter != null);
				writer.PutBool (filterNotNull);
				if (filterNotNull)
					AimFilter.Operation.Pack (writer);
			}
		}

		void IPackable.Unpack (PackageReader reader)
		{
			var isComplex = reader.GetBool ();
			if (isComplex)
			{
				var qi = new QueryInfo ();
				qi.Unpack (reader);
				SetQueryInfo (qi);
			}
			else
			{
				FeatureType = (Aran.Aim.FeatureType) reader.GetInt32 ();
				Name = reader.GetString ();
				Visible = reader.GetBool ();

				int count = reader.GetInt32 ();
				for (int i = 0; i < count; i++)
				{
					TableShapeInfo shapeInfo = new TableShapeInfo ();
					(shapeInfo as IPackable).Unpack (reader);
					ShapeInfoList.Add (shapeInfo);
				}

				bool filterNotNull = reader.GetBool ();
                if (filterNotNull) {
                    var oc = OperationChoice.UnpackOperationChoice(reader);
                    AimFilter = new Filter(oc);
                }
			}
		}

		#endregion
	}

	public class FeatLayerInfo
	{
		public FileInfo ShapefileInfo { get; set; }
		public IFeatureLayer Layer { get; set; }
        public TableShapeInfo BaseOnShapeInfo { get; set; }
    }

	public delegate void MyFLVisibleChangedEventHandler (object sender, MyFLVisibleChangedEventArgs e);

	public class MyFLVisibleChangedEventArgs
	{
		public MyFLVisibleChangedEventArgs (bool isVisible)
		{
			IsVisible = isVisible;
		}

		public bool IsVisible { get; private set; }
	}
}
