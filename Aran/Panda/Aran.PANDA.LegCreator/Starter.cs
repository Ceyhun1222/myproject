using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.AranEnvironment;
using Aran.PANDA.Common;

namespace Aran.PANDA.LegCreator
{
	public class Starter : AranPlugin
	{
		public override Guid Id => new Guid ( "9e140359-1c24-4fd1-95f2-4297d843bec3" );

		public override string Name => "Leg creator";

		public override void Startup ( IAranEnvironment aranEnv )
		{
			var menuItem = new ToolStripMenuItem { Text = @"Leg Creator" };
			menuItem.Click += MenuItem_Click;
			aranEnv.AranUI.AddMenuItem ( AranMapMenu.Applications, menuItem );

			GlobalParams.AranEnvironment = aranEnv;
			GlobalParams.SpatialRefOperation = new SpatialReferenceOperation ( GlobalParams.AranEnvironment );
		}

		private void MenuItem_Click ( object sender, EventArgs e )
		{
            var window = new MainWindow();
            var helper = new WindowInteropHelper(window) {Owner = GlobalParams.AranEnvironment.Win32Window.Handle};
			ElementHost.EnableModelessKeyboardInterop ( window );
			window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            window.Show();
        }
	}
}