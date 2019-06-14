using System;
using System.Globalization;
using System.Linq;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	[System.Runtime.InteropServices.ComVisible ( false )]
	public static class DecoderCode
	{
		private const int SsTableSize = 128;
		private const int SsTableMask = SsTableSize - 1;
		public const int SsTableBits = 7;

		private const uint MnGnerators = 6;
		private const uint MCodeLen = 36;
		private const uint MInputRange = 11;

		//m_InputRange / m_CodeLen
		private const double MCodeScale = 0.305555555555556;
		private const double MScale1 = 36.0 / 4294967296.0;
		private const double MScale2 = 36.0 / 65536.0;

		private static int _minV;

		private static int _maxV;
		private static uint _seedValue;
		private static uint _gneratorIndex;
		private static readonly uint [ ] SsArray = new uint [ 128 ];
		private static bool _bFillSs;

		private static string DeCodeString ( string valRenamed )
		{
			return valRenamed.Aggregate("", (current, t) => current + DeCodeChar(t));
		}

		private static char DeCodeChar ( char valRenamed )
		{
			int c = ( int ) valRenamed;
			if ( c > ( int ) '9' )
				c = c + 10 - Generator ( _gneratorIndex ) - ( int ) 'A';
			else
				c = c - Generator ( _gneratorIndex ) - ( int ) '0';

			c = ( int ) Math.Round ( ( c + MCodeLen * ( c < 0 ? 1 : 0 ) ) * MCodeScale );
			return ( c > 0 ? ( char ) ( c + _maxV - _minV * ( c < 11 ? 1 : 0 ) ) : '-' );
		}

		private static void SetNewSeedL ( uint initSeed )
		{
			uint i = 0;

			_bFillSs = true;
			_seedValue = initSeed;
			for ( i = 0; i <= 127; i++ )
			{
				Generator ( _gneratorIndex );
				SsArray [ i ] = ( _seedValue & 0xfffffffeu ) | ( i & 1 );
			}
			_bFillSs = false;
			_seedValue = initSeed;
		}

		private static void SetNewSeedS ( string initString )
		{
			int i;
			char ch = initString [ 0 ];

			if ( ch <= '9' )
				i = ch - '0';
			else
				i = ch + 10 - 'A';

			_gneratorIndex = ( uint ) i - MCodeLen / 2;
			if ( _gneratorIndex < 0 )
				_gneratorIndex = _gneratorIndex + MCodeLen;
			if ( _gneratorIndex >= MnGnerators )
				throw new Exception ( "Unsupported format string reached." );

			uint initValue = 0;
			for ( i = 1; i < initString.Length; i++ )
			{
				ch = initString [ i ];

				int d;
				if ( ch <= '9' )
					d = ch - '0';
				else
					d = ch + 10 - 'A';

				initValue = initValue * MCodeLen + ( uint ) d;
			}
			SetNewSeedL ( initValue );
		}

		private static byte Generator(uint index)
		{
			switch (index)
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
			var i = 22695477 * _seedValue + 37;

			if (_bFillSs)
				_seedValue = i;
			else
			{
				var j1 = (i >> 24) & SsTableMask;
				uint k = 0;
				while ((SsArray[j1] == i) && (k < 128))
				{
					j1 = (j1 + 1) & SsTableMask;
					k++;
				}

				if (SsArray[j1] == i)
					SsArray[j1] = 22695477 + i;

				var j0 = (int)j1 - 23;
				if (j0 < 0)
					j0 += SsTableSize;

				_seedValue = (SsArray[j0] & 0xFFFF0000u) | (SsArray[j1] >> 16);
				SsArray[j1] = i;
			}

			return (byte)(Math.Floor(MScale1 * _seedValue));
		}

		private static byte Generator1()
		{
			var i = 22695461 * _seedValue + 3;

			if (_bFillSs)
				_seedValue = i;
			else
			{
				var j1 = (i >> 24) & SsTableMask;
				var j0 = (_seedValue >> 24) & SsTableMask;

				_seedValue = (SsArray[j0] & 0xFFFF0000U) | (SsArray[j1] >> 16);
				SsArray[j0] = i;
			}

			return (byte)(Math.Floor(MScale1 * _seedValue));
		}

		private static byte Generator2()
		{
			var i = _seedValue * (_seedValue + 3);

			if (_bFillSs)
				_seedValue = i;
			else
			{
				var j1 = (i >> 24) & SsTableMask;
				var j0 = (_seedValue >> 24) & SsTableMask;
				_seedValue = (SsArray[j1] & 0xFFFF0000) | (SsArray[j0] >> 16);
				SsArray[j1] = i;
			}

			return (byte)(Math.Floor(MScale1 * _seedValue));
		}

		private static byte Generator3()
		{
			var i = 22695477 * _seedValue + 37;

			if (_bFillSs)
				_seedValue = i;
			else
			{
				var j1 = (i >> 24) & SsTableMask;
				uint k = 0;
				while ((SsArray[j1] == i) & (k < 128))
				{
					j1 = (j1 + 63) & SsTableMask;
					k++;
				}

				if (SsArray[j1] == i)
					SsArray[j1] = 22695477 + i;

				var j0 = (int)j1 - 23;
				if (j0 < 0)
					j0 += SsTableSize;

				_seedValue = (SsArray[j0] & 0xFFFF0000) | (SsArray[j1] >> 16);
				SsArray[j1] = i;
			}

			return (byte)(Math.Floor(MScale2 * (_seedValue & 0xFFFF)));
		}

		private static byte Generator4()
		{
			var i = 22695461 * _seedValue + 3;

			if (_bFillSs)
				_seedValue = i;
			else
			{
				var j1 = (i >> 24) & SsTableMask;
				var j0 = (_seedValue >> 24) & SsTableMask;

				_seedValue = (SsArray[j0] & 0xFFFF0000) | ((SsArray[j1] >> 16));
				SsArray[j0] = i;
			}

			return (byte)(Math.Floor(MScale2 * (_seedValue & 0xFFFF)));
		}

		private static byte Generator5()
		{
			var i = _seedValue * (_seedValue + 3);

			if (_bFillSs)
				_seedValue = i;
			else
			{
				var j1 = (i >> 24) & SsTableMask;
				var j0 = (_seedValue >> 24) & SsTableMask;
				_seedValue = (SsArray[j1] & 0xFFFF0000) | (SsArray[j0] >> 16);
				SsArray[j1] = i;
			}

			return (byte)(Math.Floor(MScale2 * (_seedValue & 0xFFFF)));
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
			_seedValue = 0xffffffffu;
			_gneratorIndex = MnGnerators;
			_maxV = 9 + 'A';
			_minV = _maxV - '0' + 1;
		}

		private static string LstStDtReader ( string lstDt, string key, int multpl = -1 )
		{
			int [ ] tab = new int [ ]{26, 27, 28, 29, 30, 31, 32, 33, 34, 35,
				0, 23, 21, 2, 11, 3, 4, 5, 16, 6,
				7, 8, 25, 24, 17, 18, 9, 12, 1, 13,
				15, 22, 10, 20, 14, 19};

			string res = "";

			foreach (char t in key)
			{
				int j;

				char ch = t;

				if ( ch <= '9' )
					j = (int) ch - '0';
				else
					j = (int) ch - 'A' + 10;

				string ind = tab[j].ToString();

				if ( tab [ j ] < 10 )
					ind = "0" + ind;
				res = res + ind;
			}

			string retval = Convert.ToString(Convert.ToDouble(lstDt) + multpl * Convert.ToDouble(res), CultureInfo.InvariantCulture);
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

        //    string LCode = RegFuncs.RegRead<String> ( Registry.CurrentUser, RegFuncs.Panda + "\\" + ModuleName, RegFuncs.LicenseKeyName, "" );

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
			
        //    return RegFuncs.RegWrite ( Registry.CurrentUser, RegFuncs.Panda + "\\" + ModuleName, "Acar", LCode );
        //}

		public static MultiPolygon DecodeLCode ( string lCode, string moduleName, SpatialReference geogSpatialRef, SpatialReference projectedSpatialRef)
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

			string countryCode;

			int k;
			int I;

			//ITopologicalOperator2 T;

			var pResult = new MultiPolygon ( );

			if ( lCode.Length <= 0 )
				return pResult;

			var crcCode = lCode.Substring ( lCode.Length - 8 );
			// получили CRC код
			lCode = lCode.Substring ( 0, lCode.Length - 8 );

			var lastStart = lCode.Substring ( lCode.Length - 10 );
			//получили дату последнего запуска
			lCode = lCode.Substring ( 0, lCode.Length - 10 );

			if ( crcCode != CRC32.CalcCRC32 ( lCode ) )
				return pResult;
			// проверили его

			var key = lCode.Substring ( lCode.Length - 5 );
			//получили ключ
			lCode = lCode.Substring ( 0, lCode.Length - 5 );

			InitModule ( );
			SetNewSeedS ( key );
			string dLCode = DeCodeString ( lCode );
			lCode = dLCode;
			//LCode = "311299980070651141141051180971080260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"
			//LCode = "311299980090681011120971141161171141010260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"

			//==================================================================================================
			var tarix = lCode.Substring ( 0, 8 );
			// получили срок "годности"
			lCode = lCode.Substring ( 8 );

			var dKey = tarix.Substring ( 0, 2 );
			var mesKey = tarix.Substring ( 2, 2 );
			var yearKey = tarix.Substring ( 4, 4 );
			var tl = Convert.ToInt32 ( dKey ) + Convert.ToInt32 ( mesKey ) * 30.4375 + ( Convert.ToInt32 ( yearKey ) - 1899 ) * 365.25;
			//==================================================================================================
			lastStart = LstStDtReader ( lastStart, key );

			dKey = lastStart.Substring ( 4, 2 );
			mesKey = lastStart.Substring ( 6, 2 );
			yearKey = lastStart.Substring ( 8 );
			var tl1 = Convert.ToInt32 ( dKey ) + Convert.ToInt32 ( mesKey ) * 30.4375 + ( Convert.ToInt32 ( yearKey ) + ( 2000 - 1899 ) ) * 365.25;

			DateTime currData = DateTime.Now;

			var dNow = currData.Day;
			var mesNow = currData.Month;
			var yrNow = currData.Year - 1899;
			var tl2 = ( dNow + mesNow * 30.4375 + yrNow * 365.25 );

			tl1 = ( tl1 < tl2 ? tl1 : tl2 );
			//==================================================================================================
			// сравним дату последнего запуска LastStart и текущую. если Текущая МЕНЬШЕ даты LastStart - вылетаем
			if ( tl < tl1 )
				return pResult;
			//==================================================================================================

			var prgNl = Convert.ToInt32 ( lCode.Substring ( 0, 3 ) );
			// получили длину названия модуля
			lCode = lCode.Substring ( 3 );

			var ri = 1;
			var prgName = "";
			var tempS = lCode.Substring ( 0, prgNl * 3 );
			//получили название модуля в символьном виде
			// теперь сконвертируем его в строку
			for ( I = 1; I <= prgNl; I++ )
			{
				prgName = prgName + Convert.ToChar ( Convert.ToInt32 ( tempS.Substring ( ri - 1, 3 ) ) );
				ri = ri + 3;
			}

			if ( prgName.ToUpper() != moduleName.ToUpper())
				return pResult;

			lCode = lCode.Substring ( prgNl * 3 );

			countryCode = lCode.Substring ( 0, 3 );
			// получили код страны
			lCode = lCode.Substring ( 3 );
			var figCount = Convert.ToInt32 ( lCode.Substring ( 0, 3 ) );
			// получили число фигур
			lCode = lCode.Substring ( 3 );

			GeometryOperators geomOper = new GeometryOperators ( );
			I = 1;
			for ( k = 0; k <= figCount - 1; k++ )
			{
				var figCode = Convert.ToInt32 ( lCode.Substring ( 0, 1 ) );
				// получили код фигуры
				I = I + 1;
				string drobnoe;
				string x;
				string y;
				Ring pol;
				double fDrobnoe = 0;
				double fCeloe = 0;
				string celoe;
				double xc;
				double yc;
				Point p;
				if ( figCode != 1 )
				{
					pol = new Ring ( );
					var pointsCount = Convert.ToInt32 ( lCode.Substring ( I - 1, 6 ) );
					// получили число точек для данной фигуры
					I = I + 6;

					int m;
					for ( m = 0; m <= pointsCount - 1; m++ )
					{
						x = lCode.Substring ( I - 1, 7 );
						y = lCode.Substring ( I + 6, 7 );
						I = I + 14;
						//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

						celoe = x.Substring ( 0, 4 );
						drobnoe = x.Substring ( 4, 3 );
						fCeloe = Convert.ToDouble ( celoe );
						fDrobnoe = Convert.ToDouble ( drobnoe );
						xc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
						// получили координату X

						celoe = y.Substring ( 0, 4 );
						drobnoe = y.Substring ( 4, 3 );
						fCeloe = Convert.ToDouble ( celoe );
						fDrobnoe = Convert.ToDouble ( drobnoe );
						yc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
						// получили координату Y

						//NativeMethods.GeographicToProjection(Xc, Yc, ref XX, ref YY);
						//P.PutCoords(XX, YY);

						p = new Point (xc, yc );
						p = (Point) geomOper.GeoTransformations ( p, geogSpatialRef, projectedSpatialRef );			//спроецировали точки

						if ( !p.IsEmpty )
							pol.Add ( p );
					}
				}
				else
				{
					x = lCode.Substring ( I - 1, 7 );
					y = lCode.Substring ( I + 6, 7 );
					I = I + 14;
					//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

					var r = lCode.Substring ( I - 1, 8 );
					I = I + 8;
					//8 позиций R (AAAAAA.BB)

					celoe = x.Substring ( 0, 4 );
					drobnoe = x.Substring ( 4, 3 );
					fCeloe = Convert.ToDouble ( celoe );
					fDrobnoe = Convert.ToDouble ( drobnoe );
					xc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
					// получили координату X

					celoe = y.Substring ( 0, 4 );
					drobnoe = y.Substring ( 4, 3 );
					fCeloe = Convert.ToDouble ( celoe );
					fDrobnoe = Convert.ToDouble ( drobnoe );
					yc = fCeloe + 0.001 * ( fCeloe < 0.0 ? -fDrobnoe : fDrobnoe );
					// получили координату Y

					//MsgBox "x = " && Xc && "    Y = " && Yc
					//NativeMethods.GeographicToProjection(Xc, Yc, ref XX, ref YY);
					//P.PutCoords(XX, YY);

					p = new Point ( xc, yc );
					p = ( Point ) geomOper.GeoTransformations ( p, geogSpatialRef, projectedSpatialRef );			//спроецировали точки

					if ( !p.IsEmpty )
					{
						celoe = r.Substring ( 0, 6 );
						drobnoe = r.Substring ( 6, 2 );

						var rc = Convert.ToDouble ( celoe ) * 1000.0 + Convert.ToDouble ( drobnoe );
						// получили значение радиуса если фигура круг
						pol = ARANFunctions.CreateCirclePrj ( p, rc );
					}
					else
						pol = new Ring ( );
				}

				if ( pol.Count >= 3 )
				{
					Polygon polygon = new Polygon {ExteriorRing = pol};
					pResult = ( MultiPolygon ) geomOper.UnionGeometry ( pResult, polygon );
				}
			}

			return pResult;
		}
	}
}
