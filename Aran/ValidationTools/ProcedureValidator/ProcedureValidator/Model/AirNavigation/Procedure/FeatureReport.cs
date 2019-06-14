using HtmlAgilityPack;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PVT.Model
{
    public class FeatureReport
    {

        public Guid Identifier { get; set; }

        public FeatureReportType ReportType { get; set; }
        public DateTime DateTime { get; set; }
        public string Html { get; set; }
        public byte[] Data { get; set; }
        public FileType FileType { get;}

        public FeatureReport(Aran.Aim.Data.FeatureReport report)
        {
            Identifier = report.Identifier;
            switch (report.ReportType)
            {
                case Aran.Aim.Data.FeatureReportType.Mixed:
                    ReportType = FeatureReportType.Mixed;
                    break;
                case Aran.Aim.Data.FeatureReportType.Obstacle:
                    ReportType = FeatureReportType.Obstacle;
                    break;
                case Aran.Aim.Data.FeatureReportType.Geometry:
                    ReportType = FeatureReportType.Geometry;
                    break;
                case Aran.Aim.Data.FeatureReportType.Log:
                    ReportType = FeatureReportType.Log;
                    break;
                case Aran.Aim.Data.FeatureReportType.Protocol:
                    ReportType = FeatureReportType.Protocol;
                    break;
                case Aran.Aim.Data.FeatureReportType.AIXM51:
                    ReportType = FeatureReportType.AIXM51;
                    break;
            }
            if (report.HtmlZipped != null)
            {
                try
                {
                    using (var ms = new MemoryStream(report.HtmlZipped))
                    using (var decompStream = new GZipStream(ms, CompressionMode.Decompress))
                    using (var sr = new StreamReader(decompStream))
                    {
                        Html = sr.ReadToEnd();
                    }

                    HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;
                    HtmlNode.ElementsFlags["meta"] = HtmlElementFlag.Closed;
                    HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(Html);
                    FileType = FileType.HTML;
                    Data = Encoding.UTF8.GetBytes(Html);
                }
                catch
                {
                    FileType = ReportType == FeatureReportType.AIXM51 ? FileType.XML : FileType.Excell;
                    Data = report.HtmlZipped;
                }
            }
            DateTime = report.DateTime;
        }
    }

    public enum FeatureReportType { Mixed, Obstacle, Geometry, Log, Protocol, AIXM51 }
    public enum FileType { Excell, HTML, XML }
}
