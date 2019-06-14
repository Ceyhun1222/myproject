<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutForm))
		Me.lblDescription = New System.Windows.Forms.Label()
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.lbllVersion = New System.Windows.Forms.Label()
		Me.lblVersionDate = New System.Windows.Forms.Label()
		Me.lblCopyRight = New System.Windows.Forms.Label()
		Me.Button1 = New System.Windows.Forms.Button()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblDescription
		'
		Me.lblDescription.AutoSize = True
		Me.lblDescription.Location = New System.Drawing.Point(5, 73)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.Size = New System.Drawing.Size(60, 13)
		Me.lblDescription.TabIndex = 1
		Me.lblDescription.Text = "Description"
		'
		'PictureBox1
		'
		Me.PictureBox1.BackColor = System.Drawing.SystemColors.Window
		Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
		Me.PictureBox1.Location = New System.Drawing.Point(8, 9)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(197, 43)
		Me.PictureBox1.TabIndex = 0
		Me.PictureBox1.TabStop = False
		'
		'lbllVersion
		'
		Me.lbllVersion.AutoSize = True
		Me.lbllVersion.Location = New System.Drawing.Point(237, 9)
		Me.lbllVersion.Name = "lbllVersion"
		Me.lbllVersion.Size = New System.Drawing.Size(42, 13)
		Me.lbllVersion.TabIndex = 2
		Me.lbllVersion.Text = "Version"
		'
		'lblVersionDate
		'
		Me.lblVersionDate.AutoSize = True
		Me.lblVersionDate.Location = New System.Drawing.Point(237, 39)
		Me.lblVersionDate.Name = "lblVersionDate"
		Me.lblVersionDate.Size = New System.Drawing.Size(66, 13)
		Me.lblVersionDate.TabIndex = 3
		Me.lblVersionDate.Text = "Version date"
		'
		'lblCopyRight
		'
		Me.lblCopyRight.AutoSize = True
		Me.lblCopyRight.Location = New System.Drawing.Point(5, 95)
		Me.lblCopyRight.Name = "lblCopyRight"
		Me.lblCopyRight.Size = New System.Drawing.Size(51, 13)
		Me.lblCopyRight.TabIndex = 4
		Me.lblCopyRight.Text = "Copyright"
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(343, 90)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(75, 23)
		Me.Button1.TabIndex = 5
		Me.Button1.Text = "&OK"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'AboutForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(430, 125)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.PictureBox1)
		Me.Controls.Add(Me.lblDescription)
		Me.Controls.Add(Me.lblCopyRight)
		Me.Controls.Add(Me.lblVersionDate)
		Me.Controls.Add(Me.lbllVersion)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "AboutForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "About"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents PictureBox1 As PictureBox
	Friend WithEvents lblDescription As Label
	Friend WithEvents lbllVersion As Label
	Friend WithEvents lblVersionDate As Label
	Friend WithEvents lblCopyRight As Label
	Friend WithEvents Button1 As Button
End Class
