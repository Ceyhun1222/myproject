Namespace Telerik.Examples.WinControls.GridView.Events
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
			Me.nwindDataSet = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
			Me.carsBindingSource = New BindingSource(Me.components)
			Me.carsTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CarsTableAdapter()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radButton1 = New Telerik.WinControls.UI.RadButton()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radListBoxEvents = New Telerik.WinControls.UI.RadListControl()
			Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
			Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.nwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.carsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radListBoxEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanel1.SuspendLayout()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radLabel3)
			Me.settingsPanel.Controls.Add(Me.radButton1)
			Me.settingsPanel.Controls.Add(Me.radListBoxEvents)
			Me.settingsPanel.Controls.Add(Me.radLabel1)
			Me.settingsPanel.Location = New Point(918, 1)
			' 
			' 
			' 
			Me.settingsPanel.Size = New Size(200, 611)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radListBoxEvents, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radButton1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel3, 0)
			' 
			' nwindDataSet
			' 
			Me.nwindDataSet.DataSetName = "NwindDataSet"
			Me.nwindDataSet.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			' 
			' carsBindingSource
			' 
			Me.carsBindingSource.DataMember = "Cars"
			Me.carsBindingSource.DataSource = Me.nwindDataSet
			' 
			' carsTableAdapter
			' 
			Me.carsTableAdapter.ClearBeforeFill = True
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = AnchorStyles.Top
			Me.radLabel1.Location = New Point(10, 35)
			Me.radLabel1.Margin = New Padding(4, 3, 4, 3)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New Size(123, 14)
			Me.radLabel1.TabIndex = 2
			Me.radLabel1.Text = "Events fired by the grid:"
			Me.radLabel1.TextAlignment = ContentAlignment.MiddleCenter
			' 
			' radButton1
			' 
			Me.radButton1.AllowShowFocusCues = True
			Me.radButton1.Anchor = AnchorStyles.Top
			Me.radButton1.Location = New Point(10, 6)
			Me.radButton1.Margin = New Padding(4, 3, 4, 3)
			Me.radButton1.Name = "radButton1"
			' 
			' 
			' 
			Me.radButton1.Size = New Size(180, 23)
			Me.radButton1.TabIndex = 3
			Me.radButton1.Text = "Clear events list"

			' 
			' radLabel3
			' 
			Me.radLabel3.Anchor = AnchorStyles.Top
			Me.radLabel3.AutoSize = False
			Me.radLabel3.Location = New Point(10, 411)
			Me.radLabel3.Margin = New Padding(4, 3, 4, 3)
			Me.radLabel3.Name = "radLabel3"
			' 
			' 
			' 
			Me.radLabel3.RootElement.StretchHorizontally = True
			Me.radLabel3.RootElement.StretchVertically = True
			Me.radLabel3.Size = New Size(180, 42)
			Me.radLabel3.TabIndex = 5
			Me.radLabel3.Text = "CellMouseMove:"
			' 
			' radListBoxEvents
			' 
			Me.radListBoxEvents.Anchor = AnchorStyles.Top
			Me.radListBoxEvents.AutoScroll = True
			Me.radListBoxEvents.Location = New Point(10, 55)
			Me.radListBoxEvents.Margin = New Padding(4, 3, 4, 3)
			Me.radListBoxEvents.Name = "radListBoxEvents"
			' 
			' 
			' 
			Me.radListBoxEvents.Size = New Size(180, 350)
			Me.radListBoxEvents.TabIndex = 1
			' 
			' radGridView1
			' 
			Me.radGridView1.Dock = DockStyle.Fill
			Me.radGridView1.EnableHotTracking = False
			Me.radGridView1.Location = New Point(0, 50)
			Me.radGridView1.Margin = New Padding(4, 3, 4, 3)
			' 
			' 
			' 
			Me.radGridView1.MasterTemplate.AllowAddNewRow = False
			Me.radGridView1.MasterTemplate.AllowDragToGroup = False
			Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
			Me.radGridView1.MasterTemplate.Caption = Nothing
			Me.radGridView1.MasterTemplate.EnableGrouping = False
			Me.radGridView1.MasterTemplate.ShowGroupedColumns = True
			Me.radGridView1.Name = "radGridView1"
			' 
			' 
			' 
			Me.radGridView1.Size = New Size(1119, 563)
			Me.radGridView1.TabIndex = 0
			Me.radGridView1.Text = "radGridView1"

			' 
			' radPanel1
			' 
			Me.radPanel1.Controls.Add(Me.radLabel2)
			Me.radPanel1.Dock = DockStyle.Top
			Me.radPanel1.ForeColor = Color.Black
			Me.radPanel1.Location = New Point(0, 0)
			Me.radPanel1.Name = "radPanel1"
			' 
			' 
			' 
			Me.radPanel1.RootElement.ForeColor = Color.Black
			Me.radPanel1.Size = New Size(1119, 50)
			Me.radPanel1.TabIndex = 1
			CType(Me.radPanel1.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = Color.FromArgb((CInt(Fix((CByte(237))))), (CInt(Fix((CByte(237))))), (CInt(Fix((CByte(237))))))
			' 
			' radLabel2
			' 
			Me.radLabel2.Location = New Point(3, 17)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New Size(412, 14)
			Me.radLabel2.TabIndex = 0
			Me.radLabel2.Text = "Click on the grid to see the events which are displayed in the text box on the ri" & "ght."
			' 
			' Form1
			' 
			Me.Controls.Add(Me.radGridView1)
			Me.Controls.Add(Me.radPanel1)
			Me.Margin = New Padding(4, 3, 4, 3)
			Me.Name = "Form1"
			Me.Size = New Size(1119, 613)

			Me.Controls.SetChildIndex(Me.radPanel1, 0)
			Me.Controls.SetChildIndex(Me.radGridView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.nwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.carsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radListBoxEvents, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanel1.ResumeLayout(False)
			Me.radPanel1.PerformLayout()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private nwindDataSet As Telerik.Examples.WinControls.DataSources.NorthwindDataSet
		Private carsBindingSource As BindingSource
		Private carsTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CarsTableAdapter
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radButton1 As Telerik.WinControls.UI.RadButton
		Private radLabel3 As Telerik.WinControls.UI.RadLabel
		Private radListBoxEvents As Telerik.WinControls.UI.RadListControl
		Private radGridView1 As Telerik.WinControls.UI.RadGridView
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		Private radLabel2 As Telerik.WinControls.UI.RadLabel


	End Class
End Namespace