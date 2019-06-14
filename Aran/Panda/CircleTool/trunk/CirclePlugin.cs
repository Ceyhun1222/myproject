using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.AranEnvironment;

namespace Aran.PANDA.CircleTool
{
	public class CirclePlugin : AranPlugin
	{
		const string PlaginName = "CirclePlugin";
		const string guidString = "1A27149B-0C86-44F9-9DEF-9236748249DA";

		public override Guid Id
		{
			get { return new Guid(guidString); }
		}

		public override string Name
		{
			get { return PlaginName; }
		}

		CircleToolbar CommandBar;

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalVars.gAranEnv = aranEnv;
			GlobalVars.gAranGraphics = aranEnv.Graphics;
			GlobalVars.Initialised = false;

			CommandBar = new CircleToolbar();
			Toolbar = CommandBar.Toolbar;

			//aranEnv.AranUI.AddMapTool(_aranTool);
			IsSystemPlugin = true;
		}

		public override List<FeatureType> GetLayerFeatureTypes()
		{
			var list = new List<FeatureType>();
			list.Add(FeatureType.AirportHeliport);
			return list;
		}

	}
}
