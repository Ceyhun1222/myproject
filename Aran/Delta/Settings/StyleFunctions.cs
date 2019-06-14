using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Display;
using System.Drawing;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using stdole;

namespace Aran.Delta.Settings
{
    internal static class StyleFunctions
    {
        public static ISymbol GetSymbol (
            string style, string classname, int id)
        {
            ISymbol symbol = null;
            IStyleGallery styleGallery = new StyleGallery ();
            IStyleGalleryStorage styleGalleryStorage =
                (IStyleGalleryStorage) styleGallery;
            styleGalleryStorage.TargetFile = style;
            IEnumStyleGalleryItem styleGalleryItems =
                styleGallery.get_Items (classname, style, "");
            styleGalleryItems.Reset ();
            IStyleGalleryItem styleGalleryItem = styleGalleryItems.Next ();
            while (styleGalleryItem != null)
            {
                if (styleGalleryItem.ID == id)
                {
                    symbol = (ISymbol) styleGalleryItem.Item;
                    break;
                }
                styleGalleryItem = styleGalleryItems.Next ();
            }
            styleGalleryItem = null;
            styleGalleryStorage = null;
            styleGallery = null;

            return symbol;
        }

        private static IPictureDisp CreatePictureFromSymbol (
            IntPtr hDCOld, ref IntPtr hBmpNew,
            ISymbol pSymbol, Size size, int lGap, int backColor)
        {
            IntPtr hDCNew = IntPtr.Zero;
            IntPtr hBmpOld = IntPtr.Zero;
            try
            {
                hDCNew = WindowsAPI.CreateCompatibleDC (hDCOld);
                hBmpNew = WindowsAPI.CreateCompatibleBitmap (
                    hDCOld, size.Width, size.Height);
                hBmpOld = WindowsAPI.SelectObject (hDCNew, hBmpNew);

                // Draw the symbol to the new device context.
                bool lResult = DrawToDC (
                    hDCNew, size, pSymbol, lGap, backColor);

                hBmpNew = WindowsAPI.SelectObject (hDCNew, hBmpOld);
                WindowsAPI.DeleteDC (hDCNew);

                return null;

                // Return the Bitmap as an OLE Picture.

                //return CreatePictureFromBitmap (hBmpNew);
            }
            catch (Exception error)
            {
                if (pSymbol != null)
                {
                    pSymbol.ResetDC ();
                    if ((hBmpNew != IntPtr.Zero) && (hDCNew != IntPtr.Zero)
                        && (hBmpOld != IntPtr.Zero))
                    {
                        hBmpNew = WindowsAPI.SelectObject (hDCNew, hBmpOld);
                        WindowsAPI.DeleteDC (hDCNew);
                    }
                }

                throw error;
            }
        }

        private static IPictureDisp CreatePictureFromBitmap (IntPtr hBmpNew)
        {
            try
            {
                Guid iidIPicture =
                    new Guid ("7BF80980-BF32-101A-8BBB-00AA00300CAB");

                WindowsAPI.PICTDESC picDesc = new WindowsAPI.PICTDESC ();
                picDesc.cbSizeOfStruct = Marshal.SizeOf (picDesc);
                picDesc.picType = (int) WindowsAPI.PictureTypeConstants.picTypeBitmap;
                picDesc.hPic = (IntPtr) hBmpNew;
                picDesc.hpal = IntPtr.Zero;

                // Create Picture object.
                IPictureDisp newPic;
                WindowsAPI.OleCreatePictureIndirect (
                    ref picDesc, ref iidIPicture, true, out newPic);

                // Return the new Picture object.
                return newPic;
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        private static bool DrawToWnd (IntPtr hWnd, ISymbol pSymbol,
            int lGap, int backColor)
        {
            IntPtr hDC = IntPtr.Zero;
            try
            {
                if (hWnd != IntPtr.Zero)
                {
                    // Calculate size of window.
                    WindowsAPI.RECT udtRect = new WindowsAPI.RECT ();
                    int lResult = WindowsAPI.GetClientRect (hWnd, ref udtRect);

                    if (lResult != 0)
                    {
                        int lWidth = (udtRect.Right - udtRect.Left);
                        int lHeight = (udtRect.Bottom - udtRect.Top);

                        hDC = WindowsAPI.GetDC (hWnd);
                        // Must release the DC afterwards.
                        if (hDC != IntPtr.Zero)
                        {
                            bool ok = DrawToDC (hDC, new Size (lWidth, lHeight)
                            , pSymbol, lGap, backColor);

                            // Release cached DC obtained with GetDC.
                            WindowsAPI.ReleaseDC (hWnd, hDC);

                            return ok;
                        }
                    }
                }
            }
            catch
            {
                if (pSymbol != null)
                {
                    // Try resetting DC
                    pSymbol.ResetDC ();

                    if ((hWnd != IntPtr.Zero) && (hDC != IntPtr.Zero))
                    {
                        WindowsAPI.ReleaseDC (hWnd, hDC); // Try to release cached DC
                    }
                }
                return false;
            }
            return true;
        }

        private static bool DrawToDC (IntPtr hDC, Size size,
            ISymbol pSymbol, int lGap, int backColor)
        {
            try
            {
                if (hDC != IntPtr.Zero)
                {
                    // First clear the existing device context.
                    if (!Clear (hDC, backColor, 0, 0,
                        size.Width, size.Height))
                    {
                        throw new Exception (
                            "Could not clear the Device Context.");
                    }

                    // Create the Transformation and Geometry
                    // required by ISymbol::Draw.
                    ITransformation pTransformation = CreateTransFromDC (
                        hDC, size.Width, size.Height);
                    IEnvelope pEnvelope = new Envelope () as IEnvelope;
                    pEnvelope.PutCoords (lGap, lGap, size.Width - lGap,
                        size.Height - lGap);
                    IGeometry pGeom = CreateSymShape (pSymbol, pEnvelope);

                    // Perform the Draw operation.
                    if ((pTransformation != null) && (pGeom != null))
                    {
                        pSymbol.SetupDC (hDC.ToInt32 (), pTransformation);
                        pSymbol.Draw (pGeom);
                        pSymbol.ResetDC ();
                    }
                    else
                    {
                        throw new Exception ("Could not create required" +
                            "Transformation or Geometry.");
                    }
                }
            }
            catch
            {
                if (pSymbol != null)
                {
                    pSymbol.ResetDC ();
                }
                return false;
            }

            return true;
        }

        private static bool Clear (IntPtr hDC, int backgroundColor,
            int xmin, int ymin, int xmax, int ymax)
        {
            IntPtr hBrushBackground = IntPtr.Zero;
            int lResult;
            bool ok;

            try
            {
                WindowsAPI.RECT udtBounds;
                udtBounds.Left = xmin;
                udtBounds.Top = ymin;
                udtBounds.Right = xmax;
                udtBounds.Bottom = ymax;

                hBrushBackground = WindowsAPI.CreateSolidBrush (backgroundColor);
                if (hBrushBackground == IntPtr.Zero)
                {
                    throw new Exception ("Could not create GDI Brush.");
                }
                lResult = WindowsAPI.FillRect (hDC, ref udtBounds, hBrushBackground);
                if (hBrushBackground == IntPtr.Zero)
                {
                    throw new Exception ("Could not fill Device Context.");
                }
                ok = WindowsAPI.DeleteObject (hBrushBackground);
                if (hBrushBackground == IntPtr.Zero)
                {
                    throw new Exception ("Could not delete GDI Brush.");
                }
            }
            catch
            {
                if (hBrushBackground != IntPtr.Zero)
                {
                    ok = WindowsAPI.DeleteObject (hBrushBackground);
                }
                return false;
            }

            return true;
        }

        private static ITransformation CreateTransFromDC (IntPtr hDC,
            int lWidth, int lHeight)
        {
            // Calculate the parameters for the new transformation,
            // based on the dimensions passed to this function.
            try
            {
                IEnvelope pBoundsEnvelope = new Envelope () as IEnvelope;
                pBoundsEnvelope.PutCoords (0.0, 0.0, (double) lWidth,
                    (double) lHeight);

                tagRECT deviceRect;
                deviceRect.left = 0;
                deviceRect.top = 0;
                deviceRect.right = lWidth;
                deviceRect.bottom = lHeight;

                int dpi = WindowsAPI.GetDeviceCaps (hDC, WindowsAPI.LOGPIXELSY);
                if (dpi == 0)
                {
                    throw new Exception (
                      "Could not retrieve Resolution from device context.");
                }

                // Create a new display transformation and set its properties
                IDisplayTransformation newTrans =
                    new DisplayTransformation () as IDisplayTransformation;
                newTrans.VisibleBounds = pBoundsEnvelope;
                newTrans.Bounds = pBoundsEnvelope;
                newTrans.set_DeviceFrame (ref deviceRect);
                newTrans.Resolution = dpi;

                return newTrans;
            }
            catch
            {
                return null;
            }
        }

        private static IGeometry CreateSymShape (ISymbol pSymbol,
            IEnvelope pEnvelope)
        {

            try
            {
                if (pSymbol is IMarkerSymbol)
                {
                    // For a MarkerSymbol return a Point.
                    IArea pArea = (IArea) pEnvelope;
                    return pArea.Centroid;
                }
                else if ((pSymbol is ILineSymbol) ||
                    (pSymbol is ITextSymbol))
                {
                    // For a LineSymbol or TextSymbol return a Polyline.
                    IPolyline pPolyline = new Polyline () as IPolyline;
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

        public static Bitmap SymbolToBitmap (ISymbol userSymbol, Size size,
            Graphics gr, int backColor)
        {
            IntPtr graphicsHdc =  gr.GetHdc();
            IntPtr hBitmap = IntPtr.Zero;
            IPictureDisp newPic = CreatePictureFromSymbol (
                graphicsHdc, ref hBitmap, userSymbol, size, 1, backColor);
            Bitmap newBitmap = Bitmap.FromHbitmap (hBitmap);
           gr.ReleaseHdc (graphicsHdc);

            return newBitmap;
        }
    }
}
