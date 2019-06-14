Imports Microsoft.VisualBasic
Imports System
Namespace Telerik.Examples.WinControls.RangeSelector.MSFTStocks
	Public Partial Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (Not components Is Nothing) Then
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
            Dim CartesianArea1 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
            Dim CartesianArea2 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radChartView2 = New Telerik.WinControls.UI.RadChartView()
            Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel2 = New Telerik.WinControls.UI.RadPanel()
            Me.radRangeSelector1 = New Telerik.WinControls.UI.RadRangeSelector()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel1.SuspendLayout()
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel2.SuspendLayout()
            CType(Me.radRangeSelector1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'radChartView1
            '
            Me.radChartView1.AreaDesign = CartesianArea1
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.Name = "radChartView1"
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.Size = New System.Drawing.Size(1035, 449)
            Me.radChartView1.TabIndex = 0
            '
            'radChartView2
            '
            Me.radChartView2.AreaDesign = CartesianArea2
            Me.radChartView2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView2.Location = New System.Drawing.Point(0, 0)
            Me.radChartView2.Name = "radChartView2"
            Me.radChartView2.ShowGrid = False
            Me.radChartView2.Size = New System.Drawing.Size(1035, 156)
            Me.radChartView2.TabIndex = 0
            Me.radChartView2.Text = "radChartView2"
            '
            'radPanel1
            '
            Me.radPanel1.Controls.Add(Me.radChartView1)
            Me.radPanel1.Controls.Add(Me.radPanel2)
            Me.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel1.Location = New System.Drawing.Point(0, 0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.Size = New System.Drawing.Size(1035, 755)
            Me.radPanel1.TabIndex = 3
            '
            'radPanel2
            '
            Me.radPanel2.Controls.Add(Me.radChartView2)
            Me.radPanel2.Controls.Add(Me.radRangeSelector1)
            Me.radPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.radPanel2.Location = New System.Drawing.Point(0, 449)
            Me.radPanel2.Name = "radPanel2"
            Me.radPanel2.Size = New System.Drawing.Size(1035, 306)
            Me.radPanel2.TabIndex = 0
            '
            'radRangeSelector1
            '
            Me.radRangeSelector1.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.radRangeSelector1.Location = New System.Drawing.Point(0, 156)
            Me.radRangeSelector1.Name = "radRangeSelector1"
            Me.radRangeSelector1.Size = New System.Drawing.Size(1035, 150)
            Me.radRangeSelector1.TabIndex = 2
            Me.radRangeSelector1.Text = "radRangeSelector1"
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScrollMinSize = New System.Drawing.Size(900, 560)
            Me.Controls.Add(Me.radPanel1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1045, 765)
            Me.Controls.SetChildIndex(Me.radPanel1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel1.ResumeLayout(False)
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel2.ResumeLayout(False)
            CType(Me.radRangeSelector1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radChartView2 As Telerik.WinControls.UI.RadChartView
        Private radRangeSelector1 As Telerik.WinControls.UI.RadRangeSelector
        Private radPanel1 As Telerik.WinControls.UI.RadPanel
        Private radPanel2 As Telerik.WinControls.UI.RadPanel
	End Class
End Namespace