using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ESRI.ArcGIS.Framework;
using VisibilityTool.Model;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geometry;
using System.Drawing;
using ESRI.ArcGIS.Controls;
using System.ComponentModel;
using VisibilityTool;

namespace VisibilityTool.ViewModel
{
	public class MainViewModel : ViewModel
    {
        #region Fields

		//private ObservableCollection<string> _featureTypes;
		private ObservableCollection<string> _fields;
		private string _selectedField;
		//private string _selectedFeatureType;
		private MainWindow _window;
		private Dictionary<LayerTemplate, ObservableCollection<FeatModel>> _allItems;
		//private ObservableCollection<FeatModel> _allItems;
		private ObservableCollection<FeatModel> _visibleItems;
		private ObservableCollection<FeatModel> _unVisibleItems;
		private ObservableCollection<LayerTemplate> _layerTemplates;
		private LayerTemplate _selectedLayTemplate;
		//private const string airspace = "Airspace";

        #endregion

		public MainViewModel ( MainWindow window, ObservableCollection<LayerTemplate> dataTemplates )
		{
			_window = window;
			//_allItems = new ObservableCollection<FeatModel> ();
			_allItems = new Dictionary<LayerTemplate, ObservableCollection<FeatModel>> ( );
			_visibleItems = new ObservableCollection<FeatModel> ();
			_unVisibleItems = new ObservableCollection<FeatModel> ();
			GlobalParams.SpatialOperation = new SpatialReferenceOperation ( );
			GlobalParams.Graphics = new Graphics (GlobalParams.HookHelper.ActiveView);

			ApplyCommand = new RelayCommand (new Action<object> (Apply));
            ShowCommand = new RelayCommand(new Action<object>(Show));
            HideCommand = new RelayCommand(new Action<object>(Hide));
			LayerTemplates = dataTemplates;
		}

		#region Properties

        public ObservableCollection<FeatModel> VisibleItems => _visibleItems;

        public ObservableCollection<FeatModel> UnVisibleItems => _unVisibleItems;

        public string VisibleCount => VisibleItems.Count.ToString();

        public string UnvisibleCount => UnVisibleItems.Count.ToString();

        public ObservableCollection<string> Fields
		{
			get => _fields;
            private set
			{
				_fields = value;
				NotifyPropertyChanged ( nameof(Fields) );
			}
		}

		public string SelectedField
		{
			get => _selectedField;
		    set
			{
				_selectedField = value;
				if ( VisibleItems.Count != 0 )
				{
					CollectionView view = ( CollectionView ) CollectionViewSource.GetDefaultView ( _window.listBxVisible.ItemsSource );
					view.GroupDescriptions.Clear ( );
					if ( _selectedField != "None" )
					{
						PropertyGroupDescription groupDescription = new PropertyGroupDescription ( "CodeType" );
						view.GroupDescriptions.Add ( groupDescription );
					}
				}
				NotifyPropertyChanged ( nameof(SelectedField) );
			}
		}

		public ObservableCollection<LayerTemplate> LayerTemplates
		{
			get => _layerTemplates;
		    set
			{
				_layerTemplates = value;
				if ( value != null && value.Count > 0 )
					SelectedLayTemplate = _layerTemplates[ 0 ];
			}
		}

		public LayerTemplate SelectedLayTemplate
		{
			get => _selectedLayTemplate;
		    set
			{
				_selectedLayTemplate = value;
				GetFields ( );
				GetItems ( );
			    NotifyPropertyChanged(nameof(SelectedLayTemplate));
            }
		}

        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand ShowCommand { get; set; }
        public RelayCommand HideCommand { get; set; }
        #endregion

		#region Methods

		private void Apply (object obj)
		{
			var doc = (IMxDocument) (GlobalParams.HookHelper.Hook as IApplication).Document;
			var map = GlobalParams.HookHelper.ActiveView.FocusMap;
			var workspace = ((IDataset)map.Layer[0]).Workspace;
			var workSpaceEdit = ( IWorkspaceEdit ) workspace;
			workSpaceEdit.StopEditOperation ( );
		    foreach ( var layerTemplate in LayerTemplates)
			{
				if ( !_allItems.ContainsKey ( layerTemplate ) )
					continue;
			    FeatureLayer featLayer;
			    IFeatureClass featClass;
			    IFeatureLayerDefinition2 def;
			    string ids, sql;
                //If LayerTemplate.ApplyRuleOn equals to 0(zero) means all, equals to 1 means PrimaryTable 
			    ILayer layer;
			    if (!layerTemplate.CanSplitLayers || layerTemplate.ApplyRuleOn == 1 || layerTemplate.ApplyRuleOn == 0)
			    {
			        layer = FindLayer(map, layerTemplate.PrimaryTableName, false);
			        if (layer == null)
			            continue;
			        featLayer = (layer as FeatureLayer);
			        featClass = featLayer.FeatureClass;
			        def = (IFeatureLayerDefinition2)featLayer;
			        _unVisibleItems = new ObservableCollection<FeatModel>(_allItems[layerTemplate].Where(it => !it.IsVisible).ToList());
			        ids = string.Join(",", _unVisibleItems.Select(item => "'" + item.Identifier + "'").ToList());
			        sql = "";
			        if (ids != "")
			            sql = layerTemplate.IdField + " NOT IN (" + ids + ")";
			        def.DefinitionExpression = sql;
			    }

			    RefLayer refLayer;
                for (int i =0; i< layerTemplate.RelatedLayers.Count;i++ )
                {
                    refLayer = layerTemplate.RelatedLayers[i];
                    if (!layerTemplate.CanSplitLayers || string.IsNullOrEmpty(refLayer.SplittedLayerName) || layerTemplate.ApplyRuleOn ==0 || layerTemplate.ApplyRuleOn == (i + 2))
                    {
                        layer = FindLayer(map, refLayer.TableName, refLayer.IsAnnotation);
                        if (!refLayer.IsAnnotation)
                        {
                            if (layer == null)
                                continue;
                            featLayer = (layer as FeatureLayer);
                            featClass = featLayer.FeatureClass;
                            def = (IFeatureLayerDefinition2) featLayer;
                            ids = string.Join(",",
                                _unVisibleItems.Select(item => "'" + item.Identifier + "'").ToList());
                            sql = "";
                            if (ids != "")
                                sql = refLayer.RefIdField + " NOT IN (" + ids + ")";
                            def.DefinitionExpression = sql;
                        }
                        else
                        {
                            featLayer = layer as FeatureLayer;
                            featClass = featLayer.FeatureClass;

                            int idIndex = featClass.Fields.FindField(refLayer.RefIdField);
                            int statusIndex = featClass.Fields.FindField("Status");
                            IFeatureCursor cursor = featClass.Search(null, true);
                            IFeature feature = cursor.NextFeature();
                            while (feature != null)
                            {
                                if (string.IsNullOrEmpty(feature.Value[idIndex].ToString()))
                                {
                                    feature = cursor.NextFeature();
                                    continue;
                                }
                                string id = feature.Value[idIndex].ToString();
                                var feat = _allItems[layerTemplate].FirstOrDefault(item => item.Identifier.Equals(id));
                                if (feat != null)
                                {
                                    if (feat.IsVisible)
                                        feature.Value[statusIndex] = esriAnnotationStatus.esriAnnoStatusPlaced;
                                    else
                                        feature.Value[statusIndex] = esriAnnotationStatus.esriAnnoStatusUnplaced;
                                    feature.Store();
                                }
                                feature = cursor.NextFeature();
                            }
                        }
                    }
				}
			}			
			((IActiveView) map).Refresh ();
			Close ();
		}

		private ILayer FindLayer (IMap map, string layerName, bool isAnno )
		{
			ILayer layer;
			for ( int i = 0; i < map.LayerCount; i++ )
			{
				layer = map.get_Layer ( i );
				if ( layer is GroupLayer )
				{
					ICompositeLayer group = ( ICompositeLayer ) layer;
					layer = SearchInGroup ( group, layerName, isAnno );
					if ( layer != null )
						return layer;
				}
				else
				{
					if(layer is FeatureLayer && (layer as FeatureLayer).FeatureClass != null && (layer as FeatureLayer).FeatureClass.AliasName == layerName)
					{
						if ( isAnno )
						{
							if ( layer is IAnnotationLayer )
								return layer;
						}
						else
						{
							if ( layer is FeatureLayer )
								return layer;
						}
					}
				}
			}
			return null;
		}

		private ILayer SearchInGroup ( ICompositeLayer group, string layerName, bool isAnno )
		{
			ILayer layer;
			for ( int j = 0; j < group.Count; j++ )
			{
				layer = group.Layer[ j ];
				if ( layer is GroupLayer )
				{
					ICompositeLayer childGroup = ( ICompositeLayer ) layer;
					layer = SearchInGroup ( childGroup, layerName, isAnno );
					if ( layer != null )
						return layer;
				}
				else
				{
					if ( layer.Name == layerName )
					{
						if ( isAnno )
						{
							if ( layer is IAnnotationLayer )
								return layer;
						}
						else
						{
							if ( layer is FeatureLayer )
								return layer;
						}
					}
				}
			}
			return null;
		}

        private void Show(object obj)
        {
			SetVisible (false);
			SplitItems ();
            NotifyVisibleProps();
        }

		private void SetVisible (bool visible)
		{
			foreach (var item in _allItems[_selectedLayTemplate])
			{
				if (item.IsSelected && item.IsVisible == visible)
				{ 
					item.IsVisible = !visible;
					item.IsSelected = false;
				}
			}
		}

		private void SplitItems ()
		{
			_visibleItems.Clear ();
			_unVisibleItems.Clear ();
			foreach (var item in _allItems[_selectedLayTemplate])
			{
				if (item.IsVisible)
					_visibleItems.Add (item);
				else
					_unVisibleItems.Add (item);
			}
		}

        private void Hide(object obj)
        {
            SetVisible(true);
            SplitItems();
            NotifyVisibleProps();
        }

        private void NotifyVisibleProps()
        {
            NotifyPropertyChanged(nameof(VisibleItems));
            NotifyPropertyChanged(nameof(UnVisibleItems));
            NotifyPropertyChanged(nameof(VisibleCount));
            NotifyPropertyChanged(nameof(UnvisibleCount));
        }

        private void GetFields ( )
		{
		    Fields = new ObservableCollection<string> {"None", SelectedLayTemplate.GroupByField};
		    SelectedField = Fields[ 0 ];
		}

        private void GetItems()
        {
            if (!_allItems.ContainsKey(SelectedLayTemplate))
            {
                var map = GlobalParams.HookHelper.ActiveView.FocusMap;
                ILayer layer = FindLayer(map, SelectedLayTemplate.PrimaryTableName, false);
                if (layer == null)
                    return;
                FeatureLayer featLayer = (layer as FeatureLayer);
                IFeatureClass featClass = featLayer.FeatureClass;
                IFeatureLayerDefinition2 def = (IFeatureLayerDefinition2) featLayer;
                string tt = def.DefinitionExpression;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = def.DefinitionExpression;
                IFeatureCursor cursor = featClass.Search(pQueryFilter, true);

                int idIndex = featClass.Fields.FindField(SelectedLayTemplate.IdField);
                int descriptionIndex = featClass.Fields.FindField(SelectedLayTemplate.DescriptField);

                int codeTypeIndex = -1;
                if (SelectedLayTemplate.GroupByField != null)
                    codeTypeIndex = featClass.Fields.FindField(SelectedLayTemplate.GroupByField);

                IFeature feature = cursor.NextFeature();
                IClone clone;
                _allItems.Add(SelectedLayTemplate, new ObservableCollection<FeatModel>());

                while (feature != null)
                {
                    FeatModel model = new FeatModel()
                    {
                        Identifier = feature.Value[idIndex].ToString()
                    };
                    //string t = feature.Value[idIndex].ToString();
                    //Guid idGuid;
                    //if (Guid.TryParse(t, out idGuid))
                    //    model.Identifier = new Guid(feature.Value[idIndex].ToString());
                    //else
                    //    model.Identifier = new Guid();
                    model.Description = feature.Value[descriptionIndex].ToString();
                    if (codeTypeIndex > -1)
                        model.CodeType = feature.Value[codeTypeIndex].ToString();

                    model.IsVisible = true;
                    if (feature.Shape != null)
                    {
                        clone = feature.Shape as IClone;
                        var cl = clone.Clone();
                        if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                            model.Shape = (IPolygon) cl;
                        else if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                            model.Shape = (IPolyline) cl;
                        else if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                            model.Shape = (IPoint) cl;
                        _allItems[SelectedLayTemplate].Add(model);
                    }

                    feature = cursor.NextFeature();
                }

                cursor = featClass.Search(null, true);
                feature = cursor.NextFeature();
                while (feature != null)
                {
                    string id = feature.Value[idIndex].ToString();
                    //Guid id;
                    //if (!Guid.TryParse(t, out id))
                    //    id = new Guid();
                    if (_allItems[SelectedLayTemplate]
                            .FirstOrDefault(item => item.Identifier == id && item.IsVisible) == null)
                    {
                        FeatModel model = new FeatModel
                        {
                            Identifier = id,
                            Description = feature.Value[descriptionIndex].ToString()
                        };
                        if (codeTypeIndex > -1)
                            model.CodeType = feature.Value[codeTypeIndex].ToString();
                        if (feature.Shape != null)
                        {
                            clone = feature.Shape as IClone;
                            var cl = clone.Clone();
                            model.Shape = cl as IPolygon;
                            model.IsVisible = false;
                            _allItems[SelectedLayTemplate].Add(model);
                        }
                    }

                    feature = cursor.NextFeature();
                }
            }

            SplitItems();
            NotifyVisibleProps();
        }

        public override void Close ()
		{
			base.Close ();
			Clear ();
		}

		public void Clear ()
		{
			GlobalParams.Graphics.Clear ();
			GlobalParams.HookHelper.ActiveView.Refresh ();
		}

		#endregion			
	}
}