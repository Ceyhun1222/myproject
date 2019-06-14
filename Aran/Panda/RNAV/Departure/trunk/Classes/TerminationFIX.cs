using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Geometries;
using Aran.AranEnvironment;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class TerminationFIX : WayPoint
	{
		public TerminationFIX(IAranEnvironment aranEnv)
			: base(aranEnv)
		{
			_terminationType = TerminationType.AtHeight;
			_FlyMode = eFlyMode.Atheight;
		}

		public TerminationFIX(eFIXRole InitialRole, IAranEnvironment aranEnv)
			: base(InitialRole, aranEnv)
		{
			_terminationType = TerminationType.AtHeight;
			_FlyMode = eFlyMode.Atheight;
		}

		public TerminationFIX(eFIXRole InitialRole, WPT_FIXType fix, IAranEnvironment aranEnv)
			: base(InitialRole, fix, aranEnv)
		{
			_terminationType = TerminationType.AtHeight;
			_FlyMode = eFlyMode.Atheight;
		}

		private TerminationType _terminationType;
		public TerminationType terminationType
		{
			get { return _terminationType; }

			set
			{
				if (value != _terminationType)
				{
					_UI.SafeDeleteGraphic(_PointElement);
					_terminationType = value;
	
					if (_terminationType == TerminationType.AtHeight)
					{
						_UI.SafeDeleteGraphic(_ToleranceElement);
						_FlyMode = eFlyMode.Atheight;
					}
					else
					{
						if (_FlyMode == eFlyMode.Atheight)
							_FlyMode = eFlyMode.Flyby;
					}

					//if (_DrawingEnabled)
						RefreshGraphics();
				}
			}
		}

		public override void RefreshGraphics()
		{
			if (_terminationType != TerminationType.AtHeight)
			{
				base.RefreshGraphics();
				return;
			}

			String Text;
			DeleteGraphics();

			//_UI.SafeDeleteGraphic(_PointElement);
			//_UI.SafeDeleteGraphic(_ptHtElem);

			if (!_DrawingEnabled)
				return;

			Text = SensorConstant.FIXRoleStyleNames[(int)_FIXRole];
			if (_Name != "")
				Text = Text + " / " + _Name;

			_PointElement = _UI.DrawPointWithText(_PrjPt, Text, _PointSymbol);
			_ptHtElem = _UI.DrawPointWithText(PrjPtH, "HT", ARANFunctions.RGB(127, 255, 0));
		}

		protected override void CreateTolerArea()
		{
			if (_terminationType == TerminationType.AtHeight)
			{
				base.CreateTolerArea();
				return;
			}

			//spanWidth = 0.5*SemiWidth;
			//if (_FIXRole < eFIXRole.MAPt_)
			//	dir = _OutDir;
			//else

			double dir = _EntryDir;

			Ring ring = new Ring();

			Point point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_ATT, -_XTT);
			ring.Add(point);

			point = ARANFunctions.LocalToPrj(_PrjPt, dir, -_ATT, _XTT);
			ring.Add(point);

			point = ARANFunctions.LocalToPrj(_PrjPt, dir, _ATT, _XTT);
			ring.Add(point);

			point = ARANFunctions.LocalToPrj(_PrjPt, dir, _ATT, -_XTT);
			ring.Add(point);

			Polygon polygon = new Polygon();
			polygon.ExteriorRing = ring;

			_TolerArea.Clear();
			_TolerArea.Add(polygon);
		}
	}
}

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/