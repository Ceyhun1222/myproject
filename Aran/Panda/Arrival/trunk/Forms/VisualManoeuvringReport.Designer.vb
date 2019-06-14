<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VisualManoeuvringReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VisualManoeuvringReport))
        Me.tbCntrl_VisualManoeuvring = New System.Windows.Forms.TabControl()
        Me.tbPg_ReferencePoints = New System.Windows.Forms.TabPage()
        Me.lstVw_ReferencePoints = New System.Windows.Forms.ListView()
        Me._lstVw_RefPnts_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_RefPnts_ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_RefPnts_ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbPg_CirclingAreaObstacles = New System.Windows.Forms.TabPage()
        Me.lstVw_CirclingAreaObstacles = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbPg_LeftCirclingBoxObstacles = New System.Windows.Forms.TabPage()
        Me.lstVw_LeftCirclingBoxObstacles = New System.Windows.Forms.ListView()
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbPg_RightCirclingBoxObstacles = New System.Windows.Forms.TabPage()
        Me.lstVw_RightCirclingBoxObstacles = New System.Windows.Forms.ListView()
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbPg_TrackSteps = New System.Windows.Forms.TabPage()
        Me.lstVw_TrackSteps = New System.Windows.Forms.ListView()
        Me._lstVw_TrckStps_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_TrckStps_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_TrckStps_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_TrckStps_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_TrckStps_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_TrckStps_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbPg_TrackObstacles = New System.Windows.Forms.TabPage()
        Me.lstVw_TrackObstacles = New System.Windows.Forms.ListView()
        Me._lstVw_TrackObstcls_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_Obstcls_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_Obstcls_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_Obstcls_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lstVw_Obstcls_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbPg_TrackStatistics = New System.Windows.Forms.TabPage()
        Me.lbl_meterSign3 = New System.Windows.Forms.Label()
        Me.txtBox_downwindLegLength = New System.Windows.Forms.TextBox()
        Me.lbl_downwindLegLength = New System.Windows.Forms.Label()
        Me.lbl_percentSign = New System.Windows.Forms.Label()
        Me.txtBox_descentGradient = New System.Windows.Forms.TextBox()
        Me.lbl_descentGradient = New System.Windows.Forms.Label()
        Me.lbl_meterSign2 = New System.Windows.Forms.Label()
        Me.lbl_meterSign1 = New System.Windows.Forms.Label()
        Me.txtBox_trackLength = New System.Windows.Forms.TextBox()
        Me.lbl_TrackLength = New System.Windows.Forms.Label()
        Me.txtBox_trackOCH = New System.Windows.Forms.TextBox()
        Me.lbl_TrackOCH = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.tbCntrl_VisualManoeuvring.SuspendLayout()
        Me.tbPg_ReferencePoints.SuspendLayout()
        Me.tbPg_CirclingAreaObstacles.SuspendLayout()
        Me.tbPg_LeftCirclingBoxObstacles.SuspendLayout()
        Me.tbPg_RightCirclingBoxObstacles.SuspendLayout()
        Me.tbPg_TrackSteps.SuspendLayout()
        Me.tbPg_TrackObstacles.SuspendLayout()
        Me.tbPg_TrackStatistics.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbCntrl_VisualManoeuvring
        '
        Me.tbCntrl_VisualManoeuvring.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_ReferencePoints)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_CirclingAreaObstacles)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_LeftCirclingBoxObstacles)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_RightCirclingBoxObstacles)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_TrackSteps)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_TrackObstacles)
        Me.tbCntrl_VisualManoeuvring.Controls.Add(Me.tbPg_TrackStatistics)
        Me.tbCntrl_VisualManoeuvring.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.tbCntrl_VisualManoeuvring.Location = New System.Drawing.Point(0, 0)
        Me.tbCntrl_VisualManoeuvring.Name = "tbCntrl_VisualManoeuvring"
        Me.tbCntrl_VisualManoeuvring.SelectedIndex = 0
        Me.tbCntrl_VisualManoeuvring.Size = New System.Drawing.Size(605, 349)
        Me.tbCntrl_VisualManoeuvring.TabIndex = 0
        '
        'tbPg_ReferencePoints
        '
        Me.tbPg_ReferencePoints.Controls.Add(Me.lstVw_ReferencePoints)
        Me.tbPg_ReferencePoints.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_ReferencePoints.Name = "tbPg_ReferencePoints"
        Me.tbPg_ReferencePoints.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPg_ReferencePoints.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_ReferencePoints.TabIndex = 0
        Me.tbPg_ReferencePoints.Text = "Reference Points"
        Me.tbPg_ReferencePoints.UseVisualStyleBackColor = True
        '
        'lstVw_ReferencePoints
        '
        Me.lstVw_ReferencePoints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstVw_ReferencePoints.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstVw_RefPnts_ColumnHeader_1, Me._lstVw_RefPnts_ColumnHeader2, Me._lstVw_RefPnts_ColumnHeader3})
        Me.lstVw_ReferencePoints.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lstVw_ReferencePoints.FullRowSelect = True
        Me.lstVw_ReferencePoints.GridLines = True
        Me.lstVw_ReferencePoints.HideSelection = False
        Me.lstVw_ReferencePoints.Location = New System.Drawing.Point(0, 0)
        Me.lstVw_ReferencePoints.Name = "lstVw_ReferencePoints"
        Me.lstVw_ReferencePoints.Size = New System.Drawing.Size(597, 322)
        Me.lstVw_ReferencePoints.TabIndex = 0
        Me.lstVw_ReferencePoints.UseCompatibleStateImageBehavior = False
        Me.lstVw_ReferencePoints.View = System.Windows.Forms.View.Details
        '
        '_lstVw_RefPnts_ColumnHeader_1
        '
        Me._lstVw_RefPnts_ColumnHeader_1.Text = "Name"
        Me._lstVw_RefPnts_ColumnHeader_1.Width = 170
        '
        '_lstVw_RefPnts_ColumnHeader2
        '
        Me._lstVw_RefPnts_ColumnHeader2.Text = "Type"
        Me._lstVw_RefPnts_ColumnHeader2.Width = 170
        '
        '_lstVw_RefPnts_ColumnHeader3
        '
        Me._lstVw_RefPnts_ColumnHeader3.Text = "Description"
        Me._lstVw_RefPnts_ColumnHeader3.Width = 170
        '
        'tbPg_CirclingAreaObstacles
        '
        Me.tbPg_CirclingAreaObstacles.Controls.Add(Me.lstVw_CirclingAreaObstacles)
        Me.tbPg_CirclingAreaObstacles.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_CirclingAreaObstacles.Name = "tbPg_CirclingAreaObstacles"
        Me.tbPg_CirclingAreaObstacles.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_CirclingAreaObstacles.TabIndex = 5
        Me.tbPg_CirclingAreaObstacles.Text = "Circling Area Obstacles"
        Me.tbPg_CirclingAreaObstacles.UseVisualStyleBackColor = True
        '
        'lstVw_CirclingAreaObstacles
        '
        Me.lstVw_CirclingAreaObstacles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstVw_CirclingAreaObstacles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lstVw_CirclingAreaObstacles.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstVw_CirclingAreaObstacles.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lstVw_CirclingAreaObstacles.FullRowSelect = True
        Me.lstVw_CirclingAreaObstacles.GridLines = True
        Me.lstVw_CirclingAreaObstacles.HideSelection = False
        Me.lstVw_CirclingAreaObstacles.Location = New System.Drawing.Point(0, 0)
        Me.lstVw_CirclingAreaObstacles.Name = "lstVw_CirclingAreaObstacles"
        Me.lstVw_CirclingAreaObstacles.Size = New System.Drawing.Size(597, 322)
        Me.lstVw_CirclingAreaObstacles.TabIndex = 3
        Me.lstVw_CirclingAreaObstacles.UseCompatibleStateImageBehavior = False
        Me.lstVw_CirclingAreaObstacles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 170
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Height"
        Me.ColumnHeader2.Width = 170
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "MOC"
        Me.ColumnHeader3.Width = 170
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Req. H"
        Me.ColumnHeader4.Width = 170
        '
        'tbPg_LeftCirclingBoxObstacles
        '
        Me.tbPg_LeftCirclingBoxObstacles.Controls.Add(Me.lstVw_LeftCirclingBoxObstacles)
        Me.tbPg_LeftCirclingBoxObstacles.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_LeftCirclingBoxObstacles.Name = "tbPg_LeftCirclingBoxObstacles"
        Me.tbPg_LeftCirclingBoxObstacles.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_LeftCirclingBoxObstacles.TabIndex = 6
        Me.tbPg_LeftCirclingBoxObstacles.Text = "Left Box Obstacles"
        Me.tbPg_LeftCirclingBoxObstacles.UseVisualStyleBackColor = True
        '
        'lstVw_LeftCirclingBoxObstacles
        '
        Me.lstVw_LeftCirclingBoxObstacles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstVw_LeftCirclingBoxObstacles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lstVw_LeftCirclingBoxObstacles.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstVw_LeftCirclingBoxObstacles.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lstVw_LeftCirclingBoxObstacles.FullRowSelect = True
        Me.lstVw_LeftCirclingBoxObstacles.GridLines = True
        Me.lstVw_LeftCirclingBoxObstacles.HideSelection = False
        Me.lstVw_LeftCirclingBoxObstacles.Location = New System.Drawing.Point(0, 0)
        Me.lstVw_LeftCirclingBoxObstacles.Name = "lstVw_LeftCirclingBoxObstacles"
        Me.lstVw_LeftCirclingBoxObstacles.Size = New System.Drawing.Size(597, 322)
        Me.lstVw_LeftCirclingBoxObstacles.TabIndex = 4
        Me.lstVw_LeftCirclingBoxObstacles.UseCompatibleStateImageBehavior = False
        Me.lstVw_LeftCirclingBoxObstacles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Name"
        Me.ColumnHeader5.Width = 170
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Height"
        Me.ColumnHeader6.Width = 170
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "MOC"
        Me.ColumnHeader7.Width = 170
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Req. H"
        Me.ColumnHeader8.Width = 170
        '
        'tbPg_RightCirclingBoxObstacles
        '
        Me.tbPg_RightCirclingBoxObstacles.Controls.Add(Me.lstVw_RightCirclingBoxObstacles)
        Me.tbPg_RightCirclingBoxObstacles.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_RightCirclingBoxObstacles.Name = "tbPg_RightCirclingBoxObstacles"
        Me.tbPg_RightCirclingBoxObstacles.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_RightCirclingBoxObstacles.TabIndex = 7
        Me.tbPg_RightCirclingBoxObstacles.Text = "Right Box Obstacles"
        Me.tbPg_RightCirclingBoxObstacles.UseVisualStyleBackColor = True
        '
        'lstVw_RightCirclingBoxObstacles
        '
        Me.lstVw_RightCirclingBoxObstacles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstVw_RightCirclingBoxObstacles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader12})
        Me.lstVw_RightCirclingBoxObstacles.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstVw_RightCirclingBoxObstacles.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lstVw_RightCirclingBoxObstacles.FullRowSelect = True
        Me.lstVw_RightCirclingBoxObstacles.GridLines = True
        Me.lstVw_RightCirclingBoxObstacles.HideSelection = False
        Me.lstVw_RightCirclingBoxObstacles.Location = New System.Drawing.Point(0, 0)
        Me.lstVw_RightCirclingBoxObstacles.Name = "lstVw_RightCirclingBoxObstacles"
        Me.lstVw_RightCirclingBoxObstacles.Size = New System.Drawing.Size(597, 322)
        Me.lstVw_RightCirclingBoxObstacles.TabIndex = 4
        Me.lstVw_RightCirclingBoxObstacles.UseCompatibleStateImageBehavior = False
        Me.lstVw_RightCirclingBoxObstacles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Name"
        Me.ColumnHeader9.Width = 170
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Height"
        Me.ColumnHeader10.Width = 170
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "MOC"
        Me.ColumnHeader11.Width = 170
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Req. H"
        Me.ColumnHeader12.Width = 170
        '
        'tbPg_TrackSteps
        '
        Me.tbPg_TrackSteps.Controls.Add(Me.lstVw_TrackSteps)
        Me.tbPg_TrackSteps.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_TrackSteps.Name = "tbPg_TrackSteps"
        Me.tbPg_TrackSteps.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPg_TrackSteps.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_TrackSteps.TabIndex = 1
        Me.tbPg_TrackSteps.Text = "Track Steps"
        Me.tbPg_TrackSteps.UseVisualStyleBackColor = True
        '
        'lstVw_TrackSteps
        '
        Me.lstVw_TrackSteps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstVw_TrackSteps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstVw_TrckStps_ColumnHeader_1, Me._lstVw_TrckStps_ColumnHeader_2, Me._lstVw_TrckStps_ColumnHeader_3, Me._lstVw_TrckStps_ColumnHeader_4, Me._lstVw_TrckStps_ColumnHeader_5, Me._lstVw_TrckStps_ColumnHeader_6})
        Me.lstVw_TrackSteps.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lstVw_TrackSteps.FullRowSelect = True
        Me.lstVw_TrackSteps.GridLines = True
        Me.lstVw_TrackSteps.HideSelection = False
        Me.lstVw_TrackSteps.Location = New System.Drawing.Point(0, 0)
        Me.lstVw_TrackSteps.Name = "lstVw_TrackSteps"
        Me.lstVw_TrackSteps.Size = New System.Drawing.Size(597, 322)
        Me.lstVw_TrackSteps.TabIndex = 1
        Me.lstVw_TrackSteps.UseCompatibleStateImageBehavior = False
        Me.lstVw_TrackSteps.View = System.Windows.Forms.View.Details
        '
        '_lstVw_TrckStps_ColumnHeader_1
        '
        Me._lstVw_TrckStps_ColumnHeader_1.Text = "Name"
        Me._lstVw_TrckStps_ColumnHeader_1.Width = 170
        '
        '_lstVw_TrckStps_ColumnHeader_2
        '
        Me._lstVw_TrckStps_ColumnHeader_2.Text = "Divergence angle"
        Me._lstVw_TrckStps_ColumnHeader_2.Width = 170
        '
        '_lstVw_TrckStps_ColumnHeader_3
        '
        Me._lstVw_TrckStps_ColumnHeader_3.Text = "Convergence angle"
        Me._lstVw_TrckStps_ColumnHeader_3.Width = 170
        '
        '_lstVw_TrckStps_ColumnHeader_4
        '
        Me._lstVw_TrckStps_ColumnHeader_4.Text = "DIstance between maneuvers"
        Me._lstVw_TrckStps_ColumnHeader_4.Width = 170
        '
        '_lstVw_TrckStps_ColumnHeader_5
        '
        Me._lstVw_TrckStps_ColumnHeader_5.Text = "Divergence point"
        Me._lstVw_TrckStps_ColumnHeader_5.Width = 170
        '
        '_lstVw_TrckStps_ColumnHeader_6
        '
        Me._lstVw_TrckStps_ColumnHeader_6.Text = "Convergence point"
        Me._lstVw_TrckStps_ColumnHeader_6.Width = 170
        '
        'tbPg_TrackObstacles
        '
        Me.tbPg_TrackObstacles.Controls.Add(Me.lstVw_TrackObstacles)
        Me.tbPg_TrackObstacles.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_TrackObstacles.Name = "tbPg_TrackObstacles"
        Me.tbPg_TrackObstacles.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_TrackObstacles.TabIndex = 2
        Me.tbPg_TrackObstacles.Text = "Track Obstacles"
        Me.tbPg_TrackObstacles.UseVisualStyleBackColor = True
        '
        'lstVw_TrackObstacles
        '
        Me.lstVw_TrackObstacles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstVw_TrackObstacles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lstVw_TrackObstcls_ColumnHeader_1, Me._lstVw_Obstcls_ColumnHeader_2, Me._lstVw_Obstcls_ColumnHeader_3, Me._lstVw_Obstcls_ColumnHeader_4, Me._lstVw_Obstcls_ColumnHeader_5})
        Me.lstVw_TrackObstacles.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstVw_TrackObstacles.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lstVw_TrackObstacles.FullRowSelect = True
        Me.lstVw_TrackObstacles.GridLines = True
        Me.lstVw_TrackObstacles.HideSelection = False
        Me.lstVw_TrackObstacles.Location = New System.Drawing.Point(0, 0)
        Me.lstVw_TrackObstacles.Name = "lstVw_TrackObstacles"
        Me.lstVw_TrackObstacles.Size = New System.Drawing.Size(597, 322)
        Me.lstVw_TrackObstacles.TabIndex = 1
        Me.lstVw_TrackObstacles.UseCompatibleStateImageBehavior = False
        Me.lstVw_TrackObstacles.View = System.Windows.Forms.View.Details
        '
        '_lstVw_TrackObstcls_ColumnHeader_1
        '
        Me._lstVw_TrackObstcls_ColumnHeader_1.Text = "Name"
        Me._lstVw_TrackObstcls_ColumnHeader_1.Width = 170
        '
        '_lstVw_Obstcls_ColumnHeader_2
        '
        Me._lstVw_Obstcls_ColumnHeader_2.Text = "Step"
        Me._lstVw_Obstcls_ColumnHeader_2.Width = 170
        '
        '_lstVw_Obstcls_ColumnHeader_3
        '
        Me._lstVw_Obstcls_ColumnHeader_3.Text = "Height"
        Me._lstVw_Obstcls_ColumnHeader_3.Width = 170
        '
        '_lstVw_Obstcls_ColumnHeader_4
        '
        Me._lstVw_Obstcls_ColumnHeader_4.Text = "MOC"
        Me._lstVw_Obstcls_ColumnHeader_4.Width = 170
        '
        '_lstVw_Obstcls_ColumnHeader_5
        '
        Me._lstVw_Obstcls_ColumnHeader_5.Text = "Req. H"
        Me._lstVw_Obstcls_ColumnHeader_5.Width = 170
        '
        'tbPg_TrackStatistics
        '
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_meterSign3)
        Me.tbPg_TrackStatistics.Controls.Add(Me.txtBox_downwindLegLength)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_downwindLegLength)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_percentSign)
        Me.tbPg_TrackStatistics.Controls.Add(Me.txtBox_descentGradient)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_descentGradient)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_meterSign2)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_meterSign1)
        Me.tbPg_TrackStatistics.Controls.Add(Me.txtBox_trackLength)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_TrackLength)
        Me.tbPg_TrackStatistics.Controls.Add(Me.txtBox_trackOCH)
        Me.tbPg_TrackStatistics.Controls.Add(Me.lbl_TrackOCH)
        Me.tbPg_TrackStatistics.Location = New System.Drawing.Point(4, 23)
        Me.tbPg_TrackStatistics.Name = "tbPg_TrackStatistics"
        Me.tbPg_TrackStatistics.Size = New System.Drawing.Size(597, 322)
        Me.tbPg_TrackStatistics.TabIndex = 4
        Me.tbPg_TrackStatistics.Text = "Track statistics"
        Me.tbPg_TrackStatistics.UseVisualStyleBackColor = True
        '
        'lbl_meterSign3
        '
        Me.lbl_meterSign3.AutoSize = True
        Me.lbl_meterSign3.Location = New System.Drawing.Point(249, 147)
        Me.lbl_meterSign3.Name = "lbl_meterSign3"
        Me.lbl_meterSign3.Size = New System.Drawing.Size(15, 14)
        Me.lbl_meterSign3.TabIndex = 11
        Me.lbl_meterSign3.Text = "m"
        '
        'txtBox_downwindLegLength
        '
        Me.txtBox_downwindLegLength.Location = New System.Drawing.Point(147, 144)
        Me.txtBox_downwindLegLength.Name = "txtBox_downwindLegLength"
        Me.txtBox_downwindLegLength.ReadOnly = True
        Me.txtBox_downwindLegLength.Size = New System.Drawing.Size(100, 20)
        Me.txtBox_downwindLegLength.TabIndex = 10
        '
        'lbl_downwindLegLength
        '
        Me.lbl_downwindLegLength.AutoSize = True
        Me.lbl_downwindLegLength.Location = New System.Drawing.Point(27, 147)
        Me.lbl_downwindLegLength.Name = "lbl_downwindLegLength"
        Me.lbl_downwindLegLength.Size = New System.Drawing.Size(112, 14)
        Me.lbl_downwindLegLength.TabIndex = 9
        Me.lbl_downwindLegLength.Text = "Downwind leg length:"
        '
        'lbl_percentSign
        '
        Me.lbl_percentSign.AutoSize = True
        Me.lbl_percentSign.Location = New System.Drawing.Point(249, 106)
        Me.lbl_percentSign.Name = "lbl_percentSign"
        Me.lbl_percentSign.Size = New System.Drawing.Size(17, 14)
        Me.lbl_percentSign.TabIndex = 8
        Me.lbl_percentSign.Text = "%"
        '
        'txtBox_descentGradient
        '
        Me.txtBox_descentGradient.Location = New System.Drawing.Point(147, 103)
        Me.txtBox_descentGradient.Name = "txtBox_descentGradient"
        Me.txtBox_descentGradient.ReadOnly = True
        Me.txtBox_descentGradient.Size = New System.Drawing.Size(100, 20)
        Me.txtBox_descentGradient.TabIndex = 7
        '
        'lbl_descentGradient
        '
        Me.lbl_descentGradient.AutoSize = True
        Me.lbl_descentGradient.Location = New System.Drawing.Point(27, 106)
        Me.lbl_descentGradient.Name = "lbl_descentGradient"
        Me.lbl_descentGradient.Size = New System.Drawing.Size(92, 14)
        Me.lbl_descentGradient.TabIndex = 6
        Me.lbl_descentGradient.Text = "Descent gradient:"
        '
        'lbl_meterSign2
        '
        Me.lbl_meterSign2.AutoSize = True
        Me.lbl_meterSign2.Location = New System.Drawing.Point(249, 68)
        Me.lbl_meterSign2.Name = "lbl_meterSign2"
        Me.lbl_meterSign2.Size = New System.Drawing.Size(15, 14)
        Me.lbl_meterSign2.TabIndex = 5
        Me.lbl_meterSign2.Text = "m"
        '
        'lbl_meterSign1
        '
        Me.lbl_meterSign1.AutoSize = True
        Me.lbl_meterSign1.Location = New System.Drawing.Point(249, 30)
        Me.lbl_meterSign1.Name = "lbl_meterSign1"
        Me.lbl_meterSign1.Size = New System.Drawing.Size(15, 14)
        Me.lbl_meterSign1.TabIndex = 4
        Me.lbl_meterSign1.Text = "m"
        '
        'txtBox_trackLength
        '
        Me.txtBox_trackLength.Location = New System.Drawing.Point(147, 65)
        Me.txtBox_trackLength.Name = "txtBox_trackLength"
        Me.txtBox_trackLength.ReadOnly = True
        Me.txtBox_trackLength.Size = New System.Drawing.Size(100, 20)
        Me.txtBox_trackLength.TabIndex = 3
        '
        'lbl_TrackLength
        '
        Me.lbl_TrackLength.AutoSize = True
        Me.lbl_TrackLength.Location = New System.Drawing.Point(24, 68)
        Me.lbl_TrackLength.Name = "lbl_TrackLength"
        Me.lbl_TrackLength.Size = New System.Drawing.Size(69, 14)
        Me.lbl_TrackLength.TabIndex = 2
        Me.lbl_TrackLength.Text = "Track length:"
        '
        'txtBox_trackOCH
        '
        Me.txtBox_trackOCH.Location = New System.Drawing.Point(147, 27)
        Me.txtBox_trackOCH.Name = "txtBox_trackOCH"
        Me.txtBox_trackOCH.ReadOnly = True
        Me.txtBox_trackOCH.Size = New System.Drawing.Size(100, 20)
        Me.txtBox_trackOCH.TabIndex = 1
        '
        'lbl_TrackOCH
        '
        Me.lbl_TrackOCH.AutoSize = True
        Me.lbl_TrackOCH.Location = New System.Drawing.Point(24, 30)
        Me.lbl_TrackOCH.Name = "lbl_TrackOCH"
        Me.lbl_TrackOCH.Size = New System.Drawing.Size(62, 14)
        Me.lbl_TrackOCH.TabIndex = 0
        Me.lbl_TrackOCH.Text = "Track OCH:"
        '
        'btn_Close
        '
        Me.btn_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Close.Image = CType(resources.GetObject("btn_Close.Image"), System.Drawing.Image)
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(507, 359)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(94, 26)
        Me.btn_Close.TabIndex = 11
        Me.btn_Close.Text = "Close"
        '
        'btn_Save
        '
        Me.btn_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_Save.Enabled = False
        Me.btn_Save.Image = CType(resources.GetObject("btn_Save.Image"), System.Drawing.Image)
        Me.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Save.Location = New System.Drawing.Point(407, 359)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(94, 26)
        Me.btn_Save.TabIndex = 12
        Me.btn_Save.Text = "Save"
        '
        'VisualManoeuvringReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(605, 392)
        Me.Controls.Add(Me.btn_Save)
        Me.Controls.Add(Me.btn_Close)
        Me.Controls.Add(Me.tbCntrl_VisualManoeuvring)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "VisualManoeuvringReport"
        Me.ShowInTaskbar = False
        Me.Text = "Visual manoeuvring Report"
        Me.tbCntrl_VisualManoeuvring.ResumeLayout(False)
        Me.tbPg_ReferencePoints.ResumeLayout(False)
        Me.tbPg_CirclingAreaObstacles.ResumeLayout(False)
        Me.tbPg_LeftCirclingBoxObstacles.ResumeLayout(False)
        Me.tbPg_RightCirclingBoxObstacles.ResumeLayout(False)
        Me.tbPg_TrackSteps.ResumeLayout(False)
        Me.tbPg_TrackObstacles.ResumeLayout(False)
        Me.tbPg_TrackStatistics.ResumeLayout(False)
        Me.tbPg_TrackStatistics.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tbCntrl_VisualManoeuvring As System.Windows.Forms.TabControl
    Friend WithEvents tbPg_ReferencePoints As System.Windows.Forms.TabPage
    Friend WithEvents tbPg_TrackSteps As System.Windows.Forms.TabPage
    Public WithEvents lstVw_ReferencePoints As System.Windows.Forms.ListView
    Friend WithEvents _lstVw_RefPnts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_RefPnts_ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_RefPnts_ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lstVw_TrackSteps As System.Windows.Forms.ListView
    Public WithEvents _lstVw_TrckStps_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents _lstVw_TrckStps_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents _lstVw_TrckStps_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents _lstVw_TrckStps_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbPg_TrackObstacles As System.Windows.Forms.TabPage
    Public WithEvents lstVw_TrackObstacles As System.Windows.Forms.ListView
    Friend WithEvents _lstVw_TrackObstcls_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_Obstcls_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_Obstcls_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_Obstcls_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_Obstcls_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbPg_TrackStatistics As System.Windows.Forms.TabPage
    Friend WithEvents txtBox_trackOCH As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TrackOCH As System.Windows.Forms.Label
    Friend WithEvents lbl_TrackLength As System.Windows.Forms.Label
    Friend WithEvents txtBox_trackLength As System.Windows.Forms.TextBox
    Friend WithEvents lbl_meterSign2 As System.Windows.Forms.Label
    Friend WithEvents lbl_meterSign1 As System.Windows.Forms.Label
    Friend WithEvents txtBox_descentGradient As System.Windows.Forms.TextBox
    Friend WithEvents lbl_descentGradient As System.Windows.Forms.Label
    Friend WithEvents lbl_percentSign As System.Windows.Forms.Label
    Friend WithEvents _lstVw_TrckStps_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lstVw_TrckStps_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lbl_meterSign3 As System.Windows.Forms.Label
    Friend WithEvents txtBox_downwindLegLength As System.Windows.Forms.TextBox
    Friend WithEvents lbl_downwindLegLength As System.Windows.Forms.Label
    Friend WithEvents tbPg_CirclingAreaObstacles As System.Windows.Forms.TabPage
    Public WithEvents lstVw_CirclingAreaObstacles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Public WithEvents btn_Close As System.Windows.Forms.Button
    Public WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents tbPg_LeftCirclingBoxObstacles As System.Windows.Forms.TabPage
    Public WithEvents lstVw_LeftCirclingBoxObstacles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbPg_RightCirclingBoxObstacles As System.Windows.Forms.TabPage
    Public WithEvents lstVw_RightCirclingBoxObstacles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
End Class
