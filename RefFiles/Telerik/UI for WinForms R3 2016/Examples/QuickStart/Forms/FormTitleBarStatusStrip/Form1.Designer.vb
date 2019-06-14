Namespace Telerik.Examples.WinControls.Forms.FormTitleBarStatusStrip
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
            Me.components = New System.ComponentModel.Container()
            Dim gridViewImageColumn1 As New Telerik.WinControls.UI.GridViewImageColumn()
            Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn2 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn3 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewDateTimeColumn1 As New Telerik.WinControls.UI.GridViewDateTimeColumn()
            Dim gridViewTextBoxColumn4 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn5 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim tableViewDefinition1 As New Telerik.WinControls.UI.TableViewDefinition()
            Me.radStatusBar1 = New Telerik.WinControls.UI.RadStatusStrip()
            Me.radButtonElement1 = New Telerik.WinControls.UI.RadButtonElement()
            Me.CommandBarSeparator3 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.radLabelElement3 = New Telerik.WinControls.UI.RadLabelElement()
            Me.CommandBarSeparator4 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.radLabelElement2 = New Telerik.WinControls.UI.RadLabelElement()
            Me.radProgressBarElement1 = New Telerik.WinControls.UI.RadProgressBarElement()
            Me.CommandBarSeparator5 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.radCheckBoxElement1 = New Telerik.WinControls.UI.RadCheckBoxElement()
            Me.CommandBarSeparator1 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.radLabelElement1 = New Telerik.WinControls.UI.RadLabelElement()
            Me.CommandBarSeparator2 = New Telerik.WinControls.UI.CommandBarSeparator()
            Me.radMenuItem1 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem2 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem9 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem10 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem3 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem4 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem5 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuSeparatorItem1 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
            Me.radMenuItem6 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem7 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem8 = New Telerik.WinControls.UI.RadMenuItem()
            Me.timer1 = New System.Windows.Forms.Timer(Me.components)
            Me.radMenuItem21 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem22 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem26 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem12 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem13 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radMenuItem17 = New Telerik.WinControls.UI.RadMenuItem()
            Me.radButton3 = New Telerik.WinControls.UI.RadButton()
            Me.radButton2 = New Telerik.WinControls.UI.RadButton()
            Me.radButton1 = New Telerik.WinControls.UI.RadButton()
            Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
            Me.employeesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.northwindDataSet = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
            Me.employeesTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter()
            Me.radMenu1 = New Telerik.WinControls.UI.RadMenu()
            Me.radPageView1 = New Telerik.WinControls.UI.RadPageView()
            Me.radPageViewPage1 = New Telerik.WinControls.UI.RadPageViewPage()
            Me.radPageViewPage2 = New Telerik.WinControls.UI.RadPageViewPage()
            Me.radCheckBox3 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
            Me.radPageViewPage3 = New Telerik.WinControls.UI.RadPageViewPage()
            Me.radRadioButton3 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton2 = New Telerik.WinControls.UI.RadRadioButton()
            Me.radRadioButton1 = New Telerik.WinControls.UI.RadRadioButton()
            DirectCast(Me.radStatusBar1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radButton3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radButton2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.employeesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radPageView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPageView1.SuspendLayout()
            Me.radPageViewPage1.SuspendLayout()
            Me.radPageViewPage2.SuspendLayout()
            DirectCast(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPageViewPage3.SuspendLayout()
            DirectCast(Me.radRadioButton3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radRadioButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radStatusBar1
            ' 
            Me.radStatusBar1.Items.AddRange(New Telerik.WinControls.RadItem() {Me.radButtonElement1, Me.CommandBarSeparator3, Me.radLabelElement3, Me.CommandBarSeparator4, Me.radLabelElement2, Me.radProgressBarElement1, _
                Me.CommandBarSeparator5, Me.radCheckBoxElement1, Me.CommandBarSeparator1, Me.radLabelElement1, Me.CommandBarSeparator2})
            Me.radStatusBar1.Location = New System.Drawing.Point(0, 427)
            Me.radStatusBar1.Margin = New System.Windows.Forms.Padding(5)
            Me.radStatusBar1.Name = "radStatusBar1"
            Me.radStatusBar1.Size = New System.Drawing.Size(676, 28)
            Me.radStatusBar1.TabIndex = 0
            Me.radStatusBar1.Text = "radStatusBar1"
            AddHandler Me.radStatusBar1.HelpRequested, AddressOf Me.radStatusBar1_HelpRequested
            DirectCast(Me.radStatusBar1.GetChildAt(0), Telerik.WinControls.UI.RadStatusBarElement).Text = "radStatusBar1"
            DirectCast(Me.radStatusBar1.GetChildAt(0), Telerik.WinControls.UI.RadStatusBarElement).Padding = New System.Windows.Forms.Padding(2)
            DirectCast(Me.radStatusBar1.GetChildAt(0), Telerik.WinControls.UI.RadStatusBarElement).MinSize = New System.Drawing.Size(0, 19)
            DirectCast(Me.radStatusBar1.GetChildAt(0).GetChildAt(3), Telerik.WinControls.UI.StatusBarBoxLayout).Margin = New System.Windows.Forms.Padding(0, 0, 14, 0)
            ' 
            ' radButtonElement1
            ' 
            Me.radButtonElement1.CanFocus = True
            Me.radButtonElement1.Margin = New System.Windows.Forms.Padding(1)
            Me.radButtonElement1.Name = "radButtonElement1"
            Me.radStatusBar1.SetSpring(Me.radButtonElement1, False)
            Me.radButtonElement1.Text = "Page 1 of 1"
            ' 
            ' CommandBarSeparator3
            ' 
            Me.CommandBarSeparator3.AccessibleDescription = "CommandBarSeparator3"
            Me.CommandBarSeparator3.AccessibleName = "CommandBarSeparator3"
            Me.CommandBarSeparator3.Margin = New System.Windows.Forms.Padding(1)
            Me.CommandBarSeparator3.Name = "CommandBarSeparator3"
            Me.radStatusBar1.SetSpring(Me.CommandBarSeparator3, False)
            Me.CommandBarSeparator3.VisibleInOverflowMenu = False
            ' 
            ' radLabelElement3
            ' 
            Me.radLabelElement3.ForeColor = System.Drawing.Color.FromArgb(CInt(CByte(165)), CInt(CByte(165)), CInt(CByte(165)))
            Me.radLabelElement3.Margin = New System.Windows.Forms.Padding(1)
            Me.radLabelElement3.Name = "radLabelElement3"
            Me.radStatusBar1.SetSpring(Me.radLabelElement3, False)
            Me.radLabelElement3.Text = "Words: 2"
            Me.radLabelElement3.TextWrap = True
            ' 
            ' CommandBarSeparator4
            ' 
            Me.CommandBarSeparator4.AccessibleDescription = "CommandBarSeparator4"
            Me.CommandBarSeparator4.AccessibleName = "CommandBarSeparator4"
            Me.CommandBarSeparator4.Margin = New System.Windows.Forms.Padding(1)
            Me.CommandBarSeparator4.Name = "CommandBarSeparator4"
            Me.radStatusBar1.SetSpring(Me.CommandBarSeparator4, False)
            Me.CommandBarSeparator4.VisibleInOverflowMenu = False
            ' 
            ' radLabelElement2
            ' 
            Me.radLabelElement2.Margin = New System.Windows.Forms.Padding(1)
            Me.radLabelElement2.Name = "radLabelElement2"
            Me.radStatusBar1.SetSpring(Me.radLabelElement2, False)
            Me.radLabelElement2.Text = "Saving..."
            Me.radLabelElement2.TextWrap = True
            ' 
            ' radProgressBarElement1
            ' 
            Me.radProgressBarElement1.AutoSize = False
            Me.radProgressBarElement1.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize
            Me.radProgressBarElement1.Bounds = New System.Drawing.Rectangle(0, 0, 80, 20)
            Me.radProgressBarElement1.ClipDrawing = True
            Me.radProgressBarElement1.DefaultSize = New System.Drawing.Size(80, 16)
            Me.radProgressBarElement1.Margin = New System.Windows.Forms.Padding(1)
            Me.radProgressBarElement1.Name = "radProgressBarElement1"
            Me.radProgressBarElement1.SeparatorColor1 = System.Drawing.Color.White
            Me.radProgressBarElement1.SeparatorColor2 = System.Drawing.Color.White
            Me.radProgressBarElement1.SeparatorColor3 = System.Drawing.Color.White
            Me.radProgressBarElement1.SeparatorColor4 = System.Drawing.Color.White
            Me.radProgressBarElement1.SeparatorGradientAngle = 0
            Me.radProgressBarElement1.SeparatorGradientPercentage1 = 0.4F
            Me.radProgressBarElement1.SeparatorGradientPercentage2 = 0.6F
            Me.radProgressBarElement1.SeparatorNumberOfColors = 2
            Me.radStatusBar1.SetSpring(Me.radProgressBarElement1, False)
            Me.radProgressBarElement1.[Step] = 1
            Me.radProgressBarElement1.StepWidth = 1
            Me.radProgressBarElement1.SweepAngle = 90
            ' 
            ' CommandBarSeparator5
            ' 
            Me.CommandBarSeparator5.AccessibleDescription = "CommandBarSeparator5"
            Me.CommandBarSeparator5.AccessibleName = "CommandBarSeparator5"
            Me.CommandBarSeparator5.Margin = New System.Windows.Forms.Padding(1)
            Me.CommandBarSeparator5.Name = "CommandBarSeparator5"
            Me.radStatusBar1.SetSpring(Me.CommandBarSeparator5, False)
            Me.CommandBarSeparator5.VisibleInOverflowMenu = False
            ' 
            ' radCheckBoxElement1
            ' 
            Me.radCheckBoxElement1.CanFocus = True
            Me.radCheckBoxElement1.Checked = False
            Me.radCheckBoxElement1.Margin = New System.Windows.Forms.Padding(1)
            Me.radCheckBoxElement1.Name = "radCheckBoxElement1"
            Me.radCheckBoxElement1.[ReadOnly] = False
            Me.radCheckBoxElement1.ShowBorder = False
            Me.radStatusBar1.SetSpring(Me.radCheckBoxElement1, False)
            Me.radCheckBoxElement1.Text = "Check for errors"
            ' 
            ' CommandBarSeparator1
            ' 
            Me.CommandBarSeparator1.AccessibleDescription = "CommandBarSeparator1"
            Me.CommandBarSeparator1.AccessibleName = "CommandBarSeparator1"
            Me.CommandBarSeparator1.Margin = New System.Windows.Forms.Padding(1)
            Me.CommandBarSeparator1.Name = "CommandBarSeparator1"
            Me.radStatusBar1.SetSpring(Me.CommandBarSeparator1, False)
            Me.CommandBarSeparator1.VisibleInOverflowMenu = False
            ' 
            ' radLabelElement1
            ' 
            Me.radLabelElement1.Margin = New System.Windows.Forms.Padding(1)
            Me.radLabelElement1.Name = "radLabelElement1"
            Me.radStatusBar1.SetSpring(Me.radLabelElement1, False)
            Me.radLabelElement1.Text = "English (US)"
            Me.radLabelElement1.TextWrap = True
            ' 
            ' CommandBarSeparator2
            ' 
            Me.CommandBarSeparator2.AccessibleDescription = "CommandBarSeparator2"
            Me.CommandBarSeparator2.AccessibleName = "CommandBarSeparator2"
            Me.CommandBarSeparator2.Margin = New System.Windows.Forms.Padding(1)
            Me.CommandBarSeparator2.Name = "CommandBarSeparator2"
            Me.radStatusBar1.SetSpring(Me.CommandBarSeparator2, False)
            Me.CommandBarSeparator2.VisibleInOverflowMenu = False
            ' 
            ' radMenuItem1
            ' 
            Me.radMenuItem1.Name = "radMenuItem1"
            Me.radMenuItem1.Text = "50%"
            Me.radMenuItem1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem2
            ' 
            Me.radMenuItem2.Name = "radMenuItem2"
            Me.radMenuItem2.Text = "75%"
            Me.radMenuItem2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem9
            ' 
            Me.radMenuItem9.Name = "radMenuItem9"
            Me.radMenuItem9.Text = "100%"
            Me.radMenuItem9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem10
            ' 
            Me.radMenuItem10.Name = "radMenuItem10"
            Me.radMenuItem10.Text = "200%"
            Me.radMenuItem10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem3
            ' 
            Me.radMenuItem3.Name = "radMenuItem3"
            Me.radMenuItem3.Text = "radMenuItem3"
            Me.radMenuItem3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem4
            ' 
            Me.radMenuItem4.Name = "radMenuItem4"
            Me.radMenuItem4.Text = "radMenuItem4"
            Me.radMenuItem4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem5
            ' 
            Me.radMenuItem5.Name = "radMenuItem5"
            Me.radMenuItem5.Text = "radMenuItem5"
            Me.radMenuItem5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuSeparatorItem1
            ' 
            Me.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1"
            Me.radMenuSeparatorItem1.Text = "radMenuSeparatorItem1"
            Me.radMenuSeparatorItem1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            ' 
            ' radMenuItem6
            ' 
            Me.radMenuItem6.Name = "radMenuItem6"
            Me.radMenuItem6.Text = "radMenuItem6"
            Me.radMenuItem6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem7
            ' 
            Me.radMenuItem7.Name = "radMenuItem7"
            Me.radMenuItem7.Text = "radMenuItem7"
            Me.radMenuItem7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem8
            ' 
            Me.radMenuItem8.Name = "radMenuItem8"
            Me.radMenuItem8.Text = "radMenuItem8"
            Me.radMenuItem8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' timer1
            ' 
            Me.timer1.Enabled = True
            Me.timer1.Interval = 25
            ' 
            ' radMenuItem21
            ' 
            Me.radMenuItem21.Items.AddRange(New Telerik.WinControls.RadItem() {Me.radMenuItem22, Me.radMenuItem26, Me.radMenuItem12})
            Me.radMenuItem21.Name = "radMenuItem21"
            Me.radMenuItem21.Text = "Change Theme"
            ' 
            ' radMenuItem22
            ' 
            Me.radMenuItem22.Name = "radMenuItem22"
            Me.radMenuItem22.Tag = "ControlDefault"
            Me.radMenuItem22.Text = "Office2010 Blue"
            ' 
            ' radMenuItem26
            ' 
            Me.radMenuItem26.Name = "radMenuItem26"
            Me.radMenuItem26.Tag = "Desert"
            Me.radMenuItem26.Text = "Desert"
            ' 
            ' radMenuItem12
            ' 
            Me.radMenuItem12.IsChecked = True
            Me.radMenuItem12.Name = "radMenuItem12"
            Me.radMenuItem12.Tag = "TelerikMetro"
            Me.radMenuItem12.Text = "TelerikMetro"
            Me.radMenuItem12.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            ' 
            ' radMenuItem13
            ' 
            Me.radMenuItem13.ClickMode = Telerik.WinControls.ClickMode.Press
            Me.radMenuItem13.Items.AddRange(New Telerik.WinControls.RadItem() {Me.radMenuItem17})
            Me.radMenuItem13.Name = "radMenuItem13"
            Me.radMenuItem13.Text = "About"
            Me.radMenuItem13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radMenuItem17
            ' 
            Me.radMenuItem17.Name = "radMenuItem17"
            Me.radMenuItem17.Padding = New System.Windows.Forms.Padding(3, 1, 3, 1)
            Me.radMenuItem17.Text = "Telerik UI for WinForms"
            Me.radMenuItem17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            ' 
            ' radButton3
            ' 
            Me.radButton3.Location = New System.Drawing.Point(2, 59)
            Me.radButton3.Name = "radButton3"
            Me.radButton3.Size = New System.Drawing.Size(124, 23)
            Me.radButton3.TabIndex = 0
            Me.radButton3.Text = "RadButton3"
            AddHandler Me.radButton3.HelpRequested, AddressOf Me.Button_HelpRequested
            ' 
            ' radButton2
            ' 
            Me.radButton2.Location = New System.Drawing.Point(2, 30)
            Me.radButton2.Name = "radButton2"
            Me.radButton2.Size = New System.Drawing.Size(124, 23)
            Me.radButton2.TabIndex = 0
            Me.radButton2.Text = "RadButton2"
            AddHandler Me.radButton2.HelpRequested, AddressOf Me.Button_HelpRequested
            ' 
            ' radButton1
            ' 
            Me.radButton1.Location = New System.Drawing.Point(2, 1)
            Me.radButton1.Name = "radButton1"
            Me.radButton1.Size = New System.Drawing.Size(124, 23)
            Me.radButton1.TabIndex = 0
            Me.radButton1.Text = "RadButton1"
            AddHandler Me.radButton1.HelpRequested, AddressOf Me.Button_HelpRequested
            ' 
            ' radGridView1
            ' 
            Me.radGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
            Me.radGridView1.Location = New System.Drawing.Point(12, 30)
            ' 
            ' 
            ' 
            Me.radGridView1.MasterTemplate.AllowAddNewRow = False
            Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
            gridViewImageColumn1.DataType = GetType(Byte())
            gridViewImageColumn1.FieldName = "Photo"
            gridViewImageColumn1.HeaderText = "Photo"
            gridViewImageColumn1.ImageLayout = System.Windows.Forms.ImageLayout.Zoom
            gridViewImageColumn1.IsAutoGenerated = True
            gridViewImageColumn1.Name = "Photo"
            gridViewImageColumn1.Width = 54
            gridViewTextBoxColumn1.FieldName = "LastName"
            gridViewTextBoxColumn1.HeaderText = "Last Name"
            gridViewTextBoxColumn1.IsAutoGenerated = True
            gridViewTextBoxColumn1.Name = "LastName"
            gridViewTextBoxColumn1.Width = 81
            gridViewTextBoxColumn2.FieldName = "FirstName"
            gridViewTextBoxColumn2.HeaderText = "First Name"
            gridViewTextBoxColumn2.IsAutoGenerated = True
            gridViewTextBoxColumn2.Name = "FirstName"
            gridViewTextBoxColumn2.Width = 81
            gridViewTextBoxColumn3.FieldName = "Title"
            gridViewTextBoxColumn3.HeaderText = "Title"
            gridViewTextBoxColumn3.IsAutoGenerated = True
            gridViewTextBoxColumn3.Name = "Title"
            gridViewTextBoxColumn3.Width = 74
            gridViewDateTimeColumn1.CustomFormat = ""
            gridViewDateTimeColumn1.FieldName = "HireDate"
            gridViewDateTimeColumn1.Format = System.Windows.Forms.DateTimePickerFormat.[Custom]
            gridViewDateTimeColumn1.FormatString = "{0:M\/d\/yyyy}"
            gridViewDateTimeColumn1.HeaderText = "Hire Date"
            gridViewDateTimeColumn1.IsAutoGenerated = True
            gridViewDateTimeColumn1.Name = "HireDate"
            gridViewDateTimeColumn1.Width = 72
            gridViewTextBoxColumn4.FieldName = "City"
            gridViewTextBoxColumn4.HeaderText = "City"
            gridViewTextBoxColumn4.IsAutoGenerated = True
            gridViewTextBoxColumn4.Name = "City"
            gridViewTextBoxColumn4.Width = 51
            gridViewTextBoxColumn5.FieldName = "Country"
            gridViewTextBoxColumn5.HeaderText = "Country"
            gridViewTextBoxColumn5.IsAutoGenerated = True
            gridViewTextBoxColumn5.Name = "Country"
            gridViewTextBoxColumn5.Width = 65
            Me.radGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {gridViewImageColumn1, gridViewTextBoxColumn1, gridViewTextBoxColumn2, gridViewTextBoxColumn3, gridViewDateTimeColumn1, gridViewTextBoxColumn4, _
                gridViewTextBoxColumn5})
            Me.radGridView1.MasterTemplate.DataSource = Me.employeesBindingSource
            Me.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1
            Me.radGridView1.Name = "radGridView1"
            Me.radGridView1.Size = New System.Drawing.Size(492, 380)
            Me.radGridView1.TabIndex = 5
            AddHandler Me.radGridView1.HelpRequested, AddressOf Me.radGridView1_HelpRequested
            ' 
            ' employeesBindingSource
            ' 
            Me.employeesBindingSource.DataMember = "Employees"
            Me.employeesBindingSource.DataSource = Me.northwindDataSet
            ' 
            ' northwindDataSet
            ' 
            Me.northwindDataSet.DataSetName = "NorthwindDataSet"
            Me.northwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            ' 
            ' employeesTableAdapter
            ' 
            Me.employeesTableAdapter.ClearBeforeFill = True
            ' 
            ' radMenu1
            ' 
            Me.radMenu1.AllowMerge = False
            Me.radMenu1.BackColor = System.Drawing.Color.Transparent
            Me.radMenu1.Items.AddRange(New Telerik.WinControls.RadItem() {Me.radMenuItem21, Me.radMenuItem13})
            Me.radMenu1.Location = New System.Drawing.Point(0, 0)
            Me.radMenu1.Name = "radMenu1"
            ' 
            ' 
            ' 
            Me.radMenu1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radMenu1.Size = New System.Drawing.Size(676, 20)
            Me.radMenu1.TabIndex = 3
            Me.radMenu1.Text = "radMenu1"
            AddHandler Me.radMenu1.HelpRequested, AddressOf Me.radMenu1_HelpRequested
            ' 
            ' radPageView1
            ' 
            Me.radPageView1.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
            Me.radPageView1.Controls.Add(Me.radPageViewPage1)
            Me.radPageView1.Controls.Add(Me.radPageViewPage2)
            Me.radPageView1.Controls.Add(Me.radPageViewPage3)
            Me.radPageView1.ItemSizeMode = Telerik.WinControls.UI.PageViewItemSizeMode.EqualWidth Or Telerik.WinControls.UI.PageViewItemSizeMode.EqualHeight
            Me.radPageView1.Location = New System.Drawing.Point(515, 30)
            Me.radPageView1.Name = "radPageView1"
            Me.radPageView1.SelectedPage = Me.radPageViewPage3
            Me.radPageView1.Size = New System.Drawing.Size(148, 301)
            Me.radPageView1.TabIndex = 6
            Me.radPageView1.Text = "radPanelBar1"
            Me.radPageView1.ViewMode = Telerik.WinControls.UI.PageViewMode.Stack
            AddHandler Me.radPageView1.HelpRequested, AddressOf Me.radPageView1_HelpRequested
            ' 
            ' radPageViewPage1
            ' 
            Me.radPageViewPage1.Controls.Add(Me.radButton3)
            Me.radPageViewPage1.Controls.Add(Me.radButton2)
            Me.radPageViewPage1.Controls.Add(Me.radButton1)
            Me.radPageViewPage1.ItemSize = New System.Drawing.SizeF(148.0F, 32.0F)
            Me.radPageViewPage1.Location = New System.Drawing.Point(5, 29)
            Me.radPageViewPage1.Name = "radPageViewPage1"
            Me.radPageViewPage1.Size = New System.Drawing.Size(138, 175)
            Me.radPageViewPage1.Text = "Buttons"
            ' 
            ' radPageViewPage2
            ' 
            Me.radPageViewPage2.Controls.Add(Me.radCheckBox3)
            Me.radPageViewPage2.Controls.Add(Me.radCheckBox2)
            Me.radPageViewPage2.Controls.Add(Me.radCheckBox1)
            Me.radPageViewPage2.ItemSize = New System.Drawing.SizeF(148.0F, 32.0F)
            Me.radPageViewPage2.Location = New System.Drawing.Point(5, 29)
            Me.radPageViewPage2.Name = "radPageViewPage2"
            Me.radPageViewPage2.Size = New System.Drawing.Size(138, 175)
            Me.radPageViewPage2.Text = "Check Boxes"
            ' 
            ' radCheckBox3
            ' 
            Me.radCheckBox3.Location = New System.Drawing.Point(3, 51)
            Me.radCheckBox3.Name = "radCheckBox3"
            Me.radCheckBox3.Size = New System.Drawing.Size(91, 18)
            Me.radCheckBox3.TabIndex = 2
            Me.radCheckBox3.Text = "radCheckBox3"
            AddHandler Me.radCheckBox3.HelpRequested, AddressOf Me.CheckBox_HelpRequested
            ' 
            ' radCheckBox2
            ' 
            Me.radCheckBox2.Location = New System.Drawing.Point(3, 27)
            Me.radCheckBox2.Name = "radCheckBox2"
            Me.radCheckBox2.Size = New System.Drawing.Size(91, 18)
            Me.radCheckBox2.TabIndex = 1
            Me.radCheckBox2.Text = "radCheckBox2"
            AddHandler Me.radCheckBox2.HelpRequested, AddressOf Me.CheckBox_HelpRequested
            ' 
            ' radCheckBox1
            ' 
            Me.radCheckBox1.Location = New System.Drawing.Point(3, 3)
            Me.radCheckBox1.Name = "radCheckBox1"
            Me.radCheckBox1.Size = New System.Drawing.Size(91, 18)
            Me.radCheckBox1.TabIndex = 0
            Me.radCheckBox1.Text = "radCheckBox1"
            AddHandler Me.radCheckBox1.HelpRequested, AddressOf Me.CheckBox_HelpRequested
            ' 
            ' radPageViewPage3
            ' 
            Me.radPageViewPage3.Controls.Add(Me.radRadioButton3)
            Me.radPageViewPage3.Controls.Add(Me.radRadioButton2)
            Me.radPageViewPage3.Controls.Add(Me.radRadioButton1)
            Me.radPageViewPage3.ItemSize = New System.Drawing.SizeF(148.0F, 32.0F)
            Me.radPageViewPage3.Location = New System.Drawing.Point(5, 29)
            Me.radPageViewPage3.Name = "radPageViewPage3"
            Me.radPageViewPage3.Size = New System.Drawing.Size(138, 175)
            Me.radPageViewPage3.Text = "Radio Buttons"
            ' 
            ' radRadioButton3
            ' 
            Me.radRadioButton3.Location = New System.Drawing.Point(4, 53)
            Me.radRadioButton3.Name = "radRadioButton3"
            Me.radRadioButton3.Size = New System.Drawing.Size(105, 18)
            Me.radRadioButton3.TabIndex = 2
            Me.radRadioButton3.Text = "radRadioButton3"
            AddHandler Me.radRadioButton3.HelpRequested, AddressOf Me.RadioButton_HelpRequested
            ' 
            ' radRadioButton2
            ' 
            Me.radRadioButton2.Location = New System.Drawing.Point(4, 29)
            Me.radRadioButton2.Name = "radRadioButton2"
            Me.radRadioButton2.Size = New System.Drawing.Size(105, 18)
            Me.radRadioButton2.TabIndex = 1
            Me.radRadioButton2.Text = "radRadioButton2"
            AddHandler Me.radRadioButton2.HelpRequested, AddressOf Me.RadioButton_HelpRequested
            ' 
            ' radRadioButton1
            ' 
            Me.radRadioButton1.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radRadioButton1.Location = New System.Drawing.Point(4, 4)
            Me.radRadioButton1.Name = "radRadioButton1"
            Me.radRadioButton1.Size = New System.Drawing.Size(105, 18)
            Me.radRadioButton1.TabIndex = 0
            Me.radRadioButton1.Text = "radRadioButton1"
            Me.radRadioButton1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            AddHandler Me.radRadioButton1.HelpRequested, AddressOf Me.RadioButton_HelpRequested
            ' 
            ' Form1
            ' 
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
            Me.AutoScroll = True
            Me.ClientSize = New System.Drawing.Size(676, 455)
            Me.Controls.Add(Me.radGridView1)
            Me.Controls.Add(Me.radMenu1)
            Me.Controls.Add(Me.radStatusBar1)
            Me.Controls.Add(Me.radPageView1)
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "RadForm Example"
            Me.ThemeName = "TelerikMetro"
            DirectCast(Me.radStatusBar1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radButton3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radButton2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.employeesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radMenu1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radPageView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPageView1.ResumeLayout(False)
            Me.radPageViewPage1.ResumeLayout(False)
            Me.radPageViewPage2.ResumeLayout(False)
            Me.radPageViewPage2.PerformLayout()
            DirectCast(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPageViewPage3.ResumeLayout(False)
            Me.radPageViewPage3.PerformLayout()
            DirectCast(Me.radRadioButton3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radRadioButton1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private radStatusBar1 As Telerik.WinControls.UI.RadStatusStrip
        Private radButtonElement1 As Telerik.WinControls.UI.RadButtonElement
        Private radProgressBarElement1 As Telerik.WinControls.UI.RadProgressBarElement
        Private CommandBarSeparator2 As Telerik.WinControls.UI.CommandBarSeparator
        Private timer1 As System.Windows.Forms.Timer
        Private radMenuItem1 As Telerik.WinControls.UI.RadMenuItem
        Private radLabelElement1 As Telerik.WinControls.UI.RadLabelElement
        Private CommandBarSeparator5 As Telerik.WinControls.UI.CommandBarSeparator
        Private radMenuItem2 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem3 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem4 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem5 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuSeparatorItem1 As Telerik.WinControls.UI.RadMenuSeparatorItem
        Private radMenuItem6 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem7 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem8 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem9 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem10 As Telerik.WinControls.UI.RadMenuItem
        Private radLabelElement2 As Telerik.WinControls.UI.RadLabelElement
        Private CommandBarSeparator3 As Telerik.WinControls.UI.CommandBarSeparator
        Private radLabelElement3 As Telerik.WinControls.UI.RadLabelElement
        Private CommandBarSeparator4 As Telerik.WinControls.UI.CommandBarSeparator
        Private radCheckBoxElement1 As Telerik.WinControls.UI.RadCheckBoxElement
        Private CommandBarSeparator1 As Telerik.WinControls.UI.CommandBarSeparator
        Private radMenuItem13 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem17 As Telerik.WinControls.UI.RadMenuItem
        Private radButton2 As Telerik.WinControls.UI.RadButton
        Private radButton1 As Telerik.WinControls.UI.RadButton
        Private radButton3 As Telerik.WinControls.UI.RadButton
        Private radMenuItem21 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem22 As Telerik.WinControls.UI.RadMenuItem
        Private radMenuItem26 As Telerik.WinControls.UI.RadMenuItem
        Private radGridView1 As Telerik.WinControls.UI.RadGridView
        Private northwindDataSet As Telerik.Examples.WinControls.DataSources.NorthwindDataSet
        Private employeesBindingSource As System.Windows.Forms.BindingSource
        Private employeesTableAdapter As Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeesTableAdapter
        Private radMenuItem12 As Telerik.WinControls.UI.RadMenuItem
        Private radMenu1 As Telerik.WinControls.UI.RadMenu
        Private radPageView1 As Telerik.WinControls.UI.RadPageView
        Private radPageViewPage1 As Telerik.WinControls.UI.RadPageViewPage
        Private radPageViewPage2 As Telerik.WinControls.UI.RadPageViewPage
        Private radPageViewPage3 As Telerik.WinControls.UI.RadPageViewPage
        Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBox3 As Telerik.WinControls.UI.RadCheckBox
        Private radRadioButton1 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton2 As Telerik.WinControls.UI.RadRadioButton
        Private radRadioButton3 As Telerik.WinControls.UI.RadRadioButton
    End Class
End Namespace