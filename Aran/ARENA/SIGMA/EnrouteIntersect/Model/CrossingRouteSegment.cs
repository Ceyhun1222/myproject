using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using EnrouteIntersect.ViewModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace EnrouteIntersect.Model
{
	public class CrossingRouteSegment : INotifyPropertyChanged
	{
		private bool _isSelected, _isChecked;
		private IPolyline _arcShape;
		private bool _restored;

		public CrossingRouteSegment ( )
		{
			_restored = false;
			CircleValue = CircleType.leftUp;
			Modified = ModifiedShape.None;
			ChangeRotationCommand = new RelayCommand ( new Action<object> ( ChangeRotation ) );
			RestoreCommand = new RelayCommand ( new Action<object> ( Restore ) );
		}

		public string LeftEnroute
		{
			get;
			set;
		}

		public string RightEnroute
		{
			get;
			set;
		}

		public string LeftRouteSegment
		{
			get;
			set;
		}

		public string RightRouteSegment
		{
			get;
			set;
		}

		public IPolyline LeftShapeGeo
		{
			get;set;
		}

		public IPolyline RightShapeGeo
		{
			get; set;			
		}

		public string Header
		{
			get
			{
				return LeftRouteSegment + "    &    " + RightRouteSegment;
			}			
		}

		public string Detail
		{
			get
			{
				return "\t" + LeftEnroute + "\t\t" + LeftRouteSegment + " \r\n\t" + RightEnroute + "\t\t" + RightRouteSegment.ToString ( );
			}			
		}

		public ModifiedShape Modified
		{
			get;
			set;
		}				

		public IPoint IntersectPoint
		{
			get;
			set;
		}

		public CircleType CircleValue
		{
			get;
			set;
		}
		
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				if ( _isSelected )
				{
					Draw ( );
					this.PropertyChanged ( this, new PropertyChangedEventArgs ( "IsSelected" ) );
				}
			}
		}

		public bool IsChecked
		{
			get
			{
				return _isChecked;
			}
			set
			{
				_isChecked = value;
				if ( PropertyChanged != null )
					this.PropertyChanged ( this, new PropertyChangedEventArgs ( "IsChecked" ) );
			}
		}		

		private void DrawArcShape ( IPolyline shape, int isUp = 1 )
		{
			CreateArcShape ( shape, isUp );
			GlobalParams.Graphics.DrawMultiLineString ( ( IPolyline ) _arcShape );			
		}

		internal void Recreate ( )
		{
			switch ( CircleValue )
			{
				case CircleType.leftUp:
					CreateArcShape ( LeftShapeGeo );
					break;
				case CircleType.leftDown:
					CreateArcShape ( LeftShapeGeo, -1 );
					break;
				case CircleType.rightUp:
					CreateArcShape ( RightShapeGeo );
					break;
				case CircleType.rightDown:
					CreateArcShape ( RightShapeGeo, -1 );
					break;
			}
		}

		private void CreateArcShape ( IPolyline shape, int isUp = 1 )
		{			
			_arcShape = new PolylineClass ( );
			IPointCollection arcPointColl = ( IPointCollection ) _arcShape;
			IClone clone = IntersectPoint as IClone;
			var pntGeo = clone.Clone ( ) as IPoint;
			var pntPrj = ( IPoint ) GlobalParams.SpatialOperation.ToProject ( pntGeo );
			//GlobalParams.Graphics.DrawPointWithText ( pntPrj, "A", 3, 255 );
			
			clone = shape as IClone;
			IPolyline newArcGeo = clone.Clone ( ) as IPolyline;
			IPolyline prjGeo = ( IPolyline ) GlobalParams.SpatialOperation.ToProject ( newArcGeo );
			IPointCollection pointColl = ( IPointCollection ) prjGeo;
			arcPointColl.AddPoint ( pointColl.Point[ 0 ] );
			double angle = System.Math.Atan2 ( pointColl.Point[ 1 ].Y - pointColl.Point[ 0 ].Y, pointColl.Point[ 1 ].X - pointColl.Point[ 0 ].X );
			double dist = ARANMath.Hypot ( pointColl.Point[ 0 ].X - pntPrj.X, pointColl.Point[ 0 ].Y - pntPrj.Y );

			var view = ((IActiveView) GlobalParams.HookHelper.FocusMap);
			double radius = view.ScreenDisplay.DisplayTransformation.FromPoints(GlobalParams.Radius);

			IPoint startArcPnt = new PointClass ( );
			startArcPnt.PutCoords ( pointColl.Point[ 0 ].X + ( dist - radius ) * Math.Cos ( angle ), pointColl.Point[ 0 ].Y + ( dist - radius ) * Math.Sin ( angle ) );
			arcPointColl.AddPoint ( startArcPnt );

			for ( int i = 180; i > 2; i-=5 )
			{
				IPoint arcPnt = new PointClass ( );
				arcPnt.PutCoords ( pntPrj.X + radius * Math.Cos ( angle + isUp * ARANMath.DegToRad ( i ) ), pntPrj.Y + radius * Math.Sin ( angle + isUp * ARANMath.DegToRad ( i ) ) );
				arcPointColl.AddPoint ( arcPnt );
			}

			IPoint endArcPnt = new PointClass ( );
			endArcPnt.PutCoords ( pointColl.Point[ 0 ].X + ( dist + radius ) * Math.Cos ( angle ), pointColl.Point[ 0 ].Y + ( dist + radius ) * Math.Sin ( angle ) );
			arcPointColl.AddPoint ( endArcPnt );

			arcPointColl.AddPoint ( pointColl.Point[ 1 ] );			
		}

		public void Draw ( )
		{
			GlobalParams.Graphics.Clear ( );
						
			if ( _restored && Modified != ModifiedShape.None )
			{
				DrawLineShape ( RightShapeGeo );
				DrawLineShape ( LeftShapeGeo );
			}
			else
			{
				switch ( CircleValue )
				{
					case CircleType.leftUp:
						DrawArcShape ( LeftShapeGeo );
						DrawLineShape ( RightShapeGeo );
						break;
					case CircleType.leftDown:
						DrawArcShape ( LeftShapeGeo, -1 );
						DrawLineShape ( RightShapeGeo );
						break;
					case CircleType.rightUp:
						DrawArcShape ( RightShapeGeo );
						DrawLineShape ( LeftShapeGeo );
						break;
					case CircleType.rightDown:
						DrawArcShape ( RightShapeGeo, -1 );
						DrawLineShape ( LeftShapeGeo );
						break;
					default:
						break;
				}
			}
			((IActiveView)GlobalParams.HookHelper.FocusMap).Refresh ( );

			var pSelect = GlobalParams.RouteSegmentLayer as IFeatureSelection;
			if ( pSelect != null )
			{
				var application = GlobalParams.HookHelper.Hook as IApplication;
				if ( application != null )
				{
					pSelect.Clear ( );

					IQueryFilter queryFilter = new QueryFilterClass ( );
					queryFilter.WhereClause = "OBJECTID in (" + LeftRouteSegmentId + "," + RightRouteSegmentId + ")";

					pSelect.SelectFeatures ( queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false );
					UID menuId = new UIDClass ( );

					menuId.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}";
					ICommandItem pCmdItem = application.Document.CommandBars.Find ( menuId );
					pCmdItem.Execute ( );
					Marshal.ReleaseComObject ( pCmdItem );
					Marshal.ReleaseComObject ( menuId );
				}

			}

			((IGraphicsContainerSelect) ((IActiveView) GlobalParams.HookHelper.FocusMap).GraphicsContainer).UnselectAllElements();
		}		

		private void DrawLineShape ( IPolyline RightShapeGeo )
		{
			IClone clone = RightShapeGeo as IClone;
			IPolyline newOtherLineGeo = clone.Clone ( ) as IPolyline;
			IPolyline prjGeo = ( IPolyline ) GlobalParams.SpatialOperation.ToProject ( newOtherLineGeo );
			if ( prjGeo != null && !prjGeo.IsEmpty )
				GlobalParams.Graphics.DrawMultiLineString ( prjGeo );
		}

		private void ChangeRotation ( object obj )
		{

			CircleValue = ( ++CircleValue ) & CircleType.rightDown;
			_restored = false;
			Draw (  );
		}

		private void Restore ( object obj )
		{
			if ( Modified == ModifiedShape.Left )
				_arcShape = LeftShapeGeo;
			else if ( Modified == ModifiedShape.Right )
				_arcShape = RightShapeGeo;
			_restored = true;
			Draw ( );
		}

		internal void Save ( IFeatureCursor cursor, int designatorIndex, int modifiedIndex )
		{
			if ( _isChecked || Modified != ModifiedShape.None)
			{
				IPolyline lineShape = null;
				string arcDesignator = "";
				string lineDesignator = "";
				if ( !_restored)
				{
					arcDesignator = RightRouteSegment;
					lineDesignator = LeftRouteSegment;
					lineShape = LeftShapeGeo;
					if ( CircleValue == CircleType.leftDown || CircleValue == CircleType.leftUp )
					{
						arcDesignator = LeftRouteSegment;
						lineDesignator = RightRouteSegment;
						lineShape = RightShapeGeo;
					}
				}
				else 
				{
					arcDesignator = RightRouteSegment;
					if ( Modified == ModifiedShape.Left )
						arcDesignator = LeftRouteSegment;
				}

				IFeature feature = cursor.NextFeature ( );
				while ( feature != null )
				{
					if ( feature.Value[ designatorIndex ].ToString() == arcDesignator )
					{
						if ( _arcShape == null )
						{
							switch ( CircleValue )
							{
								case CircleType.leftUp:
									CreateArcShape ( LeftShapeGeo );
									break;
								case CircleType.leftDown:
									CreateArcShape ( LeftShapeGeo, -1 );
									break;
								case CircleType.rightUp:
									CreateArcShape ( RightShapeGeo );
									break;
								case CircleType.rightDown:
									CreateArcShape ( RightShapeGeo, -1 );
									break;
							}
						}
						IClone clone = ( IClone ) _arcShape;
						var _arcGeo = ( IPolyline ) clone.Clone ( );
						if ( !_restored )
						{
							feature.set_Value ( modifiedIndex, 1 );
							feature.Shape = GlobalParams.SpatialOperation.ToGeo ( _arcGeo );
						}
						else
						{
							feature.set_Value ( modifiedIndex, 0 );
							feature.Shape = _arcGeo;
						}
						feature.Store ( );						
					}
					else if ( feature.Value[ designatorIndex ].ToString() == lineDesignator && !_restored && Modified != ModifiedShape.None)
					{
						feature.set_Value ( modifiedIndex, 0 );
						feature.Shape = lineShape;
						feature.Store ( );	
					}
					feature = cursor.NextFeature ( );
				}
			}
		}

		public RelayCommand ChangeRotationCommand
		{
			get;
			set;
		}

		public RelayCommand RestoreCommand
		{
			get;
			set;
		}

		public int LeftRouteSegmentId { get; set; }
		public int RightRouteSegmentId { get; set; }

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		
		#endregion
	}

	public enum CircleType
	{
		leftUp = 0,
		leftDown = 1,
		rightUp = 2, 
		rightDown = 3
	}

	public enum ModifiedShape
	{
		None,
		Left,
		Right
	}
}