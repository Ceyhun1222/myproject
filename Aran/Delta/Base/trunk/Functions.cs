using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Aran.Aim.Features;
using Aran.Delta.Settings;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using Aran.Delta.Model;

namespace Aran.Delta
{
    public class Functions
    {
        public static double DMS2DD(double xDeg, double xMin, double xSec, int Sign)
        {
            double x;
            x = System.Math.Round(Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)), 10);
            return System.Math.Abs(x);
        }

        public static void DD2DMS(double val, out double xDeg, out double xMin, out double xSec, int Sign)
        {
            double x;
            double dx;

            x = System.Math.Abs(System.Math.Round(System.Math.Abs(val) * Sign, 10));

            xDeg = Fix(x);
            dx = (x - xDeg) * 60;
            dx = System.Math.Round(dx, 8);
            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            xSec = System.Math.Round(xSec, 6);
        }

        public static void Dd2DmsStr(double longtitude, double latitude, string decSep, string lonSide, string latSide,
            int sign, int resolution, out string longtitudeStr, out string latitudeStr)
        {
            double x = System.Math.Abs(System.Math.Round(System.Math.Abs(longtitude) * sign, 10));

            double xDeg = Fix(x);
            double dx = Math.Round((x - xDeg) * 60, 8);

            double xMin = Fix(dx);
            double xSec = Math.Round((dx - xMin) * 60, 6);

            int Res = Convert.ToInt32(Math.Pow(10, resolution));
            string xDegStr;

            if (xDeg < 10)
                xDegStr = "00" + xDeg;
            else if (xDeg < 100)
                xDegStr = "0" + xDeg;
            else
                xDegStr = xDeg.ToString(CultureInfo.InvariantCulture);

           // xDegStr = xDegStr + "°";

            string xMinStr = xMin.ToString(CultureInfo.InvariantCulture);
            if (xMin < 10)
                xMinStr = "0" + xMin;

          //  xMinStr = xMinStr + "'";

            xSec = Math.Round(xSec * Res) / Res;
            string xSecStr = Math.Truncate(xSec).ToString(CultureInfo.InvariantCulture);

            xSecStr = xSecStr + decSep;

            //this line adding for writing 8 seconds  as 08 seconds
            if (xSec < 10)
                xSecStr = 0 + xSecStr;

            xSec = Math.Round((xSec - Math.Truncate(xSec)) * Res);

            xSecStr = xSecStr + xSec.ToString(CultureInfo.InvariantCulture);

            if (resolution == 0)
                xSecStr = xSecStr.Remove(xSecStr.IndexOf("."), 2);

           // xSecStr = xSecStr + "''" + lonSide;
            xSecStr = xSecStr + lonSide;
            longtitudeStr = xDegStr + xMinStr + xSecStr;


            double y = System.Math.Abs(System.Math.Round(System.Math.Abs(latitude) * sign, 10));

            double yDeg = Math.Truncate(y);
            double dy = Math.Round((y - yDeg) * 60, 8);

            double yMin = Math.Truncate(dy);
            double ySec = Math.Round((dy - yMin) * 60, 6);

            string yDegStr = yDeg.ToString(CultureInfo.InvariantCulture);
            if (yDeg < 10)
                yDegStr = "0" + yDeg.ToString(CultureInfo.InvariantCulture);

          //  yDegStr = yDegStr + "°";

            string yMinStr = yMin.ToString(CultureInfo.InvariantCulture);
            if (yMin < 10)
                yMinStr = "0" + yMin.ToString(CultureInfo.InvariantCulture);
          //  yMinStr = yMinStr + "'";

            ySec = Math.Round(ySec * Res) / Res;

            string ySecStr = Math.Truncate(ySec).ToString(CultureInfo.InvariantCulture);
            ySecStr = ySecStr + decSep;

            //this line adding for writing 8 seconds  as 08 seconds
            if (ySec < 10)
                ySecStr = 0 + ySecStr;

            ySec = Math.Round((ySec - Math.Truncate(ySec)) * Res, resolution);

            ySecStr = ySecStr + ySec.ToString(CultureInfo.InvariantCulture);

            if (resolution == 0)
                ySecStr = ySecStr.Remove(ySecStr.IndexOf("."), 2);

           // ySecStr = ySecStr + "''" + latSide;
            ySecStr = ySecStr + latSide;
            latitudeStr = yDegStr + yMinStr + ySecStr;
        }

        private static int Fix(double x)
        {
            if (double.IsNaN(x))
                return 0;

            return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
        }

        public static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }

        public static void SetTool()
        {
            if (GlobalParams.AranEnv != null)
            {
               GlobalParams.AranEnv.AranUI.SetCurrentTool(GlobalParams.Tool);
            }
            else
            {
                GlobalParams.Application.CurrentTool = GlobalParams.ByClickToolCommand;
            }
        }

        public static void SetPreviousTool()
        {
            if (GlobalParams.AranEnv != null)
            {
               GlobalParams.AranEnv.AranUI.SetPanTool();
            }
            else if (GlobalParams.Application != null)
            {
                GlobalParams.Application.CurrentTool = null;
            }
        }

        public static bool InitalizeExtension()
        {
            try
            {
                if (GlobalParams.DeltaExt == null)
                {
                    var pID = new ESRI.ArcGIS.esriSystem.UID();
                    pID.Value = "Aran.Delta.Settings.DeltaExtension";
                    GlobalParams.DeltaExt = GlobalParams.Application.FindExtensionByCLSID(pID) as DeltaExtension;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return GlobalParams.DeltaExt != null;
        }

        public static bool IsOverLapping(double min0,double max0,double min1,double max1,ref double intersectMin,ref double intersectMax)
        {
            bool isIntersect = min0 <= max1 && min1 <= max0;

            if (isIntersect)
            {
                intersectMin = min0 >= min1 ? min0 : min1;

                intersectMax = max0 <= max1 ? max0 : max1;
            }
            return isIntersect;
        }

        public static void SaveArenaProject()
        {
            try
            {
                UID menuID = new UIDClass();

                menuID.Value = "{da3b537a-ce02-44bf-be27-98194abc713a}";

                ICommandItem pCmdItem = GlobalParams.Application.Document.CommandBars.Find(menuID);
                pCmdItem.Execute();
                Marshal.ReleaseComObject(pCmdItem);
                Marshal.ReleaseComObject(menuID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<NotamPointClass> ParseText(string ptsText,ref int formatNumber)
        {
            var ptList = new List<NotamPointClass>();
            if (ptsText.Length == 0) return ptList;

            //IsEnter seperated

            var lines = ptsText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //1-4-6-7 variant.Seperated with enter
            if (lines.Length > 2)
            {
                //check is first variant. 572835N242301E
                //                       575308N235606E 

                foreach (var line in lines)
                {
                    //1 Variant

                    var coordText = line.TrimEnd(' ');
                    coordText = coordText.TrimStart(' ');


                    var splitByProbel = coordText.Split(' ');

                    //1 variant 572835N242301E 
                    if (splitByProbel == null || splitByProbel.Length == 1)
                    {
                        var ptFormat = ReturnLatLong(coordText);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);

                        formatNumber = 1;
                    }

                   //6 variant N572835 E242301 
                    else if (splitByProbel.Length == 2)
                    {
                        coordText = coordText.Replace(" ", "");
                        var ptFormat = ReturnLatLong(coordText);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);

                        formatNumber = 6;
                    }

                    //4 variant  572835N 242301E STARI 
                    else if (splitByProbel.Length == 3)
                    {
                        var text = splitByProbel[0] + splitByProbel[1];
                        var ptFormat = ReturnLatLong(text);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);
                        formatNumber = 4;
                    }
                    //7 variant N 57 28 35.26 E 24 23 01.48
                    if (splitByProbel.Length > 3)
                    {
                        coordText = coordText.Replace(" ", "");
                        var ptFormat = ReturnLatLong(coordText);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);

                        formatNumber = 7;
                    }
                }
            }
            else
            {
                ptsText = ptsText.Replace(" ", "");

                //2 variant
                var splitByDashes = ptsText.Split('–');

                if (splitByDashes.Length<2)
                    splitByDashes = ptsText.Split('-');

                if (splitByDashes.Length > 2)
                {
                    foreach (var coordText in splitByDashes)
                    {
                        var strVal = coordText;
                        if (coordText.Contains('('))
                        {
                            int startIndex = coordText.IndexOf('(');
                            int endIndex = coordText.IndexOf(')');

                            if (endIndex > startIndex)
                            {
                                strVal = coordText.Remove(startIndex, endIndex - startIndex + 1);
                            }
                            else continue;
                        }

                        var ptFormat = ReturnLatLong(strVal);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);

                        formatNumber = 2;
                    }
                }
                else
                {
                    var splitByComma = ptsText.Split(';');
                    if (splitByComma != null && splitByComma.Length > 2)
                    {
                        foreach (var coordText in splitByComma)
                        {
                            var strVal = coordText;
                            if (coordText.Contains('('))
                            {
                                int startIndex = coordText.IndexOf('(');
                                int endIndex = coordText.IndexOf(')');
                                if (endIndex > startIndex)
                                {
                                    strVal = coordText.Remove(startIndex, startIndex - endIndex);
                                }
                                else continue;
                            }

                            var ptFormat = ReturnLatLong(strVal);
                            if (ptFormat != null)
                                ptList.Add(ptFormat);

                            formatNumber = 3;
                        }
                    }

                }

            }
            return ptList;

        }

        public static NotamPointClass ReturnLatLong(string text)
        {
            var ptFormat = new NotamPointClass();
            if (text[0] == 'N')
            {
                var latIndexLast = text.IndexOf('E'); 

                ptFormat.Format = CordinateType.InStart;
                ptFormat.Y = text.Substring(1, latIndexLast - 1);
                ptFormat.X = text.Substring(latIndexLast + 1);
                if (ptFormat.X != null && !ptFormat.X.StartsWith("0"))
                    ptFormat.X = 0 + ptFormat.X;

            }
            else
            {
                var latIndexLast = text.IndexOf('N');
                ptFormat.Format = CordinateType.InStart;
                ptFormat.Y = text.Substring(0, latIndexLast);
                ptFormat.X = text.Substring(latIndexLast + 1, text.Length - latIndexLast - 2);
                if (ptFormat.X != null && !ptFormat.X.StartsWith("0"))
                    ptFormat.X = 0 + ptFormat.X;
            }
            return ptFormat;
        }

        public static List<NotamFormatClass> CreateFormatList()
        {
            var formatList = new List<NotamFormatClass>();

            var format1 = new NotamFormatClass
            {
                Title = "Format 1",
                ExampleText = "572835N242301E" + Environment.NewLine+
                              "575308N235606E" + Environment.NewLine +
                              "575359N235811E" + Environment.NewLine +
                              "572735N242301E"
            };

            formatList.Add(format1);

            var format2 = new NotamFormatClass
            {
                Title = "Format 2",
                ExampleText = "570330N0240447E – 571436N0242650E – 570846N0243623E"
            };
            formatList.Add(format2);

            var format3 = new NotamFormatClass
            {
                Title = "Format 3",
                ExampleText = "570330N0240447E; 571436N0242650E; 570846N0243623E;"
            };
            formatList.Add(format3);

            var format4 = new NotamFormatClass
            {
                Title = "Format 4",
                ExampleText = "572835N 242301E STARI " + Environment.NewLine+
                              "575308N 235606E VECBEBRI " + Environment.NewLine +
                              "575359N 235811E LEIMAŅI"
            };
            formatList.Add(format4);

            var format5 = new NotamFormatClass
            {
                Title = "Format 5",
                ExampleText = "570330N0240447E (STARI) – 571436N0242650E (VECBEBRI)– 570846N0243623E (LEIMAŅI)"
            };
            formatList.Add(format5);

            var format6 = new NotamFormatClass
            {
                Title = "Format 6",
                ExampleText = "N572835 E242301 " + Environment.NewLine +
                              "N575308 E235606 " + Environment.NewLine +
                              "N575359 E235811"
            };
            formatList.Add(format6);

            var format7 = new NotamFormatClass
            {
                Title = "Format 7",
                ExampleText = "N 57 28 35.26 E 24 23 01.48 " + Environment.NewLine +
                              "N 57 53 08.44 E 23 56 06.35  " + Environment.NewLine +
                              "N 57 53 59.11 E 23 58 11.23"
            };
            formatList.Add(format7);

            return formatList;

        }

        public static Note CreateNote()
        {
            var note = new Note { Purpose = Aim.Enums.CodeNotePurpose.REMARK };
            var linguisticNote = new LinguisticNote { Note = new Aim.DataTypes.TextNote() };
            linguisticNote.Note.Value = "Has created by Delta!";
            note.TranslatedNote.Add(linguisticNote);
            return note;
        }

        //public Aran.Geometries.Point GetSnappedPoint(bool isVertex, Aran.Geometries.Point ptMap)
        //{
        //    IProximityOperator m_mouseMove_proxOper;
        //    long I;
        //    long j;
        //    double d;
        //    IPoint pt;
        //    Aran.Geometries.Point tmpPt;
        //    double T;
        //    double minDist;
        //    minDist = 50000000;
        //    // TODO: # ... Warning!!! not translated
        //    T = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.FromPoints(GlobalParams.Settings.DeltaSnapModel.Tolerance);
        //    // -------------------
        //    // -----RouteIntersect
        //    // -------------------
        //    foreach (var snapClass in GlobalParams.Settings.DeltaSnapModel.SnapClassList)
        //    {
        //        if (!snapClass.IsSelected)
        //            continue;
        //    }


        //    if (!(layer_ROUTE_SEGMENT == null))
        //    {
        //        if (layer_ROUTE_SEGMENT.Visible)
        //        {
        //            if (((G.CurSettings.SnappedLayers && snapLayerEnum_RouteIntersect)
        //                        == snapLayerEnum_RouteIntersect))
        //            {
        //                tmpPt = SnapToSortedVertex(LIST.SPoint_RouteIntersects, 0, UBound(LIST.SPoint_RouteIntersects), ptMap, T, minDist);
        //                if (!tmpPt.IsEmpty)
        //                {
        //                    GetSnappedPoint = GetPoint(tmpPt.X, tmpPt.Y);
        //                }
        //            }
        //        }
        //    }
        //}

        //public Aran.Geometries.Point SnapToSortedVertex(Aran.Geometries.Point[] A, long fromInd, long toInd, IPoint ptMap, double tolerance, ref double minDist)
        //{
        //    var result = new Aran.Geometries.Point();
        //    long count = (toInd - fromInd);
        //    if ((count < 0))
        //    {
        //        return null;
        //    }

        //    double d;
        //    if ((count == 0))
        //    {
        //        d = Math.Sqrt ((Math.Pow((A[fromInd].X - ptMap.X),2) + Math.Pow((A[fromInd].Y - ptMap.Y),2)));
        //        if ((d < tolerance) && (d < minDist))
        //        {
        //            result = A[fromInd];
        //            minDist = d;
        //        }
        //        // TODO: Exit Function: Warning!!! Need to return the value
        //        return result;
        //    }
        //    long Mid = (fromInd + toInd)/ 2;
        //    if (((ptMap.X
        //                >= (A[Mid].X - tolerance))
        //                && (ptMap.X
        //                <= (A[Mid].X + tolerance))))
        //    {
        //        return SnapToSortedVertex(A, Mid, Mid, ptMap, tolerance, ref minDist);
        //        if ((result.X != null))
        //        {

        //        }
        //        if ((ptMap.X >= A[Mid].X))
        //        {
        //            result = SnapToSortedVertex(A, (Mid + 1), toInd, ptMap, tolerance,ref minDist);
        //        }
        //        else if ((ptMap.X <= A[Mid].X))
        //        {
        //            result = SnapToSortedVertex(A, fromInd, (Mid - 1), ptMap, tolerance,ref minDist);
        //        }
        //    }
        //    else if ((ptMap.X
        //                > (A[Mid].X + tolerance)))
        //    {
        //        result = SnapToSortedVertex(A, (Mid + 1), toInd, ptMap, tolerance,ref minDist);
        //    }
        //    else
        //    {
        //        result = SnapToSortedVertex(A, fromInd, (Mid - 1), ptMap, tolerance,ref minDist);
        //    }
        //    return result;
        //}

    }
}
