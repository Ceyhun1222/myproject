Namespace SpreadConvertion
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
            Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
            Me.pictureBox1 = New System.Windows.Forms.PictureBox()
            Me.label1 = New System.Windows.Forms.Label()
            Me.pictureBox2 = New System.Windows.Forms.PictureBox()
            Me.loadCustomDocumentButton = New Telerik.WinControls.UI.RadButton()
            Me.loadSampleDocumentButton = New Telerik.WinControls.UI.RadButton()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.fileNameLabel = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.fileExtensionsDropDownList = New Telerik.WinControls.UI.RadDropDownList()
            Me.saveButton = New Telerik.WinControls.UI.RadButton()
            Me.telerikMetroTheme1 = New Telerik.WinControls.Themes.TelerikMetroTheme()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.pictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.loadCustomDocumentButton, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.loadSampleDocumentButton, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.fileNameLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.fileExtensionsDropDownList, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.saveButton, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' pictureBox1
            ' 
            Me.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictureBox1.Image = My.Resources.CustomDocumentImage
            Me.pictureBox1.Location = New System.Drawing.Point(0, 0)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(262, 200)
            Me.pictureBox1.TabIndex = 0
            Me.pictureBox1.TabStop = False
            ' 
            ' label1
            ' 
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(269, 89)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(38, 13)
            Me.label1.TabIndex = 1
            Me.label1.Text = "- OR - "
            ' 
            ' pictureBox2
            ' 
            Me.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictureBox2.Image = My.Resources.SampleDocumentImage
            Me.pictureBox2.Location = New System.Drawing.Point(313, 0)
            Me.pictureBox2.Name = "pictureBox2"
            Me.pictureBox2.Size = New System.Drawing.Size(262, 200)
            Me.pictureBox2.TabIndex = 2
            Me.pictureBox2.TabStop = False
            ' 
            ' loadCustomDocumentButton
            ' 
            Me.loadCustomDocumentButton.Location = New System.Drawing.Point(62, 216)
            Me.loadCustomDocumentButton.Name = "loadCustomDocumentButton"
            Me.loadCustomDocumentButton.Size = New System.Drawing.Size(146, 24)
            Me.loadCustomDocumentButton.TabIndex = 3
            Me.loadCustomDocumentButton.Text = "Load Custom Document"
            Me.loadCustomDocumentButton.ThemeName = "TelerikMetro"
            AddHandler Me.loadCustomDocumentButton.Click, AddressOf Me.loadCustomDocumentButton_Click
            ' 
            ' loadSampleDocumentButton
            ' 
            Me.loadSampleDocumentButton.Location = New System.Drawing.Point(377, 216)
            Me.loadSampleDocumentButton.Name = "loadSampleDocumentButton"
            Me.loadSampleDocumentButton.Size = New System.Drawing.Size(146, 24)
            Me.loadSampleDocumentButton.TabIndex = 4
            Me.loadSampleDocumentButton.Text = "Load Sample Document"
            Me.loadSampleDocumentButton.ThemeName = "TelerikMetro"
            AddHandler Me.loadSampleDocumentButton.Click, AddressOf Me.loadSampleDocumentButton_Click
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold)
            Me.radLabel1.Location = New System.Drawing.Point(0, 252)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(31, 18)
            Me.radLabel1.TabIndex = 5
            Me.radLabel1.Text = "File: "
            Me.radLabel1.ThemeName = "TelerikMetro"
            ' 
            ' fileNameLabel
            ' 
            Me.fileNameLabel.Location = New System.Drawing.Point(37, 252)
            Me.fileNameLabel.Name = "fileNameLabel"
            Me.fileNameLabel.Size = New System.Drawing.Size(2, 2)
            Me.fileNameLabel.TabIndex = 6
            Me.fileNameLabel.ThemeName = "TelerikMetro"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold)
            Me.radLabel2.Location = New System.Drawing.Point(0, 284)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(64, 18)
            Me.radLabel2.TabIndex = 7
            Me.radLabel2.Text = "Extension: "
            Me.radLabel2.ThemeName = "TelerikMetro"
            ' 
            ' fileExtensionsDropDownList
            ' 
            Me.fileExtensionsDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            radListDataItem1.Text = "Xlsx"
            radListDataItem2.Text = "Csv"
            radListDataItem3.Text = "Txt"
            Me.fileExtensionsDropDownList.Items.Add(radListDataItem1)
            Me.fileExtensionsDropDownList.Items.Add(radListDataItem2)
            Me.fileExtensionsDropDownList.Items.Add(radListDataItem3)
            Me.fileExtensionsDropDownList.Location = New System.Drawing.Point(70, 284)
            Me.fileExtensionsDropDownList.Name = "fileExtensionsDropDownList"
            Me.fileExtensionsDropDownList.Size = New System.Drawing.Size(50, 24)
            Me.fileExtensionsDropDownList.TabIndex = 8
            Me.fileExtensionsDropDownList.ThemeName = "TelerikMetro"
            ' 
            ' saveButton
            ' 
            Me.saveButton.Enabled = False
            Me.saveButton.Location = New System.Drawing.Point(0, 320)
            Me.saveButton.Name = "saveButton"
            Me.saveButton.Size = New System.Drawing.Size(575, 24)
            Me.saveButton.TabIndex = 9
            Me.saveButton.Text = "Save"
            Me.saveButton.ThemeName = "TelerikMetro"
            AddHandler Me.saveButton.Click, AddressOf Me.saveButton_Click
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(575, 345)
            Me.Controls.Add(Me.saveButton)
            Me.Controls.Add(Me.fileExtensionsDropDownList)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.fileNameLabel)
            Me.Controls.Add(Me.radLabel1)
            Me.Controls.Add(Me.loadSampleDocumentButton)
            Me.Controls.Add(Me.loadCustomDocumentButton)
            Me.Controls.Add(Me.pictureBox2)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.pictureBox1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "Convert Documents"
            Me.ThemeName = "TelerikMetro"
            AddHandler Me.Load, AddressOf Me.Form1_Load
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.pictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.loadCustomDocumentButton, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.loadSampleDocumentButton, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.fileNameLabel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.fileExtensionsDropDownList, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.saveButton, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private pictureBox1 As System.Windows.Forms.PictureBox
        Private label1 As System.Windows.Forms.Label
        Private pictureBox2 As System.Windows.Forms.PictureBox
        Private loadCustomDocumentButton As Telerik.WinControls.UI.RadButton
        Private loadSampleDocumentButton As Telerik.WinControls.UI.RadButton
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private fileNameLabel As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private fileExtensionsDropDownList As Telerik.WinControls.UI.RadDropDownList
        Private saveButton As Telerik.WinControls.UI.RadButton
        Private telerikMetroTheme1 As Telerik.WinControls.Themes.TelerikMetroTheme
    End Class
End Namespace