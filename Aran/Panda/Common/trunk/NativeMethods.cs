using System;
using System.Runtime.InteropServices;
using Aran.Geometries;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class NativeMethods
	{
		#region MathFunctions.dll

		[DllImport("MathFunctions.dll", EntryPoint = "_InitAll@0")]
		public static extern void InitAll();

		[DllImport("MathFunctions.dll", EntryPoint = "_InitEllipsoid@16")]
		public static extern void InitEllipsoid(double EquatorialRadius, double InverseFlattening);

		[DllImport("MathFunctions.dll", EntryPoint = "_SetInverseFlattening@8")]
		public static extern void SetInverseFlattening(double InverseFlattening);

		[DllImport("MathFunctions.dll", EntryPoint = "_SetEquatorialRadius@8")]
		public static extern void SetEquatorialRadius(double EquatorialRadius);

		[DllImport("MathFunctions.dll", EntryPoint = "_InitProjection@40")]
		public static extern void InitProjection(double Lm0, double Lp0, double Sc, double Efalse, double Nfalse);

		[DllImport("MathFunctions.dll", EntryPoint = "_ATan2@16")]
		public static extern double ATan2(double Y, double X);

		[DllImport("MathFunctions.dll", EntryPoint = "_Modulus@16")]
		public static extern double Modulus(double X, double Y = 360.0);

		[DllImport("MathFunctions.dll", EntryPoint = "_ReturnGeodesicDistance@32", CallingConvention = CallingConvention.StdCall)]
		public static extern double ReturnGeodesicDistance(double X0, double Y0, double X1, double Y1);

		[DllImport("MathFunctions.dll", EntryPoint = "_ReturnGeodesicAzimuth@40")]
		public static extern int ReturnGeodesicAzimuth(double X0, double y0, double X1, double Y1, out double DirectAzimuth, out double InverseAzimuth);

		[DllImport("MathFunctions.dll", EntryPoint = "_InverseProblem@40")]
		public static extern int InverseProblem(double X0, double y0, double X1, double Y1, out double DirectAzimuth, out double InverseAzimuth);

		[DllImport("MathFunctions.dll", EntryPoint = "_DistFromPointToLine@52",CallingConvention=CallingConvention.StdCall)]
		public static extern double DistFromPointToLine(double xPt, double yPt, double xLn, double yLn, double Azimuth, ref double xres, ref double yres, ref double azimuthres);

		[DllImport("MathFunctions.dll", EntryPoint = "_TriangleBy2PointAndAngle@56")]
		public static extern int TriangleBy2PointAndAngle(double X1, double Y1, double X2, double Y2, double Angle, double h, ref double xArray, ref double yArray);

		[DllImport("MathFunctions.dll", EntryPoint = "_DMEcircles@60")]
		public static extern int DMEcircles(double X1, double Y1, double X2, double Y2, double alpha, ref double R, ref double x1res, ref double y1res, ref double x2res, ref double y2res);

		[DllImport("MathFunctions.dll", EntryPoint = "_EnterToCircle@76")]
		public static extern int EnterToCircle(double X0, double y0, double X1, double Y1, double curAzimuth, int flag, double rRMP, double rTouch, ref double xTouch, ref double yTouch, ref double xTurn, ref double yTurn);

		[DllImport("MathFunctions.dll", EntryPoint = "_OutFromTurn@64")]
		public static extern int OutFromTurn(double X0, double y0, double X1, double Y1, double Radius, double Azimuth, int flag, ref double ResX, ref double ResY, ref double resAzimuth);

		[DllImport("MathFunctions.dll", EntryPoint = "_PointAlongGeodesic@40")]
		public static extern int PointAlongGeodesic(double X, double Y, double Dist, double Azimuth, out double ResX, out double ResY);

		[DllImport("MathFunctions.dll", EntryPoint = "_GeographicToProjection@24")]
		public static extern void GeographicToProjection(double X, double Y, out double ResX, out double ResY);

		[DllImport("MathFunctions.dll", EntryPoint = "_ProjectionToGeographic@24")]
		public static extern void ProjectionToGeographic(double X, double Y, out double ResX, out double ResY);

		[DllImport("MathFunctions.dll", EntryPoint = "_ShowPandaBox@4")]
		public static extern void ShowPandaBox(int hWnd);

		[DllImport("MathFunctions.dll", EntryPoint = "_HidePandaBox@0")]
		public static extern void HidePandaBox();

        //[DllImport("MathFunctions.dll", EntryPoint = "_DistFromPointToLine@52", CallingConvention = CallingConvention.StdCall)]
        //public static extern double DistFromPointToLine(double xPt, double yPt, double xLn, double yLn, double azimuth, double xRes, double yRes, double azimuthres);

		[DllImport("MathFunctions.dll", EntryPoint = "_CalcByCourseDistance@64", CallingConvention = CallingConvention.StdCall)]
		public static extern System.Int32 CalcByCourseDistance(double x0, double y0, double azt, double x1, double y1, double dist, out double x0res, out double y0res, out double x1res, out double y1res);

		[DllImport("MathFunctions.dll", EntryPoint = "_Calc2VectIntersect@56",CallingConvention=CallingConvention.StdCall)]
		public static extern System.Int32 Calc2VectIntersect(double x0, double y0, double azimuth0, double x1, double y1, double Azimuth1,out double resx,out double resy);

		[DllImport("MathFunctions.dll", EntryPoint = "_Calc2DistIntersects@64",CallingConvention=CallingConvention.StdCall)]
		public static extern System.Int32 Calc2DistIntersects(double x0, double y0, double dist0, double x1, double y1, double dist1, out double xres0,out double yres0,out double xres1,out double yres1);

		public static double ReturnGeodesicAzimuth(Point p0, Point p1)
		{
			double result, fTmp;
			ReturnGeodesicAzimuth(p0.X, p0.Y, p1.X, p1.Y, out result, out fTmp);
			return result;
		}

		#endregion

		[DllImport("hhctrl.ocx", EntryPoint = "HtmlHelpA", SetLastError = true)]
		public static extern int HtmlHelp(int hwndCaller, string pszFile, int uCommand, int dwData);

		#region gdi32	^^^ neye lazimdir ki ??????
		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		public static extern bool DeleteObject(IntPtr hObject);
		#endregion

		#region ThreadLocale
		[DllImport("kernel32.dll")]
		public static extern bool SetThreadLocale(int dwLangID);
		[DllImport("kernel32.dll")]
		public static extern int GetThreadLocale();
		#endregion

		#region WindowLong32

		public const Int32 GWL_WNDPROC = -4;
		public const Int32 GWL_HINSTANCE = -6;
		public const Int32 GWL_HWNDPARENT = -8;
		public const Int32 GWL_STYLE = -16;
		public const Int32 GWL_EXSTYLE = -20;
		public const Int32 GWL_USERDATA = -21;
		public const Int32 GWL_ID = -12;

		[DllImport("user32.dll", EntryPoint = "GetWindowLongA", CallingConvention = CallingConvention.StdCall)]
		public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongA", CallingConvention = CallingConvention.StdCall)]
		public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 NewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongA", CallingConvention = CallingConvention.StdCall)]
		public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, IntPtr NewLong);
		#endregion
	}
}

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/