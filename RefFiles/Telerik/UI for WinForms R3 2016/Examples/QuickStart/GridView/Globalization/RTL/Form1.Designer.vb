Namespace Telerik.Examples.WinControls.GridView.Globalization.RTL
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

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn2 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn3 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn4 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn5 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim groupDescriptor1 As New Telerik.WinControls.Data.GroupDescriptor()
			Dim sortDescriptor1 As New Telerik.WinControls.Data.SortDescriptor()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim themeSource1 As New Telerik.WinControls.ThemeSource()
			Me.customersBindingSource = New BindingSource(Me.components)
			Me.nwindRadGridView = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
			Me.customersTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CustomersTableAdapter()
			Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
			Me.imageList1 = New ImageList(Me.components)
			Me.radThemeManager1 = New Telerik.WinControls.RadThemeManager()
			Me.radToggleButton1 = New Telerik.WinControls.UI.RadToggleButton()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.customersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.nwindRadGridView, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radToggleButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radToggleButton1)
			Me.settingsPanel.ForeColor = Color.Black
			Me.settingsPanel.Location = New Point(906, 1)
			' 
			' 
			' 
			Me.settingsPanel.RootElement.ForeColor = Color.Black
			Me.settingsPanel.Size = New Size(200, 599)
			Me.settingsPanel.ThemeName = "ControlDefault"
			Me.settingsPanel.Controls.SetChildIndex(Me.radToggleButton1, 0)
			' 
			' customersBindingSource
			' 
			Me.customersBindingSource.DataMember = "Customers"
			Me.customersBindingSource.DataSource = Me.nwindRadGridView
			' 
			' nwindRadGridView
			' 
			Me.nwindRadGridView.DataSetName = "NwindRadGridView"
			Me.nwindRadGridView.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			' 
			' customersTableAdapter
			' 
			Me.customersTableAdapter.ClearBeforeFill = True
			' 
			' radGridView1
			' 
			Me.radGridView1.BackColor = Color.FromArgb((CInt(Fix((CByte(236))))), (CInt(Fix((CByte(251))))), (CInt(Fix((CByte(254))))))
			Me.radGridView1.Dock = DockStyle.Fill
			Me.radGridView1.ForeColor = Color.Black
			Me.radGridView1.Location = New Point(0, 0)
			' 
			' 
			' 
			Me.radGridView1.MasterTemplate.EnableFiltering = True
			Me.radGridView1.MasterTemplate.AutoExpandGroups = True
			Me.radGridView1.MasterTemplate.AllowAddNewRow = False
			Me.radGridView1.MasterTemplate.AutoGenerateColumns = False
			Me.radGridView1.MasterTemplate.ShowGroupedColumns = True
			Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
			Me.radGridView1.MasterTemplate.Caption = Nothing
			gridViewTextBoxColumn1.FieldName = "CustomerID"
			gridViewTextBoxColumn1.HeaderText = "CustomerID"
			gridViewTextBoxColumn1.HeaderTextAlignment = ContentAlignment.MiddleLeft
			gridViewTextBoxColumn1.Name = "CustomerID"
			gridViewTextBoxColumn1.Width = 206
			gridViewTextBoxColumn2.FieldName = "ContactName"
			gridViewTextBoxColumn2.HeaderText = "Contact Name"
			gridViewTextBoxColumn2.HeaderTextAlignment = ContentAlignment.MiddleLeft
			gridViewTextBoxColumn2.Name = "ContactName"
			gridViewTextBoxColumn2.Width = 246
			gridViewTextBoxColumn3.FieldName = "CompanyName"
			gridViewTextBoxColumn3.HeaderText = "Company Name"
			gridViewTextBoxColumn3.HeaderTextAlignment = ContentAlignment.MiddleLeft
			gridViewTextBoxColumn3.Name = "CompanyName"
			gridViewTextBoxColumn3.Width = 246
			gridViewTextBoxColumn4.FieldName = "Country"
			gridViewTextBoxColumn4.HeaderText = "Country"
			gridViewTextBoxColumn4.HeaderTextAlignment = ContentAlignment.MiddleLeft
			gridViewTextBoxColumn4.Name = "Country"
			gridViewTextBoxColumn4.Width = 206
			gridViewTextBoxColumn5.FieldName = "City"
			gridViewTextBoxColumn5.HeaderText = "City"
			gridViewTextBoxColumn5.HeaderTextAlignment = ContentAlignment.MiddleLeft
			gridViewTextBoxColumn5.Name = "City"
			gridViewTextBoxColumn5.Width = 161
			Me.radGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() { gridViewTextBoxColumn1, gridViewTextBoxColumn2, gridViewTextBoxColumn3, gridViewTextBoxColumn4, gridViewTextBoxColumn5})
			sortDescriptor1.PropertyName = "Country"
			groupDescriptor1.GroupNames.AddRange(New Telerik.WinControls.Data.SortDescriptor() { sortDescriptor1})
			Me.radGridView1.MasterTemplate.GroupDescriptors.AddRange(New Telerik.WinControls.Data.GroupDescriptor() { groupDescriptor1})
			Me.radGridView1.MasterTemplate.DataSource = customersBindingSource
			Me.radGridView1.Name = "radGridView1"
			Me.radGridView1.Padding = New Padding(0, 0, 0, 1)
			' 
			' 
			' 
			Me.radGridView1.RootElement.ForeColor = Color.Black
			Me.radGridView1.Size = New Size(1107, 601)
			Me.radGridView1.TabIndex = 0
			Me.radGridView1.Text = "radGridView1"
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Fuchsia
			Me.imageList1.Images.SetKeyName(0, "Views.PNG")
			Me.imageList1.Images.SetKeyName(1, "Organize.PNG")
			' 
			' radThemeManager1
			' 
			themeSource1.StorageType = Telerik.WinControls.ThemeStorageType.Resource
			themeSource1.ThemeLocation = "Telerik.Examples.WinControls.Resources.RadToolStripVista.xml"
			Me.radThemeManager1.LoadedThemes.AddRange(New Telerik.WinControls.ThemeSource() { themeSource1})
			' 
			' radToggleButton1
			' 
			Me.radToggleButton1.Anchor = AnchorStyles.Top
			Me.radToggleButton1.Location = New Point(10, 6)
			Me.radToggleButton1.Name = "radToggleButton1"
			Me.radToggleButton1.Size = New Size(180, 23)
			Me.radToggleButton1.TabIndex = 0
			Me.radToggleButton1.Text = "Use RTL"

			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.BackColor = Color.FromArgb((CInt(Fix((CByte(236))))), (CInt(Fix((CByte(251))))), (CInt(Fix((CByte(254))))))
			Me.Controls.Add(Me.radGridView1)
			Me.Name = "Form1"
			Me.Size = New Size(1107, 601)

			Me.Controls.SetChildIndex(Me.radGridView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			CType(Me.customersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.nwindRadGridView, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radToggleButton1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private customersBindingSource As BindingSource
		Private nwindRadGridView As Telerik.Examples.WinControls.DataSources.NorthwindDataSet
		Private customersTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CustomersTableAdapter
		Private radGridView1 As Telerik.WinControls.UI.RadGridView
		Private radThemeManager1 As Telerik.WinControls.RadThemeManager
		Private imageList1 As ImageList
		Private radToggleButton1 As Telerik.WinControls.UI.RadToggleButton
	End Class
End Namespace
