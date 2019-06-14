Namespace Telerik.Examples.WinControls.MultiColumnComboBox
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

        '#region Windows Form Designer generated code

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem4 As New Telerik.WinControls.UI.RadListDataItem()
            Me.radMultiColumnComboBox1 = New Telerik.WinControls.UI.RadMultiColumnComboBox()
            Me.radGroupSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radLblAutoComplete = New Telerik.WinControls.UI.RadLabel()
            Me.radComboAutoCompl = New Telerik.WinControls.UI.RadDropDownList()
            Me.radCheckRotate = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckRTL = New Telerik.WinControls.UI.RadCheckBox()

            Me.radPanelDemoHolder.SuspendLayout()

            Me.settingsPanel.SuspendLayout()
			Me.radGroupSettings.SuspendLayout()
			Me.SuspendLayout()
            ' 
            ' radPanelDemoHolder
            ' 
            Me.radPanelDemoHolder.Controls.Add(Me.radMultiColumnComboBox1)
            Me.radPanelDemoHolder.ForeColor = System.Drawing.Color.Black
            Me.radPanelDemoHolder.MaximumSize = New System.Drawing.Size(362, 100)
            Me.radPanelDemoHolder.MinimumSize = New System.Drawing.Size(362, 100)
            ' 
            ' 
            ' 
            Me.radPanelDemoHolder.RootElement.MaxSize = New System.Drawing.Size(362, 100)
            Me.radPanelDemoHolder.RootElement.MinSize = New System.Drawing.Size(362, 100)
            Me.radPanelDemoHolder.Size = New System.Drawing.Size(362, 100)
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupSettings)
            Me.settingsPanel.Location = New System.Drawing.Point(779, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 784)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupSettings, 0)
            ' 
            ' radMultiColumnComboBox1
            ' 
            ' 
            ' radMultiColumnComboBox1.NestedRadGridView
            ' 
            Me.radMultiColumnComboBox1.EditorControl.BackColor = System.Drawing.SystemColors.ControlLightLight
            Me.radMultiColumnComboBox1.EditorControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CType((0), Byte)))
            Me.radMultiColumnComboBox1.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText
            Me.radMultiColumnComboBox1.EditorControl.Location = New System.Drawing.Point(4, 1)
            ' 
            ' 
            ' 
            Me.radMultiColumnComboBox1.EditorControl.MasterTemplate.AllowAddNewRow = False
            Me.radMultiColumnComboBox1.EditorControl.MasterTemplate.AllowCellContextMenu = False
            Me.radMultiColumnComboBox1.EditorControl.MasterTemplate.AllowColumnChooser = False
            Me.radMultiColumnComboBox1.EditorControl.MasterTemplate.EnableGrouping = False
            Me.radMultiColumnComboBox1.EditorControl.MasterTemplate.ShowFilteringRow = False
            Me.radMultiColumnComboBox1.EditorControl.Name = "NestedRadGridView"
            Me.radMultiColumnComboBox1.EditorControl.[ReadOnly] = True
            Me.radMultiColumnComboBox1.EditorControl.ShowGroupPanel = False
            Me.radMultiColumnComboBox1.EditorControl.Size = New System.Drawing.Size(282, 104)
            Me.radMultiColumnComboBox1.EditorControl.TabIndex = 0
            Me.radMultiColumnComboBox1.Location = New System.Drawing.Point(0, 0)
            Me.radMultiColumnComboBox1.Name = "radMultiColumnComboBox1"
            ' 
            ' 
            ' 
            Me.radMultiColumnComboBox1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radMultiColumnComboBox1.Size = New System.Drawing.Size(306, 21)
            Me.radMultiColumnComboBox1.TabIndex = 0
            Me.radMultiColumnComboBox1.TabStop = False
            ' 
            ' radGroupSettings
            ' 
            Me.radGroupSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupSettings.Controls.Add(Me.radLblAutoComplete)
            Me.radGroupSettings.Controls.Add(Me.radComboAutoCompl)
            Me.radGroupSettings.Controls.Add(Me.radCheckRotate)
            Me.radGroupSettings.Controls.Add(Me.radCheckRTL)
            Me.radGroupSettings.FooterText = ""
            Me.radGroupSettings.HeaderText = "Settings"
            Me.radGroupSettings.Location = New System.Drawing.Point(10, 6)
            Me.radGroupSettings.Name = "radGroupSettings"
            Me.radGroupSettings.Size = New System.Drawing.Size(180, 125)
            Me.radGroupSettings.TabIndex = 0
            Me.radGroupSettings.Text = "Settings"
            ' 
            ' radLblAutoComplete
            ' 
            Me.radLblAutoComplete.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLblAutoComplete.Location = New System.Drawing.Point(5, 21)
            Me.radLblAutoComplete.Name = "radLblAutoComplete"
            Me.radLblAutoComplete.Size = New System.Drawing.Size(113, 18)
            Me.radLblAutoComplete.TabIndex = 2
            Me.radLblAutoComplete.Text = "AutoComplete Mode:"
            ' 
            ' radComboAutoCompl
            ' 
            Me.radComboAutoCompl.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radComboAutoCompl.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radComboAutoCompl.FormatString = "{0}"
            radListDataItem1.Text = "None"
            radListDataItem1.TextWrap = True
            radListDataItem2.Text = "Suggest"
            radListDataItem2.TextWrap = True
            radListDataItem3.Text = "Append"
            radListDataItem3.TextWrap = True
            radListDataItem4.Text = "SuggestAppend"
            radListDataItem4.TextWrap = True
            Me.radComboAutoCompl.Items.Add(radListDataItem1)
            Me.radComboAutoCompl.Items.Add(radListDataItem2)
            Me.radComboAutoCompl.Items.Add(radListDataItem3)
            Me.radComboAutoCompl.Items.Add(radListDataItem4)
            Me.radComboAutoCompl.Location = New System.Drawing.Point(5, 44)
            Me.radComboAutoCompl.Name = "radComboAutoCompl"
            ' 
            ' 
            ' 
            Me.radComboAutoCompl.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radComboAutoCompl.Size = New System.Drawing.Size(170, 20)
            Me.radComboAutoCompl.TabIndex = 1
            Me.radComboAutoCompl.Text = "Select mode:"
            ' 
            ' radCheckRotate
            ' 
            Me.radCheckRotate.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckRotate.Location = New System.Drawing.Point(5, 78)
            Me.radCheckRotate.Name = "radCheckRotate"
            Me.radCheckRotate.Size = New System.Drawing.Size(138, 18)
            Me.radCheckRotate.TabIndex = 0
            Me.radCheckRotate.Text = "Rotate On Double-Click"
            ' 
            ' radCheckRTL
            ' 
            Me.radCheckRTL.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckRTL.Location = New System.Drawing.Point(5, 101)
            Me.radCheckRTL.Name = "radCheckRTL"
            Me.radCheckRTL.Size = New System.Drawing.Size(84, 18)
            Me.radCheckRTL.TabIndex = 0
            Me.radCheckRTL.Text = "Right To Left"
            ' 
            ' Form1
            ' 
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1495, 898)

            Me.radPanelDemoHolder.ResumeLayout(False)

            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()

            Me.radGroupSettings.ResumeLayout(False)
            Me.radGroupSettings.PerformLayout()
			
            Me.ResumeLayout(False)

        End Sub

        '#endregion

        Friend WithEvents radMultiColumnComboBox1 As Telerik.WinControls.UI.RadMultiColumnComboBox
        Private radGroupSettings As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents radCheckRotate As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents radCheckRTL As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents radComboAutoCompl As Telerik.WinControls.UI.RadDropDownList
        Private radLblAutoComplete As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace