<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CExcludeObstFrm
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CExcludeObstFrm))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.ListView1 = New System.Windows.Forms.ListView()
		Me._ListView1_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.OKbtn = New System.Windows.Forms.Button()
		Me.CancelBtn = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'ListView1
		'
		Me.ListView1.BackColor = System.Drawing.SystemColors.Window
		Me.ListView1.CheckBoxes = True
		Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView1_ColumnHeader_1, Me._ListView1_ColumnHeader_2, Me._ListView1_ColumnHeader_3})
		Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView1.FullRowSelect = True
		Me.ListView1.GridLines = True
		Me.ListView1.LabelWrap = False
		Me.ListView1.Location = New System.Drawing.Point(5, 5)
		Me.ListView1.Name = "ListView1"
		Me.ListView1.Size = New System.Drawing.Size(513, 303)
		Me.ListView1.TabIndex = 6
		Me.ListView1.UseCompatibleStateImageBehavior = False
		Me.ListView1.View = System.Windows.Forms.View.Details
		'
		'_ListView1_ColumnHeader_1
		'
		Me._ListView1_ColumnHeader_1.Text = "Obstacle Type"
		Me._ListView1_ColumnHeader_1.Width = 170
		'
		'_ListView1_ColumnHeader_2
		'
		Me._ListView1_ColumnHeader_2.Text = "Obstacle Name"
		Me._ListView1_ColumnHeader_2.Width = 170
		'
		'_ListView1_ColumnHeader_3
		'
		Me._ListView1_ColumnHeader_3.Text = "H. abv. DER"
		Me._ListView1_ColumnHeader_3.Width = 170
		'
		'OKbtn
		'
		Me.OKbtn.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.OKbtn.Location = New System.Drawing.Point(323, 314)
		Me.OKbtn.Name = "OKbtn"
		Me.OKbtn.Size = New System.Drawing.Size(93, 25)
		Me.OKbtn.TabIndex = 8
		Me.OKbtn.Text = "&OK"
		'
		'CancelBtn
		'
		Me.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.CancelBtn.Location = New System.Drawing.Point(423, 314)
		Me.CancelBtn.Name = "CancelBtn"
		Me.CancelBtn.Size = New System.Drawing.Size(93, 25)
		Me.CancelBtn.TabIndex = 7
		Me.CancelBtn.Text = "&Cancel"
		'
		'CExcludeObstFrm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.CancelBtn
		Me.ClientSize = New System.Drawing.Size(522, 345)
		Me.Controls.Add(Me.ListView1)
		Me.Controls.Add(Me.OKbtn)
		Me.Controls.Add(Me.CancelBtn)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "CExcludeObstFrm"
		Me.ShowInTaskbar = False
		Me.Text = "Exclude Ostacles"
		Me.ResumeLayout(False)

	End Sub
	Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents ListView1 As System.Windows.Forms.ListView
	Public WithEvents _ListView1_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents OKbtn As System.Windows.Forms.Button
	Public WithEvents CancelBtn As System.Windows.Forms.Button
End Class
