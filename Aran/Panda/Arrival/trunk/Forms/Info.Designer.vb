<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CInfoFrm
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
	Public WithEvents lblInfo As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CInfoFrm))
		Me.lblInfo = New System.Windows.Forms.Label()
		Me.txtInfo = New System.Windows.Forms.TextBox()
		Me.SuspendLayout()
		'
		'lblInfo
		'
		Me.lblInfo.AutoSize = True
		Me.lblInfo.BackColor = System.Drawing.Color.Transparent
		Me.lblInfo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInfo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblInfo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInfo.Location = New System.Drawing.Point(1, 1)
		Me.lblInfo.Name = "lblInfo"
		Me.lblInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInfo.Size = New System.Drawing.Size(0, 14)
		Me.lblInfo.TabIndex = 0
		Me.lblInfo.Visible = False
		'
		'txtInfo
		'
		Me.txtInfo.BackColor = System.Drawing.SystemColors.Info
		Me.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtInfo.Location = New System.Drawing.Point(1, 1)
		Me.txtInfo.Multiline = True
		Me.txtInfo.Name = "txtInfo"
		Me.txtInfo.ReadOnly = True
		Me.txtInfo.Size = New System.Drawing.Size(273, 182)
		Me.txtInfo.TabIndex = 1
		Me.txtInfo.WordWrap = False
		'
		'CInfoFrm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Info
		Me.ClientSize = New System.Drawing.Size(304, 194)
		Me.Controls.Add(Me.txtInfo)
		Me.Controls.Add(Me.lblInfo)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "CInfoFrm"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Info"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents txtInfo As System.Windows.Forms.TextBox
#End Region
End Class