using System;
using ESRI.ArcGIS.Geometry;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DecoderCode
	{
		private const uint m_NGnerators = 6;
		private const uint m_CodeLen = 36;
		private const uint m_InputRange = 11;

		//m_InputRange / m_CodeLen
		private const double m_CodeScale = 0.305555555555556;
		private const double m_Scale1 = 36.0 / 4294967296.0;
		private const double m_Scale2 = 36.0 / 65536.0;

		private static int MinV;

		private static int MaxV;
		private static uint SeedValue;
		private static uint GneratorIndex;
		private static uint[] SSArray = new uint[128];
		private static bool bFillSS;

		private static string DeCodeString(string val_Renamed)
		{
			string RetVal = "";
			for (int I = 0; I < val_Renamed.Length; I++)
			{
				RetVal = RetVal + DeCodeChar(val_Renamed[I]);
			}
			return RetVal;
		}

		private static char DeCodeChar(char val_Renamed)
		{
			int C = 0;
			C = (int)val_Renamed;
			if (C > '9')
				C = C + 10 - Generator(GneratorIndex) - 'A';
			else
				C = C - Generator(GneratorIndex) - '0';

			C = (int)Math.Round((C + m_CodeLen * (C < 0 ? 1 : 0)) * m_CodeScale);
			return (C > 0 ? (char)(C + MaxV - MinV * (C < 11 ? 1 : 0)) : '-');
		}

		private static void SetNewSeedL(uint initSeed)
		{
			uint I = 0;

			bFillSS = true;
			SeedValue = initSeed;
			for (I = 0; I <= 127; I++)
			{
				Generator(GneratorIndex);
				SSArray[I] = (SeedValue & 0xfffffffeu) | (I & 1);
			}
			bFillSS = false;
			SeedValue = initSeed;
		}

		private static void SetNewSeedS(string initString)
		{
			int D = 0;
			int I = 0;
			uint initValue = 0;
			char Ch = initString[0];

			if (Ch <= '9')
			{
				I = Ch - '0';
			}
			else
			{
				I = Ch + 10 - 'A';
			}

			GneratorIndex = (uint)I - m_CodeLen / 2;
			if (GneratorIndex < 0)
				GneratorIndex = GneratorIndex + m_CodeLen;
			if (GneratorIndex >= m_NGnerators)
				throw new Exception("Unsupported format string reached.");

			initValue = 0;
			for (I = 1; I < initString.Length; I++)
			{
				Ch = initString[I];

				if (Ch <= '9')
				{
					D = Ch - '0';
				}
				else
				{
					D = Ch + 10 - 'A';
				}
				initValue = initValue * m_CodeLen + (uint)D;
			}
			SetNewSeedL(initValue);
		}

		private static double Exp2(int val_Renamed)
		{
			double RetVal = 1.0;
			for (int I = 0; I <= val_Renamed - 1; I++)
			{
				RetVal = RetVal + RetVal;
			}
			return RetVal;
		}

		private static uint Unchecked(double val_Renamed)
		{
			int I = 0;
			int N = 0;
			double fExp = 0;
			uint RetVal = 0;

			if (val_Renamed < 2147483648.0)
			{
				RetVal = (uint)System.Math.Floor(val_Renamed);
			}
			else
			{
				N = (int)System.Math.Round(System.Math.Log(val_Renamed) * 1.44269504088896 - 0.49999);

				for (I = N; I >= 32; I += -1)
				{
					fExp = Exp2(I);
					if (val_Renamed >= fExp)
						val_Renamed = val_Renamed - fExp;
				}

				if (val_Renamed < 2147483648.0)
				{
					RetVal = (uint)System.Math.Floor(val_Renamed);
				}
				else
				{
					val_Renamed = val_Renamed - 2147483648.0;
					RetVal = (uint)System.Math.Floor(val_Renamed) | 0x80000000u;
				}
			}

			return RetVal;
		}

		private static uint ShiftDWORD24(uint val_Renamed)
		{
			int I = 0;
			uint Mask = 0;
			uint AddValue = 0;
			uint RetVal = 0;

			if ((val_Renamed & 0x80000000u) == 0x80000000u)
				RetVal = 128;

			Mask = 0x40000000u;
			AddValue = 64;
			for (I = 0; I <= 6; I++)
			{
				if ((val_Renamed & Mask) != 0)
					RetVal = RetVal + AddValue;
				Mask = Mask / 2;
				AddValue = AddValue / 2;
			}

			return RetVal;
		}

		private static uint ShiftDWORD16(uint val_Renamed)
		{
			int I = 0;
			uint Mask = 0;
			uint AddValue = 0;
			uint RetVal = 0;

			RetVal = 0;
			if ((val_Renamed & 0x80000000u) == 0x80000000u)
				RetVal = 32768;

			Mask = 0x40000000u;
			AddValue = 16384;
			for (I = 0; I <= 14; I++)
			{
				if ((val_Renamed & Mask) == Mask)
					RetVal = RetVal + AddValue;
				Mask = Mask / 2;
				AddValue = AddValue / 2;
			}

			return RetVal;
		}

		private static double DWORD2Double(uint val_Renamed)
		{
			return val_Renamed;
		}

		private static byte Generator(uint Index)
		{
			switch (Index)
			{
				case 0:
					return Generator0();
				case 1:
					return Generator1();
				case 2:
					return Generator2();
				case 3:
					return Generator3();
				case 4:
					return Generator4();
				case 5:
					return Generator5();
			}
			return 0;
		}

		private static byte Generator0()
		{
			double fTmp = 0;
			uint I = 0;
			uint K = 0;
			uint J0 = 0;
			uint J1 = 0;
			fTmp = DWORD2Double(SeedValue) * 22695477.0 + 37.0;

			if (bFillSS)
			{
				SeedValue = Unchecked(fTmp);
			}
			else
			{
				I = Unchecked(fTmp);
				J1 = ShiftDWORD24(I) & 127;
				K = 0;
				while ((SSArray[J1] == I) && (K < 128))
				{
					K = K + 1;
					J1 = (J1 + 1) & 127;
				}

				if (SSArray[J1] == I)
					SSArray[J1] = Unchecked(I + 22695477.0);

				J0 = J1 - 23;
				if (J0 < 0)
					J0 = J0 + 128;
				SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
				SSArray[J1] = I;
			}

			return Convert.ToByte(System.Math.Floor(DWORD2Double(SeedValue) * m_Scale1));
		}

		private static byte Generator1()
		{
			double fTmp = 0;
			uint I = 0;
			uint J0 = 0;
			uint J1 = 0;
			//    fTmp = (0.5 * DWORD2Double(SeedValue) + 1.4142135623731) * 2.23606797749979
			fTmp = DWORD2Double(SeedValue) * 22695461.0 + 3.14159265358979;

			if (bFillSS)
			{
				//        SeedValue = Unchecked((fTmp - Int(fTmp)) * 4706870449.79926)
				SeedValue = Unchecked(fTmp);
			}
			else
			{
				I = Unchecked(fTmp);
				J1 = ShiftDWORD24(I) & 127;
				J0 = ShiftDWORD24(SeedValue) & 127;
				//J1 - 21

				SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
				SSArray[J0] = I;
			}

			return Convert.ToByte(System.Math.Floor(DWORD2Double(SeedValue) * m_Scale1));
		}

		private static byte Generator2()
		{
			double fTmp = 0;
			uint I = 0;
			uint J0 = 0;
			uint J1 = 0;

			//    fTmp = DWORD2Double(SeedValue) * 0.318309886183791 + 3.14159265358979
			//    SeedValue = Unchecked((fTmp - Int(fTmp)) * 4896968389.19523)
			//    Generator2 = CByte(Int(ShiftDWORD24(SeedValue) * m_RangeScale))
			fTmp = DWORD2Double(SeedValue);
			fTmp = fTmp * (fTmp + 3.0);
			//+ 3.4142135623731
			if (bFillSS)
			{
				SeedValue = Unchecked(fTmp);
			}
			else
			{
				I = Unchecked(fTmp);
				J1 = ShiftDWORD24(I) & 127;
				J0 = ShiftDWORD24(SeedValue) & 127;
				SeedValue = (SSArray[J1] & 0xffff0000u) | (ShiftDWORD16(SSArray[J0]));
				SSArray[J1] = I;
			}

			return Convert.ToByte(System.Math.Floor(DWORD2Double(SeedValue) * m_Scale1));
		}

		private static byte Generator3()
		{
			double fTmp = 0;
			uint I = 0;
			uint K = 0;
			uint J0 = 0;
			uint J1 = 0;
			fTmp = DWORD2Double(SeedValue) * 22695477.0 + 37.0;

			if (bFillSS)
			{
				SeedValue = Unchecked(fTmp);
			}
			else
			{
				I = Unchecked(fTmp);
				J1 = ShiftDWORD24(I) & 127;
				K = 0;
				while ((SSArray[J1] == I) && (K < 128))
				{
					K = K + 1;
					J1 = (J1 + 63) & 127;
				}
				if (SSArray[J1] == I)
					SSArray[J1] = Unchecked(I + 22695477.0);

				J0 = J1 - 23;
				if (J0 < 0)
					J0 = J0 + 128;
				SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
				SSArray[J1] = I;
			}

			return Convert.ToByte(System.Math.Floor((SeedValue & 65535) * m_Scale2));
		}

		private static byte Generator4()
		{
			double fTmp = 0;
			uint I = 0;
			uint J0 = 0;
			uint J1 = 0;
			fTmp = DWORD2Double(SeedValue) * 22695461.0 + 3.14159265358979;

			if (bFillSS)
			{
				SeedValue = Unchecked(fTmp);
			}
			else
			{
				I = Unchecked(fTmp);
				J1 = ShiftDWORD24(I) & 127;
				J0 = ShiftDWORD24(SeedValue) & 127;
				//J1 - 21

				SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
				SSArray[J0] = I;
			}

			return Convert.ToByte(System.Math.Floor((SeedValue & 65535) * m_Scale2));
		}

		private static byte Generator5()
		{
			double fTmp = 0;
			uint I = 0;
			uint J0 = 0;
			uint J1 = 0;

			fTmp = DWORD2Double(SeedValue);
			fTmp = fTmp * (fTmp + 3.0);
			if (bFillSS)
			{
				SeedValue = Unchecked(fTmp);
			}
			else
			{
				I = Unchecked(fTmp);
				J1 = ShiftDWORD24(I) & 127;
				J0 = ShiftDWORD24(SeedValue) & 127;
				SeedValue = (SSArray[J1] & 0xffff0000u) | (ShiftDWORD16(SSArray[J0]));
				SSArray[J1] = I;
			}

			return Convert.ToByte(System.Math.Floor((SeedValue & 65535) * m_Scale2));
		}

		private static void InitModule()
		{
			SeedValue = 0xffffffffu;
			GneratorIndex = m_NGnerators;
			MaxV = 9 + 'A';
			MinV = MaxV - '0' + 1;
		}

		private static string LstStDtReader(string LST_DT, string key)
		{
			int[] intTab = new int[]
			{	26, 27, 28, 29, 30, 31, 32, 33, 34, 35,
				 0, 23, 21,  2, 11,  3,  4,  5, 16,  6,
				 7,  8, 25, 24, 17, 18,  9, 12,  1, 13,
				15, 22, 10, 20, 14, 19};

			string Res = "";

			for (int i = 0, j; i <= key.Length - 1; i++)
			{
				if (key[i] <= '9')
					j = key[i] - '0';
				else
					j = key[i] - 'A' + 10;

				string ind = intTab[j].ToString();

				if (intTab[j] < 10)
					ind = "0" + ind;
				Res = Res + ind;
			}

			return (Double.Parse(LST_DT) - Double.Parse(Res)).ToString();
		}

		public static int LstStDtWriter(string ModuleName)
		{
			//            /**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][LastStart][CRC]
			//             * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
			//             * PrgNL -кол-во букв в названии программы (3 позиции)
			//             * PRG_NAME - название программы
			//             *R - код страны (3 позиции)
			//             FC - кол-во фигур (3 позиции)
			//             СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
			//             PC - кол-во точек фигуры (6 позиций)
			//             P - точки (формат ZAAA.BBB)
			//                    P.X (7 позиций формат ZAAA.BBB)
			//                    P.Y (7 позиций формат ZAAA.BBB)
			//                    P.R (8 позиций формат AAAAAA.BB)
			//             K - ключ кодировки (5 позиций)
			//             LastStart ЗАКОДИРОВАННАЯ дата последнего запуска программы 10 позиций (после раскодирования формат DDMMYY)
			//             CRC - контрольная последовательность (8 позиций)
			//             **/

			string LCode = Functions.RegRead<String>(Registry.CurrentUser, 
						GlobalVars.PandaRegKey + "\\" +ModuleName, "Acar", "");

			// получили CRC код
			string CRCCode = LCode.Substring(LCode.Length - 8, 8);
			LCode = LCode.Substring(0, LCode.Length - 8);
	
			//получили дату последнего запуска
			string LastStart = LCode.Substring(LCode.Length - 10, 10);
			LCode = LCode.Substring(0, LCode.Length - 10);

			// проверили CRC код
			if (CRCCode != Aran.Panda.Common.CRC32.CalcCRC32(LCode))
				return -1;

			//получили ключ
			string key = LCode.Substring(LCode.Length - 5, 5);

			// сформируем новую дату "последнего запуска"
			DateTime CurrData = DateTime.Now;

			string dNow = CurrData.Day.ToString();
			string mesNow = CurrData.Month.ToString();
			string yrNow = (CurrData.Year - 2000).ToString();

			if (dNow.Length < 2)
				dNow = "0" + dNow;
			if (mesNow.Length < 2)
				mesNow = "0" + mesNow;
			if (yrNow.Length < 2)
				yrNow = "0" + yrNow;

			LastStart = LstStDtReader(dNow + mesNow + yrNow, key);

			while (LastStart.Length < 10)
				LastStart = "0" + LastStart;

			CRCCode = Aran.Panda.Common.CRC32.CalcCRC32(LCode);
			LCode = LCode + LastStart + CRCCode;

			return Functions.RegWrite(Registry.CurrentUser,
				GlobalVars.PandaRegKey + "\\" + ModuleName, "Acar", LCode);
		}

		public static IPolygon DecodeLCode(string LCode, string ModuleName)
		{
			//            /**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][CRC]
			//             * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
			//             * PrgNL -кол-во букв в названии программы (3 позиции)
			//             * PRG_NAME - название программы
			//             *R - код страны (3 позиции)
			//             FC - кол-во фигур (3 позиции)
			//             СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
			//             PC - кол-во точек фигуры (6 позиций)
			//             P - точки (формат ZAAA.BBB)
			//                    P.X (7 позиций формат ZAAA.BBB)
			//                    P.Y (7 позиций формат ZAAA.BBB)
			//                    P.R (8 позиций формат AAAAAA.BB)
			//             K - ключ кодировки (5 позиций)
			//             CRC - контрольная последовательность (8 позиций)
			//             **/



			string CountryCode = null;
			string PointsCount = null;
			string LastStart = null;
			string CRCCode = null;
			string PRG_NAME = null;
			string FigCount = null;
			string FigCode = null;
			string Drobnoe = null;
			string Celoe = null;
			string tempS = null;
			string Tarix = null;
			string PrgNL = null;
			string key = null;

			string yearKey = null;
			string mesKey = null;
			string dKey = null;

			int mesNow = 0;
			int yrNow = 0;
			int dNow = 0;
			int K = 0;
			int M = 0;
			int I = 0;
			int Ri;

			string X = null;
			string Y = null;
			string R = null;

			double TL1 = 0;
			double TL2 = 0;
			double TL = 0;
			double XX = 0;
			double YY = 0;
			double Xc = 0;
			double Yc = 0;
			double Rc = 0;

			ITopologicalOperator2 T = default(ITopologicalOperator2);
			IPointCollection Pol = default(IPointCollection);
			IPolygon pResult = default(IPolygon);
			IPoint P = default(IPoint);

			pResult = new Polygon() as IPolygon;

			if (LCode.Length <= 0)
				return pResult;

			CRCCode = LCode.Substring(LCode.Length - 8, 8);
			// получили CRC код
			LCode = LCode.Substring(0, LCode.Length - 8);

			LastStart = LCode.Substring(LCode.Length - 10, 10);
			//получили дату последнего запуска
			LCode = LCode.Substring(0, LCode.Length - 10);

			if (CRCCode != Aran.Panda.Common.CRC32.CalcCRC32(LCode))
				return pResult;
			// проверили его

			key = LCode.Substring(LCode.Length - 5, 5);
			//получили ключ
			LCode = LCode.Substring(0, LCode.Length - 5);

			InitModule();
			SetNewSeedS(key);
			string dLCode = DeCodeString(LCode);
			LCode = dLCode;
			//LCode = "311299980070651141141051180971080260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"
			//LCode = "311299980090681011120971141161171141010260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"

			//==================================================================================================
			Tarix = LCode.Substring(0, 8);
			// получили срок "годности"
			LCode = LCode.Substring(8, LCode.Length-9);

			dKey = Tarix.Substring(0, 2);
			mesKey = Tarix.Substring(2, 2);
			yearKey = Tarix.Substring(4, 4);
			TL = Convert.ToInt32(dKey) + Convert.ToInt32(mesKey) * 30.4375 + (Convert.ToInt32(yearKey) - 1899) * 365.25;
			//==================================================================================================
			LastStart = LstStDtReader(LastStart, key);

			dKey = LastStart.Substring(0, 2);
			mesKey = LastStart.Substring(2, 2);
			yearKey = LastStart.Substring(4, 4);
			TL1 = Convert.ToInt32(dKey) + Convert.ToInt32(mesKey) * 30.4375 + (Convert.ToInt32(yearKey) + (2000 - 1899)) * 365.25;

			DateTime CurrData = DateTime.Now;

			dNow = CurrData.Day;
			mesNow = CurrData.Month;
			yrNow = CurrData.Year - 1899;
			TL2 = (dNow + mesNow * 30.4375 + yrNow * 365.25);

			TL1 = (TL1 < TL2 ? TL1 : TL2);
			//==================================================================================================
			// сравним дату последнего запуска LastStart и текущую. если Текущая МЕНЬШЕ даты LastStart - вылетаем
			if (TL < TL1)
				return pResult;
			//==================================================================================================

			PrgNL = LCode.Substring(0, 3);
			// получили длину названия модуля
			LCode = LCode.Substring(3, LCode.Length);

			Ri = 1;
			PRG_NAME = "";
			tempS = LCode.Substring(0, Convert.ToInt32(PrgNL) * 3);
			//получили название модуля в символьном виде
			// теперь сконвертируем его в строку
			for (I = 1; I <= Convert.ToInt32(PrgNL); I += 1)
			{
				PRG_NAME = PRG_NAME + tempS.Substring(Ri, 3);
				Ri = Ri + 3;
			}

			if (PRG_NAME.ToLower() != ModuleName.ToLower())
				return pResult;

			LCode = LCode.Substring(Convert.ToInt32(PrgNL) * 3, LCode.Length);

			CountryCode = LCode.Substring(0, 3);
			// получили код страны
			LCode = LCode.Substring(3, LCode.Length);
			FigCount = LCode.Substring(0, 3);
			// получили число фигур
			LCode = LCode.Substring(3, LCode.Length);

			double fCeloe = 0;
			double fDrobnoe = 0;

			P = new ESRI.ArcGIS.Geometry.Point();
			I = 0;
			for (K = 0; K <= Convert.ToInt32(FigCount) - 1; K++)
			{
				FigCode = LCode.Substring(0, 1);
				// получили код фигуры
				I = I + 1;

				if (FigCode != "1")
				{
					Pol = new Polygon();
					PointsCount = LCode.Substring(I, 6);
					// получили число точек для данной фигуры
					I = I + 6;

					for (M = 0; M <= Convert.ToInt32(PointsCount) - 1; M++)
					{
						X = LCode.Substring(I, 7);
						Y = LCode.Substring(I + 7, 7);
						I = I + 14;
						//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

						Celoe = X.Substring(0, 4);
						Drobnoe = X.Substring(4, 3);
						fCeloe = Convert.ToDouble(Celoe);
						fDrobnoe = Convert.ToDouble(Drobnoe);
						Xc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
						// получили координату X

						Celoe = Y.Substring(0, 4);
						Drobnoe = Y.Substring(4, 3);
						fCeloe = Convert.ToDouble(Celoe);
						fDrobnoe = Convert.ToDouble(Drobnoe);
						Yc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
						// получили координату Y

						NativeMethods.GeographicToProjection(Xc, Yc, ref XX, ref YY);

						P.PutCoords(XX, YY);

						//                P.PutCoords Xc, Yc
						//                Set P = ToPrj(P)   'спроецировали точки
						Pol.AddPoint(P);
					}
				}
				else
				{
					X = LCode.Substring(I, 7);
					Y = LCode.Substring(I + 7, 7);
					I = I + 14;
					//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

					R = LCode.Substring(I, 8);
					I = I + 8;
					//8 позиций R (AAAAAA.BB)

					Celoe = X.Substring(0, 4);
					Drobnoe = X.Substring(4, 3);
					fCeloe = Convert.ToDouble(Celoe);
					fDrobnoe = Convert.ToDouble(Drobnoe);
					Xc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
					// получили координату X

					Celoe = Y.Substring(0, 4);
					Drobnoe = Y.Substring(4, 3);
					fCeloe = Convert.ToDouble(Celoe);
					fDrobnoe = Convert.ToDouble(Drobnoe);
					Yc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
					// получили координату Y

					//MsgBox "x = " && Xc && "    Y = " && Yc
					NativeMethods.GeographicToProjection(Xc, Yc, ref XX, ref YY);
					P.PutCoords(XX, YY);
					//            P.PutCoords Xc, Yc
					//            Set P = ToPrj(P)   'спроецировали точки

					Celoe = R.Substring(0, 6);
					Drobnoe = R.Substring(6, 2);

					Rc = Convert.ToDouble(Celoe) * 1000.0 + Convert.ToDouble(Drobnoe);
					// получили значение радиуса если фигура круг
					Pol = Functions.CreatePrjCircle(P, Rc) as IPointCollection;
				}

				T = Pol as ITopologicalOperator2;
				T.IsKnownSimple_2 = false;
				T.Simplify();
				pResult = T.Union(pResult) as IPolygon;

				T = pResult as ITopologicalOperator2;
				T.IsKnownSimple_2 = false;
				T.Simplify();
			}

			return pResult;
		}
	}
}
