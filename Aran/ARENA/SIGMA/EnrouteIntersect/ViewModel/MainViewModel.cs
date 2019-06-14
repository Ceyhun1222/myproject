using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Threading;
using EnrouteIntersect.Model;
using EnrouteIntersect.View;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace EnrouteIntersect.ViewModel
{
	public class MainViewModel : ViewModel
	{
		BackgroundWorker bgWorker;
		private bool _loading, _isRestorable;
		private string _crossedItemsText;
		private bool _checkedAll;
		private ObservableCollection<CrossingRouteSegment> _crossRoutes;
		private Dispatcher _uiDispatcher;

		public MainViewModel ( Dispatcher uiDispatcher)
		{
			_uiDispatcher = uiDispatcher;
			GlobalParams.Radius = 5;
			CrossRoutes = new ObservableCollection<CrossingRouteSegment> ( );
			bgWorker = new BackgroundWorker ( );
			IsLoading = true;
			GlobalParams.SpatialOperation = new SpatialReferenceOperation ( );
			GlobalParams.Graphics = new Graphics ( );

			bgWorker.WorkerReportsProgress = true;
			bgWorker.DoWork += new DoWorkEventHandler ( bgWorker_DoWork );
			bgWorker.ProgressChanged += new ProgressChangedEventHandler ( bgWorker_ProgressChanged );
			bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler ( bgWorker_RunWorkerCompleted );
			bgWorker.RunWorkerAsync ( );

			ApplyCommand = new RelayCommand ( new Action<object> ( Apply ) );
		}

		#region Properties

		public RelayCommand ApplyCommand
		{
			get;
			set;
		}

		private CrossingRouteSegment _selectedRoute;

		public CrossingRouteSegment SelectedRoute
		{
			get
			{
				return _selectedRoute;
			}
			set
			{
				_selectedRoute = value;
				NotifyPropertyChanged ( "SelectedRoute" );
				_selectedRoute.Draw ( );
			}
		}
		
		public bool IsLoading
		{
			get
			{
				return _loading;
			}
			set
			{
				_loading = value;
				NotifyPropertyChanged ( "IsLoading" );
			}
		}

		public bool IsRestorable
		{
			get
			{
				if ( SelectedRoute != null && SelectedRoute.Modified == ModifiedShape.None )
					return false;
				return true;
			}			
		}

		public double Radius
		{
			get
			{
				return GlobalParams.Radius;
			}
			set
			{
				GlobalParams.Radius = value;
				NotifyPropertyChanged ( "Radius" );
				foreach ( var item in CrossRoutes )
				{
					if ( item.IsSelected )
						item.Draw ( );
					else if ( item.IsChecked )
						item.Recreate ( );
				}
			}
		}

		public bool CheckedAll
		{
			get
			{
				return _checkedAll;
			}
			set
			{
				_checkedAll = value;
				foreach ( var item in CrossRoutes )
				{
					item.IsChecked = _checkedAll;
				}
			}
		}

		public string CrossedItemsText
		{
			get
			{
				return _crossedItemsText;
			}
			set
			{
				_crossedItemsText = value;
				NotifyPropertyChanged ( "CrossedItemsText" );
			}
		}		

		public ObservableCollection<CrossingRouteSegment> CrossRoutes
		{
			get
			{
				return _crossRoutes;
			}
			set
			{
				_crossRoutes = value;
				NotifyPropertyChanged ( "CrossRoutes" );
			}
		}

		#endregion

		#region Methods 

		public void Initialize ( int ownerId )
		{
			MainWindow mainView = new MainWindow ( );
			mainView.DataContext = this;
			var helper = new WindowInteropHelper ( mainView );
			helper.Owner = new IntPtr ( ownerId );
			ElementHost.EnableModelessKeyboardInterop ( mainView );
			mainView.ShowInTaskbar = false; // hide from taskbar and alt-tab list
			mainView.Show ( );
		}
		
		public void Clear ( )
		{
			GlobalParams.Graphics.Clear ( );
			((IActiveView)GlobalParams.HookHelper.FocusMap).Refresh ( );
		}		

		private void bgWorker_RunWorkerCompleted ( object sender, RunWorkerCompletedEventArgs e )
		{
			IsLoading = false;
			CrossedItemsText = "Found : " + CrossRoutes.Count;
		}

		private void bgWorker_ProgressChanged ( object sender, ProgressChangedEventArgs e )
		{
			object value = e.UserState;
			if ( value is CrossingRouteSegment )
			{
				_uiDispatcher.Invoke ( DispatcherPriority.Background, new Action (
( ) =>
{
	CrossRoutes.Add ( ( CrossingRouteSegment ) value );
} ) );
				
			}
			else
				CrossedItemsText = ( string ) e.UserState;
		}

		private void bgWorker_DoWork ( object sender, DoWorkEventArgs e )
		{
			var map = GlobalParams.HookHelper.FocusMap;
            GlobalParams.RouteSegmentLayer = FindLayer(map, "RouteSegment", false);
			if ( GlobalParams.RouteSegmentLayer == null )
				return;
			List<RouteSegment> featureList = GetItems ( GlobalParams.RouteSegmentLayer );
			RouteSegment routeSeg1, routeSeg2;
			IClone clone;
			IPoint pnt;
			//IRelationalOperator relationOper;
			for ( int i = 0; i < featureList.Count - 1; i++ )
			{
				routeSeg1 = featureList[ i ];
				//IRelationalOperator relationOper = ( IRelationalOperator ) routeSeg1.Shape;
				ITopologicalOperator topOperator = ( ITopologicalOperator ) routeSeg1.Shape;
				for ( int j = i + 1; j < featureList.Count; j++ )
				{
					routeSeg2 = featureList[ j ];

					IGeometry resultGeom = ( IGeometry ) topOperator.Intersect ( routeSeg2.Shape, esriGeometryDimension.esriGeometry0Dimension );
					//if (!relationOper.Disjoint(routeSeg2.Shape))
					if ( !resultGeom.IsEmpty )
					{
						IGeometryCollection pointCollection = ( IGeometryCollection ) resultGeom;
						pnt = ( IPoint ) pointCollection.get_Geometry ( 0 );
						if ( IsEdgePoint ( pnt, routeSeg1.Shape, routeSeg2.Shape ) )
							continue;
						CrossingRouteSegment crossRoute = new CrossingRouteSegment ( );
						crossRoute.IntersectPoint = pnt;
						crossRoute.LeftEnroute = routeSeg1.Route;
						crossRoute.LeftRouteSegment = routeSeg1.Designator;
						crossRoute.LeftRouteSegmentId = routeSeg1.Id;
						clone = ( IClone ) routeSeg1.Shape;
						crossRoute.LeftShapeGeo = ( IPolyline ) clone.Clone ( );

						crossRoute.RightEnroute = routeSeg2.Route;
						crossRoute.RightRouteSegment = routeSeg2.Designator;
						crossRoute.RightRouteSegmentId = routeSeg2.Id;
						clone = ( IClone ) routeSeg2.Shape;
						crossRoute.RightShapeGeo = ( IPolyline ) clone.Clone ( );

						if ( routeSeg1.Modified )
							crossRoute.Modified = ModifiedShape.Left;
						else if ( routeSeg2.Modified )
							crossRoute.Modified = ModifiedShape.Right;
						bgWorker.ReportProgress ( 0, crossRoute );
					}
				}
				bgWorker.ReportProgress ( 0, "Checking " + ( i + 1 ).ToString ( ) + " of " + featureList.Count + "  ..." );
				//System.Threading.Thread.Sleep ( 50 );
			}
		}

		private bool IsEdgePoint ( IPoint pnt, IPolyline shape1, IPolyline shape2 )
		{
			IPointCollection pointColl = ( IPointCollection ) shape1;
			for ( int i = 0; i < pointColl.PointCount; i++ )
			{
				if ( IsEqual ( pnt, pointColl.Point[ i ] ) )
					return true;
			}
			pointColl = ( IPointCollection ) shape2;
			for ( int i = 0; i < pointColl.PointCount; i++ )
			{
				if ( IsEqual ( pnt, pointColl.Point[ i ] ) )
					return true;
			}
			return false;
		}

		private bool IsEqual ( IPoint pnt1, IPoint pnt2 )
		{
			double epislon = 0.00028;
			double diffX = Math.Abs ( pnt1.X - pnt2.X );
			double diffY = Math.Abs ( pnt1.Y - pnt2.Y );
			if ( diffX >= epislon && diffY >= epislon )
				return false;
			return true;
		}

		private List<RouteSegment> GetItems ( ILayer layer )
		{
			FeatureLayer featLayer = ( layer as FeatureLayer );
			IFeatureClass featClass = featLayer.FeatureClass;
			IFeatureCursor cursor = featClass.Search ( null, true );
			IFeature feature = cursor.NextFeature ( );

			List<RouteSegment> featureList = new List<RouteSegment> ( );
			int routeIndex = featClass.Fields.FindField ( "RouteFormed" );
			int designatorIndex = featClass.Fields.FindField ( "designator" );
			int modifiedIndex = featClass.Fields.FindField ( "modified" );
			IClone clone;
			int modified;
			while ( feature != null )
			{
				RouteSegment routeSeg = new RouteSegment ( )
				{
					Id = feature.OID,
					Route = feature.Value[ routeIndex ].ToString(),
					Designator = feature.Value[ designatorIndex ].ToString()					
				};
				modified = int.Parse ( feature.Value[ modifiedIndex ].ToString ( ) );
				clone = feature.Shape as IClone;
				if ( modified == 1 )
				{
					routeSeg.Modified = true;
					IPolyline tmp = ( IPolyline ) clone.Clone ( );
					IPointCollection pointColl = ( IPointCollection ) tmp;
					//IPolyline poly = new PolylineClass ( );
					//IPointCollection ptColl = ( IPointCollection ) poly;
					//ptColl.AddPoint ( pointColl.Point[0] );
					//ptColl.AddPoint ( pointColl.Point[ pointColl.PointCount -1 ] );
					pointColl.RemovePoints ( 1, pointColl.PointCount - 2 );
					routeSeg.Shape = tmp; //( IPolyline ) poly;
				}
				else
					routeSeg.Shape = ( IPolyline ) clone.Clone ( );
				featureList.Add ( routeSeg );
				feature = cursor.NextFeature ( );
			}
			return featureList;
		}

		private ILayer FindLayer ( IMap map, string layerName, bool isAnno )
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

		private void Apply ( object obj )
		{
			var doc = ( IMxDocument ) ( GlobalParams.HookHelper.Hook as IApplication ).Document;
			var map = GlobalParams.HookHelper.FocusMap;
			var workspace = ( ( IDataset ) map.Layer[ 0 ] ).Workspace;
			var workSpaceEdit = ( IWorkspaceEdit ) workspace;
			workSpaceEdit.StopEditing ( false );
			workSpaceEdit.StopEditOperation ( );
			workSpaceEdit.StartEditOperation ( );
			FeatureLayer featLayer = ( GlobalParams.RouteSegmentLayer as FeatureLayer );
			IFeatureClass featClass = featLayer.FeatureClass;
			IFeatureCursor cursor ;//= featClass.Search ( null, true );
			int designatorIndex = featClass.Fields.FindField ( "designator" );
			int modifiedIndex = featClass.Fields.FindField ( "modified" );
			foreach ( var crossRoute in CrossRoutes )
			{
				cursor = featClass.Search ( null, true );
				crossRoute.Save ( cursor, designatorIndex, modifiedIndex );
			}
			workSpaceEdit.StopEditOperation ( );
			( ( IActiveView ) map ).Refresh ( );
			base.Close ( );
		}		

		#endregion
	}
}