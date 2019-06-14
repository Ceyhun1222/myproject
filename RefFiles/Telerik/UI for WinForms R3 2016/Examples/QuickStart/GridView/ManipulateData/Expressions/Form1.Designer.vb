Namespace Telerik.Examples.WinControls.GridView.ManipulateData.Expressions
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
			Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
			Me.radGroupPreDef = New Telerik.WinControls.UI.RadGroupBox()
			Me.radGroupNumericText = New Telerik.WinControls.UI.RadGroupBox()
			Me.radRadioAverage = New Telerik.WinControls.UI.RadRadioButton()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radRadioNumExpr2 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioNumExpr1 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radGroupCheckBox = New Telerik.WinControls.UI.RadGroupBox()
			Me.radLabel5 = New Telerik.WinControls.UI.RadLabel()
			Me.radRadioCheckExpr2 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioCheckExpr1 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radButtonExprEditor = New Telerik.WinControls.UI.RadButton()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupPreDef, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupPreDef.SuspendLayout()
			CType(Me.radGroupNumericText, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupNumericText.SuspendLayout()
			CType(Me.radRadioAverage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioNumExpr2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioNumExpr1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupCheckBox, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupCheckBox.SuspendLayout()
			CType(Me.radLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioCheckExpr2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioCheckExpr1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonExprEditor, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupPreDef)
			Me.settingsPanel.Controls.Add(Me.radButtonExprEditor)
			Me.settingsPanel.Location = New Point(751, 1)
			Me.settingsPanel.Size = New Size(230, 806)
			Me.settingsPanel.ThemeName = "ControlDefault"
			Me.settingsPanel.Controls.SetChildIndex(Me.radButtonExprEditor, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupPreDef, 0)
			' 
			' radGridView1
			' 
			Me.radGridView1.BackColor = Color.FromArgb((CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))))
			Me.radGridView1.ForeColor = Color.Black
			Me.radGridView1.Location = New Point(0, 0)
			' 
			' radGridView1
			' 
			Me.radGridView1.MasterTemplate.AllowColumnChooser = False
			Me.radGridView1.MasterTemplate.AutoGenerateColumns = False
			Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
			Me.radGridView1.MasterTemplate.Caption = Nothing
			Me.radGridView1.MasterTemplate.MultiSelect = True
			Me.radGridView1.MasterTemplate.ShowGroupedColumns = True
			Me.radGridView1.Name = "radGridView1"
			Me.radGridView1.Size = New Size(580, 176)
			Me.radGridView1.TabIndex = 2
			Me.radGridView1.Text = "radGridView1"
			Me.radGridView1.ThemeName = "Telerik"
			' 
			' radGroupPreDef
			' 
			Me.radGroupPreDef.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupPreDef.Anchor = AnchorStyles.Top
			Me.radGroupPreDef.Controls.Add(Me.radGroupNumericText)
			Me.radGroupPreDef.Controls.Add(Me.radGroupCheckBox)
			Me.radGroupPreDef.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupPreDef.HeaderText = "Predefined Expressions"
			Me.radGroupPreDef.Location = New Point(10, 6)
			Me.radGroupPreDef.Name = "radGroupPreDef"
			' 
			' 
			' 
			Me.radGroupPreDef.RootElement.Padding = New Padding(2, 18, 2, 2)
			Me.radGroupPreDef.Size = New Size(210, 264)
			Me.radGroupPreDef.TabIndex = 0
			Me.radGroupPreDef.Text = "Predefined Expressions"
			' 
			' radGroupNumericText
			' 
			Me.radGroupNumericText.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupNumericText.Controls.Add(Me.radRadioAverage)
			Me.radGroupNumericText.Controls.Add(Me.radLabel3)
			Me.radGroupNumericText.Controls.Add(Me.radRadioNumExpr2)
			Me.radGroupNumericText.Controls.Add(Me.radRadioNumExpr1)
			Me.radGroupNumericText.HeaderText = "NumericText Column"
			Me.radGroupNumericText.Location = New Point(10, 24)
			Me.radGroupNumericText.Name = "radGroupNumericText"
			Me.radGroupNumericText.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupNumericText.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupNumericText.Size = New Size(190, 116)
			Me.radGroupNumericText.TabIndex = 1
			Me.radGroupNumericText.Text = "NumericText Column"
			' 
			' radRadioAverage
			' 
			Me.radRadioAverage.Location = New Point(6, 91)
			Me.radRadioAverage.Name = "radRadioAverage"
			Me.radRadioAverage.Size = New Size(87, 18)
			Me.radRadioAverage.TabIndex = 2
			Me.radRadioAverage.Text = "Avg(Decimal)"
			' 
			' radLabel3
			' 
			Me.radLabel3.ForeColor = Color.Black
			Me.radLabel3.Location = New Point(6, 27)
			Me.radLabel3.Name = "radLabel3"
			Me.radLabel3.Size = New Size(102, 18)
			Me.radLabel3.TabIndex = 4
			Me.radLabel3.Text = "Choose expression:"
			' 
			' radRadioNumExpr2
			' 
			Me.radRadioNumExpr2.Location = New Point(6, 69)
			Me.radRadioNumExpr2.Name = "radRadioNumExpr2"
			Me.radRadioNumExpr2.Size = New Size(86, 18)
			Me.radRadioNumExpr2.TabIndex = 5
			Me.radRadioNumExpr2.Text = "Decimal + 15"
			' 
			' radRadioNumExpr1
			' 
			Me.radRadioNumExpr1.Location = New Point(6, 47)
			Me.radRadioNumExpr1.Name = "radRadioNumExpr1"
			Me.radRadioNumExpr1.Size = New Size(101, 18)
			Me.radRadioNumExpr1.TabIndex = 3
			Me.radRadioNumExpr1.Text = "(Decimal - 3) * 2"
			' 
			' radGroupCheckBox
			' 
			Me.radGroupCheckBox.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupCheckBox.Controls.Add(Me.radLabel5)
			Me.radGroupCheckBox.Controls.Add(Me.radRadioCheckExpr2)
			Me.radGroupCheckBox.Controls.Add(Me.radRadioCheckExpr1)
			Me.radGroupCheckBox.HeaderText = "CheckBox Column"
			Me.radGroupCheckBox.Location = New Point(10, 145)
			Me.radGroupCheckBox.Name = "radGroupCheckBox"
			Me.radGroupCheckBox.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupCheckBox.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupCheckBox.Size = New Size(190, 109)
			Me.radGroupCheckBox.TabIndex = 1
			Me.radGroupCheckBox.Text = "CheckBox Column"
			' 
			' radLabel5
			' 
			Me.radLabel5.Location = New Point(6, 26)
			Me.radLabel5.Name = "radLabel5"
			Me.radLabel5.Size = New Size(102, 18)
			Me.radLabel5.TabIndex = 4
			Me.radLabel5.Text = "Choose expression:"
			' 
			' radRadioCheckExpr2
			' 
			Me.radRadioCheckExpr2.Location = New Point(7, 68)
			Me.radRadioCheckExpr2.Name = "radRadioCheckExpr2"
			Me.radRadioCheckExpr2.Size = New Size(15, 15)
			Me.radRadioCheckExpr2.TabIndex = 5
			' 
			' radRadioCheckExpr1
			' 
			Me.radRadioCheckExpr1.Location = New Point(7, 46)
			Me.radRadioCheckExpr1.Name = "radRadioCheckExpr1"
			Me.radRadioCheckExpr1.Size = New Size(93, 18)
			Me.radRadioCheckExpr1.TabIndex = 3
			Me.radRadioCheckExpr1.Text = "Decimal < 600"
			' 
			' radButtonExprEditor
			' 
			Me.radButtonExprEditor.Location = New Point(0, 300)
			Me.radButtonExprEditor.Name = "radButtonExprEditor"
			Me.radButtonExprEditor.Size = New Size(120, 28)
			Me.radButtonExprEditor.TabIndex = 1
			Me.radButtonExprEditor.Text = "Show Expression Editor"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radGridView1)
			Me.Name = "Form1"
			Me.Size = New Size(1029, 602)

			Me.Controls.SetChildIndex(Me.radGridView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupPreDef, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupPreDef.ResumeLayout(False)
			CType(Me.radGroupNumericText, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupNumericText.ResumeLayout(False)
			Me.radGroupNumericText.PerformLayout()
			CType(Me.radRadioAverage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioNumExpr2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioNumExpr1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupCheckBox, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupCheckBox.ResumeLayout(False)
			Me.radGroupCheckBox.PerformLayout()
			CType(Me.radLabel5, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioCheckExpr2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioCheckExpr1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonExprEditor, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region
		Private radGridView1 As Telerik.WinControls.UI.RadGridView
		Private radGroupPreDef As Telerik.WinControls.UI.RadGroupBox
		Private radRadioNumExpr1 As Telerik.WinControls.UI.RadRadioButton
		Private radLabel3 As Telerik.WinControls.UI.RadLabel
		Private radRadioNumExpr2 As Telerik.WinControls.UI.RadRadioButton
		Private radGroupNumericText As Telerik.WinControls.UI.RadGroupBox
		Private radGroupCheckBox As Telerik.WinControls.UI.RadGroupBox
		Private radLabel5 As Telerik.WinControls.UI.RadLabel
		Private radRadioCheckExpr2 As Telerik.WinControls.UI.RadRadioButton
		Private radRadioCheckExpr1 As Telerik.WinControls.UI.RadRadioButton
		Private radRadioAverage As Telerik.WinControls.UI.RadRadioButton
		Private radButtonExprEditor As Telerik.WinControls.UI.RadButton
	End Class
End Namespace