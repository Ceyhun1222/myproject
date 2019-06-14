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

        public static ILayer getLayerByName(IMap FocusMap, string layerName)
        {
            ILayer res = null;
            bool ok = false;
            try
            {
                for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
                {
                    ILayer Layer1 = FocusMap.get_Layer(i);
                    if (Layer1 is ICompositeLayer)
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
                    else
                    {

                        if (Layer1.Name.CompareTo(layerName) == 0)
                        {
                            res = Layer1;
                            ok = true;
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

        public static ITable getTableByname(IFeatureWorkspace featureWorkspace, string nameOfTable)
        {
            return featureWorkspace.OpenTable(nameOfTable);
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
            ISpatialReference pSpRefPrj = pMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pMap.SpatialReference as IProjectedCoordinateSystem;
            ISpheroid pSpheroid = pPCS.GeographicCoordinateSystem.Datum.Spheroid;

            ISpatialReference pSpRefShp = ToSpatialReference;

            ((IGeometry)pGeo).SpatialReference = pSpRefShp;
            ((IGeometry)pGeo).Project(pSpRefPrj);
            return pGeo;
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
            IStyleGallery styleGallery = new StyleGalleryClass();
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

    }

}

