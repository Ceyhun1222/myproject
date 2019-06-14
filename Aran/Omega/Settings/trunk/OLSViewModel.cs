using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Aran.Omega.SettingsUI
{
    public class OLSViewModel:INotifyPropertyChanged
    {
        private DbProvider dbProvider = null;

        public OLSViewModel()
        {
           // Load();
        }

        private QueryModel _query;
        public QueryModel Query
        {
            get { return _query; }
            set
            {
                _query = value;

                OrganisationList.Clear();
                GetOrganisationList(_query.Organization).ForEach(org=>OrganisationList.Add(org));

                ObstacleAreaRadius = _query.Radius;
            }
        }
        private InterfaceModel _interface;
        public InterfaceModel Interface 
        {
            get { return _interface; }
            set 
            {
                _interface = value;
                if (_interface != null) 
                {
                    SelectedDistanceIndex =(int) Interface.DistanceUnit;
                    SelectedHeightIndex =(int) Interface.HeightUnit;

                    DistancePrecision = Interface.DistancePrecision;
                    HeightPrecision = Interface.HeightPrecision;
                }
            }
        }

        public  void Load()
        {
            OrganisationList = new ObservableCollection<OrganisationModel>();
            AdhpList = new ObservableCollection<AirportHeliport>();

            InterfacePanelIsVisible = System.Windows.Visibility.Visible;
            QueryPanelIsVisible = System.Windows.Visibility.Collapsed;
            StylePanelIsVisible = System.Windows.Visibility.Collapsed;

            DistanceUnits = new List<string>();
            DistanceUnits.Add("meter");
          //  DistanceUnits.Add("NM");

            HeightUnits = new List<string>();
            HeightUnits.Add("meter");
            HeightUnits.Add("feet");

            SettingsModelList = Globals.Settings.OLSModelList;
            if (Globals.Settings.OLSModelList != null && Globals.Settings.OLSModelList.Count > 0)
            {
                foreach (SettingsModel settModel in SettingsModelList)
                {
                    if (settModel is QueryModel)
                    {
                        Query = settModel as QueryModel;
                    }
                    else if (settModel is InterfaceModel) 
                    {
                        Interface = settModel as InterfaceModel;
                    }
                }
            }
            else
            {
                Query = new QueryModel{ ViewCaption = "Query", Type = MenuType.Query,IsSelected = true };
                SettingsModelList.Add(Query);
                Interface = new InterfaceModel { ViewCaption = "Interface", Type = MenuType.Interface,IsSelected=false };
                SettingsModelList.Add(Interface);

                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Inner horizontal",Surface= PANDA.Constants.SurfaceType.InnerHorizontal, Symbol = Symbols.InnerApproachDefaultSymbol,SelectedSymbol=Symbols.InnerApproachSelectedSymbol});
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Conical surface",Surface= PANDA.Constants.SurfaceType.CONICAL, Symbol = Symbols.ConicalDefaultSymbol,SelectedSymbol=Symbols.ConicalSelectedSymbol});
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Outer horizontal", Surface = PANDA.Constants.SurfaceType.OuterHorizontal, Symbol = Symbols.OuterHorizontalDefaultSymbol, SelectedSymbol = Symbols.OuterHorizontalSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Strip", Surface = PANDA.Constants.SurfaceType.Strip, Symbol = Symbols.StripDefaultSymbol, SelectedSymbol = Symbols.StripSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Approach", Surface = PANDA.Constants.SurfaceType.Approach, Symbol = Symbols.ApproachDefaultSymbol, SelectedSymbol = Symbols.ApproachSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Inner Approach", Surface = PANDA.Constants.SurfaceType.InnerApproach, Symbol = Symbols.InnerApproachDefaultSymbol, SelectedSymbol = Symbols.InnerApproachSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Transiotonal", Surface = PANDA.Constants.SurfaceType.Transitional, Symbol = Symbols.TransitionalDefaultSymbol, SelectedSymbol = Symbols.TransitionalSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Inner Transiotonal", Surface = PANDA.Constants.SurfaceType.InnerTransitional, Symbol = Symbols.InnerTransitionalDefaultSymbol, SelectedSymbol = Symbols.InnerTransitionalSelectedSymbol});
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Take of Climb", Surface = PANDA.Constants.SurfaceType.TakeOffClimb, Symbol = Symbols.TakeOfClimbDefaultSymbol, SelectedSymbol = Symbols.TakeOfClimbSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Balked Landing", Surface = PANDA.Constants.SurfaceType.BalkedLanding, Symbol = Symbols.BalkedLandingDefaultSymbol, SelectedSymbol = Symbols.BalkedLandingSelectedSymbol });
                
            }

            SelectedModel = SettingsModelList[0];
           
        }
       
        public List<SettingsModel> SettingsModelList { get; set; }

        #region :> Surface Params
        private Visibility _interfacePanelIsVisible;
        public Visibility InterfacePanelIsVisible
        {
            get { return _interfacePanelIsVisible; }
            set 
            {
                _interfacePanelIsVisible = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("InterfacePanelIsVisible"));
            }
        }

        private Visibility _queryPanelIsVisible;
        public Visibility QueryPanelIsVisible
        {
            get { return _queryPanelIsVisible; }
            set
            { 
                _queryPanelIsVisible = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("QueryPanelIsVisible"));
            }
        }

        private Visibility _stylePanelIsVisible;
        public Visibility StylePanelIsVisible
        {
            get { return _stylePanelIsVisible; }
            set
            {
                _stylePanelIsVisible = value; 
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("StylePanelIsVisible"));
            }
        }

        private System.Windows.Media.Color _fillColor;
        public System.Windows.Media.Color FillColor
        {
            get { return _fillColor; }
            set 
            {
                _fillColor = value;

                SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                if (surfaceModel != null)
                    surfaceModel.Symbol.Color= ARANFunctions.RGB(_fillColor.R, _fillColor.G, _fillColor.B);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FillColor"));
            }
        }

        private System.Windows.Media.Color _outlineColor;
        public System.Windows.Media.Color OutlineColor
        {
            get { return _outlineColor; }
            set 
            {
                _outlineColor = value;

                SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                if (surfaceModel != null)
                    surfaceModel.Symbol.Outline.Color = ARANFunctions.RGB(_outlineColor.R, _outlineColor.G, _outlineColor.B);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OutlineColor"));
            }
        }

        private int _outlineWidth;
        public int OutlineWidth
        {
            get { return _outlineWidth; }
            set 
            {
                _outlineWidth = value;
                
                SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                if (surfaceModel != null)
                    surfaceModel.Symbol.Outline.Size = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OutlineWidth"));
            }
        }

        private System.Windows.Media.Color _selectedFillColor;
        public System.Windows.Media.Color SelectedFillColor
        {
            get { return _selectedFillColor; }
            set
            {
                _selectedFillColor = value;

                SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                if (surfaceModel != null)
                    surfaceModel.SelectedSymbol.Color = ARANFunctions.RGB(_selectedFillColor.R, _selectedFillColor.G, _selectedFillColor.B);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFillColor"));
            }
        }

        private System.Windows.Media.Color _selectedOutlineColor;
        public System.Windows.Media.Color SelectedOutlineColor
        {
            get { return _selectedOutlineColor; }
            set
            {
                _selectedOutlineColor = value;

                SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                if (surfaceModel != null)
                    surfaceModel.SelectedSymbol.Outline.Color = ARANFunctions.RGB(_selectedOutlineColor.R, _selectedOutlineColor.G, _selectedOutlineColor.B);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOutlineColor"));
            }
        }

        private int _selectedOutlineWidth;
        public int SelectedOutlineWidth
        {
            get { return _selectedOutlineWidth; }
            set
            {
                _selectedOutlineWidth = value;

                SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                if (surfaceModel != null)
                    surfaceModel.SelectedSymbol.Outline.Size = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOutlineWidth"));
            }
        }

        #endregion

        #region :> Query Params
        private List<OrganisationModel> GetOrganisationList(Guid identifier)
        {
            var orgList = new List<OrganisationModel>();

            dbProvider = (DbProvider)SettingsGlobals.AranEnvironment.DbProvider;
            GettingResult result = dbProvider.GetVersionsOf(FeatureType.OrganisationAuthority, TimeSliceInterpretationType.BASELINE);

            orgList.Add(new OrganisationModel(null, OrganisationType.All));

            if (result.IsSucceed && result.List.Count > 0)
            {
                List<OrganisationAuthority> tmpList = result.GetListAs<OrganisationAuthority>();
                tmpList.Sort(FeatureSorter);
                foreach (OrganisationAuthority orgAuthority in tmpList)
                {
                    var orgModel = new OrganisationModel(orgAuthority,OrganisationType.FromDatabase);
                    if (orgAuthority.Identifier == identifier)
                        SelectedOrganisation = orgModel;

                    orgList.Add(orgModel);
                }
                if (SelectedOrganisation == null)
                    SelectedOrganisation = orgList[0];
            }
            return orgList;
        }

        public ObservableCollection<OrganisationModel> OrganisationList { get; private set; }
        public ObservableCollection<AirportHeliport> AdhpList { get; private set; }

        private OrganisationModel _selectedOrganisation;
        public OrganisationModel SelectedOrganisation
        {
            get { return _selectedOrganisation; }
            set
            {
                _selectedOrganisation = value;

                AdhpList.Clear();

                if (_selectedOrganisation == null) return;

                _selectedAdhp = null;
                var orgAdhpList = GetAirportHeliports(_selectedOrganisation);

                if (orgAdhpList.Count > 0)
                {
                    orgAdhpList.Sort(FeatureSorter);
                    foreach (AirportHeliport adhp in orgAdhpList)
                    {
                        if (adhp.Identifier == Query.Aeroport)
                            SelectedAdhp = adhp;

                        AdhpList.Add(adhp);
                    }

                    if (SelectedAdhp == null)
                        SelectedAdhp = AdhpList[0];
                }

                if (_selectedOrganisation.Organisation!=null)
                    Query.Organization = _selectedOrganisation.Organisation.Identifier;
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
                {
                    Query.Aeroport = _selectedAdhp.Identifier;
                    CentralMeridian = Math.Round(_selectedAdhp.ARP.Geo.X) + "°";
                }
                else
                {
                    Query.Aeroport = Guid.Empty;
                    CentralMeridian = "";
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedAdhp"));
            }
        }

        private double _obstacleAreaRadius;
        public double ObstacleAreaRadius
        {
            get { return _obstacleAreaRadius; }
            set 
            {
                _obstacleAreaRadius = value;
                Query.Radius = value;
               
            }
        }

        public bool ValidationIsChecked
        {
            get { return Query.ValidationReportIsCheked; }
            set            
            {
                Query.ValidationReportIsCheked = value;
            }
        }

        public string CentralMeridian
        {
            get { return _centralMeridian; }
            set
            {
                _centralMeridian = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("CentralMeridian"));
            }
        }

        #endregion

        #region :>Interface Params

        public IList<string> DistanceUnits { get; private set; }
        public int SelectedDistanceIndex
        {
            get { return (int)_interface.DistanceUnit; }
            set
            {
                _interface.DistanceUnit =(VerticalDistanceType) value;
            }
        }
        public double DistancePrecision { get { return _interface.DistancePrecision; }
            set
            {
                _interface.DistancePrecision = value;
            }
        }

        public IList<string> HeightUnits { get; set; }
        public int SelectedHeightIndex { get { return (int)_interface.HeightUnit; }
            set 
            {
                _interface.HeightUnit = (VerticalDistanceType)value;
            }
        }

        public double HeightPrecision { get { return _interface.HeightPrecision; }
            set
            {
                _interface.HeightPrecision = value;
            }
        }
        #endregion
        private SettingsModel _selectedModel;
        private string _centralMeridian;

        public SettingsModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                if (_selectedModel == null)
                    return;

                if (_selectedModel.Type == MenuType.Interface)
                {
                    InterfacePanelIsVisible = System.Windows.Visibility.Visible;
                    QueryPanelIsVisible = System.Windows.Visibility.Collapsed;
                    StylePanelIsVisible = System.Windows.Visibility.Collapsed;
                }
                else if (_selectedModel.Type == MenuType.Query)
                {
                    InterfacePanelIsVisible = System.Windows.Visibility.Collapsed;
                    QueryPanelIsVisible = System.Windows.Visibility.Visible;
                    StylePanelIsVisible = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    InterfacePanelIsVisible = System.Windows.Visibility.Collapsed;
                    QueryPanelIsVisible = System.Windows.Visibility.Collapsed;
                    StylePanelIsVisible = System.Windows.Visibility.Visible;

                    SurfaceModel surfaceModel = _selectedModel as SurfaceModel;
                    FillColor = RGB.ToWindowsColor(surfaceModel.Symbol.Color);
                    OutlineColor = RGB.ToWindowsColor(surfaceModel.Symbol.Outline.Color);
                    OutlineWidth = surfaceModel.Symbol.Outline.Width;

                    if (surfaceModel.SelectedSymbol != null)
                    {
                        SelectedFillColor = RGB.ToWindowsColor(surfaceModel.SelectedSymbol.Color);
                        SelectedOutlineColor = RGB.ToWindowsColor(surfaceModel.SelectedSymbol.Outline.Color);
                        SelectedOutlineWidth = surfaceModel.SelectedSymbol.Outline.Width;
                    }
                }
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

        private List<AirportHeliport> GetAirportHeliports(OrganisationModel organisationModel)
        {
            Filter filter = null;
            if (organisationModel.OrganisationType== OrganisationType.FromDatabase)
            {
                ComparisonOps comops = new ComparisonOps(ComparisonOpType.EqualTo, "ResponsibleOrganisation.TheOrganisationAuthority", organisationModel.Organisation.Identifier);
                var choice = new OperationChoice(comops);
                filter = new Filter(choice);
            }
            GettingResult result = dbProvider.GetVersionsOf(FeatureType.AirportHeliport, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);
            if (result.IsSucceed && result.List.Count > 0)
                return result.GetListAs<AirportHeliport>();

            return null;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
