
Namespace Telerik.Examples.WinControls.VirtualGrid.Settings
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
			Me.radVirtualGrid1 = New Telerik.WinControls.UI.RadVirtualGrid()
			Me.northwindDataSet = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
			Me.employeesTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorHeaderRowHeight = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorNewRowHeight = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorDataRowHeight = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorColumnWidth = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel5 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorCellSpacing = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel6 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorRowSpacing = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radCheckBoxShowHeaderRow = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxShowNewRow = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxShowFilterRow = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxFitColumns = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxAlternatingRowColors = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxHotTracking = New Telerik.WinControls.UI.RadCheckBox()
			Me.radLabel7 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorFilterRowHeight = New Telerik.WinControls.UI.RadSpinEditor()
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radVirtualGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorHeaderRowHeight, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorNewRowHeight, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorDataRowHeight, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorColumnWidth, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorCellSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorRowSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radCheckBoxShowHeaderRow, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radCheckBoxShowNewRow, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radCheckBoxShowFilterRow, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radCheckBoxFitColumns, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radCheckBoxAlternatingRowColors, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radCheckBoxHotTracking, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorFilterRowHeight, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radCheckBoxHotTracking)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxAlternatingRowColors)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxFitColumns)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxShowFilterRow)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxShowNewRow)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxShowHeaderRow)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorRowSpacing)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorCellSpacing)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorColumnWidth)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorDataRowHeight)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorFilterRowHeight)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorNewRowHeight)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorHeaderRowHeight)
			Me.settingsPanel.Controls.Add(Me.radLabel6)
			Me.settingsPanel.Controls.Add(Me.radLabel5)
			Me.settingsPanel.Controls.Add(Me.radLabel4)
			Me.settingsPanel.Controls.Add(Me.radLabel3)
			Me.settingsPanel.Controls.Add(Me.radLabel7)
			Me.settingsPanel.Controls.Add(Me.radLabel2)
			Me.settingsPanel.Controls.Add(Me.radLabel1)
			Me.settingsPanel.Location = New System.Drawing.Point(955, 11)
			Me.settingsPanel.Size = New System.Drawing.Size(230, 615)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel2, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel7, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel3, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel4, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel5, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel6, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorHeaderRowHeight, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorNewRowHeight, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorFilterRowHeight, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorDataRowHeight, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorColumnWidth, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorCellSpacing, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorRowSpacing, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxShowHeaderRow, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxShowNewRow, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxShowFilterRow, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxFitColumns, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxAlternatingRowColors, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxHotTracking, 0)
			' 
			' radVirtualGrid1
			' 
			Me.radVirtualGrid1.AllowCut = False
			Me.radVirtualGrid1.AllowDelete = False
			Me.radVirtualGrid1.AllowEdit = False
			Me.radVirtualGrid1.AllowPaste = False
			Me.radVirtualGrid1.AllowSorting = False
			Me.radVirtualGrid1.Location = New System.Drawing.Point(0, 0)
			Me.radVirtualGrid1.Name = "radVirtualGrid1"
			Me.radVirtualGrid1.Size = New System.Drawing.Size(800, 600)
			Me.radVirtualGrid1.StandardTab = False
			Me.radVirtualGrid1.TabIndex = 0
			Me.radVirtualGrid1.Text = "radVirtualGrid1"
			AddHandler Me.radVirtualGrid1.CellValueNeeded, AddressOf Me.radVirtualGrid1_CellValueNeeded
			Addhandler Me.radVirtualGrid1.CellFormatting, AddressOf Me.radVirtualGrid1_CellFormatting
			' 
			' northwindDataSet
			' 
			Me.northwindDataSet.DataSetName = "NorthwindDataSet"
			Me.northwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
			' 
			' employeesTableAdapter
			' 
			Me.employeesTableAdapter.ClearBeforeFill = True
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel1.Location = New System.Drawing.Point(10, 33)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New System.Drawing.Size(98, 18)
			Me.radLabel1.TabIndex = 1
			Me.radLabel1.Text = "Header row height"
			' 
			' radSpinEditorHeaderRowHeight
			' 
			Me.radSpinEditorHeaderRowHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorHeaderRowHeight.Location = New System.Drawing.Point(10, 58)
			Me.radSpinEditorHeaderRowHeight.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
			Me.radSpinEditorHeaderRowHeight.Minimum = New Decimal(New Integer() {20, 0, 0, 0})
			Me.radSpinEditorHeaderRowHeight.Name = "radSpinEditorHeaderRowHeight"
			Me.radSpinEditorHeaderRowHeight.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorHeaderRowHeight.TabIndex = 2
			Me.radSpinEditorHeaderRowHeight.TabStop = False
			Me.radSpinEditorHeaderRowHeight.Value = New Decimal(New Integer() {30, 0, 0, 0})
			' 
			' radLabel2
			' 
			Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel2.Location = New System.Drawing.Point(10, 84)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New System.Drawing.Size(85, 18)
			Me.radLabel2.TabIndex = 1
			Me.radLabel2.Text = "New row height"
			' 
			' radSpinEditorNewRowHeight
			' 
			Me.radSpinEditorNewRowHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorNewRowHeight.Location = New System.Drawing.Point(10, 109)
			Me.radSpinEditorNewRowHeight.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
			Me.radSpinEditorNewRowHeight.Minimum = New Decimal(New Integer() {20, 0, 0, 0})
			Me.radSpinEditorNewRowHeight.Name = "radSpinEditorNewRowHeight"
			Me.radSpinEditorNewRowHeight.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorNewRowHeight.TabIndex = 2
			Me.radSpinEditorNewRowHeight.TabStop = False
			Me.radSpinEditorNewRowHeight.Value = New Decimal(New Integer() {24, 0, 0, 0})
			' 
			' radLabel3
			' 
			Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel3.Location = New System.Drawing.Point(10, 186)
			Me.radLabel3.Name = "radLabel3"
			Me.radLabel3.Size = New System.Drawing.Size(62, 18)
			Me.radLabel3.TabIndex = 1
			Me.radLabel3.Text = "Row height"
			' 
			' radSpinEditorDataRowHeight
			' 
			Me.radSpinEditorDataRowHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorDataRowHeight.Location = New System.Drawing.Point(10, 211)
			Me.radSpinEditorDataRowHeight.Minimum = New Decimal(New Integer() {15, 0, 0, 0})
			Me.radSpinEditorDataRowHeight.Name = "radSpinEditorDataRowHeight"
			Me.radSpinEditorDataRowHeight.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorDataRowHeight.TabIndex = 2
			Me.radSpinEditorDataRowHeight.TabStop = False
			Me.radSpinEditorDataRowHeight.Value = New Decimal(New Integer() {24, 0, 0, 0})
			' 
			' radLabel4
			' 
			Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel4.Location = New System.Drawing.Point(10, 237)
			Me.radLabel4.Name = "radLabel4"
			Me.radLabel4.Size = New System.Drawing.Size(75, 18)
			Me.radLabel4.TabIndex = 1
			Me.radLabel4.Text = "Column width"
			' 
			' radSpinEditorColumnWidth
			' 
			Me.radSpinEditorColumnWidth.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorColumnWidth.Increment = New Decimal(New Integer() {5, 0, 0, 0})
			Me.radSpinEditorColumnWidth.Location = New System.Drawing.Point(10, 262)
			Me.radSpinEditorColumnWidth.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
			Me.radSpinEditorColumnWidth.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
			Me.radSpinEditorColumnWidth.Name = "radSpinEditorColumnWidth"
			Me.radSpinEditorColumnWidth.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorColumnWidth.TabIndex = 2
			Me.radSpinEditorColumnWidth.TabStop = False
			Me.radSpinEditorColumnWidth.Value = New Decimal(New Integer() {100, 0, 0, 0})
			' 
			' radLabel5
			' 
			Me.radLabel5.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel5.Location = New System.Drawing.Point(10, 288)
			Me.radLabel5.Name = "radLabel5"
			Me.radLabel5.Size = New System.Drawing.Size(65, 18)
			Me.radLabel5.TabIndex = 1
			Me.radLabel5.Text = "Cell spacing"
			' 
			' radSpinEditorCellSpacing
			' 
			Me.radSpinEditorCellSpacing.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorCellSpacing.Location = New System.Drawing.Point(10, 313)
			Me.radSpinEditorCellSpacing.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
			Me.radSpinEditorCellSpacing.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
			Me.radSpinEditorCellSpacing.Name = "radSpinEditorCellSpacing"
			Me.radSpinEditorCellSpacing.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorCellSpacing.TabIndex = 2
			Me.radSpinEditorCellSpacing.TabStop = False
			Me.radSpinEditorCellSpacing.Value = New Decimal(New Integer() {1, 0, 0, -2147483648})
			' 
			' radLabel6
			' 
			Me.radLabel6.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel6.Location = New System.Drawing.Point(10, 339)
			Me.radLabel6.Name = "radLabel6"
			Me.radLabel6.Size = New System.Drawing.Size(68, 18)
			Me.radLabel6.TabIndex = 1
			Me.radLabel6.Text = "Row spacing"
			' 
			' radSpinEditorRowSpacing
			' 
			Me.radSpinEditorRowSpacing.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorRowSpacing.Location = New System.Drawing.Point(10, 364)
			Me.radSpinEditorRowSpacing.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
			Me.radSpinEditorRowSpacing.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
			Me.radSpinEditorRowSpacing.Name = "radSpinEditorRowSpacing"
			Me.radSpinEditorRowSpacing.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorRowSpacing.TabIndex = 2
			Me.radSpinEditorRowSpacing.TabStop = False
			Me.radSpinEditorRowSpacing.Value = New Decimal(New Integer() {1, 0, 0, -2147483648})
			' 
			' radCheckBoxShowHeaderRow
			' 
			Me.radCheckBoxShowHeaderRow.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxShowHeaderRow.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBoxShowHeaderRow.Location = New System.Drawing.Point(10, 402)
			Me.radCheckBoxShowHeaderRow.Name = "radCheckBoxShowHeaderRow"
			Me.radCheckBoxShowHeaderRow.Size = New System.Drawing.Size(138, 18)
			Me.radCheckBoxShowHeaderRow.TabIndex = 3
			Me.radCheckBoxShowHeaderRow.Text = "Should column headers"
			Me.radCheckBoxShowHeaderRow.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
			' 
			' radCheckBoxShowNewRow
			' 
			Me.radCheckBoxShowNewRow.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxShowNewRow.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBoxShowNewRow.Location = New System.Drawing.Point(10, 426)
			Me.radCheckBoxShowNewRow.Name = "radCheckBoxShowNewRow"
			Me.radCheckBoxShowNewRow.Size = New System.Drawing.Size(93, 18)
			Me.radCheckBoxShowNewRow.TabIndex = 3
			Me.radCheckBoxShowNewRow.Text = "Show new row"
			Me.radCheckBoxShowNewRow.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
			' 
			' radCheckBoxShowFilterRow
			' 
			Me.radCheckBoxShowFilterRow.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxShowFilterRow.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBoxShowFilterRow.Location = New System.Drawing.Point(10, 450)
			Me.radCheckBoxShowFilterRow.Name = "radCheckBoxShowFilterRow"
			Me.radCheckBoxShowFilterRow.Size = New System.Drawing.Size(95, 18)
			Me.radCheckBoxShowFilterRow.TabIndex = 3
			Me.radCheckBoxShowFilterRow.Text = "Show filter row"
			Me.radCheckBoxShowFilterRow.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
			' 
			' radCheckBoxFitColumns
			' 
			Me.radCheckBoxFitColumns.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxFitColumns.Location = New System.Drawing.Point(10, 474)
			Me.radCheckBoxFitColumns.Name = "radCheckBoxFitColumns"
			Me.radCheckBoxFitColumns.Size = New System.Drawing.Size(77, 18)
			Me.radCheckBoxFitColumns.TabIndex = 3
			Me.radCheckBoxFitColumns.Text = "Fit columns"
			' 
			' radCheckBoxAlternatingRowColors
			' 
			Me.radCheckBoxAlternatingRowColors.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxAlternatingRowColors.Location = New System.Drawing.Point(10, 498)
			Me.radCheckBoxAlternatingRowColors.Name = "radCheckBoxAlternatingRowColors"
			Me.radCheckBoxAlternatingRowColors.Size = New System.Drawing.Size(131, 18)
			Me.radCheckBoxAlternatingRowColors.TabIndex = 3
			Me.radCheckBoxAlternatingRowColors.Text = "Alternating row colors"
			' 
			' radCheckBoxHotTracking
			' 
			Me.radCheckBoxHotTracking.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxHotTracking.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBoxHotTracking.Location = New System.Drawing.Point(10, 522)
			Me.radCheckBoxHotTracking.Name = "radCheckBoxHotTracking"
			Me.radCheckBoxHotTracking.Size = New System.Drawing.Size(117, 18)
			Me.radCheckBoxHotTracking.TabIndex = 3
			Me.radCheckBoxHotTracking.Text = "Enable hot tracking"
			Me.radCheckBoxHotTracking.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
			' 
			' radLabel7
			' 
			Me.radLabel7.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel7.Location = New System.Drawing.Point(10, 135)
			Me.radLabel7.Name = "radLabel7"
			Me.radLabel7.Size = New System.Drawing.Size(87, 18)
			Me.radLabel7.TabIndex = 1
			Me.radLabel7.Text = "Filter row height"
			' 
			' radSpinEditorFilterRowHeight
			' 
			Me.radSpinEditorFilterRowHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorFilterRowHeight.Location = New System.Drawing.Point(10, 160)
			Me.radSpinEditorFilterRowHeight.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
			Me.radSpinEditorFilterRowHeight.Minimum = New Decimal(New Integer() {20, 0, 0, 0})
			Me.radSpinEditorFilterRowHeight.Name = "radSpinEditorFilterRowHeight"
			Me.radSpinEditorFilterRowHeight.Size = New System.Drawing.Size(210, 20)
			Me.radSpinEditorFilterRowHeight.TabIndex = 2
			Me.radSpinEditorFilterRowHeight.TabStop = False
			Me.radSpinEditorFilterRowHeight.Value = New Decimal(New Integer() {26, 0, 0, 0})
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.radVirtualGrid1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1529, 1013)
			Me.Controls.SetChildIndex(Me.radVirtualGrid1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radVirtualGrid1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorHeaderRowHeight, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorNewRowHeight, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorDataRowHeight, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorColumnWidth, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorCellSpacing, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorRowSpacing, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radCheckBoxShowHeaderRow, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radCheckBoxShowNewRow, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radCheckBoxShowFilterRow, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radCheckBoxFitColumns, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radCheckBoxAlternatingRowColors, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radCheckBoxHotTracking, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorFilterRowHeight, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radVirtualGrid1 As Telerik.WinControls.UI.RadVirtualGrid
		Private northwindDataSet As DataSources.NorthwindDataSet
		Private employeesTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter
		Private radCheckBoxShowFilterRow As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxShowNewRow As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxShowHeaderRow As Telerik.WinControls.UI.RadCheckBox
		Private radSpinEditorRowSpacing As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorCellSpacing As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorColumnWidth As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorDataRowHeight As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorNewRowHeight As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorHeaderRowHeight As Telerik.WinControls.UI.RadSpinEditor
		Private radLabel6 As Telerik.WinControls.UI.RadLabel
		Private radLabel5 As Telerik.WinControls.UI.RadLabel
		Private radLabel4 As Telerik.WinControls.UI.RadLabel
		Private radLabel3 As Telerik.WinControls.UI.RadLabel
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radCheckBoxHotTracking As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxAlternatingRowColors As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxFitColumns As Telerik.WinControls.UI.RadCheckBox
		Private radSpinEditorFilterRowHeight As Telerik.WinControls.UI.RadSpinEditor
		Private radLabel7 As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace