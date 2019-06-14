using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Panda.RadarMA.ElevationCalculator;
using Aran.Panda.RadarMA.Models;
using Aran.Panda.RadarMA.Utils;
using Aran.Panda.RadarMA.View;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ViewModels
{
    public class ReportViewModel:NotifyableBase
    {
        private const double Buffer = 3 * 1852;
        private Sector _selectedSector;
        private Sector _selectedUnAssignedSector;
        private readonly List<VerticalStructure> _vsList;
        private readonly ElavationCalculatorFacade _elevationCalculatorFacade;
        private readonly UnitConverter _unitConverter;
        private readonly IGeometry _radarVectoringArea;

        public ReportViewModel
            (ObservableCollection<Sector> sectorList,
            List<VerticalStructure> vsList,
            ElavationCalculatorFacade elevationCalculatorFacade,
            UnitConverter unitConverter,
            IGeometry radarVectoringArea)
        {
            SectorList = sectorList;
            _elevationCalculatorFacade = elevationCalculatorFacade;
            _radarVectoringArea = radarVectoringArea;

            UnAssignedSectorList = new ObservableCollection<Sector>();

            _vsList = vsList;
            _unitConverter = unitConverter;

            CreateUnassignedCommand = new RelayCommand(CreateUnAssignedSector);

            InitializeParams();
        }

        public RelayCommand ReportCommand => new RelayCommand((sector) =>
        {
            var selectedSector = sector as Sector;
            if (selectedSector == null)
            {
                return;
                //show message
            }
            var obstacleReportView = new ObstacleReportView(selectedSector.Reports);
            obstacleReportView.Show();
        });

        public void InitializeParams()
        {
            var unionGeometry = UnionSectors();
            var unAssignedSectors = GetUnassignedSector(unionGeometry);

            CalculateObstacles(unAssignedSectors);
        }

        public string HeaderName => "More";

        private IPolygon UnionSectors()
        {
            IPolygon unionGeometry = new PolygonClass();

            foreach (var sector in SectorList)
                unionGeometry = GeomOperators.UnionPolygon(unionGeometry, sector.Geo);

            return unionGeometry;
        }

        private IPolygon GetUnassignedSector(IPolygon allSectorsGeo)
        {
            return GeomOperators.Difference((IPolygon)_radarVectoringArea, allSectorsGeo);
        }

        private void CalculateObstacles(IPolygon unAssignedSectorGeo)
        {
            var unAssignedGeomCollection = unAssignedSectorGeo as IGeometryCollection;

            if (unAssignedGeomCollection != null)
            {
                IPolygon poly = new PolygonClass();

                for (int i = 0; i < unAssignedGeomCollection.GeometryCount; i++)
                {
                    
                    var curGeometry = unAssignedGeomCollection.Geometry[i];

                    if (curGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        poly = curGeometry as IPolygon;
                       
                    }
                    else
                    {
                        var ring = unAssignedGeomCollection.Geometry[i];
                        var unAssignedSector = CreateSector(unAssignedSectorGeo);
                        unAssignedSector.Number = i + 1;

                        UnAssignedSectorList.Add(unAssignedSector);
                        break;
                    }
                }
            }
        }

        private Sector CreateSector(IPolygon stateGeo)
        {
            var buffer = GeomOperators.Buffer(stateGeo, Buffer);

            var obstacleReports = _elevationCalculatorFacade.GetObstacleReports(buffer);
            var maxElevationReport = obstacleReports.OrderByDescending(obs => obs.Elevation)
                .FirstOrDefault();

            var unAssignedSector = new Sector(stateGeo, GlobalParams.RadarSymbol.CircleSymbol,
                obstacleReports, _unitConverter) {Buffer = buffer };

            if (maxElevationReport == null) return unAssignedSector;

            unAssignedSector.Height = maxElevationReport.Elevation;
            unAssignedSector.StateMaxElevPoint = maxElevationReport.GeoPrj as IPoint;
            return unAssignedSector;
        }

        private void CreateUnAssignedSector(object obj)
        {
            SectorList.Add(SelectedUnAssignedSector);
            /*
            if (SelectedUnAssignedSector != null)
            {
                var window = new CreateSector(SelectedUnAssignedSector);
                var parentHandle = new IntPtr(GlobalParams.Handle);
                var helper = new WindowInteropHelper(window) { Owner = parentHandle };
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                window.ShowDialog();
            }
            */
        }

        public ObservableCollection<Sector> SectorList { get; set; }
        public ObservableCollection<Sector> UnAssignedSectorList { get; set; }

        public RelayCommand CreateUnassignedCommand { get; set; }

        public Sector SelectedSector
        {
            get => _selectedSector;
            set
            {
                _selectedSector?.Clear();

                _selectedSector = value;
                _selectedSector?.Draw();

                NotifyPropertyChanged(nameof(SelectedSector));
            }
        }

        public Sector SelectedUnAssignedSector
        {
            get => _selectedUnAssignedSector;
            set
            {
                _selectedUnAssignedSector?.Clear();

                _selectedUnAssignedSector = value;
                _selectedUnAssignedSector?.Draw();

                NotifyPropertyChanged(nameof(SelectedUnAssignedSector));
            }
        }

        internal void Clear()
        {
            _selectedSector?.Clear();

            _selectedUnAssignedSector?.Clear();
        }
    }
}
