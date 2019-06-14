
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports System.Globalization

Namespace Telerik.Examples.WinControls.DataEntry.Customization
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
            Me.radDataEntry1.ShowValidationPanel = True
            Me.radDataEntry1.ItemDefaultSize = New Size(460, 25)
            Me.radDataEntry1.ItemSpace = 10

            AddHandler Me.radDataEntry1.EditorInitializing, AddressOf radDataEntry1_EditorInitializing
            AddHandler Me.radDataEntry1.ItemInitialized, AddressOf radDataEntry1_ItemInitialized
            AddHandler Me.radDataEntry1.BindingCreated, AddressOf radDataEntry1_BindingCreated

            Me.radDataEntry1.DataSource = New Person(DateTime.Now, "Iva", "Ivanova", Person.OccupationPositions.SuppliesManager, "(555) 123 456", 1500)
        End Sub

        Private Sub radDataEntry1_BindingCreated(sender As Object, e As BindingCreatedEventArgs)
            If e.DataMember = "Salary" Then
                AddHandler e.Binding.Parse, AddressOf Binding_Parse
            End If
        End Sub

        Private Sub radDataEntry1_ItemInitialized(sender As Object, e As Telerik.WinControls.UI.ItemInitializedEventArgs)
            If e.Panel.Controls(1).Text = "First Name" Then
                e.Panel.Size = New Size(300, 25)
                e.Panel.Controls(1).Text = "Name"
            ElseIf e.Panel.Controls(1).Text = "Last Name" Then
                e.Panel.Size = New Size(160, 25)
                e.Panel.Controls(1).Visible = False
                e.Panel.Location = New Point(310, 10)
            Else
                e.Panel.Location = New Point(e.Panel.Location.X, e.Panel.Location.Y - 35)
            End If

            If TypeOf e.Panel.Controls(0) Is RadDateTimePicker Then
                e.Panel.Controls(0).ForeColor = Color.MediumVioletRed
            End If

            If e.Panel.Controls(1).Text = "Note" Then
                e.Panel.Size = New Size(e.Panel.Size.Width, 100)
                TryCast(e.Panel.Controls(0), RadTextBox).Multiline = True
            End If

            e.Panel.Controls(1).Font = New System.Drawing.Font(e.Panel.Controls(1).Font.Name, 12.0F, FontStyle.Bold)
            e.Panel.Controls(1).ForeColor = Color.Red
        End Sub

        Private Sub radDataEntry1_EditorInitializing(sender As Object, e As Telerik.WinControls.UI.EditorInitializingEventArgs)
            If e.[Property].Name = "Salary" Then
                Dim radMaskedEditBox As New RadMaskedEditBox()
                radMaskedEditBox.MaskType = MaskType.Numeric
                radMaskedEditBox.MaskedEditBoxElement.StretchVertically = True

                e.Editor = radMaskedEditBox
            End If

            If e.[Property].Name = "PassWord" Then
                TryCast(e.Editor, RadTextBox).PasswordChar = "*"c
            End If
        End Sub

        Private Sub Binding_Parse(sender As Object, e As ConvertEventArgs)
            Dim salary As Integer = Integer.Parse(e.Value.ToString(), NumberStyles.Currency)
            e.Value = salary
        End Sub
    End Class
End Namespace