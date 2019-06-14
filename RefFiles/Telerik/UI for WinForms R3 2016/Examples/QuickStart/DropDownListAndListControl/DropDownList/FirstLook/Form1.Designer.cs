namespace Telerik.Examples.WinControls.DropDownListAndListControl.DropDownList.FirstLook
{
	partial class Form1
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbArtist = new Telerik.WinControls.UI.RadDropDownList();
            this.artistsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.musicCollectionDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.musicCollectionDataSet = new Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet();
            this.ArtistsImageList = new System.Windows.Forms.ImageList(this.components);
            this.cmbAlbum = new Telerik.WinControls.UI.RadDropDownList();
            this.artistsAlbumsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbSong = new Telerik.WinControls.UI.RadDropDownList();
            this.albumsSongsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblSelectedSong = new Telerik.WinControls.UI.RadLabel();
            this.artistsTableAdapter = new Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.ArtistsTableAdapter();
            this.albumsTableAdapter = new Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsTableAdapter();
            this.songsTableAdapter = new Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.SongsTableAdapter();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.ArtistsLargeImageList = new System.Windows.Forms.ImageList(this.components);
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.buttonSortAscending = new Telerik.WinControls.UI.RadButton();
            this.buttonSortDescending = new Telerik.WinControls.UI.RadButton();
            this.radOffice2007ScreenTip1 = new Telerik.WinControls.UI.RadOffice2007ScreenTip();
            this.radPanelDemoHolder = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbArtist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.artistsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSet.TableAlbumsDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSet.TableSongsDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.artistsAlbumsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumsSongsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSelectedSong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonSortAscending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonSortDescending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radOffice2007ScreenTip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).BeginInit();
            this.radPanelDemoHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Location = new System.Drawing.Point(646, 1);
            this.settingsPanel.Size = new System.Drawing.Size(200, 723);
            this.settingsPanel.ThemeName = "ControlDefault";
            // 
            // cmbArtist
            // 
            this.cmbArtist.AutoCompleteDisplayMember = "ArtistName";
            this.cmbArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbArtist.DataSource = this.artistsBindingSource;
            this.cmbArtist.DisplayMember = "ArtistName";
            this.cmbArtist.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.cmbArtist.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbArtist.ImageList = this.ArtistsImageList;
            this.cmbArtist.Location = new System.Drawing.Point(18, 103);
            this.cmbArtist.Name = "cmbArtist";
            this.cmbArtist.NullText = "Select Artist";
            // 
            // 
            // 
            this.cmbArtist.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cmbArtist.ShowImageInEditorArea = false;
            this.cmbArtist.Size = new System.Drawing.Size(170, 20);
            this.cmbArtist.TabIndex = 1;
            // 
            // artistsBindingSource
            // 
            this.artistsBindingSource.DataMember = "Artists";
            this.artistsBindingSource.DataSource = this.musicCollectionDataSetBindingSource;
            // 
            // musicCollectionDataSetBindingSource
            // 
            this.musicCollectionDataSetBindingSource.DataSource = this.musicCollectionDataSet;
            this.musicCollectionDataSetBindingSource.Position = 0;
            // 
            // musicCollectionDataSet
            // 
            this.musicCollectionDataSet.DataSetName = "MusicCollectionDataSet";
            this.musicCollectionDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 
            // 
            this.musicCollectionDataSet.TableAlbumsDataTable.TableName = "AlbumsDataTable";
            // 
            // 
            // 
            this.musicCollectionDataSet.TableSongsDataTable.TableName = "SongsDataTable";
            // 
            // ArtistsImageList
            // 
            this.ArtistsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ArtistsImageList.ImageStream")));
            this.ArtistsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ArtistsImageList.Images.SetKeyName(0, "smallSting.jpg");
            this.ArtistsImageList.Images.SetKeyName(1, "smallDepeche.jpg");
            this.ArtistsImageList.Images.SetKeyName(2, "smallSheryl.jpg");
            this.ArtistsImageList.Images.SetKeyName(3, "smallClapton.jpg");
            this.ArtistsImageList.Images.SetKeyName(4, "smallFloyd.jpg");
            this.ArtistsImageList.Images.SetKeyName(5, "smallPurple.jpg");
            this.ArtistsImageList.Images.SetKeyName(6, "smallINXS.jpg");
            this.ArtistsImageList.Images.SetKeyName(7, "smallBadu.jpg");
            this.ArtistsImageList.Images.SetKeyName(8, "smallNada.jpg");
            this.ArtistsImageList.Images.SetKeyName(9, "smallBreeders.jpg");
            this.ArtistsImageList.Images.SetKeyName(10, "smallConchords.jpg");
            this.ArtistsImageList.Images.SetKeyName(11, "smallLeona.jpg");
            // 
            // cmbAlbum
            // 
            this.cmbAlbum.AutoCompleteDisplayMember = "AlbumName";
            this.cmbAlbum.DataSource = this.artistsAlbumsBindingSource;
            this.cmbAlbum.DisplayMember = "AlbumName";
            this.cmbAlbum.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.cmbAlbum.Location = new System.Drawing.Point(239, 103);
            this.cmbAlbum.Name = "cmbAlbum";
            this.cmbAlbum.NullText = "Select Album";
            // 
            // 
            // 
            this.cmbAlbum.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cmbAlbum.Size = new System.Drawing.Size(149, 20);
            this.cmbAlbum.TabIndex = 2;
            this.cmbAlbum.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // artistsAlbumsBindingSource
            // 
            this.artistsAlbumsBindingSource.DataMember = "ArtistsAlbums";
            this.artistsAlbumsBindingSource.DataSource = this.artistsBindingSource;
            // 
            // cmbSong
            // 
            this.cmbSong.AutoCompleteDisplayMember = "SongName";
            this.cmbSong.DataSource = this.albumsSongsBindingSource;
            this.cmbSong.DisplayMember = "SongName";
            this.cmbSong.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.cmbSong.Location = new System.Drawing.Point(394, 103);
            this.cmbSong.Name = "cmbSong";
            this.cmbSong.NullText = "Select Song";
            // 
            // 
            // 
            this.cmbSong.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cmbSong.Size = new System.Drawing.Size(160, 20);
            this.cmbSong.TabIndex = 3;
            this.cmbSong.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // albumsSongsBindingSource
            // 
            this.albumsSongsBindingSource.DataMember = "AlbumsSongs";
            this.albumsSongsBindingSource.DataSource = this.artistsAlbumsBindingSource;
            // 
            // lblSelectedSong
            // 
            this.lblSelectedSong.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedSong.Font = new System.Drawing.Font("Arial", 10F);
            this.lblSelectedSong.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSelectedSong.Location = new System.Drawing.Point(128, 213);
            this.lblSelectedSong.Name = "lblSelectedSong";
            this.lblSelectedSong.Size = new System.Drawing.Size(2, 2);
            this.lblSelectedSong.TabIndex = 5;
            this.lblSelectedSong.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // artistsTableAdapter
            // 
            this.artistsTableAdapter.ClearBeforeFill = true;
            // 
            // albumsTableAdapter
            // 
            this.albumsTableAdapter.ClearBeforeFill = true;
            // 
            // songsTableAdapter
            // 
            this.songsTableAdapter.ClearBeforeFill = true;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Location = new System.Drawing.Point(31, 183);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(82, 83);
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            // 
            // ArtistsLargeImageList
            // 
            this.ArtistsLargeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ArtistsLargeImageList.ImageStream")));
            this.ArtistsLargeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ArtistsLargeImageList.Images.SetKeyName(0, "sting.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(1, "depeche.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(2, "sheryl.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(3, "clapton.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(4, "pink_floyd.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(5, "deepurple.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(6, "inxs.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(7, "badu.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(8, "nada_surf.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(9, "breeders.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(10, "concords.jpg");
            this.ArtistsLargeImageList.Images.SetKeyName(11, "leona_lewis.jpg");
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.Color.Transparent;
            this.radPanel1.ForeColor = System.Drawing.Color.Black;
            this.radPanel1.Location = new System.Drawing.Point(472, 92);
            this.radPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(22, 18);
            this.radPanel1.TabIndex = 7;
            this.radPanel1.Visible = false;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // buttonSortAscending
            // 
            this.buttonSortAscending.BackColor = System.Drawing.Color.Transparent;
            this.buttonSortAscending.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.buttonSortAscending.ForeColor = System.Drawing.Color.Black;
            this.buttonSortAscending.Image = ((System.Drawing.Image)(resources.GetObject("buttonSortAscending.Image")));
            this.buttonSortAscending.Location = new System.Drawing.Point(188, 98);
            this.buttonSortAscending.MinimumSize = new System.Drawing.Size(0, 34);
            this.buttonSortAscending.Name = "buttonSortAscending";
            // 
            // 
            // 
            this.buttonSortAscending.RootElement.AutoToolTip = true;
            this.buttonSortAscending.RootElement.MinSize = new System.Drawing.Size(0, 34);
            this.buttonSortAscending.RootElement.ToolTipText = "Sort Ascending";
            this.buttonSortAscending.Size = new System.Drawing.Size(26, 34);
            this.buttonSortAscending.TabIndex = 8;
            this.buttonSortAscending.Text = "radButton1";
            ((Telerik.WinControls.UI.RadButtonElement)(this.buttonSortAscending.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.buttonSortAscending.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            ((Telerik.WinControls.UI.RadButtonElement)(this.buttonSortAscending.GetChildAt(0))).Text = "radButton1";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortAscending.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortAscending.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortAscending.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortAscending.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.buttonSortAscending.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // buttonSortDescending
            // 
            this.buttonSortDescending.BackColor = System.Drawing.Color.Transparent;
            this.buttonSortDescending.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.buttonSortDescending.ForeColor = System.Drawing.Color.Black;
            this.buttonSortDescending.Image = ((System.Drawing.Image)(resources.GetObject("buttonSortDescending.Image")));
            this.buttonSortDescending.Location = new System.Drawing.Point(208, 98);
            this.buttonSortDescending.MinimumSize = new System.Drawing.Size(0, 34);
            this.buttonSortDescending.Name = "buttonSortDescending";
            // 
            // 
            // 
            this.buttonSortDescending.RootElement.AutoToolTip = true;
            this.buttonSortDescending.RootElement.MinSize = new System.Drawing.Size(0, 34);
            this.buttonSortDescending.RootElement.ScreenTip = this.radOffice2007ScreenTip1.ScreenTipElement;
            this.buttonSortDescending.RootElement.ShouldPaint = true;
            this.buttonSortDescending.RootElement.ToolTipText = "Sort Descending";
            this.buttonSortDescending.Size = new System.Drawing.Size(25, 34);
            this.buttonSortDescending.TabIndex = 9;
            this.buttonSortDescending.Text = "radButton2";
            ((Telerik.WinControls.UI.RadButtonElement)(this.buttonSortDescending.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.buttonSortDescending.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            ((Telerik.WinControls.UI.RadButtonElement)(this.buttonSortDescending.GetChildAt(0))).Text = "radButton2";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortDescending.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortDescending.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortDescending.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortDescending.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.buttonSortDescending.GetChildAt(0).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.buttonSortDescending.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // radOffice2007ScreenTip1
            // 
            this.radOffice2007ScreenTip1.CaptionVisible = true;
            this.radOffice2007ScreenTip1.Description = "Override this property and provide custom screentip template description in Desig" +
    "nTime.";
            this.radOffice2007ScreenTip1.FooterVisible = false;
            this.radOffice2007ScreenTip1.Location = new System.Drawing.Point(0, 0);
            this.radOffice2007ScreenTip1.Name = "radOffice2007ScreenTip1";
            // 
            // 
            // 
            this.radOffice2007ScreenTip1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            // 
            // 
            // 
            this.radOffice2007ScreenTip1.ScreenTipElement.Description = "Override this property and provide custom screentip template description in Desig" +
    "nTime.";
            this.radOffice2007ScreenTip1.ScreenTipElement.TemplateType = typeof(Telerik.WinControls.UI.RadOffice2007ScreenTipElement);
            this.radOffice2007ScreenTip1.ScreenTipElement.TipSize = new System.Drawing.Size(210, 50);
            this.radOffice2007ScreenTip1.Size = new System.Drawing.Size(107, 43);
            this.radOffice2007ScreenTip1.TabIndex = 0;
            this.radOffice2007ScreenTip1.TemplateType = typeof(Telerik.WinControls.UI.RadOffice2007ScreenTipElement);
            ((Telerik.WinControls.UI.RadLabelElement)(this.radOffice2007ScreenTip1.GetChildAt(0).GetChildAt(2).GetChildAt(0))).Text = "Sort Descending";
            // 
            // radPanelDemoHolder
            // 
            this.radPanelDemoHolder.BackColor = System.Drawing.Color.Transparent;
            this.radPanelDemoHolder.BackgroundImage = global::Telerik.Examples.WinControls.Properties.Resources.ComboFirstLookBG;
            this.radPanelDemoHolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radPanelDemoHolder.Controls.Add(this.cmbArtist);
            this.radPanelDemoHolder.Controls.Add(this.buttonSortDescending);
            this.radPanelDemoHolder.Controls.Add(this.cmbAlbum);
            this.radPanelDemoHolder.Controls.Add(this.cmbSong);
            this.radPanelDemoHolder.Controls.Add(this.buttonSortAscending);
            this.radPanelDemoHolder.Controls.Add(this.radPanel1);
            this.radPanelDemoHolder.Controls.Add(this.lblSelectedSong);
            this.radPanelDemoHolder.Controls.Add(this.pictureBox);
            this.radPanelDemoHolder.ForeColor = System.Drawing.Color.Black;
            this.radPanelDemoHolder.Location = new System.Drawing.Point(0, 0);
            this.radPanelDemoHolder.Name = "radPanelDemoHolder";
            this.radPanelDemoHolder.Size = new System.Drawing.Size(575, 381);
            this.radPanelDemoHolder.TabIndex = 10;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelDemoHolder.GetChildAt(0).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelDemoHolder.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.radPanelDemoHolder);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1226, 663);
            this.Controls.SetChildIndex(this.themePanel, 0);
            this.Controls.SetChildIndex(this.radPanelDemoHolder, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbArtist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.artistsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSet.TableAlbumsDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSet.TableSongsDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicCollectionDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAlbum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.artistsAlbumsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumsSongsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSelectedSong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonSortAscending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonSortDescending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radOffice2007ScreenTip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).EndInit();
            this.radPanelDemoHolder.ResumeLayout(false);
            this.radPanelDemoHolder.PerformLayout();
            this.ResumeLayout(false);

		}

        

		#endregion

		private Telerik.WinControls.UI.RadDropDownList cmbArtist;
		private Telerik.WinControls.UI.RadDropDownList cmbAlbum;
        private Telerik.WinControls.UI.RadDropDownList cmbSong;
		private Telerik.WinControls.UI.RadLabel lblSelectedSong;
		private System.Windows.Forms.BindingSource artistsBindingSource;
		private System.Windows.Forms.BindingSource musicCollectionDataSetBindingSource;
		private Telerik.Examples.WinControls.DataSources.MusicCollectionDataSet musicCollectionDataSet;
		private Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.ArtistsTableAdapter artistsTableAdapter;
		private System.Windows.Forms.BindingSource artistsAlbumsBindingSource;
		private System.Windows.Forms.BindingSource albumsSongsBindingSource;
		private Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.AlbumsTableAdapter albumsTableAdapter;
		private Telerik.Examples.WinControls.DataSources.MusicCollectionDataSetTableAdapters.SongsTableAdapter songsTableAdapter;
		private System.Windows.Forms.ImageList ArtistsImageList;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.ImageList ArtistsLargeImageList;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton buttonSortAscending;
        private Telerik.WinControls.UI.RadButton buttonSortDescending;
        private Telerik.WinControls.UI.RadOffice2007ScreenTip radOffice2007ScreenTip1;
        private Telerik.WinControls.UI.RadPanel radPanelDemoHolder;
	}
}
