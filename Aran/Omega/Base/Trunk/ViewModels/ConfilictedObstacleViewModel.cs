using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.AranEnvironment.Symbols;
using Aran.Omega.Models;
using Aran.PANDA.Constants;
using DoddleReport;
using DoddleReport.Writers;

namespace Aran.Omega.ViewModels
{
    class ConfilictedObstacleViewModel:ViewModel
    {
        private Models.IgnoredObstaclePair _selectedConfObstacle;
        private int _obs2Index;
        private readonly FillSymbol _polygonFillSymbol;
        private readonly FillSymbol _polygonFillSymbol2;
        private int _obs1Index;
        private OLSViewModel _omegaMainViewModel;

        public ConfilictedObstacleViewModel(OLSViewModel omegaMainViewModel, List<Models.IgnoredObstaclePair> pairObstacleList)
        {
            ConfilictedObstacleList = pairObstacleList;
            _omegaMainViewModel = omegaMainViewModel;
            _polygonFillSymbol = new FillSymbol
            {
                Color = 242424,
                Outline = new LineSymbol(eLineStyle.slsDash,
                   Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 13),
                Style = eFillStyle.sfsNull
            };

            _polygonFillSymbol2 = new FillSymbol
            {
                Color = 142424,
                Outline = new LineSymbol(eLineStyle.slsDash,
                   Aran.PANDA.Common.ARANFunctions.RGB(0, 255, 0), 13),
                Style = eFillStyle.sfsNull
            };
            ApplyCommand = new RelayCommand(new Action<object>(Apply));

            IgnoredObstacleList = new HashSet<ObstacleReport>();
        }

        private void Apply(object obj)
        {
            var orderConfObstacleList = ConfilictedObstacleList.OrderBy(conf => conf.Name);
            foreach (var confilictedObstacle in orderConfObstacleList)
            {
                if (!confilictedObstacle.Checked)
                {
                    if (!IgnoredObstacleList.Contains(confilictedObstacle.Obst1))
                        IgnoredObstacleList.Add(confilictedObstacle.Obst1);
                }
                if (!confilictedObstacle.CheckedConf)
                {
                    if (confilictedObstacle.Obst2!=null && !IgnoredObstacleList.Contains(confilictedObstacle.Obst2))
                        IgnoredObstacleList.Add(confilictedObstacle.Obst2);
                }
            }
            Close();
            SaveReport();
        }

        public RelayCommand ApplyCommand { get; set; }

        public List<Models.IgnoredObstaclePair> ConfilictedObstacleList { get; set; }

        

        public Models.IgnoredObstaclePair SelectedConfObstacle
        {
            get { return _selectedConfObstacle; }
            set
            {
                _selectedConfObstacle = value;
                Clear();
                if (_selectedConfObstacle == null) return;

                if (_selectedConfObstacle.Obst1?.BufferPrj!=null)
                    _obs1Index =GlobalParams.UI.DrawMultiPolygon(_selectedConfObstacle.Obst1.BufferPrj as Aran.Geometries.MultiPolygon, _polygonFillSymbol);

                if (_selectedConfObstacle.Obst2?.BufferPrj != null)
                    _obs2Index = GlobalParams.UI.DrawMultiPolygon(_selectedConfObstacle.Obst2.BufferPrj as Aran.Geometries.MultiPolygon, _polygonFillSymbol2);

                
            }
        }

        public void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_obs1Index);
            GlobalParams.UI.SafeDeleteGraphic(_obs2Index);
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Clear();
        }

        private void SaveReport()
        {
            try
            {
                var orderConfObstacleList = ConfilictedObstacleList.OrderBy(conf => conf.Name);
                var report = new DoddleReport.Report(orderConfObstacleList.ToReportSource());
                //report.RenderingRow += new EventHandler<ReportRowEventArgs>(report_RenderingRow);

                report.TextFields.Title = "Omega Confilicted Obstacle Report";
                report.TextFields.SubTitle = "";
                report.TextFields.Footer = report.TextFields.Footer = "Report Count :" + ConfilictedObstacleList.Count +
                    "<br/><br/><b>Copyright 2016 &copy; R.I.S.K Company</b>";

                string rwyDesignators = _omegaMainViewModel.RwyClassList[0].Name;

                var category = "";
                if (_omegaMainViewModel.SelectedCategory != null)
                    category = _omegaMainViewModel.SelectedCategory.Name;

                report.TextFields.Header = string.Format(@"
                Report Generated: {0}
                Airport/Heliport: {1}
                RWY: {2}
                RWY direction: {3}
                RWY classification: {4}
                RWY category: {5}
                RWY code number: {6}
                Measurement units:: {7}
                ", DateTime.Now, GlobalParams.Database.AirportHeliport.Name, rwyDesignators,
                    _omegaMainViewModel.SelectedRwyDirection.Name, _omegaMainViewModel.SelectedClassification.Name,
                    category, _omegaMainViewModel.RwyClassList[0].CodeNumber, InitOmega.HeightConverter.Unit == "m" ? "M" : "Foot"
                    );

                // Customize the data fields
                report.DataFields["Obst1"].Hidden = true;
                report.DataFields["Obst2"].Hidden = true;
                report.DataFields["Surface"].Hidden = true;
                report.DataFields["SurfaceConf"].Hidden = true;

                report.DataFields["Checked"].HeaderText = "Added";
                report.DataFields["CheckedConf"].HeaderText = "Added";
                report.DataFields["ConfObsName"].HeaderText = "Confilict with";
                report.DataFields["HAccConf"].HeaderText = "HACC (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["VAccConf"].HeaderText = "VACC (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["SurfaceConf"].HeaderText = "Surface";

                report.DataFields["HAcc"].HeaderText = "HACC (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["VAcc"].HeaderText = "VACC (" + InitOmega.HeightConverter.Unit + ")";
                
                
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Confilicted Obstacle Report"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Confilicted Obstacle Report";
                dlg.Filter = "Html documents|*.htm" +
                             "|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xls";
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
                    MessageBox.Show("The document was saved successfully!", "Omega", MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured while saving document!", "Omega", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public int ReportCount => ConfilictedObstacleList.Count;

        public HashSet<ObstacleReport> IgnoredObstacleList { get; }


    }
}
