<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CMrkInfoForm
#Region "Windows Form Designer generated code "
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
	Public WithEvents _ListView1_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView1 As System.Windows.Forms.ListView
	Public ComDlgSave As System.Windows.Forms.SaveFileDialog
	Public WithEvents SaveBtn As System.Windows.Forms.Button
	Public WithEvents CloseBtn As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CMrkInfoForm))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.ListView1 = New System.Windows.Forms.ListView()
		Me._ListView1_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.ComDlgSave = New System.Windows.Forms.SaveFileDialog()
		Me.SaveBtn = New System.Windows.Forms.Button()
		Me.CloseBtn = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'ListView1
		'
		Me.ListView1.BackColor = System.Drawing.SystemColors.Window
		Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView1_ColumnHeader_1, Me._ListView1_ColumnHeader_2, Me._ListView1_ColumnHeader_3, Me._ListView1_ColumnHeader_4, Me._ListView1_ColumnHeader_5, Me._ListView1_ColumnHeader_6})
		Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView1.FullRowSelect = True
		Me.ListView1.GridLines = True
		Me.ListView1.HideSelection = False
		Me.ListView1.Location = New System.Drawing.Point(4, 4)
		Me.ListView1.Name = "ListView1"
		Me.ListView1.Size = New System.Drawing.Size(441, 185)
		Me.ListView1.TabIndex = 0
		Me.ListView1.UseCompatibleStateImageBehavior = False
		Me.ListView1.View = System.Windows.Forms.View.Details
		'
		'_ListView1_ColumnHeader_1
		'
		Me._ListView1_ColumnHeader_1.Text = "Name"
		Me._ListView1_ColumnHeader_1.Width = 170
		'
		'_ListView1_ColumnHeader_2
		'
		Me._ListView1_ColumnHeader_2.Text = "ID"
		Me._ListView1_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_2.Width = 170
		'
		'_ListView1_ColumnHeader_3
		'
		Me._ListView1_ColumnHeader_3.Text = "CallSign"
		Me._ListView1_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_3.Width = 170
		'
		'_ListView1_ColumnHeader_4
		'
		Me._ListView1_ColumnHeader_4.Text = "DistFromTHR"
		Me._ListView1_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_4.Width = 170
		'
		'_ListView1_ColumnHeader_5
		'
		Me._ListView1_ColumnHeader_5.Text = "Height"
		Me._ListView1_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_5.Width = 170
		'
		'_ListView1_ColumnHeader_6
		'
		Me._ListView1_ColumnHeader_6.Text = "Altitude"
		Me._ListView1_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_6.Width = 170
		'
		'ComDlgSave
		'
		Me.ComDlgSave.DefaultExt = "txt"
		Me.ComDlgSave.Filter = "Text (*.txt)|*.txt|HTML (*.html)|*.html|All Files (*.*)|*.*"
		'
		'SaveBtn
		'
		Me.SaveBtn.Location = New System.Drawing.Point(248, 195)
		Me.SaveBtn.Name = "SaveBtn"
		Me.SaveBtn.Size = New System.Drawing.Size(93, 25)
		Me.SaveBtn.TabIndex = 2
		Me.SaveBtn.Text = "Save"
		'
		'CloseBtn
		'
		Me.CloseBtn.Location = New System.Drawing.Point(348, 195)
		Me.CloseBtn.Name = "CloseBtn"
		Me.CloseBtn.Size = New System.Drawing.Size(93, 25)
		Me.CloseBtn.TabIndex = 1
		Me.CloseBtn.Text = "Close"
		'
		'CMrkInfoForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(450, 227)
		Me.Controls.Add(Me.ListView1)
		Me.Controls.Add(Me.SaveBtn)
		Me.Controls.Add(Me.CloseBtn)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "CMrkInfoForm"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Text = "Markers Info"
		Me.ResumeLayout(False)

	End Sub
#End Region
End Class