
Namespace Telerik.Examples.WinControls.ListView.CheckedListBox
    Partial Class Form1
        ''' <summary> 
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary> 
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(Disposing)
        End Sub

#Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim RadListDataItem1 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem2 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem3 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem4 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem5 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem6 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem7 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem8 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem9 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem10 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem11 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem12 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem13 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem14 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem15 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem16 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem17 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem18 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim ListViewDataItem1 As Telerik.WinControls.UI.ListViewDataItem = New Telerik.WinControls.UI.ListViewDataItem("ListViewItem 1")
            Me.optionsLabel = New Telerik.WinControls.UI.RadLabel()
            Me.ListControlThemes = New Telerik.WinControls.UI.RadListControl()
            Me.themesLabel = New Telerik.WinControls.UI.RadLabel()
            Me.radListView2 = New Telerik.WinControls.UI.RadListView()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckedListBox1 = New Telerik.WinControls.UI.RadCheckedListBox()
            Me.radCheckSelectedButton = New Telerik.WinControls.UI.RadButton()
            Me.radCheckAllButton = New Telerik.WinControls.UI.RadButton()
            Me.radClearAllButton = New Telerik.WinControls.UI.RadButton()
            Me.radOrderButton = New Telerik.WinControls.UI.RadButton()
            Me.radListView1 = New Telerik.WinControls.UI.RadListView()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.optionsLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ListControlThemes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.themesLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radListView2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckedListBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckSelectedButton, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckAllButton, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radClearAllButton, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radOrderButton, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radListView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Location = New System.Drawing.Point(1160, 8)
            Me.settingsPanel.Size = New System.Drawing.Size(230, 188)
            '
            'themePanel
            '
            Me.themePanel.Location = New System.Drawing.Point(1160, 219)
            '
            'optionsLabel
            '
            Me.optionsLabel.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.optionsLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(66, Byte), Integer))
            Me.optionsLabel.Location = New System.Drawing.Point(0, 0)
            Me.optionsLabel.Name = "optionsLabel"
            Me.optionsLabel.Size = New System.Drawing.Size(69, 22)
            Me.optionsLabel.TabIndex = 0
            Me.optionsLabel.Text = "OPTIONS"
            '
            'ListControlThemes
            '
            RadListDataItem1.Selected = True
            RadListDataItem1.Text = "TelerikMetro(Default)"
            RadListDataItem2.Text = "Office2013Dark"
            RadListDataItem3.Text = "Office2013Light"
            RadListDataItem4.Text = "Windows8"
            RadListDataItem5.Text = "VisualStudio2012Dark"
            RadListDataItem6.Text = "VisualStudio2012Light"
            RadListDataItem7.Text = "TelerikMetroBlue"
            RadListDataItem8.Text = "Office2010Black"
            RadListDataItem9.Text = "Office2010Silver"
            RadListDataItem10.Text = "Office2010Blue"
            RadListDataItem11.Text = "ControlDefault"
            RadListDataItem12.Text = "Aqua"
            RadListDataItem13.Text = "Breeze"
            RadListDataItem14.Text = "Desert"
            RadListDataItem15.Text = "HighContrastBlack"
            RadListDataItem16.Text = "Office2007Black"
            RadListDataItem17.Text = "Office2007Silver"
            RadListDataItem18.Text = "Windows7"
            Me.ListControlThemes.Items.Add(RadListDataItem1)
            Me.ListControlThemes.Items.Add(RadListDataItem2)
            Me.ListControlThemes.Items.Add(RadListDataItem3)
            Me.ListControlThemes.Items.Add(RadListDataItem4)
            Me.ListControlThemes.Items.Add(RadListDataItem5)
            Me.ListControlThemes.Items.Add(RadListDataItem6)
            Me.ListControlThemes.Items.Add(RadListDataItem7)
            Me.ListControlThemes.Items.Add(RadListDataItem8)
            Me.ListControlThemes.Items.Add(RadListDataItem9)
            Me.ListControlThemes.Items.Add(RadListDataItem10)
            Me.ListControlThemes.Items.Add(RadListDataItem11)
            Me.ListControlThemes.Items.Add(RadListDataItem12)
            Me.ListControlThemes.Items.Add(RadListDataItem13)
            Me.ListControlThemes.Items.Add(RadListDataItem14)
            Me.ListControlThemes.Items.Add(RadListDataItem15)
            Me.ListControlThemes.Items.Add(RadListDataItem16)
            Me.ListControlThemes.Items.Add(RadListDataItem17)
            Me.ListControlThemes.Items.Add(RadListDataItem18)
            Me.ListControlThemes.Location = New System.Drawing.Point(0, 0)
            Me.ListControlThemes.Name = "ListControlThemes"
            Me.ListControlThemes.Size = New System.Drawing.Size(210, 326)
            Me.ListControlThemes.TabIndex = 0
            Me.ListControlThemes.Text = "radListControl1"
            '
            'themesLabel
            '
            Me.themesLabel.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
            Me.themesLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(66, Byte), Integer))
            Me.themesLabel.Location = New System.Drawing.Point(0, 0)
            Me.themesLabel.Name = "themesLabel"
            Me.themesLabel.Size = New System.Drawing.Size(64, 22)
            Me.themesLabel.TabIndex = 0
            Me.themesLabel.Text = "THEMES"
            '
            'radListView2
            '
            Me.radListView2.AllowEdit = False
            ListViewDataItem1.Key = "{ Total = ""Total:"", Price = $0.00}"
            ListViewDataItem1.Tag = "{ Total = ""Total:"", Price = $0.00}"
            ListViewDataItem1.Text = "ListViewItem 1"
            Me.radListView2.Items.AddRange(New Telerik.WinControls.UI.ListViewDataItem() {ListViewDataItem1})
            Me.radListView2.ItemSpacing = -1
            Me.radListView2.Location = New System.Drawing.Point(390, 258)
            Me.radListView2.Margin = New System.Windows.Forms.Padding(0, 0, 0, 5)
            Me.radListView2.Name = "radListView2"
            Me.radListView2.ShowColumnHeaders = False
            Me.radListView2.ShowGridLines = True
            Me.radListView2.Size = New System.Drawing.Size(193, 22)
            Me.radListView2.TabIndex = 23
            Me.radListView2.Text = "radListView2"
            Me.radListView2.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysHide
            Me.radListView2.ViewType = Telerik.WinControls.UI.ListViewType.DetailsView
            '
            'radLabel2
            '
            Me.radLabel2.Font = New System.Drawing.Font("Segoe UI", 14.5!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radLabel2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(87, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(82, Byte), Integer))
            Me.radLabel2.Location = New System.Drawing.Point(12, 12)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(267, 30)
            Me.radLabel2.TabIndex = 22
            Me.radLabel2.Text = "What would you like to order?"
            '
            'radCheckedListBox1
            '
            Me.radCheckedListBox1.CheckBoxesAlignment = Telerik.WinControls.UI.CheckBoxesAlignment.Near
            Me.radCheckedListBox1.ItemSize = New System.Drawing.Size(0, 92)
            Me.radCheckedListBox1.Location = New System.Drawing.Point(10, 46)
            Me.radCheckedListBox1.Margin = New System.Windows.Forms.Padding(5)
            Me.radCheckedListBox1.MultiSelect = True
            Me.radCheckedListBox1.Name = "radCheckedListBox1"
            Me.radCheckedListBox1.Size = New System.Drawing.Size(370, 420)
            Me.radCheckedListBox1.TabIndex = 16
            Me.radCheckedListBox1.Text = "radCheckedListBox1"
            '
            'radCheckSelectedButton
            '
            Me.radCheckSelectedButton.Location = New System.Drawing.Point(148, 476)
            Me.radCheckSelectedButton.Margin = New System.Windows.Forms.Padding(5)
            Me.radCheckSelectedButton.Name = "radCheckSelectedButton"
            Me.radCheckSelectedButton.Size = New System.Drawing.Size(126, 24)
            Me.radCheckSelectedButton.TabIndex = 21
            Me.radCheckSelectedButton.Text = "Check selected"
            '
            'radCheckAllButton
            '
            Me.radCheckAllButton.Location = New System.Drawing.Point(12, 476)
            Me.radCheckAllButton.Margin = New System.Windows.Forms.Padding(5)
            Me.radCheckAllButton.Name = "radCheckAllButton"
            Me.radCheckAllButton.Size = New System.Drawing.Size(126, 24)
            Me.radCheckAllButton.TabIndex = 20
            Me.radCheckAllButton.Text = "Check all"
            '
            'radClearAllButton
            '
            Me.radClearAllButton.Location = New System.Drawing.Point(390, 324)
            Me.radClearAllButton.Margin = New System.Windows.Forms.Padding(5)
            Me.radClearAllButton.Name = "radClearAllButton"
            Me.radClearAllButton.Size = New System.Drawing.Size(193, 24)
            Me.radClearAllButton.TabIndex = 19
            Me.radClearAllButton.Text = "Clear all"
            '
            'radOrderButton
            '
            Me.radOrderButton.Location = New System.Drawing.Point(390, 290)
            Me.radOrderButton.Margin = New System.Windows.Forms.Padding(5)
            Me.radOrderButton.Name = "radOrderButton"
            Me.radOrderButton.Size = New System.Drawing.Size(193, 24)
            Me.radOrderButton.TabIndex = 18
            Me.radOrderButton.Text = "Finish order"
            '
            'radListView1
            '
            Me.radListView1.AllowEdit = False
            Me.radListView1.ItemSpacing = -1
            Me.radListView1.Location = New System.Drawing.Point(390, 46)
            Me.radListView1.Margin = New System.Windows.Forms.Padding(5, 5, 5, 0)
            Me.radListView1.MultiSelect = True
            Me.radListView1.Name = "radListView1"
            Me.radListView1.ShowColumnHeaders = False
            Me.radListView1.ShowGridLines = True
            Me.radListView1.Size = New System.Drawing.Size(193, 212)
            Me.radListView1.TabIndex = 17
            Me.radListView1.Text = "radListView1"
            Me.radListView1.ViewType = Telerik.WinControls.UI.ListViewType.DetailsView
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radListView2)
            Me.Controls.Add(Me.radLabel2)
            Me.Controls.Add(Me.radCheckedListBox1)
            Me.Controls.Add(Me.radCheckSelectedButton)
            Me.Controls.Add(Me.radCheckAllButton)
            Me.Controls.Add(Me.radClearAllButton)
            Me.Controls.Add(Me.radOrderButton)
            Me.Controls.Add(Me.radListView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1540, 1000)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radListView1, 0)
            Me.Controls.SetChildIndex(Me.radOrderButton, 0)
            Me.Controls.SetChildIndex(Me.radClearAllButton, 0)
            Me.Controls.SetChildIndex(Me.radCheckAllButton, 0)
            Me.Controls.SetChildIndex(Me.radCheckSelectedButton, 0)
            Me.Controls.SetChildIndex(Me.radCheckedListBox1, 0)
            Me.Controls.SetChildIndex(Me.radLabel2, 0)
            Me.Controls.SetChildIndex(Me.radListView2, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.optionsLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ListControlThemes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.themesLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radListView2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckedListBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckSelectedButton, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckAllButton, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radClearAllButton, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radOrderButton, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radListView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents optionsLabel As Telerik.WinControls.UI.RadLabel
        Friend WithEvents ListControlThemes As Telerik.WinControls.UI.RadListControl
        Friend WithEvents themesLabel As Telerik.WinControls.UI.RadLabel
        Private WithEvents radListView2 As Telerik.WinControls.UI.RadListView
        Private WithEvents radLabel2 As Telerik.WinControls.UI.RadLabel
        Private WithEvents radCheckedListBox1 As Telerik.WinControls.UI.RadCheckedListBox
        Private WithEvents radCheckSelectedButton As Telerik.WinControls.UI.RadButton
        Private WithEvents radCheckAllButton As Telerik.WinControls.UI.RadButton
        Private WithEvents radClearAllButton As Telerik.WinControls.UI.RadButton
        Private WithEvents radOrderButton As Telerik.WinControls.UI.RadButton
        Private WithEvents radListView1 As Telerik.WinControls.UI.RadListView

#End Region

    End Class
End Namespace
