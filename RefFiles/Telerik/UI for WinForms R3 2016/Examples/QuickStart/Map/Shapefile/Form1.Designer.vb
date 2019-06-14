Namespace Telerik.Examples.WinControls.Map.Shapefile
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
            Me.radGroupBoxOptions = New Telerik.WinControls.UI.RadGroupBox()
            Me.radButtonBuyNow = New Telerik.WinControls.UI.RadButton()
            Me.radButtonReserve = New Telerik.WinControls.UI.RadButton()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radMap1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBoxOptions, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBoxOptions.SuspendLayout()
            DirectCast(Me.radButtonBuyNow, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radButtonReserve, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBoxOptions)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxOptions, 0)
            ' 
            ' radMap1
            ' 
            Me.radMap1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radMap1.Location = New System.Drawing.Point(0, 0)
            Me.radMap1.Name = "radMap1"
            Me.radMap1.ShowMiniMap = False
            Me.radMap1.ShowNavigationBar = False
            Me.radMap1.ShowScaleIndicator = False
            Me.radMap1.ShowSearchBar = False
            Me.radMap1.Size = New System.Drawing.Size(1555, 1028)
            Me.radMap1.TabIndex = 0
            Me.radMap1.Text = "radMap1"
            ' 
            ' radGroupBoxOptions
            ' 
            Me.radGroupBoxOptions.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBoxOptions.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBoxOptions.Controls.Add(Me.radButtonReserve)
            Me.radGroupBoxOptions.Controls.Add(Me.radButtonBuyNow)
            Me.radGroupBoxOptions.HeaderText = "Purchaise options"
            Me.radGroupBoxOptions.Location = New System.Drawing.Point(10, 33)
            Me.radGroupBoxOptions.Name = "radGroupBoxOptions"
            Me.radGroupBoxOptions.Size = New System.Drawing.Size(210, 85)
            Me.radGroupBoxOptions.TabIndex = 1
            Me.radGroupBoxOptions.Text = "Purchaise options"
            ' 
            ' radButtonReserve
            ' 
            Me.radButtonReserve.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radButtonReserve.Location = New System.Drawing.Point(5, 57)
            Me.radButtonReserve.Name = "radButtonReserve"
            Me.radButtonReserve.Size = New System.Drawing.Size(200, 24)
            Me.radButtonReserve.TabIndex = 0
            Me.radButtonReserve.Text = "Reserve"
            ' 
            ' radButtonBuyNow
            ' 
            Me.radButtonBuyNow.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radButtonBuyNow.Location = New System.Drawing.Point(5, 29)
            Me.radButtonBuyNow.Name = "radButtonBuyNow"
            Me.radButtonBuyNow.Size = New System.Drawing.Size(200, 24)
            Me.radButtonBuyNow.TabIndex = 0
            Me.radButtonBuyNow.Text = "Buy Now"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radMap1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1565, 1038)
            Me.Controls.SetChildIndex(Me.radMap1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radMap1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBoxOptions, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBoxOptions.ResumeLayout(False)
            DirectCast(Me.radButtonBuyNow, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radButtonReserve, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private WithEvents radMap1 As Telerik.WinControls.UI.RadMap
        Private WithEvents radGroupBoxOptions As Telerik.WinControls.UI.RadGroupBox
        Private WithEvents radButtonReserve As Telerik.WinControls.UI.RadButton
        Private WithEvents radButtonBuyNow As Telerik.WinControls.UI.RadButton
    End Class
End Namespace