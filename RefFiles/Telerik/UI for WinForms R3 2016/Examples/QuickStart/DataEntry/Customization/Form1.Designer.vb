
Namespace Telerik.Examples.WinControls.DataEntry.Customization
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
            Me.radDataEntry1 = New Telerik.WinControls.UI.RadDataEntry()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDataEntry1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radDataEntry1.SuspendLayout()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Location = New System.Drawing.Point(571, 3)
            ' 
            ' themePanel
            ' 
            Me.themePanel.Location = New System.Drawing.Point(571, 169)
            ' 
            ' radDataEntry1
            ' 
            Me.radDataEntry1.AutoSize = False
            Me.radDataEntry1.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.radDataEntry1.ItemDefaultSize = New System.Drawing.Size(200, 22)
            Me.radDataEntry1.Location = New System.Drawing.Point(0, 0)
            Me.radDataEntry1.Name = "radDataEntry1"
            ' 
            ' radDataEntry1.PanelContainer
            ' 
            Me.radDataEntry1.PanelContainer.Size = New System.Drawing.Size(487, 362)
            Me.radDataEntry1.ShowValidationPanel = False
            Me.radDataEntry1.Size = New System.Drawing.Size(489, 364)
            Me.radDataEntry1.TabIndex = 2
            Me.radDataEntry1.Text = "radDataEntry1"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radDataEntry1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1243, 775)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radDataEntry1, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDataEntry1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radDataEntry1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radDataEntry1 As Telerik.WinControls.UI.RadDataEntry
    End Class
End Namespace