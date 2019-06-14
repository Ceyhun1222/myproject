
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls
Imports Telerik.WinControls.UI.Docking

Namespace Telerik.Examples.WinControls.Docking.TabStripProperties
    Partial Public Class Form1
        Inherits ExamplesForm
#Region "Constructor"

        Public Sub New()
            InitializeComponent()

            Me.docTabsVisibleCheck.Checked = Me.radDock1.DocumentTabsVisible
            Me.toolTabsVisibleCheck.Checked = Me.radDock1.ToolTabsVisible

            Me.docCloseButtonCheck.Checked = Me.radDock1.ShowDocumentCloseButton
            Me.toolCloseButtonCheck.Checked = Me.radDock1.ShowToolCloseButton

            Me.FillTabStripAlignment(Me.docTabAlignCombo, Me.radDock1.DocumentTabsAlignment)
            Me.FillTabStripAlignment(Me.toolTabAlignCombo, Me.radDock1.ToolTabsAlignment)

            Me.FillTabStripTextOrientation(Me.docTextOrientationCombo, Me.radDock1.DocumentTabsTextOrientation)
            Me.FillTabStripTextOrientation(Me.toolTextOrientationCombo, Me.radDock1.ToolTabsTextOrientation)

            Me.FillPreviewTabsCombo()
        End Sub

#End Region

#Region "Implementation"

        Private Sub FillPreviewTabsCombo()
            Me.docPreviewTabCombo.Items.Add(New RadListDataItem("(none)", Nothing))

            For Each dw As DocumentWindow In Me.radDock1.GetWindows(Of DocumentWindow)()
                Me.docPreviewTabCombo.Items.Add(New RadListDataItem(dw.Text, dw))
            Next

            Me.docPreviewTabCombo.SelectedIndex = 0
            AddHandler Me.docPreviewTabCombo.SelectedIndexChanged, AddressOf docPreviewTabCombo_SelectedIndexChanged
        End Sub

        Private Sub FillTabStripTextOrientation(combo As RadDropDownList, selected As TabStripTextOrientation)
            combo.BeginUpdate()
            combo.Items.Clear()

            Dim counter As Integer = 0
            Dim selectedIndex As Integer = -1
            For Each orientation As TabStripTextOrientation In [Enum].GetValues(GetType(TabStripTextOrientation))
                Dim item As New RadListDataItem(orientation.ToString())
                item.Value = orientation
                combo.Items.Add(item)

                If orientation = selected Then
                    selectedIndex = counter
                End If

                counter += 1
            Next

            combo.SelectedIndex = selectedIndex
            combo.EndUpdate()
        End Sub

        Private Sub FillTabStripAlignment(combo As RadDropDownList, selected As TabStripAlignment)
            combo.BeginUpdate()
            combo.Items.Clear()

            Dim counter As Integer = 0
            Dim selectedIndex As Integer = -1
            For Each align As TabStripAlignment In [Enum].GetValues(GetType(TabStripAlignment))
                Dim item As New RadListDataItem(align.ToString())
                item.Value = align
                combo.Items.Add(item)

                If align = selected Then
                    selectedIndex = counter
                End If

                counter += 1
            Next

            combo.SelectedIndex = selectedIndex
            combo.EndUpdate()
        End Sub

        Private Function GetTabStripAlignment(combo As RadDropDownList) As System.Nullable(Of TabStripAlignment)
            Dim item As RadListDataItem = TryCast(combo.SelectedItem, RadListDataItem)
            If item Is Nothing Then
                Return Nothing
            End If

            Return DirectCast(item.Value, TabStripAlignment)
        End Function

        Private Function GetTabStripTextOrientation(combo As RadDropDownList) As System.Nullable(Of TabStripTextOrientation)
            Dim item As RadListDataItem = TryCast(combo.SelectedItem, RadListDataItem)
            If item Is Nothing Then
                Return Nothing
            End If

            Return DirectCast(item.Value, TabStripTextOrientation)
        End Function

#End Region

#Region "Event Handlers"

        Private Sub docTextOrientationCombo_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim orientation As System.Nullable(Of TabStripTextOrientation) = Me.GetTabStripTextOrientation(Me.docTextOrientationCombo)
            If orientation IsNot Nothing Then
                Me.radDock1.DocumentTabsTextOrientation = orientation.Value
            End If
        End Sub

        Private Sub toolTextOrientationCombo_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim orientation As System.Nullable(Of TabStripTextOrientation) = Me.GetTabStripTextOrientation(Me.toolTextOrientationCombo)
            If orientation IsNot Nothing Then
                Me.radDock1.ToolTabsTextOrientation = orientation.Value
            End If
        End Sub

        Private Sub docCloseButtonCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDock1.ShowDocumentCloseButton = args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        End Sub

        Private Sub toolCloseButtonCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDock1.ShowToolCloseButton = args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        End Sub

        Private Sub docTabAlignCombo_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim align As System.Nullable(Of TabStripAlignment) = Me.GetTabStripAlignment(Me.docTabAlignCombo)
            If align IsNot Nothing Then
                Me.radDock1.DocumentTabsAlignment = align.Value
            End If
        End Sub

        Private Sub toolTabAlignCombo_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim align As System.Nullable(Of TabStripAlignment) = Me.GetTabStripAlignment(Me.toolTabAlignCombo)
            If align IsNot Nothing Then
                Me.radDock1.ToolTabsAlignment = align.Value
            End If
        End Sub

        Private Sub docTabsVisibleCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDock1.DocumentTabsVisible = Me.docTabsVisibleCheck.Checked
        End Sub

        Private Sub toolTabsVisibleCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDock1.ToolTabsVisible = Me.toolTabsVisibleCheck.Checked
        End Sub

        Private Sub docPinButtonCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDock1.ShowDocumentPinButton = args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        End Sub

        Private Sub docPreviewTabCombo_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Dim selectedDoc As DocumentWindow = TryCast(Me.docPreviewTabCombo.SelectedValue, DocumentWindow)
            Me.documentTabStrip1.TabStripElement.PreviewItem = If(selectedDoc IsNot Nothing AndAlso selectedDoc.DockState <> DockState.Hidden, selectedDoc.TabStripItem, Nothing)
        End Sub

#End Region

        Protected Overrides Sub WireEvents()
            AddHandler Me.docTabAlignCombo.SelectedIndexChanged, AddressOf docTabAlignCombo_SelectedIndexChanged
            AddHandler Me.toolTabAlignCombo.SelectedIndexChanged, AddressOf toolTabAlignCombo_SelectedIndexChanged
            AddHandler Me.docTabsVisibleCheck.ToggleStateChanged, AddressOf docTabsVisibleCheck_ToggleStateChanged
            AddHandler Me.toolTabsVisibleCheck.ToggleStateChanged, AddressOf Me.toolTabsVisibleCheck_ToggleStateChanged
            AddHandler Me.docCloseButtonCheck.ToggleStateChanged, AddressOf Me.docCloseButtonCheck_ToggleStateChanged
            AddHandler Me.docPinButtonCheck.ToggleStateChanged, AddressOf docPinButtonCheck_ToggleStateChanged
            AddHandler Me.docTextOrientationCombo.SelectedIndexChanged, AddressOf Me.docTextOrientationCombo_SelectedIndexChanged
            AddHandler Me.toolCloseButtonCheck.ToggleStateChanged, AddressOf toolCloseButtonCheck_ToggleStateChanged
            AddHandler Me.toolTextOrientationCombo.SelectedIndexChanged, AddressOf Me.toolTextOrientationCombo_SelectedIndexChanged
        End Sub

    End Class
End Namespace