using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Display;
using System.Drawing;
using ESRI.ArcGIS.esriSystem;
using stdole;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;

namespace EsriWorkEnvironment
{
    public static class EsriUtils
    {
        private const int COLORONCOLOR = 3;
        private const int HORZSIZE = 4;
        private const int VERTSIZE = 6;
        private const int HORZRES = 8;
        private const int VERTRES = 10;
        private const int ASPECTX = 40;
        private const int ASPECTY = 42;
        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;

        private enum PictureTypeConstants 
        {
            picTypeNone = 0,
            picTypeBitmap = 1,
            picTypeMetafile = 2,
            picTypeIcon = 3,
            picTypeEMetafile = 4
        }
        private struct PICTDESC 
        {
            public int cbSizeOfStruct;
            public int picType;
            public IntPtr hPic;
            public IntPtr hpal;
            public int _pad;
        }
        private struct RECT 
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        public static bool RowExist(ITable tbl, string FieldValue, string FieldName = "FeatureGUID")
        {
            IQueryFilter queryFilter = new QueryFilterClass();

            queryFilter.WhereClause = FieldName + " = '" + FieldValue + "'";

            ICursor crsr = tbl.Search(queryFilter, false);
            bool res = crsr.NextRow() != null;
            Marshal.ReleaseComObject(crsr);

            return res;

        }

        public static ISpatialReference  _spatialReferenceDialog(IMap FocusMap)
        {
            ISpatialReference res = FocusMap.SpatialReference;
            try
            {
                //ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);
                if ((FocusMap != null) && (FocusMap.LayerCount>0))
                {
                    ISpatialReferenceDialog spatialReferenceDialog = new SpatialReferenceDialog();
                    ISpatialReferenceDialog3 spatialReferenceDialog3 = (ISpatialReferenceDialog3)spatialReferenceDialog;

                    if (!(FocusMap.SpatialReferenceLocked))
                    {
                        ESRI.ArcGIS.Geometry.ISpatialReference spatialReference = spatialReferenceDialog3.DoModalEdit3(FocusMap.SpatialReference, false, 0);
                        if (spatialReference != null) res= spatialReference;
                    }
                    else
                    {
                        MessageBox.Show("Cannot Edit the Spatial Reference", "Probably Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }

            return res;
        }

        public static void _LayerPropertiesEdit(ILayer _Layer, IActiveView _ActiveView, int _control_hWnd)
        {
            try
            {

                IComPropertySheet myPropertySheet = new ComPropertySheet();
                myPropertySheet.Title = _Layer.Name + " - Layers Properties";
                myPropertySheet.HideHelpButton = true;

                myPropertySheet.ClearCategoryIDs();
                myPropertySheet.AddCategoryID(new ESRI.ArcGIS.esriSystem.UIDClass());
                myPropertySheet.AddPage(new LayerDrawingPropertyPage());

                if ((!(_Layer is ICompositeLayer)) && (_Layer.Name.CompareTo("Annotation Class 1") != 0)) myPropertySheet.AddPage(new LayerLabelsPropertyPage());

                myPropertySheet.AddPage(new LayerDefinitionQueryPropertyPage());

                myPropertySheet.AddPage(new FeatureLayerDisplayPropertyPage());


                ESRI.ArcGIS.esriSystem.ISet propertyObjects = new ESRI.ArcGIS.esriSystem.SetClass();

                propertyObjects.Add(_ActiveView);
                propertyObjects.Add(_Layer);

                bool objectChanged = false;
                if (myPropertySheet.CanEdit(propertyObjects))
                    objectChanged = myPropertySheet.EditProperties(propertyObjects, _control_hWnd);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }
        }

   
        public static ILayer getLayerByNameOrig(IMap FocusMap, string layerName)
        {
            ILayer res = null;
            bool ok = false;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    if (Layer1.Name.CompareTo(layerName) == 0)
                    {
                        res = Layer1;
                        ok = true;
                    }
                    else if (Layer1 is ICompositeLayer)
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                        for (int j = 0; j <= Complayer.Count - 1; j++)
                        {
                            ILayer Layer2 = Complayer.get_Layer(j);

                            if (Layer2.Name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                            {
                                res = Layer2;
                                ok = true;
                                break;
                            }
                        }
                    }
                   
                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }

        public static ILayer getLayerByName(IMap FocusMap, string layerName)
        {
            ILayer res = null;
            bool ok = false;
            string _name;
            try
            {
                
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    _name = Layer1.Name;
                    if (Layer1 is FeatureLayer && ((IFeatureLayer)Layer1).FeatureClass != null) _name = ((IFeatureLayer)Layer1).FeatureClass.AliasName;
                    if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                    {
                        res = Layer1;
                        ok = true;
                    }
                    else if (Layer1 is ICompositeLayer)
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                        for (int j = 0; j <= Complayer.Count - 1; j++)
                        {
                            ILayer Layer2 = Complayer.get_Layer(j);
                            _name = Layer2.Name;
                            if (Layer2 is FeatureLayer && ((IFeatureLayer)Layer2).FeatureClass != null) _name = ((IFeatureLayer)Layer2).FeatureClass.AliasName;
                            if (Layer2 is FDOGraphicsLayer) { FDOGraphicsLayer annoL = (FDOGraphicsLayer)Layer2; _name = ((IFeatureLayer)annoL).Name; }
                            if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                            {
                                res = Layer2;
                                ok = true;
                                break;
                            }
                        }
                    }

                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }

        public static ILayer getLayerByName2(IMap FocusMap, string layerName, bool IgnoreCompositeLayer = false)
        {
            ILayer res = null;
            bool ok = false;
            string _name;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    _name = Layer1.Name;
                    if (Layer1 is FeatureLayer && ((IFeatureLayer)Layer1).FeatureClass != null) _name = ((IFeatureLayer)Layer1).FeatureClass.AliasName;
                    if (_name.StartsWith(layerName))
                    {
                        res = Layer1;
                        ok = true;
                    }
                    else if ((Layer1 is ICompositeLayer) && (!IgnoreCompositeLayer))
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                        for (int j = 0; j <= Complayer.Count - 1; j++)
                        {
                            ILayer Layer2 = Complayer.get_Layer(j);
                            _name = Layer2.Name;
                            if (Layer2 is FeatureLayer && ((IFeatureLayer)Layer2).FeatureClass != null) _name = ((IFeatureLayer)Layer2).FeatureClass.AliasName;
                            if (Layer2 is FDOGraphicsLayer) { FDOGraphicsLayer annoL = (FDOGraphicsLayer)Layer2; _name = ((IFeatureLayer)annoL).Name; }

                            if (_name.ToUpper().StartsWith(layerName.ToUpper()))
                            {
                                res = Layer2;
                                ok = true;
                                break;
                            }
                        }
                    }

                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }

        public static ILayer getLayerByName(ICompositeLayer _GroupLayer, string layerName)
        {
            ILayer res = null;
            bool ok = false;
            string _name;
            try
            {

                for (int i = 0; i <= _GroupLayer.Count - 1; i++)
                {
                    ILayer Layer1 = _GroupLayer.Layer[i];
                    _name = Layer1.Name;
                    if (Layer1 is FeatureLayer && ((IFeatureLayer)Layer1).FeatureClass != null) _name = ((IFeatureLayer)Layer1).FeatureClass.AliasName;
                    if (_name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                    {
                        res = Layer1;
                        ok = true;
                    }
    
                    if (ok) break;
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }


        public static ILayer getCompositLayerByName(IMap FocusMap, string layerName)
        {
            ILayer res = null;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    if (!(Layer1 is ICompositeLayer)) continue;

                    ICompositeLayer Complayer = (ICompositeLayer)Layer1;
                    for (int j = 0; j <= Complayer.Count - 1; j++)
                    {
                        ILayer Layer2 = Complayer.get_Layer(j);

                        if (Layer2.Name.ToUpper().CompareTo(layerName.ToUpper()) == 0)
                        {
                            res = Layer2;

                            break;
                        }
                    }
                }
            }
            catch
            {
                res = null;
            }

            return res;

        }

        public static int getLayerIndex(IMap FocusMap, string layerName)
        {
            int res = -1;
            bool ok = false;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                        if (Layer1.Name.CompareTo(layerName) == 0)
                        {
                            res = i;
                            ok = true;
                        }
                    

                    if (ok) break;
                }
            }
            catch
            {
                res = -1;
            }

            return res;

        }

        public static List<string> getMapLayerNames(IMap FocusMap)
        {
            List<string> resList = new List<string>();
            ILayer res = null;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    if (Layer1 is ICompositeLayer)
                    {
                        ICompositeLayer Complayer = (ICompositeLayer)Layer1;

                        if ((Complayer as ILayer).Name.CompareTo("Anno&Decor") == 0) continue;

                        if ((Complayer as ILayer).Visible)
                        {

                            for (int j = 0; j <= Complayer.Count - 1; j++)
                            {
                                ILayer Layer2 = Complayer.get_Layer(j);
                                res = Layer2;
                                resList.Add(res.Name);

                            }
                        }
                    }
                    else
                    {
                        if (Layer1.Visible)
                        {
                            if (Layer1.Name.CompareTo("Area") == 0) continue;

                            res = Layer1;
                            resList.Add(res.Name);
                        }
                    }

                }
            }
            catch
            {
                res = null;
            }

            return resList;

        }

        public static IEnvelope CalcEnvelope(IGeometry GM, double zoom)
        {


            IEnvelope CalculatedEnvilope = new EnvelopeClass() as IEnvelope;


            if (GM != null && !GM.IsEmpty)
            {

                if (GM.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    IArea Area = GM as IArea;
                    CalculatedEnvilope.XMax = (GM as IPoint).X + zoom;
                    CalculatedEnvilope.YMax = (GM as IPoint).Y + zoom;
                    CalculatedEnvilope.XMin = (GM as IPoint).X - zoom;
                    CalculatedEnvilope.YMin = (GM as IPoint).Y - zoom;
                }
                if (GM.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    IArea Area = GM as IArea;
                    CalculatedEnvilope.XMax = Area.Centroid.X + zoom;
                    CalculatedEnvilope.YMax = Area.Centroid.Y + zoom;
                    CalculatedEnvilope.XMin = Area.Centroid.X - zoom;
                    CalculatedEnvilope.YMin = Area.Centroid.Y - zoom;
                }
                if (GM.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    CalculatedEnvilope = (GM as IPolyline).Envelope;

                    CalculatedEnvilope.Height = 0;

                    IArea Area = CalculatedEnvilope as IArea;

                    CalculatedEnvilope.XMax = Area.Centroid.X + zoom;
                    CalculatedEnvilope.YMax = Area.Centroid.Y + zoom;
                    CalculatedEnvilope.XMin = Area.Centroid.X - zoom;
                    CalculatedEnvilope.YMin = Area.Centroid.Y - zoom;

                }
            }

            return CalculatedEnvilope;
        }

        public static bool IsClockwise(IGeometry Geo)
        {
            if (Geo.GeometryType == esriGeometryType.esriGeometryPoint) return false;

            IPointCollection vertices = (IPointCollection)Geo;
            double sum = 0.0;
            for (int i = 0; i <= vertices.PointCount - 1; i++)
            {
                IPoint v1 = vertices.get_Point(i);
                IPoint v2 = vertices.get_Point((i + 1) % vertices.PointCount);
                sum += (v2.X - v1.X) * (v2.Y + v1.Y);
            }
            return sum >= 0.0;
        }

        public static ITable getTableByname(IFeatureWorkspace featureWorkspace, string nameOfTable)
        {
            try
            {
                if (((IWorkspace2)featureWorkspace).NameExists[esriDatasetType.esriDTFeatureClass, nameOfTable])
                    return featureWorkspace.OpenTable(nameOfTable);
                else if (((IWorkspace2)featureWorkspace).NameExists[esriDatasetType.esriDTTable, nameOfTable])
                    return featureWorkspace.OpenTable(nameOfTable);
                else
                    return null;
            }
            catch 
            {

                return null;
            }
            
        }

        public static IGeometry ToGeo(IGeometry pGeo, IMap pMap, ISpatialReference ToSpatialReference)
        {

            ISpatialReference pSpRefPrj = pMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pMap.SpatialReference as IProjectedCoordinateSystem;
            ISpheroid pSpheroid = pPCS.GeographicCoordinateSystem.Datum.Spheroid;

            ISpatialReference pSpRefShp = ToSpatialReference;

            if (pGeo is IPoint)
            {
                {
                    ((IPoint)pGeo).SpatialReference = pSpRefPrj;
                    ((IPoint)pGeo).Project(pSpRefShp);
                }
            }

            if (pGeo is IPolygon)
            {
                {
                    ((IPolygon)pGeo).SpatialReference = pSpRefPrj;
                    ((IPolygon)pGeo).Project(pSpRefShp);
                }
            }

            if (pGeo is IPolyline)
            {
                {
                    ((IPolyline)pGeo).SpatialReference = pSpRefPrj;
                    ((IPolyline)pGeo).Project(pSpRefShp);
                }
            }

            if (pGeo is ILine)
            {
                {
                    ((ILine)pGeo).SpatialReference = pSpRefPrj;
                    ((ILine)pGeo).Project(pSpRefShp);
                }
            }

            if (pGeo is IEnvelope)
            {
                {
                    ((IEnvelope)pGeo).SpatialReference = pSpRefPrj;
                    ((IEnvelope)pGeo).Project(pSpRefShp);
                }
            }

            return pGeo;
        }

        public static IGeometry ToProject(IGeometry pGeo, IMap pMap, ISpatialReference ToSpatialReference)
        {
            IClone clone = pGeo as IClone;
            var cl = clone.Clone();

            IGeometry cloneGeo = (IGeometry)cl;

            ISpatialReference pSpRefPrj = pMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pMap.SpatialReference as IProjectedCoordinateSystem;
            ISpheroid pSpheroid = pPCS.GeographicCoordinateSystem.Datum.Spheroid;

            ISpatialReference pSpRefShp = ToSpatialReference;

            ((IGeometry)cloneGeo).SpatialReference = pSpRefShp;
            ((IGeometry)cloneGeo).Project(pSpRefPrj);
            return cloneGeo;
        }

        public static IPoint CreateMZPoint(double _X, double _Y)
        {
            IPoint resPnt = new PointClass();
            resPnt.PutCoords(_X, _Y);
            IZAware zAware = resPnt as IZAware;
            zAware.ZAware = true;

            IMAware mAware = resPnt as IMAware;
            mAware.MAware = true;

            return resPnt;
        }

        public static void ChangeProjectionAndMeredian(double CMeridian, IMap pMap)
        {
            //IMap pMap = pDocument.FocusMap;

            ISpatialReferenceFactory2 pSpatRefFact = new SpatialReferenceEnvironmentClass();
            IProjectionGEN pProjection = pSpatRefFact.CreateProjection((int)esriSRProjectionType.esriSRProjection_TransverseMercator) as IProjectionGEN;

            IGeographicCoordinateSystem pGCS = pSpatRefFact.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            ILinearUnit pLinearUnit = pSpatRefFact.CreateUnit((int)esriSRUnitType.esriSRUnit_Meter) as ILinearUnit;
            IProjectedCoordinateSystemEdit pProjCoordSysEdit = new ProjectedCoordinateSystemClass();
            IParameter[] pParams = pProjection.GetDefaultParameters();
            pParams[0].Value = 500000;
            pParams[1].Value = 0;
            pParams[2].Value = Math.Round(CMeridian, 6);
            pParams[3].Value = (double)0.9996;

            object name = "Transverse_Mercator";
            object alias = "UserDefinedAlias";
            object abbreviation = "Trans_Merc";
            object remarks = "ARAN coordinate system.";
            object usage = "";
            object CS = pGCS;
            object LU = pLinearUnit;
            object PRJ = pProjection;
            object PARAMS = pParams;

            pProjCoordSysEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref usage, ref CS, ref LU, ref PRJ, ref PARAMS);

            ISpatialReference pPCS = (ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem)pProjCoordSysEdit; // pRJ
            if (pMap != null)
            {
                pMap.SpatialReference = pPCS;
            }

        }

        public static void CompactDataBase(string ConString)
        {
            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();

            IWorkspace workspace = workspaceFactory.OpenFromFile(ConString, 0);

            IDatabaseCompact cmpkt = (IDatabaseCompact)workspace;
            if (cmpkt.CanCompact()) cmpkt.Compact();

            workspace = null;
        }

        public static esriLicenseStatus CheckLicense(esriLicenseProductCode LicenseProductCode)
        {

            IAoInitialize _aoInit = new AoInitializeClass();

            return (esriLicenseStatus)_aoInit.IsProductCodeAvailable(LicenseProductCode);

        }

        public static System.Boolean CreateJPEG_FromActiveView(ESRI.ArcGIS.Carto.IActiveView activeView, System.String pathFileName)
        {
            //parameter check
            if (activeView == null || !(pathFileName.EndsWith(".jpg")))
            {
                return false;
            }
            ESRI.ArcGIS.Output.IExport export = new ESRI.ArcGIS.Output.ExportJPEGClass();
            export.ExportFileName = pathFileName;

            // Microsoft Windows default DPI resolution
            export.Resolution = 96;
            ESRI.ArcGIS.esriSystem.tagRECT exportRECT = activeView.ExportFrame;
            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
            export.PixelBounds = envelope;
            System.Int32 hDC = export.StartExporting();
            activeView.Output(hDC, (System.Int16)export.Resolution, ref exportRECT, null, null);

            // Finish writing the export file and cleanup any intermediate files
            export.FinishExporting();
            export.Cleanup();

            return true;
        }

        public static System.Boolean CreateJPEG_Hi_Resolution_FromActiveView(ESRI.ArcGIS.Carto.IActiveView activeView, System.String pathFileName)
        {
            //parameter check
            if (activeView == null || !(pathFileName.EndsWith(".jpg")))
            {
                return false;
            }
            ESRI.ArcGIS.Output.IExport export = new ESRI.ArcGIS.Output.ExportJPEGClass();
            export.ExportFileName = pathFileName;

            // Because we are exporting to a resolution that differs from screen 
            // resolution, we should assign the two values to variables for use 
            // in our sizing calculations
            System.Int32 screenResolution = 96;
            System.Int32 outputResolution = 300;

            export.Resolution = outputResolution;

            ESRI.ArcGIS.esriSystem.tagRECT exportRECT; // This is a structure
            exportRECT.left = 0;
            exportRECT.top = 0;
            exportRECT.right = activeView.ExportFrame.right * (outputResolution / screenResolution);
            exportRECT.bottom = activeView.ExportFrame.bottom * (outputResolution / screenResolution);

            // Set up the PixelBounds envelope to match the exportRECT
            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
            export.PixelBounds = envelope;

            System.Int32 hDC = export.StartExporting();

            activeView.Output(hDC, (System.Int16)export.Resolution, ref exportRECT, null, null); // Explicit Cast and 'ref' keyword needed 
            export.FinishExporting();
            export.Cleanup();

            return true;
        }


        ////////////////////////////////////

        public static chartInfo GetChartIno(string chartPath_directoryName)
        {
            string ConString = chartPath_directoryName + @"\pdm.mdb";
            if (!File.Exists(ConString))
                ConString = chartPath_directoryName + @"\aerodrome.mdb";

            chartInfo res = null;

            if (File.Exists(ConString))
            {

                IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                IWorkspace Wksp = workspaceFactory.OpenFromFile(ConString, 0);
                IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
                ITable table = fWksp.OpenTable("ChartInfo");
                if (table != null)
                {
                    IRow infoRow = table.GetRow(1);

                    try
                    {

                        chartInfo ci = new chartInfo
                        {
                            airacCircle = table.FindField("AIRAC_Cycle") > 0 && infoRow.Value[table.FindField("AIRAC_Cycle")]!=null && table.FindField("AIRAC_Cycle") >=0 ? infoRow.Value[table.FindField("AIRAC_Cycle")].ToString() : "",
                            ADHP = table.FindField("ADHP") > 0 && infoRow.Value[table.FindField("ADHP")]!=null && table.FindField("ADHP") >= 0 ? infoRow.Value[table.FindField("ADHP")].ToString() : "",
                            //efectiveDate = table.FindField("EfectiveDate") > 0 && infoRow.Value[table.FindField("EfectiveDate")] !=null && table.FindField("EfectiveDate") >= 0 ? Convert.ToDateTime(infoRow.Value[table.FindField("EfectiveDate")]) : DateTime.Now,
                            publicationDate = table.FindField("PublicationDate") > 0 && infoRow.Value[table.FindField("PublicationDate")] != null && table.FindField("PublicationDate") >= 0 ? Convert.ToDateTime(infoRow.Value[table.FindField("PublicationDate")]) : DateTime.Now,
                            organization = table.FindField("Organisation") > 0 && infoRow.Value[table.FindField("Organisation")]!=null && table.FindField("Organisation") >= 0 ? infoRow.Value[table.FindField("Organisation")].ToString() : "",
                            chartName = table.FindField("chartName") > 0 && infoRow.Value[table.FindField("chartName")] !=null && table.FindField("chartName") >= 0 ? infoRow.Value[table.FindField("chartName")].ToString() : "Chart",
                            RouteLevel = table.FindField("routeLevel") > 0 && infoRow.Value[table.FindField("routeLevel")]!=null && table.FindField("routeLevel") >= 0 ? infoRow.Value[table.FindField("routeLevel")].ToString() : "",
                            uomDist = table.FindField("distUom") > 0 && infoRow.Value[table.FindField("distUom")] != null && table.FindField("distUom") >= 0 ? infoRow.Value[table.FindField("distUom")].ToString() : "NM",
                            uomVert = table.FindField("vertUom") > 0 && infoRow.Value[table.FindField("vertUom")] != null && table.FindField("vertUom") >= 0 ? infoRow.Value[table.FindField("vertUom")].ToString() : "FT",
                            widthBufer = table.FindField("bufWidth") > 0 && infoRow.Value[table.FindField("bufWidth")] != null && table.FindField("bufWidth") >= 0 ? (int)infoRow.Value[table.FindField("bufWidth")] : -1,
                            flag = table.FindField("flag") > 0 && infoRow.Value[table.FindField("flag")] != null && table.FindField("flag") >= 0 ? infoRow.Value[table.FindField("flag")].ToString().CompareTo("1") == 1 : false,
                            baseTempName = ""
                        };

                        //DateTime tt = DateTime.Parse(infoRow.Value[table.FindField("EfectiveDate")].ToString());

                        //if (table.FindField("EfectiveDate") > 0 && infoRow.Value[table.FindField("EfectiveDate")] != null && table.FindField("EfectiveDate") >= 0)
                        //    ci.efectiveDate = DateTime.Parse(infoRow.Value[table.FindField("EfectiveDate")].ToString());
                        //else
                        //    ci.efectiveDate = DateTime.Now;


                        if (ci.chartName!=null && ci.chartName.Length > 0)
                        {
                            string[] words = ci.chartName.Split('<');
                            if (words.Length > 1)
                            {
                                ci.chartName = words[0];
                                ci.baseTempName = words[1];
                            }
                        }

                        ci.RunwayDirectionsList = new List<string>();

                        if (table.FindField("Rdn") > 0 && infoRow.Value[table.FindField("Rdn")] != null)
                        {
                            string r = infoRow.Value[table.FindField("Rdn")].ToString();
                            string[] rdn = r.Split(',');
                            foreach (var item in rdn)
                                ci.RunwayDirectionsList.Add(item);
                        }

                        res = ci;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            
                return res;
           
        }

        public static void StoreChartInfo(chartInfo CI, IFeatureWorkspace _Workspace)
        {
            ICursor rowCur =null;
            try
            {
                ITable tbl = EsriUtils.getTableByname(_Workspace, "ChartInfo");
               
                if (tbl != null)
                {


                    IQueryFilter _Filter = new QueryFilterClass();
                    _Filter.WhereClause = tbl.OIDFieldName + " = 1";
                    rowCur = tbl.Update(_Filter, false);

                    IRow _row = rowCur.NextRow();

                    if (_row == null)  _row = tbl.CreateRow();

                    int findx = -1;
                    findx = _row.Fields.FindField("AIRAC_Cycle"); if (findx >= 0) _row.set_Value(findx, CI.airacCircle);
                    findx = _row.Fields.FindField("ADHP"); if (findx >= 0) _row.set_Value(findx, CI.ADHP);
                    findx = _row.Fields.FindField("Organisation"); if (findx >= 0) _row.set_Value(findx, CI.organization);
                    findx = _row.Fields.FindField("Rdn");
                    if (findx >= 0 && CI.RunwayDirectionsList != null && CI.RunwayDirectionsList.Count > 0)
                    {
                        string rdn = CI.RunwayDirectionsList[0] + ",";
                        if (CI.RunwayDirectionsList.Count > 1)
                            for (int i = 1; i < CI.RunwayDirectionsList.Count; i++)
                            {
                                rdn = rdn + CI.RunwayDirectionsList[i] + ",";
                            }

                        if (rdn.EndsWith(",")) rdn = rdn.Remove(rdn.Length - 1, 1);
                        _row.set_Value(findx, rdn);
                    }

                   

                    
                    findx = _row.Fields.FindField("EfectiveDate"); if (findx >= 0) _row.set_Value(findx, CI.efectiveDate);
                    findx = _row.Fields.FindField("PublicationDate"); if (findx >= 0) _row.set_Value(findx, CI.publicationDate);
                    findx = _row.Fields.FindField("chartName");

                    if (findx >= 0)
                    {

                        if (CI.baseTempName != null && CI.baseTempName.Length > 0)
                        {
                            string Chartname_TempName = CI.chartName + "<" + CI.baseTempName;
                            if (_row.Fields.Field[findx].Length < Chartname_TempName.Length)
                            Chartname_TempName = CI.chartName;

                            if (_row.Fields.Field[findx].Length <= Chartname_TempName.Length)
                                Chartname_TempName = Chartname_TempName.Substring(0, _row.Fields.Field[findx].Length - 1);

                            _row.set_Value(findx, Chartname_TempName);
                        }

                    }

                    findx = _row.Fields.FindField("routeLevel"); if (findx >= 0) _row.set_Value(findx, CI.RouteLevel);
                    findx = _row.Fields.FindField("vertUom"); if (findx >= 0) _row.set_Value(findx, CI.uomVert);
                    findx = _row.Fields.FindField("distUom"); if (findx >= 0) _row.set_Value(findx, CI.uomDist);
                    findx = _row.Fields.FindField("bufWidth"); if (findx >= 0) _row.set_Value(findx, CI.widthBufer);
                    findx = _row.Fields.FindField("flag");
                    if (findx >= 0)
                    {
                        if (CI.flag)
                            _row.set_Value(findx, 1);
                        else
                            _row.set_Value(findx, 0);

                    }



                    _row.Store();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                
            }
            finally
            {
               if (rowCur !=null) Marshal.ReleaseComObject(rowCur);




            }
        }

        ////////////////////////////////////

        [DllImport("olepro32.dll", EntryPoint = "OleCreatePictureIndirect", PreserveSig = false)]
        private static extern int OleCreatePictureIndirect(ref PICTDESC pPictDesc, ref Guid riid, bool fOwn, out IPictureDisp ppvObj);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap", ExactSpelling = true,
            SetLastError = true)]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hObject, int width, int height);

        [DllImport("user32.dll", EntryPoint = "GetDC", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32", EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport("user32", EntryPoint = "FillRect", ExactSpelling = true, SetLastError = true)]
        private static extern int FillRect(IntPtr hdc, ref RECT lpRect, IntPtr hBrush);

        [DllImport("GDI32.dll", EntryPoint = "GetDeviceCaps", ExactSpelling = true, SetLastError = true)]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32", EntryPoint = "GetClientRect", ExactSpelling = true, SetLastError = true)]
        private static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

        public static ISymbol GetSymbol(string style, string classname, int id)
        {
            ISymbol symbol = null;
			IStyleGallery styleGallery=new StyleGallery();
            IStyleGalleryStorage styleGalleryStorage = (IStyleGalleryStorage)styleGallery;
            styleGalleryStorage.TargetFile = style;
            IEnumStyleGalleryItem styleGalleryItems = styleGallery.get_Items(classname, style, "");
            styleGalleryItems.Reset();
            IStyleGalleryItem styleGalleryItem = styleGalleryItems.Next();
            while (styleGalleryItem != null)
            {
                if (styleGalleryItem.ID == id)
                {
                    symbol = (ISymbol)styleGalleryItem.Item;
                    break;
                }
                styleGalleryItem = styleGalleryItems.Next();
            }
            styleGalleryItem = null;
            styleGalleryStorage = null;
            styleGallery = null;

            return symbol;
        }


        //в случае ошибки:
        //1. CTRL + ALT + E
        //2. Under "Managed Debugging Assistants" uncheck PInvokeStackImbalance.

        private static IPictureDisp CreatePictureFromSymbol(IntPtr hDCOld, ref IntPtr hBmpNew,
            ISymbol pSymbol, Size size, int lGap, int backColor)

        {
            IntPtr hDCNew = IntPtr.Zero;
            IntPtr hBmpOld = IntPtr.Zero;
            try
            {
                hDCNew = CreateCompatibleDC(hDCOld);
                hBmpNew = CreateCompatibleBitmap(hDCOld, size.Width, size.Height);
                hBmpOld = SelectObject(hDCNew, hBmpNew);

                // Draw the symbol to the new device context.
                bool lResult = DrawToDC(hDCNew, size, pSymbol, lGap, backColor);

                hBmpNew = SelectObject(hDCNew, hBmpOld);
                DeleteDC(hDCNew);

                // Return the Bitmap as an OLE Picture.
                return CreatePictureFromBitmap(hBmpNew);
            }
            catch (Exception error)
            {
                if (pSymbol != null)
                {
                    pSymbol.ResetDC();
                    if ((hBmpNew != IntPtr.Zero) && (hDCNew != IntPtr.Zero) && (hBmpOld != IntPtr.Zero))
                    {
                        hBmpNew = SelectObject(hDCNew, hBmpOld);
                        DeleteDC(hDCNew);
                    }
                }


                System.Diagnostics.Debug.WriteLine(error.StackTrace + " ERROR " + error.Message);

                throw error;
            }
        }

        private static IPictureDisp CreatePictureFromBitmap(IntPtr hBmpNew)
        {
            try
            {
                Guid iidIPicture = new Guid("7BF80980-BF32-101A-8BBB-00AA00300CAB");

                PICTDESC picDesc = new PICTDESC();
                picDesc.cbSizeOfStruct = Marshal.SizeOf(picDesc);
                picDesc.picType = (int)PictureTypeConstants.picTypeBitmap;
                picDesc.hPic = (IntPtr)hBmpNew;
                picDesc.hpal = IntPtr.Zero;

                // Create Picture object.
                IPictureDisp newPic;
                OleCreatePictureIndirect(ref picDesc, ref iidIPicture, true, out newPic);

                // Return the new Picture object.
                return newPic;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw ex;
            }
        }

        private static bool DrawToWnd(IntPtr hWnd, ISymbol pSymbol, int lGap, int backColor)
        {
            IntPtr hDC = IntPtr.Zero;
            try
            {
                if (hWnd != IntPtr.Zero)
                {
                    // Calculate size of window.
                    RECT udtRect = new RECT();
                    int lResult = GetClientRect(hWnd, ref udtRect);

                    if (lResult != 0)
                    {
                        int lWidth = (udtRect.Right - udtRect.Left);
                        int lHeight = (udtRect.Bottom - udtRect.Top);

                        hDC = GetDC(hWnd);
                        // Must release the DC afterwards.
                        if (hDC != IntPtr.Zero)
                        {
                            bool ok = DrawToDC(hDC, new Size(lWidth, lHeight), pSymbol, lGap, backColor);

                            // Release cached DC obtained with GetDC.
                            ReleaseDC(hWnd, hDC);

                            return ok;
                        }
                    }
                }
            }
            catch
            {
                if (pSymbol != null)
                {
                    // Try resetting DC, in case we have already called SetupDC for this symbol.
                    pSymbol.ResetDC();

                    if ((hWnd != IntPtr.Zero) && (hDC != IntPtr.Zero))
                    {
                        ReleaseDC(hWnd, hDC); // Try to release cached DC obtained with GetDC.
                    }
                }
                return false;
            }
            return true;
        }

        private static bool DrawToDC(IntPtr hDC, Size size, ISymbol pSymbol, int lGap, int backColor)
        {
            try
            {
                if (hDC != IntPtr.Zero)
                {
                    // First clear the existing device context.
                    if (!Clear(hDC, backColor, 0, 0, size.Width, size.Height))
                    {
                        throw new Exception("Could not clear the Device Context.");
                    }

                    // Create the Transformation and Geometry required by ISymbol::Draw.
                    ITransformation pTransformation = CreateTransFromDC(hDC, size.Width, size.Height);
                    IEnvelope pEnvelope = new EnvelopeClass();
                    pEnvelope.PutCoords(lGap, lGap, size.Width - lGap, size.Height - lGap);
                    IGeometry pGeom = CreateSymShape(pSymbol, pEnvelope);

                    // Perform the Draw operation.
                    if ((pTransformation != null) && (pGeom != null))
                    {
                        pSymbol.SetupDC(hDC.ToInt32(), pTransformation);
                        pSymbol.Draw(pGeom);
                        pSymbol.ResetDC();
                    }
                    else
                    {
                        throw new Exception("Could not create required Transformation or Geometry.");
                    }
                }
            }
            catch
            {
                if (pSymbol != null)
                {
                    pSymbol.ResetDC();
                }
                return false;
            }

            return true;
        }

        private static bool Clear(IntPtr hDC, int backgroundColor, int xmin, int ymin, int xmax, int ymax)
        {
            // This function fill the passed in device context with a solid brush,
            // based on the OLE color passed in.
            IntPtr hBrushBackground = IntPtr.Zero;
            int lResult;
            bool ok;

            try
            {
                RECT udtBounds;
                udtBounds.Left = xmin;
                udtBounds.Top = ymin;
                udtBounds.Right = xmax;
                udtBounds.Bottom = ymax;

                hBrushBackground = CreateSolidBrush(backgroundColor);
                if (hBrushBackground == IntPtr.Zero)
                {
                    throw new Exception("Could not create GDI Brush.");
                }
                lResult = FillRect(hDC, ref udtBounds, hBrushBackground);
                if (hBrushBackground == IntPtr.Zero)
                {
                    throw new Exception("Could not fill Device Context.");
                }
                ok = DeleteObject(hBrushBackground);
                if (hBrushBackground == IntPtr.Zero)
                {
                    throw new Exception("Could not delete GDI Brush.");
                }
            }
            catch
            {
                if (hBrushBackground != IntPtr.Zero)
                {
                    ok = DeleteObject(hBrushBackground);
                }
                return false;
            }

            return true;
        }

        private static ITransformation CreateTransFromDC(IntPtr hDC, int lWidth, int lHeight)
        {
            // Calculate the parameters for the new transformation,
            // based on the dimensions passed to this function.
            try
            {
                IEnvelope pBoundsEnvelope = new EnvelopeClass();
                pBoundsEnvelope.PutCoords(0.0, 0.0, (double)lWidth, (double)lHeight);

                tagRECT deviceRect;
                deviceRect.left = 0;
                deviceRect.top = 0;
                deviceRect.right = lWidth;
                deviceRect.bottom = lHeight;

                int dpi = GetDeviceCaps(hDC, LOGPIXELSY);
                if (dpi == 0)
                {
                    throw new Exception("Could not retrieve Resolution from device context.");
                }

                // Create a new display transformation and set its properties.
                IDisplayTransformation newTrans = new DisplayTransformationClass();
                newTrans.VisibleBounds = pBoundsEnvelope;
                newTrans.Bounds = pBoundsEnvelope;
                newTrans.set_DeviceFrame(ref deviceRect);
                newTrans.Resolution = dpi;

                return newTrans;
            }
            catch
            {
                return null;
            }
        }

        private static IGeometry CreateSymShape(ISymbol pSymbol, IEnvelope pEnvelope)
        {
            // This function returns an appropriate Geometry type depending on the
            // Symbol type passed in.
            try
            {
                if (pSymbol is IMarkerSymbol)
                {
                    // For a MarkerSymbol return a Point.
                    IArea pArea = (IArea)pEnvelope;
                    return pArea.Centroid;
                }
                else if ((pSymbol is ILineSymbol) || (pSymbol is ITextSymbol))
                {
                    // For a LineSymbol or TextSymbol return a Polyline.
                    IPolyline pPolyline = new PolylineClass();
                    pPolyline.FromPoint = pEnvelope.LowerLeft;
                    pPolyline.ToPoint = pEnvelope.UpperRight;
                    return pPolyline;
                }
                else
                {
                    // For any FillSymbol return an Envelope.
                    return pEnvelope;
                }
            }
            catch
            {
                return null;
            }
        }

        public static Bitmap SymbolToBitmap(ISymbol userSymbol, Size size, Graphics gr, int backColor)
        {
            IntPtr graphicsHdc = gr.GetHdc();
            IntPtr hBitmap = IntPtr.Zero;
            IPictureDisp newPic = CreatePictureFromSymbol(
                graphicsHdc, ref hBitmap, userSymbol, size, 1, backColor);
            Bitmap newBitmap = Bitmap.FromHbitmap(hBitmap);
            gr.ReleaseHdc(graphicsHdc);

            return newBitmap;
        }


        public static IPoint LocalToPrj(IPoint center, double dirInRadian, double x, double y = 0.0)
        {
            double SinA = Math.Sin(dirInRadian);
            double CosA = Math.Cos(dirInRadian);
            double Xnew = center.X + x * CosA - y * SinA;
            double Ynew = center.Y + x * SinA + y * CosA;
            return new ESRI.ArcGIS.Geometry.Point { X = Xnew, Y = Ynew };
        }
    }

    public class chartInfo
    {
        public string ADHP { get; set; }
        public string airacCircle { get; set; }
        public DateTime efectiveDate { get; set; }
        public DateTime publicationDate { get; set; }
        public string organization { get; set; }
        public string chartName { get; set; }
        public List<string> RunwayDirectionsList { get; set; }
        public string RouteLevel { get; set; }
        public string uomVert { get; set; }
        public string uomDist { get; set; }
        public int widthBufer { get; set; }
        public bool flag { get; set; }
        public string baseTempName { get; set; }

    }

}

