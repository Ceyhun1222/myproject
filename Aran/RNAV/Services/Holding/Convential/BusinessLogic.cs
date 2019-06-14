using ARAN.GeometryClasses;
using Delib.Classes.Features.Organisation;
using Delib.Classes.Features.AirportHeliport;
using Delib.Classes.Features.Navaid;
using ARAN.Common;
using System.Collections.Generic;

namespace Holding.Convential
{
    public class ConventialRacetrack
    {
        public ConventialRacetrack ( )
        {
            _vorDme = new VORDME ( InitHolding.Navaid_Database );
            _vorNdb = new VORNDB ( InitHolding.Navaid_Database );

            _entryDirection = EntryDirection.EntrDir_None;
            _time = new Time ();
            _limDist = new LimitingDistance ( _entryDirection, _time );
            _inbountTrack = new InboundTrack ();
            _dsgPntSelection = new DesignatedPntSelection (_limDist, _inbountTrack);
            _faclts = new FixFacilities ( _dsgPntSelection );
            _slctdAirdromeArea = new SelectionAirdromeArea ( _faclts );
            _proc = new Procedure ( _faclts );
            _speed = new Speed (_limDist);

            _slctdAirdromeArea.AddOrgListCreatedEvent ( OnOrganisationListCreated );
            _slctdAirdromeArea.AddAirdromeListChangedEvent ( OnAirportHeliportListChanged );
            _faclts.AddNavListChangedEvent ( OnNavaidListChanged );
            _dsgPntSelection.AddtDsgPntListChangedEvent ( OnDesignatedPointListChanged );
            _dsgPntSelection.AddDsgPntChangedEvent ( OnDesignatedPointChanged );
            _inbountTrack.AddDirectionChangedEvent ( OnDirectionChanged );
            _dsgPntSelection.AddNominalDistanceChangedEvent ( OnNominalDistanceChanged );
            _speed.AddCategoryListChangedEvent ( OnCategoryListChanged );
            _speed.AddIASIntervalChangedEvent ( OnIASIntervalChanged );
            _time.AddTimeChangedEvent ( OnTimeChanged );
            _limDist.AddLimitingDistanceChangedEvent ( OnLimitingDistanceChanged );
        }
        
        #region Add EventHandlers

        public void AddOrgListCreatedEvent ( EventHandlerOrgList OnOrgListCreated )
        {
            _organisationListCreated +=  OnOrgListCreated;
        }

        public void AddAirdromeListChangedEvent ( EventHandlerArpList OnAirdromeListChanged )
        {
            _airportHeliportListChanged += OnAirdromeListChanged;
        }

        public void AddNavListChangedEvent ( EventHandlerNavList OnNavaidListChanged )
        {
            _navaidListChanged += OnNavaidListChanged;
        }

        public void AddDsgPointListChangedEvent ( EventHandlerDsgPntList OnDsgPntListChanged )
        {
            _dsgPntListChanged += OnDsgPntListChanged;
        }

        public void AddDesignatedPointChangedEvent ( EventHandlerDsgPnt OnDsgPntChanged )
        {
            _dsgPntChanged += OnDsgPntChanged;
        }

        public void AddDirectionChangedEvent ( EventHandlerDirection OnDirectionChanged )
        {
            _directionChanged += OnDirectionChanged;
        }

        public void AddNominalDistanceChangedEvent ( EventHandlerDistance OnNominalDistanceChanged )
        {
            _nominalDistanceChanged += OnNominalDistanceChanged;
        }

        public void AddIASIntervalChangedEvent ( EventHandlerIASInterval OnIASIntervalChanged )
        {
            _iasIntervalChanged += OnIASIntervalChanged;
        }

        public void AddCategoryListChangedEvent ( EventHandlerCategoryList OnCtgListChanged )
        {
            _ctgListChanged += OnCtgListChanged;
        }

        public void AddTimeChangedEvent ( EventHandlerTime OnTimeChanged )
        {
            _timeChanged += OnTimeChanged;
        }

        public void AddLimitingDistanceChangedEvent ( EventHandlerLimitingDistance OnLimDistChanged )
        {
            _limitingDistanceChanged += OnLimDistChanged;
        }

        #endregion

        #region Set Values

        public List<NavaidPntPrj> SetNavaid ( int index ) 
        {
            return _faclts.SetNavaid ( index );
        }

        public void SetAltitude(double value)
        {
            _altitude = Common.DeConvertHeight ( value );
            _dsgPntSelection.SetAltitude(_altitude);
            _limDist.SetAltitude ( _altitude );
            _speed.SetAltitude ( _altitude );
        }
            
        public void SetNominalDistance(double value)
        {
            _dsgPntSelection.SetNominalDistance(value);
        }

        public void SetAircraftCategory ( int index )
        {
            _speed.SetAircraftCategory ( index );
        }

        public void SetDirection(double value)
        {
            if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
            {
                _dsgPntSelection.SetDirection ( value );
            }
            else
            {
                _inbountTrack.SetDirection ( value, false ); 
            }
        }
                    
        public void SetRadiusForDsgPnts(double value)
        {
            _dsgPntSelection.SetRadiusForDsgPnt(value);
        }

        public void SetPointChoice ( PointChoice choice )
        {
            _dsgPntSelection.SetPointChoice ( choice );
        }

        public void SetProcType ( ProcedureTypeConv value )
        {
            _proc.SetType ( value );
        }
        
        public void SetOrganisation ( int index )
        {
            _slctdAirdromeArea.SetOrganisation ( index );
        }
        
        public void SetRadiusForNavaids ( double value )
        {
            _slctdAirdromeArea.SetRadius ( value );
        }

        public void SetAirport(int index)
        {
            _slctdAirdromeArea.SetAirport ( index);
        }

        public void SetDesignatedPoint ( int index)
        {
            _dsgPntSelection.SetDesignatedPoint ( index);
        }

        public void SetEntryDirection ( bool isToward)
        {
            if ( isToward )
                _entryDirection = EntryDirection.EntrDir_Toward;
            else
                _entryDirection = EntryDirection.EntrDir_Away;
            _limDist.SetEntryDirection ( _entryDirection );
        }

        public void SetSideDirection ( bool isRight)
        {
            if ( isRight )
                _side = SideDirection.sideRight;
            else
                _side = SideDirection.sideLeft;
        }

        public void SetIAS ( double ias )
        {
            _speed.SetIAS ( ias );
        }

        public void SetLimitingDistance ( double limDist )
        {
            _limDist.SetValue ( limDist );
        }

        public void SetTime ( double timeInMinute)
        {
            _time.ValueInSeconds = timeInMinute * 60;
        }

        public void SetWithLimitingRadial ( bool _isWithLimitingRadial )
        {
            if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
            {
                if ( _entryDirection == EntryDirection.EntrDir_Away )
                {
                    _vorDme.SetWithLimitingRadial ( _isWithLimitingRadial );
                }
            }
        }

        #endregion

        #region Events

        private void OnOrganisationListCreated ( object sender, EventArgOrgList argOrgList )
        {
            if ( _organisationListCreated != null )
            {
                _organisationListCreated ( sender, argOrgList );
            }
        }

        private void OnAirportHeliportListChanged ( object sender, EventArgAirportList argArpList )
        {
            if ( _airportHeliportListChanged != null )
            {
                _airportHeliportListChanged ( sender, argArpList );
            }
        }

        private void OnNavaidListChanged ( object sender, EventArgNavaidList argNavList )
        {
            if ( _navaidListChanged != null )
            {
                _navaidListChanged ( sender, argNavList );
            }
        }

        private void OnDesignatedPointListChanged ( object sender, EventArgDsgPntList argDsgPntList )
        {
            if ( _dsgPntListChanged != null )
            {
                _dsgPntListChanged ( sender, argDsgPntList );
            }
        }

        private void OnDesignatedPointChanged ( object sender, EventArgDsgPnt argDsgPnt )
        {
            if ( _dsgPntChanged != null )
            {
                _dsgPntChanged ( null, argDsgPnt );
            }
        }

        private void OnDirectionChanged ( object sender, EventArgDirection argDir )
        {
            if ( _directionChanged != null )
            {
                _directionChanged ( sender, argDir );
            }
        }

        private void OnNominalDistanceChanged ( object sender, EventArgNomDistance argDist )
        {
            if ( _nominalDistanceChanged != null )
            {
                _nominalDistanceChanged ( sender, argDist );
            }
        }

        private void OnIASIntervalChanged ( object sender, EventArgIASInterval argIAS )
        {
            if ( _iasIntervalChanged != null )
            {
                _iasIntervalChanged ( sender, argIAS );
            }
        }

        private void OnCategoryListChanged ( object sender, EventArgCategoryList argCtgList )
        {
            if ( _ctgListChanged != null )
            {
                _ctgListChanged ( sender, argCtgList );
            }
        }

        private void OnTimeChanged ( object sender, EventArgTime argTime )
        {
            if ( _timeChanged != null )
            {
                _timeChanged ( sender, argTime );
            }
        }

        private void OnLimitingDistanceChanged ( object sender, EventArgLimitingDistance argLimDist )
        {
            if ( _limitingDistanceChanged != null )
            {
                _limitingDistanceChanged ( sender, argLimDist );
            }
        }

        #endregion

        public void GetOrgList ( )
        {
            _slctdAirdromeArea.GetOrgList ();
        }

        public void GetCategoryList ( )
        {
            _speed.GetCategoryList ();
        }

        public Polygon ConstructBasicArea ( )
        {
            Polygon result = null;
            if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
            {
                if ( _entryDirection == EntryDirection.EntrDir_Toward )
                {
                    result = _vorDme.TowardConstructBasicArea ( _side, _slctdAirdromeArea.SelectedNavaidsPntPrj [1].Value, _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Value, 
                                                            _dsgPntSelection.SelectedDsgPntPrj, _inbountTrack.Direction, _dsgPntSelection.NominalDistanceInPrj,
                                                            _dsgPntSelection.NominalDistanceInGeo, _limDist.ValueInPrj, _limDist.ValueInGeo, _time, 
                                                            _speed.IAS, _speed.TAS, _limDist.LegLength, _altitude, _limDist.Radius);
                }
                else if (_entryDirection == EntryDirection.EntrDir_Away)
                {
                    result = _vorDme.AwayConstructBasicArea ( _side, _slctdAirdromeArea.SelectedNavaidsPntPrj [1].Value, _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Value, 
                                                          _dsgPntSelection.SelectedDsgPntPrj, _inbountTrack.Direction, _dsgPntSelection.NominalDistanceInPrj, 
                                                          _dsgPntSelection.NominalDistanceInGeo, _limDist.ValueInPrj, _limDist.ValueInGeo, _time, 
                                                          _speed.IAS, _speed.TAS, _limDist.LegLength, _altitude, _limDist.Radius);                
                }
            }
            else if ( _proc.Type == ProcedureTypeConv.ProcType_VOR_NDB )
            {
                result = _vorNdb.ConstructBasicArea ( _slctdAirdromeArea.SelectedNavaidsPntPrj [0], _altitude, _inbountTrack.Direction, _speed.IAS, _side, _time );
            } 
            else if (_proc.Type == ProcedureTypeConv.ProcType_VORVOR)
            {
                result = null;
            }
            return result;
        }

        public Polygon ConstructProtectionSector1 ( )
        {
            Polygon result;            
            if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
            {
                Point vorPntPrj, dmePntPrj;
                if (_slctdAirdromeArea.SelectedNavaidsPntPrj[0].Type == NavType.NavType_Vor)
                {
                    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj[0].Value;
                    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj[1].Value;
                }
                else
                {
                    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj[0].Value;
                    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj[1].Value;
                }
                if ( _entryDirection == EntryDirection.EntrDir_Toward )
                {
                    result = _vorDme.TowardConstructProtectSector1( _speed.IAS, _speed.TAS, _inbountTrack.Direction, _altitude, vorPntPrj, dmePntPrj,
                                                                    _dsgPntSelection.SelectedDsgPntPrj, _side);
                }
                else
                {
                    result = _vorDme.AwayConstructProtectSector1 ( _inbountTrack.Direction, _dsgPntSelection.SelectedDsgPntPrj, dmePntPrj, vorPntPrj, 
                                                                    _side, _altitude, _speed.IAS );
                }
            }
            else if ( _proc.Type == ProcedureTypeConv.ProcType_VOR_NDB )
            {
                result = null;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public Polygon ConstructProtectionSector2 ( )
        {
            Polygon result;
            if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
            {
                Point vorPntPrj, dmePntPrj;
                if ( _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Type == NavType.NavType_Vor )
                {
                    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Value;
                    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [1].Value;
                }
                else
                {
                    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Value;
                    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [1].Value;
                }
                if ( _entryDirection == EntryDirection.EntrDir_Toward )
                {
                    result = _vorDme.TowardConstructProtectSector2 ( _speed.TAS, _limDist.ValueInPrj, _inbountTrack.Direction, _side, dmePntPrj, _dsgPntSelection.SelectedDsgPntPrj,
                                                                    _time);
                }
                else
                {
                    result = _vorDme.AwayConstructProtectSector2 ( _speed.TAS, _limDist.ValueInPrj, _inbountTrack.Direction, _side, dmePntPrj, _dsgPntSelection.SelectedDsgPntPrj,
                                                                    _time, _altitude );
                }
            }
            else if ( _proc.Type == ProcedureTypeConv.ProcType_VOR_NDB )
            {
                result = null;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public Polygon ConstructRecipDir2SecPnt (out int angle)
        {
            Polygon result;
            if ( _proc.Type == ProcedureTypeConv.ProcType_VORDME )
            {
                Point vorPntPrj, dmePntPrj;
                if ( _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Type == NavType.NavType_Vor )
                {
                    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Value;
                    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [1].Value;
                }
                else
                {
                    dmePntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [0].Value;
                    vorPntPrj = _slctdAirdromeArea.SelectedNavaidsPntPrj [1].Value;
                }
                if ( _entryDirection == EntryDirection.EntrDir_Toward )
                {
                    result = _vorDme.TowardConstructRecipDirectEntry2SecondaryPnt ( _limDist.LegLength, _inbountTrack.Direction, _dsgPntSelection.SelectedDsgPntPrj,
                                                                                  vorPntPrj, dmePntPrj, _side, _limDist.Radius, out angle );
                        //.TowardConstructProtectSector2 ( _speed.TAS, _limDist.ValueInPrj, _inbountTrack.Direction, _side, dmePntPrj, _dsgPntSelection.SelectedDsgPntPrj,
                        //                                            _limDist.Time );
                }
                else
                {
                    result = _vorDme.AwayConstructRecipDirectEntryToSecondaryPnt ( _inbountTrack.Direction, _limDist.Radius, _dsgPntSelection.SelectedDsgPntPrj, vorPntPrj, _limDist.LegLength,
                                                                                    _side, out angle );
                }
            }
            else if ( _proc.Type == ProcedureTypeConv.ProcType_VOR_NDB )
            {
                angle = 0;
                result = null;
            }
            else
            {
                angle = 0;
                result = null;
            }
            return result;
        }
        
        private EventHandlerOrgList _organisationListCreated;
        private EventHandlerArpList _airportHeliportListChanged;
        private EventHandlerNavList _navaidListChanged;
        private EventHandlerDsgPnt _dsgPntChanged;
        private EventHandlerDsgPntList _dsgPntListChanged;
        private EventHandlerDirection _directionChanged;
        private EventHandlerDistance _nominalDistanceChanged;
        private EventHandlerCategoryList _ctgListChanged;
        private EventHandlerIASInterval _iasIntervalChanged;
        private EventHandlerTime _timeChanged;
        private EventHandlerLimitingDistance _limitingDistanceChanged;

        private SelectionAirdromeArea _slctdAirdromeArea;
        private InboundTrack _inbountTrack;
        private Procedure _proc;
        private FixFacilities _faclts;
        private DesignatedPntSelection _dsgPntSelection;
        private EntryDirection _entryDirection = EntryDirection.EntrDir_None;
        private SideDirection _side = SideDirection.sideOn;
        private double _altitude;
        private Speed _speed;
        private LimitingDistance _limDist;
        private Time _time;

        private VORDME _vorDme;
        private VORNDB _vorNdb;
    }
}
