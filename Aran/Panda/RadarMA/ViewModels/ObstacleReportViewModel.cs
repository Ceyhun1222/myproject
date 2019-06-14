using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Geometries.SpatialReferences;
using Aran.Panda.RadarMA.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using DoddleReport;
using DoddleReport.Writers;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ViewModels
{
    public class ObstacleReportViewModel : NotifyableBase
    {
        public ObstacleReportViewModel(List<ObstacleReport> obstacleReports)
        {
            Reports =new List<ObstacleReport>(
                        obstacleReports.OrderByDescending(obs=>obs.Elevation));
        }

        public List<ObstacleReport> Reports { get; set; }

        public RelayCommand SaveCommand=>new RelayCommand((obj) =>
        {
            try
            {
                var sortedReports = Reports.OrderByDescending(obs => obs.Elevation);

                var report = new DoddleReport.Report(sortedReports.ToReportSource());

                report.TextFields.Title = "Radar Minimum Altitude sector Report";
                report.TextFields.SubTitle = "";
                report.TextFields.Footer = report.TextFields.Footer = "Copyright 2018 &copy; R.I.S.K Company";


                // Customize the data fields
                report.DataFields["GeoPrj"].Hidden = true;
                report.DataFields["Geo"].Hidden = true;

                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Sector report"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Omega Report";
                dlg.Filter = "Html documents|*.htm" +
                             "|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xlsx";
                // Show save file dialog box
                dlg.FilterIndex = 3;
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    System.IO.Stream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);
                    if (dlg.FilterIndex == 1)
                    {
                        var writer = new HtmlReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    else if (dlg.FilterIndex == 2)
                    {
                        var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    else if (dlg.FilterIndex == 3)
                    {
                        var writer = new DoddleReport.OpenXml.ExcelReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    MessageBox.Show("The document was saved successfully!", "Radar Minimum altitude",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while saving document!", "Omega", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        });

        private ObstacleReport _selectedObstacle;
        private int _ptPrjHandle;

        public ObstacleReport SelectedObstacle
        {
            get { return _selectedObstacle; }
            set
            {
                _selectedObstacle = value;
                Draw();
            }
        }

        private void Draw()
        {
            Clear();
            if (_selectedObstacle == null)
                return;

            if (_selectedObstacle.GeoPrj is IPoint ptPrj)
            {
                _ptPrjHandle = GlobalParams.UI.DrawPointWithText(ptPrj, _selectedObstacle.Name, 14, 255);
            }
        }

        public void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_ptPrjHandle);
        }

    }
}
