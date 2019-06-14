using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Data;
using Aran.Aim.Features;
using ESRI.ArcGIS.Carto;
using System.IO;

namespace MapEnv.Forms
{
	public partial class FormManageAerodrome : Form
	{
		private DbProvider _dbProvider;
		public IRasterLayer RasterLayer;

		public FormManageAerodrome ( DbProvider dbProvider )
		{
			InitializeComponent ( );
			_dbProvider = dbProvider;
		}

		private void FormManageAerodrome_Load ( object sender, EventArgs e )
		{
			GettingResult getResult = _dbProvider.GetVersionsOf ( Aran.Aim.FeatureType.AirportHeliport, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE );

			if ( getResult.IsSucceed )
			{
				List<AirportHeliport> airportList = getResult.List as List<AirportHeliport>;
				airportList.Sort ( delegate ( AirportHeliport arp1, AirportHeliport arp2 )
				{
					return arp1.Name.CompareTo ( arp2.Name );
				} );
				int selectedIndex = 0;
				AirportHeliport airport;
				for ( int i = 0; i < airportList.Count; i++ )
				{
					airport = airportList[ i ];
					if ( airport.Name == AirportName )
						selectedIndex = i;
					cmbBxAirport.Items.Add ( airport.Name + "  (" + airport.Designator + ")" );
				}
				cmbBxAirport.SelectedIndex = selectedIndex;
			}
		}

		private void btnOk_Click ( object sender, EventArgs e )
		{
			AirportName = cmbBxAirport.SelectedItem.ToString ( );
			AirportName = AirportName.Substring ( 0, AirportName.IndexOf ( "(" ) - 2 );
			string fileName = Path.Combine ( txtBxPath2Raster.Text, AirportName + ".jpg" );
			if ( !File.Exists ( fileName ) )
			{
				MessageBox.Show ( @"Couldn't find " + AirportName + ".jpg in '" + txtBxPath2Raster.Text + "' folder\r\n Please select airport whose raster exists !!!" );
				return;
			}
			RasterLayer = new RasterLayer ( );
			RasterLayer.CreateFromFilePath ( fileName );
			//RasterLayer.Name = airport.Name + "(" + airport.Designator + ")";
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void btnBrowse_Click ( object sender, EventArgs e )
		{
			FolderBrowserDialog folderBrowse = new FolderBrowserDialog ( );
			if ( folderBrowse.ShowDialog ( ) == System.Windows.Forms.DialogResult.OK )
			{
				txtBxPath2Raster.Text = folderBrowse.SelectedPath;
			}
		}

		private void btnCancel_Click ( object sender, EventArgs e )
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		internal DialogResult ShowDialog ( string selectedAirportName, string rasterPath )
		{
			AirportRasterPath = rasterPath;
			AirportName = selectedAirportName;
			return ShowDialog ( );
		}

		public string AirportRasterPath
		{
			get
			{
				return txtBxPath2Raster.Text;
			}

			set
			{
				txtBxPath2Raster.Text = value;
			}
		}

		public string AirportName
		{
			get;
			set;
		}
	}
}