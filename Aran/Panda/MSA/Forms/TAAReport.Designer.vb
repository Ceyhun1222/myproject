<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TAAReport
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TAAReport))
		Me.MultiPage1 = New System.Windows.Forms.TabControl()
		Me._MultiPage1_TabPage0 = New System.Windows.Forms.TabPage()
		Me.ListView01 = New System.Windows.Forms.ListView()
		Me._ListView01_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView01_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView01_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView01_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView01_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._MultiPage1_TabPage1 = New System.Windows.Forms.TabPage()
		Me.ListView02 = New System.Windows.Forms.ListView()
		Me._ListView02_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView02_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView02_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView02_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView02_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._MultiPage1_TabPage2 = New System.Windows.Forms.TabPage()
		Me.ListView03 = New System.Windows.Forms.ListView()
		Me._ListView03_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView03_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView03_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView03_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me._ListView03_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.HideBtn = New System.Windows.Forms.Button()
		Me.SaveBtn = New System.Windows.Forms.Button()
		Me.SaveDlg = New System.Windows.Forms.SaveFileDialog()
		Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
		Me.MultiPage1.SuspendLayout()
		Me._MultiPage1_TabPage0.SuspendLayout()
		Me._MultiPage1_TabPage1.SuspendLayout()
		Me._MultiPage1_TabPage2.SuspendLayout()
		Me.SuspendLayout()
		'
		'MultiPage1
		'
		Me.MultiPage1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
				  Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage0)
		Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage1)
		Me.MultiPage1.Controls.Add(Me._MultiPage1_TabPage2)
		Me.MultiPage1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.MultiPage1.ItemSize = New System.Drawing.Size(42, 27)
		Me.MultiPage1.Location = New System.Drawing.Point(0, 2)
		Me.MultiPage1.Name = "MultiPage1"
		Me.MultiPage1.SelectedIndex = 3
		Me.MultiPage1.Size = New System.Drawing.Size(546, 369)
		Me.MultiPage1.TabIndex = 12
		'
		'_MultiPage1_TabPage0
		'
		Me._MultiPage1_TabPage0.Controls.Add(Me.ListView01)
		Me._MultiPage1_TabPage0.Location = New System.Drawing.Point(4, 31)
		Me._MultiPage1_TabPage0.Name = "_MultiPage1_TabPage0"
		Me._MultiPage1_TabPage0.Size = New System.Drawing.Size(538, 334)
		Me._MultiPage1_TabPage0.TabIndex = 0
		Me._MultiPage1_TabPage0.Text = "Left TAA"
		'
		'ListView01
		'
		Me.ListView01.BackColor = System.Drawing.SystemColors.Window
		Me.ListView01.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView01_ColumnHeader_1, Me._ListView01_ColumnHeader_2, Me._ListView01_ColumnHeader_3, Me._ListView01_ColumnHeader_4, Me._ListView01_ColumnHeader_5})
		Me.ListView01.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ListView01.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ListView01.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView01.FullRowSelect = True
		Me.ListView01.GridLines = True
		Me.ListView01.HideSelection = False
		Me.ListView01.Location = New System.Drawing.Point(0, 0)
		Me.ListView01.Name = "ListView01"
		Me.ListView01.Size = New System.Drawing.Size(538, 334)
		Me.ListView01.TabIndex = 1
		Me.ListView01.UseCompatibleStateImageBehavior = False
		Me.ListView01.View = System.Windows.Forms.View.Details
		'
		'_ListView01_ColumnHeader_1
		'
		Me._ListView01_ColumnHeader_1.Text = "Name"
		Me._ListView01_ColumnHeader_1.Width = 100
		'
		'_ListView01_ColumnHeader_2
		'
		Me._ListView01_ColumnHeader_2.Text = "ID"
		Me._ListView01_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView01_ColumnHeader_2.Width = 100
		'
		'_ListView01_ColumnHeader_3
		'
		Me._ListView01_ColumnHeader_3.Text = "Elevation"
		Me._ListView01_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView01_ColumnHeader_3.Width = 100
		'
		'_ListView01_ColumnHeader_4
		'
		Me._ListView01_ColumnHeader_4.Text = "MOC"
		Me._ListView01_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView01_ColumnHeader_4.Width = 100
		'
		'_ListView01_ColumnHeader_5
		'
		Me._ListView01_ColumnHeader_5.Text = "Required altitude"
		Me._ListView01_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView01_ColumnHeader_5.Width = 100
		'
		'_MultiPage1_TabPage1
		'
		Me._MultiPage1_TabPage1.Controls.Add(Me.ListView02)
		Me._MultiPage1_TabPage1.Location = New System.Drawing.Point(4, 31)
		Me._MultiPage1_TabPage1.Name = "_MultiPage1_TabPage1"
		Me._MultiPage1_TabPage1.Size = New System.Drawing.Size(538, 334)
		Me._MultiPage1_TabPage1.TabIndex = 1
		Me._MultiPage1_TabPage1.Text = "Central TAA"
		'
		'ListView02
		'
		Me.ListView02.BackColor = System.Drawing.SystemColors.Window
		Me.ListView02.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView02_ColumnHeader_1, Me._ListView02_ColumnHeader_2, Me._ListView02_ColumnHeader_3, Me._ListView02_ColumnHeader_4, Me._ListView02_ColumnHeader_5})
		Me.ListView02.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ListView02.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ListView02.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView02.FullRowSelect = True
		Me.ListView02.GridLines = True
		Me.ListView02.HideSelection = False
		Me.ListView02.Location = New System.Drawing.Point(0, 0)
		Me.ListView02.Name = "ListView02"
		Me.ListView02.Size = New System.Drawing.Size(538, 334)
		Me.ListView02.TabIndex = 2
		Me.ListView02.UseCompatibleStateImageBehavior = False
		Me.ListView02.View = System.Windows.Forms.View.Details
		'
		'_ListView02_ColumnHeader_1
		'
		Me._ListView02_ColumnHeader_1.Text = "Name"
		Me._ListView02_ColumnHeader_1.Width = 100
		'
		'_ListView02_ColumnHeader_2
		'
		Me._ListView02_ColumnHeader_2.Text = "ID"
		Me._ListView02_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView02_ColumnHeader_2.Width = 100
		'
		'_ListView02_ColumnHeader_3
		'
		Me._ListView02_ColumnHeader_3.Text = "Elevation"
		Me._ListView02_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView02_ColumnHeader_3.Width = 100
		'
		'_ListView02_ColumnHeader_4
		'
		Me._ListView02_ColumnHeader_4.Text = "MOC"
		Me._ListView02_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView02_ColumnHeader_4.Width = 100
		'
		'_ListView02_ColumnHeader_5
		'
		Me._ListView02_ColumnHeader_5.Text = "Required altitude"
		Me._ListView02_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView02_ColumnHeader_5.Width = 100
		'
		'_MultiPage1_TabPage2
		'
		Me._MultiPage1_TabPage2.Controls.Add(Me.ListView03)
		Me._MultiPage1_TabPage2.Location = New System.Drawing.Point(4, 31)
		Me._MultiPage1_TabPage2.Name = "_MultiPage1_TabPage2"
		Me._MultiPage1_TabPage2.Size = New System.Drawing.Size(538, 334)
		Me._MultiPage1_TabPage2.TabIndex = 2
		Me._MultiPage1_TabPage2.Text = "Right TAA"
		'
		'ListView03
		'
		Me.ListView03.BackColor = System.Drawing.SystemColors.Window
		Me.ListView03.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView03_ColumnHeader_1, Me._ListView03_ColumnHeader_2, Me._ListView03_ColumnHeader_3, Me._ListView03_ColumnHeader_4, Me._ListView03_ColumnHeader_5})
		Me.ListView03.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ListView03.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ListView03.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListView03.FullRowSelect = True
		Me.ListView03.GridLines = True
		Me.ListView03.HideSelection = False
		Me.ListView03.Location = New System.Drawing.Point(0, 0)
		Me.ListView03.Name = "ListView03"
		Me.ListView03.Size = New System.Drawing.Size(538, 334)
		Me.ListView03.TabIndex = 3
		Me.ListView03.UseCompatibleStateImageBehavior = False
		Me.ListView03.View = System.Windows.Forms.View.Details
		'
		'_ListView03_ColumnHeader_1
		'
		Me._ListView03_ColumnHeader_1.Text = "Name"
		Me._ListView03_ColumnHeader_1.Width = 100
		'
		'_ListView03_ColumnHeader_2
		'
		Me._ListView03_ColumnHeader_2.Text = "ID"
		Me._ListView03_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView03_ColumnHeader_2.Width = 100
		'
		'_ListView03_ColumnHeader_3
		'
		Me._ListView03_ColumnHeader_3.Text = "Elevation"
		Me._ListView03_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView03_ColumnHeader_3.Width = 100
		'
		'_ListView03_ColumnHeader_4
		'
		Me._ListView03_ColumnHeader_4.Text = "MOC"
		Me._ListView03_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView03_ColumnHeader_4.Width = 100
		'
		'_ListView03_ColumnHeader_5
		'
		Me._ListView03_ColumnHeader_5.Text = "Required altitude"
		Me._ListView03_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._ListView03_ColumnHeader_5.Width = 100
		'
		'HideBtn
		'
		Me.HideBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.HideBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.HideBtn.Image = CType(resources.GetObject("HideBtn.Image"), System.Drawing.Image)
		Me.HideBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.HideBtn.Location = New System.Drawing.Point(437, 377)
		Me.HideBtn.Name = "HideBtn"
		Me.HideBtn.Size = New System.Drawing.Size(93, 25)
		Me.HideBtn.TabIndex = 14
		Me.HideBtn.Text = "&Cancel"
		'
		'SaveBtn
		'
		Me.SaveBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SaveBtn.Image = CType(resources.GetObject("SaveBtn.Image"), System.Drawing.Image)
		Me.SaveBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.SaveBtn.Location = New System.Drawing.Point(341, 377)
		Me.SaveBtn.Name = "SaveBtn"
		Me.SaveBtn.Size = New System.Drawing.Size(93, 25)
		Me.SaveBtn.TabIndex = 13
		Me.SaveBtn.Visible = False
		'
		'SaveDlg
		'
		Me.SaveDlg.DefaultExt = "txt"
		Me.SaveDlg.Filter = """Text files (*.txt)|*.txt|htm files (*.htm)|*.htm|html files (*.html)|*.html|All " & _
		  "files (*.*)|*.*"""
		'
		'ImageList1
		'
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.ImageList1.Images.SetKeyName(0, "")
		Me.ImageList1.Images.SetKeyName(1, "")
		Me.ImageList1.Images.SetKeyName(2, "")
		'
		'TAAReport
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(542, 414)
		Me.Controls.Add(Me.MultiPage1)
		Me.Controls.Add(Me.HideBtn)
		Me.Controls.Add(Me.SaveBtn)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "TAAReport"
		Me.ShowInTaskbar = False
		Me.Text = "TAAReport"
		Me.MultiPage1.ResumeLayout(False)
		Me._MultiPage1_TabPage0.ResumeLayout(False)
		Me._MultiPage1_TabPage1.ResumeLayout(False)
		Me._MultiPage1_TabPage2.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Public WithEvents MultiPage1 As System.Windows.Forms.TabControl
	Public WithEvents _MultiPage1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents ListView01 As System.Windows.Forms.ListView
	Public WithEvents _ListView01_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView01_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView01_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _MultiPage1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents ListView02 As System.Windows.Forms.ListView
	Public WithEvents _ListView02_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView02_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView02_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _MultiPage1_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents ListView03 As System.Windows.Forms.ListView
	Public WithEvents _ListView03_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView03_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _ListView03_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents HideBtn As System.Windows.Forms.Button
	Public WithEvents SaveBtn As System.Windows.Forms.Button
	Private WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Friend WithEvents _ListView01_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _ListView01_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _ListView02_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _ListView02_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _ListView03_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _ListView03_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
End Class
