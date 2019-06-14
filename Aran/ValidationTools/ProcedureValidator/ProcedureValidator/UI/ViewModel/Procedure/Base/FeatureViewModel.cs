using System;
using System.Windows;
using System.Windows.Interop;
using GalaSoft.MvvmLight.Command;
using PVT.Model;
using PVT.UI.View;

namespace PVT.UI.ViewModel
{
    public abstract class FeatureViewModel<T> : StateViewModel where T : Feature
    {
        public ViewModelType? Type { get; protected set; }
        private RelayCommand _exportToGDBCommand;
        private RelayCommand _screenshotViewCommand;
        private RelayCommand _reportViewCommand;

        public T Feature { get; protected set; }

        protected FeatureViewModel(MainViewModel main, StateViewModel previous) : base(main, previous)
        {
        }

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ExportToGDBCommand => _exportToGDBCommand
                                                  ?? (_exportToGDBCommand = new RelayCommand(ExportToGDB));

        public RelayCommand ScreenShotViewCommand
        {
            get
            {
                return _screenshotViewCommand
                       ?? (_screenshotViewCommand = new RelayCommand(
                           () =>
                           {
                               var screenshotView = new ScreenshotView
                               {
                                   DataContext =
                                       new ScreenshotViewModel(
                                           Engine.Environment.Current.DbProvider.GetScreenShots(Feature.Identifier),
                                           Feature)
                               };

                               new WindowInteropHelper(screenshotView) {Owner = Engine.Environment.Current.Win32Window};                        
                               screenshotView.ShowDialog();
                           }));
            }
        }

        public RelayCommand ReportViewCommand
        {
            get
            {
                return _reportViewCommand
                       ?? (_reportViewCommand = new RelayCommand(
                           () =>
                           {
                               var reportView = new ReportView {DataContext = new ReportViewModel(Feature)};
                               new WindowInteropHelper(reportView) { Owner = Engine.Environment.Current.Win32Window };

                               reportView.ShowDialog();
                           }));
            }
        }

        protected  void ExportToGDB()
        {
            var ofd = new System.Windows.Forms.FolderBrowserDialog();
            var result = ofd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var folder = ofd.SelectedPath;
                try
                {
                    BlockerModel.BlockForAction(() => Engine.Environment.Current.Utils.ExportToGDB(folder, Feature));

                }
                catch (Exception ex)
                {
                    Engine.Environment.Current.Logger.Error(ex, "ExportToGDB error");
                    MessageBox.Show("Unexpected Exception");
                }
            }
        }


        private RelayCommand _backCommand;

        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                       ?? (_backCommand = new RelayCommand(
                           () =>
                           {
                               ClearScreen();
                               Previous();
                           }));
            }
        }

        //private RelayCommand _profileGraphCommand;


        //public RelayCommand ProfileGraphCommand
        //{
        //    get
        //    {
        //        return _profileGraphCommand
        //               ?? (_profileGraphCommand = new RelayCommand(
        //                   () =>
        //                   {
        //                       var profileGraphView = new ProfileGraphView();
        //                       profileGraphView.DataContext = new ProfileGraphViewModel(Procedure);
        //                       profileGraphView.ShowDialog();
        //                   }));
        //    }
        //}

        public override bool CanNext()
        {
            return false;
        }

        public override bool CanPrevious()
        {
            return true;
        }

        protected override void next()
        {

        }

        protected override void previous()
        {

        }

        protected abstract void ClearScreen();
        

        protected override void _destroy()
        {
            ClearScreen();
        }
    }

    public enum ViewModelType {
        Procedure,
        Holding
    }
}