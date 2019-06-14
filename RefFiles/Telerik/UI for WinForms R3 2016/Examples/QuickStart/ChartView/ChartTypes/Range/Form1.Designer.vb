Imports Microsoft.VisualBasic
Imports System
Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Range
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

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Dim cartesianArea2 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
			Dim radListDataItem3 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem4 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
			Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radDropDownListChartType = New Telerik.WinControls.UI.RadDropDownList()
			Me.orientationCheckBox = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxSpline = New Telerik.WinControls.UI.RadCheckBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radDropDownListChartType, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.orientationCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxSpline, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radCheckBoxSpline)
			Me.settingsPanel.Controls.Add(Me.orientationCheckBox)
			Me.settingsPanel.Controls.Add(Me.radDropDownListChartType)
			Me.settingsPanel.Controls.Add(Me.radLabel2)
			Me.settingsPanel.Controls.Add(Me.radLabel1)
			Me.settingsPanel.Location = New System.Drawing.Point(834, 1)
			Me.settingsPanel.Size = New System.Drawing.Size(812, 883)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel2, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radDropDownListChartType, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.orientationCheckBox, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxSpline, 0)
			' 
			' radChartView1
			' 
			Me.radChartView1.AreaDesign = cartesianArea2
			Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartView1.Location = New System.Drawing.Point(0, 0)
			Me.radChartView1.MinimumSize = New System.Drawing.Size(550, 320)
			Me.radChartView1.Name = "radChartView1"
			' 
			' 
			' 
			Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(550, 320)
			Me.radChartView1.ShowGrid = False
			Me.radChartView1.ShowToolTip = True
			Me.radChartView1.Size = New System.Drawing.Size(1871, 1086)
			Me.radChartView1.TabIndex = 1
			Me.radChartView1.Text = "radChartView1"
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel1.Location = New System.Drawing.Point(10, 45)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New System.Drawing.Size(62, 18)
			Me.radLabel1.TabIndex = 0
			Me.radLabel1.Text = "Series type:"
			' 
			' radLabel2
			' 
			Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel2.Location = New System.Drawing.Point(10, 116)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New System.Drawing.Size(106, 18)
			Me.radLabel2.TabIndex = 1
			Me.radLabel2.Text = "Change Orientation:"
			' 
			' radDropDownListChartType
			' 
			Me.radDropDownListChartType.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownListChartType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			radListDataItem3.Text = "Range"
			radListDataItem4.Text = "RangeBar"
			Me.radDropDownListChartType.Items.Add(radListDataItem3)
			Me.radDropDownListChartType.Items.Add(radListDataItem4)
			Me.radDropDownListChartType.Location = New System.Drawing.Point(10, 66)
			Me.radDropDownListChartType.Name = "radDropDownListChartType"
			Me.radDropDownListChartType.Size = New System.Drawing.Size(792, 20)
			Me.radDropDownListChartType.TabIndex = 2
			' 
			' orientationCheckBox
			' 
			Me.orientationCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.orientationCheckBox.Location = New System.Drawing.Point(10, 137)
			Me.orientationCheckBox.Name = "orientationCheckBox"
			Me.orientationCheckBox.Size = New System.Drawing.Size(72, 18)
			Me.orientationCheckBox.TabIndex = 4
			Me.orientationCheckBox.Text = "Horizontal"
			' 
			' radCheckBoxSpline
			' 
			Me.radCheckBoxSpline.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxSpline.Enabled = False
			Me.radCheckBoxSpline.Location = New System.Drawing.Point(10, 92)
			Me.radCheckBoxSpline.Name = "radCheckBoxSpline"
			Me.radCheckBoxSpline.Size = New System.Drawing.Size(51, 18)
			Me.radCheckBoxSpline.TabIndex = 5
			Me.radCheckBoxSpline.Text = "Spline"
			' 
			' Form1
			' 
			Me.AutoScrollMinSize = New System.Drawing.Size(550, 320)
			Me.Controls.Add(Me.radChartView1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1881, 1096)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			Me.Controls.SetChildIndex(Me.radChartView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radDropDownListChartType, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.orientationCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxSpline, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private orientationCheckBox As Telerik.WinControls.UI.RadCheckBox
		Private radDropDownListChartType As Telerik.WinControls.UI.RadDropDownList
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radCheckBoxSpline As Telerik.WinControls.UI.RadCheckBox
	End Class
End Namespace
