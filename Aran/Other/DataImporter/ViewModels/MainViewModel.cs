using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using DataImporter.Export;
using DataImporter.Models;
using DataImporter.Repository;
using MahApps.Metro.Controls.Dialogs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DataImporter.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private string _fileName;

        
        private IFeatType _selectedFeatType;
        private IImportPageVM _currentViewModel;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly SpatialReferenceOperation _spOperation;
        private readonly List<IImportPageVM> _pageList;
        private readonly EsriObstacleExporter _esriObstacleExporter;

        public MainViewModel(List<IImportPageVM> pageList,
            IDialogCoordinator dialogCoordinator,
            SpatialReferenceOperation spOperation,
            EsriObstacleExporter esriObstacleExporter)
        {
            _pageList = pageList?? throw new ArgumentNullException($"Pages doesn't configure properly");
            _dialogCoordinator = dialogCoordinator?? throw new ArgumentNullException(
                                     $"Dialog doesn't configure properly");
            _spOperation = spOperation ?? throw new ArgumentNullException(
                               $"SpatialReferenceOperation doesn't configure properly");
            _esriObstacleExporter = esriObstacleExporter?? throw new ArgumentNullException(
                                        $"SpatialReferenceOperation doesn't configure properly");

            FeatureTypes = new List<IFeatType>();
            _pageList.ForEach(page=>
            {
                if (page != null)
                    FeatureTypes.Add(page.FeatType);
            });

            if (FeatureTypes.Count > 0)
                SelectedFeatType = FeatureTypes[0];
        }

        public RelayCommand SelectFileCommand => new RelayCommand(SelectFile);
        public RelayCommand ApplyCommand=>new RelayCommand(Apply);

        public RelayCommand ExportToGdbCommand=>new RelayCommand(async (obj) =>
        {
            var obstacleViewModel = CurrentViewModel as ObstacleViewModel;
            if (obstacleViewModel == null)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Data Importer", "It works only with obstacles!")
                    .ConfigureAwait(false);
                return;
            }

            var folderPath = FolderPath.GetFolderPath();
            if (string.IsNullOrEmpty(folderPath))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Data Importer", "Folder is not selected properly!")
                    .ConfigureAwait(false);
                return;
            }
            _esriObstacleExporter.SaveVs(obstacleViewModel.Obstacles.ToList(),folderPath);
        });

        public IImportPageVM CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public List<IFeatType> FeatureTypes { get; set; }

        

        public IFeatType SelectedFeatType
        {
            get => _selectedFeatType;
            set
            {
                _selectedFeatType = value;
                if (_selectedFeatType != null)
                    CurrentViewModel = _pageList.FirstOrDefault(page => page.FeatType.FeatType == _selectedFeatType.FeatType);

                NotifyPropertyChanged(nameof(SelectedFeatType));
            }
        }
        
        /// <exception cref="Exception" accessor="set">A delegate callback throws an exception.</exception>
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        private void Apply(object obj)
        {
            Save();
            try
            {
                Save();
                _dialogCoordinator.ShowMessageAsync(this, "Data Importer", "Successfully saved to Database");
            }
            catch (Exception e)
            {
                _dialogCoordinator.ShowMessageAsync(this, "Data Importer", "Successfully saved to Database",MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary);
            }
        }

        private void SelectFile(object obj)
        {
            var fDialog = new OpenFileDialog
            {
                DefaultExt = ".xls",
                Title = "Open Excel File"
            };
            // Default file extension
            var result = fDialog.ShowDialog();
            if (result == true)
            {
                FileName = fDialog.FileName;
                CurrentViewModel.Load(FileName);
            }
        }

        public override void Save()
        {
            CurrentViewModel?.Save();
        }

        public override void Load(string fileName)
        {
            CurrentViewModel.Load(fileName);
        }

        public override IFeatType FeatType { get; }

        //private void DecomitRunwayCenterLinePoint()
        //{
        //    var rwyDirList = _pandaQPI.GetFeatureList(Aran.Aim.FeatureType.RunwayDirection, null).
        //                 Cast<RunwayDirection>().Where(rwyDir => rwyDir.UsedRunway.Identifier == _selectedRwy.Identifier);

        //    foreach (var rwyDir in rwyDirList)
        //    {
        //        var rwyCenterList = _pandaQPI.GetFeatureList(Aran.Aim.FeatureType.RunwayCentrelinePoint, null).
        //                 Cast<RunwayCentrelinePoint>().Where(allRwy => allRwy.OnRunway.Identifier == rwyDir.Identifier);

        //        foreach (var rwyCenter in rwyCenterList)
        //        {
        //            rwyCenter.TimeSlice.FeatureLifetime.EndPosition = rwyCenter.TimeSlice.FeatureLifetime.BeginPosition;
        //            _pandaQPI.SetFeature(rwyCenter);
        //        }
        //    }

        //    bool result = _pandaQPI.CommitWithoutViewer(new FeatureType[]
        //          {FeatureType.RunwayCentrelinePoint});
        //    if (result)
        //        MessageBox.Show("Data saved database successfully!", "Data Import", MessageBoxButtons.OK,
        //            MessageBoxIcon.Information);


        //}

        //private void LoadAndUpdateGeoid()
        //{
        //    if (AdhpList == null) return;

        //    foreach (var adhp in AdhpList)
        //    {
        //        var rwyList = _pandaQPI.GetRunwayList(adhp.Identifier);
        //        var airportType = GetAirportyType(adhp.Designator);
        //        foreach (var rwy in rwyList)
        //        {
        //            var rwyDirList = _pandaQPI.GetRunwayDirectionList(rwy.Identifier);
        //            foreach (var rwyDir in rwyDirList)
        //            {
        //                var rwyCntLinePointList = _pandaQPI.GetRunwayCentrelinePointList(rwyDir.Identifier);
        //                foreach (var rwyCntPoint in rwyCntLinePointList)
        //                {
        //                    AddGeoidUndulation(airportType, rwyCntPoint);
        //                    _pandaQPI.SetFeature(rwyCntPoint);
        //                }
        //            }
        //        }
        //    }

        //    _pandaQPI.CommitWithoutViewer(new FeatureType[] { FeatureType.RunwayCentrelinePoint });


        //}

        //private void AddGeoidUndulation(AirportType airportType, RunwayCentrelinePoint rwyCenter)
        //{
        //    if (rwyCenter == null) return;

        //    rwyCenter.Location.VerticalDatum = CodeVerticalDatum.EGM_96;


        //    switch (airportType)
        //    {
        //        case AirportType.Brest:
        //            rwyCenter.Location.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned(92, UomDistance.FT);
        //            break;
        //        case AirportType.Vicicebsk:
        //            rwyCenter.Location.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned(62, UomDistance.FT);
        //            break;
        //        case AirportType.Hrodna:
        //            rwyCenter.Location.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned(90, UomDistance.FT);
        //            break;
        //        case AirportType.Homiel:
        //            rwyCenter.Location.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned(67, UomDistance.FT);
        //            break;
        //        case AirportType.Mahiliou:
        //            rwyCenter.Location.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned(64, UomDistance.FT);
        //            break;
        //        case AirportType.Minsk:
        //            rwyCenter.Location.GeoidUndulation = new Aran.Aim.DataTypes.ValDistanceSigned(74, UomDistance.FT);
        //            break;
        //        default:
        //            break;
        //    }

        //}

       
    }
}
