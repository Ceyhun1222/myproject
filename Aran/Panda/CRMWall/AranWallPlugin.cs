using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Aim;


namespace Aran.PANDA.CRMWall
{
	public class AranWallPlugin : AranPlugin
	{
		private const string _name = "PANDA Conventional - CRMWall";
		private  Guid _Id = new Guid("93D5F8D6-FDC8-4531-B611-C6DB967BE31A");

		public override string Name { get { return _name; } }
		public override Guid Id { get { return _Id; } }

		//private bool _isInitCommands;
		private ToolStripMenuItem _tsmiSlice;

		#region IAranPlugin Members

		//public AranWallPlugin()
		//{
			//_isInitCommands = false;
		//}

		public override List<FeatureType> GetLayerFeatureTypes()
		{
			var list = new List<FeatureType>();

			list.Add(FeatureType.AirportHeliport);
			list.Add(FeatureType.RunwayCentrelinePoint);
			list.Add(FeatureType.DesignatedPoint);
			list.Add(FeatureType.VOR);
			list.Add(FeatureType.NDB);
			list.Add(FeatureType.DME);
			list.Add(FeatureType.VerticalStructure);

			return list;
		}

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gAranGraphics = aranEnv.Graphics;

			//Resources.Culture = Thread.CurrentThread.CurrentUICulture;

			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = "Obstacle to CRM model";

			_tsmiSlice = new ToolStripMenuItem();
			_tsmiSlice.Text = "Export tool...";
			_tsmiSlice.Tag = 0;
			_tsmiSlice.Click += Slice_Click;
			menuItem.DropDownItems.Add(_tsmiSlice);

			aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
		}

		#endregion

		//private void InitCommand()
		//{
		//	//if (_isInitCommands)
		//	//   return;

		//	GlobalVars.InitCommand();
		//	//_isInitCommands = true;
		//}

		#region Event handlers

		private void Slice_Click(object sender, EventArgs e)
		{
			if (GlobalVars.CurrCmd == 1)
				return;
			GlobalVars.CurrCmd = 0;

			//try
			//{
				GlobalVars.InitCommand();				//InitCommand();

				MainForm SliceFrm = new MainForm();

				SliceFrm.FormClosed += Form_FormClosed;

				//_tsmiSlice.Checked = true;

				SliceFrm.Show(GlobalVars.Win32Window);
				//SliceFrm.ComboBox001.Focus();
				GlobalVars.CurrCmd = 1;
			//}
			//catch (Exception ex)
			//{
			//	var tsmi = sender as ToolStripMenuItem;
			//	MessageBox.Show(ex.Message, tsmi.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}
			//finally
			//{

			//}
		}

		void Form_FormClosed(Object sender, EventArgs e)
		{
			_tsmiSlice.Checked = false;
			_tsmiSlice.Enabled = true;

			GlobalVars.CurrCmd = 0;

			sender = null;
			GC.Collect();
		}
		#endregion

	}
}
