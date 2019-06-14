
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports System.Text.RegularExpressions

Namespace Telerik.Examples.WinControls.DataEntry.Validation
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            InitializeComponent()

            SetupControls()
        End Sub

        Private Sub SetupControls()
            Me.radDataEntry1.ShowValidationPanel = True
            Me.radDataEntry1.ItemDefaultSize = New Size(460, 25)
            Me.radDataEntry1.ItemSpace = 8

            AddHandler Me.radDataEntry1.ItemValidated, AddressOf radDataEntry1_ItemValidated
            AddHandler Me.radDataEntry1.ItemValidating, AddressOf radDataEntry1_ItemValidating

            Dim list As New BindingList(Of Person)() From { _
             New Person(DateTime.Now, "Adam", "Johnson", Person.OccupationPositions.SuppliesManager, "(555) 123 3333", 1500), _
             New Person(DateTime.Now, "Michael", "Philips", Person.OccupationPositions.StaffManager, "(555) 444 4567", 1450), _
             New Person(DateTime.Now, "Paul", "Carter", Person.OccupationPositions.Consultant, "(555) 555 4567", 1499) _
            }

            Me.bindingSource1.DataSource = list
            Me.bindingSource1.AllowNew = True

            Me.radBindingNavigator1.BindingSource = Me.bindingSource1
            Me.radDataEntry1.DataSource = Me.bindingSource1
        End Sub

       Private Sub radDataEntry1_ItemValidating(sender As Object, e As ItemValidatingEventArgs)
            If Me.radDropDownList1.SelectedItem.Text = "ItemValidating" Then
                Dim person As Person = TryCast(Me.radDataEntry1.CurrentObject, Person)
                Dim propertyName As String = e.Label.Text

                If propertyName = "First Name" Then
                    If person.FirstName.Length < 2 OrElse person.FirstName.Length > 15 OrElse Not Regex.IsMatch(person.FirstName, "^[a-zA-Z]+$") Then
                        Dim errorMessage As String = "First Name should be between 2 and 15 chars and can contain only letters."
                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)
                        e.Cancel = True

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("First Name")
                    End If
                ElseIf e.Label.Text = "Last Name" Then
                    If person.LastName.Length < 2 OrElse person.LastName.Length > 15 OrElse Not Regex.IsMatch(person.LastName, "^[a-zA-Z]+$") Then
                        Dim errorMessage As String = "Last Name should be between 2 and 15 chars and can contain only letters."

                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)
                        e.Cancel = True

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("Last Name")
                    End If
                ElseIf e.Label.Text = "Salary" Then
                    If person.Salary < e.RangeAttribute.MinValue OrElse person.Salary > e.RangeAttribute.MaxValue OrElse Not Regex.IsMatch(person.Salary.ToString(), "^[0-9]+$") Then
                        Dim errorMessage As String = "Salary can contain only numbers and should be in range 1500 - 1700."
                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)
                        e.Cancel = True

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("Salary")
                    End If
                ElseIf e.Label.Text = "Phone Number" Then
                    Dim foundInvalidChar As Boolean = False
                    For Each c As Char In person.PhoneNumber.ToCharArray()
                        If c <> " "c AndAlso c <> "("c AndAlso c <> ")"c AndAlso Not Char.IsNumber(c) Then
                            foundInvalidChar = True
                            Exit For
                        End If
                    Next

                    If foundInvalidChar Then
                        Dim errorMessage As String = "Phone number can contain only numbers."
                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)
                        e.Cancel = True

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("Phone Number")
                    End If
                End If
            End If
        End Sub

        Private Sub radDataEntry1_ItemValidated(sender As Object, e As ItemValidatedEventArgs)
            If Me.radDropDownList1.SelectedItem.Text = "ItemValidated" Then
                Dim person As Person = TryCast(Me.radDataEntry1.CurrentObject, Person)
                Dim propertyName As String = e.Label.Text

                If e.Label.Text = "First Name" Then
                    If person.FirstName.Length < 2 OrElse person.FirstName.Length > 15 OrElse Not Regex.IsMatch(person.FirstName, "^[a-zA-Z]+$") Then
                        Dim errorMessage As String = "First Name should be between 2 and 15 chars and can contain only letters."
                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("First Name")
                    End If
                ElseIf e.Label.Text = "Last Name" Then
                    If person.LastName.Length < 2 OrElse person.LastName.Length > 15 OrElse Not Regex.IsMatch(person.LastName, "^[a-zA-Z]+$") Then
                        Dim errorMessage As String = "Last Name should be between 2 and 15 chars and can contain only letters."

                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("Last Name")
                    End If
                ElseIf e.Label.Text = "Salary" Then
                    If person.Salary < e.RangeAttribute.MinValue OrElse person.Salary > e.RangeAttribute.MaxValue OrElse Not Regex.IsMatch(person.Salary.ToString(), "^[0-9]+$") Then
                        Dim errorMessage As String = "Salary can contain only numbers and should be in range 1500 - 1700."
                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("Salary")
                    End If
                ElseIf e.Label.Text = "Phone Number" Then
                    Dim foundInvalidChar As Boolean = False
                    For Each c As Char In person.PhoneNumber.ToCharArray()
                        If c <> " "c AndAlso c <> "("c AndAlso c <> ")"c AndAlso Not Char.IsNumber(c) Then
                            foundInvalidChar = True
                            Exit For
                        End If
                    Next

                    If foundInvalidChar Then
                        Dim errorMessage As String = "Phone number can contain only numbers."
                        e.ErrorProvider.SetError(TryCast(sender, Control), errorMessage)

                        AddErrorLabel(propertyName, errorMessage)
                    Else
                        e.ErrorProvider.Clear()
                        Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.RemoveByKey("Phone Number")
                    End If
                End If
            End If
        End Sub

        Private Sub AddErrorLabel(propertyName As String, errorMessage As String)
            If Not Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.ContainsKey(propertyName) Then
                Dim label As New RadLabel()
                label.Name = propertyName
                label.Text = Convert.ToString((Convert.ToString("<html><size=10><b>") & propertyName) + " : </b>") & errorMessage
                label.Dock = DockStyle.Top
                label.MaximumSize = New System.Drawing.Size(480, 0)
                label.TextWrap = True
                Me.radDataEntry1.ValidationPanel.PanelContainer.Controls.Add(label)
            End If
        End Sub
    End Class
End Namespace