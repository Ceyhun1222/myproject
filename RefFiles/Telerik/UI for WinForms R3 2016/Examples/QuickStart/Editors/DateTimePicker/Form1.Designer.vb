
Namespace Telerik.Examples.WinControls.Editors.DateTimePicker
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
            Me.radDateTimePicker1 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLblLongDateFormat = New Telerik.WinControls.UI.RadLabel()
            Me.radLblShortDateFormat = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker2 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLblTimeFormat = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker3 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radBtnClearAll = New Telerik.WinControls.UI.RadButton()
            Me.radGroupExampleSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radRadio24Hours = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadio12Hours = New Telerik.WinControls.UI.RadRadioButton()
            Me.radDateTimePicker4 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker5 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker6 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radDateTimePicker7 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanelDemoHolder.SuspendLayout()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblLongDateFormat, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblShortDateFormat, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblTimeFormat, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radBtnClearAll, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupExampleSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupExampleSettings.SuspendLayout()
            CType(Me.radRadio24Hours, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radRadio12Hours, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'radPanelDemoHolder
            '
            Me.radPanelDemoHolder.Controls.Add(Me.radLabel2)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker5)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabel3)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker6)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabel4)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker7)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker4)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabel1)
            Me.radPanelDemoHolder.Controls.Add(Me.radLblLongDateFormat)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker2)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker1)
            Me.radPanelDemoHolder.Controls.Add(Me.radLblShortDateFormat)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker3)
            Me.radPanelDemoHolder.Controls.Add(Me.radLblTimeFormat)
            Me.radPanelDemoHolder.ForeColor = System.Drawing.Color.Black
            Me.radPanelDemoHolder.Size = New System.Drawing.Size(450, 270)
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radGroupExampleSettings)
            Me.settingsPanel.Location = New System.Drawing.Point(1023, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 735)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupExampleSettings, 0)
            '
            'radDateTimePicker1
            '
            Me.radDateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker1.Checked = True
            Me.radDateTimePicker1.Location = New System.Drawing.Point(170, 7)
            Me.radDateTimePicker1.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker1.Name = "radDateTimePicker1"
            Me.radDateTimePicker1.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker1.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker1.TabIndex = 0
            Me.radDateTimePicker1.TabStop = False
            Me.radDateTimePicker1.Text = "Thursday, August 23, 2007"
            Me.radDateTimePicker1.Value = New Date(2007, 8, 23, 15, 29, 8, 309)
            '
            'radLblLongDateFormat
            '
            Me.radLblLongDateFormat.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLblLongDateFormat.Location = New System.Drawing.Point(63, 8)
            Me.radLblLongDateFormat.Name = "radLblLongDateFormat"
            Me.radLblLongDateFormat.Size = New System.Drawing.Size(96, 18)
            Me.radLblLongDateFormat.TabIndex = 1
            Me.radLblLongDateFormat.Text = "Long date format:"
            '
            'radLblShortDateFormat
            '
            Me.radLblShortDateFormat.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLblShortDateFormat.Location = New System.Drawing.Point(62, 43)
            Me.radLblShortDateFormat.Name = "radLblShortDateFormat"
            Me.radLblShortDateFormat.Size = New System.Drawing.Size(97, 18)
            Me.radLblShortDateFormat.TabIndex = 3
            Me.radLblShortDateFormat.Text = "Short date format:"
            '
            'radDateTimePicker2
            '
            Me.radDateTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker2.Checked = True
            Me.radDateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.radDateTimePicker2.Location = New System.Drawing.Point(170, 42)
            Me.radDateTimePicker2.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker2.Name = "radDateTimePicker2"
            Me.radDateTimePicker2.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker2.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker2.TabIndex = 2
            Me.radDateTimePicker2.TabStop = False
            Me.radDateTimePicker2.Text = "1/1/1980"
            Me.radDateTimePicker2.Value = New Date(1980, 1, 1, 0, 0, 0, 0)
            '
            'radLblTimeFormat
            '
            Me.radLblTimeFormat.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLblTimeFormat.Location = New System.Drawing.Point(89, 78)
            Me.radLblTimeFormat.Name = "radLblTimeFormat"
            Me.radLblTimeFormat.Size = New System.Drawing.Size(70, 18)
            Me.radLblTimeFormat.TabIndex = 5
            Me.radLblTimeFormat.Text = "Time format:"
            '
            'radDateTimePicker3
            '
            Me.radDateTimePicker3.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker3.Checked = True
            Me.radDateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Time
            Me.radDateTimePicker3.Location = New System.Drawing.Point(170, 77)
            Me.radDateTimePicker3.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker3.Name = "radDateTimePicker3"
            Me.radDateTimePicker3.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker3.ShowUpDown = True
            Me.radDateTimePicker3.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker3.TabIndex = 4
            Me.radDateTimePicker3.TabStop = False
            Me.radDateTimePicker3.Text = "12:00:00 AM"
            Me.radDateTimePicker3.Value = New Date(1980, 1, 1, 0, 0, 0, 0)
            '
            'radBtnClearAll
            '
            Me.radBtnClearAll.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radBtnClearAll.Location = New System.Drawing.Point(5, 84)
            Me.radBtnClearAll.Name = "radBtnClearAll"
            Me.radBtnClearAll.Size = New System.Drawing.Size(170, 23)
            Me.radBtnClearAll.TabIndex = 6
            Me.radBtnClearAll.Text = "Clear All"
            '
            'radGroupExampleSettings
            '
            Me.radGroupExampleSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupExampleSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupExampleSettings.Controls.Add(Me.radRadio24Hours)
            Me.radGroupExampleSettings.Controls.Add(Me.radRadio12Hours)
            Me.radGroupExampleSettings.Controls.Add(Me.radBtnClearAll)
            Me.radGroupExampleSettings.FooterText = ""
            Me.radGroupExampleSettings.HeaderMargin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            Me.radGroupExampleSettings.HeaderText = " Settings "
            Me.radGroupExampleSettings.Location = New System.Drawing.Point(10, 6)
            Me.radGroupExampleSettings.Name = "radGroupExampleSettings"
            Me.radGroupExampleSettings.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            Me.radGroupExampleSettings.Size = New System.Drawing.Size(180, 122)
            Me.radGroupExampleSettings.TabIndex = 7
            Me.radGroupExampleSettings.Text = " Settings "
            '
            'radRadio24Hours
            '
            Me.radRadio24Hours.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadio24Hours.Location = New System.Drawing.Point(5, 53)
            Me.radRadio24Hours.Name = "radRadio24Hours"
            Me.radRadio24Hours.Size = New System.Drawing.Size(65, 18)
            Me.radRadio24Hours.TabIndex = 7
            Me.radRadio24Hours.Text = "24 Hours"
            '
            'radRadio12Hours
            '
            Me.radRadio12Hours.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radRadio12Hours.Location = New System.Drawing.Point(5, 31)
            Me.radRadio12Hours.Name = "radRadio12Hours"
            Me.radRadio12Hours.Size = New System.Drawing.Size(65, 18)
            Me.radRadio12Hours.TabIndex = 7
            Me.radRadio12Hours.Text = "12 Hours"
            '
            'radDateTimePicker4
            '
            Me.radDateTimePicker4.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker4.Checked = True
            Me.radDateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.radDateTimePicker4.Location = New System.Drawing.Point(170, 112)
            Me.radDateTimePicker4.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker4.Name = "radDateTimePicker4"
            Me.radDateTimePicker4.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker4.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker4.TabIndex = 6
            Me.radDateTimePicker4.TabStop = False
            Me.radDateTimePicker4.Text = "1/1/1980"
            Me.radDateTimePicker4.Value = New Date(1980, 1, 1, 0, 0, 0, 0)
            '
            'radLabel1
            '
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLabel1.Location = New System.Drawing.Point(5, 113)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(154, 18)
            Me.radLabel1.TabIndex = 7
            Me.radLabel1.Text = "Time format with Time Picker:"
            '
            'radDateTimePicker5
            '
            Me.radDateTimePicker5.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker5.Checked = True
            Me.radDateTimePicker5.Culture = New System.Globalization.CultureInfo("ar-SA")
            Me.radDateTimePicker5.Location = New System.Drawing.Point(170, 147)
            Me.radDateTimePicker5.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker5.Name = "radDateTimePicker5"
            Me.radDateTimePicker5.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker5.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker5.TabIndex = 0
            Me.radDateTimePicker5.TabStop = False
            Me.radDateTimePicker5.Text = "10/شعبان/1428"
            Me.radDateTimePicker5.Value = New Date(2007, 8, 23, 15, 29, 8, 309)
            '
            'radLabel2
            '
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLabel2.Location = New System.Drawing.Point(68, 183)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(91, 18)
            Me.radLabel2.TabIndex = 1
            Me.radLabel2.Text = "Hijri date format:"
            '
            'radDateTimePicker6
            '
            Me.radDateTimePicker6.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker6.Checked = True
            Me.radDateTimePicker6.Location = New System.Drawing.Point(170, 217)
            Me.radDateTimePicker6.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker6.Name = "radDateTimePicker6"
            Me.radDateTimePicker6.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker6.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker6.TabIndex = 0
            Me.radDateTimePicker6.TabStop = False
            Me.radDateTimePicker6.Text = "Thursday, August 23, 2007"
            Me.radDateTimePicker6.Value = New Date(2007, 8, 23, 15, 29, 8, 309)
            '
            'radLabel3
            '
            Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLabel3.Location = New System.Drawing.Point(36, 148)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(123, 18)
            Me.radLabel3.TabIndex = 1
            Me.radLabel3.Text = "UmAlQura date format:"
            '
            'radDateTimePicker7
            '
            Me.radDateTimePicker7.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radDateTimePicker7.Checked = True
            Me.radDateTimePicker7.Culture = New System.Globalization.CultureInfo("ps-AF")
            Me.radDateTimePicker7.Location = New System.Drawing.Point(170, 182)
            Me.radDateTimePicker7.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker7.Name = "radDateTimePicker7"
            Me.radDateTimePicker7.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
            Me.radDateTimePicker7.Size = New System.Drawing.Size(191, 20)
            Me.radDateTimePicker7.TabIndex = 0
            Me.radDateTimePicker7.TabStop = False
            Me.radDateTimePicker7.Text = "1 وږی 1386"
            Me.radDateTimePicker7.Value = New Date(2007, 8, 23, 15, 29, 8, 309)
            '
            'radLabel4
            '
            Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radLabel4.Location = New System.Drawing.Point(53, 218)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(106, 18)
            Me.radLabel4.TabIndex = 1
            Me.radLabel4.Text = "Persian date format:"
            '
            'Form1
            '
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1039, 917)
            CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanelDemoHolder.ResumeLayout(False)
            Me.radPanelDemoHolder.PerformLayout()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblLongDateFormat, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblShortDateFormat, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblTimeFormat, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radBtnClearAll, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGroupExampleSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupExampleSettings.ResumeLayout(False)
            Me.radGroupExampleSettings.PerformLayout()
            CType(Me.radRadio24Hours, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radRadio12Hours, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radDateTimePicker1 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLblLongDateFormat As Telerik.WinControls.UI.RadLabel
        Private radLblShortDateFormat As Telerik.WinControls.UI.RadLabel
        Private radDateTimePicker2 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLblTimeFormat As Telerik.WinControls.UI.RadLabel
        Private radDateTimePicker3 As Telerik.WinControls.UI.RadDateTimePicker
        Private radBtnClearAll As Telerik.WinControls.UI.RadButton
        Private radGroupExampleSettings As Telerik.WinControls.UI.RadGroupBox
        Private radRadio24Hours As Telerik.WinControls.UI.RadRadioButton
        Private radRadio12Hours As Telerik.WinControls.UI.RadRadioButton
        Private radDateTimePicker4 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radDateTimePicker5 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radDateTimePicker6 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private radDateTimePicker7 As Telerik.WinControls.UI.RadDateTimePicker
        Private radLabel4 As Telerik.WinControls.UI.RadLabel

    End Class
End Namespace


