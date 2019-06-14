using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PVT.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ReportViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ReportViewModel class.
        /// </summary>
        /// 
        public Feature Procedure { get; private set; }
        public ObservableCollection<FeatureReport> Reports { get; }

        public ReportViewModel(Feature procedure)
        {
            Procedure = procedure;
            Reports = new ObservableCollection<FeatureReport>();
            if (procedure.Reports != null && procedure.Reports.Count > 0)
                procedure.Reports.ForEach(x => Reports.Add(x));
        }




        private FeatureReport _report;

        public FeatureReport Report
        {
            get => _report;
            set
            {
                Set(() => Report, ref _report, value, true);
            }
        }


        private RelayCommand _exportCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ExportCommand => _exportCommand
                                             ?? (_exportCommand = new RelayCommand(Export));


        private void Export()
        {
            string filter;
            byte[] data;
            switch (Report.FileType)
            {
                case FileType.Excell:
                    filter = "Excell Files (*.xlsx)|*.xlsx";
                    data = Report.Data;
                    break;
                case FileType.XML:
                    filter = "XML Files (*.xml)|*.xml";
                    data = Report.Data;
                    break;
                case FileType.HTML:
                    filter = "HTML Files (*.html)|*.html";
                    data = System.Text.Encoding.UTF8.GetBytes(Report.Html);
                    break;
                default:
                    filter = "(*.)|*.";
                    data = Report.Data;
                    break;
            }

            var ofd = new Microsoft.Win32.SaveFileDialog() { Filter = filter };
            var result = ofd.ShowDialog();
            if (result == false) return;
            var fileName = ofd.FileName;
            BlockerModel.BlockForAction(() => { File.WriteAllBytes(fileName, data); });
            
        }


        private RelayCommand _exportAllCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ExportAllCommand => _exportAllCommand ?? (_exportAllCommand = new RelayCommand(
                                                    ExportAll,
                                                    CanExportAll));

        private bool CanExportAll()
        {
            return Reports.Count > 0;
        }

        private void ExportAll()
        {

            var ofd = new Microsoft.Win32.SaveFileDialog() { Filter = "Zip Files (*.zip)|*.zip" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            var fileName = ofd.FileName;
            
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (var report in Reports)
                    {
                        var zipArchiveEntry  = archive.CreateEntry(CreateFileName(report), CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(report.Data, 0, report.Data.Length);
                    }
                }
            }
        }

        private string CreateFileName(FeatureReport report)
        {
            var ext = "";
            switch (report.FileType)
            {
                case FileType.Excell:
                    ext += ".xlsx";
                    break;
                case FileType.HTML:
                    ext += ".html";
                    break;
                case FileType.XML:
                    ext += ".xml";
                    break;
            }

            return $"{report.ReportType}_{RandomHash()}{ext}";
        }

        private string RandomHash()
        {
            var g = Guid.NewGuid();
            var guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Replace("/", "");
            guidString = guidString.Replace("\\", "");
            return guidString;
        }
    }
}