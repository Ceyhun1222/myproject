using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Aran.Omega.TypeB.ViewModels;
using Aran.Panda.Constants;

namespace Aran.Omega.TypeB.View
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private ReportViewModel viewModel;
        public Report()
        {
            InitializeComponent();
            
        }

        public Report(TypeBViewModel omegaMainViewModel)
            : this()
        {
            viewModel = new ReportViewModel(omegaMainViewModel);
            this.DataContext = viewModel;
            Closing += viewModel.OnWindowClosing;
            btnSave.Click += viewModel.SaveReport;
            btnSaveAll.Click += viewModel.SaveAll;
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
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
                            g.Columns[4].Header = "X (" + InitOmega.DistanceConverter.Unit + " )";
                            g.Columns[5].Header = "Y (" + InitOmega.DistanceConverter.Unit + " )";
                            g.Columns[6].Header = "Elevation (" + InitOmega.HeightConverter.Unit + " )";
                            g.Columns[7].Header = "Penetrate (" + InitOmega.HeightConverter.Unit + " )";
                            g.Columns[8].Header = "Equation (" + InitOmega.HeightConverter.Unit + " )";

                            var selectedSurfaceType = viewModel.SelectedSurface.SurfaceType;
                            if (selectedSurfaceType == SurfaceType.InnerHorizontal ||
                                selectedSurfaceType == SurfaceType.OuterHorizontal)
                            {
                                g.Columns[5].Visibility = System.Windows.Visibility.Collapsed;
                                g.Columns[4].Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else if (selectedSurfaceType == SurfaceType.CONICAL)
                            {
                                g.Columns[4].Header = "Distance ("+InitOmega.DistanceConverter.Unit+")";
                                //g.Columns[4].Header = "Azimuth";
                                g.Columns[4].Visibility = System.Windows.Visibility.Visible;
                                g.Columns[5].Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else
                            {
                                g.Columns[5].Visibility = System.Windows.Visibility.Visible;
                                g.Columns[4].Visibility = System.Windows.Visibility.Visible;
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
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Close();
            }));
        }
    }
}
