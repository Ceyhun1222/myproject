using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Conventions;
using MvvmCore;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Control.XmlViewer;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class ImportToolViewModel : ToolViewModel, IPresenterParent
    {
        public static string ToolContentId = "Import";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/import.png", UriKind.RelativeOrAbsolute);

        public ImportToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
            DataPresenter.AddPredefinedColumn("#", "IsChecked", typeof(CheckBox));
            DataPresenter2 = new DataPresenterModel { ViewModel = this };
        }


        private XmlViewerViewModel _xmlViewerViewModel;
        public XmlViewerViewModel XmlViewerViewModel => _xmlViewerViewModel ?? (_xmlViewerViewModel = new XmlViewerViewModel());


        private string _sourceFileName;
        public string SourceFileName
        {
            get => _sourceFileName;
            set
            {
                _sourceFileName = value;
                OnPropertyChanged("SourceFileName");
            }
        }


        private int _importFeaturesIndex;
        public int ImportFeaturesIndex
        {
            get => _importFeaturesIndex;
            set
            {
                _importFeaturesIndex = value;
                OnPropertyChanged("ImportFeaturesIndex");
                ReloadDataToBeImported();
            }
        }


        private bool _canImport = false;
        public bool CanImport
        {
            get => _canImport;
            set
            {
                _canImport = value;
                OnPropertyChanged(nameof(CanImport));
            }
        }


        public bool IgnoreNotes
        {
            get => AObjectListConfig.IgnoreNotes;
            set
            {
                AObjectListConfig.IgnoreNotes = value;
                OnPropertyChanged(nameof(IgnoreNotes));
            }
        }

        private bool _importMade;
        public bool ImportMade
        {
            get => _importMade;
            set
            {
                _importMade = value;
                OnPropertyChanged(nameof(ImportMade));
            }
        }


        private static string SimpleName(string filename)
        {
            var i = filename.LastIndexOf("\\", StringComparison.Ordinal);
            if (i == -1)
            {
                i = filename.LastIndexOf("//", StringComparison.Ordinal);
            }

            if (i > -1)
            {
                filename = filename.Substring(i + 1);
            }

            return filename;
        }

        private readonly AixmHelper _aimHelper = new AixmHelper();


        public RelayCommand ViewXmlCommand
        {
            get
            {
                return _viewXmlCommand ?? (_viewXmlCommand = new RelayCommand(

              t =>
              {
                  var wrapper = DataPresenter.SelectedFeature as ReadonlyFeatureWrapper;
                  if (wrapper == null) return;

                  var xml = wrapper.Xml;
                  if (xml != null)
                  {
                      var xmlDocument = new XmlDocument();
                      try
                      {
                          xmlDocument.LoadXml(xml);

                          XmlViewerViewModel.XmlDocument = xmlDocument;
                      }
                      catch (XmlException)
                      {
                          MessageBoxHelper.Show("The XML content is invalid.");
                          return;
                      }



                      XmlViewerViewModel.Visibility = Visibility.Visible;
                  }

              },
              t => DataPresenter.SelectedFeature != null));
            }
        }

        private RelayCommand _addToExportCommand;
        public RelayCommand AddToExportCommand
        {
            get
            {


                return _addToExportCommand ?? (_addToExportCommand = new RelayCommand(
                           t =>
                           {
                               DataPresenter.BlockerModel.BlockForAction(
                                   () =>
                                   {
                                       foreach (var relation in RelationsData2)
                                       {
                                           foreach (var feature in relation.Items)
                                           {
                                               MainManagerModel.Instance.AddToExport(((ReadonlyFeatureWrapper)feature).Feature.Feature);
                                           }
                                       }

                                       MainManagerModel.Instance.ExportToolViewModel.SelectedRelation = null;
                                       MainManagerModel.Instance.ExportToolViewModel.Reload();

                                   }
                               );

                           },
                           t => ImportMade));
            }
        }

        public RelayCommand OpenSourceCommand
        {
            get
            {
                return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(
                    t2 =>
                    {

                        ImportMade = false;
                        MemoryUtil.CompactLoh();

                        var dlg = new Microsoft.Win32.OpenFileDialog
                        {
                            Title = "Open Snapshot",
                            DefaultExt = ".xml",
                            Filter = "Xml Files (*.xml)|*.xml|All Files|*.*"
                        };

                        if (dlg.ShowDialog() == true)
                        {
                            DataPresenter.BlockerModel.BlockForAction(
                                () =>
                                {
                                    var ci = Thread.CurrentThread.CurrentCulture;
                                    try
                                    {
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                                        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                                        //clear all
                                        RelationsData.Clear();
                                        SourceFileName = null;

                                        MainManagerModel.Instance.StatusText = "Analyzing file...";
                                        RelationsDataGridVisibility = Visibility.Visible;

                                        int count = 0;
                                        int totalCount = 0;
                                        var relationList = new List<SingleTypeRelationViewModel>();

                                        var list = relationList;
                                        _aimHelper.Open(dlg.FileName,
                                            () =>
                                            {
                                                totalCount++;
                                                MainManagerModel.Instance.StatusText =
                                                    totalCount + " features loaded...";
                                            },
                                            () => { MainManagerModel.Instance.StatusText = "Cleaning memory..."; },
                                            (aixmFeatureList, collection) =>
                                            {
                                                foreach (Feature feature in aixmFeatureList)
                                                {
                                                    var xml = aixmFeatureList.Xml(feature);
                                                    AddFeatureToModels(feature, xml, list);
                                                    count++;
                                                    MainManagerModel.Instance.StatusText =
                                                        "Processed " + count + " features from " + totalCount + "...";
                                                }

                                                collection.Clear(); //clear memory
                                            });

                                        if (_aimHelper.IsOpened)
                                        {
                                            SourceFileName = SimpleName(dlg.FileName);
                                            //sort relations
                                            relationList = relationList.OrderBy(t => t.Direction)
                                                .ThenBy(t => t.FeatureType.ToString()).ToList();

                                            //apply relations
                                            RelationsData.Clear();
                                            foreach (var model in relationList)
                                            {
                                                RelationsData.Add(model);
                                            }

                                            //select first
                                            if (RelationsData.Count > 0)
                                            {
                                                SelectedRelation = RelationsData.First();
                                            }
                                        }
                                        else
                                        {
                                            //fail to load
                                            //clear all
                                            RelationsData.Clear();
                                            SourceFileName = null;
                                        }

                                        ReloadDataToBeImported();

                                        MainManagerModel.Instance.StatusText = "Cleaning memory...";
                                        MemoryUtil.CompactLoh();
                                        GC.WaitForPendingFinalizers();

                                        MainManagerModel.Instance.StatusText =
                                            _aimHelper.IsOpened ? "Done" : "Failed to open";
                                    }
                                    finally
                                    {
                                        Thread.CurrentThread.CurrentCulture = ci;
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                    }
                                });
                        }
                    }, t3 => CurrentDataContext.CurrentUser?.ActivePrivateSlot != null));
            }
        }


        private void AddFeatureToModels(AimFeature feature, string xml, List<SingleTypeRelationViewModel> relationList)
        {
            //aimFeature.InitEsriExtension();
            var interpretation = feature.Feature.TimeSlice.Interpretation;
            var relatedModel = relationList.FirstOrDefault(t =>
                  t.FeatureType == feature.FeatureType &&
                  t.TimeSliceInterpretationType == interpretation);
            if (relatedModel == null)
            {
                relatedModel = new SingleTypeRelationViewModel
                {
                    FeatureType = feature.FeatureType,
                    OnFeatureTypeChecked = OnFeatureTypeChecked,
                    TimeSliceInterpretationType = interpretation
                };
                relationList.Add(relatedModel);
            }
            relatedModel.Items.Add(new ReadonlyFeatureWrapper(feature) { Xml = xml, OnFeatureChecked = OnFeatureChecked });
        }


        private bool _isReloading;

        private void OnFeatureTypeChecked(SingleTypeRelationViewModel model)
        {
            if (_isReloading) return;

            _isReloading = true;

            foreach (var modelItem in model.Items)
            {
                ((ReadonlyFeatureWrapper)modelItem).IsChecked = model.IsFeatureTypeChecked;
            }

            if (ImportFeaturesIndex > 0)
            {
                ReloadDataToBeImported();
            }
            _isReloading = false;
        }
        private void OnFeatureChecked(ReadonlyFeatureWrapper wrapper)
        {
            if (_isReloading) return;

            _isReloading = true;
            if (ImportFeaturesIndex > 0)
            {

                ReloadDataToBeImported();
            }



            foreach (var reloadData in RelationsData.Where(t => t.Items.All(i => !((ReadonlyFeatureWrapper)i).IsChecked)))
            {
                reloadData.IsFeatureTypeChecked = false;
            }

            foreach (var reloadData in RelationsData.Where(t => t.Items.All(i => ((ReadonlyFeatureWrapper)i).IsChecked)))
            {
                reloadData.IsFeatureTypeChecked = true;
            }

            _isReloading = false;
        }

        private void ReloadDataToBeImported()
        {
            ReloadIssues();
        }

        private IEnumerable<ReadonlyFeatureWrapper> AllAimFeatures()
        {
            return RelationsData.SelectMany(t => t.Items).AsEnumerable().Cast<ReadonlyFeatureWrapper>();
        }

        private void ReloadIssues()
        {
            DataPresenter.BlockerModel.BlockForAction(
                () =>
                {
                    DataPresenter2.BlockerModel.Block();
                    RelationsData2.Clear();

                    if (ImportFeaturesIndex == 0)
                    {
                        //import all
                        AddAllRelations();
                    }
                    else if (ImportFeaturesIndex == 1)
                    {
                        HashSet<AimFeature> addedAimFeatures = AddSelectedRelations();
                        if (CurrentDataContext.CurrentUser.ActivePrivateSlot != null)
                        {
                            CanImport = true;

                            foreach (var aimFeature in addedAimFeatures)
                            {
                                var featurePropList = new List<RefFeatureProp>();
                                AimMetadataUtility.GetReferencesFeatures(aimFeature.Feature, featurePropList);
                                foreach (var prop in featurePropList)
                                {
                                    var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                                    {
                                        Guid = prop.RefIdentifier,
                                        FeatureTypeId = (int)prop.FeatureType
                                    }, false,
                                        CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate);
                                    if (states != null && states.Count > 0)
                                        continue;
                                    CanImport = false;
                                    MessageBoxHelper.Show(
                                        $"{aimFeature.FeatureType} {aimFeature.Identifier} has broken link {prop.FeatureType} {prop.RefIdentifier}");
                                    break;
                                }
                                if (CanImport == false)
                                    break;
                            }
                        }
                        else
                        {
                            CanImport = false;
                        }
                    }
                    else
                    {
                        //import selected
                        HashSet<AimFeature> addedAimFeatures = AddSelectedRelations();

                        //add linked features
                        var toBeAdded = new List<ReadonlyFeatureWrapper>();
                        do
                        {
                            toBeAdded.Clear();
                            foreach (var aimFeature in addedAimFeatures)
                            {
                                var featurePropList = new List<RefFeatureProp>();
                                AimMetadataUtility.GetReferencesFeatures(aimFeature.Feature, featurePropList);
                                foreach (var prop in featurePropList)
                                {
                                    if (toBeAdded.All(t => t.Identifier != prop.RefIdentifier)
                                        && addedAimFeatures.All(t => t.Feature.Identifier != prop.RefIdentifier))
                                    {
                                        var referencedFeatures =
                                            AllAimFeatures()
                                                .Where(
                                                    t => t.Identifier == prop.RefIdentifier)
                                                .ToList();
                                        toBeAdded.AddRange(referencedFeatures);
                                    }
                                }
                            }
                            //check for exit
                            if (toBeAdded.Count == 0)
                            {
                                break;
                            }
                            //add more
                            foreach (var wrapper in toBeAdded)
                            {
                                //add to relations
                                var aimFeature = wrapper.Feature;

                                var model = RelationsData2.FirstOrDefault(
                                    t =>
                                        t.FeatureType == aimFeature.FeatureType &&
                                        t.TimeSliceInterpretationType == aimFeature.Feature.TimeSlice.Interpretation);

                                if (model == null)
                                {
                                    model = new SingleTypeRelationViewModel
                                    {
                                        FeatureType = aimFeature.FeatureType,
                                        TimeSliceInterpretationType = aimFeature.Feature.TimeSlice.Interpretation
                                    };
                                    RelationsData2.Add(model);
                                }


                                model.Items.Add(wrapper);
                                //add to list
                                addedAimFeatures.Add(aimFeature);
                            }
                        } while (toBeAdded.Count > 0);
                    }
                    DataPresenter2.BlockerModel.Unblock();
                }
                );
        }

        private HashSet<AimFeature> AddSelectedRelations()
        {
            var addedAimFeatures = new HashSet<AimFeature>();
            foreach (var model in RelationsData.Where(t => t.Items.Any(i => ((ReadonlyFeatureWrapper)i).IsChecked)))
            {
                var clone = new SingleTypeRelationViewModel
                {
                    FeatureType = model.FeatureType,
                    TimeSliceInterpretationType = model.TimeSliceInterpretationType
                };
                foreach (var item in model.Items.Where(t => ((ReadonlyFeatureWrapper)t).IsChecked))
                {
                    clone.Items.Add(item);
                    addedAimFeatures.Add(((ReadonlyFeatureWrapper)item).Feature);
                }

                RelationsData2.Add(clone);
            }

            return addedAimFeatures;
        }

        private void AddAllRelations()
        {
            foreach (var model in RelationsData)
            {
                var clone = new SingleTypeRelationViewModel
                {
                    FeatureType = model.FeatureType,
                    TimeSliceInterpretationType = model.TimeSliceInterpretationType
                };
                foreach (var item in model.Items)
                {
                    clone.Items.Add(item);
                }

                RelationsData2.Add(clone);
            }
        }

        public DataPresenterModel DataPresenter { get; private set; }
        public DataPresenterModel DataPresenter2 { get; private set; }

        #region Load



        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;

        }

        #endregion

        #region Relations UI

        private SingleTypeRelationViewModel _selectedRelation;
        public SingleTypeRelationViewModel SelectedRelation
        {
            get => _selectedRelation;
            set
            {
                _selectedRelation = value;
                OnPropertyChanged("SelectedRelation");

                if (SelectedRelation == null) return;

                DataPresenter.FeatureType = SelectedRelation.FeatureType;
            }
        }

        private ICollectionView _relationsDataFiltered;
        public ICollectionView RelationsDataFiltered
        {
            get
            {
                if (_relationsDataFiltered == null)
                {
                    _relationsDataFiltered = CollectionViewSource.GetDefaultView(RelationsData);
                    _relationsDataFiltered.GroupDescriptions.Clear();
                    _relationsDataFiltered.GroupDescriptions.Add(new PropertyGroupDescription("PurposeDescription"));
                }
                return _relationsDataFiltered;
            }
            set => _relationsDataFiltered = value;
        }

        private MtObservableCollection<SingleTypeRelationViewModel> _relationsData;
        public MtObservableCollection<SingleTypeRelationViewModel> RelationsData
        {
            get => _relationsData ?? (_relationsData = new MtObservableCollection<SingleTypeRelationViewModel>());
            set
            {

                _relationsData = value;
                OnPropertyChanged("RelationsData");
            }
        }

        private Visibility _relationsDataGridVisibility;
        public Visibility RelationsDataGridVisibility
        {
            get => _relationsDataGridVisibility;
            set
            {
                _relationsDataGridVisibility = value;
                OnPropertyChanged("RelationsDataGridVisibility");
            }
        }

        #endregion

        #region Relations2 UI

        private SingleTypeRelationViewModel _selectedRelation2;
        public SingleTypeRelationViewModel SelectedRelation2
        {
            get => _selectedRelation2;
            set
            {
                _selectedRelation2 = value;
                OnPropertyChanged("SelectedRelation2");

                if (SelectedRelation2 == null) return;

                DataPresenter2.FeatureType = SelectedRelation2.FeatureType;
            }
        }

        private ICollectionView _relationsDataFiltered2;
        public ICollectionView RelationsDataFiltered2
        {
            get
            {
                if (_relationsDataFiltered2 == null)
                {
                    _relationsDataFiltered2 = CollectionViewSource.GetDefaultView(RelationsData2);
                    _relationsDataFiltered2.GroupDescriptions.Clear();
                    _relationsDataFiltered2.GroupDescriptions.Add(new PropertyGroupDescription("PurposeDescription"));
                }
                return _relationsDataFiltered2;
            }
            set => _relationsDataFiltered2 = value;
        }

        private MtObservableCollection<SingleTypeRelationViewModel> _relationsData2;
        public MtObservableCollection<SingleTypeRelationViewModel> RelationsData2
        {
            get => _relationsData2 ?? (_relationsData2 = new MtObservableCollection<SingleTypeRelationViewModel>());
            set
            {

                _relationsData2 = value;
                OnPropertyChanged("RelationsData2");
            }
        }

        private Visibility _relationsDataGridVisibility2;
        public Visibility RelationsDataGridVisibility2
        {
            get => _relationsDataGridVisibility2;
            set
            {
                _relationsDataGridVisibility2 = value;
                OnPropertyChanged("RelationsDataGridVisibility2");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _unlinkCommand;
        public RelayCommand UnlinkCommand
        {
            get
            {
                return _unlinkCommand ?? (_unlinkCommand = new RelayCommand(
                t => throw new NotImplementedException(),
                delegate
                    {
                        if (DataPresenter.SelectedFeature == null) return false;
                        if (SelectedRelation == null) return false;
                        if (SelectedRelation.Direction != RelationDirection.Reverse) return false;
                        if (CurrentDataContext.CurrentUser == null) return false;
                        if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;

                        return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate == AiracDate;
                    }));
            }
            set => _unlinkCommand = value;
        }

        private RelayCommand _importCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ImportCommand
        {
            get
            {
                return _importCommand
                    ?? (_importCommand = new RelayCommand(
                        execute: t =>
                        {
                            DataPresenter.BlockerModel.BlockForAction(() =>
                           ImportToSlot(CurrentDataContext.CurrentUser.ActivePrivateSlot.Id,
                               CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate));
                            ImportMade = true;
                        },
                        canExecute: t => !DataPresenter.BlockerModel.IsBlocked
                        && CurrentDataContext.CurrentUser?.ActivePrivateSlot != null && RelationsData2.IsNotEmpty() && (ImportFeaturesIndex != 1 || CanImport)));
            }
        }




        private void ImportToSlot(int slotId, DateTime effectiveDate)
        {
            bool wasFirstNotification = false;
            bool showNotifications = true;
            int totalCount = 0;
            int count = 0;
            totalCount = RelationsData2.Sum(t => t.Items.Count);
            foreach (var relation in RelationsData2)
            {
                var type = relation.FeatureType;
                var typeTotalCount = relation.Items.Count;
                var typeCount = 0;

                foreach (var feature in relation.Items)
                {
                    var f = ((ReadonlyFeatureWrapper) feature)?.Feature?.Feature;
                    try
                    {
                        if (f != null)
                            Migrator.AddFeatureToSlot(f, slotId, effectiveDate);
                    }
                    catch (Exception e)
                    {
                        if (showNotifications)
                        {
                            MessageBoxHelper.Show(e.Message);

                            if (!wasFirstNotification)
                            {
                                if (MessageBoxResult.Yes == MessageBoxHelper.Show(
                                        "Do you want to ignore all feature notifications?",
                                        "Notifications", MessageBoxButton.YesNo, MessageBoxImage.Question))
                                    showNotifications = false;

                                wasFirstNotification = true;
                            }
                        }
                    }

                    typeCount++;
                    count++;
                    MainManagerModel.Instance.StatusText = $"Total: {count}/{totalCount}, {type}: {typeCount}/{typeTotalCount}, {f?.Identifier}";
                }
            }

            MainManagerModel.Instance.StatusText = "Done";
        }



        #endregion

        #region Implementation of IPresenterParent

        public override void OnClosed()
        {
            base.OnClosed();
            DataPresenter.IsTerminated = true;
        }

        protected override void OnDispose()
        {
            DataPresenter.IsTerminated = true;
        }


        public void ReloadData(DataPresenterModel model)
        {
            if (model == DataPresenter)
            {
                if (SelectedRelation == null)
                {
                    DataPresenter.FeatureData = new List<object>();
                    DataPresenter.UpdateFeatureDataFiltered();
                    return;
                }
                DataPresenter.FeatureData = new List<object>(SelectedRelation.Items);
                //DataPresenter.UpdateFeatureDataFiltered();
            }
            else
            {
                if (SelectedRelation2 == null)
                {
                    DataPresenter2.FeatureData = new List<object>();
                    DataPresenter2.UpdateFeatureDataFiltered();
                    return;
                }
                DataPresenter2.FeatureData = new List<object>(SelectedRelation2.Items);
                //DataPresenter2.UpdateFeatureDataFiltered();
            }


        }

        public DateTime AiracDate { get; set; }

        #endregion

        #region Selection

        private ReadonlyFeatureWrapper SelectedWrapper()
        {
            var selected = DataPresenter.SelectedFeature;

            if (selected is ReadonlyFeatureWrapper)
            {
                return selected as ReadonlyFeatureWrapper;
            }

            return null;
        }

        private string _selectedCellColumnHeader;
        private RelayCommand _openSourceCommand;
        private RelayCommand _viewXmlCommand;

        public string SelectedCellColumnHeader
        {
            get => _selectedCellColumnHeader;
            set
            {
                if (_selectedCellColumnHeader == value) return;
                _selectedCellColumnHeader = value;
            }
        }

        #endregion

    }
}
