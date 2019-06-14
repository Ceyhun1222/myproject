
Namespace Telerik.Examples.WinControls.Map.Layers
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
            Me.radMap1 = New Telerik.WinControls.UI.RadMap()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radMap1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radMap1
            ' 
            Me.radMap1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radMap1.Location = New System.Drawing.Point(0, 0)
            Me.radMap1.Name = "radMap1"
            Me.radMap1.ShowSearchBar = False
            Me.radMap1.ShowMiniMap = False
            Me.radMap1.Size = New System.Drawing.Size(1871, 1028)
            Me.radMap1.TabIndex = 0
            Me.radMap1.Text = "radMap1"

            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radMap1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1881, 1038)
            Me.Controls.SetChildIndex(Me.radMap1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radMap1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private WithEvents radMap1 As Telerik.WinControls.UI.RadMap
    End Class
End Namespace

