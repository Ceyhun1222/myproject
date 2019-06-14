Imports Telerik.WinControls
Namespace Telerik.Examples.WinControls.ChartView.FirstLook
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
            Dim cartesianArea1 As New Telerik.WinControls.UI.CartesianArea()
            Dim cartesianArea2 As New Telerik.WinControls.UI.CartesianArea()
            Me.tableLayoutPanel1 = New Telerik.Examples.WinControls.ChartView.FirstLook.CustomTableLayoutPanel()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radChartView2 = New Telerik.WinControls.UI.RadChartView()
            Me.radChartView3 = New Telerik.WinControls.UI.RadChartView()
            Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
            Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel2 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel3 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel4 = New Telerik.WinControls.UI.RadPanel()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tableLayoutPanel1.SuspendLayout()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Location = New System.Drawing.Point(1501, 19)
            Me.settingsPanel.Size = New System.Drawing.Size(0, 0)
            ' 
            ' tableLayoutPanel1
            ' 
            Me.tableLayoutPanel1.ColumnCount = 4
            Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F))
            Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0F))
            Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F))
            Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 151.0F))
            Me.tableLayoutPanel1.Controls.Add(Me.radChartView1, 0, 2)
            Me.tableLayoutPanel1.Controls.Add(Me.radChartView2, 2, 2)
            Me.tableLayoutPanel1.Controls.Add(Me.radChartView3, 0, 4)
            Me.tableLayoutPanel1.Controls.Add(Me.radGridView1, 2, 4)
            Me.tableLayoutPanel1.Controls.Add(Me.radPanel1, 0, 0)
            Me.tableLayoutPanel1.Controls.Add(Me.radPanel2, 0, 1)
            Me.tableLayoutPanel1.Controls.Add(Me.radPanel3, 2, 1)
            Me.tableLayoutPanel1.Controls.Add(Me.radPanel4, 0, 3)
            Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.tableLayoutPanel1.MinimumSize = New System.Drawing.Size(1148, 610)
            Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
            Me.tableLayoutPanel1.RowCount = 5
            Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45.0F))
            Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0F))
            Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F))
            Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0F))
            Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F))
            Me.tableLayoutPanel1.Size = New System.Drawing.Size(1148, 680)
            Me.tableLayoutPanel1.TabIndex = 1
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaDesign = cartesianArea1
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Location = New System.Drawing.Point(0, 75)
            Me.radChartView1.Margin = New System.Windows.Forms.Padding(0)
            Me.radChartView1.Name = "radChartView1"
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.Size = New System.Drawing.Size(448, 287)
            Me.radChartView1.TabIndex = 0
            Me.radChartView1.Text = "radChartView1"
            ' 
            ' radChartView2
            ' 
            Me.radChartView2.AreaType = Telerik.WinControls.UI.ChartAreaType.Pie
            Me.radChartView2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView2.Location = New System.Drawing.Point(548, 75)
            Me.radChartView2.Margin = New System.Windows.Forms.Padding(0)
            Me.radChartView2.Name = "radChartView2"
            Me.radChartView2.ShowGrid = False
            Me.radChartView2.Size = New System.Drawing.Size(448, 287)
            Me.radChartView2.TabIndex = 1
            Me.radChartView2.Text = "radChartView2"
            ' 
            ' radChartView3
            ' 
            Me.radChartView3.AreaDesign = cartesianArea2
            Me.radChartView3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView3.Location = New System.Drawing.Point(0, 392)
            Me.radChartView3.Margin = New System.Windows.Forms.Padding(0)
            Me.radChartView3.Name = "radChartView3"
            Me.radChartView3.ShowGrid = False
            Me.radChartView3.Size = New System.Drawing.Size(448, 288)
            Me.radChartView3.TabIndex = 2
            Me.radChartView3.Text = "radChartView3"
            ' 
            ' radGridView1
            ' 
            Me.tableLayoutPanel1.SetColumnSpan(Me.radGridView1, 2)
            Me.radGridView1.AllowCellContextMenu = False
            Me.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radGridView1.Location = New System.Drawing.Point(548, 392)
            Me.radGridView1.Margin = New System.Windows.Forms.Padding(0)
            Me.radGridView1.Name = "radGridView1"
            Me.radGridView1.Size = New System.Drawing.Size(600, 288)
            Me.radGridView1.TabIndex = 0
            Me.radGridView1.Text = "radGridView1"
            CType(Me.radGridView1.GetChildAt(0), Telerik.WinControls.UI.RadGridViewElement).BorderColor = System.Drawing.Color.White
            ' 
            ' radPanel1
            ' 
            Me.radPanel1.BackColor = System.Drawing.Color.FromArgb(CInt(CByte(238)), CInt(CByte(239)), CInt(CByte(238)))
            Me.tableLayoutPanel1.SetColumnSpan(Me.radPanel1, 4)
            Me.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel1.Font = New System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radPanel1.Location = New System.Drawing.Point(0, 0)
            Me.radPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.Size = New System.Drawing.Size(1148, 45)
            Me.radPanel1.TabIndex = 4
            Me.radPanel1.Text = "SALES DASHBOARD 2012"
            CType(Me.radPanel1.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).Text = "SALES DASHBOARD 2012"
            CType(Me.radPanel1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).ForeColor = System.Drawing.Color.FromArgb(CInt(CByte(238)), CInt(CByte(239)), CInt(CByte(238)))
            CType(Me.radPanel1.GetChildAt(0).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            ' 
            ' radPanel2
            ' 
            Me.radPanel2.BackColor = System.Drawing.Color.White
            Me.tableLayoutPanel1.SetColumnSpan(Me.radPanel2, 2)
            Me.radPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel2.Font = New System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radPanel2.Location = New System.Drawing.Point(0, 45)
            Me.radPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.radPanel2.Name = "radPanel2"
            Me.radPanel2.Size = New System.Drawing.Size(548, 30)
            Me.radPanel2.TabIndex = 4
            Me.radPanel2.Text = "SALES DASHBOARD 2012"
            CType(Me.radPanel2.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).Text = "Total sales by product"
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).ForeColor = System.Drawing.Color.White
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            ' 
            ' radPanel3
            ' 
            Me.radPanel3.BackColor = System.Drawing.Color.White
            Me.tableLayoutPanel1.SetColumnSpan(Me.radPanel3, 2)
            Me.radPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel3.Font = New System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radPanel3.Location = New System.Drawing.Point(548, 45)
            Me.radPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.radPanel3.Name = "radPanel3"
            Me.radPanel3.Size = New System.Drawing.Size(600, 30)
            Me.radPanel3.TabIndex = 4
            Me.radPanel3.Text = "SALES DASHBOARD 2012"
            CType(Me.radPanel3.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).Text = "Sales breakdown by region"
            CType(Me.radPanel3.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).ForeColor = System.Drawing.Color.White
            CType(Me.radPanel3.GetChildAt(0).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            ' 
            ' radPanel4
            ' 
            Me.radPanel4.BackColor = System.Drawing.Color.White
            Me.tableLayoutPanel1.SetColumnSpan(Me.radPanel4, 4)
            Me.radPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel4.Font = New System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radPanel4.Location = New System.Drawing.Point(0, 362)
            Me.radPanel4.Margin = New System.Windows.Forms.Padding(0)
            Me.radPanel4.Name = "radPanel4"
            Me.radPanel4.Size = New System.Drawing.Size(1148, 30)
            Me.radPanel4.TabIndex = 4
            Me.radPanel4.Text = "SALES DASHBOARD 2012"
            CType(Me.radPanel4.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).Text = "Percent of target"
            CType(Me.radPanel4.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).ForeColor = System.Drawing.Color.White
            CType(Me.radPanel4.GetChildAt(0).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScrollMinSize = New System.Drawing.Size(1148, 610)
            Me.Controls.Add(Me.tableLayoutPanel1)
            Me.Name = "Form1"
            Me.Padding = New System.Windows.Forms.Padding(0, 0, 20, 10)
            Me.Size = New System.Drawing.Size(1168, 690)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.tableLayoutPanel1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tableLayoutPanel1.ResumeLayout(False)
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private tableLayoutPanel1 As CustomTableLayoutPanel
		Private radGridView1 As Telerik.WinControls.UI.RadGridView
		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radChartView2 As Telerik.WinControls.UI.RadChartView
		Private radChartView3 As Telerik.WinControls.UI.RadChartView
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		Private radPanel2 As Telerik.WinControls.UI.RadPanel
		Private radPanel3 As Telerik.WinControls.UI.RadPanel
		Private radPanel4 As Telerik.WinControls.UI.RadPanel

	End Class
End Namespace
