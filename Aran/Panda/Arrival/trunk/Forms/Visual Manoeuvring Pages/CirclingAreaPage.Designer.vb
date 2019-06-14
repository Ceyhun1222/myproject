<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CirclingAreaPage
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
        Me.cmbBox_AircraftCat = New System.Windows.Forms.ComboBox()
        Me.lstVw_RWY = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBox_IAS = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtBox_BankAngle = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtBox_RateOfTurn = New System.Windows.Forms.TextBox()
        Me.txtBox_ADElev = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtBox_MOC = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBox_MinOCH = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtBox_TASWind = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtBox_RadiusOfTurn = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtBox_StraightSegment = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtBox_RadiusFromTHR = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtBox_ObstacleID = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtBox_OCA = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtBox_OCH = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(156, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Aircraft cat :"
        '
        'cmbBox_AircraftCat
        '
        Me.cmbBox_AircraftCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBox_AircraftCat.Items.AddRange(New Object() {"A", "B", "C", "D", "E"})
        Me.cmbBox_AircraftCat.Location = New System.Drawing.Point(226, 16)
        Me.cmbBox_AircraftCat.Name = "cmbBox_AircraftCat"
        Me.cmbBox_AircraftCat.Size = New System.Drawing.Size(62, 21)
        Me.cmbBox_AircraftCat.TabIndex = 1
        '
        'lstVw_RWY
        '
        Me.lstVw_RWY.CheckBoxes = True
        Me.lstVw_RWY.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lstVw_RWY.FullRowSelect = True
        Me.lstVw_RWY.Location = New System.Drawing.Point(15, 16)
        Me.lstVw_RWY.Name = "lstVw_RWY"
        Me.lstVw_RWY.Size = New System.Drawing.Size(125, 136)
        Me.lstVw_RWY.TabIndex = 2
        Me.lstVw_RWY.UseCompatibleStateImageBehavior = False
        Me.lstVw_RWY.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "RWY"
        Me.ColumnHeader1.Width = 120
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(156, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "IAS :"
        '
        'txtBox_IAS
        '
        Me.txtBox_IAS.Location = New System.Drawing.Point(226, 64)
        Me.txtBox_IAS.Name = "txtBox_IAS"
        Me.txtBox_IAS.ReadOnly = True
        Me.txtBox_IAS.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_IAS.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(156, 114)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Bank angle :"
        '
        'txtBox_BankAngle
        '
        Me.txtBox_BankAngle.Location = New System.Drawing.Point(226, 111)
        Me.txtBox_BankAngle.Name = "txtBox_BankAngle"
        Me.txtBox_BankAngle.ReadOnly = True
        Me.txtBox_BankAngle.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_BankAngle.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(156, 159)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Rate of turn :"
        '
        'txtBox_RateOfTurn
        '
        Me.txtBox_RateOfTurn.Location = New System.Drawing.Point(226, 156)
        Me.txtBox_RateOfTurn.Name = "txtBox_RateOfTurn"
        Me.txtBox_RateOfTurn.ReadOnly = True
        Me.txtBox_RateOfTurn.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_RateOfTurn.TabIndex = 8
        '
        'txtBox_ADElev
        '
        Me.txtBox_ADElev.Location = New System.Drawing.Point(226, 204)
        Me.txtBox_ADElev.Name = "txtBox_ADElev"
        Me.txtBox_ADElev.ReadOnly = True
        Me.txtBox_ADElev.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_ADElev.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(156, 194)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 26)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "AD " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "elevation :"
        '
        'txtBox_MOC
        '
        Me.txtBox_MOC.Location = New System.Drawing.Point(226, 246)
        Me.txtBox_MOC.Name = "txtBox_MOC"
        Me.txtBox_MOC.ReadOnly = True
        Me.txtBox_MOC.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_MOC.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(156, 249)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(37, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "MOC :"
        '
        'txtBox_MinOCH
        '
        Me.txtBox_MinOCH.Location = New System.Drawing.Point(226, 289)
        Me.txtBox_MinOCH.Name = "txtBox_MinOCH"
        Me.txtBox_MinOCH.ReadOnly = True
        Me.txtBox_MinOCH.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_MinOCH.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(156, 292)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Min. OCH :"
        '
        'txtBox_TASWind
        '
        Me.txtBox_TASWind.Location = New System.Drawing.Point(430, 16)
        Me.txtBox_TASWind.Name = "txtBox_TASWind"
        Me.txtBox_TASWind.ReadOnly = True
        Me.txtBox_TASWind.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_TASWind.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(345, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "TAS + wind :"
        '
        'txtBox_RadiusOfTurn
        '
        Me.txtBox_RadiusOfTurn.Location = New System.Drawing.Point(430, 64)
        Me.txtBox_RadiusOfTurn.Name = "txtBox_RadiusOfTurn"
        Me.txtBox_RadiusOfTurn.ReadOnly = True
        Me.txtBox_RadiusOfTurn.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_RadiusOfTurn.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(345, 67)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(79, 13)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Raduis of turn :"
        '
        'txtBox_StraightSegment
        '
        Me.txtBox_StraightSegment.Location = New System.Drawing.Point(430, 111)
        Me.txtBox_StraightSegment.Name = "txtBox_StraightSegment"
        Me.txtBox_StraightSegment.ReadOnly = True
        Me.txtBox_StraightSegment.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_StraightSegment.TabIndex = 20
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(345, 101)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 26)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Straight " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "segment :"
        '
        'txtBox_RadiusFromTHR
        '
        Me.txtBox_RadiusFromTHR.Location = New System.Drawing.Point(430, 156)
        Me.txtBox_RadiusFromTHR.Name = "txtBox_RadiusFromTHR"
        Me.txtBox_RadiusFromTHR.ReadOnly = True
        Me.txtBox_RadiusFromTHR.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_RadiusFromTHR.TabIndex = 22
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(345, 146)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(66, 26)
        Me.Label11.TabIndex = 21
        Me.Label11.Text = "Radius from " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "THR :"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(345, 207)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(36, 13)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "OCH :"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(345, 249)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(35, 13)
        Me.Label13.TabIndex = 24
        Me.Label13.Text = "OCA :"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(345, 292)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(69, 13)
        Me.Label14.TabIndex = 25
        Me.Label14.Text = "Obstacle ID :"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(294, 67)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(32, 13)
        Me.Label15.TabIndex = 26
        Me.Label15.Text = "km/h"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(294, 114)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(11, 13)
        Me.Label16.TabIndex = 27
        Me.Label16.Text = "°"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(294, 159)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(33, 13)
        Me.Label17.TabIndex = 28
        Me.Label17.Text = "°/sec"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(294, 207)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(15, 13)
        Me.Label18.TabIndex = 29
        Me.Label18.Text = "m"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(294, 249)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(15, 13)
        Me.Label19.TabIndex = 30
        Me.Label19.Text = "m"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(294, 292)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(15, 13)
        Me.Label20.TabIndex = 31
        Me.Label20.Text = "m"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(498, 19)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(32, 13)
        Me.Label21.TabIndex = 32
        Me.Label21.Text = "km/h"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(498, 67)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(21, 13)
        Me.Label22.TabIndex = 41
        Me.Label22.Text = "km"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(498, 114)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(21, 13)
        Me.Label23.TabIndex = 40
        Me.Label23.Text = "km"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(498, 159)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(21, 13)
        Me.Label24.TabIndex = 39
        Me.Label24.Text = "km"
        '
        'txtBox_ObstacleID
        '
        Me.txtBox_ObstacleID.Location = New System.Drawing.Point(430, 289)
        Me.txtBox_ObstacleID.Name = "txtBox_ObstacleID"
        Me.txtBox_ObstacleID.ReadOnly = True
        Me.txtBox_ObstacleID.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_ObstacleID.TabIndex = 38
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(498, 207)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(15, 13)
        Me.Label25.TabIndex = 37
        Me.Label25.Text = "m"
        '
        'txtBox_OCA
        '
        Me.txtBox_OCA.Location = New System.Drawing.Point(430, 246)
        Me.txtBox_OCA.Name = "txtBox_OCA"
        Me.txtBox_OCA.ReadOnly = True
        Me.txtBox_OCA.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_OCA.TabIndex = 36
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(498, 249)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(15, 13)
        Me.Label26.TabIndex = 35
        Me.Label26.Text = "m"
        '
        'txtBox_OCH
        '
        Me.txtBox_OCH.Location = New System.Drawing.Point(430, 204)
        Me.txtBox_OCH.Name = "txtBox_OCH"
        Me.txtBox_OCH.ReadOnly = True
        Me.txtBox_OCH.Size = New System.Drawing.Size(62, 20)
        Me.txtBox_OCH.TabIndex = 34
        '
        'CirclingAreaPage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.txtBox_ObstacleID)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.txtBox_OCA)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.txtBox_OCH)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtBox_RadiusFromTHR)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtBox_StraightSegment)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtBox_RadiusOfTurn)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtBox_TASWind)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtBox_MinOCH)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtBox_MOC)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtBox_ADElev)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtBox_RateOfTurn)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtBox_BankAngle)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtBox_IAS)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lstVw_RWY)
        Me.Controls.Add(Me.cmbBox_AircraftCat)
        Me.Controls.Add(Me.Label1)
        Me.Name = "CirclingAreaPage"
        Me.Size = New System.Drawing.Size(560, 382)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbBox_AircraftCat As System.Windows.Forms.ComboBox
    Friend WithEvents lstVw_RWY As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBox_IAS As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBox_BankAngle As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtBox_RateOfTurn As System.Windows.Forms.TextBox
    Friend WithEvents txtBox_ADElev As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtBox_MOC As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtBox_MinOCH As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtBox_TASWind As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtBox_RadiusOfTurn As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtBox_StraightSegment As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtBox_RadiusFromTHR As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtBox_ObstacleID As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txtBox_OCA As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtBox_OCH As System.Windows.Forms.TextBox

End Class
