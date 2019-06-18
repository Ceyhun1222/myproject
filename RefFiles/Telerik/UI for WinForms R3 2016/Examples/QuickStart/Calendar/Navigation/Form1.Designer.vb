
Namespace Telerik.Examples.WinControls.Calendar.Navigation
    Partial Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.radCalendar1 = New Telerik.WinControls.UI.RadCalendar()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBox3 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radLabel9 = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker3 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radDateTimePicker1 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel8 = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker2 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel7 = New Telerik.WinControls.UI.RadLabel()
            Me.radGroupRenderOpts = New Telerik.WinControls.UI.RadGroupBox()
            Me.radGroupDateOpts = New Telerik.WinControls.UI.RadGroupBox()
            Me.radSpinNavigationStep = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radDateTimePicker4 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radSpinEditor1 = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCalendar1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel9, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDateTimePicker3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupRenderOpts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupRenderOpts.SuspendLayout()
            DirectCast(Me.radGroupDateOpts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupDateOpts.SuspendLayout()
            DirectCast(Me.radSpinNavigationStep, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            DirectCast(Me.radDateTimePicker4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radSpinEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Controls.Add(Me.radGroupRenderOpts)
            Me.settingsPanel.Controls.Add(Me.radGroupDateOpts)
            Me.settingsPanel.Location = New System.Drawing.Point(729, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(250, 867)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupDateOpts, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupRenderOpts, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            ' 
            ' radCalendar1
            ' 
            Me.radCalendar1.BackColor = System.Drawing.Color.FromArgb(CInt(CByte(248)), CInt(CByte(248)), CInt(CByte(248)))
            Me.radCalendar1.FocusedDate = New System.DateTime(2009, 3, 7, 0, 0, 0, _
             0)
            Me.radCalendar1.ForeColor = System.Drawing.Color.Black
            Me.radCalendar1.Location = New System.Drawing.Point(0, 0)
            Me.radCalendar1.Name = "radCalendar1"
            Me.radCalendar1.Size = New System.Drawing.Size(288, 229)
            Me.radCalendar1.TabIndex = 0
            Me.radCalendar1.Text = "radCalendar1"
            Me.radCalendar1.ZoomFactor = 1.2F
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(5, 147)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(89, 18)
            Me.radLabel1.TabIndex = 4
            Me.radLabel1.Text = "Navigate to date"
            Me.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBox1.Location = New System.Drawing.Point(5, 50)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(147, 18)
            Me.radCheckBox1.TabIndex = 9
            Me.radCheckBox1.Text = "Show Navigation Buttons"
            Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBox2
            ' 
            Me.radCheckBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox2.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBox2.Location = New System.Drawing.Point(5, 73)
            Me.radCheckBox2.Name = "radCheckBox2"
            Me.radCheckBox2.Size = New System.Drawing.Size(128, 18)
            Me.radCheckBox2.TabIndex = 10
            Me.radCheckBox2.Text = "Show Fast Navigation"
            Me.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radCheckBox3
            ' 
            Me.radCheckBox3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox3.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBox3.Location = New System.Drawing.Point(5, 27)
            Me.radCheckBox3.Name = "radCheckBox3"
            Me.radCheckBox3.Size = New System.Drawing.Size(124, 18)
            Me.radCheckBox3.TabIndex = 11
            Me.radCheckBox3.Text = "Show Navigation Bar"
            Me.radCheckBox3.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radLabel9
            ' 
            Me.radLabel9.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel9.Location = New System.Drawing.Point(5, 129)
            Me.radLabel9.Name = "radLabel9"
            Me.radLabel9.Size = New System.Drawing.Size(106, 18)
            Me.radLabel9.TabIndex = 23
            Me.radLabel9.Text = "Fast navigation step"
            Me.radLabel9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            ' 
            ' radDateTimePicker3
            ' 
            Me.radDateTimePicker3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDateTimePicker3.Checked = True
            Me.radDateTimePicker3.Location = New System.Drawing.Point(5, 168)
            Me.radDateTimePicker3.MinDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker3.Name = "radDateTimePicker3"
            Me.radDateTimePicker3.NullDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker3.Size = New System.Drawing.Size(220, 20)
            Me.radDateTimePicker3.TabIndex = 13
            Me.radDateTimePicker3.TabStop = False
            Me.radDateTimePicker3.Text = "Monday, September 10, 2007"
            Me.radDateTimePicker3.Value = New System.DateTime(2007, 9, 10, 10, 33, 34, _
             212)
            ' 
            ' radDateTimePicker1
            ' 
            Me.radDateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDateTimePicker1.Checked = True
            Me.radDateTimePicker1.Location = New System.Drawing.Point(5, 108)
            Me.radDateTimePicker1.MinDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker1.Name = "radDateTimePicker1"
            Me.radDateTimePicker1.NullDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker1.Size = New System.Drawing.Size(220, 20)
            Me.radDateTimePicker1.TabIndex = 19
            Me.radDateTimePicker1.TabStop = False
            Me.radDateTimePicker1.Text = "Friday, August 24, 1900"
            Me.radDateTimePicker1.Value = New System.DateTime(1900, 8, 24, 14, 50, 0, _
             0)
            ' 
            ' radLabel8
            ' 
            Me.radLabel8.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel8.Location = New System.Drawing.Point(5, 87)
            Me.radLabel8.Name = "radLabel8"
            Me.radLabel8.Size = New System.Drawing.Size(86, 18)
            Me.radLabel8.TabIndex = 22
            Me.radLabel8.Text = "Range Min Date"
            Me.radLabel8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            ' 
            ' radDateTimePicker2
            ' 
            Me.radDateTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDateTimePicker2.Checked = True
            Me.radDateTimePicker2.Location = New System.Drawing.Point(5, 48)
            Me.radDateTimePicker2.MinDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker2.Name = "radDateTimePicker2"
            Me.radDateTimePicker2.NullDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker2.Size = New System.Drawing.Size(220, 20)
            Me.radDateTimePicker2.TabIndex = 20
            Me.radDateTimePicker2.TabStop = False
            Me.radDateTimePicker2.Text = "Tuesday, August 24, 2100"
            Me.radDateTimePicker2.Value = New System.DateTime(2100, 8, 24, 14, 50, 0, _
             0)
            ' 
            ' radLabel7
            ' 
            Me.radLabel7.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel7.Location = New System.Drawing.Point(5, 27)
            Me.radLabel7.Name = "radLabel7"
            Me.radLabel7.Size = New System.Drawing.Size(88, 18)
            Me.radLabel7.TabIndex = 21
            Me.radLabel7.Text = "Range Max Date"
            Me.radLabel7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            ' 
            ' radGroupRenderOpts
            ' 
            Me.radGroupRenderOpts.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupRenderOpts.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupRenderOpts.Controls.Add(Me.radCheckBox3)
            Me.radGroupRenderOpts.Controls.Add(Me.radCheckBox1)
            Me.radGroupRenderOpts.Controls.Add(Me.radCheckBox2)
            Me.radGroupRenderOpts.HeaderText = "Rendering Options"
            Me.radGroupRenderOpts.Location = New System.Drawing.Point(10, 6)
            Me.radGroupRenderOpts.Name = "radGroupRenderOpts"
            Me.radGroupRenderOpts.Size = New System.Drawing.Size(230, 104)
            Me.radGroupRenderOpts.TabIndex = 13
            Me.radGroupRenderOpts.Text = "Rendering Options"
            ' 
            ' radGroupDateOpts
            ' 
            Me.radGroupDateOpts.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupDateOpts.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupDateOpts.Controls.Add(Me.radLabel7)
            Me.radGroupDateOpts.Controls.Add(Me.radDateTimePicker3)
            Me.radGroupDateOpts.Controls.Add(Me.radDateTimePicker2)
            Me.radGroupDateOpts.Controls.Add(Me.radLabel8)
            Me.radGroupDateOpts.Controls.Add(Me.radDateTimePicker1)
            Me.radGroupDateOpts.Controls.Add(Me.radLabel1)
            Me.radGroupDateOpts.HeaderText = "Date Options"
            Me.radGroupDateOpts.Location = New System.Drawing.Point(10, 116)
            Me.radGroupDateOpts.Name = "radGroupDateOpts"
            Me.radGroupDateOpts.Size = New System.Drawing.Size(230, 202)
            Me.radGroupDateOpts.TabIndex = 13
            Me.radGroupDateOpts.Text = "Date Options"
            ' 
            ' radSpinNavigationStep
            ' 
            Me.radSpinNavigationStep.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinNavigationStep.Location = New System.Drawing.Point(5, 150)
            Me.radSpinNavigationStep.Name = "radSpinNavigationStep"
            ' 
            ' 
            ' 
            Me.radSpinNavigationStep.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinNavigationStep.Size = New System.Drawing.Size(220, 20)
            Me.radSpinNavigationStep.TabIndex = 24
            Me.radSpinNavigationStep.TabStop = False
            Me.radSpinNavigationStep.Value = New Decimal(New Integer() {3, 0, 0, 0})
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox1.Controls.Add(Me.radDropDownList1)
            Me.radGroupBox1.Controls.Add(Me.radLabel3)
            Me.radGroupBox1.Controls.Add(Me.radLabel2)
            Me.radGroupBox1.Controls.Add(Me.radSpinEditor1)
            Me.radGroupBox1.Controls.Add(Me.radLabel9)
            Me.radGroupBox1.Controls.Add(Me.radSpinNavigationStep)
            Me.radGroupBox1.HeaderText = "Navigation Options"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 334)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(230, 188)
            Me.radGroupBox1.TabIndex = 25
            Me.radGroupBox1.Text = "Navigation Options"
            ' 
            ' radDateTimePicker4
            ' 
            Me.radDateTimePicker4.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDateTimePicker4.Checked = True
            Me.radDateTimePicker4.Location = New System.Drawing.Point(5, 168)
            Me.radDateTimePicker4.MinDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker4.Name = "radDateTimePicker4"
            Me.radDateTimePicker4.NullDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
             0)
            Me.radDateTimePicker4.Size = New System.Drawing.Size(220, 20)
            Me.radDateTimePicker4.TabIndex = 13
            Me.radDateTimePicker4.TabStop = False
            Me.radDateTimePicker4.Text = "Monday, September 10, 2007"
            Me.radDateTimePicker4.Value = New System.DateTime(2007, 9, 10, 10, 33, 34, _
             212)
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(5, 80)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(64, 18)
            Me.radLabel2.TabIndex = 25
            Me.radLabel2.Text = "Month step"
            Me.radLabel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            ' 
            ' radSpinEditor1
            ' 
            Me.radSpinEditor1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditor1.Location = New System.Drawing.Point(5, 101)
            Me.radSpinEditor1.Name = "radSpinEditor1"
            ' 
            ' 
            ' 
            Me.radSpinEditor1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditor1.Size = New System.Drawing.Size(220, 20)
            Me.radSpinEditor1.TabIndex = 26
            Me.radSpinEditor1.TabStop = False
            Me.radSpinEditor1.Value = New Decimal(New Integer() {1, 0, 0, 0})
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel3.Location = New System.Drawing.Point(6, 28)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(130, 18)
            Me.radLabel3.TabIndex = 27
            Me.radLabel3.Text = "Header navigation mode"
            Me.radLabel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            ' 
            ' radDropDownList1
            ' 
            Me.radDropDownList1.AllowShowFocusCues = False
            Me.radDropDownList1.Location = New System.Drawing.Point(6, 49)
            Me.radDropDownList1.Name = "radDropDownList1"
            Me.radDropDownList1.Size = New System.Drawing.Size(219, 20)
            Me.radDropDownList1.TabIndex = 28
            Me.radDropDownList1.Text = "radDropDownList1"
            Me.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            ' 
            ' Form1
            ' 
            Me.Controls.Add(Me.radCalendar1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1237, 814)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radCalendar1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCalendar1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel9, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDateTimePicker3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupRenderOpts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupRenderOpts.ResumeLayout(False)
            Me.radGroupRenderOpts.PerformLayout()
            DirectCast(Me.radGroupDateOpts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupDateOpts.ResumeLayout(False)
            Me.radGroupDateOpts.PerformLayout()
            DirectCast(Me.radSpinNavigationStep, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.radGroupBox1.PerformLayout()
            DirectCast(Me.radDateTimePicker4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radSpinEditor1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radCalendar1 As Telerik.WinControls.UI.RadCalendar
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBox3 As Telerik.WinControls.UI.RadCheckBox
        Private radDateTimePicker1 As Telerik.WinControls.UI.RadDateTimePicker
        Private radDateTimePicker2 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel7 As Telerik.WinControls.UI.RadLabel
        Private radLabel8 As Telerik.WinControls.UI.RadLabel
        Private radDateTimePicker3 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel9 As Telerik.WinControls.UI.RadLabel
        Private radGroupRenderOpts As Telerik.WinControls.UI.RadGroupBox
        Private radGroupDateOpts As Telerik.WinControls.UI.RadGroupBox
        Private radSpinNavigationStep As Telerik.WinControls.UI.RadSpinEditor
        Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Private radDateTimePicker4 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radSpinEditor1 As Telerik.WinControls.UI.RadSpinEditor
        Private radDropDownList1 As Telerik.WinControls.UI.RadDropDownList
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace