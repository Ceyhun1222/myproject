Namespace Telerik.Examples.WinControls.ChartView.TitleAndLegend
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
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radCheckBoxFlipText = New Telerik.WinControls.UI.RadCheckBox()
            Me.radTextBoxControlTitle = New Telerik.WinControls.UI.RadTextBoxControl()
            Me.radLabelTitle = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownListTitlePosition = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabelTitleOrientation = New Telerik.WinControls.UI.RadLabel()
            Me.radRadioButtonHorizontalTitle = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButtonVerticalTitle = New Telerik.WinControls.UI.RadRadioButton()
            Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radButtonEditShape = New Telerik.WinControls.UI.RadButton()
            Me.radDropDownListShapes = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabelMarkerShape = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel5 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownListLegendPosition = New Telerik.WinControls.UI.RadDropDownList()
            Me.radSpinEditorLegendX = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radRadioButtonVerticalLegend = New Telerik.WinControls.UI.RadRadioButton()
            Me.radSpinEditorLegendY = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radRadioButtonHorizontalLegend = New Telerik.WinControls.UI.RadRadioButton()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            CType(Me.radCheckBoxFlipText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTextBoxControlTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownListTitlePosition, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelTitleOrientation, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radRadioButtonHorizontalTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radRadioButtonVerticalTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox2.SuspendLayout()
            CType(Me.radButtonEditShape, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownListShapes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelMarkerShape, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownListLegendPosition, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorLegendX, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radRadioButtonVerticalLegend, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorLegendY, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radRadioButtonHorizontalLegend, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBox2)
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(835, 45)
            Me.settingsPanel.Size = New System.Drawing.Size(244, 682)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox1.Controls.Add(Me.radCheckBoxFlipText)
            Me.radGroupBox1.Controls.Add(Me.radTextBoxControlTitle)
            Me.radGroupBox1.Controls.Add(Me.radLabelTitle)
            Me.radGroupBox1.Controls.Add(Me.radLabel1)
            Me.radGroupBox1.Controls.Add(Me.radDropDownListTitlePosition)
            Me.radGroupBox1.Controls.Add(Me.radLabelTitleOrientation)
            Me.radGroupBox1.Controls.Add(Me.radRadioButtonHorizontalTitle)
            Me.radGroupBox1.Controls.Add(Me.radRadioButtonVerticalTitle)
            Me.radGroupBox1.HeaderText = "Title"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 3)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(224, 194)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "Title"
            ' 
            ' radCheckBoxFlipText
            ' 
            Me.radCheckBoxFlipText.Location = New System.Drawing.Point(115, 144)
            Me.radCheckBoxFlipText.Name = "radCheckBoxFlipText"
            Me.radCheckBoxFlipText.Size = New System.Drawing.Size(60, 18)
            Me.radCheckBoxFlipText.TabIndex = 13
            Me.radCheckBoxFlipText.Tag = "NotTouch"
            Me.radCheckBoxFlipText.Text = "Flip text"
            ' 
            ' radTextBoxControlTitle
            ' 
            Me.radTextBoxControlTitle.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radTextBoxControlTitle.Location = New System.Drawing.Point(5, 44)
            Me.radTextBoxControlTitle.Name = "radTextBoxControlTitle"
            Me.radTextBoxControlTitle.NullText = "Chart title"
            Me.radTextBoxControlTitle.Size = New System.Drawing.Size(214, 20)
            Me.radTextBoxControlTitle.TabIndex = 12
            ' 
            ' radLabelTitle
            ' 
            Me.radLabelTitle.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelTitle.Location = New System.Drawing.Point(5, 20)
            Me.radLabelTitle.Name = "radLabelTitle"
            Me.radLabelTitle.Size = New System.Drawing.Size(26, 18)
            Me.radLabelTitle.TabIndex = 8
            Me.radLabelTitle.Text = "Title"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(5, 70)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(45, 18)
            Me.radLabel1.TabIndex = 8
            Me.radLabel1.Text = "Position"
            ' 
            ' radDropDownListTitlePosition
            ' 
            Me.radDropDownListTitlePosition.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListTitlePosition.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListTitlePosition.Location = New System.Drawing.Point(5, 94)
            Me.radDropDownListTitlePosition.Name = "radDropDownListTitlePosition"
            Me.radDropDownListTitlePosition.Size = New System.Drawing.Size(214, 20)
            Me.radDropDownListTitlePosition.TabIndex = 7
            Me.radDropDownListTitlePosition.Text = "Title position"
            ' 
            ' radLabelTitleOrientation
            ' 
            Me.radLabelTitleOrientation.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelTitleOrientation.Location = New System.Drawing.Point(5, 120)
            Me.radLabelTitleOrientation.Name = "radLabelTitleOrientation"
            Me.radLabelTitleOrientation.Size = New System.Drawing.Size(84, 18)
            Me.radLabelTitleOrientation.TabIndex = 9
            Me.radLabelTitleOrientation.Text = "Title orientation"
            ' 
            ' radRadioButtonHorizontalTitle
            ' 
            Me.radRadioButtonHorizontalTitle.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadioButtonHorizontalTitle.Location = New System.Drawing.Point(5, 144)
            Me.radRadioButtonHorizontalTitle.Name = "radRadioButtonHorizontalTitle"
            Me.radRadioButtonHorizontalTitle.Size = New System.Drawing.Size(72, 18)
            Me.radRadioButtonHorizontalTitle.TabIndex = 10
            Me.radRadioButtonHorizontalTitle.TabStop = True
            Me.radRadioButtonHorizontalTitle.Text = "Horizontal"
            Me.radRadioButtonHorizontalTitle.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radRadioButtonVerticalTitle
            ' 
            Me.radRadioButtonVerticalTitle.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadioButtonVerticalTitle.Location = New System.Drawing.Point(5, 168)
            Me.radRadioButtonVerticalTitle.Name = "radRadioButtonVerticalTitle"
            Me.radRadioButtonVerticalTitle.Size = New System.Drawing.Size(57, 18)
            Me.radRadioButtonVerticalTitle.TabIndex = 11
            Me.radRadioButtonVerticalTitle.Text = "Vertical"
            ' 
            ' radGroupBox2
            ' 
            Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox2.Controls.Add(Me.radButtonEditShape)
            Me.radGroupBox2.Controls.Add(Me.radDropDownListShapes)
            Me.radGroupBox2.Controls.Add(Me.radLabelMarkerShape)
            Me.radGroupBox2.Controls.Add(Me.radLabel4)
            Me.radGroupBox2.Controls.Add(Me.radLabel3)
            Me.radGroupBox2.Controls.Add(Me.radLabel5)
            Me.radGroupBox2.Controls.Add(Me.radLabel2)
            Me.radGroupBox2.Controls.Add(Me.radDropDownListLegendPosition)
            Me.radGroupBox2.Controls.Add(Me.radSpinEditorLegendX)
            Me.radGroupBox2.Controls.Add(Me.radRadioButtonVerticalLegend)
            Me.radGroupBox2.Controls.Add(Me.radSpinEditorLegendY)
            Me.radGroupBox2.Controls.Add(Me.radRadioButtonHorizontalLegend)
            Me.radGroupBox2.HeaderText = "Legend"
            Me.radGroupBox2.Location = New System.Drawing.Point(10, 203)
            Me.radGroupBox2.Name = "radGroupBox2"
            Me.radGroupBox2.Size = New System.Drawing.Size(224, 265)
            Me.radGroupBox2.TabIndex = 8
            Me.radGroupBox2.Text = "Legend"
            ' 
            ' radButtonEditShape
            ' 
            Me.radButtonEditShape.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radButtonEditShape.Location = New System.Drawing.Point(5, 229)
            Me.radButtonEditShape.Name = "radButtonEditShape"
            Me.radButtonEditShape.Size = New System.Drawing.Size(214, 24)
            Me.radButtonEditShape.TabIndex = 10
            Me.radButtonEditShape.Text = "Edit shape"
            ' 
            ' radDropDownListShapes
            ' 
            Me.radDropDownListShapes.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListShapes.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListShapes.Location = New System.Drawing.Point(5, 203)
            Me.radDropDownListShapes.Name = "radDropDownListShapes"
            Me.radDropDownListShapes.Size = New System.Drawing.Size(214, 20)
            Me.radDropDownListShapes.TabIndex = 9
            ' 
            ' radLabelMarkerShape
            ' 
            Me.radLabelMarkerShape.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelMarkerShape.Location = New System.Drawing.Point(5, 179)
            Me.radLabelMarkerShape.Name = "radLabelMarkerShape"
            Me.radLabelMarkerShape.Size = New System.Drawing.Size(78, 18)
            Me.radLabelMarkerShape.TabIndex = 8
            Me.radLabelMarkerShape.Text = "Markers shape"
            ' 
            ' radLabel4
            ' 
            Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel4.Location = New System.Drawing.Point(117, 73)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(11, 18)
            Me.radLabel4.TabIndex = 7
            Me.radLabel4.Tag = "NotTouch"
            Me.radLabel4.Text = "Y"
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel3.Location = New System.Drawing.Point(32, 73)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(12, 18)
            Me.radLabel3.TabIndex = 7
            Me.radLabel3.Tag = "NotTouch"
            Me.radLabel3.Text = "X"
            ' 
            ' radLabel5
            ' 
            Me.radLabel5.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel5.Location = New System.Drawing.Point(5, 107)
            Me.radLabel5.Name = "radLabel5"
            Me.radLabel5.Size = New System.Drawing.Size(91, 18)
            Me.radLabel5.TabIndex = 1
            Me.radLabel5.Text = "Items orientation"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(5, 21)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(45, 18)
            Me.radLabel2.TabIndex = 1
            Me.radLabel2.Text = "Position"
            ' 
            ' radDropDownListLegendPosition
            ' 
            Me.radDropDownListLegendPosition.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListLegendPosition.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListLegendPosition.Location = New System.Drawing.Point(5, 45)
            Me.radDropDownListLegendPosition.Name = "radDropDownListLegendPosition"
            Me.radDropDownListLegendPosition.Size = New System.Drawing.Size(214, 20)
            Me.radDropDownListLegendPosition.TabIndex = 1
            Me.radDropDownListLegendPosition.Text = "Legend position"
            ' 
            ' radSpinEditorLegendX
            ' 
            Me.radSpinEditorLegendX.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorLegendX.Location = New System.Drawing.Point(51, 71)
            Me.radSpinEditorLegendX.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.radSpinEditorLegendX.Name = "radSpinEditorLegendX"
            Me.radSpinEditorLegendX.Size = New System.Drawing.Size(57, 20)
            Me.radSpinEditorLegendX.TabIndex = 2
            Me.radSpinEditorLegendX.TabStop = False
            Me.radSpinEditorLegendX.Tag = "NotTouch"
            ' 
            ' radRadioButtonVerticalLegend
            ' 
            Me.radRadioButtonVerticalLegend.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadioButtonVerticalLegend.Location = New System.Drawing.Point(5, 155)
            Me.radRadioButtonVerticalLegend.Name = "radRadioButtonVerticalLegend"
            Me.radRadioButtonVerticalLegend.Size = New System.Drawing.Size(57, 18)
            Me.radRadioButtonVerticalLegend.TabIndex = 6
            Me.radRadioButtonVerticalLegend.TabStop = True
            Me.radRadioButtonVerticalLegend.Text = "Vertical"
            Me.radRadioButtonVerticalLegend.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radSpinEditorLegendY
            ' 
            Me.radSpinEditorLegendY.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorLegendY.Location = New System.Drawing.Point(135, 71)
            Me.radSpinEditorLegendY.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.radSpinEditorLegendY.Name = "radSpinEditorLegendY"
            Me.radSpinEditorLegendY.Size = New System.Drawing.Size(57, 20)
            Me.radSpinEditorLegendY.TabIndex = 2
            Me.radSpinEditorLegendY.TabStop = False
            Me.radSpinEditorLegendY.Tag = "NotTouch"
            ' 
            ' radRadioButtonHorizontalLegend
            ' 
            Me.radRadioButtonHorizontalLegend.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadioButtonHorizontalLegend.Location = New System.Drawing.Point(5, 131)
            Me.radRadioButtonHorizontalLegend.Name = "radRadioButtonHorizontalLegend"
            Me.radRadioButtonHorizontalLegend.Size = New System.Drawing.Size(72, 18)
            Me.radRadioButtonHorizontalLegend.TabIndex = 6
            Me.radRadioButtonHorizontalLegend.Text = "Horizontal"
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaDesign = cartesianArea1
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.MinimumSize = New System.Drawing.Size(460, 350)
            Me.radChartView1.Name = "radChartView1"
            ' 
            ' 
            ' 
            Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(460, 350)
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.Size = New System.Drawing.Size(1158, 695)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            Me.radChartView1.Title = "Chart title"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScrollMinSize = New System.Drawing.Size(460, 350)
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
            CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.radGroupBox1.PerformLayout()
            CType(Me.radCheckBoxFlipText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTextBoxControlTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownListTitlePosition, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelTitleOrientation, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radRadioButtonHorizontalTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radRadioButtonVerticalTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox2.ResumeLayout(False)
            Me.radGroupBox2.PerformLayout()
            CType(Me.radButtonEditShape, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownListShapes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelMarkerShape, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownListLegendPosition, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorLegendX, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radRadioButtonVerticalLegend, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorLegendY, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radRadioButtonHorizontalLegend, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
		Private radLabel4 As Telerik.WinControls.UI.RadLabel
		Private radLabel3 As Telerik.WinControls.UI.RadLabel
		Private radLabel5 As Telerik.WinControls.UI.RadLabel
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radDropDownListLegendPosition As Telerik.WinControls.UI.RadDropDownList
		Private radSpinEditorLegendX As Telerik.WinControls.UI.RadSpinEditor
		Private radRadioButtonVerticalLegend As Telerik.WinControls.UI.RadRadioButton
		Private radSpinEditorLegendY As Telerik.WinControls.UI.RadSpinEditor
		Private radRadioButtonHorizontalLegend As Telerik.WinControls.UI.RadRadioButton
		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radDropDownListTitlePosition As Telerik.WinControls.UI.RadDropDownList
		Private radLabelTitleOrientation As Telerik.WinControls.UI.RadLabel
		Private radRadioButtonHorizontalTitle As Telerik.WinControls.UI.RadRadioButton
		Private radRadioButtonVerticalTitle As Telerik.WinControls.UI.RadRadioButton
		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radDropDownListShapes As Telerik.WinControls.UI.RadDropDownList
		Private radLabelMarkerShape As Telerik.WinControls.UI.RadLabel
		Private radCheckBoxFlipText As Telerik.WinControls.UI.RadCheckBox
		Private radTextBoxControlTitle As Telerik.WinControls.UI.RadTextBoxControl
		Private radLabelTitle As Telerik.WinControls.UI.RadLabel
		Private radButtonEditShape As Telerik.WinControls.UI.RadButton
	End Class
End Namespace