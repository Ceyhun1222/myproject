Imports System.Collections.Generic
Imports Aran
Imports Aran.Aim
Imports Aran.AranEnvironment

Public Class MSAAranPlugin
	Inherits AranPlugin

	Const _name As String = "PANDA Conventional - MSA & TAA"

	'Dim _isInit As Boolean
	Dim _tsmiCreateMSA As ToolStripMenuItem
	Dim _tsmiCreateTAA As ToolStripMenuItem
	Dim _tsmiMSAInfo As ToolStripMenuItem

#Region "IAranPlugin Members"

	Public Sub New()
	End Sub

	Public Overrides ReadOnly Property Id As Guid
		Get
			Return New Guid("6D1C1DB5-C881-4B25-8470-26DFCDD01D29")
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
		featTypes.Add(FeatureType.NDB)
		featTypes.Add(FeatureType.DME)
		featTypes.Add(FeatureType.DesignatedPoint)

		Return featTypes
	End Function

	Public Overrides Sub Startup(aranEnv As AranEnvironment.IAranEnvironment)
		gAranEnv = aranEnv
		gHookHelper = New ESRI.ArcGIS.Controls.HookHelper
		gHookHelper.Hook = gAranEnv.HookObject

        LangCode = 1033
		SetThreadLocale(LangCode)
		My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

		Dim menuItem As New ToolStripMenuItem()
		menuItem.Text = My.Resources.str02015

		_tsmiCreateMSA = New ToolStripMenuItem()
		_tsmiCreateMSA.Text = My.Resources.str08000 'My.Resources.str181	'Create MSA
		_tsmiCreateMSA.Tag = 0
		AddHandler _tsmiCreateMSA.Click, AddressOf CreateMSA_Click
		menuItem.DropDownItems.Add(_tsmiCreateMSA)

		_tsmiCreateTAA = New ToolStripMenuItem()
		_tsmiCreateTAA.Text = My.Resources.str02016 'My.Resources.str181	'Create MSA
		_tsmiCreateTAA.Tag = 0
		AddHandler _tsmiCreateTAA.Click, AddressOf CreateTAA_Click
		menuItem.DropDownItems.Add(_tsmiCreateTAA)

		_tsmiMSAInfo = New ToolStripMenuItem()
		_tsmiMSAInfo.Text = My.Resources.str00019	'MSA Info
		_tsmiMSAInfo.Tag = 0
		AddHandler _tsmiMSAInfo.Click, AddressOf MSAInfo_Click
		menuItem.DropDownItems.Add(_tsmiMSAInfo)

		aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem)
	End Sub
#End Region

	Public Function InitCommands(FillObstacles As Boolean) As Boolean
		Return InitCommand(FillObstacles) > 0
	End Function

	Public Sub CreateMSA_Click(sender As Object, e As EventArgs)
		If CurrCmd = 1 Then Return
		CurrCmd = 0

		Try
			ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

			If InitCommands(False) Then

				Dim CreateMSAFrm As CCreateMSA = New CCreateMSA()

				AddHandler CreateMSAFrm.FormClosed, AddressOf AllForms_FormClosed

				_tsmiCreateMSA.Checked = True
				_tsmiCreateTAA.Enabled = False
				_tsmiMSAInfo.Enabled = False

				CreateMSAFrm.Show(s_Win32Window)
				'CreateMSAFrm.ListView0001.Focus()
				CurrCmd = 1
				CreateMSAFrm = Nothing
			Else

			End If

			HidePandaBox()

		Catch aranEx As AranException
			HidePandaBox()
			If Not aranEx.Handled Then
				aranEx.ShowMessageBox()
			End If
		Catch ex As Exception
			HidePandaBox()
			Throw ex 'Throw New Exception("Form initialization error.", exception)
		End Try
	End Sub

	Public Sub CreateTAA_Click(sender As Object, e As EventArgs)
		If CurrCmd = 2 Then Return
		CurrCmd = 0

		Try
			ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

			InitCommands(True)
			Dim CreateTAAFrm As CCreateTAA = New CCreateTAA()

			AddHandler CreateTAAFrm.FormClosed, AddressOf AllForms_FormClosed

			_tsmiCreateMSA.Enabled = False
			_tsmiCreateTAA.Checked = True
			_tsmiMSAInfo.Enabled = False

			CreateTAAFrm.Show(s_Win32Window)
			'CreateMSAFrm.ListView0001.Focus()
			CurrCmd = 2
			CreateTAAFrm = Nothing

			HidePandaBox()
		Catch aranEx As AranException
			HidePandaBox()
			If Not aranEx.Handled Then
				aranEx.ShowMessageBox()
			End If
		Catch ex As Exception
			HidePandaBox()
			Throw ex 'Throw New Exception("Form initialization error.", exception)
		End Try
	End Sub

	Public Sub MSAInfo_Click(sender As Object, e As EventArgs)
		If CurrCmd = 3 Then Return
		CurrCmd = 0

		Try
			ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())

			If InitCommands(False) Then
				Dim MSAInfoFrm As CMSAInfo = New CMSAInfo

				AddHandler MSAInfoFrm.FormClosed, AddressOf AllForms_FormClosed

				_tsmiCreateMSA.Enabled = False
				_tsmiCreateTAA.Enabled = False
				_tsmiMSAInfo.Checked = True

				MSAInfoFrm.Show(s_Win32Window)
				'MSAInfoFrm.ListView0001.Focus()
				CurrCmd = 3
				MSAInfoFrm = Nothing
			End If

			HidePandaBox()
		Catch ex As Exception
			HidePandaBox()
			Throw ex 'New Exception("Form initialization error.")
		End Try
	End Sub

	Private Sub AllForms_FormClosed(sender As Object, e As FormClosedEventArgs)
		_tsmiCreateMSA.Checked = False
		_tsmiCreateTAA.Checked = False
		_tsmiMSAInfo.Checked = False

		_tsmiCreateMSA.Enabled = True
		_tsmiCreateTAA.Enabled = True
		_tsmiMSAInfo.Enabled = True

		CurrCmd = 0
		sender = Nothing
		GC.Collect()
	End Sub
End Class
