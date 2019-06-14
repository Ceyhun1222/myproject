using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Delta.Settings.model;
using Aran.PANDA.Common;

namespace Aran.Delta.Settings
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private QueryModel _queryModel;
        private bool _isArena = false;

        public SettingsViewModel()
        {
        }

        public void SetAppType(bool isArena)
        {
            _isArena = isArena;
        }

        public void Load()
        {
            if (Globals.Settings != null)
            {
                _queryModel = Globals.Settings.DeltaQuery;
                Snap = Globals.Settings.DeltaSnapModel;
                Interface = Globals.Settings.DeltaInterface;
                _selectedDistanceIndex = (int)Interface.DistanceUnit;
                _distancePrecision = Interface.DistancePrecision;

                _selectedHeightIndex = (int) Interface.HeightUnit;
                _heightPrecision = Interface.HeightPrecision;

                _selectedCoordinateUnit = (int) Interface.CoordinateUnit;
                _coordinatePrecision = Interface.CoordinatePrecision;

                _anglePrecision = Interface.AnglePrecision;

                Symbols = Globals.Settings.SymbolModel;
                DistanceUnits = new List<string>();
                DistanceUnits.Add("km");
                DistanceUnits.Add("NM");
                //  DistanceUnits.Add("NM");

                HeightUnits = new List<string>();
                HeightUnits.Add("metr");
                HeightUnits.Add("feet");

                CoordinateUnits = new List<string>();
                CoordinateUnits.Add("DD.xx");
                CoordinateUnits.Add("DD MM SS.xx");
                if (!_isArena)
                    LoadOrganisationList(Globals.Settings.DeltaQuery.Organization);
            }
            if (PropertyChanged!=null)
                PropertyChanged(this,new PropertyChangedEventArgs(""));
        }

        public InterfaceModel Interface { get; set; }

        public SnapModel Snap { get; set; }
        #region :> Query Params
     
        private void LoadOrganisationList(Guid identifier)
        {
            var result = dbProvider.GetVersionsOf(FeatureType.OrganisationAuthority, TimeSliceInterpretationType.BASELINE);

            OrganisationList = new ObservableCollection<OrganisationAuthority>();
            if (result.IsSucceed && result.List.Count > 0)
            {
                List<OrganisationAuthority> tmpList = result.GetListAs<OrganisationAuthority>();
                tmpList.Sort(FeatureSorter);
                foreach (OrganisationAuthority orgAuthority in tmpList)
                {
                    if (orgAuthority.Identifier == identifier)
                        SelectedOrganisation = orgAuthority;

                    OrganisationList.Add(orgAuthority);
                }
                if (SelectedOrganisation == null)
                    SelectedOrganisation = OrganisationList[0];
            }
        }

        public ObservableCollection<OrganisationAuthority> OrganisationList { get; private set; }
        public ObservableCollection<AirportHeliport> AdhpList { get; private set; }

        private OrganisationAuthority _selectedOrganisation;
        public OrganisationAuthority SelectedOrganisation
        {
            get { return _selectedOrganisation; }
            set
            {
                _selectedOrganisation = value;

                var comops = new ComparisonOps(ComparisonOpType.EqualTo, "ResponsibleOrganisation.TheOrganisationAuthority", _selectedOrganisation.Identifier);
                var choice = new OperationChoice(comops);
                var filter = new Filter(choice);
                var result = dbProvider.GetVersionsOf(FeatureType.AirportHeliport, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);

                AdhpList = new ObservableCollection<AirportHeliport>();
                _selectedAdhp = null;
                if (result.IsSucceed && result.List.Count > 0)
                {
                    var tmpList = result.GetListAs<AirportHeliport>();
                    tmpList.Sort(FeatureSorter);
                    foreach (var adhp in tmpList)
                    {
                        if (adhp.Identifier == Globals.Settings.DeltaQuery.Aeroport)
                            SelectedAdhp = adhp;

                        AdhpList.Add(adhp);
                    }

                    if (SelectedAdhp == null)
                        SelectedAdhp = AdhpList[0];
                }

                _queryModel.Organization = _selectedOrganisation.Identifier;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AdhpList"));

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrganisation"));
            }
        }

        private AirportHeliport _selectedAdhp;
        public AirportHeliport SelectedAdhp
        {
            get { return _selectedAdhp; }
            set
            {
                _selectedAdhp = value;
                if (_selectedAdhp != null)
                    _queryModel.Aeroport = _selectedAdhp.Identifier;
                else
                    _queryModel.Aeroport = Guid.Empty;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedAdhp"));
            }
        }

        private double _obstacleAreaRadius;
        public double ObstacleAreaRadius
        {
            get
            {
                return _obstacleAreaRadius;
            }
            set
            {
                _obstacleAreaRadius = value;
                if (_queryModel!=null)
                    _queryModel.Radius = value;
            }
        }

        private int FeatureSorter(Feature f1, Feature f2)
        {
            if (f1 is OrganisationAuthority)
            {
                OrganisationAuthority oa1 = f1 as OrganisationAuthority;
                OrganisationAuthority oa2 = f2 as OrganisationAuthority;
                return string.Compare(oa1.Designator, oa2.Designator);
            }
            else
            {
                AirportHeliport ah1 = f1 as AirportHeliport;
                AirportHeliport ah2 = f2 as AirportHeliport;
                return string.Compare(ah1.Designator, ah2.Designator);
            }
        }

        #endregion

        #region :>Interface Params

        public IList<string> DistanceUnits { get; private set; }

        private int _selectedDistanceIndex;
        public int SelectedDistanceIndex
        {
            get
            {
                return _selectedDistanceIndex;
            }
            set
            {
                _selectedDistanceIndex = value;
                if (Interface!=null)
                    Interface.DistanceUnit = (HorizantalDistanceType)_selectedDistanceIndex;
            }
        }

        private double _distancePrecision;
        public double DistancePrecision
        {
            get
            {
                return _distancePrecision;
            }
            set
            {
                _distancePrecision = value;
                if (Interface!=null)
                    Interface.DistancePrecision = value;
            }
        }

        public IList<string> HeightUnits { get; set; }

        private int _selectedHeightIndex;
        public int SelectedHeightIndex
        {
            get
            {
                return _selectedHeightIndex;
            }
            set
            {
                _selectedHeightIndex = value;
                if (Interface!=null)
                    Interface.HeightUnit = (VerticalDistanceType)_selectedHeightIndex;
            }
        }

        private double _heightPrecision;
        public double HeightPrecision
        {
            get
            {
                return _heightPrecision;
            }
            set
            {
                _heightPrecision = value;
                if (Interface!=null)
                    Interface.HeightPrecision = value;
            }
        }

        public IList<string> CoordinateUnits { get; set; }

        private int _selectedCoordinateUnit;
        public int SelectedCoordinateUnit
        {
            get
            {
                return _selectedCoordinateUnit;
            }
            set
            {
                _selectedCoordinateUnit = value;
                if (Interface!=null)
                    Interface.CoordinateUnit = (CoordinateUnitType)_selectedCoordinateUnit;
            }
        }

        private double _anglePrecision;

        public double AnglePrecision
        {
            get
            {
                return _anglePrecision;
            }
            set
            {
                _anglePrecision = value;
                if (Interface!=null)
                    Interface.AnglePrecision = value;
                
            }
        }

        private double _coordinatePrecision;
        public double CoordinatePrecision
        {
            get
            {
                return _coordinatePrecision;
            }
            set
            {
                _coordinatePrecision = value;
                if (Interface!=null)
                    Interface.CoordinatePrecision = value;
            }
        }

        #endregion

        public SymbolModel Symbols { get; set; }

        
        public event PropertyChangedEventHandler PropertyChanged;

        private DbProvider dbProvider
        {
            get
            {
                if (SettingsGlobals.AranEnvironment == null)
                    return null;

                return SettingsGlobals.AranEnvironment.DbProvider as DbProvider;
            }
        }

        
    }
}
