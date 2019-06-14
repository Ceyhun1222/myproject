using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Omega.Models;
using Aran.PANDA.Common;

namespace Aran.Omega.Export
{
    class ValidationExporter
    {
        public ValidationExporter()
        {
            
        }

        public bool Export(List<RwyClass> rwyClassList)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "OmegaValidation",
                DefaultExt = ".text",
                Title = "Save Omega Validation Report",
                Filter = "Html documents|*.htm"
            };
            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true) return false;

            System.IO.Stream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);

            // ReSharper disable once ComplexConditionExpression
            string html = "<html><head>" +
                          "<meta http-equiv='content-type;' content='text/html;charset=utf-8' />" +
                          "<style type='text/css'>" +
                          ".htmlReport { font: 13px Verdana;font-weight:bold}" +
                          ".body { font: 11.5px Verdana;line-height:19px;margin-left:15px; }" +
                          "</style></head>";
            // ReSharper disable once ComplexConditionExpression
            html += "<body>" +
                    "<div class='htmlReport'>" +
                    "<center>Omega Validation Report<br /></center>" +
                    "<div class='body'><br />Report Generated: " + DateTime.Now + "<br /> " +
                    "Airport/Heliport: " + GlobalParams.Database.AirportHeliport.Designator + "(" + GlobalParams.Database.AirportHeliport.Name + ")<br />";

            foreach (var rwyClass in rwyClassList)
            {
                html += "Rwy : " + rwyClass.Name + "<br />" +
                        "Nominal Length: " + rwyClass.Length + " " + InitOmega.DistanceConverter.Unit + " <br />" +
                        "Calculation Length: " + rwyClass.RwyDirClassList[0].CalculationLength + " " + InitOmega.DistanceConverter.Unit + " <br />";
                foreach (var rwyDir in rwyClass.RwyDirClassList)
                {
                    html += "Rwy direction : " + rwyDir.Name +
                            "<div style='margin-left:10px;font-family:Arial'>" +
                            " -True direction : " + Math.Round(ARANMath.Modulus(rwyDir.Aziumuth), 1) + " &#0176 <br />" +
                            " -TDZ Elevation : " + Common.ConvertHeight(rwyDir.TDZElevation, Enums.RoundType.ToNearest) + " " + InitOmega.HeightConverter.Unit + "<br />";

                    if (rwyDir.Validation.DeclaredDistanceNotAvailable)
                        html += "<span style='color:red;font-weight:bold'>" + rwyDir.StartCntlPt.Designator + " Declared distances not available<br /></span>";
                    else
                        html += " -Declared distances <br />" +
                                "   -TORA : " + rwyDir.Tora + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -TODA : " + rwyDir.Toda + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -ASDA : " + rwyDir.Asda + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -LDA : " + rwyDir.LDA + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -ClearWay : " + rwyDir.ClearWay + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -StopWay : " + rwyDir.StopWay + " " + InitOmega.DistanceConverter.Unit + "<br />";

                    if (!string.IsNullOrEmpty(rwyDir.Validation.ShiftLogs))
                        html += " -Center Line Points shifts" +
                                "<div style='padding-left:15px;'>" + rwyDir.Validation.ShiftLogs + "</div>";

                    html += "</div>";
                }
            }
            html += "</div></div></body></html>";

            stream.Write(Encoding.ASCII.GetBytes(html), 0, html.Length);
            stream.Close();
            return true;
        }
    }
}
