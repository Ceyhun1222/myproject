using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Omega.Models;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Queries;

namespace Aran.Omega.Validation
{
    public class EtodValidationReport
    {
        private readonly List<DrawingSurface> _surfaceList;

        public EtodValidationReport(List<DrawingSurface> surfaceList)
        {
            _surfaceList = surfaceList;
        }

        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is FileMode.Truncate or FileMode.Open, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes. </exception>
        /// <exception cref="IOException">An I/O error, such as specifying FileMode.CreateNew when the file specified by <paramref name="path" /> already exists, occurred.-or-The stream has been closed. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
        public async Task<bool> SaveAsync(List<RwyClass> rwyClassList)
        {
            var fileName = GetFileName();

            var task = Task.Factory.StartNew(() =>
            {
                System.IO.Stream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                var html = CreateHtml(rwyClassList);

                stream.Write(Encoding.ASCII.GetBytes(html), 0, html.Length);
                stream.Close();
                return true;
            });
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            return await task.ConfigureAwait(false);
        }

        private string CreateHtml(List<RwyClass> rwyClassList)
        {
            var html = GetHeader();
            html = GetCaption();

            html += GetRwyText(rwyClassList);

            var area1Validation = new Area1Validation(_surfaceList);

            html += area1Validation.GetHtmlReport(_surfaceList.Find(surface => surface.EtodSurfaceType == EtodSurfaceType.Area1));

            var area2Validation = new Area2Validation(_surfaceList);

            html += area2Validation.GetHtmlReport(
                _surfaceList.Find(surface => surface.EtodSurfaceType == EtodSurfaceType.Area2));

            var area3Validaton = new Area3Validation(_surfaceList);
            html += area3Validaton.GetHtmlReport(
                _surfaceList.Find(surface => surface.EtodSurfaceType == EtodSurfaceType.Area3));

            var area4Validation = new Area1Validation(_surfaceList);
            html += area4Validation.GetHtmlReport(
                _surfaceList.Find(surface => surface.EtodSurfaceType == EtodSurfaceType.Area4));

            html += CloseHtml();

            return html;
        }

        private string GetFileName()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "EtodDataValidation"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Title = "Save Etod Data Validation Report";
            dlg.Filter = "Html documents|*.htm";
            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true) throw  new Exception("Canceled");

            return dlg.FileName;
        }

        private string GetHeader()
        {
            string header = "<html><head>" +
                          "<meta http-equiv='content-type;' content='text/html;charset=utf-8' />" +
                          "<style type='text/css'>" +
                          ".htmlReport { font: 13px Verdana;font-weight:bold}" +
                          ".body { font: 11.5px Verdana;line-height:19px;margin-left:15px; }" +
                          " .caption{font: 14px Verdana;font-weight:bold;margin:20px 0px;}" +
                          "                         .htmlReport td { font-weight: normal;text-decoration: none;font-style: none;font-size: 9pt;text-align: Left;vertical-align: Top;}" +
                          "                     .htmlReport th { font-weight: normal;text-decoration: none;font-style: none;font-size: 9pt;text-align: Left;vertical-align: Top;}" +
                          "</style></head>";
            return header;
        }

        private string GetCaption()
        {
            return "<body>" +
                    "<div class='htmlReport'>" +
                    "<center>Etod Data Validation Report<br /></center>" +
                    "<div class='body'><br />Report Generated: " + DateTime.Now + "<br /> " +
                    "Organisation: " + GlobalParams.Database.Organisation.Designator + "(" + GlobalParams.Database.Organisation.Name + ")<br />" +
                    "Airport/Heliport: " + GlobalParams.Database.AirportHeliport.Designator + "(" + GlobalParams.Database.AirportHeliport.Name + ")<br />" +
                    "Distance unit: " + InitOmega.DistanceConverter.Unit + "<br />" +
                    "Elev/Hgt unit: " + InitOmega.HeightConverter.Unit + "<br />";

        }

        private string GetRwyText(List<RwyClass> rwyClassList)
        {
            var html = "";
            foreach (var rwyClass in rwyClassList)
            {
                html += "Rwy : " + rwyClass.Name + "<br />" +
                        "Nominal Length: " + rwyClass.Length + " " + InitOmega.DistanceConverter.Unit + " <br />" +
                        "Calculation Length: " + rwyClass.RwyDirClassList[0].CalculationLength + " " + InitOmega.DistanceConverter.Unit + " <br />";
            }
            return html;
        }

        private string CloseHtml()
        {
            return "</div></div></body></html>";
        }
    }
}
