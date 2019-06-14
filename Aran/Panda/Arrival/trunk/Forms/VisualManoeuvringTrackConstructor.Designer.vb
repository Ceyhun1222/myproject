<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VisualManoeuvringTrackConstructor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VisualManoeuvringTrackConstructor))
        Me.grpBox_targetPnt = New System.Windows.Forms.GroupBox()
        Me.chkBox_rightGE180turn = New System.Windows.Forms.CheckBox()
        Me.chkBox_leftGE180turn = New System.Windows.Forms.CheckBox()
        Me.lbl_TargetPntAngleRange = New System.Windows.Forms.Label()
        Me.nmrcUpDown_TargetPntAngle = New System.Windows.Forms.NumericUpDown()
        Me.chkBox_finalStep = New System.Windows.Forms.CheckBox()
        Me.chkBox_isOnCirclingBox = New System.Windows.Forms.CheckBox()
        Me.btn_targetPntDone = New System.Windows.Forms.Button()
        Me.btn_pickTargetPnt = New System.Windows.Forms.Button()
        Me.cmbBox_targetPntLonSide = New System.Windows.Forms.ComboBox()
        Me.cmbBox_targetPntLatSide = New System.Windows.Forms.ComboBox()
        Me.lbl_targetPntAngleDegree = New System.Windows.Forms.Label()
        Me.lbl_targetPntLonSecond = New System.Windows.Forms.Label()
        Me.txtBox_targetPntLonSecond = New System.Windows.Forms.TextBox()
        Me.lbl_targetPntLonMinute = New System.Windows.Forms.Label()
        Me.txtBox_targetPntLonMinute = New System.Windows.Forms.TextBox()
        Me.lbl_targetPntLonDegree = New System.Windows.Forms.Label()
        Me.txtBox_targetPntLonDegree = New System.Windows.Forms.TextBox()
        Me.lbl_targetPntLatSecond = New System.Windows.Forms.Label()
        Me.lbl_targetPntLatMinute = New System.Windows.Forms.Label()
        Me.txtBox_targetPntLatSecond = New System.Windows.Forms.TextBox()
        Me.txtBox_targetPntLatMinute = New System.Windows.Forms.TextBox()
        Me.lbl_targetPntLatDegree = New System.Windows.Forms.Label()
        Me.txtBox_targetPntLatDegree = New System.Windows.Forms.TextBox()
        Me.lbl_tagetPntAngle = New System.Windows.Forms.Label()
        Me.lbl_targetPntLongitude = New System.Windows.Forms.Label()
        Me.lbl_targetPntLatitude = New System.Windows.Forms.Label()
        Me.grpBox_visRefPntPair = New System.Windows.Forms.GroupBox()
        Me.lbl_convergencePnt = New System.Windows.Forms.Label()
        Me.lbl_divergencePnt = New System.Windows.Forms.Label()
        Me.cmbBox_convergencePnt = New System.Windows.Forms.ComboBox()
        Me.cmbBox_divergencePnt = New System.Windows.Forms.ComboBox()
        Me.btn_SaveTrack = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_RemoveStep = New System.Windows.Forms.Button()
        Me.btn_AddStep = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lbl_ConvergenceAngle = New System.Windows.Forms.Label()
        Me.lbl_DivergenceAngle = New System.Windows.Forms.Label()
        Me.nmrcUpDown_ConvergenceAngle = New System.Windows.Forms.NumericUpDown()
        Me.nmrcUpDown_DivergenceAngle = New System.Windows.Forms.NumericUpDown()
        Me.grpBox_targetPnt.SuspendLayout()
        CType(Me.nmrcUpDown_TargetPntAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBox_visRefPntPair.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nmrcUpDown_ConvergenceAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmrcUpDown_DivergenceAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpBox_targetPnt
        '
        Me.grpBox_targetPnt.Controls.Add(Me.chkBox_rightGE180turn)
        Me.grpBox_targetPnt.Controls.Add(Me.chkBox_leftGE180turn)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_TargetPntAngleRange)
        Me.grpBox_targetPnt.Controls.Add(Me.nmrcUpDown_TargetPntAngle)
        Me.grpBox_targetPnt.Controls.Add(Me.chkBox_finalStep)
        Me.grpBox_targetPnt.Controls.Add(Me.chkBox_isOnCirclingBox)
        Me.grpBox_targetPnt.Controls.Add(Me.btn_targetPntDone)
        Me.grpBox_targetPnt.Controls.Add(Me.btn_pickTargetPnt)
        Me.grpBox_targetPnt.Controls.Add(Me.cmbBox_targetPntLonSide)
        Me.grpBox_targetPnt.Controls.Add(Me.cmbBox_targetPntLatSide)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntAngleDegree)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLonSecond)
        Me.grpBox_targetPnt.Controls.Add(Me.txtBox_targetPntLonSecond)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLonMinute)
        Me.grpBox_targetPnt.Controls.Add(Me.txtBox_targetPntLonMinute)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLonDegree)
        Me.grpBox_targetPnt.Controls.Add(Me.txtBox_targetPntLonDegree)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLatSecond)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLatMinute)
        Me.grpBox_targetPnt.Controls.Add(Me.txtBox_targetPntLatSecond)
        Me.grpBox_targetPnt.Controls.Add(Me.txtBox_targetPntLatMinute)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLatDegree)
        Me.grpBox_targetPnt.Controls.Add(Me.txtBox_targetPntLatDegree)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_tagetPntAngle)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLongitude)
        Me.grpBox_targetPnt.Controls.Add(Me.lbl_targetPntLatitude)
        Me.grpBox_targetPnt.Location = New System.Drawing.Point(12, 12)
        Me.grpBox_targetPnt.Name = "grpBox_targetPnt"
        Me.grpBox_targetPnt.Size = New System.Drawing.Size(282, 216)
        Me.grpBox_targetPnt.TabIndex = 42
        Me.grpBox_targetPnt.TabStop = False
        Me.grpBox_targetPnt.Text = "Target point"
        '
        'chkBox_rightGE180turn
        '
        Me.chkBox_rightGE180turn.AutoSize = True
        Me.chkBox_rightGE180turn.Location = New System.Drawing.Point(128, 166)
        Me.chkBox_rightGE180turn.Name = "chkBox_rightGE180turn"
        Me.chkBox_rightGE180turn.Size = New System.Drawing.Size(109, 17)
        Me.chkBox_rightGE180turn.TabIndex = 47
        Me.chkBox_rightGE180turn.Text = "Right >=180° turn"
        Me.chkBox_rightGE180turn.UseVisualStyleBackColor = True
        '
        'chkBox_leftGE180turn
        '
        Me.chkBox_leftGE180turn.AutoSize = True
        Me.chkBox_leftGE180turn.Location = New System.Drawing.Point(128, 143)
        Me.chkBox_leftGE180turn.Name = "chkBox_leftGE180turn"
        Me.chkBox_leftGE180turn.Size = New System.Drawing.Size(105, 17)
        Me.chkBox_leftGE180turn.TabIndex = 46
        Me.chkBox_leftGE180turn.Text = "Left >= 180° turn"
        Me.chkBox_leftGE180turn.UseVisualStyleBackColor = True
        '
        'lbl_TargetPntAngleRange
        '
        Me.lbl_TargetPntAngleRange.AutoSize = True
        Me.lbl_TargetPntAngleRange.Location = New System.Drawing.Point(148, 116)
        Me.lbl_TargetPntAngleRange.Name = "lbl_TargetPntAngleRange"
        Me.lbl_TargetPntAngleRange.Size = New System.Drawing.Size(22, 13)
        Me.lbl_TargetPntAngleRange.TabIndex = 45
        Me.lbl_TargetPntAngleRange.Text = "xxx"
        '
        'nmrcUpDown_TargetPntAngle
        '
        Me.nmrcUpDown_TargetPntAngle.BackColor = System.Drawing.Color.White
        Me.nmrcUpDown_TargetPntAngle.Enabled = False
        Me.nmrcUpDown_TargetPntAngle.Location = New System.Drawing.Point(56, 111)
        Me.nmrcUpDown_TargetPntAngle.Name = "nmrcUpDown_TargetPntAngle"
        Me.nmrcUpDown_TargetPntAngle.Size = New System.Drawing.Size(81, 20)
        Me.nmrcUpDown_TargetPntAngle.TabIndex = 44
        '
        'chkBox_finalStep
        '
        Me.chkBox_finalStep.AutoSize = True
        Me.chkBox_finalStep.Location = New System.Drawing.Point(14, 146)
        Me.chkBox_finalStep.Name = "chkBox_finalStep"
        Me.chkBox_finalStep.Size = New System.Drawing.Size(71, 17)
        Me.chkBox_finalStep.TabIndex = 42
        Me.chkBox_finalStep.Text = "Final step"
        Me.chkBox_finalStep.UseVisualStyleBackColor = True
        '
        'chkBox_isOnCirclingBox
        '
        Me.chkBox_isOnCirclingBox.AutoSize = True
        Me.chkBox_isOnCirclingBox.Location = New System.Drawing.Point(14, 166)
        Me.chkBox_isOnCirclingBox.Name = "chkBox_isOnCirclingBox"
        Me.chkBox_isOnCirclingBox.Size = New System.Drawing.Size(105, 17)
        Me.chkBox_isOnCirclingBox.TabIndex = 41
        Me.chkBox_isOnCirclingBox.Text = "Is on circling box"
        Me.chkBox_isOnCirclingBox.UseVisualStyleBackColor = True
        '
        'btn_targetPntDone
        '
        Me.btn_targetPntDone.Location = New System.Drawing.Point(226, 187)
        Me.btn_targetPntDone.Name = "btn_targetPntDone"
        Me.btn_targetPntDone.Size = New System.Drawing.Size(48, 23)
        Me.btn_targetPntDone.TabIndex = 40
        Me.btn_targetPntDone.Text = "Done"
        Me.btn_targetPntDone.UseVisualStyleBackColor = True
        '
        'btn_pickTargetPnt
        '
        Me.btn_pickTargetPnt.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btn_pickTargetPnt.Location = New System.Drawing.Point(226, 111)
        Me.btn_pickTargetPnt.Name = "btn_pickTargetPnt"
        Me.btn_pickTargetPnt.Size = New System.Drawing.Size(48, 23)
        Me.btn_pickTargetPnt.TabIndex = 25
        Me.btn_pickTargetPnt.Text = "+"
        Me.btn_pickTargetPnt.UseVisualStyleBackColor = True
        '
        'cmbBox_targetPntLonSide
        '
        Me.cmbBox_targetPntLonSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_targetPntLonSide.FormattingEnabled = True
        Me.cmbBox_targetPntLonSide.Location = New System.Drawing.Point(226, 67)
        Me.cmbBox_targetPntLonSide.Name = "cmbBox_targetPntLonSide"
        Me.cmbBox_targetPntLonSide.Size = New System.Drawing.Size(48, 21)
        Me.cmbBox_targetPntLonSide.TabIndex = 39
        '
        'cmbBox_targetPntLatSide
        '
        Me.cmbBox_targetPntLatSide.AllowDrop = True
        Me.cmbBox_targetPntLatSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_targetPntLatSide.Location = New System.Drawing.Point(226, 27)
        Me.cmbBox_targetPntLatSide.Name = "cmbBox_targetPntLatSide"
        Me.cmbBox_targetPntLatSide.Size = New System.Drawing.Size(48, 21)
        Me.cmbBox_targetPntLatSide.TabIndex = 38
        '
        'lbl_targetPntAngleDegree
        '
        Me.lbl_targetPntAngleDegree.AutoSize = True
        Me.lbl_targetPntAngleDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntAngleDegree.Location = New System.Drawing.Point(136, 113)
        Me.lbl_targetPntAngleDegree.Name = "lbl_targetPntAngleDegree"
        Me.lbl_targetPntAngleDegree.Size = New System.Drawing.Size(11, 13)
        Me.lbl_targetPntAngleDegree.TabIndex = 37
        Me.lbl_targetPntAngleDegree.Text = "°"
        '
        'lbl_targetPntLonSecond
        '
        Me.lbl_targetPntLonSecond.AutoSize = True
        Me.lbl_targetPntLonSecond.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLonSecond.Location = New System.Drawing.Point(208, 68)
        Me.lbl_targetPntLonSecond.Name = "lbl_targetPntLonSecond"
        Me.lbl_targetPntLonSecond.Size = New System.Drawing.Size(12, 13)
        Me.lbl_targetPntLonSecond.TabIndex = 35
        Me.lbl_targetPntLonSecond.Text = """"
        '
        'txtBox_targetPntLonSecond
        '
        Me.txtBox_targetPntLonSecond.BackColor = System.Drawing.Color.White
        Me.txtBox_targetPntLonSecond.Location = New System.Drawing.Point(148, 67)
        Me.txtBox_targetPntLonSecond.Name = "txtBox_targetPntLonSecond"
        Me.txtBox_targetPntLonSecond.ReadOnly = True
        Me.txtBox_targetPntLonSecond.Size = New System.Drawing.Size(61, 20)
        Me.txtBox_targetPntLonSecond.TabIndex = 34
        '
        'lbl_targetPntLonMinute
        '
        Me.lbl_targetPntLonMinute.AutoSize = True
        Me.lbl_targetPntLonMinute.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLonMinute.Location = New System.Drawing.Point(131, 67)
        Me.lbl_targetPntLonMinute.Name = "lbl_targetPntLonMinute"
        Me.lbl_targetPntLonMinute.Size = New System.Drawing.Size(9, 13)
        Me.lbl_targetPntLonMinute.TabIndex = 33
        Me.lbl_targetPntLonMinute.Text = "'"
        '
        'txtBox_targetPntLonMinute
        '
        Me.txtBox_targetPntLonMinute.BackColor = System.Drawing.Color.White
        Me.txtBox_targetPntLonMinute.Location = New System.Drawing.Point(102, 67)
        Me.txtBox_targetPntLonMinute.Name = "txtBox_targetPntLonMinute"
        Me.txtBox_targetPntLonMinute.ReadOnly = True
        Me.txtBox_targetPntLonMinute.Size = New System.Drawing.Size(30, 20)
        Me.txtBox_targetPntLonMinute.TabIndex = 32
        '
        'lbl_targetPntLonDegree
        '
        Me.lbl_targetPntLonDegree.AutoSize = True
        Me.lbl_targetPntLonDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLonDegree.Location = New System.Drawing.Point(85, 67)
        Me.lbl_targetPntLonDegree.Name = "lbl_targetPntLonDegree"
        Me.lbl_targetPntLonDegree.Size = New System.Drawing.Size(11, 13)
        Me.lbl_targetPntLonDegree.TabIndex = 31
        Me.lbl_targetPntLonDegree.Text = "°"
        '
        'txtBox_targetPntLonDegree
        '
        Me.txtBox_targetPntLonDegree.BackColor = System.Drawing.Color.White
        Me.txtBox_targetPntLonDegree.Location = New System.Drawing.Point(56, 67)
        Me.txtBox_targetPntLonDegree.Name = "txtBox_targetPntLonDegree"
        Me.txtBox_targetPntLonDegree.ReadOnly = True
        Me.txtBox_targetPntLonDegree.Size = New System.Drawing.Size(30, 20)
        Me.txtBox_targetPntLonDegree.TabIndex = 30
        '
        'lbl_targetPntLatSecond
        '
        Me.lbl_targetPntLatSecond.AutoSize = True
        Me.lbl_targetPntLatSecond.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLatSecond.Location = New System.Drawing.Point(208, 27)
        Me.lbl_targetPntLatSecond.Name = "lbl_targetPntLatSecond"
        Me.lbl_targetPntLatSecond.Size = New System.Drawing.Size(12, 13)
        Me.lbl_targetPntLatSecond.TabIndex = 29
        Me.lbl_targetPntLatSecond.Text = """"
        '
        'lbl_targetPntLatMinute
        '
        Me.lbl_targetPntLatMinute.AutoSize = True
        Me.lbl_targetPntLatMinute.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLatMinute.Location = New System.Drawing.Point(131, 27)
        Me.lbl_targetPntLatMinute.Name = "lbl_targetPntLatMinute"
        Me.lbl_targetPntLatMinute.Size = New System.Drawing.Size(9, 13)
        Me.lbl_targetPntLatMinute.TabIndex = 28
        Me.lbl_targetPntLatMinute.Text = "'"
        '
        'txtBox_targetPntLatSecond
        '
        Me.txtBox_targetPntLatSecond.BackColor = System.Drawing.Color.White
        Me.txtBox_targetPntLatSecond.Location = New System.Drawing.Point(148, 28)
        Me.txtBox_targetPntLatSecond.Name = "txtBox_targetPntLatSecond"
        Me.txtBox_targetPntLatSecond.ReadOnly = True
        Me.txtBox_targetPntLatSecond.Size = New System.Drawing.Size(61, 20)
        Me.txtBox_targetPntLatSecond.TabIndex = 27
        '
        'txtBox_targetPntLatMinute
        '
        Me.txtBox_targetPntLatMinute.BackColor = System.Drawing.Color.White
        Me.txtBox_targetPntLatMinute.Location = New System.Drawing.Point(102, 28)
        Me.txtBox_targetPntLatMinute.Name = "txtBox_targetPntLatMinute"
        Me.txtBox_targetPntLatMinute.ReadOnly = True
        Me.txtBox_targetPntLatMinute.Size = New System.Drawing.Size(30, 20)
        Me.txtBox_targetPntLatMinute.TabIndex = 26
        '
        'lbl_targetPntLatDegree
        '
        Me.lbl_targetPntLatDegree.AutoSize = True
        Me.lbl_targetPntLatDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLatDegree.Location = New System.Drawing.Point(85, 28)
        Me.lbl_targetPntLatDegree.Name = "lbl_targetPntLatDegree"
        Me.lbl_targetPntLatDegree.Size = New System.Drawing.Size(11, 13)
        Me.lbl_targetPntLatDegree.TabIndex = 25
        Me.lbl_targetPntLatDegree.Text = "°"
        '
        'txtBox_targetPntLatDegree
        '
        Me.txtBox_targetPntLatDegree.BackColor = System.Drawing.Color.White
        Me.txtBox_targetPntLatDegree.Location = New System.Drawing.Point(56, 28)
        Me.txtBox_targetPntLatDegree.Name = "txtBox_targetPntLatDegree"
        Me.txtBox_targetPntLatDegree.ReadOnly = True
        Me.txtBox_targetPntLatDegree.Size = New System.Drawing.Size(30, 20)
        Me.txtBox_targetPntLatDegree.TabIndex = 24
        '
        'lbl_tagetPntAngle
        '
        Me.lbl_tagetPntAngle.AutoSize = True
        Me.lbl_tagetPntAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_tagetPntAngle.Location = New System.Drawing.Point(11, 116)
        Me.lbl_tagetPntAngle.Name = "lbl_tagetPntAngle"
        Me.lbl_tagetPntAngle.Size = New System.Drawing.Size(37, 13)
        Me.lbl_tagetPntAngle.TabIndex = 23
        Me.lbl_tagetPntAngle.Text = "Angle:"
        '
        'lbl_targetPntLongitude
        '
        Me.lbl_targetPntLongitude.AutoSize = True
        Me.lbl_targetPntLongitude.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLongitude.Location = New System.Drawing.Point(11, 70)
        Me.lbl_targetPntLongitude.Name = "lbl_targetPntLongitude"
        Me.lbl_targetPntLongitude.Size = New System.Drawing.Size(28, 13)
        Me.lbl_targetPntLongitude.TabIndex = 22
        Me.lbl_targetPntLongitude.Text = "Lon:"
        '
        'lbl_targetPntLatitude
        '
        Me.lbl_targetPntLatitude.AutoSize = True
        Me.lbl_targetPntLatitude.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_targetPntLatitude.Location = New System.Drawing.Point(11, 31)
        Me.lbl_targetPntLatitude.Name = "lbl_targetPntLatitude"
        Me.lbl_targetPntLatitude.Size = New System.Drawing.Size(25, 13)
        Me.lbl_targetPntLatitude.TabIndex = 21
        Me.lbl_targetPntLatitude.Text = "Lat:"
        '
        'grpBox_visRefPntPair
        '
        Me.grpBox_visRefPntPair.Controls.Add(Me.lbl_convergencePnt)
        Me.grpBox_visRefPntPair.Controls.Add(Me.lbl_divergencePnt)
        Me.grpBox_visRefPntPair.Controls.Add(Me.cmbBox_convergencePnt)
        Me.grpBox_visRefPntPair.Controls.Add(Me.cmbBox_divergencePnt)
        Me.grpBox_visRefPntPair.Location = New System.Drawing.Point(13, 327)
        Me.grpBox_visRefPntPair.Name = "grpBox_visRefPntPair"
        Me.grpBox_visRefPntPair.Size = New System.Drawing.Size(282, 85)
        Me.grpBox_visRefPntPair.TabIndex = 46
        Me.grpBox_visRefPntPair.TabStop = False
        Me.grpBox_visRefPntPair.Text = "Visual reference points"
        '
        'lbl_convergencePnt
        '
        Me.lbl_convergencePnt.AutoSize = True
        Me.lbl_convergencePnt.Location = New System.Drawing.Point(147, 26)
        Me.lbl_convergencePnt.Name = "lbl_convergencePnt"
        Me.lbl_convergencePnt.Size = New System.Drawing.Size(100, 13)
        Me.lbl_convergencePnt.TabIndex = 3
        Me.lbl_convergencePnt.Text = "Convergence point:"
        '
        'lbl_divergencePnt
        '
        Me.lbl_divergencePnt.AutoSize = True
        Me.lbl_divergencePnt.Location = New System.Drawing.Point(13, 27)
        Me.lbl_divergencePnt.Name = "lbl_divergencePnt"
        Me.lbl_divergencePnt.Size = New System.Drawing.Size(91, 13)
        Me.lbl_divergencePnt.TabIndex = 2
        Me.lbl_divergencePnt.Text = "Divergence point:"
        '
        'cmbBox_convergencePnt
        '
        Me.cmbBox_convergencePnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_convergencePnt.FormattingEnabled = True
        Me.cmbBox_convergencePnt.Location = New System.Drawing.Point(147, 46)
        Me.cmbBox_convergencePnt.Name = "cmbBox_convergencePnt"
        Me.cmbBox_convergencePnt.Size = New System.Drawing.Size(100, 21)
        Me.cmbBox_convergencePnt.TabIndex = 1
        '
        'cmbBox_divergencePnt
        '
        Me.cmbBox_divergencePnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_divergencePnt.FormattingEnabled = True
        Me.cmbBox_divergencePnt.Location = New System.Drawing.Point(13, 46)
        Me.cmbBox_divergencePnt.Name = "cmbBox_divergencePnt"
        Me.cmbBox_divergencePnt.Size = New System.Drawing.Size(100, 21)
        Me.cmbBox_divergencePnt.TabIndex = 0
        '
        'btn_SaveTrack
        '
        Me.btn_SaveTrack.Location = New System.Drawing.Point(160, 418)
        Me.btn_SaveTrack.Name = "btn_SaveTrack"
        Me.btn_SaveTrack.Size = New System.Drawing.Size(135, 30)
        Me.btn_SaveTrack.TabIndex = 53
        Me.btn_SaveTrack.Text = "Save"
        Me.btn_SaveTrack.UseVisualStyleBackColor = True
        '
        'btn_Close
        '
        Me.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Close.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btn_Close.Location = New System.Drawing.Point(160, 453)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(135, 30)
        Me.btn_Close.TabIndex = 52
        Me.btn_Close.Text = "Close"
        Me.btn_Close.UseVisualStyleBackColor = True
        '
        'btn_RemoveStep
        '
        Me.btn_RemoveStep.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_RemoveStep.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btn_RemoveStep.Location = New System.Drawing.Point(12, 454)
        Me.btn_RemoveStep.Name = "btn_RemoveStep"
        Me.btn_RemoveStep.Size = New System.Drawing.Size(135, 30)
        Me.btn_RemoveStep.TabIndex = 51
        Me.btn_RemoveStep.Text = "Remove step"
        Me.btn_RemoveStep.UseVisualStyleBackColor = True
        '
        'btn_AddStep
        '
        Me.btn_AddStep.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_AddStep.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btn_AddStep.Location = New System.Drawing.Point(12, 418)
        Me.btn_AddStep.Name = "btn_AddStep"
        Me.btn_AddStep.Size = New System.Drawing.Size(135, 30)
        Me.btn_AddStep.TabIndex = 50
        Me.btn_AddStep.Text = "Add step"
        Me.btn_AddStep.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lbl_ConvergenceAngle)
        Me.GroupBox1.Controls.Add(Me.lbl_DivergenceAngle)
        Me.GroupBox1.Controls.Add(Me.nmrcUpDown_ConvergenceAngle)
        Me.GroupBox1.Controls.Add(Me.nmrcUpDown_DivergenceAngle)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 228)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(282, 93)
        Me.GroupBox1.TabIndex = 54
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Visible = False
        '
        'lbl_ConvergenceAngle
        '
        Me.lbl_ConvergenceAngle.AutoSize = True
        Me.lbl_ConvergenceAngle.Location = New System.Drawing.Point(11, 62)
        Me.lbl_ConvergenceAngle.Name = "lbl_ConvergenceAngle"
        Me.lbl_ConvergenceAngle.Size = New System.Drawing.Size(103, 13)
        Me.lbl_ConvergenceAngle.TabIndex = 3
        Me.lbl_ConvergenceAngle.Text = "Convergence angle:"
        '
        'lbl_DivergenceAngle
        '
        Me.lbl_DivergenceAngle.AutoSize = True
        Me.lbl_DivergenceAngle.Location = New System.Drawing.Point(11, 25)
        Me.lbl_DivergenceAngle.Name = "lbl_DivergenceAngle"
        Me.lbl_DivergenceAngle.Size = New System.Drawing.Size(94, 13)
        Me.lbl_DivergenceAngle.TabIndex = 2
        Me.lbl_DivergenceAngle.Text = "Divergence angle:"
        '
        'nmrcUpDown_ConvergenceAngle
        '
        Me.nmrcUpDown_ConvergenceAngle.BackColor = System.Drawing.Color.White
        Me.nmrcUpDown_ConvergenceAngle.Location = New System.Drawing.Point(138, 60)
        Me.nmrcUpDown_ConvergenceAngle.Name = "nmrcUpDown_ConvergenceAngle"
        Me.nmrcUpDown_ConvergenceAngle.ReadOnly = True
        Me.nmrcUpDown_ConvergenceAngle.Size = New System.Drawing.Size(79, 20)
        Me.nmrcUpDown_ConvergenceAngle.TabIndex = 1
        '
        'nmrcUpDown_DivergenceAngle
        '
        Me.nmrcUpDown_DivergenceAngle.BackColor = System.Drawing.Color.White
        Me.nmrcUpDown_DivergenceAngle.Location = New System.Drawing.Point(138, 23)
        Me.nmrcUpDown_DivergenceAngle.Name = "nmrcUpDown_DivergenceAngle"
        Me.nmrcUpDown_DivergenceAngle.ReadOnly = True
        Me.nmrcUpDown_DivergenceAngle.Size = New System.Drawing.Size(79, 20)
        Me.nmrcUpDown_DivergenceAngle.TabIndex = 0
        '
        'VisualManoeuvringTrackConstructor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btn_Close
        Me.ClientSize = New System.Drawing.Size(306, 490)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btn_SaveTrack)
        Me.Controls.Add(Me.btn_Close)
        Me.Controls.Add(Me.btn_RemoveStep)
        Me.Controls.Add(Me.btn_AddStep)
        Me.Controls.Add(Me.grpBox_visRefPntPair)
        Me.Controls.Add(Me.grpBox_targetPnt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "VisualManoeuvringTrackConstructor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Track constructor"
        Me.grpBox_targetPnt.ResumeLayout(False)
        Me.grpBox_targetPnt.PerformLayout()
        CType(Me.nmrcUpDown_TargetPntAngle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBox_visRefPntPair.ResumeLayout(False)
        Me.grpBox_visRefPntPair.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nmrcUpDown_ConvergenceAngle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmrcUpDown_DivergenceAngle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpBox_targetPnt As System.Windows.Forms.GroupBox
    Friend WithEvents chkBox_isOnCirclingBox As System.Windows.Forms.CheckBox
    Friend WithEvents btn_targetPntDone As System.Windows.Forms.Button
    Friend WithEvents btn_pickTargetPnt As System.Windows.Forms.Button
    Friend WithEvents cmbBox_targetPntLonSide As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBox_targetPntLatSide As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_targetPntLonSecond As System.Windows.Forms.Label
    Friend WithEvents txtBox_targetPntLonSecond As System.Windows.Forms.TextBox
    Friend WithEvents lbl_targetPntLonMinute As System.Windows.Forms.Label
    Friend WithEvents txtBox_targetPntLonMinute As System.Windows.Forms.TextBox
    Friend WithEvents lbl_targetPntLonDegree As System.Windows.Forms.Label
    Friend WithEvents txtBox_targetPntLonDegree As System.Windows.Forms.TextBox
    Friend WithEvents lbl_targetPntLatSecond As System.Windows.Forms.Label
    Friend WithEvents lbl_targetPntLatMinute As System.Windows.Forms.Label
    Friend WithEvents txtBox_targetPntLatSecond As System.Windows.Forms.TextBox
    Friend WithEvents txtBox_targetPntLatMinute As System.Windows.Forms.TextBox
    Friend WithEvents lbl_targetPntLatDegree As System.Windows.Forms.Label
    Friend WithEvents txtBox_targetPntLatDegree As System.Windows.Forms.TextBox
    Friend WithEvents lbl_tagetPntAngle As System.Windows.Forms.Label
    Friend WithEvents lbl_targetPntLongitude As System.Windows.Forms.Label
    Friend WithEvents lbl_targetPntLatitude As System.Windows.Forms.Label
    Friend WithEvents grpBox_visRefPntPair As System.Windows.Forms.GroupBox
    Friend WithEvents lbl_convergencePnt As System.Windows.Forms.Label
    Friend WithEvents lbl_divergencePnt As System.Windows.Forms.Label
    Friend WithEvents cmbBox_convergencePnt As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBox_divergencePnt As System.Windows.Forms.ComboBox
    Friend WithEvents btn_SaveTrack As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_RemoveStep As System.Windows.Forms.Button
    Friend WithEvents btn_AddStep As System.Windows.Forms.Button
    Friend WithEvents chkBox_finalStep As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents nmrcUpDown_ConvergenceAngle As System.Windows.Forms.NumericUpDown
    Friend WithEvents nmrcUpDown_DivergenceAngle As System.Windows.Forms.NumericUpDown
    Friend WithEvents lbl_ConvergenceAngle As System.Windows.Forms.Label
    Friend WithEvents lbl_DivergenceAngle As System.Windows.Forms.Label
    Friend WithEvents nmrcUpDown_TargetPntAngle As System.Windows.Forms.NumericUpDown
    Friend WithEvents lbl_TargetPntAngleRange As System.Windows.Forms.Label
    Friend WithEvents lbl_targetPntAngleDegree As System.Windows.Forms.Label
    Friend WithEvents chkBox_leftGE180turn As System.Windows.Forms.CheckBox
    Friend WithEvents chkBox_rightGE180turn As System.Windows.Forms.CheckBox
End Class
