Imports Telerik.WinControls.UI
Namespace Telerik.Examples.WinControls.ChartView.LabelFormatting
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
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radCheckBox2)
            Me.settingsPanel.Controls.Add(Me.radCheckBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(989, 19)
            Me.settingsPanel.Size = New System.Drawing.Size(304, 832)
            Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBox2, 0)
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaDesign = cartesianArea1
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.MinimumSize = New System.Drawing.Size(680, 420)
            Me.radChartView1.Name = "radChartView1"
            ' 
            ' 
            ' 
            Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(680, 420)
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.ShowTitle = True
            Me.radChartView1.Size = New System.Drawing.Size(1158, 680)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            Me.radChartView1.Title = "Stock Index"
            CType(Me.radChartView1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Text = "Stock Index"
            CType(Me.radChartView1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Font = New System.Drawing.Font("Segoe UI Light", 20.0F)
            CType(Me.radChartView1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox1.Location = New System.Drawing.Point(10, 5)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(82, 18)
            Me.radCheckBox1.TabIndex = 1
            Me.radCheckBox1.Text = "Show Labels"
            Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBox2
            ' 
            Me.radCheckBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox2.Location = New System.Drawing.Point(10, 30)
            Me.radCheckBox2.Name = "radCheckBox2"
            Me.radCheckBox2.Size = New System.Drawing.Size(154, 18)
            Me.radCheckBox2.TabIndex = 2
            Me.radCheckBox2.Text = "Enable custom appearance"
            Me.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScrollMinSize = New System.Drawing.Size(680, 420)
            Me.Controls.Add(Me.radChartView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1168, 690)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.radChartView1, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
		Private radChartView1 As Telerik.WinControls.UI.RadChartView
	End Class
End Namespace