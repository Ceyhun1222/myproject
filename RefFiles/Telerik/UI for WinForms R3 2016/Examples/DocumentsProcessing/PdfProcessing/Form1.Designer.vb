Namespace PdfProcessing
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
            Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Me.pictureBox1 = New System.Windows.Forms.PictureBox()
            Me.dateTimePicker1 = New System.Windows.Forms.DateTimePicker()
            Me.buttonSave = New Telerik.WinControls.UI.RadButton()
            Me.telerikMetroTheme1 = New Telerik.WinControls.Themes.TelerikMetroTheme()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.buttonSave, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' pictureBox1
            ' 
            Me.pictureBox1.Image = DirectCast(resources.GetObject("pictureBox1.Image"), System.Drawing.Image)
            Me.pictureBox1.Location = New System.Drawing.Point(-2, 0)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(432, 572)
            Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.pictureBox1.TabIndex = 0
            Me.pictureBox1.TabStop = False
            ' 
            ' dateTimePicker1
            ' 
            Me.dateTimePicker1.Location = New System.Drawing.Point(678, 545)
            Me.dateTimePicker1.Name = "dateTimePicker1"
            Me.dateTimePicker1.Size = New System.Drawing.Size(200, 20)
            Me.dateTimePicker1.TabIndex = 1
            ' 
            ' buttonSave
            ' 
            Me.buttonSave.Location = New System.Drawing.Point(436, 0)
            Me.buttonSave.Name = "buttonSave"
            Me.buttonSave.Size = New System.Drawing.Size(110, 24)
            Me.buttonSave.TabIndex = 2
            Me.buttonSave.Text = "Save Document"
            Me.buttonSave.ThemeName = "TelerikMetro"
            AddHandler Me.buttonSave.Click, AddressOf Me.buttonSave_Click
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(546, 573)
            Me.Controls.Add(Me.buttonSave)
            Me.Controls.Add(Me.dateTimePicker1)
            Me.Controls.Add(Me.pictureBox1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "PDF Document Processing"
            Me.ThemeName = "TelerikMetro"
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.buttonSave, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private pictureBox1 As System.Windows.Forms.PictureBox
        Private dateTimePicker1 As System.Windows.Forms.DateTimePicker
        Private buttonSave As Telerik.WinControls.UI.RadButton
        Private telerikMetroTheme1 As Telerik.WinControls.Themes.TelerikMetroTheme
    End Class
End Namespace