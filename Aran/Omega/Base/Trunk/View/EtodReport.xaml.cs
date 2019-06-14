using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Aran.Omega.ViewModels;
using Aran.PANDA.Constants;
using MahApps.Metro.Controls;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for EtodReport.xaml
    /// </summary>
    public partial class EtodReport : MetroWindow
    {
        private EtodReportViewModel viewModel;

        public EtodReport()
        {
            InitializeComponent();
        }

        public EtodReport(EtodViewModel etodViewModel)
            : this()
        {
            viewModel = new EtodReportViewModel(etodViewModel);
            this.DataContext = viewModel;
            Closing += viewModel.OnWindowClosing;
            btnSave.Click += viewModel.SaveReport;
            //btnSaveValidationReport.Click += viewModel.SaveValidationReport;
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
                            var xColumn = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("X"));
                            if (xColumn != null)
                            {
                                xColumn.Header = "X (" + InitOmega.DistanceConverter.Unit + " )";
                                xColumn.Visibility = Visibility.Visible;
                            }

                            var yColumn = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("Y"));
                            if (yColumn != null)
                            {
                                yColumn.Header = "Y (" + InitOmega.DistanceConverter.Unit + " )";
                                yColumn.Visibility = Visibility.Visible;
                            }

                            var column = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("HorizontalAccuracy"));
                            if ( column!= null)
                                column.Header = "HAcc (" + InitOmega.HeightConverter.Unit + " )";
                            column = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("VerticalAccuracy"));
                            if (column != null)
                                column.Header = "VAcc (" + InitOmega.HeightConverter.Unit + " )";

                            column = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("VAcc"));
                            if (column != null)
                                column.Header = "Elevation (" + InitOmega.HeightConverter.Unit + " )";

                            column = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("Height"));
                            if (column != null)
                                column.Header = "Height (" + InitOmega.HeightConverter.Unit + " )";

                            column = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("Penetrate"));
                            if (column != null)
                                column.Header = "Penetrate (" + InitOmega.HeightConverter.Unit + " )";

                            column = g.Columns.FirstOrDefault(col => col.SortMemberPath.Equals("Equation"));
                            if (column != null)
                                column.Header = "Equation (" + InitOmega.HeightConverter.Unit + " )";

                            var selectedSurfaceType = viewModel.SelectedSurface.SurfaceBase.EtodSurfaceType;
                            if (selectedSurfaceType == EtodSurfaceType.Area1 ||
                                selectedSurfaceType == EtodSurfaceType.Area2D ||
                                selectedSurfaceType == EtodSurfaceType.Area3)
                            {
                                yColumn.Visibility = System.Windows.Visibility.Collapsed;
                                xColumn.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                       // MessageBox.Show(ex.Message);
                        return;
                    }

                }), DispatcherPriority.DataBind);
               // viewModel.SelectedSurface.SurfaceBase.SearchName = "";
            }

            catch (Exception)
            {
                return;
            }
        }
    }
}
