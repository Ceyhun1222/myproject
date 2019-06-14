Namespace WordGeneration
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
            Me.telerikMetroTheme1 = New Telerik.WinControls.Themes.TelerikMetroTheme()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.exportFormatDropDownList = New Telerik.WinControls.UI.RadDropDownList()
            Me.exportButton = New Telerik.WinControls.UI.RadButton()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
            Me.pictureBox1 = New System.Windows.Forms.PictureBox()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.exportFormatDropDownList, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.exportButton, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Location = New System.Drawing.Point(394, 22)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(45, 16)
            Me.radLabel1.TabIndex = 1
            Me.radLabel1.Text = "Format:"
            Me.radLabel1.ThemeName = "TelerikMetro"
            ' 
            ' exportFormatDropDownList
            ' 
            Me.exportFormatDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.exportFormatDropDownList.Location = New System.Drawing.Point(441, 20)
            Me.exportFormatDropDownList.Name = "exportFormatDropDownList"
            Me.exportFormatDropDownList.Size = New System.Drawing.Size(62, 24)
            Me.exportFormatDropDownList.TabIndex = 2
            Me.exportFormatDropDownList.Text = "radDropDownList1"
            Me.exportFormatDropDownList.ThemeName = "TelerikMetro"
            ' 
            ' exportButton
            ' 
            Me.exportButton.Location = New System.Drawing.Point(398, 102)
            Me.exportButton.Name = "exportButton"
            Me.exportButton.Size = New System.Drawing.Size(105, 24)
            Me.exportButton.TabIndex = 3
            Me.exportButton.Text = "Generate"
            Me.exportButton.ThemeName = "TelerikMetro"
            AddHandler Me.exportButton.Click, AddressOf Me.exportButton_Click
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBox1.Location = New System.Drawing.Point(397, 44)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(106, 19)
            Me.radCheckBox1.TabIndex = 4
            Me.radCheckBox1.Text = "Include Header"
            Me.radCheckBox1.ThemeName = "TelerikMetro"
            Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            AddHandler Me.radCheckBox1.CheckStateChanged, AddressOf Me.radCheckBox1_CheckStateChanged
            ' 
            ' radCheckBox2
            ' 
            Me.radCheckBox2.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBox2.Location = New System.Drawing.Point(397, 72)
            Me.radCheckBox2.Name = "radCheckBox2"
            Me.radCheckBox2.Size = New System.Drawing.Size(102, 19)
            Me.radCheckBox2.TabIndex = 5
            Me.radCheckBox2.Text = "Include Footer"
            Me.radCheckBox2.ThemeName = "TelerikMetro"
            Me.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            AddHandler Me.radCheckBox2.CheckStateChanged, AddressOf Me.radCheckBox2_CheckStateChanged
            ' 
            ' pictureBox1
            ' 
            Me.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictureBox1.Image = My.Resources.EmailTemplate
            Me.pictureBox1.Location = New System.Drawing.Point(0, 22)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(388, 492)
            Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.pictureBox1.TabIndex = 6
            Me.pictureBox1.TabStop = False
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Location = New System.Drawing.Point(-4, 0)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(85, 16)
            Me.radLabel2.TabIndex = 7
            Me.radLabel2.Text = "Email Template"
            Me.radLabel2.ThemeName = "TelerikMetro"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(506, 515)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.pictureBox1)
            Me.Controls.Add(Me.radCheckBox2)
            Me.Controls.Add(Me.radCheckBox1)
            Me.Controls.Add(Me.exportButton)
            Me.Controls.Add(Me.exportFormatDropDownList)
            Me.Controls.Add(Me.radLabel1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "Generate Documents"
            Me.ThemeName = "TelerikMetro"
            AddHandler Me.Load, AddressOf Me.Form1_Load
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.exportFormatDropDownList, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.exportButton, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private telerikMetroTheme1 As Telerik.WinControls.Themes.TelerikMetroTheme
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private exportFormatDropDownList As Telerik.WinControls.UI.RadDropDownList
        Private exportButton As Telerik.WinControls.UI.RadButton
        Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
        Private pictureBox1 As System.Windows.Forms.PictureBox
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace