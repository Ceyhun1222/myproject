using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DisplayUI;
using ESRI.ArcGIS.Geometry;

namespace Aran.Delta.Settings
{
    public enum styleType 
    {
    
    }
    internal static class Globals
    {
        public static int Handle { get; set; }

        public static DeltaSettings Settings { get; set; }
       
        public static bool SelectSymbol(ISymbol inSymbol, out ISymbol outSymbol)
        {
            ISymbolSelector symbolSelector = new SymbolSelector();
            if (inSymbol == null)
            {
                outSymbol = null;
                return false;
            }

            symbolSelector.AddSymbol(inSymbol);

            if (symbolSelector.SelectSymbol(Globals.Handle))
            {
                outSymbol = symbolSelector.GetSymbolAt(0);
                return true;
            }

            outSymbol = null;
            return false;
        }

        public static ISymbol CreateDefaultSymbol(esriGeometryType geomType, FeatureType featureType = 0)
        {
            ISymbol symbol = null;

            if (geomType == esriGeometryType.esriGeometryPoint)
            {
                if (featureType != 0)
                {
                    symbol = CreateDefaultAimFeatCharacterSymbol(featureType) as ISymbol;
                    if (symbol != null)
                        return symbol;
                }

                ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol();
                Random random = new Random();
                markerSymbol.Color = Globals.ColorFromRGB(
                    random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                markerSymbol.Size = 8;

                symbol = markerSymbol as ISymbol;
            }
            else if (geomType == esriGeometryType.esriGeometryPolyline)
            {
                ISimpleLineSymbol sls = new SimpleLineSymbol();
                Random random = new Random();
                sls.Color = Globals.ColorFromRGB(
                            random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                sls.Style = esriSimpleLineStyle.esriSLSSolid;

                symbol = sls as ISymbol;
            }
            else if (geomType == esriGeometryType.esriGeometryPath)
            {
                ITextSymbol textSymbol = new TextSymbol ();
                stdole.IFontDisp fontDisp = (stdole.IFontDisp) (new stdole.StdFont ());
                fontDisp.Name = "Arial";
                fontDisp.Name = "Arial";
                fontDisp.Size = 8;
                textSymbol.Font = fontDisp;
                textSymbol.Text = "AaBbCc";
                symbol = textSymbol as ISymbol;
                }
            else if (geomType == esriGeometryType.esriGeometryPolygon)
            {
                ISimpleFillSymbol sfSym = new SimpleFillSymbol();

                ISimpleLineSymbol sls = new SimpleLineSymbol();
                Random random = new Random();
                sls.Color = Globals.ColorFromRGB(
                            random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                sls.Style = esriSimpleLineStyle.esriSLSSolid;

                sfSym.Outline = sls;
                sfSym.Style = esriSimpleFillStyle.esriSFSNull;

                symbol = sfSym as ISymbol;
            }

            return symbol;
        }

        public static ITextSymbol CreateDefaultTextSymbol()
        {
            ITextSymbol textSymbol = new TextSymbol();
            stdole.IFontDisp fontDisp = (stdole.IFontDisp)(new stdole.StdFont());
            fontDisp.Name = "Arial";
            fontDisp.Size = 8;
            textSymbol.Font = fontDisp;
            textSymbol.Text = "AaBbCc";
            return textSymbol;
        }

        public static ICharacterMarkerSymbol CreateDefaultAimFeatCharacterSymbol(FeatureType featureType)
        {
            ICharacterMarkerSymbol cms = new CharacterMarkerSymbol();

            switch (featureType)
            {
                case FeatureType.AirportHeliport:
                    cms.CharacterIndex = (int)0xF04E;
                    cms.Size = 20;
                    break;
                case FeatureType.VOR:
                    cms.CharacterIndex = (int)0xF041;
                    cms.Size = 18;
                    break;
                case FeatureType.DME:
                    cms.CharacterIndex = (int)0xF042;
                    cms.Size = 18;
                    break;
                case FeatureType.NDB:
                    cms.CharacterIndex = (int)0xF043;
                    cms.Size = 18;
                    break;
                case FeatureType.DesignatedPoint:
                    cms.CharacterIndex = (int)0xF047;
                    cms.Size = 14;
                    break;
                case FeatureType.RunwayCentrelinePoint:
                    cms.CharacterIndex = (int)0xF04D;
                    cms.Size = 16;
                    break;
                default:
                    return null;
            }

            //stdole.IFontDisp font= new stdole.StdFontClass() as stdole.IFontDisp;
            //font.Name = "RISK Aero";
            //cms.Font = font;

            return cms;
        }
        public static IColor ColorFromRGB(int red, int green, int blue)
        {
            IRgbColor rgbColor = new RgbColor();
            rgbColor.Red = red;
            rgbColor.Green = green;
            rgbColor.Blue = blue;
            return rgbColor;
        }

        public static ESRI.ArcGIS.esriSystem.IVariantStream Stream { get; set; }
    }
}
