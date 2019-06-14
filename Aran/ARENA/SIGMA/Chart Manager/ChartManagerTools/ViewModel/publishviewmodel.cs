using System;
using System.Collections.Generic;
using System.Linq;
using ChartManager;
using ChartManager.ViewModel;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SigmaChart;
using System.IO;
using System.Threading.Tasks;
using ArenaStatic;
using ChartManager.Logging;
using ESRI.ArcGIS.ArcMapUI;
using AiracUtil;

namespace ChartManagerTools.ViewModel
{
    public class PublishViewModel : ViewModelBase
    {
        private string _organization;
        private string _name;
        private string _airport;
        private string _rwyDirs;
        private bool _isLoading;
        private readonly IApplication _application;
        private readonly ViewModelLocator _viewModelLocator;
        private readonly string _foldername;
        private readonly bool _setLock;
        private readonly List<string> _referenceIdList;
        private readonly ILogger _logger;
        private RelayCommand _cancelCommand;
        private RelayCommand _applyCommand;
        private bool _isNameReadOnly;
        private List<AiracModel> _airacList;
        private AiracModel _selectedCycle;
        private DateTime _publicationDate;
        private string _routeLevel;
        private chartInfo _chartInfo;
        private const int airacCycleCount = 25;
        private string _note;
        private bool _isEffectiveDateEnabled;

        public PublishViewModel(IApplication application, ViewModelLocator viewModelLocator, string foldername,
            bool setLock, List<string> referenceIdList)
        {
            _application = application;
            _viewModelLocator = viewModelLocator;
            _foldername = foldername;
            _setLock = setLock;
            _referenceIdList = referenceIdList;
            _logger = LogManager.GetLogger(this);
        }

        public EventHandler Close;
        

        public void SetChartInfo(chartInfo ci)
        {
            _chartInfo = ci;
            Name = ci.chartName;
            var found = AiracList.FirstOrDefault(cycle => cycle.Index == AiracUtil.AiracUtil.GetAiracCycleByDate(ci.efectiveDate));
            _selectedCycle = found ?? AiracList[airacCycleCount];
            if (ci.publicationDate == default(DateTime))
                PublicationDate = DateTime.Now;
            else
                PublicationDate = ci.publicationDate;

            Organization = ci.organization;
            Airport = ci.ADHP;
            RwyDirs = String.Join(",", ci.RunwayDirectionsList);
            IsNameReadOnly = EsriExtensionData.IsInitialized;
            _routeLevel = ci.RouteLevel;
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(() =>
                {
                    Close?.Invoke(null, null);
                }));
            }
        }

        public RelayCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    IsLoading = true;
                    try
                    {
                        string sourceFileName = "", previewFileName = "";
                        bool decreased = false;
                        await Task.Factory.StartNew(() =>
                        {
                            _logger.InfoWithMemberName($"{nameof(EsriUtils.StoreChartInfo)} is calling");
                            _chartInfo.chartName = Name;
                            _chartInfo.efectiveDate = _selectedCycle.DateTime;
                            _chartInfo.publicationDate = PublicationDate;
                            EsriUtils.StoreChartInfo(_chartInfo,((IFeatureWorkspace) SigmaDataCash.environmentWorkspaceEdit));
                            if (!EsriExtensionData.IsInitialized)
                                EsriExtensionData.Initialize(_selectedCycle.DateTime);
                            else if (EsriExtensionData.EffectiveDate != _selectedCycle.DateTime)
                            {
                                var oldDate = EsriExtensionData.EffectiveDate;
                                EsriExtensionData.EffectiveDate = _selectedCycle.DateTime;
                                _logger.InfoWithMemberName($"{nameof(_viewModelLocator.GetLatestChartAsync)} is calling");
                                var latestChart = _viewModelLocator.GetLatestChartAsync();

                                if (latestChart.Result != null)
                                {
                                    EsriExtensionData.EffectiveDate = oldDate;
                                    _viewModelLocator.ShowMessage(
                                            "Server already contains published versions of this chart on this date.\rPlease, change date or get the latest version from server and then publish.");                                        
                                    return;
                                }
                                EsriExtensionData.ChartVersion = 0;
                            }

                            EsriExtensionData.ChartVersion++;
                            // Temporary set UpdateId to zero (0) for not saving updateId as ArcObject extension while publishing
                            var updateId = EsriExtensionData.UpdateId;
                            var hasUpdate = EsriExtensionData.HasUpdate;
                            if (hasUpdate)
                            {
                                EsriExtensionData.UpdateId = default(int);
                            }
                            _application.SaveDocument();
                            // Revert UpdateId                            
                            if (hasUpdate)
                                EsriExtensionData.UpdateId = updateId;
                            // End of temporary setting UpdateId

                            decreased = false;
                            if (EsriExtensionData.ChartVersion != 1)
                            {
                                EsriExtensionData.ChartVersion--;
                                decreased = true;
                            }
                        });

                        await Task.Factory.StartNew(() =>
                        {
                            previewFileName = Path.Combine(_foldername, @"ContentImage.jpg");
                            _logger.InfoWithMemberName(
                                $"{nameof(GetSourceFileNames)}(Folder:{_foldername},SourceFile:{sourceFileName},PreviewFile:{previewFileName}) is calling");
                            GetSourceFileNames(_foldername, _application, previewFileName, out sourceFileName);
                        });
                        _logger.InfoWithMemberName(
                            $"{nameof(_viewModelLocator.PublishAsync)}(Name:{Name},Preview:{previewFileName},Source:{sourceFileName},Organization:{Organization},Airport:{Airport},RwyDir:{RwyDirs},SetLock:{_setLock}) is calling");
                        await _viewModelLocator.PublishAsync(Name, PublicationDate, previewFileName, sourceFileName,
                            Organization, Airport,RwyDirs, Note, _referenceIdList, _setLock);

                        _viewModelLocator.ShowMessage("Published successfuly");

                        if (decreased)
                            EsriExtensionData.ChartVersion++;
                    }
                    catch (Exception ex)
                    {
                        if (EsriExtensionData.ChartVersion != 1)
                            EsriExtensionData.Clear();
                        else
                            EsriExtensionData.ChartVersion--;
                        _application.SaveDocument();
                        ErrorHandler.Handle(this, ex, _viewModelLocator.GetNotifyService());
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    Close?.Invoke(null, null);
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        #region Properties

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string Note
        {
            get => _note;
            set => Set(ref _note, value);
        }

        public bool IsNameReadOnly
        {
            get => _isNameReadOnly;
            set => Set(ref _isNameReadOnly, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public bool IsEffectiveDateEnabled
        {
            get => _isEffectiveDateEnabled;
            set => Set(ref _isEffectiveDateEnabled, value);
        }

        public string Organization
        {
            get => _organization;
            set => Set(ref _organization, value);
        }

        public string Airport
        {
            get => _airport;
            set => Set(ref _airport, value);
        }

        public string RwyDirs
        {
            get => _rwyDirs;
            set => Set(ref _rwyDirs, value);
        }

        public List<AiracModel> AiracList
        {
            get
            {
                return _airacList ?? (_airacList = AiracUtil.AiracUtil.GetAiracList(airacCycleCount).ToList());
            }
        }

        public AiracModel SelectedCycle
        {
            get => _selectedCycle;
            set => Set(ref _selectedCycle, value);
        }

        public DateTime PublicationDate
        {
            get => _publicationDate;
            set => Set(ref _publicationDate, value);
        }
        #endregion

        private void GetSourceFileNames(string foldername, IApplication application, string previewFileName,
out string sourceFileName)
        {
            EsriUtils.CreateJPEG_FromActiveView(
                ((IMxDocument)application.Document).ActiveView, previewFileName);
            sourceFileName = ArenaStaticProc.GetZippedChartPath(foldername);
        }
    }
}