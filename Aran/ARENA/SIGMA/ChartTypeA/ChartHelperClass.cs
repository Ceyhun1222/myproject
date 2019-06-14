using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using ANCOR.MapCore;
using ANCOR.MapElements;
using Aran.PANDA.Common;
using ChartTypeA.Models;
using ChartTypeA.Utils;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PDM;
using EsriWorkEnvironment;

namespace ChartTypeA
{
    public static class ChartHelperClass
    {
        

        public static void SelectChartTemplate(ref AxPageLayoutControl axPageLayoutControl, string TemplateFolder, ref ListBox lstBx, out double mapSize_Width, out double mapSize_Height)
        {
            axPageLayoutControl.LoadMxFile(TemplateFolder);
            lstBx.Items.Clear();
            var PageOrientation = "Portret";
            if (axPageLayoutControl.PageLayout.Page.Orientation == 2) PageOrientation = "Landscape";
            lstBx.Items.Add("Page orientation = " + PageOrientation);
            lstBx.Items.Add("Page Size = " + ToString(axPageLayoutControl.PageLayout.Page.FormID));

            esriUnits unts = axPageLayoutControl.PageLayout.Page.Units;
            axPageLayoutControl.PageLayout.Page.Units = esriUnits.esriMeters;

            //получить mapSize_Width и mapSize_Height строго в метрах
            IGraphicsContainer graphics = (IGraphicsContainer)axPageLayoutControl.PageLayout;
            IFrameElement frameElement = graphics.FindFrame(axPageLayoutControl.ActiveView.FocusMap);
            IMapFrame mapFrame = (IMapFrame)frameElement;
            IElement mapElement = (IElement)mapFrame;
            IGeometry frameGmtr = mapElement.Geometry;
            mapSize_Width = frameGmtr.Envelope.Width;
            mapSize_Height = frameGmtr.Envelope.Height;

            //поменять единицы измерения и вывести значения mapSize_Width и mapSize_Height на экран
            axPageLayoutControl.PageLayout.Page.Units = unts;

            frameElement = graphics.FindFrame(axPageLayoutControl.ActiveView.FocusMap);
            mapFrame = (IMapFrame)frameElement;
            mapElement = (IElement)mapFrame;
            frameGmtr = mapElement.Geometry;


            lstBx.Items.Add("Map Frame Width = " + Math.Round(frameGmtr.Envelope.Width, 2).ToString());
            lstBx.Items.Add("Map Frame Height = " + Math.Round(frameGmtr.Envelope.Height, 2).ToString());

            lstBx.Items.Add("Page Units = " + ToString(axPageLayoutControl.PageLayout.Page.Units));
        }

        private static string ToString(ESRI.ArcGIS.Carto.esriPageFormID esriPageFormID)
        {
            string res = "A0";
            switch (esriPageFormID)
            {
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA0:
                    res = "A0";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA1:
                    res = "A1";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA2:
                    res = "A2";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA3:
                    res = "A3";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA4:
                    res = "A4";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA5:
                    res = "A5";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormC:
                    res = "C";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormCUSTOM:
                    res = "CUSTOM";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormD:
                    res = "D";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormE:
                    res = "E";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormLegal:
                    res = "Legal";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormLetter:
                    res = "Letter";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormSameAsPrinter:
                    res = "SameAsPrinter";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormTabloid:
                    res = "Tabloid";
                    break;
                default:
                    res = "A0";
                    break;
            }
            return res;
        }

        private static string ToString(ESRI.ArcGIS.esriSystem.esriUnits esriUnits)
        {
            string res = "";

            switch (esriUnits)
            {
                case ESRI.ArcGIS.esriSystem.esriUnits.esriCentimeters:
                    res = "Centimeters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriDecimalDegrees:
                    res = "DecimalDegrees";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriDecimeters:
                    res = "Decimeters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriFeet:
                    res = "Feet";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriInches:
                    res = "Inches";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriKilometers:
                    res = "Kilometers";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMeters:
                    res = "Meters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMiles:
                    res = "Miles";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMillimeters:
                    res = "Millimeters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriNauticalMiles:
                    res = "Nautical Miles";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriPoints:
                    res = "Points";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriUnitsLast:
                    res = "Units Last";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriUnknownUnits:
                    res = "Unknown Units";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriYards:
                    res = "Yards";
                    break;
                default:
                    res = "";
                    break;
            }

            return res;
        }
    }
}
