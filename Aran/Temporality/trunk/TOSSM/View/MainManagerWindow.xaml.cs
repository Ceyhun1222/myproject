using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using MvvmCore;
using TOSSM.Common;
using TOSSM.Properties;
using TOSSM.Util;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Pane.Base;
using TOSSM.ViewModel.Tool;
using TOSSM.ViewModel.Tool.PropertyPrecision.Editor;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace TOSSM.View
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow
    {
        private const string ConfigPath = @".\Layout.config";
        public static IntPtr WindowHandle { get; private set; }

        public MainManagerWindow()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                Top = Settings.Default.Top;
                Left = Settings.Default.Left;
                Height = Settings.Default.Height;
                Width = Settings.Default.Width;
                WindowState = Settings.Default.IsMaximized ? WindowState.Maximized : WindowState.Normal;
                // Adding Hook to process second instance requests
                WindowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
                HwndSource.FromHwnd(WindowHandle)?.AddHook(new HwndSourceHook(HandleMessages));
            };

            Closing += (a, b) =>
            {
                if (WindowState == WindowState.Maximized)
                {
                    Settings.Default.Top = RestoreBounds.Top;
                    Settings.Default.Left = RestoreBounds.Left;
                    Settings.Default.Height = RestoreBounds.Height;
                    Settings.Default.Width = RestoreBounds.Width;
                    Settings.Default.IsMaximized = true;
                }
                else
                {
                    Settings.Default.Top = Top;
                    Settings.Default.Left = Left;
                    Settings.Default.Height = Height;
                    Settings.Default.Width = Width;
                    Settings.Default.IsMaximized = false;
                }
            };

            DataContext = MainManagerModel.Instance;

            MainManagerModel.Instance.OnLoaded(this);
        }

        // Handle Parameters from TOSSM EntryPoint
        internal static void HandleParameter(string[] args)
        {
            // Processing args here for second instances
            // For first instance args processing in the MainManagerModel.InitUrlHandler()
            if (args.Length > 0)
            {
                string argString = String.Join("|", args);
                if (argString.ToLowerInvariant().StartsWith("eaip"))
                  MainManagerModel.Instance.ProcessAIPUrl(argString);
            }
        }
        

        private static IntPtr HandleMessages(IntPtr handle, int message, IntPtr wParameter, IntPtr lParameter, ref Boolean handled)
        {
            try
            {
                var data = UnsafeNative.GetMessage(message, lParameter);

                if (data != null)
                {
                    UnsafeNative.ActivateMainWindow();
                    if (data != "ActivateWindow")
                    {
                        var args = data.Split(' ');
                        HandleParameter(args);
                        handled = true;
                    }
                }

                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                return IntPtr.Zero;
            }
        }

        #region LoadLayoutCommand
        RelayCommand _loadLayoutCommand;
        public RelayCommand LoadLayoutCommand => _loadLayoutCommand ?? (_loadLayoutCommand = new RelayCommand(OnLoadLayout, CanLoadLayout));

        public bool CanLoadLayout(object parameter)
        {
            return false;
            return File.Exists(ConfigPath);
        }

        public void OnLoadLayout(object parameter)
        {
            MainManagerModel.Instance.Tools.Clear();

            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            //Here I've implemented the LayoutSerializationCallback just to show
            // a way to feed layout desarialization with content loaded at runtime
            //Actually I could in this case let AvalonDock to attach the contents
            //from current layout using the content ids
            //LayoutSerializationCallback should anyway be handled to attach contents
            //not currently loaded
            layoutSerializer.LayoutSerializationCallback += (s, e) =>
                                                                {
                                                                    //if (e.Model.ContentId == FeatureDependencyManagerToolViewModel.ToolContentId)
                                                                    //{
                                                                    //    e.Content = MainManagerModel.Instance.FeatureDependencyManagerToolViewModel;
                                                                    //    MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    //}

                                                                    if (e.Model.ContentId == DataSourceTemplateManagerViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.DataSourceTemplateManagerViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == LogViewerToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.LogViewerToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == ExportToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.ExportToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == BusinessRulesManagerToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.BusinessRulesManagerToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == PrecisionSubEditorViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.PrecisionEditorToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == RelationFinderToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.RelationFinderToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == FeatureSelectorToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.FeatureSelectorToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == MyAccountToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.MyAccountToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == FeaturePresenterToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.FeaturePresenterToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == ImportToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.ImportToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == AIMSLToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.AIMSLToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == SlotMergeViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.SlotMergeViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }


                                                                    //if (e.Model.ContentId == InformerToolViewModel.ToolContentId)
                                                                    //{
                                                                    //    e.Content = MainManagerModel.Instance.InformerToolViewModel;
                                                                    //MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    //}


                                                                    if (e.Model.ContentId == SlotSelectorToolViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.SlotSelectorToolViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }

                                                                    if (e.Model.ContentId == NotamPresenterViewModel.ToolContentId)
                                                                    {
                                                                        e.Content = MainManagerModel.Instance.NotamPresenterViewModel;
                                                                        MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                    }
                                                                    if (e.Model.ContentId ==
                                                                        UserManagerToolViewModel.ToolContentId)
                                                                    {
                                                                        if ((CurrentDataContext.CurrentUser.RoleFlag &
                                                                             (int)UserRole.SuperAdmin) != 0)
                                                                        {
                                                                            e.Content =
                                                                                MainManagerModel.Instance
                                                                                    .UserManagerToolViewModel;
                                                                            MainManagerModel.Instance.LoadTool((ToolViewModel)e.Content);
                                                                        }
                                                                    }



                                                                    //else if (!string.IsNullOrWhiteSpace(e.Model.ContentId) &&
                                                                    //    File.Exists(e.Model.ContentId))
                                                                    //    e.Content = Workspace.This.Open(e.Model.ContentId);
                                                                };
            layoutSerializer.Deserialize(ConfigPath);
        }

        #endregion

        #region SaveLayoutCommand

        RelayCommand _saveLayoutCommand;
        public RelayCommand SaveLayoutCommand => _saveLayoutCommand ?? (_saveLayoutCommand = new RelayCommand(OnSaveLayout, CanSaveLayout));

        public bool CanSaveLayout(object parameter)
        {
            return true;
        }

        public void OnSaveLayout(object parameter)
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.Serialize(ConfigPath);
        }

        #endregion



        private void DockManagerDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            var model = e.Document.Content as DocViewModel;
            if (model != null)
            {
                //ask to save changes
                if (model.IsDirty)
                {
                    if (MessageBoxHelper.Show("Are you sure you want to close pane and discard all pending changes?",
                        "Some changes were not saved", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                        == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }

                }
            }
        }

        private void DockManagerDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            var model = e.Document.Content;
            if (!(model is DocViewModel)) return;

            (model as DocViewModel).IsClosed = true;


            var mainModel = DataContext as MainManagerModel;
            mainModel?.Documents.Remove(model as DocViewModel);
        }


        private void ProgressBar_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var before = Process.GetCurrentProcess().VirtualMemorySize64;
            MainManagerModel.Instance.StatusText = "Cleaning memory...";
            MemoryUtil.CompactLoh();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();
            var after = Process.GetCurrentProcess().VirtualMemorySize64;
            if (after > before) before = after;
            var fried = (before - after) >> 20;
            MainManagerModel.Instance.StatusText = fried < 1 ? "Done" : "Done, " + fried + " mb fried";
        }
    }
}
