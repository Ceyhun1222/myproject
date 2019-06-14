Namespace WordGridIntegration
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
            Dim tableViewDefinition2 As New Telerik.WinControls.UI.TableViewDefinition()
            Me.telerikMetroTheme1 = New Telerik.WinControls.Themes.TelerikMetroTheme()
            Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
            Me.headerRowColorBox = New Telerik.WinControls.UI.RadColorBox()
            Me.groupHeaderColorBox = New Telerik.WinControls.UI.RadColorBox()
            Me.dataRowColorBox = New Telerik.WinControls.UI.RadColorBox()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.exportButton = New Telerik.WinControls.UI.RadButton()
            Me.exportFormatDropDownList = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.headerRowColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.groupHeaderColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dataRowColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.exportButton, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.exportFormatDropDownList, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radGridView1
            ' 
            Me.radGridView1.BackColor = System.Drawing.Color.White
            Me.radGridView1.Cursor = System.Windows.Forms.Cursors.[Default]
            Me.radGridView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
            Me.radGridView1.ForeColor = System.Drawing.SystemColors.ControlText
            Me.radGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.radGridView1.Location = New System.Drawing.Point(0, 0)
            ' 
            ' 
            ' 
            Me.radGridView1.MasterTemplate.AllowAddNewRow = False
            Me.radGridView1.MasterTemplate.AllowColumnChooser = False
            Me.radGridView1.MasterTemplate.AllowColumnReorder = False
            Me.radGridView1.MasterTemplate.AllowDeleteRow = False
            Me.radGridView1.MasterTemplate.AllowDragToGroup = False
            Me.radGridView1.MasterTemplate.AllowEditRow = False
            Me.radGridView1.MasterTemplate.AllowRowResize = False
            Me.radGridView1.MasterTemplate.ShowGroupedColumns = True
            Me.radGridView1.MasterTemplate.ShowRowHeaderColumn = False
            Me.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition2
            Me.radGridView1.Name = "radGridView1"
            Me.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.radGridView1.Size = New System.Drawing.Size(661, 361)
            Me.radGridView1.TabIndex = 0
            Me.radGridView1.Text = "radGridView1"
            Me.radGridView1.ThemeName = "TelerikMetro"
            ' 
            ' headerRowColorBox
            ' 
            Me.headerRowColorBox.Location = New System.Drawing.Point(154, 404)
            Me.headerRowColorBox.Name = "headerRowColorBox"
            Me.headerRowColorBox.Size = New System.Drawing.Size(139, 24)
            Me.headerRowColorBox.TabIndex = 1
            Me.headerRowColorBox.Text = "radColorBox1"
            Me.headerRowColorBox.ThemeName = "TelerikMetro"
            ' 
            ' groupHeaderColorBox
            ' 
            Me.groupHeaderColorBox.Location = New System.Drawing.Point(154, 434)
            Me.groupHeaderColorBox.Name = "groupHeaderColorBox"
            Me.groupHeaderColorBox.Size = New System.Drawing.Size(139, 24)
            Me.groupHeaderColorBox.TabIndex = 2
            Me.groupHeaderColorBox.Text = "radColorBox2"
            Me.groupHeaderColorBox.ThemeName = "TelerikMetro"
            ' 
            ' dataRowColorBox
            ' 
            Me.dataRowColorBox.Location = New System.Drawing.Point(154, 464)
            Me.dataRowColorBox.Name = "dataRowColorBox"
            Me.dataRowColorBox.Size = New System.Drawing.Size(139, 24)
            Me.dataRowColorBox.TabIndex = 3
            Me.dataRowColorBox.Text = "radColorBox3"
            Me.dataRowColorBox.ThemeName = "TelerikMetro"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Location = New System.Drawing.Point(0, 408)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(106, 16)
            Me.radLabel1.TabIndex = 4
            Me.radLabel1.Text = "Header background"
            Me.radLabel1.ThemeName = "TelerikMetro"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Location = New System.Drawing.Point(0, 438)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(138, 16)
            Me.radLabel2.TabIndex = 5
            Me.radLabel2.Text = "Group header background"
            Me.radLabel2.ThemeName = "TelerikMetro"
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Location = New System.Drawing.Point(0, 468)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(91, 16)
            Me.radLabel3.TabIndex = 6
            Me.radLabel3.Text = "Row background"
            Me.radLabel3.ThemeName = "TelerikMetro"
            ' 
            ' exportButton
            ' 
            Me.exportButton.Location = New System.Drawing.Point(0, 535)
            Me.exportButton.Name = "exportButton"
            Me.exportButton.Size = New System.Drawing.Size(661, 24)
            Me.exportButton.TabIndex = 7
            Me.exportButton.Text = "Export"
            Me.exportButton.ThemeName = "TelerikMetro"
            AddHandler Me.exportButton.Click, AddressOf Me.exportButton_Click
            ' 
            ' exportFormatDropDownList
            ' 
            Me.exportFormatDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.exportFormatDropDownList.Location = New System.Drawing.Point(154, 376)
            Me.exportFormatDropDownList.Name = "exportFormatDropDownList"
            Me.exportFormatDropDownList.Size = New System.Drawing.Size(139, 24)
            Me.exportFormatDropDownList.TabIndex = 8
            Me.exportFormatDropDownList.Text = "radDropDownList1"
            Me.exportFormatDropDownList.ThemeName = "TelerikMetro"
            AddHandler Me.exportFormatDropDownList.SelectedIndexChanged, AddressOf Me.exportFormatDropDownList_SelectedIndexChanged
            ' 
            ' radLabel4
            ' 
            Me.radLabel4.Location = New System.Drawing.Point(0, 378)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(78, 16)
            Me.radLabel4.TabIndex = 9
            Me.radLabel4.Text = "Export Format"
            Me.radLabel4.ThemeName = "TelerikMetro"
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBox1.Location = New System.Drawing.Point(0, 502)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(207, 19)
            Me.radCheckBox1.TabIndex = 10
            Me.radCheckBox1.Text = "Repeat header row on every page"
            Me.radCheckBox1.ThemeName = "TelerikMetro"
            Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            AddHandler Me.radCheckBox1.CheckStateChanged, AddressOf Me.radCheckBox1_CheckStateChanged
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(661, 559)
            Me.Controls.Add(Me.radCheckBox1)
            Me.Controls.Add(Me.radLabel4)
            Me.Controls.Add(Me.exportFormatDropDownList)
            Me.Controls.Add(Me.exportButton)
            Me.Controls.Add(Me.radLabel3)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.radLabel1)
            Me.Controls.Add(Me.dataRowColorBox)
            Me.Controls.Add(Me.groupHeaderColorBox)
            Me.Controls.Add(Me.headerRowColorBox)
            Me.Controls.Add(Me.radGridView1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "Word Processing Grid Integration"
            Me.ThemeName = "TelerikMetro"
            AddHandler Me.Load, AddressOf Me.Form1_Load
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.headerRowColorBox, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.groupHeaderColorBox, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dataRowColorBox, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.exportButton, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.exportFormatDropDownList, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private telerikMetroTheme1 As Telerik.WinControls.Themes.TelerikMetroTheme
        Private radGridView1 As Telerik.WinControls.UI.RadGridView
        Private headerRowColorBox As Telerik.WinControls.UI.RadColorBox
        Private groupHeaderColorBox As Telerik.WinControls.UI.RadColorBox
        Private dataRowColorBox As Telerik.WinControls.UI.RadColorBox
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private exportButton As Telerik.WinControls.UI.RadButton
        Private exportFormatDropDownList As Telerik.WinControls.UI.RadDropDownList
        Private radLabel4 As Telerik.WinControls.UI.RadLabel
        Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
    End Class
End Namespace