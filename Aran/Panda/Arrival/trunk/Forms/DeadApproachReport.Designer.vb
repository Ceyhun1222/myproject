<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CDeadApproachReport
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
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Public WithEvents _ListView1_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView1 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents _ListView2_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView2 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents _ListView3_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView3_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView3_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView3_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView3_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView3_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView3 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents MultiPage1 As System.Windows.Forms.TabControl
	Public WithEvents CloseBtn As System.Windows.Forms.Button
	Public WithEvents SaveBtn As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CDeadApproachReport))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog()
        Me.MultiPage1 = New System.Windows.Forms.TabControl()
        Me._MultiPage1_TabPage0 = New System.Windows.Forms.TabPage()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me._ListView1_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._MultiPage1_TabPage1 = New System.Windows.Forms.TabPage()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me._ListView2_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._MultiPage1_TabPage2 = New System.Windows.Forms.TabPage()
        Me.ListView3 = New System.Windows.Forms.ListView()
        Me._ListView3_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView3_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView3_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView3_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView3_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView3_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.CloseBtn = New System.Windows.Forms.Button()
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.lblCountNumber = New System.Windows.Forms.Label()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.MultiPage1.SuspendLayout
        Me._MultiPage1_TabPage0.SuspendLayout
        Me._MultiPage1_TabPage1.SuspendLayout
        Me._MultiPage1_TabPage2.SuspendLayout
        Me.SuspendLayout
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "space.ico")
        '
        'SaveDlg
        '
        Me.SaveDlg.DefaultExt = "txt"
        Me.SaveDlg.Filter = """Text files (*.txt)|*.txt|htm files (*.htm)|*.htm|html files (*.html)|*.html|All "& _ 
    "files (*.*)|*.*"""
        '
        'MultiPage1
        '
        Me.MultiPage1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage0)
        Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage1)
        Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage2)
        Me.MultiPage1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.MultiPage1.ItemSize = New System.Drawing.Size(42, 18)
        Me.MultiPage1.Location = New System.Drawing.Point(0, 1)
        Me.MultiPage1.Name = "MultiPage1"
        Me.MultiPage1.SelectedIndex = 1
        Me.MultiPage1.Size = New System.Drawing.Size(645, 450)
        Me.MultiPage1.TabIndex = 0
        '
        '_MultiPage1_TabPage0
        '
        Me._MultiPage1_TabPage0.Controls.Add(Me.ListView1)
        Me._MultiPage1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._MultiPage1_TabPage0.Name = "_MultiPage1_TabPage0"
        Me._MultiPage1_TabPage0.Size = New System.Drawing.Size(637, 424)
        Me._MultiPage1_TabPage0.TabIndex = 0
        Me._MultiPage1_TabPage0.Text = "Intermediate Area"
        '
        'ListView1
        '
        Me.ListView1.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.ListView1.BackColor = System.Drawing.SystemColors.Window
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView1_ColumnHeader_1, Me._ListView1_ColumnHeader_2, Me._ListView1_ColumnHeader_3, Me._ListView1_ColumnHeader_4, Me._ListView1_ColumnHeader_5, Me._ListView1_ColumnHeader_6})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListView1.FullRowSelect = true
        Me.ListView1.GridLines = true
        Me.ListView1.LabelWrap = false
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(637, 424)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = false
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
        Me._ListView1_ColumnHeader_5.Text = "Req. Elev"
        Me._ListView1_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView1_ColumnHeader_5.Width = 170
        '
        '_ListView1_ColumnHeader_6
        '
        Me._ListView1_ColumnHeader_6.Text = "Area"
        Me._ListView1_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView1_ColumnHeader_6.Width = 170
        '
        '_MultiPage1_TabPage1
        '
        Me._MultiPage1_TabPage1.Controls.Add(Me.ListView2)
        Me._MultiPage1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._MultiPage1_TabPage1.Name = "_MultiPage1_TabPage1"
        Me._MultiPage1_TabPage1.Size = New System.Drawing.Size(592, 440)
        Me._MultiPage1_TabPage1.TabIndex = 1
        Me._MultiPage1_TabPage1.Text = "Initial Turn Area"
        '
        'ListView2
        '
        Me.ListView2.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.ListView2.BackColor = System.Drawing.SystemColors.Window
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView2_ColumnHeader_1, Me._ListView2_ColumnHeader_2, Me._ListView2_ColumnHeader_3, Me._ListView2_ColumnHeader_4, Me._ListView2_ColumnHeader_5, Me._ListView2_ColumnHeader_6})
        Me.ListView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ListView2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListView2.FullRowSelect = true
        Me.ListView2.GridLines = true
        Me.ListView2.Location = New System.Drawing.Point(0, 0)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(592, 440)
        Me.ListView2.SmallImageList = Me.ImageList1
        Me.ListView2.TabIndex = 2
        Me.ListView2.UseCompatibleStateImageBehavior = false
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
        Me._ListView2_ColumnHeader_5.Text = "Req. Alt"
        Me._ListView2_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_5.Width = 170
        '
        '_ListView2_ColumnHeader_6
        '
        Me._ListView2_ColumnHeader_6.Text = "Area"
        Me._ListView2_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_6.Width = 170
        '
        '_MultiPage1_TabPage2
        '
        Me._MultiPage1_TabPage2.Controls.Add(Me.ListView3)
        Me._MultiPage1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._MultiPage1_TabPage2.Name = "_MultiPage1_TabPage2"
        Me._MultiPage1_TabPage2.Size = New System.Drawing.Size(592, 440)
        Me._MultiPage1_TabPage2.TabIndex = 2
        Me._MultiPage1_TabPage2.Text = "Straight Initial Area"
        '
        'ListView3
        '
        Me.ListView3.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.ListView3.BackColor = System.Drawing.SystemColors.Window
        Me.ListView3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView3_ColumnHeader_1, Me._ListView3_ColumnHeader_2, Me._ListView3_ColumnHeader_3, Me._ListView3_ColumnHeader_4, Me._ListView3_ColumnHeader_5, Me._ListView3_ColumnHeader_6})
        Me.ListView3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ListView3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListView3.FullRowSelect = true
        Me.ListView3.GridLines = true
        Me.ListView3.Location = New System.Drawing.Point(0, 0)
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Size = New System.Drawing.Size(592, 440)
        Me.ListView3.SmallImageList = Me.ImageList1
        Me.ListView3.TabIndex = 3
        Me.ListView3.UseCompatibleStateImageBehavior = false
        Me.ListView3.View = System.Windows.Forms.View.Details
        '
        '_ListView3_ColumnHeader_1
        '
        Me._ListView3_ColumnHeader_1.Text = "Name"
        Me._ListView3_ColumnHeader_1.Width = 170
        '
        '_ListView3_ColumnHeader_2
        '
        Me._ListView3_ColumnHeader_2.Text = "ID"
        Me._ListView3_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView3_ColumnHeader_2.Width = 170
        '
        '_ListView3_ColumnHeader_3
        '
        Me._ListView3_ColumnHeader_3.Text = "Elevation"
        Me._ListView3_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView3_ColumnHeader_3.Width = 170
        '
        '_ListView3_ColumnHeader_4
        '
        Me._ListView3_ColumnHeader_4.Text = "MOC"
        Me._ListView3_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView3_ColumnHeader_4.Width = 170
        '
        '_ListView3_ColumnHeader_5
        '
        Me._ListView3_ColumnHeader_5.Text = "Req. Elev"
        Me._ListView3_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView3_ColumnHeader_5.Width = 170
        '
        '_ListView3_ColumnHeader_6
        '
        Me._ListView3_ColumnHeader_6.Text = "Area"
        Me._ListView3_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView3_ColumnHeader_6.Width = 170
        '
        'CloseBtn
        '
        Me.CloseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.CloseBtn.Image = CType(resources.GetObject("CloseBtn.Image"),System.Drawing.Image)
        Me.CloseBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.CloseBtn.Location = New System.Drawing.Point(545, 456)
        Me.CloseBtn.Name = "CloseBtn"
        Me.CloseBtn.Size = New System.Drawing.Size(92, 25)
        Me.CloseBtn.TabIndex = 5
        '
        'SaveBtn
        '
        Me.SaveBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.SaveBtn.Image = CType(resources.GetObject("SaveBtn.Image"),System.Drawing.Image)
        Me.SaveBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SaveBtn.Location = New System.Drawing.Point(449, 456)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(92, 25)
        Me.SaveBtn.TabIndex = 4
        '
        'lblCountNumber
        '
        Me.lblCountNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.lblCountNumber.AutoSize = true
        Me.lblCountNumber.Location = New System.Drawing.Point(101, 463)
        Me.lblCountNumber.Name = "lblCountNumber"
        Me.lblCountNumber.Size = New System.Drawing.Size(13, 14)
        Me.lblCountNumber.TabIndex = 16
        Me.lblCountNumber.Text = "0"
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.lblCount.AutoSize = true
        Me.lblCount.Location = New System.Drawing.Point(12, 463)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(87, 14)
        Me.lblCount.TabIndex = 15
        Me.lblCount.Text = "Obstacle Count :"
        '
        'CDeadApproachReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 14!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(645, 486)
        Me.Controls.Add(Me.lblCountNumber)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.MultiPage1)
        Me.Controls.Add(Me.CloseBtn)
        Me.Controls.Add(Me.SaveBtn)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.KeyPreview = true
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Name = "CDeadApproachReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = false
        Me.Text = "PANDA Calculation Report"
        Me.MultiPage1.ResumeLayout(false)
        Me._MultiPage1_TabPage0.ResumeLayout(false)
        Me._MultiPage1_TabPage1.ResumeLayout(false)
        Me._MultiPage1_TabPage2.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
	Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lblCountNumber As Label
    Friend WithEvents lblCount As Label
#End Region
End Class