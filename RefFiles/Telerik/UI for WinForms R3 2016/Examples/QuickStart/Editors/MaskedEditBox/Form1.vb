Imports Telerik.Examples.WinControls.Editors.ComboBox
Imports System.Collections
Imports System.Globalization
Imports Telerik.WinControls.UI
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Editors.MaskedEditBox
	Partial Public Class Form1
		Inherits EditorExampleBaseForm
		Public Sub New()
			InitializeComponent()

			Me.LoadCultureInfos()
		End Sub

		Private Sub LoadCultureInfos()
			Dim cultures As New SortedList()

			Dim temp As CultureInfo
			For i As Integer = 0 To CultureInfo.GetCultures(CultureTypes.SpecificCultures).Length - 1
                temp = CultureInfo.GetCultures(CultureTypes.SpecificCultures)(i)

                If Not cultures.ContainsKey(temp.EnglishName) Then
                    cultures.Add(temp.EnglishName, temp)
                End If

			Next i

			Dim ie As IEnumerator = cultures.Keys.GetEnumerator()
			Do While ie.MoveNext()
				Dim name As String = CStr(ie.Current)
				Me.radComboCultures.Items.Add(New RadListDataItem(name, cultures(name))) '.ToString()
			Loop

			radComboCultures.SelectedItem = radComboCultures.Items(radComboCultures.FindStringExact(CultureInfo.CurrentCulture.EnglishName))
			AddHandler radComboCultures.SelectedIndexChanged, AddressOf CulturesList_SelectedIndexChanged
		End Sub

		Private Sub CulturesList_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
			For Each control As Control In Me.radPanelDemoHolder.Controls
				If TypeOf control Is RadMaskedEditBox Then
					Dim maskEdit As RadMaskedEditBox = TryCast(control, RadMaskedEditBox)

					maskEdit.Culture = (TryCast(radComboCultures.SelectedValue, CultureInfo))
					If maskEdit.MaskType = MaskType.DateTime Then
						maskEdit.Value = Date.Now
					End If
				End If
			Next control
		End Sub

		Private Sub radTextBox1_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
			Me.radMaskedEditBox19.Mask = radTextBox1.Text
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			Me.RadMaskedEditBox1.Value = Date.Now
			Me.RadMaskedEditBox2.Value = Date.Now
			Me.RadMaskedEditBox3.Value = Date.Now
			Me.RadMaskedEditBox4.Value = Date.Now
			Me.RadMaskedEditBox5.Value = Date.Now
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radTextBox1.TextChanged, AddressOf radTextBox1_TextChanged
		End Sub
	End Class
End Namespace