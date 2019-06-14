Namespace Telerik.Examples.WinControls.ListView.FirstLook
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
            Me.components = New System.ComponentModel.Container()
            Dim RadListDataItem1 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem2 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem3 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem4 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem5 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem6 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem7 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem8 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem9 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Me.radListView1 = New Telerik.WinControls.UI.RadListView()
            Me.carsRatesDataTableBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.sofiaCarRentalDataSet = New Telerik.Examples.WinControls.DataSources.SofiaCarRentalDataSet()
            Me.radCommandBar1 = New Telerik.WinControls.UI.RadCommandBar()
            Me.commandBarRowElement1 = New Telerik.WinControls.UI.CommandBarRowElement()
            Me.commandBarStripElement1 = New Telerik.WinControls.UI.CommandBarStripElement()
            Me.commandBarToggleList = New Telerik.WinControls.UI.CommandBarToggleButton()
            Me.commandBarToggleTiles = New Telerik.WinControls.UI.CommandBarToggleButton()
            Me.commandBarToggleDetails = New Telerik.WinControls.UI.CommandBarToggleButton()
            Me.CommandBarSeparator4 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.commandBarLabel1 = New Telerik.WinControls.UI.CommandBarLabel()
            Me.commandBarDropDownSort = New Telerik.WinControls.UI.CommandBarDropDownList()
            Me.commandBarSeparator1 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.commandBarLabel2 = New Telerik.WinControls.UI.CommandBarLabel()
            Me.commandBarDropDownGroup = New Telerik.WinControls.UI.CommandBarDropDownList()
            Me.commandBarSeparator2 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.commandBarSeparator3 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.CommandBarLabel3 = New Telerik.WinControls.UI.CommandBarLabel()
            Me.commandBarTextBoxFilter = New Telerik.WinControls.UI.CommandBarTextBox()
            Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
            Me.carsRatesDataTableTableAdapter = New Telerik.Examples.WinControls.DataSources.SofiaCarRentalDataSetTableAdapters.CarsRatesDataTableTableAdapter()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radListView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.carsRatesDataTableBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.sofiaCarRentalDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCommandBar1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Location = New System.Drawing.Point(1032, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 747)
            Me.settingsPanel.ThemeName = "ControlDefault"
            '
            'radListView1
            '
            Me.radListView1.DataSource = Me.carsRatesDataTableBindingSource
            Me.radListView1.DisplayMember = "Make"
            Me.radListView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radListView1.EnableColumnSort = True
            Me.radListView1.EnableSorting = True
            Me.radListView1.FullRowSelect = False
            Me.radListView1.GroupIndent = 0
            Me.radListView1.ItemSize = New System.Drawing.Size(64, 64)
            Me.radListView1.Location = New System.Drawing.Point(0, 55)
            Me.radListView1.Name = "radListView1"
            Me.radListView1.Padding = New System.Windows.Forms.Padding(0, 50, 0, 0)
            Me.radListView1.Size = New System.Drawing.Size(1449, 833)
            Me.radListView1.TabIndex = 0
            Me.radListView1.Text = "radListView1"
            Me.radListView1.ValueMember = "CarID"
            Me.radListView1.ViewType = Telerik.WinControls.UI.ListViewType.IconsView
            '
            'carsRatesDataTableBindingSource
            '
            Me.carsRatesDataTableBindingSource.DataMember = "CarsRatesDataTable"
            Me.carsRatesDataTableBindingSource.DataSource = Me.sofiaCarRentalDataSet
            '
            'sofiaCarRentalDataSet
            '
            Me.sofiaCarRentalDataSet.DataSetName = "SofiaCarRentalDataSet"
            Me.sofiaCarRentalDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'radCommandBar1
            '
            Me.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top
            Me.radCommandBar1.Location = New System.Drawing.Point(0, 0)
            Me.radCommandBar1.Name = "radCommandBar1"
            Me.radCommandBar1.Rows.AddRange(New Telerik.WinControls.UI.CommandBarRowElement() {Me.commandBarRowElement1})
            Me.radCommandBar1.Size = New System.Drawing.Size(1449, 55)
            Me.radCommandBar1.TabIndex = 4
            '
            'commandBarRowElement1
            '
            Me.commandBarRowElement1.DisplayName = Nothing
            Me.commandBarRowElement1.MinSize = New System.Drawing.Size(25, 25)
            Me.commandBarRowElement1.Strips.AddRange(New Telerik.WinControls.UI.CommandBarStripElement() {Me.commandBarStripElement1})
            '
            'commandBarStripElement1
            '
            Me.commandBarStripElement1.DisplayName = "commandBarStripElement1"
            Me.commandBarStripElement1.EnableDragging = False
            Me.commandBarStripElement1.Items.AddRange(New Telerik.WinControls.UI.RadCommandBarBaseItem() {Me.commandBarToggleList, Me.commandBarToggleTiles, Me.commandBarToggleDetails, Me.CommandBarSeparator4, Me.commandBarLabel1, Me.commandBarDropDownSort, Me.commandBarSeparator1, Me.commandBarLabel2, Me.commandBarDropDownGroup, Me.commandBarSeparator2, Me.commandBarSeparator3, Me.CommandBarLabel3, Me.commandBarTextBoxFilter})
            Me.commandBarStripElement1.Name = "commandBarStripElement1"
            Me.commandBarStripElement1.StretchHorizontally = True
            Me.commandBarStripElement1.Text = ""
            '
            'commandBarToggleList
            '
            Me.commandBarToggleList.AccessibleDescription = "commandBarToggleButton1"
            Me.commandBarToggleList.AccessibleName = "commandBarToggleButton1"
            Me.commandBarToggleList.DisplayName = "commandBarToggleButton1"
            Me.commandBarToggleList.Image = My.Resources.tiles_icon
            Me.commandBarToggleList.Name = "commandBarToggleList"
            Me.commandBarToggleList.Padding = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.commandBarToggleList.Text = "commandBarToggleButton1"
            Me.commandBarToggleList.ToolTipText = "ListView"
            Me.commandBarToggleList.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarToggleTiles
            '
            Me.commandBarToggleTiles.AccessibleDescription = "commandBarToggleButton2"
            Me.commandBarToggleTiles.AccessibleName = "commandBarToggleButton2"
            Me.commandBarToggleTiles.CheckState = System.Windows.Forms.CheckState.Checked
            Me.commandBarToggleTiles.DisplayName = "commandBarToggleButton2"
            Me.commandBarToggleTiles.Image = My.Resources.list_icon
            Me.commandBarToggleTiles.Name = "commandBarToggleTiles"
            Me.commandBarToggleTiles.Padding = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.commandBarToggleTiles.Text = "commandBarToggleButton2"
            Me.commandBarToggleTiles.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            Me.commandBarToggleTiles.ToolTipText = "IconsView"
            Me.commandBarToggleTiles.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarToggleDetails
            '
            Me.commandBarToggleDetails.AccessibleDescription = "commandBarToggleButton3"
            Me.commandBarToggleDetails.AccessibleName = "commandBarToggleButton3"
            Me.commandBarToggleDetails.DisplayName = "commandBarToggleButton3"
            Me.commandBarToggleDetails.Image = My.Resources.details_icon
            Me.commandBarToggleDetails.Name = "commandBarToggleDetails"
            Me.commandBarToggleDetails.Padding = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.commandBarToggleDetails.Text = "commandBarToggleButton3"
            Me.commandBarToggleDetails.ToolTipText = "DetailsView"
            Me.commandBarToggleDetails.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'CommandBarSeparator4
            '
            Me.CommandBarSeparator4.AccessibleDescription = "CommandBarSeparator4"
            Me.CommandBarSeparator4.AccessibleName = "CommandBarSeparator4"
            Me.CommandBarSeparator4.DisplayName = "CommandBarSeparator4"
            Me.CommandBarSeparator4.Name = "CommandBarSeparator4"
            Me.CommandBarSeparator4.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Me.CommandBarSeparator4.VisibleInOverflowMenu = False
            '
            'commandBarLabel1
            '
            Me.commandBarLabel1.AccessibleDescription = "Sort By:"
            Me.commandBarLabel1.AccessibleName = "Sort By:"
            Me.commandBarLabel1.DisplayName = "commandBarLabel1"
            Me.commandBarLabel1.Name = "commandBarLabel1"
            Me.commandBarLabel1.Text = "Sort By:"
            Me.commandBarLabel1.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarDropDownSort
            '
            Me.commandBarDropDownSort.AccessibleDescription = "None"
            Me.commandBarDropDownSort.AccessibleName = "None"
            Me.commandBarDropDownSort.DisplayName = "commandBarDropDownList1"
            Me.commandBarDropDownSort.DropDownAnimationEnabled = True
            Me.commandBarDropDownSort.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            RadListDataItem1.Text = "None"
            RadListDataItem1.TextWrap = True
            RadListDataItem2.Text = "Make"
            RadListDataItem2.TextWrap = True
            RadListDataItem3.Text = "Model"
            RadListDataItem3.TextWrap = True
            RadListDataItem4.Text = "Category"
            RadListDataItem4.TextWrap = True
            RadListDataItem5.Text = "Year"
            RadListDataItem5.TextWrap = True
            Me.commandBarDropDownSort.Items.Add(RadListDataItem1)
            Me.commandBarDropDownSort.Items.Add(RadListDataItem2)
            Me.commandBarDropDownSort.Items.Add(RadListDataItem3)
            Me.commandBarDropDownSort.Items.Add(RadListDataItem4)
            Me.commandBarDropDownSort.Items.Add(RadListDataItem5)
            Me.commandBarDropDownSort.MaxDropDownItems = 0
            Me.commandBarDropDownSort.Name = "commandBarDropDownSort"
            Me.commandBarDropDownSort.Text = "None"
            Me.commandBarDropDownSort.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarSeparator1
            '
            Me.commandBarSeparator1.AccessibleDescription = "commandBarSeparator1"
            Me.commandBarSeparator1.AccessibleName = "commandBarSeparator1"
            Me.commandBarSeparator1.DisplayName = "commandBarSeparator1"
            Me.commandBarSeparator1.Name = "commandBarSeparator1"
            Me.commandBarSeparator1.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Me.commandBarSeparator1.VisibleInOverflowMenu = False
            '
            'commandBarLabel2
            '
            Me.commandBarLabel2.AccessibleDescription = "Group By:"
            Me.commandBarLabel2.AccessibleName = "Group By:"
            Me.commandBarLabel2.DisplayName = "commandBarLabel2"
            Me.commandBarLabel2.Name = "commandBarLabel2"
            Me.commandBarLabel2.Text = "Group By:"
            Me.commandBarLabel2.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarDropDownGroup
            '
            Me.commandBarDropDownGroup.DisplayName = "commandBarDropDownList2"
            Me.commandBarDropDownGroup.DropDownAnimationEnabled = True
            Me.commandBarDropDownGroup.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            RadListDataItem6.Text = "None"
            RadListDataItem6.TextWrap = True
            RadListDataItem7.Text = "Make"
            RadListDataItem7.TextWrap = True
            RadListDataItem8.Text = "Category"
            RadListDataItem8.TextWrap = True
            RadListDataItem9.Text = "Year"
            RadListDataItem9.TextWrap = True
            Me.commandBarDropDownGroup.Items.Add(RadListDataItem6)
            Me.commandBarDropDownGroup.Items.Add(RadListDataItem7)
            Me.commandBarDropDownGroup.Items.Add(RadListDataItem8)
            Me.commandBarDropDownGroup.Items.Add(RadListDataItem9)
            Me.commandBarDropDownGroup.MaxDropDownItems = 0
            Me.commandBarDropDownGroup.Name = "commandBarDropDownGroup"
            Me.commandBarDropDownGroup.Text = ""
            Me.commandBarDropDownGroup.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarSeparator2
            '
            Me.commandBarSeparator2.AccessibleDescription = "commandBarSeparator2"
            Me.commandBarSeparator2.AccessibleName = "commandBarSeparator2"
            Me.commandBarSeparator2.DisplayName = "commandBarSeparator2"
            Me.commandBarSeparator2.Name = "commandBarSeparator2"
            Me.commandBarSeparator2.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Me.commandBarSeparator2.VisibleInOverflowMenu = False
            '
            'commandBarSeparator3
            '
            Me.commandBarSeparator3.AccessibleDescription = "commandBarSeparator3"
            Me.commandBarSeparator3.AccessibleName = "commandBarSeparator3"
            Me.commandBarSeparator3.DisplayName = "commandBarSeparator3"
            Me.commandBarSeparator3.Name = "commandBarSeparator3"
            Me.commandBarSeparator3.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Me.commandBarSeparator3.VisibleInOverflowMenu = False
            '
            'CommandBarLabel3
            '
            Me.CommandBarLabel3.AccessibleDescription = "Filter:"
            Me.CommandBarLabel3.AccessibleName = "Filter:"
            Me.CommandBarLabel3.DisplayName = "CommandBarLabel3"
            Me.CommandBarLabel3.Name = "CommandBarLabel3"
            Me.CommandBarLabel3.Text = "Filter:"
            Me.CommandBarLabel3.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'commandBarTextBoxFilter
            '
            Me.commandBarTextBoxFilter.DisplayName = "commandBarTextBox1"
            Me.commandBarTextBoxFilter.MinSize = New System.Drawing.Size(200, 0)
            Me.commandBarTextBoxFilter.Name = "commandBarTextBoxFilter"
            Me.commandBarTextBoxFilter.StretchHorizontally = True
            Me.commandBarTextBoxFilter.Text = ""
            Me.commandBarTextBoxFilter.Visibility = Telerik.WinControls.ElementVisibility.Visible
            '
            'radPanel1
            '
            Me.radPanel1.Controls.Add(Me.radListView1)
            Me.radPanel1.Controls.Add(Me.radCommandBar1)
            Me.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPanel1.Location = New System.Drawing.Point(0, 0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.Size = New System.Drawing.Size(1449, 888)
            Me.radPanel1.TabIndex = 5
            Me.radPanel1.Text = "radPanel1"
            '
            'carsRatesDataTableTableAdapter
            '
            Me.carsRatesDataTableTableAdapter.ClearBeforeFill = True
            '
            'Form1
            '
            Me.Controls.Add(Me.radPanel1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1459, 898)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radPanel1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radListView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.carsRatesDataTableBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.sofiaCarRentalDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCommandBar1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel1.ResumeLayout(False)
            Me.radPanel1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

		#End Region

        WithEvents radListView1 As Telerik.WinControls.UI.RadListView
		Private radCommandBar1 As Telerik.WinControls.UI.RadCommandBar
		Private commandBarRowElement1 As Telerik.WinControls.UI.CommandBarRowElement
		Private commandBarStripElement1 As Telerik.WinControls.UI.CommandBarStripElement
		Private commandBarLabel1 As Telerik.WinControls.UI.CommandBarLabel
		Private commandBarDropDownSort As Telerik.WinControls.UI.CommandBarDropDownList
		Private commandBarSeparator1 As Telerik.WinControls.UI.CommandBarSeparator
		Private commandBarLabel2 As Telerik.WinControls.UI.CommandBarLabel
		Private commandBarDropDownGroup As Telerik.WinControls.UI.CommandBarDropDownList
		Private commandBarSeparator2 As Telerik.WinControls.UI.CommandBarSeparator
		Private commandBarTextBoxFilter As Telerik.WinControls.UI.CommandBarTextBox
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		Private commandBarToggleList As Telerik.WinControls.UI.CommandBarToggleButton
		Private commandBarToggleTiles As Telerik.WinControls.UI.CommandBarToggleButton
		Private commandBarToggleDetails As Telerik.WinControls.UI.CommandBarToggleButton
		Private commandBarSeparator3 As Telerik.WinControls.UI.CommandBarSeparator
		Private sofiaCarRentalDataSet As DataSources.SofiaCarRentalDataSet
		Private carsRatesDataTableBindingSource As BindingSource
        Private carsRatesDataTableTableAdapter As DataSources.SofiaCarRentalDataSetTableAdapters.CarsRatesDataTableTableAdapter
        Friend WithEvents CommandBarSeparator4 As Telerik.WinControls.UI.CommandBarSeparator
        Friend WithEvents CommandBarLabel3 As Telerik.WinControls.UI.CommandBarLabel
	End Class
End Namespace