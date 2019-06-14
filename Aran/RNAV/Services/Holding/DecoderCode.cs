//#define oldGen

using System;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Aran.Geometries;
//using Aran.Geometries.Operators;
using System.Runtime.InteropServices;
using Aran.Geometries.SpatialReferences;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;

namespace Holding
{
	[System.Runtime.InteropServices.ComVisible ( false )]
	public static class DecoderCode
	{
		public const int SSTableSize = 128;
		public const int SSTableMask = SSTableSize - 1;
		public const int SSTableBits = 7;

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
		private static uint [ ] SSArray = new uint [ 128 ];
		private static bool bFillSS;

		private static string DeCodeString ( string val_Renamed )
		{
			string RetVal = "";
			for ( int i = 0; i < val_Renamed.Length; i++ )
			{
				RetVal = RetVal + DeCodeChar ( val_Renamed [ i ] );
			}
			return RetVal;
		}

		private static char DeCodeChar ( char val_Renamed )
		{
			int c = ( int ) val_Renamed;
			if ( c > ( int ) '9' )
				c = c + 10 - Generator ( GneratorIndex ) - ( int ) 'A';
			else
				c = c - Generator ( GneratorIndex ) - ( int ) '0';

			c = ( int ) Math.Round ( ( c + m_CodeLen * ( c < 0 ? 1 : 0 ) ) * m_CodeScale );
			return ( c > 0 ? ( char ) ( c + MaxV - MinV * ( c < 11 ? 1 : 0 ) ) : '-' );
		}

		private static void SetNewSeedL ( uint initSeed )
		{
			uint i = 0;

			bFillSS = true;
			SeedValue = initSeed;
			for ( i = 0; i <= 127; i++ )
			{
				Generator ( GneratorIndex );
				SSArray [ i ] = ( SeedValue & 0xfffffffeu ) | ( i & 1 );
			}
			bFillSS = false;
			SeedValue = initSeed;
		}

		private static void SetNewSeedS ( string initString )
		{
			int d, i;
			uint initValue;
			char Ch = initString [ 0 ];

			if ( Ch <= '9' )
				i = Ch - '0';
			else
				i = Ch + 10 - 'A';

			GneratorIndex = ( uint ) i - m_CodeLen / 2;
			if ( GneratorIndex < 0 )
				GneratorIndex = GneratorIndex + m_CodeLen;
			if ( GneratorIndex >= m_NGnerators )
				throw new Exception ( "Unsupported format string reached." );

			initValue = 0;
			for ( i = 1; i < initString.Length; i++ )
			{
				Ch = initString [ i ];

				if ( Ch <= '9' )
					d = Ch - '0';
				else
					d = Ch + 10 - 'A';

				initValue = initValue * m_CodeLen + ( uint ) d;
			}
			SetNewSeedL ( initValue );
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

#if oldGen == false
		private static byte Generator0()
		{
			uint i, k, j1;
			int j0;

			i = 22695477 * SeedValue + 37;

			if (bFillSS)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				k = 0;
				while ((SSArray[j1] == i) && (k < 128))
				{
					j1 = (j1 + 1) & SSTableMask;
					k++;
				}

				if (SSArray[j1] == i)
					SSArray[j1] = 22695477 + i;

				j0 = (int)j1 - 23;
				if (j0 < 0)
					j0 += SSTableSize;

				SeedValue = (SSArray[j0] & 0xFFFF0000u) | (SSArray[j1] >> 16);
				SSArray[j1] = i;
			}

			return (byte)(Math.Floor(m_Scale1 * SeedValue));
		}

		private static byte Generator1()
		{
			uint i, j0, j1;

			i = 22695461 * SeedValue + 3;

			if (bFillSS)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				j0 = (SeedValue >> 24) & SSTableMask;

				SeedValue = (SSArray[j0] & 0xFFFF0000U) | (SSArray[j1] >> 16);
				SSArray[j0] = i;
			}

			return (byte)(Math.Floor(m_Scale1 * SeedValue));
		}

		private static byte Generator2()
		{
			uint i, j0, j1;

			i = SeedValue * (SeedValue + 3);

			if (bFillSS)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				j0 = (SeedValue >> 24) & SSTableMask;
				SeedValue = (SSArray[j1] & 0xFFFF0000) | (SSArray[j0] >> 16);
				SSArray[j1] = i;
			}

			return (byte)(Math.Floor(m_Scale1 * SeedValue));
		}

		private static byte Generator3()
		{
			uint i, k, j1;
			int j0;

			i = 22695477 * SeedValue + 37;

			if (bFillSS)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				k = 0;
				while ((SSArray[j1] == i) & (k < 128))
				{
					j1 = (j1 + 63) & SSTableMask;
					k++;
				}

				if (SSArray[j1] == i)
					SSArray[j1] = 22695477 + i;

				j0 = (int)j1 - 23;
				if (j0 < 0)
					j0 += SSTableSize;

				SeedValue = (SSArray[j0] & 0xFFFF0000) | (SSArray[j1] >> 16);
				SSArray[j1] = i;
			}

			return (byte)(Math.Floor(m_Scale2 * (SeedValue & 0xFFFF)));
		}

		private static byte Generator4()
		{
			uint i, j0, j1;

			i = 22695461 * SeedValue + 3;

			if (bFillSS)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				j0 = (SeedValue >> 24) & SSTableMask;	//J1 - 21

				SeedValue = (SSArray[j0] & 0xFFFF0000) | ((SSArray[j1] >> 16));
				SSArray[j0] = i;
			}

			return (byte)(Math.Floor(m_Scale2 * (SeedValue & 0xFFFF)));
		}

		private static byte Generator5()
		{
			uint i, j0, j1;

			i = SeedValue * (SeedValue + 3);

			if (bFillSS)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				j0 = (SeedValue >> 24) & SSTableMask;
				SeedValue = (SSArray[j1] & 0xFFFF0000) | (SSArray[j0] >> 16);
				SSArray[j1] = i;
			}

			return (byte)(Math.Floor(m_Scale2 * (SeedValue & 0xFFFF)));
		}
#else
		private static double Exp2 ( int val_Renamed )
		{
			double RetVal = 1.0;

			for ( int i = 0; i <= val_Renamed - 1; i++ )
				RetVal = RetVal + RetVal;

			return RetVal;
		}

		private static uint Unchecked ( double val_Renamed )
		{
			if ( val_Renamed < 2147483648.0 )
				return ( uint ) System.Math.Floor ( val_Renamed );

			int i, n = ( int ) System.Math.Round ( System.Math.Log ( val_Renamed ) * 1.44269504088896 - 0.49999 );

			for ( i = n; i >= 32; i += -1 )
			{
				double fExp = Exp2 ( i );
				if ( val_Renamed >= fExp )
					val_Renamed = val_Renamed - fExp;
			}

			if ( val_Renamed < 2147483648.0 )
				return ( uint ) System.Math.Floor ( val_Renamed );

			val_Renamed = val_Renamed - 2147483648.0;
			return ( uint ) System.Math.Floor ( val_Renamed ) | 0x80000000u;
		}

		private static uint ShiftDWORD24 ( uint val_Renamed )
		{
			uint RetVal = 0;
			if ( ( val_Renamed & 0x80000000u ) == 0x80000000u )
				RetVal = 128;

			uint Mask = 0x40000000u;
			uint AddValue = 64;

			for ( int i = 0; i <= 6; i++ )
			{
				if ( ( val_Renamed & Mask ) != 0 )
					RetVal = RetVal + AddValue;
				Mask = Mask / 2;
				AddValue = AddValue / 2;
			}

			return RetVal;
		}

		private static uint ShiftDWORD16 ( uint val_Renamed )
		{
			uint RetVal = 0;
			if ( ( val_Renamed & 0x80000000u ) == 0x80000000u )
				RetVal = 32768;

			uint Mask = 0x40000000u;
			uint AddValue = 16384;

			for ( int i = 0; i <= 14; i++ )
			{
				if ( ( val_Renamed & Mask ) == Mask )
					RetVal = RetVal + AddValue;
				Mask = Mask / 2;
				AddValue = AddValue / 2;
			}

			return RetVal;
		}

		private static double DWORD2Double ( uint val_Renamed )
		{
			return val_Renamed;
		}

		private static byte Generator0 ( )
		{
			double fTmp = DWORD2Double ( SeedValue ) * 22695477.0 + 37.0;

			if ( bFillSS )
				SeedValue = Unchecked ( fTmp );
			else
			{
				uint I = Unchecked ( fTmp );
				uint J1 = ShiftDWORD24 ( I ) & 127;
				uint K = 0;

				while ( ( SSArray [ J1 ] == I ) && ( K < 128 ) )
				{
					K = K + 1;
					J1 = ( J1 + 1 ) & 127;
				}

				if ( SSArray [ J1 ] == I )
					SSArray [ J1 ] = Unchecked ( I + 22695477.0 );

				uint J0 = J1 - 23;

				if ( J0 < 0 )
					J0 = J0 + 128;
				SeedValue = ( SSArray [ J0 ] & 0xffff0000u ) | ( ShiftDWORD16 ( SSArray [ J1 ] ) );
				SSArray [ J1 ] = I;
			}

			return Convert.ToByte ( System.Math.Floor ( DWORD2Double ( SeedValue ) * m_Scale1 ) );
		}

		private static byte Generator1 ( )
		{
			//    fTmp = (0.5 * DWORD2Double(SeedValue) + 1.4142135623731) * 2.23606797749979
			double fTmp = DWORD2Double ( SeedValue ) * 22695461.0 + 3.14159265358979;

			if ( bFillSS )
				//        SeedValue = Unchecked((fTmp - Int(fTmp)) * 4706870449.79926)
				SeedValue = Unchecked ( fTmp );
			else
			{
				uint I = Unchecked ( fTmp );
				uint J1 = ShiftDWORD24 ( I ) & 127;
				uint J0 = ShiftDWORD24 ( SeedValue ) & 127;
				//J1 - 21

				SeedValue = ( SSArray [ J0 ] & 0xffff0000u ) | ( ShiftDWORD16 ( SSArray [ J1 ] ) );
				SSArray [ J0 ] = I;
			}

			return Convert.ToByte ( System.Math.Floor ( DWORD2Double ( SeedValue ) * m_Scale1 ) );
		}

		private static byte Generator2 ( )
		{
			//    fTmp = DWORD2Double(SeedValue) * 0.318309886183791 + 3.14159265358979
			//    SeedValue = Unchecked((fTmp - Int(fTmp)) * 4896968389.19523)
			//    Generator2 = CByte(Int(ShiftDWORD24(SeedValue) * m_RangeScale))
			double fTmp = DWORD2Double ( SeedValue );
			fTmp = fTmp * ( fTmp + 3.0 );			//+ 3.4142135623731
			if ( bFillSS )
				SeedValue = Unchecked ( fTmp );
			else
			{
				uint I = Unchecked ( fTmp );
				uint J1 = ShiftDWORD24 ( I ) & 127;
				uint J0 = ShiftDWORD24 ( SeedValue ) & 127;
				SeedValue = ( SSArray [ J1 ] & 0xffff0000u ) | ( ShiftDWORD16 ( SSArray [ J0 ] ) );
				SSArray [ J1 ] = I;
			}

			return Convert.ToByte ( System.Math.Floor ( DWORD2Double ( SeedValue ) * m_Scale1 ) );
		}

		private static byte Generator3 ( )
		{
			double fTmp = DWORD2Double ( SeedValue ) * 22695477.0 + 37.0;

			if ( bFillSS )
				SeedValue = Unchecked ( fTmp );
			else
			{
				uint I = Unchecked ( fTmp );
				uint J1 = ShiftDWORD24 ( I ) & 127;

				uint K = 0;
				while ( ( SSArray [ J1 ] == I ) && ( K < 128 ) )
				{
					K = K + 1;
					J1 = ( J1 + 63 ) & 127;
				}
				if ( SSArray [ J1 ] == I )
					SSArray [ J1 ] = Unchecked ( I + 22695477.0 );

				int J0 = (int)J1 - 23;
				if ( J0 < 0 )
					J0 = J0 + 128;
				SeedValue = ( SSArray [ J0 ] & 0xffff0000u ) | ( ShiftDWORD16 ( SSArray [ J1 ] ) );
				SSArray [ J1 ] = I;
			}

			return Convert.ToByte ( System.Math.Floor ( ( SeedValue & 65535 ) * m_Scale2 ) );
		}

		private static byte Generator4 ( )
		{
			double fTmp = DWORD2Double ( SeedValue ) * 22695461.0 + 3.14159265358979;

			if ( bFillSS )
				SeedValue = Unchecked ( fTmp );
			else
			{
				uint I = Unchecked ( fTmp );
				uint J1 = ShiftDWORD24 ( I ) & 127;
				uint J0 = ShiftDWORD24 ( SeedValue ) & 127;
				//J1 - 21

				SeedValue = ( SSArray [ J0 ] & 0xffff0000u ) | ( ShiftDWORD16 ( SSArray [ J1 ] ) );
				SSArray [ J0 ] = I;
			}

			return Convert.ToByte ( System.Math.Floor ( ( SeedValue & 65535 ) * m_Scale2 ) );
		}

		private static byte Generator5 ( )
		{
			double fTmp = DWORD2Double ( SeedValue );
			fTmp = fTmp * ( fTmp + 3.0 );

			if ( bFillSS )
				SeedValue = Unchecked ( fTmp );
			else
			{
				uint I = Unchecked ( fTmp );
				uint J1 = ShiftDWORD24 ( I ) & 127;
				uint J0 = ShiftDWORD24 ( SeedValue ) & 127;
				SeedValue = ( SSArray [ J1 ] & 0xffff0000u ) | ( ShiftDWORD16 ( SSArray [ J0 ] ) );
				SSArray [ J1 ] = I;
			}

			return Convert.ToByte ( System.Math.Floor ( ( SeedValue & 65535 ) * m_Scale2 ) );
		}
#endif

		private static void InitModule ( )
		{
			SeedValue = 0xffffffffu;
			GneratorIndex = m_NGnerators;
			MaxV = 9 + 'A';
			MinV = MaxV - '0' + 1;
		}

		private static string LstStDtReader ( string LST_DT, string key, int Multpl = -1 )
		{
			int [ ] Tab = new int [ ]{26, 27, 28, 29, 30, 31, 32, 33, 34, 35,
				0, 23, 21, 2, 11, 3, 4, 5, 16, 6,
				7, 8, 25, 24, 17, 18, 9, 12, 1, 13,
				15, 22, 10, 20, 14, 19};

			string Res = "";

			for ( int i = 0; i < key.Length; i++ )
			{
				int j;

				char Ch = key [ i ];

				if ( Ch <= '9' )
					j = (int) Ch - '0';
				else
					j = (int) Ch - 'A' + 10;

				string ind = Tab[j].ToString();

				if ( Tab [ j ] < 10 )
					ind = "0" + ind;
				Res = Res + ind;
			}

			string retval = Convert.ToString(Convert.ToDouble(LST_DT) + Multpl * Convert.ToDouble(Res));
			while (retval.Length < 10)
				retval = "0" + retval;

			return retval;
		}

        //public static int LstStDtWriter ( string ModuleName )
        //{
        //    /**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][LastStart][CRC]
        //     * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
        //     * PrgNL -кол-во букв в названии программы (3 позиции)
        //     * PRG_NAME - название программы
        //     * R - код страны (3 позиции)
        //     * FC - кол-во фигур (3 позиции)
        //     * СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
        //     * PC - кол-во точек фигуры (6 позиций)
        //     * P - точки (формат ZAAA.BBB)
        //            P.X (7 позиций формат ZAAA.BBB)
        //            P.Y (7 позиций формат ZAAA.BBB)
        //            P.R (8 позиций формат AAAAAA.BB)
        //     * K - ключ кодировки (5 позиций)
        //     * LastStart ЗАКОДИРОВАННАЯ дата последнего запуска программы 10 позиций (после раскодирования формат DDMMYY)
        //     * CRC - контрольная последовательность (8 позиций)
        //    **/

        //    string LCode = RegFuncs.RegRead<String> ( Microsoft.Win32.Registry.CurrentUser, ModuleName, "acar", null );
        //    string CRCCode = LCode.Substring ( LCode.Length - 8 );
        //    // получили CRC код
        //    LCode = LCode.Substring ( 0, LCode.Length - 8 );

        //    string LastStart = LCode.Substring ( LCode.Length - 10 );
        //    //получили дату последнего запуска
        //    LCode = LCode.Substring ( 0, LCode.Length - 10 );
        //    // проверили его
        //    if ( CRCCode != CRC32.CalcCRC32 ( LCode ) )
        //        return -1;

        //    string key = LCode.Substring ( LCode.Length - 5 );//получили ключ

        //    // сформируем новую дату "последнего запуска"

        //    DateTime CurrData = DateTime.Now;
        //    string dNow = Convert.ToString ( CurrData.Day );
        //    string mesNow = Convert.ToString ( CurrData.Month );
        //    string yrNow = Convert.ToString ( CurrData.Year - 2000 );

        //    if ( dNow.Length < 2 )
        //        dNow = "0" + dNow;
        //    if ( mesNow.Length < 2 )
        //        mesNow = "0" + mesNow;
        //    if ( yrNow.Length < 2 )
        //        yrNow = "0" + yrNow;

        //    LastStart = LstStDtReader ( dNow + mesNow + yrNow, key, 1 );
        //    CRCCode = CRC32.CalcCRC32 ( LCode );
        //    LCode = LCode + LastStart + CRCCode;

        //    return RegFuncs.RegWrite ( Registry.CurrentUser, ModuleName, "Acar", LCode );
        //}

		public static MultiPolygon DecodeLCode ( string LCode, string ModuleName, SpatialReference geogSpatialRef, SpatialReference projectedSpatialRef)
		{
			/**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][LastStart][CRC]
			 * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
			 * PrgNL -кол-во букв в названии программы (3 позиции)
			 * PRG_NAME - название программы
			 * R - код страны (3 позиции)
			 * FC - кол-во фигур (3 позиции)
			 * СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
			 * PC - кол-во точек фигуры (6 позиций)
			 * P - точки (формат ZAAA.BBB)
					P.X (7 позиций формат ZAAA.BBB)
					P.Y (7 позиций формат ZAAA.BBB)
					P.R (8 позиций формат AAAAAA.BB)
			 * K - ключ кодировки (5 позиций)
			 * LastStart ЗАКОДИРОВАННАЯ дата последнего запуска программы 10 позиций (после раскодирования формат DDMMYY)
			 * CRC - контрольная последовательность (8 позиций)
			**/

			string CountryCode;
			string LastStart;
			string CRCCode;
			string PRG_NAME;
			string Drobnoe;
			string Celoe;
			string tempS;
			string Tarix;
			string key;
			string X;
			string Y;
			string R;

			string yearKey;
			string mesKey;
			string dKey;

			int FigCount;
			int FigCode;
			int PointsCount;
			int mesNow;
			int yrNow;
			int dNow;
			int PrgNL;
			int K;
			int M;
			int I;
			int Ri;

			double TL1;
			double TL2;
			double TL;
			double Xc;
			double Yc;
			double Rc;

			//ITopologicalOperator2 T;
			Ring Pol;
			MultiPolygon pResult;
			Point P;

			pResult = new MultiPolygon ( );

			if ( LCode.Length <= 0 )
				return pResult;

			CRCCode = LCode.Substring ( LCode.Length - 8 );
			// получили CRC код
			LCode = LCode.Substring ( 0, LCode.Length - 8 );

			LastStart = LCode.Substring ( LCode.Length - 10 );
			//получили дату последнего запуска
			LCode = LCode.Substring ( 0, LCode.Length - 10 );

			if ( CRCCode != CRC32.CalcCRC32 ( LCode ) )
				return pResult;
			// проверили его

			key = LCode.Substring ( LCode.Length - 5 );
			//получили ключ
			LCode = LCode.Substring ( 0, LCode.Length - 5 );

			InitModule ( );
			SetNewSeedS ( key );
			string dLCode = DeCodeString ( LCode );
			LCode = dLCode;
			//LCode = "311299980070651141141051180971080260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"
			//LCode = "311299980090681011120971141161171141010260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"

			//==================================================================================================
			Tarix = LCode.Substring ( 0, 8 );
			// получили срок "годности"
			LCode = LCode.Substring ( 8 );

			dKey = Tarix.Substring ( 0, 2 );
			mesKey = Tarix.Substring ( 2, 2 );
			yearKey = Tarix.Substring ( 4, 4 );
			TL = Convert.ToInt32 ( dKey ) + Convert.ToInt32 ( mesKey ) * 30.4375 + ( Convert.ToInt32 ( yearKey ) - 1899 ) * 365.25;
			//==================================================================================================
			LastStart = LstStDtReader ( LastStart, key );

			dKey = LastStart.Substring ( 4, 2 );
			mesKey = LastStart.Substring ( 6, 2 );
			yearKey = LastStart.Substring ( 8 );
			TL1 = Convert.ToInt32 ( dKey ) + Convert.ToInt32 ( mesKey ) * 30.4375 + ( Convert.ToInt32 ( yearKey ) + ( 2000 - 1899 ) ) * 365.25;

			DateTime CurrData = DateTime.Now;

			dNow = CurrData.Day;
			mesNow = CurrData.Month;
			yrNow = CurrData.Year - 1899;
			TL2 = ( dNow + mesNow * 30.4375 + yrNow * 365.25 );

			TL1 = ( TL1 < TL2 ? TL1 : TL2 );
			//==================================================================================================
			// сравним дату последнего запуска LastStart и текущую. если Текущая МЕНЬШЕ даты LastStart - вылетаем
			if ( TL < TL1 )
				return pResult;
			//==================================================================================================

			PrgNL = Convert.ToInt32 ( LCode.Substring ( 0, 3 ) );
			// получили длину названия модуля
			LCode = LCode.Substring ( 3 );

			Ri = 1;
			PRG_NAME = "";
			tempS = LCode.Substring ( 0, PrgNL * 3 );
			//получили название модуля в символьном виде
			// теперь сконвертируем его в строку
			for ( I = 1; I <= PrgNL; I++ )
			{
				PRG_NAME = PRG_NAME + Convert.ToChar ( Convert.ToInt32 ( tempS.Substring ( Ri - 1, 3 ) ) );
				Ri = Ri + 3;
			}

			if ( PRG_NAME.ToUpper() != ModuleName.ToUpper())
				return pResult;

			LCode = LCode.Substring ( PrgNL * 3 );

			CountryCode = LCode.Substring ( 0, 3 );
			// получили код страны
			LCode = LCode.Substring ( 3 );
			FigCount = Convert.ToInt32 ( LCode.Substring ( 0, 3 ) );
			// получили число фигур
			LCode = LCode.Substring ( 3 );

			double fCeloe = 0;
			double fDrobnoe = 0;

			GeometryOperators geomOper = new GeometryOperators ( );
			I = 1;
			for ( K = 0; K <= FigCount - 1; K++ )
			{
				FigCode = Convert.ToInt32 ( LCode.Substring ( 0, 1 ) );
				// получили код фигуры
				I = I + 1;
				if ( FigCode != 1 )
				{
					Pol = new Ring ( );
					PointsCount = Convert.ToInt32 ( LCode.Substring ( I - 1, 6 ) );
					// получили число точек для данной фигуры
					I = I + 6;

					for ( M = 0; M <= PointsCount - 1; M++ )
					{
						X = LCode.Substring ( I - 1, 7 );
						Y = LCode.Substring ( I + 6, 7 );
						I = I + 14;
						//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

						Celoe = X.Substring ( 0, 4 );
						Drobnoe = X.Substring ( 4, 3 );
						fCeloe = Convert.ToDouble ( Celoe );
						fDrobnoe = Convert.ToDouble ( Drobnoe );
						Xc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
						// получили координату X

						Celoe = Y.Substring ( 0, 4 );
						Drobnoe = Y.Substring ( 4, 3 );
						fCeloe = Convert.ToDouble ( Celoe );
						fDrobnoe = Convert.ToDouble ( Drobnoe );
						Yc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
						// получили координату Y

						//NativeMethods.GeographicToProjection(Xc, Yc, ref XX, ref YY);
						//P.PutCoords(XX, YY);

						P = new Point (Xc, Yc );
						P = (Point) geomOper.GeoTransformations ( P, geogSpatialRef, projectedSpatialRef );			//спроецировали точки

						if ( !P.IsEmpty)
							Pol.Add ( P );
					}
				}
				else
				{
					X = LCode.Substring ( I - 1, 7 );
					Y = LCode.Substring ( I + 6, 7 );
					I = I + 14;
					//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

					R = LCode.Substring ( I - 1, 8 );
					I = I + 8;
					//8 позиций R (AAAAAA.BB)

					Celoe = X.Substring ( 0, 4 );
					Drobnoe = X.Substring ( 4, 3 );
					fCeloe = Convert.ToDouble ( Celoe );
					fDrobnoe = Convert.ToDouble ( Drobnoe );
					Xc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
					// получили координату X

					Celoe = Y.Substring ( 0, 4 );
					Drobnoe = Y.Substring ( 4, 3 );
					fCeloe = Convert.ToDouble ( Celoe );
					fDrobnoe = Convert.ToDouble ( Drobnoe );
					Yc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
					// получили координату Y

					//MsgBox "x = " && Xc && "    Y = " && Yc
					//NativeMethods.GeographicToProjection(Xc, Yc, ref XX, ref YY);
					//P.PutCoords(XX, YY);

					P = new Point ( Xc, Yc );
					P = ( Point ) geomOper.GeoTransformations ( P, geogSpatialRef, projectedSpatialRef );			//спроецировали точки

					if ( !P.IsEmpty)
					{
						Celoe = R.Substring ( 0, 6 );
						Drobnoe = R.Substring ( 6, 2 );

						Rc = Convert.ToDouble ( Celoe ) * 1000.0 + Convert.ToDouble ( Drobnoe );
						// получили значение радиуса если фигура круг
						Pol = ARANFunctions.CreateCirclePrj ( P, Rc );
					}
					else
						Pol = new Ring ( );
				}

				if ( Pol.Count >= 3 )
				{
					Polygon polygon = new Polygon ( );
					//polygon.Add ( Pol );
                    polygon.ExteriorRing = Pol;
					pResult = ( MultiPolygon ) geomOper.UnionGeometry ( pResult, polygon );
				}
			}

			return pResult;
		}
	}
}
