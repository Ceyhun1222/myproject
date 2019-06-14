<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VisualManoeuvringForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VisualManoeuvringForm))
        Me.btn_ShowPanel = New System.Windows.Forms.CheckBox()
        Me.btn_Cancel = New System.Windows.Forms.Button()
        Me.btn_Ok = New System.Windows.Forms.Button()
        Me.btn_Next = New System.Windows.Forms.Button()
        Me.btn_Prev = New System.Windows.Forms.Button()
        Me.btn_Report = New System.Windows.Forms.CheckBox()
        Me.btn_Profile = New System.Windows.Forms.CheckBox()
        Me.WorkPanel = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lbl_RWYNavaidPage = New System.Windows.Forms.Label()
        Me.lbl_CirclingAreaPage = New System.Windows.Forms.Label()
        Me.lbl_PrescribedTrackPage = New System.Windows.Forms.Label()
        Me.TabInfoFrame = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.TabInfoFrame.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_ShowPanel
        '
        Me.btn_ShowPanel.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn_ShowPanel.BackColor = System.Drawing.SystemColors.Control
        Me.btn_ShowPanel.Checked = True
        Me.btn_ShowPanel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.btn_ShowPanel.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_ShowPanel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_ShowPanel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.btn_ShowPanel.Image = Global.Aran.Panda.Arrival.My.Resources.bmpSHOW_INFO
        Me.btn_ShowPanel.Location = New System.Drawing.Point(572, 0)
        Me.btn_ShowPanel.Name = "btn_ShowPanel"
        Me.btn_ShowPanel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_ShowPanel.Size = New System.Drawing.Size(17, 453)
        Me.btn_ShowPanel.TabIndex = 443
        Me.btn_ShowPanel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn_ShowPanel.UseVisualStyleBackColor = False
        '
        'btn_Cancel
        '
        Me.btn_Cancel.BackColor = System.Drawing.SystemColors.Control
        Me.btn_Cancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Cancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Cancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btn_Cancel.Image = CType(resources.GetObject("btn_Cancel.Image"), System.Drawing.Image)
        Me.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Cancel.Location = New System.Drawing.Point(279, 11)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_Cancel.Size = New System.Drawing.Size(84, 29)
        Me.btn_Cancel.TabIndex = 515
        Me.btn_Cancel.Text = "Cancel"
        Me.btn_Cancel.UseVisualStyleBackColor = False
        '
        'btn_Ok
        '
        Me.btn_Ok.BackColor = System.Drawing.SystemColors.Control
        Me.btn_Ok.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_Ok.Enabled = False
        Me.btn_Ok.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Ok.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btn_Ok.Image = CType(resources.GetObject("btn_Ok.Image"), System.Drawing.Image)
        Me.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Ok.Location = New System.Drawing.Point(188, 11)
        Me.btn_Ok.Name = "btn_Ok"
        Me.btn_Ok.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_Ok.Size = New System.Drawing.Size(84, 29)
        Me.btn_Ok.TabIndex = 514
        Me.btn_Ok.Text = "Ok"
        Me.btn_Ok.UseVisualStyleBackColor = False
        '
        'btn_Next
        '
        Me.btn_Next.BackColor = System.Drawing.SystemColors.Control
        Me.btn_Next.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_Next.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Next.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btn_Next.Image = CType(resources.GetObject("btn_Next.Image"), System.Drawing.Image)
        Me.btn_Next.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Next.Location = New System.Drawing.Point(97, 11)
        Me.btn_Next.Name = "btn_Next"
        Me.btn_Next.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_Next.Size = New System.Drawing.Size(84, 29)
        Me.btn_Next.TabIndex = 513
        Me.btn_Next.Text = "Next"
        Me.btn_Next.UseVisualStyleBackColor = False
        '
        'btn_Prev
        '
        Me.btn_Prev.BackColor = System.Drawing.SystemColors.Control
        Me.btn_Prev.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_Prev.Enabled = False
        Me.btn_Prev.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Prev.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btn_Prev.Image = CType(resources.GetObject("btn_Prev.Image"), System.Drawing.Image)
        Me.btn_Prev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Prev.Location = New System.Drawing.Point(6, 11)
        Me.btn_Prev.Name = "btn_Prev"
        Me.btn_Prev.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_Prev.Size = New System.Drawing.Size(84, 29)
        Me.btn_Prev.TabIndex = 512
        Me.btn_Prev.Text = "Prev"
        Me.btn_Prev.UseVisualStyleBackColor = False
        '
        'btn_Report
        '
        Me.btn_Report.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn_Report.BackColor = System.Drawing.SystemColors.Control
        Me.btn_Report.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_Report.Enabled = False
        Me.btn_Report.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btn_Report.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btn_Report.Image = CType(resources.GetObject("btn_Report.Image"), System.Drawing.Image)
        Me.btn_Report.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Report.Location = New System.Drawing.Point(381, 11)
        Me.btn_Report.Name = "btn_Report"
        Me.btn_Report.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_Report.Size = New System.Drawing.Size(84, 29)
        Me.btn_Report.TabIndex = 511
        Me.btn_Report.Text = "Report"
        Me.btn_Report.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn_Report.UseVisualStyleBackColor = False
        '
        'btn_Profile
        '
        Me.btn_Profile.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn_Profile.BackColor = System.Drawing.SystemColors.Control
        Me.btn_Profile.Cursor = System.Windows.Forms.Cursors.Default
        Me.btn_Profile.Enabled = False
        Me.btn_Profile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btn_Profile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btn_Profile.Image = CType(resources.GetObject("btn_Profile.Image"), System.Drawing.Image)
        Me.btn_Profile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Profile.Location = New System.Drawing.Point(470, 11)
        Me.btn_Profile.Name = "btn_Profile"
        Me.btn_Profile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btn_Profile.Size = New System.Drawing.Size(84, 29)
        Me.btn_Profile.TabIndex = 510
        Me.btn_Profile.Text = "Profile"
        Me.btn_Profile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn_Profile.UseVisualStyleBackColor = False
        '
        'WorkPanel
        '
        Me.WorkPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.WorkPanel.Location = New System.Drawing.Point(6, 7)
        Me.WorkPanel.Name = "WorkPanel"
        Me.WorkPanel.Size = New System.Drawing.Size(560, 400)
        Me.WorkPanel.TabIndex = 516
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btn_Prev)
        Me.GroupBox1.Controls.Add(Me.btn_Cancel)
        Me.GroupBox1.Controls.Add(Me.btn_Profile)
        Me.GroupBox1.Controls.Add(Me.btn_Ok)
        Me.GroupBox1.Controls.Add(Me.btn_Report)
        Me.GroupBox1.Controls.Add(Me.btn_Next)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 407)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(560, 46)
        Me.GroupBox1.TabIndex = 517
        Me.GroupBox1.TabStop = False
        '
        'lbl_RWYNavaidPage
        '
        Me.lbl_RWYNavaidPage.BackColor = System.Drawing.Color.Transparent
        Me.lbl_RWYNavaidPage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lbl_RWYNavaidPage.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.lbl_RWYNavaidPage.Location = New System.Drawing.Point(8, 44)
        Me.lbl_RWYNavaidPage.Name = "lbl_RWYNavaidPage"
        Me.lbl_RWYNavaidPage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_RWYNavaidPage.Size = New System.Drawing.Size(169, 16)
        Me.lbl_RWYNavaidPage.TabIndex = 431
        Me.lbl_RWYNavaidPage.Tag = "2"
        Me.lbl_RWYNavaidPage.Text = "RWY / Navaid"
        Me.lbl_RWYNavaidPage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_CirclingAreaPage
        '
        Me.lbl_CirclingAreaPage.BackColor = System.Drawing.Color.Transparent
        Me.lbl_CirclingAreaPage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lbl_CirclingAreaPage.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.lbl_CirclingAreaPage.Location = New System.Drawing.Point(8, 16)
        Me.lbl_CirclingAreaPage.Name = "lbl_CirclingAreaPage"
        Me.lbl_CirclingAreaPage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_CirclingAreaPage.Size = New System.Drawing.Size(169, 16)
        Me.lbl_CirclingAreaPage.TabIndex = 432
        Me.lbl_CirclingAreaPage.Tag = "3"
        Me.lbl_CirclingAreaPage.Text = "Circling Area"
        Me.lbl_CirclingAreaPage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lbl_PrescribedTrackPage
        '
        Me.lbl_PrescribedTrackPage.BackColor = System.Drawing.Color.Transparent
        Me.lbl_PrescribedTrackPage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lbl_PrescribedTrackPage.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.lbl_PrescribedTrackPage.Location = New System.Drawing.Point(8, 71)
        Me.lbl_PrescribedTrackPage.Name = "lbl_PrescribedTrackPage"
        Me.lbl_PrescribedTrackPage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_PrescribedTrackPage.Size = New System.Drawing.Size(169, 16)
        Me.lbl_PrescribedTrackPage.TabIndex = 433
        Me.lbl_PrescribedTrackPage.Tag = "3"
        Me.lbl_PrescribedTrackPage.Text = "Prescribed Track"
        Me.lbl_PrescribedTrackPage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TabInfoFrame
        '
        Me.TabInfoFrame.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.TabInfoFrame.Controls.Add(Me.lbl_PrescribedTrackPage)
        Me.TabInfoFrame.Controls.Add(Me.lbl_CirclingAreaPage)
        Me.TabInfoFrame.Controls.Add(Me.lbl_RWYNavaidPage)
        Me.TabInfoFrame.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TabInfoFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TabInfoFrame.Location = New System.Drawing.Point(581, -7)
        Me.TabInfoFrame.Name = "TabInfoFrame"
        Me.TabInfoFrame.Padding = New System.Windows.Forms.Padding(0)
        Me.TabInfoFrame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TabInfoFrame.Size = New System.Drawing.Size(185, 460)
        Me.TabInfoFrame.TabIndex = 426
        Me.TabInfoFrame.TabStop = False
        '
        'VisualManoeuvringForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(769, 454)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.WorkPanel)
        Me.Controls.Add(Me.btn_ShowPanel)
        Me.Controls.Add(Me.TabInfoFrame)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "VisualManoeuvringForm"
        Me.ShowInTaskbar = False
        Me.Text = "Visual manoeuvring using prescribed track v1.0"
        Me.GroupBox1.ResumeLayout(False)
        Me.TabInfoFrame.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents btn_ShowPanel As System.Windows.Forms.CheckBox
    Public WithEvents btn_Cancel As System.Windows.Forms.Button
    Public WithEvents btn_Ok As System.Windows.Forms.Button
    Public WithEvents btn_Next As System.Windows.Forms.Button
    Public WithEvents btn_Prev As System.Windows.Forms.Button
    Public WithEvents btn_Report As System.Windows.Forms.CheckBox
    Public WithEvents btn_Profile As System.Windows.Forms.CheckBox
    Friend WithEvents WorkPanel As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents lbl_RWYNavaidPage As System.Windows.Forms.Label
    Public WithEvents lbl_CirclingAreaPage As System.Windows.Forms.Label
    Public WithEvents lbl_PrescribedTrackPage As System.Windows.Forms.Label
    Public WithEvents TabInfoFrame As System.Windows.Forms.GroupBox
End Class
