using Aran.AranEnvironment;
using Aran.PANDA.Constants;
using Aran.PANDA.Common;
namespace Holding
{
	public static class GlobalParams
	{
		public static IAranGraphics UI { get; set; }
		public static Aran.Geometries.Operators.GeometryOperators GeomOperators { get; set; }
		public static SpatialReferenceOperation SpatialRefOperation { get; set; }
        public static Aran.PANDA.Common.Settings AranSettings { get; set; }
      //  public static IPandaAranExtension AranExtension {get;set;}
		public static DBModule Database { get; set; }
		public static Constants Constant_G { get; set; }
        public static IAranEnvironment AranEnvironment { get; set; }
        public static double Radius { get; set; }
        public static AranTool AranMapToolMenuItem { get; set; }
	}   
}
