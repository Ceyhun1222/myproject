<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bgl2aixm
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
        Me.xmlfile_txt = New System.Windows.Forms.TextBox()
        Me.xmlfile_btn = New System.Windows.Forms.Button()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.xmlload_btn = New System.Windows.Forms.Button()
        Me.status_line = New System.Windows.Forms.Label()
        Me.adhplist_cmb = New System.Windows.Forms.ComboBox()
        Me.lvwBooks = New System.Windows.Forms.ListView()
        Me.start_btn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'xmlfile_txt
        '
        Me.xmlfile_txt.Location = New System.Drawing.Point(18, 33)
        Me.xmlfile_txt.Name = "xmlfile_txt"
        Me.xmlfile_txt.Size = New System.Drawing.Size(456, 20)
        Me.xmlfile_txt.TabIndex = 0
        '
        'xmlfile_btn
        '
        Me.xmlfile_btn.Location = New System.Drawing.Point(480, 33)
        Me.xmlfile_btn.Name = "xmlfile_btn"
        Me.xmlfile_btn.Size = New System.Drawing.Size(54, 24)
        Me.xmlfile_btn.TabIndex = 1
        Me.xmlfile_btn.Text = "xml ..."
        Me.xmlfile_btn.UseVisualStyleBackColor = True
        '
        'xmlload_btn
        '
        Me.xmlload_btn.Location = New System.Drawing.Point(540, 33)
        Me.xmlload_btn.Name = "xmlload_btn"
        Me.xmlload_btn.Size = New System.Drawing.Size(59, 24)
        Me.xmlload_btn.TabIndex = 2
        Me.xmlload_btn.Text = "xml load"
        Me.xmlload_btn.UseVisualStyleBackColor = True
        '
        'status_line
        '
        Me.status_line.AutoSize = True
        Me.status_line.Location = New System.Drawing.Point(549, 273)
        Me.status_line.Name = "status_line"
        Me.status_line.Size = New System.Drawing.Size(58, 13)
        Me.status_line.TabIndex = 3
        Me.status_line.Text = "processing"
        '
        'adhplist_cmb
        '
        Me.adhplist_cmb.FormattingEnabled = True
        Me.adhplist_cmb.Location = New System.Drawing.Point(18, 69)
        Me.adhplist_cmb.Name = "adhplist_cmb"
        Me.adhplist_cmb.Size = New System.Drawing.Size(124, 21)
        Me.adhplist_cmb.TabIndex = 4
        '
        'lvwBooks
        '
        Me.lvwBooks.Location = New System.Drawing.Point(18, 96)
        Me.lvwBooks.Name = "lvwBooks"
        Me.lvwBooks.Size = New System.Drawing.Size(581, 174)
        Me.lvwBooks.TabIndex = 5
        Me.lvwBooks.UseCompatibleStateImageBehavior = False
        Me.lvwBooks.View = System.Windows.Forms.View.Details
        '
        'start_btn
        '
        Me.start_btn.Location = New System.Drawing.Point(540, 69)
        Me.start_btn.Name = "start_btn"
        Me.start_btn.Size = New System.Drawing.Size(58, 21)
        Me.start_btn.TabIndex = 6
        Me.start_btn.Text = "start"
        Me.start_btn.UseVisualStyleBackColor = True
        '
        'bgl2aixm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(619, 295)
        Me.Controls.Add(Me.start_btn)
        Me.Controls.Add(Me.lvwBooks)
        Me.Controls.Add(Me.adhplist_cmb)
        Me.Controls.Add(Me.status_line)
        Me.Controls.Add(Me.xmlload_btn)
        Me.Controls.Add(Me.xmlfile_btn)
        Me.Controls.Add(Me.xmlfile_txt)
        Me.Name = "bgl2aixm"
        Me.Text = "bgl2aixm5.1 converter"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents xmlfile_txt As System.Windows.Forms.TextBox
    Friend WithEvents xmlfile_btn As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents xmlload_btn As System.Windows.Forms.Button
    Friend WithEvents status_line As System.Windows.Forms.Label
    Friend WithEvents adhplist_cmb As System.Windows.Forms.ComboBox
    Friend WithEvents lvwBooks As System.Windows.Forms.ListView
    Friend WithEvents start_btn As System.Windows.Forms.Button

End Class
