using System;
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Threading.Tasks;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using AmdbManager.ViewModel;
using AmdbManager;
using AmdbManager.Logging;
using Aerodrome.Metadata;

namespace ChartManagerTools.ViewModel
{
    
    public class PublishViewModel : ViewModelBase
    {
        private string _organization;
        private string _name;
        private string _airport;       
        private bool _isLoading;
        private readonly IApplication _application;
        private readonly ViewModelLocator _viewModelLocator;
        
        private readonly bool _setLock;
        private RelayCommand _cancelCommand;
        private RelayCommand _applyCommand;
        private bool _isNameReadOnly;       
       
        private ExtensionData _amdbInfo;       

        public PublishViewModel(IApplication application, ViewModelLocator viewModelLocator,
            bool setLock)
        {
            _application = application;
            _viewModelLocator = viewModelLocator;            
            _setLock = setLock;
        }

        public EventHandler Close;

        public void SetChartInfo(ExtensionData ed)
        {
            _amdbInfo = ed;
            Name = ed.Name;
            Organization = ed.Organization;
            Airport = ed.ADHP;

            var latestAmdb = _viewModelLocator.GetLatestAmdb();
            if (latestAmdb is null)
                IsNameReadOnly = false;
            else
                IsNameReadOnly = true; 

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
                    LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Started");
                    IsLoading = true;
                    try
                    {
                        string sourceFileName = "";
                        bool decreased = false;

                        if (!ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsInitialized)
                            ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Initialize();
                        else
                        {
                            var latestAmdb = _viewModelLocator.GetLatestAmdb();
                            if (latestAmdb is null)
                            {
                                ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion = 0;
                            }
                        }

                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion++;
                        HelperMethods.SaveAmdbProject(_application);

                        var m_filePaths = HelperMethods.GetRecentFilesAmdm();
                        sourceFileName = m_filePaths != null && m_filePaths.Length > 0 ? m_filePaths[0] : "";

                        decreased = false;
                        if (ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion != 1)
                        {
                            ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion--;
                            decreased = true;
                        }

                        await _viewModelLocator.PublishAsync(Name, sourceFileName, Organization, Airport, _setLock);

                        _viewModelLocator.ShowMessage("Published successfuly");

                        if (decreased)
                            ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.AmdbVersion++;

                        HelperMethods.SaveAmdbProject(_application, isCommitFeatures: false);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, _viewModelLocator.GetNotifyService());
                        HelperMethods.SaveAmdbProject(_application, isCommitFeatures: false);
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    Close?.Invoke(null, null);
                    LogManager.GetLogger(typeof(GlobalController)).InfoWithMemberName($"Finished");
                }));
            }
        }

        #region Properties

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
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


        #endregion

        private void GetSourceFileNames(string foldername, IApplication application, out string sourceFileName,
            out string previewFileName)
        {
            previewFileName = Path.Combine(foldername, @"\ContentImage.jpg");
            sourceFileName = ArenaStaticProc.GetZippedChartPath(foldername);
        }

    }
}