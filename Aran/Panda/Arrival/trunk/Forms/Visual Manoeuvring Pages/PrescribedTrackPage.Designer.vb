<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrescribedTrackPage
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lbl_MaxVisibilityDistance = New System.Windows.Forms.Label()
        Me.txtBox_maxVisibilityDistance = New System.Windows.Forms.TextBox()
        Me.lbl_meterSign1 = New System.Windows.Forms.Label()
        Me.lbl_maxConvergenceAngle = New System.Windows.Forms.Label()
        Me.lbl_meterSign8 = New System.Windows.Forms.Label()
        Me.txtBox_maxConvergenceAngle = New System.Windows.Forms.TextBox()
        Me.txtBox_bankEstablishDistance = New System.Windows.Forms.TextBox()
        Me.lbl_degreeSign3 = New System.Windows.Forms.Label()
        Me.lbl_stabilizationDistance = New System.Windows.Forms.Label()
        Me.lbl_maxDivergenceAngle = New System.Windows.Forms.Label()
        Me.txtBox_maxDivergenceAngle = New System.Windows.Forms.TextBox()
        Me.lbl_degreeSign2 = New System.Windows.Forms.Label()
        Me.lbl_stabilizationTime = New System.Windows.Forms.Label()
        Me.lbl_MinManeuverDistance = New System.Windows.Forms.Label()
        Me.txtBox_minManeuverDistance = New System.Windows.Forms.TextBox()
        Me.lbl_meterSign2 = New System.Windows.Forms.Label()
        Me.txtBox_bankEstablishTime = New System.Windows.Forms.TextBox()
        Me.lbl_secondSign1 = New System.Windows.Forms.Label()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.lbl_meterSign10 = New System.Windows.Forms.Label()
        Me.lbl_meterSign9 = New System.Windows.Forms.Label()
        Me.txtBox_leftBoxOCH = New System.Windows.Forms.TextBox()
        Me.lbl_leftBoxOCH = New System.Windows.Forms.Label()
        Me.txtBox_rightBoxOCH = New System.Windows.Forms.TextBox()
        Me.lbl_rightBoxOCH = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.chkBox_showFinalSegmentCetreline = New System.Windows.Forms.CheckBox()
        Me.chkbox_showLeftBoxCetreline = New System.Windows.Forms.CheckBox()
        Me.chkBox_showRightBoxCentreline = New System.Windows.Forms.CheckBox()
        Me.chkBox_showFinalSegment = New System.Windows.Forms.CheckBox()
        Me.chkBox_showLeftBox = New System.Windows.Forms.CheckBox()
        Me.chkBox_showRightBox = New System.Windows.Forms.CheckBox()
        Me.btn_VisManReport = New System.Windows.Forms.Button()
        Me.btn_addVisRefPnts = New System.Windows.Forms.Button()
        Me.btn_drawTrack = New System.Windows.Forms.Button()
        Me.chkBox_showTrackBuffer = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.nmrcUpDown_FinalSegmentTime = New System.Windows.Forms.NumericUpDown()
        Me.nmrcUpDown_FinalSegmentIAS = New System.Windows.Forms.NumericUpDown()
        Me.lbl_corridorSemiWidth = New System.Windows.Forms.Label()
        Me.txtBox_corridorSemiWidth = New System.Windows.Forms.TextBox()
        Me.lbl_meterSign7 = New System.Windows.Forms.Label()
        Me.lbl_finalSegmentIAS = New System.Windows.Forms.Label()
        Me.lbl_kmhSign2 = New System.Windows.Forms.Label()
        Me.lbl_finalSegmentIASrange = New System.Windows.Forms.Label()
        Me.lbl_finalSegmentLength = New System.Windows.Forms.Label()
        Me.txtBox_finalSegmentLength = New System.Windows.Forms.TextBox()
        Me.lbl_meterSign6 = New System.Windows.Forms.Label()
        Me.lbl_secondSign2 = New System.Windows.Forms.Label()
        Me.lbl_finalSegmentTime = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.nmrcUpDown_FinalSegmentTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmrcUpDown_FinalSegmentIAS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lbl_MaxVisibilityDistance)
        Me.GroupBox1.Controls.Add(Me.txtBox_maxVisibilityDistance)
        Me.GroupBox1.Controls.Add(Me.lbl_meterSign1)
        Me.GroupBox1.Controls.Add(Me.lbl_maxConvergenceAngle)
        Me.GroupBox1.Controls.Add(Me.lbl_meterSign8)
        Me.GroupBox1.Controls.Add(Me.txtBox_maxConvergenceAngle)
        Me.GroupBox1.Controls.Add(Me.txtBox_bankEstablishDistance)
        Me.GroupBox1.Controls.Add(Me.lbl_degreeSign3)
        Me.GroupBox1.Controls.Add(Me.lbl_stabilizationDistance)
        Me.GroupBox1.Controls.Add(Me.lbl_maxDivergenceAngle)
        Me.GroupBox1.Controls.Add(Me.txtBox_maxDivergenceAngle)
        Me.GroupBox1.Controls.Add(Me.lbl_degreeSign2)
        Me.GroupBox1.Controls.Add(Me.lbl_stabilizationTime)
        Me.GroupBox1.Controls.Add(Me.lbl_MinManeuverDistance)
        Me.GroupBox1.Controls.Add(Me.txtBox_minManeuverDistance)
        Me.GroupBox1.Controls.Add(Me.lbl_meterSign2)
        Me.GroupBox1.Controls.Add(Me.txtBox_bankEstablishTime)
        Me.GroupBox1.Controls.Add(Me.lbl_secondSign1)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 1)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(547, 122)
        Me.GroupBox1.TabIndex = 57
        Me.GroupBox1.TabStop = False
        '
        'lbl_MaxVisibilityDistance
        '
        Me.lbl_MaxVisibilityDistance.AutoSize = True
        Me.lbl_MaxVisibilityDistance.Location = New System.Drawing.Point(6, 97)
        Me.lbl_MaxVisibilityDistance.Name = "lbl_MaxVisibilityDistance"
        Me.lbl_MaxVisibilityDistance.Size = New System.Drawing.Size(114, 13)
        Me.lbl_MaxVisibilityDistance.TabIndex = 56
        Me.lbl_MaxVisibilityDistance.Text = "Max. visibility distance:"
        '
        'txtBox_maxVisibilityDistance
        '
        Me.txtBox_maxVisibilityDistance.Location = New System.Drawing.Point(143, 94)
        Me.txtBox_maxVisibilityDistance.Name = "txtBox_maxVisibilityDistance"
        Me.txtBox_maxVisibilityDistance.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_maxVisibilityDistance.TabIndex = 57
        '
        'lbl_meterSign1
        '
        Me.lbl_meterSign1.AutoSize = True
        Me.lbl_meterSign1.Location = New System.Drawing.Point(207, 97)
        Me.lbl_meterSign1.Name = "lbl_meterSign1"
        Me.lbl_meterSign1.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign1.TabIndex = 58
        Me.lbl_meterSign1.Text = "m"
        '
        'lbl_maxConvergenceAngle
        '
        Me.lbl_maxConvergenceAngle.AutoSize = True
        Me.lbl_maxConvergenceAngle.Location = New System.Drawing.Point(6, 45)
        Me.lbl_maxConvergenceAngle.Name = "lbl_maxConvergenceAngle"
        Me.lbl_maxConvergenceAngle.Size = New System.Drawing.Size(128, 13)
        Me.lbl_maxConvergenceAngle.TabIndex = 30
        Me.lbl_maxConvergenceAngle.Text = "Max. convergence angle:"
        '
        'lbl_meterSign8
        '
        Me.lbl_meterSign8.AutoSize = True
        Me.lbl_meterSign8.Location = New System.Drawing.Point(492, 19)
        Me.lbl_meterSign8.Name = "lbl_meterSign8"
        Me.lbl_meterSign8.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign8.TabIndex = 55
        Me.lbl_meterSign8.Text = "m"
        '
        'txtBox_maxConvergenceAngle
        '
        Me.txtBox_maxConvergenceAngle.Location = New System.Drawing.Point(143, 42)
        Me.txtBox_maxConvergenceAngle.Name = "txtBox_maxConvergenceAngle"
        Me.txtBox_maxConvergenceAngle.ReadOnly = True
        Me.txtBox_maxConvergenceAngle.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_maxConvergenceAngle.TabIndex = 31
        '
        'txtBox_bankEstablishDistance
        '
        Me.txtBox_bankEstablishDistance.Location = New System.Drawing.Point(427, 16)
        Me.txtBox_bankEstablishDistance.Name = "txtBox_bankEstablishDistance"
        Me.txtBox_bankEstablishDistance.ReadOnly = True
        Me.txtBox_bankEstablishDistance.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_bankEstablishDistance.TabIndex = 54
        '
        'lbl_degreeSign3
        '
        Me.lbl_degreeSign3.AutoSize = True
        Me.lbl_degreeSign3.Location = New System.Drawing.Point(208, 45)
        Me.lbl_degreeSign3.Name = "lbl_degreeSign3"
        Me.lbl_degreeSign3.Size = New System.Drawing.Size(11, 13)
        Me.lbl_degreeSign3.TabIndex = 32
        Me.lbl_degreeSign3.Text = "°"
        '
        'lbl_stabilizationDistance
        '
        Me.lbl_stabilizationDistance.AutoSize = True
        Me.lbl_stabilizationDistance.Location = New System.Drawing.Point(290, 19)
        Me.lbl_stabilizationDistance.Name = "lbl_stabilizationDistance"
        Me.lbl_stabilizationDistance.Size = New System.Drawing.Size(136, 13)
        Me.lbl_stabilizationDistance.TabIndex = 53
        Me.lbl_stabilizationDistance.Text = "Bank establishing distance:"
        '
        'lbl_maxDivergenceAngle
        '
        Me.lbl_maxDivergenceAngle.AutoSize = True
        Me.lbl_maxDivergenceAngle.Location = New System.Drawing.Point(6, 19)
        Me.lbl_maxDivergenceAngle.Name = "lbl_maxDivergenceAngle"
        Me.lbl_maxDivergenceAngle.Size = New System.Drawing.Size(118, 13)
        Me.lbl_maxDivergenceAngle.TabIndex = 42
        Me.lbl_maxDivergenceAngle.Text = "Max. divergence angle:"
        '
        'txtBox_maxDivergenceAngle
        '
        Me.txtBox_maxDivergenceAngle.Location = New System.Drawing.Point(143, 16)
        Me.txtBox_maxDivergenceAngle.Name = "txtBox_maxDivergenceAngle"
        Me.txtBox_maxDivergenceAngle.ReadOnly = True
        Me.txtBox_maxDivergenceAngle.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_maxDivergenceAngle.TabIndex = 43
        '
        'lbl_degreeSign2
        '
        Me.lbl_degreeSign2.AutoSize = True
        Me.lbl_degreeSign2.Location = New System.Drawing.Point(207, 19)
        Me.lbl_degreeSign2.Name = "lbl_degreeSign2"
        Me.lbl_degreeSign2.Size = New System.Drawing.Size(11, 13)
        Me.lbl_degreeSign2.TabIndex = 44
        Me.lbl_degreeSign2.Text = "°"
        '
        'lbl_stabilizationTime
        '
        Me.lbl_stabilizationTime.AutoSize = True
        Me.lbl_stabilizationTime.Location = New System.Drawing.Point(6, 71)
        Me.lbl_stabilizationTime.Name = "lbl_stabilizationTime"
        Me.lbl_stabilizationTime.Size = New System.Drawing.Size(101, 13)
        Me.lbl_stabilizationTime.TabIndex = 33
        Me.lbl_stabilizationTime.Text = "Bank establish time:"
        '
        'lbl_MinManeuverDistance
        '
        Me.lbl_MinManeuverDistance.AutoSize = True
        Me.lbl_MinManeuverDistance.Location = New System.Drawing.Point(291, 62)
        Me.lbl_MinManeuverDistance.Name = "lbl_MinManeuverDistance"
        Me.lbl_MinManeuverDistance.Size = New System.Drawing.Size(117, 26)
        Me.lbl_MinManeuverDistance.TabIndex = 7
        Me.lbl_MinManeuverDistance.Text = "Min. distance between " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "maneuvers:"
        '
        'txtBox_minManeuverDistance
        '
        Me.txtBox_minManeuverDistance.Location = New System.Drawing.Point(428, 68)
        Me.txtBox_minManeuverDistance.Name = "txtBox_minManeuverDistance"
        Me.txtBox_minManeuverDistance.ReadOnly = True
        Me.txtBox_minManeuverDistance.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_minManeuverDistance.TabIndex = 8
        '
        'lbl_meterSign2
        '
        Me.lbl_meterSign2.AutoSize = True
        Me.lbl_meterSign2.Location = New System.Drawing.Point(492, 73)
        Me.lbl_meterSign2.Name = "lbl_meterSign2"
        Me.lbl_meterSign2.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign2.TabIndex = 9
        Me.lbl_meterSign2.Text = "m"
        '
        'txtBox_bankEstablishTime
        '
        Me.txtBox_bankEstablishTime.Location = New System.Drawing.Point(143, 68)
        Me.txtBox_bankEstablishTime.Name = "txtBox_bankEstablishTime"
        Me.txtBox_bankEstablishTime.ReadOnly = True
        Me.txtBox_bankEstablishTime.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_bankEstablishTime.TabIndex = 34
        '
        'lbl_secondSign1
        '
        Me.lbl_secondSign1.AutoSize = True
        Me.lbl_secondSign1.Location = New System.Drawing.Point(208, 71)
        Me.lbl_secondSign1.Name = "lbl_secondSign1"
        Me.lbl_secondSign1.Size = New System.Drawing.Size(12, 13)
        Me.lbl_secondSign1.TabIndex = 35
        Me.lbl_secondSign1.Text = "s"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.lbl_meterSign10)
        Me.GroupBox7.Controls.Add(Me.lbl_meterSign9)
        Me.GroupBox7.Controls.Add(Me.txtBox_leftBoxOCH)
        Me.GroupBox7.Controls.Add(Me.lbl_leftBoxOCH)
        Me.GroupBox7.Controls.Add(Me.txtBox_rightBoxOCH)
        Me.GroupBox7.Controls.Add(Me.lbl_rightBoxOCH)
        Me.GroupBox7.Location = New System.Drawing.Point(3, 215)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(266, 83)
        Me.GroupBox7.TabIndex = 67
        Me.GroupBox7.TabStop = False
        '
        'lbl_meterSign10
        '
        Me.lbl_meterSign10.AutoSize = True
        Me.lbl_meterSign10.Location = New System.Drawing.Point(207, 51)
        Me.lbl_meterSign10.Name = "lbl_meterSign10"
        Me.lbl_meterSign10.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign10.TabIndex = 5
        Me.lbl_meterSign10.Text = "m"
        '
        'lbl_meterSign9
        '
        Me.lbl_meterSign9.AutoSize = True
        Me.lbl_meterSign9.Location = New System.Drawing.Point(207, 17)
        Me.lbl_meterSign9.Name = "lbl_meterSign9"
        Me.lbl_meterSign9.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign9.TabIndex = 4
        Me.lbl_meterSign9.Text = "m"
        '
        'txtBox_leftBoxOCH
        '
        Me.txtBox_leftBoxOCH.Location = New System.Drawing.Point(142, 48)
        Me.txtBox_leftBoxOCH.Name = "txtBox_leftBoxOCH"
        Me.txtBox_leftBoxOCH.ReadOnly = True
        Me.txtBox_leftBoxOCH.Size = New System.Drawing.Size(64, 20)
        Me.txtBox_leftBoxOCH.TabIndex = 3
        '
        'lbl_leftBoxOCH
        '
        Me.lbl_leftBoxOCH.AutoSize = True
        Me.lbl_leftBoxOCH.Location = New System.Drawing.Point(10, 51)
        Me.lbl_leftBoxOCH.Name = "lbl_leftBoxOCH"
        Me.lbl_leftBoxOCH.Size = New System.Drawing.Size(74, 13)
        Me.lbl_leftBoxOCH.TabIndex = 2
        Me.lbl_leftBoxOCH.Text = "Left box OCH:"
        '
        'txtBox_rightBoxOCH
        '
        Me.txtBox_rightBoxOCH.Location = New System.Drawing.Point(142, 14)
        Me.txtBox_rightBoxOCH.Name = "txtBox_rightBoxOCH"
        Me.txtBox_rightBoxOCH.ReadOnly = True
        Me.txtBox_rightBoxOCH.Size = New System.Drawing.Size(64, 20)
        Me.txtBox_rightBoxOCH.TabIndex = 1
        '
        'lbl_rightBoxOCH
        '
        Me.lbl_rightBoxOCH.AutoSize = True
        Me.lbl_rightBoxOCH.Location = New System.Drawing.Point(10, 17)
        Me.lbl_rightBoxOCH.Name = "lbl_rightBoxOCH"
        Me.lbl_rightBoxOCH.Size = New System.Drawing.Size(81, 13)
        Me.lbl_rightBoxOCH.TabIndex = 0
        Me.lbl_rightBoxOCH.Text = "Right box OCH:"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkBox_showFinalSegmentCetreline)
        Me.GroupBox6.Controls.Add(Me.chkbox_showLeftBoxCetreline)
        Me.GroupBox6.Controls.Add(Me.chkBox_showRightBoxCentreline)
        Me.GroupBox6.Controls.Add(Me.chkBox_showFinalSegment)
        Me.GroupBox6.Controls.Add(Me.chkBox_showLeftBox)
        Me.GroupBox6.Controls.Add(Me.chkBox_showRightBox)
        Me.GroupBox6.Location = New System.Drawing.Point(275, 215)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(275, 83)
        Me.GroupBox6.TabIndex = 66
        Me.GroupBox6.TabStop = False
        '
        'chkBox_showFinalSegmentCetreline
        '
        Me.chkBox_showFinalSegmentCetreline.AutoSize = True
        Me.chkBox_showFinalSegmentCetreline.Checked = True
        Me.chkBox_showFinalSegmentCetreline.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBox_showFinalSegmentCetreline.Location = New System.Drawing.Point(163, 43)
        Me.chkBox_showFinalSegmentCetreline.Name = "chkBox_showFinalSegmentCetreline"
        Me.chkBox_showFinalSegmentCetreline.Size = New System.Drawing.Size(91, 30)
        Me.chkBox_showFinalSegmentCetreline.TabIndex = 51
        Me.chkBox_showFinalSegmentCetreline.Text = "Final segment" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "centreline"
        Me.chkBox_showFinalSegmentCetreline.UseVisualStyleBackColor = True
        '
        'chkbox_showLeftBoxCetreline
        '
        Me.chkbox_showLeftBoxCetreline.AutoSize = True
        Me.chkbox_showLeftBoxCetreline.Checked = True
        Me.chkbox_showLeftBoxCetreline.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbox_showLeftBoxCetreline.Location = New System.Drawing.Point(93, 43)
        Me.chkbox_showLeftBoxCetreline.Name = "chkbox_showLeftBoxCetreline"
        Me.chkbox_showLeftBoxCetreline.Size = New System.Drawing.Size(72, 30)
        Me.chkbox_showLeftBoxCetreline.TabIndex = 50
        Me.chkbox_showLeftBoxCetreline.Text = "Left box" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "centreline"
        Me.chkbox_showLeftBoxCetreline.UseVisualStyleBackColor = True
        '
        'chkBox_showRightBoxCentreline
        '
        Me.chkBox_showRightBoxCentreline.AutoSize = True
        Me.chkBox_showRightBoxCentreline.Checked = True
        Me.chkBox_showRightBoxCentreline.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBox_showRightBoxCentreline.Location = New System.Drawing.Point(16, 43)
        Me.chkBox_showRightBoxCentreline.Name = "chkBox_showRightBoxCentreline"
        Me.chkBox_showRightBoxCentreline.Size = New System.Drawing.Size(72, 30)
        Me.chkBox_showRightBoxCentreline.TabIndex = 49
        Me.chkBox_showRightBoxCentreline.Text = "Right box" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "centreline"
        Me.chkBox_showRightBoxCentreline.UseVisualStyleBackColor = True
        '
        'chkBox_showFinalSegment
        '
        Me.chkBox_showFinalSegment.AutoSize = True
        Me.chkBox_showFinalSegment.Checked = True
        Me.chkBox_showFinalSegment.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBox_showFinalSegment.Location = New System.Drawing.Point(163, 15)
        Me.chkBox_showFinalSegment.Name = "chkBox_showFinalSegment"
        Me.chkBox_showFinalSegment.Size = New System.Drawing.Size(91, 17)
        Me.chkBox_showFinalSegment.TabIndex = 48
        Me.chkBox_showFinalSegment.Text = "Final segment"
        Me.chkBox_showFinalSegment.UseVisualStyleBackColor = True
        '
        'chkBox_showLeftBox
        '
        Me.chkBox_showLeftBox.AutoSize = True
        Me.chkBox_showLeftBox.Checked = True
        Me.chkBox_showLeftBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBox_showLeftBox.Location = New System.Drawing.Point(93, 15)
        Me.chkBox_showLeftBox.Name = "chkBox_showLeftBox"
        Me.chkBox_showLeftBox.Size = New System.Drawing.Size(64, 17)
        Me.chkBox_showLeftBox.TabIndex = 47
        Me.chkBox_showLeftBox.Text = "Left box"
        Me.chkBox_showLeftBox.UseVisualStyleBackColor = True
        '
        'chkBox_showRightBox
        '
        Me.chkBox_showRightBox.AutoSize = True
        Me.chkBox_showRightBox.Checked = True
        Me.chkBox_showRightBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBox_showRightBox.Location = New System.Drawing.Point(16, 15)
        Me.chkBox_showRightBox.Name = "chkBox_showRightBox"
        Me.chkBox_showRightBox.Size = New System.Drawing.Size(71, 17)
        Me.chkBox_showRightBox.TabIndex = 46
        Me.chkBox_showRightBox.Text = "Right box"
        Me.chkBox_showRightBox.UseVisualStyleBackColor = True
        '
        'btn_VisManReport
        '
        Me.btn_VisManReport.Location = New System.Drawing.Point(456, 333)
        Me.btn_VisManReport.Name = "btn_VisManReport"
        Me.btn_VisManReport.Size = New System.Drawing.Size(91, 25)
        Me.btn_VisManReport.TabIndex = 70
        Me.btn_VisManReport.Text = "Report"
        Me.btn_VisManReport.UseVisualStyleBackColor = True
        '
        'btn_addVisRefPnts
        '
        Me.btn_addVisRefPnts.Location = New System.Drawing.Point(262, 333)
        Me.btn_addVisRefPnts.Name = "btn_addVisRefPnts"
        Me.btn_addVisRefPnts.Size = New System.Drawing.Size(91, 25)
        Me.btn_addVisRefPnts.TabIndex = 69
        Me.btn_addVisRefPnts.Text = "Add points"
        Me.btn_addVisRefPnts.UseVisualStyleBackColor = True
        '
        'btn_drawTrack
        '
        Me.btn_drawTrack.Location = New System.Drawing.Point(359, 333)
        Me.btn_drawTrack.Name = "btn_drawTrack"
        Me.btn_drawTrack.Size = New System.Drawing.Size(91, 25)
        Me.btn_drawTrack.TabIndex = 68
        Me.btn_drawTrack.Text = "Draw track"
        Me.btn_drawTrack.UseVisualStyleBackColor = True
        '
        'chkBox_showTrackBuffer
        '
        Me.chkBox_showTrackBuffer.AutoSize = True
        Me.chkBox_showTrackBuffer.Checked = True
        Me.chkBox_showTrackBuffer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBox_showTrackBuffer.Location = New System.Drawing.Point(10, 304)
        Me.chkBox_showTrackBuffer.Name = "chkBox_showTrackBuffer"
        Me.chkBox_showTrackBuffer.Size = New System.Drawing.Size(84, 17)
        Me.chkBox_showTrackBuffer.TabIndex = 71
        Me.chkBox_showTrackBuffer.Text = "Track buffer"
        Me.chkBox_showTrackBuffer.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.nmrcUpDown_FinalSegmentTime)
        Me.GroupBox3.Controls.Add(Me.nmrcUpDown_FinalSegmentIAS)
        Me.GroupBox3.Controls.Add(Me.lbl_corridorSemiWidth)
        Me.GroupBox3.Controls.Add(Me.txtBox_corridorSemiWidth)
        Me.GroupBox3.Controls.Add(Me.lbl_meterSign7)
        Me.GroupBox3.Controls.Add(Me.lbl_finalSegmentIAS)
        Me.GroupBox3.Controls.Add(Me.lbl_kmhSign2)
        Me.GroupBox3.Controls.Add(Me.lbl_finalSegmentIASrange)
        Me.GroupBox3.Controls.Add(Me.lbl_finalSegmentLength)
        Me.GroupBox3.Controls.Add(Me.txtBox_finalSegmentLength)
        Me.GroupBox3.Controls.Add(Me.lbl_meterSign6)
        Me.GroupBox3.Controls.Add(Me.lbl_secondSign2)
        Me.GroupBox3.Controls.Add(Me.lbl_finalSegmentTime)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 123)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(547, 90)
        Me.GroupBox3.TabIndex = 72
        Me.GroupBox3.TabStop = False
        '
        'nmrcUpDown_FinalSegmentTime
        '
        Me.nmrcUpDown_FinalSegmentTime.Location = New System.Drawing.Point(141, 59)
        Me.nmrcUpDown_FinalSegmentTime.Name = "nmrcUpDown_FinalSegmentTime"
        Me.nmrcUpDown_FinalSegmentTime.Size = New System.Drawing.Size(77, 20)
        Me.nmrcUpDown_FinalSegmentTime.TabIndex = 53
        '
        'nmrcUpDown_FinalSegmentIAS
        '
        Me.nmrcUpDown_FinalSegmentIAS.Location = New System.Drawing.Point(141, 17)
        Me.nmrcUpDown_FinalSegmentIAS.Name = "nmrcUpDown_FinalSegmentIAS"
        Me.nmrcUpDown_FinalSegmentIAS.Size = New System.Drawing.Size(77, 20)
        Me.nmrcUpDown_FinalSegmentIAS.TabIndex = 52
        '
        'lbl_corridorSemiWidth
        '
        Me.lbl_corridorSemiWidth.AutoSize = True
        Me.lbl_corridorSemiWidth.Location = New System.Drawing.Point(291, 61)
        Me.lbl_corridorSemiWidth.Name = "lbl_corridorSemiWidth"
        Me.lbl_corridorSemiWidth.Size = New System.Drawing.Size(98, 13)
        Me.lbl_corridorSemiWidth.TabIndex = 49
        Me.lbl_corridorSemiWidth.Text = "Corridor semi width:"
        '
        'txtBox_corridorSemiWidth
        '
        Me.txtBox_corridorSemiWidth.Location = New System.Drawing.Point(428, 58)
        Me.txtBox_corridorSemiWidth.Name = "txtBox_corridorSemiWidth"
        Me.txtBox_corridorSemiWidth.ReadOnly = True
        Me.txtBox_corridorSemiWidth.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_corridorSemiWidth.TabIndex = 50
        '
        'lbl_meterSign7
        '
        Me.lbl_meterSign7.AutoSize = True
        Me.lbl_meterSign7.Location = New System.Drawing.Point(492, 61)
        Me.lbl_meterSign7.Name = "lbl_meterSign7"
        Me.lbl_meterSign7.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign7.TabIndex = 51
        Me.lbl_meterSign7.Text = "m"
        '
        'lbl_finalSegmentIAS
        '
        Me.lbl_finalSegmentIAS.AutoSize = True
        Me.lbl_finalSegmentIAS.Location = New System.Drawing.Point(6, 19)
        Me.lbl_finalSegmentIAS.Name = "lbl_finalSegmentIAS"
        Me.lbl_finalSegmentIAS.Size = New System.Drawing.Size(95, 13)
        Me.lbl_finalSegmentIAS.TabIndex = 23
        Me.lbl_finalSegmentIAS.Text = "Final segment IAS:"
        '
        'lbl_kmhSign2
        '
        Me.lbl_kmhSign2.AutoSize = True
        Me.lbl_kmhSign2.Location = New System.Drawing.Point(219, 19)
        Me.lbl_kmhSign2.Name = "lbl_kmhSign2"
        Me.lbl_kmhSign2.Size = New System.Drawing.Size(32, 13)
        Me.lbl_kmhSign2.TabIndex = 25
        Me.lbl_kmhSign2.Text = "km/h"
        '
        'lbl_finalSegmentIASrange
        '
        Me.lbl_finalSegmentIASrange.AutoSize = True
        Me.lbl_finalSegmentIASrange.Location = New System.Drawing.Point(6, 34)
        Me.lbl_finalSegmentIASrange.Name = "lbl_finalSegmentIASrange"
        Me.lbl_finalSegmentIASrange.Size = New System.Drawing.Size(74, 13)
        Me.lbl_finalSegmentIASrange.TabIndex = 26
        Me.lbl_finalSegmentIASrange.Text = "(xxx-xxx km/h)"
        '
        'lbl_finalSegmentLength
        '
        Me.lbl_finalSegmentLength.AutoSize = True
        Me.lbl_finalSegmentLength.Location = New System.Drawing.Point(291, 19)
        Me.lbl_finalSegmentLength.Name = "lbl_finalSegmentLength"
        Me.lbl_finalSegmentLength.Size = New System.Drawing.Size(107, 13)
        Me.lbl_finalSegmentLength.TabIndex = 27
        Me.lbl_finalSegmentLength.Text = "Final segment length:"
        '
        'txtBox_finalSegmentLength
        '
        Me.txtBox_finalSegmentLength.Location = New System.Drawing.Point(428, 16)
        Me.txtBox_finalSegmentLength.Name = "txtBox_finalSegmentLength"
        Me.txtBox_finalSegmentLength.ReadOnly = True
        Me.txtBox_finalSegmentLength.Size = New System.Drawing.Size(63, 20)
        Me.txtBox_finalSegmentLength.TabIndex = 28
        '
        'lbl_meterSign6
        '
        Me.lbl_meterSign6.AutoSize = True
        Me.lbl_meterSign6.Location = New System.Drawing.Point(492, 19)
        Me.lbl_meterSign6.Name = "lbl_meterSign6"
        Me.lbl_meterSign6.Size = New System.Drawing.Size(15, 13)
        Me.lbl_meterSign6.TabIndex = 29
        Me.lbl_meterSign6.Text = "m"
        '
        'lbl_secondSign2
        '
        Me.lbl_secondSign2.AutoSize = True
        Me.lbl_secondSign2.Location = New System.Drawing.Point(219, 61)
        Me.lbl_secondSign2.Name = "lbl_secondSign2"
        Me.lbl_secondSign2.Size = New System.Drawing.Size(12, 13)
        Me.lbl_secondSign2.TabIndex = 48
        Me.lbl_secondSign2.Text = "s"
        '
        'lbl_finalSegmentTime
        '
        Me.lbl_finalSegmentTime.AutoSize = True
        Me.lbl_finalSegmentTime.Location = New System.Drawing.Point(7, 61)
        Me.lbl_finalSegmentTime.Name = "lbl_finalSegmentTime"
        Me.lbl_finalSegmentTime.Size = New System.Drawing.Size(97, 13)
        Me.lbl_finalSegmentTime.TabIndex = 46
        Me.lbl_finalSegmentTime.Text = "Final segment time:"
        '
        'PrescribedTrackPage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.chkBox_showTrackBuffer)
        Me.Controls.Add(Me.btn_VisManReport)
        Me.Controls.Add(Me.btn_addVisRefPnts)
        Me.Controls.Add(Me.btn_drawTrack)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "PrescribedTrackPage"
        Me.Size = New System.Drawing.Size(560, 382)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.nmrcUpDown_FinalSegmentTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmrcUpDown_FinalSegmentIAS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lbl_maxConvergenceAngle As System.Windows.Forms.Label
    Friend WithEvents lbl_meterSign8 As System.Windows.Forms.Label
    Friend WithEvents txtBox_maxConvergenceAngle As System.Windows.Forms.TextBox
    Friend WithEvents txtBox_bankEstablishDistance As System.Windows.Forms.TextBox
    Friend WithEvents lbl_degreeSign3 As System.Windows.Forms.Label
    Friend WithEvents lbl_stabilizationDistance As System.Windows.Forms.Label
    Friend WithEvents lbl_maxDivergenceAngle As System.Windows.Forms.Label
    Friend WithEvents txtBox_maxDivergenceAngle As System.Windows.Forms.TextBox
    Friend WithEvents lbl_degreeSign2 As System.Windows.Forms.Label
    Friend WithEvents lbl_stabilizationTime As System.Windows.Forms.Label
    Friend WithEvents lbl_MinManeuverDistance As System.Windows.Forms.Label
    Friend WithEvents txtBox_minManeuverDistance As System.Windows.Forms.TextBox
    Friend WithEvents lbl_meterSign2 As System.Windows.Forms.Label
    Friend WithEvents txtBox_bankEstablishTime As System.Windows.Forms.TextBox
    Friend WithEvents lbl_secondSign1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents lbl_meterSign10 As System.Windows.Forms.Label
    Friend WithEvents lbl_meterSign9 As System.Windows.Forms.Label
    Friend WithEvents txtBox_leftBoxOCH As System.Windows.Forms.TextBox
    Friend WithEvents lbl_leftBoxOCH As System.Windows.Forms.Label
    Friend WithEvents txtBox_rightBoxOCH As System.Windows.Forms.TextBox
    Friend WithEvents lbl_rightBoxOCH As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents chkBox_showFinalSegmentCetreline As System.Windows.Forms.CheckBox
    Friend WithEvents chkbox_showLeftBoxCetreline As System.Windows.Forms.CheckBox
    Friend WithEvents chkBox_showRightBoxCentreline As System.Windows.Forms.CheckBox
    Friend WithEvents chkBox_showFinalSegment As System.Windows.Forms.CheckBox
    Friend WithEvents chkBox_showLeftBox As System.Windows.Forms.CheckBox
    Friend WithEvents chkBox_showRightBox As System.Windows.Forms.CheckBox
    Friend WithEvents btn_VisManReport As System.Windows.Forms.Button
    Friend WithEvents btn_addVisRefPnts As System.Windows.Forms.Button
    Friend WithEvents btn_drawTrack As System.Windows.Forms.Button
    Friend WithEvents lbl_MaxVisibilityDistance As System.Windows.Forms.Label
    Friend WithEvents txtBox_maxVisibilityDistance As System.Windows.Forms.TextBox
    Friend WithEvents lbl_meterSign1 As System.Windows.Forms.Label
    Friend WithEvents chkBox_showTrackBuffer As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents nmrcUpDown_FinalSegmentTime As System.Windows.Forms.NumericUpDown
    Friend WithEvents nmrcUpDown_FinalSegmentIAS As System.Windows.Forms.NumericUpDown
    Friend WithEvents lbl_corridorSemiWidth As System.Windows.Forms.Label
    Friend WithEvents txtBox_corridorSemiWidth As System.Windows.Forms.TextBox
    Friend WithEvents lbl_meterSign7 As System.Windows.Forms.Label
    Friend WithEvents lbl_finalSegmentIAS As System.Windows.Forms.Label
    Friend WithEvents lbl_kmhSign2 As System.Windows.Forms.Label
    Friend WithEvents lbl_finalSegmentIASrange As System.Windows.Forms.Label
    Friend WithEvents lbl_finalSegmentLength As System.Windows.Forms.Label
    Friend WithEvents txtBox_finalSegmentLength As System.Windows.Forms.TextBox
    Friend WithEvents lbl_meterSign6 As System.Windows.Forms.Label
    Friend WithEvents lbl_secondSign2 As System.Windows.Forms.Label
    Friend WithEvents lbl_finalSegmentTime As System.Windows.Forms.Label

End Class
