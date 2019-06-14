Option Strict Off
Option Explicit On

Module Navaids_DataBase

	Public Const TACANDME As Short = 128
	Public Const TACANVOR As Short = 256

	Public NavTypeNames() As String = {"VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX", "MKR"}

	Structure VORData
		Dim Range As Double
		Dim FA_Range As Double
		Dim InitWidth As Double
		Dim SplayAngle As Double
		Dim TrackingTolerance As Double
		Dim IntersectingTolerance As Double
		Dim ConeAngle As Double
		Dim TrackAccuracy As Double
		Dim LateralDeviationCoef As Double
		Dim EnRouteTrackingToler As Double
		Dim EnRouteTracking2Toler As Double
		Dim EnRouteInterToler As Double
		Dim EnRoutePrimAreaWith As Double
		Dim EnRouteSecAreaWith As Double
		Dim OnNAVRadius As Double
	End Structure
	
	Structure NDBData
		Dim Range As Double
		Dim FA_Range As Double
		Dim InitWidth As Double
		Dim SplayAngle As Double
		Dim TrackingTolerance As Double
		Dim IntersectingTolerance As Double
		Dim ConeAngle As Double
		Dim TrackAccuracy As Double
		Dim Entry2ConeAccuracy As Double
		Dim LateralDeviationCoef As Double
		Dim EnRouteTrackingToler As Double
		Dim EnRouteTracking2Toler As Double
		Dim EnRouteInterToler As Double
		Dim EnRoutePrimAreaWith As Double
		Dim EnRouteSecAreaWith As Double
		Dim OnNAVRadius As Double
	End Structure
	
	Structure DMEData
		Dim Range As Double
		Dim MinimalError As Double
		Dim ErrorScalingUp As Double
		Dim SlantAngle As Double
		Dim TP_div As Double
	End Structure
	
	Structure LLZData
		Dim Range As Double
		Dim TrackingTolerance As Double
		Dim IntersectingTolerance As Double
	End Structure

	Structure TACANData
		Dim DMECon As DMEData
		Dim VORCon As VORData
	End Structure

	Public NavaidTypesCount As Integer
	Public NavaidTypes As ESRI.ArcGIS.Geodatabase.ITable
	Public Navaids() As ESRI.ArcGIS.Geodatabase.ITable

	Public VOR As VORData
	Public NDB As NDBData
	Public DME As DMEData
	Public LLZ As LLZData
	Public TACAN As TACANData

	Function InitModule() As Boolean
		Dim I As Integer
		Dim N As Integer
		Dim J As Integer
		Dim iBaseName As Integer
		Dim iNavTypeName As Integer
		Dim iParam_Name As Integer
		Dim iValue As Integer
		Dim iUnit As Integer
		Dim iMultiplier As Integer
		
		Dim Value As Double
		Dim Multiplier As Double
		Dim ParName As String
		
		Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = Nothing '		Dim pField As ESRI.ArcGIS.Geodatabase.IField
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

		On Error GoTo EH

		If Not OpenTableFromFile(NavaidTypes, ConstDir + "\Navaids\", "Navaids") Then Return False

		iBaseName = NavaidTypes.FindField("BaseName")
		iNavTypeName = NavaidTypes.FindField("Name")

		NavaidTypesCount = NavaidTypes.RowCount(Nothing)
		ReDim Navaids(NavaidTypesCount - 1)		'FillNameAndIndex()

		For J = 0 To NavaidTypesCount - 1
			If Not OpenTableFromFile(pTable, ConstDir + "\Navaids\", NavaidTypes.GetRow(J).Value(iBaseName)) Then Return False

			Navaids(J) = pTable

			Select Case NavaidTypes.GetRow(J).Value(iNavTypeName)
				'============================ VOR ====================================
				Case "VOR"
					iParam_Name = -1
					iValue = -1
					iUnit = -1
					iMultiplier = -1

					For I = 0 To Navaids(J).Fields.FieldCount - 1
						If Navaids(J).Fields.Field(I).Name = "PARAM_NAME" Then iParam_Name = I
						If Navaids(J).Fields.Field(I).Name = "VALUE" Then iValue = I
						If Navaids(J).Fields.Field(I).Name = "UNIT" Then iUnit = I
						If Navaids(J).Fields.Field(I).Name = "MULTIPLIER" Then iMultiplier = I
					Next I

					N = Navaids(J).RowCount(Nothing)

					For I = 0 To N - 1
						pRow = Navaids(J).GetRow(I)
						ParName = pRow.Value(iParam_Name)
						Multiplier = pRow.Value(iMultiplier)
						Value = pRow.Value(iValue) * Multiplier
						If (pRow.Value(iUnit) = "rad") Or (pRow.Value(iUnit) = "°") Then Value = System.Math.Round(RadToDeg(Value), 1)

						If ParName = "Range" Then VOR.Range = Value
						If ParName = "FA Range" Then VOR.FA_Range = Value
						If ParName = "Initial width" Then VOR.InitWidth = Value
						If ParName = "Splay angle" Then VOR.SplayAngle = Value
						If ParName = "Tracking tolerance" Then VOR.TrackingTolerance = Value
						If ParName = "Intersecting tolerance" Then VOR.IntersectingTolerance = Value
						If ParName = "Cone angle" Then VOR.ConeAngle = Value
						If ParName = "Track accuracy" Then VOR.TrackAccuracy = Value
						If ParName = "Lateral deviation coef." Then VOR.LateralDeviationCoef = Value
						If ParName = "EnRoute Tracking toler" Then VOR.EnRouteTrackingToler = Value
						If ParName = "EnRoute Tracking2 toler" Then VOR.EnRouteTracking2Toler = Value
						If ParName = "EnRoute Inter toler" Then VOR.EnRouteInterToler = Value
						If ParName = "EnRoute PrimArea With" Then VOR.EnRoutePrimAreaWith = Value
						If ParName = "EnRoute SecArea With" Then VOR.EnRouteSecAreaWith = Value
					Next I
					'============================ NDB ====================================
				Case "NDB"
					iParam_Name = -1
					iValue = -1
					iUnit = -1
					iMultiplier = -1

					For I = 0 To Navaids(J).Fields.FieldCount - 1
						If Navaids(J).Fields.Field(I).Name = "PARAM_NAME" Then iParam_Name = I
						If Navaids(J).Fields.Field(I).Name = "VALUE" Then iValue = I
						If Navaids(J).Fields.Field(I).Name = "UNIT" Then iUnit = I
						If Navaids(J).Fields.Field(I).Name = "MULTIPLIER" Then iMultiplier = I
					Next I

					N = Navaids(J).RowCount(Nothing)

					For I = 0 To N - 1
						pRow = Navaids(J).GetRow(I)
						ParName = pRow.Value(iParam_Name)
						Multiplier = pRow.Value(iMultiplier)
						Value = pRow.Value(iValue) * Multiplier
						If (pRow.Value(iUnit) = "rad") Or (pRow.Value(iUnit) = "°") Then Value = System.Math.Round(RadToDeg(Value), 1)

						If ParName = "Range" Then NDB.Range = Value
						If ParName = "FA Range" Then NDB.FA_Range = Value
						If ParName = "Initial width" Then NDB.InitWidth = Value
						If ParName = "Splay angle" Then NDB.SplayAngle = Value
						If ParName = "Tracking tolerance" Then NDB.TrackingTolerance = Value
						If ParName = "Intersecting tolerance" Then NDB.IntersectingTolerance = Value
						If ParName = "Cone angle" Then NDB.ConeAngle = Value
						If ParName = "Track accuracy" Then NDB.TrackAccuracy = Value
						If ParName = "Entry into the cone accuracy" Then NDB.Entry2ConeAccuracy = Value
						If ParName = "Lateral deviation coef." Then NDB.LateralDeviationCoef = Value
						If ParName = "EnRoute Tracking toler" Then NDB.EnRouteTrackingToler = Value
						If ParName = "EnRoute Tracking2 toler" Then NDB.EnRouteTracking2Toler = Value
						If ParName = "EnRoute Inter toler" Then NDB.EnRouteInterToler = Value
						If ParName = "EnRoute PrimArea With" Then NDB.EnRoutePrimAreaWith = Value
						If ParName = "EnRoute SecArea With" Then NDB.EnRouteSecAreaWith = Value
					Next I
					'============================ DME ====================================
				Case "DME"
					iParam_Name = -1
					iValue = -1
					iUnit = -1
					iMultiplier = -1

					For I = 0 To Navaids(J).Fields.FieldCount - 1
						If Navaids(J).Fields.Field(I).Name = "PARAM_NAME" Then iParam_Name = I
						If Navaids(J).Fields.Field(I).Name = "VALUE" Then iValue = I
						If Navaids(J).Fields.Field(I).Name = "UNIT" Then iUnit = I
						If Navaids(J).Fields.Field(I).Name = "MULTIPLIER" Then iMultiplier = I
					Next I

					N = Navaids(J).RowCount(Nothing)

					For I = 0 To N - 1
						pRow = Navaids(J).GetRow(I)
						ParName = pRow.Value(iParam_Name)
						Multiplier = pRow.Value(iMultiplier)
						Value = pRow.Value(iValue) * Multiplier
						If (pRow.Value(iUnit) = "rad") Or (pRow.Value(iUnit) = "°") Then Value = Math.Round(RadToDeg(Value), 2)

						If ParName = "Range" Then DME.Range = Value
						If ParName = "Minimal Error" Then DME.MinimalError = Value
						If ParName = "Error Scaling Up" Then DME.ErrorScalingUp = Value
						If ParName = "Slant Angle" Then DME.SlantAngle = Value
						If ParName = "TP_div" Then DME.TP_div = Value
					Next I
					'============================ LLZ ====================================
				Case "LLZ"
					iParam_Name = -1
					iValue = -1
					iUnit = -1
					iMultiplier = -1

					For I = 0 To Navaids(J).Fields.FieldCount - 1
						If Navaids(J).Fields.Field(I).Name = "PARAM_NAME" Then iParam_Name = I
						If Navaids(J).Fields.Field(I).Name = "VALUE" Then iValue = I
						If Navaids(J).Fields.Field(I).Name = "UNIT" Then iUnit = I
						If Navaids(J).Fields.Field(I).Name = "MULTIPLIER" Then iMultiplier = I
					Next I

					N = Navaids(J).RowCount(Nothing)

					For I = 0 To N - 1
						pRow = Navaids(J).GetRow(I)
						ParName = pRow.Value(iParam_Name)
						Multiplier = pRow.Value(iMultiplier)
						Value = pRow.Value(iValue) * Multiplier
						If (pRow.Value(iUnit) = "rad") Or (pRow.Value(iUnit) = "°") Then Value = Math.Round(RadToDeg(Value), 2)

						If ParName = "Range" Then LLZ.Range = Value
						If ParName = "Tracking tolerance" Then LLZ.TrackingTolerance = Value
						If ParName = "Intersecting tolerance" Then LLZ.IntersectingTolerance = Value
					Next I
					'============================ FIN ====================================
			End Select
		Next J

		TACAN.DMECon = DME
		TACAN.VORCon = VOR

		VOR.OnNAVRadius = System.Math.Sin(DegToRad(VOR.TrackAccuracy + arTrackAccuracy.Value)) / System.Math.Sin(DegToRad(90.0# - arTrackAccuracy.Value)) * arOverHeadToler.Value
		NDB.OnNAVRadius = System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy + arTrackAccuracy.Value)) / System.Math.Sin(DegToRad(90.0# - arTrackAccuracy.Value)) * arOverHeadToler.Value

		Return True
EH:
		MessageBox.Show(Err.Number + "  " + Err.Description)
		Return False
	End Function

	Function OnNAVShift(ByRef NavType As Integer, ByRef Hrel As Double) As Double
		If (NavType = eNavaidType.VOR) Or (NavType = eNavaidType.TACAN) Then
			OnNAVShift = VOR.OnNAVRadius / arOverHeadToler.Value * Hrel * System.Math.Tan(DegToRad(VOR.ConeAngle))
		ElseIf NavType = eNavaidType.NDB Then
			OnNAVShift = NDB.OnNAVRadius / arOverHeadToler.Value * Hrel * System.Math.Tan(DegToRad(NDB.ConeAngle))
		Else ' (NavType = eNavaidType.CodeLLZ) Then
			OnNAVShift = -1000000.0
		End If
	End Function

	Function GetNavTypeName(NavaidType As eNavaidType) As String
		If NavaidType = eNavaidType.NONE Then
			Return "WPT"
		Else
			Return NavTypeNames(NavaidType)
		End If
	End Function

	Public Function FixToNav(ByRef fi As WPT_FIXType) As NavaidType
		Dim I As Integer
		Dim N As Integer

		If fi.TypeCode = eNavaidType.DME Then
			N = UBound(DMEList)
			For I = 0 To N
				If DMEList(I).CallSign = fi.Name Then Return DMEList(I)
			Next I
		Else
			N = UBound(NavaidList)
			For I = 0 To N
				If (NavaidList(I).TypeCode = fi.TypeCode) And (NavaidList(I).CallSign = fi.Name) Then Return NavaidList(I)
			Next I
		End If

		Dim Navaid As New NavaidType
		Navaid.TypeCode = -1
		Navaid.CallSign = ""
		Return Navaid
	End Function

	'Function NavaidData2FixableNavaid(Val As NavaidData) As FIXableNavaid
	'Dim Res As FIXableNavaid
	'
	'Set Res.Pt = Val.Pt
	'Set Res.ptPrj = Val.ptPrj
	'    Res.Name = Val.Name
	'    Res.CallSign = Val.CallSign
	'    Res.MagVar = Val.MagVar
	'
	'    Res.TypeName = Val.TypeName
	'    Res.TypeCode = Val.TypeCode
	'    Res.Range = Val.Range
	'    Res.Index = Val.Index
	'    Res.ValCnt = -1
	'
	'    NavaidData2FixableNavaid = Res
	'End Function
	'
	'Function FixableNavaid2NavaidData(Val As FIXableNavaid) As NavaidData
	'Dim Res As NavaidData
	'
	'Set Res.Pt = Val.Pt
	'Set Res.ptPrj = Val.ptPrj
	'    Res.Name = Val.Name
	'    Res.CallSign = Val.CallSign
	'    Res.MagVar = Val.MagVar
	'
	'    Res.TypeName = Val.TypeName
	'    Res.TypeCode = Val.TypeCode
	'    Res.Range = Val.Range
	'    Res.Index = Val.Index
	'
	'    FixableNavaid2NavaidData = Res
	'End Function

	'Private Structure NameAndIndex
	'	Dim TypeName_Renamed As String
	'	Dim TypeIndex As Integer
	'End Structure

	'Private TypeName2TypeIndex() As NameAndIndex

	'Private Sub FillNameAndIndex()
	'	Dim I As Integer
	'	Dim N As Integer
	'	Dim iNAME As Integer
	'	Dim iCode As Integer

	'	Dim pRow As ESRI.ArcGIS.Geodatabase.IRow
	'	Dim pFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter
	'	Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor

	'	pFilter = New ESRI.ArcGIS.Geodatabase.QueryFilter

	'	pFilter.WhereClause = "NAME <> ''"
	'	N = NavaidTypes.RowCount(pFilter)

	'	If N > 0 Then
	'		ReDim TypeName2TypeIndex(N - 1)

	'		pCursor = NavaidTypes.Search(pFilter, True)
	'		iNAME = pCursor.FindField("Name")
	'		iCode = pCursor.FindField("Code")

	'		I = -1
	'		pRow = pCursor.NextRow
	'		Do While Not pRow Is Nothing
	'			I = I + 1
	'			TypeName2TypeIndex(I).TypeName_Renamed = pRow.Value(iNAME)
	'			TypeName2TypeIndex(I).TypeIndex = pRow.Value(iCode)
	'			pRow = pCursor.NextRow
	'		Loop
	'	Else
	'		ReDim TypeName2TypeIndex(-1)
	'	End If

	'	pFilter = Nothing
	'	pCursor = Nothing
	'	pRow = Nothing
	'End Sub

	'Function Name2Index(ByRef Name As String) As Integer
	'	Dim I As Integer
	'	Dim N As Integer

	'	Name2Index = -1
	'	N = UBound(TypeName2TypeIndex)

	'	For I = 0 To N
	'		If TypeName2TypeIndex(I).TypeName_Renamed = Name Then
	'			Name2Index = TypeName2TypeIndex(I).TypeIndex
	'			Exit For
	'		End If
	'	Next I

	'End Function

End Module