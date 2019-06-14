using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.PANDA.CircleTool
{
	class GlobalVars
	{
		public static IAranEnvironment gAranEnv;
		public static IAranGraphics gAranGraphics;
		public static Constants.Constants constants;

		public static Settings settings;
		public static UnitConverter unitConverter;
		public static ADHPType CurrADHP;

		public static SpatialReferenceOperation pspatialReferenceOperation;
		public static SpatialReference pSpRefPrj;
		public static SpatialReference pSpRefGeo;

		public static string UserName;
		public static int p15NMCircleElem;
		public static int p30NMCircleElem;
		public static bool Initialised;

		public static void InitCommand()
		{
			//HandleThreadException();
			//ARANFunctions.InitEllipsoid();

			constants = new Constants.Constants();

			settings = new Settings();
			settings.Load(gAranEnv);

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...
			unitConverter = new Aran.PANDA.Common.UnitConverter(settings);

			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			pspatialReferenceOperation = new SpatialReferenceOperation(gAranEnv);
			pSpRefPrj = pspatialReferenceOperation.SpRefPrj;

			if (pSpRefPrj.SpatialReferenceType == SpatialReferenceType.srtGeographic)
				throw new Exception("Invalid Map projection.");

			pSpRefGeo = pspatialReferenceOperation.SpRefGeo;

			DBModule.InitModule();
			//'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...'''...

			Initialised = true; 
		}

		public static void FillADHP()
		{
			settings.Load(gAranEnv);
			CurrADHP.Identifier = settings.Aeroport;
			CurrADHP.pPtGeo = null;

			DBModule.FillADHPFields(ref CurrADHP);

			if (CurrADHP.pPtGeo == null)
				throw new Exception("Initialization of ADHP failed.");
		}

	}
}
