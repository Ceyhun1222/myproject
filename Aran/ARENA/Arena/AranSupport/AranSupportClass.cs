using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using EsriWorkEnvironment;
using System.Runtime.ExceptionServices;
using System.IO;
using System.Threading;

namespace AranSupport
{
    public class Utilitys
    {
        public string tag;
        public struct AranSupportStruct_Azimuth
        {
            public double Azimuth;
            public double ReverseAzimuth;
        }

        [DllImport("MathFunctions.dll", CharSet = CharSet.Auto)]
        public static extern Int32 ReturnGeodesicAzimuth(double X0, double Y0, double X1, double Y1,
            out double DirectAzimuth, out double InverseAzimuth);

        [DllImport("MathFunctions.dll", CharSet = CharSet.Auto)]
        public static extern void InitEllipsoid(double EquatorialRadius, double InverseFlattening);

        [DllImport("MathFunctions.dll", CharSet = CharSet.Auto)]
        public static extern double ReturnGeodesicDistance(double X0, double Y0, double X1, double Y1);

        [DllImport("MathFunctions.dll", CharSet = CharSet.Auto)]
        public static extern int PointAlongGeodesic(double X, double Y, double Dist, double Azimuth, ref double resx, ref double resy);

        [DllImport("MathFunctions.dll", CharSet = CharSet.Auto)]
        public static extern double Modulus (double X, double Y  = 360.0);
  
        public double GetLONGITUDEFromAIXMString(string AIXM_COORD)
        {
            // ПЕРЕВОД ДОЛГОТЫ из AIXM в формат DD.MM 

            //				A string of "digits" (plus, optionally, a period) followed by one of the 
            //				"Simple Latin upper case letters" E or W, in the forms DDDMMSS.ssY, DDDMMSSY, DDDMM.mm...Y, DDDMMY, and DDD.dd...Y . 
            //				The Y stands for either E (= East) or W (= West), DDD represents whole degrees, MM whole minutes, and SS whole seconds. 
            //				The period indicates that there are decimal fractions present; whether these are fractions of seconds, minutes, 
            //				or degrees can easily be deduced from the position of the period. The number of digits representing the fractions 
            //				of seconds is 1 = s... <= 4; the relevant number for fractions of minutes and degrees is 1 <= d.../m... <= 8.

            string DD = "";
            string MM = "";
            string SS = "";
            string STORONA_SVETA = "";
            int SIGN = 1;
            double Coord = 0;
            double Gradusy = 0.0;
            double Minuty = 0.0;
            double Sekundy = 0.0;
            string Coordinata = "";

            try
            {

                NumberFormatInfo nfi = new NumberFormatInfo();

                nfi.NumberDecimalSeparator = ".";
                nfi.NumberGroupSeparator = " ";
                nfi.PositiveSign = "+";

                AIXM_COORD = AIXM_COORD.Trim();

                if (IsNumeric(AIXM_COORD))
                {
                    AIXM_COORD = (Convert.ToDouble(AIXM_COORD)) > 0 ? Math.Round(Convert.ToDouble(AIXM_COORD), 2).ToString() + "E" : Math.Round(Convert.ToDouble(AIXM_COORD)*-1, 2).ToString() + "W";

                    while (AIXM_COORD.Length < 7)
                        AIXM_COORD = "0" + AIXM_COORD;
                }

                STORONA_SVETA = AIXM_COORD.Substring(AIXM_COORD.Length - 1, 1);
                if (STORONA_SVETA == "W") SIGN = -1;


                if (IsNumeric(STORONA_SVETA))
                {
                    STORONA_SVETA = AIXM_COORD.Substring(0, 1);
                    if (STORONA_SVETA == "W") SIGN = -1;
                    AIXM_COORD = AIXM_COORD.Substring(1, AIXM_COORD.Length - 1);
                }
                else
                    AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);
                //AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);

                int SepPos = AIXM_COORD.LastIndexOf(".");

                if (SepPos > 0) //DDDMMSS.ss...X, DDDMM.mm...X, and DDD.dd...X
                {
                    Coordinata = AIXM_COORD.Substring(0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 3:  //DDD.dd...
                            Coord = Convert.ToDouble(AIXM_COORD, nfi) * SIGN;
                            break;

                        case 5:  //DDDMM.mm... 
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, AIXM_COORD.Length - 3);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 7:  //DDDMMSS.ss... 
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, 2);
                            SS = AIXM_COORD.Substring(5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }
                else //DDDMMSSX and DDDMMX
                {
                    Coordinata = AIXM_COORD;
                    switch (Coordinata.Length)
                    {
                        case 5:  //DDDMM 
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, AIXM_COORD.Length - 3);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 7:  //DDDMMSS
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, 2);
                            SS = AIXM_COORD.Substring(5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                        case 9:  //DDDMMSS.SS
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, 2);
                            SS = AIXM_COORD.Substring(5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi)/100;
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Coord = 0;
            }

            return Coord;

        }

        public double GetLATITUDEFromAIXMString(string AIXM_COORD)
        {
            // ПЕРЕВОД ШИРОТЫ из AIXM в формат DD.MM 

            //A string of "digits" (plus, optionally, a period) followed by one of the "Simple Latin upper case letters" N or S, 
            //in the forms DDMMSS.ss...X, DDMMSSX, DDMM.mm...X, DDMMX, and DD.dd...X. The X stands for either N (= North) or S (= South), 
            //DD represents whole degrees, MM whole minutes, and SS whole seconds. The period indicates that there are decimal 
            //fractions present; whether these are fractions of seconds, minutes, or degrees can easily be deduced from the position 
            //of the period. The number of digits representing the fractions of seconds is 1<= s... <= 4; the relevant number for 
            //fractions of minutes and degrees is 1 <= d.../m... <= 8.

            string DD = "";
            string MM = "";
            string SS = "";
            string STORONA_SVETA = "";
            int SIGN = 1;
            double Coord = 0;
            double Gradusy = 0.0;
            double Minuty = 0.0;
            double Sekundy = 0.0;
            string Coordinata = "";

            try
            {
                NumberFormatInfo nfi = new NumberFormatInfo();

                nfi.NumberDecimalSeparator = ".";
                nfi.NumberGroupSeparator = " ";
                nfi.PositiveSign = "+";

                AIXM_COORD = AIXM_COORD.Trim();
                if (IsNumeric(AIXM_COORD))
                {
                    AIXM_COORD = (Convert.ToDouble(AIXM_COORD)) > 0 ? Math.Round(Convert.ToDouble(AIXM_COORD), 2).ToString() + "N" : Math.Round(Convert.ToDouble(AIXM_COORD), 2).ToString() + "S";

                    while (AIXM_COORD.Length < 6)
                        AIXM_COORD = "0" + AIXM_COORD;
                }

                STORONA_SVETA = AIXM_COORD.Substring(AIXM_COORD.Length - 1, 1);
                if (STORONA_SVETA == "S") SIGN = -1;

               

                if (IsNumeric(STORONA_SVETA))
                {
                    STORONA_SVETA = AIXM_COORD.Substring(0, 1);
                    if (STORONA_SVETA == "S") SIGN = -1;
                    AIXM_COORD = AIXM_COORD.Substring(1, AIXM_COORD.Length - 1);
                }
                else 
                    AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);

                

                int SepPos = AIXM_COORD.LastIndexOf(".");

                if (SepPos > 0) //DDMMSS.ss...X, DDMM.mm...X, and DD.dd...X
                {
                    Coordinata = AIXM_COORD.Substring(0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 2:  //DD.dd...
                        case 3: //DD.d...
                            Coord = Convert.ToDouble(AIXM_COORD, nfi) * SIGN;
                            break;

                        case 4:  //DDMM.mm... 
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, AIXM_COORD.Length - 2);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 6:  //DDMMSS.ss... 
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, 2);
                            SS = AIXM_COORD.Substring(4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }
                else //DDMMSSX and DDMMX
                {
                    Coordinata = AIXM_COORD;
                    switch (Coordinata.Length)
                    {
                        case 4:  //DDMM 
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, AIXM_COORD.Length - 2);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 6:  //DDMMSS
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, 2);
                            SS = AIXM_COORD.Substring(4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                        case 8:  //DDMMSS.ss
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, 2);
                            SS = AIXM_COORD.Substring(4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi)/100;
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }


            }
            catch 
            {
                //MessageBox.Show(ex.Message);
                Coord = 0;
            }

            return Coord;

        }

        public double ConvertValueToMeter(string AIXM_VALUE, string AIXM_UOM)
        {
            double Mvalue = 0;
            double V = 0;

            if ((AIXM_UOM == null) || (AIXM_UOM.Trim().Length <= 0)) AIXM_UOM = "M";

            AIXM_UOM = AIXM_UOM.ToUpper();
            NumberFormatInfo nfi = new NumberFormatInfo();


            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = " ";
            nfi.PositiveSign = "+";


            if ((AIXM_VALUE.Trim().Length == 0) || (!IsNumeric(AIXM_VALUE)))
            {
                AIXM_VALUE = "-9999";
                if (AIXM_UOM.CompareTo("FT") == 0) AIXM_VALUE = 32805.02.ToString();
            }

            try
            {
                if (AIXM_VALUE != "") V = Convert.ToDouble(AIXM_VALUE);
                else V = 0;
                switch (AIXM_UOM)
                {
                    case "M":
                        Mvalue = V * 1;
                        break;
                    case "KM":
                        Mvalue = V * 1000;
                        break;
                    case "FT":
                        Mvalue = V * 0.3048;
                        break;
                    case "NM":
                        Mvalue = V * 1852;
                        break;
                    case "FL":
                        Mvalue = V * 30.48;
                        break;
                    default:
                        Mvalue = 0;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Mvalue;
        }

        public double ConvertDurationToSecond(string AIXM_VALUE, string AIXM_UOM)
        {
            double Mvalue = 0;
            double V = 0;

            AIXM_UOM = AIXM_UOM.ToUpper();
            try
            {
                if (AIXM_VALUE != "") V = Convert.ToDouble(AIXM_VALUE);
                else V = 0;
                switch (AIXM_UOM)
                {
                    case "H":
                        Mvalue = V * 3600;
                        break;
                    case "M":
                        Mvalue = V * 60;
                        break;
                    case "S":
                        Mvalue = V * 1;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Mvalue;
        }

        public double ConvertSpeedToKilometresPerHour(string AIXM_VALUE, string AIXM_UOM)
        {
            double Svalue = 0;
            double V = 0;

            AIXM_UOM = AIXM_UOM.ToUpper();
            NumberFormatInfo nfi = new NumberFormatInfo();

            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = " ";
            nfi.PositiveSign = "+";

            try
            {
                if (AIXM_VALUE != "") V = Convert.ToDouble(AIXM_VALUE, nfi);
                else V = 0;
                switch (AIXM_UOM)
                {
                    case "KM/H":
                        Svalue = V * 1;
                        break;
                    case "KT":
                        Svalue = V * 1.852;
                        break;
                    case "MACH":
                        Svalue = V * 1193.256;
                        break;
                    case "M/MIN":
                        Svalue = V * 0.06;
                        break;
                    case "FT/MIN":
                        Svalue = V * 0.018288;
                        break;
                    case "M/SEC":
                        Svalue = V * 3.6;
                        break;
                    case "FT/SEC":
                        Svalue = V * 1.09728;
                        break;
                    default:
                        Svalue = 0;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Svalue;
        }

        public IPoint Create_ESRI_POINT(string Lat, string Lon, string Elev, string ElevUom)
        {
            
            IPoint pnt = new PointClass();
            if (IsNumeric(Lon) && IsNumeric(Lat))
                return Create_ESRI_POINT2(Convert.ToDouble(Lat), Convert.ToDouble(Lon), ConvertValueToMeter(Elev, ElevUom), ElevUom);

            pnt.PutCoords(GetLONGITUDEFromAIXMString(Lon), GetLATITUDEFromAIXMString(Lat));
            IZAware zAware = pnt as IZAware;
            zAware.ZAware = true;
            pnt.Z = ConvertValueToMeter(Elev, ElevUom); // сохрагним в Z значение, переведенное в метры

            IMAware mAware = pnt as IMAware;
            mAware.MAware = true;
            pnt.M = ConvertValueToMeter(Elev, "M"); // сохрагним в М значение в тех ЕИ, которые представлены в файле AIXM


            return pnt;

        }

        public IPoint Create_ESRI_POINT2(double Lat, double Lon, double Elev, string ElevUom)
        {

            IPoint pnt = new PointClass();
            pnt.PutCoords(Lon, Lat);
            IZAware zAware = pnt as IZAware;
            zAware.ZAware = true;
            pnt.Z = ConvertValueToMeter(Elev.ToString(), ElevUom); // сохрагним в Z значение, переведенное в метры

            IMAware mAware = pnt as IMAware;
            mAware.MAware = true;
            pnt.M = ConvertValueToMeter(Elev.ToString(), "M");     // сохрагним в М значение в тех ЕИ, которые представлены в файле AIXM

            return pnt;

        }


        public void SaveShapeToTable(object iP, string ConString, string tblName, Int32 rowId)
        {

            try
            {
                IRow row;

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable(tblName);
                try
                {
                    row = table.GetRow(rowId);
                }
                catch 
                {
                    workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                    Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                    fWksp = (IFeatureWorkspace)Wksp;
                    table = fWksp.OpenTable(tblName);
                    row = table.GetRow(rowId);
                }
                row.set_Value(row.Fields.FindField("SHAPE"), iP);
                row.Store();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void DrawPoint(IActiveView activeView, IPoint pnt, int pSize, IRgbColor rgbColor)
        {
            if (activeView == null)
            {
                return;
            }

            IScreenDisplay screenDisplay = activeView.ScreenDisplay;

            // Constant.
            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast
            ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
            simpleMarkerSymbol.Color = rgbColor;
            simpleMarkerSymbol.Size = pSize;

            ISymbol symbol = simpleMarkerSymbol as ISymbol; // Dynamic cast.

            screenDisplay.SetSymbol(symbol);
            IDisplayTransformation displayTransformation = screenDisplay.DisplayTransformation;

            screenDisplay.DrawPoint(pnt);
            screenDisplay.FinishDrawing();
        }

        public bool IsNumeric(string anyString)
        {
            if (anyString == null)
            {
                anyString = "";
            }
            if (anyString.Length > 0)
            {
                double dummyOut = new double();
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
                return Double.TryParse(anyString, System.Globalization.NumberStyles.Any, cultureInfo.NumberFormat, out dummyOut);
            }
            else
            {
                return false;
            }
        }

        public string SetObjectToBlob(object SHP, string propertyName)
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
                //while (b.Length <=2) b = "0"+b;
                res = res + b + ":";
            }


            return res;
        }

        public object GetObjectFromBlob(object anObject, string propName)
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
            catch 
            {
                return null;
            }
        }

        public bool WithinPolygon(IGeometry Poly, IPoint pnt)
        {
            bool res = true;
            try
            {
                IRelationalOperator relOp = Poly as IRelationalOperator;
                res = relOp.Contains(pnt);
                    
            }
            catch
            {
                res = true;
            }

            return res;
        }

        public bool WithinPolygon(IGeometry Poly, ILine line)
        {
            bool res = false;
            try
            {
                IRelationalOperator relOp = Poly as IRelationalOperator;
                res = res || relOp.Contains(line.ToPoint);
                res = res || relOp.Contains(line.FromPoint);
            }
            catch
            {
                res = true;
            }

            return res;
        }

        public bool WithinPolygon(IGeometry Poly, IPolygon plgn)
        {
            bool res = false;
            try
            {
                IRelationalOperator relOp = Poly as IRelationalOperator;
                res = relOp.Contains(plgn);
            }
            catch
            {
                res = true;
            }

            return res;
        }

        public bool WithinPolygon(IGeometry Poly, IGeometry pdmObjGeo)
        {
            bool res = false;
            if (pdmObjGeo == null) return res;
            if (pdmObjGeo is IPoint) res = WithinPolygon(Poly, (IPoint)pdmObjGeo);
            if (pdmObjGeo is ILine) res = WithinPolygon(Poly, (ILine)pdmObjGeo);
            if (pdmObjGeo is IPolygon) res = WithinPolygon(Poly, (IPolygon)pdmObjGeo);

            return res;
        }


        public IPointCollection CreatePrjCircleZone(IPoint prj_CentrePoint, double Radius)
        {

            IPoint pt = new PointClass();
            Polygon pPolygon = new PolygonClass();
            for (int i = 0; i <= 359; i++)
            {
                pt.PutCoords(prj_CentrePoint.X + Radius * Math.Cos(i * (Math.PI / 180)), prj_CentrePoint.Y + Radius * Math.Sin(i * (Math.PI / 180)));
                pPolygon.AddPoint(pt);
            }

            ITopologicalOperator2 pTopo = (ITopologicalOperator2)pPolygon;

            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();

            return pPolygon;

        }

        public string ConvertLongitudeToAIXM45COORD(double GeoLong, string LonSign)
        {
            string _geoLong = Math.Abs(GeoLong) + LonSign;

            int SepPos = _geoLong.LastIndexOf(".");
            if (SepPos == -1) SepPos = _geoLong.Length;
            string tmp = _geoLong.Substring(0, SepPos);

            while (tmp.Length < 3)
            {
                _geoLong = "0" + _geoLong;
                SepPos = _geoLong.LastIndexOf(".");
                tmp = _geoLong.Substring(0, SepPos);
            }

            return _geoLong;
        }

        public string ConvertLatgitudeToAIXM45COORD(double GeoLat, string LatSign)
        {
            string _geoLat = Math.Abs(GeoLat) + LatSign;

            int SepPos = _geoLat.LastIndexOf(".");
            if (SepPos == -1) SepPos = _geoLat.Length;
            string tmp = _geoLat.Substring(0, SepPos);

            while (tmp.Length < 2)
            {
                _geoLat = "0" + _geoLat;
                SepPos = _geoLat.LastIndexOf(".");
                tmp = _geoLat.Substring(0, SepPos);
            }

            return _geoLat;
        }

        public double GetDistanceBetweenPoints_Elips(IPoint geoPntStart, IPoint geoPntEnd)
        {
            InitEllipsoid(6356752.31424518, 298.257223563);

            return Math.Round(ReturnGeodesicDistance(geoPntStart.X, geoPntStart.Y, geoPntEnd.X, geoPntEnd.Y), 2);

        }


        public double Azt2Direction(IPoint ptGeo, double Azt, IMap FocusMap, ISpatialReference pSpatialReference)
        {
            InitEllipsoid(6356752.31424518, 298.257223563);

            double resx = 0;
            double resy = 0;

            int j = PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, ref resx, ref resy);

            IClone pClone = ptGeo as IClone;
            IPoint Pt10 = pClone.Clone() as IPoint;

            Pt10.PutCoords(resx, resy);

            ILine pLine = new ESRI.ArcGIS.Geometry.Line();

            pLine.PutCoords(ptGeo, Pt10);

            pLine = (ILine)EsriUtils.ToProject(pLine, FocusMap, pSpatialReference);
       

           
            double res = Modulus(RadToDeg(pLine.Angle), 360.0); ;

            Pt10 = null;
            pLine = null;
            return res;
        }
        public  double RadToDeg(double X ) 
        {
            return X * (180 / Math.PI);
        }

        public double PointToLineDistance(IPoint refPoint_Prj, IPoint linePoint_Prj, double lineDirDegree)
        {
            double lineDirInRadian = lineDirDegree * Math.PI / 180;
            double CosA = Math.Cos(lineDirInRadian);
            double SinA = Math.Sin(lineDirInRadian);
            return (refPoint_Prj.Y - linePoint_Prj.Y) * CosA - (refPoint_Prj.X - linePoint_Prj.X) * SinA;
        }



        public double GetDistanceBetweenPoints_Proj(IPoint prjPntStart, IPoint prjPntEnd)
        {
            IPolyline ln = new PolylineClass();
            ln.FromPoint = prjPntStart;
            ln.ToPoint = prjPntEnd;

            return ln.Length;
        }

        public AranSupportStruct_Azimuth Return_Azimuth_ReverseAzimuth(IPoint geoPntStart, IPoint geoPntEnd)
        {
            InitEllipsoid(6356752.31424518, 298.257223563);
            double Azimuth;
            double InversAzimuth;
            
            ReturnGeodesicAzimuth(geoPntStart.X, geoPntStart.Y, geoPntEnd.X, geoPntEnd.Y, out	Azimuth, out InversAzimuth);

            return new AranSupportStruct_Azimuth { Azimuth = Azimuth, ReverseAzimuth = InversAzimuth };

        }

        [HandleProcessCorruptedStateExceptions]
        public IGeometry GetGeometry(string objectID, string objectTypeName, string pathToMdb)
        {
            IGeometry geo = null;
            IFeatureCursor cursor = null;
            ITable table = null;
            try
            {

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();

                IWorkspace Wksp = workspaceFactory.OpenFromFile(pathToMdb, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                table = fWksp.OpenTable(GetTblName(objectTypeName));

                IFeatureClass fc = (IFeatureClass)table;

                Application.DoEvents();

                IQueryFilter qry = new QueryFilterClass();
                qry.WhereClause = "FeatureGUID = \"" + objectID + "\"";
                cursor = fc.Search(qry, true);
                //comReleaser.ManageLifetime(cursor);

                IFeature row = cursor.NextFeature();
                if (row == null) return null;

                //int shapeIndx = row.Fields.FindField("SHAPE");
                //if (shapeIndx < 0) shapeIndx = row.Fields.FindField("Shape");
                //if (shapeIndx < 0) shapeIndx = row.Fields.FindField("shape");

                if (row.Shape == null) return null;
                geo = row.Shape;
                Marshal.ReleaseComObject(cursor);

                IClone clone = geo as IClone;
                var cl = clone.Clone();

                return cl as IGeometry;//geo;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return geo;
            }
            finally
            {
                Marshal.ReleaseComObject(cursor);
                Marshal.ReleaseComObject(table);

                //GC.Collect();
            }
        }

        public IGeometry GetGeometryFromChart(string objectID, string tableName, string pathToMdb)
        {
            IGeometry geo = null;
            IFeatureCursor cursor = null;
            ITable table = null;
            try
            {

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();

                IWorkspace Wksp = workspaceFactory.OpenFromFile(pathToMdb, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                table = fWksp.OpenTable(tableName);

                IFeatureClass fc = (IFeatureClass)table;

                Application.DoEvents();

                IQueryFilter qry = new QueryFilterClass();
                qry.WhereClause = "FeatureGUID = \"" + objectID + "\"";
                cursor = fc.Search(qry, true);

                IFeature row = cursor.NextFeature();
                if (row == null) return null;

                if (row.Shape == null) return null;
                geo = row.Shape;
                Marshal.ReleaseComObject(cursor);

                IClone clone = geo as IClone;
                var cl = clone.Clone();

                return cl as IGeometry;//geo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return geo;
            }
            finally
            {
                Marshal.ReleaseComObject(cursor);
                Marshal.ReleaseComObject(table);

                //GC.Collect();
            }
        }


        public IGeometry GetGeometry(string objectID, string objectTypeName, string pathToMdb, esriGeometryType geoType)
        {
            //using (ComReleaser cr = new ComReleaser())
            //{
            //    IFeatureCursor updateCursor = featureClass.Update(queryFilter, false);
            //    cr.ManageLifetime(updateCursor);
            //    // Do stuff  
            //}  

            IGeometry geo = null;
            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
            IWorkspace Wksp = workspaceFactory.OpenFromFile(pathToMdb, 0);
            IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
            ITable table = fWksp.OpenTable(GetTblName(objectTypeName, geoType));

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = '" + objectID + "'";
            
            ICursor cursor = table.Search(qry, true);
            //comReleaser.ManageLifetime(cursor);

            IRow row = cursor.NextRow();
            if (row == null) return null;

            int shapeIndx = row.Fields.FindField("SHAPE");
            if (shapeIndx < 0) shapeIndx = row.Fields.FindField("Shape");
            if (shapeIndx < 0) shapeIndx = row.Fields.FindField("shape");

            if (shapeIndx < 0) return null;
            geo = (IGeometry)row.get_Value(shapeIndx);
            
            Marshal.ReleaseComObject(cursor);
            Marshal.ReleaseComObject(table);

            //GC.Collect();
            return geo;
        }

        public string GetTblName(string ObjtypeName)
        {
            string res = "";
            

            switch (ObjtypeName)
            {
                case ("AirportHeliport"):
                    res = "AirportHeliport";
                    break;
                case ("RunwayDirection"):
                    res = "RunwayDirection";
                    break;
                case ("RunwayCenterLinePoint"):
                    res = "RunwayDirectionCenterLinePoint";
                    break;
                case ("GlidePath"):
                    res = "GlidePath";
                    break;
                case ("Localizer"):
                    res = "Localizer";
                    break;
                case ("VOR"):
                    res = "VOR";
                    break;
                case ("DME"):
                    res = "DME";
                    break;
                case ("NDB"):
                    res = "NDB";
                    break;
                case ("TACAN"):
                    res = "TACAN";
                    break;
                case ("WayPoint"):
                    res = "WayPoint";
                    break;
                case ("Enroute"):
                case ("RouteSegment"):
                    res = "RouteSegment";
                    break;
                case ("Airspace"):
                case ("AirspaceVolume"):
                    res = "AirspaceVolume";
                    break;
                //case ("VerticalStructure"):
                //case ("VerticalStructurePart"):
                //    if (sellObj.Geo != null)
                //    {
                //        switch (sellObj.Geo.GeometryType)
                //        {
                //            case (esriGeometryType.esriGeometryPoint):
                //                selectedLayer = selectedLayer ="VerticalStructure_Point");
                //                break;

                //            case (esriGeometryType.esriGeometryPolyline):
                //            case (esriGeometryType.esriGeometryLine):
                //                selectedLayer = selectedLayer ="VerticalStructure_Curve");
                //                break;

                //            case (esriGeometryType.esriGeometryPolygon):
                //                selectedLayer = selectedLayer ="VerticalStructure_Surface");
                //                break;
                //        }
                //    }
                //    break;

                case ("InstrumentApproachProcedure"):
                case ("StandardInstrumentArrival"):
                case ("StandardInstrumentDeparture"):
                case ("ProcedureTransitions"):
                case ("FinalLeg"):
                case ("ProcedureLeg"):
                case ("MissaedApproachLeg"):
                    res = "ProcedureLegs";

                    break;

                case ("FacilityMakeUp"):
                    res = "FacilityMakeUp";

                    break;
                default:
                    res = ObjtypeName;
                    break;
            }

            return res;
        }

        public string GetTblName(string ObjtypeName, esriGeometryType geoType)
        {
            string res = "";


            switch (ObjtypeName)
            {
                case ("AirportHeliport"):
                    res = "AirportHeliport";
                    break;
                case ("RunwayDirection"):
                    res = "RunwayDirection";
                    break;
                case ("RunwayCenterLinePoint"):
                    res = "RunwayDirectionCenterLinePoint";
                    break;
                case ("GlidePath"):
                    res = "GlidePath";
                    break;
                case ("Localizer"):
                    res = "Localizer";
                    break;
                case ("VOR"):
                    res = "VOR";
                    break;
                case ("DME"):
                    res = "DME";
                    break;
                case ("NDB"):
                    res = "NDB";
                    break;
                case ("TACAN"):
                    res = "TACAN";
                    break;
                case ("WayPoint"):
                    res = "WayPoint";
                    break;
                case ("Enroute"):
                case ("RouteSegment"):
                    res = "RouteSegment";
                    break;
                case ("Airspace"):
                case ("AirspaceVolume"):
                    res = "AirspaceVolume";
                    break;
                case ("VerticalStructure"):
                case ("VerticalStructurePart"):
                    //if (sellObj.Geo != null)
                    {
                        switch (geoType)
                        {
                            case (esriGeometryType.esriGeometryPoint):
                                res ="VerticalStructure_Point";
                                break;

                            case (esriGeometryType.esriGeometryPolyline):
                            case (esriGeometryType.esriGeometryLine):
                                res  ="VerticalStructure_Curve";
                                break;

                            case (esriGeometryType.esriGeometryPolygon):
                                res ="VerticalStructure_Surface";
                                break;
                        }
                    }
                    break;

                case ("InstrumentApproachProcedure"):
                case ("StandardInstrumentArrival"):
                case ("StandardInstrumentDeparture"):
                case ("ProcedureTransitions"):
                case ("FinalLeg"):
                case ("ProcedureLeg"):
                case ("MissaedApproachLeg"):
                    res = "ProceduresLegs";

                    break;

                case ("FacilityMakeUp"):
                    res = "FacilityMakeUp";

                    break;
            }

            return res;
        }

        public ITable GetTableByName(string tblName, string pathToMdb)
        {
            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
            IWorkspace Wksp = workspaceFactory.OpenFromFile(pathToMdb, 0);
            IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
            return fWksp.OpenTable(tblName);
        }

        public int GetIDfromDB(string objectID, string objectTypeName, string pathToMdb)
        {

            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
            IWorkspace Wksp = workspaceFactory.OpenFromFile(pathToMdb, 0);
            IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;

            ITable table = fWksp.OpenTable(GetTblName(objectTypeName));

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = \"" + objectID + "\"";
            Application.DoEvents();
            ICursor cursor = table.Update(qry, true);
            Application.DoEvents();

            IRow row = cursor.NextRow();
            if (row == null) return -1;

            int shapeIndx = row.Fields.FindField("OBJECTID");
            if (shapeIndx < 0) shapeIndx = row.Fields.FindField("OID");
            if (shapeIndx < 0) shapeIndx = row.Fields.FindField("ObjectID");

            if (shapeIndx < 0) return -1;
            int res = (int)row.get_Value(shapeIndx);
            Marshal.ReleaseComObject(cursor);

            return res;
        }

        public int GetIDfromDB(string objectID, string objectTypeName, string pathToMdb, esriGeometryType geoType)
        {
            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
            IWorkspace Wksp = workspaceFactory.OpenFromFile(pathToMdb, 0);
            IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
            ITable table = fWksp.OpenTable(GetTblName(objectTypeName, geoType));

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = \"" + objectID + "\"";
            Application.DoEvents();
            ICursor cursor = table.Update(qry, true);
            Application.DoEvents();

            IRow row = cursor.NextRow();
            if (row == null) return -1;

            int shapeIndx = row.Fields.FindField("OBJECTID");
            if (shapeIndx < 0) shapeIndx = row.Fields.FindField("OID");
            if (shapeIndx < 0) shapeIndx = row.Fields.FindField("ObjectID");

            if (shapeIndx < 0) return -1;
            int res = (int)row.get_Value(shapeIndx);
            Marshal.ReleaseComObject(cursor);

            return res;
        }

        public void DeleteObject(ITable Tbl, string objID)
        {
            if (Tbl != null)
            {
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "FeatureGUID = '" + objID + "' OR FeatureGUID = ''";
                Tbl.DeleteSearchedRows(queryFilter);

                //// Create and manage a cursor.
                //ICursor updateCursor = Tbl.Update(queryFilter, false);

                //// Delete the retrieved features.
                //IRow feature = null;
                //while ((feature = updateCursor.NextRow()) != null)
                //{
                //    updateCursor.DeleteRow();
                //}


                //Marshal.ReleaseComObject(updateCursor);
            }


        }




    }

}
