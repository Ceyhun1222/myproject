<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CReportForm
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
	Public WithEvents _ListView2_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView2 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents _ListView1_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView1 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents MultiPage1 As System.Windows.Forms.TabControl
	Public SaveDialog1 As System.Windows.Forms.SaveFileDialog
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CReportForm))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MultiPage1 = New System.Windows.Forms.TabControl()
		Me._MultiPage1_TabPage0 = New System.Windows.Forms.TabPage()
		Me.ListView2 = New System.Windows.Forms.ListView()
		Me._ListView2_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView2_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView2_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView2_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView2_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._MultiPage1_TabPage1 = New System.Windows.Forms.TabPage()
		Me.ListView1 = New System.Windows.Forms.ListView()
		Me._ListView1_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.SaveDialog1 = New System.Windows.Forms.SaveFileDialog()
		Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
		Me.Label1 = New System.Windows.Forms.Label()
		Me.UpDown1 = New System.Windows.Forms.NumericUpDown()
		Me.CloseBtn = New System.Windows.Forms.Button()
		Me.SaveBtn = New System.Windows.Forms.Button()
		Me.MultiPage1.SuspendLayout()
		Me._MultiPage1_TabPage0.SuspendLayout()
		Me._MultiPage1_TabPage1.SuspendLayout()
		CType(Me.UpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'MultiPage1
		'
		Me.MultiPage1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage0)
		Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage1)
		Me.MultiPage1.ItemSize = New System.Drawing.Size(42, 18)
		Me.MultiPage1.Location = New System.Drawing.Point(3, 3)
		Me.MultiPage1.Name = "MultiPage1"
		Me.MultiPage1.SelectedIndex = 0
		Me.MultiPage1.Size = New System.Drawing.Size(589, 466)
		Me.MultiPage1.TabIndex = 0
		'
		'_MultiPage1_TabPage0
		'
		Me._MultiPage1_TabPage0.Controls.Add(Me.ListView2)
		Me._MultiPage1_TabPage0.Location = New System.Drawing.Point(4, 22)
		Me._MultiPage1_TabPage0.Name = "_MultiPage1_TabPage0"
		Me._MultiPage1_TabPage0.Size = New System.Drawing.Size(581, 440)
		Me._MultiPage1_TabPage0.TabIndex = 0
		Me._MultiPage1_TabPage0.Text = "Last obstructions"
		'
		'ListView2
		'
		Me.ListView2.Alignment = System.Windows.Forms.ListViewAlignment.Left
		Me.ListView2.BackColor = System.Drawing.SystemColors.Window
		Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView2_ColumnHeader_1, Me._ListView2_ColumnHeader_2, Me._ListView2_ColumnHeader_3, Me._ListView2_ColumnHeader_4, Me._ListView2_ColumnHeader_5})
		Me.ListView2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ListView2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView2.FullRowSelect = True
		Me.ListView2.GridLines = True
		Me.ListView2.Location = New System.Drawing.Point(0, 0)
		Me.ListView2.Name = "ListView2"
		Me.ListView2.Size = New System.Drawing.Size(581, 440)
		Me.ListView2.SmallImageList = Me.ImageList1
		Me.ListView2.TabIndex = 5
		Me.ListView2.UseCompatibleStateImageBehavior = False
		Me.ListView2.View = System.Windows.Forms.View.Details
		'
		'_ListView2_ColumnHeader_1
		'
		Me._ListView2_ColumnHeader_1.Text = "Name"
		Me._ListView2_ColumnHeader_1.Width = 170
		'
		'_ListView2_ColumnHeader_2
		'
		Me._ListView2_ColumnHeader_2.Text = "ID"
		Me._ListView2_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView2_ColumnHeader_2.Width = 170
		'
		'_ListView2_ColumnHeader_3
		'
		Me._ListView2_ColumnHeader_3.Text = "Elevation"
		Me._ListView2_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView2_ColumnHeader_3.Width = 170
		'
		'_ListView2_ColumnHeader_4
		'
		Me._ListView2_ColumnHeader_4.Text = "MOC"
		Me._ListView2_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView2_ColumnHeader_4.Width = 170
		'
		'_ListView2_ColumnHeader_5
		'
		Me._ListView2_ColumnHeader_5.Text = "Req.Alt."
		Me._ListView2_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView2_ColumnHeader_5.Width = 170
		'
		'_MultiPage1_TabPage1
		'
		Me._MultiPage1_TabPage1.Controls.Add(Me.ListView1)
		Me._MultiPage1_TabPage1.Location = New System.Drawing.Point(4, 22)
		Me._MultiPage1_TabPage1.Name = "_MultiPage1_TabPage1"
		Me._MultiPage1_TabPage1.Size = New System.Drawing.Size(581, 440)
		Me._MultiPage1_TabPage1.TabIndex = 1
		Me._MultiPage1_TabPage1.Text = "Segment obstructions"
		'
		'ListView1
		'
		Me.ListView1.Alignment = System.Windows.Forms.ListViewAlignment.Left
		Me.ListView1.BackColor = System.Drawing.SystemColors.Window
		Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView1_ColumnHeader_1, Me._ListView1_ColumnHeader_2, Me._ListView1_ColumnHeader_3, Me._ListView1_ColumnHeader_4, Me._ListView1_ColumnHeader_5})
		Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView1.FullRowSelect = True
		Me.ListView1.GridLines = True
		Me.ListView1.Location = New System.Drawing.Point(0, 0)
		Me.ListView1.Name = "ListView1"
		Me.ListView1.Size = New System.Drawing.Size(581, 440)
		Me.ListView1.SmallImageList = Me.ImageList1
		Me.ListView1.TabIndex = 4
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
		Me._ListView1_ColumnHeader_3.Text = "Elevation"
		Me._ListView1_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_3.Width = 170
		'
		'_ListView1_ColumnHeader_4
		'
		Me._ListView1_ColumnHeader_4.Text = "MOC"
		Me._ListView1_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_4.Width = 170
		'
		'_ListView1_ColumnHeader_5
		'
		Me._ListView1_ColumnHeader_5.Text = "Req.Alt."
		Me._ListView1_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView1_ColumnHeader_5.Width = 170
		'
		'SaveDialog1
		'
		Me.SaveDialog1.DefaultExt = "txt"
		Me.SaveDialog1.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*"
		'
		'ImageList1
		'
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.ImageList1.Images.SetKeyName(0, "")
		Me.ImageList1.Images.SetKeyName(1, "")
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(15, 486)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(52, 13)
		Me.Label1.TabIndex = 3
		Me.Label1.Text = "Segment:"
		Me.Label1.Visible = False
		'
		'UpDown1
		'
		Me.UpDown1.Location = New System.Drawing.Point(179, 482)
		Me.UpDown1.Name = "UpDown1"
		Me.UpDown1.Size = New System.Drawing.Size(82, 20)
		Me.UpDown1.TabIndex = 8
		'
		'CloseBtn
		'
		Me.CloseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CloseBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.CloseBtn.Image = CType(resources.GetObject("CloseBtn.Image"), System.Drawing.Image)
		Me.CloseBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.CloseBtn.Location = New System.Drawing.Point(478, 479)
		Me.CloseBtn.Name = "CloseBtn"
		Me.CloseBtn.Size = New System.Drawing.Size(94, 26)
		Me.CloseBtn.TabIndex = 12
		'
		'SaveBtn
		'
		Me.SaveBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SaveBtn.Image = CType(resources.GetObject("SaveBtn.Image"), System.Drawing.Image)
		Me.SaveBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.SaveBtn.Location = New System.Drawing.Point(382, 479)
		Me.SaveBtn.Name = "SaveBtn"
		Me.SaveBtn.Size = New System.Drawing.Size(94, 26)
		Me.SaveBtn.TabIndex = 11
		'
		'ReportForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(595, 517)
		Me.Controls.Add(Me.CloseBtn)
		Me.Controls.Add(Me.SaveBtn)
		Me.Controls.Add(Me.UpDown1)
		Me.Controls.Add(Me.MultiPage1)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.Name = "ReportForm"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.Text = "Report"
		Me.MultiPage1.ResumeLayout(False)
		Me._MultiPage1_TabPage0.ResumeLayout(False)
		Me._MultiPage1_TabPage1.ResumeLayout(False)
		CType(Me.UpDown1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents UpDown1 As System.Windows.Forms.NumericUpDown
	Public WithEvents CloseBtn As System.Windows.Forms.Button
	Public WithEvents SaveBtn As System.Windows.Forms.Button
#End Region
End Class