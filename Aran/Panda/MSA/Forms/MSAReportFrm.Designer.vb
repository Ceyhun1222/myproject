<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CMSAReportFrm
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
	Public WithEvents HideBtn As System.Windows.Forms.Button
    Public WithEvents SaveBtn As System.Windows.Forms.Button
    Public WithEvents Combo1 As System.Windows.Forms.ComboBox
    Public WithEvents _ListView1_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView1_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Public WithEvents ListView1 As System.Windows.Forms.ListView
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CMSAReportFrm))
		Me.HideBtn = New System.Windows.Forms.Button()
		Me.SaveBtn = New System.Windows.Forms.Button()
		Me.Combo1 = New System.Windows.Forms.ComboBox()
		Me.ListView1 = New System.Windows.Forms.ListView()
		Me._ListView1_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView1_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.SaveDialog1 = New System.Windows.Forms.SaveFileDialog()
		Me.SuspendLayout()
		'
		'HideBtn
		'
		Me.HideBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.HideBtn.BackColor = System.Drawing.SystemColors.Control
		Me.HideBtn.Cursor = System.Windows.Forms.Cursors.Default
		Me.HideBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.HideBtn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.HideBtn.Location = New System.Drawing.Point(450, 381)
		Me.HideBtn.Name = "HideBtn"
		Me.HideBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HideBtn.Size = New System.Drawing.Size(70, 25)
		Me.HideBtn.TabIndex = 4
		Me.HideBtn.Text = "&Hide"
		Me.HideBtn.UseVisualStyleBackColor = False
		'
		'SaveBtn
		'
		Me.SaveBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SaveBtn.BackColor = System.Drawing.SystemColors.Control
		Me.SaveBtn.Cursor = System.Windows.Forms.Cursors.Default
		Me.SaveBtn.ForeColor = System.Drawing.SystemColors.ControlText
		Me.SaveBtn.Location = New System.Drawing.Point(369, 381)
		Me.SaveBtn.Name = "SaveBtn"
		Me.SaveBtn.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.SaveBtn.Size = New System.Drawing.Size(73, 25)
		Me.SaveBtn.TabIndex = 3
		Me.SaveBtn.Text = "&Save..."
		Me.SaveBtn.UseVisualStyleBackColor = False
		'
		'Combo1
		'
		Me.Combo1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.Combo1.BackColor = System.Drawing.SystemColors.Window
		Me.Combo1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Combo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.Combo1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Combo1.Location = New System.Drawing.Point(60, 383)
		Me.Combo1.Name = "Combo1"
		Me.Combo1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Combo1.Size = New System.Drawing.Size(103, 21)
		Me.Combo1.TabIndex = 2
		'
		'ListView1
		'
		Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
				  Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ListView1.BackColor = System.Drawing.SystemColors.Window
		Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView1_ColumnHeader_1, Me._ListView1_ColumnHeader_2, Me._ListView1_ColumnHeader_3, Me._ListView1_ColumnHeader_4, Me._ListView1_ColumnHeader_5, Me._ListView1_ColumnHeader_6, Me._ListView1_ColumnHeader_7, Me._ListView1_ColumnHeader_8})
		Me.ListView1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView1.FullRowSelect = True
		Me.ListView1.GridLines = True
		Me.ListView1.HideSelection = False
		Me.ListView1.LabelWrap = False
		Me.ListView1.Location = New System.Drawing.Point(3, 3)
		Me.ListView1.Name = "ListView1"
		Me.ListView1.Size = New System.Drawing.Size(523, 361)
		Me.ListView1.SmallImageList = Me.ImageList1
		Me.ListView1.TabIndex = 0
		Me.ListView1.UseCompatibleStateImageBehavior = False
		Me.ListView1.View = System.Windows.Forms.View.Details
		'
		'_ListView1_ColumnHeader_1
		'
		Me._ListView1_ColumnHeader_1.Text = "Название"
		Me._ListView1_ColumnHeader_1.Width = 170
		'
		'_ListView1_ColumnHeader_2
		'
		Me._ListView1_ColumnHeader_2.Text = "ID"
		Me._ListView1_ColumnHeader_2.Width = 170
		'
		'_ListView1_ColumnHeader_3
		'
		Me._ListView1_ColumnHeader_3.Text = "Удаление"
		Me._ListView1_ColumnHeader_3.Width = 170
		'
		'_ListView1_ColumnHeader_4
		'
		Me._ListView1_ColumnHeader_4.Text = "Радиал °"
		Me._ListView1_ColumnHeader_4.Width = 170
		'
		'_ListView1_ColumnHeader_5
		'
		Me._ListView1_ColumnHeader_5.Text = "Абс. высота"
		Me._ListView1_ColumnHeader_5.Width = 170
		'
		'_ListView1_ColumnHeader_6
		'
		Me._ListView1_ColumnHeader_6.Text = "Мин. высота пролёта"
		Me._ListView1_ColumnHeader_6.Width = 170
		'
		'_ListView1_ColumnHeader_7
		'
		Me._ListView1_ColumnHeader_7.Text = "Округ. мин. высота пролёта"
		Me._ListView1_ColumnHeader_7.Width = 170
		'
		'_ListView1_ColumnHeader_8
		'
		Me._ListView1_ColumnHeader_8.Text = "Зона"
		Me._ListView1_ColumnHeader_8.Width = 170
		'
		'ImageList1
		'
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.ImageList1.Images.SetKeyName(0, "")
		Me.ImageList1.Images.SetKeyName(1, "")
		'
		'Label2
		'
		Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(189, 389)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(10, 13)
		Me.Label2.TabIndex = 5
		Me.Label2.Text = "-"
		'
		'Label1
		'
		Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(9, 387)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(46, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Сектор:"
		'
		'SaveDialog1
		'
		Me.SaveDialog1.Filter = "PANDA Report File|*.htm"
		'
		'CMSAReportFrm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.HideBtn
		Me.ClientSize = New System.Drawing.Size(530, 421)
		Me.Controls.Add(Me.HideBtn)
		Me.Controls.Add(Me.SaveBtn)
		Me.Controls.Add(Me.Combo1)
		Me.Controls.Add(Me.ListView1)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Name = "CMSAReportFrm"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.Text = "MSA Report"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents SaveDialog1 As System.Windows.Forms.SaveFileDialog
#End Region
End Class