
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.ListView.CheckedListBox
    Partial Public Class Form1
        Inherits ListViewExamplesControl
        Public Sub New()
            InitializeComponent()
            Me.InitializeData()
            Dim nameColumn As New ListViewDetailColumn("Product")
            nameColumn.Width = 132
            Me.radListView1.Columns.Add(nameColumn)
            Dim priceColumn As New ListViewDetailColumn("Price")
            priceColumn.Width = 60
            Me.radListView1.Columns.Add(priceColumn)

            Dim totalColumn As New ListViewDetailColumn("Total")
            totalColumn.Width = 132
            Me.radListView2.Columns.Add(totalColumn)
            Dim totalPriceColumn As New ListViewDetailColumn("TotalPrice")
            totalPriceColumn.Width = 60
            Me.radListView2.Columns.Add(totalPriceColumn)

            AddHandler Me.radListView1.CellFormatting, AddressOf radListView1_CellFormatting
            AddHandler Me.radListView2.CellFormatting, AddressOf radListView2_CellFormatting
            AddHandler Me.radCheckedListBox1.VisualItemFormatting, AddressOf radCheckedListBox1_VisualItemFormatting
            AddHandler Me.radCheckedListBox1.SelectedItemsChanged, AddressOf radCheckedListBox1_SelectedItemsChanged
        End Sub

        Private Sub Form1_Load(sender As Object, e As EventArgs)
            Dim item As New ListViewDataItem()
            Me.radListView2.Items.Add(item)

            item("Total") = "Total"
            item("TotalPrice") = String.Format("{0:C2}", 0)
        End Sub

        Public Overrides ReadOnly Property ContentControl() As Control
            Get
                Return Me.radCheckedListBox1
            End Get
        End Property

        Private Sub InitializeData()
            Dim mealProducts As IEnumerable(Of Product) = Me.CreateProducts()

            For Each product As Product In mealProducts
                Me.radCheckedListBox1.Items.Add(Me.CreateItem(product))
            Next
        End Sub

        Private Function CreateProducts() As IEnumerable(Of Product)
            Dim products As New List(Of Product)() From { _
                New Product() With { _
                    .Name = "Beef Burger", _
                    .Price = 7.95D, _
                    .Weight = 350, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\beef-burger.jpg") _
                }, _
                New Product() With { _
                    .Name = "Big Burger", _
                    .Price = 7.45D, _
                    .Weight = 450, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\big-burger.jpg") _
                }, _
                New Product() With { _
                    .Name = "Burger with fries", _
                    .Price = 5.95D, _
                    .Weight = 480, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\burger-fries.jpg") _
                }, _
                New Product() With { _
                    .Name = "Classical Burger", _
                    .Price = 3.45D, _
                    .Weight = 250, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\burger.jpg") _
                }, _
                New Product() With { _
                    .Name = "Chicken Toast", _
                    .Price = 4.99D, _
                    .Weight = 300, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\chicken-toast.jpg") _
                }, _
                New Product() With { _
                    .Name = "Chicken Wings", _
                    .Price = 5.9D, _
                    .Weight = 300, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\chicken-wings.jpg") _
                }, _
                New Product() With { _
                    .Name = "Crab meat sandwich", _
                    .Price = 5.5D, _
                    .Weight = 290, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\crab-sandwich.jpg") _
                }, _
                New Product() With { _
                    .Name = "Ham sandwich", _
                    .Price = 2.95D, _
                    .Weight = 300, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\ham-cheese-sandwich.jpg") _
                }, _
                New Product() With { _
                    .Name = "Hot dog", _
                    .Price = 2D, _
                    .Weight = 250, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\hot-dog.jpg") _
                }, _
                New Product() With { _
                    .Name = "Meatballs", _
                    .Price = 4.35D, _
                    .Weight = 200, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\meatballs.jpg") _
                }, _
                New Product() With { _
                    .Name = "Pork ribs", _
                    .Price = 9.99D, _
                    .Weight = 450, _
                    .Image = Image.FromFile(Application.StartupPath + "\Resources\MealImages\pork-ribs.jpg") _
                } _
            }

            Return products
        End Function

        Private Function CreateItem(product As Product) As ListViewDataItem
            Dim item As New ListViewDataItem()
            item.Image = product.Image.GetThumbnailImage(139, 84, Nothing, IntPtr.Zero)
            item.Text = product.Name
            item.Tag = product

            Return item
        End Function

        Private Sub radCheckedListBox1_ItemCheckedChanged(sender As Object, e As ListViewItemEventArgs) Handles radCheckedListBox1.ItemCheckedChanged
            Dim product As Product = TryCast(e.Item.Tag, Product)

            If e.Item.CheckState = Telerik.WinControls.Enumerations.ToggleState.[On] Then
                Dim item As New ListViewDataItem()
                item.Tag = product
                Me.radListView1.Items.Add(item)

                item("Product") = product.Name
                item("Price") = product.Price
            Else
                For Each item As ListViewDataItem In Me.radListView1.Items
                    If item("Product").ToString() = product.Name Then
                        Me.radListView1.Items.Remove(item)
                        Exit For
                    End If
                Next
            End If

            Dim total As Decimal = 0
            For Each item As ListViewDataItem In radListView1.Items
                total += Convert.ToDecimal(item("Price"))
            Next

            Me.radListView2.Items.Clear()
            Dim totalItem As New ListViewDataItem()
            Me.radListView2.Items.Add(totalItem)

            totalItem("Total") = "Total"
            totalItem("TotalPrice") = String.Format("{0:C2}", total)
            If Me.radCheckedListBox1.Items.Count = Me.radCheckedListBox1.CheckedItems.Count Then
                Me.radCheckAllButton.Text = "Uncheck all"
            Else
                Me.radCheckAllButton.Text = "Check all"
            End If

            Me.UpdateSelectedButtonText()
        End Sub

        Private Sub radCheckedListBox1_SelectedItemsChanged(sender As Object, e As EventArgs)
            Me.UpdateSelectedButtonText()
        End Sub

        Private Sub radCheckedListBox1_VisualItemFormatting(sender As Object, e As ListViewVisualItemEventArgs)
            Dim item As SimpleListViewVisualItem = TryCast(e.VisualItem, SimpleListViewVisualItem)

            Dim product As Product = TryCast(item.Data.Tag, Product)
            Dim color As String = "#681406"
            If Me.radListView1.ThemeName = "HighContrastBlack" OrElse Me.radListView1.ThemeName = "VisualStudio2012Dark" Then
                color = "#FFFFFF"
            End If

            If item.Children.Count > 0 Then
                Dim checkBoxItem As ListViewItemCheckbox = TryCast(item.Children(0), ListViewItemCheckbox)
                checkBoxItem.Margin = New Padding(2, 2, 3, 2)
            End If

            item.Data.Text = (Convert.ToString((Convert.ToString("<html>" & "<span style=""font-size:14.5pt;font-family:Segoe UI Semibold;"">  ") & product.Name) & "</span>" & "<br><span style=""font-size:10.5pt;""><i>   " & product.Weight & "gr</i></span>" & "<br><span style=""font-size:19pt;""> </span>" & "<span style=""color:") & color) & ";font-size:14.5pt;""> " & String.Format("{0:C2}", product.Price) & "</span>"

            item.ImageLayout = ImageLayout.Center
            item.ImageAlignment = ContentAlignment.MiddleLeft
        End Sub

        Private Sub radListView1_CellFormatting(sender As Object, e As ListViewCellFormattingEventArgs)
            Dim cell As DetailListViewDataCellElement = TryCast(e.CellElement, DetailListViewDataCellElement)
            If cell IsNot Nothing Then
                If cell.Text <> String.Empty Then
                    Dim price As Decimal = 0
                    If Decimal.TryParse(cell.Text, price) Then
                        cell.Text = New String(" "c, 5) & String.Format("{0:C2}", price)
                    Else
                        cell.Text = New String(" "c, 2) & String.Format("{0}", cell.Text)
                    End If

                    e.CellElement.BorderGradientStyle = Telerik.WinControls.GradientStyles.Solid
                Else
                    e.CellElement.ResetValue(LightVisualElement.BorderGradientStyleProperty, Telerik.WinControls.ValueResetFlags.Local)
                End If
            End If
        End Sub

        Private Sub radListView2_CellFormatting(sender As Object, e As ListViewCellFormattingEventArgs)
            Me.radListView1_CellFormatting(sender, e)
            Dim cell As DetailListViewDataCellElement = TryCast(e.CellElement, DetailListViewDataCellElement)
            If cell IsNot Nothing AndAlso cell.Text <> String.Empty Then
                Dim price As Decimal = 0
                If Decimal.TryParse(cell.Text.Substring(3), price) Then
                    Dim color__1 As Color = Color.FromArgb(255, 104, 20, 6)
                    If Me.radListView1.ThemeName = "HighContrastBlack" OrElse Me.radListView1.ThemeName = "VisualStudio2012Dark" Then
                        color__1 = Color.FromArgb(255, 255, 255, 255)
                    End If

                    Dim indent As Integer = 4
                    If price >= 10 Then
                        indent = 3
                    End If

                    cell.Text = New String(" "c, indent) & String.Format("{0:C2}", price)
                    e.CellElement.ForeColor = color__1
                Else
                    e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, Telerik.WinControls.ValueResetFlags.Local)
                End If

                e.CellElement.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
            Else
                e.CellElement.ResetValue(LightVisualElement.FontProperty, Telerik.WinControls.ValueResetFlags.Local)
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, Telerik.WinControls.ValueResetFlags.Local)
            End If
        End Sub

        Private Sub radClearAllButton_Click(sender As Object, e As EventArgs) Handles radClearAllButton.Click
            Me.radCheckedListBox1.UncheckAllItems()
        End Sub

        Private Sub radOrderButton_Click(sender As Object, e As EventArgs) Handles radOrderButton.Click
            If Me.radListView1.Items.Count < 1 Then
                Return
            End If

            Dim orderedItemsTexts As New List(Of String)()
            For Each item As ListViewDataItem In Me.radListView1.Items
                orderedItemsTexts.Add(TryCast(item.Tag, Product).Name)
            Next

            Dim message As String = "You ordered:" & Environment.NewLine & String.Join(Environment.NewLine, orderedItemsTexts.ToArray()) & Environment.NewLine & "Total: " & Me.radListView2.Items(0)("TotalPrice").ToString()
            RadMessageBox.ThemeName = Me.CurrentThemeName
            RadMessageBox.Show(message)
            Me.radCheckedListBox1.UncheckAllItems()
        End Sub

        Private Sub radCheckAllButton_Click(sender As Object, e As EventArgs) Handles radCheckAllButton.Click
            If Me.radCheckAllButton.Text = "Check all" Then
                Me.radCheckedListBox1.CheckAllItems()
                Me.radCheckAllButton.Text = "Uncheck all"
            Else
                Me.radCheckedListBox1.UncheckAllItems()
                Me.radCheckAllButton.Text = "Check all"
            End If
        End Sub

        Private Sub radCheckSelectedButton_Click(sender As Object, e As EventArgs) Handles radCheckSelectedButton.Click
            If Me.radCheckSelectedButton.Text = "Check selected" Then
                Me.radCheckedListBox1.CheckSelectedItems()
                Me.radCheckSelectedButton.Text = "Uncheck selected"
            Else
                Me.radCheckedListBox1.UncheckSelectedItems()
                Me.radCheckSelectedButton.Text = "Check selected"
            End If
        End Sub

        Private Sub UpdateSelectedButtonText()
            For Each item As ListViewDataItem In Me.radCheckedListBox1.SelectedItems
                If item.CheckState <> Telerik.WinControls.Enumerations.ToggleState.[On] Then
                    Me.radCheckSelectedButton.Text = "Check selected"
                    Return
                End If
            Next

            Me.radCheckSelectedButton.Text = "Uncheck selected"
        End Sub
    End Class

    Friend Class Product
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = Value
            End Set
        End Property
        Private m_Name As String

        Public Property Price() As Decimal
            Get
                Return m_Price
            End Get
            Set(value As Decimal)
                m_Price = Value
            End Set
        End Property
        Private m_Price As Decimal

        Public Property Weight() As Integer
            Get
                Return m_Weight
            End Get
            Set(value As Integer)
                m_Weight = Value
            End Set
        End Property
        Private m_Weight As Integer

        Public Property Image() As Image
            Get
                Return m_Image
            End Get
            Set(value As Image)
                m_Image = Value
            End Set
        End Property
        Private m_Image As Image
    End Class
End Namespace

