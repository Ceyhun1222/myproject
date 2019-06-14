
Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.DisplayUI
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ADF.CATIDs

<Guid("052096A1-0214-454C-99D1-2999DA33FDA2"), ClassInterface(ClassInterfaceType.None), ProgId("TextCallouts.SigmaLineCalloutPropertyPage")> Public Class SigmaCalloutPropertyPage
	Inherits System.Windows.Forms.UserControl
	Implements IComPropertyPage, IPropertyPageContext, ISymbolPropertyPage


#Region "COM Registration Function(s)"

    'Private Property DME As Boolean

    <ComRegisterFunction(), ComVisible(False)> _
    Private Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        '
        ' TODO: Add any COM registration code here
        '
    End Sub

	<ComUnregisterFunction(), ComVisible(False)> _
	Private Shared Sub UnregisterFunction(ByVal registerType As Type)
		' Required for ArcGIS Component Category Registrar support
		ArcGISCategoryUnregistration(registerType)

		'
		' TODO: Add any COM unregistration code here
		'
	End Sub

#Region "ArcGIS Component Category Registrar generated code"
	''' <summary>
	''' Required method for ArcGIS Component Category registration -
	''' Do not modify the contents of this method with the code editor.
	''' </summary>
	Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
		Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
		PropertyPages.Register(regKey)
		SymbolPropertyPages.Register(regKey)
	End Sub
	''' <summary>
	''' Required method for ArcGIS Component Category unregistration -
	''' Do not modify the contents of this method with the code editor.
	''' </summary>
	Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
		Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
		PropertyPages.Unregister(regKey)
		SymbolPropertyPages.Unregister(regKey)
	End Sub

#End Region
#End Region

	Private _priority As Integer = 11
	'Initialise the Title
	Private _pageTitle As String = "Sigma Callout"

	Private _isDirty As Boolean
	Private _pageSite As IComPropertyPageSite
	Private _sigmaCallout As RISKCallouts.SigmaCallout
	Private _units As esriUnits

	'Private m_pCallout As ICallout
	'Private m_pCustTextCallout As TextCallouts.SigmaLineCallout
	'Private m_pTextCallout As ICallout
    Private _leaderSym As LineSymbol
    Private _snap As Integer
    Private _dme As Boolean
	Private _leaderTolerance As Double
	Private _size As Double
	Private _color As IRgbColor 'IColor
	Private _topMargine As Double
	Private _leftMargine As Double
	Private _rightMargine As Double
	Private _bottomtMargine As Double

#Region "class constructor"
	Public Sub New()
		InitializeComponent()

		_color = New RgbColor
		_color.Red = 0
		_color.Green = 255
		_color.Blue = 255
        _snap = 0
        _dme = False
		_leaderTolerance = 1
		_size = 1.0

		_topMargine = 1
		_leftMargine = 1
		_rightMargine = 1
		_bottomtMargine = 1

		_units = esriUnits.esriPoints
    End Sub

	Protected Overrides Sub Finalize()
		_color = Nothing
		MyBase.Finalize()
	End Sub

#End Region

#Region "IComPropertyPage Members"

	Private Property Title() As String Implements IComPropertyPage.Title
		Get
			Return _pageTitle
		End Get

		'TODO: Uncomment if title can be modified
		Set(ByVal Value As String)
			'm_pageTitle = Value
		End Set
	End Property

	Private ReadOnly Property Width1() As Integer Implements IComPropertyPage.Width
		Get
			Return Me.Width
		End Get
	End Property

	Private ReadOnly Property Height1() As Integer Implements IComPropertyPage.Height
		Get
			Return Me.Height
		End Get
	End Property

	Private Function Activate() As Integer Implements IComPropertyPage.Activate
		Return Me.Handle.ToInt32()
	End Function

	Private Sub Deactivate() Implements IComPropertyPage.Deactivate
		'dispose any unnecessary resources

		Me.Dispose(True)
	End Sub

	Private Function Applies(ByVal Objects As ISet) As Boolean Implements IComPropertyPage.Applies
		'Uses IPropertyPageContext::Applies instead
	End Function

	Private Sub SetObjects(ByVal Objects As ISet) Implements IComPropertyPage.SetObjects
		If Objects Is Nothing OrElse Objects.Count = 0 Then
			Return
		End If

		Objects.Reset()
		_sigmaCallout = Objects.Next	'QI
		'm_pCallout = m_pSigmaCallout	'QI
	End Sub

	Private WriteOnly Property PageSite() As IComPropertyPageSite Implements IComPropertyPage.PageSite
		Set(ByVal Value As IComPropertyPageSite)
			_pageSite = Value
		End Set
	End Property

	Private ReadOnly Property IsPageDirty() As Boolean Implements IComPropertyPage.IsPageDirty
		Get
			'If the form has changed return True for IsPageDirty
			Return _isDirty
		End Get
	End Property

	Private Sub Apply() Implements IComPropertyPage.Apply
		If Not _isDirty Then Return

		QueryObject(_sigmaCallout)

		IsDirty = False
	End Sub

	Private Sub Cancel() Implements IComPropertyPage.Cancel
		If _isDirty Then
			IsDirty = False
		End If
	End Sub

    Private Sub CheckSnap(val As Integer)
        Select Case val
            Case 0
                _Option1_0.Checked = True
            Case 1
                _Option1_1.Checked = True
            Case 2
                _Option1_2.Checked = True
            Case 3
                _Option1_3.Checked = True
            Case 4
                _Option1_4.Checked = True
            Case 5
                _Option1_5.Checked = True
            Case 6
                _Option1_6.Checked = True
            Case 7
                _Option1_7.Checked = True
            Case 8
                _Option1_8.Checked = True
            Case 9
                _Option1_9.Checked = True
        End Select
    End Sub
    Private Sub Show1() Implements IComPropertyPage.Show
        'Show the property page.
        'Also update the content of the property page with the current data
        'from the Map object
        On Error GoTo ErrorHandler

        If Not _sigmaCallout Is Nothing Then
            '_leaderTolerance = PointsToMapUnits(_sigmaCallout.LeaderTolerance)
            _leaderTolerance = MapUnitsToPoints(_sigmaCallout.LeaderTolerance)

            _color = _sigmaCallout.Color
            _snap = _sigmaCallout.Snap
            _dme = _sigmaCallout.DME
            _size = _sigmaCallout.Size
            _topMargine = _sigmaCallout.TopMargine
            _leftMargine = _sigmaCallout.LeftMargine
            _rightMargine = _sigmaCallout.RightMargine
            _bottomtMargine = _sigmaCallout.BottomMargine

            numericToler.Value = _leaderTolerance

            pnlColor.BackColor = Drawing.Color.FromArgb(_color.Red, _color.Green, _color.Blue)
            'btnSelectColor.BackColor = Drawing.Color.FromArgb(_color.RGB Or (255 << 24))
            'btnSelectColor.ForeColor = Drawing.Color.FromArgb((Not _color.RGB) Or (255 << 24))
            CheckSnap(_snap)
            dmeCheck.Checked = _dme
            numericSize.Value = _size
            numericTopMarg.Value = _topMargine
            numericLeftMarg.Value = _leftMargine
            numericRightMarg.Value = _rightMargine
            numericBottomMarg.Value = _bottomtMargine
        End If

        TryCast(Me, UserControl).Show()
        Return
ErrorHandler:

        'Visible = True
    End Sub

    Private Sub Hide1() Implements IComPropertyPage.Hide
        TryCast(Me, UserControl).Hide()
    End Sub

    Private ReadOnly Property HelpFile() As String Implements IComPropertyPage.HelpFile
        Get
            Return String.Empty
        End Get
    End Property
    Private ReadOnly Property HelpContextID(ByVal controlID As Integer) As Integer Implements IComPropertyPage.HelpContextID
        Get
            Return 0
        End Get
    End Property

    Private Property Priority() As Integer Implements IComPropertyPage.Priority
        Get
            Return _priority
        End Get

        Set(ByVal Value As Integer)
            'm_lPriority = Value
        End Set
    End Property

#End Region

#Region "IPropertyPageContext Implementation"

    Private Function Applies(ByVal unkArray As Object) As Boolean Implements IPropertyPageContext.Applies
        Dim arr As Object() = CType(unkArray, Object())
        If Nothing Is arr OrElse 0 = arr.Length Then
            Return False
        End If

        Dim i As Integer = 0
        Do While i < arr.Length
            If TypeOf arr(i) Is SigmaCallout Then
                Return True
            End If
            i += 1
        Loop

        Return False
    End Function

    Private Sub Cancel1() Implements IPropertyPageContext.Cancel
        'IsDirty = False
    End Sub

    Private Function CreateCompatibleObject(ByVal kind As Object) As Object Implements IPropertyPageContext.CreateCompatibleObject
        Return Nothing
    End Function

    Private Function GetHelpFile(ByVal controlID As Integer) As String Implements IPropertyPageContext.GetHelpFile
        Return String.Empty
    End Function

    Private Function GetHelpId(ByVal controlID As Integer) As Integer Implements IPropertyPageContext.GetHelpId
        Return -1
    End Function

    Private ReadOnly Property Priority1() As Integer Implements IPropertyPageContext.Priority
        Get
            Return _priority
        End Get
    End Property

    Private Sub QueryObject(ByVal theObject As Object) Implements IPropertyPageContext.QueryObject
        Dim sgmCallout As SigmaCallout = CType(theObject, ISigmaCallout)

        'sgmCallout.LeaderTolerance = MapUnitsToPoints(_leaderTolerance)
        sgmCallout.LeaderTolerance = PointsToMapUnits(_leaderTolerance)
        sgmCallout.Color = _color
        sgmCallout.Snap = _snap
        sgmCallout.DME = _dme
        sgmCallout.Size = _size
        sgmCallout.TopMargine = _topMargine
        sgmCallout.LeftMargine = _leftMargine
        sgmCallout.RightMargine = _rightMargine
        sgmCallout.BottomMargine = _bottomtMargine
    End Sub

#End Region

#Region "ISymbolPropertyPage  Implementation"

    Private Property ISymbolPropertyPage_Units() As esriUnits Implements ISymbolPropertyPage.Units
        Get
            Return _units
        End Get

        Set(ByVal Value As esriUnits)
            _units = Value
        End Set
    End Property

#End Region

#Region "private methods"

    Public Property IsDirty() As Boolean
        Get
            Return _isDirty
        End Get

        Set(ByVal Value As Boolean)
            If _isDirty <> Value Then
                _isDirty = Value
                If Not _pageSite Is Nothing Then _pageSite.PageChanged()
                CType(Me, IComPropertyPage).Apply()
            End If
        End Set
    End Property

    Private Function PointsToMapUnits(ByRef val As Double) As Double
        'Converts from points to ESRI map unites
        Select Case _units
            Case esriUnits.esriMillimeters
                Return val * (25.4 / 72.0)
            Case esriUnits.esriCentimeters
                Return val * (2.54 / 72.0)
            Case esriUnits.esriInches
                Return val / 72.0
        End Select

        Return val
    End Function

    Private Function MapUnitsToPoints(ByRef val As Double) As Double
        'Convert from ESRI map unites to points
        Select Case _units
            Case esriUnits.esriMillimeters
                Return val * (72.0 / 25.4)
            Case esriUnits.esriCentimeters
                Return val * (72.0 / 2.54)
            Case esriUnits.esriInches
                Return val * 72.0
        End Select

        Return val
    End Function

    Private Sub PropertyPageUI_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'Reset the IsDirty flag when the form is loaded
        _isDirty = False
    End Sub

    Private Sub btnSelectColor_Click(sender As Object, e As EventArgs) Handles btnSelectColor.Click
        Dim pColorBrowser As IColorBrowser = New ColorBrowser
        pColorBrowser.Color = _color

        If pColorBrowser.DoModal(Me.Handle.ToInt32) Then
            _color = pColorBrowser.Color
            pnlColor.BackColor = Drawing.Color.FromArgb(_color.Transparency, _color.Red, _color.Green, _color.Blue)

            '	btnSelectColor.BackColor = Drawing.Color.FromArgb(_color.RGB)
            '	btnSelectColor.ForeColor = Drawing.Color.FromArgb(Not _color.RGB)
            IsDirty = True
        End If

        'Return

        'colorDialog1.Color = Drawing.Color.FromArgb(_color.Red, _color.Green, _color.Blue)

        'If colorDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
        '	'get the selected color
        '	Dim newColor As Color = colorDialog1.Color
        '	pnlColor.BackColor = newColor

        '	_color.Red = newColor.R
        '	_color.Green = newColor.G
        '	_color.Blue = newColor.B

        '	'btnSelectColor.BackColor = newColor
        '	'btnSelectColor.ForeColor = Drawing.Color.FromArgb((Not newColor.ToArgb()) Or (255 << 24))
        '	IsDirty = True
        'End If
    End Sub

    Private Sub _Option1_Click(sender As Object, e As EventArgs) Handles _Option1_0.Click, _Option1_9.Click, _Option1_8.Click, _Option1_7.Click, _Option1_6.Click, _Option1_5.Click, _Option1_4.Click, _Option1_3.Click, _Option1_2.Click, _Option1_1.Click
        Dim iTmp As Integer = CInt(sender.Tag)

        Label8.Text = CStr(iTmp)
        If iTmp <> _snap Then
            _snap = iTmp
            IsDirty = True
        End If
    End Sub

	Private Sub numericToler_ValueChanged(sender As Object, e As EventArgs) Handles numericToler.ValueChanged
		If numericToler.Value <> _leaderTolerance Then
			_leaderTolerance = CDbl(numericToler.Value)
			IsDirty = True
		End If
	End Sub

	Private Sub numericSize_ValueChanged(sender As Object, e As EventArgs) Handles numericSize.ValueChanged
		If numericSize.Value <> _size Then
			_size = CDbl(numericSize.Value)
			IsDirty = True
		End If
	End Sub

	Private Sub numericTopMarg_ValueChanged(sender As Object, e As EventArgs) Handles numericTopMarg.ValueChanged
		If numericTopMarg.Value <> _topMargine Then
			_topMargine = CDbl(numericTopMarg.Value)
			IsDirty = True
		End If
	End Sub

	Private Sub numericLeftMarg_ValueChanged(sender As Object, e As EventArgs) Handles numericLeftMarg.ValueChanged
		If numericLeftMarg.Value <> _leftMargine Then
			_leftMargine = CDbl(numericLeftMarg.Value)
			IsDirty = True
		End If
	End Sub

	Private Sub numericRightMarg_ValueChanged(sender As Object, e As EventArgs) Handles numericRightMarg.ValueChanged
		If numericRightMarg.Value <> _rightMargine Then
			_rightMargine = CDbl(numericRightMarg.Value)
			IsDirty = True
		End If
	End Sub

	Private Sub numericBottomMarg_ValueChanged(sender As Object, e As EventArgs) Handles numericBottomMarg.ValueChanged
		If numericBottomMarg.Value <> _bottomtMargine Then
			_bottomtMargine = CDbl(numericBottomMarg.Value)
			IsDirty = True
		End If
	End Sub

#End Region

   
    Private Sub dmeCheck_CheckedChanged(sender As Object, e As EventArgs) Handles dmeCheck.CheckedChanged
        If dmeCheck.Checked <> _dme Then
            _dme = dmeCheck.Checked
            IsDirty = True
        End If
    End Sub

    'Private Sub cmdLeadSym_Click(sender As Object, e As EventArgs) Handles cmdLeadSym.Click
    '    cmdLeadSym.Enabled = False
    '    Dim pLineSym As ISimpleLineSymbol
    '    Dim pSymbolSelect As ISymbolSelector
    '    pSymbolSelect = New SymbolSelector

    '    If _leaderSym Is Nothing Then
    '        Dim pRgbColor As IRgbColor
    '        pRgbColor = New RgbColor
    '        With pRgbColor
    '            .Red = 135
    '            .Green = 135
    '            .Blue = 135
    '            .NullColor = False
    '        End With
    '        Dim pLeader As ISimpleLineSymbol
    '        pLeader = New SimpleLineSymbol
    '        With pLeader
    '            .Color = pRgbColor
    '            .Style = esriSimpleLineStyle.esriSLSSolid
    '        End With
    '        pLineSym = pLeader
    '    Else
    '        pLineSym = _leaderSym
    '    End If

    '    If Not pSymbolSelect.AddSymbol(pLineSym) Then
    '        MsgBox("Unexpected error! Unable to change the Leader Symbol!")
    '        cmdLeadSym.Enabled = True
    '        Exit Sub
    '    Else
    '        If pSymbolSelect.SelectSymbol(0) Then
    '            _leaderSym = pSymbolSelect.GetSymbolAt(0)
    '        Else
    '            cmdLeadSym.Enabled = True
    '            Exit Sub
    '        End If
    '    End If
    '    _isDirty = True
    '    'Set m_pLeaderSymbol = m_pCustTextCallout.LeaderSym
    '    'If (Not m_pPageSite Is Nothing) Then m_pPageSite.PageChanged()
    '    cmdLeadSym.Enabled = True
    'End Sub

End Class
