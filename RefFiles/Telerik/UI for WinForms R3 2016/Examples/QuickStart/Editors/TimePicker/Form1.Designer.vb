
Namespace Telerik.Examples.WinControls.Editors.TimePicker
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
            Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem4 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem5 As New Telerik.WinControls.UI.RadListDataItem()
            Me.radTimePicker1 = New Telerik.WinControls.UI.RadTimePicker()
            Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radLabel7 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel5 = New Telerik.WinControls.UI.RadLabel()
            Me.radTimePicker3 = New Telerik.WinControls.UI.RadTimePicker()
            Me.radTimePicker2 = New Telerik.WinControls.UI.RadTimePicker()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.TablesDropDownList = New Telerik.WinControls.UI.RadDropDownList()
            Me.ClockPossitionDropDownList = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radPageView1 = New Telerik.WinControls.UI.RadPageView()
            Me.radPageViewPage1 = New Telerik.WinControls.UI.RadPageViewPage()
            Me.radButton1 = New Telerik.WinControls.UI.RadButton()
            Me.radLabelTimeZone = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel6 = New Telerik.WinControls.UI.RadLabel()
            Me.radSeparator1 = New Telerik.WinControls.UI.RadSeparator()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelDate = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radClock1 = New Telerik.WinControls.UI.RadClock()
            Me.radPageViewPage2 = New Telerik.WinControls.UI.RadPageViewPage()
            Me.radPageViewPage3 = New Telerik.WinControls.UI.RadPageViewPage()
            DirectCast(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanelDemoHolder.SuspendLayout()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox2.SuspendLayout()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTimePicker3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTimePicker2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.TablesDropDownList, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.ClockPossitionDropDownList, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radPageView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPageView1.SuspendLayout()
            Me.radPageViewPage1.SuspendLayout()
            DirectCast(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabelTimeZone, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radSeparator1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabelDate, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radClock1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radPanelDemoHolder
            ' 
            Me.radPanelDemoHolder.Controls.Add(Me.radPageView1)
            Me.radPanelDemoHolder.Size = New System.Drawing.Size(469, 323)
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBox2)
            Me.settingsPanel.Location = New System.Drawing.Point(493, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(264, 467)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
            ' 
            ' themePanel
            ' 
            Me.themePanel.Location = New System.Drawing.Point(0, 202)
            ' 
            ' radTimePicker1
            ' 
            Me.radTimePicker1.Location = New System.Drawing.Point(192, 116)
            Me.radTimePicker1.Name = "radTimePicker1"
            Me.radTimePicker1.Size = New System.Drawing.Size(106, 20)
            Me.radTimePicker1.TabIndex = 0
            Me.radTimePicker1.TabStop = False
            Me.radTimePicker1.Text = "radTimePicker1"
            Me.radTimePicker1.Value = New System.DateTime(CLng(0))
            ' 
            ' radGroupBox2
            ' 
            Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox2.Controls.Add(Me.radLabel7)
            Me.radGroupBox2.Controls.Add(Me.radLabel5)
            Me.radGroupBox2.Controls.Add(Me.radTimePicker3)
            Me.radGroupBox2.Controls.Add(Me.radTimePicker2)
            Me.radGroupBox2.Controls.Add(Me.radLabel4)
            Me.radGroupBox2.Controls.Add(Me.TablesDropDownList)
            Me.radGroupBox2.Controls.Add(Me.ClockPossitionDropDownList)
            Me.radGroupBox2.Controls.Add(Me.radLabel2)
            Me.radGroupBox2.Controls.Add(Me.radCheckBox1)
            Me.radGroupBox2.HeaderText = "RadTimePicker Settings"
            Me.radGroupBox2.Location = New System.Drawing.Point(10, 47)
            Me.radGroupBox2.Name = "radGroupBox2"
            Me.radGroupBox2.Size = New System.Drawing.Size(244, 241)
            Me.radGroupBox2.TabIndex = 3
            Me.radGroupBox2.Text = "RadTimePicker Settings"
            ' 
            ' radLabel7
            ' 
            Me.radLabel7.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel7.Location = New System.Drawing.Point(5, 191)
            Me.radLabel7.Name = "radLabel7"
            Me.radLabel7.Size = New System.Drawing.Size(56, 18)
            Me.radLabel7.TabIndex = 7
            Me.radLabel7.Text = "Max Time:"
            ' 
            ' radLabel5
            ' 
            Me.radLabel5.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel5.Location = New System.Drawing.Point(5, 145)
            Me.radLabel5.Name = "radLabel5"
            Me.radLabel5.Size = New System.Drawing.Size(54, 18)
            Me.radLabel5.TabIndex = 6
            Me.radLabel5.Text = "Min Time:"
            ' 
            ' radTimePicker3
            ' 
            ' this.radTimePicker3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            Me.radTimePicker3.Location = New System.Drawing.Point(5, 212)
            Me.radTimePicker3.Name = "radTimePicker3"
            Me.radTimePicker3.Size = New System.Drawing.Size(234, 24)
            Me.radTimePicker3.TabIndex = 5
            Me.radTimePicker3.TabStop = False
            Me.radTimePicker3.Text = "radTimePicker3"
            Me.radTimePicker3.Value = New System.DateTime(2014, 6, 10, 16, 15, 44, _
             0)
            ' 
            ' radTimePicker2
            ' 
            'this.radTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            Me.radTimePicker2.Location = New System.Drawing.Point(5, 166)
            Me.radTimePicker2.Name = "radTimePicker2"
            Me.radTimePicker2.Size = New System.Drawing.Size(234, 24)
            Me.radTimePicker2.TabIndex = 4
            Me.radTimePicker2.TabStop = False
            Me.radTimePicker2.Text = "radTimePicker2"
            Me.radTimePicker2.Value = New System.DateTime(2014, 6, 10, 16, 15, 44, _
             0)
            ' 
            ' radLabel4
            ' 
            Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel4.Location = New System.Drawing.Point(5, 101)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(86, 18)
            Me.radLabel4.TabIndex = 3
            Me.radLabel4.Text = "Time table style:"
            ' 
            ' TablesDropDownList
            ' 
            Me.TablesDropDownList.AllowShowFocusCues = False
            Me.TablesDropDownList.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.TablesDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            radListDataItem1.Text = "One table"
            radListDataItem1.TextWrap = True
            radListDataItem2.Text = "Two tables"
            radListDataItem2.TextWrap = True
            Me.TablesDropDownList.Items.Add(radListDataItem1)
            Me.TablesDropDownList.Items.Add(radListDataItem2)
            Me.TablesDropDownList.Location = New System.Drawing.Point(5, 121)
            Me.TablesDropDownList.Name = "TablesDropDownList"
            Me.TablesDropDownList.Size = New System.Drawing.Size(234, 20)
            Me.TablesDropDownList.TabIndex = 2
            ' 
            ' ClockPossitionDropDownList
            ' 
            Me.ClockPossitionDropDownList.AllowShowFocusCues = False
            Me.ClockPossitionDropDownList.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.ClockPossitionDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            radListDataItem3.Text = "Clock Before Tables"
            radListDataItem3.TextWrap = True
            radListDataItem4.Text = "Clock Above Tables"
            radListDataItem4.TextWrap = True
            radListDataItem5.Text = "Hide Clock"
            radListDataItem5.TextWrap = True
            Me.ClockPossitionDropDownList.Items.Add(radListDataItem3)
            Me.ClockPossitionDropDownList.Items.Add(radListDataItem4)
            Me.ClockPossitionDropDownList.Items.Add(radListDataItem5)
            Me.ClockPossitionDropDownList.Location = New System.Drawing.Point(5, 75)
            Me.ClockPossitionDropDownList.Name = "ClockPossitionDropDownList"
            Me.ClockPossitionDropDownList.Size = New System.Drawing.Size(234, 20)
            Me.ClockPossitionDropDownList.TabIndex = 1
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(5, 56)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(78, 18)
            Me.radLabel2.TabIndex = 2
            Me.radLabel2.Text = "Clock position:"
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBox1.Location = New System.Drawing.Point(5, 21)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(68, 18)
            Me.radCheckBox1.TabIndex = 0
            Me.radCheckBox1.Text = "ReadOnly"
            ' 
            ' radPageView1
            ' 
            Me.radPageView1.Controls.Add(Me.radPageViewPage1)
            Me.radPageView1.Controls.Add(Me.radPageViewPage2)
            Me.radPageView1.Controls.Add(Me.radPageViewPage3)
            Me.radPageView1.Location = New System.Drawing.Point(0, 0)
            Me.radPageView1.Name = "radPageView1"
            Me.radPageView1.SelectedPage = Me.radPageViewPage1
            Me.radPageView1.Size = New System.Drawing.Size(454, 306)
            Me.radPageView1.TabIndex = 1
            DirectCast(Me.radPageView1.GetChildAt(0), Telerik.WinControls.UI.RadPageViewStripElement).StripButtons = Telerik.WinControls.UI.StripViewButtons.None
            ' 
            ' radPageViewPage1
            ' 
            Me.radPageViewPage1.Controls.Add(Me.radButton1)
            Me.radPageViewPage1.Controls.Add(Me.radTimePicker1)
            Me.radPageViewPage1.Controls.Add(Me.radLabelTimeZone)
            Me.radPageViewPage1.Controls.Add(Me.radLabel6)
            Me.radPageViewPage1.Controls.Add(Me.radSeparator1)
            Me.radPageViewPage1.Controls.Add(Me.radLabel3)
            Me.radPageViewPage1.Controls.Add(Me.radLabelDate)
            Me.radPageViewPage1.Controls.Add(Me.radLabel1)
            Me.radPageViewPage1.Controls.Add(Me.radClock1)
            Me.radPageViewPage1.ItemSize = New System.Drawing.SizeF(89.0F, 28.0F)
            Me.radPageViewPage1.Location = New System.Drawing.Point(10, 37)
            Me.radPageViewPage1.Name = "radPageViewPage1"
            Me.radPageViewPage1.Size = New System.Drawing.Size(433, 258)
            Me.radPageViewPage1.Text = "Date and Time"
            ' 
            ' radButton1
            ' 
            Me.radButton1.Location = New System.Drawing.Point(221, 228)
            Me.radButton1.Name = "radButton1"
            Me.radButton1.Size = New System.Drawing.Size(184, 24)
            Me.radButton1.TabIndex = 9
            Me.radButton1.Text = "Change time zone..."
            ' 
            ' radLabelTimeZone
            ' 
            Me.radLabelTimeZone.Location = New System.Drawing.Point(4, 205)
            Me.radLabelTimeZone.Name = "radLabelTimeZone"
            Me.radLabelTimeZone.Size = New System.Drawing.Size(27, 18)
            Me.radLabelTimeZone.TabIndex = 8
            Me.radLabelTimeZone.Text = "UTC"
            ' 
            ' radLabel6
            ' 
            Me.radLabel6.Location = New System.Drawing.Point(2, 180)
            Me.radLabel6.Name = "radLabel6"
            Me.radLabel6.Size = New System.Drawing.Size(64, 18)
            Me.radLabel6.TabIndex = 7
            Me.radLabel6.Text = "Time zone  "
            ' 
            ' radSeparator1
            ' 
            Me.radSeparator1.Location = New System.Drawing.Point(3, 185)
            Me.radSeparator1.Name = "radSeparator1"
            Me.radSeparator1.Size = New System.Drawing.Size(427, 10)
            Me.radSeparator1.TabIndex = 7
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Location = New System.Drawing.Point(189, 94)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(33, 18)
            Me.radLabel3.TabIndex = 3
            Me.radLabel3.Text = "Time:"
            ' 
            ' radLabelDate
            ' 
            Me.radLabelDate.Location = New System.Drawing.Point(189, 54)
            Me.radLabelDate.Name = "radLabelDate"
            Me.radLabelDate.Size = New System.Drawing.Size(104, 18)
            Me.radLabelDate.TabIndex = 2
            Me.radLabelDate.Text = "12 February 2012 y."
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Location = New System.Drawing.Point(189, 34)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(32, 18)
            Me.radLabel1.TabIndex = 1
            Me.radLabel1.Text = "Date:"
            ' 
            ' radClock1
            ' 
            Me.radClock1.BackColor = System.Drawing.Color.Transparent
            Me.radClock1.Location = New System.Drawing.Point(21, 16)
            Me.radClock1.Name = "radClock1"
            Me.radClock1.Size = New System.Drawing.Size(134, 135)
            Me.radClock1.TabIndex = 0
            Me.radClock1.Text = "radClock1"
            ' 
            ' radPageViewPage2
            ' 
            Me.radPageViewPage2.ItemSize = New System.Drawing.SizeF(103.0F, 28.0F)
            Me.radPageViewPage2.Location = New System.Drawing.Point(10, 37)
            Me.radPageViewPage2.Name = "radPageViewPage2"
            Me.radPageViewPage2.Size = New System.Drawing.Size(433, 258)
            Me.radPageViewPage2.Text = "Additional Clocks"
            ' 
            ' radPageViewPage3
            ' 
            Me.radPageViewPage3.ItemSize = New System.Drawing.SizeF(83.0F, 28.0F)
            Me.radPageViewPage3.Location = New System.Drawing.Point(10, 37)
            Me.radPageViewPage3.Name = "radPageViewPage3"
            Me.radPageViewPage3.Size = New System.Drawing.Size(433, 258)
            Me.radPageViewPage3.Text = "Internet Time"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1079, 520)
            DirectCast(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanelDemoHolder.ResumeLayout(False)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox2.ResumeLayout(False)
            Me.radGroupBox2.PerformLayout()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTimePicker3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTimePicker2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.TablesDropDownList, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.ClockPossitionDropDownList, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radPageView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPageView1.ResumeLayout(False)
            Me.radPageViewPage1.ResumeLayout(False)
            Me.radPageViewPage1.PerformLayout()
            DirectCast(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabelTimeZone, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radSeparator1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabelDate, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radClock1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radTimePicker1 As Telerik.WinControls.UI.RadTimePicker
        Private radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
        Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
        Private ClockPossitionDropDownList As Telerik.WinControls.UI.RadDropDownList
        Private TablesDropDownList As Telerik.WinControls.UI.RadDropDownList
        Private radPageView1 As Telerik.WinControls.UI.RadPageView
        Private radPageViewPage1 As Telerik.WinControls.UI.RadPageViewPage
        Private radButton1 As Telerik.WinControls.UI.RadButton
        Private radLabelTimeZone As Telerik.WinControls.UI.RadLabel
        Private radLabel6 As Telerik.WinControls.UI.RadLabel
        Private radSeparator1 As Telerik.WinControls.UI.RadSeparator
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private radLabelDate As Telerik.WinControls.UI.RadLabel
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radClock1 As Telerik.WinControls.UI.RadClock
        Private radPageViewPage2 As Telerik.WinControls.UI.RadPageViewPage
        Private radPageViewPage3 As Telerik.WinControls.UI.RadPageViewPage
        Private radLabel4 As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radLabel7 As Telerik.WinControls.UI.RadLabel
        Private radLabel5 As Telerik.WinControls.UI.RadLabel
        Private radTimePicker3 As Telerik.WinControls.UI.RadTimePicker
        Private radTimePicker2 As Telerik.WinControls.UI.RadTimePicker
    End Class
End Namespace
