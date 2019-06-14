Option Strict Off
Option Explicit On

Friend Class CMSAInfo
	Inherits System.Windows.Forms.Form

	Const SelectColor As Integer = &HFFFF00
	Const BufferColor As Integer = &HFF
	'Const BufferSize As Double = 9000.0

	Private CurrADHP As ADHPType

	Private MSARecords() As MSAType
	Private MSARecordsNo As Integer

	Private MSASectorElements() As ESRI.ArcGIS.Carto.IElement
	Private MSASectorSelElements() As ESRI.ArcGIS.Carto.IElement
	Private MSASectorBufElements() As ESRI.ArcGIS.Carto.IElement
	Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

#Region "Form"

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		Text = My.Resources.str02000
		Label1.Text = My.Resources.str02001
		Label2.Text = My.Resources.str02002
		Label4.Text = My.Resources.str02003
		Label5.Text = My.Resources.str02011
		Label7.Text = My.Resources.str02004
		Label8.Text = My.Resources.str02014

		Label3.Text = HeightConverter(HeightUnit).Unit
		Label6.Text = DistanceConverter(DistanceUnit).Unit

		ListView001.Columns.Item(0).Text = My.Resources.str02005
		ListView001.Columns.Item(1).Text = My.Resources.str02006
		ListView001.Columns.Item(2).Text = My.Resources.str02007
		ListView001.Columns.Item(3).Text = My.Resources.str02008 & " (" & HeightConverter(HeightUnit).Unit & ")"
		ListView001.Columns.Item(4).Text = My.Resources.str02009 & " (" & HeightConverter(HeightUnit).Unit & ")"

		ReDim MSARecords(-1)
		ReDim MSASectorElements(-1)
		ReDim MSASectorSelElements(-1)
		ReDim MSASectorBufElements(-1)

		pGraphics = GetActiveView().GraphicsContainer

		'      ComboBox0002.Items.Clear()
		'      N = UBound(ADHPList)

		'      If N >= 0 Then
		'          For i = 0 To N
		'              ComboBox0002.Items.Add(ADHPList(i).Name)
		'          Next i

		'	ComboBox0002.SelectedIndex = 0
		'End If

		GetAIXMMSA()
	End Sub

	Private Sub MSAInfo_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		ClearSectors()
		ClearSelections()

		Erase MSARecords
		Erase MSASectorElements
		Erase MSASectorSelElements
		Erase MSASectorBufElements

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewBackground, Nothing, Nothing)
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

	Private Sub ReListSectors()
		Dim i As Integer
		Dim SelectedMSA As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		Dim MSA As Double
		Dim fM1 As Double
		Dim fM2 As Double

		If HeightUnit = 0 Then
			fM1 = 0.02
			fM2 = 50.0
		Else
			fM1 = 0.01
			fM2 = 100.0
		End If

		SelectedMSA = ComboBox001.SelectedIndex
		ListView001.Items.Clear()

		For i = 0 To MSARecords(SelectedMSA).SectorsNum - 1
			itmX = ListView001.Items.Add(My.Resources.str02010 & CStr(i + 1))

			If MSARecords(SelectedMSA).SectorsNum > 1 Then
				If itmX.SubItems.Count > 1 Then
					itmX.SubItems(1).Text = CStr(Modulus(System.Math.Round(MSARecords(SelectedMSA).Sectors(i).FromDir - MSARecords(SelectedMSA).Navaid.MagVar, 2)))
				Else
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Modulus(System.Math.Round(MSARecords(SelectedMSA).Sectors(i).FromDir - MSARecords(SelectedMSA).Navaid.MagVar, 2)))))
				End If

				If itmX.SubItems.Count > 2 Then
					itmX.SubItems(2).Text = CStr(Modulus(System.Math.Round(MSARecords(SelectedMSA).Sectors(i).ToDir - MSARecords(SelectedMSA).Navaid.MagVar, 2)))
				Else
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Modulus(System.Math.Round(MSARecords(SelectedMSA).Sectors(i).ToDir - MSARecords(SelectedMSA).Navaid.MagVar, 2)))))
				End If
			Else
				If itmX.SubItems.Count > 1 Then
					itmX.SubItems(1).Text = "0"
				Else
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "0"))
				End If

				If itmX.SubItems.Count > 2 Then
					itmX.SubItems(2).Text = "360"
				Else
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "360"))
				End If
			End If

			If itmX.SubItems.Count > 3 Then
				itmX.SubItems(3).Text = CStr(ConvertHeight(MSARecords(SelectedMSA).Sectors(i).LowerLimit, 2))
			Else
				itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(MSARecords(SelectedMSA).Sectors(i).LowerLimit, 2))))
			End If

			MSA = System.Math.Round(ConvertHeight(MSARecords(SelectedMSA).Sectors(i).LowerLimit, 0) * fM1 + 0.499999) * fM2

			If itmX.SubItems.Count > 4 Then
				itmX.SubItems(4).Text = CStr(MSA)
			Else
				itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(MSA)))
			End If
		Next i
	End Sub

	Private Sub ReDrawSectors()
		Dim I As Integer
		Dim SelectedMSA As Integer

		ClearSectors()
		SelectedMSA = ComboBox001.SelectedIndex

		ReDim MSASectorElements(MSARecords(SelectedMSA).SectorsNum - 1)
		ReDim MSASectorSelElements(MSARecords(SelectedMSA).SectorsNum - 1)
		ReDim MSASectorBufElements(MSARecords(SelectedMSA).SectorsNum - 1)

		For I = 0 To MSARecords(SelectedMSA).SectorsNum - 1
			MSASectorElements(I) = DrawPolygon(MSARecords(SelectedMSA).Sectors(I).SectorPoly, 0, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow)
		Next I
	End Sub

	Private Sub ClearSectors()
		Dim i As Integer
		Dim N As Integer

		N = UBound(MSASectorElements)

		On Error Resume Next
		For i = 0 To N
			pGraphics.DeleteElement(MSASectorElements(i))
		Next i
		On Error GoTo 0
	End Sub

	Private Sub ReSelectSectors()
		Dim i As Integer
		Dim SelectedMSA As Integer

		Dim pSectorPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pBufferPoly As ESRI.ArcGIS.Geometry.IPolygon

		Dim FromAngle As Double
		Dim ToAngle As Double

		ClearSelections()
		SelectedMSA = ComboBox001.SelectedIndex

		For i = 0 To ListView001.Items.Count - 1

			If (ListView001.Items.Item(i).Selected) Then
				ToAngle = Modulus(System.Math.Round(Azt2Dir(MSARecords(SelectedMSA).Navaid.pPtGeo, MSARecords(SelectedMSA).Sectors(i).FromDir), 2) + 180.0)
				FromAngle = Modulus(System.Math.Round(Azt2Dir(MSARecords(SelectedMSA).Navaid.pPtGeo, MSARecords(SelectedMSA).Sectors(i).ToDir), 2) + 180.0)

				pSectorPoly = MSARecords(SelectedMSA).Sectors(i).SectorPoly

				'            Set pTopo = pSectorPoly
				'            Set pBufferPoly = pTopo.Buffer(arBufferMSA.Value)
				'            Set pTopo = pBufferPoly
				'            pTopo.IsKnownSimple_2 = False
				'            pTopo.Simplify
				'arBufferMSA.Value

				pBufferPoly = ArcBuf(MSARecords(SelectedMSA).Navaid.pPtPrj, MSARecords(SelectedMSA).Sectors(i).InnerDist, MSARecords(SelectedMSA).Sectors(i).OuterDist, MSARecords(SelectedMSA).Sectors(i).BufferWidth, FromAngle, ToAngle)
				MSASectorSelElements(i) = DrawPolygon(pSectorPoly, SelectColor, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 2, SelectColor)
				MSASectorBufElements(i) = DrawPolygon(pBufferPoly, BufferColor, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, True, 1, BufferColor)
			End If
		Next i
	End Sub

	Private Sub ClearSelections()
		Dim i As Integer
		Dim N As Integer

		On Error Resume Next

		N = UBound(MSASectorSelElements)
		For i = 0 To N
			If (Not MSASectorSelElements(i) Is Nothing) Then
				pGraphics.DeleteElement(MSASectorSelElements(i))
				MSASectorSelElements(i) = Nothing
			End If
		Next i

		N = UBound(MSASectorBufElements)
		For i = 0 To N
			If (Not MSASectorBufElements(i) Is Nothing) Then
				pGraphics.DeleteElement(MSASectorBufElements(i))
				MSASectorBufElements(i) = Nothing
			End If
		Next i

		On Error GoTo 0
	End Sub

	Private Sub ComboBox001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox001.SelectedIndexChanged
		Dim SelectedMSA As Integer

		SelectedMSA = ComboBox001.SelectedIndex

		TextBox1.Text = CStr(ConvertHeight(MSARecords(SelectedMSA).MaxHeight, 2))
		TextBox2.Text = CStr(MSARecords(SelectedMSA).Navaid.CallSign)
		TextBox3.Text = CStr(ConvertDistance(MSARecords(SelectedMSA).MaxRadius, 2))

		ClearSelections()
		ReListSectors()
		ReDrawSectors()

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewBackground, Nothing, Nothing)
	End Sub

	Private Sub ListView001_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView001.SelectedIndexChanged
		ReSelectSectors()
	End Sub

	'Private Sub ComboBox0002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0002.SelectedIndexChanged
	'	Dim i As Integer

	'	i = ComboBox0002.SelectedIndex
	'	If i < 0 Then Exit Sub

	'	CurrADHP = ADHPList(i)
	'	FillADHPFields(CurrADHP)

	'	FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, 100000.0)
	'	GetAIXMMSA()
	'End Sub

	Private Sub GetAIXMMSA()
		Dim i As Integer

		MSARecordsNo = FillAIXMMSAList(MSARecords, NavaidList)

		If (MSARecordsNo = 0) Then
			MessageBox.Show(My.Resources.str05023, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Exit Sub
		End If

		ListView001.Items.Clear()
		ClearSectors()
		ClearSelections()
		ComboBox001.Items.Clear()

		For i = 0 To MSARecordsNo - 1
			ComboBox001.Items.Add(MSARecords(i).Name)
		Next i

		If MSARecordsNo > 0 Then ComboBox001.SelectedIndex = 0
	End Sub

End Class
