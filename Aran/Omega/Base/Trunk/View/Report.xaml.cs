using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Aran.Omega.Models;
using Aran.Omega.ViewModels;
using Aran.PANDA.Constants;
using MahApps.Metro.Controls;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : MetroWindow
    {
        private ReportViewModel viewModel;
        private DrawingSurface _selectedDrawingSurface;
        private OLSViewModel _olsViewModel;

        public Report()
        {
            InitializeComponent();
            
        }

        public Report(OLSViewModel omegaMainViewModel)
            : this()
        {

            _olsViewModel = omegaMainViewModel;
            viewModel = new ReportViewModel(omegaMainViewModel);
            this.DataContext = viewModel;
            Closing += viewModel.OnWindowClosing;
            btnSave.Click += viewModel.SaveReport;
            btnSaveAll.Click += viewModel.SaveAll;
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);

            GlobalParams.Dispatcher = this.Dispatcher;

           
#if Riga
            Models.FilterLatv filterLatv = new FilterLatv(_olsViewModel.AvailableSurfaceList);
            var confWindow = new ConfilictedObstacleView(filterLatv.IgnoredObstacleList.Values.ToList());
            var helperConf = new WindowInteropHelper(confWindow);
            ElementHost.EnableModelessKeyboardInterop(confWindow);
            helperConf.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
            confWindow.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            confWindow.Show();
            confWindow.Closed += new EventHandler(ConfWindowClosed);
#endif


        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var tmpSelectedSurface = ReportTabControl.SelectedContent as DrawingSurface;
                if (tmpSelectedSurface == null || (_selectedDrawingSurface != null && _selectedDrawingSurface.SurfaceType == tmpSelectedSurface.SurfaceType))
                    return;
                _selectedDrawingSurface = tmpSelectedSurface;
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    try
                    {

                        this.ApplyTemplate();
                        var cp = ReportTabControl.Template.FindName("PART_SelectedContentHost", ReportTabControl) as ContentPresenter;
                        if (cp != null)
                        {
                            this.ApplyTemplate();
                            var g = ReportTabControl.ContentTemplate.FindName("reportGrid", cp) as DataGrid;
                            g.Columns[8].Header = "X (" + InitOmega.DistanceConverter.Unit + " )";
                            g.Columns[9].Header = "Y (" + InitOmega.DistanceConverter.Unit + " )";
                            g.Columns[10].Header = "Elevation (" + InitOmega.HeightConverter.Unit + " )";
                            g.Columns[11].Header = "Penetrate (" + InitOmega.HeightConverter.Unit + " )";
                            g.Columns[12].Header = "Equation (" + InitOmega.HeightConverter.Unit + " )";

                            //g.Columns[4].Visibility = Visibility.Collapsed;
                            
                            var selectedSurfaceType = viewModel.SelectedSurface.SurfaceType;
                            if (selectedSurfaceType == SurfaceType.InnerHorizontal ||
                                selectedSurfaceType == SurfaceType.OuterHorizontal)
                            {
                                g.Columns[9].Visibility = System.Windows.Visibility.Collapsed;
                                g.Columns[8].Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else if (selectedSurfaceType == SurfaceType.CONICAL)
                            {
                                g.Columns[8].Header = "Distance ("+InitOmega.DistanceConverter.Unit+")";
                                //g.Columns[4].Header = "Azimuth";
                                g.Columns[8].Visibility = System.Windows.Visibility.Visible;
                                g.Columns[9].Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else
                            {
                                g.Columns[9].Visibility = System.Windows.Visibility.Visible;
                                g.Columns[8].Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                        
                    }

                    catch (Exception)
                    {
                        return;
                    }

                }), DispatcherPriority.DataBind);
                viewModel.SelectedSurface.SurfaceBase.SearchName = "";
            }

            catch (Exception)
            {
                return;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Close();
                }));
            }
            catch (Exception)
            {
               // MessageBox.Show("unseccess");
            }
        }

        private void ConfWindowClosed(object sender, EventArgs e)
        {
            //var confWindow = sender as ConfilictedObstacleView;
            //if (confWindow == null) return;

            //foreach (var obsReport in confWindow.IgnoredObstacleList)
            //{
            //   bool result =_olsViewModel.AvailableSurfaceList.First(surface => surface.SurfaceType == obsReport.SurfaceType)
            //        .SurfaceBase.Report.Remove(obsReport);
            //}
            this.Show();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
