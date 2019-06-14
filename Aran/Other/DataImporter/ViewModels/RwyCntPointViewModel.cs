using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using DataImporter.Enums;
using DataImporter.Factories.RunwayLoader;
using DataImporter.Models;
using DataImporter.Repository;
using MahApps.Metro.Controls.Dialogs;

namespace DataImporter.ViewModels
{
    public class RwyCntPointViewModel:ViewModel
    {
        private readonly IRepository _repository;
        private readonly IAranGraphics _graphics;

        private AirportHeliport _selectedAdhp;
        private int _cntHandle;
        private RwyCenterPoint _selectedRwyCenterPoint;
        private readonly IRunwayLoaderFactory _runwayLoaderFactory;
        private readonly ILogger _logger;
        private IDialogCoordinator _dialogCoordinator;
        private IRunwayCntDbSaver _runwayCntDbSaver;

        public RwyCntPointViewModel(IRepository repository,
            IAranGraphics graphics,
            IRunwayLoaderFactory runwayLoaderFactory,
            IRunwayCntDbSaver runwayCntDbSaver,
            ICommonObject commonObject)
        {
            _repository = repository;
            _graphics = graphics;
            _runwayLoaderFactory = runwayLoaderFactory;
            _runwayCntDbSaver = runwayCntDbSaver;
            _logger =commonObject?.Logger;
            _dialogCoordinator = commonObject?.DialogCoordinator;

            RwyList = new ObservableCollection<Runway>();
            RwyCntList = new ObservableCollection<RwyCenterPoint>();

            if (AdhpList?.Count > 0)
                SelectedAdhp = AdhpList[0];

            ARANFunctions.InitEllipsoid();
        }

        public List<AirportHeliport> AdhpList => _repository?.AirportHeliportList;
        public ObservableCollection<Runway> RwyList { get; set; }

        public ObservableCollection<RwyCenterPoint> RwyCntList { get; set; }

        public AirportHeliport SelectedAdhp
        {
            get => _selectedAdhp;
            set
            {
                if (Equals(_selectedAdhp, value)) return;
                _selectedAdhp = value;

                RwyList?.Clear();
                var rwyList = _repository.GetRunwayList(_selectedAdhp.Identifier);
                rwyList.ForEach(rwy => RwyList.Add(rwy));
                if (RwyList != null && RwyList.Count > 0)
                    SelectedRwy = RwyList[0];

                NotifyPropertyChanged(nameof(SelectedAdhp));
            }
        }

        public Runway SelectedRwy { get; set; }

        public RwyCenterPoint SelectedRwyCenterPoint
        {
            get => _selectedRwyCenterPoint;
            set
            {
                _selectedRwyCenterPoint = value;
                _graphics.SafeDeleteGraphic(_cntHandle);

                if (_selectedRwyCenterPoint?.GeoPrj != null)
                    _cntHandle = _graphics.DrawPointWithText(_selectedRwyCenterPoint.GeoPrj, _selectedRwyCenterPoint.ID);

                NotifyPropertyChanged(nameof(SelectedRwyCenterPoint));
            }
        }

        public bool DecomitCntPoints { get; set; }

        public override void Save()
        {
            if (DecomitCntPoints)
                _runwayCntDbSaver.DecommitCenterPoints(SelectedRwy);

            _runwayCntDbSaver.Save(RwyCntList.ToList(), SelectedRwy);
        }

        public override void Load(string fileName)
        {
            RwyCntList.Clear();
            var rwyCntLoader = _runwayLoaderFactory.CreateFileLoader(RunwayFileFormatType.ExcelStandard, fileName);
            rwyCntLoader.RwyCenterPoints?.ForEach(rwyCnt => RwyCntList.Add(rwyCnt));
        }

        public override IFeatType FeatType =>new FeatTypeClass{ FeatType = FeatureType.RunwayCenterLine,Header = "Runway Centerlinepoint"};
    }
}
