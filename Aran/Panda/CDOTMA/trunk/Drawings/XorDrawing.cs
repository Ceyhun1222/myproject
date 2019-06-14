using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CDOTMA.Drawings
{
	public static class XorDrawing
	{
		[DllImport("user32.dll", EntryPoint = "GetDC", CallingConvention = CallingConvention.StdCall)]
		private extern static IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll", EntryPoint = "ReleaseDC", CallingConvention = CallingConvention.StdCall)]
		private extern static void ReleaseDC(IntPtr intPtr, IntPtr hdc);


		[DllImport("gdi32.dll", EntryPoint = "SetROP2", CallingConvention = CallingConvention.StdCall)]
		private extern static int SetROP2(IntPtr hdc, int fnDrawMode);

		[DllImport("gdi32.dll", EntryPoint = "MoveToEx", CallingConvention = CallingConvention.StdCall)]
		private extern static bool MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

		[DllImport("gdi32.dll", EntryPoint = "LineTo", CallingConvention = CallingConvention.StdCall)]
		private extern static bool LineTo(IntPtr hdc, int x, int y);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint = "Rectangle", CallingConvention = CallingConvention.StdCall)]
		public static extern void Rectangle(IntPtr hdc, int x1, int y1, int x2, int y2);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		private static extern IntPtr GetStockObject(int brStyle);

		[DllImport("gdi32.dll", SetLastError = true)]
		static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll", EntryPoint = "SelectObject")]
		public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		static extern bool DeleteObject(IntPtr target);

		[DllImport("gdi32.dll")]
		static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		static extern IntPtr CreateSolidBrush(BrushStyle enBrushStyle, int crColor);

		[DllImport("gdi32.dll")]
		static extern bool SetWorldTransform(IntPtr hdc, [In] ref XFORM lpXform);

		[DllImport("gdi32.dll")]
		public static extern int SetGraphicsMode(IntPtr hdc, int iMode);

		const int NULL_BRUSH = 5;

		/// <summary>
		///   The XFORM structure specifies a world-space to page-space transformation.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct XFORM
		{
			public float eM11;
			public float eM12;
			public float eM21;
			public float eM22;
			public float eDx;
			public float eDy;

			public XFORM(float eM11, float eM12, float eM21, float eM22, float eDx, float eDy)
			{
				this.eM11 = eM11;
				this.eM12 = eM12;
				this.eM21 = eM21;
				this.eM22 = eM22;
				this.eDx = eDx;
				this.eDy = eDy;
			}

			/// <summary>
			///   Allows implicit converstion to a managed transformation matrix.
			/// </summary>
			public static implicit operator System.Drawing.Drawing2D.Matrix(XFORM xf)
			{
				return new System.Drawing.Drawing2D.Matrix(xf.eM11, xf.eM12, xf.eM21, xf.eM22, xf.eDx, xf.eDy);
			}

			/// <summary>
			///   Allows implicit converstion from a managed transformation matrix.
			/// </summary>
			public static implicit operator XFORM(System.Drawing.Drawing2D.Matrix m)
			{
				float[] elems = m.Elements;
				return new XFORM(elems[0], elems[1], elems[2], elems[3], elems[4], elems[5]);
			}
		}

		public enum BinaryRasterOperations
		{
			R2_BLACK = 1,
			R2_NOTMERGEPEN = 2,
			R2_MASKNOTPEN = 3,
			R2_NOTCOPYPEN = 4,
			R2_MASKPENNOT = 5,
			R2_NOT = 6,
			R2_XORPEN = 7,
			R2_NOTMASKPEN = 8,
			R2_MASKPEN = 9,
			R2_NOTXORPEN = 10,
			R2_NOP = 11,
			R2_MERGENOTPEN = 12,
			R2_COPYPEN = 13,
			R2_MERGEPENNOT = 14,
			R2_MERGEPEN = 15,
			R2_WHITE = 16
		}

		private enum PenStyle : int
		{
			PS_SOLID = 0,		//The pen is solid.
			PS_DASH = 1,		//The pen is dashed.
			PS_DOT = 2,			//The pen is dotted.
			PS_DASHDOT = 3,		//The pen has alternating dashes and dots.
			PS_DASHDOTDOT = 4,	//The pen has alternating dashes and double dots.
			PS_NULL = 5,		//The pen is invisible.
			PS_INSIDEFRAME = 6,	// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
			// outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn 
			// completely inside the outer edge of the shape.
			PS_USERSTYLE = 7,
			PS_ALTERNATE = 8,
			PS_STYLE_MASK = 0x0000000F,

			PS_ENDCAP_ROUND = 0x00000000,
			PS_ENDCAP_SQUARE = 0x00000100,
			PS_ENDCAP_FLAT = 0x00000200,
			PS_ENDCAP_MASK = 0x00000F00,

			PS_JOIN_ROUND = 0x00000000,
			PS_JOIN_BEVEL = 0x00001000,
			PS_JOIN_MITER = 0x00002000,
			PS_JOIN_MASK = 0x0000F000,

			PS_COSMETIC = 0x00000000,
			PS_GEOMETRIC = 0x00010000,
			PS_TYPE_MASK = 0x000F0000
		};

		public enum BrushStyle : int
		{
			BS_SOLID = 0,
			BS_NULL = 1,
			BS_HATCHED = 2,
			BS_PATTERN = 3,
			BS_INDEXED = 4,
			BS_DIBPATTERN = 5,
			BS_DIBPATTERNPT = 6,
			BS_PATTERN8X8 = 7,
			BS_MONOPATTERN = 9
		}

		public enum GraphicsMode : int
		{
			GM_COMPATIBLE = 1,
			GM_ADVANCED = 2,
		}

		//private static IntPtr BeginDraw(System.Drawing.Bitmap bmp, System.Drawing.Graphics graphics, bool dash, out int oldRop, out IntPtr img, out IntPtr oldpen)
		//{
		//    var gHdc = graphics.GetHdc();
		//    var hdc = CreateCompatibleDC(gHdc);
		//    graphics.ReleaseHdc(hdc);

		//    img = bmp.GetHbitmap();
		//    SelectObject(hdc, img);

		//    oldpen = IntPtr.Zero;
		//    if (dash)
		//    {
		//        var pen = CreatePen(PenStyle.PS_DASH, 1, 0);
		//        oldpen = SelectObject(hdc, pen);
		//    }
		//    oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_NOTXORPEN);	// Switch to inverted mode. (XOR)

		//    SetGraphicsMode(hdc, (int)GraphicsMode.GM_ADVANCED);
		//    XFORM transform = graphics.Transform;
		//    SetWorldTransform(hdc, ref transform);

		//    return hdc;
		//}

		//private static void FinishDraw(System.Drawing.Graphics graphics, IntPtr hdc, IntPtr oldpen, int oldRop, IntPtr img, bool dash)
		//{
		//    SetROP2(hdc, oldRop);

		//    var transform = graphics.Transform;
		//    graphics.ResetTransform();					//in case there is transform
		//    var outBmp = System.Drawing.Image.FromHbitmap(img);

		//    //CopyChannel(bmp, outBmp, ChannelARGB.Alpha, ChannelARGB.Alpha);
		//    graphics.Clear(Color.Transparent);
		//    graphics.DrawImage(outBmp, 0, 0);			//draw the xored image on the bitmap
		//    graphics.Transform = transform;

		//    if (dash)
		//        DeleteObject(SelectObject(hdc, oldpen));	//delete new pen (switch to oldpen)
		//    DeleteObject(img);								// Delete the GDI bitmap (important).
		//    DeleteObject(hdc);
		//}

		//public static void DrawXorLine(this System.Drawing.Graphics graphics, System.Drawing.Bitmap bmp, int x1, int y1, int x2, int y2, bool dash = true)
		//{
		//    int oldRop;
		//    IntPtr oldpen, img;
		//    var hdc = BeginDraw(bmp, graphics, dash, out oldRop, out img, out oldpen);

		//    MoveToEx(hdc, x1, y1, IntPtr.Zero);
		//    LineTo(hdc, x2, y2);

		//    FinishDraw(graphics, hdc, oldpen, oldRop, img, dash);
		//}

		//public static void DrawXorRectangle(this System.Drawing.Graphics graphics, System.Drawing.Bitmap bmp, int x1, int y1, int x2, int y2, bool dash = true)
		//{
		//    int oldRop;
		//    IntPtr oldpen, img;
		//    var hdc = BeginDraw(bmp, graphics, dash, out oldRop, out img, out oldpen);

		//    MoveToEx(hdc, x1, y1, IntPtr.Zero);		//clockwise
		//    LineTo(hdc, x2, y1);
		//    LineTo(hdc, x2, y2);
		//    LineTo(hdc, x1, y2);
		//    LineTo(hdc, x1, y1);

		//    FinishDraw(graphics, hdc, oldpen, oldRop, img, dash);
		//}

		//public static void DrawXorRectangle(IntPtr hwnd, int x1, int y1, int x2, int y2)
		//{
		//    IntPtr hdc = GetDC(hwnd);
		//    IntPtr pen = CreatePen(PenStyle.PS_DOT, 1, 0);

		//    IntPtr oldPen = SelectObject(hdc, pen);
		//    int oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_XORPEN);

		//    IntPtr oldBrush = SelectObject(hdc, GetStockObject(NULL_BRUSH));

		//    Rectangle(hdc, x1, y1, x2, y2);

		//    SetROP2(hdc, oldRop);						// Switch to previos mode.
		//    DeleteObject(SelectObject(hdc, oldPen));	//delete new pen (switch to oldpen)
		//    SelectObject(hdc, oldBrush);				// no need to delete a stock object

		//    ReleaseDC(hwnd, hdc);
		//}

		//public static void test(IntPtr hwnd, int x1, int y1, int x2, int y2)
		//{
		//    IntPtr hdc = GetDC(hwnd);	// Get device context.
		//    int oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_NOT);		// Switch to inverted mode. (XOR)
		//    MoveToEx(hdc, x1, y1, IntPtr.Zero);
		//    LineTo(hdc, x2, y2);

		//    SetROP2(hdc, oldRop);		// Switch to previos mode.

		//    ReleaseDC(IntPtr.Zero, hdc); // Release device context.
		//}

		//public static void test1(IntPtr hwnd, int x1, int y1, int x2, int y2)
		//{
		//    IntPtr hdc = GetDC(hwnd);	// Get device context.
		//    IntPtr pen = CreatePen(PenStyle.PS_DASH, 1, (uint)Functions.RGB(255, 255, 255));

		//    int oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_NOT);		// Switch to inverted mode. (XOR)
		//    IntPtr oldPen = SelectObject(hdc, pen);

		//    MoveToEx(hdc, x1, y1, IntPtr.Zero); //clockwise
		//    LineTo(hdc, x2, y1);
		//    LineTo(hdc, x2, y2);
		//    LineTo(hdc, x1, y2);
		//    LineTo(hdc, x1, y1);

		//    SetROP2(hdc, oldRop);		// Switch to previos mode.

		//    DeleteObject(SelectObject(hdc, oldPen)); //delete new pen (switch to oldpen)
		//    ReleaseDC(hwnd, hdc); // Release device context.
		//    //DeleteObject(hdc);
		//}

		public static void DrawXorLine(IntPtr hdc, int x1, int y1, int x2, int y2, bool dash = true)
		{
			int oldRop;

			var pen = CreatePen(PenStyle.PS_DASH, 1, 0);
			IntPtr oldpen = SelectObject(hdc, pen);
			oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_NOTXORPEN);	// Switch to inverted mode. (XOR)

			MoveToEx(hdc, x1, y1, IntPtr.Zero);
			LineTo(hdc, x2, y2);

			    SetROP2(hdc, oldRop);

			DeleteObject(SelectObject(hdc, oldpen));	//delete new pen (switch to oldpen)
		}

		public static IntPtr BeginDraw(IntPtr hwnd, out int oldRop, out IntPtr oldpen)
		{
			IntPtr hdc = GetDC(hwnd);
			IntPtr pen = CreatePen(PenStyle.PS_DOT, 1, 0);

			oldpen = SelectObject(hdc, pen);
			oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_XORPEN);
			IntPtr oldBrush = SelectObject(hdc, GetStockObject(NULL_BRUSH));

			return hdc;
		}

		public static void FinishDraw(IntPtr hwnd, IntPtr hdc, int oldRop, IntPtr oldpen)
		{
			SetROP2(hdc, oldRop);
			DeleteObject(SelectObject(hdc, oldpen));	//delete new pen (switch to oldpen)
			ReleaseDC(hwnd, hdc);						// Release device context.
			//DeleteObject(hdc);
		}

	}
}
