
Imports Telerik.WinControls
Namespace Telerik.Examples.WinControls.PanelsLabels.GroupBox
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
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radRadioButton2 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton1 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radRadioButton3 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton5 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton6 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton4 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radGroupBox4 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radRadioButton11 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton10 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton9 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton8 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton7 = New Telerik.WinControls.UI.RadRadioButton()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox2.SuspendLayout()
            DirectCast(Me.radRadioButton2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox3.SuspendLayout()
            DirectCast(Me.radRadioButton3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton5, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton6, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox4.SuspendLayout()
            DirectCast(Me.radRadioButton11, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton10, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton9, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton8, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton7, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Location = New System.Drawing.Point(1052, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 830)
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Controls.Add(Me.radGroupBox2)
            Me.radGroupBox1.Controls.Add(Me.radGroupBox3)
            Me.radGroupBox1.Controls.Add(Me.radGroupBox4)
            Me.radGroupBox1.HeaderImage = My.Resources.info2
            Me.radGroupBox1.HeaderText = "Telerik Groupbox "
            Me.radGroupBox1.Location = New System.Drawing.Point(0, 0)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            ' 
            ' 
            ' 
            Me.radGroupBox1.RootElement.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            Me.radGroupBox1.Size = New System.Drawing.Size(439, 271)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "Telerik Groupbox "
            ' 
            ' radGroupBox2
            ' 
            Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox2.Controls.Add(Me.radRadioButton2)
            Me.radGroupBox2.Controls.Add(Me.radRadioButton1)
            Me.radGroupBox2.HeaderText = "Groupbox style"
            Me.radGroupBox2.Location = New System.Drawing.Point(52, 187)
            Me.radGroupBox2.Name = "radGroupBox2"
            Me.radGroupBox2.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            ' 
            ' 
            ' 
            Me.radGroupBox2.RootElement.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            Me.radGroupBox2.Size = New System.Drawing.Size(313, 47)
            Me.radGroupBox2.TabIndex = 0
            Me.radGroupBox2.Text = "Groupbox style"
            ' 
            ' radRadioButton2
            ' 
            Me.radRadioButton2.Location = New System.Drawing.Point(12, 24)
            Me.radRadioButton2.Name = "radRadioButton2"
            Me.radRadioButton2.Size = New System.Drawing.Size(65, 18)
            Me.radRadioButton2.TabIndex = 1
            Me.radRadioButton2.Text = "Standard"
            ' 
            ' radRadioButton1
            ' 
            Me.radRadioButton1.Location = New System.Drawing.Point(128, 24)
            Me.radRadioButton1.Name = "radRadioButton1"
            Me.radRadioButton1.Size = New System.Drawing.Size(50, 18)
            Me.radRadioButton1.TabIndex = 0
            Me.radRadioButton1.Text = "Office"
            ' 
            ' radGroupBox3
            ' 
            Me.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox3.Controls.Add(Me.radRadioButton3)
            Me.radGroupBox3.Controls.Add(Me.radRadioButton5)
            Me.radGroupBox3.Controls.Add(Me.radRadioButton6)
            Me.radGroupBox3.Controls.Add(Me.radRadioButton4)
            Me.radGroupBox3.HeaderText = "Header Position"
            Me.radGroupBox3.Location = New System.Drawing.Point(52, 43)
            Me.radGroupBox3.Name = "radGroupBox3"
            Me.radGroupBox3.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            ' 
            ' 
            ' 
            Me.radGroupBox3.RootElement.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            Me.radGroupBox3.Size = New System.Drawing.Size(116, 138)
            Me.radGroupBox3.TabIndex = 1
            Me.radGroupBox3.Text = "Header Position"
            ' 
            ' radRadioButton3
            ' 
            Me.radRadioButton3.Location = New System.Drawing.Point(12, 21)
            Me.radRadioButton3.Name = "radRadioButton3"
            Me.radRadioButton3.Size = New System.Drawing.Size(39, 18)
            Me.radRadioButton3.TabIndex = 0
            Me.radRadioButton3.Text = "Top"
            ' 
            ' radRadioButton5
            ' 
            Me.radRadioButton5.Location = New System.Drawing.Point(12, 65)
            Me.radRadioButton5.Name = "radRadioButton5"
            Me.radRadioButton5.Size = New System.Drawing.Size(57, 18)
            Me.radRadioButton5.TabIndex = 2
            Me.radRadioButton5.Text = "Bottom"
            ' 
            ' radRadioButton6
            ' 
            Me.radRadioButton6.Location = New System.Drawing.Point(12, 87)
            Me.radRadioButton6.Name = "radRadioButton6"
            Me.radRadioButton6.Size = New System.Drawing.Size(39, 18)
            Me.radRadioButton6.TabIndex = 3
            Me.radRadioButton6.Text = "Left"
            ' 
            ' radRadioButton4
            ' 
            Me.radRadioButton4.Location = New System.Drawing.Point(12, 43)
            Me.radRadioButton4.Name = "radRadioButton4"
            Me.radRadioButton4.Size = New System.Drawing.Size(47, 18)
            Me.radRadioButton4.TabIndex = 1
            Me.radRadioButton4.Text = "Right"
            ' 
            ' radGroupBox4
            ' 
            Me.radGroupBox4.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox4.Controls.Add(Me.radRadioButton11)
            Me.radGroupBox4.Controls.Add(Me.radRadioButton10)
            Me.radGroupBox4.Controls.Add(Me.radRadioButton9)
            Me.radGroupBox4.Controls.Add(Me.radRadioButton8)
            Me.radGroupBox4.Controls.Add(Me.radRadioButton7)
            Me.radGroupBox4.HeaderText = "Text and Image Relation"
            Me.radGroupBox4.Location = New System.Drawing.Point(176, 43)
            Me.radGroupBox4.Name = "radGroupBox4"
            Me.radGroupBox4.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            ' 
            ' 
            ' 
            Me.radGroupBox4.RootElement.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            Me.radGroupBox4.Size = New System.Drawing.Size(189, 138)
            Me.radGroupBox4.TabIndex = 2
            Me.radGroupBox4.Text = "Text and Image Relation"
            ' 
            ' radRadioButton11
            ' 
            Me.radRadioButton11.Location = New System.Drawing.Point(16, 109)
            Me.radRadioButton11.Name = "radRadioButton11"
            Me.radRadioButton11.Size = New System.Drawing.Size(111, 18)
            Me.radRadioButton11.TabIndex = 4
            Me.radRadioButton11.Text = "Text before image"
            ' 
            ' radRadioButton10
            ' 
            Me.radRadioButton10.Location = New System.Drawing.Point(16, 87)
            Me.radRadioButton10.Name = "radRadioButton10"
            Me.radRadioButton10.Size = New System.Drawing.Size(109, 18)
            Me.radRadioButton10.TabIndex = 3
            Me.radRadioButton10.Text = "Text above image"
            ' 
            ' radRadioButton9
            ' 
            Me.radRadioButton9.Location = New System.Drawing.Point(16, 65)
            Me.radRadioButton9.Name = "radRadioButton9"
            Me.radRadioButton9.Size = New System.Drawing.Size(58, 18)
            Me.radRadioButton9.TabIndex = 2
            Me.radRadioButton9.Text = "Overlay"
            ' 
            ' radRadioButton8
            ' 
            Me.radRadioButton8.Location = New System.Drawing.Point(16, 43)
            Me.radRadioButton8.Name = "radRadioButton8"
            Me.radRadioButton8.Size = New System.Drawing.Size(109, 18)
            Me.radRadioButton8.TabIndex = 1
            Me.radRadioButton8.Text = "Image before text"
            ' 
            ' radRadioButton7
            ' 
            Me.radRadioButton7.Location = New System.Drawing.Point(16, 21)
            Me.radRadioButton7.Name = "radRadioButton7"
            Me.radRadioButton7.Size = New System.Drawing.Size(107, 18)
            Me.radRadioButton7.TabIndex = 0
            Me.radRadioButton7.Text = "Image above text"
            ' 
            ' Form1
            ' 
            Me.Controls.Add(Me.radGroupBox1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1223, 659)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.radGroupBox1, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox2.ResumeLayout(False)
            Me.radGroupBox2.PerformLayout()
            DirectCast(Me.radRadioButton2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox3.ResumeLayout(False)
            Me.radGroupBox3.PerformLayout()
            DirectCast(Me.radRadioButton3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton5, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton6, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox4.ResumeLayout(False)
            Me.radGroupBox4.PerformLayout()
            DirectCast(Me.radRadioButton11, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton10, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton9, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton8, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton7, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Private radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
        Private radGroupBox3 As Telerik.WinControls.UI.RadGroupBox
        Private radGroupBox4 As Telerik.WinControls.UI.RadGroupBox
        Private radRadioButton2 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton1 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton3 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton6 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton5 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton4 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton7 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton8 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton9 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton10 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton11 As Telerik.WinControls.UI.RadRadioButton

    End Class
End Namespace