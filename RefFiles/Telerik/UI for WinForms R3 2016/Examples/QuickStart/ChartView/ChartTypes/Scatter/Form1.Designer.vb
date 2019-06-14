
Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Scatter
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

#Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Dim cartesianArea2 As New Telerik.WinControls.UI.CartesianArea()
            Me.employeeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.adventureLT2008DataSet = New Telerik.Examples.WinControls.DataSources.AdventureLT2008DataSet()
            Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.employeeTableAdapter = New Telerik.Examples.WinControls.DataSources.AdventureLT2008DataSetTableAdapters.EmployeeTableAdapter()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radSpinEditorPointRadius = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radButtonEditShape = New Telerik.WinControls.UI.RadButton()
            Me.radDropDownListShapes = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radDropDownListSeriesType = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radCheckBoxSpline = New Telerik.WinControls.UI.RadCheckBox()
            Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.adventureLT2008DataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            DirectCast(Me.radSpinEditorPointRadius, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radButtonEditShape, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDropDownListShapes, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radDropDownListSeriesType, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radCheckBoxSpline, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox2.SuspendLayout()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBox2)
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Controls.Add(Me.radLabel2)
            Me.settingsPanel.Location = New System.Drawing.Point(1031, 216)
            Me.settingsPanel.Size = New System.Drawing.Size(220, 624)
            Me.settingsPanel.ThemeName = "ControlDefault"
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabel2, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
            ' 
            ' employeeBindingSource
            ' 
            Me.employeeBindingSource.DataMember = "Employee"
            Me.employeeBindingSource.DataSource = Me.adventureLT2008DataSet
            ' 
            ' adventureLT2008DataSet
            ' 
            Me.adventureLT2008DataSet.DataSetName = "AdventureLT2008DataSet"
            Me.adventureLT2008DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            ' 
            ' imageList1
            ' 
            Me.imageList1.ImageStream = DirectCast(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.imageList1.Images.SetKeyName(0, "User Business Male.png")
            ' 
            ' employeeTableAdapter
            ' 
            Me.employeeTableAdapter.ClearBeforeFill = True
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaDesign = cartesianArea2
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.ImageList = Me.imageList1
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.MinimumSize = New System.Drawing.Size(550, 320)
            Me.radChartView1.Name = "radChartView1"
            ' 
            ' 
            ' 
            Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(550, 320)
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.Size = New System.Drawing.Size(1871, 1003)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox1.Controls.Add(Me.radCheckBoxSpline)
            Me.radGroupBox1.Controls.Add(Me.radDropDownListSeriesType)
            Me.radGroupBox1.Controls.Add(Me.radLabel4)
            Me.radGroupBox1.HeaderText = "Series"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 32)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(200, 100)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "Series"
            ' 
            ' radSpinEditorPointRadius
            ' 
            Me.radSpinEditorPointRadius.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorPointRadius.Location = New System.Drawing.Point(5, 92)
            Me.radSpinEditorPointRadius.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
            Me.radSpinEditorPointRadius.Name = "radSpinEditorPointRadius"
            Me.radSpinEditorPointRadius.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditorPointRadius.Size = New System.Drawing.Size(190, 20)
            Me.radSpinEditorPointRadius.TabIndex = 3
            Me.radSpinEditorPointRadius.TabStop = False
            Me.radSpinEditorPointRadius.Value = New Decimal(New Integer() {1, 0, 0, 0})
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel3.Location = New System.Drawing.Point(5, 71)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(53, 18)
            Me.radLabel3.TabIndex = 2
            Me.radLabel3.Text = "Point size"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(5, 21)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(36, 18)
            Me.radLabel1.TabIndex = 2
            Me.radLabel1.Text = "Shape"
            ' 
            ' radButtonEditShape
            ' 
            Me.radButtonEditShape.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radButtonEditShape.Location = New System.Drawing.Point(5, 124)
            Me.radButtonEditShape.Name = "radButtonEditShape"
            Me.radButtonEditShape.Size = New System.Drawing.Size(190, 30)
            Me.radButtonEditShape.TabIndex = 1
            Me.radButtonEditShape.Text = "Edit shape"
            ' 
            ' radDropDownListShapes
            ' 
            Me.radDropDownListShapes.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListShapes.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListShapes.Location = New System.Drawing.Point(5, 42)
            Me.radDropDownListShapes.Name = "radDropDownListShapes"
            Me.radDropDownListShapes.Size = New System.Drawing.Size(190, 20)
            Me.radDropDownListShapes.TabIndex = 0
            Me.radDropDownListShapes.Text = "radDropDownList1"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(10, 0)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(35, 18)
            Me.radLabel2.TabIndex = 2
            Me.radLabel2.Text = "Series"
            ' 
            ' radDropDownListSeriesType
            ' 
            Me.radDropDownListSeriesType.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownListSeriesType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownListSeriesType.Location = New System.Drawing.Point(5, 45)
            Me.radDropDownListSeriesType.Name = "radDropDownListSeriesType"
            Me.radDropDownListSeriesType.Size = New System.Drawing.Size(190, 20)
            Me.radDropDownListSeriesType.TabIndex = 4
            Me.radDropDownListSeriesType.Text = "radDropDownList1"
            ' 
            ' radLabel4
            ' 
            Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel4.Location = New System.Drawing.Point(5, 21)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(60, 18)
            Me.radLabel4.TabIndex = 2
            Me.radLabel4.Text = "Series type"
            ' 
            ' radCheckBoxSpline
            ' 
            Me.radCheckBoxSpline.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxSpline.Location = New System.Drawing.Point(5, 72)
            Me.radCheckBoxSpline.Name = "radCheckBoxSpline"
            Me.radCheckBoxSpline.Size = New System.Drawing.Size(51, 18)
            Me.radCheckBoxSpline.TabIndex = 5
            Me.radCheckBoxSpline.Text = "Spline"
            ' 
            ' radGroupBox2
            ' 
            Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox2.Controls.Add(Me.radLabel1)
            Me.radGroupBox2.Controls.Add(Me.radDropDownListShapes)
            Me.radGroupBox2.Controls.Add(Me.radSpinEditorPointRadius)
            Me.radGroupBox2.Controls.Add(Me.radButtonEditShape)
            Me.radGroupBox2.Controls.Add(Me.radLabel3)
            Me.radGroupBox2.HeaderText = "Data points"
            Me.radGroupBox2.Location = New System.Drawing.Point(10, 138)
            Me.radGroupBox2.Name = "radGroupBox2"
            Me.radGroupBox2.Size = New System.Drawing.Size(200, 168)
            Me.radGroupBox2.TabIndex = 3
            Me.radGroupBox2.Text = "Data points"
            ' 
            ' Form1
            ' 
            Me.AutoScrollMinSize = New System.Drawing.Size(550, 320)
            Me.Controls.Add(Me.radChartView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1881, 1013)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radChartView1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.employeeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.adventureLT2008DataSet, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.radGroupBox1.PerformLayout()
            DirectCast(Me.radSpinEditorPointRadius, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radButtonEditShape, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDropDownListShapes, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radDropDownListSeriesType, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radCheckBoxSpline, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox2.ResumeLayout(False)
            Me.radGroupBox2.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private employeeBindingSource As System.Windows.Forms.BindingSource
        Private adventureLT2008DataSet As DataSources.AdventureLT2008DataSet
        Private employeeTableAdapter As DataSources.AdventureLT2008DataSetTableAdapters.EmployeeTableAdapter
        Private imageList1 As System.Windows.Forms.ImageList
        Private radChartView1 As Telerik.WinControls.UI.RadChartView
        Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Private radDropDownListShapes As Telerik.WinControls.UI.RadDropDownList
        Private radButtonEditShape As Telerik.WinControls.UI.RadButton
        Private radLabel1 As Telerik.WinControls.UI.RadLabel
        Private radLabel2 As Telerik.WinControls.UI.RadLabel
        Private radLabel3 As Telerik.WinControls.UI.RadLabel
        Private radSpinEditorPointRadius As Telerik.WinControls.UI.RadSpinEditor
        Private radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
        Private radCheckBoxSpline As Telerik.WinControls.UI.RadCheckBox
        Private radDropDownListSeriesType As Telerik.WinControls.UI.RadDropDownList
        Private radLabel4 As Telerik.WinControls.UI.RadLabel
    End Class
End Namespace