using Delib.Classes.Features.Navaid;
using ARAN.GeometryClasses;
using System.Collections.Generic;
using ARAN.Common;
using System;

namespace Holding.Convential
{
    public class DesignatedPntSelection
    {
        public DesignatedPntSelection (LimitingDistance limDist, InboundTrack inboundTrack )
        {
            _createDsgPnt = new CreateDesignatedPoint ( inboundTrack);
            _selectDsgPnt = new SelectDesignatedPoint ( _createDsgPnt );
            _selectDsgPnt.AddtDsgPntListChangedEvent ( OnDesignatedPointListChanged );
            //_createDsgPnt.AddDirectionChangedEvent ( OnDirectionChanged );
            _createDsgPnt.AddDistanceChangedEvent ( OnNominalDistanceChanged );
            _selectDsgPnt.AddDsgPntChangedEvent ( OnDsgPntChanged );
            _createDsgPnt.AddSelectedDesignatedPointChangedEvent ( OnDsgPntChanged );
            _limitingDistance = limDist;            
        }

        #region Add EventHandlers

        //public void AddDirectionChangedEvent ( EventHandlerDirection OnDirChanged )
        //{
        //    _directionChanged += OnDirChanged;
        //}

        public void AddNominalDistanceChangedEvent ( EventHandlerDistance OnDistChanged )
        {
            _nominalDistanceChanged += OnDistChanged;
        }

        public void AddtDsgPntListChangedEvent ( EventHandlerDsgPntList OnDsgPntListChanged )
        {
            _dsgPntListChanged += OnDsgPntListChanged;
        }

        public void AddDsgPntChangedEvent ( EventHandlerDsgPnt OnDsgPntChanged )
        {
            _dsgPntChanged += OnDsgPntChanged;
        }

        #endregion

        #region SetValues

        public void SetPointChoice ( PointChoice pntChoice )
        {
            ChosenPntType = pntChoice;
        }

        public void SetDesignatedPoint ( int index)
        {
            _selectDsgPnt.SetDesignatedPoint ( index );
            _nomDistChanged = false;
            _dirChanged = false;
        }

        public void SetAltitude ( double altitude )
        {
            _createDsgPnt.SetAltitude ( altitude );
        }

        public void SetNominalDistance ( double dist )
        {
            if ( ChosenPntType == PointChoice.PntChoice_Create )
            {
                _createDsgPnt.SetNominalDistance ( dist, false );
                _nomDistChanged = true;
            }
        }

        public void SetDirection ( double direct )
        {
            if ( ChosenPntType == PointChoice.PntChoice_Create )
            {
                _createDsgPnt.SetDirection ( direct, false );
                _dirChanged = true;
            }
        }

        public void SetVorPnts ( Point vorPntGeo, Point vorPntPrj )
        {
            _createDsgPnt.SetVorPnts ( vorPntGeo, vorPntPrj, ( ChosenPntType == PointChoice.PntChoice_Create ) );
        }

        public void SetRadiusForDsgPnt ( double radius )
        {
            _selectDsgPnt.SetRadius ( radius );
        }

        public void SetNavaid ( Navaid navaid )
        {
            _selectedNavaid = navaid;
            if ( ChosenPntType == PointChoice.PntChoice_Select )
            {
                _selectDsgPnt.SetNavaid ( _selectedNavaid, _dirChanged, _nomDistChanged );
            }
        }

        public void SetDmePntPrj ( Point dmePntPrj )
        {
            _selectDsgPnt.SetDmePntPrj ( dmePntPrj );
        }

        #endregion

        #region Events

        private void OnDesignatedPointListChanged ( object sender, EventArgDsgPntList argDsgPntList )
        {
            if ( _dsgPntListChanged != null )
            {
                _dsgPntListChanged ( sender, argDsgPntList );
            }
        }

        //private void OnDirectionChanged ( object sender, EventArgDirection argDir )
        //{
        //    if ((ChosenPntType == PointChoice.PntChoice_Select) && (_directionChanged != null ))
        //    {
        //        _directionChanged ( sender, argDir );
        //    }
        //    Direction = argDir.Direction;
        //}

        private void OnNominalDistanceChanged ( object sender, EventArgNomDistance argDist )
        {
            if ( ( ChosenPntType == PointChoice.PntChoice_Select ) && ( _nominalDistanceChanged != null ) )
            {
                _nominalDistanceChanged ( sender, argDist );
            }
            if ( argDist.NomDistanceInPrj != 0 )
            {
                NominalDistanceInPrj = argDist.NomDistanceInPrj;
                NominalDistanceInGeo = argDist.NomDistanceInGeo;

                _limitingDistance.SetNominalDistance ( _nominalDistanceInPrj );
            }
        }

        private void OnDsgPntChanged ( object sender, EventArgDsgPnt argDsgPnt )
        {
            SelectedDsgPntPrj = argDsgPnt.DsgPntPrj;
            if ( _dsgPntChanged != null )
            {
                _dsgPntChanged ( null, argDsgPnt);
            }
        }

        #endregion

        public Point SelectedDsgPntPrj
        {
            get
            {
                return _selectedDsgPntPrj;
            }
            private set
            {
                _selectedDsgPntPrj = value;
            }
        }

        public double NominalDistanceInPrj 
        {
            get
            {
                return _nominalDistanceInPrj;
            }
            private set
            {
                _nominalDistanceInPrj = value;
            }
        }

        public double NominalDistanceInGeo 
        {
            get
            {
                return _nominalDistance;
            }

            private set
            {
                _nominalDistance = value;
            }
        }

        //public double Direction
        //{
        //    get
        //    {
        //        return _direction;
        //    }
        //    private set
        //    {
        //        _direction = value;
        //    }
        //}

        public PointChoice ChosenPntType
        {
            get
            {
                return _pntChoice;
            }
            set
            {
                if ( _pntChoice == value )
                    return;
                _pntChoice = value;
                if ( _pntChoice == PointChoice.PntChoice_Select )
                {
                    _selectDsgPnt.SetNavaid ( _selectedNavaid, _dirChanged, _nomDistChanged);
                }
                else
                {
                    _createDsgPnt.SetDesignatedPoint ();
                }
            }
        }

        private LimitingDistance _limitingDistance;
        private bool _nomDistChanged = false, _dirChanged = false;
        private Point _selectedDsgPntPrj;//, _vorPntGeo, _vorPntPrj;
        private Navaid _selectedNavaid;
        private SelectDesignatedPoint _selectDsgPnt;
        private CreateDesignatedPoint _createDsgPnt;
        private PointChoice _pntChoice;
        private double _nominalDistanceInPrj, /*_direction, */_nominalDistance;

        private EventHandlerDsgPntList _dsgPntListChanged;
        //private EventHandlerDirection _directionChanged;
        private EventHandlerDistance _nominalDistanceChanged;
        private EventHandlerDsgPnt  _dsgPntChanged;
    }

    public class SelectDesignatedPoint
    {
        public SelectDesignatedPoint ( CreateDesignatedPoint createDsgPnt )
        {
            _createDsgPnt = createDsgPnt;
        }

        #region Add EventHandlers

        public void AddtDsgPntListChangedEvent ( EventHandlerDsgPntList OnDsgPntListChanged )
        {
            _dsgPntListChanged += OnDsgPntListChanged;
        }

        public void AddDsgPntChangedEvent ( EventHandlerDsgPnt OnDsgPntChanged )
        {
            _selectedDsgPntChanged += OnDsgPntChanged;
        }

        #endregion

        #region Set Values

        public void SetRadius ( double radius )
        {
            Radius = radius;
        }

        public void SetDesignatePointList ( )
        {
            if ( _selectednavaid != null )
                DesgntPntList = GlobalParams.Database.HoldingQpi.GetDesignatedPointList ( _selectednavaid, _radius );
            else
                DesgntPntList = null;
        }

        public void SetDesignatedPoint ( int index )
        {
            if ( index == -1 )
            {
                SelectedDesignatedPoint = null;
            }
            else if ( _selectedDsgPnt != DesgntPntList[index] )
            {
                SelectedDesignatedPoint = DesgntPntList [index];
            }
        }

        public void SetNavaid ( Navaid navaid, bool DirChanged, bool DistChanged)
        {
            if ( _selectednavaid != navaid )
            {
                _selectednavaid = navaid;
                SetDesignatePointList ();
            }
            else
            {
                SetDirAndDist ( DirChanged, DistChanged );
            }
        }

        public void SetDmePntPrj ( Point dmePntPrj )
        {
            _selectedDmePntPrj = dmePntPrj;
        }

        private void SetDirAndDist (bool DirChanged, bool DistChanged )
        {
            if (DirChanged)
                _createDsgPnt.SetDirection ( ARANMath.Modulus ( ARANFunctions.ReturnAngleAsRadian ( _createDsgPnt.VorPntPrj, _selectedDsgPntPrj ), 2*ARANMath.C_PI ), true );
            if (DistChanged)
                _createDsgPnt.SetNominalDistance ( ARANFunctions.ReturnDistanceAsMeter ( _selectedDmePntPrj, _selectedDsgPntPrj ), true );
            if ( _selectedDsgPntChanged != null )
            {
                EventArgDsgPnt argDsgPnt = new EventArgDsgPnt ( _selectedDsgPntPrj );
                _selectedDsgPntChanged ( null, argDsgPnt );
            }
        }

        #endregion

        #region Properties

        public double Radius
        {
            get
            {
                return _radius;
            }
            private set
            {
                _radius = Common.DeConvertDistance ( value );
                SetDesignatePointList ();
            }
        }

        public List<DesignatedPoint> DesgntPntList
        {
            get
            {
                return _desgntPntList;
            }

            private set
            {
                if ( value == null || value.Count == 0 )
                    _desgntPntList = null;
                else
                    _desgntPntList = value;
                if ( _dsgPntListChanged != null )
                {
                    _dsgPntListChanged ( null, new EventArgDsgPntList ( _desgntPntList ) );
                }
                //if ( _desgntPntList.Count == 0 )
                //{
                //    _createDsgPnt.SetDirection ( 0, true );
                //    _createDsgPnt.SetNominalDistance ( 0, true );
                //    _selectedDsgPnt = null;
                //}
            }
        }

        public DesignatedPoint SelectedDesignatedPoint
        {
            get
            {
                return _selectedDsgPnt;
            }

            private set
            {
                _selectedDsgPnt = value;
                if ( _selectedDsgPnt == null )
                {
                    _selectedDsgPntGeo =null;
                    _selectedDsgPntPrj =null;
                    SetDirAndDist ( false, false );
                }
                else
                {
                    _selectedDsgPntGeo = GeomFunctions.GmlToAranPoint ( _selectedDsgPnt );
                    _selectedDsgPntPrj = GlobalParams.SpatialRefOperation.GeoToPrj ( _selectedDsgPntGeo );
                    SetDirAndDist ( true, true );
                }
            }
        }

        #endregion

        private EventHandlerDsgPntList _dsgPntListChanged;
        private EventHandlerDsgPnt _selectedDsgPntChanged;

        private double _radius = 0;
        private List<DesignatedPoint> _desgntPntList;
        private DesignatedPoint _selectedDsgPnt;
        private Point _selectedDsgPntGeo, _selectedDsgPntPrj;
        private Navaid _selectednavaid;
        private Point _selectedDmePntPrj;
        private CreateDesignatedPoint _createDsgPnt;
    }

    public class CreateDesignatedPoint
    {
        public CreateDesignatedPoint ( InboundTrack inboundTrack )
        {
            _inboundTrack = inboundTrack;
        }

        #region Add EventHandlers

        //public void AddDirectionChangedEvent ( EventHandlerDirection OnDirChanged )
        //{
        //    _directionChanged += OnDirChanged;
        //}

        public void AddDistanceChangedEvent ( EventHandlerDistance OnDistChanged )
        {
            _distanceChanged += OnDistChanged;
        }

        public void AddSelectedDesignatedPointChangedEvent ( EventHandlerDsgPnt OnSelectedDsgPntChanged )
        {
            _selectedDsgPntChanged += OnSelectedDsgPntChanged;
        }

        #endregion

        #region Set Values

        public void SetAltitude ( double altitude )
        {
            _altitude = Common.DeConvertHeight ( altitude );
            NominalDistance = Math.Sqrt (NominalDistanceInPrj*NominalDistanceInPrj + _altitude*_altitude);
        }

        public void SetVorPnts ( Point vorPntGeo, Point vorPntPrj, bool doSetDsgPnt )
        {
            _vorPntGeo = vorPntGeo;
            _vorPntPrj = vorPntPrj;
            _inboundTrack.SetVorPntPrj ( _vorPntPrj, _vorPntGeo );
            if ( doSetDsgPnt )
                SetDesignatedPoint ();
        }

        public void SetNominalDistance ( double distance, bool isCalculated )
        {
            if ( !isCalculated )
                _nominalDistanceInPrj  = Common.DeConvertDistance ( distance );
            else
                _nominalDistanceInPrj = distance;
            NominalDistance = Math.Sqrt ( _nominalDistanceInPrj*_nominalDistanceInPrj + _altitude*_altitude );
            if ( _distanceChanged != null )
            {
                EventArgNomDistance argDist = new EventArgNomDistance ( _nominalDistanceInPrj, Common.ConvertDistance ( _nominalDistanceInPrj, roundType.toNearest ), NominalDistance );
                _distanceChanged ( null, argDist );
            }
            if (! isCalculated)
                SetDesignatedPoint ();
        }

        public void SetDirection ( double direction, bool isCalculated )
        {
            _inboundTrack.SetDirection ( direction, isCalculated );
            //if ( isCalculated )
            //    /* _directionInRadInDir */
            //    _inboundTrack.Direction = direction;
            //else
            //    /*_directionInRadInDir*/
            //    _inboundTrack.Direction = ARANMath.Modulus ( ARANFunctions.AztToDirection ( _vorPntGeo, ARANFunctions.DegToRad ( ( double ) direction ), GlobalParams.SpatialRefOperation.GeoSp, GlobalParams.SpatialRefOperation.PrjSp ), 2*ARANMath.C_PI );

            //if ( _directionChanged != null )
            //{
            //    EventArgDirection argDir = new EventArgDirection ( /*_directionInRadInDir*/_inboundTrack.Direction, ARANFunctions.RadToDeg ( ARANFunctions.DirToAzimuth ( VorPntPrj, /*_directionInRadInDir*/_inboundTrack.Direction, GlobalParams.SpatialRefOperation.PrjSp, GlobalParams.SpatialRefOperation.GeoSp ) ) );
            //    _directionChanged ( null, argDir );
            //}
            if ( !isCalculated )
                SetDesignatedPoint ();
        }

        public void SetDesignatedPoint ( )
        {
            if ( NominalDistanceInPrj != 0 && _inboundTrack.Direction != 0 )
            {
                if ( _selectedDsgPntChanged != null )
                {
                    EventArgDsgPnt argDsgPnt;
                    if ( VorPntPrj != null )
                        argDsgPnt = new EventArgDsgPnt ( ARANFunctions.LocalToPrj ( VorPntPrj, _inboundTrack.Direction, NominalDistanceInPrj, 0 ) );
                    else
                        argDsgPnt = new EventArgDsgPnt ( null );
                    _selectedDsgPntChanged ( null, argDsgPnt );
                }
            }
        }

        #endregion

        #region Properties

        //public double Direction
        //{
        //    get
        //    {
        //        return _directionInRadInDir;
        //    }
        //}

        public Point VorPntPrj
        {
            get
            {
                return _vorPntPrj;
            }
            private set
            {
                _vorPntPrj = value;
                SetDesignatedPoint ();
            }
        }

        public double NominalDistanceInPrj 
        {
            get
            {
                return _nominalDistanceInPrj;
            }
        }

        public double NominalDistance 
        {
            get
            {
                return _nominalDistance;
            }

            private set
            {
                _nominalDistance = value;
            }
        }

        #endregion

        //private EventHandlerDirection _directionChanged;
        private EventHandlerDistance _distanceChanged;
        private EventHandlerDsgPnt _selectedDsgPntChanged;

        private double _altitude;
        private double _nominalDistanceInPrj, _nominalDistance;
        //private double _directionInRadInDir;
        private Point _vorPntGeo, _vorPntPrj;
        private InboundTrack _inboundTrack;
    }

}