'using Telerik.Examples.WinControls.ChartView.FirstLook;
Namespace Telerik.Examples.WinControls.ChartView.LiveData
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
            Dim CartesianArea1 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
            Dim CartesianArea2 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.ExampleCustomShapeControl4 = New Telerik.Examples.WinControls.ChartView.LiveData.ExampleCustomShapeControl()
            Me.radChartView2 = New Telerik.WinControls.UI.RadChartView()
            Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel2 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel4 = New Telerik.WinControls.UI.RadPanel()
            Me.radPanel3 = New Telerik.WinControls.UI.RadPanel()
            Me.exampleCustomShapeControl1 = New Telerik.Examples.WinControls.ChartView.LiveData.ExampleCustomShapeControl()
            Me.exampleCustomShapeControl2 = New Telerik.Examples.WinControls.ChartView.LiveData.ExampleCustomShapeControl()
            Me.exampleCustomShapeControl3 = New Telerik.Examples.WinControls.ChartView.LiveData.ExampleCustomShapeControl()
            Me.RadioButton1 = New Telerik.WinControls.UI.RadRadioButton()
            Me.RadRadioButton1 = New Telerik.WinControls.UI.RadRadioButton()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radChartView1.SuspendLayout()
            CType(Me.ExampleCustomShapeControl4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel1.SuspendLayout()
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel2.SuspendLayout()
            CType(Me.radPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel4.SuspendLayout()
            CType(Me.radPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel3.SuspendLayout()
            CType(Me.exampleCustomShapeControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.exampleCustomShapeControl2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.exampleCustomShapeControl3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadioButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadRadioButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.RadRadioButton1)
            Me.settingsPanel.Controls.Add(Me.RadioButton1)
            Me.settingsPanel.Location = New System.Drawing.Point(938, 19)
            Me.settingsPanel.Size = New System.Drawing.Size(164, 360)
            Me.settingsPanel.Controls.SetChildIndex(Me.RadioButton1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.RadRadioButton1, 0)
            '
            'radChartView1
            '
            Me.radChartView1.AreaDesign = CartesianArea1
            Me.radChartView1.BackColor = System.Drawing.Color.Transparent
            Me.radChartView1.Controls.Add(Me.ExampleCustomShapeControl4)
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.Name = "radChartView1"
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.ShowTitle = True
            Me.radChartView1.Size = New System.Drawing.Size(1290, 471)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            Me.radChartView1.Title = "Messages Sent/Received"
            '
            'ExampleCustomShapeControl4
            '
            Me.ExampleCustomShapeControl4.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ExampleCustomShapeControl4.LeftText = "..."
            Me.ExampleCustomShapeControl4.Location = New System.Drawing.Point(1108, 45)
            Me.ExampleCustomShapeControl4.Name = "ExampleCustomShapeControl4"
            Me.ExampleCustomShapeControl4.RightText = "FPS"
            Me.ExampleCustomShapeControl4.Size = New System.Drawing.Size(144, 45)
            Me.ExampleCustomShapeControl4.TabIndex = 0
            Me.ExampleCustomShapeControl4.Text = "exampleCustomShapeControl1"
            CType(Me.ExampleCustomShapeControl4.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.LightVisualElement).Text = "FPS"
            CType(Me.ExampleCustomShapeControl4.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.LightVisualElement).ForeColor = System.Drawing.Color.White
            '
            'radChartView2
            '
            Me.radChartView2.AreaDesign = CartesianArea2
            Me.radChartView2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radChartView2.Location = New System.Drawing.Point(0, 0)
            Me.radChartView2.Name = "radChartView2"
            Me.radChartView2.ShowGrid = False
            Me.radChartView2.ShowTitle = True
            Me.radChartView2.Size = New System.Drawing.Size(1011, 234)
            Me.radChartView2.TabIndex = 2
            Me.radChartView2.Text = "radChartView2"
            Me.radChartView2.Title = "Site Activity by Time of Day"
            '
            'radPanel1
            '
            Me.radPanel1.Controls.Add(Me.radChartView1)
            Me.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel1.Location = New System.Drawing.Point(0, 0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.Size = New System.Drawing.Size(1290, 471)
            Me.radPanel1.TabIndex = 3
            Me.radPanel1.Text = ""
            '
            'radPanel2
            '
            Me.radPanel2.Controls.Add(Me.radPanel4)
            Me.radPanel2.Controls.Add(Me.radPanel3)
            Me.radPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.radPanel2.Location = New System.Drawing.Point(0, 471)
            Me.radPanel2.Name = "radPanel2"
            Me.radPanel2.Size = New System.Drawing.Size(1290, 234)
            Me.radPanel2.TabIndex = 4
            Me.radPanel2.Text = "radPanel2"
            '
            'radPanel4
            '
            Me.radPanel4.Controls.Add(Me.radChartView2)
            Me.radPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel4.Location = New System.Drawing.Point(0, 0)
            Me.radPanel4.Name = "radPanel4"
            Me.radPanel4.Size = New System.Drawing.Size(1011, 234)
            Me.radPanel4.TabIndex = 1
            Me.radPanel4.Text = "radPanel4"
            '
            'radPanel3
            '
            Me.radPanel3.BackColor = System.Drawing.Color.White
            Me.radPanel3.Controls.Add(Me.exampleCustomShapeControl1)
            Me.radPanel3.Controls.Add(Me.exampleCustomShapeControl2)
            Me.radPanel3.Controls.Add(Me.exampleCustomShapeControl3)
            Me.radPanel3.Dock = System.Windows.Forms.DockStyle.Right
            Me.radPanel3.Location = New System.Drawing.Point(1011, 0)
            Me.radPanel3.Name = "radPanel3"
            Me.radPanel3.Size = New System.Drawing.Size(279, 234)
            Me.radPanel3.TabIndex = 0
            CType(Me.radPanel3.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).BoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder
            CType(Me.radPanel3.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).ForeColor = System.Drawing.Color.White
            '
            'exampleCustomShapeControl1
            '
            Me.exampleCustomShapeControl1.Font = New System.Drawing.Font("Segoe UI", 8.25F)
            Me.exampleCustomShapeControl1.LeftText = "1,200"
            Me.exampleCustomShapeControl1.Location = New System.Drawing.Point(90, 45)
            Me.exampleCustomShapeControl1.Name = "exampleCustomShapeControl1"
            Me.exampleCustomShapeControl1.RightText = "AVG Messages per second"
            Me.exampleCustomShapeControl1.Size = New System.Drawing.Size(190, 45)
            Me.exampleCustomShapeControl1.TabIndex = 0
            Me.exampleCustomShapeControl1.Text = "exampleCustomShapeControl1"
            '
            'exampleCustomShapeControl2
            '
            Me.exampleCustomShapeControl2.Font = New System.Drawing.Font("Segoe UI", 8.25F)
            Me.exampleCustomShapeControl2.LeftText = "53,210"
            Me.exampleCustomShapeControl2.Location = New System.Drawing.Point(65, 100)
            Me.exampleCustomShapeControl2.Name = "exampleCustomShapeControl2"
            Me.exampleCustomShapeControl2.RightText = "AVG Messages per minute"
            Me.exampleCustomShapeControl2.Size = New System.Drawing.Size(215, 45)
            Me.exampleCustomShapeControl2.TabIndex = 0
            Me.exampleCustomShapeControl2.Text = "exampleCustomShapeControl2"
            '
            'exampleCustomShapeControl3
            '
            Me.exampleCustomShapeControl3.Font = New System.Drawing.Font("Segoe UI", 8.25F)
            Me.exampleCustomShapeControl3.LeftText = "3,729,600"
            Me.exampleCustomShapeControl3.Location = New System.Drawing.Point(40, 155)
            Me.exampleCustomShapeControl3.Name = "exampleCustomShapeControl3"
            Me.exampleCustomShapeControl3.RightText = "AVG Messages per hour"
            Me.exampleCustomShapeControl3.Size = New System.Drawing.Size(240, 45)
            Me.exampleCustomShapeControl3.TabIndex = 0
            Me.exampleCustomShapeControl3.Text = "exampleCustomShapeControl3"
            '
            'RadioButton1
            '
            Me.RadioButton1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadioButton1.Location = New System.Drawing.Point(10, 31)
            Me.RadioButton1.Name = "RadioButton1"
            Me.RadioButton1.Size = New System.Drawing.Size(73, 18)
            Me.RadioButton1.TabIndex = 1
            Me.RadioButton1.TabStop = False
            Me.RadioButton1.Text = "Line Series"
            '
            'RadRadioButton1
            '
            Me.RadRadioButton1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadRadioButton1.CheckState = System.Windows.Forms.CheckState.Checked
            Me.RadRadioButton1.Location = New System.Drawing.Point(10, 55)
            Me.RadRadioButton1.Name = "RadRadioButton1"
            Me.RadRadioButton1.Size = New System.Drawing.Size(96, 18)
            Me.RadRadioButton1.TabIndex = 1
            Me.RadRadioButton1.Text = "Fast Line Series"
            Me.RadRadioButton1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'Form1
            '
            Me.Controls.Add(Me.radPanel1)
            Me.Controls.Add(Me.radPanel2)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1300, 715)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radPanel2, 0)
            Me.Controls.SetChildIndex(Me.radPanel1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radChartView1.ResumeLayout(False)
            CType(Me.ExampleCustomShapeControl4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel1.ResumeLayout(False)
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel2.ResumeLayout(False)
            CType(Me.radPanel4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel4.ResumeLayout(False)
            CType(Me.radPanel3, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel3.ResumeLayout(False)
            CType(Me.exampleCustomShapeControl1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.exampleCustomShapeControl2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.exampleCustomShapeControl3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadioButton1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadRadioButton1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radChartView2 As Telerik.WinControls.UI.RadChartView
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		Private radPanel2 As Telerik.WinControls.UI.RadPanel
		Private radPanel4 As Telerik.WinControls.UI.RadPanel
		Private radPanel3 As Telerik.WinControls.UI.RadPanel
		Private exampleCustomShapeControl1 As ExampleCustomShapeControl
		Private exampleCustomShapeControl2 As ExampleCustomShapeControl
        Private exampleCustomShapeControl3 As ExampleCustomShapeControl
        Private WithEvents ExampleCustomShapeControl4 As Telerik.Examples.WinControls.ChartView.LiveData.ExampleCustomShapeControl
        Friend WithEvents RadioButton1 As Telerik.WinControls.UI.RadRadioButton
        Friend WithEvents RadRadioButton1 As Telerik.WinControls.UI.RadRadioButton
	End Class
End Namespace