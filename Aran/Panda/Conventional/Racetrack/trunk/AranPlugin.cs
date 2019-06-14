using System;
using Aran.AranEnvironment;
using System.Windows.Forms;

namespace Aran.Panda.Conventional.Racetrack 
{
	public class AranPlugin : IAranPlugin
	{
		MainController Controller;

		public AranPlugin ( )
		{
			
		}

		public void Startup ( IAranEnvironment aranEnv )
		{
			GlobalParams.AranEnvironment = aranEnv;

			GlobalParams.MenuItem = new ToolStripMenuItem ( );
			GlobalParams.MenuItem.Text = "Racetrack";
			GlobalParams.MenuItem.CheckOnClick = true;
			GlobalParams.MenuItem.Click += new EventHandler ( RacetrackMenuItem_Clicked );

			GlobalParams.AranEnvironment.AranUI.AddMenuItem ( AranMapMenu.Applications, GlobalParams.MenuItem );
			
			//GlobalParams.AranEnvironment.AranUI.AddApplicationMenuItem ( menuItem );
		}

		public void RacetrackMenuItem_Clicked ( object sender, EventArgs e )
		{
			if ( GlobalParams.MenuItem.Checked )
			{
				if ( Controller == null )
					Controller = new MainController ( );
				if ( Controller.Initialize ( ) )
				{
					AranTool aranToolItem = new AranTool ( );
					aranToolItem.Visible = false;
					GlobalParams.AranMapToolMenuItem = aranToolItem;
					aranToolItem.Cursor = Cursors.Cross;

					GlobalParams.AranEnvironment.AranUI.AddMapTool ( GlobalParams.AranMapToolMenuItem );

					InitHolding.FrmConventialInitial = new FormMain ( );
					GlobalParams.AranMapToolMenuItem.MouseClickedOnMap += InitHolding.FrmConventialInitial.OnMouseClickedOnMap;
					GlobalParams.AranMapToolMenuItem.Deactivated += InitHolding.FrmConventialInitial.OnDeacitvatedPointPickerTool;

					InitHolding.FrmConventialInitial.Show ( GlobalParams.AranEnvironment.Win32Window );
				}
				else
					GlobalParams.MenuItem.Checked = false;
			}
			else
			{
				if ( InitHolding.FrmConventialInitial != null )
					InitHolding.FrmConventialInitial.Close ( );
			}
		}

		#region IAranPlugin Members


		public string Name
		{
			get
			{
				return "Racetrack";
			}
		}

		public void AddChildSubMenu ( System.Collections.Generic.List<string> hierarcy )
		{
			// It doesn't have child sub menu to add
		}

		#endregion
	}
}