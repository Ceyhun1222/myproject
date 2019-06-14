Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Carousel.FirstLook
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
            Dim CarouselBezierPath1 As Telerik.WinControls.UI.CarouselBezierPath = New Telerik.WinControls.UI.CarouselBezierPath()
            Dim ThemeSource1 As Telerik.WinControls.ThemeSource = New Telerik.WinControls.ThemeSource()
            Me.radCarouselAlbums = New Telerik.WinControls.UI.RadCarousel()
            Me.albumsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.musicCollectionDataSet = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet()
            Me.roundRectShape1 = New Telerik.WinControls.RoundRectShape(Me.components)
            Me.albumsTableAdapter = New Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsTableAdapter()
            Me.radTitleBar1 = New Telerik.WinControls.UI.RadTitleBar()
            Me.radBtnDownloads = New Telerik.WinControls.UI.RadButton()
            Me.radBtnArtists = New Telerik.WinControls.UI.RadButton()
            Me.radBtnAlbums = New Telerik.WinControls.UI.RadButton()
            Me.radBtnSongs = New Telerik.WinControls.UI.RadButton()
            Me.radComboSearch = New Telerik.WinControls.UI.RadDropDownList()
            Me.RadThemeManager1 = New Telerik.WinControls.RadThemeManager()
            CType(Me.radCarouselAlbums, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.albumsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.musicCollectionDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.musicCollectionDataSet.AlbumsDataTableProperty, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.musicCollectionDataSet.SongsDataTableProperty, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radBtnDownloads, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radBtnArtists, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radBtnAlbums, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radBtnSongs, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radComboSearch, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'radCarouselAlbums
            '
            Me.radCarouselAlbums.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.radCarouselAlbums.BackColor = System.Drawing.Color.Transparent
            Me.radCarouselAlbums.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            CarouselBezierPath1.CtrlPoint1 = New Telerik.WinControls.UI.Point3D(12.2786304604486R, 48.1662591687042R, 300.0R)
            CarouselBezierPath1.CtrlPoint2 = New Telerik.WinControls.UI.Point3D(87.8394332939787R, 47.6772616136919R, 300.0R)
            CarouselBezierPath1.FirstPoint = New Telerik.WinControls.UI.Point3D(6.3754427390791R, 50.8557457212714R, -400.0R)
            CarouselBezierPath1.LastPoint = New Telerik.WinControls.UI.Point3D(93.8606847697757R, 51.1002444987775R, -400.0R)
            CarouselBezierPath1.ZScale = 200.0R
            Me.radCarouselAlbums.CarouselPath = CarouselBezierPath1
            Me.radCarouselAlbums.DataSource = Me.albumsBindingSource
            Me.radCarouselAlbums.ForeColor = System.Drawing.Color.White
            Me.radCarouselAlbums.Location = New System.Drawing.Point(2, 161)
            Me.radCarouselAlbums.Name = "radCarouselAlbums"
            Me.radCarouselAlbums.NavigationButtonsOffset = New System.Drawing.Size(80, 20)
            Me.radCarouselAlbums.SelectedIndex = -1
            Me.radCarouselAlbums.Size = New System.Drawing.Size(845, 404)
            Me.radCarouselAlbums.TabIndex = 0
            Me.radCarouselAlbums.ThemeName = "ControlDefault"
            Me.radCarouselAlbums.VisibleItemCount = 7
            CType(Me.radCarouselAlbums.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            CType(Me.radCarouselAlbums.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radCarouselAlbums.GetChildAt(0).GetChildAt(3), Telerik.WinControls.UI.RadRepeatButtonElement).Image = Global.My.Resources.carousel_leftArrow
            CType(Me.radCarouselAlbums.GetChildAt(0).GetChildAt(4), Telerik.WinControls.UI.RadRepeatButtonElement).Image = Global.My.Resources.carousel_rightArrow
            '
            'albumsBindingSource
            '
            Me.albumsBindingSource.DataMember = "Albums"
            Me.albumsBindingSource.DataSource = Me.musicCollectionDataSet
            '
            'musicCollectionDataSet
            '
            '
            '
            '
            Me.musicCollectionDataSet.AlbumsDataTableProperty.TableName = "AlbumsDataTable"
            Me.musicCollectionDataSet.DataSetName = "MusicCollectionDataSet"
            Me.musicCollectionDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            '
            '
            Me.musicCollectionDataSet.SongsDataTableProperty.TableName = "SongsDataTable"
            '
            'albumsTableAdapter
            '
            Me.albumsTableAdapter.ClearBeforeFill = True
            '
            'radTitleBar1
            '
            Me.radTitleBar1.BackColor = System.Drawing.Color.Transparent
            Me.radTitleBar1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
            Me.radTitleBar1.Location = New System.Drawing.Point(2, 2)
            Me.radTitleBar1.Name = "radTitleBar1"
            Me.radTitleBar1.Size = New System.Drawing.Size(846, 28)
            Me.radTitleBar1.TabIndex = 2
            Me.radTitleBar1.TabStop = False
            Me.radTitleBar1.Text = "Music Library Demo"
            CType(Me.radTitleBar1.GetChildAt(0), Telerik.WinControls.UI.RadTitleBarElement).Text = "Music Library Demo"
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.[Default]
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.Primitives.ImagePrimitive).Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.Primitives.ImagePrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadImageButtonElement).Image = Global.My.Resources.FL_min
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadImageButtonElement).Margin = New System.Windows.Forms.Padding(0, 0, -2, 0)
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1), Telerik.WinControls.UI.RadImageButtonElement).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Name = "MinimizeButtonFill"
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1).GetChildAt(2), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2), Telerik.WinControls.UI.RadImageButtonElement).Image = Global.My.Resources.FL_Max
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2), Telerik.WinControls.UI.RadImageButtonElement).Margin = New System.Windows.Forms.Padding(0, 0, -2, 0)
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2), Telerik.WinControls.UI.RadImageButtonElement).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Name = "MaximizeButtonFill"
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3), Telerik.WinControls.UI.RadImageButtonElement).Image = Global.My.Resources.FL_Close
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Name = "CloseButtonFill"
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(2), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).ForeColor = System.Drawing.Color.White
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            CType(Me.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(2), Telerik.WinControls.Primitives.TextPrimitive).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
            '
            'radBtnDownloads
            '
            Me.radBtnDownloads.BackColor = System.Drawing.Color.Transparent
            Me.radBtnDownloads.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radBtnDownloads.ForeColor = System.Drawing.Color.Black
            Me.radBtnDownloads.Location = New System.Drawing.Point(489, 47)
            Me.radBtnDownloads.Name = "radBtnDownloads"
            Me.radBtnDownloads.Size = New System.Drawing.Size(119, 34)
            Me.radBtnDownloads.TabIndex = 3
            Me.radBtnDownloads.Text = "Downloads"
            Me.radBtnDownloads.ThemeName = "MusicLibrary"
            CType(Me.radBtnDownloads.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Downloads"
            CType(Me.radBtnDownloads.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).BackColor = System.Drawing.Color.Transparent
            CType(Me.radBtnDownloads.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            CType(Me.radBtnDownloads.GetChildAt(0).GetChildAt(3), Telerik.WinControls.Primitives.FocusPrimitive).BackColor = System.Drawing.Color.Transparent
            '
            'radBtnArtists
            '
            Me.radBtnArtists.BackColor = System.Drawing.Color.Transparent
            Me.radBtnArtists.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radBtnArtists.ForeColor = System.Drawing.Color.Black
            Me.radBtnArtists.Location = New System.Drawing.Point(619, 47)
            Me.radBtnArtists.Name = "radBtnArtists"
            Me.radBtnArtists.Size = New System.Drawing.Size(77, 34)
            Me.radBtnArtists.TabIndex = 3
            Me.radBtnArtists.Text = "Artists"
            Me.radBtnArtists.ThemeName = "MusicLibrary"
            CType(Me.radBtnArtists.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Artists"
            CType(Me.radBtnArtists.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            '
            'radBtnAlbums
            '
            Me.radBtnAlbums.BackColor = System.Drawing.Color.Transparent
            Me.radBtnAlbums.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radBtnAlbums.ForeColor = System.Drawing.Color.Black
            Me.radBtnAlbums.Location = New System.Drawing.Point(705, 47)
            Me.radBtnAlbums.Name = "radBtnAlbums"
            Me.radBtnAlbums.Size = New System.Drawing.Size(77, 34)
            Me.radBtnAlbums.TabIndex = 3
            Me.radBtnAlbums.Text = "Albums"
            Me.radBtnAlbums.ThemeName = "MusicLibrary"
            CType(Me.radBtnAlbums.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Albums"
            CType(Me.radBtnAlbums.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            '
            'radBtnSongs
            '
            Me.radBtnSongs.BackColor = System.Drawing.Color.Transparent
            Me.radBtnSongs.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radBtnSongs.ForeColor = System.Drawing.Color.Black
            Me.radBtnSongs.Location = New System.Drawing.Point(786, 47)
            Me.radBtnSongs.Name = "radBtnSongs"
            Me.radBtnSongs.Size = New System.Drawing.Size(67, 34)
            Me.radBtnSongs.TabIndex = 3
            Me.radBtnSongs.Text = "Songs"
            Me.radBtnSongs.ThemeName = "MusicLibrary"
            CType(Me.radBtnSongs.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Songs"
            CType(Me.radBtnSongs.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Hidden
            '
            'radComboSearch
            '
            Me.radComboSearch.Location = New System.Drawing.Point(642, 113)
            Me.radComboSearch.Name = "radComboSearch"
            Me.radComboSearch.NullText = "Search..."
            '
            '
            '
            Me.radComboSearch.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radComboSearch.Size = New System.Drawing.Size(180, 20)
            Me.radComboSearch.TabIndex = 4
            '
            'RadThemeManager1
            '
            ThemeSource1.StorageType = Telerik.WinControls.ThemeStorageType.Resource
            ThemeSource1.ThemeLocation = "ButtonMusicLibrary.xml"
            Me.RadThemeManager1.LoadedThemes.AddRange(New Telerik.WinControls.ThemeSource() {ThemeSource1})
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.BackgroundImage = Global.My.Resources.carousel_first_look_bg
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.BorderAlignment = System.Drawing.Drawing2D.PenAlignment.Center
            Me.BorderColor = System.Drawing.SystemColors.WindowFrame
            Me.ClientSize = New System.Drawing.Size(850, 620)
            Me.Controls.Add(Me.radComboSearch)
            Me.Controls.Add(Me.radBtnSongs)
            Me.Controls.Add(Me.radBtnAlbums)
            Me.Controls.Add(Me.radBtnArtists)
            Me.Controls.Add(Me.radBtnDownloads)
            Me.Controls.Add(Me.radTitleBar1)
            Me.Controls.Add(Me.radCarouselAlbums)
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(850, 620)
            Me.MinimumSize = New System.Drawing.Size(850, 620)
            Me.Name = "Form1"
            Me.Padding = New System.Windows.Forms.Padding(2, 2, 2, 5)
            Me.Shape = Me.roundRectShape1
            Me.Text = "Music Library Demo"
            CType(Me.radCarouselAlbums, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.albumsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.musicCollectionDataSet.AlbumsDataTableProperty, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.musicCollectionDataSet.SongsDataTableProperty, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.musicCollectionDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radBtnDownloads, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radBtnArtists, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radBtnAlbums, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radBtnSongs, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radComboSearch, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private radCarouselAlbums As Telerik.WinControls.UI.RadCarousel
        Private musicCollectionDataSet As Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet
        Private roundRectShape1 As Telerik.WinControls.RoundRectShape
        Private albumsBindingSource As System.Windows.Forms.BindingSource
        Private albumsTableAdapter As Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsTableAdapter
        Private radTitleBar1 As Telerik.WinControls.UI.RadTitleBar
        Private radBtnDownloads As Telerik.WinControls.UI.RadButton
        Private radBtnArtists As Telerik.WinControls.UI.RadButton
        Private radBtnAlbums As Telerik.WinControls.UI.RadButton
        Private radBtnSongs As Telerik.WinControls.UI.RadButton
        Private radComboSearch As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents RadThemeManager1 As Telerik.WinControls.RadThemeManager
    End Class
End Namespace