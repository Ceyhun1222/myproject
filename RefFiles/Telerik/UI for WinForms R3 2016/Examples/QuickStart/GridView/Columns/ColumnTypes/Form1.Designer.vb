Namespace Telerik.Examples.WinControls.GridView.Columns.ColumnTypes
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
            Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
            Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
            Me.nwindDataSet = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
            Me.carsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.carsTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CarsTableAdapter()
            Me.employeesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.employeesTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter()
            Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.radGroupBoxColumns = New Telerik.WinControls.UI.RadGroupBox()
            Me.radCheckBoxCustom = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxCheckBox = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxBrowse = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxCalculator = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxColor = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxHyperlink = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxMaskBox = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxLookUp = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxImage = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxText = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxDecimal = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxRating = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCommandCheckBox = New Telerik.WinControls.UI.RadCheckBox()
            Me.radMultiComboBoxCheckBox = New Telerik.WinControls.UI.RadCheckBox()
            Me.radDateTimeCheckBox = New Telerik.WinControls.UI.RadCheckBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.carsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.employeesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupBoxColumns, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxColumns.SuspendLayout()
            CType(Me.radCheckBoxCustom, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxBrowse, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxCalculator, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxColor, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxHyperlink, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxMaskBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxLookUp, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxImage, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxDecimal, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxRating, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCommandCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radMultiComboBoxCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimeCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radGroupBoxColumns)
            Me.settingsPanel.Location = New System.Drawing.Point(973, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(303, 747)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxColumns, 0)
            '
            'radGridView1
            '
            Me.radGridView1.BackColor = System.Drawing.Color.White
            Me.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radGridView1.EnableHotTracking = False
            Me.radGridView1.ForeColor = System.Drawing.Color.Black
            Me.radGridView1.Location = New System.Drawing.Point(0, 0)
            '
            '
            '
            Me.radGridView1.MasterTemplate.AllowAddNewRow = False
            Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
            Me.radGridView1.MasterTemplate.Caption = Nothing
            Me.radGridView1.MasterTemplate.ShowGroupedColumns = True
            Me.radGridView1.MasterTemplate.ViewDefinition = TableViewDefinition2
            Me.radGridView1.Name = "radGridView1"
            Me.radGridView1.Size = New System.Drawing.Size(1190, 947)
            Me.radGridView1.TabIndex = 0
            Me.radGridView1.Text = "radGridView1"
            Me.radGridView1.ThemeName = "Telerik"
            '
            'nwindDataSet
            '
            Me.nwindDataSet.DataSetName = "NwindDataSet"
            Me.nwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'carsBindingSource
            '
            Me.carsBindingSource.DataMember = "Cars"
            Me.carsBindingSource.DataSource = Me.nwindDataSet
            '
            'carsTableAdapter
            '
            Me.carsTableAdapter.ClearBeforeFill = True
            '
            'employeesBindingSource
            '
            Me.employeesBindingSource.DataMember = "Employees"
            Me.employeesBindingSource.DataSource = Me.nwindDataSet
            '
            'employeesTableAdapter
            '
            Me.employeesTableAdapter.ClearBeforeFill = True
            '
            'radGroupBoxColumns
            '
            Me.radGroupBoxColumns.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxColumns.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxColumns.Controls.Add(Me.radDateTimeCheckBox)
            Me.radGroupBoxColumns.Controls.Add(Me.radMultiComboBoxCheckBox)
            Me.radGroupBoxColumns.Controls.Add(Me.radCommandCheckBox)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxCustom)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxCheckBox)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxBrowse)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxCalculator)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxColor)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxHyperlink)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxMaskBox)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxLookUp)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxImage)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxText)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxDecimal)
            Me.radGroupBoxColumns.Controls.Add(Me.radCheckBoxRating)
            Me.radGroupBoxColumns.HeaderMargin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            Me.radGroupBoxColumns.HeaderText = "Columns"
            Me.radGroupBoxColumns.Location = New System.Drawing.Point(10, 6)
            Me.radGroupBoxColumns.Name = "radGroupBoxColumns"
            Me.radGroupBoxColumns.Size = New System.Drawing.Size(283, 391)
            Me.radGroupBoxColumns.TabIndex = 1
            Me.radGroupBoxColumns.Text = "Columns"
            '
            'radCheckBoxCustom
            '
            Me.radCheckBoxCustom.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxCustom.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxCustom.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxCustom.Location = New System.Drawing.Point(5, 291)
            Me.radCheckBoxCustom.Name = "radCheckBoxCustom"
            '
            '
            '
            Me.radCheckBoxCustom.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxCustom.Size = New System.Drawing.Size(59, 18)
            Me.radCheckBoxCustom.TabIndex = 34
            Me.radCheckBoxCustom.Text = "Custom"
            Me.radCheckBoxCustom.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxCustom.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxCheckBox
            '
            Me.radCheckBoxCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxCheckBox.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxCheckBox.Location = New System.Drawing.Point(5, 243)
            Me.radCheckBoxCheckBox.Name = "radCheckBoxCheckBox"
            '
            '
            '
            Me.radCheckBoxCheckBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxCheckBox.Size = New System.Drawing.Size(69, 18)
            Me.radCheckBoxCheckBox.TabIndex = 33
            Me.radCheckBoxCheckBox.Text = "CheckBox"
            Me.radCheckBoxCheckBox.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxCheckBox.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxBrowse
            '
            Me.radCheckBoxBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxBrowse.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxBrowse.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxBrowse.Location = New System.Drawing.Point(5, 219)
            Me.radCheckBoxBrowse.Name = "radCheckBoxBrowse"
            '
            '
            '
            Me.radCheckBoxBrowse.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxBrowse.Size = New System.Drawing.Size(56, 18)
            Me.radCheckBoxBrowse.TabIndex = 32
            Me.radCheckBoxBrowse.Text = "Browse"
            Me.radCheckBoxBrowse.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxBrowse.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxCalculator
            '
            Me.radCheckBoxCalculator.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxCalculator.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxCalculator.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxCalculator.Location = New System.Drawing.Point(5, 195)
            Me.radCheckBoxCalculator.Name = "radCheckBoxCalculator"
            '
            '
            '
            Me.radCheckBoxCalculator.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxCalculator.Size = New System.Drawing.Size(70, 18)
            Me.radCheckBoxCalculator.TabIndex = 31
            Me.radCheckBoxCalculator.Text = "Calculator"
            Me.radCheckBoxCalculator.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxCalculator.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxColor
            '
            Me.radCheckBoxColor.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxColor.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxColor.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxColor.Location = New System.Drawing.Point(5, 171)
            Me.radCheckBoxColor.Name = "radCheckBoxColor"
            '
            '
            '
            Me.radCheckBoxColor.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxColor.Size = New System.Drawing.Size(47, 18)
            Me.radCheckBoxColor.TabIndex = 28
            Me.radCheckBoxColor.Text = "Color"
            Me.radCheckBoxColor.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxColor.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxHyperlink
            '
            Me.radCheckBoxHyperlink.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxHyperlink.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxHyperlink.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxHyperlink.Location = New System.Drawing.Point(5, 147)
            Me.radCheckBoxHyperlink.Name = "radCheckBoxHyperlink"
            '
            '
            '
            Me.radCheckBoxHyperlink.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxHyperlink.Size = New System.Drawing.Size(68, 18)
            Me.radCheckBoxHyperlink.TabIndex = 28
            Me.radCheckBoxHyperlink.Text = "Hyperlink"
            Me.radCheckBoxHyperlink.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxHyperlink.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxMaskBox
            '
            Me.radCheckBoxMaskBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxMaskBox.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxMaskBox.Location = New System.Drawing.Point(5, 123)
            Me.radCheckBoxMaskBox.Name = "radCheckBoxMaskBox"
            '
            '
            '
            Me.radCheckBoxMaskBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxMaskBox.Size = New System.Drawing.Size(65, 18)
            Me.radCheckBoxMaskBox.TabIndex = 28
            Me.radCheckBoxMaskBox.Text = "MaskBox"
            Me.radCheckBoxMaskBox.ThemeName = "GridFeaturesBrowser"
            '
            'radCheckBoxLookUp
            '
            Me.radCheckBoxLookUp.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxLookUp.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxLookUp.Location = New System.Drawing.Point(5, 99)
            Me.radCheckBoxLookUp.Name = "radCheckBoxLookUp"
            '
            '
            '
            Me.radCheckBoxLookUp.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxLookUp.Size = New System.Drawing.Size(59, 18)
            Me.radCheckBoxLookUp.TabIndex = 30
            Me.radCheckBoxLookUp.Text = "LookUp"
            Me.radCheckBoxLookUp.ThemeName = "GridFeaturesBrowser"
            '
            'radCheckBoxImage
            '
            Me.radCheckBoxImage.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxImage.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxImage.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxImage.Location = New System.Drawing.Point(5, 75)
            Me.radCheckBoxImage.Name = "radCheckBoxImage"
            '
            '
            '
            Me.radCheckBoxImage.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxImage.Size = New System.Drawing.Size(51, 18)
            Me.radCheckBoxImage.TabIndex = 29
            Me.radCheckBoxImage.Text = "Image"
            Me.radCheckBoxImage.ThemeName = "GridFeaturesBrowser"
            Me.radCheckBoxImage.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxText
            '
            Me.radCheckBoxText.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxText.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxText.Location = New System.Drawing.Point(5, 51)
            Me.radCheckBoxText.Name = "radCheckBoxText"
            '
            '
            '
            Me.radCheckBoxText.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxText.Size = New System.Drawing.Size(41, 18)
            Me.radCheckBoxText.TabIndex = 28
            Me.radCheckBoxText.Text = "Text"
            Me.radCheckBoxText.ThemeName = "GridFeaturesBrowser"
            '
            'radCheckBoxDecimal
            '
            Me.radCheckBoxDecimal.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxDecimal.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxDecimal.Location = New System.Drawing.Point(5, 27)
            Me.radCheckBoxDecimal.Name = "radCheckBoxDecimal"
            '
            '
            '
            Me.radCheckBoxDecimal.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxDecimal.Size = New System.Drawing.Size(60, 18)
            Me.radCheckBoxDecimal.TabIndex = 27
            Me.radCheckBoxDecimal.Text = "Decimal"
            Me.radCheckBoxDecimal.ThemeName = "GridFeaturesBrowser"
            '
            'radCheckBoxRating
            '
            Me.radCheckBoxRating.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxRating.BackColor = System.Drawing.Color.Transparent
            Me.radCheckBoxRating.Location = New System.Drawing.Point(5, 267)
            Me.radCheckBoxRating.Name = "radCheckBoxRating"
            '
            '
            '
            Me.radCheckBoxRating.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCheckBoxRating.Size = New System.Drawing.Size(52, 18)
            Me.radCheckBoxRating.TabIndex = 28
            Me.radCheckBoxRating.Text = "Rating"
            Me.radCheckBoxRating.ThemeName = "GridFeaturesBrowser"
            '
            'radCommandCheckBox
            '
            Me.radCommandCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCommandCheckBox.BackColor = System.Drawing.Color.Transparent
            Me.radCommandCheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer))
            Me.radCommandCheckBox.Location = New System.Drawing.Point(5, 315)
            Me.radCommandCheckBox.Name = "radCommandCheckBox"
            '
            '
            '
            Me.radCommandCheckBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radCommandCheckBox.Size = New System.Drawing.Size(72, 18)
            Me.radCommandCheckBox.TabIndex = 35
            Me.radCommandCheckBox.Text = "Command"
            Me.radCommandCheckBox.ThemeName = "GridFeaturesBrowser"
            '
            'radMultiComboBoxCheckBox
            '
            Me.radMultiComboBoxCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radMultiComboBoxCheckBox.BackColor = System.Drawing.Color.Transparent
            Me.radMultiComboBoxCheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer))
            Me.radMultiComboBoxCheckBox.Location = New System.Drawing.Point(5, 339)
            Me.radMultiComboBoxCheckBox.Name = "radMultiComboBoxCheckBox"
            '
            '
            '
            Me.radMultiComboBoxCheckBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radMultiComboBoxCheckBox.Size = New System.Drawing.Size(101, 18)
            Me.radMultiComboBoxCheckBox.TabIndex = 36
            Me.radMultiComboBoxCheckBox.Text = "MultiComboBox"
            Me.radMultiComboBoxCheckBox.ThemeName = "GridFeaturesBrowser"
            '
            'radDateTimeCheckBox
            '
            Me.radDateTimeCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDateTimeCheckBox.BackColor = System.Drawing.Color.Transparent
            Me.radDateTimeCheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer))
            Me.radDateTimeCheckBox.Location = New System.Drawing.Point(5, 363)
            Me.radDateTimeCheckBox.Name = "radDateTimeCheckBox"
            '
            '
            '
            Me.radDateTimeCheckBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radDateTimeCheckBox.Size = New System.Drawing.Size(68, 18)
            Me.radDateTimeCheckBox.TabIndex = 37
            Me.radDateTimeCheckBox.Text = "DateTime"
            Me.radDateTimeCheckBox.ThemeName = "GridFeaturesBrowser"
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.Controls.Add(Me.radGridView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1200, 957)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radGridView1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.carsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.employeesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGroupBoxColumns, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxColumns.ResumeLayout(False)
            Me.radGroupBoxColumns.PerformLayout()
            CType(Me.radCheckBoxCustom, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxBrowse, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxCalculator, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxColor, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxHyperlink, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxMaskBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxLookUp, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxImage, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxDecimal, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxRating, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCommandCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radMultiComboBoxCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimeCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radGridView1 As Telerik.WinControls.UI.RadGridView
        Private nwindDataSet As Telerik.Examples.WinControls.DataSources.NorthwindDataSet
        Private carsBindingSource As System.Windows.Forms.BindingSource
        Private carsTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CarsTableAdapter
        Private employeesBindingSource As System.Windows.Forms.BindingSource
        Private employeesTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter
        Private toolTip1 As System.Windows.Forms.ToolTip
        Private radGroupBoxColumns As Telerik.WinControls.UI.RadGroupBox
        Private radCheckBoxDecimal As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxCustom As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxCheckBox As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxBrowse As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxCalculator As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxColor As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxHyperlink As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxMaskBox As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxLookUp As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxImage As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxText As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxRating As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radDateTimeCheckBox As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radMultiComboBoxCheckBox As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents radCommandCheckBox As Telerik.WinControls.UI.RadCheckBox


    End Class
End Namespace