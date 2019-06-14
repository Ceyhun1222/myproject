Namespace Telerik.Examples.WinControls.CardView.Settings
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
            Me.components = New System.ComponentModel.Container()
            Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
            Dim listViewDetailColumn1 As New Telerik.WinControls.UI.ListViewDetailColumn("Column 0", "Image")
            Dim listViewDetailColumn2 As New Telerik.WinControls.UI.ListViewDetailColumn("Column 1", "City")
            Dim listViewDetailColumn3 As New Telerik.WinControls.UI.ListViewDetailColumn("Column 2", "Population")
            Dim listViewDetailColumn4 As New Telerik.WinControls.UI.ListViewDetailColumn("Column 3", "Country")
            Me.radGroupBoxProperties = New Telerik.WinControls.UI.RadGroupBox()
            Me.radDropDownListOrientation = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabelOrientation = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckBoxAllowCustomize = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxHotTracking = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxShowGroups = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxAllowEdit = New Telerik.WinControls.UI.RadCheckBox()
            Me.radGroupBoxVisualSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radColorBoxBorderColor = New Telerik.WinControls.UI.RadColorBox()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radBrowseEditorFont = New Telerik.WinControls.UI.RadBrowseEditor()
            Me.radColorBoxForeColor = New Telerik.WinControls.UI.RadColorBox()
            Me.radColorBoxBackColor = New Telerik.WinControls.UI.RadColorBox()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownListVisualItems = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabelSelectItem = New Telerik.WinControls.UI.RadLabel()
            Me.radCardView1 = New Telerik.WinControls.UI.RadCardView()
            Me.cardViewGroupItem1 = New Telerik.WinControls.UI.CardViewGroupItem()
            Me.cardViewItem1 = New Telerik.WinControls.UI.CardViewItem()
            Me.cardViewItem2 = New Telerik.WinControls.UI.CardViewItem()
            Me.cardViewItem3 = New Telerik.WinControls.UI.CardViewItem()
            Me.cardViewItem4 = New Telerik.WinControls.UI.CardViewItem()
            Me.radContextMenu1 = New Telerik.WinControls.UI.RadContextMenu(Me.components)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxProperties.SuspendLayout()
            DirectCast(Me.radDropDownListOrientation, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabelOrientation, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxAllowCustomize, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxHotTracking, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxShowGroups, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxAllowEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxVisualSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxVisualSettings.SuspendLayout()
            DirectCast(Me.radColorBoxBorderColor, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radBrowseEditorFont, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radColorBoxForeColor, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radColorBoxBackColor, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDropDownListVisualItems, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabelSelectItem, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCardView1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCardView1.CardTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radCardView1.SuspendLayout()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBoxVisualSettings)
            Me.settingsPanel.Controls.Add(Me.radGroupBoxProperties)
            Me.settingsPanel.Location = New System.Drawing.Point(1238, 20)
            Me.settingsPanel.Size = New System.Drawing.Size(235, 393)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxProperties, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxVisualSettings, 0)
            ' 
            ' themePanel
            ' 
            Me.themePanel.Location = New System.Drawing.Point(1243, 419)
            ' 
            ' radGroupBoxProperties
            ' 
            Me.radGroupBoxProperties.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxProperties.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxProperties.Controls.Add(Me.radDropDownListOrientation)
            Me.radGroupBoxProperties.Controls.Add(Me.radLabelOrientation)
            Me.radGroupBoxProperties.Controls.Add(Me.radCheckBoxAllowCustomize)
            Me.radGroupBoxProperties.Controls.Add(Me.radCheckBoxHotTracking)
            Me.radGroupBoxProperties.Controls.Add(Me.radCheckBoxShowGroups)
            Me.radGroupBoxProperties.Controls.Add(Me.radCheckBoxAllowEdit)
            Me.radGroupBoxProperties.HeaderText = "Properties"
            Me.radGroupBoxProperties.Location = New System.Drawing.Point(10, 33)
            Me.radGroupBoxProperties.Name = "radGroupBoxProperties"
            Me.radGroupBoxProperties.Size = New System.Drawing.Size(215, 152)
            Me.radGroupBoxProperties.TabIndex = 1
            Me.radGroupBoxProperties.Tag = "1"
            Me.radGroupBoxProperties.Text = "Properties"
            ' 
            ' radDropDownListOrientation
            ' 
            Me.radDropDownListOrientation.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListOrientation.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            radListDataItem1.Selected = True
            radListDataItem1.Text = "Horizontal"
            radListDataItem2.Text = "Vertical"
            Me.radDropDownListOrientation.Items.Add(radListDataItem1)
            Me.radDropDownListOrientation.Items.Add(radListDataItem2)
            Me.radDropDownListOrientation.Location = New System.Drawing.Point(105, 121)
            Me.radDropDownListOrientation.Name = "radDropDownListOrientation"
            Me.radDropDownListOrientation.Size = New System.Drawing.Size(96, 20)
            Me.radDropDownListOrientation.TabIndex = 7
            Me.radDropDownListOrientation.Tag = "1"
            Me.radDropDownListOrientation.Text = "Horizontal"
            AddHandler Me.radDropDownListOrientation.SelectedIndexChanged, AddressOf Me.radDropDownListOrientation_SelectedIndexChanged
            ' 
            ' radLabelOrientation
            ' 
            Me.radLabelOrientation.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelOrientation.Location = New System.Drawing.Point(5, 123)
            Me.radLabelOrientation.Name = "radLabelOrientation"
            Me.radLabelOrientation.Size = New System.Drawing.Size(92, 18)
            Me.radLabelOrientation.TabIndex = 6
            Me.radLabelOrientation.Text = "Items Orientation"
            ' 
            ' radCheckBoxAllowCustomize
            ' 
            Me.radCheckBoxAllowCustomize.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxAllowCustomize.Location = New System.Drawing.Point(5, 97)
            Me.radCheckBoxAllowCustomize.Name = "radCheckBoxAllowCustomize"
            Me.radCheckBoxAllowCustomize.Size = New System.Drawing.Size(176, 18)
            Me.radCheckBoxAllowCustomize.TabIndex = 3
            Me.radCheckBoxAllowCustomize.Text = "Allow Customize CardTemplate"
            AddHandler Me.radCheckBoxAllowCustomize.ToggleStateChanged, AddressOf Me.radCheckBoxAllowCustomize_ToggleStateChanged
            ' 
            ' radCheckBoxHotTracking
            ' 
            Me.radCheckBoxHotTracking.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxHotTracking.Location = New System.Drawing.Point(5, 72)
            Me.radCheckBoxHotTracking.Name = "radCheckBoxHotTracking"
            Me.radCheckBoxHotTracking.Size = New System.Drawing.Size(117, 18)
            Me.radCheckBoxHotTracking.TabIndex = 2
            Me.radCheckBoxHotTracking.Text = "Enable HotTracking"
            AddHandler Me.radCheckBoxHotTracking.ToggleStateChanged, AddressOf Me.radCheckBoxHotTracking_ToggleStateChanged
            ' 
            ' radCheckBoxShowGroups
            ' 
            Me.radCheckBoxShowGroups.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxShowGroups.Location = New System.Drawing.Point(5, 47)
            Me.radCheckBoxShowGroups.Name = "radCheckBoxShowGroups"
            Me.radCheckBoxShowGroups.Size = New System.Drawing.Size(86, 18)
            Me.radCheckBoxShowGroups.TabIndex = 1
            Me.radCheckBoxShowGroups.Text = "Show groups"
            AddHandler Me.radCheckBoxShowGroups.ToggleStateChanged, AddressOf Me.radCheckBoxShowGroups_ToggleStateChanged
            ' 
            ' radCheckBoxAllowEdit
            ' 
            Me.radCheckBoxAllowEdit.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxAllowEdit.Location = New System.Drawing.Point(5, 22)
            Me.radCheckBoxAllowEdit.Name = "radCheckBoxAllowEdit"
            Me.radCheckBoxAllowEdit.Size = New System.Drawing.Size(109, 18)
            Me.radCheckBoxAllowEdit.TabIndex = 0
            Me.radCheckBoxAllowEdit.Text = "Enable Edit Items "
            AddHandler Me.radCheckBoxAllowEdit.ToggleStateChanged, AddressOf Me.radCheckBoxAllowEdit_ToggleStateChanged
            ' 
            ' radGroupBoxVisualSettings
            ' 
            Me.radGroupBoxVisualSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxVisualSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radColorBoxBorderColor)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radLabel4)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radBrowseEditorFont)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radColorBoxForeColor)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radColorBoxBackColor)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radLabel3)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radLabel2)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radLabel1)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radDropDownListVisualItems)
            Me.radGroupBoxVisualSettings.Controls.Add(Me.radLabelSelectItem)
            Me.radGroupBoxVisualSettings.HeaderText = "Edit Visual Settings"
            Me.radGroupBoxVisualSettings.Location = New System.Drawing.Point(10, 191)
            Me.radGroupBoxVisualSettings.Name = "radGroupBoxVisualSettings"
            Me.radGroupBoxVisualSettings.Size = New System.Drawing.Size(215, 189)
            Me.radGroupBoxVisualSettings.TabIndex = 2
            Me.radGroupBoxVisualSettings.Tag = "1"
            Me.radGroupBoxVisualSettings.Text = "Edit Visual Settings"
            ' 
            ' radColorBoxBorderColor
            ' 
            Me.radColorBoxBorderColor.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radColorBoxBorderColor.Location = New System.Drawing.Point(83, 153)
            Me.radColorBoxBorderColor.Name = "radColorBoxBorderColor"
            Me.radColorBoxBorderColor.Size = New System.Drawing.Size(118, 20)
            Me.radColorBoxBorderColor.TabIndex = 10
            Me.radColorBoxBorderColor.Tag = "1"
            Me.radColorBoxBorderColor.Text = "radColorBox1"
            AddHandler Me.radColorBoxBorderColor.ValueChanged, AddressOf Me.radColorBoxBorderColor_ValueChanged
            ' 
            ' radLabel4
            ' 
            Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel4.Location = New System.Drawing.Point(5, 156)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(66, 18)
            Me.radLabel4.TabIndex = 8
            Me.radLabel4.Text = "BorderColor"
            ' 
            ' radBrowseEditorFont
            ' 
            Me.radBrowseEditorFont.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radBrowseEditorFont.DialogType = Telerik.WinControls.UI.BrowseEditorDialogType.FontDialog
            Me.radBrowseEditorFont.Location = New System.Drawing.Point(83, 119)
            Me.radBrowseEditorFont.Name = "radBrowseEditorFont"
            Me.radBrowseEditorFont.Size = New System.Drawing.Size(118, 20)
            Me.radBrowseEditorFont.TabIndex = 7
            Me.radBrowseEditorFont.Tag = "1"
            Me.radBrowseEditorFont.Text = "radBrowseEditor1"
            AddHandler Me.radBrowseEditorFont.ValueChanged, AddressOf Me.radBrowseEditorFont_ValueChanged
            ' 
            ' radColorBoxForeColor
            ' 
            Me.radColorBoxForeColor.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radColorBoxForeColor.Location = New System.Drawing.Point(84, 85)
            Me.radColorBoxForeColor.Name = "radColorBoxForeColor"
            Me.radColorBoxForeColor.Size = New System.Drawing.Size(117, 20)
            Me.radColorBoxForeColor.TabIndex = 6
            Me.radColorBoxForeColor.Tag = "1"
            Me.radColorBoxForeColor.Text = "radColorBox2"
            AddHandler Me.radColorBoxForeColor.ValueChanged, AddressOf Me.radColorBoxForeColor_ValueChanged
            ' 
            ' radColorBoxBackColor
            ' 
            Me.radColorBoxBackColor.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radColorBoxBackColor.Location = New System.Drawing.Point(84, 53)
            Me.radColorBoxBackColor.Name = "radColorBoxBackColor"
            Me.radColorBoxBackColor.Size = New System.Drawing.Size(117, 20)
            Me.radColorBoxBackColor.TabIndex = 5
            Me.radColorBoxBackColor.Tag = "1"
            Me.radColorBoxBackColor.Text = "radColorBox1"
            AddHandler Me.radColorBoxBackColor.ValueChanged, AddressOf Me.radColorBoxBackColor_ValueChanged
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel3.Location = New System.Drawing.Point(5, 120)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(28, 18)
            Me.radLabel3.TabIndex = 4
            Me.radLabel3.Text = "Font"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(5, 87)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(54, 18)
            Me.radLabel2.TabIndex = 3
            Me.radLabel2.Text = "ForeColor"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(5, 55)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(55, 18)
            Me.radLabel1.TabIndex = 2
            Me.radLabel1.Text = "BackColor"
            ' 
            ' radDropDownListVisualItems
            ' 
            Me.radDropDownListVisualItems.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListVisualItems.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListVisualItems.Location = New System.Drawing.Point(84, 21)
            Me.radDropDownListVisualItems.Name = "radDropDownListVisualItems"
            Me.radDropDownListVisualItems.Size = New System.Drawing.Size(117, 20)
            Me.radDropDownListVisualItems.TabIndex = 1
            Me.radDropDownListVisualItems.Tag = "1"
            AddHandler Me.radDropDownListVisualItems.SelectedIndexChanged, AddressOf Me.radDropDownListVisualItems_SelectedIndexChanged
            ' 
            ' radLabelSelectItem
            ' 
            Me.radLabelSelectItem.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelSelectItem.Location = New System.Drawing.Point(5, 21)
            Me.radLabelSelectItem.Name = "radLabelSelectItem"
            Me.radLabelSelectItem.Size = New System.Drawing.Size(63, 18)
            Me.radLabelSelectItem.TabIndex = 0
            Me.radLabelSelectItem.Text = "Select Item:"
            ' 
            ' radCardView1
            ' 
            Me.radCardView1.AllowArbitraryItemHeight = False
            ' 
            ' radCardView1Template
            ' 
            Me.radCardView1.CardTemplate.HiddenItems.AddRange(New Telerik.WinControls.RadItem() {Me.cardViewGroupItem1})
            Me.radCardView1.CardTemplate.Items.AddRange(New Telerik.WinControls.RadItem() {Me.cardViewItem1, Me.cardViewItem2, Me.cardViewItem3, Me.cardViewItem4})
            Me.radCardView1.CardTemplate.Location = New System.Drawing.Point(0, 0)
            Me.radCardView1.CardTemplate.Name = "radCardView1Template"
            Me.radCardView1.CardTemplate.Size = New System.Drawing.Size(220, 300)
            Me.radCardView1.CardTemplate.TabIndex = 0
            listViewDetailColumn1.HeaderText = "Image"
            listViewDetailColumn2.HeaderText = "City"
            listViewDetailColumn3.HeaderText = "Population"
            listViewDetailColumn4.HeaderText = "Country"
            Me.radCardView1.Columns.AddRange(New Telerik.WinControls.UI.ListViewDetailColumn() {listViewDetailColumn1, listViewDetailColumn2, listViewDetailColumn3, listViewDetailColumn4})
            Me.radCardView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radCardView1.ItemSize = New System.Drawing.Size(220, 300)
            Me.radCardView1.Location = New System.Drawing.Point(0, 0)
            Me.radCardView1.Name = "radCardView1"
            Me.radCardView1.SelectLastAddedItem = False
            Me.radCardView1.Size = New System.Drawing.Size(1501, 907)
            Me.radCardView1.TabIndex = 2
            Me.radCardView1.Text = "radCardView1"
            ' 
            ' cardViewGroupItem1
            ' 
            Me.cardViewGroupItem1.Bounds = New System.Drawing.Rectangle(0, 0, 230, 320)
            Me.cardViewGroupItem1.Name = "cardViewGroupItem1"
            ' 
            ' cardViewItem1
            ' 
            Me.cardViewItem1.Bounds = New System.Drawing.Rectangle(0, 0, 220, 222)
            Me.cardViewItem1.FieldName = "Column 0"
            Me.cardViewItem1.Name = "cardViewItem1"
            Me.cardViewItem1.Text = "Column 0"
            Me.cardViewItem1.TextProportionalSize = 0.0F
            ' 
            ' cardViewItem2
            ' 
            Me.cardViewItem2.Bounds = New System.Drawing.Rectangle(0, 222, 220, 26)
            Me.cardViewItem2.FieldName = "Column 1"
            Me.cardViewItem2.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold)
            Me.cardViewItem2.Name = "cardViewItem2"
            Me.cardViewItem2.Text = "Column 1"
            Me.cardViewItem2.TextProportionalSize = 0.35F
            ' 
            ' cardViewItem3
            ' 
            Me.cardViewItem3.Bounds = New System.Drawing.Rectangle(0, 248, 220, 26)
            Me.cardViewItem3.FieldName = "Column 2"
            Me.cardViewItem3.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold)
            Me.cardViewItem3.Name = "cardViewItem3"
            Me.cardViewItem3.Text = "Column 2"
            Me.cardViewItem3.TextProportionalSize = 0.35F
            ' 
            ' cardViewItem4
            ' 
            Me.cardViewItem4.Bounds = New System.Drawing.Rectangle(0, 274, 220, 26)
            Me.cardViewItem4.FieldName = "Column 3"
            Me.cardViewItem4.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold)
            Me.cardViewItem4.Name = "cardViewItem4"
            Me.cardViewItem4.Text = "Column 3"
            Me.cardViewItem4.TextProportionalSize = 0.35F
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radCardView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1511, 917)
            AddHandler Me.Load, AddressOf Me.Form1_Load
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radCardView1, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxProperties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxProperties.ResumeLayout(False)
            Me.radGroupBoxProperties.PerformLayout()
            DirectCast(Me.radDropDownListOrientation, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabelOrientation, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxAllowCustomize, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxHotTracking, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxShowGroups, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxAllowEdit, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxVisualSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxVisualSettings.ResumeLayout(False)
            Me.radGroupBoxVisualSettings.PerformLayout()
            DirectCast(Me.radColorBoxBorderColor, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radBrowseEditorFont, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radColorBoxForeColor, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radColorBoxBackColor, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDropDownListVisualItems, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabelSelectItem, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCardView1.CardTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCardView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radCardView1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radGroupBoxProperties As Telerik.WinControls.UI.RadGroupBox
        Private radDropDownListOrientation As Telerik.WinControls.UI.RadDropDownList
        Private radLabelOrientation As Telerik.WinControls.UI.RadLabel
        Private radCheckBoxAllowCustomize As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxHotTracking As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxShowGroups As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxAllowEdit As Telerik.WinControls.UI.RadCheckBox
        Private radGroupBoxVisualSettings As Telerik.WinControls.UI.RadGroupBox
        Private radDropDownListVisualItems As Telerik.WinControls.UI.RadDropDownList
        Private radLabelSelectItem As Telerik.WinControls.UI.RadLabel
        Private radCardView1 As Telerik.WinControls.UI.RadCardView
        Private cardViewGroupItem1 As Telerik.WinControls.UI.CardViewGroupItem
        Private cardViewItem1 As Telerik.WinControls.UI.CardViewItem
        Private cardViewItem2 As Telerik.WinControls.UI.CardViewItem
        Private cardViewItem3 As Telerik.WinControls.UI.CardViewItem
        Private cardViewItem4 As Telerik.WinControls.UI.CardViewItem
        Private radColorBoxForeColor As Telerik.WinControls.UI.RadColorBox
        Private radColorBoxBackColor As Telerik.WinControls.UI.RadColorBox
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radContextMenu1 As Telerik.WinControls.UI.RadContextMenu
        Private radColorBoxBorderColor As Telerik.WinControls.UI.RadColorBox
        Private radLabel4 As Telerik.WinControls.UI.RadLabel
        Private radBrowseEditorFont As Telerik.WinControls.UI.RadBrowseEditor
    End Class
End Namespace