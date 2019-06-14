Namespace Telerik.Examples.WinControls.Scheduler.CustomElements
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
            Dim dateTimeInterval2 As New Telerik.WinControls.UI.DateTimeInterval()
            Dim schedulerDailyPrintStyle2 As New Telerik.WinControls.UI.SchedulerDailyPrintStyle()
            Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
            Me.radScheduler1 = New Telerik.WinControls.UI.RadScheduler()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel1.SuspendLayout()
            DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Location = New System.Drawing.Point(1281, 166)
            ' 
            ' themePanel
            ' 
            Me.themePanel.Location = New System.Drawing.Point(1291, 390)
            ' 
            ' radPanel1
            ' 
            Me.radPanel1.Controls.Add(Me.radScheduler1)
            Me.radPanel1.Location = New System.Drawing.Point(0, 0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.Size = New System.Drawing.Size(964, 662)
            Me.radPanel1.TabIndex = 2
            Me.radPanel1.Text = "radPanel1"
            ' 
            ' radScheduler1
            ' 
            dateTimeInterval2.[End] = New System.DateTime(CLng(0))
            dateTimeInterval2.Start = New System.DateTime(CLng(0))
            Me.radScheduler1.AccessibleInterval = dateTimeInterval2
            Me.radScheduler1.Culture = New System.Globalization.CultureInfo("en-US")
            Me.radScheduler1.DataSource = Nothing
            Me.radScheduler1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radScheduler1.Location = New System.Drawing.Point(0, 0)
            Me.radScheduler1.Name = "radScheduler1"
            schedulerDailyPrintStyle2.AppointmentFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            schedulerDailyPrintStyle2.DateEndRange = New System.DateTime(2014, 6, 10, 0, 0, 0, _
                0)
            schedulerDailyPrintStyle2.DateHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold)
            schedulerDailyPrintStyle2.DateStartRange = New System.DateTime(2014, 6, 5, 0, 0, 0, _
                0)
            schedulerDailyPrintStyle2.PageHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 22.0F, System.Drawing.FontStyle.Bold)
            Me.radScheduler1.PrintStyle = schedulerDailyPrintStyle2
            Me.radScheduler1.Size = New System.Drawing.Size(964, 662)
            Me.radScheduler1.TabIndex = 0
            Me.radScheduler1.Text = "radScheduler1"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radPanel1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1476, 1000)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radPanel1, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel1.ResumeLayout(False)
            DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radPanel1 As Telerik.WinControls.UI.RadPanel
        Private radScheduler1 As Telerik.WinControls.UI.RadScheduler

    End Class
End Namespace