Imports System.Collections.Generic
Imports Aran.Aim.Features
Imports Aran.Aim.Enums
Imports Aran.Aim.DataTypes
Imports Aran.Geometries
Imports Aran.Queries
Imports Aran.Aim
Imports Aran.Converters
Imports ESRI.ArcGIS.Geometry

Public Class CCreateTAA

	Friend Const PrimaryR As Double = 46300.0
	Friend Const BufferR As Double = 9260
	Friend Const FullR As Double = PrimaryR + BufferR

	Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

	Private pCommonPartElement As ESRI.ArcGIS.Carto.IElement
	Private pLeftLeafElement As ESRI.ArcGIS.Carto.IElement
	Private pCentralLeafElement As ESRI.ArcGIS.Carto.IElement
	Private pRightLeafElement As ESRI.ArcGIS.Carto.IElement


	Private ProcedureName As String
	Private Category As Integer
	Private IAPArray() As IAPArrayItem
	Private pSelectedIAP As IAPArrayItem

	Private pLandingTakeoff As LandingTakeoffAreaCollection
	Private bFormInitialised As Boolean = False
	Private ReportFrm As TAAReport
	Private pIAPList As List(Of InstrumentApproachProcedure)

	'Private Sub DrawLeg(pProcedureLeg As SegmentLeg)
	'	Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline

	'	pPolyline = CType(ConvertToEsriGeom.FromMultiLineString(pProcedureLeg.Trajectory.Geo), Global.ESRI.ArcGIS.Geometry.IPolyline)
	'	pPolyline = CType(ToPrj(CType(pPolyline, Global.ESRI.ArcGIS.Geometry.IGeometry)), Global.ESRI.ArcGIS.Geometry.IPolyline)
	'	DrawPolyLine(CType(pPolyline, Global.ESRI.ArcGIS.Geometry.Polyline), -1, 2)

	'	Application.DoEvents()
	'End Sub

	'Private Sub TestIAPTransitions(pIAP As InstrumentApproachProcedure, pProcedureTransition As ProcedureTransition)
	'	Dim J As Integer
	'	Dim M As Integer
	'	Dim pProcedureLeg As SegmentLeg
	'	Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline
	'	Dim trajectory As Curve

	'	M = pProcedureTransition.TransitionLeg.Count
	'	For J = 0 To M - 1
	'		pProcedureLeg = CType(pProcedureTransition.TransitionLeg.Item(J).TheSegmentLeg.GetFeature(), SegmentLeg)
	'		trajectory = pProcedureLeg.Trajectory
	'		pPolyline = CType(ConvertToEsriGeom.FromMultiLineString(trajectory.Geo), Global.ESRI.ArcGIS.Geometry.IPolyline)
	'		pPolyline = CType(ToPrj(CType(pPolyline, Global.ESRI.ArcGIS.Geometry.IGeometry)), Global.ESRI.ArcGIS.Geometry.IPolyline)
	'		DrawPolyLine(CType(pPolyline, Global.ESRI.ArcGIS.Geometry.Polyline), -1, 2)

	'	Next J
	'End Sub

#Region "Form"

	Public Sub New()
		' This call is required by the designer.
		InitializeComponent()
		' Add any initialization after the InitializeComponent() call.
		'==========================================================================================
		Dim I As Integer
		Dim N As Integer

		pGraphics = GetActiveView().GraphicsContainer
		ReportFrm = New TAAReport
		ReportFrm.SetBtn(ReportBtn)

		lbLeftUnit.Text = HeightConverter(HeightUnit).Unit
		lbCentralUnit.Text = HeightConverter(HeightUnit).Unit
		lbRightUnit.Text = HeightConverter(HeightUnit).Unit

		'==========================================================================================
		N = EnrouteMOCValues.Length - 1
		For I = 0 To N
			cbLeftMOC.Items.Add(ConvertHeight(GlobalVars.EnrouteMOCValues(I), eRoundMode.SPECIAL_NERAEST).ToString())
			cbCentralMOC.Items.Add(ConvertHeight(GlobalVars.EnrouteMOCValues(I), eRoundMode.SPECIAL_NERAEST).ToString())
			cbRightMOC.Items.Add(ConvertHeight(GlobalVars.EnrouteMOCValues(I), eRoundMode.SPECIAL_NERAEST).ToString())
		Next

		cbLeftMOC.SelectedIndex = 0
		cbCentralMOC.SelectedIndex = 0
		cbRightMOC.SelectedIndex = 0
		'==========================================================================================

		pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)
		FillTAAData()
		'==========================================================================================

		bFormInitialised = True

		If ComboBox001.Items.Count > 0 Then ComboBox001.SelectedIndex = 0
	End Sub

	Private Sub CCreateTAA_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		On Error Resume Next
		If Not (pCommonPartElement Is Nothing) Then pGraphics.DeleteElement(pCommonPartElement)
		If Not (pLeftLeafElement Is Nothing) Then pGraphics.DeleteElement(pLeftLeafElement)
		If Not (pCentralLeafElement Is Nothing) Then pGraphics.DeleteElement(pCentralLeafElement)
		If Not (pRightLeafElement Is Nothing) Then pGraphics.DeleteElement(pRightLeafElement)
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		On Error GoTo 0

		ReportFrm.Close()
	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		' Get a handle to a copy of this form's system (window) menu
		Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
		' Add a separator
		AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
		' Add the About menu item
		AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Windows.Forms.Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

#End Region

#Region "Utils"
	Private Function GetGeometry(termPt As TerminalSegmentPoint) As ESRI.ArcGIS.Geometry.IPoint
		If Not termPt Is Nothing Then

			Dim Identifier As Guid

			If Not termPt.PointChoice Is Nothing Then
				If termPt.PointChoice.Choice = Aran.Aim.SignificantPointChoice.DesignatedPoint Then
					Identifier = termPt.PointChoice.FixDesignatedPoint.Identifier
				ElseIf termPt.PointChoice.Choice = Aran.Aim.SignificantPointChoice.Navaid Then
					Identifier = termPt.PointChoice.NavaidSystem.Identifier
				End If

				If Identifier <> Guid.Empty Then
					Dim I As Integer
					Dim N As Integer

					N = UBound(WPTList)
					For I = 0 To N
						If (WPTList(I).GetFeatureRef().Identifier = Identifier) Then
							Return WPTList(I).pPtGeo
						End If
					Next I
				End If
			End If
		End If

		Return Nothing
	End Function

	Function FillTAAData() As Integer
		Dim bHaveFAF As Boolean
		Dim bHaveIF As Boolean

		Dim I As Integer
		Dim J As Integer
		Dim Ci As Integer

		Dim K As Integer
		Dim N As Integer
		Dim M As Integer
		Dim LegCnt As Integer
		Dim currLeg As Integer

		Dim centrDir As Double
		Dim tmpDir As Double
		Dim tmpDist As Double
		Dim dDir As Double
		Dim dCenter As Double
		Dim dLeft As Double
		Dim dRight As Double

		Dim pPtTmp As IPoint
		Dim pProcedureLeg As SegmentLeg
		Dim pIAP As InstrumentApproachProcedure
		Dim pCurrTransition As ProcedureTransition

		Dim pFinalTransitions As New List(Of ProcedureTransition)
		Dim pIAFPts As New List(Of IPoint)
		Dim pIAFPtCh As New List(Of SignificantPoint)

		N = pIAPList.Count
		ReDim IAPArray(N)
		ComboBox001.Items.Clear()
		pSelectedIAP = Nothing

		Ci = 0

		For I = 0 To N - 1
			pIAP = pIAPList.Item(I)

			If Not pIAP.RNAV Then Continue For
			If pIAP.Name Is Nothing Then Continue For
			'If pIAP.FlightTransition.Count < 3 Then Continue For

			IAPArray(Ci).pIAP = pIAP

			M = pIAP.FlightTransition.Count
			ReDim IAPArray(Ci).app(3)

			pFinalTransitions.Clear()
			pIAFPts.Clear()
			pIAFPtCh.Clear()

			bHaveFAF = False
			bHaveIF = False

			For J = 0 To M - 1
				pCurrTransition = pIAP.FlightTransition.Item(J)
				LegCnt = pCurrTransition.TransitionLeg.Count

				If pCurrTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.FINAL Then
					pFinalTransitions.Add(pCurrTransition)
				ElseIf pCurrTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.APPROACH Then
					For currLeg = 0 To LegCnt - 1
						pProcedureLeg = CType(pCurrTransition.TransitionLeg.Item(currLeg).TheSegmentLeg.GetFeature(), SegmentLeg)

						If pProcedureLeg Is Nothing Then Continue For
						If pProcedureLeg.EndPoint Is Nothing Then Continue For
						If pProcedureLeg.EndPoint.Role Is Nothing Then Continue For
						If pProcedureLeg.StartPoint Is Nothing Then Continue For
						If pProcedureLeg.StartPoint.Role Is Nothing Then Continue For

						If (pProcedureLeg.StartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF) And (pProcedureLeg.EndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IF) Then
							If Not bHaveIF Then
								pPtTmp = GetGeometry(pProcedureLeg.EndPoint)
								bHaveIF = Not (pPtTmp Is Nothing)
								IAPArray(Ci).pIFPtPrj = CType(ToPrj(pPtTmp), Global.ESRI.ArcGIS.Geometry.IPoint)
								IAPArray(Ci).pIFPtCh = pProcedureLeg.EndPoint.PointChoice
							End If

							pPtTmp = GetGeometry(pProcedureLeg.StartPoint)
							pIAFPts.Add(pPtTmp)
							pIAFPtCh.Add(pProcedureLeg.StartPoint.PointChoice)

							'If Not bHaveFAF Then Exit For
						ElseIf (Not bHaveIF Or Not bHaveFAF) And (pProcedureLeg.StartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IF) And (pProcedureLeg.EndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF) Then
							If Not bHaveIF Then
								pPtTmp = GetGeometry(pProcedureLeg.StartPoint)
								bHaveIF = Not (pPtTmp Is Nothing)
								IAPArray(Ci).pIFPtPrj = CType(ToPrj(pPtTmp), Global.ESRI.ArcGIS.Geometry.IPoint)
								IAPArray(Ci).pIFPtCh = pProcedureLeg.StartPoint.PointChoice
							End If

							If Not bHaveFAF Then
								pPtTmp = GetGeometry(pProcedureLeg.EndPoint)
								bHaveFAF = Not (pPtTmp Is Nothing)
								IAPArray(Ci).pFAFPtPrj = CType(ToPrj(pPtTmp), Global.ESRI.ArcGIS.Geometry.IPoint)
							End If
						End If
						'If bHaveIF And bHaveFAF Then Exit For
					Next currLeg
				End If
				'If bHaveIF And bHaveFAF Then Exit For
			Next J

			Dim nPts As Integer = pIAFPts.Count
			If nPts = 0 Then Continue For

			If (Not bHaveFAF Or Not bHaveIF) And pFinalTransitions.Count > 0 Then
				For Each pCurrTransition In pFinalTransitions
					For currLeg = 0 To LegCnt - 1
						pProcedureLeg = CType(pCurrTransition.TransitionLeg.Item(currLeg).TheSegmentLeg.GetFeature(), SegmentLeg)
						If pProcedureLeg Is Nothing Then Continue For
						If pProcedureLeg.EndPoint Is Nothing Then Continue For
						If pProcedureLeg.EndPoint.Role Is Nothing Then Continue For
						If pProcedureLeg.StartPoint Is Nothing Then Continue For
						If pProcedureLeg.StartPoint.Role Is Nothing Then Continue For

						If pProcedureLeg.EndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
							If Not bHaveIF Then
								pPtTmp = GetGeometry(pProcedureLeg.StartPoint)
								bHaveIF = Not (pPtTmp Is Nothing)
								IAPArray(Ci).pIFPtPrj = CType(ToPrj(pPtTmp), Global.ESRI.ArcGIS.Geometry.IPoint)
								IAPArray(Ci).pIFPtCh = pProcedureLeg.StartPoint.PointChoice
							End If

							If Not bHaveFAF Then
								pPtTmp = GetGeometry(pProcedureLeg.EndPoint)
								bHaveFAF = Not (pPtTmp Is Nothing)
								IAPArray(Ci).pFAFPtPrj = CType(ToPrj(pPtTmp), Global.ESRI.ArcGIS.Geometry.IPoint)
							End If
						End If

						If bHaveIF And bHaveFAF Then Exit For
					Next
				Next
			End If

			If Not bHaveFAF Or Not bHaveIF Then Continue For

			IAPArray(Ci).IFtoFAFDir = ReturnAngleInDegrees(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).pFAFPtPrj)
			IAPArray(Ci).IFtoFAFDist = ReturnDistanceInMeters(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).pFAFPtPrj)

			centrDir = IAPArray(Ci).IFtoFAFDir

			dCenter = 5.0
			dLeft = 0.0
			dRight = 0.0

			Dim flg As Integer = 0

			For J = 0 To nPts - 1
				pPtTmp = CType(ToPrj(pIAFPts(J)), Global.ESRI.ArcGIS.Geometry.IPoint)

				tmpDist = ReturnDistanceInMeters(pPtTmp, IAPArray(Ci).pIFPtPrj)
				If tmpDist < distEps Then
					tmpDir = centrDir
					tmpDist = 9000.0
					'Continue For
				Else
					tmpDir = ReturnAngleInDegrees(pPtTmp, IAPArray(Ci).pIFPtPrj)
				End If
				dDir = SubtractAngles(tmpDir, centrDir)

				If dDir < dCenter Then
					dCenter = dDir
					IAPArray(Ci).app(1).pIAFPtCh = pIAFPtCh(J)

					IAPArray(Ci).app(1).IAFtoIFDir = tmpDir
					IAPArray(Ci).app(1).pIAFPtPrj = pPtTmp
					flg = flg Or 2
				ElseIf (dDir >= 69.0) And (dDir <= 91.0) Then
					If (AnglesSideDef(tmpDir, centrDir) = -1) And (dLeft < dDir) Then
						dLeft = dDir
						IAPArray(Ci).app(0).pIAFPtCh = pIAFPtCh(J)

						IAPArray(Ci).app(0).IAFtoIFDir = tmpDir
						IAPArray(Ci).app(0).pIAFPtPrj = pPtTmp
						flg = flg Or 1
					ElseIf (AnglesSideDef(tmpDir, centrDir) = 1) And (dRight < dDir) Then
						dRight = dDir
						IAPArray(Ci).app(2).pIAFPtCh = pIAFPtCh(J)

						IAPArray(Ci).app(2).IAFtoIFDir = tmpDir
						IAPArray(Ci).app(2).pIAFPtPrj = pPtTmp
						flg = flg Or 4
					End If
				End If
			Next

			Dim pCircle As IPolygon
			Dim pPointcollection As IPointCollection
			Dim pCutter As IPolyline

			Dim pTopo As ITopologicalOperator2
			Dim leftGeom As IGeometry = Nothing
			Dim rightGeom As IGeometry = Nothing

			pCutter = New Polyline()
			pPointcollection = pCutter
			'========================================================================
			If IAPArray(Ci).app(1).pIAFPtPrj Is Nothing Then
				IAPArray(Ci).app(1).pIAFPtPrj = IAPArray(Ci).pIFPtPrj
				IAPArray(Ci).app(1).pIAFPtCh = IAPArray(Ci).pIFPtCh
				IAPArray(Ci).app(1).IAFtoIFDir = IAPArray(Ci).IFtoFAFDir
			End If

			Dim dist As Double = ReturnDistanceInMeters(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).app(1).pIAFPtPrj)
			If dist >= PrimaryR Then Continue For

			If Not (IAPArray(Ci).app(0).pIAFPtPrj Is Nothing) Then
				IAPArray(Ci).app(1).StartDir = IAPArray(Ci).app(0).IAFtoIFDir
			Else
				IAPArray(Ci).app(1).StartDir = IAPArray(Ci).IFtoFAFDir - 90.0
			End If

			If Not (IAPArray(Ci).app(2).pIAFPtPrj Is Nothing) Then
				IAPArray(Ci).app(1).EndDir = IAPArray(Ci).app(2).IAFtoIFDir
			Else
				IAPArray(Ci).app(1).EndDir = IAPArray(Ci).IFtoFAFDir + 90.0
			End If

			pPointcollection.AddPoint(PointAlongPlane(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).app(1).StartDir + 180.0, 2 * FullR))
			pPointcollection.AddPoint(IAPArray(Ci).pIFPtPrj)
			pPointcollection.AddPoint(PointAlongPlane(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).app(1).EndDir + 180.0, 2 * FullR))

			pCircle = CreatePrjCircle(IAPArray(Ci).app(1).pIAFPtPrj, PrimaryR)
			pTopo = pCircle

			pTopo.Cut(pCutter, leftGeom, rightGeom)

			pTopo = rightGeom
			IAPArray(Ci).app(1).PrimPoly = rightGeom
			IAPArray(Ci).app(1).FullPoly = pTopo.Buffer(BufferR)

			K = GetTAAObstacles(IAPArray(Ci).app(1).Obstacles, IAPArray(Ci).app(1).FullPoly)
			IAPArray(Ci).app(1).SignObstacle.Identifier = Guid.Empty
			If K >= 0 Then IAPArray(Ci).app(1).SignObstacle = IAPArray(Ci).app(1).Obstacles(K)

			pPointcollection.RemovePoints(0, pPointcollection.PointCount)

			'========================================================================
			If Not (IAPArray(Ci).app(0).pIAFPtPrj Is Nothing) Then
				IAPArray(Ci).app(0).StartDir = IAPArray(Ci).IFtoFAFDir + 180.0
				IAPArray(Ci).app(0).EndDir = IAPArray(Ci).app(0).IAFtoIFDir

				pPointcollection.AddPoint(PointAlongPlane(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).IFtoFAFDir, 2 * FullR))
				pPointcollection.AddPoint(IAPArray(Ci).pIFPtPrj)
				pPointcollection.AddPoint(PointAlongPlane(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).app(0).IAFtoIFDir + 180.0, 2 * FullR))

				pCircle = CreatePrjCircle(IAPArray(Ci).app(0).pIAFPtPrj, PrimaryR)
				pTopo = pCircle
				pTopo.Cut(pCutter, leftGeom, rightGeom)

				pTopo = rightGeom
				IAPArray(Ci).app(0).PrimPoly = rightGeom
				IAPArray(Ci).app(0).FullPoly = pTopo.Buffer(BufferR)

				K = GetTAAObstacles(IAPArray(Ci).app(0).Obstacles, IAPArray(Ci).app(0).FullPoly)
				IAPArray(Ci).app(0).SignObstacle.Identifier = Guid.Empty
				If K >= 0 Then IAPArray(Ci).app(0).SignObstacle = IAPArray(Ci).app(0).Obstacles(K)

				pPointcollection.RemovePoints(0, pPointcollection.PointCount)
			Else
				ReDim IAPArray(Ci).app(0).Obstacles(-1)
			End If

			'========================================================================
			If Not (IAPArray(Ci).app(2).pIAFPtPrj Is Nothing) Then
				IAPArray(Ci).app(2).StartDir = IAPArray(Ci).app(2).IAFtoIFDir
				IAPArray(Ci).app(2).EndDir = IAPArray(Ci).IFtoFAFDir + 180.0

				pPointcollection.AddPoint(PointAlongPlane(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).app(2).IAFtoIFDir + 180.0, 2 * FullR))
				pPointcollection.AddPoint(IAPArray(Ci).pIFPtPrj)
				pPointcollection.AddPoint(PointAlongPlane(IAPArray(Ci).pIFPtPrj, IAPArray(Ci).IFtoFAFDir, 2 * FullR))

				pCircle = CreatePrjCircle(IAPArray(Ci).app(2).pIAFPtPrj, PrimaryR)
				pTopo = pCircle

				pTopo.Cut(pCutter, leftGeom, rightGeom)

				pTopo = rightGeom
				IAPArray(Ci).app(2).PrimPoly = rightGeom
				IAPArray(Ci).app(2).FullPoly = pTopo.Buffer(BufferR)

				K = GetTAAObstacles(IAPArray(Ci).app(2).Obstacles, IAPArray(Ci).app(2).FullPoly)
				IAPArray(Ci).app(2).SignObstacle.Identifier = Guid.Empty
				If K >= 0 Then IAPArray(Ci).app(2).SignObstacle = IAPArray(Ci).app(2).Obstacles(K)
			Else
				ReDim IAPArray(Ci).app(2).Obstacles(-1)
			End If

			ComboBox001.Items.Add(IAPArray(Ci))
			Ci += 1
		Next I

		Ci -= 1
		If Ci >= 0 Then
			ReDim Preserve IAPArray(Ci)
		Else
			ReDim IAPArray(-1)
		End If

		Return Ci
	End Function
#End Region

	Private Sub ComboBox001_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox001.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		Dim fTmp As Double
		Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement

		K = ComboBox001.SelectedIndex
		If K <0 Then Return

		pSelectedIAP= IAPArray(K)
		ProcedureName= pSelectedIAP.ToString()

		On Error Resume Next
		If Not (pCommonPartElement Is Nothing) Then pGraphics.DeleteElement(pCommonPartElement)
		If Not (pLeftLeafElement Is Nothing) Then pGraphics.DeleteElement(pLeftLeafElement)
		If Not (pCentralLeafElement Is Nothing) Then pGraphics.DeleteElement(pCentralLeafElement)
		If Not (pRightLeafElement Is Nothing) Then pGraphics.DeleteElement(pRightLeafElement)
		On Error GoTo 0

		'======================================================================
		pGroupElement = New ESRI.ArcGIS.Carto.GroupElement()

		pGroupElement.AddElement(DrawPointWithText(pSelectedIAP.pIFPtPrj, "IF", 255, False))
		pGroupElement.AddElement(DrawPointWithText(pSelectedIAP.pFAFPtPrj, "FAF", 255, False))
		'pGroupElement.AddElement(DrawPointWithText(pSelectedIAP.pMAPtPrj, "MAPt", 255, False))

		pCommonPartElement = pGroupElement
		pGraphics.AddElement(pCommonPartElement, 0)

		GroupBox1.Enabled = Not pSelectedIAP.app(0).pIAFPtPrj Is Nothing
		GroupBox3.Enabled = Not pSelectedIAP.app(2).pIAFPtPrj Is Nothing

		For I = 0 To 2
			If Not (pSelectedIAP.app(I).pIAFPtPrj Is Nothing) Then
				pGroupElement = New ESRI.ArcGIS.Carto.GroupElement()
				pGroupElement.AddElement(DrawPolygon(pSelectedIAP.app(I).FullPoly, 0, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, False))
				pGroupElement.AddElement(DrawPolygon(pSelectedIAP.app(I).PrimPoly, 0, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, False))
				pGroupElement.AddElement(DrawPointWithText(pSelectedIAP.app(I).pIAFPtPrj, "IAF " + (I + 1).ToString(), RGB(0, 0, 255), False))

				If I = 0 Then
					pLeftLeafElement = pGroupElement
					If ShowLeft.Checked Then pGraphics.AddElement(pLeftLeafElement, 0)
				ElseIf I = 1 Then
					pCentralLeafElement = pGroupElement
					If ShowCentral.Checked Then pGraphics.AddElement(pCentralLeafElement, 0)
				ElseIf I = 2 Then
					pRightLeafElement = pGroupElement
					If ShowRight.Checked Then pGraphics.AddElement(pRightLeafElement, 0)
				End If
			End If
		Next

		IAPArray(K).app(0).fMOC = DeConvertHeight(CDbl(cbLeftMOC.Text))
		IAPArray(K).app(0).fMOCA = IAPArray(K).app(0).fMOC

		IAPArray(K).app(1).fMOC = DeConvertHeight(CDbl(cbCentralMOC.Text))
		IAPArray(K).app(1).fMOCA = IAPArray(K).app(1).fMOC

		IAPArray(K).app(2).fMOC = DeConvertHeight(CDbl(cbRightMOC.Text))
		IAPArray(K).app(2).fMOCA = IAPArray(K).app(2).fMOC

		If GroupBox1.Enabled Then
			fTmp = Dir2Azt(pSelectedIAP.app(0).pIAFPtPrj, pSelectedIAP.app(0).EndDir)
			StartLeft.Text = Math.Round(fTmp, 1).ToString()

			fTmp = Dir2Azt(pSelectedIAP.app(0).pIAFPtPrj, pSelectedIAP.app(0).StartDir)
			EndLeft.Text = Math.Round(fTmp, 1).ToString()
		End If

		fTmp = Dir2Azt(pSelectedIAP.app(1).pIAFPtPrj, pSelectedIAP.app(1).EndDir)
		StartCentral.Text = Math.Round(fTmp, 1).ToString()

		fTmp = Dir2Azt(pSelectedIAP.app(1).pIAFPtPrj, pSelectedIAP.app(1).StartDir)
		EndCentral.Text = Math.Round(fTmp, 1).ToString()

		If GroupBox3.Enabled Then
			fTmp = Dir2Azt(pSelectedIAP.app(2).pIAFPtPrj, pSelectedIAP.app(2).EndDir)
			StartRight.Text = Math.Round(fTmp, 1).ToString()

			fTmp = Dir2Azt(pSelectedIAP.app(2).pIAFPtPrj, pSelectedIAP.app(2).StartDir)
			EndRight.Text = Math.Round(fTmp, 1).ToString()
		End If

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		ReportFrm.FillLeftPage(pSelectedIAP.app(0).Obstacles, pSelectedIAP.app(0).fMOC)
		ReportFrm.FillCentralPage(pSelectedIAP.app(1).Obstacles, pSelectedIAP.app(1).fMOC)
		ReportFrm.FillRightPage(pSelectedIAP.app(2).Obstacles, pSelectedIAP.app(2).fMOC)
	End Sub

	Private Sub OKBtn_Click(sender As System.Object, e As System.EventArgs) Handles OKBtn.Click
		If SaveTAA() Then Me.Close()
	End Sub

	Private Function SaveTAA() As Boolean
		Dim i As Integer
		Dim fTmp As Double
		Dim result As Boolean

		Dim mUomHDistance As UomDistance
		Dim mUomVDistance As UomDistanceVertical

		Dim uomDistHorzTab() As UomDistance
		Dim uomDistVerTab() As UomDistanceVertical

		'Dim pIFSignificantPoint As SignificantPoint

		Dim pTAA As TerminalArrivalArea
		Dim pTAASector As TerminalArrivalAreaSector
		Dim pDistance As ValDistance
		Dim pDistanceVertical As ValDistanceVertical
		Dim pSectorDefinition As CircleSector
		Dim SignObstacle As Obstruction

		pObjectDir.ClearAllFeatures()
		uomDistHorzTab = {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = {UomDistanceVertical.M, UomDistanceVertical.FT}

		mUomHDistance = uomDistHorzTab(DistanceUnit)
		mUomVDistance = uomDistVerTab(HeightUnit)

		'pIFSignificantPoint = New SignificantPoint()

		For i = 0 To 2
			If Not (pSelectedIAP.app(i).pIAFPtPrj Is Nothing) Then
				pTAA = DBModule.pObjectDir.CreateFeature(Of TerminalArrivalArea)()
				'pTAA = New TerminalArrivalArea()

				pTAA.ApproachRNAV = pSelectedIAP.pIAP.GetFeatureRef()

				If i = 0 Then
					pTAA.ArrivalAreaType = CodeTAA.LEFT_BASE
				ElseIf i = 1 Then
					pTAA.ArrivalAreaType = CodeTAA.STRAIGHT_IN
				Else 'If I = 2 Then
					pTAA.ArrivalAreaType = CodeTAA.RIGHT_BASE
				End If

				pTAA.IAF = pSelectedIAP.app(i).pIAFPtCh
				pTAA.IF = pSelectedIAP.pIFPtCh

				'pTAA.IAF = pSelectedIAP.IAFtoIFLegs(i).StartPoint.PointChoice
				'pTAA.IF = pSelectedIAP.IAFtoIFLegs(i).EndPoint.PointChoice

				pTAA.Buffer = ESRIPolygonToAIXMSurface(ToGeo(pSelectedIAP.app(i).FullPoly))

				'DrawPolygon(pSelectedIAP.app(i).FullPoly)
				'Application.DoEvents()

				fTmp = ConvertDistance(PrimaryR, eRoundMode.NERAEST)
				pDistance = New ValDistance(fTmp, mUomHDistance)
				pTAA.LateralBufferWidth = pDistance

				fTmp = ConvertDistance(BufferR, eRoundMode.NERAEST)
				pDistance = New ValDistance(fTmp, mUomHDistance)
				pTAA.OuterBufferWidth = pDistance

				pTAASector = New TerminalArrivalAreaSector()
				pTAASector.AltitudeDescription = CodeAltitudeUse.ABOVE_LOWER
				pTAASector.FlyByCode = True
				pTAASector.ProcedureTurnRequired = i <> 1

				SignObstacle = Nothing

				If pSelectedIAP.app(i).SignObstacle.Identifier <> Guid.Empty Then
					pSelectedIAP.app(i).fMOCA = pSelectedIAP.app(i).SignObstacle.Height + pSelectedIAP.app(i).fMOC

					SignObstacle = New Obstruction()
					SignObstacle.VerticalStructureObstruction = pSelectedIAP.app(i).SignObstacle.GetFeatureRef()
					SignObstacle.MinimumAltitude = New ValDistanceVertical(ConvertHeight(pSelectedIAP.app(i).fMOCA), mUomVDistance)
					'SignObstacle.Id = IAPArray(iiap).app(iseg).Obstacles(i).Index

					pTAASector.SignificantObstacle.Add(SignObstacle)
				End If

				'=================================================================================================

				pSectorDefinition = New CircleSector()

				pSectorDefinition.AngleDirectionReference = CodeDirectionReference.TO
				pSectorDefinition.AngleType = CodeBearing.TRUE
				pSectorDefinition.ArcDirection = CodeArcDirection.CWA
				pSectorDefinition.LowerLimitReference = CodeVerticalReference.MSL
				pSectorDefinition.UpperLimitReference = CodeVerticalReference.MSL

				'pSectorDefinition.ID = ""

				pSectorDefinition.FromAngle = Dir2Azt(pSelectedIAP.app(i).pIAFPtPrj, pSelectedIAP.app(i).EndDir)
				pSectorDefinition.ToAngle = Dir2Azt(pSelectedIAP.app(i).pIAFPtPrj, pSelectedIAP.app(i).StartDir)

				'pSelectedIAP.app(I).fMOC

				pDistanceVertical = New ValDistanceVertical(ConvertHeight(pSelectedIAP.app(i).fMOCA), mUomVDistance)
				pSectorDefinition.LowerLimit = pDistanceVertical
				pSectorDefinition.UpperLimit = pDistanceVertical

				pDistance = New ValDistance(0.0, mUomHDistance)
				pSectorDefinition.InnerDistance = pDistance

				pDistance = New ValDistance(ConvertDistance(PrimaryR, eRoundMode.NERAEST), mUomHDistance)
				pSectorDefinition.OuterDistance = pDistance

				'=================================================================================================

				pTAASector.SectorDefinition = pSectorDefinition
				pTAASector.Extent = ESRIPolygonToAIXMSurface(ToGeo(pSelectedIAP.app(i).PrimPoly))

				'=================================================================================================
				pTAA.Sector.Add(pTAASector)

				pObjectDir.SetFeature(pTAA)

				Try
					Try
						pObjectDir.AddCreatedRefToSrcLocalStorage()
					Catch exc As Exception
						MessageBox.Show("Error on find ref features." + vbCrLf + exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
						Return False
					End Try

					pObjectDir.SetRootFeatureType(FeatureType.TerminalArrivalArea)
					result = pObjectDir.Commit({FeatureType.DesignatedPoint})

					If Not Result Then Return False
				Catch ex As Exception
					MessageBox.Show("Error on commit." + vbCrLf + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
					Return False
				End Try
			End If
		Next

		Return True
	End Function

	Private Sub CancelBtn_Click(sender As System.Object, e As System.EventArgs) Handles CancelBtn.Click
		Me.Close()
	End Sub

	Private Sub ReportBtn_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ReportBtn.CheckedChanged
		If Not bFormInitialised Then Return

		If ReportBtn.Checked Then
			ReportFrm.Show(s_Win32Window)
		Else
			ReportFrm.Hide()
		End If
	End Sub

	Private Sub HelpBtn_Click(sender As System.Object, e As System.EventArgs) Handles HelpBtn.Click

	End Sub

	Private Sub ShowLeft_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ShowLeft.CheckedChanged
		If Not bFormInitialised Then Exit Sub

		If ShowLeft.Checked Then
			pGraphics.AddElement(pLeftLeafElement, 0)
		Else
			On Error Resume Next
			If Not (pLeftLeafElement Is Nothing) Then pGraphics.DeleteElement(pLeftLeafElement)
			On Error GoTo 0
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	End Sub

	Private Sub ShowCentral_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ShowCentral.CheckedChanged
		If Not bFormInitialised Then Exit Sub

		If ShowCentral.Checked Then
			pGraphics.AddElement(pCentralLeafElement, 0)
		Else
			On Error Resume Next
			If Not (pCentralLeafElement Is Nothing) Then pGraphics.DeleteElement(pCentralLeafElement)
			On Error GoTo 0
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	End Sub

	Private Sub ShowRight_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ShowRight.CheckedChanged
		If Not bFormInitialised Then Exit Sub

		If ShowRight.Checked Then
			pGraphics.AddElement(pRightLeafElement, 0)
		Else
			On Error Resume Next
			If Not (pRightLeafElement Is Nothing) Then pGraphics.DeleteElement(pRightLeafElement)
			On Error GoTo 0
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	End Sub

	Private Sub cbLeftMOC_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbLeftMOC.SelectedIndexChanged
		If Not bFormInitialised Then Exit Sub

		Dim K As Integer
		K = ComboBox001.SelectedIndex
		If K < 0 Then Return

		IAPArray(K).app(0).fMOC = DeConvertHeight(CDbl(cbLeftMOC.Text))
		IAPArray(K).app(0).fMOCA = IAPArray(K).app(0).fMOC
		pSelectedIAP = IAPArray(K)

		ReportFrm.FillLeftPage(IAPArray(K).app(0).Obstacles, IAPArray(K).app(0).fMOC)
	End Sub

	Private Sub cbCentralMOC_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbCentralMOC.SelectedIndexChanged
		If Not bFormInitialised Then Exit Sub

		Dim K As Integer
		K = ComboBox001.SelectedIndex
		If K < 0 Then Return

		IAPArray(K).app(1).fMOC = DeConvertHeight(CDbl(cbCentralMOC.Text))
		IAPArray(K).app(1).fMOCA = IAPArray(K).app(1).fMOC
		pSelectedIAP = IAPArray(K)

		ReportFrm.FillCentralPage(IAPArray(K).app(1).Obstacles, IAPArray(K).app(1).fMOC)
	End Sub

	Private Sub cbRightMOC_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbRightMOC.SelectedIndexChanged
		If Not bFormInitialised Then Exit Sub

		Dim K As Integer
		K = ComboBox001.SelectedIndex
		If K < 0 Then Return

		IAPArray(K).app(2).fMOC = DeConvertHeight(CDbl(cbRightMOC.Text))
		IAPArray(K).app(2).fMOCA = IAPArray(K).app(2).fMOC
		pSelectedIAP = IAPArray(K)

		ReportFrm.FillRightPage(IAPArray(K).app(2).Obstacles, IAPArray(K).app(2).fMOC)
	End Sub

End Class