using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

using Aran.Aim.Features;
using Aran.Queries;
using Aran.Panda.Common;

using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;

namespace Aran.Panda.EnrouteStar
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum SensorType
	{
		stGNSS,
		stDME_DME
	}

	/*
	[System.Runtime.InteropServices.ComVisible(false)]
	public class LegFIX
	{
		WPT_FIXType _WPT_FIX;

		public WPT_FIXType WPT_FIX { get { return _WPT_FIX; } }

		public Point pPtGeo
		{
			get { return _WPT_FIX.pPtGeo; }
			set { _WPT_FIX.pPtGeo = value; }
		}

		public Point pPtPrj
		{
			get { return _WPT_FIX.pPtPrj; }
			set { _WPT_FIX.pPtPrj = value; }
		}

		public string Name
		{
			get { return _WPT_FIX.Name; }
			set { _WPT_FIX.Name = value; }
		}

		public string CallSign
		{
			get { return _WPT_FIX.CallSign; }
			set { _WPT_FIX.CallSign = value; }
		}

		public eNavaidType TypeCode { get { return _WPT_FIX.TypeCode; } }

		public Feature pFeature { get { return _WPT_FIX.pFeature; } }
		public Guid Identifier { get { return _WPT_FIX.Identifier; } }
		public double MagVar { get { return _WPT_FIX.MagVar; } }
		public long Tag { get { return _WPT_FIX.Tag; } }

		public double InDirection, OutDirection;

		public LegFIX(WPT_FIXType wptFix)
		{
			_WPT_FIX = wptFix;
			InDirection = OutDirection = 0.0;
		}

		public LegFIX(LegFIX another)
		{
			_WPT_FIX = another._WPT_FIX;
			InDirection = another.InDirection;
			OutDirection = another.OutDirection;
		}
	}
	*/
}
