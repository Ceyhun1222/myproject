using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.AranEnvironment;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DisplayUI;
using ESRI.ArcGIS.Geometry;
using Microsoft.Win32;
using Aran.Aim;
using Aran.Aim.Utilities;
using Aran.Aim.DataTypes;
using Aran.Aim.Metadata.UI;
using Aran.Controls;
using System.Runtime.InteropServices;
using System.IO;
using Aran.Queries.Common;
using System.Security.Cryptography;
using Aran.Aim.Data.Filters;

namespace MapEnv
{
    internal static class Globals
    {
        static Globals ()
        {
			TempDir = System.IO.Path.GetTempPath () + @"RISK\IAIMEnvironment";
            AppDir = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "RISK");
            Settings = new Settings ();
            SettingsPageList = new List<EnvAranSettingsPage>();
            AranPluginList = new List<EnvAranPlugin>();
            OpenFileFilters = "Aran Map Project|*.amp";
            MapEdited = false;
            _startedPlugins = new List<AranPlugin>();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            VersionText = version.ToString(3);

            AppText = "IAIM Environment - " + VersionText;
        }

        public static event LayerAddedEventHandler LayerAdded;

        public static bool MapEdited { get; set; }

        public static readonly string VersionText;

		public static readonly string AppText;

		public static readonly string TempDir;

        public static readonly string AppDir;

        public static ISpatialReference DbSpatialRef
		{
			get
			{
				if (_dbSpatialRef == null)
				{
					_dbSpatialRef = CreateWGS84SR ();
				}

				return _dbSpatialRef;
			}
		}

        public static GraphicsLayer GraphicsLayer
        {
            get
            {
                if (_graphicsLayer == null) {
                    _graphicsLayer = new GraphicsLayer();
                    MainForm.Map.AddLayer(_graphicsLayer);
                }

                return _graphicsLayer;
            }
        }


		public static IEnumerable<Aran.Aim.Features.Feature> LoadFeatures (
            Aran.Aim.FeatureType featureType,
            Aran.Aim.Data.Filters.Filter filter)
		{
			return LoadFeatures (featureType, filter, true);
		}

        public static IEnumerable<Aran.Aim.Features.Feature> LoadFeatures (
            Aran.Aim.FeatureType featureType,
            Aran.Aim.Data.Filters.Filter filter,
            bool clearPrevLogs)
        {
            lock (MainForm)
            {
                lock (MainForm.DbProvider)
                {
                    DbProvider dbProvider = MainForm.DbProvider;

                    if (dbProvider == null || dbProvider.State != System.Data.ConnectionState.Open)
                        return new List<Aran.Aim.Features.Feature> ();

                    var result = dbProvider.GetVersionsOf (
                                        featureType,
                                        Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
                                        Guid.Empty, false, null, null, filter);

                    if (!result.IsSucceed)
                    {
                        Globals.ShowError ("Error on loading [" + featureType + "] features");
                        return new List<Aran.Aim.Features.Feature> ();
                    }
                    else
                    {
                        string [] errors = dbProvider.GetLastErrors ();
                        if (Globals.Settings.ShowFeatureLoaderError && errors != null && errors.Length > 0)
                        {
                            Globals.Environment.ShowLogs (errors, clearPrevLogs);
                        }
                    }

                    return result.List as IEnumerable<Aran.Aim.Features.Feature>;
                }
            }
        }

        public static ILayer GetLayerByName (string layerName)
        {
            for (int i = 0; i < Globals.MainForm.Map.LayerCount; i++)
            {
                ILayer layer = Globals.MainForm.Map.get_Layer (i);
                if (layer.Name.ToLower () == layerName.ToLower ())
                {
                    return layer;
                }
            }
            return null;
        }

        public static MainForm MainForm { get; set; }

        public static IAranEnvironment Environment
        {
            get { return MainForm; }
        }

        public static Connection DbConnection { get; set; }

        public static void OnLayerAdded (ILayer layer, int index = -1)
        {
            if (LayerAdded != null)
            {
                MapEdited = true;
				LayerAdded ( new LayerAddedEventArgs ( layer, index ) );
            }
        }

        public static IMapDocument MapDocument { get; set; }

        public static Image ZoomToImage { get; set; }

        public static ISymbol GetLayerSymbol (IGeoFeatureLayer geoFeatLayer, 
            out IUniqueValueRenderer uniqValRen,
            out IClassBreaksRenderer classBreaksRen)
        {
            uniqValRen = null;
            classBreaksRen = null;

            if (geoFeatLayer == null || geoFeatLayer.Renderer == null)
                return null;

            if (geoFeatLayer.Renderer is ISimpleRenderer)
            {
                ISimpleRenderer sr = geoFeatLayer.Renderer as ISimpleRenderer;
                return sr.Symbol;
            }
            else if (geoFeatLayer.Renderer is IUniqueValueRenderer)
            {
                uniqValRen = geoFeatLayer.Renderer as IUniqueValueRenderer;
                return uniqValRen.DefaultSymbol;
            }
            else if (geoFeatLayer.Renderer is IClassBreaksRenderer)
            {
                classBreaksRen = geoFeatLayer.Renderer as IClassBreaksRenderer;
            }
            return null;
        }

        public static void SetLayerSymbol (ILayer layer, ISymbol symbol)
        {
            IGeoFeatureLayer gfl = layer as IGeoFeatureLayer;

            if (gfl != null && gfl.Renderer is ISimpleRenderer)
            {
                ISimpleRenderer sr = gfl.Renderer as ISimpleRenderer;
                sr.Symbol = symbol;
            }
        }

        public static string GeoPropertyNameCaption (string geoPropertyName)
        {
            if (geoPropertyName.ToLower ().EndsWith ("/geo"))
                geoPropertyName = geoPropertyName.Remove (geoPropertyName.Length - 4);

            return geoPropertyName.Replace ("/", "  >  ");
        }

        public static ISymbol CreateDefaultSymbol (esriGeometryType geomType, FeatureType featureType = 0)
        {
            ISymbol symbol = null;

            if (geomType == esriGeometryType.esriGeometryPoint)
            {
                if (featureType != 0)
                {
                    symbol = CreateDefaultAimFeatCharacterSymbol (featureType) as ISymbol;
                    if (symbol != null)
                        return symbol;
                }

                ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol ();
                Random random = new Random ();
                markerSymbol.Color = Globals.ColorFromRGB (
                    random.Next (0, 255), random.Next (0, 255), random.Next (0, 255));
                markerSymbol.Size = 8;

                symbol = markerSymbol as ISymbol;
            }
            else if (geomType == esriGeometryType.esriGeometryPolyline)
            {
                ISimpleLineSymbol sls = new SimpleLineSymbol ();
                Random random = new Random ();
                sls.Color = Globals.ColorFromRGB (
                            random.Next (0, 255), random.Next (0, 255), random.Next (0, 255));
                sls.Style = esriSimpleLineStyle.esriSLSSolid;
                
                symbol = sls as ISymbol;
            }
            else if (geomType == esriGeometryType.esriGeometryPolygon)
            {
                ISimpleFillSymbol sfSym = new SimpleFillSymbol ();

                ISimpleLineSymbol sls = new SimpleLineSymbol ();
                Random random = new Random ();
                sls.Color = Globals.ColorFromRGB (
                            random.Next (0, 255), random.Next (0, 255), random.Next (0, 255));
                sls.Style = esriSimpleLineStyle.esriSLSSolid;

                sfSym.Outline = sls;
                sfSym.Style = esriSimpleFillStyle.esriSFSNull;

                symbol = sfSym as ISymbol;
            }

            return symbol;
        }

        public static ICharacterMarkerSymbol CreateDefaultAimFeatCharacterSymbol (FeatureType featureType)
        {
            ICharacterMarkerSymbol cms = new CharacterMarkerSymbol ();

            switch (featureType)
            {
                case FeatureType.AirportHeliport:
                    cms.CharacterIndex = (int) 0xF04E;
                    cms.Size = 20;
                    break;
                case FeatureType.VOR:
                    cms.CharacterIndex = (int) 0xF041;
                    cms.Size = 18;
                    break;
                case FeatureType.DME:
                    cms.CharacterIndex = (int) 0xF042;
                    cms.Size = 18;
                    break;
                case FeatureType.NDB:
                    cms.CharacterIndex = (int) 0xF043;
                    cms.Size = 18;
                    break;
                case FeatureType.DesignatedPoint:
                    cms.CharacterIndex = (int) 0xF047;
                    cms.Size = 14;
                    break;
                case FeatureType.RunwayCentrelinePoint:
                    cms.CharacterIndex = (int) 0xF04D;
                    cms.Size = 16;
                    break;
                default:
                    return null;
            }

            stdole.IFontDisp font = new stdole.StdFontClass () as stdole.IFontDisp;
            font.Name = "RISK Aero";
            cms.Font = font;

            return cms;
        }

        public static ITextSymbol CreateDefaultTextSymbol ()
        {
            ITextSymbol textSymbol = new TextSymbol ();
            stdole.IFontDisp fontDisp = (stdole.IFontDisp) (new stdole.StdFont ());
            fontDisp.Name = "Arial";
            fontDisp.Size = 8;
            textSymbol.Font = fontDisp;
            textSymbol.Text = "AaBbCc";
            return textSymbol;
        }

        public static bool SelectSymbol (ISymbol inSymbol, out ISymbol outSymbol)
        {
            ISymbolSelector symbolSelector = new SymbolSelector ();
            if (inSymbol == null)
            {
                outSymbol = null;
                return false;
            }
            
            symbolSelector.AddSymbol (inSymbol);

            if (symbolSelector.SelectSymbol (MainForm.Handle.ToInt32 ()))
            {
                outSymbol = symbolSelector.GetSymbolAt (0);
                return true;
            }

            outSymbol = null;
            return false;
        }

        public static IColor ColorFromRGB (int red, int green, int blue)
        {
            IRgbColor rgbColor = new RgbColor ();
            rgbColor.Red = red;
            rgbColor.Green = green;
            rgbColor.Blue = blue;
            return rgbColor;
        }

        public static void ShowError (string message)
        {
			EventHandler h = (object o, EventArgs e) => 
				MessageBox.Show (MainForm, message, MainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

			lock (Globals.MainForm)
			{
				Globals.MainForm.Invoke (h);
			}
        }

        public static ISpatialReference CreateWGS84SR()
        {
            ISpatialReferenceFactory spatialRefFatcory = new SpatialReferenceEnvironment();
            IGeographicCoordinateSystem geoCoordSys;
            geoCoordSys = spatialRefFatcory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            geoCoordSys.SetFalseOriginAndUnits(-180.0, -180.0, 5000000.0);
            geoCoordSys.SetZFalseOriginAndUnits(0.0, 100000.0);
            geoCoordSys.SetMFalseOriginAndUnits(0.0, 100000.0);

            geoCoordSys.SetZDomain(-2000.0, 14000.0);
            geoCoordSys.SetMDomain(-2000.0, 14000.0);
            geoCoordSys.SetDomain(-360.0, 360.0, -360.0, 360.0);

            return geoCoordSys as ISpatialReference;
        }

        public static ISpatialReference CreageProjectedWGS84(double centralMeridian)
        {
            ISpatialReferenceFactory spatialRefFatcory = new SpatialReferenceEnvironment();
            IProjectedCoordinateSystem prjCoordSys;
            prjCoordSys = spatialRefFatcory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRProjCS_WGS1984UTM_34N);
            prjCoordSys.set_CentralMeridian(true, centralMeridian);

            prjCoordSys.SetZDomain(-2000.0, 14000.0);
            prjCoordSys.SetMDomain(-2000.0, 14000.0);
            prjCoordSys.SetDomain(0.0, 10000000.0, 0.0, 10000000.0);

            return prjCoordSys as ISpatialReference;
        }

        public static void GetFeatureListByDepend (object sender, FeatureListByDependEventArgs e)
        {
            Aran.Aim.Data.Filters.Filter filter = null;

            if (e.DependFeature != null)
            {
                string propertyName = GetFeatureRelationPropName (e.FeatureType, e.DependFeature.FeatureType);
                if (propertyName != null)
                {
                    object identifier = e.DependFeature.Identifier;
                    var compOp = new Aran.Aim.Data.Filters.ComparisonOps (
                        Aran.Aim.Data.Filters.ComparisonOpType.EqualTo, propertyName, identifier);
                    filter = new Aran.Aim.Data.Filters.Filter (new Aran.Aim.Data.Filters.OperationChoice (compOp));
                }
            }

            var propList = GetVisiblePropList (e.FeatureType);

            if (propList.Count == 0 || (propList.Count == 1 && propList [0] == "Identifier"))
                propList = null;

            var getResult = MainForm.DbProvider.GetVersionsOf (
                e.FeatureType,
                e.InterpretationType,
                default (Guid),
                false,
                null,
                propList,
                filter);

            if (!getResult.IsSucceed)
                throw new Exception (getResult.Message);

            e.FeatureList.AddRange (getResult.List as IEnumerable<Aran.Aim.Features.Feature>);
        }

        public static string GetFeatureRelationPropName (FeatureType featureType, FeatureType propertyFeatureType)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
            AimPropInfo propInfo = classInfo.Properties.Where (pi => pi.ReferenceFeature == propertyFeatureType).FirstOrDefault ();
            if (propInfo != null)
                return propInfo.AixmName;

            return null;
        }

        public static List<string> GetVisiblePropList (FeatureType featureType)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
            var propList = new List<string> ();

            foreach (var propInfo in classInfo.Properties)
            {
                var uiPropInfo = propInfo.UiPropInfo ();
                if (uiPropInfo.ShowGridView)
                    propList.Add (propInfo.Name);
            }

            return propList;
        }

        public static Aran.Aim.Features.Feature FeatureViewer_GetFeature (Aran.Aim.FeatureType featureType, Guid identifier)
        {
            GettingResult result = MainForm.DbProvider.GetVersionsOf (featureType,
                TimeSliceInterpretationType.BASELINE,
                identifier,
                true, null, null, null);

            if (!result.IsSucceed)
            {
                Globals.ShowError (result.Message);
                return null;
            }
            if (result.List == null || result.List.Count == 0)
                return null;

            return (Aran.Aim.Features.Feature) result.List [0];
        }

        public static MapEnvData MapData { get; set; }

        public static List<EnvAranSettingsPage> SettingsPageList { get; private set; }

        public static List<EnvAranPlugin> AranPluginList { get; private set; }

        public static Settings Settings { get; private set; }

        public static string OpenFileFilters { get; private set; }

        public static Cursor GetCursor (string name)
        {
            string cursorFileName = Application.LocalUserAppDataPath + "\\" + name + ".cur";

            if (!File.Exists (cursorFileName))
            {
                FileStream fs = new FileStream (cursorFileName, FileMode.Create, FileAccess.Write);

                object obj = Properties.Resources.ResourceManager.GetObject (name);

                var data = (byte []) obj;
                fs.Write (data, 0, data.Length);
                fs.Close ();
            }

            IntPtr imgHandle = Win32.LoadImage (IntPtr.Zero,
                cursorFileName,
                Win32.IMAGE_CURSOR,
                0,
                0,
                Win32.LR_LOADFROMFILE);

            return new Cursor (imgHandle);
        }

        public static string DD2DMS (double xORy, bool isX, int round)
        {
            double k = xORy;
            double deg, min, sec;
            int sign = Math.Sign (k);

            {
                double n = Math.Abs (Math.Round (Math.Abs (k) * sign, 10));

                deg = (int) n;
                double dn = (n - deg) * 60;
                dn = Math.Round (dn, 8);
                min = (int) dn;
                sec = (dn - min) * 60;
            }

            string degStr = deg.ToString ();
            string signSymb = "SN";

            if (isX)
            {
                signSymb = "WE";
            }

            sec = Math.Round (sec, round);

            //while (degStr.Length < strLen)
            //    degStr = "0" + degStr;

            return string.Format ((isX ? "{0:000}" : "{0:00}") +"°{1:00}'{2:00.00}\" {3}", degStr, min, sec, signSymb [(sign + 1) >> 1]);
        }

        public static IRgbColor ToRGBColor (System.Drawing.Color color)
        {
            #if ARCGIS931
            	return ESRI.ArcGIS.ADF.Converter.ToRGBColor (color);
            #else
            	return ESRI.ArcGIS.ADF.Connection.Local.Converter.ToRGBColor (color);
            #endif
        }

		internal static void ClearTempShapefiles ()
		{
			//var proc = Process.GetCurrentProcess ();
			//var startFileName = proc.StartInfo.FileName;

			string procName = Process.GetCurrentProcess ().ProcessName;
			var procArr = Process.GetProcessesByName (procName);

			if (procArr.Length == 1 && Directory.Exists (AimFLGlobal.ShapeFileDirectory))
			{

				var fileNameArr = Directory.GetFiles (AimFLGlobal.ShapeFileDirectory);

				foreach (var fileName in fileNameArr)
				{
					try
					{
						File.Delete (fileName);
					}
					catch { }
				}
			}

		}

        public static void StartPlugin(AranPlugin plugin)
        {
            if (_startedPlugins.Contains(plugin))
                return;

            plugin.ToolbarChanged += MainForm.AranPlugin_ToolbarChanged;
            plugin.Startup(Globals.MainForm);
            _startedPlugins.Add(plugin);
        }

        public static string GetAndCreateMapEnvDocumentsDir()
        {
            var path = Globals.Settings.MapEnvDocumentsDir;
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            return path;
        }

        public static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return System.Environment.GetEnvironmentVariable("ProgramFiles");
        }

        public static void SetDGV_DoubleBuffered(DataGridView dgv)
        {
            var pi = dgv.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            pi.SetValue(dgv, true, null);
        }

        public static ConnectionType DbProTypeToConnectionType(DbProviderType dbProType)
        {
            switch (dbProType) {
                case DbProviderType.Aran: return ConnectionType.Aran;
                case DbProviderType.ComSoft: return ConnectionType.ComSoft;
                case DbProviderType.TDB: return ConnectionType.TDB;
                case DbProviderType.XmlFile: return ConnectionType.XmlFile;
                case DbProviderType.AimLocal: return ConnectionType.AimLocal;
            }

            throw new Exception("DbProviderType is invalid!");
        }

        public static string GetConnectionTypeText(ConnectionType connType)
        {
            switch (connType) {
                case ConnectionType.AimLocal: return "ARAN Local File";
                case ConnectionType.Aran: return "ARAN Server";
                case ConnectionType.ComSoft: return "CADAS-AIMDB";
                case ConnectionType.TDB: return "TOSS";
                case ConnectionType.XmlFile: return "AIXM Message XML File";
                default: return string.Empty;
            }
        }
        
        #region AimPropertyControl Events

        public static string AimPropertyControl_FeatureDescription (object sender, FeatureEventArgs e)
        {
            return UIUtilities.GetFeatureDescription (e.Feature);
        }

        public static void AimPropertyControl_FillDataGridColumn (AimClassInfo classInfo, DataGridView dgv)
        {
            UIUtilities.FillColumns (classInfo, dgv);
        }

        public static void AimPropertyControl_SetRow (DataGridView dgv, Aran.Aim.Features.Feature feature, int rowIndex = -1)
        {
            UIUtilities.SetRow (dgv, feature, rowIndex);
        }

        #endregion

        
        private static void FocusMap_SpatialReferenceChanged ()
        {
            MainForm.DoSpatialReferenceChanged ();
        }

        //internal static T RegRead<T> (RegistryKey hKey, string key, string valueName, T defaultValue)
        //{
        //    try
        //    {
        //        RegistryKey regKey = hKey.OpenSubKey (key, false);
        //        if (regKey != null)
        //        {
        //            object value = regKey.GetValue (valueName);
        //            if (value != null)
        //            {
        //                try
        //                {
        //                    return (T) Convert.ChangeType (value, typeof (T));
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //    catch { }

        //    return defaultValue;
        //}

        internal static bool PM ()
        {
            //string PandaRegKey = "SOFTWARE\\RISK\\Panda";
            //string ModuleName = "Departure";
            //string Achar = RegRead<String> (Registry.CurrentUser, PandaRegKey + "\\" + ModuleName, "Acar", "");
            //IPolygon p_LicenseRect = DecoderCode.DecodeLCode (Achar, ModuleName);

            //if (DecoderCode.LstStDtWriter (ModuleName) != 0)
            //{
            //    Globals.ShowError ("CRITICAL ERROR!!");
            //    return false;
            //}

            //if (p_LicenseRect.IsEmpty)
            //{
            //    Globals.ShowError ("ERROR #10LR512");
            //    return false;
            //}

            return true;
        }

		private static ISpatialReference _dbSpatialRef = null;
        private static List<AranPlugin> _startedPlugins;
        private static GraphicsLayer _graphicsLayer;
	}

    public static class Win32
    {
        [DllImport ("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadImage (IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        public const uint IMAGE_CURSOR = 2;
        public const uint LR_LOADFROMFILE = 0x00000010;
    }

    public class FeatureFinder
    {
        public FeatureFinder (List<Aran.Aim.Features.Feature> featureList)
        {
            FeatureList = featureList;
        }

        public List<Aran.Aim.Features.Feature> FeatureList { get; private set; }

        public Aran.Aim.Features.Feature GetFeature (Aran.Aim.FeatureType featureType, Guid identifier)
        {
            if (FeatureList != null)
            {
                foreach (var feat in FeatureList)
                {
                    if (feat.FeatureType == featureType && feat.Identifier == identifier)
                    {
                        return feat;
                    }
                }
            }

            GettingResult result = Globals.MainForm.DbProvider.GetVersionsOf (featureType,
                TimeSliceInterpretationType.BASELINE,
                identifier,
                true, null, null, null);

            if (!result.IsSucceed)
            {
                Globals.ShowError (result.Message);
                return null;
            }
            if (result.List == null || result.List.Count == 0)
                return null;

            return (Aran.Aim.Features.Feature) result.List [0];
        }
    }
}
