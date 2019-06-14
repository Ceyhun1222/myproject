using System.Collections.Generic;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Queries;
using System;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class FixFacilities
	{
		public FixFacilities ( DesignatedPntSelection dsgPntSelection, MainController controller )
		{
			_controller = controller;
			_dsgPntSelect = dsgPntSelection;			
			//_servicesInitialized = false;
			_arp = new Point ( );
			_radius = 0;
			NavaidList = new List<Navaid> ( );
			dsgPntSelection.AddIntersectionDirectionChanged ( OnIntersectionDirectionChanged );
		}

		#region Add EventHandlers

		public void AddNavaidChangedEvent ( NavaidEventHandler onNavaidChanged )
		{
			_navaidChanged += onNavaidChanged;
		}

		public void AddNavListChangedEvent ( NavaidListEventHandler value )
		{
			_navaidListChanged += value;
		}

		public void AddIntersectingDirectionChangedEvent ( DirectionEventHandler onIntersectionDirectionChanged)
		{
			_intersectionDirectionChanged += onIntersectionDirectionChanged;
		}

		public void AddIntersectionVorListChangedEvent ( NavaidListEventHandler onIntersectionVorListChanged)
		{
			_intersectingVorListChanged += onIntersectionVorListChanged;
		}

		#endregion

		#region Set Values

		private void SetNavaidList ( )
		{
			if ( !_arp.IsEmpty /*&& _servicesInitialized */)
			{
			    Polygon polygon = new Polygon
			    {
			        ExteriorRing = ARANFunctions.CreateCirclePrj(GlobalParams.SpatialRefOperation.ToPrj<Point>(_arp), _radius)
			    };
				NavaidList = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes ( polygon, GlobalParams.SpatialRefOperation, _navaidServTypes );
			}
			else
				NavaidList.Clear ( );
		}

		public void SetServiceTypes ( CodeNavaidService [] serviceTypes, ProcedureTypeConv procTypeConv )
		{
			_navaidServTypes = serviceTypes;
			//_servicesInitialized = true;
			_procTypeConv = procTypeConv;
			SetNavaidList ( );
		}

		public void SetArp ( double x, double y )
		{
			_arp.X = x;
			_arp.Y = y;
		}

		public void SetRadius ( double radius )
		{
			_radius = radius;
		}

        /// <summary>
        /// Returns false if magnetic variation of selected VOR is null else returns true
        /// </summary>
        /// <param name="navaid"></param>
        /// <returns></returns>
		internal bool SetSelectedNavaid ( Navaid navaid )
		{
			if ( SelectedNavaid == navaid )
				return true;

            bool result = false;
			SelectedNavaid = navaid;
			List<NavaidPntPrj> navaidPntPrjList = new List<NavaidPntPrj> ( );

			if ( _procTypeConv == ProcedureTypeConv.Vordme )
			{
				if ( SelectedNavaid == null )
				{
				    _controller.SetError("Navaid is null", true);
                    Vor = null;
					Dme = null;				    
				}
				else
				{
					if ( SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR )
					{
						Vor = ( VOR ) SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
						Dme = ( DME ) SelectedNavaid.NavaidEquipment[ 1 ].TheNavaidEquipment.GetFeature ( );
					}
					else
					{
						Vor = ( VOR ) SelectedNavaid.NavaidEquipment[ 1 ].TheNavaidEquipment.GetFeature ( );
						Dme = ( DME ) SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
					}
				}
                if (Vor != null && Dme != null)
                {
                    result = _dsgPntSelect.SetNavaid(SelectedNavaid, Vor.MagneticVariation);                    
                }
                if (!result)
                    return result;
                navaidPntPrjList.Add(new NavaidPntPrj(VorPntPrj, NavType.Vor));
				navaidPntPrjList.Add ( new NavaidPntPrj ( DmePntPrj, NavType.Dme ) );

				NavaidEventArg argNav = new NavaidEventArg ( /*_procTypeConv, */ navaidPntPrjList );
				_navaidChanged ( null, argNav );
			}
			else if ( _procTypeConv == ProcedureTypeConv.VorNdb )
			{
				if ( SelectedNavaid == null )
				{
					Vor = null;
					Ndb = null;
					//navaidPntPrjList.Add ( null );
				}
				else
				{
					if ( SelectedNavaid.NavaidEquipment.Count > 1 )
					{
						if ( SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR )
						{
							Vor = ( VOR ) SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
                            result = _dsgPntSelect.ChangeMagneticValue(Vor.MagneticVariation);
							navaidPntPrjList.Add ( new NavaidPntPrj ( VorPntPrj, NavType.Vor ) );
						}
						else if ( SelectedNavaid.NavaidEquipment[ 1 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR )
						{
							Vor = ( VOR ) SelectedNavaid.NavaidEquipment[ 1 ].TheNavaidEquipment.GetFeature ( );
                            result = _dsgPntSelect.ChangeMagneticValue(Vor.MagneticVariation);
							navaidPntPrjList.Add ( new NavaidPntPrj ( VorPntPrj, NavType.Vor ) );
						}
						else if ( SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.NDB )
						{
							Ndb = ( NDB ) SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
                            result = _dsgPntSelect.ChangeMagneticValue(Ndb.MagneticVariation);
							navaidPntPrjList.Add ( new NavaidPntPrj ( NdbPntPrj, NavType.Ndb ) );
						}
						else if ( SelectedNavaid.NavaidEquipment[ 1 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.NDB )
						{
							Ndb = ( NDB ) SelectedNavaid.NavaidEquipment[ 1 ].TheNavaidEquipment.GetFeature ( );
                            result = _dsgPntSelect.ChangeMagneticValue(Ndb.MagneticVariation);
							navaidPntPrjList.Add ( new NavaidPntPrj ( NdbPntPrj, NavType.Ndb ) );
						}
					}
					else
					{
						if ( SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR )
						{
							Vor = ( VOR ) SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
                            result = _dsgPntSelect.ChangeMagneticValue(Vor.MagneticVariation);
							navaidPntPrjList.Add ( new NavaidPntPrj ( VorPntPrj, NavType.Vor ) );
						}
						else
						{
							Ndb = ( NDB ) SelectedNavaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
                            result = _dsgPntSelect.ChangeMagneticValue(Ndb.MagneticVariation);
							navaidPntPrjList.Add ( new NavaidPntPrj ( NdbPntPrj, NavType.Ndb ) );
						}
					}
				}
                
                if (!result)
                    return result;
				NavaidEventArg argNav = new NavaidEventArg ( /*_procTypeConv, */ navaidPntPrjList );
				_navaidChanged ( null, argNav );
			}
			else
			{
				if (SelectedNavaid == null)
					Vor = null;
				else
					Vor = (VOR) SelectedNavaid.NavaidEquipment.Find(
							navComp =>
								navComp.TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR
						).
						TheNavaidEquipment.GetFeature();

                if (Vor != null)
                    result = _dsgPntSelect.SetNavaid(SelectedNavaid, Vor.MagneticVariation);
                if (!result)
                    return false;
				//navaidPntPrjList.Add ( new NavaidPntPrj ( VorPntPrj, NavType.Vor ) );
				if ( !double.IsNaN ( _dsgPntSelect.Direction ) )
					SelectIntersectingVorList ( );
			}
            return true;
		}

		internal void SetAltitude ( double altitude )
		{
			_altitude = altitude;
		}

		internal void SetDirection ( double directionInDeg )
		{
			_dsgPntSelect.SetDirection ( directionInDeg, false, false );
			//SelectedDsgPntPrj = ARANFunctions.LocalToPrj ( VorPntPrj, Direction, NominalDistanceInPrj, 0 );
			SelectIntersectingVorList ( );
			//if ( _procTypeConv == ProcedureTypeConv.VORVOR )
			//    _dsgPntSelect.CalculateIntersectionVorRadialInterval ( );
		}

		internal void SetSelectedIntersectingNavaid ( Navaid intersectNavaid )
		{
			SelectedIntersectingNavaid = intersectNavaid;
			if ( SelectedIntersectingNavaid == null )
			{
				IntersectingVor = null;
				return;
			}
			IntersectingVor = ( VOR ) SelectedIntersectingNavaid.NavaidEquipment.Find (
				navComp => navComp.TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR).TheNavaidEquipment.GetFeature ( );
			_dsgPntSelect.SetIntersectVorMagVar ( IntersectingVor.MagneticVariation );
			List<NavaidPntPrj> navaidPntPrjList = new List<NavaidPntPrj>
			{
				new NavaidPntPrj(VorPntPrj, NavType.Vor),
				new NavaidPntPrj(IntersectingVorPntPrj, NavType.Vor)
			};

			NavaidEventArg argNav = new NavaidEventArg ( navaidPntPrjList );
			_navaidChanged ( null, argNav );

			_dsgPntSelect.SetIntersectingVorPnts ( IntersectingVorPntPrj );
		}

		#endregion

		#region Properties

		public Navaid SelectedNavaid
		{
			get; private set;
		}

		public Navaid SelectedIntersectingNavaid
		{
			get;
			private set;
		}

		private List<Navaid> NavaidList
		{
			get
			{
				return _navaidList;
			}

            set
            {
                //if ( value != null && value.Count == 0 )
                //    _navaidList = null;
                //else
                _navaidList = value;
                Comparison<Navaid> dgt = (nav1, nav2) => {
                    if (nav1.Designator == null || nav2.Designator == null)
                        return -1;
                    return String.Compare(nav1.Designator, nav2.Designator, StringComparison.Ordinal);
                };
                _navaidList.Sort(dgt);

	            _navaidListChanged?.Invoke(null, new NavaidListEventArg(_navaidList));
            }
		}

		public VOR Vor
		{
			get
			{
				return _vor;
			}
			private set
			{
				_vor = value;
				if ( _vor == null )
				{
                    
				    _controller.SetError("Vor is not defined in ("+SelectedNavaid?.Name + ")", true);
                    VorPntGeo = null;
					VorPntPrj = null;
				}
				else
				{
					VorPntGeo = _vor.Location.Geo; //GeomFunctions.GmlToAranPoint ( _vor );
					VorPntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( VorPntGeo );
				    if (VorPntGeo == null || VorPntGeo.IsEmpty || VorPntPrj == null || VorPntPrj.IsEmpty)
				        _controller.SetError("Location of Vor(" + _vor.Name + ") is empty", true);
				}
				_dsgPntSelect.SetVorPnts ( VorPntGeo, VorPntPrj );
			}
		}

		internal Point VorPntGeo
		{
			get; set;
		}

		public Point VorPntPrj
		{
			get
			{
				return _vorPntPrj;
			}
			private set
			{
				_vorPntPrj = value;
				_controller.DrawNavaid ( _vorPntPrj );
			}
		}

		public VOR IntersectingVor
		{
			get
			{
				return _intersectingVor;
			}

			private set
			{
				_intersectingVor = value;
				if ( _intersectingVor == null )
				{                    
					IntersectingVorPntGeo = null;
					IntersectingVorPntPrj = null;
				}
				else
				{
					IntersectingVorPntGeo = _intersectingVor.Location.Geo;
					IntersectingVorPntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( IntersectingVorPntGeo );
				    if (IntersectingVorPntGeo == null || IntersectingVorPntGeo.IsEmpty || IntersectingVorPntPrj == null || IntersectingVorPntPrj.IsEmpty)
				        _controller.SetError("Location of Vor("+ _intersectingVor.Name +") is empty",true);

                }
            }
		}

		private Point IntersectingVorPntGeo
		{
			get;
			set;
		}

		private Point IntersectingVorPntPrj
		{
			get;
			set;
		}

		public DME Dme
		{
			get
			{
				return _dme;
			}
			private set
			{
				_dme = value;
				if ( _dme == null )
				{
				    _controller.SetError("DME is not defined (" + SelectedNavaid?.Name + ") ", true);
					DmePntGeo = null;
					DmePntPrj = null;
				}
				else
				{

					DmePntGeo = _dme.Location.Geo;//GeomFunctions.GmlToAranPoint ( _dme );
					DmePntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( DmePntGeo );
				    if (DmePntGeo == null || DmePntGeo.IsEmpty || DmePntPrj == null || DmePntPrj.IsEmpty)
				        _controller.SetError("Location of Dme(" + _dme.Name + ") is empty", true);
				}
			    _dsgPntSelect.SetDmePnts(DmePntGeo);
			}
		}

		internal Point DmePntGeo
		{
			get; set;
		}

		private Point DmePntPrj
		{
			get
			{
				return _dmePntPrj;
			}
			set
			{
				_dmePntPrj = value;
				_dsgPntSelect.SetDmePntPrj ( _dmePntPrj );
			}
		}

		public NDB Ndb
		{
			get
			{
				return _ndb;
			}
			private set
			{
				_ndb = value;
				if ( _ndb == null )
				{
				    _controller.SetError("NDB is not defined (" + SelectedNavaid?.Name + ") ", true);
					NdbPntGeo = null;
					NdbPntPrj = null;
				}
				else
				{
					NdbPntGeo = _ndb.Location.Geo; //GeomFunctions.GmlToAranPoint ( _ndb );
					NdbPntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( NdbPntGeo );
				    if (NdbPntGeo == null || NdbPntGeo.IsEmpty || NdbPntPrj == null || NdbPntPrj.IsEmpty)
				        _controller.SetError("Location of Ndb(" + _ndb.Name + ") is empty",true);
				}
                _dsgPntSelect.SetNdbGeo(NdbPntGeo);
			}
		}

		private Point NdbPntGeo { get; set; }
	    

        private Point NdbPntPrj { get; set; }

		#endregion

		internal double GetNavaidElevation()
		{
			if(SelectedNavaid?.Location?.Elevation != null)
				return Converters.ConverterToSI.Convert(SelectedNavaid.Location.Elevation, 0);

			double result=0;
			double tmpElevation=0;
			if(_procTypeConv==ProcedureTypeConv.Vordme)
			{
				if(Vor?.Location?.Elevation != null)
					tmpElevation=Converters.ConverterToSI.Convert(Vor.Location.Elevation, 0);
				if(Dme?.Location?.Elevation != null)
					result=Converters.ConverterToSI.Convert(Dme.Location.Elevation, 0);
				if(result<tmpElevation)
					result=tmpElevation;
			}
			else if(_procTypeConv==ProcedureTypeConv.VorNdb)
			{
				if(Vor?.Location?.Elevation!=null)
					tmpElevation=Converters.ConverterToSI.Convert(Vor.Location.Elevation, 0);
				if(Ndb?.Location?.Elevation!=null)
					result=Converters.ConverterToSI.Convert(Ndb.Location.Elevation, 0);
				if(result<tmpElevation)
					result=tmpElevation;
			}
			else
			{
				if(Vor?.Location?.Elevation != null)
					result=Converters.ConverterToSI.Convert(Vor.Location.Elevation, 0);
			}
			return result;
		}

		private void OnIntersectionDirectionChanged ( object sender, DirectionEventArg argDir )
		{
			_intersectionDirectionChanged ( sender, argDir );
		}

		public double IntersectionDirection => _dsgPntSelect.IntersectionDirection;

		private void SelectIntersectingVorList ()
		{
			List<Navaid> result = new List<Navaid> ( );
			Polygon circleOverheadIntersectVor = new Polygon ( );
			LineString homingVorBeams = new LineString
			{
				ARANFunctions.LocalToPrj(VorPntPrj, _dsgPntSelect.Direction + GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					500000, 0),
				VorPntPrj,
				ARANFunctions.LocalToPrj(VorPntPrj, _dsgPntSelect.Direction - GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					500000, 0)
			};

			Feature feat  = null;
			foreach ( Navaid navaid in NavaidList )
			{
				if ( navaid.Identifier == SelectedNavaid.Identifier )
					continue;

				if ( navaid.NavaidEquipment[ 0 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR )
					feat = navaid.NavaidEquipment[ 0 ].TheNavaidEquipment.GetFeature ( );
				else if ( navaid.NavaidEquipment[ 1 ].TheNavaidEquipment.Type == Aim.NavaidEquipmentType.VOR )
					feat = navaid.NavaidEquipment[ 1 ].TheNavaidEquipment.GetFeature ( );
					
				if ( feat == null )
						continue;

				var navEquipPntGeo = ( ( VOR ) feat ).Location.Geo;
				var navEquipPntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( navEquipPntGeo );

				var side = ARANMath.SideDef ( VorPntPrj, _dsgPntSelect.Direction + GlobalParams.NavaidDatabase.Vor.TrackingTolerance, navEquipPntPrj );
				if ( side == SideDirection.sideOn )
					continue;

				if ( side == SideDirection.sideRight)
				{
					side = ARANMath.SideDef ( VorPntPrj, _dsgPntSelect.Direction - GlobalParams.NavaidDatabase.Vor.TrackingTolerance, navEquipPntPrj );
					if ( side != SideDirection.sideRight)
						continue;
				}

				var radius = _altitude * Math.Tan ( ARANMath.DegToRad ( 50 ) );
				circleOverheadIntersectVor.Clear ( );
				Ring rng = ARANFunctions.CreateCirclePrj ( navEquipPntPrj, radius );
				circleOverheadIntersectVor.ExteriorRing = rng;
				//_controller.DrawRing ( rng, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );

				var geom = GlobalParams.GeomOperators.Intersect ( circleOverheadIntersectVor, homingVorBeams );
				if (geom.Type == GeometryType.MultiPoint && ( ( MultiPoint ) geom ).Count > 0 )
					continue;
				if (geom.Type == GeometryType.MultiLineString && (( MultiLineString ) geom ).Count > 0 )
					continue;

				result.Add ( navaid );
			}
			//_controller.DrawLineString ( homingVorBeams, 1, 1 );

			int index = 0;
			if ( SelectedIntersectingNavaid != null )
			{
                index = result.FindIndex(nav => nav.Identifier == SelectedIntersectingNavaid.Identifier);
				if ( index == -1 )
					index = 0;
			}
			//if ( result.Count != 0 )
				_intersectingVorListChanged ( index, new NavaidListEventArg ( result ) );
		}

		private NavaidEventHandler _navaidChanged;
		private NavaidListEventHandler _navaidListChanged;
		private DirectionEventHandler _intersectionDirectionChanged;

		private VOR _vor, _intersectingVor;
		private DME _dme;
		private Point _dmePntPrj;
		private NDB _ndb;
		private List<Navaid> _navaidList;
		private CodeNavaidService [] _navaidServTypes;
		private double _radius;
		private readonly DesignatedPntSelection _dsgPntSelect;
		private ProcedureTypeConv _procTypeConv;
		private NavaidListEventHandler _intersectingVorListChanged;
		private Point _vorPntPrj;
		private readonly Point _arp;
		private readonly MainController _controller;
		private double _altitude;
	}
}
