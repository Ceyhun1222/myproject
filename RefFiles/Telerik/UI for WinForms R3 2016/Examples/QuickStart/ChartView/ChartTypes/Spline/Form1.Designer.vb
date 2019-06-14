Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Spline
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
            Dim cartesianArea1 As New Telerik.WinControls.UI.CartesianArea()
            Me.employeeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.adventureLT2008DataSet = New Telerik.Examples.WinControls.DataSources.AdventureLT2008DataSet()
            Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.employeeTableAdapter = New Telerik.Examples.WinControls.DataSources.AdventureLT2008DataSetTableAdapters.EmployeeTableAdapter()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownList2 = New Telerik.WinControls.UI.RadDropDownList()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.adventureLT2008DataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownList2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radLabel1)
            Me.settingsPanel.Controls.Add(Me.radLabel2)
            Me.settingsPanel.Controls.Add(Me.radDropDownList1)
            Me.settingsPanel.Controls.Add(Me.radDropDownList2)
            Me.settingsPanel.Controls.Add(Me.radCheckBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(982, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(238, 747)
            Me.settingsPanel.ThemeName = "ControlDefault"
            Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radDropDownList2, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radDropDownList1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabel2, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
            ' 
            ' employeeBindingSource
            ' 
            Me.employeeBindingSource.DataMember = "Employee"
            Me.employeeBindingSource.DataSource = Me.adventureLT2008DataSet
            ' 
            ' adventureLT2008DataSet
            ' 
            Me.adventureLT2008DataSet.DataSetName = "AdventureLT2008DataSet"
            Me.adventureLT2008DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            ' 
            ' imageList1
            ' 
            Me.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
            Me.imageList1.ImageSize = New System.Drawing.Size(16, 16)
            Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
            ' 
            ' employeeTableAdapter
            ' 
            Me.employeeTableAdapter.ClearBeforeFill = True
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaDesign = cartesianArea1
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.ImageList = Me.imageList1
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.MinimumSize = New System.Drawing.Size(700, 320)
            Me.radChartView1.Name = "radChartView1"
            ' 
            ' 
            ' 
            Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(700, 320)
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.Size = New System.Drawing.Size(1158, 612)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(10, 60)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(60, 18)
            Me.radLabel1.TabIndex = 1
            Me.radLabel1.Text = "Chart type:"
            ' 
            ' radDropDownList1
            ' 
            Me.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownList1.Location = New System.Drawing.Point(10, 81)
            Me.radDropDownList1.Name = "radDropDownList1"
            Me.radDropDownList1.Size = New System.Drawing.Size(218, 20)
            Me.radDropDownList1.TabIndex = 2
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(10, 112)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(85, 18)
            Me.radLabel2.TabIndex = 3
            Me.radLabel2.Text = "Combine mode:"
            ' 
            ' radDropDownList2
            ' 
            Me.radDropDownList2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownList2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownList2.Location = New System.Drawing.Point(10, 133)
            Me.radDropDownList2.Name = "radDropDownList2"
            Me.radDropDownList2.Size = New System.Drawing.Size(218, 20)
            Me.radDropDownList2.TabIndex = 4
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox1.Location = New System.Drawing.Point(10, 159)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(79, 18)
            Me.radCheckBox1.TabIndex = 5
            Me.radCheckBox1.Text = "Show labels"
            Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' Form1
            ' 
            Me.AutoScrollMinSize = New System.Drawing.Size(700, 320)
            Me.Controls.Add(Me.radChartView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1168, 622)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radChartView1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.adventureLT2008DataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownList2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private employeeBindingSource As BindingSource
		Private adventureLT2008DataSet As DataSources.AdventureLT2008DataSet
		Private employeeTableAdapter As DataSources.AdventureLT2008DataSetTableAdapters.EmployeeTableAdapter
		Private imageList1 As ImageList
		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radDropDownList1 As Telerik.WinControls.UI.RadDropDownList
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radDropDownList2 As Telerik.WinControls.UI.RadDropDownList
		Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
	End Class
End Namespace
