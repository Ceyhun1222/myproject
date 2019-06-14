<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CRemoveForm
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents OptionButton2 As System.Windows.Forms.RadioButton
	Public WithEvents OptionButton1 As System.Windows.Forms.RadioButton
	Public WithEvents bOK As System.Windows.Forms.Button
	Public WithEvents bCancel As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CRemoveForm))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.OptionButton2 = New System.Windows.Forms.RadioButton()
		Me.OptionButton1 = New System.Windows.Forms.RadioButton()
		Me.bOK = New System.Windows.Forms.Button()
		Me.bCancel = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'OptionButton2
		'
		Me.OptionButton2.BackColor = System.Drawing.SystemColors.Control
		Me.OptionButton2.Cursor = System.Windows.Forms.Cursors.Default
		Me.OptionButton2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.OptionButton2.Location = New System.Drawing.Point(150, 15)
		Me.OptionButton2.Name = "OptionButton2"
		Me.OptionButton2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.OptionButton2.Size = New System.Drawing.Size(121, 21)
		Me.OptionButton2.TabIndex = 3
		Me.OptionButton2.TabStop = True
		Me.OptionButton2.Text = "Remove All Legs"
		Me.OptionButton2.UseVisualStyleBackColor = False
		'
		'OptionButton1
		'
		Me.OptionButton1.BackColor = System.Drawing.SystemColors.Control
		Me.OptionButton1.Checked = True
		Me.OptionButton1.Cursor = System.Windows.Forms.Cursors.Default
		Me.OptionButton1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.OptionButton1.Location = New System.Drawing.Point(15, 15)
		Me.OptionButton1.Name = "OptionButton1"
		Me.OptionButton1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.OptionButton1.Size = New System.Drawing.Size(126, 21)
		Me.OptionButton1.TabIndex = 2
		Me.OptionButton1.TabStop = True
		Me.OptionButton1.Text = "Remove Last Leg"
		Me.OptionButton1.UseVisualStyleBackColor = False
		'
		'bOK
		'
		Me.bOK.BackColor = System.Drawing.SystemColors.Control
		Me.bOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.bOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.bOK.Image = CType(resources.GetObject("bOK.Image"), System.Drawing.Image)
		Me.bOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.bOK.Location = New System.Drawing.Point(95, 50)
		Me.bOK.Name = "bOK"
		Me.bOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.bOK.Size = New System.Drawing.Size(86, 26)
		Me.bOK.TabIndex = 1
		Me.bOK.Text = "&OK"
		Me.bOK.UseVisualStyleBackColor = False
		'
		'bCancel
		'
		Me.bCancel.BackColor = System.Drawing.SystemColors.Control
		Me.bCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.bCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.bCancel.Image = CType(resources.GetObject("bCancel.Image"), System.Drawing.Image)
		Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.bCancel.Location = New System.Drawing.Point(190, 50)
		Me.bCancel.Name = "bCancel"
		Me.bCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.bCancel.Size = New System.Drawing.Size(86, 26)
		Me.bCancel.TabIndex = 0
		Me.bCancel.Text = "&Cancel"
		Me.bCancel.UseVisualStyleBackColor = False
		'
		'RemoveForm
		'
		Me.AcceptButton = Me.bOK
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.bCancel
		Me.ClientSize = New System.Drawing.Size(285, 87)
		Me.Controls.Add(Me.OptionButton2)
		Me.Controls.Add(Me.OptionButton1)
		Me.Controls.Add(Me.bOK)
		Me.Controls.Add(Me.bCancel)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "RemoveForm"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Remove Leg"
		Me.ResumeLayout(False)

	End Sub
#End Region
End Class