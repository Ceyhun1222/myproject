Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Bar
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
            Dim cartesianArea1 As New Telerik.WinControls.UI.CartesianArea()
            Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
            Me.showLabelsCheckBox = New Telerik.WinControls.UI.RadCheckBox()
            Me.orientationCheckBox = New Telerik.WinControls.UI.RadCheckBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.showLabelsCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.orientationCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.orientationCheckBox)
            Me.settingsPanel.Controls.Add(Me.showLabelsCheckBox)
            Me.settingsPanel.Controls.Add(Me.radDropDownList1)
            Me.settingsPanel.Controls.Add(Me.radLabel2)
            Me.settingsPanel.Controls.Add(Me.radLabel1)
            Me.settingsPanel.Location = New System.Drawing.Point(834, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(812, 883)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabel2, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radDropDownList1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.showLabelsCheckBox, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.orientationCheckBox, 0)
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaDesign = cartesianArea1
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
            Me.radChartView1.Size = New System.Drawing.Size(1158, 695)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(10, 45)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(85, 18)
            Me.radLabel1.TabIndex = 0
            Me.radLabel1.Text = "Combine mode:"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(10, 130)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(106, 18)
            Me.radLabel2.TabIndex = 1
            Me.radLabel2.Text = "Change Orientation:"
            ' 
            ' radDropDownList1
            ' 
            Me.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            radListDataItem1.Text = "Cluster"
            radListDataItem1.TextWrap = True
            radListDataItem2.Text = "Stack"
            radListDataItem2.TextWrap = True
            radListDataItem3.Text = "Stack100"
            radListDataItem3.TextWrap = True
            Me.radDropDownList1.Items.Add(radListDataItem1)
            Me.radDropDownList1.Items.Add(radListDataItem2)
            Me.radDropDownList1.Items.Add(radListDataItem3)
            Me.radDropDownList1.Location = New System.Drawing.Point(10, 66)
            Me.radDropDownList1.Name = "radDropDownList1"
            Me.radDropDownList1.Size = New System.Drawing.Size(792, 20)
            Me.radDropDownList1.TabIndex = 2
            ' 
            ' showLabelsCheckBox
            ' 
            Me.showLabelsCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.showLabelsCheckBox.Location = New System.Drawing.Point(10, 92)
            Me.showLabelsCheckBox.Name = "showLabelsCheckBox"
            Me.showLabelsCheckBox.Size = New System.Drawing.Size(82, 18)
            Me.showLabelsCheckBox.TabIndex = 3
            Me.showLabelsCheckBox.Text = "Show Labels"
            ' 
            ' orientationCheckBox
            ' 
            Me.orientationCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.orientationCheckBox.Location = New System.Drawing.Point(10, 151)
            Me.orientationCheckBox.Name = "orientationCheckBox"
            Me.orientationCheckBox.Size = New System.Drawing.Size(72, 18)
            Me.orientationCheckBox.TabIndex = 4
            Me.orientationCheckBox.Text = "Horizontal"
            ' 
            ' Form1
            ' 
            Me.AutoScrollMinSize = New System.Drawing.Size(550, 320)
            Me.Controls.Add(Me.radChartView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1168, 705)
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
            CType(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.showLabelsCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.orientationCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private orientationCheckBox As Telerik.WinControls.UI.RadCheckBox
		Private showLabelsCheckBox As Telerik.WinControls.UI.RadCheckBox
		Private radDropDownList1 As Telerik.WinControls.UI.RadDropDownList
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace
