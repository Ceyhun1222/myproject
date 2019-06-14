<System.Runtime.InteropServices.ComVisibleAttribute(False)> Module NativeMethods
	Public Declare Sub InitEllipsoid Lib "MathFunctions.dll" Alias "_InitEllipsoid@16" (ByVal EquatorialRadius As Double, ByVal InverseFlattening As Double)
	Public Declare Sub InitProjection Lib "MathFunctions.dll" Alias "_InitProjection@40" (ByVal Lm0 As Double, ByVal Lp0 As Double, ByVal Sc As Double, ByVal Efalse As Double, ByVal Nfalse As Double)
	Public Declare Function Modulus Lib "MathFunctions.dll" Alias "_Modulus@16" (ByVal X As Double, Optional ByVal Y As Double = 360.0) As Double
	Public Declare Function PointAlongGeodesic Lib "MathFunctions.dll" Alias "_PointAlongGeodesic@40" (ByVal X As Double, ByVal Y As Double, ByVal Dist As Double, ByVal Azimuth As Double, ByRef resx As Double, ByRef resy As Double) As Integer
	Public Declare Function ReturnGeodesicAzimuth Lib "MathFunctions.dll" Alias "_ReturnGeodesicAzimuth@40" (ByVal X0 As Double, ByVal Y0 As Double, ByVal X1 As Double, ByVal Y1 As Double, ByRef DirectAzimuth As Double, ByRef InverseAzimuth As Double) As Integer

	Public Declare Function SetThreadLocale Lib "kernel32.dll" (ByVal dwLangID As Integer) As Boolean

	Public Declare Function HtmlHelp Lib "hhctrl.ocx" Alias "HtmlHelpA" (ByVal hwndCaller As Integer, ByVal pszFile As String, ByVal uCommand As Integer, ByVal dwData As Integer) As Integer
	'===============================================================
	'Public Declare Sub AboutBox Lib "MathFunctions.dll" Alias "_AboutBox@0" ()
	'Public Declare Sub InitAll Lib "MathFunctions.dll" Alias "_InitAll@0" ()
	'Public Declare Sub SetInverseFlattening Lib "MathFunctions.dll" Alias "_SetInverseFlattening@8" (ByVal InverseFlattening As Double)
	'Public Declare Sub SetEquatorialRadius Lib "MathFunctions.dll" Alias "_SetEquatorialRadius@8" (ByVal EquatorialRadius As Double)
	'Public Declare Function ReturnGeodesicDistance Lib "MathFunctions.dll" Alias "_ReturnGeodesicDistance@32" (ByVal X0 As Double, ByVal Y0 As Double, ByVal X1 As Double, ByVal Y1 As Double) As Double
	'Public Declare Function DistFromPointToLine Lib "MathFunctions.dll" Alias "_DistFromPointToLine@52" (ByVal xPt As Double, ByVal yPt As Double, ByVal xLn As Double, ByVal yLn As Double, ByVal Azimuth As Double, ByRef xres As Double, ByRef yres As Double, ByRef azimuthres As Double) As Double
	'Public Declare Function TriangleBy2PointAndAngle Lib "MathFunctions.dll" Alias "_TriangleBy2PointAndAngle@56" (ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal y2 As Double, ByVal Angle As Double, ByVal H As Double, ByRef xArray As Double, ByRef yArray As Double) As Integer
	'Public Declare Function ExcludeAreaCreate Lib "MathFunctions.dll" Alias "_ExcludeAreaCreate@64" (ByVal X1 As Double, ByVal Y1 As Double, ByVal phi1 As Double, ByVal X2 As Double, ByVal y2 As Double, ByVal phi2 As Double, ByVal H As Double, ByRef xArray As Double, ByRef yArray As Double) As Integer
	'Public Declare Function CreatePrevDMEOrbitalFixZone Lib "MathFunctions.dll" Alias "_CreatePrevDMEOrbitalFixZone@76" (ByVal xDME As Double, ByVal yDME As Double, ByVal xVORNDB As Double, ByVal yVORNDB As Double, ByRef Phi As Double, ByRef A As Double, ByRef H As Double, ByRef pCnt As Integer, ByRef x1Array As Double, ByRef y1Array As Double, ByRef x2Array As Double, ByRef y2Array As Double) As Integer
	'Public Declare Function DMEcircles Lib "MathFunctions.dll" Alias "_DMEcircles@60" (ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal y2 As Double, ByVal Alpha As Double, ByRef R As Double, ByRef x1res As Double, ByRef y1res As Double, ByRef x2res As Double, ByRef y2res As Double) As Integer
	'Public Declare Function CreatePrevDMEFixZone Lib "MathFunctions.dll" Alias "_CreatePrevDMEFixZone@76" (ByVal xDME As Double, ByVal yDME As Double, ByVal toleranceDME As Double, ByVal xVORNDB As Double, ByVal yVORNDB As Double, ByVal A As Double, ByVal H As Double, ByRef pCnt As Integer, ByRef x1Array As Double, ByRef y1Array As Double, ByRef x2Array As Double, ByRef y2Array As Double) As Integer
	'Public Declare Function CreateFixZone Lib "MathFunctions.dll" Alias "_CreateFixZone@80" (ByVal X1 As Double, ByVal Y1 As Double, ByVal phi1 As Double, ByVal X2 As Double, ByVal y2 As Double, ByVal phi2 As Double, ByVal H As Double, ByVal xFix As Double, ByVal yFix As Double, ByRef xArray As Double, ByRef yArray As Double) As Integer
	'Public Declare Function CreatePrevFixZone Lib "MathFunctions.dll" Alias "_CreatePrevFixZone@76" (ByVal P1x As Double, ByVal P1y As Double, ByVal P2x As Double, ByVal P2y As Double, ByVal P2phi As Double, ByVal A As Double, ByVal H As Double, ByRef pCnt As Integer, ByRef x1Array As Double, ByRef y1Array As Double, ByRef x2Array As Double, ByRef y2Array As Double) As Integer
	'Public Declare Function EnterToCircle Lib "MathFunctions.dll" Alias "_EnterToCircle@76" (ByVal X0 As Double, ByVal Y0 As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal curAzimuth As Double, ByVal flag As Integer, ByVal rRMP As Double, ByVal rTouch As Double, ByRef xTouch As Double, ByRef yTouch As Double, ByRef xTurn As Double, ByRef yTurn As Double) As Integer
	'Public Declare Function OutFromTurn Lib "MathFunctions.dll" Alias "_OutFromTurn@64" (ByVal X0 As Double, ByVal Y0 As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal Radius As Double, ByVal Azimuth As Double, ByVal flag As Integer, ByRef resx As Double, ByRef resy As Double, ByRef resAzimuth As Double) As Integer
	'Public Declare Function CalcByCourseDistance Lib "MathFunctions.dll" Alias "_CalcByCourseDistance@64" (ByVal X0 As Double, ByVal Y0 As Double, ByVal azt As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal Dist As Double, ByRef x0res As Double, ByRef y0res As Double, ByRef x1res As Double, ByRef y1res As Double) As Integer
	'Public Declare Function Calc2VectIntersect Lib "MathFunctions.dll" Alias "_Calc2VectIntersect@56" (ByVal X0 As Double, ByVal Y0 As Double, ByVal azimuth0 As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal azimuth1 As Double, ByRef resx As Double, ByRef resy As Double) As Integer
	'Public Declare Function Calc2DistIntersects Lib "MathFunctions.dll" Alias "_Calc2DistIntersects@64" (ByVal X0 As Double, ByVal Y0 As Double, ByVal dist0 As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal Dist1 As Double, ByRef xres0 As Double, ByRef yres0 As Double, ByRef xres1 As Double, ByRef yres1 As Double) As Integer
	'Public Declare Sub GeographicToProjection Lib "MathFunctions.dll" Alias "_GeographicToProjection@24" (ByVal X As Double, ByVal Y As Double, ByRef resx As Double, ByRef resy As Double)
	'Public Declare Sub ProjectionToGeographic Lib "MathFunctions.dll" Alias "_ProjectionToGeographic@24" (ByVal X As Double, ByVal Y As Double, ByRef resx As Double, ByRef resy As Double)
	'Public Declare Function GetStringFromTable Lib "Comp.dll" (ByVal hModule As Integer, ByVal uID As Integer, ByVal lpBuffer As String, ByVal nBufferMax As Integer, ByVal wLanguage As Short) As Integer
	'===============================================================

	'Public Declare Function GetThreadLocale Lib "kernel32.dll" () As Integer
	'Public Declare Function LoadLibrary Lib "kernel32.dll" Alias "LoadLibraryA" (ByVal lpLibFileName As String) As Integer
	'Public Declare Function FreeLibrary Lib "kernel32.dll" (ByVal hLibModule As Integer) As Boolean
	'===============================================================
	'Public Declare Function GetWindowLong Lib "user32.dll" Alias "GetWindowLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer
	'Public Declare Function SetWindowLong Lib "user32.dll" Alias "SetWindowLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
	'Public Declare Function GetDC Lib "user32" (ByVal hwnd As Integer) As Integer
	'Public Declare Function ReleaseDC Lib "user32" (ByVal hwnd As Integer, ByVal hdc As Integer) As Integer
	'Public Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Integer, ByVal nIndex As Integer) As Integer
End Module
