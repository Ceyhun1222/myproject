using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using Microsoft.Win32;
using MvvmCore;
using SnapshotStatistics.Template;

namespace SnapshotStatistics.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isFeatures = true;
        private bool _isLinks = true;
        private bool _isRootProperties = true;
        private bool _isAllProperties = true;

        public bool IsFeatures
        {
            get { return _isFeatures; }
            set
            {
                _isFeatures = value;
                OnPropertyChanged("IsFeatures");
            }
        }

        public bool IsLinks
        {
            get { return _isLinks; }
            set
            {
                _isLinks = value;
                OnPropertyChanged("IsLinks");
            }
        }

        public bool IsRootProperties
        {
            get { return _isRootProperties; }
            set
            {
                _isRootProperties = value;
                OnPropertyChanged("IsRootProperties");
            }
        }

        public bool IsAllProperties
        {
            get { return _isAllProperties; }
            set
            {
                _isAllProperties = value;
                OnPropertyChanged("IsAllProperties");
            }
        }


        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }


        private RelayCommand _setFileCommand;
        private string _filePath;
        private RelayCommand _saveCommand;

        public RelayCommand SetFileCommand
        {
            get { return _setFileCommand ?? (_setFileCommand = new RelayCommand(
                t =>
                {
                    var dialog = new OpenFileDialog
                    {
                        Multiselect = false,
                        Title = "Select AIX Message File",
                        Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        FilePath = dialog.FileName;
                    }
                })); }
        }

        public Dictionary<FeatureType, Issue> Statistics = new Dictionary<FeatureType, Issue>();
        private string _statusText;
        private BlockerModel _blockerModel;

        public BlockerModel BlockerModel
        {
            get { return _blockerModel??(_blockerModel=new BlockerModel()); }
        }

        public RelayCommand SaveCommand
        {
            get { return _saveCommand??(_saveCommand=new RelayCommand(
                t =>
                {

                    CultureInfo ci = CultureInfo.InvariantCulture;
                    Thread.CurrentThread.CurrentCulture = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;

                    var dialog = new SaveFileDialog
                    {
                        Title = "Save Statistics",
                        Filter = "HTML Files (*.htm)|*.htm|All Files (*.*)|*.*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        BlockerModel.BlockForAction(
                            () =>
                            {
                                Statistics.Clear();

                                AixmHelper aimHelper = new AixmHelper();
                                int totalCount = 0;
                                int count = 0;

                                aimHelper.Open(FilePath,
                                                          () =>
                                                          {
                                                              totalCount++;
                                                              StatusText = totalCount + " features loaded...";
                                                          },
                                                          () =>
                                                          {
                                                              StatusText = "Cleaning memory...";
                                                          },
                                    (aixmFeatureList, collection) =>
                                    {
                                        foreach (Feature feature in aixmFeatureList)
                                        {
                                            ProcessFeature(feature);
                                            count++;
                                            StatusText = "Processed " + count + " features from " + totalCount + "...";
                                        }
                                        collection.Clear(); //clear memory
                                    });

                                StatusText = "Done";
                                if (aimHelper.IsOpened)
                                {
                                    var page = new StatisticsPage
                                    {
                                        FileName = FilePath,
                                        Issues = Statistics.Values.OrderBy(t2=>t2.FeatureType.ToString()).ToList()
                                    };

                                    page.Count = Statistics.Values.Aggregate(0, (result, item) => result + item.Count);
                                    page.Links = Statistics.Values.Aggregate(0, (result, item) => result + item.Links);
                                    page.RootProperties = Statistics.Values.Aggregate(0, (result, item) => result + item.RootProperties); 

                                    var pageContent = page.TransformText();
                                    File.WriteAllText(dialog.FileName, pageContent);


                                    try
                                    {
                                        Process.Start(dialog.FileName);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show("Can not open " + dialog.FileName + " in default browser.",
                                            "Can Not Open", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                }

                            }
                            );
                    }
                },
                t => !string.IsNullOrEmpty(FilePath))); }
        }


       

        private void ProcessFeature(Feature feature)
        {
            Issue issue;
            if (!Statistics.TryGetValue(feature.FeatureType, out issue))
            {
                issue = new Issue { FeatureType = feature.FeatureType.ToString() };
                Statistics[feature.FeatureType] = issue;
            }

            issue.Count++;
            var links = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(feature, links);
            issue.Links += links.Count;
            var infos=AimMetadata.GetAimPropInfos(feature);
            if (infos!=null)
            {
                foreach (var aimPropInfo in infos)
                {
                    var reason = feature.GetNilReason(aimPropInfo.Index);
                    if (reason == null)
                    {
                        issue.RootProperties++;
                    }
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }
    }
}
