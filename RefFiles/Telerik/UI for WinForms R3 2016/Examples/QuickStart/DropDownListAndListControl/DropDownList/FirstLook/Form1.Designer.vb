Namespace Telerik.Examples.WinControls.DropDownListAndListControl.DropDownList.FirstLook
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

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Me.cmbArtist = New Telerik.WinControls.UI.RadDropDownList()
			Me.artistsBindingSource = New BindingSource(Me.components)
			Me.musicCollectionDataSetBindingSource = New BindingSource(Me.components)
			Me.musicCollectionDataSet = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet()
			Me.ArtistsImageList = New ImageList(Me.components)
			Me.cmbAlbum = New Telerik.WinControls.UI.RadDropDownList()
			Me.artistsAlbumsBindingSource = New BindingSource(Me.components)
			Me.cmbSong = New Telerik.WinControls.UI.RadDropDownList()
			Me.albumsSongsBindingSource = New BindingSource(Me.components)
			Me.lblSelectedSong = New Telerik.WinControls.UI.RadLabel()
			Me.artistsTableAdapter = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.ArtistsTableAdapter()
			Me.albumsTableAdapter = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsTableAdapter()
			Me.songsTableAdapter = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.SongsTableAdapter()
			Me.pictureBox = New PictureBox()
			Me.ArtistsLargeImageList = New ImageList(Me.components)
			Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
			Me.buttonSortAscending = New Telerik.WinControls.UI.RadButton()
			Me.buttonSortDescending = New Telerik.WinControls.UI.RadButton()
			Me.radOffice2007ScreenTip1 = New Telerik.WinControls.UI.RadOffice2007ScreenTip()
			Me.radPanelDemoHolder = New Telerik.WinControls.UI.RadPanel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.cmbArtist, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.artistsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.musicCollectionDataSetBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.musicCollectionDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.musicCollectionDataSet.AlbumsDataTableProperty, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.musicCollectionDataSet.SongsDataTableProperty, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbAlbum, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.artistsAlbumsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbSong, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.albumsSongsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.lblSelectedSong, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.buttonSortAscending, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.buttonSortDescending, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radOffice2007ScreenTip1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanelDemoHolder.SuspendLayout()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Location = New Point(646, 1)
            Me.settingsPanel.Size = New Size(200, 723)
            Me.settingsPanel.ThemeName = "ControlDefault"
            ' 
            ' cmbArtist
            ' 
            Me.cmbArtist.AutoCompleteDisplayMember = "ArtistName"
            Me.cmbArtist.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            Me.cmbArtist.DataSource = Me.artistsBindingSource
            Me.cmbArtist.DisplayMember = "ArtistName"
            Me.cmbArtist.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
            Me.cmbArtist.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.cmbArtist.ImageList = Me.ArtistsImageList
            Me.cmbArtist.Location = New Point(18, 103)
            Me.cmbArtist.Name = "cmbArtist"
            Me.cmbArtist.NullText = "Select Artist"
            ' 
            ' 
            ' 
            Me.cmbArtist.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.cmbArtist.ShowImageInEditorArea = False
            Me.cmbArtist.Size = New Size(170, 22)
            Me.cmbArtist.TabIndex = 1
            ' 
            ' artistsBindingSource
            ' 
            Me.artistsBindingSource.DataMember = "Artists"
            Me.artistsBindingSource.DataSource = Me.musicCollectionDataSetBindingSource
            ' 
            ' musicCollectionDataSetBindingSource
            ' 
            Me.musicCollectionDataSetBindingSource.DataSource = Me.musicCollectionDataSet
            Me.musicCollectionDataSetBindingSource.Position = 0
            ' 
            ' musicCollectionDataSet
            ' 
            Me.musicCollectionDataSet.DataSetName = "MusicCollectionDataSet"
            Me.musicCollectionDataSet.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
            ' 
            ' 
            ' 
            Me.musicCollectionDataSet.AlbumsDataTableProperty.TableName = "AlbumsDataTable"
            ' 
            ' 
            ' 
            Me.musicCollectionDataSet.SongsDataTableProperty.TableName = "SongsDataTable"
            ' 
            ' ArtistsImageList
            ' 
            Me.ArtistsImageList.ImageStream = (CType(resources.GetObject("ArtistsImageList.ImageStream"), ImageListStreamer))
            Me.ArtistsImageList.TransparentColor = Color.Transparent
            Me.ArtistsImageList.Images.SetKeyName(0, "smallSting.jpg")
            Me.ArtistsImageList.Images.SetKeyName(1, "smallDepeche.jpg")
            Me.ArtistsImageList.Images.SetKeyName(2, "smallSheryl.jpg")
            Me.ArtistsImageList.Images.SetKeyName(3, "smallClapton.jpg")
            Me.ArtistsImageList.Images.SetKeyName(4, "smallFloyd.jpg")
            Me.ArtistsImageList.Images.SetKeyName(5, "smallPurple.jpg")
            Me.ArtistsImageList.Images.SetKeyName(6, "smallINXS.jpg")
            Me.ArtistsImageList.Images.SetKeyName(7, "smallBadu.jpg")
            Me.ArtistsImageList.Images.SetKeyName(8, "smallNada.jpg")
            Me.ArtistsImageList.Images.SetKeyName(9, "smallBreeders.jpg")
            Me.ArtistsImageList.Images.SetKeyName(10, "smallConchords.jpg")
            Me.ArtistsImageList.Images.SetKeyName(11, "smallLeona.jpg")
            ' 
            ' cmbAlbum
            ' 
            Me.cmbAlbum.AutoCompleteDisplayMember = "AlbumName"
            Me.cmbAlbum.DataSource = Me.artistsAlbumsBindingSource
            Me.cmbAlbum.DisplayMember = "AlbumName"
            Me.cmbAlbum.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
            Me.cmbAlbum.Location = New Point(239, 103)
            Me.cmbAlbum.Name = "cmbAlbum"
            Me.cmbAlbum.NullText = "Select Album"
            Me.cmbAlbum.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            ' 
            ' 
            ' 
            Me.cmbAlbum.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.cmbAlbum.Size = New Size(149, 20)
            Me.cmbAlbum.TabIndex = 2
            ' 
            ' artistsAlbumsBindingSource
            ' 
            Me.artistsAlbumsBindingSource.DataMember = "ArtistsAlbums"
            Me.artistsAlbumsBindingSource.DataSource = Me.artistsBindingSource
            ' 
            ' cmbSong
            ' 
            Me.cmbSong.AutoCompleteDisplayMember = "SongName"
            Me.cmbSong.DataSource = Me.albumsSongsBindingSource
            Me.cmbSong.DisplayMember = "SongName"
            Me.cmbSong.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
            Me.cmbSong.Location = New Point(394, 103)
            Me.cmbSong.Name = "cmbSong"
            Me.cmbSong.NullText = "Select Song"
            ' 
            ' 
            ' 
            Me.cmbSong.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.cmbSong.Size = New Size(160, 20)
            Me.cmbSong.TabIndex = 3
            Me.cmbSong.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            ' 
            ' albumsSongsBindingSource
            ' 
            Me.albumsSongsBindingSource.DataMember = "AlbumsSongs"
            Me.albumsSongsBindingSource.DataSource = Me.artistsAlbumsBindingSource
            ' 
            ' lblSelectedSong
            ' 
            Me.lblSelectedSong.BackColor = Color.Transparent
            Me.lblSelectedSong.Font = New Font("Arial", 10.0F)
            Me.lblSelectedSong.Location = New Point(128, 213)
            Me.lblSelectedSong.Name = "lblSelectedSong"
            Me.lblSelectedSong.Size = New Size(2, 2)
            Me.lblSelectedSong.TabIndex = 5
            Me.lblSelectedSong.TextAlignment = ContentAlignment.MiddleCenter
            ' 
            ' artistsTableAdapter
            ' 
            Me.artistsTableAdapter.ClearBeforeFill = True
            ' 
            ' albumsTableAdapter
            ' 
            Me.albumsTableAdapter.ClearBeforeFill = True
            ' 
            ' songsTableAdapter
            ' 
            Me.songsTableAdapter.ClearBeforeFill = True
            ' 
            ' pictureBox
            ' 
            Me.pictureBox.BackColor = Color.Transparent
            Me.pictureBox.Location = New Point(31, 183)
            Me.pictureBox.Name = "pictureBox"
            Me.pictureBox.Size = New Size(82, 83)
            Me.pictureBox.TabIndex = 6
            Me.pictureBox.TabStop = False
            ' 
            ' ArtistsLargeImageList
            ' 
            Me.ArtistsLargeImageList.ImageStream = (CType(resources.GetObject("ArtistsLargeImageList.ImageStream"), ImageListStreamer))
            Me.ArtistsLargeImageList.TransparentColor = Color.Transparent
            Me.ArtistsLargeImageList.Images.SetKeyName(0, "sting.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(1, "depeche.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(2, "sheryl.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(3, "clapton.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(4, "pink_floyd.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(5, "deepurple.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(6, "inxs.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(7, "badu.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(8, "nada_surf.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(9, "breeders.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(10, "concords.jpg")
            Me.ArtistsLargeImageList.Images.SetKeyName(11, "leona_lewis.jpg")
            ' 
            ' radPanel1
            ' 
            Me.radPanel1.BackColor = Color.Transparent
            Me.radPanel1.Location = New Point(472, 92)
            Me.radPanel1.Margin = New Padding(0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.Size = New Size(22, 18)
            Me.radPanel1.TabIndex = 7
            Me.radPanel1.Visible = False
            CType(Me.radPanel1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            ' 
            ' buttonSortAscending
            ' 
            Me.buttonSortAscending.BackColor = Color.Transparent
            Me.buttonSortAscending.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
            Me.buttonSortAscending.Image = (CType(resources.GetObject("buttonSortAscending.Image"), Image))
            Me.buttonSortAscending.Location = New Point(188, 98)
            Me.buttonSortAscending.MinimumSize = New Size(0, 34)
            Me.buttonSortAscending.Name = "buttonSortAscending"
            ' 
            ' 
            ' 
            Me.buttonSortAscending.RootElement.AutoToolTip = True
            Me.buttonSortAscending.RootElement.MinSize = New Size(0, 34)
            Me.buttonSortAscending.RootElement.ToolTipText = "Sort Ascending"
            Me.buttonSortAscending.Size = New Size(26, 34)
            Me.buttonSortAscending.TabIndex = 8
            Me.buttonSortAscending.Text = "radButton1"
            CType(Me.buttonSortAscending.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Image = (CType(resources.GetObject("resource.Image"), Image))
            CType(Me.buttonSortAscending.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).DisplayStyle = Telerik.WinControls.DisplayStyle.Image
            CType(Me.buttonSortAscending.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "radButton1"
            CType(Me.buttonSortAscending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor2 = Color.Transparent
            CType(Me.buttonSortAscending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor3 = Color.Transparent
            CType(Me.buttonSortAscending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor4 = Color.Transparent
            CType(Me.buttonSortAscending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = Color.Transparent
            CType(Me.buttonSortAscending.GetChildAt(0).GetChildAt(2), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            ' 
            ' buttonSortDescending
            ' 
            Me.buttonSortDescending.BackColor = Color.Transparent
            Me.buttonSortDescending.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
            Me.buttonSortDescending.Image = (CType(resources.GetObject("buttonSortDescending.Image"), Image))
            Me.buttonSortDescending.Location = New Point(208, 98)
            Me.buttonSortDescending.MinimumSize = New Size(0, 34)
            Me.buttonSortDescending.Name = "buttonSortDescending"
            ' 
            ' 
            ' 
            Me.buttonSortDescending.RootElement.AutoToolTip = True
            Me.buttonSortDescending.RootElement.MinSize = New Size(0, 34)
            Me.buttonSortDescending.RootElement.ScreenTip = Me.radOffice2007ScreenTip1.ScreenTipElement
            Me.buttonSortDescending.RootElement.ShouldPaint = True
            Me.buttonSortDescending.RootElement.ToolTipText = "Sort Descending"
            Me.buttonSortDescending.Size = New Size(25, 34)
            Me.buttonSortDescending.TabIndex = 9
            Me.buttonSortDescending.Text = "radButton2"
            CType(Me.buttonSortDescending.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Image = (CType(resources.GetObject("resource.Image1"), Image))
            CType(Me.buttonSortDescending.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).DisplayStyle = Telerik.WinControls.DisplayStyle.Image
            CType(Me.buttonSortDescending.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "radButton2"
            CType(Me.buttonSortDescending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor2 = Color.Transparent
            CType(Me.buttonSortDescending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor3 = Color.Transparent
            CType(Me.buttonSortDescending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor4 = Color.Transparent
            CType(Me.buttonSortDescending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = Color.Transparent
            CType(Me.buttonSortDescending.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            CType(Me.buttonSortDescending.GetChildAt(0).GetChildAt(2), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            ' 
            ' radOffice2007ScreenTip1
            ' 
            Me.radOffice2007ScreenTip1.CaptionVisible = True
            Me.radOffice2007ScreenTip1.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
            Me.radOffice2007ScreenTip1.FooterVisible = False
            Me.radOffice2007ScreenTip1.Location = New Point(0, 0)
            Me.radOffice2007ScreenTip1.Name = "radOffice2007ScreenTip1"
            ' 
            ' 
            ' 
            Me.radOffice2007ScreenTip1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            ' 
            ' 
            ' 
            Me.radOffice2007ScreenTip1.ScreenTipElement.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
            Me.radOffice2007ScreenTip1.ScreenTipElement.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
            Me.radOffice2007ScreenTip1.ScreenTipElement.TipSize = New Size(210, 50)
            Me.radOffice2007ScreenTip1.Size = New Size(107, 43)
            Me.radOffice2007ScreenTip1.TabIndex = 0
            Me.radOffice2007ScreenTip1.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
            CType(Me.radOffice2007ScreenTip1.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "Sort Descending"
            ' 
            ' radPanelDemoHolder
            ' 
            Me.radPanelDemoHolder.BackColor = Color.Transparent
            Me.radPanelDemoHolder.BackgroundImage = My.Resources.ComboFirstLookBG
            Me.radPanelDemoHolder.BackgroundImageLayout = ImageLayout.None
            Me.radPanelDemoHolder.Controls.Add(Me.cmbArtist)
            Me.radPanelDemoHolder.Controls.Add(Me.buttonSortDescending)
            Me.radPanelDemoHolder.Controls.Add(Me.cmbAlbum)
            Me.radPanelDemoHolder.Controls.Add(Me.cmbSong)
            Me.radPanelDemoHolder.Controls.Add(Me.buttonSortAscending)
            Me.radPanelDemoHolder.Controls.Add(Me.radPanel1)
            Me.radPanelDemoHolder.Controls.Add(Me.lblSelectedSong)
            Me.radPanelDemoHolder.Controls.Add(Me.pictureBox)
            Me.radPanelDemoHolder.Location = New Point(0, 0)
            Me.radPanelDemoHolder.Name = "radPanelDemoHolder"
            Me.radPanelDemoHolder.Size = New Size(575, 381)
            Me.radPanelDemoHolder.TabIndex = 10
            CType(Me.radPanelDemoHolder.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            CType(Me.radPanelDemoHolder.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            ' 
            ' Form1
            ' 
            Me.BackColor = Color.FromArgb((CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))))
            Me.BackgroundImageLayout = ImageLayout.None
            Me.Controls.Add(Me.radPanelDemoHolder)
            Me.Location = New Point(15, 15)
            Me.Name = "Form1"
            Me.Size = New Size(1142, 516)
            Me.Controls.SetChildIndex(Me.radPanelDemoHolder, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbArtist, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.artistsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.musicCollectionDataSetBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.musicCollectionDataSet.AlbumsDataTableProperty, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.musicCollectionDataSet.SongsDataTableProperty, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.musicCollectionDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.cmbAlbum, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.artistsAlbumsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.cmbSong, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.albumsSongsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lblSelectedSong, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.buttonSortAscending, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.buttonSortDescending, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radOffice2007ScreenTip1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			Me.ResumeLayout(False)

		End Sub



		#End Region

		Private cmbArtist As Telerik.WinControls.UI.RadDropDownList
		Private cmbAlbum As Telerik.WinControls.UI.RadDropDownList
		Private cmbSong As Telerik.WinControls.UI.RadDropDownList
		Private lblSelectedSong As Telerik.WinControls.UI.RadLabel
		Private artistsBindingSource As BindingSource
		Private musicCollectionDataSetBindingSource As BindingSource
		Private musicCollectionDataSet As Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet
		Private artistsTableAdapter As Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.ArtistsTableAdapter
		Private artistsAlbumsBindingSource As BindingSource
		Private albumsSongsBindingSource As BindingSource
		Private albumsTableAdapter As Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsTableAdapter
		Private songsTableAdapter As Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.SongsTableAdapter
		Private ArtistsImageList As ImageList
		Private pictureBox As PictureBox
		Private ArtistsLargeImageList As ImageList
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		Private buttonSortAscending As Telerik.WinControls.UI.RadButton
		Private buttonSortDescending As Telerik.WinControls.UI.RadButton
		Private radOffice2007ScreenTip1 As Telerik.WinControls.UI.RadOffice2007ScreenTip
		Private radPanelDemoHolder As Telerik.WinControls.UI.RadPanel
	End Class
End Namespace
