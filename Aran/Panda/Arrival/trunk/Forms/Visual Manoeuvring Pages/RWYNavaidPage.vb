Option Strict Off
Option Explicit On
Option Compare Text

Friend Class RWYNavaidPage
    Private _RWYDATA As RWYType()
    Public Property RWYDATA() As RWYType()
        Get
            Return _RWYDATA
        End Get
        Set(ByVal value As RWYType())
            _RWYDATA = value
        End Set
    End Property
    Private RWYCollection As ESRI.ArcGIS.Geometry.IPointCollection
    Private FixAngl() As WPT_FIXType
    Private m_pCentroid As ESRI.ArcGIS.Geometry.IPoint
    Dim CntX As Double
    Dim CntY As Double
    Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
    Dim pRWYsPolygon As ESRI.ArcGIS.Geometry.IPolygon
    Dim pArea As ESRI.ArcGIS.Geometry.IArea
    Private ArNavList() As NavaidType
    Private _FinalNav As NavaidType
    Public Property FinalNav() As NavaidType
        Get
            Return _FinalNav
        End Get
        Set(ByVal value As NavaidType)
            _FinalNav = value
        End Set
    End Property

    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
    Private RWYTHRPrj As ESRI.ArcGIS.Geometry.IPoint
    Private OnAero As Boolean
    'Private Solutions() As LowHigh
    Private _Solutions As LowHigh()
    Public Property Solutions() As LowHigh()
        Get
            Return _Solutions
        End Get
        Set(ByVal value As LowHigh())
            _Solutions = value
        End Set
    End Property

    Private _Category As Integer
    Public Property Category() As Integer
        Get
            Return _Category
        End Get
        Set(ByVal value As Integer)
            _Category = value
        End Set
    End Property
    Private _SelectedRWY As RWYType
    Public Property SelectedRWY() As RWYType
        Get
            Return _SelectedRWY
        End Get
        Set(ByVal value As RWYType)
            _SelectedRWY = value
        End Set
    End Property

    Private RWYIndex() As Integer
    Private m_ArDir As Double
    Private FictTHR As ESRI.ArcGIS.Geometry.IPoint

    Public FASegmElement As ESRI.ArcGIS.Carto.IElement
    Public LeftPolyElement As ESRI.ArcGIS.Carto.IElement
    Public RightPolyElement As ESRI.ArcGIS.Carto.IElement
    Public PrimePolyElement As ESRI.ArcGIS.Carto.IElement

    Private fAlignOCH As Double
    Private fMinOCH As Double
    Private fVisAprOCH As Double
    Private pPolyLeft As ESRI.ArcGIS.Geometry.IPointCollection
    Private pPolyPrime As ESRI.ArcGIS.Geometry.IPointCollection
    Private pPolyRight As ESRI.ArcGIS.Geometry.IPointCollection
    Private NavGuadArea2 As ESRI.ArcGIS.Geometry.IPointCollection

    Private Ss As Double
    Private Vs As Double

    Private InSectList() As InSectorNav
    Private bFormInitialised As Boolean = False

    Private _initialDirectionAngle As Double
    Public Property InitialDirectionAngle() As Double
        Get
            Return _initialDirectionAngle
        End Get
        Set(ByVal value As Double)
            _initialDirectionAngle = value
        End Set
    End Property

    Private _fRefHeight As Double
    Public Property fRefHeight() As Double
        Get
            Return _fRefHeight
        End Get
        Set(ByVal value As Double)
            _fRefHeight = value
        End Set
    End Property


    Private Sub RWYNavaidPage_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim N As Integer
        Dim J As Integer
        Dim I As Integer
        bFormInitialised = True
        ReDim RWYIndex(-1)
        pPolyLeft = New ESRI.ArcGIS.Geometry.Polygon
        pPolyRight = New ESRI.ArcGIS.Geometry.Polygon
        pPolyPrime = New ESRI.ArcGIS.Geometry.Polygon
        pGraphics = GetActiveView().GraphicsContainer
        CLState = True
        cmbBox_RWYTHR.Items.Clear()

        N = UBound(RWYDATA)
        If RWYCollection Is Nothing Then
            RWYCollection = New ESRI.ArcGIS.Geometry.Multipoint
        Else
            RWYCollection.RemovePoints(0, RWYCollection.PointCount)
        End If
        ReDim RWYIndex(N)
        J = -1
        For I = 0 To N
            If RWYDATA(I).Selected Then
                J = J + 1
                RWYIndex(J) = I
                RWYCollection.AddPoint(RWYDATA(I).pPtPrj(eRWY.PtTHR))
                cmbBox_RWYTHR.Items.Add(RWYDATA(I).Name)
            End If
        Next

        cmbBox_RWYTHR.SelectedIndex = 0

        If J < 1 Then
            'CurrPage = MultiPage1.SelectedIndex
            HidePandaBox()
            '        msgbox My.Resources.str503) '"Выберите пару тарцов."
            MessageBox.Show(My.Resources.str300)
            Return
        Else
            If J < 2 Then
                CntX = 0.0
                CntY = 0.0
                For I = 0 To 1
                    CntX = CntX + RWYCollection.Point(I).X
                    CntY = CntY + RWYCollection.Point(I).Y
                Next I

                CntX = 0.5 * CntX
                CntY = 0.5 * CntY

                m_pCentroid = New ESRI.ArcGIS.Geometry.Point
                m_pCentroid.PutCoords(CntX, CntY)
                '    pCentroid.PutCoords CurrADHP.pPtPrj.X, CurrADHP.pPtPrj.Y
            Else
                pTopo = RWYCollection
                pRWYsPolygon = pTopo.ConvexHull

                pTopo = pRWYsPolygon
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()

                pArea = pRWYsPolygon

                m_pCentroid = pArea.Centroid '
            End If
        End If
        '==========================================================================

		FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, MaxNAVDist)
		FillWPT_FIXList(WPTList, CurrADHP, MaxNAVDist)

        N = UBound(WPTList)
        If N >= 0 Then
            ReDim FixAngl(N)
            J = -1
            For I = 0 To N
                If (WPTList(I).TypeCode = eNavaidType.CodeVOR) Or (WPTList(I).TypeCode = eNavaidType.CodeNDB) Then
                    J = J + 1
                    FixAngl(J) = WPTList(I)
                End If
            Next I

            If J >= 0 Then
                ReDim Preserve FixAngl(J)
            Else
                ReDim FixAngl(-1)
                'OptionButton1202.Enabled = False
            End If
        Else
            ReDim FixAngl(-1)
            'OptionButton1202.Enabled = False
            'OptionButton1203.Enabled = False
            'ComboBox1201.Enabled = False
        End If

        cmbBox_GuidanceFacility.Items.Clear()

        N = FillArNavListForCircling(m_pCentroid)

        If N >= 0 Then
            For I = 0 To N
                cmbBox_GuidanceFacility.Items.Add(ArNavList(I).CallSign)
            Next I
            cmbBox_GuidanceFacility.SelectedIndex = 0
        Else
            MessageBox.Show("There are not suitable facility for guidance.")
            Return
        End If

		txtBox_ShortestFacARPDist.Text = CStr(ConvertDistance(ReturnDistanceFromGeomInMeters(_FinalNav.pPtPrj, CurrADHP.pPtPrj), 2))
		'lbl_ShortestFacARPDist.Text = My.Resources.str10209
		'===============================================

		_fRefHeight = CurrADHP.pPtGeo.Z
		'_Label0101_8.Text = My.Resources.str10212  '"OCH определяется относительно превышения аэродрома"     

		Label9.Text = DistanceConverter(DistanceUnit).Unit

	End Sub

	Private Function FillArNavListForCircling(ByVal pCentroid As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim N As Integer
		Dim M As Integer
		Dim K As Integer
		Dim I As Integer
		Dim J As Integer
		Dim fDist As Double
		Dim ILS As NavaidType

		N = UBound(NavaidList)
		M = UBound(RWYDATA)
		ReDim ArNavList(N + M + 2)
		J = -1

		For I = 0 To M
			If RWYDATA(I).Selected Then
				K = GetILS(RWYDATA(I), ILS, CurrADHP)
				If K >= 1 Then
					J = J + 1
					ArNavList(J).pPtGeo = ILS.pPtGeo
					ArNavList(J).pPtPrj = ILS.pPtPrj

					ArNavList(J).Name = ILS.CallSign 'ILS.RWY_ID
					ArNavList(J).CallSign = ILS.CallSign
					ArNavList(J).Identifier = ILS.Identifier

					ArNavList(J).MagVar = ILS.MagVar
					'ArNavList(J).TypeName_Renamed = "LLZ"
					ArNavList(J).TypeCode = eNavaidType.CodeLLZ

					ArNavList(J).Range = 70000.0 'LLZData.Range
					ArNavList(J).Index = ILS.Index
					ArNavList(J).PairNavaidIndex = -1

					ArNavList(J).GPAngle = ILS.GPAngle
					ArNavList(J).GP_RDH = ILS.GP_RDH

					ArNavList(J).Course = ILS.Course
					ArNavList(J).LLZ_THR = ILS.LLZ_THR
					ArNavList(J).SecWidth = ILS.SecWidth
					ArNavList(J).AngleWidth = ILS.AngleWidth
					ArNavList(J).pFeature = ILS.pFeature
				End If
			End If
		Next

		If (N < 0) And (J < 0) Then
			FillArNavListForCircling = -1
			ReDim ArNavList(-1)
			Exit Function
		End If

		For I = 0 To N
			fDist = ReturnDistanceFromGeomInMeters(NavaidList(I).pPtPrj, pCentroid)
			If fDist < ArPANSOPS_MaxNavDist Then 'Срочно: внести в базу ArPANSOPS
				J = J + 1
				ArNavList(J) = NavaidList(I)
			End If
		Next

		FillArNavListForCircling = J

		If J >= 0 Then
			ReDim Preserve ArNavList(J)
		Else
			ReDim ArNavList(-1)
		End If
	End Function


	Private Sub cmbBox_RWYTHR_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_RWYTHR.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim K As Integer

		K = cmbBox_RWYTHR.SelectedIndex
		If K < 0 Then Return

		_SelectedRWY = RWYDATA(RWYIndex(K))

		RWYTHRPrj = _SelectedRWY.pPtPrj(eRWY.PtTHR)

		txtBox_TrueBRG.Text = CStr(System.Math.Round(Modulus(_SelectedRWY.TrueBearing, 360.0), 2)) 'ik
		txtBox_MAGBRG.Text = CStr(System.Math.Round(Modulus(_SelectedRWY.TrueBearing - CurrADHP.MagVar, 360.0), 2))	'mk


		_fRefHeight = CurrADHP.pPtGeo.Z
		'_Label0101_8.Text = My.Resources.str10212  '"OCH определяется относительно превышения аэродрома"

		If cmbBox_GuidanceFacility.Items.Count > 0 Then
			cmbBox_GuidanceFacility.SelectedIndex = 0
		End If
	End Sub

	Private Sub cmbBox_GuidanceFacility_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_GuidanceFacility.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim K As Integer
		Dim Side1 As Integer
		Dim Side2 As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim bSolution As Boolean
		Dim NavOn As Boolean 'Средство находится дальше 1400 м от ВПП
		Dim TopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRoxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pRWYsPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pRWYLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint

		Dim Rad1 As Double
		Dim Rad2 As Double
		Dim Rad3 As Double
		Dim Rad4 As Double
		Dim fTmp1 As Double
		Dim fTmp As Double

		'===========Constants==================
		Dim MinDistStraightInApproach As Double
		Dim MaxDistStraightInApproach As Double
		Dim OnAeroRange As Double
		Dim TolerDist As Double
		Dim Theta1 As Double
		Dim Theta2 As Double

		Dim dRad As Double
		Dim dRad1 As Double
		Dim dRad2 As Double
		Dim dRad3 As Double
		Dim dRad4 As Double

		Dim Dist As Double
		Dim Dist1 As Double
		Dim Dist2 As Double

		Dim RadToAirport As Double

		Dim SideNav As Integer
		Dim ConstrPoint As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim PtR1400 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtL1400 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim Prestr(1) As String
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim ItemStr As String

		On Error Resume Next

		K = cmbBox_GuidanceFacility.SelectedIndex
		If K < 0 Then Return

		_FinalNav = ArNavList(K)

		lbl_NavaidType.Text = GetNavTypeName(_FinalNav.TypeCode)

		'Functions.DrawPoint(_FinalNav.pPtPrj, 255, True)

		If _FinalNav.TypeCode <> eNavaidType.CodeLLZ Then
			_FinalNav.GP_RDH = arAbv_Treshold.Value
		End If
		'===========================================================================

		OnAeroRange = arCirclAprShift.Value
		TolerDist = arMinInterToler.Value
		Theta2 = arStrInAlignment.Value
		Theta1 = arMaxInterAngle.Values(Category)

		MinDistStraightInApproach = arMinInterDist.Value
		MaxDistStraightInApproach = arMinInterDist.Value + arMinInterToler.Value / System.Math.Tan(DegToRad(arStrInAlignment.Value)) 'arFAFLenght.Value

		lstVw_Solutions.Items.Clear()

		pRWYLine = New ESRI.ArcGIS.Geometry.Polyline

		'========== Проверка приаэродромного средства =====
		If RWYCollection.PointCount < 3 Then 'Расстояние РНС от порога ВПП :
			Dist = ReturnDistanceFromGeomInMeters(_FinalNav.pPtPrj, RWYTHRPrj)

			txtBox_ShortestFacARPDist.Text = CStr(ConvertDistance(Dist, 2))
			lbl_ShortestFacARPDist.Text = My.Resources.str10204	 ' "Ближайшее расстояние от РНС до порога ВПП :"

			pRWYLine.FromPoint = RWYCollection.Point(0)
			pRWYLine.ToPoint = RWYCollection.Point(1)
			pRoxi = pRWYLine
		Else
			Dist = ReturnDistanceFromGeomInMeters(_FinalNav.pPtPrj, m_pCentroid)

			txtBox_ShortestFacARPDist.Text = CStr(ConvertDistance(ReturnDistanceFromGeomInMeters(_FinalNav.pPtPrj, CurrADHP.pPtPrj), 2))
			lbl_ShortestFacARPDist.Text = My.Resources.str10209	 '"Ближайшее расстояние от РНС до КТА :"

			TopoOper = RWYCollection
			pRWYsPolygon = TopoOper.ConvexHull

			TopoOper = pRWYsPolygon
			TopoOper.IsKnownSimple_2 = False
			TopoOper.Simplify()
			pRoxi = pRWYsPolygon
		End If

		If _FinalNav.TypeCode = eNavaidType.CodeVOR Then		 '        bSolution = VOR.FA_Range > Dist
			If VOR.FA_Range <= Dist Then
				txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(&HFF0000)
			Else
				txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0)
			End If
		ElseIf _FinalNav.TypeCode = eNavaidType.CodeNDB Then		 '        bSolution = NDB.FA_Range > Dist
			If NDB.FA_Range <= Dist Then
				txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(&HFF0000)
			Else
				txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0)
			End If
		ElseIf _FinalNav.TypeCode = eNavaidType.CodeLLZ Then		 '        bSolution = (LLZ.Range > Dist) 'And (abs(Azt2Dir(_FinalNav.pPtGeo, _FinalNav.Course)-)
			If LLZ.Range <= Dist Then
				txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(&HFF0000)
			Else
				txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0)
			End If
		End If

		bSolution = ArPANSOPS_MaxNavDist > Dist
		Dist = pRoxi.ReturnDistance(_FinalNav.pPtPrj)
		OnAero = Dist < OnAeroRange

		If OnAero Then
			lbl_OnOffARDFac.Text = My.Resources.str10210  '"Аэродромный"
		Else
			lbl_OnOffARDFac.Text = My.Resources.str10211  '"Не аэродромный"
		End If

		'If _OptionButton0201_1.Enabled Then
		'    _OptionButton0201_0.Value = Not OnAero
		'    _OptionButton0201_1.Value = OnAero
		'Else
		'OptionButton0201_0.Checked = True
		'OptionButton0201_1.Checked = False
		'End If

		ReDim Solutions(-1)

		If Not bSolution Then
			ItemStr = My.Resources.str303
			Replace(ItemStr, Chr(10), " ")
			itmX = lstVw_Solutions.Items.Add(ItemStr)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
		ElseIf _FinalNav.TypeCode = eNavaidType.CodeLLZ Then
			ReDim Solutions(0 To 0)
			Solutions(0).Low = _FinalNav.Course
			Solutions(0).High = _FinalNav.Course
			Solutions(0).Ix = 0

			itmX = lstVw_Solutions.Items.Add("LLZ")
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(0).Low) + "°"))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(0).High) + "°"))
		Else '=========По кругу
			If OnAero Then
				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217)
				'        itmX.SubItems(1) = My.Resources.str10221

				itmX = lstVw_Solutions.Items.Add(My.Resources.str10221)

				ReDim Solutions(0)
				Solutions(0).Low = 0.0
				Solutions(0).High = 359.0
				Solutions(0).Ix = 0
			Else
				ReDim Solutions(5)
				RadToAirport = ReturnAngleInDegrees(_FinalNav.pPtPrj, m_pCentroid)

				Solutions(0).Low = System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, RadToAirport) + 0.4999)
				Solutions(0).High = Solutions(0).Low
				Solutions(0).Ix = 0

				Solutions(1).Low = Modulus(Solutions(0).Low + 180.0, 360.0)
				Solutions(1).High = Solutions(1).Low
				Solutions(1).Ix = 0

				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217 + "(" + My.Resources.str10218 + ")")
				itmX = lstVw_Solutions.Items.Add(My.Resources.str10218)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(0).Low) & "°"))

				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217 + "(" + My.Resources.str10218 + ")")
				itmX = lstVw_Solutions.Items.Add(My.Resources.str10218)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(1).Low) & "°"))

				dRad1 = 0.0
				dRad2 = 0.0
				dRad3 = 0.0
				dRad4 = 0.0

				For I = 0 To RWYCollection.PointCount - 1
					Side1 = SideDef(_FinalNav.pPtPrj, RadToAirport, RWYCollection.Point(I))
					Dist1 = ReturnDistanceFromGeomInMeters(_FinalNav.pPtPrj, RWYCollection.Point(I))

					fTmp1 = OnAeroRange / Dist1
					fTmp = 180.0 * System.Math.Atan(fTmp1 / System.Math.Sqrt(-fTmp1 * fTmp1 + 1)) / PI
					fTmp1 = ReturnAngleInDegrees(_FinalNav.pPtPrj, RWYCollection.Point(I))

					If Side1 < 0 Then
						dRad = SubtractAngles(RadToAirport, fTmp1)
						If dRad > dRad1 Then
							dRad1 = dRad
							Rad1 = fTmp1
						End If
					Else
						dRad = SubtractAngles(RadToAirport, fTmp1)
						If dRad > dRad2 Then
							dRad2 = dRad
							Rad2 = fTmp1
						End If
					End If

					dRad = SubtractAngles(RadToAirport, fTmp1 + fTmp)
					If dRad > dRad3 Then
						dRad3 = dRad
						Rad3 = fTmp1 + fTmp
					End If

					dRad = SubtractAngles(RadToAirport, fTmp1 - fTmp)
					If dRad > dRad4 Then
						dRad4 = dRad
						Rad4 = fTmp1 - fTmp
					End If
				Next I

				Solutions(2).Low = System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, Rad1) + 0.4999)
				Solutions(2).High = System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, Rad2) - 0.4999)
				If Solutions(2).Low > Solutions(2).High Then
					Solutions(2).Low = System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, Rad1))
					Solutions(2).High = System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, Rad2))
				End If
				Solutions(2).Ix = 1

				Solutions(3).Low = Modulus(Solutions(2).Low + 180.0, 360.0)
				Solutions(3).High = Modulus(Solutions(2).High + 180.0, 360.0)
				Solutions(3).Ix = 1

				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217 + "(" + My.Resources.str10219 + ")")
				itmX = lstVw_Solutions.Items.Add(My.Resources.str10219)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(2).Low) & "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(2).High) & "°"))

				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217 + "(" + My.Resources.str10219 + ")")
				itmX = lstVw_Solutions.Items.Add(My.Resources.str10219)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(3).Low) & "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(3).High) & "°"))

				'        fTmp = OnAeroRange / Dist1
				'        Rad1 = Rad1 + 180.0 * Atn(fTmp / Sqr(-fTmp * fTmp + 1)) / PI

				'        fTmp = OnAeroRange / Dist2
				'        Rad2 = Rad2 - 180.0 * Atn(fTmp / Sqr(-fTmp * fTmp + 1)) / PI

				Rad1 = Dir2Azt(_FinalNav.pPtPrj, Rad3)
				Rad2 = Dir2Azt(_FinalNav.pPtPrj, Rad4)

				Rad3 = Modulus(Rad1 + 180.0, 360.0)
				Rad4 = Modulus(Rad2 + 180.0, 360.0)

				Solutions(4).Low = System.Math.Round(Rad1 + 0.4999)
				Solutions(4).High = System.Math.Round(Rad2 - 0.4999)
				Solutions(4).Ix = 2

				Solutions(5).Low = System.Math.Round(Rad3 + 0.4999)
				Solutions(5).High = System.Math.Round(Rad4 - 0.4999)
				Solutions(5).Ix = 2

				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217 + "(" + My.Resources.str10220 + ")")
				itmX = lstVw_Solutions.Items.Add(My.Resources.str10220)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(4).Low) & "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(4).High) & "°"))

				'        Set itmX = ListView0101.ListItems.Add(, , My.Resources.str10217 + "(" + My.Resources.str10220 + ")")
				itmX = lstVw_Solutions.Items.Add(My.Resources.str10220)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(5).Low) & "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Solutions(5).High) & "°"))
			End If
		End If

		cmbBox_TrueBRGRange.Items.Clear()
		N = UBound(Solutions)
		For I = 0 To N
			If Solutions(I).Low = Solutions(I).High Then
				cmbBox_TrueBRGRange.Items.Add(CStr(Solutions(I).Low))
			Else
				cmbBox_TrueBRGRange.Items.Add(CStr(Solutions(I).Low) & "-" & CStr(Solutions(I).High))
			End If
		Next I

		cmbBox_TrueBRGRange.SelectedIndex = 0

		'===================================
		'NextBtn.Enabled = UBound(Solutions) >= 0
	End Sub

	Private Sub cmbBox_TrueBRGRange_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_TrueBRGRange.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim Optima As Integer
		Dim fTmp As Double
		Dim fLow As Double
		Dim fHigh As Double
		Dim RWYAzt As Double

		'Dim itmX As System.Windows.Forms.ListViewItem
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pSector As ESRI.ArcGIS.Geometry.IPointCollection

		K = cmbBox_TrueBRGRange.SelectedIndex
		If (K < 0) Or (cmbBox_TrueBRGRange.Tag = "A") Then Return
		cmbBox_TrueBRGRange.Tag = "A"

		N = UBound(Solutions)

		For I = 0 To N
			If Solutions(I).Low = Solutions(I).High Then
				cmbBox_TrueBRGRange.Items.Item(I) = CStr(Solutions(I).Low)
			Else
				cmbBox_TrueBRGRange.Items.Item(I) = CStr(Solutions(I).Low) & "-" & CStr(Solutions(I).High)
			End If
		Next I

		cmbBox_TrueBRGRange.Tag = ""

		'SpinButton0201.Enabled = False
		'SpinButton0201.Minimum = Solutions(K).Low

		RWYAzt = Dir2Azt(_FinalNav.pPtPrj, RWYTHRPrj.M)

		Optima = Solutions(K).Low
		fLow = SubtractAngles(RWYAzt, Optima)

		For I = Solutions(K).Low To Solutions(K).High
			fTmp = SubtractAngles(RWYAzt, I)
			If fTmp < fLow Then
				fLow = fTmp
				Optima = I
			End If
		Next I

		'If OnAero Then
		'    _Label0201_7.Text = ""
		'Else
		'    Select Case Solutions(K).Tag
		'        Case 0
		'            _Label0201_7.Text = My.Resources.str10218
		'        Case 1
		'            _Label0201_7.Text = My.Resources.str10219
		'        Case 2
		'            _Label0201_7.Text = My.Resources.str10220
		'    End Select
		'End If

		nmrcUpDown_TrueBRGRange.Tag = False

		nmrcUpDown_TrueBRGRange.Minimum = Math.Round(Solutions(K).Low)

		If Solutions(K).Low <= Solutions(K).High Then
			nmrcUpDown_TrueBRGRange.Maximum = Math.Round(Solutions(K).High)
		Else
			nmrcUpDown_TrueBRGRange.Maximum = Math.Round(Solutions(K).High) + 360
		End If

		nmrcUpDown_TrueBRGRange.Tag = True

		If nmrcUpDown_TrueBRGRange.Value = Optima Then
			nmrcUpDown_TrueBRGRange_ValueChanged(nmrcUpDown_TrueBRGRange, New System.EventArgs())
		Else
			nmrcUpDown_TrueBRGRange.Value = Optima
		End If

		If Solutions(K).Low = Solutions(K).High Then
			txtBox_TrueBRGRange.ReadOnly = True
			txtBox_TrueBRGRange.BackColor = System.Drawing.SystemColors.Control
			nmrcUpDown_TrueBRGRange.Enabled = False
		Else
			txtBox_TrueBRGRange.ReadOnly = False
			txtBox_TrueBRGRange.BackColor = System.Drawing.SystemColors.Window
			nmrcUpDown_TrueBRGRange.Enabled = True
		End If

		If SubtractAngles(Solutions(K).Low, Solutions(K).High) < 1.0 Then
			fHigh = Azt2Dir(_FinalNav.pPtGeo, Solutions(K).High + 0.5 * LLZ.TrackingTolerance)
			fLow = Azt2Dir(_FinalNav.pPtGeo, Solutions(K).Low - 0.5 * LLZ.TrackingTolerance)
		Else
			fHigh = Azt2Dir(_FinalNav.pPtGeo, Solutions(K).High)
			fLow = Azt2Dir(_FinalNav.pPtGeo, Solutions(K).Low)
		End If

		If OnAero Then
			pSector = CreatePrjCircle(_FinalNav.pPtPrj, 10.0 * RModel)
		Else
			pSector = New ESRI.ArcGIS.Geometry.Polygon
			pSector.AddPoint(PointAlongPlane(_FinalNav.pPtPrj, fHigh, 10.0 * RModel))
			pSector.AddPoint(PointAlongPlane(_FinalNav.pPtPrj, fLow, 10.0 * RModel))
			pSector.AddPoint(_FinalNav.pPtPrj)
			pSector.AddPoint(PointAlongPlane(_FinalNav.pPtPrj, fLow + 180.0, 10.0 * RModel))
			pSector.AddPoint(PointAlongPlane(_FinalNav.pPtPrj, fHigh + 180.0, 10.0 * RModel))
			pSector.AddPoint(_FinalNav.pPtPrj)

			pTopo = pSector
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		End If
		'DrawPolygon pSector, 0

		NavInSector(_FinalNav, NavaidList, InSectList, pSector, fLow)

		'N = UBound(InSectList)
		'ListView0201.Items.Clear()

		'For I = 0 To N
		'    itmX = ListView0201.Items.Add(InSectList(I).CallSign)
		'    itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(InSectList(I).Distance, 2))))
		'    If InSectList(I).Tag = 1 Then
		'        itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "0"))
		'        itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "360"))
		'        itmX.Checked = True
		'    Else
		'        itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, InSectList(I).ToAngle), 3))))
		'        itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(Dir2Azt(_FinalNav.pPtPrj, InSectList(I).FromAngle), 3))))
		'    End If

		'    itmX.Tag = True
		'    itmX.Font = New Font(itmX.Font, FontStyle.Bold)

		'    itmX.SubItems.Item(1).Font = New Font(itmX.SubItems.Item(1).Font, FontStyle.Bold)
		'    itmX.SubItems.Item(2).Font = New Font(itmX.SubItems.Item(2).Font, FontStyle.Bold)
		'    itmX.SubItems.Item(3).Font = New Font(itmX.SubItems.Item(3).Font, FontStyle.Bold)
		'Next I
	End Sub

	Private Sub nmrcUpDown_TrueBRGRange_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nmrcUpDown_TrueBRGRange.ValueChanged
		If Not bFormInitialised Then Return

		If Not CBool(nmrcUpDown_TrueBRGRange.Tag) Then Return
		AzimuthChanged(nmrcUpDown_TrueBRGRange.Value)
	End Sub

	Private Sub AzimuthChanged(ByVal NewAzt As Double)
		Dim CurrTheta As Double
		Dim FA_Range As Double
		Dim NomGrad As Double
		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim fDist As Double
		Dim MaxDh As Double
		Dim Dist As Double
		Dim Bank As Double
		Dim lTAS As Double
		Dim fTmp As Double
		Dim R__ As Double
		Dim K As Double

		Dim pReleation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPoly As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLine1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt1400 As ESRI.ArcGIS.Geometry.IPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pPath As ESRI.ArcGIS.Geometry.IPath

		Dim Dushdu As Boolean
		Dim Side0 As Integer
		Dim Side1 As Integer
		Dim cIx As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		pTmpPoint = New ESRI.ArcGIS.Geometry.Point

		If _FinalNav.TypeCode = eNavaidType.CodeLLZ Then
			txtBox_TrueBRGRange.Text = CStr(_FinalNav.pPtGeo.M)
			m_ArDir = _FinalNav.pPtPrj.M
			'        Set FictTHR = PointAlongPlane(_FinalNav.pPtPrj, _FinalNav.pPtPrj.M + 180.0, _FinalNav.LLZ_THR)
			FictTHR = New ESRI.ArcGIS.Geometry.Point
			pConstruct = FictTHR
			pConstruct.ConstructAngleIntersection(RWYTHRPrj, DegToRad(_FinalNav.pPtPrj.M + 90.0), _FinalNav.pPtPrj, DegToRad(_FinalNav.pPtPrj.M))

			FictTHR.Z = CurrADHP.pPtGeo.Z
		Else
			txtBox_TrueBRGRange.Text = CStr(Modulus(NewAzt, 360.0))
			m_ArDir = Azt2Dir(_FinalNav.pPtGeo, NewAzt)

			pLine = New ESRI.ArcGIS.Geometry.Polyline
			pPoly = pLine
			N = UBound(RWYDATA)

			For I = 0 To N Step 2
				If RWYDATA(I).Selected Then
					pPath = New ESRI.ArcGIS.Geometry.Path
					pPath.FromPoint = RWYDATA(I).pPtPrj(eRWY.PtTHR)
					pPath.ToPoint = RWYDATA(I + 1).pPtPrj(eRWY.PtTHR)
					pPoly.AddGeometry(pPath)
				End If
			Next I

			pLine1 = New ESRI.ArcGIS.Geometry.Polyline
			pLine1.FromPoint = PointAlongPlane(_FinalNav.pPtPrj, m_ArDir, 400000.0)
			pLine1.ToPoint = PointAlongPlane(_FinalNav.pPtPrj, m_ArDir + 180.0, 400000.0)
			pReleation = pLine

			If pReleation.Crosses(pLine1) Then
				pTopo = pLine
				pPoints = pTopo.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)

				pProxi = pLine1.ToPoint
				MaxDh = 900000.0
				J = -1

				For I = 0 To pPoints.PointCount - 1
					fDist = pProxi.ReturnDistance(pPoints.Point(I))
					'fTmp = Azt2Dir(CurrADHP.pPtGeo, pPoints.Point(I).M)
					If fDist < MaxDh Then 'And (SubtractAngles(fTmp + 180.0, ArDir) <= 90.0)
						J = I
						MaxDh = fDist
					End If
				Next I

				If J >= 0 Then
					FictTHR = pPoints.Point(J)
				ElseIf pPoints.PointCount > 0 Then
					FictTHR = pPoints.Point(0)
				Else
					FictTHR = New ESRI.ArcGIS.Geometry.Point
				End If
			Else
				pProxi = pLine1
				pPoints = pLine
				MaxDh = 90000.0
				J = -1
				For I = 0 To pPoints.PointCount - 1
					fDist = pProxi.ReturnDistance(pPoints.Point(I))
					fTmp = Azt2Dir(CurrADHP.pPtGeo, pPoints.Point(I).M)
					If (fDist < MaxDh) And (SubtractAngles(fTmp + 180.0, m_ArDir) <= 90.0) Then
						J = I
						MaxDh = fDist
					End If
				Next I

				FictTHR = New ESRI.ArcGIS.Geometry.Point
				If J >= 0 Then
					ptTmp = pPoints.Point(J)
					pConstruct = FictTHR
					pConstruct.ConstructAngleIntersection(_FinalNav.pPtPrj, DegToRad(m_ArDir), ptTmp, DegToRad(m_ArDir + 90.0))
				Else
					FictTHR.PutCoords(pPoints.Point(0).X, pPoints.Point(0).Y)
				End If
			End If

			'_Label0201_3.Text = My.Resources.str10222  '"Расстояние РНС от ближайщей" + Chr(13) + "поверхности ВПП:"
			'_Label0201_10.Text = CStr(ConvertHeight(ReturnDistanceInMeters(FictTHR, _FinalNav.pPtPrj), 2))
			FictTHR.Z = CurrADHP.pPtGeo.Z
		End If
		'================================================================
		If _FinalNav.TypeCode = 0 Then
			FA_Range = VOR.FA_Range
		ElseIf _FinalNav.TypeCode = 2 Then
			FA_Range = NDB.FA_Range
		ElseIf _FinalNav.TypeCode = 3 Then
			FA_Range = LLZ.Range
		End If
		K = 2 * FA_Range

		pLine = New ESRI.ArcGIS.Geometry.Polyline
		pLine.AddPoint(_FinalNav.pPtPrj)
		pLine.AddPoint(PointAlongPlane(_FinalNav.pPtPrj, m_ArDir + 180.0, FA_Range))

		Ss = CDbl(txtBox_ARCSemiSpan.Text)
		Vs = CDbl(txtBox_ARCVeticalDimension.Text)

		CreateNavaidZone(_FinalNav, m_ArDir, FictTHR, Ss, Vs, FA_Range, K, pPolyLeft, pPolyRight, pPolyPrime)

		Dim aStr(2) As String
		aStr(0) = My.Resources.str10315
		aStr(1) = ""
		aStr(2) = My.Resources.str10314

		fAlignOCH = 0


		fMinOCH = fVisAprOCH

		On Error Resume Next
		If Not FASegmElement Is Nothing Then pGraphics.DeleteElement(FASegmElement)
		If Not LeftPolyElement Is Nothing Then pGraphics.DeleteElement(LeftPolyElement)
		If Not RightPolyElement Is Nothing Then pGraphics.DeleteElement(RightPolyElement)
		If Not PrimePolyElement Is Nothing Then pGraphics.DeleteElement(PrimePolyElement)

		pClone = pPolyLeft
		pTmpPoly = pClone.Clone
		pTopo = pTmpPoly
		pTopo.Simplify()
		LeftPolyElement = DrawPolygon(pTmpPoly, RGB(0, 0, 255))

		pTopo = pPolyPrime
		pTopo.Simplify()

		NavGuadArea2 = pTopo.Union(pTmpPoly)

		pClone = pPolyRight
		pTmpPoly = pClone.Clone
		pTopo = pTmpPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		RightPolyElement = DrawPolygon(pTmpPoly, RGB(0, 0, 255))

		pTopo = NavGuadArea2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		NavGuadArea2 = pTopo.Union(pTmpPoly)

		pTopo = NavGuadArea2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		NavGuadArea2 = pTopo.Union(pPolyLeft)

		pTopo = NavGuadArea2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		PrimePolyElement = DrawPolygon(pPolyPrime, 0)
		FASegmElement = DrawPolyLine(pLine, RGB(0, 0, 255))
		FASegmElement.Locked = True
		LeftPolyElement.Locked = True
		RightPolyElement.Locked = True
		PrimePolyElement.Locked = True

		On Error GoTo 0
	End Sub

	Private Sub NavInSector(ByRef ForNavaid As NavaidType, ByRef InList() As NavaidType, ByRef OutList() As InSectorNav, ByRef pSector As ESRI.ArcGIS.Geometry.IPolygon2, ByRef fLow As Double)
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim tmpDist As Double
		Dim TmpDir As Double
		Dim fTmp As Double
		Dim d0 As Double

		Dim pSectProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		N = UBound(InList)

		If pSector.IsEmpty() Then
			ReDim OutList(-1)
			Return
		Else
			ReDim OutList(N + 1)
		End If

		pSectProxi = pSector

		J = -1

		For I = 0 To N
			tmpDist = ReturnDistanceFromGeomInMeters(ForNavaid.pPtPrj, InList(I).pPtPrj)
			If tmpDist <= distEps Then Continue For
			If pSectProxi.ReturnDistance(InList(I).pPtPrj) > 0.0 Then Continue For

			If (InList(I).TypeCode = 0) Or (InList(I).TypeCode = 2) Or (InList(I).TypeCode = 4) Then
				J = J + 1
				OutList(J) = NavaidType2InSectorNav(InList(I))

				'OutList(J).pPtGeo = InList(I).pPtGeo
				'OutList(J).pPtPrj = InList(I).pPtPrj
				'OutList(J).Name = InList(I).Name
				'OutList(J).CallSign = InList(I).CallSign
				'OutList(J).ID = InList(I).ID

				'OutList(J).Range = InList(I).Range
				'OutList(J).MagVar = InList(I).MagVar
				'OutList(J).Index = InList(I).Index

				'OutList(J).TypeCode = InList(I).TypeCode
				'OutList(J).TypeName_Renamed = InList(I).TypeName_Renamed
				OutList(J).Distance = Geometry2LineDistancePrj(OutList(J).pPtPrj, RWYTHRPrj, RWYTHRPrj.M - 90.0) * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, OutList(J).pPtPrj)

				'            If tmpDist > distEps Then
				'            If OutList(J).Type = 0 Then
				'                d0 = VOR.OnNAVRadius
				'            Else
				'                d0 = NDB.OnNAVRadius
				'            End If
				d0 = OnNAVShift(OutList(J).TypeCode, 150.0)

				TmpDir = ReturnAngleInDegrees(ForNavaid.pPtPrj, OutList(J).pPtPrj)
				If SubtractAngles(TmpDir, m_ArDir) > 90.0 Then TmpDir = TmpDir + 180.0

				fTmp = RadToDeg(ArcSin(d0 / tmpDist))
				OutList(J).FromAngle = Modulus(TmpDir - fTmp, 360.0)
				OutList(J).ToAngle = Modulus(TmpDir + fTmp, 360.0)
				'            Else
				'                OutList(J).FromAngle = fHigh
				'                OutList(J).ToAngle = fLow
				'            End If
				OutList(J).Tag = 0
			End If
		Next I

		J = J + 1
		OutList(J) = NavaidType2InSectorNav(ForNavaid)
		'OutList(J).pPtGeo = ForNavaid.pPtGeo
		'OutList(J).pPtPrj = ForNavaid.pPtPrj
		'OutList(J).Name = ForNavaid.Name
		'OutList(J).CallSign = ForNavaid.CallSign
		'OutList(J).ID = ForNavaid.ID

		'OutList(J).Range = ForNavaid.Range
		'OutList(J).MagVar = ForNavaid.MagVar
		'OutList(J).Index = ForNavaid.Index

		'OutList(J).TypeCode = ForNavaid.TypeCode
		'OutList(J).TypeName_Renamed = ForNavaid.TypeName_Renamed
		OutList(J).Distance = Geometry2LineDistancePrj(OutList(J).pPtPrj, RWYTHRPrj, RWYTHRPrj.M - 90.0) * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, OutList(J).pPtPrj)

		'            If tmpDist > distEps Then
		'            If OutList(J).Type = 0 Then
		'                d0 = VOR.OnNAVRadius
		'            Else
		'                d0 = NDB.OnNAVRadius
		'            End If
		d0 = OnNAVShift(OutList(J).TypeCode, 150.0)

		OutList(J).FromAngle = 0
		OutList(J).ToAngle = 360.0
		OutList(J).Tag = 1

		If J < 0 Then
			ReDim OutList(-1)
		Else
			ReDim Preserve OutList(J)
		End If
	End Sub

    Private Sub txtBox_TrueBRGRange_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_TrueBRGRange.KeyPress
        If e.KeyChar = Chr(13) Then
            txtBox_TrueBRGRange_Validating(txtBox_TrueBRGRange, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxInteger(e.KeyChar)
        End If

        If e.KeyChar = Chr(0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtBox_TrueBRGRange_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtBox_TrueBRGRange.Validating
        Dim Cancel As Boolean = e.Cancel
        Dim iTmp As Integer

        If Not IsNumeric(txtBox_TrueBRGRange.Text) Then txtBox_TrueBRGRange.Text = CStr(nmrcUpDown_TrueBRGRange.Value)

        iTmp = CInt(txtBox_TrueBRGRange.Text)

        If iTmp < nmrcUpDown_TrueBRGRange.Minimum Then iTmp = nmrcUpDown_TrueBRGRange.Minimum
        If iTmp > nmrcUpDown_TrueBRGRange.Maximum Then iTmp = nmrcUpDown_TrueBRGRange.Maximum

        nmrcUpDown_TrueBRGRange.Value = iTmp
        e.Cancel = Cancel
    End Sub

    Private Sub txtBox_ARCSemiSpan_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_ARCSemiSpan.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)
        If KeyAscii = 13 Then
            txtBox_ARCSemiSpan_Validating(txtBox_ARCSemiSpan, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_ARCSemiSpan.Text))
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtBox_ARCSemiSpan_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtBox_ARCSemiSpan.Validating
        Dim lSs As Double
        Dim fTmp As Double

        If Not IsNumeric(txtBox_ARCSemiSpan.Text) Then Return
        lSs = CDbl(txtBox_ARCSemiSpan.Text)
        fTmp = lSs

        If lSs < arSemiSpan.Values(Category) Then lSs = arSemiSpan.Values(Category)
        If lSs > 60.0 Then lSs = 60.0
        If lSs <> fTmp Then txtBox_ARCSemiSpan.Text = CStr(lSs)
    End Sub

    Private Sub txtBox_ARCVeticalDimension_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_ARCVeticalDimension.KeyPress
        If e.KeyChar = Chr(13) Then
            txtBox_ARCVeticalDimension_Validating(txtBox_ARCVeticalDimension, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxInteger(e.KeyChar)
        End If

        If e.KeyChar = Chr(0) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtBox_ARCVeticalDimension_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtBox_ARCVeticalDimension.Validating
        Dim lVs As Double
        Dim fTmp As Double
        If Not IsNumeric(txtBox_ARCVeticalDimension.Text) Then Return
        lVs = CDbl(txtBox_ARCVeticalDimension.Text)
        fTmp = lVs

        If lVs < arVerticalSize.Values(Category) Then lVs = arVerticalSize.Values(Category)
        If lVs > 10.0 Then lVs = 10.0
        If lVs <> fTmp Then txtBox_ARCVeticalDimension.Text = CStr(lVs)
    End Sub

    Private Sub txtBox_TrueBRGRange_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBox_TrueBRGRange.TextChanged
        _initialDirectionAngle = CDbl(txtBox_TrueBRGRange.Text)
    End Sub
End Class
