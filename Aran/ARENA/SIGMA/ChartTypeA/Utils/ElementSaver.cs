using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartTypeA.Models;
using ESRI.ArcGIS.Carto;

namespace ChartTypeA.Utils
{
    class ElementSaver
    {
        // ReSharper disable once TooManyArguments
        public static  void Save(PDM.AirportHeliport adhp,RwyDirWrapper rwyDirWrapper,DateTime airacDateTime,double offset)
        {
            SaveAiracDate(airacDateTime);
            SaveMagValue(adhp);
            SaveElevUnit();
            SaveOtherDimensionUnit();
            SaveAdhp(adhp,rwyDirWrapper,offset);
        }

        public static void SaveAiracDate(DateTime airacDateTime)
        {
            var airacElem = EsriElementFinder.GetElementByname("Sigma_EffectiveDate");
            var airacTextElement = airacElem as ITextElement;

            if (airacTextElement == null)
                return;

            airacTextElement.Text = airacDateTime.ToString("MMMM yyyy");
        }

        public static void SaveMagValue(PDM.AirportHeliport adhp)
        {
            var elem = EsriElementFinder.GetElementByname("MagValue");
            var textElement = elem as ITextElement;

            if (!adhp.MagneticVariation.HasValue)
                return;

            if (textElement == null)
                return;

            var ew = adhp.MagneticVariation.Value < 0
                ? "W"
                : "E";
            textElement.Text = "Magnetic variation " + adhp.MagneticVariation.Value + "°" + ew + " " + adhp.DateMagneticVariation;
        }

        public static void SaveAdhp(PDM.AirportHeliport adhp,RwyDirWrapper rwyDirWrapper,double offset)
        {
            var elem = EsriElementFinder.GetElementByname("ElemAdhp");
            var textElement = elem as ITextElement;

            if (!adhp.MagneticVariation.HasValue)
                return;

            if (textElement == null)
                return;

            textElement.Text = adhp.Name + "/" + adhp.Designator;
            if (rwyDirWrapper != null)
            {
                textElement.Text += Environment.NewLine + rwyDirWrapper.Name;
                if (Math.Abs(offset) > 0.01)
                    textElement.Text += " + " + offset + "°offset";
            }
        }

        public static void SaveElevUnit()
        {
            var elem = EsriElementFinder.GetElementByname("ElevUnit");
            var textElement = elem as ITextElement;

          
            if (textElement == null)
                return;

            var ftM = InitChartTypeA.HeightConverter.Unit == "FT"
                ? "feet"
                : "metres";
            textElement.Text = "Elevations in " + ftM;
        }

        public static void SaveOtherDimensionUnit()
        {
            var elem = EsriElementFinder.GetElementByname("OtherDimension");
            var textElement = elem as ITextElement;

            if (textElement == null)
                return;

            var ftm = InitChartTypeA.DistanceConverter.Unit == "ft"
                ? "feet"
                : "metres";
            textElement.Text = "All other dimensions in " + ftm;
        }

      


        //public static void SaveOtherParams(PDM.AirportHeliport adhp, RwyDirWrapper selectedRwyDir, double offset, int airacValue)
        //{
        //    var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

        //    graphicsContainer.Reset();
        //    var element = graphicsContainer.Next();

        //    while (element != null)
        //    {
        //        if (element is IElement)
        //        {
        //            var textElement = element as ITextElement;
        //            if (textElement != null)
        //            {
        //                IElementProperties3 elemProperties = element as IElementProperties3;
        //                if (elemProperties == null) continue;
        //                if (elemProperties.Name == "MagValue" && adhp.MagneticVariation.HasValue)
        //                {
        //                    var ew = adhp.MagneticVariation.Value < 0
        //                        ? "W"
        //                        : "E";
        //                    textElement.Text = "Magnetic variation " + adhp.MagneticVariation.Value + "°" + ew + " " + adhp.DateMagneticVariation;

        //                }
        //                else if (elemProperties.Name == "ElevUnit")
        //                {
        //                    var ftM = InitChartTypeA.HeightConverter.Unit == "FT"
        //                        ? "feet"
        //                        : "metres";
        //                    textElement.Text = "Elevations in " + ftM;
        //                }
        //                else if (elemProperties.Name == "OtherDimension")
        //                {
        //                    var ftm = InitChartTypeA.DistanceConverter.Unit == "ft"
        //                        ? "feet"
        //                        : "metres";
        //                    textElement.Text = "All other dimensions in " + ftm;
        //                }

        //                else if (elemProperties.Name == "Sigma_EffectiveDate")
        //                    textElement.Text = DateTime.Now.ToString("dd-MM-yyyy");

        //                else if (elemProperties.Name == "ElemAdhp")
        //                {
        //                    textElement.Text = adhp.Name + "/" + adhp.Designator;
        //                    if (selectedRwyDir != null)
        //                    {
        //                        textElement.Text += Environment.NewLine + selectedRwyDir.Name;
        //                        if (Math.Abs(offset) > 0.01)
        //                            textElement.Text += " + " + offset + "°offset";
        //                    }
        //                }
        //            }
        //        }
        //        element = graphicsContainer.Next();
        //    }
        //}

    }
}
