using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Aran.Omega.Models;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows;
using DoddleReport;
using DoddleReport.Writers;
using System.Windows.Input;
using Aran.PANDA.Constants;
using Aran.Geometries.SpatialReferences;
using Aran.Omega.Enums;

namespace Aran.Omega.ViewModels
{
    public class ReportViewModel:ViewModel
    {
        #region :>Fields
        private OLSViewModel _omegaMainViewModel;
        private DrawingSurface _selectedSurface;
        private string _obstacleName;

        #endregion

        #region :>Ctor
        public ReportViewModel(OLSViewModel omegaMainViewModel)
        {
            _omegaMainViewModel = omegaMainViewModel;
            SurfaceList = omegaMainViewModel.AvailableSurfaceList;
            Report = new List<ObstacleReport>();
            _isPenetrated = false;
        }

        #endregion

        #region :>Property
        public List<ReportItem> ReportItemList { get; private set; }
        public ObservableCollection<DrawingSurface> SurfaceList { get; private set; }
        public IList<ObstacleReport> Report { get; set; }

        public string PenetrateHeader => "Penetrate (" + InitOmega.HeightConverter.Unit + " )";
        public string ElevationHeader => "Elevation (" + InitOmega.HeightConverter.Unit + " )";
        public string EquationHeader => "Equation (" + InitOmega.HeightConverter.Unit + " )";

        public Cursor CurrCursor { get; set; }

        public DrawingSurface SelectedSurface
        {
            get { return _selectedSurface; }
            set
            {
                CurrCursor = Cursors.Wait;
                _selectedSurface?.ClearSelectedObstacle();

                _selectedSurface = value;
                if (_selectedSurface?.SurfaceBase != null)
                    Report = _selectedSurface.SurfaceBase.FilteredReport;

                NotifyPropertyChanged("ReportCount");
                NotifyPropertyChanged("Report");

                CurrCursor = Cursors.Arrow;
            }
        }

        public string Caption { get; set; }

        public string ReportCount
        {
            get
            {
                var reportCount = "Obstacle Count: ";
                if (SelectedSurface?.SurfaceBase?.FilteredReport != null)
                    return reportCount + SelectedSurface.SurfaceBase.FilteredReport.Count.ToString();
                return reportCount + "0";
            }
        }

        public static bool ShowOnlyPenetrated { get; private set; }
        private bool _isPenetrated;

        public bool IsPenetrated
        {
            get { return _isPenetrated; }
            set
            {
                _isPenetrated = value;
                ShowOnlyPenetrated = _isPenetrated;
                SelectedSurface.SurfaceBase.IsPenetratedAction();
                NotifyPropertyChanged("ReportCount");
            }
        }

        #endregion

        #region :>Methods

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            _selectedSurface?.ClearSelectedObstacle();
        }

        public void SaveReport(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSurface.SurfaceBase.FilteredReport.Count == 0)
                {
                    MessageBox.Show("There are not any obstruction");
                    return;
                }

                var report = new DoddleReport.Report(SelectedSurface.SurfaceBase.FilteredReport.OrderBy(item=>item.Name).ToReportSource());
                report.RenderingRow += report_RenderingRow;

                int penetratedObstacleCount = SelectedSurface.SurfaceBase.FilteredReport
                                                        .Where(rep => rep.Penetrate > 0)
                                                        .GroupBy(rep => rep.Name)
                                                        .Count();

                int obsCount = SelectedSurface.SurfaceBase.FilteredReport
                                                        .GroupBy(rep => rep.Name)
                                                        .Count();

                report.TextFields.Title = "Omega Obstacle Penetration Report" + Environment.NewLine +
                                          SelectedSurface.ViewCaption;
                report.TextFields.SubTitle = "";
                report.TextFields.Footer = report.TextFields.Footer = "Penetrated Obstacle Count :" + penetratedObstacleCount+Environment.NewLine+"Obstacle Count : "+ obsCount
                    + Environment.NewLine+Environment.NewLine+" Copyright 2018 &copy; R.I.S.K Company";

                string rwyDesignators = _omegaMainViewModel.RwyClassList[0].Name;

                string category = "";
                if (_omegaMainViewModel.SelectedCategory != null)
                    category = _omegaMainViewModel.SelectedCategory.Name;

              var centralMeridian =GlobalParams.SpatialRefOperation.SpRefPrj.ParamList
                  .FirstOrDefault(param => param.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
                  ?.Value;

                report.TextFields.Header = $@"
                Report Generated: {DateTime.Now:dd MMMM yyyy}
                Airport/Heliport: {GlobalParams.Database.AirportHeliport.Name}
                RWY: {rwyDesignators}
                RWY direction: {_omegaMainViewModel.SelectedRwyDirection.Name}
                RWY classification: {_omegaMainViewModel.SelectedClassification.Name}
                RWY category: {category}
                RWY code number: {_omegaMainViewModel.RwyClassList[0].CodeNumber}
                Measurement units: {(InitOmega.HeightConverter.Unit == "m" ? "M" : "Foot")}
                Central Meridian : {centralMeridian}
                ";

                // Customize the data fields
                report.DataFields["GeomPrj"].Hidden = true;
                report.DataFields["SurfaceElevation"].Hidden = true;
                report.DataFields["Obstacle"].Hidden = true;
                report.DataFields["SelectedObstacle"].Hidden = true;
                report.DataFields["SurfaceType"].Hidden = true;
                report.DataFields["ExactVertexGeom"].Hidden = true;
                report.DataFields["CloseCommand"].Hidden = true;
                report.DataFields["Distance"].Hidden = true;
                report.DataFields["BufferPrj"].Hidden = true;
                report.DataFields["IntersectGeom"].Hidden = true;
                report.DataFields["Height"].Hidden = true;
                report.DataFields["Lat"].Hidden = true;
                report.DataFields["Long"].Hidden = true;
                report.DataFields["Part"].Hidden = true;

                report.DataFields["Elevation"].HeaderText = "Elevation ("+InitOmega.HeightConverter.Unit+")";
                report.DataFields["Penetrate"].HeaderText = "Penetrate (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["Plane"].HeaderText = "Equation (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["VsType"].HeaderText = "Type";
                report.DataFields["VerticalAccuracy"].HeaderText= "Vertical Accuracy (m)";
                report.DataFields["HorizontalAccuracy"].HeaderText = "Horizontal Accuracy (m)";

                if (SelectedSurface.SurfaceType == SurfaceType.InnerHorizontal ||
                    SelectedSurface.SurfaceType == SurfaceType.OuterHorizontal)
                {
                    report.DataFields["X"].Hidden = true;
                    report.DataFields["Y"].Hidden = true;
                }
                else if (SelectedSurface.SurfaceType == SurfaceType.CONICAL)
                {
                    report.DataFields["Y"].Hidden = true;
                    report.DataFields["X"].HeaderText = "Distance (" + InitOmega.DistanceConverter.Unit + ")";
                }
                else
                {
                    report.DataFields["X"].HeaderText = "X (" + InitOmega.DistanceConverter.Unit + ")";
                    report.DataFields["Y"].HeaderText = "Y (" + InitOmega.DistanceConverter.Unit + ")";
                }
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = SelectedSurface.ViewCaption; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Omega Report";
                dlg.Filter = "Html documents|*.htm" +
                             "|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xlsx";
                // Show save file dialog box
                dlg.FilterIndex = 3;
                bool? toSave = dlg.ShowDialog();

                // Process save file dialog box results
                if (toSave == true)
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
                    MessageBox.Show("The document was saved successfully!", "Omega", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                GlobalParams.Logger.Error(ex);
                MessageBox.Show("Error occured while saving document!", "Omega", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void SaveAll(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportItemList();
               // MessageBox.Show(ReportItemList.Count.ToString());
                var report = new DoddleReport.Report(ReportItemList.ToReportSource());
                report.RenderingRow += reportAll_RenderingRow;

                report.TextFields.Title = "Omega Obstacle Penetration Report";
                report.TextFields.SubTitle = "";

                foreach (var surface in SurfaceList)
                {
                    var surfObsCount = surface.SurfaceBase.GetReport
                        .Where(rep => rep.Penetrate > 0)
                        .GroupBy(rep => rep.Name)
                        .Count();
                    //surface.SurfaceBase.GetReport.Count(obs => obs.Penetrate > 0);
                    report.TextFields.Footer += surface.ViewCaption + " Obstacles : " + surfObsCount + Environment.NewLine;
                }

                int allObsCount = ReportItemList
                    .GroupBy(rep => rep.Name)
                    .Count();

                report.TextFields.Footer += "Obstacle Count :"+ allObsCount + Environment.NewLine+Environment.NewLine+
                                                "Copyright 2018 &copy; R.I.S.K Company";
                string rwyDesignators = _omegaMainViewModel.RwyClassList[0].Name;

                string category = "";
                if (_omegaMainViewModel.SelectedCategory != null)
                    category = _omegaMainViewModel.SelectedCategory.Name;

                string elevUnit = InitOmega.HeightConverter.Unit == "m" ? "M" : "Foot";

                var centralMeridian = GlobalParams.SpatialRefOperation.SpRefPrj.ParamList
                    .FirstOrDefault(param => param.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
                    ?.Value;

                report.TextFields.Header = $@"
                Report Generated: {DateTime.Now:dd MMMM yyyy}
                Airport/Heliport: {GlobalParams.Database.AirportHeliport.Name}
                RWY: {rwyDesignators}
                RWY direction: {_omegaMainViewModel.SelectedRwyDirection.Name}
                RWY classification: {_omegaMainViewModel.SelectedClassification.Name}
                RWY category: {category}
                RWY code number: {_omegaMainViewModel.RwyClassList[0].CodeNumber}
                Measurement units: {elevUnit}
                Inner horizontal surface - elevation datum: { Common.ConvertHeight(_omegaMainViewModel.SelectedElevationDatum.Height,RoundType.ToNearest)}
                X\Y:{$"Penatration \\ Surface Elevation"}
                 Central Meridian:{centralMeridian}"
                ;


                // Customize the data fields
                report.DataFields["Id"].Hidden = true;
                report.DataFields["Identifier"].Hidden = true;

                var dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "All Report",
                    DefaultExt = ".text",
                    Title = "Save Omega Report",
                    Filter = "Html documents|*.htm" +
                             "|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xlsx",
                    FilterIndex = 3
                };
                // Default file name
                // Default file extension

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result != true) return;
              
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
                MessageBox.Show("The document was saved successfully!","Omega",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured while saving document!","Omega",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void CreateReportItemList()
        {
            int i = 0;
            try
            {
                var allSurfaceReport = new List<ObstacleReport>();
                foreach (var surface in SurfaceList)
                    allSurfaceReport.AddRange(surface.SurfaceBase.GetReport);

                allSurfaceReport =
                    allSurfaceReport.Where(obs => obs.Penetrate > 0).OrderBy(obs => obs.Name).ToList();

                ReportItemList = new List<ReportItem>();
             
                foreach (var obsReport in allSurfaceReport)
                {
                    bool isHave = false;

                    ReportItem reportItem;
                    var items = ReportItemList.Where(item => item.Identifier.Equals(obsReport.Obstacle.Identifier) && item.Designator==obsReport.Designator).ToList();

                    if (items.Count > 0)
                    {
                        isHave = true;
                        reportItem = items[0];
                    }
                    else
                    {
                        reportItem = new ReportItem
                        {
                            Id = obsReport.Id,
                            Identifier = obsReport.Obstacle.Identifier,
                            Name = obsReport.Name,
                            Designator = obsReport.Designator ?? obsReport.Name,
                            ObstacleElevation = obsReport.Elevation.ToString(CultureInfo.InvariantCulture)
                        };
                        //+InitOmega.HeightConverter.Unit;
                    }
                    string penetration = obsReport.Penetrate.ToString(CultureInfo.InvariantCulture) + @"\" + Common.ConvertHeight(obsReport.SurfaceElevation,RoundType.ToNearest);
                    
                    switch (obsReport.SurfaceType)
                    {
                        case SurfaceType.InnerHorizontal:
                            reportItem.InnerHorizontal = penetration;
                            break;
                        case SurfaceType.CONICAL:
                            reportItem.Conical = penetration;
                            break;
                        case SurfaceType.OuterHorizontal:
                            reportItem.OuterHorizontal = penetration;
                            break;
                        case SurfaceType.InnerApproach:
                            reportItem.InnerApproach = penetration;
                            break;
                        case SurfaceType.Approach:
                            reportItem.Approach = penetration;
                            break;
                        case SurfaceType.Transitional:
                            reportItem.Transitional = penetration;
                            break;
                        case SurfaceType.InnerTransitional:
                            reportItem.InnerTransitional = penetration;
                            break;
                        case SurfaceType.BalkedLanding:
                            reportItem.BalkedLanding = penetration;
                            break;
                        case SurfaceType.TakeOffClimb:
                            reportItem.TakeOffClimb = penetration;
                            break;
                        case SurfaceType.Strip:
                            reportItem.Strip = penetration;
                            break;
                        case SurfaceType.Area2A:
                            reportItem.Area2A = penetration;
                            break;
                        case SurfaceType.TakeOffFlihtPathArea:
                            reportItem.TakeOffFlightPathArea = penetration;
                            break;
                    }
                    if (!isHave)
                        ReportItemList.Add(reportItem);
                    i++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + "  " + i.ToString());
            }
        }

        private void reportAll_RenderingRow(object sender, ReportRowEventArgs e)
        {
            if (e.Row.RowType == ReportRowType.DataRow)
            {
                var reportItem = e.Row.DataItem as ReportItem;
                if (reportItem != null)
                {
                    int i = 0;
                    while (i >= 0)
                    {
                        try
                        {
                            e.Row.Fields[i].DataStyle.ForeColor = System.Drawing.Color.Red;
                            i++;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void report_RenderingRow(object sender, ReportRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;
            var penetrate = (double)e.Row["Penetrate"];
            if (penetrate > 0)
            {
                var color = Color.Red;
                if (e.Row.Fields.Contains("Frangible"))
                {
                    var frang = e.Row["Frangible"].ToString();
                    if (frang=="Yes")
                        color = Color.Blue;
                }

                foreach (var field in e.Row.Fields)
                    field.DataStyle.ForeColor = color;
            }
            
        }

        #endregion
    }
}
