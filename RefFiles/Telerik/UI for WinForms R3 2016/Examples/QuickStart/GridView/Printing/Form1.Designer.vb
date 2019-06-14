Namespace Telerik.Examples.WinControls.GridView.Printing
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
			Dim gridViewDecimalColumn1 As New Telerik.WinControls.UI.GridViewDecimalColumn()
			Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn2 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn3 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn4 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewDateTimeColumn1 As New Telerik.WinControls.UI.GridViewDateTimeColumn()
			Dim gridViewDateTimeColumn2 As New Telerik.WinControls.UI.GridViewDateTimeColumn()
			Dim gridViewTextBoxColumn5 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewImageColumn1 As New Telerik.WinControls.UI.GridViewImageColumn()
			Dim gridViewTextBoxColumn6 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn7 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn8 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn9 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn10 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn11 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn12 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewDecimalColumn2 As New Telerik.WinControls.UI.GridViewDecimalColumn()
			Dim radPrintWatermark1 As New Telerik.WinControls.UI.RadPrintWatermark()
			Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
			Me.employeesBindingSource = New BindingSource(Me.components)
			Me.northwindDataSet = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
			Me.radRadioButtonTable = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioButtonHtml = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioButtonColumnGroups = New Telerik.WinControls.UI.RadRadioButton()
			Me.radButtonPrint = New Telerik.WinControls.UI.RadButton()
			Me.radButtonPrintPreview = New Telerik.WinControls.UI.RadButton()
			Me.radButtonPrintSettings = New Telerik.WinControls.UI.RadButton()
			Me.radPrintDocument1 = New Telerik.WinControls.UI.RadPrintDocument()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.employeesTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.employeesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioButtonTable, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioButtonHtml, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioButtonColumnGroups, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonPrint, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonPrintPreview, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonPrintSettings, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Controls.Add(Me.radButtonPrint)
			Me.settingsPanel.Controls.Add(Me.radButtonPrintPreview)
			Me.settingsPanel.Controls.Add(Me.radButtonPrintSettings)
			Me.settingsPanel.ForeColor = SystemColors.ControlText
			Me.settingsPanel.Location = New Point(381, 90)
			Me.settingsPanel.Padding = New Padding(0, 0, 20, 0)
			' 
			' 
			' 
			Me.settingsPanel.RootElement.Padding = New Padding(0, 0, 20, 0)
			Me.settingsPanel.Size = New Size(230, 293)
			Me.settingsPanel.Controls.SetChildIndex(Me.radButtonPrintSettings, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radButtonPrintPreview, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radButtonPrint, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			' 
			' radGridView1
			' 
			Me.radGridView1.Dock = DockStyle.Fill
			Me.radGridView1.Location = New Point(0, 0)
			' 
			' radGridView1
			' 
			Me.radGridView1.MasterTemplate.AllowAddNewRow = False
			Me.radGridView1.MasterTemplate.AutoExpandGroups = True
			gridViewDecimalColumn1.DataType = GetType(Integer)
			gridViewDecimalColumn1.FieldName = "EmployeeID"
			gridViewDecimalColumn1.HeaderText = "EmployeeID"
			gridViewDecimalColumn1.IsAutoGenerated = True
			gridViewDecimalColumn1.IsVisible = False
			gridViewDecimalColumn1.Name = "EmployeeID"
			gridViewTextBoxColumn1.FieldName = "TitleOfCourtesy"
			gridViewTextBoxColumn1.HeaderText = "TitleOfCourtesy"
			gridViewTextBoxColumn1.IsAutoGenerated = True
			gridViewTextBoxColumn1.Name = "TitleOfCourtesy"
			gridViewTextBoxColumn2.FieldName = "FirstName"
			gridViewTextBoxColumn2.HeaderText = "FirstName"
			gridViewTextBoxColumn2.IsAutoGenerated = True
			gridViewTextBoxColumn2.Name = "FirstName"
			gridViewTextBoxColumn2.Width = 100
			gridViewTextBoxColumn3.FieldName = "LastName"
			gridViewTextBoxColumn3.HeaderText = "LastName"
			gridViewTextBoxColumn3.IsAutoGenerated = True
			gridViewTextBoxColumn3.Name = "LastName"
			gridViewTextBoxColumn3.Width = 100
			gridViewTextBoxColumn4.FieldName = "Title"
			gridViewTextBoxColumn4.HeaderText = "Title"
			gridViewTextBoxColumn4.IsAutoGenerated = True
			gridViewTextBoxColumn4.Name = "Title"
			gridViewTextBoxColumn4.Width = 80
			gridViewDateTimeColumn1.FieldName = "BirthDate"
			gridViewDateTimeColumn1.HeaderText = "BirthDate"
			gridViewDateTimeColumn1.IsAutoGenerated = True
			gridViewDateTimeColumn1.IsVisible = False
			gridViewDateTimeColumn1.Name = "BirthDate"
			gridViewDateTimeColumn2.FieldName = "HireDate"
			gridViewDateTimeColumn2.HeaderText = "HireDate"
			gridViewDateTimeColumn2.IsAutoGenerated = True
			gridViewDateTimeColumn2.IsVisible = False
			gridViewDateTimeColumn2.Name = "HireDate"
			gridViewTextBoxColumn5.FieldName = "Country"
			gridViewTextBoxColumn5.HeaderText = "Country"
			gridViewTextBoxColumn5.IsAutoGenerated = True
			gridViewTextBoxColumn5.Name = "Country"
			gridViewImageColumn1.DataType = GetType(Byte())
			gridViewImageColumn1.FieldName = "Photo"
			gridViewImageColumn1.HeaderText = "Photo"
			gridViewImageColumn1.IsAutoGenerated = True
			gridViewImageColumn1.IsVisible = False
			gridViewImageColumn1.Name = "Photo"
			gridViewTextBoxColumn6.FieldName = "Address"
			gridViewTextBoxColumn6.HeaderText = "Address"
			gridViewTextBoxColumn6.IsAutoGenerated = True
			gridViewTextBoxColumn6.Name = "Address"
			gridViewTextBoxColumn6.Width = 150
			gridViewTextBoxColumn7.FieldName = "City"
			gridViewTextBoxColumn7.HeaderText = "City"
			gridViewTextBoxColumn7.IsAutoGenerated = True
			gridViewTextBoxColumn7.IsVisible = False
			gridViewTextBoxColumn7.Name = "City"
			gridViewTextBoxColumn8.FieldName = "Region"
			gridViewTextBoxColumn8.HeaderText = "Region"
			gridViewTextBoxColumn8.IsAutoGenerated = True
			gridViewTextBoxColumn8.IsVisible = False
			gridViewTextBoxColumn8.Name = "Region"
			gridViewTextBoxColumn9.FieldName = "PostalCode"
			gridViewTextBoxColumn9.HeaderText = "PostalCode"
			gridViewTextBoxColumn9.IsAutoGenerated = True
			gridViewTextBoxColumn9.IsVisible = False
			gridViewTextBoxColumn9.Name = "PostalCode"
			gridViewTextBoxColumn10.FieldName = "HomePhone"
			gridViewTextBoxColumn10.HeaderText = "Phone"
			gridViewTextBoxColumn10.IsAutoGenerated = True
			gridViewTextBoxColumn10.Name = "HomePhone"
			gridViewTextBoxColumn10.Width = 80
			gridViewTextBoxColumn11.FieldName = "Extension"
			gridViewTextBoxColumn11.HeaderText = "Extension"
			gridViewTextBoxColumn11.IsAutoGenerated = True
			gridViewTextBoxColumn11.IsVisible = False
			gridViewTextBoxColumn11.Name = "Extension"
			gridViewTextBoxColumn12.FieldName = "Notes"
			gridViewTextBoxColumn12.HeaderText = "Notes"
			gridViewTextBoxColumn12.IsAutoGenerated = True
			gridViewTextBoxColumn12.IsVisible = False
			gridViewTextBoxColumn12.Name = "Notes"
			gridViewDecimalColumn2.DataType = GetType(Integer)
			gridViewDecimalColumn2.FieldName = "ReportsTo"
			gridViewDecimalColumn2.HeaderText = "ReportsTo"
			gridViewDecimalColumn2.IsAutoGenerated = True
			gridViewDecimalColumn2.IsVisible = False
			gridViewDecimalColumn2.Name = "ReportsTo"
			Me.radGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() { gridViewDecimalColumn1, gridViewTextBoxColumn1, gridViewTextBoxColumn2, gridViewTextBoxColumn3, gridViewTextBoxColumn4, gridViewDateTimeColumn1, gridViewDateTimeColumn2, gridViewTextBoxColumn5, gridViewImageColumn1, gridViewTextBoxColumn6, gridViewTextBoxColumn7, gridViewTextBoxColumn8, gridViewTextBoxColumn9, gridViewTextBoxColumn10, gridViewTextBoxColumn11, gridViewTextBoxColumn12, gridViewDecimalColumn2})
			Me.radGridView1.MasterTemplate.DataSource = Me.employeesBindingSource
			Me.radGridView1.Name = "radGridView1"
			Me.radGridView1.Size = New Size(1160, 519)
			Me.radGridView1.TabIndex = 0
			Me.radGridView1.Text = "radGridView1"
			' 
			' employeesBindingSource
			' 
			Me.employeesBindingSource.DataMember = "Employees"
			Me.employeesBindingSource.DataSource = Me.northwindDataSet
			' 
			' northwindDataSet
			' 
			Me.northwindDataSet.DataSetName = "NorthwindDataSet"
			Me.northwindDataSet.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			' 
			' radRadioButtonTable
			' 
			Me.radRadioButtonTable.Location = New Point(5, 21)
			Me.radRadioButtonTable.Name = "radRadioButtonTable"
			Me.radRadioButtonTable.Size = New Size(47, 18)
			Me.radRadioButtonTable.TabIndex = 1
			Me.radRadioButtonTable.Text = "Table"

			' 
			' radRadioButtonHtml
			' 
			Me.radRadioButtonHtml.Location = New Point(5, 45)
			Me.radRadioButtonHtml.Name = "radRadioButtonHtml"
			Me.radRadioButtonHtml.Size = New Size(50, 18)
			Me.radRadioButtonHtml.TabIndex = 1
			Me.radRadioButtonHtml.TabStop = True
			Me.radRadioButtonHtml.Text = "HTML"

			' 
			' radRadioButtonColumnGroups
			' 
			Me.radRadioButtonColumnGroups.Location = New Point(5, 69)
			Me.radRadioButtonColumnGroups.Name = "radRadioButtonColumnGroups"
			Me.radRadioButtonColumnGroups.Size = New Size(97, 18)
			Me.radRadioButtonColumnGroups.TabIndex = 1
			Me.radRadioButtonColumnGroups.TabStop = True
			Me.radRadioButtonColumnGroups.Text = "Column groups"
			Me.radRadioButtonColumnGroups.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On

			' 
			' radButtonPrint
			' 
			Me.radButtonPrint.Location = New Point(10, 134)
			Me.radButtonPrint.Name = "radButtonPrint"
			Me.radButtonPrint.Size = New Size(210, 24)
			Me.radButtonPrint.TabIndex = 2
			Me.radButtonPrint.Text = "Print"

			' 
			' radButtonPrintPreview
			' 
			Me.radButtonPrintPreview.Location = New Point(10, 164)
			Me.radButtonPrintPreview.Name = "radButtonPrintPreview"
			Me.radButtonPrintPreview.Size = New Size(210, 24)
			Me.radButtonPrintPreview.TabIndex = 2
			Me.radButtonPrintPreview.Text = "Print preview"

			' 
			' radButtonPrintSettings
			' 
			Me.radButtonPrintSettings.Location = New Point(10, 194)
			Me.radButtonPrintSettings.Name = "radButtonPrintSettings"
			Me.radButtonPrintSettings.Size = New Size(210, 24)
			Me.radButtonPrintSettings.TabIndex = 2
			Me.radButtonPrintSettings.Text = "Print settings"

			' 
			' radPrintDocument1
			' 
			Me.radPrintDocument1.AssociatedObject = Me.radGridView1
			Me.radPrintDocument1.FooterFont = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radPrintDocument1.HeaderFont = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			radPrintWatermark1.Pages = Nothing
			radPrintWatermark1.Text = Nothing
			Me.radPrintDocument1.Watermark = radPrintWatermark1
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupBox1.Controls.Add(Me.radRadioButtonTable)
			Me.radGroupBox1.Controls.Add(Me.radRadioButtonColumnGroups)
			Me.radGroupBox1.Controls.Add(Me.radRadioButtonHtml)
			Me.radGroupBox1.HeaderText = "Print style"
			Me.radGroupBox1.Location = New Point(10, 11)
			Me.radGroupBox1.Name = "radGroupBox1"
			' 
			' 
			' 
			Me.radGroupBox1.RootElement.Padding = New Padding(2, 18, 2, 2)
			Me.radGroupBox1.Size = New Size(210, 100)
			Me.radGroupBox1.TabIndex = 3
			Me.radGroupBox1.Text = "Print style"
			' 
			' employeesTableAdapter
			' 
			Me.employeesTableAdapter.ClearBeforeFill = True
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radGridView1)
			Me.Name = "Form1"
			Me.Size = New Size(1170, 529)
			Me.Controls.SetChildIndex(Me.radGridView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.employeesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioButtonTable, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioButtonHtml, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioButtonColumnGroups, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonPrint, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonPrintPreview, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonPrintSettings, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radGridView1 As Telerik.WinControls.UI.RadGridView
		Private radRadioButtonTable As Telerik.WinControls.UI.RadRadioButton
		Private radRadioButtonHtml As Telerik.WinControls.UI.RadRadioButton
		Private radRadioButtonColumnGroups As Telerik.WinControls.UI.RadRadioButton
		Private radButtonPrint As Telerik.WinControls.UI.RadButton
		Private radButtonPrintPreview As Telerik.WinControls.UI.RadButton
		Private radButtonPrintSettings As Telerik.WinControls.UI.RadButton
		Private radPrintDocument1 As Telerik.WinControls.UI.RadPrintDocument
		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private northwindDataSet As DataSources.NorthwindDataSet
		Private employeesBindingSource As BindingSource
		Private employeesTableAdapter As DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter
	End Class
End Namespace