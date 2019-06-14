﻿
Namespace Telerik.Examples.WinControls.PivotGrid.ExportToExcelML
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
            Me.buttonExport = New Telerik.WinControls.UI.RadButton()
            Me.radCheckBoxFlatData = New Telerik.WinControls.UI.RadCheckBox()
            Me.radProgressBarExport = New Telerik.WinControls.UI.RadProgressBar()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.buttonExport, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxFlatData, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radProgressBarExport, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radProgressBarExport)
            Me.settingsPanel.Controls.Add(Me.radCheckBoxFlatData)
            Me.settingsPanel.Controls.Add(Me.buttonExport)
            Me.settingsPanel.Dock = System.Windows.Forms.DockStyle.Right
            Me.settingsPanel.Location = New System.Drawing.Point(1107, 0)
            Me.settingsPanel.Size = New System.Drawing.Size(286, 831)
            Me.settingsPanel.Controls.SetChildIndex(Me.buttonExport, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxFlatData, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radProgressBarExport, 0)
            ' 
            ' buttonExport
            ' 
            Me.buttonExport.Location = New System.Drawing.Point(17, 41)
            Me.buttonExport.Name = "buttonExport"
            Me.buttonExport.Size = New System.Drawing.Size(130, 24)
            Me.buttonExport.TabIndex = 2
            Me.buttonExport.Text = "Export"

            ' 
            ' radCheckBoxFlatData
            ' 
            Me.radCheckBoxFlatData.Location = New System.Drawing.Point(17, 89)
            Me.radCheckBoxFlatData.Name = "radCheckBoxFlatData"
            Me.radCheckBoxFlatData.Size = New System.Drawing.Size(100, 18)
            Me.radCheckBoxFlatData.TabIndex = 3
            Me.radCheckBoxFlatData.Text = "Export Flat Data"
            Me.radCheckBoxFlatData.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]

            ' 
            ' radProgressBarExport
            ' 
            Me.radProgressBarExport.Location = New System.Drawing.Point(17, 131)
            Me.radProgressBarExport.Name = "radProgressBarExport"
            Me.radProgressBarExport.Size = New System.Drawing.Size(130, 24)
            Me.radProgressBarExport.TabIndex = 4
            Me.radProgressBarExport.Text = "Export Progress"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1403, 841)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.buttonExport, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxFlatData, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radProgressBarExport, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        WithEvents buttonExport As Telerik.WinControls.UI.RadButton
        WithEvents radCheckBoxFlatData As Telerik.WinControls.UI.RadCheckBox
        WithEvents radProgressBarExport As Telerik.WinControls.UI.RadProgressBar
    End Class
End Namespace

