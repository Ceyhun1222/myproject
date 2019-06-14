Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports Aran.Aim.Features
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Enums
Imports Aran.Aim
Imports Aran.Queries

Friend Class CCreateMSA
	Inherits System.Windows.Forms.Form

	Private Const LabelLevel As Integer = 157
	Private Const MaxMSARange As Integer = 120000

	Private Const SelectRGB As Integer = &HFFFF00
	Private Const BufferRGB As Integer = &HF4F
	Private Const MinSectorAngle As Short = 1

	Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
	Private SectorElements() As ESRI.ArcGIS.Carto.IElement
	Private InnerSectorElements() As ESRI.ArcGIS.Carto.IElement

	Private SectorElementsSel() As ESRI.ArcGIS.Carto.IElement
	Private SectorElementsBuf() As ESRI.ArcGIS.Carto.IElement
	Private SectorElementsObs() As ESRI.ArcGIS.Carto.IElement

	Private InnerSectorElementsSel() As ESRI.ArcGIS.Carto.IElement
	Private InnerSectorElementsBuf() As ESRI.ArcGIS.Carto.IElement
	Private InnerSectorElementsObs() As ESRI.ArcGIS.Carto.IElement

	Private OuterSectorElementsSel() As ESRI.ArcGIS.Carto.IElement
	Private OuterSectorElementsBuf() As ESRI.ArcGIS.Carto.IElement
	Private OuterSectorElementsObs() As ESRI.ArcGIS.Carto.IElement

	Private Sectors() As MSASectorType
	'Private ViewSectors() As MSASectorType

	Private InnerMaxObstacle As ObstacleType

	Private ObstacleList() As ObstacleType
	Private WorkObstacleList() As ObstacleType

	'Private CurrADHP As ADHPType
	Private Navaid As NavaidType
	Private ptNavPrj As ESRI.ArcGIS.Geometry.IPoint

	Private RMSA As Double
	Private MOCValue As Double
	Private maxIndex As Integer
	Private CurrPage As Integer
	Private HelpContextID As Integer
	Private msaReportFrm As CMSAReportFrm
	Private bFormInitialised As Boolean

#Region "Form"
	Public Sub New()
		MyBase.New()
		InitializeComponent()

		Dim i As Integer
		Dim N As Integer
		Dim fTmp As Double
		'=================================================
		Me.Text = My.Resources.str08000

		PrevBtn.Left = MultiPage1.Left
		PrevBtn.Text = "<-" & My.Resources.str00904

		NextBtn.Text = My.Resources.str00905 & "->"
		OKBtn.Text = My.Resources.str00900
		CancelBtn.Text = My.Resources.str00901
		ReportBtn.Text = My.Resources.str00951
		HelpBtn.Text = My.Resources.str00903

		MultiPage1.TabPages.Item(0).Text = My.Resources.str08001
		MultiPage1.TabPages.Item(1).Text = My.Resources.str08002
		MultiPage1.TabPages.Item(2).Text = My.Resources.str08003
		'=================================================
		Label001.Text = My.Resources.str02014
		Label002.Text = My.Resources.str08100
		Label004.Text = My.Resources.str08101
		Label005.Text = DistanceConverter(DistanceUnit).Unit
		Label006.Text = My.Resources.str08102
		Label007.Text = HeightConverter(HeightUnit).Unit
		OptionButton002.Text = My.Resources.str08103
		OptionButton003.Text = My.Resources.str08104
		Label008.Text = My.Resources.str08105
		Frame001.Text = My.Resources.str08106
		'=================================================
		UnionBtn.Text = My.Resources.str08107
		chbCutByDist.Text = My.Resources.str08003

		Label201.Text = My.Resources.str08205

		'Label202.Caption = "°"
		Label203.Text = My.Resources.str08206

		'Label204.Caption = "°"

		ListView201.Columns.Item(0).Text = My.Resources.str08200
		ListView201.Columns.Item(1).Text = My.Resources.str08201
		ListView201.Columns.Item(2).Text = My.Resources.str08202
		ListView201.Columns.Item(3).Text = My.Resources.str08203
		ListView201.Columns.Item(4).Text = My.Resources.str08204 & " (" & HeightConverter(HeightUnit).Unit & ")"
		ListView201.Columns.Item(5).Text = My.Resources.str08108 & " (" & HeightConverter(HeightUnit).Unit & ")"
		'=================================================
		If DistanceConverter(DistanceUnit).Rounding <= 0 Then
			N = 0
		Else
			N = -Math.Log10(DistanceConverter(DistanceUnit).Rounding)
			If N < 0 Then N = 0
		End If

		updnCutDist.DecimalPlaces = N

		ListView301.Columns.Item(0).Text = My.Resources.str08200
		ListView301.Columns.Item(1).Text = My.Resources.str08207 & " (" & DistanceConverter(DistanceUnit).Unit & ")"
		ListView301.Columns.Item(2).Text = My.Resources.str08203
		ListView301.Columns.Item(3).Text = My.Resources.str08204 & " (" & HeightConverter(HeightUnit).Unit & ")"
		ListView301.Columns.Item(4).Text = My.Resources.str08108 & " (" & HeightConverter(HeightUnit).Unit & ")"

		Label301.Text = My.Resources.str08301
		Label303.Text = My.Resources.str08302
		Label304.Text = My.Resources.str08303
		Label305.Text = My.Resources.str08108 & ":"
		'=================================================
		CurrPage = 0
		MultiPage1.SelectedIndex = 0
		'MultiPage1.TabPages.Item(2).Visible = False

		Label302.Text = DistanceConverter(DistanceUnit).Unit
		TextBox001.Text = CStr(ConvertDistance(arMSARange, 2))

		If HeightUnit = 0 Then
			fTmp = 300.0
		Else
			fTmp = 1000.0
		End If
		TextBox002.Text = CStr(fTmp)

		ReDim SectorElements(-1)
		ReDim Sectors(-1)

		msaReportFrm = New CMSAReportFrm()
		msaReportFrm.SetBtn(ReportBtn)
		msaReportFrm.Owner = Me

		'Application.HelpFile = "Panda.chm"
		Me.HelpContextID = 1800
		pGraphics = GetActiveView().GraphicsContainer

		'========================
		N = UBound(NavaidList)
		ComboBox001.Items.Clear()

		ComboBox001.Items.Add(CurrADHP.Name)

		For i = 0 To N
			ComboBox001.Items.Add(NavaidList(i).CallSign)
		Next i

		If N >= 0 Then
			ComboBox001.SelectedIndex = 0
			NextBtn.Enabled = True
			MultiPage1.TabPages.Item(1).Enabled = True
		Else
			NextBtn.Enabled = False
			MultiPage1.TabPages.Item(1).Enabled = False
		End If

		bFormInitialised = True

		'ComboBox002.Items.Clear()
		'N = UBound(ADHPList)

		'If N >= 0 Then
		'	For i = 0 To N
		'		ComboBox002.Items.Add(ADHPList(i).Name)
		'	Next i

		'	ComboBox002.SelectedIndex = 0
		'End If
	End Sub

	Private Sub CreateMSA_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim i As Integer
		Dim N As Integer

		msaReportFrm.Close()
		N = UBound(Sectors)

		On Error Resume Next
		ClearSectors()
		ClearSelections()

		Erase Sectors

		Erase ObstacleList
		Erase WorkObstacleList

		Erase SectorElements
		Erase InnerSectorElements

		Erase SectorElementsSel
		Erase SectorElementsBuf
		Erase SectorElementsObs

		On Error GoTo 0

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

	Private Sub UpdateList1UnionBtn()
		Dim i As Integer
		Dim j As Integer
		Dim t As Integer
		Dim bEnable As Boolean

		bEnable = ListView201.SelectedItems.Count = 2

		If bEnable Then
			i = ListView201.SelectedItems(0).Index
			j = ListView201.SelectedItems(1).Index
			If i > j Then
				t = i
				i = j
				j = t
			End If

			bEnable = (j = i + 1) Or ((i = 0) And (j = ListView201.Items.Count - 1))
		End If

		UnionBtn.Enabled = bEnable
	End Sub

	Private Sub UpdateList1UpDownBtn()
		Dim bEnable As Boolean

		bEnable = ListView201.SelectedItems.Count = 1

		If bEnable Then
			updnSectorFrom.Enabled = False
			updnSectorTo.Enabled = False

			updnSectorFrom.Text = ListView201.SelectedItems(0).SubItems(1).Text
			updnSectorTo.Text = ListView201.SelectedItems(0).SubItems(2).Text

			'updnSectorFrom.Enabled = True
			'updnSectorTo.Enabled = True
			'Application.DoEvents()

			msaReportFrm.Combo1.SelectedIndex = ListView201.SelectedItems(0).Index
			bEnable = Sectors.Length > 1
		Else
			updnSectorFrom.Text = ""
			updnSectorTo.Text = ""
		End If

		updnSectorFrom.Enabled = bEnable
		updnSectorTo.Enabled = bEnable

		Label201.Enabled = bEnable
		Label202.Enabled = bEnable
		Label203.Enabled = bEnable
		Label204.Enabled = bEnable
	End Sub

	Private Sub UpdateList2DistanceBtn(Item As System.Windows.Forms.ListViewItem)
		Dim bEnable As Boolean = Not Item Is Nothing
		If bEnable Then bEnable = Item.Checked

		If bEnable Then
			Dim flMaxDist As Double = System.Math.Min(MaxDistanceSect, Sectors(Item.Index).DominicObstacle.MinDist - arBufferMSA.Value)
			updnCutDist.Minimum = ConvertDistance(MinDistanceSect, 2) 'RoundDist(MinDistanceSect)
			updnCutDist.Maximum = ConvertDistance(flMaxDist, 2)
			updnCutDist.Text = Item.SubItems(1).Text
		Else
			updnCutDist.Text = ""
		End If

		Label301.Enabled = bEnable
		Label302.Enabled = bEnable
		updnCutDist.Enabled = bEnable
	End Sub

	Private Sub UpdateList2InnerSectorProperties(Item As System.Windows.Forms.ListViewItem)
		Dim bEnable As Boolean = Not Item Is Nothing
		If bEnable Then bEnable = Item.Checked

		If bEnable Then
			txtInnerObs.Text = Sectors(Item.Index).InnerDominicObstacle.UnicalName
			txtInnerMSA.Text = CStr(Sectors(Item.Index).InnerDominicObstacle.MSA)
		Else
			txtInnerObs.Text = ""
			txtInnerMSA.Text = ""
		End If

		Label304.Enabled = bEnable
		txtInnerObs.Enabled = bEnable

		Label305.Enabled = bEnable
		txtInnerMSA.Enabled = bEnable
	End Sub

	Private Shared Sub QuickSortObstacles(ByRef A() As ObstacleType, ByRef iLo As Integer, ByRef iHi As Integer)
		Dim T As ObstacleType
		Dim Midle As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Midle = A((Lo + Hi) / 2).Height

		Do
			While A(Lo).Height > Midle
				Lo = Lo + 1
			End While

			While A(Hi).Height < Midle
				Hi = Hi - 1
			End While

			If (Lo <= Hi) Then
				T = A(Lo)
				A(Lo) = A(Hi)
				A(Hi) = T

				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then QuickSortObstacles(A, iLo, Hi)
		If (Lo < iHi) Then QuickSortObstacles(A, Lo, iHi)
	End Sub

	Private Shared Sub SortObstacles(ByRef A() As ObstacleType)
		Dim Lo As Integer
		Dim Hi As Integer

		Hi = UBound(A)
		If Hi < 0 Then Return

		Lo = LBound(A)
		'If (Lo = Hi) Then Return
		QuickSortObstacles(A, Lo, Hi)
	End Sub

	Function CreateOptimalSectors() As Integer
		Dim i As Integer
		Dim j As Integer
		Dim N As Integer

		Dim fTmp As Double
		Dim fDist As Double
		Dim fAngle As Double
		Dim CntAngle As Double

		Dim ToDir As Double
		Dim FromDir As Double

		Dim NewSector As MSASectorType
		Dim tmpSector As MSASectorType
		Dim LocSectors As SectorList
		Dim pMSABufferPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator

		pMSABufferPoly = CreatePrjCircle(ptNavPrj, arBufferMSA.Value)
		pRelational = pMSABufferPoly

		SortObstacles(WorkObstacleList)
		N = WorkObstacleList.Length

		LocSectors = New SectorList()

		For i = 0 To N - 1
			NewSector = New MSASectorType()
			NewSector.DominicObstacle = WorkObstacleList(i)

			If pRelational.Disjoint(NewSector.DominicObstacle.pGeomPrj) Then
				'PartObstacle(ptNavPrj, NewSector.DominicObstacle)
				fDist = NewSector.DominicObstacle.FromPoint.R
				fAngle = NewSector.DominicObstacle.FromPoint.a

				If fDist <= RMSA Then
					fTmp = (arBufferMSA.Value) / fDist
					CntAngle = RadToDegValue * (System.Math.Atan(fTmp / System.Math.Sqrt(1.0 - fTmp * fTmp)))
				Else
					CntAngle = RadToDegValue * (System.Math.Acos((fDist * fDist + RMSA * RMSA - arBufferMSA.Value * arBufferMSA.Value) / (2.0 * fDist * RMSA)))
				End If

				FromDir = fAngle + CntAngle
				NewSector.FromAngle = System.Math.Round(Modulus(Dir2Azt(ptNavPrj, FromDir) - Navaid.MagVar + 180.0) - 0.4999999)

				fDist = NewSector.DominicObstacle.ToPoint.R
				fAngle = NewSector.DominicObstacle.ToPoint.a

				If fDist <= RMSA Then
					fTmp = (arBufferMSA.Value) / fDist
					CntAngle = RadToDegValue * (System.Math.Atan(fTmp / System.Math.Sqrt(1.0 - fTmp * fTmp)))
				Else
					CntAngle = RadToDegValue * (System.Math.Acos((fDist * fDist + RMSA * RMSA - arBufferMSA.Value * arBufferMSA.Value) / (2.0 * fDist * RMSA)))
				End If

				ToDir = fAngle - CntAngle
				NewSector.ToAngle = System.Math.Round(Modulus(Dir2Azt(ptNavPrj, ToDir) - Navaid.MagVar + 180.0) + 0.4999999)

				If NewSector.FromAngle > NewSector.ToAngle Then
					tmpSector = New MSASectorType

					tmpSector.FromAngle = NewSector.FromAngle
					tmpSector.ToAngle = 360
					tmpSector.DominicObstacle = NewSector.DominicObstacle
					NewSector.FromAngle = 0
					LocSectors.AddSector(tmpSector)
				End If
			Else
				NewSector.FromAngle = 0
				NewSector.ToAngle = 360
			End If

			LocSectors.AddSector(NewSector)
		Next

		N = LocSectors.Count
		If N > 0 Then
			Dim k As Integer

			tmpSector = LocSectors(N - 1)
			If tmpSector.ToAngle = 360 Then
				tmpSector.ToAngle = 0
				LocSectors(N - 1) = tmpSector
			End If

			For i = 0 To N - 1
				NewSector = LocSectors(i)
				NewSector.FromDir = Azt2Dir(Navaid.pPtGeo, LocSectors(i).FromAngle + Navaid.MagVar + 180.0)
				NewSector.ToDir = Azt2Dir(Navaid.pPtGeo, LocSectors(i).ToAngle + Navaid.MagVar + 180.0)
				LocSectors(i) = NewSector
			Next

			ReDim Sectors(2 * N)

			k = 0
			For i = 0 To N - 1
				Sectors(k) = LocSectors(i)
				k += 1

				j = (i + 1) Mod N
				If LocSectors(i).ToAngle <> LocSectors(j).FromAngle Then
					Sectors(k) = New MSASectorType

					Sectors(k).FromAngle = LocSectors(i).ToAngle
					Sectors(k).FromDir = LocSectors(i).ToDir

					Sectors(k).ToAngle = LocSectors(j).FromAngle
					Sectors(k).ToDir = LocSectors(j).FromDir

					k += 1
				End If
			Next i

			N = k

			i = 0
			While i < N
				j = i + 1
				If j >= N Then j = 0

				If (Sectors(i).DominicObstacle.MSA = Sectors(j).DominicObstacle.MSA) Then
					Sectors(i).ToAngle = Sectors(j).ToAngle
					Sectors(i).ToDir = Sectors(j).ToDir
					If (Sectors(i).DominicObstacle.ReqH < Sectors(j).DominicObstacle.ReqH) Then
						Sectors(i).DominicObstacle = Sectors(j).DominicObstacle
					End If

					N -= 1
					For k = j To N - 1
						Sectors(k) = Sectors(k + 1)
					Next k
				Else
					i += 1
				End If
			End While

			ReDim Preserve Sectors(N - 1)

			If N > 0 Then
				ReDim Preserve Sectors(N - 1)

				For i = 0 To N - 1
					Sectors(i).InnerDist = 0.0
					Sectors(i).OuterDist = RMSA

					Sectors(i).AbsAngle = SubtractAngles(Sectors(i).FromAngle, Sectors(i).ToAngle)
					Sectors(i).SectorPoly = CreateSectorPoly(ptNavPrj, 0, RMSA, Sectors(i).FromDir, Sectors(i).ToDir)
					Sectors(i).BufferPoly = ArcBuf(ptNavPrj, 0, RMSA, arBufferMSA.Value, Sectors(i).ToDir, Sectors(i).FromDir)
				Next i
			End If
		End If

		If N <= 0 Then
			N = 1

			ReDim Sectors(0)
			Sectors(0).InnerDist = 0.0
			Sectors(0).OuterDist = RMSA

			Sectors(0).FromDir = 0.0
			Sectors(0).ToDir = 360.0

			Sectors(0).FromAngle = 0.0
			Sectors(0).ToAngle = 360.0

			Sectors(0).AbsAngle = 360.0
			Sectors(0).SectorPoly = CreatePrjCircle(ptNavPrj, RMSA)
			Sectors(0).BufferPoly = CreatePrjCircle(ptNavPrj, RMSA + arBufferMSA.Value)
		End If

		CalcObstacleAllocation()

		msaReportFrm.FillList(ObstacleList, Sectors, Navaid)

		ReDim SectorElements(N - 1)
		ReDim InnerSectorElements(N - 1)

		ReDim SectorElementsSel(N - 1)
		ReDim SectorElementsBuf(N - 1)
		ReDim SectorElementsObs(N - 1)

		ReDim InnerSectorElementsSel(N - 1)
		ReDim InnerSectorElementsBuf(N - 1)
		ReDim InnerSectorElementsObs(N - 1)

		ReDim OuterSectorElementsSel(N - 1)
		ReDim OuterSectorElementsBuf(N - 1)
		ReDim OuterSectorElementsObs(N - 1)

		Return N
	End Function

	Function CreateUniformSectors(ByRef N As Integer) As Integer
		Dim i As Integer

		Dim ObstCount As Integer
		Dim dAn As Double
		Dim OneSectorFlag As Boolean

		If N <> 1 Then
			OneSectorFlag = maxIndex < 0
			If Not OneSectorFlag Then
				OneSectorFlag = InnerMaxObstacle.Height >= WorkObstacleList(maxIndex).Height
			End If
		Else
			OneSectorFlag = True
		End If

		If OneSectorFlag Then
			N = 1
			ReDim Sectors(0)

			Sectors(0).FromAngle = 0
			Sectors(0).ToAngle = 360.0

			Sectors(0).FromDir = 0.0
			Sectors(0).ToDir = 360.0

			Sectors(0).InnerDist = 0.0
			Sectors(0).OuterDist = RMSA
			Sectors(0).AbsAngle = 360.0

			Sectors(0).SectorPoly = CreatePrjCircle(ptNavPrj, RMSA)
			Sectors(0).BufferPoly = CreatePrjCircle(ptNavPrj, RMSA + arBufferMSA.Value)
		Else
			dAn = 360.0 / N

			ReDim Sectors(N - 1)
			ObstCount = UBound(ObstacleList)

			For i = 0 To N - 1
				Sectors(i).FromAngle = System.Math.Round(Modulus(dAn * i + 180.0 - Navaid.MagVar)) '- 0.4999999
				Sectors(i).ToAngle = System.Math.Round(Modulus(dAn * (i + 1) + 180.0 - Navaid.MagVar)) '+ 0.4999999

				Sectors(i).FromDir = Azt2Dir(Navaid.pPtGeo, Sectors(i).FromAngle + Navaid.MagVar + 180.0)
				Sectors(i).ToDir = Azt2Dir(Navaid.pPtGeo, Sectors(i).ToAngle + Navaid.MagVar + 180.0)

				Sectors(i).InnerDist = 0.0
				Sectors(i).OuterDist = RMSA
				Sectors(i).AbsAngle = dAn

				Sectors(i).SectorPoly = CreateSectorPoly(ptNavPrj, 0, RMSA, Sectors(i).FromDir, Sectors(i).ToDir)
				Sectors(i).BufferPoly = ArcBuf(ptNavPrj, 0, RMSA, arBufferMSA.Value, Sectors(i).ToDir, Sectors(i).FromDir)

			Next i

			SortSectors(Sectors)
		End If

		CalcObstacleAllocation()

		msaReportFrm.FillList(ObstacleList, Sectors, Navaid)

		ReDim SectorElements(N - 1)
		ReDim InnerSectorElements(N - 1)

		ReDim SectorElementsSel(N - 1)
		ReDim SectorElementsBuf(N - 1)
		ReDim SectorElementsObs(N - 1)

		ReDim InnerSectorElementsSel(N - 1)
		ReDim InnerSectorElementsBuf(N - 1)
		ReDim InnerSectorElementsObs(N - 1)

		ReDim OuterSectorElementsSel(N - 1)
		ReDim OuterSectorElementsBuf(N - 1)
		ReDim OuterSectorElementsObs(N - 1)

		Return N
	End Function

	Sub UpDateSectors(ByVal Index As Integer, ByVal bChangedFrom As Boolean, ByVal bChangedTo As Boolean)
		Dim i As Integer
		Dim j As Integer
		Dim K As Integer
		Dim N As Integer
		Dim Ix As Integer
		Dim NextI As Integer
		Dim PrevI As Integer
		Dim ObstCount As Integer
		Dim Indices(2) As Integer

		Dim hMax As Double

		Dim pBufRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pSectRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pCenterRelational As ESRI.ArcGIS.Geometry.IRelationalOperator

		pCenterRelational = CreatePrjCircle(ptNavPrj, arBufferMSA.Value)

		N = 0
		Indices(0) = Index
		NextI = Index
		PrevI = Index

		If bChangedFrom Then
			NextI = Index + 1
			If NextI >= ListView201.Items.Count Then
				NextI = 0
			End If

			N = N + 1
			Indices(N) = NextI
		End If

		If bChangedTo Then
			PrevI = Index - 1
			If PrevI < 0 Then
				PrevI = ListView201.Items.Count - 1
			End If

			N = N + 1
			Indices(N) = PrevI
		End If

		ObstCount = UBound(ObstacleList)

		For K = 0 To N
			i = Indices(K)

			Sectors(i).SectorPoly = CreateSectorPoly(ptNavPrj, 0, RMSA, Sectors(i).FromDir, Sectors(i).ToDir)
			Sectors(i).BufferPoly = ArcBuf(ptNavPrj, 0, RMSA, arBufferMSA.Value, Sectors(i).ToDir, Sectors(i).FromDir)

			pSectRelation = Sectors(i).SectorPoly
			pBufRelation = Sectors(i).BufferPoly

			'X = Sectors(i).ToDir
			'Y = Sectors(i).FromDir
			'Sector = Modulus(Y - X)

			Ix = -1
			hMax = InnerMaxObstacle.Height

			For j = 0 To ObstCount
				ObstacleList(j).iSector(i) = 0
				If pBufRelation.Disjoint(ObstacleList(j).pGeomPrj) Then Continue For

				If Not pCenterRelational.Disjoint(ObstacleList(j).pGeomPrj) Then
					ObstacleList(j).iSector(i) = WholeArea
				ElseIf Not pSectRelation.Disjoint(ObstacleList(j).pGeomPrj) Then
					ObstacleList(j).iSector(i) = PrimaryArea
				Else
					ObstacleList(j).iSector(i) = BufferArea
				End If

				If (ObstacleList(j).Height > hMax) Then 'And (ObstacleList(j).iSector(i) > WholeSector)
					hMax = ObstacleList(j).Height
					Ix = j
				End If
			Next j

			If Ix < 0 Then
				Sectors(i).DominicObstacle = InnerMaxObstacle
			ElseIf ObstacleList(Ix).ReqH < InnerMaxObstacle.ReqH Then
				Sectors(i).DominicObstacle = InnerMaxObstacle
			Else
				Sectors(i).DominicObstacle = ObstacleList(Ix)
			End If

			If ListView201.Items.Item(i).SubItems.Count > 3 Then
				ListView201.Items.Item(i).SubItems(3).Text = Sectors(i).DominicObstacle.UnicalName
			Else
				ListView201.Items.Item(i).SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).DominicObstacle.UnicalName))
			End If

			If ListView201.Items.Item(i).SubItems.Count > 4 Then
				ListView201.Items.Item(i).SubItems(4).Text = CStr(ConvertHeight(Sectors(i).DominicObstacle.Height, 3))
			Else
				ListView201.Items.Item(i).SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Sectors(i).DominicObstacle.Height, 3))))
			End If

			If ListView201.Items.Item(i).SubItems.Count > 5 Then
				ListView201.Items.Item(i).SubItems(5).Text = CStr(Sectors(i).DominicObstacle.MSA)
			Else
				ListView201.Items.Item(i).SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Sectors(i).DominicObstacle.MSA)))
			End If
		Next K

		DrawSectors()
		DrawAllSelections()
		msaReportFrm.FillList(ObstacleList, Sectors, Navaid)
	End Sub

	Private Sub CalcObstacleAllocation()
		Dim i As Integer
		Dim j As Integer
		Dim Ix As Integer
		Dim SecCount As Integer
		Dim ObstCount As Integer
		Dim hMax As Double

		Dim pMSABufferPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim BufRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim SectRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pCenterRelational As ESRI.ArcGIS.Geometry.IRelationalOperator

		pMSABufferPoly = CreatePrjCircle(ptNavPrj, arBufferMSA.Value)
		pCenterRelational = pMSABufferPoly

		ObstCount = UBound(ObstacleList)
		SecCount = UBound(Sectors)

		For i = 0 To ObstCount
			ReDim ObstacleList(i).iSector(SecCount)
		Next i

		For i = 0 To SecCount
			SectRelation = Sectors(i).SectorPoly
			BufRelation = Sectors(i).BufferPoly

			Ix = -1
			hMax = InnerMaxObstacle.Height

			For j = 0 To ObstCount
				ObstacleList(j).iSector(i) = 0

				If BufRelation.Disjoint(ObstacleList(j).pGeomPrj) Then Continue For

				If Not pCenterRelational.Disjoint(ObstacleList(j).pGeomPrj) Then
					ObstacleList(j).iSector(i) = WholeArea
				ElseIf Not SectRelation.Disjoint(ObstacleList(j).pGeomPrj) Then
					ObstacleList(j).iSector(i) = PrimaryArea
				Else
					ObstacleList(j).iSector(i) = BufferArea
				End If

				If (ObstacleList(j).Height > hMax) Then 'And (ObstacleList(j).iSector(i) > 0) 
					hMax = ObstacleList(j).Height
					Ix = j
				End If
			Next j

			If Ix < 0 Then
				Sectors(i).DominicObstacle = InnerMaxObstacle
			ElseIf ObstacleList(Ix).ReqH < InnerMaxObstacle.ReqH Then
				Sectors(i).DominicObstacle = InnerMaxObstacle
			Else
				Sectors(i).DominicObstacle = ObstacleList(Ix)
			End If
		Next i
	End Sub

	Private Sub FillListView201()
		Dim i As Integer
		Dim j As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		ListView201.Items.Clear()
		N = UBound(Sectors)

		For i = 0 To N
			itmX = ListView201.Items.Add("Sector " & CStr(i + 1))

			j = Sectors(i).FromAngle

			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(j)))

			j = Sectors(i).ToAngle
			If j = 0 Then j = 360

			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(j)))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).DominicObstacle.UnicalName))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Sectors(i).DominicObstacle.Height, 3))))
			itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Sectors(i).DominicObstacle.MSA)))
		Next i
	End Sub

	Private Sub FillListView301()
		Dim i As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		ListView301.Items.Clear()
		N = UBound(Sectors)

		For i = 0 To N
			itmX = ListView301.Items.Add("Sector" & CStr(i + 1))

			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(Sectors(i).InnerDist, 2))))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).DominicObstacle.UnicalName))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Sectors(i).DominicObstacle.Height, 3))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Sectors(i).DominicObstacle.MSA)))

			itmX.Checked = Sectors(i).IsCuted

			If Not Sectors(i).IsCutable Then
				itmX.ForeColor = System.Drawing.Color.Silver
				itmX.SubItems.Item(1).ForeColor = System.Drawing.Color.Silver
				itmX.SubItems.Item(2).ForeColor = System.Drawing.Color.Silver
				itmX.SubItems.Item(3).ForeColor = System.Drawing.Color.Silver
				itmX.SubItems.Item(4).ForeColor = System.Drawing.Color.Silver
			End If
		Next i
	End Sub

	Private Sub QuickSortSectors(ByRef A() As MSASectorType, ByRef iLo As Integer, ByRef iHi As Integer)
		Dim T As MSASectorType
		Dim Midle As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Midle = A((Lo + Hi) / 2).FromAngle

		Do
			While A(Lo).FromAngle < Midle
				Lo = Lo + 1
			End While

			While A(Hi).FromAngle > Midle
				Hi = Hi - 1
			End While

			If (Lo <= Hi) Then
				T = A(Lo)
				A(Lo) = A(Hi)
				A(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then QuickSortSectors(A, iLo, Hi)
		If (Lo < iHi) Then QuickSortSectors(A, Lo, iHi)
	End Sub

	Private Sub SortSectors(ByRef A() As MSASectorType)
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = LBound(A)
		Hi = UBound(A)

		If (Lo = Hi) Then Exit Sub
		QuickSortSectors(A, Lo, Hi)
	End Sub

	Private Sub GetWorkObstacleList(ByRef CurObstacle() As ObstacleType, ByRef WorkObstacle() As ObstacleType, ByRef PtNAV As ESRI.ArcGIS.Geometry.Point, ByRef maxIndex As Integer)
		Dim i As Integer
		Dim j As Integer
		Dim K As Integer
		Dim N As Integer

		Dim hbMax As Double
		Dim hwMax As Double
		Dim InvRound As Double
		Dim graetRadius As Double
		Dim MSARoundThreshold As Double

		Dim pGreatCircle As ESRI.ArcGIS.Geometry.IPolyline
		Dim pCircleProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		If HeightUnit = 0 Then
			MSARoundThreshold = 50.0
			InvRound = 0.02
		Else
			MSARoundThreshold = 100.0
			InvRound = 0.01
		End If

		InnerMaxObstacle.TypeName = ""
		InnerMaxObstacle.UnicalName = ""
		InnerMaxObstacle.Height = 0.0
		InnerMaxObstacle.ReqH = MOCValue
		InnerMaxObstacle.MSA = MSAvalue(MOCValue)

		N = UBound(CurObstacle)
		ReDim WorkObstacle(N)

		maxIndex = -1

		If N < 0 Then Return

		j = -1
		K = -1
		hbMax = 0

		graetRadius = 2 * (RMSA + arBufferMSA.Value)
		pGreatCircle = CreateCircleBorder(ptNavPrj, graetRadius)
		pCircleProxi = pGreatCircle

		For i = 0 To N
			PartObstacle(ptNavPrj, graetRadius, pCircleProxi, CurObstacle(i))

			CurObstacle(i).ReqH = CurObstacle(i).Height + MOCValue
			CurObstacle(i).MSA = System.Math.Round(ConvertHeight(CurObstacle(i).ReqH, 0) * InvRound + 0.4999999) * MSARoundThreshold
			'CurObstacle(i).iFlag = WholeSector

			If CurObstacle(i).MinDist <= arBufferMSA.Value Then
				If CurObstacle(i).Height > hbMax Then
					hbMax = CurObstacle(i).Height
					K = i
				End If
			ElseIf CurObstacle(i).Height > hbMax Then
				j += 1
				WorkObstacle(j) = CurObstacle(i)
			End If
		Next i

		If K >= 0 Then
			InnerMaxObstacle = CurObstacle(K)

			hwMax = -9999.0
			i = 0
			While i <= j
				If WorkObstacle(i).Height <= hbMax Then
					WorkObstacle(i) = WorkObstacle(j)
					j -= 1
				Else
					If WorkObstacle(i).Height > hwMax Then
						hwMax = WorkObstacle(i).Height
						maxIndex = i
					End If
					i += 1
				End If
			End While
		End If

		If j >= 0 Then
			ReDim Preserve WorkObstacle(j)
		Else
			ReDim WorkObstacle(-1)
		End If
	End Sub

	Private Sub DrawSectors()
		Dim N As Integer
		Dim i As Integer

		N = UBound(Sectors)

		On Error Resume Next
		For i = 0 To N
			If (Not SectorElements(i) Is Nothing) Then
				pGraphics.DeleteElement(SectorElements(i))
				SectorElements(i) = Nothing
			End If

			If (Not InnerSectorElements(i) Is Nothing) Then
				pGraphics.DeleteElement(InnerSectorElements(i))
				InnerSectorElements(i) = Nothing
			End If

			If Sectors(i).IsCuted Then
				SectorElements(i) = DrawPolygon(Sectors(i).OuterSector, 0, , , , RGB(0, 0, 0))
				InnerSectorElements(i) = DrawPolygon(Sectors(i).InnerSector, 0, , , , RGB(0, 0, 0))
			Else
				SectorElements(i) = DrawPolygon(Sectors(i).SectorPoly, 0, , , , RGB(0, 0, 0))
			End If
		Next i
		On Error GoTo 0
		'GetActiveView().PartialRefresh( esriViewBackground, Nothing, Nothing)
	End Sub

	Private Sub DrawSelectedSector(ByVal SectorID As Integer)
		SectorElementsSel(SectorID) = DrawPolygon(Sectors(SectorID).SectorPoly, SelectRGB, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 2, SelectRGB)
		SectorElementsBuf(SectorID) = DrawPolygon(Sectors(SectorID).BufferPoly, BufferRGB, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 1, BufferRGB)

		If (Not Sectors(SectorID).DominicObstacle.pGeomPrj Is Nothing) Then
			SectorElementsObs(SectorID) = DrawObstacle(Sectors(SectorID).DominicObstacle, 255)
		End If
	End Sub

	Private Sub DrawCuttedSelectedSector(ByRef SectorID As Integer)
		InnerSectorElementsSel(SectorID) = DrawPolygon(Sectors(SectorID).InnerSector, SelectRGB, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 2, SelectRGB)
		InnerSectorElementsBuf(SectorID) = DrawPolygon(Sectors(SectorID).InnerBuffer, BufferRGB, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 1, BufferRGB)

		If (Not Sectors(SectorID).InnerDominicObstacle.pGeomPrj Is Nothing) Then
			InnerSectorElementsObs(SectorID) = DrawObstacle(Sectors(SectorID).InnerDominicObstacle, RGB(0, 0, 255))
		End If

		OuterSectorElementsSel(SectorID) = DrawPolygon(Sectors(SectorID).OuterSector, SelectRGB, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 2, SelectRGB)
		OuterSectorElementsBuf(SectorID) = DrawPolygon(Sectors(SectorID).OuterBuffer, BufferRGB, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 1, BufferRGB)

		If (Not Sectors(SectorID).DominicObstacle.pGeomPrj Is Nothing) Then
			OuterSectorElementsObs(SectorID) = DrawObstacle(Sectors(SectorID).DominicObstacle, RGB(255, 0, 0))
		End If
	End Sub

	Private Sub ClearSectors()
		Dim N As Integer
		Dim i As Integer

		On Error Resume Next
		N = UBound(Sectors)

		For i = 0 To N
			If Not SectorElements(i) Is Nothing Then
				pGraphics.DeleteElement(SectorElements(i))
				SectorElements(i) = Nothing
			End If

			If Not InnerSectorElements(i) Is Nothing Then
				pGraphics.DeleteElement(InnerSectorElements(i))
				InnerSectorElements(i) = Nothing
			End If
		Next i

		On Error GoTo 0
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewBackground, Nothing, Nothing)
	End Sub

	Private Sub ClearSelections()
		Dim i As Integer
		Dim n As Integer

		n = UBound(SectorElementsSel)
		For i = 0 To n
			On Error Resume Next
			If (Not OuterSectorElementsSel(i) Is Nothing) Then
				pGraphics.DeleteElement(OuterSectorElementsSel(i))
				OuterSectorElementsSel(i) = Nothing
			End If

			If (Not OuterSectorElementsBuf(i) Is Nothing) Then pGraphics.DeleteElement(OuterSectorElementsBuf(i))
			OuterSectorElementsBuf(i) = Nothing

			If (Not OuterSectorElementsObs(i) Is Nothing) Then pGraphics.DeleteElement(OuterSectorElementsObs(i))
			OuterSectorElementsObs(i) = Nothing

			If (Not InnerSectorElementsSel(i) Is Nothing) Then pGraphics.DeleteElement(InnerSectorElementsSel(i))
			InnerSectorElementsSel(i) = Nothing

			If (Not InnerSectorElementsBuf(i) Is Nothing) Then pGraphics.DeleteElement(InnerSectorElementsBuf(i))
			InnerSectorElementsBuf(i) = Nothing

			If (Not InnerSectorElementsObs(i) Is Nothing) Then pGraphics.DeleteElement(InnerSectorElementsObs(i))
			InnerSectorElementsObs(i) = Nothing

			If (Not SectorElementsSel(i) Is Nothing) Then
				pGraphics.DeleteElement(SectorElementsSel(i))
				SectorElementsSel(i) = Nothing
			End If

			If (Not SectorElementsBuf(i) Is Nothing) Then
				pGraphics.DeleteElement(SectorElementsBuf(i))
				SectorElementsBuf(i) = Nothing
			End If

			If (Not SectorElementsObs(i) Is Nothing) Then
				pGraphics.DeleteElement(SectorElementsObs(i))
				SectorElementsObs(i) = Nothing
			End If
		Next i
		On Error GoTo 0

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewBackground, Nothing, Nothing)
	End Sub

	Private Sub DrawAllSelections()
		Dim i As Integer
		Dim j As Integer
		Dim n As Integer

		ClearSelections()

		n = ListView201.SelectedItems.Count

		For j = 0 To n - 1
			i = ListView201.SelectedItems(j).Index
			DrawSelectedSector(i)
		Next j
	End Sub

	Private Sub DrawSelectedDuple()
		Dim i As Integer
		Dim j As Integer
		Dim n As Integer

		ClearSelections()

		'Application.DoEvents()

		n = ListView301.SelectedItems.Count
		If n = 0 Then Return

		For j = 0 To n - 1
			i = ListView301.SelectedItems(j).Index

			If Sectors(i).IsCuted Then
				DrawCuttedSelectedSector(i)
			Else
				DrawSelectedSector(i)
			End If
		Next j

	End Sub

	Private Function PossibleDistSectorization() As Boolean
		Dim n As Integer
		Dim i As Integer
		Dim j As Integer

		n = UBound(Sectors)
		PossibleDistSectorization = False

		If (n < 0) Or (Navaid.PairNavaidIndex < 0) Then
			Exit Function
		End If

		n = UBound(Sectors)

		j = 0

		For i = 0 To n
			Sectors(i).IsCutable = (Sectors(i).DominicObstacle.MSA > InnerMaxObstacle.MSA) And (Sectors(i).DominicObstacle.MinDist - arBufferMSA.Value > MinDistanceSect)
			If Sectors(i).IsCutable Then
				PossibleDistSectorization = True
			End If

			'        If (Sectors(I).DominicObstacle.MSA > minMSA) And _
			''           (Sectors(I).DominicObstacle.Dist - arBufferMSA.Value > MinDistanceSect) Then

			'Dim oMax As Long

			'#If false Then
			'Dim pPoly As IPolygon
			'Set pPoly = ArcBuf(ptNavPrj, 0, MinDistanceSect, arBufferMSA.Value, Sectors(I).ToAngle, Sectors(I).FromAngle)
			'            oMax = HmaxInPolygon(pPoly, WorkObstacleList)
			'#Else
			'    oMax = HmaxInSector(I, MinDistanceSect, WorkObstacleList)
			'#End If
			'            If (oMax <> -1) Then
			'                If (MSAvalue(Sectors(I).DominicObstacle.ReqH) > MSAvalue(ObstacleList(WorkObstacleList(oMax)).Height + MOCValue)) Then
			'                    J = J + 1
			'                End If
			'            Else
			'                J = J + 1
			'            End If

			'            Sectors(I).IsCutable = True
			'            J = J + 1
			'        End If
		Next i

		'    If (J > 0) Then
		'        PossibleDistSectorization = True
		'    Else
		'        PossibleDistSectorization = False
		'    End If
	End Function

	Private Sub CutSector(ByVal SectorID As Integer, Optional ByVal CutByDist As Double = -1)
		Dim IoMax As Integer

		If (CutByDist = -1) Then
			Sectors(SectorID).InnerDist = Sectors(SectorID).DominicObstacle.MinDist - arBufferMSA.Value
		Else
			Sectors(SectorID).InnerDist = CutByDist
		End If

		If (Sectors(SectorID).InnerDist > MaxDistanceSect) Then
			Sectors(SectorID).InnerDist = MaxDistanceSect
		End If

		If (Sectors(SectorID).InnerDist < MinDistanceSect) Then
			Sectors(SectorID).InnerDist = MinDistanceSect
		End If

		If Sectors(SectorID).AbsAngle = 360 Then
			Sectors(SectorID).OuterSector = CreateCirclePoly(ptNavPrj, Sectors(SectorID).InnerDist, Sectors(SectorID).OuterDist)
			Sectors(SectorID).InnerSector = CreateCirclePoly(ptNavPrj, 0.0, Sectors(SectorID).InnerDist)
		Else
			Sectors(SectorID).OuterSector = CreateSectorPoly(ptNavPrj, Sectors(SectorID).InnerDist, Sectors(SectorID).OuterDist, Sectors(SectorID).FromDir, Sectors(SectorID).ToDir)
			Sectors(SectorID).InnerSector = CreateSectorPoly(ptNavPrj, 0, Sectors(SectorID).InnerDist, Sectors(SectorID).FromDir, Sectors(SectorID).ToDir)
		End If

		Sectors(SectorID).OuterBuffer = ArcBuf(ptNavPrj, Sectors(SectorID).InnerDist, Sectors(SectorID).OuterDist, arBufferMSA.Value, Sectors(SectorID).ToDir, Sectors(SectorID).FromDir)
		Sectors(SectorID).InnerBuffer = ArcBuf(ptNavPrj, 0, Sectors(SectorID).InnerDist, arBufferMSA.Value, Sectors(SectorID).ToDir, Sectors(SectorID).FromDir)

		IoMax = HmaxInInnerSector(ObstacleList, SectorID, Sectors(SectorID), Navaid)

		If IoMax < 0 Then
			Sectors(SectorID).InnerDominicObstacle = InnerMaxObstacle
		ElseIf ObstacleList(IoMax).ReqH < InnerMaxObstacle.ReqH Then
			Sectors(SectorID).InnerDominicObstacle = InnerMaxObstacle
		Else
			Sectors(SectorID).InnerDominicObstacle = ObstacleList(IoMax)
		End If

		Sectors(SectorID).IsCuted = True
	End Sub

	Private Sub CutAllSectors()
		Dim n As Integer
		Dim i As Integer
		n = UBound(Sectors)

		For i = 0 To n
			Sectors(i).IsCuted = False
			If Sectors(i).IsCutable Then
				CutSector(i)
			End If
		Next i
	End Sub

	Private Sub UnCutAllSectors()
		Dim n As Integer
		Dim i As Integer

		n = UBound(Sectors)

		For i = 0 To n
			Sectors(i).IsCuted = False
		Next i
	End Sub

	'======== Page 1
#Region "Page 1"

	Private Sub ComboBox001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox001.SelectedIndexChanged
		If ComboBox001.SelectedIndex = 0 Then
			Navaid = New NavaidType()
			Navaid.Name = CurrADHP.Name
			Navaid.pPtGeo = CurrADHP.pPtGeo
			Navaid.pPtPrj = CurrADHP.pPtPrj
			Navaid.MagVar = CurrADHP.MagVar
			Navaid.TypeCode = eNavaidType.NONE
			Navaid.Identifier = CurrADHP.Identifier
			Navaid.PairNavaidIndex = -1
			Navaid.Index = -1
			Label003.Text = "ADHP"
		Else
			Navaid = NavaidList(ComboBox001.SelectedIndex - 1)
			Label003.Text = GetNavTypeName(Navaid.TypeCode)
		End If

		ptNavPrj = Navaid.pPtPrj
		NextBtn.Enabled = True
		MultiPage1.TabPages.Item(1).Enabled = True
	End Sub

	'Private Sub ComboBox002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox002.SelectedIndexChanged
	'	Dim i As Integer
	'	Dim N As Integer

	'	i = ComboBox002.SelectedIndex
	'	If i < 0 Then Exit Sub

	'	CurrADHP = ADHPList(i)
	'	FillADHPFields(CurrADHP)
	'	FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, 100000.0)

	'	N = UBound(NavaidList)
	'	ComboBox001.Items.Clear()

	'	For i = 0 To N
	'		ComboBox001.Items.Add(NavaidList(i).CallSign)
	'	Next i

	'	If N > 0 Then
	'		ComboBox001.SelectedIndex = 0
	'		NextBtn.Enabled = True
	'		MultiPage1.TabPages.Item(1).Enabled = True
	'	Else
	'		NextBtn.Enabled = False
	'		MultiPage1.TabPages.Item(1).Enabled = False
	'	End If
	'End Sub

	Private Sub OptionButton002_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton002.CheckedChanged
		If eventSender.Checked Then
			UpDown001.Enabled = False
			Label008.Enabled = False
		End If
	End Sub

	Private Sub OptionButton003_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton003.CheckedChanged
		If eventSender.Checked Then
			UpDown001.Enabled = True
			Label008.Enabled = True
		End If
	End Sub

	Private Sub TextBox001_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox001.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox001_Validating(TextBox001, New System.ComponentModel.CancelEventArgs())
			eventArgs.Handled = True
		Else
			TextBoxInteger(eventArgs.KeyChar, TextBox001.Text)
			eventArgs.Handled = eventArgs.KeyChar = Chr(0)
		End If
	End Sub

	Private Sub TextBox001_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox001.Validating
		Dim fTmp As Double
		If Not IsNumeric(TextBox001.Text) Then Return

		fTmp = DeConvertDistance(CDbl(TextBox001.Text))
		If fTmp < MinDistanceSect Then
			fTmp = MinDistanceSect
			TextBox001.Text = CStr(ConvertDistance(fTmp, 2))
		ElseIf fTmp > MaxMSARange Then
			fTmp = MaxMSARange
			TextBox001.Text = CStr(ConvertDistance(fTmp, 2))
		End If
	End Sub

	Private Sub TextBox002_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox002.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox002_Validating(TextBox002, New System.ComponentModel.CancelEventArgs(False))
			eventArgs.Handled = True
		Else
			TextBoxInteger(eventArgs.KeyChar, (TextBox002.Text))
			eventArgs.Handled = eventArgs.KeyChar = Chr(0)
		End If
	End Sub

	Private Sub TextBox002_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox002.Validating
		Dim fTmp As Double
		If Not IsNumeric(TextBox002.Text) Then Return

		fTmp = DeConvertHeight(CDbl(TextBox002.Text))
		If fTmp < 300.0 Then
			fTmp = 300.0
			TextBox002.Text = CStr(ConvertHeight(fTmp, 2))
		End If
	End Sub

#End Region
	'======== Page 2
#Region "Page 2"

	Private Sub ListView201_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView201.SelectedIndexChanged
		UpdateList1UnionBtn()
		UpdateList1UpDownBtn()
		DrawAllSelections()
	End Sub

	Private Sub chbCutByDist_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chbCutByDist.CheckStateChanged
		If chbCutByDist.Enabled And chbCutByDist.CheckState Then
			NextBtn.Enabled = True
			'MultiPage1.TabPages.Item(2).Visible = True
		Else
			NextBtn.Enabled = False
			'MultiPage1.TabPages.Item(2).Visible = False
		End If
	End Sub

	Private Sub txbSectorFrom_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles updnSectorFrom.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			txbSectorFrom_Validating(updnSectorFrom, New System.ComponentModel.CancelEventArgs(False))
			eventArgs.Handled = True
		Else
			TextBoxInteger(eventArgs.KeyChar, (updnSectorFrom.Text))
			eventArgs.Handled = eventArgs.KeyChar = Chr(0)
		End If
	End Sub

	Private Sub txbSectorFrom_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles updnSectorFrom.Validating
		Dim i As Integer
		Dim d0 As Double
		Dim d1 As Double
		Dim PrevI As Integer
		Dim iTmp As Integer

		If ListView201.FocusedItem Is Nothing Then Return
		i = ListView201.FocusedItem.Index

		PrevI = i - 1
		If PrevI < 0 Then PrevI = ListView201.Items.Count - 1

		iTmp = CInt(updnSectorFrom.Value)

		d0 = Modulus(iTmp - Sectors(i).FromAngle) - 180.0
		d1 = 180.0 - Modulus(iTmp - Sectors(PrevI).ToAngle)

		If (d0 > 0.0) And (d1 > 0.0) Then
			If d0 > d1 Then
				iTmp = Modulus(Sectors(i).FromAngle + MinSectorAngle - 1) + 1
			Else
				iTmp = Modulus(Sectors(PrevI).ToAngle - MinSectorAngle - 1) + 1
			End If
		End If

		updnSectorFrom.Value = iTmp
	End Sub

	Private Sub updnSectorFrom_ValueChanged(sender As System.Object, e As System.EventArgs) Handles updnSectorFrom.ValueChanged
		Dim i As Integer
		Dim PrevI As Integer

		If Not updnSectorFrom.Enabled Then Return
		If ListView201.SelectedItems.Count <> 1 Then Return

		If (updnSectorFrom.Value = 361) Then
			updnSectorFrom.Value = 1
			Return
		ElseIf (updnSectorFrom.Value = 0) Then
			updnSectorFrom.Value = 360
			Return
		End If

		i = ListView201.SelectedItems(0).Index

		PrevI = i - 1
		If PrevI < 0 Then PrevI = ListView201.Items.Count - 1

		If Modulus(Sectors(i).ToAngle - updnSectorFrom.Value) = 0 Then
			updnSectorFrom.Value = Modulus(Sectors(i).ToAngle - MinSectorAngle - 1) + 1
		ElseIf Modulus(updnSectorFrom.Value - Sectors(PrevI).FromAngle) = 0 Then
			updnSectorFrom.Value = Modulus(Sectors(PrevI).FromAngle + MinSectorAngle - 1) + 1
		Else
			Sectors(i).FromAngle = updnSectorFrom.Value
			Sectors(i).FromDir = Azt2Dir(Navaid.pPtGeo, Sectors(i).FromAngle + Navaid.MagVar + 180.0)
			Sectors(i).AbsAngle = SubtractAngles(Sectors(i).ToDir, Sectors(i).FromDir)

			Sectors(PrevI).ToAngle = Sectors(i).FromAngle
			Sectors(PrevI).ToDir = Sectors(i).FromDir
			Sectors(PrevI).AbsAngle = SubtractAngles(Sectors(PrevI).FromDir, Sectors(PrevI).ToDir)

			If ListView201.Items.Item(i).SubItems.Count > 1 Then
				ListView201.Items.Item(i).SubItems(1).Text = Sectors(i).FromAngle.ToString()
			Else
				ListView201.Items.Item(i).SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).FromAngle.ToString()))
			End If

			If ListView201.Items.Item(PrevI).SubItems.Count > 2 Then
				ListView201.Items.Item(PrevI).SubItems(2).Text = Sectors(i).FromAngle.ToString()
			Else
				ListView201.Items.Item(PrevI).SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).FromAngle.ToString()))
			End If

			UpDateSectors(i, False, True)

			chbCutByDist.Enabled = PossibleDistSectorization()
			chbCutByDist_CheckStateChanged(chbCutByDist, New System.EventArgs())
		End If
	End Sub
	'=============================================================================================================================================================
	Private Sub txbSectorTo_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles updnSectorTo.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			txbSectorTo_Validating(updnSectorTo, New ComponentModel.CancelEventArgs(False))
			eventArgs.Handled = True
		Else
			TextBoxInteger(eventArgs.KeyChar, (updnSectorTo.Text))
			eventArgs.Handled = eventArgs.KeyChar = Chr(0)
		End If
	End Sub

	Private Sub txbSectorTo_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles updnSectorTo.Validating
		Dim i As Integer
		Dim NextI As Integer
		Dim d0 As Double
		Dim d1 As Double
		Dim iTmp As Integer

		If ListView201.FocusedItem Is Nothing Then Return
		i = ListView201.FocusedItem.Index

		NextI = i + 1
		If NextI >= ListView201.Items.Count Then NextI = 0

		iTmp = CInt(updnSectorTo.Value)

		d0 = 180.0 - Modulus(iTmp - Sectors(i).ToAngle)
		d1 = Modulus(iTmp - Sectors(NextI).FromAngle) - 180.0

		If (d0 > 0) And (d1 > 0) Then
			If d0 > d1 Then
				iTmp = Modulus(Sectors(i).ToAngle - MinSectorAngle - 1) + 1
			Else
				iTmp = Modulus(Sectors(NextI).FromAngle + MinSectorAngle - 1) + 1
			End If
		End If

		updnSectorTo.Value = iTmp
	End Sub

	Private Sub updnSectorTo_ValueChanged(sender As System.Object, e As System.EventArgs) Handles updnSectorTo.ValueChanged
		Dim i As Integer
		Dim NextI As Integer

		If Not updnSectorTo.Enabled Then Return
		If ListView201.SelectedItems.Count <> 1 Then Return

		If (updnSectorTo.Value = 361) Then
			updnSectorTo.Value = 1
			Return
		ElseIf (updnSectorTo.Value = 360) Then
			updnSectorTo.Value = 0
			Return
		End If

		i = ListView201.SelectedItems(0).Index
		NextI = i + 1
		If NextI >= ListView201.Items.Count Then NextI = 0

		If Modulus(updnSectorTo.Value - Sectors(i).FromAngle) = 0 Then
			updnSectorTo.Value = Modulus(Sectors(i).FromAngle + MinSectorAngle - 1) + 1
		ElseIf Modulus(Sectors(NextI).ToAngle - updnSectorTo.Value) = 0 Then
			updnSectorTo.Value = Modulus(Sectors(NextI).ToAngle - MinSectorAngle - 1) + 1
		Else
			Sectors(i).ToAngle = updnSectorTo.Value
			Sectors(i).ToDir = Azt2Dir(Navaid.pPtGeo, Sectors(i).ToAngle + Navaid.MagVar + 180.0)
			Sectors(i).AbsAngle = SubtractAngles(Sectors(i).ToDir, Sectors(i).FromDir)

			Sectors(NextI).FromAngle = Sectors(i).ToAngle
			Sectors(NextI).FromDir = Sectors(i).ToDir
			Sectors(NextI).AbsAngle = SubtractAngles(Sectors(NextI).ToDir, Sectors(NextI).FromDir)

			If ListView201.Items.Item(i).SubItems.Count > 2 Then
				ListView201.Items.Item(i).SubItems(2).Text = Sectors(i).ToAngle.ToString()
			Else
				ListView201.Items.Item(i).SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).ToAngle.ToString()))
			End If

			If ListView201.Items.Item(NextI).SubItems.Count > 1 Then
				ListView201.Items.Item(NextI).SubItems(1).Text = Sectors(i).ToAngle.ToString()
			Else
				ListView201.Items.Item(NextI).SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Sectors(i).ToAngle.ToString()))
			End If

			UpDateSectors(i, True, False)

			chbCutByDist.Enabled = PossibleDistSectorization()
			chbCutByDist_CheckStateChanged(chbCutByDist, New System.EventArgs())
		End If
	End Sub

	Private Sub UnionBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles UnionBtn.Click
		Dim i As Integer
		Dim N As Integer
		Dim I1 As Integer
		Dim I2 As Integer
		Dim AbsAngle As Integer

		For i = 0 To ListView201.Items.Count - 1
			If ListView201.Items.Item(i).Selected Then
				If N = 0 Then
					I1 = i
					N = 1
				Else
					I2 = i
					Exit For
				End If
			End If
		Next i

		N = UBound(Sectors)

		ClearSectors()
		ClearSelections()

		AbsAngle = Sectors(I1).AbsAngle + Sectors(I2).AbsAngle

		If Sectors(I1).DominicObstacle.ReqH < Sectors(I2).DominicObstacle.ReqH Then
			If Sectors(I1).ToAngle < Sectors(I2).FromAngle Then
				Sectors(I2).ToAngle = Sectors(I1).ToAngle
				Sectors(I2).ToDir = Sectors(I1).ToDir

				Sectors(I1) = Sectors(I2)
			Else
				Sectors(I2).FromAngle = Sectors(I1).FromAngle
				Sectors(I2).FromDir = Sectors(I1).FromDir

				Sectors(I1) = Sectors(I2)
			End If
		Else
			If Sectors(I1).ToAngle < Sectors(I2).FromAngle Then
				Sectors(I1).FromDir = Sectors(I2).FromDir
				Sectors(I1).FromAngle = Sectors(I2).FromAngle
			Else
				Sectors(I1).ToDir = Sectors(I2).ToDir
				Sectors(I1).ToAngle = Sectors(I2).ToAngle
			End If
		End If
		Sectors(I1).AbsAngle = AbsAngle

		N = N - 1
		For i = I2 To N
			Sectors(i) = Sectors(i + 1)
		Next i

		ReDim Preserve Sectors(N)
		ReDim SectorElements(N)

		SortSectors(Sectors)

		If N = 0 Then
			Sectors(0).SectorPoly = CreatePrjCircle(ptNavPrj, RMSA)
			Sectors(0).BufferPoly = CreatePrjCircle(ptNavPrj, RMSA + arBufferMSA.Value)
		Else
			For i = 0 To N
				Sectors(i).SectorPoly = CreateSectorPoly(ptNavPrj, 0, RMSA, Sectors(i).FromDir, Sectors(i).ToDir)
				Sectors(i).BufferPoly = ArcBuf(ptNavPrj, 0, RMSA, arBufferMSA.Value, Sectors(i).ToDir, Sectors(i).FromDir)
			Next i
		End If

		ReDim SectorElementsSel(N)
		ReDim SectorElementsBuf(N)
		ReDim SectorElementsObs(N)

		CalcObstacleAllocation()

		msaReportFrm.FillList(ObstacleList, Sectors, Navaid)

		FillListView201()
		DrawSectors()
		ClearSelections()

		chbCutByDist.Enabled = PossibleDistSectorization()
		chbCutByDist_CheckStateChanged(chbCutByDist, New System.EventArgs())
		UnionBtn.Enabled = False
	End Sub
#End Region
	'======== Page 3
#Region "Page 3"
	Private Sub ListView301_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView301.SelectedIndexChanged
		Dim N As Integer = ListView301.SelectedItems.Count
		Dim Item As System.Windows.Forms.ListViewItem = Nothing

		If N <> 0 Then Item = ListView301.SelectedItems(0)

		UpdateList2DistanceBtn(Item)
		UpdateList2InnerSectorProperties(Item)

		DrawSelectedDuple()
	End Sub

	Private Sub ListView301_ItemChecked(sender As Object, eventArgs As ItemCheckedEventArgs) Handles ListView301.ItemChecked
		Dim Item As System.Windows.Forms.ListViewItem = eventArgs.Item
		Dim i As Integer
		i = Item.Index

		If Sectors(i).IsCutable Then
			ClearSectors()
			ClearSelections()

			Sectors(i).IsCuted = Item.Checked

			If Item.Checked Then
				If Item.SubItems.Count > 1 Then
					Item.SubItems(1).Text = CStr(ConvertDistance(Sectors(i).InnerDist, 2))
				Else
					Item.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(Sectors(i).InnerDist, 2))))
				End If
			Else
				If Item.SubItems.Count > 1 Then
					Item.SubItems(1).Text = ""
				Else
					Item.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, ""))
				End If
			End If

			msaReportFrm.FillCuttedList(Sectors, ObstacleList, Navaid)

			DrawSectors()
			ListView301_SelectedIndexChanged(ListView301, Nothing)
		Else
			Item.Checked = False
		End If
	End Sub

	Private Sub updnCutDist_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updnCutDist.ValueChanged
		Dim i As Integer
		Dim fTmp As Double

		If ListView301.SelectedItems.Count = 0 Then Return
		i = ListView301.SelectedItems(0).Index

		fTmp = DeConvertDistance(updnCutDist.Value)

		If Sectors(i).InnerDist <> fTmp Then
			If ListView301.Items.Item(i).SubItems.Count > 1 Then
				ListView301.Items.Item(i).SubItems(1).Text = CStr(updnCutDist.Value)
			Else
				ListView301.Items.Item(i).SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(updnCutDist.Value)))
			End If

			CutSector(i, fTmp)

			txtInnerObs.Text = Sectors(i).InnerDominicObstacle.UnicalName
			txtInnerMSA.Text = CStr(Sectors(i).InnerDominicObstacle.MSA)

			Sectors(i).InnerDist = fTmp
			'Sectors(i).InnerDominicObstacle = Sectors(i).InnerDominicObstacle
			msaReportFrm.FillCuttedList(Sectors, ObstacleList, Navaid)

			DrawSectors()
			DrawSelectedDuple()
		End If
	End Sub

#End Region
	'======== Main
#Region "Main"

	Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
		Static PreviousTab As Short = MultiPage1.SelectedIndex()
		'If MultiPage1.Tag = "a" Then Exit Sub
		'MultiPage1.Tag = "a"

		'    MultiPage1.Value = PreviousTab
		'    If PreviousTab = 0 Then
		'        NextBtn_Click
		'    Else
		'        PrevBtn_Click
		'    End If

		If CurrPage > MultiPage1.SelectedIndex Then
			MultiPage1.SelectedIndex = CurrPage
			PrevBtn_Click(PrevBtn, New System.EventArgs())
		ElseIf CurrPage < MultiPage1.SelectedIndex Then
			MultiPage1.SelectedIndex = CurrPage
			NextBtn_Click(NextBtn, New System.EventArgs())
		End If

		'MultiPage1.Tag = ""
		PreviousTab = MultiPage1.SelectedIndex()
	End Sub

	Private Sub NextBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click

		ShowPandaBox(Me.Handle.ToInt32)

		Select Case MultiPage1.SelectedIndex
			Case 0
				RMSA = DeConvertDistance(CDbl(TextBox001.Text))
				MOCValue = DeConvertHeight(CDbl(TextBox002.Text))

				GetObstaclesByDist(ObstacleList, ptNavPrj, RMSA + arBufferMSA.Value)
				GetWorkObstacleList(ObstacleList, WorkObstacleList, ptNavPrj, maxIndex)

				If Not OptionButton003.Checked Then
					CreateOptimalSectors()
				Else
					CreateUniformSectors(CInt(UpDown001.Value))
				End If

				FillListView201()
				DrawSectors()

				UpdateList1UpDownBtn()
				DrawAllSelections()

				UnionBtn.Enabled = False
				chbCutByDist.Enabled = PossibleDistSectorization()
				chbCutByDist.CheckState = False

				PrevBtn.Enabled = True
				NextBtn.Enabled = False
				OKBtn.Enabled = True
				ReportBtn.Enabled = True

				CurrPage = 1
			Case 1
				ClearSectors()
				ClearSelections()

				CutAllSectors()
				'CreateViewSectors()
				msaReportFrm.FillCuttedList(Sectors, ObstacleList, Navaid)

				FillListView301()
				DrawSectors()

				UpdateList2DistanceBtn(ListView301.Items(0))
				UpdateList2InnerSectorProperties(ListView301.Items(0))
				DrawSelectedDuple()

				PrevBtn.Enabled = True
				NextBtn.Enabled = False

				CurrPage = 2
		End Select

		MultiPage1.SelectedIndex = CurrPage
		HidePandaBox()
	End Sub

	Private Sub PrevBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PrevBtn.Click
		Select Case MultiPage1.SelectedIndex
			Case 1
				ClearSectors()
				ClearSelections()

				PrevBtn.Enabled = False
				ReportBtn.Enabled = False
				NextBtn.Enabled = True
				OKBtn.Enabled = False

				CurrPage = 0
			Case 2
				ClearSectors()
				ClearSelections()

				UnCutAllSectors()
				FillListView201()

				DrawSectors()

				UpdateList1UpDownBtn()
				DrawAllSelections()

				PrevBtn.Enabled = True
				NextBtn.Enabled = chbCutByDist.CheckState

				CurrPage = 1
		End Select

		MultiPage1.SelectedIndex = CurrPage
	End Sub

	Private Sub ReportBtn_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ReportBtn.CheckedChanged
		If Not bFormInitialised Then Return

		If ReportBtn.Checked Then
			msaReportFrm.Show(s_Win32Window)
		Else
			msaReportFrm.Hide()
		End If
	End Sub

	Private Sub HelpBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles HelpBtn.Click
		HtmlHelp(0, "Panda.chm", HH_HELP_CONTEXT, 1800)
	End Sub

	Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
		Me.Close()
	End Sub

	Private Sub OKBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OKBtn.Click
		If SaveMSA() Then Me.Close()
	End Sub

	Private Function SaveMSA() As Boolean
		Dim i As Integer
		Dim j As Integer
		Dim K As Integer
		Dim N As Integer

		Dim fTmp As Double
		Dim Result As Boolean

		Dim pSectorDefinition As CircleSector
		Dim pSignificantPoint As SignificantPoint
		Dim pSafeAltitudeArea As SafeAltitudeArea
		Dim pSafeAltitudeSector As SafeAltitudeAreaSector

		Dim pDistance As ValDistance
		Dim pDistanceVertical As ValDistanceVertical

		Dim mUomHDistance As UomDistance
		Dim mUomVDistance As UomDistanceVertical

		Dim uomDistHorzTab() As UomDistance
		Dim uomDistVerTab() As UomDistanceVertical
		Dim uomDistHVTab() As UomDistance
		'Dim Obstacles As List(Of Obstruction)

		pObjectDir.ClearAllFeatures()

		uomDistHorzTab = {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = {UomDistanceVertical.M, UomDistanceVertical.FT}
		uomDistHVTab = {UomDistance.M, UomDistance.FT}

		mUomHDistance = uomDistHorzTab(DistanceUnit)
		mUomVDistance = uomDistVerTab(HeightUnit)

		'mUomSpeed = uomSpeedTab(SpeedUnit)

		pSignificantPoint = New SignificantPoint
		If Navaid.TypeCode = eNavaidType.NONE Then
			pSignificantPoint.AirportReferencePoint = Navaid.GetFeatureRef()
		Else
			pSignificantPoint.NavaidSystem = Navaid.GetFeatureRef()
		End If

		pSafeAltitudeArea = DBModule.pObjectDir.CreateFeature(Of SafeAltitudeArea)()
		pSafeAltitudeArea.CentrePoint = pSignificantPoint
		pSafeAltitudeArea.SafeAreaType = CodeSafeAltitude.MSA

		'pSAASectorList = New List(Of SafeAltitudeAreaSector)()

		N = UBound(Sectors)
		For i = 0 To N
			j = Sectors(i).FromAngle    'System.Math.Round(Modulus(Dir2Azt(ptNavPrj, Sectors(i).FromDir) - Navaid.MagVar + 180))
			K = Sectors(i).ToAngle      'System.Math.Round(Modulus(Dir2Azt(ptNavPrj, Sectors(i).ToDir) - Navaid.MagVar + 180))

			If K = 0 Then K = 360

			pSafeAltitudeSector = New SafeAltitudeAreaSector
			pSectorDefinition = New CircleSector

			pSectorDefinition.AngleDirectionReference = CodeDirectionReference.TO
			pSectorDefinition.AngleType = CodeBearing.MAG
			pSectorDefinition.ArcDirection = CodeArcDirection.CWA
			pSectorDefinition.LowerLimitReference = CodeVerticalReference.MSL
			pSectorDefinition.UpperLimitReference = CodeVerticalReference.MSL

			pSectorDefinition.FromAngle = j
			pSectorDefinition.ToAngle = K

			pDistanceVertical = New ValDistanceVertical(Sectors(i).DominicObstacle.MSA, mUomVDistance)
			pSectorDefinition.LowerLimit = pDistanceVertical
			pSectorDefinition.UpperLimit = pDistanceVertical

			If Sectors(i).IsCuted Then
				fTmp = ConvertDistance(Sectors(i).InnerDist)
			Else
				fTmp = 0.0
			End If
			pDistance = New ValDistance(fTmp, mUomHDistance)
			pSectorDefinition.InnerDistance = pDistance

			pDistance = New ValDistance(ConvertDistance(Sectors(i).OuterDist), mUomHDistance)
			pSectorDefinition.OuterDistance = pDistance

			pSafeAltitudeSector.SectorDefinition = pSectorDefinition

			If Sectors(i).IsCuted Then
				pSafeAltitudeSector.Extent = ESRIPolygonToAIXMSurface(ToGeo(Sectors(i).OuterSector))
			Else
				pSafeAltitudeSector.Extent = ESRIPolygonToAIXMSurface(ToGeo(Sectors(i).SectorPoly))
			End If

			pSafeAltitudeSector.BufferWidth = New ValDistance(ConvertDistance(arBufferMSA.Value), mUomHDistance)

			If Sectors(i).DominicObstacle.Identifier <> Guid.Empty Then
				Dim obstacle As Obstruction = New Obstruction()

				obstacle.VerticalStructureObstruction = Sectors(i).DominicObstacle.GetFeatureRef()
				obstacle.MinimumAltitude = pDistanceVertical

				pDistance = New ValDistance(ConvertHeight(MOCValue), uomDistHVTab(HeightUnit))
				obstacle.RequiredClearance = pDistance
				pSafeAltitudeSector.SignificantObstacle.Add(obstacle)
			End If

			pSafeAltitudeArea.Sector.Add(pSafeAltitudeSector)

			If Sectors(i).IsCuted Then
				pSafeAltitudeSector = New SafeAltitudeAreaSector
				pSectorDefinition = New CircleSector

				pSectorDefinition.AngleDirectionReference = CodeDirectionReference.TO
				pSectorDefinition.AngleType = CodeBearing.MAG
				pSectorDefinition.ArcDirection = CodeArcDirection.CWA
				pSectorDefinition.LowerLimitReference = CodeVerticalReference.MSL
				pSectorDefinition.UpperLimitReference = CodeVerticalReference.MSL

				'pSectorDefinition.ID = ""

				pSectorDefinition.FromAngle = j
				pSectorDefinition.ToAngle = K

				pDistanceVertical = New ValDistanceVertical(Sectors(i).InnerDominicObstacle.MSA, mUomVDistance)
				pSectorDefinition.LowerLimit = pDistanceVertical
				pSectorDefinition.UpperLimit = pDistanceVertical

				pDistance = New ValDistance(0, mUomHDistance)
				pSectorDefinition.InnerDistance = pDistance

				pDistance = New ValDistance(ConvertDistance(Sectors(i).InnerDist), mUomHDistance)
				pSectorDefinition.OuterDistance = pDistance

				pSafeAltitudeSector.SectorDefinition = pSectorDefinition
				pSafeAltitudeSector.Extent = ESRIPolygonToAIXMSurface(ToGeo(Sectors(i).InnerSector))
				pSafeAltitudeSector.BufferWidth = New ValDistance(ConvertDistance(arBufferMSA.Value), mUomHDistance)

				If Sectors(i).InnerDominicObstacle.Identifier <> Guid.Empty Then
					Dim obstacle As Obstruction = New Obstruction()
					obstacle.VerticalStructureObstruction = Sectors(i).InnerDominicObstacle.GetFeatureRef()
					obstacle.MinimumAltitude = pDistanceVertical

					pDistance = New ValDistance(ConvertHeight(MOCValue), uomDistHVTab(HeightUnit))
					obstacle.RequiredClearance = pDistance

					pSafeAltitudeSector.SignificantObstacle.Add(obstacle)
				End If

				pSafeAltitudeArea.Sector.Add(pSafeAltitudeSector)
			Else
			End If
		Next i

		pObjectDir.SetFeature(pSafeAltitudeArea)

		Try
			Try
				pObjectDir.AddCreatedRefToSrcLocalStorage()
			Catch exc As Exception
				MessageBox.Show("Error on find ref features." + vbCrLf + exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return False
			End Try

			pObjectDir.SetRootFeatureType(FeatureType.SafeAltitudeArea)
			Result = pObjectDir.Commit({FeatureType.DesignatedPoint})

			If Not Result Then Return False

			pObjectDir.SaveSrcLocalStorage("")
			gAranEnv.RefreshAllAimLayers()
		Catch ex As Exception
			MessageBox.Show("Error on commit." + vbCrLf + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return False
		End Try

		Return True
	End Function
#End Region

End Class
