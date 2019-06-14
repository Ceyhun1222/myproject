Namespace Telerik.Examples.WinControls.ListView.CustomItems
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
			Me.radListView1 = New Telerik.WinControls.UI.RadListView()
			Me.albumsDataTableBindingSource = New BindingSource(Me.components)
			Me.musicCollectionDataSet = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet()
			Me.albumsDataTableTableAdapter = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsDataTableTableAdapter()
			Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radListView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.albumsDataTableBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.musicCollectionDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radCheckBox1)
			Me.settingsPanel.Location = New Point(1065, 1)
			Me.settingsPanel.Size = New Size(200, 747)
			Me.settingsPanel.ThemeName = "ControlDefault"
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBox1, 0)
			' 
			' radListView1
			' 
			Me.radListView1.AllowEdit = False
			Me.radListView1.AllowRemove = False
			Me.radListView1.DataSource = Me.albumsDataTableBindingSource
			Me.radListView1.FullRowSelect = False
			Me.radListView1.ItemSize = New Size(64, 64)
			Me.radListView1.Location = New Point(0, 0)
			Me.radListView1.Name = "radListView1"
			Me.radListView1.Size = New Size(640, 329)
			Me.radListView1.TabIndex = 0
			Me.radListView1.Text = "radListView1"
			Me.radListView1.ViewType = Telerik.WinControls.UI.ListViewType.IconsView
'			Me.radListView1.VisualItemCreating += New Telerik.WinControls.UI.ListViewVisualItemCreatingEventHandler(Me.radListView1_VisualItemCreating)
			' 
			' albumsDataTableBindingSource
			' 
			Me.albumsDataTableBindingSource.DataMember = "AlbumsDataTable"
			Me.albumsDataTableBindingSource.DataSource = Me.musicCollectionDataSet
			' 
			' musicCollectionDataSet
			' 
			Me.musicCollectionDataSet.DataSetName = "MusicCollectionDataSet"
			Me.musicCollectionDataSet.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			' 
			' 
			' 
			' 
			' albumsDataTableTableAdapter
			' 
			Me.albumsDataTableTableAdapter.ClearBeforeFill = True
			' 
			' radCheckBox1
			' 
			Me.radCheckBox1.Anchor = AnchorStyles.Top
			Me.radCheckBox1.Location = New Point(10, 37)
			Me.radCheckBox1.Name = "radCheckBox1"
			Me.radCheckBox1.Size = New Size(136, 18)
			Me.radCheckBox1.TabIndex = 1
			Me.radCheckBox1.Text = "Enable Kinetic Scrolling"
			Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
'			Me.radCheckBox1.ToggleStateChanged += New Telerik.WinControls.UI.StateChangedEventHandler(Me.radCheckBox1_ToggleStateChanged)
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radListView1)
			Me.Name = "Form1"
			Me.Size = New Size(1029, 672)
			Me.Controls.SetChildIndex(Me.radListView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radListView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.albumsDataTableBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.musicCollectionDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

        Friend WithEvents radListView1 As Telerik.WinControls.UI.RadListView
        Friend WithEvents musicCollectionDataSet As DataSources.MusicCollectionDataSet
        Friend WithEvents albumsDataTableBindingSource As BindingSource
        Friend WithEvents albumsDataTableTableAdapter As DataSources.MusicCollectionDataSetTableAdapters.AlbumsDataTableTableAdapter
        Friend WithEvents radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
	End Class
End Namespace