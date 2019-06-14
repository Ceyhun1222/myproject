using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ANCOR.MapCore;
using ChartTypeA.Models;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartTypeA.ViewModels
{
    public class SelectRunwayViewModel:ViewModel
    {
        private PDM.Runway _selectedRunway;
        private int _selectedElevUnit;
        private int _selectedDistanceUnit;
        private int _adhpHandle;
        private PDM.AirportHeliport _selectedAirport;

        public SelectRunwayViewModel()
        {
            Header = "ICAO Chart Type A ( Select Runway )";

            RwyDirList = new ObservableCollection<RwyDirWrapper>();
            RunwayList = new ObservableCollection<Runway>();
            AirportList = GlobalParams.DbModule.AirportHeliportList;
            if (AirportList != null && AirportList.Count > 0)
                SelectedAirport = AirportList[0];

            DistanceUnits = new List<string>();
            DistanceUnits.Add("m");
       //     DistanceUnits.Add("ft");
            //  DistanceUnits.Add("NM");
            SelectedDistanceUnit = 0;

            HeightUnits = new List<string>();
            HeightUnits.Add("m"); 
            HeightUnits.Add("ft");
            SelectedElevUnit = 0;
            
            HorAccuracy = 1;
            VerAccuracy = 1;

        }

        public List<PDM.AirportHeliport> AirportList { get; set; }
        public ObservableCollection<PDM.Runway> RunwayList { get; set; }
        public ObservableCollection<Models.RwyDirWrapper> RwyDirList { get; set; }
        public List<string> DistanceUnits { get; set; }
        public List<string> HeightUnits { get; set; }

        public PDM.AirportHeliport SelectedAirport
        {
            get { return _selectedAirport; }
            set
            {
                _selectedAirport = value;
                RunwayList.Clear();

                GlobalParams.UI.SafeDeleteGraphic(_adhpHandle);
                if (_selectedAirport != null)
                {
                    var tmpList = GlobalParams.DbModule.GetRunwayList(_selectedAirport);
                    if (tmpList != null)
                        tmpList.ForEach(rwy => RunwayList.Add(rwy));

                    if (RunwayList.Count > 0)
                        SelectedRunway = RunwayList[0];

                    _selectedAirport.RebuildGeo();
                    if (_selectedAirport.Geo != null)
                    {
                        var adhpPt = GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedAirport.Geo) as IPoint;
                        if (adhpPt!=null)
                            _adhpHandle = GlobalParams.UI.DrawEsriPoint((IPoint)adhpPt);

                        //ChartHelperClass.ChangeProjectionAndMeredian((_selectedAirport.Geo as IPoint).X, GlobalParams.Map);
                    }

                    if (RunwayList == null && RunwayList.Count == 0)
                    {
                        if (CanGoNextEvent != null)
                            CanGoNextEvent(RunwayList.Count > 0 ? true : false, new EventArgs());
                    }
                }

                NotifyPropertyChanged("SelectedAirport");
            }
        }

        public PDM.Runway SelectedRunway
        {
            get { return _selectedRunway; }
            set
            {
                _selectedRunway = value;
                RwyDirList.Clear();
                if (_selectedRunway != null)
                {
                    if (_selectedRunway.RunwayDirectionList != null)
                    {
                        foreach (var runwayDirection in _selectedRunway.RunwayDirectionList)
                        {
                            var rwyDirWrapper = new RwyDirWrapper(runwayDirection);
                            rwyDirWrapper.RwyCheckedIsChanged += rwyDirWrapper_RwyCheckedIsChanged;
                            
                            if (rwyDirWrapper.IsEligible)
                                RwyDirList.Add(rwyDirWrapper);
                        }
                    }

                    if (CanGoNextEvent != null)
                        CanGoNextEvent(RwyDirList.Count > 0 ? true : false, new EventArgs());
                }
                NotifyPropertyChanged("SelectedRunway");
            }
        }

        void rwyDirWrapper_RwyCheckedIsChanged(object sender, EventArgs e)
        {
            var selectedCount = RwyDirList.Count(rwyDir => rwyDir.Checked);
                if (CanGoNextEvent != null) 
                    CanGoNextEvent(selectedCount>0?true:false,new EventArgs());

            //Profile profile =new Profile(RwyDirList[0]);
        }

        public int SelectedElevUnit
        {
            get { return _selectedElevUnit; }
            set
            {
                _selectedElevUnit = value;
                InitChartTypeA.HeightConverter = InitChartTypeA.HeightConverterList[_selectedElevUnit];
                //GlobalParams.HeightUnitConverter.HeightUnitIndex = _selectedElevUnit;
                SelectedAirport = _selectedAirport;

                NotifyPropertyChanged("SelectedElevUnit");
                NotifyPropertyChanged("VerUnit");
            }
        }

        public int SelectedDistanceUnit
        {
            get { return _selectedDistanceUnit; }
            set
            {
                _selectedDistanceUnit = value;
                InitChartTypeA.DistanceConverter = InitChartTypeA.DistanceConverterList[_selectedDistanceUnit];
                //GlobalParams.DistanceUnitConverter.HeightUnitIndex = _selectedDistanceUnit;
                SelectedAirport = _selectedAirport;
                NotifyPropertyChanged("SelectedDistanceUnit");
                NotifyPropertyChanged("HorUnit");
            }
        }

        public double HorAccuracy { get; set; }

        public double VerAccuracy { get; set; }

        public string HorUnit {
            get { return InitChartTypeA.DistanceConverter.Unit; }
        }

        public string VerUnit {
            get { return InitChartTypeA.HeightConverter.Unit; }
        }

        public override void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_adhpHandle);
        }

        public virtual event EventHandler CanGoNextEvent;
    }
}
