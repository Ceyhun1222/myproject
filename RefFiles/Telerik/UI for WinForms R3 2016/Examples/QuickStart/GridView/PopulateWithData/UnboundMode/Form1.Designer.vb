Namespace Telerik.Examples.WinControls.GridView.PopulateWithData.UnboundMode
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
			Me.radBtnRmRows = New Telerik.WinControls.UI.RadButton()
			Me.radBtnAddRow = New Telerik.WinControls.UI.RadButton()
			Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
			Me.radBtnRmCol = New Telerik.WinControls.UI.RadButton()
			Me.radComboBox1 = New Telerik.WinControls.UI.RadDropDownList()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radBtnAddCol = New Telerik.WinControls.UI.RadButton()
			Me.radCheckShowNewRow = New Telerik.WinControls.UI.RadCheckBox()
			Me.radGroupCols = New Telerik.WinControls.UI.RadGroupBox()
			Me.radGroupAddCol = New Telerik.WinControls.UI.RadGroupBox()
			Me.radGroupRmCol = New Telerik.WinControls.UI.RadGroupBox()
			Me.radComboColumnsRemove = New Telerik.WinControls.UI.RadDropDownList()
			Me.radGroupRows = New Telerik.WinControls.UI.RadGroupBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnRmRows, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnAddRow, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnRmCol, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnAddCol, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckShowNewRow, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupCols, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupCols.SuspendLayout()
			CType(Me.radGroupAddCol, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupAddCol.SuspendLayout()
			CType(Me.radGroupRmCol, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupRmCol.SuspendLayout()
			CType(Me.radComboColumnsRemove, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupRows, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupRows.SuspendLayout()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupCols)
			Me.settingsPanel.Controls.Add(Me.radGroupRows)
			Me.settingsPanel.ForeColor = Color.Black
			Me.settingsPanel.Location = New Point(1023, 1)
			' 
			' 
			' 
			Me.settingsPanel.RootElement.ForeColor = Color.Black
			Me.settingsPanel.Size = New Size(200, 818)
			Me.settingsPanel.ThemeName = "ControlDefault"
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupRows, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupCols, 0)
			' 
			' radGridView1
			' 
			Me.radGridView1.BackColor = Color.FromArgb((CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))))
			Me.radGridView1.Dock = DockStyle.Fill
			Me.radGridView1.ForeColor = Color.Black
			Me.radGridView1.Location = New Point(0, 0)
			' 
			' 
			' 
			Me.radGridView1.MasterTemplate.AllowColumnChooser = False
			Me.radGridView1.MasterTemplate.AutoGenerateColumns = False
			Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
			Me.radGridView1.MasterTemplate.Caption = Nothing
			Me.radGridView1.MasterTemplate.ShowGroupedColumns = True
			Me.radGridView1.MultiSelect = True
			Me.radGridView1.Name = "radGridView1"
			' 
			' 
			' 
			Me.radGridView1.RootElement.ForeColor = Color.Black
			Me.radGridView1.Size = New Size(1224, 820)
			Me.radGridView1.TabIndex = 2
			Me.radGridView1.Text = "radGridView1"
			Me.radGridView1.ThemeName = "Telerik"

			' 
			' radBtnRmRows
			' 
			Me.radBtnRmRows.Location = New Point(12, 58)
			Me.radBtnRmRows.Name = "radBtnRmRows"
			Me.radBtnRmRows.Size = New Size(156, 23)
			Me.radBtnRmRows.TabIndex = 6
			Me.radBtnRmRows.Text = "Remove Selected Rows"

			' 
			' radBtnAddRow
			' 
			Me.radBtnAddRow.Location = New Point(12, 29)
			Me.radBtnAddRow.Name = "radBtnAddRow"
			Me.radBtnAddRow.Size = New Size(156, 23)
			Me.radBtnAddRow.TabIndex = 5
			Me.radBtnAddRow.Text = "Add Row"

			' 
			' radLabel4
			' 
			Me.radLabel4.ForeColor = Color.FromArgb((CInt(Fix((CByte(0))))), (CInt(Fix((CByte(20))))), (CInt(Fix((CByte(107))))))
			Me.radLabel4.Location = New Point(15, 22)
			Me.radLabel4.Name = "radLabel4"
			Me.radLabel4.Size = New Size(37, 16)
			Me.radLabel4.TabIndex = 3
			Me.radLabel4.Text = "Index:"
			' 
			' radBtnRmCol
			' 
			Me.radBtnRmCol.Location = New Point(15, 48)
			Me.radBtnRmCol.Name = "radBtnRmCol"
			Me.radBtnRmCol.Size = New Size(130, 23)
			Me.radBtnRmCol.TabIndex = 3
			Me.radBtnRmCol.Text = "Remove Column"

			' 
			' radComboBox1
			' 
			Me.radComboBox1.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
			Me.radComboBox1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radComboBox1.Location = New Point(54, 19)
			Me.radComboBox1.Name = "radComboBox1"
			' 
			' 
			' 
			Me.radComboBox1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboBox1.Size = New Size(91, 20)
			Me.radComboBox1.TabIndex = 0
			Me.radComboBox1.TabStop = False
			' 
			' radLabel2
			' 
			Me.radLabel2.ForeColor = Color.FromArgb((CInt(Fix((CByte(0))))), (CInt(Fix((CByte(20))))), (CInt(Fix((CByte(107))))))
			Me.radLabel2.Location = New Point(15, 22)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New Size(35, 16)
			Me.radLabel2.TabIndex = 2
			Me.radLabel2.Text = "Type:"
			' 
			' radBtnAddCol
			' 
			Me.radBtnAddCol.Location = New Point(15, 56)
			Me.radBtnAddCol.Name = "radBtnAddCol"
			Me.radBtnAddCol.Size = New Size(130, 23)
			Me.radBtnAddCol.TabIndex = 1
			Me.radBtnAddCol.Text = "Add Column"
			' 
			' radCheckShowNewRow
			' 
			Me.radCheckShowNewRow.AutoSize = True
			Me.radCheckShowNewRow.Location = New Point(12, 93)
			Me.radCheckShowNewRow.Name = "radCheckShowNewRow"
			Me.radCheckShowNewRow.Size = New Size(129, 14)
			Me.radCheckShowNewRow.TabIndex = 7
			Me.radCheckShowNewRow.Text = "Show ""Add New Row"""
			Me.radCheckShowNewRow.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' radGroupCols
			' 
			Me.radGroupCols.Anchor = AnchorStyles.Top
			Me.radGroupCols.Controls.Add(Me.radGroupAddCol)
			Me.radGroupCols.Controls.Add(Me.radGroupRmCol)
			Me.radGroupCols.FooterImageIndex = -1
			Me.radGroupCols.FooterImageKey = ""
			Me.radGroupCols.HeaderImageIndex = -1
			Me.radGroupCols.HeaderImageKey = ""
			Me.radGroupCols.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupCols.HeaderText = "Columns"
			Me.radGroupCols.Location = New Point(10, 6)
			Me.radGroupCols.Name = "radGroupCols"
			Me.radGroupCols.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupCols.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupCols.Size = New Size(180, 222)
			Me.radGroupCols.TabIndex = 8
			Me.radGroupCols.Text = "Columns"
			' 
			' radGroupAddCol
			' 
			Me.radGroupAddCol.Controls.Add(Me.radLabel2)
			Me.radGroupAddCol.Controls.Add(Me.radComboBox1)
			Me.radGroupAddCol.Controls.Add(Me.radBtnAddCol)
			Me.radGroupAddCol.FooterImageIndex = -1
			Me.radGroupAddCol.FooterImageKey = ""
			Me.radGroupAddCol.HeaderImageIndex = -1
			Me.radGroupAddCol.HeaderImageKey = ""
			Me.radGroupAddCol.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupAddCol.HeaderText = "Add"
			Me.radGroupAddCol.Location = New Point(10, 119)
			Me.radGroupAddCol.Name = "radGroupAddCol"
			Me.radGroupAddCol.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupAddCol.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupAddCol.Size = New Size(161, 91)
			Me.radGroupAddCol.TabIndex = 8
			Me.radGroupAddCol.Text = "Add"
			' 
			' radGroupRmCol
			' 
			Me.radGroupRmCol.Controls.Add(Me.radLabel4)
			Me.radGroupRmCol.Controls.Add(Me.radComboColumnsRemove)
			Me.radGroupRmCol.Controls.Add(Me.radBtnRmCol)
			Me.radGroupRmCol.FooterImageIndex = -1
			Me.radGroupRmCol.FooterImageKey = ""
			Me.radGroupRmCol.HeaderImageIndex = -1
			Me.radGroupRmCol.HeaderImageKey = ""
			Me.radGroupRmCol.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupRmCol.HeaderText = "Remove"
			Me.radGroupRmCol.Location = New Point(10, 22)
			Me.radGroupRmCol.Name = "radGroupRmCol"
			Me.radGroupRmCol.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupRmCol.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupRmCol.Size = New Size(161, 91)
			Me.radGroupRmCol.TabIndex = 8
			Me.radGroupRmCol.Text = "Remove"
			' 
			' radComboColumnsRemove
			' 
			Me.radComboColumnsRemove.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
			Me.radComboColumnsRemove.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radComboColumnsRemove.Location = New Point(56, 22)
			Me.radComboColumnsRemove.Name = "radComboColumnsRemove"
			' 
			' 
			' 
			Me.radComboColumnsRemove.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboColumnsRemove.Size = New Size(91, 20)
			Me.radComboColumnsRemove.TabIndex = 0
			Me.radComboColumnsRemove.TabStop = False
			' 
			' radGroupRows
			' 
			Me.radGroupRows.Anchor = AnchorStyles.Top
			Me.radGroupRows.Controls.Add(Me.radBtnAddRow)
			Me.radGroupRows.Controls.Add(Me.radBtnRmRows)
			Me.radGroupRows.Controls.Add(Me.radCheckShowNewRow)
			Me.radGroupRows.FooterImageIndex = -1
			Me.radGroupRows.FooterImageKey = ""
			Me.radGroupRows.HeaderImageIndex = -1
			Me.radGroupRows.HeaderImageKey = ""
			Me.radGroupRows.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupRows.HeaderText = "Rows"
			Me.radGroupRows.Location = New Point(10, 234)
			Me.radGroupRows.Name = "radGroupRows"
			Me.radGroupRows.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupRows.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupRows.Size = New Size(180, 128)
			Me.radGroupRows.TabIndex = 9
			Me.radGroupRows.Text = "Rows"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radGridView1)
			Me.Name = "Form1"
			Me.Size = New Size(1224, 820)

			Me.Controls.SetChildIndex(Me.radGridView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			CType(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnRmRows, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnAddRow, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnRmCol, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnAddCol, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckShowNewRow, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupCols, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupCols.ResumeLayout(False)
			CType(Me.radGroupAddCol, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupAddCol.ResumeLayout(False)
			Me.radGroupAddCol.PerformLayout()
			CType(Me.radGroupRmCol, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupRmCol.ResumeLayout(False)
			Me.radGroupRmCol.PerformLayout()
			CType(Me.radComboColumnsRemove, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupRows, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupRows.ResumeLayout(False)
			Me.radGroupRows.PerformLayout()
			Me.ResumeLayout(False)

		End Sub

		#End Region
		Private radGridView1 As Telerik.WinControls.UI.RadGridView
		Private radBtnRmCol As Telerik.WinControls.UI.RadButton
		Private radBtnAddCol As Telerik.WinControls.UI.RadButton
		Private radComboBox1 As Telerik.WinControls.UI.RadDropDownList
		Private radCheckShowNewRow As Telerik.WinControls.UI.RadCheckBox
		Private radBtnRmRows As Telerik.WinControls.UI.RadButton
		Private radBtnAddRow As Telerik.WinControls.UI.RadButton
		Private radLabel4 As Telerik.WinControls.UI.RadLabel
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radGroupRows As Telerik.WinControls.UI.RadGroupBox
		Private radGroupCols As Telerik.WinControls.UI.RadGroupBox
		Private radGroupAddCol As Telerik.WinControls.UI.RadGroupBox
		Private radGroupRmCol As Telerik.WinControls.UI.RadGroupBox
		Private radComboColumnsRemove As Telerik.WinControls.UI.RadDropDownList
	End Class
End Namespace