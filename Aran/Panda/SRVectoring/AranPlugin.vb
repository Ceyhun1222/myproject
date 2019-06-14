Imports Aran.AranEnvironment
Imports Microsoft.Win32
Imports Aran.Aim
Imports System.Collections.Generic

Public Class SRVectoringAranPlugin
	Inherits AranPlugin

	Const _name As String = "PANDA Conventional - SRVectoring"
	Const _guid As String = "2B4F11D0-C354-4C2E-86A2-07D6F28375B9"

	'Private _isInit As Boolean
	Private _id As Guid
	Private _CurrCmd As Integer = -1
	Private _tsmiSRVectoring As ToolStripMenuItem

#Region "IAranPlugin Members"

	Public Overrides ReadOnly Property Name As String
		Get
			Return _name
		End Get
	End Property

	Public Overrides ReadOnly Property Id As Guid
		Get
			Return _id
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
		GlobalVars.gAranGraphics = aranEnv.Graphics

		gHookHelper = New ESRI.ArcGIS.Controls.HookHelper
		gHookHelper.Hook = gAranEnv.HookObject

		_id = New Guid(_guid)

		_tsmiSRVectoring = New ToolStripMenuItem()
		_tsmiSRVectoring.Text = My.Resources.str1000    '"Radar vectoring"
		_tsmiSRVectoring.Tag = 0
		'_isInit = False

		LangCode = 1033
		SetThreadLocale(LangCode)
		SRVectoring.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

		Dim menuItem As New ToolStripMenuItem()
		menuItem.Text = My.Resources.str1000
		AddHandler _tsmiSRVectoring.Click, AddressOf NonSRVectoring_Click
		menuItem.DropDownItems.Add(_tsmiSRVectoring)

		aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem)
	End Sub
#End Region

	Public Function InitCommands() As Boolean
		Return InitCommand() > 0
	End Function

	Public Sub NonSRVectoring_Click(sender As Object, e As EventArgs)
		If _CurrCmd = 1 Then Return
		_CurrCmd = -1

		Try
			If InitCommands() Then
				Dim ManevreForm As CFrmManevre = New CFrmManevre
				AddHandler ManevreForm.FormClosed, AddressOf AllForms_FormClosed
				ManevreForm.Show(s_Win32Window)
				'ManevreForm.ListView0001.Focus()

				_tsmiSRVectoring.Checked = True
				_CurrCmd = 1
				ManevreForm = Nothing
			End If
		Catch ex As Exception
			'HidePandaBox()
			_CurrCmd = -1
			MessageBox.Show(s_Win32Window, ex.Message, CType(sender, ToolStripMenuItem).Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

			'Throw New Exception("Form initialization error.", ex)
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

	Private Sub AllForms_FormClosed(sender As Object, e As FormClosedEventArgs)
		_tsmiSRVectoring.Checked = False
		_tsmiSRVectoring.Enabled = True
		_CurrCmd = -1

		GC.Collect()
	End Sub
End Class
