
Imports System.Windows.Forms
Imports Telerik.Examples.WinControls.Editors.ComboBox
Imports System.Collections
Imports System.Globalization
Imports Telerik.WinControls.UI
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Editors.FreeFormatDateInput
    Partial Public Class Form1
        Inherits EditorExampleBaseForm
        Private errorProvider As ErrorProvider

        Public Sub New()
            InitializeComponent()

            errorProvider = New ErrorProvider()

            Me.SetupEditors()

            Me.LoadCultureInfos()
        End Sub

        Private Sub SetupEditors()
            Me.radDateTimePicker2.Value = New DateTime(DateTime.Now.Year - 100, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
            Me.radDateTimePicker2.Format = DateTimePickerFormat.[Custom]
            Me.radDateTimePicker2.CustomFormat = "MMM - dd - yyyy hh:mm tt"
            TryCast(Me.radDateTimePicker2.DateTimePickerElement.CurrentBehavior, RadDateTimePickerCalendar).ShowTimePicker = True
            TryCast(Me.radDateTimePicker2.DateTimePickerElement.CurrentBehavior, RadDateTimePickerCalendar).DropDownMinSize = New System.Drawing.Size(300, 250)
            TryCast(Me.radDateTimePicker2.DateTimePickerElement.CurrentBehavior, RadDateTimePickerCalendar).Calendar.HeaderNavigationMode = HeaderNavigationMode.Zoom

            Me.radDateTimePicker3.Value = New DateTime(DateTime.Now.Year + 100, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59)
            Me.radDateTimePicker3.Format = DateTimePickerFormat.[Custom]
            Me.radDateTimePicker3.CustomFormat = "MMM - dd - yyyy hh:mm tt"
            TryCast(Me.radDateTimePicker3.DateTimePickerElement.CurrentBehavior, RadDateTimePickerCalendar).ShowTimePicker = True
            TryCast(Me.radDateTimePicker3.DateTimePickerElement.CurrentBehavior, RadDateTimePickerCalendar).DropDownMinSize = New System.Drawing.Size(300, 250)
            TryCast(Me.radDateTimePicker3.DateTimePickerElement.CurrentBehavior, RadDateTimePickerCalendar).Calendar.HeaderNavigationMode = HeaderNavigationMode.Zoom


            Me.radMaskedEditBox1.Value = DateTime.Now
            TryCast(Me.radMaskedEditBox1.MaskedEditBoxElement.Provider, FreeFormDateTimeProvider).MinDate = Me.radDateTimePicker2.Value
            TryCast(Me.radMaskedEditBox1.MaskedEditBoxElement.Provider, FreeFormDateTimeProvider).MaxDate = Me.radDateTimePicker3.Value
            AddHandler TryCast(Me.radMaskedEditBox1.MaskedEditBoxElement.Provider, FreeFormDateTimeProvider).ParsingDateTime, AddressOf Form1_ParsingDateTime

            Me.radDateTimePicker1.Value = DateTime.Now
            Me.radDateTimePicker1.MinDate = Me.radDateTimePicker2.Value
            Me.radDateTimePicker1.MaxDate = Me.radDateTimePicker3.Value
            Me.radDateTimePicker1.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime
            AddHandler TryCast(Me.radDateTimePicker1.DateTimePickerElement.TextBoxElement.Provider, FreeFormDateTimeProvider).ParsingDateTime, AddressOf Form1_ParsingDateTime

            Me.radTimePicker1.Value = DateTime.Now
            Me.radTimePicker1.TimePickerElement.MaskedEditBox.MaskType = MaskType.FreeFormDateTime
            Me.radTimePicker1.TimePickerElement.MinValue = Me.radDateTimePicker2.Value
            Me.radTimePicker1.TimePickerElement.MaxValue = Me.radDateTimePicker3.Value
            AddHandler TryCast(Me.radTimePicker1.TimePickerElement.MaskedEditBox.Provider, FreeFormDateTimeProvider).ParsingDateTime, AddressOf Form1_ParsingDateTime

        End Sub

        Private Sub Form1_ParsingDateTime(sender As Object, e As ParsingDateTimeEventArgs)
            Dim control As Control = DirectCast(sender, Telerik.WinControls.UI.MaskDateTimeProvider).Owner.ElementTree.Control

            If e.Result Is Nothing Then
                Me.errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight)
                Me.errorProvider.SetIconPadding(control, 2)
                Me.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.BlinkIfDifferentError
                Me.errorProvider.SetError(control, "Invalid Input")

                e.Cancel = True
            Else
                Me.errorProvider.SetError(control, "")
            End If
        End Sub

        Private Sub LoadCultureInfos()
            Dim cultures As New SortedList()

            Dim temp As CultureInfo
            For i As Integer = 0 To CultureInfo.GetCultures(CultureTypes.SpecificCultures).Length - 1
                temp = CultureInfo.GetCultures(CultureTypes.SpecificCultures)(i)

                If Not cultures.ContainsKey(temp.EnglishName) Then
                    cultures.Add(temp.EnglishName, temp)
                End If

            Next

            Dim ie As IEnumerator = cultures.Keys.GetEnumerator()
            While ie.MoveNext()
                Dim name As String = DirectCast(ie.Current, String)
                '.ToString()
                Me.radComboCultures.Items.Add(New RadListDataItem(name, cultures(name)))
            End While

            radComboCultures.SelectedItem = radComboCultures.Items(radComboCultures.FindStringExact(CultureInfo.CurrentCulture.EnglishName))
        End Sub

        Protected Overrides Sub WireEvents()

            AddHandler Me.radDateTimePicker2.Validated, AddressOf radDateTimePicker2_Validated
            AddHandler Me.radDateTimePicker3.Validated, AddressOf radDateTimePicker3_Validated

            AddHandler Me.radComboCultures.SelectedIndexChanged, AddressOf CulturesList_SelectedIndexChanged
        End Sub

        Private Sub radDateTimePicker3_Validated(sender As Object, e As EventArgs)
            TryCast(Me.radMaskedEditBox1.MaskedEditBoxElement.Provider, FreeFormDateTimeProvider).MaxDate = Me.radDateTimePicker3.Value
            Me.radDateTimePicker1.MaxDate = Me.radDateTimePicker3.Value
            Me.radTimePicker1.TimePickerElement.MaxValue = Me.radDateTimePicker3.Value
        End Sub

        Private Sub radDateTimePicker2_Validated(sender As Object, e As EventArgs)
            TryCast(Me.radMaskedEditBox1.MaskedEditBoxElement.Provider, FreeFormDateTimeProvider).MinDate = Me.radDateTimePicker2.Value
            Me.radDateTimePicker1.MinDate = Me.radDateTimePicker2.Value
            Me.radTimePicker1.TimePickerElement.MinValue = Me.radDateTimePicker2.Value
        End Sub

        Private Sub CulturesList_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim culture As CultureInfo = TryCast(radComboCultures.SelectedValue, CultureInfo)
            Me.radMaskedEditBox1.Culture = culture
            Me.radDateTimePicker1.Culture = culture
            Me.radTimePicker1.Culture = culture
        End Sub
    End Class
End Namespace