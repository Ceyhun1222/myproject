using Aran.Geometries;
using System.Collections.Generic;
using Aran.Panda.Common;
using Aran.AranEnvironment.Symbols;
using System;
using System.Linq;
using Aran.Aim.Features;
using Aran.Queries;
using Aran.Panda.Conventional.Racetrack.Procedures;
using Aran.Aim.DataTypes;
using Aran.Converters;
using Aran.Aim.Enums;
using Aran.Panda.Conventional.Racetrack.Controller;

namespace Aran.Panda.Conventional.Racetrack
{
	public class ConventialRacetrack
	{
		public ConventialRacetrack ( )
		{
			Side = SideDirection.sideOn;
			EntryDirection = EntryDirection.None;
			_vorDme = new VORDME ( );
			_vorNdb = new VORNDB ( );
			_vorVor = new VorVor ();

			EntryDirection = EntryDirection.None;
			//_time = new Time ( );
			LimDist = new LimitingDistance ( EntryDirection);
			InbountTrack = new InboundTrack ( );
			DsgPntSelection = new DesignatedPntSelection ( LimDist, InbountTrack );
			//_dsgPntSelection.AddIntersectionDirectionChanged ( OnIntersectionDirectionChanged );
			Faclts = new FixFacilities ( DsgPntSelection );
			Faclts.AddIntersectingDirectionChangedEvent ( OnIntersectionDirectionChanged );
			Faclts.AddIntersectionVorListChangedEvent ( OnIntersectionVorListChanged );
			SlctdAirdromeArea = new SelectionAirdromeArea ( Faclts );
			Procedure = new Procedure ( Faclts );
			Speed = new Speed ( LimDist );

			SlctdAirdromeArea.AddOrganisationCreatedEvent ( OnOrganisationCreated );
			SlctdAirdromeArea.AddAirdromeChangedEvent ( OnAirportHeliportCreated );
			SlctdAirdromeArea.AddSelectedNavaidChangedEvent ( OnSelectedNavaidChanged );
			Faclts.AddNavListChangedEvent ( OnNavaidListChanged );
			DsgPntSelection.AddtDsgPntListChangedEvent ( OnDesignatedPointListChanged );
			DsgPntSelection.AddDsgPntChangedEvent ( OnDesignatedPointChanged );
			InbountTrack.AddDirectionChangedEvent ( OnDirectionChanged );
			DsgPntSelection.AddNominalDistanceChangedEvent ( OnNominalDistanceChanged );
			Speed.AddCategoryListChangedEvent ( OnCategoryListChanged );
			Speed.AddIASIntervalChangedEvent ( OnIASIntervalChanged );
			LimDist.AddTimeChangedEvent ( OnTimeChanged );
			LimDist.AddLimDistIntervalChangedEvent ( OnLimitDistIntervalChanged );
			LimDist.AddLimDistChangedEvent ( OnLimDistChanged );

			GUIController = new AppliedGUIValues ( );
			GUIController.AddAppliedValueChangedHandler ( OnAppliedValueChanged );
			//Report = new RacetrackReport ( );
			_dbResult = new DbResult ( );
			ProtectionSector1 = new Polygon ( );
			ProtectionSector2 = new Polygon ( );
			ProtectionRecipEntry = new Polygon ( );
            ProtectionInterRadEntryArea = new MultiPolygon();
		}

		#region Add EventHandlers

		public void AddOrganisationChangedEvent ( OrganisationEventHandler OnOrganisationChanged )
		{
			_organisationCreated += OnOrganisationChanged;
		}

		public void AddAirdromeChangedEvent ( AirportEventHandler OnAirdromeChanged )
		{
			_airportHeliportChanged += OnAirdromeChanged;
		}

		public void AddNavListChangedEvent ( NavaidListEventHandler OnNavaidListChanged )
		{
			_navaidListChanged += OnNavaidListChanged;
		}

		public void AddIntersectingNavListChangedHandler ( NavaidListEventHandler OnIntersectingNavaidListChanged )
		{
			_intersectingVorListChanged += OnIntersectingNavaidListChanged;
		}

		public void AddDsgPointListChangedEvent ( DsgPntListEventHandler OnDsgPntListChanged )
		{
			_dsgPntListChanged += OnDsgPntListChanged;
		}

		public void AddDesignatedPointChangedEvent ( DsgPntEventHandler OnDsgPntChanged )
		{
			_dsgPntChanged += OnDsgPntChanged;
		}

		public void AddDirectionChangedEvent ( DirectionEventHandler OnDirectionChanged )
		{
			_directionChanged += OnDirectionChanged;
		}

		public void AddIntersectionDirectionChangedEvent ( DirectionEventHandler OnIntersectionDirectionChanged )
		{
			_intersectionDirectionChanged += OnIntersectionDirectionChanged;
		}

		public void AddNominalDistanceChangedEvent ( DistanceEventHandler OnNominalDistanceChanged )
		{
			_nominalDistanceChanged += OnNominalDistanceChanged;
		}

		public void AddIASIntervalChangedEvent ( IntervalEventHandler OnIASIntervalChanged )
		{
			_iasIntervalChanged += OnIASIntervalChanged;
		}

		public void AddCategoryListChangedEvent ( CategoryListEventHandler OnCtgListChanged )
		{
			_ctgListChanged += OnCtgListChanged;
		}

		public void AddTimeChangedEvent ( CommonEventHandler OnTimeChanged )
		{
			_timeChanged += OnTimeChanged;
		}

		public void AddLimitDistIntervalChangedEvent ( IntervalEventHandler OnLimDistIntervalChanged )
		{
			_limDistIntervalChanged += OnLimDistIntervalChanged;
		}

		public void AddLimDistChangedEvent ( CommonEventHandler OnLimDistChanged )
		{
			_limDistChanged += OnLimDistChanged;
		}

		public void AddRE_AngleChangedEvent ( CommonEventHandler OnRE_AngleChanged )
		{
			_RE_AngleChanged += OnRE_AngleChanged;
		}

		public void AddAppliedValueChangedEvent ( AppliedValueChangedEventHandler OnAppliedValueChanged )
		{
			_appliedValueChanged += OnAppliedValueChanged;
		}

		#endregion

		#region Set Values

		public void SetNavaid ( Navaid navaid )
		{
			double elevation = Aran.Converters.ConverterToSI.Convert ( navaid.Location.Elevation, 0 );
			if ( Procedure.Type != ProcedureTypeConv.VORVOR )
				Altitude = Altitude - elevation;
			Faclts.SetNavaid ( navaid, Altitude );
			//DrawNavaids ( );
			GUIController.SetNavaid ( Faclts.SelectedNavaid );
		}

		internal double[] SetIntersectingVor ( Navaid navaid )
		{
			double [] result = Faclts.SetIntersectingVor ( navaid );
			//DrawIntersectingVor ( );
			_priorFixtolerance = result [ 0 ];
			_postFixTolerance = result [ 1 ];
			GUIController.SetIntersectingVor ( Faclts.IntersectingVor );
			return result;
		}

		private bool IsEqual ( List<NavaidPntPrj> appliedNavList, List<NavaidPntPrj> newNavList )
		{
			if ( Procedure.Type == ProcedureTypeConv.VORDME )
			{
				return ( appliedNavList [ 0 ] == newNavList [ 0 ] || appliedNavList [ 1 ] == newNavList [ 1 ] );
			}
			else if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
			{
				return ( appliedNavList [ 0 ] == newNavList [ 0 ] );
			}
			else if ( Procedure.Type == ProcedureTypeConv.VORVOR )
			{

			}
			throw new Exception ( "Not found Procedure type" );
		}

		public void SetAltitude ( double value )
		{
			double elevation = Aran.Converters.ConverterToSI.Convert ( Faclts.SelectedNavaid.Location.Elevation, 0 );
			Altitude = GlobalParams.UnitConverter.HeightToInternalUnits ( value );
			if ( Procedure.Type != ProcedureTypeConv.VORVOR )
				Altitude = Altitude - elevation;
			DsgPntSelection.SetAltitude ( Altitude );
			LimDist.SetAltitude ( Altitude );
			Speed.SetAltitude ( Altitude );
			GUIController.SetAltitude ( value );
		}

		public void SetNominalDistance ( double value )
		{
			DsgPntSelection.SetNominalDistance ( value );
			GUIController.SetNominalDistance ( value );
		}

		public void SetAircraftCategory ( int index )
		{
			Speed.SetAircraftCategory ( index );
		}

		public void SetDirection ( double directionInDeg )
		{
			DsgPntSelection.SetDirection ( directionInDeg );			
			//if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
			//{
				
			//}
			//else
			//{
			//    _inbountTrack.SetDirection ( directionInDeg, false, false );
			//}
			GUIController.SetDirection ( directionInDeg );
		}

		internal void SetOverheadDirection ( double direction)
		{
			InbountTrack.SetDirection ( direction, false, false );
			GUIController.SetOverheadDirection ( direction );
		}

		public void SetRadiusForDsgPnts ( double value )
		{
			DsgPntSelection.SetRadiusForDsgPnt ( value );
		}

		public void SetPointChoice ( PointChoice choice )
		{
			DsgPntSelection.SetPointChoice ( choice );
		}

		public void SetProcType ( ProcedureTypeConv value )
		{
			Procedure.SetType ( value );
			GUIController.SetProcedureType ( value );
		}

		public void SetRadiusForNavaids ( double value )
		{
			SlctdAirdromeArea.SetRadius ( value );
		}

		public void SetDesignatedPoint ( DesignatedPoint dsgPnt )
		{
			DsgPntSelection.SetDesignatedPoint ( dsgPnt );
			ApplyDesignatePoint ( );
		}

		internal void SetDesignatedPoint ( double x, double y )
		{
			DsgPntSelection.SetDesignatedPoint ( x, y );
			ApplyDesignatePoint ( );
		}

		private void ApplyDesignatePoint ( )
		{
			GUIController.SetDesignatedPoint ( DsgPntSelection.SelectedDsgPntPrj );
		}

		public void SetEntryDirection ( bool isToward )
		{
			if ( isToward )
				EntryDirection = EntryDirection.Toward;
			else
				EntryDirection = EntryDirection.Away;
			LimDist.SetEntryDirection ( EntryDirection );
			GUIController.SetIsToward ( isToward );
		}

		public void SetSideDirection ( bool isRight )
		{
			if ( isRight )
				Side = SideDirection.sideRight;
			else
				Side = SideDirection.sideLeft;
			GUIController.SetSide ( Side );
		}

		public void SetIAS ( double ias )
		{
			Speed.SetIAS ( ias );
			GUIController.SetIAS ( ias );
		}

		public void SetLimitingDistance ( double limDist )
		{
			LimDist.SetValue ( limDist );
			CalculateLimDistAngle ( );
			GUIController.SetLimitingDistance ( limDist );
		}

		private void CalculateLimDistAngle ( )
		{
			Point navPnt = GlobalParams.SpatialRefOperation.ToPrj<Point> ( Faclts.SelectedNavaid.Location.Geo );
			double direction = ARANFunctions.ReturnAngleInDegrees ( navPnt, ARANFunctions.LocalToPrj ( navPnt, InbountTrack.Direction, LimDist.ValueInPrj,0 ) );
			LimDist.AngleInDeg = ARANFunctions.DirToAzimuth ( navPnt, direction, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo );
		}

		public void SetTime ( double timeInMinute )
		{
			LimDist.SetTime ( timeInMinute );
			//_time.SetValue ( timeInMinute, false );
			GUIController.SetTime ( timeInMinute );
		}

		public void SetWithLimitingRadial ( bool _isWithLimitingRadial )
		{
			_vorDme.SetWithLimitingRadial ( _isWithLimitingRadial );
			GUIController.SetIsWithLimitingRadial ( _isWithLimitingRadial );
		}

		internal void SetIntersectingVorRadial ( double direction )
		{
			DsgPntSelection.SetIntersectingVorRadial ( direction );
			GUIController.SetIntersectingVorRadial ( direction );
		}

		#endregion

		#region Events

		private void OnOrganisationCreated ( object sender, OrganisationEventArg argOrgList )
		{
			if ( _organisationCreated != null )
				_organisationCreated ( sender, argOrgList );
		}

		private void OnAirportHeliportCreated ( object sender, AirportEventArg argArp )
		{
			if ( _airportHeliportChanged != null )
			{
				_airportHeliportChanged ( sender, argArp );
			}
		}

		private void OnIntersectionVorListChanged ( object sender, NavaidListEventArg argNavList )
		{
			if ( _intersectingVorListChanged != null )
				_intersectingVorListChanged ( sender, argNavList );
		}

		private void OnNavaidListChanged ( object sender, NavaidListEventArg argNavList )
		{
			if ( _navaidListChanged != null )
			{
				_navaidListChanged ( sender, argNavList );
			}
		}

		private void OnIntersectingVorListChanged ( object sender, NavaidListEventArg argNavList )
		{
			if ( _intersectingVorListChanged != null )
			{
				_intersectingVorListChanged ( sender, argNavList );
			}
		}

		private void OnDesignatedPointListChanged ( object sender, DsgPntListEventArg argDsgPntList )
		{
			if ( _dsgPntListChanged != null )
			{
				_dsgPntListChanged ( sender, argDsgPntList );
			}
		}

		private void OnDesignatedPointChanged ( object sender, DsgPntEventArg argDsgPnt )
		{
			if ( _dsgPntChanged != null )
			{
				_dsgPntChanged ( null, argDsgPnt );				
				DrawDesigntdPoint ( argDsgPnt.DsgPntPrj );
				if ( Procedure.Type == ProcedureTypeConv.VORVOR && Faclts.SelectedIntersectingVor != null )
				{
					Faclts.CheckIntersectNavaid (Altitude );
					DsgPntSelection.CalculateIntersectionVorRadial ( );
				}
			}
		}

		private void OnDirectionChanged ( object sender, DirectionEventArg argDir )
		{
			if ( _directionChanged != null )
			{
				_directionChanged ( sender, argDir );
			}
		}

		public void OnIntersectionDirectionChanged ( object sender, DirectionEventArg arg )
		{
			if ( _intersectionDirectionChanged != null )
				_intersectionDirectionChanged ( sender, arg );
		}

		private void OnNominalDistanceChanged ( object sender, NomDistanceEventArg argDist )
		{
			if ( _nominalDistanceChanged != null )
			{
				_nominalDistanceChanged ( sender, argDist );
			}
		}

		private void OnIASIntervalChanged ( object sender, Interval argIAS )
		{
			if ( _iasIntervalChanged != null )
			{
				_iasIntervalChanged ( sender, argIAS );
			}
		}

		private void OnCategoryListChanged ( object sender, CategoryListEventArg argCtgList )
		{
			if ( _ctgListChanged != null )
			{
				_ctgListChanged ( sender, argCtgList );
			}
		}

		private void OnTimeChanged ( object sender, CommonEventArg argTime )
		{
			if ( _timeChanged != null )
			{
				_timeChanged ( sender, argTime );
			}
		}

		private void OnLimitDistIntervalChanged ( object sender, Interval interval )
		{
			if ( _limDistIntervalChanged != null )
			{
				_limDistIntervalChanged ( sender, interval );
			}
		}

		public void OnLimDistChanged ( object sender, CommonEventArg arg )
		{
			if ( _limDistChanged != null )
				_limDistChanged ( sender, arg );
			CalculateLimDistAngle ( );
		}

		private void OnSelectedNavaidChanged ( object sender, NavaidEventArg arg )
		{
			GlobalParams.SafeDeleteGraphic ( _handleForNav1 );

			if ( SlctdAirdromeArea.SelectedNavaidsPntPrj == null && SlctdAirdromeArea.SelectedNavaidsPntPrj.Count == 0 )
				return;

            if (SlctdAirdromeArea.SelectedNavaidsPntPrj[0].Value != null)
                //_handleForNav1 = GlobalParams.DrawPointWithText ( SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value, 0, "1" );
                _handleForNav1 = GlobalParams.DrawPoint(SlctdAirdromeArea.SelectedNavaidsPntPrj[0].Value, 0);

			GlobalParams.SafeDeleteGraphic ( _handleForNav2 );
			if ( SlctdAirdromeArea.SelectedNavaidsPntPrj.Count > 1 && SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value != null && SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ] .Type != NavType.DME)
			{
				//_handleForNav2 = GlobalParams.DrawPointWithText ( SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, 0, "2" );
                _handleForNav2 = GlobalParams.DrawPoint(SlctdAirdromeArea.SelectedNavaidsPntPrj[1].Value, 0);
			}

			if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
				GlobalParams.SafeDeleteGraphic ( _handleDesignatedPoint );
		}

		private void OnAppliedValueChanged ( bool isEqual )
		{
			if ( _appliedValueChanged != null )
				_appliedValueChanged ( isEqual );
		}

		#endregion

		public void GetOrganisation ( )
		{
			SlctdAirdromeArea.GetOrganisation ( );
		}

		public void GetCategoryList ( )
		{
			Speed.GetCategoryList ( );
		}

		public void ConstructBasicArea ( )
		{
			//GlobalParams.ClearDrawedItems ( );
			if ( !GUIController.IsEqualPrevious )
			{
				if ( Procedure.Type == ProcedureTypeConv.VORDME )
				{
					if ( EntryDirection == EntryDirection.Toward )
					{
						BasicArea = _vorDme.TowardConstructBasicArea ( InitHolding.Navaid_Database,Side, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value,
																DsgPntSelection.SelectedDsgPntPrj, InbountTrack.Direction + ARANMath.C_PI, DsgPntSelection.NominalDistanceInPrj,
																DsgPntSelection.NominalDistanceInGeo, LimDist.ValueInPrj, LimDist.ValueInGeo, LimDist.TimeInMin,
																Speed.IAS, Speed.TAS, LimDist.LegLength, Altitude, LimDist.Radius );
					}
					else if ( EntryDirection == EntryDirection.Away )
					{
						BasicArea = _vorDme.AwayConstructBasicArea (InitHolding.Navaid_Database, Side, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value,
															  DsgPntSelection.SelectedDsgPntPrj, InbountTrack.Direction, DsgPntSelection.NominalDistanceInPrj,
															  DsgPntSelection.NominalDistanceInGeo, LimDist.ValueInPrj, LimDist.ValueInGeo, LimDist.TimeInMin,
															  Speed.IAS, Speed.TAS, LimDist.LegLength, Altitude, LimDist.Radius);
					}
				}
				else if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
				{
					BasicArea = _vorNdb.ConstructBasicArea ( SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ], Altitude, InbountTrack.Direction, Speed.IAS, Side, LimDist.TimeInMin);					
				}
				else if ( Procedure.Type == ProcedureTypeConv.VORVOR )
				{
					BasicArea = _vorVor.ConstructBasicArea ( SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, Altitude, InbountTrack.Direction, Faclts.IntersectionDirection, Speed.IAS, Side, LimDist.TimeInMin, DsgPntSelection.SelectedDsgPntPrj, EntryDirection);
				}
			}
			
			GlobalParams.SafeDeleteGraphic ( _handleBasicArea );
			if ( BasicArea != null && !BasicArea.IsEmpty)
			{
				_handleBasicArea = GlobalParams.DrawPolygon ( BasicArea, 0, eFillStyle.sfsHollow );
			}
			GUIController.Constructed = true;

			_reportCalculated = false;
			_calculatedPrtctSctr1 = false;
			_calculatedPrtctSctr2 = false;
			_calculatedPrtcRecipEntry = false;
			_calculatedInter_Rad_Entry = false;
		}

		public void ConstructProtectionSector1 ( bool justDeleteOld )
		{
			//_protectionSector1Clicked = !justDeleteOld;
			if ( !GUIController.Constructed )
				return;

			if ( justDeleteOld )
			{
				GlobalParams.SafeDeleteGraphic ( _handlePrtctSector1 );
				return;
			}

			if ( !_calculatedPrtctSctr1 )
			{
				if ( Procedure.Type == ProcedureTypeConv.VORDME )
				{
					//Point vorPntPrj, dmePntPrj;
					//if ( _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Type == NavType.Vor )
					//{
					//    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value;
					//    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value;
					//}
					//else
					//{
					//    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value;
					//    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value;
					//}
					if ( EntryDirection == EntryDirection.Toward )
					{
						ProtectionSector1 = _vorDme.TowardConstructProtectSector1 ( InitHolding.Navaid_Database.VOR, Speed.IAS, Speed.TAS, InbountTrack.Direction, Altitude, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value,
																		DsgPntSelection.SelectedDsgPntPrj, Side );
					}
					else
					{
						ProtectionSector1 = _vorDme.AwayConstructProtectSector1 ( InitHolding.Navaid_Database.VOR, InbountTrack.Direction, DsgPntSelection.SelectedDsgPntPrj, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value,
																		Side, Altitude, Speed.IAS );
					}
					if ( !ProtectionSector1.IsEmpty )
					{
						ProtectionSector1 = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( ProtectionSector1, BasicArea ) ) [ 0 ];
					}
				}
				else if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
				{
					ProtectionSector1.Clear ( );
				}
				else
				{
					ProtectionSector1.Clear ( );
				}
				_calculatedPrtctSctr1 = true;
			}

			if ( !ProtectionSector1.IsEmpty )
			{
				_handlePrtctSector1 = GlobalParams.DrawPolygon ( ProtectionSector1, 255, eFillStyle.sfsCross );
			}
		}

		internal void ConstructProtectionSector2 ( bool justDeleteOld )
		{
			//_protectionSector2Clicked = !justDeleteOld;
			if ( !GUIController.Constructed )
				return;
			if ( justDeleteOld )
			{
				GlobalParams.SafeDeleteGraphic ( _handlePrtctSector2 );
				return;
			}

			if ( !_calculatedPrtctSctr2)
			{
				if ( Procedure.Type == ProcedureTypeConv.VORDME )
				{
					//Point vorPntPrj, dmePntPrj;
					//if ( _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Type == NavType.Vor )
					//{
					//    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value;
					//    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value;
					//}
					//else
					//{
					//    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value;
					//    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value;
					//}
					if ( EntryDirection == EntryDirection.Toward )
					{
						ProtectionSector2 = _vorDme.TowardConstructProtectSector2 ( Speed.TAS, LimDist.ValueInPrj, InbountTrack.Direction, Side, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, DsgPntSelection.SelectedDsgPntPrj,
																		LimDist.TimeInMin );
					}
					else
					{
						ProtectionSector2 = _vorDme.AwayConstructProtectSector2 ( Speed.TAS, LimDist.ValueInPrj, InbountTrack.Direction, Side, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, DsgPntSelection.SelectedDsgPntPrj,
																		LimDist.TimeInMin, Altitude );
					}
					if ( !ProtectionSector2.IsEmpty )
					{
						ProtectionSector2 = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( ProtectionSector2, BasicArea ) ) [ 0 ];
					}
				}
				else if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
				{
					ProtectionSector2.Clear ( );
				}
				else
				{
					ProtectionSector2.Clear ( );
				}
				_calculatedPrtctSctr2 = true;
			}

			GlobalParams.SafeDeleteGraphic ( _handlePrtctSector2 );
			if ( !ProtectionSector2.IsEmpty )
			{
				_handlePrtctSector2 = GlobalParams.DrawPolygon ( ProtectionSector2, 255 * 255, eFillStyle.sfsVertical );
			}
		}

		internal void ConstructReciprocalEntryArea ( bool justDeleteOld )
		{
			//_protectionRecipDirClicked = !justDeleteOld;
			if ( !GUIController.Constructed || Procedure.Type == ProcedureTypeConv.VOR_NDB)
				return;

			if ( justDeleteOld )
			{
				GlobalParams.SafeDeleteGraphic ( _handlePrtctRecipDir2SecPnt );
				if ( _RE_AngleChanged != null )
					_RE_AngleChanged ( null, new CommonEventArg ( double.NaN ) );
				return;
			}

			if ( !_calculatedPrtcRecipEntry )
			{
				if ( Procedure.Type == ProcedureTypeConv.VORDME )
				{
					int angle = 0;
					//Point vorPntPrj, dmePntPrj;
					//if ( _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Type == NavType.Vor )
					//{
					//    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value;
					//    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value;
					//}
					//else
					//{
					//    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value;
					//    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value;
					//}
					if ( EntryDirection == EntryDirection.Toward )
					{
						ProtectionRecipEntry = _vorDme.TowardConstructRecipDirectEntry2SecondaryPnt (InitHolding.Navaid_Database.VOR, LimDist.LegLength, InbountTrack.Direction, DsgPntSelection.SelectedDsgPntPrj,
																					  SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, Side, LimDist.Radius, out angle );
					}
					else
					{
						_vorDme.AwayConstructRecipDirectEntryToSecondaryPnt ( InbountTrack.Direction, LimDist.Radius, DsgPntSelection.SelectedDsgPntPrj, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value, LimDist.LegLength,
																						Side, out angle );
						ProtectionRecipEntry.Clear ( );
					}
					if ( _RE_AngleChanged != null )
						_RE_AngleChanged ( null, new CommonEventArg ( angle ) );
					if ( EntryDirection == EntryDirection.Away )
						return;
				}
				else if ( Procedure.Type == ProcedureTypeConv.VORVOR )
				{
					ProtectionRecipEntry = _vorVor.ReciprocalEntryArea ( );
				}
				if ( !ProtectionRecipEntry.IsEmpty )
				{
					ProtectionRecipEntry = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( ProtectionRecipEntry, BasicArea ) ) [ 0 ];
				}
				_calculatedPrtcRecipEntry = true;
			}

			GlobalParams.SafeDeleteGraphic ( _handlePrtctRecipDir2SecPnt );
			if ( !ProtectionRecipEntry.IsEmpty)
			{
				_handlePrtctRecipDir2SecPnt = GlobalParams.DrawPolygon ( ProtectionRecipEntry, 255 * 255, eFillStyle.sfsVertical );
			}
		}

		internal void ConstructIntersectingRadialEntry ( bool justDeleteOld )
		{
			if ( !GUIController.Constructed )
				return;
			if ( justDeleteOld )
			{
				GlobalParams.SafeDeleteGraphic ( _handlePrtctInterRadEntry );
				return;
			}

			if ( !_calculatedInter_Rad_Entry )
			{
				ProtectionInterRadEntryArea = _vorVor.ProtectIntersectingRadialEntry ( SlctdAirdromeArea.SelectedNavaidsPntPrj [ 0 ].Value, SlctdAirdromeArea.SelectedNavaidsPntPrj [ 1 ].Value, InbountTrack.Direction, Faclts.IntersectionDirection, Side, DsgPntSelection.SelectedDsgPntPrj, Altitude );
				_calculatedInter_Rad_Entry = true;
				ProtectionInterRadEntryArea = ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( ProtectionInterRadEntryArea, BasicArea );
			}

			GlobalParams.SafeDeleteGraphic ( _handlePrtctInterRadEntry );
			if ( ProtectionInterRadEntryArea != null && !ProtectionInterRadEntryArea.IsEmpty )
			{
				_handlePrtctInterRadEntry = GlobalParams.DrawMultiPolygon ( ProtectionInterRadEntryArea, 255 * 255, eFillStyle.sfsVertical );
			}
		}

		/// <summary>
		/// Returns whether save button should be enabled or not
		/// </summary>
		/// <returns></returns>
		internal bool CreateReport ( )
		{
			CalculateReport ( );
			return ( Report.ObstacleReport.Where ( rep => !rep.Validation ).Count ( ) == 0 || Report.ReportCount == 0 );
		}

		internal void Save ( )
		{
			CalculateReport ( );
			_dbResult.Save ( Report, this );
		}

		private void CalculateReport ( )
		{
			if ( !_reportCalculated )
			{
				Buffer = GetBuffer ( );
                if (Procedure.Type != ProcedureTypeConv.VORVOR || !_calculatedInter_Rad_Entry)
					ProtectionInterRadEntryArea.Clear ( );
				if ( Report == null )
					Report = new Report ( BasicArea, ProtectionSector1, ProtectionSector2, ProtectionRecipEntry, ProtectionInterRadEntryArea, Buffer, Altitude, Moc, _bufferWidth );
				else
					Report.Create ( BasicArea, ProtectionSector1, ProtectionSector2, ProtectionRecipEntry, ProtectionInterRadEntryArea, Buffer, Altitude, Moc, _bufferWidth );
				_reportCalculated = true;
			}
		}

		public void CleanDisplay ( )
		{
			GlobalParams.ClearDrawedItems ( );
			//DeleteDesignatedPoint ( );
			//GlobalParams.SafeDeleteGraphic ( _handleBasicArea );
			//GlobalParams.SafeDeleteGraphic ( _handlePrtctSector1 );
			//GlobalParams.SafeDeleteGraphic ( _handlePrtctSector2 );
			//GlobalParams.SafeDeleteGraphic ( _handlePrtctRecipDir2SecPnt );
			//GlobalParams.SafeDeleteGraphic ( _handleForNav1 );
			//GlobalParams.SafeDeleteGraphic ( _handleForNav2 );
		}

        public bool IsDisplayClear 
        {
            get 
            {
                return GlobalParams.IsDisplayClear;
            }
        }

		private Polygon GetBuffer ( )
		{
			Polygon fullArea = BasicArea;
			if ( /*_protectionSector1Clicked*/ _calculatedPrtctSctr1 && Procedure.Type != ProcedureTypeConv.VORVOR )
			{
				fullArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( fullArea, ProtectionSector1 ) ) [ 0 ];
			}
			else
				ProtectionSector1.Clear ( );

			if ( /*_protectionSector2Clicked*/ _calculatedPrtctSctr2 && Procedure.Type == ProcedureTypeConv.VORDME )
			{
				fullArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( fullArea, ProtectionSector2 ) ) [ 0 ];
			}
			else
				ProtectionSector2.Clear ( );

			if ( /*_protectionRecipDirClicked*/ _calculatedPrtcRecipEntry )
			{
				fullArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( fullArea, ProtectionRecipEntry ) ) [ 0 ];
			}

			Polygon _buffer = ( ( MultiPolygon ) GlobalParams.GeomOperators.Buffer ( fullArea, _bufferWidth ) ) [ 0 ];
			_buffer = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( _buffer, fullArea ) ) [ 0 ];
			GlobalParams.DrawPolygon ( _buffer, 1,eFillStyle.sfsCross);
			return _buffer;
		}

		public AppliedGUIValues GUIController
		{
			get;
			set;
		}

		private void DrawDesigntdPoint ( Point designatedPoint )
		{
			DeleteDesignatedPoint ( );
            if (designatedPoint != null)
                //_handleDesignatedPoint = GlobalParams.DrawPointWithText ( designatedPoint, 255, "DsgPnt" );
                _handleDesignatedPoint = GlobalParams.DrawPoint(designatedPoint, 255);
		}

		private void DeleteDesignatedPoint ( )
		{
			GlobalParams.SafeDeleteGraphic ( _handleDesignatedPoint );
		}

		internal Ring ToleranceArea
		{
			get
			{
				switch ( Procedure.Type )
				{
					case ProcedureTypeConv.NONE:
						return null;

					case ProcedureTypeConv.VORDME:
						return _vorDme.ToleranceArea;

					case ProcedureTypeConv.VOR_NDB:
						return _vorNdb.ToleranceArea;

					case ProcedureTypeConv.VORVOR:
						return _vorVor.ToleranceArea;

					default:
						throw new NotImplementedException ( "Not founde Procedure type !" );
				}
			}
		}

		internal LineString NominalTrack
		{
			get
			{
				switch ( Procedure.Type )
				{
					case ProcedureTypeConv.NONE:
						return null;

					case ProcedureTypeConv.VORDME:
						return _vorDme.NominalTrack;
						
					case ProcedureTypeConv.VOR_NDB:
						return _vorNdb.NominalTrack;

					case ProcedureTypeConv.VORVOR:
						return _vorVor.NominalTrack;
					default:
						throw new NotImplementedException ( "Not founde Procedure type !" );
				}
			}
		}

		internal double PostFixTolerance
		{
			get
			{
				if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
					return _vorNdb.PostFixTolerance;
				if ( Procedure.Type == ProcedureTypeConv.VORDME )
					return _vorDme.PostFixTolerance;
				return _postFixTolerance;
			}
		}

		internal double PriorFixTolerance
		{
			get
			{
				if ( Procedure.Type == ProcedureTypeConv.VOR_NDB )
					return _vorNdb.PriorFixTolerance;
				if ( Procedure.Type == ProcedureTypeConv.VORDME )
					return _vorDme.PriorFixTolerance;
				return _priorFixtolerance;
			}
		}

		internal InboundTrack InbountTrack
		{
			get;
			private set;
		}

		internal Procedure Procedure
		{
			get;
			private set;
		}

		internal Speed Speed
		{
			get;
			private set;
		}

        //public bool CalculatedReport
        //{
        //    get
        //    {
        //        return _reportCalculated;
        //    }
        //}

		internal LimitingDistance LimDist
		{
			get;
			private set;
		}

		internal SideDirection Side
		{
			get;
			private set;
		}

		internal Report Report
		{
			get;
			private set;
		}

		internal double Altitude
		{
			get;
			private set;
		}

		internal SelectionAirdromeArea SlctdAirdromeArea
		{
			get;
			private set;
		}

		internal FixFacilities Faclts
		{
			get;
			private set;
		}

		internal DesignatedPntSelection DsgPntSelection
		{
			get;
			private set;
		}

		internal EntryDirection EntryDirection
		{
			get;
			private set;
		}

		internal Polygon Buffer
		{
			get;
			private set;
		}

		internal Polygon BasicArea
		{
			get;
			private set;
		}

		internal Polygon ProtectionSector1
		{
			get;
			private set;
		}

		internal Polygon ProtectionSector2
		{
			get;
			private set;
		}

		internal Polygon ProtectionRecipEntry
		{
			get;
			private set;
		}

		internal MultiPolygon ProtectionInterRadEntryArea
		{
			get;
			private set;
		}

		public const double Moc = 300;

		private const double _bufferWidth = 4600;

		private OrganisationEventHandler _organisationCreated;
		private AirportEventHandler _airportHeliportChanged;
		private NavaidListEventHandler _navaidListChanged;
		private NavaidListEventHandler _intersectingVorListChanged;
		private DsgPntEventHandler _dsgPntChanged;
		private DsgPntListEventHandler _dsgPntListChanged;
		private DirectionEventHandler _directionChanged;
		private DistanceEventHandler _nominalDistanceChanged;
		private CategoryListEventHandler _ctgListChanged;
		private IntervalEventHandler _iasIntervalChanged;
		private CommonEventHandler _timeChanged;
		private IntervalEventHandler _limDistIntervalChanged;
		private CommonEventHandler _limDistChanged;
		private DirectionEventHandler _intersectionDirectionChanged;
		private CommonEventHandler _RE_AngleChanged;
		private AppliedValueChangedEventHandler _appliedValueChanged;

		private VORDME _vorDme;
		private VORNDB _vorNdb;
		private VorVor _vorVor;

		private double _priorFixtolerance, _postFixTolerance;
		private int _handleForNav1, _handleForNav2, _handleBasicArea, _handleDesignatedPoint, _handlePrtctSector1, _handlePrtctSector2, _handlePrtctRecipDir2SecPnt;
		private bool _calculatedPrtctSctr1, _calculatedPrtctSctr2, _calculatedPrtcRecipEntry;
		private bool _calculatedInter_Rad_Entry;
		private int _handlePrtctInterRadEntry;
		//private bool _protectionRecipDirClicked;
		//private bool _protectionSector1Clicked;
		//private bool _protectionSector2Clicked;
		private DbResult _dbResult;
		private bool _reportCalculated;
    }
}