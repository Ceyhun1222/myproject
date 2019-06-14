using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Constants;

namespace Holding.Convential
{
    public class Speed
    {
        public Speed ( LimitingDistance limDist)
        {
            _limDist = limDist;
            _intervals = new List<Interval> ();

            _intervals.Add ( new Interval ( GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMin ) [eAircraftCategory.acA],
                                            GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMax ) [eAircraftCategory.acA] ) );

            _intervals.Add ( new Interval ( GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMin ) [eAircraftCategory.acB],
                                            GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMax ) [eAircraftCategory.acB] ) );

            _intervals.Add ( new Interval ( GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMin ) [eAircraftCategory.acC],
                                            GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMax ) [eAircraftCategory.acC] ) );

            _intervals.Add ( new Interval ( GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMin ) [eAircraftCategory.acD],
                                            GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMax ) [eAircraftCategory.acD] ) );

            _intervals.Add ( new Interval ( GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMin ) [eAircraftCategory.acE],
                                            GlobalParams.Constant_G.AircraftCategory.Constant ( eAircraftCategoryData.RacetrckIASMax ) [eAircraftCategory.acE] ) );
        }

        #region Add EventHandlers
        
        public void AddIASIntervalChangedEvent ( EventHandlerIASInterval OnIASChanged )
        {
            _iasIntervalChanged += OnIASChanged;
        }

        public void AddCategoryListChangedEvent ( EventHandlerCategoryList OnCategoryListChanged )
        {
            _categoryListChanged += OnCategoryListChanged;
        }
        
        #endregion

        #region Set Values

        public void SetAircraftCategory ( int index ) 
        {
            if ( _selectedAircraftCategoryIndex != index )
            {
                SelectedAircraftCategoryIndex = index;
            }
        }

        public void SetAltitude ( double altitude ) 
        {
            _altitude = altitude;
            if (IAS != 0)
                TAS = ARANMath.IASToTAS ( IAS, _altitude, 15 );
        }

        public void SetIAS ( double ias ) 
        {
            IAS = Common.DeConvertSpeed ( ias );
        }

        #endregion

        #region Properties

        public Interval this [int index] 
        {
            get
            {
                return _intervals [index];
            }
        }

        public int SelectedAircraftCategoryIndex 
        {
            get
            {
                return _selectedAircraftCategoryIndex;
            }
            private set
            {
                _selectedAircraftCategoryIndex = value;
                if (_iasIntervalChanged != null)
                {
                    EventArgIASInterval argIASInterval = new EventArgIASInterval (  Common.ConvertSpeed_ ( _intervals [SelectedAircraftCategoryIndex].Left, roundType.toNearest ),
                                                                                    Common.ConvertSpeed_ ( _intervals [SelectedAircraftCategoryIndex].Right, roundType.toNearest ) );
                    _iasIntervalChanged ( null, argIASInterval );
                }
            }
        }

        public double IAS
        {
            get
            {
                return _ias;
            }
            
            private set
            {
                _ias = value;
                TAS = ARANMath.IASToTAS ( _ias, _altitude, 15 );
            }
        }

        public double TAS
        {
            get
            {
                return _tas;
            }
            private set
            {
                _tas = value;
                _limDist.SetTAS ( _tas );
            }
        }

        public List<string> CategoryList
        {
            get
            {
                return _categorieList;
            }
            private set
            {
                _categorieList = value;
                if ( _categoryListChanged != null )
                {
                    EventArgCategoryList argCtgList = new EventArgCategoryList ( _categorieList );
                    _categoryListChanged ( null, argCtgList );
                }
            }
        }

        #endregion

        public void GetCategoryList ( )
        {
            List<string> result = new List<string> ();

            result.Add ( "A" );
            result.Add ( "B" );
            result.Add ( "C" );
            result.Add ( "D" );
            result.Add ( "E" );

            CategoryList = result;
        }

        private EventHandlerIASInterval _iasIntervalChanged;
        private EventHandlerCategoryList _categoryListChanged;

        private LimitingDistance _limDist;
        private double _ias = 0;
        private double _altitude = 0;
        private double _tas = 0;
        private List<Interval> _intervals;
        private List<string> _categorieList;
        private int _selectedAircraftCategoryIndex = -1;
    }
}
