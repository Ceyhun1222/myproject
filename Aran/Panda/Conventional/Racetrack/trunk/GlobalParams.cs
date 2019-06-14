using Aran.Geometries.Operators;
using Aran.AranEnvironment;
using System.Collections.Generic;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	internal static class GlobalParams
	{
		public static IAranEnvironment AranEnvironment
		{
			get;
			set;
		}

		public static AranTool AranMapToolMenuItem
		{
			get;
			set;
		}

		public static GeometryOperators GeomOperators
		{
			get;
			set;
		}

		public static SpatialReferenceOperation SpatialRefOperation
		{
			get;
			set;
		}

		public static UnitConverter UnitConverter
		{
			get;
			set;
		}

		public static Settings Settings
		{
			get;
			set;
		}

		public static DbModule Database
		{
			get;
			set;
		}
		
		public static Constants.Constants ConstantG
		{
			get;
			set;
		}

		public static NavaidsDataBase NavaidDatabase
		{
			get;
			set;
			//{
			//    return _navaid_database;
			//}
		}

		public static bool IsTestVersion
		{
			get;
			set;
		}

		public static double SpiralParameterR { get; set; }
		public static double SpiralParameterE45 { get; set; }
	    public static ILogger Logger { get; set; }

	    public static void ClearTestVersionElements ( )
		{
			foreach ( int handle in TestVersionHandles )
			{
				AranEnvironment.Graphics.SafeDeleteGraphic ( handle );
			}
			TestVersionHandles.Clear ( );
		}

		public static List<int> TestVersionHandles = new List<int> ( );

		//public static ToolStripMenuItem MenuItem
		//{
		//    get;
		//    set;
		//}
	}
}
