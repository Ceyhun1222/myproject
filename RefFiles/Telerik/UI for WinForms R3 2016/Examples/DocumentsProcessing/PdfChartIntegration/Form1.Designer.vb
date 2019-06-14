Namespace PdfChartIntegration
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
            Dim radListDataItem4 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem5 As New Telerik.WinControls.UI.RadListDataItem()
            Dim radListDataItem6 As New Telerik.WinControls.UI.RadListDataItem()
            Me.checkBoxQ1 = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxQ2 = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxQ3 = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxQ4 = New Telerik.WinControls.UI.RadCheckBox()
            Me.dropDownListNumberOfProducts = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.pictureBox1 = New System.Windows.Forms.PictureBox()
            Me.chartValueStepEditor = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.buttonSave = New Telerik.WinControls.UI.RadButton()
            Me.telerikMetroTheme1 = New Telerik.WinControls.Themes.TelerikMetroTheme()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            DirectCast(Me.checkBoxQ1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxQ2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxQ3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxQ4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dropDownListNumberOfProducts, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.chartValueStepEditor, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.buttonSave, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' checkBoxQ1
            ' 
            Me.checkBoxQ1.Location = New System.Drawing.Point(-1, 24)
            Me.checkBoxQ1.Name = "checkBoxQ1"
            Me.checkBoxQ1.Size = New System.Drawing.Size(39, 19)
            Me.checkBoxQ1.TabIndex = 0
            Me.checkBoxQ1.Text = "Q1"
            Me.checkBoxQ1.ThemeName = "TelerikMetro"
            ' 
            ' checkBoxQ2
            ' 
            Me.checkBoxQ2.Location = New System.Drawing.Point(118, 24)
            Me.checkBoxQ2.Name = "checkBoxQ2"
            Me.checkBoxQ2.Size = New System.Drawing.Size(39, 19)
            Me.checkBoxQ2.TabIndex = 1
            Me.checkBoxQ2.Text = "Q2"
            Me.checkBoxQ2.ThemeName = "TelerikMetro"
            ' 
            ' checkBoxQ3
            ' 
            Me.checkBoxQ3.Location = New System.Drawing.Point(238, 24)
            Me.checkBoxQ3.Name = "checkBoxQ3"
            Me.checkBoxQ3.Size = New System.Drawing.Size(39, 19)
            Me.checkBoxQ3.TabIndex = 2
            Me.checkBoxQ3.Text = "Q3"
            Me.checkBoxQ3.ThemeName = "TelerikMetro"
            ' 
            ' checkBoxQ4
            ' 
            Me.checkBoxQ4.Location = New System.Drawing.Point(358, 24)
            Me.checkBoxQ4.Name = "checkBoxQ4"
            Me.checkBoxQ4.Size = New System.Drawing.Size(39, 19)
            Me.checkBoxQ4.TabIndex = 3
            Me.checkBoxQ4.Text = "Q4"
            Me.checkBoxQ4.ThemeName = "TelerikMetro"
            ' 
            ' dropDownListNumberOfProducts
            ' 
            Me.dropDownListNumberOfProducts.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            radListDataItem1.Text = "1"
            radListDataItem2.Text = "2"
            radListDataItem3.Text = "3"
            radListDataItem4.Text = "4"
            radListDataItem5.Text = "5"
            radListDataItem6.Text = "6"
            Me.dropDownListNumberOfProducts.Items.Add(radListDataItem1)
            Me.dropDownListNumberOfProducts.Items.Add(radListDataItem2)
            Me.dropDownListNumberOfProducts.Items.Add(radListDataItem3)
            Me.dropDownListNumberOfProducts.Items.Add(radListDataItem4)
            Me.dropDownListNumberOfProducts.Items.Add(radListDataItem5)
            Me.dropDownListNumberOfProducts.Items.Add(radListDataItem6)
            Me.dropDownListNumberOfProducts.Location = New System.Drawing.Point(0, 82)
            Me.dropDownListNumberOfProducts.Name = "dropDownListNumberOfProducts"
            Me.dropDownListNumberOfProducts.Size = New System.Drawing.Size(123, 24)
            Me.dropDownListNumberOfProducts.TabIndex = 5
            Me.dropDownListNumberOfProducts.ThemeName = "TelerikMetro"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radLabel1.Location = New System.Drawing.Point(-4, 60)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(127, 21)
            Me.radLabel1.TabIndex = 6
            Me.radLabel1.Text = "Number of Products"
            Me.radLabel1.ThemeName = "TelerikMetro"
            ' 
            ' pictureBox1
            ' 
            Me.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictureBox1.Location = New System.Drawing.Point(0, 158)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(480, 301)
            Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.pictureBox1.TabIndex = 7
            Me.pictureBox1.TabStop = False
            ' 
            ' chartValueStepEditor
            ' 
            Me.chartValueStepEditor.Increment = New Decimal(New Integer() {500, 0, 0, 0})
            Me.chartValueStepEditor.Location = New System.Drawing.Point(240, 82)
            Me.chartValueStepEditor.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.chartValueStepEditor.Minimum = New Decimal(New Integer() {5000, 0, 0, 0})
            Me.chartValueStepEditor.Name = "chartValueStepEditor"
            Me.chartValueStepEditor.Size = New System.Drawing.Size(122, 24)
            Me.chartValueStepEditor.TabIndex = 8
            Me.chartValueStepEditor.TabStop = False
            Me.chartValueStepEditor.ThemeName = "TelerikMetro"
            Me.chartValueStepEditor.Value = New Decimal(New Integer() {5000, 0, 0, 0})
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radLabel2.Location = New System.Drawing.Point(237, 60)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(106, 21)
            Me.radLabel2.TabIndex = 7
            Me.radLabel2.Text = "Chart Value Step"
            Me.radLabel2.ThemeName = "TelerikMetro"
            ' 
            ' buttonSave
            ' 
            Me.buttonSave.Location = New System.Drawing.Point(0, 124)
            Me.buttonSave.Name = "buttonSave"
            Me.buttonSave.Size = New System.Drawing.Size(480, 24)
            Me.buttonSave.TabIndex = 9
            Me.buttonSave.Text = "Save Document"
            Me.buttonSave.ThemeName = "TelerikMetro"
            AddHandler Me.buttonSave.Click, AddressOf Me.buttonSave_Click
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Font = New System.Drawing.Font("Segoe UI", 10.0F)
            Me.radLabel3.Location = New System.Drawing.Point(-3, 0)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(59, 21)
            Me.radLabel3.TabIndex = 10
            Me.radLabel3.Text = "Quarters"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(480, 459)
            Me.Controls.Add(Me.radLabel3)
            Me.Controls.Add(Me.checkBoxQ4)
            Me.Controls.Add(Me.checkBoxQ1)
            Me.Controls.Add(Me.checkBoxQ3)
            Me.Controls.Add(Me.checkBoxQ2)
            Me.Controls.Add(Me.buttonSave)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.chartValueStepEditor)
            Me.Controls.Add(Me.pictureBox1)
            Me.Controls.Add(Me.radLabel1)
            Me.Controls.Add(Me.dropDownListNumberOfProducts)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "Bar Chart"
            Me.ThemeName = "TelerikMetro"
            AddHandler Me.Load, AddressOf Me.Form1_Load
            DirectCast(Me.checkBoxQ1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxQ2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxQ3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxQ4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dropDownListNumberOfProducts, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.chartValueStepEditor, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.buttonSave, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private checkBoxQ1 As Telerik.WinControls.UI.RadCheckBox
        Private checkBoxQ2 As Telerik.WinControls.UI.RadCheckBox
        Private checkBoxQ3 As Telerik.WinControls.UI.RadCheckBox
        Private checkBoxQ4 As Telerik.WinControls.UI.RadCheckBox
        Private dropDownListNumberOfProducts As Telerik.WinControls.UI.RadDropDownList
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private pictureBox1 As System.Windows.Forms.PictureBox
        Private chartValueStepEditor As Telerik.WinControls.UI.RadSpinEditor
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private buttonSave As Telerik.WinControls.UI.RadButton
        Private telerikMetroTheme1 As Telerik.WinControls.Themes.TelerikMetroTheme
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace