Namespace Telerik.Examples.WinControls.DropDownListAndListControl.CheckedDropDownList
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



        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim RadCheckedListDataItem1 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem2 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem3 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem4 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem5 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem6 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem7 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem8 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem9 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem10 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem11 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem12 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Dim RadCheckedListDataItem13 As Telerik.WinControls.UI.RadCheckedListDataItem = New Telerik.WinControls.UI.RadCheckedListDataItem()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckedDropDownList1 = New Telerik.WinControls.UI.RadCheckedDropDownList()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckedDropDownList2 = New Telerik.WinControls.UI.RadCheckedDropDownList()
            Me.radTimePicker1 = New Telerik.WinControls.UI.RadTimePicker()
            Me.radTextBox1 = New Telerik.WinControls.UI.RadTextBox()
            Me.radButton1 = New Telerik.WinControls.UI.RadButton()
            Me.radToggleButton1 = New Telerik.WinControls.UI.RadToggleButton()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckedDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckedDropDownList2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radToggleButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radToggleButton1)
            Me.settingsPanel.Padding = New System.Windows.Forms.Padding(3)
            Me.settingsPanel.Controls.SetChildIndex(Me.radToggleButton1, 0)
            '
            'themePanel
            '
            Me.themePanel.Padding = New System.Windows.Forms.Padding(3)
            '
            'radLabel1
            '
            Me.radLabel1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
            Me.radLabel1.Location = New System.Drawing.Point(0, 27)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(62, 21)
            Me.radLabel1.TabIndex = 2
            Me.radLabel1.Text = "New task"
            '
            'radLabel2
            '
            Me.radLabel2.Location = New System.Drawing.Point(0, 102)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(51, 18)
            Me.radLabel2.TabIndex = 4
            Me.radLabel2.Text = "Category"
            '
            'radCheckedDropDownList1
            '
            RadCheckedListDataItem1.Checked = True
            RadCheckedListDataItem1.Text = "work"
            RadCheckedListDataItem2.Checked = True
            RadCheckedListDataItem2.Text = "to do"
            RadCheckedListDataItem3.Checked = True
            RadCheckedListDataItem3.Text = "emails"
            RadCheckedListDataItem4.Text = "forwarded"
            RadCheckedListDataItem5.Text = "replied"
            RadCheckedListDataItem6.Text = "duplicate"
            Me.radCheckedDropDownList1.Items.Add(RadCheckedListDataItem1)
            Me.radCheckedDropDownList1.Items.Add(RadCheckedListDataItem2)
            Me.radCheckedDropDownList1.Items.Add(RadCheckedListDataItem3)
            Me.radCheckedDropDownList1.Items.Add(RadCheckedListDataItem4)
            Me.radCheckedDropDownList1.Items.Add(RadCheckedListDataItem5)
            Me.radCheckedDropDownList1.Items.Add(RadCheckedListDataItem6)
            Me.radCheckedDropDownList1.Location = New System.Drawing.Point(0, 121)
            Me.radCheckedDropDownList1.Name = "radCheckedDropDownList1"
            Me.radCheckedDropDownList1.Size = New System.Drawing.Size(300, 20)
            Me.radCheckedDropDownList1.TabIndex = 5
            Me.radCheckedDropDownList1.Text = "work;to do;emails;"
            '
            'radLabel3
            '
            Me.radLabel3.Location = New System.Drawing.Point(0, 154)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(54, 18)
            Me.radLabel3.TabIndex = 6
            Me.radLabel3.Text = "Reminder"
            '
            'radLabel4
            '
            Me.radLabel4.Location = New System.Drawing.Point(0, 210)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(98, 18)
            Me.radLabel4.TabIndex = 7
            Me.radLabel4.Text = "Appointment time"
            '
            'radCheckedDropDownList2
            '
            RadCheckedListDataItem7.Checked = True
            RadCheckedListDataItem7.Text = "Monday"
            RadCheckedListDataItem8.Checked = True
            RadCheckedListDataItem8.Text = "Tuesday"
            RadCheckedListDataItem9.Text = "Wednesday"
            RadCheckedListDataItem10.Text = "Thursday"
            RadCheckedListDataItem11.Text = "Friday"
            RadCheckedListDataItem12.Text = "Saturday"
            RadCheckedListDataItem13.Text = "Sunday"
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem7)
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem8)
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem9)
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem10)
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem11)
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem12)
            Me.radCheckedDropDownList2.Items.Add(RadCheckedListDataItem13)
            Me.radCheckedDropDownList2.Location = New System.Drawing.Point(0, 173)
            Me.radCheckedDropDownList2.Name = "radCheckedDropDownList2"
            Me.radCheckedDropDownList2.Size = New System.Drawing.Size(300, 20)
            Me.radCheckedDropDownList2.TabIndex = 8
            Me.radCheckedDropDownList2.Text = "Monday;Tuesday;"
            '
            'radTimePicker1
            '
            Me.radTimePicker1.Location = New System.Drawing.Point(0, 230)
            Me.radTimePicker1.MaxValue = New Date(9999, 12, 31, 23, 59, 59, 0)
            Me.radTimePicker1.MinValue = New Date(CType(0, Long))
            Me.radTimePicker1.Name = "radTimePicker1"
            Me.radTimePicker1.Size = New System.Drawing.Size(298, 20)
            Me.radTimePicker1.TabIndex = 9
            Me.radTimePicker1.TabStop = False
            Me.radTimePicker1.Text = "radTimePicker1"
            Me.radTimePicker1.Value = New Date(2014, 9, 3, 10, 43, 22, 508)
            '
            'radTextBox1
            '
            Me.radTextBox1.Location = New System.Drawing.Point(0, 70)
            Me.radTextBox1.Name = "radTextBox1"
            Me.radTextBox1.NullText = "Task name"
            Me.radTextBox1.Size = New System.Drawing.Size(300, 20)
            Me.radTextBox1.TabIndex = 10
            Me.radTextBox1.Text = "Task name"
            '
            'radButton1
            '
            Me.radButton1.Location = New System.Drawing.Point(0, 279)
            Me.radButton1.Name = "radButton1"
            Me.radButton1.Size = New System.Drawing.Size(110, 24)
            Me.radButton1.TabIndex = 11
            Me.radButton1.Text = "Create"
            '
            'radToggleButton1
            '
            Me.radToggleButton1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radToggleButton1.Location = New System.Drawing.Point(10, 62)
            Me.radToggleButton1.Name = "radToggleButton1"
            Me.radToggleButton1.Size = New System.Drawing.Size(210, 24)
            Me.radToggleButton1.TabIndex = 2
            Me.radToggleButton1.Text = "Show CheckAll Item"
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radButton1)
            Me.Controls.Add(Me.radTextBox1)
            Me.Controls.Add(Me.radTimePicker1)
            Me.Controls.Add(Me.radCheckedDropDownList2)
            Me.Controls.Add(Me.radLabel4)
            Me.Controls.Add(Me.radLabel3)
            Me.Controls.Add(Me.radCheckedDropDownList1)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.radLabel1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1220, 695)
            Me.Controls.SetChildIndex(Me.radLabel1, 0)
            Me.Controls.SetChildIndex(Me.radLabel2, 0)
            Me.Controls.SetChildIndex(Me.radCheckedDropDownList1, 0)
            Me.Controls.SetChildIndex(Me.radLabel3, 0)
            Me.Controls.SetChildIndex(Me.radLabel4, 0)
            Me.Controls.SetChildIndex(Me.radCheckedDropDownList2, 0)
            Me.Controls.SetChildIndex(Me.radTimePicker1, 0)
            Me.Controls.SetChildIndex(Me.radTextBox1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radButton1, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckedDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckedDropDownList2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radToggleButton1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub



        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radCheckedDropDownList1 As Telerik.WinControls.UI.RadCheckedDropDownList
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private radLabel4 As Telerik.WinControls.UI.RadLabel
        Private radCheckedDropDownList2 As Telerik.WinControls.UI.RadCheckedDropDownList
        Private radTimePicker1 As Telerik.WinControls.UI.RadTimePicker
        Private radTextBox1 As Telerik.WinControls.UI.RadTextBox
        Friend WithEvents radButton1 As Telerik.WinControls.UI.RadButton
        Private WithEvents radToggleButton1 As Telerik.WinControls.UI.RadToggleButton
    End Class
End Namespace