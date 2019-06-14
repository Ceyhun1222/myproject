
Namespace Telerik.Examples.WinControls.VirtualGrid.Validation
	Partial Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
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
			Me.radVirtualGrid1 = New Telerik.WinControls.UI.RadVirtualGrid()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorSecondCondition = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radSpinEditorFirstCondition = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radDropDownListSecondCondition = New Telerik.WinControls.UI.RadDropDownList()
			Me.radDropDownListFirstCondition = New Telerik.WinControls.UI.RadDropDownList()
			Me.radDropDownListColumnName = New Telerik.WinControls.UI.RadDropDownList()
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radVirtualGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorSecondCondition, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorFirstCondition, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radDropDownListSecondCondition, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radDropDownListFirstCondition, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radDropDownListColumnName, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Size = New System.Drawing.Size(230, 285)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			' 
			' themePanel
			' 
			Me.themePanel.Location = New System.Drawing.Point(0, 312)
			' 
			' radVirtualGrid1
			' 
			Me.radVirtualGrid1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radVirtualGrid1.Location = New System.Drawing.Point(0, 0)
			Me.radVirtualGrid1.MasterViewInfo.RowCount = 1000
			Me.radVirtualGrid1.Name = "radVirtualGrid1"
			Me.radVirtualGrid1.Size = New System.Drawing.Size(1871, 1086)
			Me.radVirtualGrid1.TabIndex = 0
			Me.radVirtualGrid1.Text = "radVirtualGrid1"
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox1.Controls.Add(Me.radLabel1)
			Me.radGroupBox1.Controls.Add(Me.radSpinEditorSecondCondition)
			Me.radGroupBox1.Controls.Add(Me.radSpinEditorFirstCondition)
			Me.radGroupBox1.Controls.Add(Me.radDropDownListSecondCondition)
			Me.radGroupBox1.Controls.Add(Me.radDropDownListFirstCondition)
			Me.radGroupBox1.Controls.Add(Me.radDropDownListColumnName)
			Me.radGroupBox1.HeaderText = "Column restrictions:"
			Me.radGroupBox1.Location = New System.Drawing.Point(10, 33)
			Me.radGroupBox1.Name = "radGroupBox1"
			Me.radGroupBox1.Size = New System.Drawing.Size(210, 180)
			Me.radGroupBox1.TabIndex = 1
			Me.radGroupBox1.Text = "Column restrictions:"
			Me.radGroupBox1.UseMnemonic = False
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel1.Location = New System.Drawing.Point(5, 101)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New System.Drawing.Size(24, 18)
			Me.radLabel1.TabIndex = 2
			Me.radLabel1.Text = "and"
			' 
			' radSpinEditorSecondCondition
			' 
			Me.radSpinEditorSecondCondition.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorSecondCondition.Location = New System.Drawing.Point(5, 151)
			Me.radSpinEditorSecondCondition.Name = "radSpinEditorSecondCondition"
			Me.radSpinEditorSecondCondition.Size = New System.Drawing.Size(200, 20)
			Me.radSpinEditorSecondCondition.TabIndex = 1
			Me.radSpinEditorSecondCondition.TabStop = False
			' 
			' radSpinEditorFirstCondition
			' 
			Me.radSpinEditorFirstCondition.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorFirstCondition.Location = New System.Drawing.Point(5, 75)
			Me.radSpinEditorFirstCondition.Name = "radSpinEditorFirstCondition"
			Me.radSpinEditorFirstCondition.Size = New System.Drawing.Size(200, 20)
			Me.radSpinEditorFirstCondition.TabIndex = 1
			Me.radSpinEditorFirstCondition.TabStop = False
			' 
			' radDropDownListSecondCondition
			' 
			Me.radDropDownListSecondCondition.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownListSecondCondition.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownListSecondCondition.Location = New System.Drawing.Point(5, 125)
			Me.radDropDownListSecondCondition.Name = "radDropDownListSecondCondition"
			Me.radDropDownListSecondCondition.Size = New System.Drawing.Size(200, 20)
			Me.radDropDownListSecondCondition.TabIndex = 0
			' 
			' radDropDownListFirstCondition
			' 
			Me.radDropDownListFirstCondition.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownListFirstCondition.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownListFirstCondition.Location = New System.Drawing.Point(5, 48)
			Me.radDropDownListFirstCondition.Name = "radDropDownListFirstCondition"
			Me.radDropDownListFirstCondition.Size = New System.Drawing.Size(200, 20)
			Me.radDropDownListFirstCondition.TabIndex = 0
			' 
			' radDropDownListColumnName
			' 
			Me.radDropDownListColumnName.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownListColumnName.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownListColumnName.Location = New System.Drawing.Point(5, 22)
			Me.radDropDownListColumnName.Name = "radDropDownListColumnName"
			Me.radDropDownListColumnName.Size = New System.Drawing.Size(200, 20)
			Me.radDropDownListColumnName.TabIndex = 0
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.radVirtualGrid1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1881, 1096)
			Me.Controls.SetChildIndex(Me.radVirtualGrid1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radVirtualGrid1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorSecondCondition, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorFirstCondition, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radDropDownListSecondCondition, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radDropDownListFirstCondition, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radDropDownListColumnName, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

        Private WithEvents radVirtualGrid1 As Telerik.WinControls.UI.RadVirtualGrid
		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radSpinEditorSecondCondition As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorFirstCondition As Telerik.WinControls.UI.RadSpinEditor
		Private radDropDownListSecondCondition As Telerik.WinControls.UI.RadDropDownList
		Private radDropDownListFirstCondition As Telerik.WinControls.UI.RadDropDownList
		Private radDropDownListColumnName As Telerik.WinControls.UI.RadDropDownList
	End Class
End Namespace