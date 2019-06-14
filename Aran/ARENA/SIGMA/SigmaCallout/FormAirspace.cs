using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;

namespace SigmaCallout
{
	public partial class FormAirspace : Form
	{
		public void AddSymbolChangedHandler(EventHandler eventHandler)
		{
			txtBxAirspaceClass.TextChanged += eventHandler;
		}

		public void AddBackColorChangedHandler (EventHandler eventHandler)
		{
			pnlAirspaceBackColor.BackColorChanged += eventHandler;
		}

		public void AddLocationChangedHandler ( EventHandler eventHandler )
		{
			radBtnLeft.CheckedChanged += eventHandler;
		}

		public FormAirspace ( string symbol, IRgbColor color, bool onLeftSide )
		{
			InitializeComponent ( );
			this.SuspendLayout ( );
			txtBxAirspaceClass.Text = symbol;
			pnlAirspaceBackColor.BackColor = System.Drawing.Color.FromArgb ( color.Transparency, color.Red, color.Green, color.Blue );
			if ( onLeftSide )
				radBtnLeft.Checked = true;
			else
				radBtnRight.Checked = true;
			this.ResumeLayout ( false );
			this.PerformLayout ( );
		}

		private void btnAirspaceBackColor_Click ( object sender, EventArgs e )
		{
			IColorBrowser colorBrowser = new ColorBrowserClass ( );
			IRgbColor esriColor = new RgbColor ( );
			esriColor.RGB = pnlAirspaceBackColor.BackColor.ToArgb ( );
			colorBrowser.Color = esriColor;
			if ( colorBrowser.DoModal ( this.Handle.ToInt32 ( ) ) )
			{
				esriColor = ( IRgbColor ) colorBrowser.Color;
				pnlAirspaceBackColor.BackColor = System.Drawing.Color.FromArgb ( esriColor.Transparency, esriColor.Red, esriColor.Green, esriColor.Blue );
			}
		}

		private void btnClose_Click ( object sender, EventArgs e )
		{
			this.Close ( );
		}
	}
}