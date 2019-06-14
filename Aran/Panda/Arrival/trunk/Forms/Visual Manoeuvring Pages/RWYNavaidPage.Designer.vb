<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RWYNavaidPage
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbBox_RWYTHR = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBox_TrueBRG = New System.Windows.Forms.TextBox()
        Me.txtBox_MAGBRG = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtBox_ShortestFacARPDist = New System.Windows.Forms.TextBox()
        Me.lbl_ShortestFacARPDist = New System.Windows.Forms.Label()
        Me.lbl_GuidanceFacility = New System.Windows.Forms.Label()
        Me.lstVw_Solutions = New System.Windows.Forms.ListView()
        Me._ListView0101_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._ListView0101_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._ListView0101_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmbBox_TrueBRGRange = New System.Windows.Forms.ComboBox()
        Me.nmrcUpDown_TrueBRGRange = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbBox_GuidanceFacility = New System.Windows.Forms.ComboBox()
        Me.lbl_TrueBRHRange = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lbl_OnOffARDFac = New System.Windows.Forms.Label()
        Me.lbl_NavaidType = New System.Windows.Forms.Label()
        Me.txtBox_TrueBRGRange = New System.Windows.Forms.TextBox()
        Me.grpBox_ARCParameters = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBox_ARCVeticalDimension = New System.Windows.Forms.TextBox()
        Me.lbl_ARCVerticalDimension = New System.Windows.Forms.Label()
        Me.lbl_ARCSemiSpan = New System.Windows.Forms.Label()
        Me.txtBox_ARCSemiSpan = New System.Windows.Forms.TextBox()
        CType(Me.nmrcUpDown_TrueBRGRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBox_ARCParameters.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(31, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "RWY THR :"
        '
        'cmbBox_RWYTHR
        '
        Me.cmbBox_RWYTHR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_RWYTHR.FormattingEnabled = True
        Me.cmbBox_RWYTHR.Location = New System.Drawing.Point(102, 21)
        Me.cmbBox_RWYTHR.Name = "cmbBox_RWYTHR"
        Me.cmbBox_RWYTHR.Size = New System.Drawing.Size(72, 21)
        Me.cmbBox_RWYTHR.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(31, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "TRUE BRG :"
        '
        'txtBox_TrueBRG
        '
        Me.txtBox_TrueBRG.BackColor = System.Drawing.SystemColors.Control
        Me.txtBox_TrueBRG.Location = New System.Drawing.Point(102, 48)
        Me.txtBox_TrueBRG.Name = "txtBox_TrueBRG"
        Me.txtBox_TrueBRG.ReadOnly = True
        Me.txtBox_TrueBRG.Size = New System.Drawing.Size(72, 20)
        Me.txtBox_TrueBRG.TabIndex = 3
        '
        'txtBox_MAGBRG
        '
        Me.txtBox_MAGBRG.BackColor = System.Drawing.SystemColors.Control
        Me.txtBox_MAGBRG.Location = New System.Drawing.Point(102, 74)
        Me.txtBox_MAGBRG.Name = "txtBox_MAGBRG"
        Me.txtBox_MAGBRG.ReadOnly = True
        Me.txtBox_MAGBRG.Size = New System.Drawing.Size(72, 20)
        Me.txtBox_MAGBRG.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(31, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "MAG BRG :"
        '
        'txtBox_ShortestFacARPDist
        '
        Me.txtBox_ShortestFacARPDist.BackColor = System.Drawing.SystemColors.Control
        Me.txtBox_ShortestFacARPDist.Location = New System.Drawing.Point(408, 74)
        Me.txtBox_ShortestFacARPDist.Name = "txtBox_ShortestFacARPDist"
        Me.txtBox_ShortestFacARPDist.ReadOnly = True
        Me.txtBox_ShortestFacARPDist.Size = New System.Drawing.Size(72, 20)
        Me.txtBox_ShortestFacARPDist.TabIndex = 9
        '
        'lbl_ShortestFacARPDist
        '
        Me.lbl_ShortestFacARPDist.AutoSize = True
        Me.lbl_ShortestFacARPDist.Location = New System.Drawing.Point(227, 77)
        Me.lbl_ShortestFacARPDist.Name = "lbl_ShortestFacARPDist"
        Me.lbl_ShortestFacARPDist.Size = New System.Drawing.Size(175, 13)
        Me.lbl_ShortestFacARPDist.TabIndex = 8
        Me.lbl_ShortestFacARPDist.Text = "Shortest facility distacne from ARP :"
        '
        'lbl_GuidanceFacility
        '
        Me.lbl_GuidanceFacility.AutoSize = True
        Me.lbl_GuidanceFacility.Location = New System.Drawing.Point(227, 24)
        Me.lbl_GuidanceFacility.Name = "lbl_GuidanceFacility"
        Me.lbl_GuidanceFacility.Size = New System.Drawing.Size(91, 13)
        Me.lbl_GuidanceFacility.TabIndex = 6
        Me.lbl_GuidanceFacility.Text = "Guidance facility :"
        '
        'lstVw_Solutions
        '
        Me.lstVw_Solutions.BackColor = System.Drawing.SystemColors.Window
        Me.lstVw_Solutions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._ListView0101_ColumnHeader_1, Me._ListView0101_ColumnHeader_2, Me._ListView0101_ColumnHeader_3})
        Me.lstVw_Solutions.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstVw_Solutions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstVw_Solutions.FullRowSelect = True
        Me.lstVw_Solutions.GridLines = True
        Me.lstVw_Solutions.LabelWrap = False
        Me.lstVw_Solutions.Location = New System.Drawing.Point(15, 260)
        Me.lstVw_Solutions.Name = "lstVw_Solutions"
        Me.lstVw_Solutions.Size = New System.Drawing.Size(527, 109)
        Me.lstVw_Solutions.TabIndex = 451
        Me.lstVw_Solutions.UseCompatibleStateImageBehavior = False
        Me.lstVw_Solutions.View = System.Windows.Forms.View.Details
        '
        '_ListView0101_ColumnHeader_1
        '
        Me._ListView0101_ColumnHeader_1.Text = "Alignment"
        Me._ListView0101_ColumnHeader_1.Width = 191
        '
        '_ListView0101_ColumnHeader_2
        '
        Me._ListView0101_ColumnHeader_2.Text = "GEO From"
        Me._ListView0101_ColumnHeader_2.Width = 158
        '
        '_ListView0101_ColumnHeader_3
        '
        Me._ListView0101_ColumnHeader_3.Text = "GEO To"
        Me._ListView0101_ColumnHeader_3.Width = 161
        '
        'cmbBox_TrueBRGRange
        '
        Me.cmbBox_TrueBRGRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_TrueBRGRange.FormattingEnabled = True
        Me.cmbBox_TrueBRGRange.Location = New System.Drawing.Point(146, 149)
        Me.cmbBox_TrueBRGRange.Name = "cmbBox_TrueBRGRange"
        Me.cmbBox_TrueBRGRange.Size = New System.Drawing.Size(121, 21)
        Me.cmbBox_TrueBRGRange.TabIndex = 453
        '
        'nmrcUpDown_TrueBRGRange
        '
        Me.nmrcUpDown_TrueBRGRange.Location = New System.Drawing.Point(376, 150)
        Me.nmrcUpDown_TrueBRGRange.Name = "nmrcUpDown_TrueBRGRange"
        Me.nmrcUpDown_TrueBRGRange.Size = New System.Drawing.Size(17, 20)
        Me.nmrcUpDown_TrueBRGRange.TabIndex = 455
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(176, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(11, 13)
        Me.Label8.TabIndex = 456
        Me.Label8.Text = "°"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(486, 77)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(21, 13)
        Me.Label9.TabIndex = 457
        Me.Label9.Text = "km"
        '
        'cmbBox_GuidanceFacility
        '
        Me.cmbBox_GuidanceFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_GuidanceFacility.FormattingEnabled = True
        Me.cmbBox_GuidanceFacility.Location = New System.Drawing.Point(334, 21)
        Me.cmbBox_GuidanceFacility.Name = "cmbBox_GuidanceFacility"
        Me.cmbBox_GuidanceFacility.Size = New System.Drawing.Size(72, 21)
        Me.cmbBox_GuidanceFacility.TabIndex = 461
        '
        'lbl_TrueBRHRange
        '
        Me.lbl_TrueBRHRange.AutoSize = True
        Me.lbl_TrueBRHRange.Location = New System.Drawing.Point(31, 152)
        Me.lbl_TrueBRHRange.Name = "lbl_TrueBRHRange"
        Me.lbl_TrueBRHRange.Size = New System.Drawing.Size(99, 13)
        Me.lbl_TrueBRHRange.TabIndex = 462
        Me.lbl_TrueBRHRange.Text = "TRUE BRG range :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(176, 51)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(11, 13)
        Me.Label7.TabIndex = 463
        Me.Label7.Text = "°"
        '
        'lbl_OnOffARDFac
        '
        Me.lbl_OnOffARDFac.AutoSize = True
        Me.lbl_OnOffARDFac.Location = New System.Drawing.Point(436, 48)
        Me.lbl_OnOffARDFac.Name = "lbl_OnOffARDFac"
        Me.lbl_OnOffARDFac.Size = New System.Drawing.Size(106, 13)
        Me.lbl_OnOffARDFac.TabIndex = 464
        Me.lbl_OnOffARDFac.Text = "On-aerodrome facility"
        '
        'lbl_NavaidType
        '
        Me.lbl_NavaidType.AutoSize = True
        Me.lbl_NavaidType.Location = New System.Drawing.Point(436, 24)
        Me.lbl_NavaidType.Name = "lbl_NavaidType"
        Me.lbl_NavaidType.Size = New System.Drawing.Size(51, 13)
        Me.lbl_NavaidType.TabIndex = 465
        Me.lbl_NavaidType.Text = "NavType"
        '
        'txtBox_TrueBRGRange
        '
        Me.txtBox_TrueBRGRange.BackColor = System.Drawing.SystemColors.Window
        Me.txtBox_TrueBRGRange.Location = New System.Drawing.Point(273, 150)
        Me.txtBox_TrueBRGRange.Name = "txtBox_TrueBRGRange"
        Me.txtBox_TrueBRGRange.ReadOnly = True
        Me.txtBox_TrueBRGRange.Size = New System.Drawing.Size(104, 20)
        Me.txtBox_TrueBRGRange.TabIndex = 466
        '
        'grpBox_ARCParameters
        '
        Me.grpBox_ARCParameters.Controls.Add(Me.Label10)
        Me.grpBox_ARCParameters.Controls.Add(Me.Label6)
        Me.grpBox_ARCParameters.Controls.Add(Me.txtBox_ARCVeticalDimension)
        Me.grpBox_ARCParameters.Controls.Add(Me.lbl_ARCVerticalDimension)
        Me.grpBox_ARCParameters.Controls.Add(Me.lbl_ARCSemiSpan)
        Me.grpBox_ARCParameters.Controls.Add(Me.txtBox_ARCSemiSpan)
        Me.grpBox_ARCParameters.Location = New System.Drawing.Point(15, 197)
        Me.grpBox_ARCParameters.Name = "grpBox_ARCParameters"
        Me.grpBox_ARCParameters.Size = New System.Drawing.Size(527, 57)
        Me.grpBox_ARCParameters.TabIndex = 467
        Me.grpBox_ARCParameters.TabStop = False
        Me.grpBox_ARCParameters.Text = "Aircraft parameters"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(467, 27)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(15, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "m"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(188, 27)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(15, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "m"
        '
        'txtBox_ARCVeticalDimension
        '
        Me.txtBox_ARCVeticalDimension.Location = New System.Drawing.Point(397, 24)
        Me.txtBox_ARCVeticalDimension.Name = "txtBox_ARCVeticalDimension"
        Me.txtBox_ARCVeticalDimension.ReadOnly = True
        Me.txtBox_ARCVeticalDimension.Size = New System.Drawing.Size(64, 20)
        Me.txtBox_ARCVeticalDimension.TabIndex = 3
        '
        'lbl_ARCVerticalDimension
        '
        Me.lbl_ARCVerticalDimension.AutoSize = True
        Me.lbl_ARCVerticalDimension.Location = New System.Drawing.Point(258, 27)
        Me.lbl_ARCVerticalDimension.Name = "lbl_ARCVerticalDimension"
        Me.lbl_ARCVerticalDimension.Size = New System.Drawing.Size(133, 13)
        Me.lbl_ARCVerticalDimension.TabIndex = 2
        Me.lbl_ARCVerticalDimension.Text = "Aircraft vertical dimension :"
        '
        'lbl_ARCSemiSpan
        '
        Me.lbl_ARCSemiSpan.AutoSize = True
        Me.lbl_ARCSemiSpan.Location = New System.Drawing.Point(16, 27)
        Me.lbl_ARCSemiSpan.Name = "lbl_ARCSemiSpan"
        Me.lbl_ARCSemiSpan.Size = New System.Drawing.Size(96, 13)
        Me.lbl_ARCSemiSpan.TabIndex = 1
        Me.lbl_ARCSemiSpan.Text = "Aircraft semi span :"
        '
        'txtBox_ARCSemiSpan
        '
        Me.txtBox_ARCSemiSpan.Location = New System.Drawing.Point(118, 24)
        Me.txtBox_ARCSemiSpan.Name = "txtBox_ARCSemiSpan"
        Me.txtBox_ARCSemiSpan.ReadOnly = True
        Me.txtBox_ARCSemiSpan.Size = New System.Drawing.Size(64, 20)
        Me.txtBox_ARCSemiSpan.TabIndex = 0
        '
        'RWYNavaidPage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpBox_ARCParameters)
        Me.Controls.Add(Me.txtBox_TrueBRGRange)
        Me.Controls.Add(Me.lbl_NavaidType)
        Me.Controls.Add(Me.lbl_OnOffARDFac)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lbl_TrueBRHRange)
        Me.Controls.Add(Me.cmbBox_GuidanceFacility)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.nmrcUpDown_TrueBRGRange)
        Me.Controls.Add(Me.cmbBox_TrueBRGRange)
        Me.Controls.Add(Me.lstVw_Solutions)
        Me.Controls.Add(Me.txtBox_ShortestFacARPDist)
        Me.Controls.Add(Me.lbl_ShortestFacARPDist)
        Me.Controls.Add(Me.lbl_GuidanceFacility)
        Me.Controls.Add(Me.txtBox_MAGBRG)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtBox_TrueBRG)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbBox_RWYTHR)
        Me.Controls.Add(Me.Label1)
        Me.Name = "RWYNavaidPage"
        Me.Size = New System.Drawing.Size(560, 382)
        CType(Me.nmrcUpDown_TrueBRGRange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBox_ARCParameters.ResumeLayout(False)
        Me.grpBox_ARCParameters.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbBox_RWYTHR As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBox_TrueBRG As System.Windows.Forms.TextBox
    Friend WithEvents txtBox_MAGBRG As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBox_ShortestFacARPDist As System.Windows.Forms.TextBox
    Friend WithEvents lbl_ShortestFacARPDist As System.Windows.Forms.Label
    Friend WithEvents lbl_GuidanceFacility As System.Windows.Forms.Label
    Public WithEvents lstVw_Solutions As System.Windows.Forms.ListView
    Public WithEvents _ListView0101_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView0101_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents _ListView0101_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmbBox_TrueBRGRange As System.Windows.Forms.ComboBox
    Friend WithEvents nmrcUpDown_TrueBRGRange As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbBox_GuidanceFacility As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_TrueBRHRange As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lbl_OnOffARDFac As System.Windows.Forms.Label
    Friend WithEvents lbl_NavaidType As System.Windows.Forms.Label
    Friend WithEvents txtBox_TrueBRGRange As System.Windows.Forms.TextBox
    Friend WithEvents grpBox_ARCParameters As System.Windows.Forms.GroupBox
    Friend WithEvents txtBox_ARCVeticalDimension As System.Windows.Forms.TextBox
    Friend WithEvents lbl_ARCVerticalDimension As System.Windows.Forms.Label
    Friend WithEvents lbl_ARCSemiSpan As System.Windows.Forms.Label
    Friend WithEvents txtBox_ARCSemiSpan As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label

End Class
