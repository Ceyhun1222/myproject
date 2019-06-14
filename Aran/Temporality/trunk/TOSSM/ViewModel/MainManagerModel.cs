using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Utilities;
using Aran.Converters;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.View;
using TOSSM.ViewModel.Document;
using TOSSM.ViewModel.Document.Editor;
using TOSSM.ViewModel.Document.Evolution;
using TOSSM.ViewModel.Document.Graph;
using TOSSM.ViewModel.Document.Relations;
using TOSSM.ViewModel.Document.Slot;
using TOSSM.ViewModel.Pane.Base;
using TOSSM.ViewModel.Tool;
using TOSSM.ViewModel.Tool.PropertyPrecision;
using VisualizerCommon.Remote;
using Aran.Temporality.Common.ArcGis.Wkt;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Logging;
using FluentNHibernate.Conventions;
using TOSSM.Util.Notams;
using Xceed.Wpf.AvalonDock.Themes;

namespace TOSSM.ViewModel
{
    public interface IMapHandler
    {
        void SelectFeatureOnMap(AimFeature feature);
        void SelectGeometryOnMap(Aran.Geometries.Geometry value);
    }

    public class MapHandler : IMapHandler
    {
        private readonly VisualizerClient _client;
        private AimFeature _featureToSelect;
        private bool _isFeatureToSelectSet;
        private Aran.Geometries.Geometry _geometryToSet;
        private bool _isGeometryToSelectSet;
        private readonly DispatcherTimer _timer;
        public MapHandler()
        {
            _client = new VisualizerClient();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            _timer.Tick += (a, b) =>
            {
                try
                {
                    if (_isGeometryToSelectSet)
                    {
                        _isGeometryToSelectSet = false;
                        Application.Current.Dispatcher.Invoke(
                           DispatcherPriority.Normal,
                           (Action)(
                               () =>
                               {
                                   DoSelectGeometryOnMap(_geometryToSet);
                               }));

                    }
                    else if (_isFeatureToSelectSet)
                    {
                        _isFeatureToSelectSet = false;
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal,
                            (Action)(
                                () =>
                                {
                                    DoSelectFeatureOnMap(_featureToSelect);
                                }));
                    }
                }
                catch (Exception exception)
                {

                }

            };
            _timer.Start();

        }

        public void SelectFeatureOnMap(AimFeature aimFeature)
        {
            _featureToSelect = aimFeature;
            _isFeatureToSelectSet = true;
        }

        public void SelectGeometryOnMap(Aran.Geometries.Geometry value)
        {
            _geometryToSet = value;
            _isGeometryToSelectSet = true;
        }

        private void DoSelectFeatureOnMap(AimFeature aimFeature)
        {
            MainManagerModel.Instance.StatusText = "Analysing selected feature geometry...";
            aimFeature.InitEsriExtension();
            var esriList =
                aimFeature.PropertyExtensions.Where(t => t is EsriPropertyExtension)
                    .Cast<EsriPropertyExtension>()
                    .ToList();

            var desc = HumanReadableConverter.ShortAimDescription(aimFeature.Feature);
            desc = string.IsNullOrEmpty(desc) ? "Selected Feature" : aimFeature.FeatureType + " " + desc;
            if (esriList.Count > 0)
            {
                MainManagerModel.Instance.StatusText = esriList.Count + " geometry components of " + desc + " are presented on map, zooming map...";
                var list = new List<GeometrySelection>();
                foreach (var item in esriList)
                {
                    var selection = new GeometrySelection
                    {
                        Name = desc,
                        Data = item.EsriData
                    };
                    list.Add(selection);
                }

                _client.SetSelection(list);

                MainManagerModel.Instance.StatusText = esriList.Count + " geometry components of " + desc + " are presented on map";
            }
            else
            {
                MainManagerModel.Instance.StatusText = desc + " does not have any geometry components";
            }
        }
        private void DoSelectGeometryOnMap(Aran.Geometries.Geometry value)
        {
            var esriGeometry = ConvertToEsriGeom.FromGeometry(value);
            _client.SetSelection(new List<GeometrySelection>
            {new GeometrySelection
            {
                Name = "Selected Geometry",
                Data = MainManagerModel.EsriToBytes(esriGeometry)
            }});
        }
    }

    internal class MainManagerModel : ViewModelBase
    {
        //private Visibility _statusBusyIndicatorVisibility = Visibility.Hidden;
        //public Visibility StatusBusyIndicatorVisibility
        //{
        //    get { return _statusBusyIndicatorVisibility; }
        //    set
        //    {
        //        _statusBusyIndicatorVisibility = value;
        //        OnPropertyChanged("StatusBusyIndicatorVisibility");
        //    }
        //}

        //private bool _isBusy;
        //public bool IsBusy
        //{
        //    get { return _isBusy; }
        //    set
        //    {
        //        _isBusy = value;
        //        StatusBusyIndicatorVisibility = IsBusy ? Visibility.Visible : Visibility.Hidden;
        //    }
        //}

        public int MemoryStatusValue
        {
            get => _memoryStatusValue;
            set
            {
                _memoryStatusValue = value;
                OnPropertyChanged("MemoryStatusValue");
            }
        }

        public string MemoryStatusText
        {
            get => _memoryStatusText;
            set
            {
                _memoryStatusText = value;
                OnPropertyChanged("MemoryStatusText");
            }
        }

        #region Instance, Ctor, initialization

        public static MainManagerModel Instance = new MainManagerModel();

        public MainManagerWindow MainManagerWindow { get; set; }


        private BlockerModel _blockerModel;

        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }

        private SecuredApplication _application;


        private bool IsClosing;


        private bool _loaded;
        [STAThread]
        public void OnLoaded(MainManagerWindow win)
        {
            if (_loaded) return;
            _loaded = true;

            InitStatusUpdater();




            //var airport = new AirportHeliport
            //{
            //    TimeSliceMetadata = new FeatureTimeSliceMetadata {DateStamp = DateTime.Now}
            //};
            //var clone=(AirportHeliport) FormatterUtil.Clone(airport);



            _application = (SecuredApplication)Application.Current;
            ConnectionProvider.OnStatusChanged = OnConnectionStatusChanged;
            ConnectionProvider.OnServerTimeChanged = OnServerTimeChanged;
            OnConnectionStatusChanged();

            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject),
                                                                 new FrameworkPropertyMetadata(Int32.MaxValue));
            ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof(DependencyObject),
                                                                 new FrameworkPropertyMetadata(true));



            MainManagerWindow = win;

            MainManagerWindow.Closing += (t, e) =>
            {
                IsClosing = true;
                CloseStatusUpdater();

                if (Documents != null)
                {
                    Documents.Clear();
                }
                //save dock layout
                MainManagerWindow.OnSaveLayout(null);
                //save metadata
                UIMetadata.Instance.Save();


                ClipboardMonitor.Stop(); // do not forget to stop

            };

            //Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
            //   (Action)(() =>
            //                {

            //                    //var thread = new Thread(CreateMapForm);
            //                    //thread.SetApartmentState(ApartmentState.STA);
            //                    //thread.Start();
            //                    //thread.Join();


            //                }));

            //set map

            Theme = (CurrentDataContext.CurrentService.GetServerType() == SystemType.Test) ? (Theme)new MetroTheme() : new VS2010Theme();
            BlockerModel.BlockForAction(
                () => Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (Action)(
                    () =>
                        {
                            if (MainManagerWindow.CanLoadLayout(null))
                            {
                                MainManagerWindow.OnLoadLayout(null);
                            }


                            InitTools();

                            Documents?.Clear();


                            StatusText = "Logged in as " + CurrentDataContext.CurrentUser.Name;
                            UpdateTitle();

                            InitUrlHandler();

                        })));


            //do load esri
            IPolygon dummy = new Polygon() as IPolygon;

            //load visualizer
#if LOCAL_SERVER
            //var visualizerName = "VisualizerEnvironment";
            //var processList = Process.GetProcessesByName(visualizerName);
            //if (processList.Length == 0)
            //{
            //    var process=Process.Start(visualizerName + ".exe");
            //}
#endif

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                (Action)(() =>
           {
               UpdaterUtil.Update();
           }
            ));

            _startMemory = Process.GetCurrentProcess().VirtualMemorySize64;




            ClipboardMonitor.Start();
            ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            LoadFromClipBoard();


        }

        void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            LoadFromClipBoard();
        }

        private DispatcherTimer _statusUpdater;
        private void CloseStatusUpdater()
        {
            _statusUpdater.Stop();
            _statusUpdater = null;
        }

        private long _startMemory;

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void ProcessUrl(string url)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                (Action)(() =>
               {
                   try
                   {
                       url = url.Split('|').First();
                       url = url.Split(':')[1];
                       while (url.StartsWith("/")) url = url.Substring(1);
                       var id = Guid.Parse(url.Split('_')[0]);

                       var date = DateTime.Now;
                       if (CurrentDataContext.CurrentUser == null) return;
                       if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return;
                       date = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;

                       try
                       {
                           date = UnixTimeStampToDateTime(Convert.ToDouble(url.Split('_')[1]));
                       }
                       catch (Exception)
                       {
                       }


                       int ft = CurrentDataContext.CurrentService.GetFeatureTypeById(id);

                       if (ft > 0)
                       {
                           MainManagerWindow.Activate();
                           View(new LightFeatureWrapper(new LightFeature { FeatureType = ft, Guid = id }), date);
                       }


                   }
                   catch (Exception)
                   {
                   }
               }));
        }

        public void ProcessAIPUrl(string url)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,(Action)(() => {
                    try
                    {
                        if(!url.ToLowerInvariant().StartsWith("eaip://") || CurrentDataContext.CurrentUser == null || CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return; 
                        var parseStrings = url.Replace("eaip://","").Split('/');
                        var action = parseStrings[0];
                        var args = parseStrings[1];
                        Dictionary<string,string> paramList = new Dictionary<string, string>();
                        foreach (var arg in args.Split('&'))
                        {
                            var param = arg.Split('=');
                            paramList.Add(param[0],param[1]);
                        }
                        var id = Guid.Parse(paramList["id"]);
                        Enum.TryParse(paramList["featureType"], out FeatureType fType);
                        var date = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
                        var feat = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId() { FeatureTypeId = (int)fType, WorkPackage = 0, Guid = id }, false, date).FirstOrDefault()?.Data.Feature;
                        
                        if (feat != null)
                        {
                            MainManagerWindow.Activate();
                            View(feat, date);
                        }

                    }
                    catch (Exception)
                    {
                    }
                }));
        }

        private void InitUrlHandler()
        {

            if (App.IsUserAdministrator())
            {
                try
                {
                    var args = App.Instance.StartupArgs;
                    if (args.Length > 0)
                    {
                        string argString = String.Join("|", args);
                        if(argString.ToLowerInvariant().StartsWith("eaip"))
                            ProcessAIPUrl(argString);
                        else
                            ProcessUrl(argString);
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        var server = new NamedPipeServerStream("TossmPipe");
                        while (true)
                        {
                            try
                            {
                                server.WaitForConnection();
                                var reader = new StreamReader(server);
                                ProcessUrl(reader.ReadLine());
                                server.Disconnect();
                            }
                            catch (Exception)
                            {

                            }
                        }
                    });
                }
                catch (Exception)
                {

                }
            }

        }

        private void InitStatusUpdater()
        {
            _statusUpdater = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _statusUpdater.Tick += (a, b) =>
            {
                Process proc = Process.GetCurrentProcess();
                const long maxVirtualMemory32 = 2147483648;//2 gb limit for 32 bits

                var shift = Math.Min(_startMemory, proc.VirtualMemorySize64);

                MemoryStatusValue = (int)(100 * ((proc.VirtualMemorySize64 - shift) >> 10) / ((maxVirtualMemory32 - shift) >> 10));
                MemoryStatusText =
                    $"Physical memory: {(proc.WorkingSet64 >> 20)} mb\nVirtual memory: {(proc.VirtualMemorySize64 >> 20)} mb\n\nDouble click to clean memory";
            };
            _statusUpdater.Start();
        }

        public void UpdateTitle()
        {
            var version = CurrentDataContext.Version;
            if (CurrentDataContext.CurrentUser == null)
            {
                if (string.IsNullOrEmpty(CurrentDataContext.StorageName))
                {
                    Title = $"TOSSM v. {version} - No user logged in, Server: {CurrentDataContext.ServiceHost}, System: {CurrentDataContext.SystemType}";
                }
                else
                {
                    Title = $"TOSSM v. {version} - Database: {CurrentDataContext.StorageName}, No user logged in, Server: {CurrentDataContext.ServiceHost}, System: {CurrentDataContext.SystemType}";
                }

                return;
            }

            var slot = CurrentDataContext.CurrentUser.ActivePrivateSlot;

            if (slot == null)
            {
                Title =
                    $"TOSSM v. {version} - Database: {CurrentDataContext.StorageName}, No Active Slot ({CurrentDataContext.CurrentUser.Name}), Server: {CurrentDataContext.ServiceHost}, System: {CurrentDataContext.SystemType}";
                return;
            }

            var cycle = AiracCycle.GetAiracCycleByStrictDate(slot.PublicSlot.EffectiveDate);
            var airacMessage = (cycle > -1) ? $"AIRAC: {cycle}, Effective date: "
                : "Custom AIRAC, Effective date: ";
            airacMessage += slot.PublicSlot.EffectiveDate.ToString("yyyy/MM/dd HH:mm UTC");

            Title =
                $"TOSSM v. {version} - Database: {CurrentDataContext.StorageName}, Active Slot: {slot.Name}, {airacMessage} ({CurrentDataContext.CurrentUser.Name}), Server: {CurrentDataContext.ServiceHost}, System: {CurrentDataContext.SystemType}";
        }


        #endregion

        #region Documents

        private PaneViewModel _activeDocument;
        public PaneViewModel ActiveDocument
        {
            get => _activeDocument;
            set
            {
                _activeDocument = value;
                OnPropertyChanged("ActiveDocument");
                ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ActiveDocumentChanged;
        private MtObservableCollection<DocViewModel> _documents;

        public MtObservableCollection<DocViewModel> Documents => _documents ?? (_documents = new MtObservableCollection<DocViewModel>());

        #endregion

        #region Tools

        private readonly List<ToolViewModel> LoadedTools = new List<ToolViewModel>();
        public void LoadTool(ToolViewModel model)
        {
            LoadedTools.Add(model);
        }

        public void InitTools()
        {
            var allTools = Tools.ToList().Union(LoadedTools).ToList();

            if (!allTools.Any(t => t is SlotSelectorToolViewModel))
            {
                Tools.Add(SlotSelectorToolViewModel);
            }

            if (!allTools.Any(t => t is FeaturePresenterToolViewModel))
            {
                Tools.Add(FeaturePresenterToolViewModel);
            }

            if (!allTools.Any(t => t is MyAccountToolViewModel))
            {
                Tools.Add(MyAccountToolViewModel);
            }

            if (!allTools.Any(t => t is PrecisionEditorToolViewModel))
            {
                Tools.Add(PrecisionEditorToolViewModel);
            }

            if (!allTools.Any(t => t is BusinessRulesManagerToolViewModel))
            {
                Tools.Add(BusinessRulesManagerToolViewModel);
            }

            if (!allTools.Any(t => t is RelationFinderToolViewModel))
            {
                Tools.Add(RelationFinderToolViewModel);
            }


            if (!allTools.Any(t => t is ImportToolViewModel))
            {
                Tools.Add(ImportToolViewModel);
            }

            if (!allTools.Any(t => t is AIMSLToolViewModel))
            {
                Tools.Add(AIMSLToolViewModel);
            }

            if (!allTools.Any(t => t is SlotMergeViewModel))
            {
                Tools.Add(SlotMergeViewModel);
            }


            if (!allTools.Any(t => t is ExportToolViewModel))
            {
                Tools.Add(ExportToolViewModel);
            }

            if (!allTools.Any(t => t is NotamPresenterViewModel))
            {
                Tools.Add(NotamPresenterViewModel);
            }

            if (!allTools.Any(t => t is DataSourceTemplateManagerViewModel))
            {
                Tools.Add(DataSourceTemplateManagerViewModel);
            }

            //admins tools
            if ((CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.SuperAdmin) != 0)
            {
                if (!allTools.Any(t => t is UserManagerToolViewModel))
                {
                    Tools.Add(UserManagerToolViewModel);
                }

                if (!allTools.Any(t => t is LogViewerToolViewModel))
                {
                    Tools.Add(LogViewerToolViewModel);
                }
            }

        }

        private ImportToolViewModel _importToolViewModel;
        public ImportToolViewModel ImportToolViewModel
        {
            get => _importToolViewModel ?? (_importToolViewModel = new ImportToolViewModel());
            set => _importToolViewModel = value;
        }

        private AIMSLToolViewModel _aimslToolViewModel;
        public AIMSLToolViewModel AIMSLToolViewModel
        {
            get => _aimslToolViewModel ?? (_aimslToolViewModel = new AIMSLToolViewModel());
            set => _aimslToolViewModel = value;
        }


        private SlotMergeViewModel _slotMergeViewModel;
        public SlotMergeViewModel SlotMergeViewModel
        {
            get => _slotMergeViewModel ?? (_slotMergeViewModel = new SlotMergeViewModel());
            set => _slotMergeViewModel = value;
        }

        private DataSourceTemplateManagerViewModel _dataSourceTemplateManagerViewModel;
        public DataSourceTemplateManagerViewModel DataSourceTemplateManagerViewModel => _dataSourceTemplateManagerViewModel ?? (_dataSourceTemplateManagerViewModel = new DataSourceTemplateManagerViewModel());


        private LogViewerToolViewModel _logViewerToolViewModel;
        public LogViewerToolViewModel LogViewerToolViewModel => _logViewerToolViewModel ?? (_logViewerToolViewModel = new LogViewerToolViewModel());

        private MtObservableCollection<ToolViewModel> _tools;
        public MtObservableCollection<ToolViewModel> Tools => _tools ?? (_tools = new MtObservableCollection<ToolViewModel>());


        private PrecisionEditorToolViewModel _precisionEditorToolViewModel;
        public PrecisionEditorToolViewModel PrecisionEditorToolViewModel => _precisionEditorToolViewModel ?? (_precisionEditorToolViewModel = new PrecisionEditorToolViewModel());

        private SlotSelectorToolViewModel _slotSelectorToolViewModel;
        public SlotSelectorToolViewModel SlotSelectorToolViewModel => _slotSelectorToolViewModel ?? (_slotSelectorToolViewModel = new SlotSelectorToolViewModel());

        private FeatureSelectorToolViewModel _featureSelectorModel;
        public FeatureSelectorToolViewModel FeatureSelectorToolViewModel
        {
            get => _featureSelectorModel ?? (_featureSelectorModel = new FeatureSelectorToolViewModel());
            set
            {
                _featureSelectorModel = value;
                _featureSelectorModel.PropertyChanged += (t, e) =>
                                                             {
                                                                 var model = t as FeatureSelectorToolViewModel;
                                                                 if (model == null) return;
                                                                 if (FeaturePresenterToolViewModel == null) return;

                                                                 if (e.PropertyName != "SelectedFeature") return;
                                                                 var s = model.SelectedFeature;
                                                                 if (s is FeatureType)
                                                                 {
                                                                     FeaturePresenterToolViewModel.DataPresenter.
                                                                         FeatureType = (FeatureType)s;
                                                                 }
                                                             };
                OnPropertyChanged("FeatureSelectorToolViewModel");
            }
        }

        private FeatureDependencyManagerToolViewModel _featureDependencyManagerToolViewModel;
        public FeatureDependencyManagerToolViewModel FeatureDependencyManagerToolViewModel => _featureDependencyManagerToolViewModel ?? (_featureDependencyManagerToolViewModel = new FeatureDependencyManagerToolViewModel());

        private BusinessRulesManagerToolViewModel _businessRulesManagerToolViewModel;
        public BusinessRulesManagerToolViewModel BusinessRulesManagerToolViewModel => _businessRulesManagerToolViewModel ?? (_businessRulesManagerToolViewModel = new BusinessRulesManagerToolViewModel());

        private RelationFinderToolViewModel _relationFinderToolViewModel;
        public RelationFinderToolViewModel RelationFinderToolViewModel => _relationFinderToolViewModel ?? (_relationFinderToolViewModel = new RelationFinderToolViewModel());

        private MyAccountToolViewModel _myAccountToolViewModel;
        public MyAccountToolViewModel MyAccountToolViewModel => _myAccountToolViewModel ?? (_myAccountToolViewModel = new MyAccountToolViewModel());

        private UserManagerToolViewModel _userManagerToolViewModel;
        public UserManagerToolViewModel UserManagerToolViewModel => _userManagerToolViewModel ?? (_userManagerToolViewModel = new UserManagerToolViewModel());


        private InformerToolViewModel _informerToolViewModel;
        public InformerToolViewModel InformerToolViewModel => _informerToolViewModel ?? (_informerToolViewModel = new InformerToolViewModel());

        private NotamPresenterViewModel _notamPresenterViewModel;
        public NotamPresenterViewModel NotamPresenterViewModel => _notamPresenterViewModel ?? (_notamPresenterViewModel = new NotamPresenterViewModel());

        private FeaturePresenterToolViewModel _featurePresenterModel;
        public FeaturePresenterToolViewModel FeaturePresenterToolViewModel
        {
            get => _featurePresenterModel ?? (_featurePresenterModel = new FeaturePresenterToolViewModel());
            set => _featurePresenterModel = value;
        }

        #endregion

        #region Document commands

        private T GetDocViewModel<T>(Guid featureId, DateTime date) where T : DocViewModel
        {
            return Documents.OfType<T>().FirstOrDefault(
                    docModel => docModel.FeatureIdentifier == featureId &&
                    docModel.AiracDate == date);
        }

        private FeatureEditorDocViewModel GetDocViewModel(Guid featureId, DateTime date, int workpackage)
        {
            return Documents.OfType<FeatureEditorDocViewModel>().FirstOrDefault(
                    docModel => docModel.FeatureIdentifier == featureId &&
                    docModel.AiracDate == date && docModel.Workpackage == workpackage);
        }

        private NotamFeatureEditorViewModel GetNotamDocViewModel(int notam, int workpackage)
        {
            return Documents.OfType<NotamFeatureEditorViewModel>().FirstOrDefault(
                docModel => docModel.Notam.Id == notam && docModel.Workpackage == workpackage);
        }

        private T GetDocViewModel<T>(int id) where T : DocViewModel
        {
            return Documents.OfType<T>().FirstOrDefault(docModel => docModel.Id == id);
        }

        private SlotContentDocViewModel GetSlotDocViewModel(int slotId)
        {
            return Documents.OfType<SlotContentDocViewModel>().FirstOrDefault(
                   docModel => docModel.EditedSlot.Id == slotId);
        }

        private static Guid GetIdentifier(object obj)
        {
            if (obj is Guid) return (Guid)obj;
            if (obj is Feature) return (obj as Feature).Identifier;
            if (obj is ReadonlyFeatureWrapper) return (obj as ReadonlyFeatureWrapper).Feature.Identifier;
            if (obj is LightFeatureWrapper) return (obj as LightFeatureWrapper).LightFeature.Guid;

            throw new Exception("bad object to operate");
        }

        private static FeatureType GetFeatureType(object obj)
        {
            if (obj is Feature) return (obj as Feature).FeatureType;
            if (obj is ReadonlyFeatureWrapper) return (obj as ReadonlyFeatureWrapper).Feature.FeatureType;
            if (obj is LightFeatureWrapper) return (FeatureType)(obj as LightFeatureWrapper).LightFeature.FeatureType;
            throw new Exception("bad object to operate");
        }

        private static TimeSlice GetTimeSlice(object obj)
        {
            if (obj is Feature) return (obj as Feature).TimeSlice;
            if (obj is ReadonlyFeatureWrapper) return (obj as ReadonlyFeatureWrapper).Feature.Feature.TimeSlice;
            if (obj is LightFeatureWrapper) return (obj as LightFeatureWrapper).ReadonlyFeatureWrapper.Feature.Feature.TimeSlice;

            throw new Exception("bad object to operate");
        }

        private static TimePeriod GetLifeTime(object obj)
        {
            if (obj is Feature) return (obj as Feature).TimeSlice.FeatureLifetime;
            if (obj is ReadonlyFeatureWrapper) return (obj as ReadonlyFeatureWrapper).Feature.Feature.TimeSlice.FeatureLifetime;
            if (obj is LightFeatureWrapper) return (obj as LightFeatureWrapper).ReadonlyFeatureWrapper.Feature.Feature.TimeSlice.FeatureLifetime;
            throw new Exception("bad object to operate");
        }

        public void ShowValidationReport(int privateSlotId)
        {
            var model = GetDocViewModel<SlotValidationReportViewModel>(privateSlotId);
            if (model == null)
            {
                model = new SlotValidationReportViewModel(privateSlotId);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void Edit(object wrapper, DateTime date, int workpackage = 0)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);

            var model = GetDocViewModel(id, date, workpackage);
            if (model == null)
            {
                model = new FeatureEditorDocViewModel(featureType, id, date)
                {
                    IsNotNullFilter = true,
                    AiracDate = date,
                    DocType = DocumentType.EditFeatureState
                };
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void Notam(Notam notam, int workpackage = 0)
        {

            var model = GetNotamDocViewModel(notam.Id, workpackage);
            if (model == null)
            {
                try
                {
                    if (notam.Type == (int)NotamType.C)
                    {
                        Notam notamToCancel = NotamUtils.GetNotamToCancel(notam);
                        if (notamToCancel == null)
                        {
                            MessageBoxHelper.Show(
                                "Sorry, there was error while requesting data. Canceling notam is not exist.",
                                "Error while requesting data", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        else
                            model = NotamManager.GetOperation(notam, notamToCancel).GetViewModel(notam);
                    }
                    else
                        model = NotamManager.GetOperation(notam).GetViewModel(notam);
                }
                catch (NotImplementedException ex)
                {
                    model = null;
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    model = null;
                    MessageBox.Show("Unexpected execption");
                    LogManager.GetLogger(typeof(NotamFeatureEditorViewModel)).Error(ex, ex.Message);
                }
                
                if (model == null)
                    return;
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void View(object wrapper, DateTime date, int workpackage = 0)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);
            var timeslice = GetTimeSlice(wrapper);

            View(id, featureType, timeslice, date, workpackage);
        }

        public void View(Feature feature, DateTime date, int workpackage = 0)
        {
            var id = feature.Identifier;
            var featureType = feature.FeatureType;
            var timeslice = feature.TimeSlice;

            View(id, featureType, timeslice, date, workpackage);
        }


        private void View(Guid id, FeatureType featureType, TimeSlice timeslice, DateTime date, int workpackage = 0)
        {
            var model = GetDocViewModel(id, date, workpackage);
            if (model == null)
            {
                model = new FeatureEditorDocViewModel(featureType, id, date, workpackage)
                {
                    AiracDate = date,
                    IsNotNullFilter = true,
                    DocType = DocumentType.ViewFeatureState,
                    Interpretation = timeslice.Interpretation == TimeSliceInterpretationType.TEMPDELTA
                        ? Interpretation.Snapshot
                        : Interpretation.BaseLine,
                    StartDateTime = timeslice.ValidTime.BeginPosition,
                    EndDateTime = timeslice.ValidTime.EndPosition
                };
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void New(object selectedFeatureType, DateTime date)
        {
            if (!(selectedFeatureType is FeatureType)) return;

            var featureType = (FeatureType)selectedFeatureType;
            var id = Guid.NewGuid();


            var model = new FeatureEditorDocViewModel(featureType, id, date)
            {
                AiracDate = date,
                DocType = DocumentType.CreateNewFeature,
            };

            Documents.Add(model);

            ActiveDocument = model;

            model.Load();
        }

        public void Paste(DateTime date)
        {
            if (BufferFeatureIdentifier == Guid.Empty) return;
            if (BufferFeatureType == null) return;


            var featureType = (FeatureType)BufferFeatureType;
            var id = Guid.NewGuid();


            var model = new FeatureEditorDocViewModel(featureType, id, date)
            {
                AiracDate = date,
                DocType = DocumentType.CreateNewFeature,
                BufferFeatureIdentifier = BufferFeatureIdentifier,
                BufferDateTime = BufferDateTime
            };

            Documents.Add(model);

            ActiveDocument = model;

            model.Load();
        }

        public bool CanPaste()
        {
            return BufferFeatureIdentifier != Guid.Empty && BufferFeatureType != null;
        }



        public void RelationChart(object wrapper, DateTime date)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);

            var model = GetDocViewModel<RelationExplorerDocViewModel>(id, date);
            if (model == null)
            {
                model = new RelationExplorerDocViewModel(featureType, id, date);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void Relation(object wrapper, DateTime date)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);

            var model = GetDocViewModel<RelationsDocViewModel>(id, date);
            if (model == null)
            {
                model = new RelationsDocViewModel(featureType, id, date);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void ShowSlotContent(int privateSlotId)
        {
            var model = GetSlotDocViewModel(privateSlotId);
            if (model == null)
            {
                model = new SlotContentDocViewModel(privateSlotId);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void ShowFeatureDependencyReport(PrivateSlot privateSlot)
        {
            var model = GetDocViewModel<FeatureDependencyReportDocViewModel>(privateSlot.Id);

            if (model == null)
            {
                model = new FeatureDependencyReportDocViewModel(privateSlot);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void Evolution(object wrapper, DateTime date)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);

            var model = GetDocViewModel<EvolutionDocViewModel>(id, date);
            if (model == null)
            {
                model = new EvolutionDocViewModel(featureType, id, date);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void Delete(object wrapper, DateTime date)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);

            var model = GetDocViewModel<EraserDocViewModel>(id, date);
            if (model == null)
            {
                model = new EraserDocViewModel(featureType, id, date);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public void GeoIntersection(object wrapper, DateTime date)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);

            var model = GetDocViewModel<GeoIntersectionDocViewModel>(id, date);
            if (model == null)
            {
                model = new GeoIntersectionDocViewModel(featureType, id, date);
                Documents.Add(model);
                ActiveDocument = model;
                model.Load();
            }
            else
            {
                ActiveDocument = model;
            }
        }

        public FeatureType? BufferFeatureType { get; set; }
        public Guid BufferFeatureIdentifier { get; set; }
        public TimePeriod BufferLifeTime { get; set; }
        public DateTime BufferDateTime { get; set; }

        public void Copy(object wrapper, DateTime date)
        {
            BufferFeatureIdentifier = GetIdentifier(wrapper);
            BufferFeatureType = GetFeatureType(wrapper);
            BufferLifeTime = GetLifeTime(wrapper);
            BufferDateTime = date;

            StatusText = GetDescriptionForBufferedFeature(wrapper);
        }

        private string GetDescriptionForBufferedFeature(object obj)
        {
            if (obj is Feature) return (obj as Feature).FeatureType + " " +
                HumanReadableConverter.ShortAimDescription(obj as Feature) + " copied in buffer";

            if (obj is ReadonlyFeatureWrapper) return (obj as ReadonlyFeatureWrapper).Feature.Feature.FeatureType + " " +
                HumanReadableConverter.ShortAimDescription((obj as ReadonlyFeatureWrapper).Feature.Feature) + " copied in buffer";

            throw new Exception("bad object to operate");
        }

        public void OnMap(object wrapper, DateTime date)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);
        }

        #endregion

        #region Database command


        public void OnFeatureChanged(Feature feature)
        {
            //update Documents


            //update Tools

            //update feature presenter
            if (FeaturePresenterToolViewModel.DataPresenter.FeatureType != null)
            {
                var ft = (FeatureType)FeaturePresenterToolViewModel.DataPresenter.FeatureType;
                if (ft == feature.FeatureType)
                {
                    FeaturePresenterToolViewModel.ReloadData();
                }
            }

            if (SlotMergeViewModel.IsLoaded)
                SlotMergeViewModel.Reload(feature);

        }

        #endregion

        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }


        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private Theme _theme;
        public Theme Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }



        private string _connectionStatusToolTip;
        public string ConnectionStatusToolTip
        {
            get => _connectionStatusToolTip;
            set
            {
                _connectionStatusToolTip = value;
                OnPropertyChanged("ConnectionStatusToolTip");
            }
        }

        private string _connectionStatusImage = "status_off";
        public string ConnectionStatusImage
        {
            get => _connectionStatusImage;
            set
            {
                _connectionStatusImage = value;
                OnPropertyChanged("ConnectionStatusImage");
            }
        }

        private void OnConnectionStatusChanged()
        {
            ConnectionStatusImage = ConnectionProvider.ServerIsOffline ? "status_off" : "status_on";
            ConnectionStatusToolTip = ConnectionProvider.ServerIsOffline ? "Server is down" : "Server is up";
        }

        private bool _isInOnServerTimeChanged;
        private void OnServerTimeChanged()
        {
            if (_isInOnServerTimeChanged) return;
            _isInOnServerTimeChanged = true;

            ConnectionStatusToolTip = ConnectionProvider.ServerIsOffline ? "Server is down" : "Server is up, server time is " + ConnectionProvider.ServerTimeString;

            if (Math.Abs(ConnectionProvider.ServerTime.Subtract(DateTime.Now).TotalMinutes) > 5)
            {
                MessageBoxHelper.Show(
                    "In order to work properly date and time on local PC should not differ from server time by more than 5 minutes, please set correct time.",
                    "Date and Time is not correct", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            _isInOnServerTimeChanged = false;
        }

        public void OnSaveBusinessRules()
        {
            foreach (var docViewModel in Documents.OfType<SlotValidationReportViewModel>())
            {
                docViewModel.ReloadData();
            }

            BusinessRulesManagerToolViewModel.ReloadData();
        }

        public void Close(DocViewModel docViewModel)
        {

            if (Documents.Contains(docViewModel))
                Documents.Remove(docViewModel);

        }

        public void CloseAllDocuments()
        {
            if (Documents.Any(t => t.IsDirty))
            {
                if (MessageBoxHelper.Show("In order to proceed all currently active panes should be closed. Some of panes contain unsaved changes. Are you sure you want to close all panes and discard all pending changes?",
                       "Some changes were not saved", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                       == MessageBoxResult.Yes)
                {
                    Documents.Clear();
                }
            }
            else
            {
                Documents.Clear();
            }
        }

        #region Clipboard


        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        public void LoadFromClipBoard()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            try
            {
                bool esriData = false;
                if (System.Windows.Clipboard.ContainsData("Esri IGeometry"))
                {
                    esriData = true;
                    var data = System.Windows.Clipboard.GetData("Esri IGeometry") as string;
                    var geometry = data?.ToGeometry();
                    if (geometry != null)
                    {
                        Clipboard.Clear();
                        Clipboard.Add(geometry);
                        SetClipboardStatus();
                    }
                }

                if (System.Windows.Clipboard.ContainsData("Esri IGeometry List"))
                {
                    esriData = true;
                    var data = System.Windows.Clipboard.GetData("Esri IGeometry List") as List<string>;
                    if (data != null)
                    {
                        Clipboard.Clear();
                        Clipboard.AddRange(data.Select(t => t.ToGeometry()).ToList());
                        SetClipboardStatus();
                    }


                }

                if (esriData)
                    System.Windows.Clipboard.Clear();
            }
            catch (Exception exception)
            {
                LogManager.GetLogger(typeof(MainManagerModel))
                    .Error(exception, "Error wahile reading from clipdoard");
            }
        }

        private void SetClipboardStatus()
        {
            if (Clipboard.ClipboardEsriGeometry != null)
                switch (Clipboard.ClipboardEsriGeometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        StatusText = "Point geometry copied in clipboard";
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        StatusText = "Polyline geometry copied in clipboard";
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        StatusText = "Polygon geometry copied in clipboard";
                        break;
                }
        }


        private IMapHandler _onMapViewerToolViewModel;
        private string _memoryStatusText;
        private int _memoryStatusValue;
        private ExportToolViewModel _exportToolViewModel;


        public IMapHandler OnMapViewerToolViewModel => _onMapViewerToolViewModel ?? (_onMapViewerToolViewModel = new MapHandler());

        public ExportToolViewModel ExportToolViewModel => _exportToolViewModel ?? (_exportToolViewModel = new ExportToolViewModel());

        public ClipboardWrapper Clipboard { get; } = new ClipboardWrapper();

        #endregion

        #region esri serialization
        private const string PropertyName = "esri";

        public static byte[] EsriToBytes(object esriGeometry)
        {
            var memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;

            IPropertySet propertySet = new PropertySetClass();

            var perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(PropertyName, esriGeometry);
            perStr.Save(objStr, 0);

            object obj;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out obj);

            return (byte[])obj;
        }

        public static object EsriFromBytes(byte[] bytes)
        {
            try
            {
                var memBlobStream = new MemoryBlobStream();

                var varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                var anObjectStream = new ObjectStreamClass { Stream = memBlobStream };

                IPropertySet aPropSet = new PropertySetClass();

                var aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                return aPropSet.GetProperty(PropertyName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public void AddToExport(object wrapper)
        {
            var id = GetIdentifier(wrapper);
            var featureType = GetFeatureType(wrapper);
            ExportToolViewModel.AddFeatureToExport(featureType, id);
        }
    }

    internal class ClipboardWrapper : IEnumerable<Aran.Geometries.Geometry>
    {
        private readonly List<Aran.Geometries.Geometry> _clipboardAranGeometries = new List<Aran.Geometries.Geometry>();

        public Aran.Geometries.Geometry ClipboardAranGeometry => _clipboardAranGeometries.IsEmpty() ? null : _clipboardAranGeometries.First();
        public IGeometry ClipboardEsriGeometry { get; private set; }

        public void Clear()
        {
            _clipboardAranGeometries.Clear();
        }

        public void Add(IGeometry geometry)
        {
            if (_clipboardAranGeometries.IsEmpty())
                ClipboardEsriGeometry = geometry;
            _clipboardAranGeometries.Add(ConvertFromEsriGeom.ToGeometry(geometry, true));
        }

        public void AddRange(IList<IGeometry> geometries)
        {
            if (_clipboardAranGeometries.IsEmpty() && geometries.IsNotEmpty())
                ClipboardEsriGeometry = geometries.First();
            _clipboardAranGeometries.AddRange(geometries.Select(geometry => ConvertFromEsriGeom.ToGeometry(geometry, true)));
        }

        #region IEnumerable

        public IEnumerator<Aran.Geometries.Geometry> GetEnumerator()
        {
            return _clipboardAranGeometries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_clipboardAranGeometries).GetEnumerator();
        }

        public int Count => _clipboardAranGeometries.Count;

        #endregion

    }
}
