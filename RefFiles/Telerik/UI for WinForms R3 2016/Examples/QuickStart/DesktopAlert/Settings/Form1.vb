Imports System.Drawing
Imports Telerik.Examples.WinControls.Editors.ComboBox
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.DesktopAlert.Settings
    Partial Public Class Form1
        Inherits EditorExampleBaseForm
        Public Sub New()
            InitializeComponent()

            Me.FillDropDownThemes()
            Me.radDesktopAlert1.ThemeName = Me.ddThemeName.SelectedItem.Text
        End Sub

        Protected Overrides Sub WireEvents()
            AddHandler Me.closeButtonCheck.ToggleStateChanged, AddressOf Me.closeButtonCheck_ToggleStateChanged
            AddHandler Me.pinButtonCheck.ToggleStateChanged, AddressOf Me.pinButtonCheck_ToggleStateChanged
            AddHandler Me.ddScreenPosition.SelectedIndexChanged, AddressOf Me.OnPositionList_IndexChaned
            AddHandler Me.ddThemeName.SelectedIndexChanged, AddressOf Me.OnThemesList_IndexChanged
            AddHandler Me.fadeOutCheck.ToggleStateChanged, AddressOf Me.fadeOutCheck_ToggleStateChanged
            AddHandler Me.checkPopupAnimation.ToggleStateChanged, AddressOf Me.checkPopupAnimation_ToggleStateChanged
            AddHandler Me.fadeInCheck.ToggleStateChanged, AddressOf Me.fadeInCheck_ToggleStateChanged
            AddHandler Me.spinPopupAnimationFrames.ValueChanged, AddressOf Me.spinPopupAnimationFrames_ValueChanged
            AddHandler Me.btnPreview.Click, AddressOf Me.btnPreview_Click
            AddHandler Me.spinFadeDuration.ValueChanged, AddressOf Me.spinFadeDuration_ValueChanged
            AddHandler Me.autoCloseCheck.ToggleStateChanged, AddressOf Me.autoCloseCheck_ToggleStateChanged
            AddHandler Me.spinFadeDuration.ValueChanged, AddressOf Me.spinFadeDuration_ValueChanged
            AddHandler Me.spinOpacity.ValueChanged, AddressOf Me.SpinOpacity_ValueChanged
            AddHandler Me.optionsButtonCheck.ToggleStateChanged, AddressOf Me.optionsButtonCheck_ToggleStateChanged
            AddHandler Me.spinEditorHeight.ValueChanged, AddressOf Me.SpinEditorHeight_ValueChanged
            AddHandler Me.spinEditorWidth.ValueChanged, AddressOf Me.SpinEditorWidth_ValueChanged
            AddHandler Me.autoCloseDelaySpin.ValueChanged, AddressOf Me.autoCloseDelaySpin_ValueChanged
            AddHandler Me.autoCloseCheck.ToggleStateChanged, AddressOf Me.autoCloseCheck_ToggleStateChanged
            AddHandler Me.ddAnimationDirection.SelectedIndexChanged, AddressOf Me.ddAnimationDirection_SelectedIndexChanged
            AddHandler Me.ddThemeName.ThemeNameChanged, AddressOf ddThemeName_ThemeNameChanged
        End Sub

        Private Sub FillDropDownThemes()
            For Each themeName As String In Utils.AllThemes
                Me.ddThemeName.Items.Add(themeName)
            Next

            ' This is needed to set the order of themes in the DropDownList the same as ThemePanel.
            Me.ddThemeName.Items.RemoveAt(Me.ddThemeName.Items.IndexOf(Utils.ThemeName))
            Me.ddThemeName.Items.Insert(0, New RadListDataItem(Utils.ThemeName))
            Me.ddThemeName.Items(0).Selected = True
        End Sub

#Region "Event handling"

        Private Sub OnThemesList_IndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.radDesktopAlert1.ThemeName = Me.ddThemeName.SelectedItem.Text
        End Sub

        Private Sub OnPositionList_IndexChaned(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.radDesktopAlert1.ScreenPosition = DirectCast([Enum].Parse(GetType(AlertScreenPosition), Me.ddScreenPosition.SelectedItem.Text), AlertScreenPosition)
        End Sub

        Private Sub SpinEditorWidth_ValueChanged(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.FixedSize = New Size(CInt(Me.spinEditorWidth.Value), CInt(Me.spinEditorHeight.Value))
        End Sub
        Private Sub SpinEditorHeight_ValueChanged(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.FixedSize = New Size(CInt(Me.spinEditorWidth.Value), CInt(Me.spinEditorHeight.Value))
        End Sub

        Private Sub fadeInCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            If Me.fadeInCheck.ToggleState = ToggleState.[On] Then
                Me.radDesktopAlert1.FadeAnimationType = Me.radDesktopAlert1.FadeAnimationType Or FadeAnimationType.FadeIn
            Else
                Me.radDesktopAlert1.FadeAnimationType = Me.radDesktopAlert1.FadeAnimationType And Not FadeAnimationType.FadeIn
            End If
        End Sub

        Private Sub fadeOutCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            If Me.fadeOutCheck.ToggleState = ToggleState.[On] Then
                Me.radDesktopAlert1.FadeAnimationType = Me.radDesktopAlert1.FadeAnimationType Or FadeAnimationType.FadeOut
            Else
                Me.radDesktopAlert1.FadeAnimationType = (Me.radDesktopAlert1.FadeAnimationType And Not FadeAnimationType.FadeOut)
            End If
        End Sub

        Private Sub spinFadeDuration_ValueChanged(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.FadeAnimationFrames = CInt(Me.spinFadeDuration.Value)
        End Sub

        Private Sub autoCloseCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDesktopAlert1.AutoClose = Me.autoCloseCheck.ToggleState = ToggleState.[On]
        End Sub

        Private Sub closeButtonCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDesktopAlert1.ShowCloseButton = args.ToggleState = ToggleState.[On]
        End Sub

        Private Sub pinButtonCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDesktopAlert1.ShowPinButton = args.ToggleState = ToggleState.[On]
        End Sub

        Private Sub optionsButtonCheck_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDesktopAlert1.ShowOptionsButton = args.ToggleState = ToggleState.[On]
        End Sub

        Private Sub checkPopupAnimation_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radDesktopAlert1.PopupAnimation = args.ToggleState = ToggleState.[On]
        End Sub

        Private Sub ddAnimationDirection_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.radDesktopAlert1.PopupAnimationDirection = DirectCast([Enum].Parse(GetType(RadDirection), Me.ddAnimationDirection.SelectedItem.Text), RadDirection)
        End Sub

        Private Sub spinPopupAnimationFrames_ValueChanged(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.PopupAnimationFrames = CInt(Me.spinPopupAnimationFrames.Value)
        End Sub

        Private Sub btnPreview_Click(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.CaptionText = Me.txtCaption.Text
            Me.radDesktopAlert1.ContentText = Me.txtContent.Text
            Me.radDesktopAlert1.Show()
        End Sub

        Private Sub SpinOpacity_ValueChanged(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.Opacity = CSng(Me.spinOpacity.Value)
        End Sub

        Private Sub autoCloseDelaySpin_ValueChanged(sender As Object, e As EventArgs)
            Me.radDesktopAlert1.AutoCloseDelay = CInt(Me.autoCloseDelaySpin.Value)
        End Sub

        Private Sub ddThemeName_ThemeNameChanged(source As Object, args As ThemeNameChangedEventArgs)
            Me.ddThemeName.SelectedIndex = Me.ddThemeName.Items.IndexOf(args.newThemeName)
        End Sub

#End Region
    End Class
End Namespace