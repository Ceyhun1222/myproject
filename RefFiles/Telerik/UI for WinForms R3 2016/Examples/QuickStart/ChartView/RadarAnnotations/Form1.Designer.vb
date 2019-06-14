Imports Microsoft.VisualBasic
Imports System
Imports Telerik.WinControls.UI
Namespace Telerik.Examples.WinControls.ChartView.RadarAnnotations
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
			Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radCheckBox30N = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBox60N = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBox30S = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBox60S = New Telerik.WinControls.UI.RadCheckBox()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radCheckBoxSouthernSolstice = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxSouthwardEquinox = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxNorthernSolstice = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxNorthwardEquinox = New Telerik.WinControls.UI.RadCheckBox()
			Me.radGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radCheckBoxWinter = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxAutumn = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxSummer = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxSpring = New Telerik.WinControls.UI.RadCheckBox()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radGroupBox4 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
			Me.radCheckBoxWorkHours = New Telerik.WinControls.UI.RadCheckBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			CType(Me.radCheckBox30N, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox60N, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox30S, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox60S, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox2.SuspendLayout()
			CType(Me.radCheckBoxSouthernSolstice, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxSouthwardEquinox, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxNorthernSolstice, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxNorthwardEquinox, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox3.SuspendLayout()
			CType(Me.radCheckBoxWinter, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxAutumn, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxSummer, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxSpring, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox4, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox4.SuspendLayout()
			CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxWorkHours, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupBox4)
			Me.settingsPanel.Controls.Add(Me.radGroupBox3)
			Me.settingsPanel.Controls.Add(Me.radGroupBox2)
			Me.settingsPanel.Controls.Add(Me.radLabel1)
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Location = New System.Drawing.Point(1564, 3)
			Me.settingsPanel.Size = New System.Drawing.Size(304, 832)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox3, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox4, 0)
			' 
			' radChartView1
			' 
			Me.radChartView1.AreaType = Telerik.WinControls.UI.ChartAreaType.Polar
			Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartView1.Location = New System.Drawing.Point(0, 0)
			Me.radChartView1.Name = "radChartView1"
			Me.radChartView1.Size = New System.Drawing.Size(1871, 1086)
			Me.radChartView1.TabIndex = 2
			Me.radChartView1.Text = "radChartView1"
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox1.Controls.Add(Me.radCheckBox30N)
			Me.radGroupBox1.Controls.Add(Me.radCheckBox60N)
			Me.radGroupBox1.Controls.Add(Me.radCheckBox30S)
			Me.radGroupBox1.Controls.Add(Me.radCheckBox60S)
			Me.radGroupBox1.HeaderText = "Latitude"
			Me.radGroupBox1.Location = New System.Drawing.Point(10, 66)
			Me.radGroupBox1.Name = "radGroupBox1"
			Me.radGroupBox1.Size = New System.Drawing.Size(284, 123)
			Me.radGroupBox1.TabIndex = 0
			Me.radGroupBox1.Text = "Latitude"
			' 
			' radCheckBox30N
			' 
			Me.radCheckBox30N.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBox30N.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBox30N.Location = New System.Drawing.Point(5, 94)
			Me.radCheckBox30N.Name = "radCheckBox30N"
			Me.radCheckBox30N.Size = New System.Drawing.Size(48, 18)
			Me.radCheckBox30N.TabIndex = 0
			Me.radCheckBox30N.Text = "30°N"
			Me.radCheckBox30N.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			CType(Me.radCheckBox30N.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadCheckmark).Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' radCheckBox60N
			' 
			Me.radCheckBox60N.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBox60N.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBox60N.Location = New System.Drawing.Point(5, 70)
			Me.radCheckBox60N.Name = "radCheckBox60N"
			Me.radCheckBox60N.Size = New System.Drawing.Size(48, 18)
			Me.radCheckBox60N.TabIndex = 0
			Me.radCheckBox60N.Text = "60°N"
			Me.radCheckBox60N.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			CType(Me.radCheckBox60N.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadCheckmark).Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' radCheckBox30S
			' 
			Me.radCheckBox30S.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBox30S.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBox30S.Location = New System.Drawing.Point(5, 46)
			Me.radCheckBox30S.Name = "radCheckBox30S"
			Me.radCheckBox30S.Size = New System.Drawing.Size(46, 18)
			Me.radCheckBox30S.TabIndex = 0
			Me.radCheckBox30S.Text = "30°S"
			Me.radCheckBox30S.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			CType(Me.radCheckBox30S.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadCheckmark).Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' radCheckBox60S
			' 
			Me.radCheckBox60S.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBox60S.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBox60S.Location = New System.Drawing.Point(5, 22)
			Me.radCheckBox60S.Name = "radCheckBox60S"
			Me.radCheckBox60S.Size = New System.Drawing.Size(46, 18)
			Me.radCheckBox60S.TabIndex = 0
			Me.radCheckBox60S.Text = "60°S"
			Me.radCheckBox60S.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			CType(Me.radCheckBox60S.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadCheckmark).Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel1.Location = New System.Drawing.Point(10, 32)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New System.Drawing.Size(150, 18)
			Me.radLabel1.TabIndex = 1
			Me.radLabel1.Text = "Year-round hours of daylight"
			' 
			' radGroupBox2
			' 
			Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox2.Controls.Add(Me.radLabel2)
			Me.radGroupBox2.Controls.Add(Me.radCheckBoxSouthernSolstice)
			Me.radGroupBox2.Controls.Add(Me.radCheckBoxSouthwardEquinox)
			Me.radGroupBox2.Controls.Add(Me.radCheckBoxNorthernSolstice)
			Me.radGroupBox2.Controls.Add(Me.radCheckBoxNorthwardEquinox)
			Me.radGroupBox2.HeaderText = "Solstices and equinoxes"
			Me.radGroupBox2.Location = New System.Drawing.Point(10, 195)
			Me.radGroupBox2.Name = "radGroupBox2"
			Me.radGroupBox2.Size = New System.Drawing.Size(284, 147)
			Me.radGroupBox2.TabIndex = 2
			Me.radGroupBox2.Text = "Solstices and equinoxes"
			' 
			' radCheckBoxSouthernSolstice
			' 
			Me.radCheckBoxSouthernSolstice.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxSouthernSolstice.Location = New System.Drawing.Point(5, 118)
			Me.radCheckBoxSouthernSolstice.Name = "radCheckBoxSouthernSolstice"
			Me.radCheckBoxSouthernSolstice.Size = New System.Drawing.Size(107, 18)
			Me.radCheckBoxSouthernSolstice.TabIndex = 1
			Me.radCheckBoxSouthernSolstice.Text = "Southern Solstice"
			' 
			' radCheckBoxSouthwardEquinox
			' 
			Me.radCheckBoxSouthwardEquinox.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxSouthwardEquinox.Location = New System.Drawing.Point(5, 94)
			Me.radCheckBoxSouthwardEquinox.Name = "radCheckBoxSouthwardEquinox"
			Me.radCheckBoxSouthwardEquinox.Size = New System.Drawing.Size(117, 18)
			Me.radCheckBoxSouthwardEquinox.TabIndex = 2
			Me.radCheckBoxSouthwardEquinox.Text = "Southward Equinox"
			' 
			' radCheckBoxNorthernSolstice
			' 
			Me.radCheckBoxNorthernSolstice.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxNorthernSolstice.Location = New System.Drawing.Point(5, 70)
			Me.radCheckBoxNorthernSolstice.Name = "radCheckBoxNorthernSolstice"
			Me.radCheckBoxNorthernSolstice.Size = New System.Drawing.Size(107, 18)
			Me.radCheckBoxNorthernSolstice.TabIndex = 3
			Me.radCheckBoxNorthernSolstice.Text = "Northern Solstice"
			' 
			' radCheckBoxNorthwardEquinox
			' 
			Me.radCheckBoxNorthwardEquinox.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxNorthwardEquinox.Location = New System.Drawing.Point(5, 46)
			Me.radCheckBoxNorthwardEquinox.Name = "radCheckBoxNorthwardEquinox"
			Me.radCheckBoxNorthwardEquinox.Size = New System.Drawing.Size(117, 18)
			Me.radCheckBoxNorthwardEquinox.TabIndex = 4
			Me.radCheckBoxNorthwardEquinox.Text = "Northward Equinox"
			' 
			' radGroupBox3
			' 
			Me.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox3.Controls.Add(Me.radLabel3)
			Me.radGroupBox3.Controls.Add(Me.radCheckBoxWinter)
			Me.radGroupBox3.Controls.Add(Me.radCheckBoxAutumn)
			Me.radGroupBox3.Controls.Add(Me.radCheckBoxSummer)
			Me.radGroupBox3.Controls.Add(Me.radCheckBoxSpring)
			Me.radGroupBox3.HeaderText = "Seasons"
			Me.radGroupBox3.Location = New System.Drawing.Point(10, 348)
			Me.radGroupBox3.Name = "radGroupBox3"
			Me.radGroupBox3.Size = New System.Drawing.Size(284, 146)
			Me.radGroupBox3.TabIndex = 2
			Me.radGroupBox3.Text = "Seasons"
			' 
			' radCheckBoxWinter
			' 
			Me.radCheckBoxWinter.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxWinter.Location = New System.Drawing.Point(5, 117)
			Me.radCheckBoxWinter.Name = "radCheckBoxWinter"
			Me.radCheckBoxWinter.Size = New System.Drawing.Size(54, 18)
			Me.radCheckBoxWinter.TabIndex = 1
			Me.radCheckBoxWinter.Text = "Winter"
			' 
			' radCheckBoxAutumn
			' 
			Me.radCheckBoxAutumn.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxAutumn.Location = New System.Drawing.Point(5, 93)
			Me.radCheckBoxAutumn.Name = "radCheckBoxAutumn"
			Me.radCheckBoxAutumn.Size = New System.Drawing.Size(60, 18)
			Me.radCheckBoxAutumn.TabIndex = 2
			Me.radCheckBoxAutumn.Text = "Autumn"
			' 
			' radCheckBoxSummer
			' 
			Me.radCheckBoxSummer.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxSummer.Location = New System.Drawing.Point(5, 69)
			Me.radCheckBoxSummer.Name = "radCheckBoxSummer"
			Me.radCheckBoxSummer.Size = New System.Drawing.Size(62, 18)
			Me.radCheckBoxSummer.TabIndex = 3
			Me.radCheckBoxSummer.Text = "Summer"
			' 
			' radCheckBoxSpring
			' 
			Me.radCheckBoxSpring.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxSpring.Location = New System.Drawing.Point(5, 45)
			Me.radCheckBoxSpring.Name = "radCheckBoxSpring"
			Me.radCheckBoxSpring.Size = New System.Drawing.Size(53, 18)
			Me.radCheckBoxSpring.TabIndex = 4
			Me.radCheckBoxSpring.Text = "Spring"
			' 
			' radLabel2
			' 
			Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel2.Location = New System.Drawing.Point(5, 22)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New System.Drawing.Size(164, 18)
			Me.radLabel2.TabIndex = 5
			Me.radLabel2.Text = "Radial axis grid line annotations"
			' 
			' radLabel3
			' 
			Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel3.Location = New System.Drawing.Point(5, 21)
			Me.radLabel3.Name = "radLabel3"
			Me.radLabel3.Size = New System.Drawing.Size(171, 18)
			Me.radLabel3.TabIndex = 5
			Me.radLabel3.Text = "Radial axis plot band annotations"
			' 
			' radGroupBox4
			' 
			Me.radGroupBox4.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox4.Controls.Add(Me.radLabel4)
			Me.radGroupBox4.Controls.Add(Me.radCheckBoxWorkHours)
			Me.radGroupBox4.HeaderText = "Work hours"
			Me.radGroupBox4.Location = New System.Drawing.Point(10, 501)
			Me.radGroupBox4.Name = "radGroupBox4"
			Me.radGroupBox4.Size = New System.Drawing.Size(284, 79)
			Me.radGroupBox4.TabIndex = 3
			Me.radGroupBox4.Text = "Work hours"
			' 
			' radLabel4
			' 
			Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel4.Location = New System.Drawing.Point(5, 21)
			Me.radLabel4.Name = "radLabel4"
			Me.radLabel4.Size = New System.Drawing.Size(161, 18)
			Me.radLabel4.TabIndex = 5
			Me.radLabel4.Text = "Polar axis plot band annotation"
			' 
			' radCheckBoxWorkHours
			' 
			Me.radCheckBoxWorkHours.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxWorkHours.Location = New System.Drawing.Point(5, 45)
			Me.radCheckBoxWorkHours.Name = "radCheckBoxWorkHours"
			Me.radCheckBoxWorkHours.Size = New System.Drawing.Size(78, 18)
			Me.radCheckBoxWorkHours.TabIndex = 4
			Me.radCheckBoxWorkHours.Text = "Work hours"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoScrollMinSize = New System.Drawing.Size(680, 420)
			Me.Controls.Add(Me.radChartView1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1881, 1096)
			Me.Controls.SetChildIndex(Me.radChartView1, 0)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			CType(Me.radCheckBox30N, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox60N, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox30S, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox60S, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox2.ResumeLayout(False)
			Me.radGroupBox2.PerformLayout()
			CType(Me.radCheckBoxSouthernSolstice, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxSouthwardEquinox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxNorthernSolstice, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxNorthwardEquinox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox3.ResumeLayout(False)
			Me.radGroupBox3.PerformLayout()
			CType(Me.radCheckBoxWinter, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxAutumn, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxSummer, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxSpring, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox4, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox4.ResumeLayout(False)
			Me.radGroupBox4.PerformLayout()
			CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxWorkHours, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radGroupBox3 As RadGroupBox
		Private radCheckBoxWinter As RadCheckBox
		Private radCheckBoxAutumn As RadCheckBox
		Private radCheckBoxSummer As RadCheckBox
		Private radCheckBoxSpring As RadCheckBox
		Private radGroupBox2 As RadGroupBox
		Private radCheckBoxSouthernSolstice As RadCheckBox
		Private radCheckBoxSouthwardEquinox As RadCheckBox
		Private radCheckBoxNorthernSolstice As RadCheckBox
		Private radCheckBoxNorthwardEquinox As RadCheckBox
		Private radLabel1 As RadLabel
		Private radGroupBox1 As RadGroupBox
		Private radCheckBox30N As RadCheckBox
		Private radCheckBox60N As RadCheckBox
		Private radCheckBox30S As RadCheckBox
		Private radCheckBox60S As RadCheckBox
		Private radChartView1 As RadChartView
		Private radGroupBox4 As RadGroupBox
		Private radLabel4 As RadLabel
		Private radCheckBoxWorkHours As RadCheckBox
		Private radLabel3 As RadLabel
		Private radLabel2 As RadLabel

	End Class
End Namespace