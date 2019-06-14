Namespace Telerik.Examples.WinControls.Map.Settings
    Partial Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.radMap1 = New Telerik.WinControls.UI.RadMap()
            Me.radGroupBoxElementSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radCheckBoxShowMiniMap = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxShowScaleIndicator = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxShowNavigationBar = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxShowLegend = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxShowSearchBar = New Telerik.WinControls.UI.RadCheckBox()
            Me.radGroupBoxLegendSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radRadioButtonHorizontal = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButtonVertical = New Telerik.WinControls.UI.RadRadioButton()
            Me.radGroupBoxNavigationBarSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radDropDownListNavBarLocation = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabelNavBarLocation = New Telerik.WinControls.UI.RadLabel()
            Me.radGroupBoxMinimapSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radSpinEditorZoomOffset = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radDropDownListMinimapMode = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelMode = New Telerik.WinControls.UI.RadLabel()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radMap1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxElementSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxElementSettings.SuspendLayout()
            DirectCast(Me.radCheckBoxShowMiniMap, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxShowScaleIndicator, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxShowNavigationBar, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxShowLegend, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxShowSearchBar, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxLegendSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxLegendSettings.SuspendLayout()
            DirectCast(Me.radRadioButtonHorizontal, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButtonVertical, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxNavigationBarSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxNavigationBarSettings.SuspendLayout()
            DirectCast(Me.radDropDownListNavBarLocation, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabelNavBarLocation, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxMinimapSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxMinimapSettings.SuspendLayout()
            DirectCast(Me.radSpinEditorZoomOffset, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDropDownListMinimapMode, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabelMode, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBoxMinimapSettings)
            Me.settingsPanel.Controls.Add(Me.radGroupBoxNavigationBarSettings)
            Me.settingsPanel.Controls.Add(Me.radGroupBoxLegendSettings)
            Me.settingsPanel.Controls.Add(Me.radGroupBoxElementSettings)
            Me.settingsPanel.Size = New System.Drawing.Size(230, 786)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxElementSettings, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxLegendSettings, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxNavigationBarSettings, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxMinimapSettings, 0)
            ' 
            ' radMap1
            ' 
            Me.radMap1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radMap1.Location = New System.Drawing.Point(0, 0)
            Me.radMap1.Name = "radMap1"
            Me.radMap1.Size = New System.Drawing.Size(1282, 626)
            Me.radMap1.TabIndex = 0
            Me.radMap1.Text = "radMap1"
            ' 
            ' radGroupBoxElementSettings
            ' 
            Me.radGroupBoxElementSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxElementSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxElementSettings.Controls.Add(Me.radCheckBoxShowMiniMap)
            Me.radGroupBoxElementSettings.Controls.Add(Me.radCheckBoxShowScaleIndicator)
            Me.radGroupBoxElementSettings.Controls.Add(Me.radCheckBoxShowNavigationBar)
            Me.radGroupBoxElementSettings.Controls.Add(Me.radCheckBoxShowLegend)
            Me.radGroupBoxElementSettings.Controls.Add(Me.radCheckBoxShowSearchBar)
            Me.radGroupBoxElementSettings.HeaderText = "Element settings"
            Me.radGroupBoxElementSettings.Location = New System.Drawing.Point(10, 32)
            Me.radGroupBoxElementSettings.Name = "radGroupBoxElementSettings"
            Me.radGroupBoxElementSettings.Size = New System.Drawing.Size(210, 148)
            Me.radGroupBoxElementSettings.TabIndex = 1
            Me.radGroupBoxElementSettings.Text = "Element settings"
            ' 
            ' radCheckBoxShowMiniMap
            ' 
            Me.radCheckBoxShowMiniMap.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxShowMiniMap.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxShowMiniMap.Location = New System.Drawing.Point(5, 126)
            Me.radCheckBoxShowMiniMap.Name = "radCheckBoxShowMiniMap"
            Me.radCheckBoxShowMiniMap.Size = New System.Drawing.Size(97, 18)
            Me.radCheckBoxShowMiniMap.TabIndex = 0
            Me.radCheckBoxShowMiniMap.Text = "Show mini map"
            Me.radCheckBoxShowMiniMap.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBoxShowScaleIndicator
            ' 
            Me.radCheckBoxShowScaleIndicator.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxShowScaleIndicator.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxShowScaleIndicator.Location = New System.Drawing.Point(5, 102)
            Me.radCheckBoxShowScaleIndicator.Name = "radCheckBoxShowScaleIndicator"
            Me.radCheckBoxShowScaleIndicator.Size = New System.Drawing.Size(122, 18)
            Me.radCheckBoxShowScaleIndicator.TabIndex = 0
            Me.radCheckBoxShowScaleIndicator.Text = "Show scale indicator"
            Me.radCheckBoxShowScaleIndicator.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBoxShowNavigationBar
            ' 
            Me.radCheckBoxShowNavigationBar.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxShowNavigationBar.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxShowNavigationBar.Location = New System.Drawing.Point(5, 78)
            Me.radCheckBoxShowNavigationBar.Name = "radCheckBoxShowNavigationBar"
            Me.radCheckBoxShowNavigationBar.Size = New System.Drawing.Size(122, 18)
            Me.radCheckBoxShowNavigationBar.TabIndex = 0
            Me.radCheckBoxShowNavigationBar.Text = "Show navigation bar"
            Me.radCheckBoxShowNavigationBar.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBoxShowLegend
            ' 
            Me.radCheckBoxShowLegend.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxShowLegend.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxShowLegend.Location = New System.Drawing.Point(5, 54)
            Me.radCheckBoxShowLegend.Name = "radCheckBoxShowLegend"
            Me.radCheckBoxShowLegend.Size = New System.Drawing.Size(85, 18)
            Me.radCheckBoxShowLegend.TabIndex = 0
            Me.radCheckBoxShowLegend.Text = "Show legend"
            Me.radCheckBoxShowLegend.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBoxShowSearchBar
            ' 
            Me.radCheckBoxShowSearchBar.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxShowSearchBar.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxShowSearchBar.Location = New System.Drawing.Point(5, 30)
            Me.radCheckBoxShowSearchBar.Name = "radCheckBoxShowSearchBar"
            Me.radCheckBoxShowSearchBar.Size = New System.Drawing.Size(102, 18)
            Me.radCheckBoxShowSearchBar.TabIndex = 0
            Me.radCheckBoxShowSearchBar.Text = "Show search bar"
            Me.radCheckBoxShowSearchBar.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radGroupBoxLegendSettings
            ' 
            Me.radGroupBoxLegendSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxLegendSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxLegendSettings.Controls.Add(Me.radRadioButtonHorizontal)
            Me.radGroupBoxLegendSettings.Controls.Add(Me.radRadioButtonVertical)
            Me.radGroupBoxLegendSettings.HeaderText = "Legend settings"
            Me.radGroupBoxLegendSettings.Location = New System.Drawing.Point(10, 184)
            Me.radGroupBoxLegendSettings.Name = "radGroupBoxLegendSettings"
            Me.radGroupBoxLegendSettings.Size = New System.Drawing.Size(210, 75)
            Me.radGroupBoxLegendSettings.TabIndex = 2
            Me.radGroupBoxLegendSettings.Text = "Legend settings"
            ' 
            ' radRadioButtonHorizontal
            ' 
            Me.radRadioButtonHorizontal.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadioButtonHorizontal.Location = New System.Drawing.Point(5, 53)
            Me.radRadioButtonHorizontal.Name = "radRadioButtonHorizontal"
            Me.radRadioButtonHorizontal.Size = New System.Drawing.Size(72, 18)
            Me.radRadioButtonHorizontal.TabIndex = 0
            Me.radRadioButtonHorizontal.TabStop = False
            Me.radRadioButtonHorizontal.Text = "Horizontal"
            ' 
            ' radRadioButtonVertical
            ' 
            Me.radRadioButtonVertical.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadioButtonVertical.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radRadioButtonVertical.Location = New System.Drawing.Point(5, 30)
            Me.radRadioButtonVertical.Name = "radRadioButtonVertical"
            Me.radRadioButtonVertical.Size = New System.Drawing.Size(57, 18)
            Me.radRadioButtonVertical.TabIndex = 0
            Me.radRadioButtonVertical.Text = "Vertical"
            Me.radRadioButtonVertical.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radGroupBoxNavigationBarSettings
            ' 
            Me.radGroupBoxNavigationBarSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxNavigationBarSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxNavigationBarSettings.Controls.Add(Me.radDropDownListNavBarLocation)
            Me.radGroupBoxNavigationBarSettings.Controls.Add(Me.radLabelNavBarLocation)
            Me.radGroupBoxNavigationBarSettings.HeaderText = "Navigation bar settings"
            Me.radGroupBoxNavigationBarSettings.Location = New System.Drawing.Point(10, 263)
            Me.radGroupBoxNavigationBarSettings.Name = "radGroupBoxNavigationBarSettings"
            Me.radGroupBoxNavigationBarSettings.Size = New System.Drawing.Size(210, 80)
            Me.radGroupBoxNavigationBarSettings.TabIndex = 2
            Me.radGroupBoxNavigationBarSettings.Text = "Navigation bar settings"
            ' 
            ' radDropDownListNavBarLocation
            ' 
            Me.radDropDownListNavBarLocation.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListNavBarLocation.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListNavBarLocation.Location = New System.Drawing.Point(5, 51)
            Me.radDropDownListNavBarLocation.Name = "radDropDownListNavBarLocation"
            AddHandler Me.radDropDownListNavBarLocation.SelectedIndexChanged, AddressOf Me.radDropDownListNavBarLocation_SelectedIndexChanged
            Me.radDropDownListNavBarLocation.Size = New System.Drawing.Size(200, 20)
            Me.radDropDownListNavBarLocation.TabIndex = 1
            Me.radDropDownListNavBarLocation.Text = "Location"
            ' 
            ' radLabelNavBarLocation
            ' 
            Me.radLabelNavBarLocation.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelNavBarLocation.Location = New System.Drawing.Point(5, 30)
            Me.radLabelNavBarLocation.Name = "radLabelNavBarLocation"
            Me.radLabelNavBarLocation.Size = New System.Drawing.Size(48, 18)
            Me.radLabelNavBarLocation.TabIndex = 0
            Me.radLabelNavBarLocation.Text = "Location"
            ' 
            ' radGroupBoxMinimapSettings
            ' 
            Me.radGroupBoxMinimapSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxMinimapSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxMinimapSettings.Controls.Add(Me.radSpinEditorZoomOffset)
            Me.radGroupBoxMinimapSettings.Controls.Add(Me.radDropDownListMinimapMode)
            Me.radGroupBoxMinimapSettings.Controls.Add(Me.radLabel1)
            Me.radGroupBoxMinimapSettings.Controls.Add(Me.radLabelMode)
            Me.radGroupBoxMinimapSettings.HeaderText = "Mini map settings"
            Me.radGroupBoxMinimapSettings.Location = New System.Drawing.Point(10, 347)
            Me.radGroupBoxMinimapSettings.Name = "radGroupBoxMinimapSettings"
            Me.radGroupBoxMinimapSettings.Size = New System.Drawing.Size(210, 130)
            Me.radGroupBoxMinimapSettings.TabIndex = 2
            Me.radGroupBoxMinimapSettings.Text = "Mini map settings"
            ' 
            ' radSpinEditorZoomOffset
            ' 
            Me.radSpinEditorZoomOffset.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorZoomOffset.Enabled = False
            Me.radSpinEditorZoomOffset.Location = New System.Drawing.Point(5, 101)
            Me.radSpinEditorZoomOffset.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.radSpinEditorZoomOffset.Name = "radSpinEditorZoomOffset"
            Me.radSpinEditorZoomOffset.Size = New System.Drawing.Size(200, 20)
            Me.radSpinEditorZoomOffset.TabIndex = 2
            Me.radSpinEditorZoomOffset.TabStop = False
            Me.radSpinEditorZoomOffset.Value = New Decimal(New Integer() {5, 0, 0, 0})
            ' 
            ' radDropDownListMinimapMode
            ' 
            Me.radDropDownListMinimapMode.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListMinimapMode.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListMinimapMode.Location = New System.Drawing.Point(5, 51)
            Me.radDropDownListMinimapMode.Name = "radDropDownListMinimapMode"
            AddHandler Me.radDropDownListMinimapMode.SelectedIndexChanged, AddressOf Me.radDropDownListMinimapMode_SelectedIndexChanged
            Me.radDropDownListMinimapMode.Size = New System.Drawing.Size(200, 20)
            Me.radDropDownListMinimapMode.TabIndex = 1
            Me.radDropDownListMinimapMode.Text = "Mode"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(5, 80)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(66, 18)
            Me.radLabel1.TabIndex = 0
            Me.radLabel1.Text = "Zoom offset"
            ' 
            ' radLabelMode
            ' 
            Me.radLabelMode.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelMode.Location = New System.Drawing.Point(5, 30)
            Me.radLabelMode.Name = "radLabelMode"
            Me.radLabelMode.Size = New System.Drawing.Size(35, 18)
            Me.radLabelMode.TabIndex = 0
            Me.radLabelMode.Text = "Mode"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radMap1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1292, 636)
            Me.Controls.SetChildIndex(Me.radMap1, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radMap1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxElementSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxElementSettings.ResumeLayout(False)
            Me.radGroupBoxElementSettings.PerformLayout()
            DirectCast(Me.radCheckBoxShowMiniMap, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxShowScaleIndicator, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxShowNavigationBar, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxShowLegend, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxShowSearchBar, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxLegendSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxLegendSettings.ResumeLayout(False)
            Me.radGroupBoxLegendSettings.PerformLayout()
            DirectCast(Me.radRadioButtonHorizontal, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButtonVertical, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxNavigationBarSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxNavigationBarSettings.ResumeLayout(False)
            Me.radGroupBoxNavigationBarSettings.PerformLayout()
            DirectCast(Me.radDropDownListNavBarLocation, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabelNavBarLocation, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxMinimapSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxMinimapSettings.ResumeLayout(False)
            Me.radGroupBoxMinimapSettings.PerformLayout()
            DirectCast(Me.radSpinEditorZoomOffset, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDropDownListMinimapMode, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabelMode, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private WithEvents radMap1 As Telerik.WinControls.UI.RadMap
        Private WithEvents radGroupBoxMinimapSettings As Telerik.WinControls.UI.RadGroupBox
        Private WithEvents radGroupBoxNavigationBarSettings As Telerik.WinControls.UI.RadGroupBox
        Private WithEvents radDropDownListNavBarLocation As Telerik.WinControls.UI.RadDropDownList
        Private WithEvents radLabelNavBarLocation As Telerik.WinControls.UI.RadLabel
        Private WithEvents radGroupBoxLegendSettings As Telerik.WinControls.UI.RadGroupBox
        Private WithEvents radRadioButtonHorizontal As Telerik.WinControls.UI.RadRadioButton
        Private WithEvents radRadioButtonVertical As Telerik.WinControls.UI.RadRadioButton
        Private WithEvents radGroupBoxElementSettings As Telerik.WinControls.UI.RadGroupBox
        Private WithEvents radCheckBoxShowMiniMap As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radCheckBoxShowScaleIndicator As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radCheckBoxShowNavigationBar As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radCheckBoxShowLegend As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radCheckBoxShowSearchBar As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radSpinEditorZoomOffset As Telerik.WinControls.UI.RadSpinEditor
        Private WithEvents radDropDownListMinimapMode As Telerik.WinControls.UI.RadDropDownList
        Private WithEvents radLabel1 As Telerik.WinControls.UI.RadLabel
        Private WithEvents radLabelMode As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace