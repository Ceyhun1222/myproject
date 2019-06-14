
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports System.IO
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.DataEntry.FirstLook
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            InitializeComponent()
            SetupControls()
        End Sub

        Private Sub SetupControls()
            Me.SuspendLayout()

            Me.radDataEntry1.ItemDefaultSize = New Size(300, 22)
            Me.radDataEntry1.ColumnCount = 2
            Me.radDataEntry1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            Me.radDataEntry1.FitToParentWidth = True
            Me.radDataEntry1.ItemSpace = 8

            AddHandler Me.radDataEntry1.ItemInitializing, AddressOf radDataEntry1_ItemInitializing
            AddHandler Me.radDataEntry1.EditorInitializing, AddressOf radDataEntry1_EditorInitializing
            AddHandler Me.radDataEntry1.BindingCreating, AddressOf radDataEntry1_BindingCreating
            AddHandler Me.radDataEntry1.BindingCreated, AddressOf radDataEntry1_BindingCreated

            Me.productsTableAdapter.Fill(Me.furnitureDataSet.Products)
            Me.bindingSource1.AllowNew = True

            Me.radBindingNavigator1.BindingSource = Me.bindingSource1
            Me.radBindingNavigator1.AutoHandleAddNew = False
            AddHandler Me.radBindingNavigator1AddNewItem.Click, AddressOf radBindingNavigator1AddNewItem_Click

            Me.radDataEntry1.DataSource = Me.bindingSource1

            Me.ResumeLayout()
        End Sub

        Private Sub radBindingNavigator1AddNewItem_Click(sender As Object, e As EventArgs)
            Dim row As Telerik.Examples.WinControls.DataSources.FurnitureDataSet.ProductsRow = Me.furnitureDataSet.Products.NewProductsRow()
            row.Price = 0
            row.Photo = Telerik.WinControls.ImageHelper.GetBytesFromImage(My.Resources.insert5)
            row.Lining = ""
            row.Manufacturer = ""
            row.ProductName = ""
            row.Quantity = 0
            row.SalesRepresentative = ""
            row.Front = ""
            row.Dimensions = ""
            row.Condition = False

            Me.furnitureDataSet.Products.Rows.Add(row)

            Me.furnitureDataSet.AcceptChanges()

            productsTableAdapter.Update(Me.furnitureDataSet.Products)

            Me.bindingSource1.Position = Me.bindingSource1.Count - 1
        End Sub

        Private Sub radDataEntry1_BindingCreated(sender As Object, e As Telerik.WinControls.UI.BindingCreatedEventArgs)
            If e.DataMember = "Photo" Then
                AddHandler e.Binding.Format, AddressOf Binding_Format
            End If
        End Sub

        Private Sub radDataEntry1_BindingCreating(sender As Object, e As Telerik.WinControls.UI.BindingCreatingEventArgs)
            If e.DataMember = "Photo" Then
                e.PropertyName = "Image"
            End If
        End Sub

        Private Sub Binding_Format(sender As Object, e As ConvertEventArgs)
            Dim img As Image = Telerik.WinControls.ImageHelper.GetImageFromBytes(DirectCast(e.Value, Byte()))
            e.Value = img
        End Sub

        Private Sub radDataEntry1_EditorInitializing(sender As Object, e As Telerik.WinControls.UI.EditorInitializingEventArgs)
            If e.[Property].Name = "Photo" Then
                Dim pictureBox As New PictureBox()
                pictureBox.Dock = DockStyle.Fill
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage
                e.Editor = pictureBox
            End If
        End Sub

        Private Sub radDataEntry1_ItemInitializing(sender As Object, e As Telerik.WinControls.UI.ItemInitializingEventArgs)
            If e.Panel.Controls(1).Text = "Photo" Then
                e.Panel.Location = New Point(8, 200)
                e.Panel.Size = New Size(500, 220)
            End If
        End Sub

    End Class
End Namespace