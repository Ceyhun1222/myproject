Namespace Telerik.Examples.WinControls.LayoutControl.FirstLook
    Partial Class Form1

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor. 
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn2 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn3 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn4 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn5 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewTextBoxColumn6 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
            Dim gridViewDecimalColumn1 As New Telerik.WinControls.UI.GridViewDecimalColumn()
            Dim gridViewDecimalColumn2 As New Telerik.WinControls.UI.GridViewDecimalColumn()
            Dim tableViewDefinition1 As New Telerik.WinControls.UI.TableViewDefinition()
            Me.radLayoutControl1 = New Telerik.WinControls.UI.RadLayoutControl()
            Me.pictureBox1 = New System.Windows.Forms.PictureBox()
            Me.radTextBox1 = New Telerik.WinControls.UI.RadTextBox()
            Me.radTextBox2 = New Telerik.WinControls.UI.RadTextBox()
            Me.radTextBox4 = New Telerik.WinControls.UI.RadTextBox()
            Me.radDateTimePicker1 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radGridView1 = New Telerik.WinControls.UI.RadGridView()
            Me.employeeOrdersBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.northwindDataSet = New Telerik.Examples.WinControls.DataSources.NorthwindDataSet()
            Me.radTextBox3 = New Telerik.WinControls.UI.RadTextBox()
            Me.radTextBox5 = New Telerik.WinControls.UI.RadTextBox()
            Me.radTextBox6 = New Telerik.WinControls.UI.RadTextBox()
            Me.radTextBox7 = New Telerik.WinControls.UI.RadTextBox()
            Me.radDateTimePicker2 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.layoutControlItem2 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlTabbedGroup1 = New Telerik.WinControls.UI.LayoutControlTabbedGroup()
            Me.layoutControlGroupItem1 = New Telerik.WinControls.UI.LayoutControlGroupItem()
            Me.layoutControlItem5 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlGroupItem2 = New Telerik.WinControls.UI.LayoutControlGroupItem()
            Me.layoutControlItem11 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem12 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem13 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem3 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem4 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem10 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem18 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlItem1 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlLabelItem1 = New Telerik.WinControls.UI.LayoutControlLabelItem()
            Me.layoutControlSeparatorItem1 = New Telerik.WinControls.UI.LayoutControlSeparatorItem()
            Me.layoutControlGroupItem3 = New Telerik.WinControls.UI.LayoutControlGroupItem()
            Me.layoutControlItem6 = New Telerik.WinControls.UI.LayoutControlItem()
            Me.layoutControlSplitterItem1 = New Telerik.WinControls.UI.LayoutControlSplitterItem()
            Me.layoutControlSplitterItem2 = New Telerik.WinControls.UI.LayoutControlSplitterItem()
            Me.employeeOrdersTableAdapter = New Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.EmployeeOrdersTableAdapter()
            DirectCast(Me.radLayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radLayoutControl1.SuspendLayout()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.employeeOrdersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radTextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radLayoutControl1
            ' 
            Me.radLayoutControl1.BackColor = System.Drawing.Color.Transparent
            Me.radLayoutControl1.Controls.Add(Me.pictureBox1)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox1)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox2)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox4)
            Me.radLayoutControl1.Controls.Add(Me.radDateTimePicker1)
            Me.radLayoutControl1.Controls.Add(Me.radGridView1)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox3)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox5)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox6)
            Me.radLayoutControl1.Controls.Add(Me.radTextBox7)
            Me.radLayoutControl1.Controls.Add(Me.radDateTimePicker2)
            Me.radLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radLayoutControl1.Items.AddRange(New Telerik.WinControls.RadItem() {Me.layoutControlItem2, Me.layoutControlTabbedGroup1, Me.layoutControlItem3, Me.layoutControlItem4, Me.layoutControlItem10, Me.layoutControlItem18, _
                Me.layoutControlItem1, Me.layoutControlLabelItem1, Me.layoutControlSeparatorItem1, Me.layoutControlGroupItem3, Me.layoutControlSplitterItem1, Me.layoutControlSplitterItem2})
            Me.radLayoutControl1.Location = New System.Drawing.Point(0, 0)
            Me.radLayoutControl1.Name = "radLayoutControl1"
            Me.radLayoutControl1.Size = New System.Drawing.Size(598, 496)
            Me.radLayoutControl1.TabIndex = 0
            Me.radLayoutControl1.Text = "radLayoutControl1"
            ' 
            ' pictureBox1
            ' 
            Me.pictureBox1.BackgroundImage = My.Resources.Janet
            Me.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.pictureBox1.Location = New System.Drawing.Point(422, 3)
            Me.pictureBox1.MaximumSize = New System.Drawing.Size(0, 202)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(173, 202)
            Me.pictureBox1.TabIndex = 3
            Me.pictureBox1.TabStop = False
            ' 
            ' radTextBox1
            ' 
            Me.radTextBox1.Location = New System.Drawing.Point(3, 23)
            Me.radTextBox1.Name = "radTextBox1"
            Me.radTextBox1.Size = New System.Drawing.Size(202, 20)
            Me.radTextBox1.TabIndex = 4
            Me.radTextBox1.Text = "Janet"
            ' 
            ' radTextBox2
            ' 
            Me.radTextBox2.Location = New System.Drawing.Point(3, 127)
            Me.radTextBox2.Name = "radTextBox2"
            Me.radTextBox2.Size = New System.Drawing.Size(202, 20)
            Me.radTextBox2.TabIndex = 5
            Me.radTextBox2.Text = "Sales Representative"
            ' 
            ' radTextBox4
            ' 
            Me.radTextBox4.AutoSize = False
            Me.radTextBox4.Location = New System.Drawing.Point(7, 181)
            Me.radTextBox4.Multiline = True
            Me.radTextBox4.Name = "radTextBox4"
            Me.radTextBox4.Size = New System.Drawing.Size(401, 86)
            Me.radTextBox4.TabIndex = 7
            Me.radTextBox4.Text = resources.GetString("radTextBox4.Text")
            ' 
            ' radDateTimePicker1
            ' 
            Me.radDateTimePicker1.Location = New System.Drawing.Point(211, 127)
            Me.radDateTimePicker1.Name = "radDateTimePicker1"
            Me.radDateTimePicker1.Size = New System.Drawing.Size(201, 20)
            Me.radDateTimePicker1.TabIndex = 13
            Me.radDateTimePicker1.TabStop = False
            Me.radDateTimePicker1.Text = "Wednesday, April 01, 1992"
            Me.radDateTimePicker1.Value = New System.DateTime(1992, 4, 1, 0, 0, 0, _
                0)
            ' 
            ' radGridView1
            ' 
            Me.radGridView1.Location = New System.Drawing.Point(13, 316)
            ' 
            ' 
            ' 
            Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
            gridViewTextBoxColumn1.FieldName = "LastName"
            gridViewTextBoxColumn1.HeaderText = "LastName"
            gridViewTextBoxColumn1.IsAutoGenerated = True
            gridViewTextBoxColumn1.Name = "LastName"
            gridViewTextBoxColumn1.Width = 70
            gridViewTextBoxColumn2.FieldName = "FirstName"
            gridViewTextBoxColumn2.HeaderText = "FirstName"
            gridViewTextBoxColumn2.IsAutoGenerated = True
            gridViewTextBoxColumn2.Name = "FirstName"
            gridViewTextBoxColumn2.Width = 70
            gridViewTextBoxColumn3.FieldName = "Title"
            gridViewTextBoxColumn3.HeaderText = "Title"
            gridViewTextBoxColumn3.IsAutoGenerated = True
            gridViewTextBoxColumn3.Name = "Title"
            gridViewTextBoxColumn3.Width = 70
            gridViewTextBoxColumn4.FieldName = "City"
            gridViewTextBoxColumn4.HeaderText = "City"
            gridViewTextBoxColumn4.IsAutoGenerated = True
            gridViewTextBoxColumn4.Name = "City"
            gridViewTextBoxColumn4.Width = 70
            gridViewTextBoxColumn5.FieldName = "Country"
            gridViewTextBoxColumn5.HeaderText = "Country"
            gridViewTextBoxColumn5.IsAutoGenerated = True
            gridViewTextBoxColumn5.Name = "Country"
            gridViewTextBoxColumn5.Width = 70
            gridViewTextBoxColumn6.FieldName = "ShipName"
            gridViewTextBoxColumn6.HeaderText = "ShipName"
            gridViewTextBoxColumn6.IsAutoGenerated = True
            gridViewTextBoxColumn6.Name = "ShipName"
            gridViewTextBoxColumn6.Width = 70
            gridViewDecimalColumn1.FieldName = "UnitPrice"
            gridViewDecimalColumn1.HeaderText = "UnitPrice"
            gridViewDecimalColumn1.IsAutoGenerated = True
            gridViewDecimalColumn1.Name = "UnitPrice"
            gridViewDecimalColumn1.Width = 70
            gridViewDecimalColumn2.DataType = GetType(Short)
            gridViewDecimalColumn2.FieldName = "Quantity"
            gridViewDecimalColumn2.HeaderText = "Quantity"
            gridViewDecimalColumn2.IsAutoGenerated = True
            gridViewDecimalColumn2.Name = "Quantity"
            gridViewDecimalColumn2.Width = 68
            Me.radGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {gridViewTextBoxColumn1, gridViewTextBoxColumn2, gridViewTextBoxColumn3, gridViewTextBoxColumn4, gridViewTextBoxColumn5, gridViewTextBoxColumn6, _
                gridViewDecimalColumn1, gridViewDecimalColumn2})
            Me.radGridView1.MasterTemplate.DataSource = Me.employeeOrdersBindingSource
            Me.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1
            Me.radGridView1.Name = "radGridView1"
            Me.radGridView1.Size = New System.Drawing.Size(571, 161)
            Me.radGridView1.TabIndex = 14
            Me.radGridView1.Text = "radGridView1"
            ' 
            ' employeeOrdersBindingSource
            ' 
            Me.employeeOrdersBindingSource.DataMember = "EmployeeOrders"
            Me.employeeOrdersBindingSource.DataSource = Me.northwindDataSet
            ' 
            ' northwindDataSet
            ' 
            Me.northwindDataSet.DataSetName = "NorthwindDataSet"
            Me.northwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            ' 
            ' radTextBox3
            ' 
            Me.radTextBox3.AutoSize = False
            Me.radTextBox3.Location = New System.Drawing.Point(13, 384)
            Me.radTextBox3.Multiline = True
            Me.radTextBox3.Name = "radTextBox3"
            Me.radTextBox3.Size = New System.Drawing.Size(571, 97)
            Me.radTextBox3.TabIndex = 15
            Me.radTextBox3.Text = "507 - 20th Ave. E."
            ' 
            ' radTextBox5
            ' 
            Me.radTextBox5.Location = New System.Drawing.Point(510, 338)
            Me.radTextBox5.Name = "radTextBox5"
            Me.radTextBox5.Size = New System.Drawing.Size(74, 20)
            Me.radTextBox5.TabIndex = 16
            Me.radTextBox5.Text = "Seattle"
            ' 
            ' radTextBox6
            ' 
            Me.radTextBox6.Location = New System.Drawing.Point(13, 338)
            Me.radTextBox6.Name = "radTextBox6"
            Me.radTextBox6.Size = New System.Drawing.Size(491, 20)
            Me.radTextBox6.TabIndex = 17
            Me.radTextBox6.Text = "USA"
            ' 
            ' radTextBox7
            ' 
            Me.radTextBox7.Location = New System.Drawing.Point(211, 23)
            Me.radTextBox7.Name = "radTextBox7"
            Me.radTextBox7.Size = New System.Drawing.Size(201, 20)
            Me.radTextBox7.TabIndex = 23
            Me.radTextBox7.Text = "Leverling"
            ' 
            ' radDateTimePicker2
            ' 
            Me.radDateTimePicker2.Location = New System.Drawing.Point(3, 73)
            Me.radDateTimePicker2.Name = "radDateTimePicker2"
            Me.radDateTimePicker2.Size = New System.Drawing.Size(202, 20)
            Me.radDateTimePicker2.TabIndex = 25
            Me.radDateTimePicker2.TabStop = False
            Me.radDateTimePicker2.Text = "Saturday, August 17, 1963"
            Me.radDateTimePicker2.Value = New System.DateTime(1963, 8, 17, 0, 0, 0, _
                0)
            ' 
            ' layoutControlItem2
            ' 
            Me.layoutControlItem2.AssociatedControl = Me.pictureBox1
            Me.layoutControlItem2.Bounds = New System.Drawing.Rectangle(419, 0, 179, 274)
            Me.layoutControlItem2.MaxSize = New System.Drawing.Size(270, 0)
            Me.layoutControlItem2.MinSize = New System.Drawing.Size(120, 26)
            Me.layoutControlItem2.Name = "layoutControlItem2"
            ' 
            ' layoutControlTabbedGroup1
            ' 
            Me.layoutControlTabbedGroup1.Bounds = New System.Drawing.Rectangle(0, 278, 598, 218)
            Me.layoutControlTabbedGroup1.ItemGroups.AddRange(New Telerik.WinControls.RadItem() {Me.layoutControlGroupItem1, Me.layoutControlGroupItem2})
            Me.layoutControlTabbedGroup1.Name = "layoutControlTabbedGroup1"
            Me.layoutControlTabbedGroup1.SelectedGroup = Me.layoutControlGroupItem2
            ' 
            ' layoutControlGroupItem1
            ' 
            Me.layoutControlGroupItem1.AccessibleDescription = "layoutControlGroupItem1"
            Me.layoutControlGroupItem1.AccessibleName = "layoutControlGroupItem1"
            Me.layoutControlGroupItem1.Bounds = New System.Drawing.Rectangle(0, 0, 587, 178)
            Me.layoutControlGroupItem1.Items.AddRange(New Telerik.WinControls.RadItem() {Me.layoutControlItem5})
            Me.layoutControlGroupItem1.Name = "layoutControlGroupItem1"
            Me.layoutControlGroupItem1.Text = "Sales"
            ' 
            ' layoutControlItem5
            ' 
            Me.layoutControlItem5.AssociatedControl = Me.radGridView1
            Me.layoutControlItem5.Bounds = New System.Drawing.Rectangle(0, 0, 577, 167)
            Me.layoutControlItem5.MinSize = New System.Drawing.Size(46, 120)
            Me.layoutControlItem5.Name = "layoutControlItem5"
            ' 
            ' layoutControlGroupItem2
            ' 
            Me.layoutControlGroupItem2.AccessibleDescription = "layoutControlGroupItem2"
            Me.layoutControlGroupItem2.AccessibleName = "layoutControlGroupItem2"
            Me.layoutControlGroupItem2.Bounds = New System.Drawing.Rectangle(0, 0, 577, 170)
            Me.layoutControlGroupItem2.Items.AddRange(New Telerik.WinControls.RadItem() {Me.layoutControlItem11, Me.layoutControlItem12, Me.layoutControlItem13})
            Me.layoutControlGroupItem2.Name = "layoutControlGroupItem2"
            Me.layoutControlGroupItem2.Text = "Address"
            ' 
            ' layoutControlItem11
            ' 
            Me.layoutControlItem11.AccessibleDescription = "Street Address"
            Me.layoutControlItem11.AccessibleName = "Street Address"
            Me.layoutControlItem11.AssociatedControl = Me.radTextBox3
            Me.layoutControlItem11.Bounds = New System.Drawing.Rectangle(0, 46, 577, 123)
            Me.layoutControlItem11.DrawText = True
            Me.layoutControlItem11.MaxSize = New System.Drawing.Size(0, 0)
            Me.layoutControlItem11.Name = "layoutControlItem11"
            Me.layoutControlItem11.Text = "Street Address"
            Me.layoutControlItem11.TextAlignment = System.Drawing.ContentAlignment.TopLeft
            Me.layoutControlItem11.TextFixedSize = 20
            Me.layoutControlItem11.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem11.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem12
            ' 
            Me.layoutControlItem12.AccessibleDescription = "City"
            Me.layoutControlItem12.AccessibleName = "City"
            Me.layoutControlItem12.AssociatedControl = Me.radTextBox5
            Me.layoutControlItem12.Bounds = New System.Drawing.Rectangle(497, 0, 80, 46)
            Me.layoutControlItem12.DrawText = True
            Me.layoutControlItem12.MaxSize = New System.Drawing.Size(0, 46)
            Me.layoutControlItem12.MinSize = New System.Drawing.Size(46, 46)
            Me.layoutControlItem12.Name = "layoutControlItem12"
            Me.layoutControlItem12.Text = "City"
            Me.layoutControlItem12.TextAlignment = System.Drawing.ContentAlignment.TopLeft
            Me.layoutControlItem12.TextFixedSize = 20
            Me.layoutControlItem12.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem12.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem13
            ' 
            Me.layoutControlItem13.AccessibleDescription = "Country"
            Me.layoutControlItem13.AccessibleName = "Country"
            Me.layoutControlItem13.AssociatedControl = Me.radTextBox6
            Me.layoutControlItem13.Bounds = New System.Drawing.Rectangle(0, 0, 497, 46)
            Me.layoutControlItem13.DrawText = True
            Me.layoutControlItem13.MaxSize = New System.Drawing.Size(0, 46)
            Me.layoutControlItem13.MinSize = New System.Drawing.Size(46, 46)
            Me.layoutControlItem13.Name = "layoutControlItem13"
            Me.layoutControlItem13.Text = "Country"
            Me.layoutControlItem13.TextAlignment = System.Drawing.ContentAlignment.TopLeft
            Me.layoutControlItem13.TextFixedSize = 20
            Me.layoutControlItem13.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem13.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem3
            ' 
            Me.layoutControlItem3.AccessibleDescription = "First Name"
            Me.layoutControlItem3.AccessibleName = "First Name"
            Me.layoutControlItem3.AssociatedControl = Me.radTextBox1
            Me.layoutControlItem3.Bounds = New System.Drawing.Rectangle(0, 0, 208, 50)
            Me.layoutControlItem3.DrawText = True
            Me.layoutControlItem3.MaxSize = New System.Drawing.Size(0, 50)
            Me.layoutControlItem3.MinSize = New System.Drawing.Size(100, 50)
            Me.layoutControlItem3.Name = "layoutControlItem3"
            Me.layoutControlItem3.Text = "First Name"
            Me.layoutControlItem3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            Me.layoutControlItem3.TextFixedSize = 20
            Me.layoutControlItem3.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem3.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem4
            ' 
            Me.layoutControlItem4.AccessibleDescription = "Job Title"
            Me.layoutControlItem4.AccessibleName = "Job Title"
            Me.layoutControlItem4.AssociatedControl = Me.radTextBox2
            Me.layoutControlItem4.Bounds = New System.Drawing.Rectangle(0, 104, 208, 50)
            Me.layoutControlItem4.DrawText = True
            Me.layoutControlItem4.MaxSize = New System.Drawing.Size(0, 50)
            Me.layoutControlItem4.MinSize = New System.Drawing.Size(100, 50)
            Me.layoutControlItem4.Name = "layoutControlItem4"
            Me.layoutControlItem4.Text = "Job Title"
            Me.layoutControlItem4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            Me.layoutControlItem4.TextFixedSize = 20
            Me.layoutControlItem4.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem4.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem10
            ' 
            Me.layoutControlItem10.AccessibleDescription = "Hire Date"
            Me.layoutControlItem10.AccessibleName = "Hire Date"
            Me.layoutControlItem10.AssociatedControl = Me.radDateTimePicker1
            Me.layoutControlItem10.Bounds = New System.Drawing.Rectangle(208, 104, 207, 50)
            Me.layoutControlItem10.DrawText = True
            Me.layoutControlItem10.MaxSize = New System.Drawing.Size(0, 50)
            Me.layoutControlItem10.MinSize = New System.Drawing.Size(100, 50)
            Me.layoutControlItem10.Name = "layoutControlItem10"
            Me.layoutControlItem10.Text = "Hire Date"
            Me.layoutControlItem10.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            Me.layoutControlItem10.TextFixedSize = 20
            Me.layoutControlItem10.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem10.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem18
            ' 
            Me.layoutControlItem18.AccessibleDescription = "Last Name"
            Me.layoutControlItem18.AccessibleName = "Last Name"
            Me.layoutControlItem18.AssociatedControl = Me.radTextBox7
            Me.layoutControlItem18.Bounds = New System.Drawing.Rectangle(208, 0, 207, 50)
            Me.layoutControlItem18.DrawText = True
            Me.layoutControlItem18.MaxSize = New System.Drawing.Size(0, 50)
            Me.layoutControlItem18.MinSize = New System.Drawing.Size(100, 50)
            Me.layoutControlItem18.Name = "layoutControlItem18"
            Me.layoutControlItem18.Text = "Last Name"
            Me.layoutControlItem18.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            Me.layoutControlItem18.TextFixedSize = 20
            Me.layoutControlItem18.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem18.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlItem1
            ' 
            Me.layoutControlItem1.AccessibleDescription = "Birth date"
            Me.layoutControlItem1.AccessibleName = "Birth date"
            Me.layoutControlItem1.AssociatedControl = Me.radDateTimePicker2
            Me.layoutControlItem1.Bounds = New System.Drawing.Rectangle(0, 50, 208, 50)
            Me.layoutControlItem1.DrawText = True
            Me.layoutControlItem1.MaxSize = New System.Drawing.Size(0, 50)
            Me.layoutControlItem1.MinSize = New System.Drawing.Size(100, 50)
            Me.layoutControlItem1.Name = "layoutControlItem1"
            Me.layoutControlItem1.Text = "Birth date"
            Me.layoutControlItem1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            Me.layoutControlItem1.TextFixedSize = 20
            Me.layoutControlItem1.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem1.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlLabelItem1
            ' 
            Me.layoutControlLabelItem1.Bounds = New System.Drawing.Rectangle(208, 50, 207, 50)
            Me.layoutControlLabelItem1.DrawText = False
            Me.layoutControlLabelItem1.MaxSize = New System.Drawing.Size(0, 50)
            Me.layoutControlLabelItem1.MinSize = New System.Drawing.Size(46, 50)
            Me.layoutControlLabelItem1.Name = "layoutControlLabelItem1"
            ' 
            ' layoutControlSeparatorItem1
            ' 
            Me.layoutControlSeparatorItem1.Bounds = New System.Drawing.Rectangle(0, 100, 415, 4)
            Me.layoutControlSeparatorItem1.Name = "layoutControlSeparatorItem1"
            ' 
            ' layoutControlGroupItem3
            ' 
            Me.layoutControlGroupItem3.AccessibleDescription = "Description"
            Me.layoutControlGroupItem3.AccessibleName = "Description"
            Me.layoutControlGroupItem3.Bounds = New System.Drawing.Rectangle(0, 154, 415, 120)
            Me.layoutControlGroupItem3.Items.AddRange(New Telerik.WinControls.RadItem() {Me.layoutControlItem6})
            Me.layoutControlGroupItem3.Name = "layoutControlGroupItem3"
            Me.layoutControlGroupItem3.Text = "Description"
            ' 
            ' layoutControlItem6
            ' 
            Me.layoutControlItem6.AccessibleDescription = "Details"
            Me.layoutControlItem6.AccessibleName = "Details"
            Me.layoutControlItem6.AssociatedControl = Me.radTextBox4
            Me.layoutControlItem6.Bounds = New System.Drawing.Rectangle(0, 0, 407, 92)
            Me.layoutControlItem6.DrawText = False
            Me.layoutControlItem6.MinSize = New System.Drawing.Size(46, 60)
            Me.layoutControlItem6.Name = "layoutControlItem6"
            Me.layoutControlItem6.Text = "Details"
            Me.layoutControlItem6.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
            Me.layoutControlItem6.TextFixedSize = 22
            Me.layoutControlItem6.TextOrientation = System.Windows.Forms.Orientation.Horizontal
            Me.layoutControlItem6.TextPosition = Telerik.WinControls.UI.LayoutItemTextPosition.Top
            Me.layoutControlItem6.TextSizeMode = Telerik.WinControls.UI.LayoutItemTextSizeMode.Fixed
            ' 
            ' layoutControlSplitterItem1
            ' 
            Me.layoutControlSplitterItem1.Bounds = New System.Drawing.Rectangle(0, 274, 598, 4)
            Me.layoutControlSplitterItem1.Name = "layoutControlSplitterItem1"
            ' 
            ' layoutControlSplitterItem2
            ' 
            Me.layoutControlSplitterItem2.Bounds = New System.Drawing.Rectangle(415, 0, 4, 274)
            Me.layoutControlSplitterItem2.Name = "layoutControlSplitterItem2"
            ' 
            ' employeeOrdersTableAdapter
            ' 
            Me.employeeOrdersTableAdapter.ClearBeforeFill = True
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(598, 496)
            Me.Controls.Add(Me.radLayoutControl1)
            Me.Name = "Form1"
            ' 
            ' 
            ' 
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "Form1"
            DirectCast(Me.radLayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radLayoutControl1.ResumeLayout(False)
            Me.radLayoutControl1.PerformLayout()
            DirectCast(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.employeeOrdersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.northwindDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radTextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private radLayoutControl1 As Telerik.WinControls.UI.RadLayoutControl
        Private pictureBox1 As System.Windows.Forms.PictureBox
        Private layoutControlItem2 As Telerik.WinControls.UI.LayoutControlItem
        Private radTextBox1 As Telerik.WinControls.UI.RadTextBox
        Private radTextBox2 As Telerik.WinControls.UI.RadTextBox
        Private radTextBox4 As Telerik.WinControls.UI.RadTextBox
        Private layoutControlTabbedGroup1 As Telerik.WinControls.UI.LayoutControlTabbedGroup
        Private layoutControlGroupItem1 As Telerik.WinControls.UI.LayoutControlGroupItem
        Private layoutControlGroupItem2 As Telerik.WinControls.UI.LayoutControlGroupItem
        Private layoutControlItem3 As Telerik.WinControls.UI.LayoutControlItem
        Private layoutControlItem4 As Telerik.WinControls.UI.LayoutControlItem
        Private layoutControlItem6 As Telerik.WinControls.UI.LayoutControlItem
        Private radDateTimePicker1 As Telerik.WinControls.UI.RadDateTimePicker
        Private radGridView1 As Telerik.WinControls.UI.RadGridView
        Private radTextBox3 As Telerik.WinControls.UI.RadTextBox
        Private layoutControlItem5 As Telerik.WinControls.UI.LayoutControlItem
        Private layoutControlItem11 As Telerik.WinControls.UI.LayoutControlItem
        Private layoutControlItem10 As Telerik.WinControls.UI.LayoutControlItem
        Private radTextBox5 As Telerik.WinControls.UI.RadTextBox
        Private radTextBox6 As Telerik.WinControls.UI.RadTextBox
        Private layoutControlItem12 As Telerik.WinControls.UI.LayoutControlItem
        Private layoutControlItem13 As Telerik.WinControls.UI.LayoutControlItem
        Private radTextBox7 As Telerik.WinControls.UI.RadTextBox
        Private layoutControlItem18 As Telerik.WinControls.UI.LayoutControlItem
        Private radDateTimePicker2 As Telerik.WinControls.UI.RadDateTimePicker
        Private layoutControlItem1 As Telerik.WinControls.UI.LayoutControlItem
        Private layoutControlLabelItem1 As Telerik.WinControls.UI.LayoutControlLabelItem
        Private layoutControlSeparatorItem1 As Telerik.WinControls.UI.LayoutControlSeparatorItem
        Private layoutControlGroupItem3 As Telerik.WinControls.UI.LayoutControlGroupItem
        Private layoutControlSplitterItem1 As Telerik.WinControls.UI.LayoutControlSplitterItem
        Private layoutControlSplitterItem2 As Telerik.WinControls.UI.LayoutControlSplitterItem
        Private northwindDataSet As DataSources.NorthwindDataSet
        Private employeeOrdersBindingSource As System.Windows.Forms.BindingSource
        Private employeeOrdersTableAdapter As DataSources.NorthwindDataSetTableAdapters.EmployeeOrdersTableAdapter
    End Class
End Namespace