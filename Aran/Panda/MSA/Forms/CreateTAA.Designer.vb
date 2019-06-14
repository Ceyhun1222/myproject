<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CCreateTAA
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CCreateTAA))
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.ReportBtn = New System.Windows.Forms.CheckBox()
		Me.HelpBtn = New System.Windows.Forms.Button()
		Me.OKBtn = New System.Windows.Forms.Button()
		Me.CancelBtn = New System.Windows.Forms.Button()
		Me.ComboBox001 = New System.Windows.Forms.ComboBox()
		Me.Label001 = New System.Windows.Forms.Label()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.lbLeftUnit = New System.Windows.Forms.Label()
		Me.Label204 = New System.Windows.Forms.Label()
		Me.Label202 = New System.Windows.Forms.Label()
		Me.EndLeft = New System.Windows.Forms.TextBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.cbLeftMOC = New System.Windows.Forms.ComboBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.StartLeft = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.ShowLeft = New System.Windows.Forms.CheckBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.lbCentralUnit = New System.Windows.Forms.Label()
		Me.cbCentralMOC = New System.Windows.Forms.ComboBox()
		Me.Label14 = New System.Windows.Forms.Label()
		Me.EndCentral = New System.Windows.Forms.TextBox()
		Me.Label10 = New System.Windows.Forms.Label()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.StartCentral = New System.Windows.Forms.TextBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.ShowCentral = New System.Windows.Forms.CheckBox()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.lbRightUnit = New System.Windows.Forms.Label()
		Me.cbRightMOC = New System.Windows.Forms.ComboBox()
		Me.Label16 = New System.Windows.Forms.Label()
		Me.EndRight = New System.Windows.Forms.TextBox()
		Me.Label11 = New System.Windows.Forms.Label()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.StartRight = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.ShowRight = New System.Windows.Forms.CheckBox()
		Me.Panel1.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		Me.GroupBox2.SuspendLayout()
		Me.GroupBox3.SuspendLayout()
		Me.SuspendLayout()
		'
		'Panel1
		'
		Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Panel1.Controls.Add(Me.ReportBtn)
		Me.Panel1.Controls.Add(Me.HelpBtn)
		Me.Panel1.Controls.Add(Me.OKBtn)
		Me.Panel1.Controls.Add(Me.CancelBtn)
		Me.Panel1.Location = New System.Drawing.Point(-1, 286)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(537, 35)
		Me.Panel1.TabIndex = 1
		'
		'ReportBtn
		'
		Me.ReportBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ReportBtn.Appearance = System.Windows.Forms.Appearance.Button
		Me.ReportBtn.BackColor = System.Drawing.SystemColors.Control
		Me.ReportBtn.Cursor = System.Windows.Forms.Cursors.Default
		Me.ReportBtn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ReportBtn.Location = New System.Drawing.Point(394, 3)
		Me.ReportBtn.Name = "ReportBtn"
		Me.ReportBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ReportBtn.Size = New System.Drawing.Size(64, 27)
		Me.ReportBtn.TabIndex = 36
		Me.ReportBtn.Text = "&Report"
		Me.ReportBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.ReportBtn.UseVisualStyleBackColor = False
		'
		'HelpBtn
		'
		Me.HelpBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.HelpBtn.BackColor = System.Drawing.SystemColors.Control
		Me.HelpBtn.Cursor = System.Windows.Forms.Cursors.Default
		Me.HelpBtn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.HelpBtn.Location = New System.Drawing.Point(466, 3)
		Me.HelpBtn.Name = "HelpBtn"
		Me.HelpBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpBtn.Size = New System.Drawing.Size(64, 27)
		Me.HelpBtn.TabIndex = 37
		Me.HelpBtn.Text = "&Help"
		Me.HelpBtn.UseVisualStyleBackColor = False
		'
		'OKBtn
		'
		Me.OKBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.OKBtn.BackColor = System.Drawing.SystemColors.Control
		Me.OKBtn.Cursor = System.Windows.Forms.Cursors.Default
		Me.OKBtn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.OKBtn.Location = New System.Drawing.Point(246, 3)
		Me.OKBtn.Name = "OKBtn"
		Me.OKBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.OKBtn.Size = New System.Drawing.Size(64, 27)
		Me.OKBtn.TabIndex = 34
		Me.OKBtn.Text = "&OK"
		Me.OKBtn.UseVisualStyleBackColor = False
		'
		'CancelBtn
		'
		Me.CancelBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CancelBtn.BackColor = System.Drawing.SystemColors.Control
		Me.CancelBtn.Cursor = System.Windows.Forms.Cursors.Default
		Me.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.CancelBtn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CancelBtn.Location = New System.Drawing.Point(322, 3)
		Me.CancelBtn.Name = "CancelBtn"
		Me.CancelBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CancelBtn.Size = New System.Drawing.Size(64, 27)
		Me.CancelBtn.TabIndex = 35
		Me.CancelBtn.Text = "&Cancel"
		Me.CancelBtn.UseVisualStyleBackColor = False
		'
		'ComboBox001
		'
		Me.ComboBox001.BackColor = System.Drawing.SystemColors.Window
		Me.ComboBox001.Cursor = System.Windows.Forms.Cursors.Default
		Me.ComboBox001.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.ComboBox001.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ComboBox001.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ComboBox001.Location = New System.Drawing.Point(78, 12)
		Me.ComboBox001.Name = "ComboBox001"
		Me.ComboBox001.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ComboBox001.Size = New System.Drawing.Size(165, 22)
		Me.ComboBox001.TabIndex = 97
		'
		'Label001
		'
		Me.Label001.AutoSize = True
		Me.Label001.BackColor = System.Drawing.SystemColors.Control
		Me.Label001.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label001.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label001.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label001.Location = New System.Drawing.Point(6, 16)
		Me.Label001.Name = "Label001"
		Me.Label001.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label001.Size = New System.Drawing.Size(60, 14)
		Me.Label001.TabIndex = 90
		Me.Label001.Text = "Procedure:"
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.lbLeftUnit)
		Me.GroupBox1.Controls.Add(Me.Label204)
		Me.GroupBox1.Controls.Add(Me.Label202)
		Me.GroupBox1.Controls.Add(Me.EndLeft)
		Me.GroupBox1.Controls.Add(Me.Label5)
		Me.GroupBox1.Controls.Add(Me.cbLeftMOC)
		Me.GroupBox1.Controls.Add(Me.Label4)
		Me.GroupBox1.Controls.Add(Me.StartLeft)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Controls.Add(Me.ShowLeft)
		Me.GroupBox1.Location = New System.Drawing.Point(8, 52)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(165, 225)
		Me.GroupBox1.TabIndex = 99
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Left TAA"
		'
		'lbLeftUnit
		'
		Me.lbLeftUnit.AutoSize = True
		Me.lbLeftUnit.Location = New System.Drawing.Point(134, 126)
		Me.lbLeftUnit.Name = "lbLeftUnit"
		Me.lbLeftUnit.Size = New System.Drawing.Size(28, 13)
		Me.lbLeftUnit.TabIndex = 27
		Me.lbLeftUnit.Text = "Metr"
		'
		'Label204
		'
		Me.Label204.AutoSize = True
		Me.Label204.BackColor = System.Drawing.SystemColors.Control
		Me.Label204.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label204.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label204.Location = New System.Drawing.Point(134, 87)
		Me.Label204.Name = "Label204"
		Me.Label204.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label204.Size = New System.Drawing.Size(11, 13)
		Me.Label204.TabIndex = 26
		Me.Label204.Text = "°"
		'
		'Label202
		'
		Me.Label202.AutoSize = True
		Me.Label202.BackColor = System.Drawing.SystemColors.Control
		Me.Label202.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label202.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label202.Location = New System.Drawing.Point(134, 52)
		Me.Label202.Name = "Label202"
		Me.Label202.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label202.Size = New System.Drawing.Size(11, 13)
		Me.Label202.TabIndex = 25
		Me.Label202.Text = "°"
		'
		'EndLeft
		'
		Me.EndLeft.Location = New System.Drawing.Point(48, 83)
		Me.EndLeft.Name = "EndLeft"
		Me.EndLeft.ReadOnly = True
		Me.EndLeft.Size = New System.Drawing.Size(76, 20)
		Me.EndLeft.TabIndex = 6
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(3, 87)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(26, 13)
		Me.Label5.TabIndex = 5
		Me.Label5.Text = "End"
		'
		'cbLeftMOC
		'
		Me.cbLeftMOC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbLeftMOC.FormattingEnabled = True
		Me.cbLeftMOC.Location = New System.Drawing.Point(48, 122)
		Me.cbLeftMOC.Name = "cbLeftMOC"
		Me.cbLeftMOC.Size = New System.Drawing.Size(76, 21)
		Me.cbLeftMOC.TabIndex = 4
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(3, 126)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(31, 13)
		Me.Label4.TabIndex = 3
		Me.Label4.Text = "MOC"
		'
		'StartLeft
		'
		Me.StartLeft.Location = New System.Drawing.Point(48, 48)
		Me.StartLeft.Name = "StartLeft"
		Me.StartLeft.ReadOnly = True
		Me.StartLeft.Size = New System.Drawing.Size(76, 20)
		Me.StartLeft.TabIndex = 2
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(3, 52)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(32, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Start "
		'
		'ShowLeft
		'
		Me.ShowLeft.AutoSize = True
		Me.ShowLeft.Checked = True
		Me.ShowLeft.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ShowLeft.Location = New System.Drawing.Point(3, 19)
		Me.ShowLeft.Name = "ShowLeft"
		Me.ShowLeft.Size = New System.Drawing.Size(53, 17)
		Me.ShowLeft.TabIndex = 0
		Me.ShowLeft.Text = "Show"
		Me.ShowLeft.UseVisualStyleBackColor = True
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.lbCentralUnit)
		Me.GroupBox2.Controls.Add(Me.cbCentralMOC)
		Me.GroupBox2.Controls.Add(Me.Label14)
		Me.GroupBox2.Controls.Add(Me.EndCentral)
		Me.GroupBox2.Controls.Add(Me.Label10)
		Me.GroupBox2.Controls.Add(Me.Label6)
		Me.GroupBox2.Controls.Add(Me.Label7)
		Me.GroupBox2.Controls.Add(Me.StartCentral)
		Me.GroupBox2.Controls.Add(Me.Label2)
		Me.GroupBox2.Controls.Add(Me.ShowCentral)
		Me.GroupBox2.Location = New System.Drawing.Point(184, 52)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(165, 225)
		Me.GroupBox2.TabIndex = 100
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Central TAA"
		'
		'lbCentralUnit
		'
		Me.lbCentralUnit.AutoSize = True
		Me.lbCentralUnit.Location = New System.Drawing.Point(133, 126)
		Me.lbCentralUnit.Name = "lbCentralUnit"
		Me.lbCentralUnit.Size = New System.Drawing.Size(28, 13)
		Me.lbCentralUnit.TabIndex = 31
		Me.lbCentralUnit.Text = "Metr"
		'
		'cbCentralMOC
		'
		Me.cbCentralMOC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbCentralMOC.FormattingEnabled = True
		Me.cbCentralMOC.Location = New System.Drawing.Point(47, 122)
		Me.cbCentralMOC.Name = "cbCentralMOC"
		Me.cbCentralMOC.Size = New System.Drawing.Size(76, 21)
		Me.cbCentralMOC.TabIndex = 30
		'
		'Label14
		'
		Me.Label14.AutoSize = True
		Me.Label14.Location = New System.Drawing.Point(2, 126)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(31, 13)
		Me.Label14.TabIndex = 29
		Me.Label14.Text = "MOC"
		'
		'EndCentral
		'
		Me.EndCentral.Location = New System.Drawing.Point(47, 83)
		Me.EndCentral.Name = "EndCentral"
		Me.EndCentral.ReadOnly = True
		Me.EndCentral.Size = New System.Drawing.Size(76, 20)
		Me.EndCentral.TabIndex = 28
		'
		'Label10
		'
		Me.Label10.AutoSize = True
		Me.Label10.Location = New System.Drawing.Point(2, 87)
		Me.Label10.Name = "Label10"
		Me.Label10.Size = New System.Drawing.Size(26, 13)
		Me.Label10.TabIndex = 27
		Me.Label10.Text = "End"
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.BackColor = System.Drawing.SystemColors.Control
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Location = New System.Drawing.Point(133, 87)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(11, 13)
		Me.Label6.TabIndex = 26
		Me.Label6.Text = "°"
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.BackColor = System.Drawing.SystemColors.Control
		Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label7.Location = New System.Drawing.Point(133, 52)
		Me.Label7.Name = "Label7"
		Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label7.Size = New System.Drawing.Size(11, 13)
		Me.Label7.TabIndex = 25
		Me.Label7.Text = "°"
		'
		'StartCentral
		'
		Me.StartCentral.Location = New System.Drawing.Point(47, 48)
		Me.StartCentral.Name = "StartCentral"
		Me.StartCentral.ReadOnly = True
		Me.StartCentral.Size = New System.Drawing.Size(76, 20)
		Me.StartCentral.TabIndex = 2
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(2, 52)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(29, 13)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Start"
		'
		'ShowCentral
		'
		Me.ShowCentral.AutoSize = True
		Me.ShowCentral.Checked = True
		Me.ShowCentral.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ShowCentral.Location = New System.Drawing.Point(2, 19)
		Me.ShowCentral.Name = "ShowCentral"
		Me.ShowCentral.Size = New System.Drawing.Size(53, 17)
		Me.ShowCentral.TabIndex = 0
		Me.ShowCentral.Text = "Show"
		Me.ShowCentral.UseVisualStyleBackColor = True
		'
		'GroupBox3
		'
		Me.GroupBox3.Controls.Add(Me.lbRightUnit)
		Me.GroupBox3.Controls.Add(Me.cbRightMOC)
		Me.GroupBox3.Controls.Add(Me.Label16)
		Me.GroupBox3.Controls.Add(Me.EndRight)
		Me.GroupBox3.Controls.Add(Me.Label11)
		Me.GroupBox3.Controls.Add(Me.Label8)
		Me.GroupBox3.Controls.Add(Me.Label9)
		Me.GroupBox3.Controls.Add(Me.StartRight)
		Me.GroupBox3.Controls.Add(Me.Label3)
		Me.GroupBox3.Controls.Add(Me.ShowRight)
		Me.GroupBox3.Location = New System.Drawing.Point(360, 52)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(165, 225)
		Me.GroupBox3.TabIndex = 101
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "Right TAA"
		'
		'lbRightUnit
		'
		Me.lbRightUnit.AutoSize = True
		Me.lbRightUnit.Location = New System.Drawing.Point(135, 126)
		Me.lbRightUnit.Name = "lbRightUnit"
		Me.lbRightUnit.Size = New System.Drawing.Size(28, 13)
		Me.lbRightUnit.TabIndex = 31
		Me.lbRightUnit.Text = "Metr"
		'
		'cbRightMOC
		'
		Me.cbRightMOC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbRightMOC.FormattingEnabled = True
		Me.cbRightMOC.Location = New System.Drawing.Point(49, 122)
		Me.cbRightMOC.Name = "cbRightMOC"
		Me.cbRightMOC.Size = New System.Drawing.Size(76, 21)
		Me.cbRightMOC.TabIndex = 30
		'
		'Label16
		'
		Me.Label16.AutoSize = True
		Me.Label16.Location = New System.Drawing.Point(4, 126)
		Me.Label16.Name = "Label16"
		Me.Label16.Size = New System.Drawing.Size(31, 13)
		Me.Label16.TabIndex = 29
		Me.Label16.Text = "MOC"
		'
		'EndRight
		'
		Me.EndRight.Location = New System.Drawing.Point(49, 83)
		Me.EndRight.Name = "EndRight"
		Me.EndRight.ReadOnly = True
		Me.EndRight.Size = New System.Drawing.Size(76, 20)
		Me.EndRight.TabIndex = 28
		'
		'Label11
		'
		Me.Label11.AutoSize = True
		Me.Label11.Location = New System.Drawing.Point(4, 87)
		Me.Label11.Name = "Label11"
		Me.Label11.Size = New System.Drawing.Size(26, 13)
		Me.Label11.TabIndex = 27
		Me.Label11.Text = "End"
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.BackColor = System.Drawing.SystemColors.Control
		Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label8.Location = New System.Drawing.Point(135, 87)
		Me.Label8.Name = "Label8"
		Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label8.Size = New System.Drawing.Size(11, 13)
		Me.Label8.TabIndex = 26
		Me.Label8.Text = "°"
		'
		'Label9
		'
		Me.Label9.AutoSize = True
		Me.Label9.BackColor = System.Drawing.SystemColors.Control
		Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label9.Location = New System.Drawing.Point(135, 52)
		Me.Label9.Name = "Label9"
		Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label9.Size = New System.Drawing.Size(11, 13)
		Me.Label9.TabIndex = 25
		Me.Label9.Text = "°"
		'
		'StartRight
		'
		Me.StartRight.Location = New System.Drawing.Point(49, 48)
		Me.StartRight.Name = "StartRight"
		Me.StartRight.ReadOnly = True
		Me.StartRight.Size = New System.Drawing.Size(76, 20)
		Me.StartRight.TabIndex = 2
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(4, 52)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(29, 13)
		Me.Label3.TabIndex = 1
		Me.Label3.Text = "Start"
		'
		'ShowRight
		'
		Me.ShowRight.AutoSize = True
		Me.ShowRight.Checked = True
		Me.ShowRight.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ShowRight.Location = New System.Drawing.Point(4, 19)
		Me.ShowRight.Name = "ShowRight"
		Me.ShowRight.Size = New System.Drawing.Size(53, 17)
		Me.ShowRight.TabIndex = 0
		Me.ShowRight.Text = "Show"
		Me.ShowRight.UseVisualStyleBackColor = True
		'
		'CCreateTAA
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(534, 324)
		Me.Controls.Add(Me.GroupBox3)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.ComboBox001)
		Me.Controls.Add(Me.Label001)
		Me.Controls.Add(Me.Panel1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "CCreateTAA"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Create TAA"
		Me.Panel1.ResumeLayout(False)
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.GroupBox3.ResumeLayout(False)
		Me.GroupBox3.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Panel1 As System.Windows.Forms.Panel
	Public WithEvents ReportBtn As System.Windows.Forms.CheckBox
	Public WithEvents HelpBtn As System.Windows.Forms.Button
	Public WithEvents OKBtn As System.Windows.Forms.Button
	Public WithEvents CancelBtn As System.Windows.Forms.Button
	Public WithEvents ComboBox001 As System.Windows.Forms.ComboBox
	Public WithEvents Label001 As System.Windows.Forms.Label
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents ShowLeft As System.Windows.Forms.CheckBox
	Friend WithEvents StartLeft As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
	Friend WithEvents StartCentral As System.Windows.Forms.TextBox
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents ShowCentral As System.Windows.Forms.CheckBox
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
	Friend WithEvents StartRight As System.Windows.Forms.TextBox
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents ShowRight As System.Windows.Forms.CheckBox
	Friend WithEvents EndLeft As System.Windows.Forms.TextBox
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents cbLeftMOC As System.Windows.Forms.ComboBox
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label204 As System.Windows.Forms.Label
	Public WithEvents Label202 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Friend WithEvents EndCentral As System.Windows.Forms.TextBox
	Friend WithEvents Label10 As System.Windows.Forms.Label
	Friend WithEvents EndRight As System.Windows.Forms.TextBox
	Friend WithEvents Label11 As System.Windows.Forms.Label
	Friend WithEvents lbLeftUnit As System.Windows.Forms.Label
	Friend WithEvents lbCentralUnit As System.Windows.Forms.Label
	Friend WithEvents cbCentralMOC As System.Windows.Forms.ComboBox
	Friend WithEvents Label14 As System.Windows.Forms.Label
	Friend WithEvents lbRightUnit As System.Windows.Forms.Label
	Friend WithEvents cbRightMOC As System.Windows.Forms.ComboBox
	Friend WithEvents Label16 As System.Windows.Forms.Label
End Class
