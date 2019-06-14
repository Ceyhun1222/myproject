Namespace Telerik.Examples.WinControls.ChartView.Printing
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
            Me.buttonPrint = New Telerik.WinControls.UI.RadButton()
            Me.buttonPrintPreview = New Telerik.WinControls.UI.RadButton()
            Me.buttonPrintSettings = New Telerik.WinControls.UI.RadButton()
            Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.RadDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
            Me.RadSpinEditorWidth = New Telerik.WinControls.UI.RadSpinEditor()
            Me.RadSpinEditorHeight = New Telerik.WinControls.UI.RadSpinEditor()
            Me.RadButtonExport = New Telerik.WinControls.UI.RadButton()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.buttonPrint, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.buttonPrintPreview, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.buttonPrintSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.RadGroupBox1.SuspendLayout()
            CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.RadGroupBox2.SuspendLayout()
            CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadSpinEditorWidth, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadSpinEditorHeight, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RadButtonExport, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.RadGroupBox2)
            Me.settingsPanel.Controls.Add(Me.RadGroupBox1)
            Me.settingsPanel.Dock = System.Windows.Forms.DockStyle.Right
            Me.settingsPanel.Location = New System.Drawing.Point(1585, 0)
            Me.settingsPanel.Size = New System.Drawing.Size(286, 1058)
            Me.settingsPanel.Controls.SetChildIndex(Me.RadGroupBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.RadGroupBox2, 0)
            '
            'buttonPrint
            '
            Me.buttonPrint.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.buttonPrint.Location = New System.Drawing.Point(5, 21)
            Me.buttonPrint.Name = "buttonPrint"
            Me.buttonPrint.Size = New System.Drawing.Size(256, 24)
            Me.buttonPrint.TabIndex = 1
            Me.buttonPrint.Text = "Print"
            '
            'buttonPrintPreview
            '
            Me.buttonPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.buttonPrintPreview.Location = New System.Drawing.Point(5, 51)
            Me.buttonPrintPreview.Name = "buttonPrintPreview"
            Me.buttonPrintPreview.Size = New System.Drawing.Size(256, 24)
            Me.buttonPrintPreview.TabIndex = 1
            Me.buttonPrintPreview.Text = "Print Preview"
            '
            'buttonPrintSettings
            '
            Me.buttonPrintSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.buttonPrintSettings.Location = New System.Drawing.Point(5, 81)
            Me.buttonPrintSettings.Name = "buttonPrintSettings"
            Me.buttonPrintSettings.Size = New System.Drawing.Size(256, 24)
            Me.buttonPrintSettings.TabIndex = 1
            Me.buttonPrintSettings.Text = "Print Settings"
            '
            'RadGroupBox1
            '
            Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.RadGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadGroupBox1.Controls.Add(Me.buttonPrint)
            Me.RadGroupBox1.Controls.Add(Me.buttonPrintSettings)
            Me.RadGroupBox1.Controls.Add(Me.buttonPrintPreview)
            Me.RadGroupBox1.HeaderText = "Printing"
            Me.RadGroupBox1.Location = New System.Drawing.Point(10, 32)
            Me.RadGroupBox1.Name = "RadGroupBox1"
            Me.RadGroupBox1.Size = New System.Drawing.Size(266, 117)
            Me.RadGroupBox1.TabIndex = 2
            Me.RadGroupBox1.Text = "Printing"
            '
            'RadGroupBox2
            '
            Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.RadGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadGroupBox2.Controls.Add(Me.RadButtonExport)
            Me.RadGroupBox2.Controls.Add(Me.RadSpinEditorHeight)
            Me.RadGroupBox2.Controls.Add(Me.RadSpinEditorWidth)
            Me.RadGroupBox2.Controls.Add(Me.RadDropDownList1)
            Me.RadGroupBox2.Controls.Add(Me.RadLabel3)
            Me.RadGroupBox2.Controls.Add(Me.RadLabel2)
            Me.RadGroupBox2.Controls.Add(Me.RadLabel1)
            Me.RadGroupBox2.HeaderText = "Export"
            Me.RadGroupBox2.Location = New System.Drawing.Point(10, 162)
            Me.RadGroupBox2.Name = "RadGroupBox2"
            Me.RadGroupBox2.Size = New System.Drawing.Size(266, 211)
            Me.RadGroupBox2.TabIndex = 3
            Me.RadGroupBox2.Text = "Export"
            '
            'RadLabel1
            '
            Me.RadLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadLabel1.Location = New System.Drawing.Point(5, 25)
            Me.RadLabel1.Name = "RadLabel1"
            Me.RadLabel1.Size = New System.Drawing.Size(73, 18)
            Me.RadLabel1.TabIndex = 0
            Me.RadLabel1.Text = "Image format"
            '
            'RadLabel2
            '
            Me.RadLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadLabel2.Location = New System.Drawing.Point(5, 76)
            Me.RadLabel2.Name = "RadLabel2"
            Me.RadLabel2.Size = New System.Drawing.Size(35, 18)
            Me.RadLabel2.TabIndex = 0
            Me.RadLabel2.Text = "Width"
            '
            'RadLabel3
            '
            Me.RadLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadLabel3.Location = New System.Drawing.Point(5, 126)
            Me.RadLabel3.Name = "RadLabel3"
            Me.RadLabel3.Size = New System.Drawing.Size(39, 18)
            Me.RadLabel3.TabIndex = 0
            Me.RadLabel3.Text = "Height"
            '
            'RadDropDownList1
            '
            Me.RadDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadDropDownList1.Location = New System.Drawing.Point(5, 45)
            Me.RadDropDownList1.Name = "RadDropDownList1"
            Me.RadDropDownList1.Size = New System.Drawing.Size(256, 20)
            Me.RadDropDownList1.TabIndex = 1
            Me.RadDropDownList1.Text = "RadDropDownList1"
            '
            'RadSpinEditorWidth
            '
            Me.RadSpinEditorWidth.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadSpinEditorWidth.Location = New System.Drawing.Point(5, 96)
            Me.RadSpinEditorWidth.Maximum = New Decimal(New Integer() {3000, 0, 0, 0})
            Me.RadSpinEditorWidth.Minimum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.RadSpinEditorWidth.Name = "RadSpinEditorWidth"
            Me.RadSpinEditorWidth.Size = New System.Drawing.Size(256, 20)
            Me.RadSpinEditorWidth.TabIndex = 2
            Me.RadSpinEditorWidth.TabStop = False
            Me.RadSpinEditorWidth.Value = New Decimal(New Integer() {1000, 0, 0, 0})
            '
            'RadSpinEditorHeight
            '
            Me.RadSpinEditorHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadSpinEditorHeight.Location = New System.Drawing.Point(5, 146)
            Me.RadSpinEditorHeight.Maximum = New Decimal(New Integer() {3000, 0, 0, 0})
            Me.RadSpinEditorHeight.Minimum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.RadSpinEditorHeight.Name = "RadSpinEditorHeight"
            Me.RadSpinEditorHeight.Size = New System.Drawing.Size(256, 20)
            Me.RadSpinEditorHeight.TabIndex = 2
            Me.RadSpinEditorHeight.TabStop = False
            Me.RadSpinEditorHeight.Value = New Decimal(New Integer() {3000, 0, 0, 0})
            '
            'RadButtonExport
            '
            Me.RadButtonExport.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.RadButtonExport.Location = New System.Drawing.Point(5, 174)
            Me.RadButtonExport.Name = "RadButtonExport"
            Me.RadButtonExport.Size = New System.Drawing.Size(256, 24)
            Me.RadButtonExport.TabIndex = 3
            Me.RadButtonExport.Text = "Export"
            '
            'Form1
            '
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1881, 1068)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.buttonPrint, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.buttonPrintPreview, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.buttonPrintSettings, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.RadGroupBox1.ResumeLayout(False)
            CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.RadGroupBox2.ResumeLayout(False)
            Me.RadGroupBox2.PerformLayout()
            CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadSpinEditorWidth, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadSpinEditorHeight, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RadButtonExport, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private buttonPrint As Telerik.WinControls.UI.RadButton
        Private buttonPrintPreview As Telerik.WinControls.UI.RadButton
        Private buttonPrintSettings As Telerik.WinControls.UI.RadButton
        Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents RadSpinEditorHeight As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents RadSpinEditorWidth As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents RadDropDownList1 As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents RadButtonExport As Telerik.WinControls.UI.RadButton
    End Class
End Namespace