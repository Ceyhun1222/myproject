Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.PageView.ViewModes
    Partial Public Class Form1
        Inherits ExamplesForm
#Region "Constructor/Initialization"

        Public Sub New()
            InitializeComponent()

            Me.radPageView1.Anchor = AnchorStyles.None

            ExamplesForm.FillComboFromEnum(Me.viewModeCombo, GetType(PageViewMode), Me.radPageView1.ViewMode)
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            Me.AddSamplePages()
            MyBase.OnLoad(e)

            Me.radPageView1.Location = Point.Empty
        End Sub
        Sub SetLocationToZero() Handles radPageView1.LocationChanged
            Me.radPageView1.Location = Point.Empty
        End Sub
        Private Sub AddSamplePages()
            PageViewLabels.currIndex = 0

            For i As Integer = 0 To 4
                Dim page As New RadPageViewPage()
                page.Text = PageViewImages.Names(i)
                page.Image = PageViewImages.Images(i)
                page.Title = page.Text & " - [Title]"
                page.Description = page.Text & " - [Description]"

                page.Controls.Add(PageViewLabels.CreateLabel())

                Me.radPageView1.Pages.Add(page)
            Next i
        End Sub

#End Region

#Region "Event Handlers"

        Private Sub viewModeCombo_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim item As RadListDataItem = TryCast(Me.viewModeCombo.SelectedItem, RadListDataItem)
            Me.radPageView1.ViewMode = CType(item.Value, PageViewMode)
            If Me.radPageView1.ViewMode = PageViewMode.Stack Then
                Dim stack As RadPageViewStackElement = TryCast(Me.radPageView1.ViewElement, RadPageViewStackElement)
                stack.ItemSelectionMode = StackViewItemSelectionMode.ContentWithSelected
            End If
        End Sub

#End Region

        Protected Overrides Sub WireEvents()
            AddHandler viewModeCombo.SelectedIndexChanged, AddressOf viewModeCombo_SelectedIndexChanged
        End Sub
    End Class
End Namespace
