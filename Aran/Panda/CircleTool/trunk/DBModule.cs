using System;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.Queries;
using Aran.Queries.Panda_2;

namespace Aran.PANDA.CircleTool
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		public static IPandaSpecializedQPI pObjectDir;
		private static bool isOpen = false;

		public static void InitModule()
		{
			if (!isOpen)
			{
				Aim.Data.DbProvider dbPro = (Aim.Data.DbProvider)GlobalVars.gAranEnv.DbProvider;

				pObjectDir = PandaSQPIFactory.Create();
				Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir;
				pObjectDir.Open(dbPro);

				//var terrainDataReaderHandler = GlobalVars.gAranEnv.CommonData.GetObject("terrainDataReader") as TerrainDataReaderEventHandler;
				//if (terrainDataReaderHandler != null)
				//	pObjectDir.TerrainDataReader += terrainDataReaderHandler;

				GlobalVars.UserName = dbPro.CurrentUser.Name;

				isOpen = true;
			}
		}

		public static void CloseDB()
		{
			if (isOpen)
			{
				isOpen = false;
			}
		}

		public static int FillADHPFields(ref ADHPType CurrADHP)
		{
			AirportHeliport pADHP = pObjectDir.GetFeature(FeatureType.AirportHeliport, CurrADHP.Identifier) as Aran.Aim.Features.AirportHeliport;
			CurrADHP.pAirportHeliport = pADHP;
			if (pADHP == null)
				return -1;

			Point pPtGeo = pADHP.ARP.Geo;
			pPtGeo.Z = ConverterToSI.Convert(pADHP.ARP.Elevation, 0);

			Point pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

			if (pPtPrj.IsEmpty)
				return -1;

			CurrADHP.Name = pADHP.Designator;

			GeometryOperators geomOperators = new GeometryOperators();

			CurrADHP.pPtGeo = pPtGeo;
			CurrADHP.pPtPrj = pPtPrj;
			CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

			if (pADHP.MagneticVariation == null)
				CurrADHP.MagVar = 0.0;
			else
				CurrADHP.MagVar = pADHP.MagneticVariation.Value;

			CurrADHP.Elev = pPtGeo.Z;

			CurrADHP.ISAtC = ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);

			CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
		}
	}
}
