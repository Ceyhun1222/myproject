using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CDOTMA.Utilities;

namespace CDOTMA.CoordinatSystems
{
	#region Interpreter

	public class ProjectionLoader
	{
		//private static class Sequences
		//{
		//    public static readonly PrjToken[][] Sequence = new PrjToken[][] {
		//    new PrjToken[]{TokenBase.NONE},
		//    new PrjToken[]{PrjToken.PROJECTION, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.RBRA},									//	PROJECTION["Transverse_Mercator"]
		//    new PrjToken[]{PrjToken.UNIT, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, TokenBase.NUMBER, PrjToken.RBRA},				//	UNIT["Meter", 1.0]
		//    new PrjToken[]{PrjToken.PARAMETER, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, TokenBase.NUMBER, PrjToken.RBRA},			//	PARAMETER["Scale_Factor", 0.9996]
		//    new PrjToken[]{PrjToken.PRIMEM, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, TokenBase.NUMBER, PrjToken.RBRA},			//	PRIMEM["Greenwich",0.0]
		//    new PrjToken[]{PrjToken.SPHEROID, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, TokenBase.NUMBER,
		//                                                PrjToken.COMMA, TokenBase.NUMBER, PrjToken.RBRA},							//	SPHEROID["GRS_1980",6378137.0,298.257222101]
		//    new PrjToken[]{PrjToken.DATUM, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, PrjToken.SPHEROID, PrjToken.RBRA},			//	DATUM["D_Latvia_1992",	SPHEROID["GRS_1980",6378137.0,298.257222101]]
		//    new PrjToken[]{PrjToken.GEOGCS, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, PrjToken.DATUM, PrjToken.COMMA,
		//                                                PrjToken.PRIMEM, PrjToken.COMMA, PrjToken.UNIT, PrjToken.RBRA},				//	GEOGCS["GCS_LKS_1992", DATUM["D_Latvia_1992", SPHEROID["GRS_1980",6378137.0,298.257222101]], PRIMEM["Greenwich",0.0], UNIT["Degree",0.0174532925199433]]

		//    new PrjToken[]{PrjToken.PROJCS, PrjToken.LBRA, TokenBase.QUOTED_WORD, PrjToken.COMMA, PrjToken.GEOGCS, PrjToken.COMMA,			//	PROJCS["LKS_1992_Latvia_TM", 	GEOGCS[	"GCS_LKS_1992",	DATUM["D_Latvia_1992",	SPHEROID["GRS_1980",6378137.0,298.257222101]],	PRIMEM["Greenwich",0.0], UNIT["Degree",0.0174532925199433]],
		//        PrjToken.PROJECTION, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA,				//  PROJECTION["Transverse_Mercator"], PARAMETER["False_Easting",500000.0], PARAMETER["False_Northing",-6000000.0],
		//        PrjToken.PARAMETER, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA,				// PARAMETER["Central_Meridian",24.0], PARAMETER["Scale_Factor",0.9996], PARAMETER["Latitude_Of_Origin",0.0],
		//                                                PrjToken.UNIT, PrjToken.RBRA}											//  UNIT["Meter",1.0]]

		//    //new int[]{Token.PROJCS,  Token.LBRA, Token.QUOTED_WORD, Token.GEOGCS,										//	PROJCS["LKS_1992_Latvia_TM", 	GEOGCS[	"GCS_LKS_1992",	DATUM["D_Latvia_1992",	SPHEROID["GRS_1980",6378137.0,298.257222101]],	PRIMEM["Greenwich",0.0], UNIT["Degree",0.0174532925199433]],
		//    //    Token.PROJECTION, Token.ARRAYOF, Token.PARAMETER,														//  PROJECTION["Transverse_Mercator"], PARAMETER["False_Easting",500000.0], PARAMETER["False_Northing",-6000000.0], PARAMETER["Central_Meridian",24.0],
		//    //    Token.UNIT, Token.RBRA}																				//  PARAMETER["Scale_Factor",0.9996], PARAMETER["Latitude_Of_Origin",0.0], UNIT["Meter",1.0]]
		//    };
		//}

		private static class Sequences
		{
			public static readonly int[][] Sequence = new int[][] {
			new int[]{BaseToken.NONE},
			new int[]{PrjToken.PROJECTION, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.RBRA},									//	PROJECTION["Transverse_Mercator"]
			new int[]{PrjToken.UNIT, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, BaseToken.NUMBER, PrjToken.RBRA},		//	UNIT["Meter", 1.0]
			new int[]{PrjToken.PARAMETER, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, BaseToken.NUMBER, PrjToken.RBRA},	//	PARAMETER["Scale_Factor", 0.9996]
			new int[]{PrjToken.PRIMEM, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, BaseToken.NUMBER, PrjToken.RBRA},		//	PRIMEM["Greenwich",0.0]
			new int[]{PrjToken.SPHEROID, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, BaseToken.NUMBER,
														PrjToken.COMMA, BaseToken.NUMBER, PrjToken.RBRA},							//	SPHEROID["GRS_1980",6378137.0,298.257222101]
			new int[]{PrjToken.DATUM, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, PrjToken.SPHEROID, PrjToken.RBRA},		//	DATUM["D_Latvia_1992",	SPHEROID["GRS_1980",6378137.0,298.257222101]]
			new int[]{PrjToken.GEOGCS, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, PrjToken.DATUM, PrjToken.COMMA,
														PrjToken.PRIMEM, PrjToken.COMMA, PrjToken.UNIT, PrjToken.RBRA},				//	GEOGCS["GCS_LKS_1992", DATUM["D_Latvia_1992", SPHEROID["GRS_1980",6378137.0,298.257222101]], PRIMEM["Greenwich",0.0], UNIT["Degree",0.0174532925199433]]

			new int[]{PrjToken.PROJCS, PrjToken.LBRA, BaseToken.QUOTED_WORD, PrjToken.COMMA, PrjToken.GEOGCS, PrjToken.COMMA,		//	PROJCS["LKS_1992_Latvia_TM", 	GEOGCS[	"GCS_LKS_1992",	DATUM["D_Latvia_1992",	SPHEROID["GRS_1980",6378137.0,298.257222101]],	PRIMEM["Greenwich",0.0], UNIT["Degree",0.0174532925199433]],
			    PrjToken.PROJECTION, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA,		//  PROJECTION["Transverse_Mercator"], PARAMETER["False_Easting",500000.0], PARAMETER["False_Northing",-6000000.0],
				PrjToken.PARAMETER, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA, PrjToken.PARAMETER, PrjToken.COMMA,			// PARAMETER["Central_Meridian",24.0], PARAMETER["Scale_Factor",0.9996], PARAMETER["Latitude_Of_Origin",0.0],
														PrjToken.UNIT, PrjToken.RBRA}												//  UNIT["Meter",1.0]]

			//new int[]{Token.PROJCS,  Token.LBRA, Token.QUOTED_WORD, Token.GEOGCS,										//	PROJCS["LKS_1992_Latvia_TM", 	GEOGCS[	"GCS_LKS_1992",	DATUM["D_Latvia_1992",	SPHEROID["GRS_1980",6378137.0,298.257222101]],	PRIMEM["Greenwich",0.0], UNIT["Degree",0.0174532925199433]],
			//    Token.PROJECTION, Token.ARRAYOF, Token.PARAMETER,														//  PROJECTION["Transverse_Mercator"], PARAMETER["False_Easting",500000.0], PARAMETER["False_Northing",-6000000.0], PARAMETER["Central_Meridian",24.0],
			//    Token.UNIT, Token.RBRA}																				//  PARAMETER["Scale_Factor",0.9996], PARAMETER["Latitude_Of_Origin",0.0], UNIT["Meter",1.0]]
			};
		}

		static Parser _parser;

		static bool LoadProjection(ref string Projection)
		{
			const int seq = 1;
			int n = Sequences.Sequence[seq].Length;
			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				if (tok == BaseToken.QUOTED_WORD)
					Projection = _parser.StrValue;

				_parser.NextToken();
			}

			return true;
		}

		static bool LoadUnit(ref Unit unit)
		{
			const int seq = 2;

			int n = Sequences.Sequence[seq].Length;
			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				if (tok == BaseToken.QUOTED_WORD)
					unit.Name = _parser.StrValue;
				else if (tok == BaseToken.NUMBER)
					unit.Multiplier = _parser.DoubleValue;

				_parser.NextToken();
			}

			return true;
		}

		static bool LoadParameter(ref Parameter parametr)
		{
			const int seq = 3;

			int n = Sequences.Sequence[seq].Length;

			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				if (tok == BaseToken.QUOTED_WORD)
					parametr.Name = _parser.StrValue;
				else if (tok == BaseToken.NUMBER)
					parametr.Value = _parser.DoubleValue;

				_parser.NextToken();
			}

			return true;
		}

		static bool LoadPrimem(ref Primem primem)
		{
			const int seq = 4;

			int n = Sequences.Sequence[seq].Length;

			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				if (tok == BaseToken.QUOTED_WORD)
					primem.Name = _parser.StrValue;
				else if (tok == BaseToken.NUMBER)
					primem.Value = _parser.DoubleValue;

				_parser.NextToken();
			}

			return true;
		}

		static bool LoadSpheroid(ref Spheroid spheroid)
		{
			const int seq = 5;

			int n = Sequences.Sequence[seq].Length;
			string spheroidName = "";
			double spheroidA = 6378137.0, spheroidF = 298.257223563;

			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				if (tok == BaseToken.QUOTED_WORD)
					spheroidName = _parser.StrValue;
				else if (tok == BaseToken.NUMBER)
				{
					if (i == 4)
						spheroidA = _parser.DoubleValue;
					else
						spheroidF = _parser.DoubleValue;
				}

				_parser.NextToken();
			}

			spheroid = new Spheroid(spheroidName, spheroidA, spheroidF);
			return true;
		}

		static bool LoadDatum(ref Datum datum)
		{
			const int seq = 6;

			int n = Sequences.Sequence[seq].Length;
			string datumName = "";
			Spheroid spheroid = null;

			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				if (tok == PrjToken.SPHEROID)
				{
					if (!LoadSpheroid(ref spheroid))
						return false;
				}
				else
				{
					if (tok == BaseToken.QUOTED_WORD)
						datumName = _parser.StrValue;
					_parser.NextToken();
				}
			}

			datum = new Datum(datumName, spheroid);
			return true;
		}

		static bool LoadGEOGCS(ref GeographicCoordinatSystem gcs)
		{
			const int seq = 7;

			int n = Sequences.Sequence[seq].Length;
			string gcsName = "";
			Datum datum = null;
			Primem primem = default(Primem);
			Unit unit = default(Unit);

			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				switch (tok)
				{
					case PrjToken.DATUM:
						if (!LoadDatum(ref datum))
							return false;
						break;

					case PrjToken.PRIMEM:
						if (!LoadPrimem(ref primem))
							return false;
						break;

					case PrjToken.UNIT:
						if (!LoadUnit(ref unit))
							return false;
						break;

					default:
						if (tok == BaseToken.QUOTED_WORD)
							gcsName = _parser.StrValue;

						_parser.NextToken();
						break;
				}
			}

			gcs = new GeographicCoordinatSystem(gcsName, datum, primem, unit);
			return true;
		}

		static bool LoadPROJCS(ref ProjectedCoordinatSystem pcs)
		{
			const int seq = 8;

			int n = Sequences.Sequence[seq].Length;
			string pcsName = "";
			string prjName = "";

			GeographicCoordinatSystem gcs = null;
			Parameter param = default(Parameter);
			Unit unit = default(Unit);

			double scaleFactor = 1.0;		// 0.9996
			double centralMeridian = 0.0;	//41.0
			double latitudeOfOrigin = 0.0;
			double falseEasting = 500000.0;
			double falseNorthing = 0.0;

			for (int i = 0; i < n; i++)
			{
				int tok;
				if ((tok = _parser.PeekToken()) != Sequences.Sequence[seq][i])
					return false;

				switch (tok)
				{
					case PrjToken.GEOGCS:
						if (!LoadGEOGCS(ref gcs))
							return false;
						break;

					case PrjToken.PROJECTION:
						if (!LoadProjection(ref prjName))
							return false;
						break;

					case PrjToken.PARAMETER:
						if (!LoadParameter(ref param))
							return false;

						if (param.Name == "Scale_Factor")
							scaleFactor = param.Value;
						else if (param.Name == "Central_Meridian")
							centralMeridian = param.Value;
						else if (param.Name == "Latitude_Of_Origin")
							latitudeOfOrigin = param.Value;
						else if (param.Name == "False_Easting")
							falseEasting = param.Value;
						else if (param.Name == "False_Northing")
							falseNorthing = param.Value;

						break;

					case PrjToken.UNIT:
						if (!LoadUnit(ref unit))
							return false;
						break;

					default:
						if (tok == BaseToken.QUOTED_WORD)
							pcsName = _parser.StrValue;

						_parser.NextToken();
						break;
				}
			}

			pcs = new ProjectedCoordinatSystem(pcsName, prjName, gcs, unit, centralMeridian, scaleFactor, falseEasting, falseNorthing, latitudeOfOrigin);
			return true;
		}

		static public CoordinatSystem Read(Stream input)
		{
			_parser = new Parser(input, 0);

			int currToken = _parser.PeekToken();
				
			if (currToken == PrjToken.GEOGCS)
			{
				GeographicCoordinatSystem gcs = null;
				LoadGEOGCS(ref gcs);
				return gcs;
			}

			if (currToken == PrjToken.PROJCS)
			{
				ProjectedCoordinatSystem pcs = null;
				LoadPROJCS(ref pcs);
				return pcs;
			}

			return null;
		}
	}

	#endregion
}
