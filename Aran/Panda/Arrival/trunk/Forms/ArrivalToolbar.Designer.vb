<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArrivalToolbar
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
		Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
		Me.tsBtnOAS_Cat1 = New System.Windows.Forms.ToolStripButton()
		Me.tsBtnOAS_Cat2 = New System.Windows.Forms.ToolStripButton()
		Me.tsBtnBasic_ILS = New System.Windows.Forms.ToolStripButton()
		Me.tsBtnOFZ = New System.Windows.Forms.ToolStripButton()
		Me.ToolStrip1.SuspendLayout()
		Me.SuspendLayout()
		'
		'ToolStrip1
		'
		Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
		Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsBtnOAS_Cat1, Me.tsBtnOAS_Cat2, Me.tsBtnBasic_ILS, Me.tsBtnOFZ})
		Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
		Me.ToolStrip1.Name = "ToolStrip1"
		Me.ToolStrip1.Size = New System.Drawing.Size(247, 25)
		Me.ToolStrip1.TabIndex = 0
		Me.ToolStrip1.Text = "Approach - Precision"
		'
		'tsBtnOAS_Cat1
		'
		Me.tsBtnOAS_Cat1.Checked = True
		Me.tsBtnOAS_Cat1.CheckOnClick = True
		Me.tsBtnOAS_Cat1.CheckState = System.Windows.Forms.CheckState.Checked
		Me.tsBtnOAS_Cat1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.tsBtnOAS_Cat1.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsBtnOAS_Cat1.Name = "tsBtnOAS_Cat1"
		Me.tsBtnOAS_Cat1.Size = New System.Drawing.Size(62, 22)
		Me.tsBtnOAS_Cat1.Text = "OAS cat 1"
		Me.tsBtnOAS_Cat1.ToolTipText = "OAS cat 1"
		'
		'tsBtnOAS_Cat2
		'
		Me.tsBtnOAS_Cat2.CheckOnClick = True
		Me.tsBtnOAS_Cat2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.tsBtnOAS_Cat2.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsBtnOAS_Cat2.Name = "tsBtnOAS_Cat2"
		Me.tsBtnOAS_Cat2.Size = New System.Drawing.Size(62, 22)
		Me.tsBtnOAS_Cat2.Text = "OAS cat 2"
		Me.tsBtnOAS_Cat2.ToolTipText = "OAS cat 2"
		'
		'tsBtnBasic_ILS
		'
		Me.tsBtnBasic_ILS.CheckOnClick = True
		Me.tsBtnBasic_ILS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.tsBtnBasic_ILS.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsBtnBasic_ILS.Name = "tsBtnBasic_ILS"
		Me.tsBtnBasic_ILS.Size = New System.Drawing.Size(56, 22)
		Me.tsBtnBasic_ILS.Text = "Basic ILS"
		'
		'tsBtnOFZ
		'
		Me.tsBtnOFZ.CheckOnClick = True
		Me.tsBtnOFZ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.tsBtnOFZ.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsBtnOFZ.Name = "tsBtnOFZ"
		Me.tsBtnOFZ.Size = New System.Drawing.Size(33, 22)
		Me.tsBtnOFZ.Text = "OFZ"
		'
		'ArrivalToolbar
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(401, 27)
		Me.ControlBox = False
		Me.Controls.Add(Me.ToolStrip1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "ArrivalToolbar"
		Me.ShowIcon = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Arrival toolbar"
		Me.ToolStrip1.ResumeLayout(False)
		Me.ToolStrip1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
	Friend WithEvents tsBtnOAS_Cat2 As System.Windows.Forms.ToolStripButton
	Friend WithEvents tsBtnOAS_Cat1 As System.Windows.Forms.ToolStripButton
	Friend WithEvents tsBtnBasic_ILS As System.Windows.Forms.ToolStripButton
	Friend WithEvents tsBtnOFZ As System.Windows.Forms.ToolStripButton
End Class
