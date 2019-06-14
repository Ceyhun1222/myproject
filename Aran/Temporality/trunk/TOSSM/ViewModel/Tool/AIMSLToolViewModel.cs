using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.MetaData;
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
using Aran.Temporality.Common.Entity;
using ZipArchive = System.IO.Compression.ZipArchive;

namespace TOSSM.ViewModel.Tool
{
    public class AIMSLToolViewModel : UpdatableToolViewModel, IPresenterParent
    {
        public static string ToolContentId = "AIMSL";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/export.png", UriKind.RelativeOrAbsolute);

        public AIMSLToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
            DataPresenter2 = new DataPresenterModel { ViewModel = this };
            SleepTime = 5000;
        //    Priority = ThreadPriority.BelowNormal;
            StartLoading();
        }

        protected override bool Enable()
        {
            return (CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.Aimsl) != 0;
        }

        protected override void LoadFunction()
        {
            if (HasAccess)
                Update();
        }

    
        private XmlViewerViewModel _xmlViewerViewModel;
        public XmlViewerViewModel XmlViewerViewModel => _xmlViewerViewModel ?? (_xmlViewerViewModel = new XmlViewerViewModel());


        public string FileName { get; set; }

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


        public RelayCommand OpenSourceCommand
        {
            get
            {
                return _openSourceCommand ?? (_openSourceCommand = new RelayCommand(
                           t2 =>
                           {

                               AObjectListConfig.IgnoreNotes = false;
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
                                           //clear all
                                           RelationsData.Clear();
                                           SourceFileName = null;
                                           FileName = null;

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
                                                   MainManagerModel.Instance.StatusText = totalCount + " features loaded...";
                                               },
                                               () =>
                                               {
                                                   MainManagerModel.Instance.StatusText = "Cleaning memory...";
                                               },
                                               (aixmFeatureList, collection) =>
                                               {
                                                   foreach (Feature feature in aixmFeatureList)
                                                   {
                                                       var xml = aixmFeatureList.Xml(feature);
                                                       AddFeatureToModels(feature, xml, list);
                                                       count++;
                                                       MainManagerModel.Instance.StatusText = "Processed " + count + " features from " + totalCount + "...";
                                                   }
                                                   collection.Clear();//clear memory
                                               });

                                           if (_aimHelper.IsOpened)
                                           {
                                               FileName = dlg.FileName;
                                               SourceFileName = SimpleName(dlg.FileName);
                                               //sort relations
                                               relationList = relationList.OrderBy(t => t.Direction).ThenBy(t => t.FeatureType.ToString()).ToList();

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
                                               FileName = null;
                                           }
                                           //ReloadDataToBeImported();

                                           MainManagerModel.Instance.StatusText = "Cleaning memory...";
                                           MemoryUtil.CompactLoh();
                                           GC.WaitForPendingFinalizers();

                                           MainManagerModel.Instance.StatusText = _aimHelper.IsOpened ? "Done" : "Failed to open";
                                       });
                               }
                           }));
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
            relatedModel.Items.Add(new ReadonlyFeatureWrapper(feature) { Xml = xml });
        }


        private void OnFeatureTypeChecked(SingleTypeRelationViewModel model)
        {
            //if (ImportFeaturesIndex == 1)
            //{
            //    ReloadDataToBeImported();
            //}
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

        //public ObservableCollection<AimslOperationViewModel> AimslOperations = new ObservableCollection<AimslOperationViewModel>();
        private MtObservableCollection<AimslOperationViewModel> _aimslOperations = new MtObservableCollection<AimslOperationViewModel>();
        public MtObservableCollection<AimslOperationViewModel> AimslOperations
        {
            get => _aimslOperations ?? (_aimslOperations = new MtObservableCollection<AimslOperationViewModel>());
            set
            {

                _aimslOperations = value;
                OnPropertyChanged("AimslOperations");
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


        private RelayCommand _uploadCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand UploadCommand
        {
            get
            {
                return _uploadCommand
                       ?? (_uploadCommand = new RelayCommand(
                           execute: t =>
                           {
                               StopLoading();
                               DataPresenter.BlockerModel.BlockForAction(Upload);
                               StartLoading();
                           },
                           canExecute: t => !DataPresenter.BlockerModel.IsBlocked && HasAccess
                                            && RelationsData.IsNotEmpty()));
            }
        }

        private void Upload()
        {
            byte[] content = GetZippedFileContent(FileName);
            var operation = CurrentDataContext.CurrentNoAixmDataService.AimslUploadAixmFile(content, SourceFileName);
            AimslOperations.Add(new AimslOperationViewModel(operation));     
            MainManagerModel.Instance.StatusText = operation.Status == "ERROR" ? $"{SourceFileName} upload failed" : $"{SourceFileName} upload succeed";
        }

        protected byte[] GetZippedFileContent(string filePath)
        {
            byte[] content;
            using (var ms = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {

                    zipArchive.CreateEntryFromFile(Path.GetFullPath(filePath), Path.GetFileName(filePath),
                        CompressionLevel.Fastest);
                }
                content = ms.ToArray();
            }

            return content;
        }

        public void Update()
        {
            var list = CurrentDataContext.CurrentNoAixmDataService.AimslGetAllAimslOperations();

            var newIds = new HashSet<int>(list.Select(t => t.Id));

            //delete missing
            var toBeDeleted = AimslOperations.Where(t => !newIds.Contains(t.Id)).ToList();
            foreach (var model in toBeDeleted)
            {
                AimslOperations.Remove(model);
            }

            //add new
            var oldIds = new HashSet<int>(AimslOperations.Select(t => t.Id));
            var newOperations = list.Where(t => !oldIds.Contains(t.Id)).ToList();
            foreach (var operation in newOperations)
            {
                AimslOperations.Add(new AimslOperationViewModel(operation));
            }

            //update current
            var currentOperations = list.Where(t => oldIds.Contains(t.Id)).ToList();
            foreach (var operation in currentOperations)
            {
                var correspondingOperation = AimslOperations.FirstOrDefault(t => t.Id == operation.Id);
                correspondingOperation?.Update(operation);
            }
            //AIMSLServiceClient.Services.AimslService.Init();
            //var service = AIMSLServiceClient.Services.AimslService.GetService(AIMSLServiceClient.Services.ServiceType.FTS);
            //foreach (var operation in AimslOperations)
            //{
            //    if (operation.Closed)
            //        continue;
            //    try
            //    {
            //        lock (operation)
            //        {
            //            if (operation.PullPoint == null)
            //            {
            //                var pullpoint = service.CreatePullPoint();
            //                if (pullpoint != null)
            //                    operation.PullPoint = pullpoint;
            //            }

            //            if (operation.Subscription == null)
            //            {
            //                var subscription = service.Subscribe(operation.PullPoint, operation.JobId);
            //                if (subscription != null)
            //                    operation.Subscription = subscription;
            //            }

            //            var messages = service.GetMessages(operation.PullPoointId);
            //            if (messages != null)
            //                foreach (var message in messages)
            //                {
            //                    if (message.Message != null)
            //                    {
            //                        operation.Status = message.Message.Status;
            //                        operation.LastChangeTime = message.Message.LastChangeTime;
            //                        operation.StartTime = message.Message.CreateTime;
            //                        if (message.Message.ProcessingMessages != null)
            //                        {
            //                            operation.AddMessages(message.Message.ProcessingMessages);
            //                        }
            //                    }
            //                }

            //            if (operation.Status == "FINISHED" || operation.Status == "FAILED")
            //            {
            //                operation.Closed = true;
            //                service.DestroyPullPoint(operation.PullPoointId);
            //            }
            //        }


            //    }
            //    catch (Exception e)
            //    {
            //        LogManager.GetLogger(typeof(AIMSLToolViewModel)).Warn(e, e.Message);
            //    }

            //}
        }

        private RelayCommand _testConnectionCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand TestConnectionCommand
        {
            get
            {
                return _testConnectionCommand
                       ?? (_testConnectionCommand = new RelayCommand(
                           execute: t =>
                           {
                               DataPresenter.BlockerModel.BlockForAction(TestConnection);
                           },
                           canExecute: t=> HasAccess));
            }
        }


        public void TestConnection()
        {
            if (CurrentDataContext.CurrentNoAixmDataService.AimslTestConnection())
            {
                MessageBoxHelper.Show("Connection is open");
            }
            else
            {
                MessageBoxHelper.Show("Connection failed");
            }
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
                if (SelectedRelation == null)
                {
                    DataPresenter.FeatureData = new List<object>();
                    DataPresenter.UpdateFeatureDataFiltered();
                    return;
                }
                DataPresenter.FeatureData = new List<object>(SelectedRelation.Items);
                //DataPresenter.UpdateFeatureDataFiltered();
        }

     

        public DateTime AiracDate { get; set; }

        #endregion

        #region Selection


        private string _selectedCellColumnHeader;
        private RelayCommand _openSourceCommand;
        private RelayCommand _viewXmlCommand;
        public bool HasAccess { get; } = (CurrentDataContext.CurrentUser.RoleFlag & (int) UserRole.Aimsl) != 0;

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

    public class AimslOperationViewModel : ViewModelBase
    {

        private AimslOperation _operation;

        public AimslOperationViewModel()
        {
            
        }

        public AimslOperationViewModel(AimslOperation operation)
        {
            Update(operation);
        }

        public void Update(AimslOperation operation)
        {
            Id = operation.Id;
            JobId = operation.JobId;
            StartTime = operation.CreationTime;
            LastChangeTime = operation.LastChangeTime;
            Status = operation.Status;
            FileName = operation.FileName;
            Description = operation.Description;
            Closed = operation.InternalStatus == AimslOperationStatusType.Closed || operation.InternalStatus == AimslOperationStatusType.Destroyed;
            PullPoint = operation.PullPoint;
            Subscription = operation.Subscription;
            Messages = operation.Messages;
        }

        public int Id { get; set; }
        public string PullPoint { get; set; }

        public string PullPoointId => PullPoint?.Split('=')[1];

        public string Subscription { get; set; }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private DateTime _lastChangeTime;
        public DateTime LastChangeTime
        {
            get => _lastChangeTime;
            set
            {
                _lastChangeTime = value;
                OnPropertyChanged("LastChangeTime");
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public string JobId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }


        private string _messages;
        public string Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        public bool Closed { get; set; } = false;
    }
}