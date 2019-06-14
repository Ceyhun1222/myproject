Imports Aran.AranEnvironment
Imports Microsoft.Win32
Imports Aran.Aim
Imports System.Collections.Generic

'#Const IncludeVisualManoeuvring = True

Public Class ArrivalAranPlugin
    Inherits AranPlugin

    Const _name As String = "PANDA Conventional - Arrival"

    'Dim _isInit As Boolean
    Dim _tsmiNonPrecArrival As ToolStripMenuItem
    Dim _tsmiPrecArrival As ToolStripMenuItem
    Dim _tsmiInitialArea As ToolStripMenuItem
    Dim _tsmiDeadRekconing As ToolStripMenuItem
    Dim _tsmiVisualManoeuvring As ToolStripMenuItem

#Region "IAranPlugin Members"

	Public Overrides ReadOnly Property Id As Guid
		Get
			Return New Guid("d548ff47-74c9-49ab-aa9a-80161ed94a27")
		End Get
	End Property

	Public Overrides ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public Overrides Function GetLayerFeatureTypes() As List(Of FeatureType)
        Dim featTypes As List(Of FeatureType) = New List(Of FeatureType)()

        featTypes.Add(FeatureType.AirportHeliport)
        featTypes.Add(FeatureType.VerticalStructure)
        featTypes.Add(FeatureType.VOR)
        featTypes.Add(FeatureType.DME)
        featTypes.Add(FeatureType.NDB)
		featTypes.Add(FeatureType.InstrumentApproachProcedure)
		featTypes.Add(FeatureType.DesignatedPoint)
        featTypes.Add(FeatureType.RunwayCentrelinePoint)

        Return featTypes
    End Function

    Public Overrides Sub Startup(aranEnv As AranEnvironment.IAranEnvironment)
        gAranEnv = aranEnv
        gHookHelper = New ESRI.ArcGIS.Controls.HookHelper
        gHookHelper.Hook = gAranEnv.HookObject

        LangCode = 1033

        SetThreadLocale(LangCode)
        Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)


        Dim menuItem As New ToolStripMenuItem()
        menuItem.Text = My.Resources.str00080
        _tsmiNonPrecArrival = New ToolStripMenuItem()
        _tsmiNonPrecArrival.Text = My.Resources.str00157  'NonPrecApproach
        _tsmiNonPrecArrival.Tag = 0
        AddHandler _tsmiNonPrecArrival.Click, AddressOf NonPrecArrival_Click
        menuItem.DropDownItems.Add(_tsmiNonPrecArrival)
        _tsmiPrecArrival = New ToolStripMenuItem()
        _tsmiPrecArrival.Text = My.Resources.str00158 'PrecApproach
        _tsmiPrecArrival.Tag = 1
        AddHandler _tsmiPrecArrival.Click, AddressOf PrecArrival_Click
        menuItem.DropDownItems.Add(_tsmiPrecArrival)

        _tsmiInitialArea = New ToolStripMenuItem()
        _tsmiInitialArea.Text = My.Resources.str00159 'InitialArea
        _tsmiInitialArea.Tag = 2
        AddHandler _tsmiInitialArea.Click, AddressOf InitialArea_Click
        menuItem.DropDownItems.Add(_tsmiInitialArea)

        _tsmiDeadRekconing = New ToolStripMenuItem()
        _tsmiDeadRekconing.Text = My.Resources.str00160   'DeadRekconing
        _tsmiDeadRekconing.Tag = 3
        AddHandler _tsmiDeadRekconing.Click, AddressOf DeadRekconing_Click
        menuItem.DropDownItems.Add(_tsmiDeadRekconing)

#If IncludeVisualManoeuvring Then
        'Aran.PANDA.VisualManoeuvring.GlobalVars.gAranEnv = aranEnv
        Aran.PANDA.VisualManoeuvring.GlobalVars.gAranEnv = aranEnv

        _tsmiVisualManoeuvring = New ToolStripMenuItem()
        _tsmiVisualManoeuvring.Text = "Visual Manoeuvring"
        _tsmiVisualManoeuvring.Tag = 4
        AddHandler _tsmiVisualManoeuvring.Click, AddressOf VisualManoeuvring_Click
        menuItem.DropDownItems.Add(_tsmiVisualManoeuvring)
#End If

		aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem)
    End Sub
#End Region

    Public Function InitCommands() As Boolean
        Return InitCommand() > 0
    End Function

	Public Sub NonPrecArrival_Click(sender As Object, e As EventArgs)
		If CurrCmd = 1 Then Return
		CurrCmd = 0

		Try
            If (SlotSelector()=False)
                Return
            End If
            NativeMethods.ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

            If InitCommands() Then

				Dim ArrivalFrm As CArrivalFrm = New CArrivalFrm

				If ArrivalFrm.IsDisposed Then
					NativeMethods.HidePandaBox()
					Return
				End If

				CurrCmd = 1
				AddHandler ArrivalFrm.FormClosed, AddressOf AllForms_FormClosed

                'If toolbar Is Nothing OrElse toolbar.IsDisposed Then
                '	toolbar = New ArrivalToolbar
                'End If

                _tsmiNonPrecArrival.Checked = True
                _tsmiPrecArrival.Enabled = False
                _tsmiInitialArea.Enabled = False
                _tsmiDeadRekconing.Enabled = False
#If IncludeVisualManoeuvring Then
                _tsmiVisualManoeuvring.Enabled = False
#End If
				ArrivalFrm.Show(_Win32Window)
				ArrivalFrm.ListView0001.Focus()

			End If

			NativeMethods.HidePandaBox()

		Catch aranEx As AranException
			NativeMethods.HidePandaBox()
			If Not aranEx.Handled Then
				aranEx.ShowMessageBox()
			End If
		Catch exception As Exception
			NativeMethods.HidePandaBox()
            MessageBox.Show("Form initialization error."+exception.Message,"Arrival",MessageBoxButtons.OK,MessageBoxIcon.Error)
		End Try

		'If InitCommands() Then
		'	Dim tsb As ToolStripItem = CType(sender, ToolStripItem)
		'	tsb.Enabled = False

		'	Dim arrivalFrm As CArrivalFrm = New CArrivalFrm
		'	arrivalFrm.Show(s_Win32Window)
		'	arrivalFrm.ListView0001.Focus()

		'	AddHandler arrivalFrm.FormClosed, AddressOf AllForms_FormClosed
		'	arrivalFrm.Tag = tsb
		'End If
	End Sub

	Public Sub PrecArrival_Click(sender As Object, e As EventArgs)
		If CurrCmd = 2 Then Return
		CurrCmd = 0

		Try
            If (SlotSelector()=False)
                Return
            End If
			NativeMethods.ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

			If InitCommands() Then

				Dim PrecisionFrm As CPrecisionFrm = New CPrecisionFrm

				If PrecisionFrm.IsDisposed Then
					NativeMethods.HidePandaBox()
					Return
				End If

				CurrCmd = 2

				AddHandler PrecisionFrm.FormClosed, AddressOf AllForms_FormClosed

				If CommandBar Is Nothing OrElse CommandBar.IsDisposed Then
					CommandBar = New ArrivalToolbar
				End If

				_tsmiNonPrecArrival.Enabled = False
				_tsmiPrecArrival.Checked = True
				_tsmiInitialArea.Enabled = False
				_tsmiDeadRekconing.Enabled = False
#If IncludeVisualManoeuvring Then
                _tsmiVisualManoeuvring.Enabled = False
#End If
				'CommandBar.Show(s_Win32Window)

				Toolbar = CommandBar.Toolbar

				PrecisionFrm.Show(_Win32Window)
				PrecisionFrm.ComboBox0001.Focus()
			End If
			NativeMethods.HidePandaBox()
		Catch ex As Exception
			NativeMethods.HidePandaBox()
			Throw ex 'New Exception("Form initialization error.")
		End Try
	End Sub

	Public Sub InitialArea_Click(sender As Object, e As EventArgs)
		If CurrCmd = 3 Then Return
		CurrCmd = 0

		Try
            If (SlotSelector()=False)
                Return
            End If
			NativeMethods.ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

			If InitCommands() Then
				Dim InitialApproach As InitialApproach = New InitialApproach

				If InitialApproach.IsDisposed Then
					NativeMethods.HidePandaBox()
					Return
				End If

				CurrCmd = 3
				AddHandler InitialApproach.FormClosed, AddressOf AllForms_FormClosed

				_tsmiNonPrecArrival.Enabled = False
				_tsmiPrecArrival.Enabled = False
				_tsmiInitialArea.Checked = True
				_tsmiDeadRekconing.Enabled = False
#If IncludeVisualManoeuvring Then
                _tsmiVisualManoeuvring.Enabled = False
#End If
				InitialApproach.Show(_Win32Window)
				InitialApproach.ComboBox002.Focus()
			End If

			NativeMethods.HidePandaBox()

		Catch ex As Exception

			NativeMethods.HidePandaBox()

            MessageBox.Show("Form initialization error."+ex.Message)
			'Throw ex  'New Exception("Form initialization error.")
		End Try
	End Sub

	Public Sub DeadRekconing_Click(sender As Object, e As EventArgs)
		If CurrCmd = 4 Then Return
		CurrCmd = 0

		Try
            If (SlotSelector()=False)
                Return
            End If
			NativeMethods.ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

			If InitCommands() Then
				Dim InitialDeadApproach As CInitialDeadApproach = New CInitialDeadApproach

				If InitialDeadApproach.IsDisposed Then
					NativeMethods.HidePandaBox()
					Return
				End If

				CurrCmd = 4
				AddHandler InitialDeadApproach.FormClosed, AddressOf AllForms_FormClosed

				_tsmiNonPrecArrival.Enabled = False
				_tsmiPrecArrival.Enabled = False
				_tsmiInitialArea.Enabled = False
				_tsmiDeadRekconing.Checked = True
#If IncludeVisualManoeuvring Then
                _tsmiVisualManoeuvring.Enabled = False
#End If
				InitialDeadApproach.Show(_Win32Window)
				InitialDeadApproach.TextBox014.Focus()
			End If

			NativeMethods.HidePandaBox()

		Catch ex As Exception

			NativeMethods.HidePandaBox()

			Throw ex  'New Exception("Form initialization error.")
		End Try
	End Sub

#If IncludeVisualManoeuvring Then
    'Dim VisualManoeuvring As Aran.PANDA.VisualManoeuvring.Forms.MainForm

    'Public Sub VisualManoeuvring_Click(sender As Object, e As EventArgs)
    '    If CurrCmd = 5 Then Return
    '    CurrCmd = 0

    '    Try
    '        Aran.PANDA.VisualManoeuvring.GlobalVars.InitCommand()
    '        If VisualManoeuvring Is Nothing OrElse VisualManoeuvring.IsDisposed Then
    '            VisualManoeuvring = New Aran.PANDA.VisualManoeuvring.Forms.MainForm
    '        End If

    '        CurrCmd = 5
    '        AddHandler VisualManoeuvring.FormClosed, AddressOf AllForms_FormClosed

    '        _tsmiNonPrecArrival.Enabled = False
    '        _tsmiPrecArrival.Enabled = False
    '        _tsmiInitialArea.Enabled = False
    '        _tsmiDeadRekconing.Enabled = False
    '        _tsmiVisualManoeuvring.Checked = True

    '        VisualManoeuvring.Show(s_Win32Window)
    '        'VisualManoeuvring.NestedControl.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("Form initialization error.")
    '    End Try
    'End Sub


    Dim VisualManoeuvring As Aran.PANDA.VisualManoeuvring.Forms.MainForm

    Public Sub VisualManoeuvring_Click(sender As Object, e As EventArgs)
        If CurrCmd = 5 Then Return
        CurrCmd = 0

        Try
            NativeMethods.ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

            Aran.PANDA.VisualManoeuvring.GlobalVars.InitCommand()
            If VisualManoeuvring Is Nothing OrElse VisualManoeuvring.IsDisposed Then
                VisualManoeuvring = New Aran.PANDA.VisualManoeuvring.Forms.MainForm
            End If

            CurrCmd = 5
            AddHandler VisualManoeuvring.FormClosed, AddressOf AllForms_FormClosed

            _tsmiNonPrecArrival.Enabled = False
            _tsmiPrecArrival.Enabled = False
            _tsmiInitialArea.Enabled = False
            _tsmiDeadRekconing.Enabled = False
            _tsmiVisualManoeuvring.Checked = True

            VisualManoeuvring.Show(s_Win32Window)
            'VisualManoeuvring.NestedControl.Focus()

            NativeMethods.HidePandaBox()

        Catch ex As Exception

            NativeMethods.HidePandaBox()

            Throw ex  'New Exception("Form initialization error.")
        End Try
    End Sub
#End If

	Private Sub AllForms_FormClosed(sender As Object, e As FormClosedEventArgs)
		_tsmiNonPrecArrival.Checked = False
		_tsmiPrecArrival.Checked = False
		_tsmiInitialArea.Checked = False
		_tsmiDeadRekconing.Checked = False
#If IncludeVisualManoeuvring Then
        _tsmiVisualManoeuvring.Checked = False
#End If
		_tsmiNonPrecArrival.Enabled = True
		_tsmiPrecArrival.Enabled = True
		_tsmiInitialArea.Enabled = True
		_tsmiDeadRekconing.Enabled = True

        Toolbar = Nothing

		If Not (CommandBar Is Nothing) Then
			CommandBar.Close()
			CommandBar = Nothing
        End If
        
        Dim formd As System.Windows.Forms.Form= CType(sender, System.Windows.Forms.Form)  
        RemoveHandler formd.FormClosed,AddressOf AllForms_FormClosed  

        sender = Nothing
        GC.Collect()
		
        'If Not CommandBar Is Nothing And Not CommandBar.IsDisposed Then
		'	CommandBar.Dispose()
		'	CommandBar = Nothing
		'End If

		'If Not PrecisionFrm Is Nothing And Not PrecisionFrm.IsDisposed Then
		'	PrecisionFrm.Dispose()
		'End If

#If IncludeVisualManoeuvring Then
        _tsmiVisualManoeuvring.Enabled = True
#End If
		CurrCmd = 0
        

	End Sub
End Class
