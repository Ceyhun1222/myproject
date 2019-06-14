Imports Microsoft.Win32
Imports System.Collections.Generic
Imports Aran
Imports Aran.Aim
Imports Aran.AranEnvironment

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Public Class ArrivalAranPlugin
	Inherits AranPlugin

	Const _name As String = "PANDA  -  EnRoute"

	Dim _tsmiEnRoute As ToolStripMenuItem

#Region "IAranPlugin Members"

	Public Sub New()
		'_isInit = False
	End Sub

	Public Overrides ReadOnly Property Id As Guid
		Get
			Return New Guid("160D6C2F-5547-4A40-93A1-C9A0198565AE")
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
		My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)


		Dim menuItem As New ToolStripMenuItem()
        menuItem.Text = "PANDA  -  Enroute" 'My.Resources.str80
		_tsmiEnRoute = New ToolStripMenuItem()
		_tsmiEnRoute.Text = My.Resources.str0002	'My.Resources.str158	'PrecApproach
		_tsmiEnRoute.Tag = 1
		AddHandler _tsmiEnRoute.Click, AddressOf Enroute_Click
		menuItem.DropDownItems.Add(_tsmiEnRoute)


		aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem)
	End Sub
#End Region

	Public Function InitCommands() As Boolean
		Return InitCommand() > 0
	End Function

	Public Sub Enroute_Click(sender As Object, e As EventArgs)


		Try
			Aran.PANDA.Common.NativeMethods.ShowPandaBox(GlobalVars.gAranEnv.Win32Window.Handle.ToInt32())

			If InitCommands() Then
				Dim EnrouteFrm As CEnrouteForm = New CEnrouteForm

				AddHandler EnrouteFrm.FormClosed, AddressOf AllForms_FormClosed

				_tsmiEnRoute.Checked = True
				EnrouteFrm.Show(s_Win32Window)
				'EnrouteFrm.ComboBox0001.Focus()
				EnrouteFrm = Nothing
			End If

			Aran.PANDA.Common.NativeMethods.HidePandaBox()
		Catch ex As Exception
			Aran.PANDA.Common.NativeMethods.HidePandaBox()
			Throw ex
		End Try
	End Sub

	Private Sub AllForms_FormClosed(sender As Object, e As FormClosedEventArgs)
		_tsmiEnRoute.Checked = False
		_tsmiEnRoute.Enabled = True

		sender = Nothing
		GC.Collect()
	End Sub
End Class
