using System.Collections.Generic;

using ARAN.GeometryClasses;
using Delib.Classes.Features.AirportHeliport;
using Delib.Classes.Codes;
using Delib.Classes.Features.Organisation;
using Delib.Classes.Features.Navaid;

namespace Holding.Convential
{
    public class SelectionAirdromeArea
    {
        public SelectionAirdromeArea ( FixFacilities navaidList )
        {
            _navaids = navaidList;
            _navaids.AddNavaidChangedEvent ( OnSelectedNavaidChanged );
        }

        #region Add EventHandlers

        public void AddAirdromeListChangedEvent ( EventHandlerArpList value )
        {
            _airdromeListChanged += value;
        }

        public void AddOrgListCreatedEvent ( EventHandlerOrgList value )
        {
            _organisationListChanged += value;
        }

        public void AddSelectedNavaidChangedEvent ( EventHandlerNav value )
        {
            _selectedNavaidChanged += value;
        }

        #endregion

        #region Set Values

        public void SetOrganisation ( int index)
        {
            if ( index == -1 )
            {
                SelectedOrg = null;
            }
            else if ( SelectedOrg != OrgList[index])
            {
                SelectedOrg = OrgList [index];
            }
        }

        public void SetAirport ( int index)
        {
            if ( index == -1 )
            {
                SelectedAirport = null;
            }
            if ( SelectedAirport != AirportList[index])
            {
                SelectedAirport = AirportList [index];
            }
        }

        public void SetRadius ( double radius )
        {
            _navaids.SetRadius ( Common.DeConvertDistance ( radius ) );
        }

        #endregion

        #region Properties 

        private List<OrganisationAuthority> OrgList
        {
            get
            {
                return _orgList;
            }
            set
            {
                if ( value.Count == 0 )
                    _orgList = null;
                else
                    _orgList = value;
                if ( _organisationListChanged != null )
                {
                    _organisationListChanged ( null, new EventArgOrgList ( _orgList ) );
                }
            }
        }

        private OrganisationAuthority SelectedOrg
        {
            get
            {
                return _selectedOrg;
            }
            set
            {
                _selectedOrg = value;
                if ( _selectedOrg == null )
                {
                    AirportList = null;
                }
                else
                {
                    AirportList = GlobalParams.Database.HoldingQpi.GetAirportHeliport ( _selectedOrg );
                }
            }
        }

        private List<AirportHeliport> AirportList
        {
            get
            {
                return _airportList;
            }
            set
            {
                if ( value == null || value.Count == 0)
                    _airportList = null;
                else
                    _airportList = value;
                if ( _airdromeListChanged != null )
                {
                    _airdromeListChanged ( null, new EventArgAirportList ( _airportList ) );
                }
            }
        }

        private AirportHeliport SelectedAirport
        {
            get
            {
                return _selectedAirport;
            }
            set
            {
                _selectedAirport = value;
                if ( value == null )
                {
                    _navaids.SetArp ( double.NaN, double.NaN );
                }
                else
                {
                    _navaids.SetArp ( _selectedAirport.arp.x, _selectedAirport.arp.y );
                }
            }
        }

        public List<NavaidPntPrj> SelectedNavaidsPntPrj
        {
            get
            {
                return _selectedNavPnts;
            }
        }

        #endregion

        #region Events

        private void OnSelectedNavaidChanged ( object sender, EventArgNavaid argNav )
        {
            _selectedNavPnts = new List<NavaidPntPrj> ();
            foreach ( NavaidPntPrj navPntPrj in argNav.NavaidPntPrjList )
                _selectedNavPnts.Add ( navPntPrj.Clone () );
        }

        #endregion

        public void GetOrgList ( )
        {
            OrgList = GlobalParams.Database.HoldingQpi.GetOrganisationAuthorityList ();
        }

        private EventHandlerOrgList _organisationListChanged;
        private EventHandlerArpList _airdromeListChanged;
        private EventHandlerNav _selectedNavaidChanged;

        private List<NavaidPntPrj> _selectedNavPnts;
        private List<OrganisationAuthority> _orgList;
        private OrganisationAuthority _selectedOrg;
        private List<AirportHeliport> _airportList;
        private AirportHeliport _selectedAirport;
        private FixFacilities _navaids;
    }

    public class Procedure
    {
        public Procedure ( FixFacilities navaidList )
        {
            _navaids = navaidList;
        }

        public void SetType ( ProcedureTypeConv value )
        {
            if ( value == _type )
                return;
            Type = value;
        }

        public ProcedureTypeConv Type
        {
            get
            {
                return _type;
            }
            private set
            {
                _type = value;
                if ( _type == ProcedureTypeConv.ProcType_VORDME )
                {
                    _navaids.SetServiceTypes ( new NavaidServiceType [] { NavaidServiceType.VOR_DME }, _type);
                }
                else if ( _type == ProcedureTypeConv.ProcType_VOR_NDB )
                {
                    _navaids.SetServiceTypes ( new NavaidServiceType [] {   NavaidServiceType.VOR, NavaidServiceType.NDB, 
                                                                            NavaidServiceType.VOR_DME, NavaidServiceType.VORTAC, 
                                                                            NavaidServiceType.NDB_DME, NavaidServiceType.NDB_MKR },
                                                                            _type);
                }
                else
                {
                }
            }
        }

        private ProcedureTypeConv _type = ProcedureTypeConv.ProcType_NONE;
        private FixFacilities _navaids;
    }

    public class FixFacilities
    {
        public FixFacilities ( DesignatedPntSelection dsgPntSelection )
        {
            _dsgPntSelect = dsgPntSelection;
        }

        #region Add EventHandlers

        public void AddNavaidChangedEvent ( EventHandlerNav OnNavaidChanged )
        {
            _navaidChanged += OnNavaidChanged;
        }

        public void AddNavListChangedEvent ( EventHandlerNavList value )
        {
            _navaidListChanged += value;
        }

        #endregion

        #region Set Values

        public List<NavaidPntPrj> SetNavaid ( int index)
        {
            if ( index == -1 )
            {
                return SetSelectedNavaid ( null );
            }
            else if ( SelectedNavaid != NavaidList [index] )
            {
                return SetSelectedNavaid ( NavaidList [index] );
            }
            else
            {
                return null;
            }
        }

        public void SetNavaidList ( )
        {
            if ( ( _x != double.NaN ) && ( _y != double.NaN ) && _servicesInitialized )
                NavaidList = GlobalParams.Database.HoldingQpi.GetNavaidListByTypes ( _x, _y, _radius, _navaidServTypes );
            else
                NavaidList = null;
        }

        public void SetServiceTypes ( NavaidServiceType [] ServiceTypes, ProcedureTypeConv procTypeConv )
        {
            _navaidServTypes = ServiceTypes;
            _servicesInitialized = true;
            _procTypeConv = procTypeConv;
            SetNavaidList ();
        }

        public void SetArp ( double x, double y )
        {
            _x = x;
            _y = y;
            SetNavaidList ();
        }

        public void SetRadius ( double radius )
        {
            _radius = radius;
            SetNavaidList ();
        }

        #endregion

        #region Properties

        public Navaid SelectedNavaid
        {
            get
            {
                return _selectedNavaid;
            }
            private set
            {
                SetSelectedNavaid ( value );
            }
        }

        private List<NavaidPntPrj> SetSelectedNavaid(Navaid navaid)
        {
            List<NavaidPntPrj> result = new List<NavaidPntPrj> ();
            _selectedNavaid = navaid;
            if ( _procTypeConv == ProcedureTypeConv.ProcType_VORDME )
            {
                if ( _selectedNavaid == null )
                {
                    Vor = null;
                    Dme = null;
                }
                else
                {
                    if ( _selectedNavaid.navaidComponent [0].theNavaidEquipment is VOR )
                    {
                        Vor = _selectedNavaid.navaidComponent [0].theNavaidEquipment as VOR;
                        Dme = _selectedNavaid.navaidComponent [1].theNavaidEquipment as DME;
                    }
                    else
                    {
                        Vor = _selectedNavaid.navaidComponent [1].theNavaidEquipment as VOR;
                        Dme = _selectedNavaid.navaidComponent [0].theNavaidEquipment as DME;
                    }
                }
                _dsgPntSelect.SetNavaid ( _selectedNavaid );
                result.Add ( new NavaidPntPrj ( VorPntPrj, NavType.NavType_Vor ) );
                result.Add ( new NavaidPntPrj ( DmePntPrj, NavType.NavType_DME ) );
                if ( _navaidChanged != null )
                {
                    EventArgNavaid argNav = new EventArgNavaid ( /*_procTypeConv, */ result );
                    _navaidChanged ( null, argNav );
                }
                return result;
            }
            else if ( _procTypeConv == ProcedureTypeConv.ProcType_VOR_NDB )
            {
                if ( _selectedNavaid == null )
                {
                    Vor = null;
                    Ndb = null;
                    result.Add ( null );
                }
                else
                {
                    if ( _selectedNavaid.navaidComponent.Count > 1 )
                    {
                        if ( _selectedNavaid.navaidComponent [0].theNavaidEquipment is VOR )
                        {
                            Vor = _selectedNavaid.navaidComponent [0].theNavaidEquipment as VOR;
                            result.Add ( new NavaidPntPrj ( VorPntPrj, NavType.NavType_Vor ) );
                        }
                        else if ( _selectedNavaid.navaidComponent [1].theNavaidEquipment is VOR )
                        {                         
                            Vor = _selectedNavaid.navaidComponent [1].theNavaidEquipment as VOR;
                            result.Add ( new NavaidPntPrj ( VorPntPrj, NavType.NavType_Vor ) );
                        }
                        else if ( _selectedNavaid.navaidComponent [0].theNavaidEquipment is NDB )
                        {
                            Ndb = _selectedNavaid.navaidComponent [0].theNavaidEquipment as NDB;
                            result.Add ( new NavaidPntPrj ( NdbPntPrj, NavType.NavType_NDB ) );
                        }
                        else if ( _selectedNavaid.navaidComponent [1].theNavaidEquipment is NDB )
                        {
                            Ndb = _selectedNavaid.navaidComponent [1].theNavaidEquipment as NDB;
                            result.Add ( new NavaidPntPrj ( NdbPntPrj, NavType.NavType_NDB ) );
                        }
                    }
                    else
                    {
                        if ( _selectedNavaid.navaidComponent [0].theNavaidEquipment is VOR )
                        {
                            Vor = _selectedNavaid.navaidComponent [0].theNavaidEquipment as VOR;
                            result.Add ( new NavaidPntPrj ( VorPntPrj, NavType.NavType_Vor ) );
                        }
                        else
                        {
                            Ndb = _selectedNavaid.navaidComponent [0].theNavaidEquipment as NDB;
                            result.Add ( new NavaidPntPrj ( NdbPntPrj, NavType.NavType_NDB ) );
                        }
                    }
                }
                if ( _navaidChanged != null )
                {
                    EventArgNavaid argNav =new EventArgNavaid ( /*_procTypeConv, */ result );
                    _navaidChanged ( null, argNav );
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        private List<Navaid> NavaidList
        {
            get
            {
                return _navaidList;
            }
            
            set
            {
                if ( value ==null ||  value.Count == 0 )
                    _navaidList = null;
                else
                    _navaidList = value;
                if ( _navaidListChanged != null )
                {
                        _navaidListChanged ( null, new EventArgNavaidList ( _navaidList ) );
                }

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
                    VorPntGeo = null;
                    VorPntPrj = null;
                }
                else
                {
                    VorPntGeo = GeomFunctions.GmlToAranPoint ( _vor );
                    VorPntPrj = GlobalParams.SpatialRefOperation.GeoToPrj ( VorPntGeo );
                }
                _dsgPntSelect.SetVorPnts ( VorPntGeo, VorPntPrj );
            }
        }

        public Point VorPntGeo
        {
            get
            {
                return _vorPntGeo;
            }
            private set
            {
                _vorPntGeo = value;
            }
        }

        public Point VorPntPrj
        {
            get
            {
                return _vorPntPrj;
            }
            private set
            {
                _vorPntPrj =value;
            }
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
                    DmePntGeo = null;
                    DmePntPrj = null;
                }
                else
                {

                    DmePntGeo = GeomFunctions.GmlToAranPoint ( _dme );
                    DmePntPrj = GlobalParams.SpatialRefOperation.GeoToPrj ( DmePntGeo );
                }
            }
        }

        public Point DmePntGeo
        {
            get
            {
                return _dmePntGeo;
            }
            private set
            {
                _dmePntGeo = value;
            }
        }

        public Point DmePntPrj
        {
            get
            {
                return _dmePntPrj;
            }
            private set
            {
                _dmePntPrj =value;
                _dsgPntSelect.SetDmePntPrj ( _dmePntPrj );
            }
        }

        public NDB Ndb
        {
            get
            {
                return _ndb;
            }
            set
            {
                _ndb = value;
                if ( _ndb == null )
                {
                    NdbPntGeo = null;
                    NdbPntPrj = null;
                }
                else
                {
                    NdbPntGeo = GeomFunctions.GmlToAranPoint ( _ndb );
                    NdbPntPrj = GlobalParams.SpatialRefOperation.GeoToPrj ( NdbPntGeo );
                }
            }
        }

        public Point NdbPntGeo
        {
            get
            {
                return _ndbPntGeo;
            }
            set
            {

                _ndbPntGeo = value;
            }
        }

        public Point NdbPntPrj
        {
            get
            {
                return _ndbPntPrj;
            }
            set
            {
                _ndbPntPrj = value;
            }
        }

        #endregion 

        private EventHandlerNav _navaidChanged;
        private EventHandlerNavList _navaidListChanged;

        private VOR _vor;
        private Point _vorPntGeo, _vorPntPrj;
        private DME _dme;
        private Point _dmePntGeo, _dmePntPrj;
        private NDB _ndb;
        private Point _ndbPntGeo, _ndbPntPrj;
        private List<Navaid> _navaidList;
        private bool _servicesInitialized = false;
        private NavaidServiceType[] _navaidServTypes;
        private double _x = double.NaN, _y = double.NaN, _radius = 0;
        private DesignatedPntSelection _dsgPntSelect;
        private Navaid _selectedNavaid;
        private ProcedureTypeConv _procTypeConv;        
    }
}
