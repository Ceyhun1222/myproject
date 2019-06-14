Namespace Telerik.Examples.WinControls.GridView.Grouping.CustomGrouping
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
			Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn2 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn3 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn4 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn5 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn6 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn7 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewTextBoxColumn8 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Me.radGridView = New Telerik.WinControls.UI.RadGridView()
			Me.customersBindingSource = New BindingSource(Me.components)
			Me.nwindRadGridView = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
			Me.customersTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CustomersTableAdapter()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.customersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.nwindRadGridView, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.ForeColor = Color.Black
			Me.settingsPanel.Location = New Point(1446, 1)
			' 
			' 
			' 
			Me.settingsPanel.RootElement.ForeColor = Color.Black
			Me.settingsPanel.Size = New Size(200, 747)
			Me.settingsPanel.ThemeName = "ControlDefault"
			' 
			' radGridView
			' 
			Me.radGridView.Anchor = (CType((((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radGridView.BackColor = Color.FromArgb((CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))))
			Me.radGridView.ForeColor = Color.Black
			Me.radGridView.Location = New Point(3, 4)
			' 
			' radGridView
			' 
			Me.radGridView.MasterTemplate.AllowAddNewRow = False
			Me.radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
			Me.radGridView.MasterTemplate.Caption = Nothing
			gridViewTextBoxColumn1.FieldName = "CompanyName"
			gridViewTextBoxColumn1.HeaderText = "CompanyName"
			gridViewTextBoxColumn1.IsAutoGenerated = True
			gridViewTextBoxColumn1.Name = "CompanyName"
			gridViewTextBoxColumn1.Width = 186
			gridViewTextBoxColumn2.FieldName = "ContactName"
			gridViewTextBoxColumn2.HeaderText = "ContactName"
			gridViewTextBoxColumn2.IsAutoGenerated = True
			gridViewTextBoxColumn2.Name = "ContactName"
			gridViewTextBoxColumn2.Width = 186
			gridViewTextBoxColumn3.FieldName = "ContactTitle"
			gridViewTextBoxColumn3.HeaderText = "ContactTitle"
			gridViewTextBoxColumn3.IsAutoGenerated = True
			gridViewTextBoxColumn3.Name = "ContactTitle"
			gridViewTextBoxColumn3.Width = 186
			gridViewTextBoxColumn4.FieldName = "Address"
			gridViewTextBoxColumn4.HeaderText = "Address"
			gridViewTextBoxColumn4.IsAutoGenerated = True
			gridViewTextBoxColumn4.Name = "Address"
			gridViewTextBoxColumn4.Width = 186
			gridViewTextBoxColumn5.FieldName = "City"
			gridViewTextBoxColumn5.HeaderText = "City"
			gridViewTextBoxColumn5.IsAutoGenerated = True
			gridViewTextBoxColumn5.Name = "City"
			gridViewTextBoxColumn5.Width = 186
			gridViewTextBoxColumn6.FieldName = "PostalCode"
			gridViewTextBoxColumn6.HeaderText = "PostalCode"
			gridViewTextBoxColumn6.IsAutoGenerated = True
			gridViewTextBoxColumn6.Name = "PostalCode"
			gridViewTextBoxColumn6.Width = 186
			gridViewTextBoxColumn7.FieldName = "Country"
			gridViewTextBoxColumn7.HeaderText = "Country"
			gridViewTextBoxColumn7.IsAutoGenerated = True
			gridViewTextBoxColumn7.Name = "Country"
			gridViewTextBoxColumn7.Width = 186
			gridViewTextBoxColumn8.FieldName = "Phone"
			gridViewTextBoxColumn8.HeaderText = "Phone"
			gridViewTextBoxColumn8.IsAutoGenerated = True
			gridViewTextBoxColumn8.Name = "Phone"
			gridViewTextBoxColumn8.Width = 183
			Me.radGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() { gridViewTextBoxColumn1, gridViewTextBoxColumn2, gridViewTextBoxColumn3, gridViewTextBoxColumn4, gridViewTextBoxColumn5, gridViewTextBoxColumn6, gridViewTextBoxColumn7, gridViewTextBoxColumn8 })
			Me.radGridView.MasterTemplate.DataSource = Me.customersBindingSource
			Me.radGridView.Name = "radGridView"
			' 
			' 
			' 
			Me.radGridView.RootElement.ForeColor = Color.Black
			Me.radGridView.Size = New Size(1635, 745)
			Me.radGridView.TabIndex = 0
			Me.radGridView.Text = "radGridView1"
			Me.radGridView.ThemeName = "Telerik"

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
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radGridView)
			Me.Name = "Form1"
			Me.Size = New Size(1647, 749)

			Me.Controls.SetChildIndex(Me.radGridView, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.customersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.nwindRadGridView, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radGridView As Telerik.WinControls.UI.RadGridView
		Private customersBindingSource As BindingSource
		Private nwindRadGridView As Telerik.Examples.WinControls.DataSources.NorthwindDataSet
		Private customersTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CustomersTableAdapter
	End Class
End Namespace