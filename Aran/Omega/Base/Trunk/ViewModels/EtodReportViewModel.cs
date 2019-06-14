using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Omega.Models;
using Aran.PANDA.Constants;
using DoddleReport;
using DoddleReport.Writers;

namespace Aran.Omega.ViewModels
{
    public class EtodReportViewModel:ViewModel
    {
        #region :>Fields
        private EtodViewModel _etodViewModel;
        private DrawingSurface _selectedSurface;
        private string _obstacleName;

        #endregion

        #region :>Ctor
        public EtodReportViewModel(EtodViewModel etodViewModel)
        {
            _etodViewModel = etodViewModel;
            SurfaceList = etodViewModel.AvailableSurfaceList;
            DrawingSurface area1Surface = new DrawingSurface("Area 1");
            area1Surface.SurfaceBase = new Area1();
            area1Surface.RwyDirClass = etodViewModel.SelectedRwyDirection;
            if (SurfaceList.FirstOrDefault(surface=>surface.SurfaceBase.EtodSurfaceType== EtodSurfaceType.Area1)==null)
                SurfaceList.Insert(0, area1Surface);

            SelectedSurface = SurfaceList[0];
            Report = new List<ObstacleReport>();
            _isPenetrated = false;
        }


        public static bool ShowOnlyPenetrated { get; private set; }
        #endregion

        #region :>Property
        public List<ReportItem> ReportItemList { get; private set; }
        public ObservableCollection<DrawingSurface> SurfaceList { get; private set; }
        public IList<ObstacleReport> Report { get; set; }

        public string PenetrateHeader { get { return "Penetrate (" + InitOmega.HeightConverter.Unit + " )"; } }
        public string ElevationHeader { get { return "Elevation (" + InitOmega.HeightConverter.Unit + " )"; } }
        public string EquationHeader { get { return "Equation (" + InitOmega.HeightConverter.Unit + " )"; } }

        public System.Windows.Input.Cursor CurrCursor { get; set; }

        public DrawingSurface SelectedSurface
        {
            get { return _selectedSurface; }
            set
            {
                CurrCursor = Cursors.Wait;
                if (_selectedSurface != null)
                    _selectedSurface.ClearSelectedObstacle();

                _selectedSurface = value;
                if (_selectedSurface!=null && _selectedSurface.SurfaceBase != null)
                    Report = _selectedSurface.SurfaceBase.FilteredReport;
              
                NotifyPropertyChanged("ReportCount");

                CurrCursor = Cursors.Arrow;
            }
        }

        public string Caption { get; set; }

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

        public string ReportCount {
            get
            {
                var reportCount = "Obstacle Count: ";
                if (SelectedSurface?.SurfaceBase?.FilteredReport != null)
                    return reportCount + SelectedSurface.SurfaceBase.FilteredReport.Count.ToString();
                return reportCount + "0";
            }
        }


        #endregion

        #region :>Methods

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (_selectedSurface!=null)
                _selectedSurface.ClearSelectedObstacle();
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
                var report = new DoddleReport.Report(SelectedSurface.SurfaceBase.GetReport.OrderBy(item=>item.Name).ToReportSource());
                report.RenderingRow += report_RenderingRow;

                report.TextFields.Title = "Etod Obstacle Penetration Report" + Environment.NewLine +
                                          SelectedSurface.ViewCaption;

                int penetratedObstacleCount = SelectedSurface.SurfaceBase.GetReport.Count(rep => rep.Penetrate > 0);

                report.TextFields.SubTitle = "";
                report.TextFields.Footer = report.TextFields.Footer = "Penetrate Obstacle Count :" + penetratedObstacleCount + Environment.NewLine +
                   "Obstacle Count : " + SelectedSurface.SurfaceBase.FilteredReport.Count + Environment.NewLine+ "Copyright 2018 &copy; R.I.S.K Company";
                string rwyDesignators = _etodViewModel.RwyClassList[0].Name;

                string category = "";
                if (_etodViewModel.SelectedCategory != null)
                    category = _etodViewModel.SelectedCategory.Name;

                report.TextFields.Header = string.Format(@"
                Report Generated: {0}
                Aiport / Heliport: {1}
                RWY: {2}
                RWY direction: {3}
                RWY classification: {4}
                RWY category: {5}
                RWY code number: {6}
                Measurement units:: {7}
                Elevation Datum: {8}
                ", DateTime.Now, GlobalParams.Database.AirportHeliport.Name, rwyDesignators,
                    _etodViewModel.SelectedRwyDirection.Name, _etodViewModel.SelectedClassification.Name,
                    category, _etodViewModel.RwyClassList[0].CodeNumber, InitOmega.HeightConverter.Unit == "m" ? "M" : "Foot",
                    Common.ConvertHeight(_etodViewModel.SelectedElevationDatum.Height,Enums.RoundType.ToNearest));

                // Customize the data fields
                report.DataFields["Id"].Hidden = true;
                report.DataFields["IntersectGeom"].Hidden = true;
                report.DataFields["GeomPrj"].Hidden = true;
                report.DataFields["SurfaceElevation"].Hidden = true;
                report.DataFields["Obstacle"].Hidden = true;
                report.DataFields["SelectedObstacle"].Hidden = true;
                report.DataFields["SurfaceType"].Hidden = true;
                report.DataFields["ExactVertexGeom"].Hidden = true;
                report.DataFields["CloseCommand"].Hidden = true;
                report.DataFields["Distance"].Hidden = true;
                report.DataFields["BufferPrj"].Hidden = true;
                
                report.DataFields["Elevation"].HeaderText = "Elevation ("+InitOmega.HeightConverter.Unit+")";
                report.DataFields["Height"].HeaderText = "Height (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["Penetrate"].HeaderText = "Penetrate (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["Plane"].HeaderText = "Equation (" + InitOmega.HeightConverter.Unit + ")";
                report.DataFields["VsType"].HeaderText = "Type";


                 var selectedSurfaceType = SelectedSurface.SurfaceBase.EtodSurfaceType;
                 if (selectedSurfaceType == EtodSurfaceType.Area1 ||
                     selectedSurfaceType == EtodSurfaceType.Area2D ||
                     selectedSurfaceType == EtodSurfaceType.Area3 ||
                     selectedSurfaceType == EtodSurfaceType.Area4)
                 {
                     report.DataFields["X"].Hidden = true;
                     report.DataFields["Y"].Hidden = true;
                     //report.DataFields["Elevation"].HeaderText = "Height ("+InitOmega.HeightConverter.Unit+")";
                 }
                 
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = this.SelectedSurface.ViewCaption;// "Document"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Omega Report";
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
                    MessageBox.Show("The document was saved successfully!", "Omega", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured while saving document!", "Omega", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void SaveAll(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportItemList();
                var report = new DoddleReport.Report(ReportItemList.ToReportSource());
                report.RenderingRow += reportAll_RenderingRow;

                report.TextFields.Title = "Omega Obstacle Penetration Report";
                report.TextFields.SubTitle = "";
                report.TextFields.Footer = "Copyright 2018 &copy; R.I.S.K Company";
                string rwyDesignators = _etodViewModel.RwyClassList[0].Name;

                string category = "";
                if (_etodViewModel.SelectedCategory != null)
                    category = _etodViewModel.SelectedCategory.Name;

                report.TextFields.Header = string.Format(@"
                Report Generated: {0}
                Airport/Heliport: {1}
                RWY: {2}
                RWY direction: {3}
                RWY classification: {4}
                RWY category: {5}
                RWY code number: {6}
                Measurement units:: {7}
                Elevation Datum: {8}
                ", DateTime.Now, GlobalParams.Database.AirportHeliport.Name, rwyDesignators,
                 _etodViewModel.SelectedRwyDirection.Name, _etodViewModel.SelectedClassification.Name, category, _etodViewModel.RwyClassList[0].CodeNumber,  InitOmega.HeightConverter.Unit=="m"?"M":"Foot",Common.ConvertHeight(_etodViewModel.SelectedElevationDatum.Height,Enums.RoundType.ToNearest));


                // Customize the data fields
                report.DataFields["Id"].Hidden = true;

                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Omega Report";
                dlg.Filter = "Html documents|*.htm" +
                 "|Pdf document|*.pdf" +
                 "|Excel Worksheets|*.xls";
                // Show save file dialog box
                dlg.FilterIndex = 3;
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
            var allSurfaceReport = new List<ObstacleReport>();
            foreach (var surface in SurfaceList)
            {
                allSurfaceReport.AddRange(surface.SurfaceBase.GetReport);
            }
            allSurfaceReport = allSurfaceReport.OrderBy(obs => obs.Id).ToList<ObstacleReport>(); ;

            ReportItemList = new List<ReportItem>();
            foreach (var obsReport in allSurfaceReport)
            {
                bool isHave = false;

                ReportItem reportItem;
                var items = ReportItemList.Where(item => item.Name == obsReport.Name).ToList<ReportItem>();
                if (items.Count > 0)
                {
                    isHave = true;
                    reportItem = items[0];
                }
                else
                {
                    reportItem = new ReportItem();
                    reportItem.Id = obsReport.Id;
                    reportItem.Name = obsReport.Name;
                    reportItem.ObstacleElevation = obsReport.Elevation+InitOmega.HeightConverter.Unit=="m"?"M":"Foot";
                }
                string penetration = obsReport.Penetrate + "/" + Common.ConvertHeight(obsReport.SurfaceElevation,Enums.RoundType.ToNearest);
                switch (obsReport.SurfaceType)
                {
                    case Aran.PANDA.Constants.SurfaceType.InnerHorizontal:
                        reportItem.InnerHorizontal = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.CONICAL:
                        reportItem.Conical = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.OuterHorizontal:
                        reportItem.OuterHorizontal = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.InnerApproach:
                        reportItem.InnerApproach = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.Approach:
                        reportItem.Approach = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.Transitional:
                        reportItem.Transitional = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.InnerTransitional:
                        reportItem.InnerTransitional = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.BalkedLanding:
                        reportItem.BalkedLanding = penetration;
                        break;
                    case Aran.PANDA.Constants.SurfaceType.TakeOffClimb:
                        reportItem.TakeOffClimb = penetration;
                        break;
                   
                    case Aran.PANDA.Constants.SurfaceType.Strip:
                        reportItem.Strip = penetration;
                        break;
                    default:
                        break;
                }
                if (!isHave)
                    ReportItemList.Add(reportItem);
            }
        }

        private void reportAll_RenderingRow(object sender, ReportRowEventArgs e)
        {
            if (e.Row.RowType == ReportRowType.DataRow)
            {
                var reportItem = e.Row.DataItem as ReportItem;
                if (reportItem != null)
                {
                    for (int i = 3; i < e.Row.Fields.Count(); i++)
                    {
                        string obstruction = "";
                        switch (e.Row.Fields[i].HeaderText)
                        {
                            case "Inner Horizontal":
                                obstruction = (string)e.Row["InnerHorizontal"];
                                break;
                            case "Conical":
                                obstruction = (string)e.Row["Conical"];
                                break;
                            case "Outer Horizontal":
                                obstruction = (string)e.Row["OuterHorizontal"];
                                break;
                            case "Strip":
                                obstruction = (string)e.Row["Strip"];
                                break;
                            case "Transitional":
                                obstruction = (string)e.Row["Transitional"];
                                break;
                            case "Approach":
                                obstruction = (string)e.Row["Approach"];
                                break;
                            case "Inner Approach":
                                obstruction = (string)e.Row["InnerApproach"];
                                break;
                            case "Inner Transitional":
                                obstruction = (string)e.Row["InnerTransitional"];
                                break;
                            case "Take Off Climb":
                                obstruction = (string)e.Row["TakeOffClimb"];
                                break;
                            case "Balked Landing":
                                obstruction = (string)e.Row["BalkedLanding"];
                                break;
                            default:
                                break;
                        }
                        if (obstruction.Length > 2 && !obstruction.StartsWith("-"))
                            e.Row.Fields[i].DataStyle.ForeColor = System.Drawing.Color.Red;
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
                e.Row.Fields["Id"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Name"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Elevation"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Plane"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["GeomType"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["VsType"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Penetrate"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Y"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["X"].DataStyle.ForeColor = System.Drawing.Color.Red;
            }
        }

        #endregion
    }
}
