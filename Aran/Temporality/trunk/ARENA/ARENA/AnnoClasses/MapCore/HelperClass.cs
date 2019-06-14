using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Accent.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using System.Reflection;


namespace Accent.MapElements
{
    public class HelperClass
    {
        public static void FormatText(ref TextSymbolClass pTextSymbol, textPosition TextPosition, double Leading, textCase TextCase, horizontalAlignment HorizontalAlignment, verticalAlignment VerticalAlignment, double CharacterSpacing, double CharacterWidth, double WordSpacing)
        {
            IFormattedTextSymbol FrmT = (IFormattedTextSymbol)pTextSymbol;
            if (TextPosition == textPosition.Superscript) FrmT.Position = esriTextPosition.esriTPSuperscript;
            if (TextPosition == textPosition.Subscript) FrmT.Position = esriTextPosition.esriTPSubscript;
            if (TextPosition == textPosition.Normal) FrmT.Position = esriTextPosition.esriTPNormal;

            if (TextCase == textCase.AllCaps) pTextSymbol.Case = esriTextCase.esriTCAllCaps;
            if (TextCase == textCase.Normal) pTextSymbol.Case = esriTextCase.esriTCNormal;
            if (TextCase == textCase.SmallCaps) pTextSymbol.Case = esriTextCase.esriTCSmallCaps;
            if (TextCase == textCase.Lowercase) pTextSymbol.Case = esriTextCase.esriTCLowercase;

            if (HorizontalAlignment == horizontalAlignment.Left) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            if (HorizontalAlignment == horizontalAlignment.Right) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
            if (HorizontalAlignment == horizontalAlignment.Center) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            if (HorizontalAlignment == horizontalAlignment.Full) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull;

            if (VerticalAlignment == verticalAlignment.Baselene) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
            if (VerticalAlignment == verticalAlignment.Bottom) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
            if (VerticalAlignment == verticalAlignment.Center) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
            if (VerticalAlignment == verticalAlignment.Top) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;



            // параметры текста
            pTextSymbol.CharacterSpacing = CharacterSpacing;
            pTextSymbol.CharacterWidth = CharacterWidth;
            pTextSymbol.WordSpacing = WordSpacing;


        }

        public static void CreateFont(ref TextSymbolClass pTextSymbol, AcntFont Font)
        {
            // параметры шрифта
            stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
            pFontDisp.Name = Font.Name;
            pFontDisp.Bold = Font.Bold;
            pFontDisp.Italic = Font.Italic;
            pFontDisp.Underline = Font.UnderLine;


            pFontDisp.Size = new decimal(Font.Size);

            pTextSymbol.Font = pFontDisp;


            IRgbColor FntClr = new RgbColorClass();
            FntClr.Red = Font.FontColor.Red;
            FntClr.Green = Font.FontColor.Green;
            FntClr.Blue = Font.FontColor.Blue;

            pTextSymbol.Color = FntClr;
        }

        public static LineCallout GetSimpleBorder(AcntColor FillColor, fillStyle FillStyle, AcntFrame Border)
        {
            IPoint AnchorPnt = new PointClass();

            LineCallout pCallout = new LineCallout();

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();

            IRgbColor rgbClr = new RgbColor();  //цвет заполнения
            rgbClr.Red = FillColor.Red;
            rgbClr.Blue = FillColor.Blue;
            rgbClr.Green = FillColor.Green;

            smplFill.Color = rgbClr;

            //стиль заполнения
            if (FillStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
            if (FillStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
            if (FillStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            if (FillStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            if (FillStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
            if (FillStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
            if (FillStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
            if (FillStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
            if (FillStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;

            if (Border != null)
            {

                IRgbColor rgbClr1 = new RgbColor(); //цвет рамочного элемента
                rgbClr1.Red = Border.FrameColor.Red;
                rgbClr1.Blue = Border.FrameColor.Blue;
                rgbClr1.Green = Border.FrameColor.Green;

                IColor lineClr = new RgbColor(); // создание рамки
                ISimpleLineSymbol pSimpleLine = new SimpleLineSymbol();
                pSimpleLine.Width = Border.Thickness;
                pSimpleLine.Color = rgbClr1;

                if (Border.FrameLineStyle == lineStyle.lsDash) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDash;
                if (Border.FrameLineStyle == lineStyle.lsDashDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (Border.FrameLineStyle == lineStyle.lsDashDotDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (Border.FrameLineStyle == lineStyle.lsDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDot;
                if (Border.FrameLineStyle == lineStyle.lsInsideFrame) pSimpleLine.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (Border.FrameLineStyle == lineStyle.lsNull) pSimpleLine.Style = esriSimpleLineStyle.esriSLSNull;
                if (Border.FrameLineStyle == lineStyle.lsSolid) pSimpleLine.Style = esriSimpleLineStyle.esriSLSSolid;


                smplFill.Outline = pSimpleLine;

                pCallout.Border = smplFill;

                
                IPoint pnt = new PointClass();
                pnt.PutCoords(10, 10);
                pCallout.AnchorPoint = pnt;

            }
            else
            {
                pCallout.Border = null;
            }


            pCallout.LeaderLine = null;



            //смещение текста внутри рамки
            if (pCallout != null)
            {
                pCallout.Gap = 0;
                pCallout.AccentBar = null;

                ITextMargins pTextMrgns = (ITextMargins)pCallout;

                if (Border == null) pTextMrgns.PutMargins(0, 0, 0, 0);
                else pTextMrgns.PutMargins(Border.FrameMargins.Left, Border.FrameMargins.Top, Border.FrameMargins.Right, Border.FrameMargins.Bottom);
            }

            return pCallout;

        }

        public static LineCallout GetLeaderLineBorder(AcntColor FillColor, fillStyle FillStyle, AcntFrame Border, AcntLeaderLine LeaderLine, AcntPoint Shift)
        {
            IPoint AnchorPnt = new PointClass();

            LineCallout pCallout = new LineCallout();

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();

            IRgbColor rgbClr = new RgbColor();  //цвет заполнения
            rgbClr.Red = FillColor.Red;
            rgbClr.Blue = FillColor.Blue;
            rgbClr.Green = FillColor.Green;

            smplFill.Color = rgbClr;

            //стиль заполнения
            if (FillStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
            if (FillStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
            if (FillStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            if (FillStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            if (FillStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
            if (FillStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
            if (FillStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
            if (FillStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
            if (FillStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;

            if (Border != null)
            {

                IRgbColor rgbClr1 = new RgbColor(); //цвет рамочного элемента
                rgbClr1.Red = Border.FrameColor.Red;
                rgbClr1.Blue = Border.FrameColor.Blue;
                rgbClr1.Green = Border.FrameColor.Green;

                IColor lineClr = new RgbColor(); // создание рамки
                ISimpleLineSymbol pSimpleLine = new SimpleLineSymbol();
                pSimpleLine.Width = Border.Thickness;
                pSimpleLine.Color = rgbClr1;

                if (Border.FrameLineStyle == lineStyle.lsDash) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDash;
                if (Border.FrameLineStyle == lineStyle.lsDashDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (Border.FrameLineStyle == lineStyle.lsDashDotDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (Border.FrameLineStyle == lineStyle.lsDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDot;
                if (Border.FrameLineStyle == lineStyle.lsInsideFrame) pSimpleLine.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (Border.FrameLineStyle == lineStyle.lsNull) pSimpleLine.Style = esriSimpleLineStyle.esriSLSNull;
                if (Border.FrameLineStyle == lineStyle.lsSolid) pSimpleLine.Style = esriSimpleLineStyle.esriSLSSolid;


                smplFill.Outline = pSimpleLine;

                pCallout.Border = smplFill;


                IRgbColor rgbClr2 = new RgbColor(); //цвет рамочного элемента
                rgbClr2.Red = LeaderLine.LeaderColor.Red;
                rgbClr2.Blue = LeaderLine.LeaderColor.Blue;
                rgbClr2.Green = LeaderLine.LeaderColor.Green;

                pCallout.LeaderLine = new SimpleLineSymbolClass();
                pCallout.LeaderLine.Color = rgbClr2;
                pCallout.LeaderLine.Width = LeaderLine.LeaderLineWidth;
                pCallout.LeaderTolerance = 0;


                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSBase) pCallout.Style = esriLineCalloutStyle.esriLCSBase;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCCW;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCW;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSFourPoint) pCallout.Style = esriLineCalloutStyle.esriLCSFourPoint;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSMidpoint) pCallout.Style = esriLineCalloutStyle.esriLCSMidpoint;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSThreePoint) pCallout.Style = esriLineCalloutStyle.esriLCSThreePoint;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSUnderline) pCallout.Style = esriLineCalloutStyle.esriLCSUnderline;


                IPoint pnt = new PointClass();
                pnt.PutCoords(Shift.X, Shift.Y);
                pCallout.AnchorPoint = pnt;

            }
            else
            {
                pCallout.Border = null;
            }



            //смещение текста внутри рамки
            if (pCallout != null)
            {
                pCallout.Gap = 0;
                pCallout.AccentBar = null;

                ITextMargins pTextMrgns = (ITextMargins)pCallout;

                if (Border == null) pTextMrgns.PutMargins(0, 0, 0, 0);
                else pTextMrgns.PutMargins(Border.FrameMargins.Left, Border.FrameMargins.Top, Border.FrameMargins.Right, Border.FrameMargins.Bottom);
            }

            return pCallout;

        }

        public static LineCallout GetmArrowLeaderLine(AcntLeaderLine LeaderLine, AcntArrowMarker ArrowMarker,  AcntPoint Shift)
        {
            LineCallout pCallout = new LineCallout();


            // привязка созданного элемента

            IRgbColor rgbClr2 = new RgbColor(); //цвет рамочного элемента
            rgbClr2.Red = LeaderLine.LeaderColor.Red;
            rgbClr2.Blue = LeaderLine.LeaderColor.Blue;
            rgbClr2.Green = LeaderLine.LeaderColor.Green;

            pCallout.LeaderLine = new CartographicLineSymbol();
            pCallout.LeaderLine.Color = rgbClr2;
            pCallout.LeaderLine.Width = LeaderLine.LeaderLineWidth;

            IArrowMarkerSymbol pMarker = new ArrowMarkerSymbol();
            pMarker.Length = ArrowMarker.Length;

            pMarker.Width = ArrowMarker.Width;

            ISimpleLineDecorationElement pLineDecElem = new SimpleLineDecorationElement();
            pLineDecElem.AddPosition((double)ArrowMarker.Position);
            pLineDecElem.MarkerSymbol = (IMarkerSymbol)pMarker;


            ILineProperties lineProp = (ILineProperties)pCallout.LeaderLine;
            LineDecoration ld = new LineDecorationClass();
            ld.AddElement(pLineDecElem);
            lineProp.LineDecoration = ld;


            pCallout.Gap = 0;
            pCallout.AccentBar = null;

            pCallout.LeaderTolerance = 0;

            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSBase) pCallout.Style = esriLineCalloutStyle.esriLCSBase;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCCW;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCW;
            //if (LineCalloutStyle == lineCalloutStyle.CSCustom) pCallout.Style = esriLineCalloutStyle.esriLCSCustom;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSFourPoint) pCallout.Style = esriLineCalloutStyle.esriLCSFourPoint;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSMidpoint) pCallout.Style = esriLineCalloutStyle.esriLCSMidpoint;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSThreePoint) pCallout.Style = esriLineCalloutStyle.esriLCSThreePoint;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSUnderline) pCallout.Style = esriLineCalloutStyle.esriLCSUnderline;


            IPoint pnt = new PointClass();
            pnt.PutCoords(Shift.X, Shift.Y);
            pCallout.AnchorPoint = pnt;

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();

            smplFill.Style = esriSimpleFillStyle.esriSFSNull;

            IFillSymbol fillSym = smplFill;
            pCallout.Border = fillSym;
            pCallout.Border = null;

            ITextMargins pTextMrgns = (ITextMargins)pCallout;

            pTextMrgns.PutMargins(0, 0, 0, 0);


            return pCallout;
        }

        public static void UseHaloMask(ref TextSymbolClass pTextSymbol, double HaloMaskSize, AcntColor HaloColor)
        {
            IMask haloMsk = (IMask)pTextSymbol;
            haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
            haloMsk.MaskSize = HaloMaskSize;

            ISimpleFillSymbol smplFillHalo = new SimpleFillSymbol();

            IRgbColor rgbClrHalo = new RgbColor();  //цвет заполнения
            rgbClrHalo.Red = HaloColor.Red;
            rgbClrHalo.Blue = HaloColor.Blue;
            rgbClrHalo.Green = HaloColor.Green;

            smplFillHalo.Color = rgbClrHalo;

            ISimpleLineSymbol HaloLine = new SimpleLineSymbolClass();
            HaloLine.Color = rgbClrHalo;
            HaloLine.Width = 0;


            smplFillHalo.Outline = HaloLine;


            haloMsk.MaskSymbol = smplFillHalo;
        }

        public static string TextConstructor(List<List<AcntChartElementWord>> AcntLine)
        {
            try
            {
                string res = "";

                if ((AcntLine != null) && (AcntLine.Count > 0))
                {
                    foreach (List<AcntChartElementWord> Ln in AcntLine)
                    {
                        foreach (AcntChartElementWord AcntWord in Ln)
                        {

                            res = res + AcntWord.ToString() + " ";
                        }

                        res = res + (char)13 + (char)10;
                    }
                }
                return res;
            }


            catch { return ""; }


        }

        public static bool RemoveIElementFromMap(IGraphicsContainer graphicsContainer, string IElementName)
        {
            try
            {
                graphicsContainer.Reset();

                IElement El = graphicsContainer.Next();
                while (El != null)
                {
                    IElementProperties3 ElProp2 = (IElementProperties3)El;
                    if (ElProp2.Name.CompareTo(IElementName) == 0)
                    {
                        graphicsContainer.DeleteElement(El);
                        break;
                    }
                    else
                        El = graphicsContainer.Next();
                }
                return true;
            }
            catch
            { return false; }

        }

        public static void RemoveAllIElementsFromMap(IGraphicsContainer graphicsContainer)
        {
            graphicsContainer.Reset();

            IElement El = graphicsContainer.Next();
            while (El != null)
            {
                IElementProperties3 ElProp2 = (IElementProperties3)El;
                if (ElProp2.Name.StartsWith("AcntElement")) graphicsContainer.DeleteElement(El);

                El = graphicsContainer.Next();
            }
        }

        public static string SetObjectToBlob(object SHP, string propertyName)
        {
            //public static byte[] GeometryToByteArray(object SHP, string propertyName)

            // вначале переведем IGeometry к типу IMemoryBlobStream 
            IMemoryBlobStream memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;
            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            IPersistStream perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(propertyName, SHP);
            perStr.Save(objStr, 0);

            ////затем полученный IMemoryBlobStream представим в виде массива байтов
            object o;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out o);

            byte[] bytes = (byte[])o;

            string res = "";

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                string b = bytes[i].ToString();
                res = res + b + ":";
            }


            return res;
        }

        public static object GetObjectFromBlob(object anObject, string propName)
        {

            try
            {
                string[] words = ((string)anObject).Split(':');

                byte[] bytes = new byte[words.Length];

                for (int i = 0; i <= words.Length - 2; i++) bytes[i] = Convert.ToByte(words[i]);


                // сконвертируем его в геометрию 
                IMemoryBlobStream memBlobStream = new MemoryBlobStream();

                IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                IObjectStream anObjectStream = new ObjectStreamClass();
                anObjectStream.Stream = memBlobStream;

                IPropertySet aPropSet = new PropertySetClass();

                IPersistStream aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                object result = aPropSet.GetProperty(propName);

                return result;


            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static string GetObjectValue(object objectValue, string _link)
        {

            object objVal = objectValue;

            string[] sa = _link.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
               System.Reflection.PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                object objPropVal = propInfo.GetValue(objVal, null);

                if (objPropVal is IList)
                {
                    objPropVal = (objPropVal as IList)[0];
                }

                objVal = objPropVal;

                if (objVal == null)
                    return null;
            }

            //System.Diagnostics.Debug.WriteLine(_link + " - " + objVal.GetType().ToString());

            return (objVal == null ? "<null>" : objVal.ToString());
        }


        public static byte[] EsriToBytes(object esriGeometry, string PropertyName)
        {
            var memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;

            IPropertySet propertySet = new PropertySetClass();

            var perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(PropertyName, esriGeometry);
            perStr.Save(objStr, 0);

            object obj;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out obj);

            return (byte[])obj;
        }

        public static object EsriFromBytes(byte[] bytes, string PropertyName)
        {
            try
            {
                var memBlobStream = new MemoryBlobStream();

                var varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                var anObjectStream = new ObjectStreamClass { Stream = memBlobStream };

                IPropertySet aPropSet = new PropertySetClass();

                var aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                return aPropSet.GetProperty(PropertyName);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
