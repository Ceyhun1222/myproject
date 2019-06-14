<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CInitialReportsFrm
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
	Public WithEvents _ListView2_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView2_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView2 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents _ListView1_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView1_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents ListView1 As System.Windows.Forms.ListView
	Public WithEvents _MultiPage1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents MultiPage1 As System.Windows.Forms.TabControl
	Public WithEvents CloseBtn As System.Windows.Forms.Button
	Public WithEvents SaveBtn As System.Windows.Forms.Button
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CInitialReportsFrm))
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MultiPage1 = New System.Windows.Forms.TabControl()
        Me._MultiPage1_TabPage0 = New System.Windows.Forms.TabPage()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me._ListView2_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView2_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._MultiPage1_TabPage1 = New System.Windows.Forms.TabPage()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me._ListView1_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._ListView1_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.CloseBtn = New System.Windows.Forms.Button()
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.MultiPage1.SuspendLayout
        Me._MultiPage1_TabPage0.SuspendLayout
        Me._MultiPage1_TabPage1.SuspendLayout
        CType(Me.UpDown1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'SaveDlg
        '
        Me.SaveDlg.DefaultExt = "txt"
        Me.SaveDlg.Filter = "Text files (*.txt)|*.txt|htm files (*.htm)|*.htm|html files (*.html)|*.html|All f"& _ 
    "iles (*.*)|*.*"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "space.ico")
        '
        'MultiPage1
        '
        Me.MultiPage1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage0)
        Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage1)
        Me.MultiPage1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.MultiPage1.ItemSize = New System.Drawing.Size(42, 18)
        Me.MultiPage1.Location = New System.Drawing.Point(2, 2)
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
        Me._MultiPage1_TabPage0.Text = "Current obstructions"
        '
        'ListView2
        '
        Me.ListView2.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.ListView2.BackColor = System.Drawing.SystemColors.Window
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView2_ColumnHeader_1, Me._ListView2_ColumnHeader_2, Me._ListView2_ColumnHeader_3, Me._ListView2_ColumnHeader_4, Me._ListView2_ColumnHeader_5, Me._ListView2_ColumnHeader_6, Me._ListView2_ColumnHeader_7, Me._ListView2_ColumnHeader_8, Me._ListView2_ColumnHeader_9, Me._ListView2_ColumnHeader_10, Me._ListView2_ColumnHeader_11})
        Me.ListView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ListView2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListView2.FullRowSelect = true
        Me.ListView2.GridLines = true
        Me.ListView2.Location = New System.Drawing.Point(0, 0)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(581, 440)
        Me.ListView2.SmallImageList = Me.ImageList1
        Me.ListView2.TabIndex = 1
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
        Me._ListView2_ColumnHeader_3.Text = "Height"
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
        Me._ListView2_ColumnHeader_5.Text = "Req. H"
        Me._ListView2_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_5.Width = 170
        '
        '_ListView2_ColumnHeader_6
        '
        Me._ListView2_ColumnHeader_6.Text = "Req PDG"
        Me._ListView2_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_6.Width = 170
        '
        '_ListView2_ColumnHeader_7
        '
        Me._ListView2_ColumnHeader_7.Text = "Area"
        Me._ListView2_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_7.Width = 170
        '
        '_ListView2_ColumnHeader_8
        '
        Me._ListView2_ColumnHeader_8.Text = "Dist"
        Me._ListView2_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_8.Width = 170
        '
        '_ListView2_ColumnHeader_9
        '
        Me._ListView2_ColumnHeader_9.Text = "RMin"
        Me._ListView2_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_9.Width = 170
        '
        '_ListView2_ColumnHeader_10
        '
        Me._ListView2_ColumnHeader_10.Text = "RMax (Xs)"
        Me._ListView2_ColumnHeader_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_10.Width = 170
        '
        '_ListView2_ColumnHeader_11
        '
        Me._ListView2_ColumnHeader_11.Text = "Turn Dist"
        Me._ListView2_ColumnHeader_11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView2_ColumnHeader_11.Width = 170
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
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView1_ColumnHeader_1, Me._ListView1_ColumnHeader_2, Me._ListView1_ColumnHeader_3, Me._ListView1_ColumnHeader_4, Me._ListView1_ColumnHeader_5, Me._ListView1_ColumnHeader_6, Me._ListView1_ColumnHeader_7, Me._ListView1_ColumnHeader_8})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListView1.FullRowSelect = true
        Me.ListView1.GridLines = true
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(581, 440)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.TabIndex = 2
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
        Me._ListView1_ColumnHeader_3.Text = "Height"
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
        Me._ListView1_ColumnHeader_5.Text = "Req. H"
        Me._ListView1_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView1_ColumnHeader_5.Width = 170
        '
        '_ListView1_ColumnHeader_6
        '
        Me._ListView1_ColumnHeader_6.Text = "Req PDG"
        Me._ListView1_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView1_ColumnHeader_6.Width = 170
        '
        '_ListView1_ColumnHeader_7
        '
        Me._ListView1_ColumnHeader_7.Text = "Dist"
        Me._ListView1_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView1_ColumnHeader_7.Width = 170
        '
        '_ListView1_ColumnHeader_8
        '
        Me._ListView1_ColumnHeader_8.Text = "Area"
        Me._ListView1_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._ListView1_ColumnHeader_8.Width = 170
        '
        'CloseBtn
        '
        Me.CloseBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.CloseBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseBtn.Image = CType(resources.GetObject("CloseBtn.Image"),System.Drawing.Image)
        Me.CloseBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.CloseBtn.Location = New System.Drawing.Point(498, 477)
        Me.CloseBtn.Name = "CloseBtn"
        Me.CloseBtn.Size = New System.Drawing.Size(92, 25)
        Me.CloseBtn.TabIndex = 9
        '
        'SaveBtn
        '
        Me.SaveBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.SaveBtn.Image = CType(resources.GetObject("SaveBtn.Image"),System.Drawing.Image)
        Me.SaveBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SaveBtn.Location = New System.Drawing.Point(405, 477)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(92, 25)
        Me.SaveBtn.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = true
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(179, 483)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(143, 14)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Высота конца сегмента (метр):"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = true
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(354, 484)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(15, 14)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "--"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = true
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(15, 482)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(52, 14)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Segment:"
        Me.Label1.Visible = false
        '
        'UpDown1
        '
        Me.UpDown1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.UpDown1.Location = New System.Drawing.Point(90, 479)
        Me.UpDown1.Maximum = New Decimal(New Integer() {0, 0, 0, 0})
        Me.UpDown1.Name = "UpDown1"
        Me.UpDown1.Size = New System.Drawing.Size(83, 20)
        Me.UpDown1.TabIndex = 11
        '
        'CInitialReportsFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 14!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CloseBtn
        Me.ClientSize = New System.Drawing.Size(593, 511)
        Me.Controls.Add(Me.UpDown1)
        Me.Controls.Add(Me.MultiPage1)
        Me.Controls.Add(Me.CloseBtn)
        Me.Controls.Add(Me.SaveBtn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.KeyPreview = true
        Me.Location = New System.Drawing.Point(3, 22)
        Me.Name = "CInitialReportsFrm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = false
        Me.Text = "PANDA Calculation Report"
        Me.MultiPage1.ResumeLayout(false)
        Me._MultiPage1_TabPage0.ResumeLayout(false)
        Me._MultiPage1_TabPage1.ResumeLayout(false)
        CType(Me.UpDown1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
	Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
	Friend WithEvents UpDown1 As System.Windows.Forms.NumericUpDown
#End Region
End Class