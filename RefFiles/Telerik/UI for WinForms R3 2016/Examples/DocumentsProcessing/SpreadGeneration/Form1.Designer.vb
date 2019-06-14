Namespace SpreadGeneration
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
            Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn2 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn3 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn4 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim tableViewDefinition1 As New Telerik.WinControls.UI.TableViewDefinition()
            Me.telerikMetroTheme1 = New Telerik.WinControls.Themes.TelerikMetroTheme()
            Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.exportFormatDropDownList = New Telerik.WinControls.UI.RadDropDownList()
            Me.exportButton = New Telerik.WinControls.UI.RadButton()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.totalSumLabel = New Telerik.WinControls.UI.RadLabel()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.exportFormatDropDownList, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.exportButton, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.totalSumLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radGridView1
            ' 
            Me.radGridView1.AllowDrop = True
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
            Me.radGridView1.MasterTemplate.AllowCellContextMenu = False
            Me.radGridView1.MasterTemplate.AllowColumnChooser = False
            Me.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = False
            Me.radGridView1.MasterTemplate.AllowColumnReorder = False
            Me.radGridView1.MasterTemplate.AllowDeleteRow = False
            Me.radGridView1.MasterTemplate.AllowDragToGroup = False
            Me.radGridView1.MasterTemplate.AllowEditRow = False
            Me.radGridView1.MasterTemplate.AllowRowReorder = True
            Me.radGridView1.MasterTemplate.AllowRowResize = False
            gridViewTextBoxColumn1.EnableExpressionEditor = False
            gridViewTextBoxColumn1.FieldName = "Name"
            gridViewTextBoxColumn1.HeaderText = "ITEM"
            gridViewTextBoxColumn1.Name = "ITEM"
            gridViewTextBoxColumn1.Width = 41
            gridViewTextBoxColumn2.EnableExpressionEditor = False
            gridViewTextBoxColumn2.FieldName = "Quantity"
            gridViewTextBoxColumn2.HeaderText = "QTY"
            gridViewTextBoxColumn2.Name = "QTY"
            gridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
            gridViewTextBoxColumn2.Width = 36
            gridViewTextBoxColumn3.EnableExpressionEditor = False
            gridViewTextBoxColumn3.FieldName = "UnitPrice"
            gridViewTextBoxColumn3.FormatString = "{0:C}"
            gridViewTextBoxColumn3.HeaderText = "PRICE"
            gridViewTextBoxColumn3.Name = "Price"
            gridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleRight
            gridViewTextBoxColumn3.Width = 46
            gridViewTextBoxColumn4.EnableExpressionEditor = False
            gridViewTextBoxColumn4.FieldName = "SubTotal"
            gridViewTextBoxColumn4.FormatString = "{0:C}"
            gridViewTextBoxColumn4.HeaderText = "SUB TOTAL"
            gridViewTextBoxColumn4.Name = "Sub Total"
            gridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleRight
            gridViewTextBoxColumn4.Width = 81
            Me.radGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {gridViewTextBoxColumn1, gridViewTextBoxColumn2, gridViewTextBoxColumn3, gridViewTextBoxColumn4})
            Me.radGridView1.MasterTemplate.EnableGrouping = False
            Me.radGridView1.MasterTemplate.EnableSorting = False
            Me.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1
            Me.radGridView1.Name = "radGridView1"
            Me.radGridView1.[ReadOnly] = True
            Me.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.radGridView1.Size = New System.Drawing.Size(388, 350)
            Me.radGridView1.TabIndex = 0
            Me.radGridView1.Text = "radGridView1"
            Me.radGridView1.ThemeName = "TelerikMetro"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Location = New System.Drawing.Point(0, 398)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(81, 16)
            Me.radLabel1.TabIndex = 1
            Me.radLabel1.Text = "Export Format:"
            Me.radLabel1.ThemeName = "TelerikMetro"
            ' 
            ' exportFormatDropDownList
            ' 
            Me.exportFormatDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.exportFormatDropDownList.Location = New System.Drawing.Point(87, 396)
            Me.exportFormatDropDownList.Name = "exportFormatDropDownList"
            Me.exportFormatDropDownList.Size = New System.Drawing.Size(62, 24)
            Me.exportFormatDropDownList.TabIndex = 2
            Me.exportFormatDropDownList.Text = "radDropDownList1"
            Me.exportFormatDropDownList.ThemeName = "TelerikMetro"
            ' 
            ' exportButton
            ' 
            Me.exportButton.Location = New System.Drawing.Point(0, 443)
            Me.exportButton.Name = "exportButton"
            Me.exportButton.Size = New System.Drawing.Size(388, 24)
            Me.exportButton.TabIndex = 3
            Me.exportButton.Text = "Export"
            Me.exportButton.ThemeName = "TelerikMetro"
            AddHandler Me.exportButton.Click, AddressOf Me.exportButton_Click
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.radLabel2.Location = New System.Drawing.Point(192, 367)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(45, 20)
            Me.radLabel2.TabIndex = 4
            Me.radLabel2.Text = "Total:"
            Me.radLabel2.ThemeName = "TelerikMetro"
            ' 
            ' totalSumLabel
            ' 
            Me.totalSumLabel.AutoSize = False
            Me.totalSumLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.totalSumLabel.Location = New System.Drawing.Point(252, 368)
            Me.totalSumLabel.Name = "totalSumLabel"
            Me.totalSumLabel.Size = New System.Drawing.Size(118, 18)
            Me.totalSumLabel.TabIndex = 5
            Me.totalSumLabel.Text = "$0"
            Me.totalSumLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight
            Me.totalSumLabel.ThemeName = "TelerikMetro"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(388, 468)
            Me.Controls.Add(Me.totalSumLabel)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.exportButton)
            Me.Controls.Add(Me.exportFormatDropDownList)
            Me.Controls.Add(Me.radLabel1)
            Me.Controls.Add(Me.radGridView1)
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
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.exportFormatDropDownList, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.exportButton, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.totalSumLabel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private telerikMetroTheme1 As Telerik.WinControls.Themes.TelerikMetroTheme
        Private radGridView1 As Telerik.WinControls.UI.RadGridView
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private exportFormatDropDownList As Telerik.WinControls.UI.RadDropDownList
        Private exportButton As Telerik.WinControls.UI.RadButton
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private totalSumLabel As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace