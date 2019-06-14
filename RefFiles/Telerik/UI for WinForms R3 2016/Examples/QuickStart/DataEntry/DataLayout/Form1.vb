
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.DataEntry.DataLayout
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()

            InitializeComponent()
        End Sub

        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
            SetupControls()
            ArrangePictureBox()
        End Sub

        Private Sub SetupControls()
            Me.radDataLayout1.ItemDefaultHeight = 26
            Me.radDataLayout1.ColumnCount = 2
            Me.radDataLayout1.FlowDirection = FlowDirection.TopDown
            Me.radDataLayout1.AutoSizeLabels = True

            AddHandler Me.radDataLayout1.EditorInitializing, AddressOf radDataEntry1_EditorInitializing
            AddHandler Me.radDataLayout1.BindingCreating, AddressOf radDataEntry1_BindingCreating
            AddHandler Me.radDataLayout1.BindingCreated, AddressOf radDataEntry1_BindingCreated

            Me.productsTableAdapter.Fill(Me.furnitureDataSet.Products)
            Me.bindingSource1.AllowNew = True

            Me.radBindingNavigator1.BindingSource = Me.bindingSource1
            Me.radBindingNavigator1.AutoHandleAddNew = False
            AddHandler Me.radBindingNavigator1AddNewItem.Click, AddressOf radBindingNavigator1AddNewItem_Click

            Me.radDataLayout1.DataSource = Me.bindingSource1
        End Sub

        Private Sub ArrangePictureBox()
            Dim layoutControl As RadLayoutControl = Me.radDataLayout1.LayoutControl

            layoutControl.AddItem(DirectCast(layoutControl.Items(5), LayoutControlItemBase), DirectCast(layoutControl.Items(11), LayoutControlItemBase), LayoutControlDropPosition.Top)

            layoutControl.ResizeItem(DirectCast(layoutControl.Items(5), LayoutControlItemBase), 22 - layoutControl.Items(5).Bounds.Height)
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
                pictureBox.Name = "PictureBoxPhoto"
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage
                e.Editor = pictureBox
            End If
        End Sub

        Private Sub radButtonCustomize_Click(sender As Object, e As EventArgs) Handles radButtonCustomize.Click
            Me.radDataLayout1.LayoutControl.ShowCustomizeDialog()
        End Sub

        Private Sub radButtonSaveLayout_Click(sender As Object, e As EventArgs) Handles radButtonSaveLayout.Click
            Using sfd As New SaveFileDialog()
                sfd.DefaultExt = ".xml"
                sfd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
                If sfd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    Me.radDataLayout1.LayoutControl.SaveLayout(sfd.FileName)
                End If
            End Using
        End Sub

        Private Sub radButtonLoadLayout_Click(sender As Object, e As EventArgs) Handles radButtonLoadLayout.Click
            Using ofd As New OpenFileDialog()
                ofd.DefaultExt = ".xml"
                ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
                If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    Me.radDataLayout1.LayoutControl.LoadLayout(ofd.FileName)
                End If
            End Using
        End Sub
    End Class
End Namespace