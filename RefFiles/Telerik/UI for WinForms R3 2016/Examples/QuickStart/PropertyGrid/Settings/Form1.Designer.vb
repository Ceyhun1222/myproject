﻿Namespace Telerik.Examples.WinControls.PropertyGrid.Settings
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
            Me.radPropertyGrid1 = New Telerik.WinControls.UI.RadPropertyGrid()
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownListPropertySort = New Telerik.WinControls.UI.RadDropDownList()
            Me.radSpinEditorItemSpacing = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radSpinEditorItemIndent = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radSpinEditorItemHeight = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radCheckBoxAutoExpandGroups = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxReadOnly = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxSearchVisible = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxHelpVisible = New Telerik.WinControls.UI.RadCheckBox()
            Me.RadCheckBoxKeyboardSearch = New Telerik.WinControls.UI.RadCheckBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPropertyGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownListPropertySort, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorItemSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorItemIndent, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorItemHeight, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxAutoExpandGroups, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxReadOnly, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxSearchVisible, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxHelpVisible, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadCheckBoxKeyboardSearch, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(1081, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 788)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            '
            'radPropertyGrid1
            '
            Me.radPropertyGrid1.HelpVisible = False
            Me.radPropertyGrid1.Location = New System.Drawing.Point(350, 25)
            Me.radPropertyGrid1.Name = "radPropertyGrid1"
            Me.radPropertyGrid1.Size = New System.Drawing.Size(315, 343)
            Me.radPropertyGrid1.TabIndex = 1
            Me.radPropertyGrid1.Text = "radPropertyGrid1"
            '
            'radGroupBox1
            '
            Me.radGroupBox1.Controls.Add(Me.RadCheckBoxKeyboardSearch)
            Me.radGroupBox1.Controls.Add(Me.radLabel4)
            Me.radGroupBox1.Controls.Add(Me.radLabel3)
            Me.radGroupBox1.Controls.Add(Me.radLabel2)
            Me.radGroupBox1.Controls.Add(Me.radLabel1)
            Me.radGroupBox1.Controls.Add(Me.radDropDownListPropertySort)
            Me.radGroupBox1.Controls.Add(Me.radSpinEditorItemSpacing)
            Me.radGroupBox1.Controls.Add(Me.radSpinEditorItemIndent)
            Me.radGroupBox1.Controls.Add(Me.radSpinEditorItemHeight)
            Me.radGroupBox1.Controls.Add(Me.radCheckBoxAutoExpandGroups)
            Me.radGroupBox1.Controls.Add(Me.radCheckBoxReadOnly)
            Me.radGroupBox1.Controls.Add(Me.radCheckBoxSearchVisible)
            Me.radGroupBox1.Controls.Add(Me.radCheckBoxHelpVisible)
            Me.radGroupBox1.HeaderText = "UI Settings"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 72)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(180, 256)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "UI Settings"
            '
            'radLabel4
            '
            Me.radLabel4.Location = New System.Drawing.Point(5, 225)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(70, 18)
            Me.radLabel4.TabIndex = 3
            Me.radLabel4.Text = "Property sort"
            '
            'radLabel3
            '
            Me.radLabel3.Location = New System.Drawing.Point(5, 199)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(69, 18)
            Me.radLabel3.TabIndex = 3
            Me.radLabel3.Text = "Item spacing"
            '
            'radLabel2
            '
            Me.radLabel2.Location = New System.Drawing.Point(5, 173)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(63, 18)
            Me.radLabel2.TabIndex = 3
            Me.radLabel2.Text = "Item indent"
            '
            'radLabel1
            '
            Me.radLabel1.Location = New System.Drawing.Point(5, 147)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(63, 18)
            Me.radLabel1.TabIndex = 3
            Me.radLabel1.Text = "Item height"
            '
            'radDropDownListPropertySort
            '
            Me.radDropDownListPropertySort.Location = New System.Drawing.Point(83, 223)
            Me.radDropDownListPropertySort.Name = "radDropDownListPropertySort"
            Me.radDropDownListPropertySort.Size = New System.Drawing.Size(92, 20)
            Me.radDropDownListPropertySort.TabIndex = 2
            Me.radDropDownListPropertySort.Tag = "Right"
            Me.radDropDownListPropertySort.Text = "radDropDownList1"
            '
            'radSpinEditorItemSpacing
            '
            Me.radSpinEditorItemSpacing.Location = New System.Drawing.Point(83, 197)
            Me.radSpinEditorItemSpacing.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
            Me.radSpinEditorItemSpacing.Name = "radSpinEditorItemSpacing"
            '
            '
            '
            Me.radSpinEditorItemSpacing.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditorItemSpacing.Size = New System.Drawing.Size(92, 20)
            Me.radSpinEditorItemSpacing.TabIndex = 1
            Me.radSpinEditorItemSpacing.TabStop = False
            Me.radSpinEditorItemSpacing.Tag = "Right"
            '
            'radSpinEditorItemIndent
            '
            Me.radSpinEditorItemIndent.Location = New System.Drawing.Point(83, 171)
            Me.radSpinEditorItemIndent.Name = "radSpinEditorItemIndent"
            '
            '
            '
            Me.radSpinEditorItemIndent.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditorItemIndent.Size = New System.Drawing.Size(92, 20)
            Me.radSpinEditorItemIndent.TabIndex = 1
            Me.radSpinEditorItemIndent.TabStop = False
            Me.radSpinEditorItemIndent.Tag = "Right"
            '
            'radSpinEditorItemHeight
            '
            Me.radSpinEditorItemHeight.Location = New System.Drawing.Point(83, 145)
            Me.radSpinEditorItemHeight.Name = "radSpinEditorItemHeight"
            '
            '
            '
            Me.radSpinEditorItemHeight.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditorItemHeight.Size = New System.Drawing.Size(92, 20)
            Me.radSpinEditorItemHeight.TabIndex = 1
            Me.radSpinEditorItemHeight.TabStop = False
            Me.radSpinEditorItemHeight.Tag = "Right"
            '
            'radCheckBoxAutoExpandGroups
            '
            Me.radCheckBoxAutoExpandGroups.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxAutoExpandGroups.Location = New System.Drawing.Point(5, 94)
            Me.radCheckBoxAutoExpandGroups.Name = "radCheckBoxAutoExpandGroups"
            Me.radCheckBoxAutoExpandGroups.Size = New System.Drawing.Size(122, 18)
            Me.radCheckBoxAutoExpandGroups.TabIndex = 0
            Me.radCheckBoxAutoExpandGroups.Text = "Auto expand groups"
            '
            'radCheckBoxReadOnly
            '
            Me.radCheckBoxReadOnly.Location = New System.Drawing.Point(5, 70)
            Me.radCheckBoxReadOnly.Name = "radCheckBoxReadOnly"
            Me.radCheckBoxReadOnly.Size = New System.Drawing.Size(70, 18)
            Me.radCheckBoxReadOnly.TabIndex = 0
            Me.radCheckBoxReadOnly.Text = "Read only"
            '
            'radCheckBoxSearchVisible
            '
            Me.radCheckBoxSearchVisible.Location = New System.Drawing.Point(5, 46)
            Me.radCheckBoxSearchVisible.Name = "radCheckBoxSearchVisible"
            Me.radCheckBoxSearchVisible.Size = New System.Drawing.Size(88, 18)
            Me.radCheckBoxSearchVisible.TabIndex = 0
            Me.radCheckBoxSearchVisible.Text = "Search visible"
            '
            'radCheckBoxHelpVisible
            '
            Me.radCheckBoxHelpVisible.Location = New System.Drawing.Point(5, 22)
            Me.radCheckBoxHelpVisible.Name = "radCheckBoxHelpVisible"
            Me.radCheckBoxHelpVisible.Size = New System.Drawing.Size(78, 18)
            Me.radCheckBoxHelpVisible.TabIndex = 0
            Me.radCheckBoxHelpVisible.Text = "Help visible"
            '
            'RadCheckBoxKeyboardSearch
            '
            Me.RadCheckBoxKeyboardSearch.Location = New System.Drawing.Point(5, 118)
            Me.RadCheckBoxKeyboardSearch.Name = "RadCheckBoxKeyboardSearch"
            Me.RadCheckBoxKeyboardSearch.Size = New System.Drawing.Size(162, 18)
            Me.RadCheckBoxKeyboardSearch.TabIndex = 1
            Me.RadCheckBoxKeyboardSearch.Text = "Enable Keyboard Navigation"
            '
            'Form1
            '
            Me.Controls.Add(Me.radPropertyGrid1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1405, 681)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radPropertyGrid1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPropertyGrid1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.radGroupBox1.PerformLayout()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownListPropertySort, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorItemSpacing, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorItemIndent, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorItemHeight, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxAutoExpandGroups, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxReadOnly, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxSearchVisible, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxHelpVisible, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadCheckBoxKeyboardSearch, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radPropertyGrid1 As Telerik.WinControls.UI.RadPropertyGrid
        Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Private radCheckBoxHelpVisible As Telerik.WinControls.UI.RadCheckBox
        Private radSpinEditorItemHeight As Telerik.WinControls.UI.RadSpinEditor
        Private radSpinEditorItemIndent As Telerik.WinControls.UI.RadSpinEditor
        Private radCheckBoxSearchVisible As Telerik.WinControls.UI.RadCheckBox
        Private radSpinEditorItemSpacing As Telerik.WinControls.UI.RadSpinEditor
        Private radDropDownListPropertySort As Telerik.WinControls.UI.RadDropDownList
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private radLabel4 As Telerik.WinControls.UI.RadLabel
        Private radCheckBoxReadOnly As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxAutoExpandGroups As Telerik.WinControls.UI.RadCheckBox
        Private WithEvents RadCheckBoxKeyboardSearch As Telerik.WinControls.UI.RadCheckBox
	End Class
End Namespace