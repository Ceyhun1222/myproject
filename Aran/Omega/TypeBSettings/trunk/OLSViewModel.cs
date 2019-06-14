using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Aran.Aim.Features;
using System.Windows;
using System.ComponentModel;
using Aran.AranEnvironment.Symbols;
using Aran.Aim.Data;
using Aran.Panda.Common;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Data.Filters;

namespace Aran.Omega.TypeB.Settings
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
                Globals.Settings.OLSQuery = value;
                OrganisationList = GetOrganisationList(_query.Organization);
                ObstacleAreaRadius = _query.Radius;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("OrganisationList"));
            }
        }
        private InterfaceModel _interface;
        public InterfaceModel Interface 
        {
            get { return _interface; }
            set 
            {
                _interface = value;
                Globals.Settings.OLSInterface = value;
                if (_interface != null) 
                {
                    SelectedDistanceIndex =(int) Interface.DistanceUnit;
                    SelectedHeightIndex =(int) Interface.HeightUnit;

                    DistancePrecision = Interface.DistancePrecision;
                    HeightPrecision = Interface.HeightPrecision;
                }
            }
        }

        public DbConnectionModel DbConnectionModel { get; set; }

        public  void Load()
        {
            OrganisationList = new ObservableCollection<OrganisationAuthority>();

            InterfacePanelIsVisible = System.Windows.Visibility.Visible;
            QueryPanelIsVisible = System.Windows.Visibility.Collapsed;
            StylePanelIsVisible = System.Windows.Visibility.Collapsed;

            DistanceUnits = new List<string>();
            DistanceUnits.Add("meter");
          //  DistanceUnits.Add("NM");

            HeightUnits = new List<string>();
            HeightUnits.Add("meter");
            HeightUnits.Add("feet");

            DbConnectionModel = Globals.Settings.DbConnection;
            if (DbConnectionModel != null)
                dbProvider = DbConnectionModel.DbProvider;

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

                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Inner horizontal",Surface= Panda.Constants.SurfaceType.InnerHorizontal, Symbol = Symbols.InnerApproachDefaultSymbol,SelectedSymbol=Symbols.InnerApproachSelectedSymbol});
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Conical surface",Surface= Panda.Constants.SurfaceType.CONICAL, Symbol = Symbols.ConicalDefaultSymbol,SelectedSymbol=Symbols.ConicalSelectedSymbol});
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Outer horizontal", Surface = Panda.Constants.SurfaceType.OuterHorizontal, Symbol = Symbols.OuterHorizontalDefaultSymbol, SelectedSymbol = Symbols.OuterHorizontalSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Strip", Surface = Panda.Constants.SurfaceType.Strip, Symbol = Symbols.StripDefaultSymbol, SelectedSymbol = Symbols.StripSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Approach", Surface = Panda.Constants.SurfaceType.Approach, Symbol = Symbols.ApproachDefaultSymbol, SelectedSymbol = Symbols.ApproachSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Inner Approach", Surface = Panda.Constants.SurfaceType.InnerApproach, Symbol = Symbols.InnerApproachDefaultSymbol, SelectedSymbol = Symbols.InnerApproachSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Transiotonal", Surface = Panda.Constants.SurfaceType.Transitional, Symbol = Symbols.TransitionalDefaultSymbol, SelectedSymbol = Symbols.TransitionalSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Inner Transiotonal", Surface = Panda.Constants.SurfaceType.InnerTransitional, Symbol = Symbols.InnerTransitionalDefaultSymbol, SelectedSymbol = Symbols.InnerTransitionalSelectedSymbol});
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Take of Climb", Surface = Panda.Constants.SurfaceType.TakeOffClimb, Symbol = Symbols.TakeOfClimbDefaultSymbol, SelectedSymbol = Symbols.TakeOfClimbSelectedSymbol });
                SettingsModelList.Add(new SurfaceModel { ViewCaption = "Balked Landing", Surface = Panda.Constants.SurfaceType.BalkedLanding, Symbol = Symbols.BalkedLandingDefaultSymbol, SelectedSymbol = Symbols.BalkedLandingSelectedSymbol });
                
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
        private ObservableCollection<OrganisationAuthority> GetOrganisationList(Guid identifier)
        {
            ObservableCollection<OrganisationAuthority> orgList = new ObservableCollection<OrganisationAuthority>();

            if (dbProvider != null)
            {
                GettingResult result = dbProvider.GetVersionsOf(FeatureType.OrganisationAuthority, TimeSliceInterpretationType.BASELINE);


                if (result.IsSucceed && result.List.Count > 0)
                {
                    List<OrganisationAuthority> tmpList = result.GetListAs<OrganisationAuthority>();
                    tmpList.Sort(FeatureSorter);
                    foreach (OrganisationAuthority orgAuthority in tmpList)
                    {
                        if (orgAuthority.Identifier == identifier)
                            SelectedOrganisation = orgAuthority;

                        orgList.Add(orgAuthority);
                    }
                    if (SelectedOrganisation == null)
                        SelectedOrganisation = orgList[0];
                }
                
            }
            return orgList;
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

                ComparisonOps comops = new ComparisonOps(ComparisonOpType.EqualTo, "ResponsibleOrganisation.TheOrganisationAuthority", _selectedOrganisation.Identifier);
                OperationChoice choice = new OperationChoice(comops);
                Filter filter = new Filter(choice);
                GettingResult result = dbProvider.GetVersionsOf(FeatureType.AirportHeliport, TimeSliceInterpretationType.BASELINE, default(Guid), false, null, null, filter);

                AdhpList = new ObservableCollection<AirportHeliport>();
                _selectedAdhp = null;
                if (result.IsSucceed && result.List.Count > 0)
                {
                    List<AirportHeliport> tmpList = result.GetListAs<AirportHeliport>();
                    tmpList.Sort(FeatureSorter);
                    foreach (AirportHeliport adhp in tmpList)
                    {
                        if (adhp.Identifier == Query.Aeroport)
                            SelectedAdhp = adhp;

                        AdhpList.Add(adhp);
                    }

                    if (SelectedAdhp == null)
                        SelectedAdhp = AdhpList[0];
                }

                Query.Organization = _selectedOrganisation.Identifier;
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
                    Query.Aeroport = _selectedAdhp.Identifier;
                else
                    Query.Aeroport = Guid.Empty;

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
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
