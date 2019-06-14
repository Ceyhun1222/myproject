using System;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using ARAN.GeometryClasses;
using ARAN.Contracts.GeometryOperators;
using System.Runtime.InteropServices;

namespace ARAN.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DecoderCode
	{

        [DllImport("MathRNAV", EntryPoint = "_GeographicToProjection@24")]
        public static extern void GeographicToProjection(double X, double Y, ref double ResX, ref double ResY);
        [DllImport("MathRNAV.dll", EntryPoint = "_ProjectionToGeographic@24")]
        public static extern void ProjectionToGeographic(double X, double Y, ref double ResX, ref double ResY);	

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

        private static Generator generator;

        //private static string DeCodeString(string val_Renamed)
        //{
        //    string RetVal = "";
        //    try
        //    {
        //        for (int I = 0; I < val_Renamed.Length; I++)
        //        {
        //            RetVal = RetVal + DeCodeChar(val_Renamed[I]);
        //        }

        //    }
        //    catch (Exception e)
        //    {
                
        //        throw e;
        //    }
		
        //    return RetVal;
        //}

        //private static char DeCodeChar(char val_Renamed)
        //{
        //    int C = 0;
        //    C = (int)val_Renamed;
        //    if (C > (int)'9')
        //    {
        //        C = C + 10 - Generator(GneratorIndex) - (int)'A';
        //    }
        //    else
        //    {
        //        C = C - Generator(GneratorIndex) - (int)'0';
        //    }

        //    C = (int)Math.Round((C + m_CodeLen * (C < 0 ? 1 : 0)) * m_CodeScale);
        //    return (C > 0 ? (char)(C + MaxV - MinV * (C < 11 ? 1 : 0)) : '-');
        //}

        static DecoderCode()
        {
            generator = new Generator();
        }

        private static void SetNewSeedL(uint initSeed)
        {
            generator.SetNewSeedL(initSeed);
        }

        private static void SetNewSeedS(String initString)
        {
            generator.SetNewSeedS(initString);
        }

        private  static String DeCodeString(String val)
        {
            String result = "";
            int N = val.Length;

            for (int i = 0; i < N; i++)
                result = result + DeCodeChar(val[i]);
            return result;
        }

        private static Char DeCodeChar(Char Val)
        {
            int C, r;

            if (Val > '9') C = Val - 'A' + 10;
            else C = Val - '0';

            r = generator.Generate();
            C = C - r;
            if (C < 0) C += KeyParameters.CodeLen;

            C = (int)Math.Round(C * m_CodeScale);

            if (C == 0) return '-';
            else if (C > 10) return (Char)(C + 9 + 'A');
            else return (Char)(C - 1 + '0');
        }


        //private static void SetNewSeedL(uint initSeed)
        //{
        //    uint I = 0;

        //    bFillSS = true;
        //    SeedValue = initSeed;
        //    for (I = 0; I <= 127; I++)
        //    {
        //        Generator(GneratorIndex);
        //        SSArray[I] = (SeedValue & 0xfffffffeu) | (I & 1);
        //    }
        //    bFillSS = false;
        //    SeedValue = initSeed;
        //}

        //private static void SetNewSeedS(string initString)
        //{
        //    int D = 0;
        //    int I = 0;
        //    uint initValue = 0;
        //    char Ch = initString[0];

        //    if (Ch <= '9')
        //    {
        //        I = Ch - '0';
        //    }
        //    else
        //    {
        //        I = Ch + 10 - 'A';
        //    }

        //    GneratorIndex = (uint)I - m_CodeLen / 2;
        //    if (GneratorIndex < 0)
        //        GneratorIndex = GneratorIndex + m_CodeLen;
        //    if (GneratorIndex >= m_NGnerators)
        //        throw new Exception("Unsupported format string reached.");

        //    initValue = 0;
        //    for (I = 1; I < initString.Length; I++)
        //    {
        //        Ch = initString[I];

        //        if (Ch <= '9')
        //        {
        //            D = Ch - '0';
        //        }
        //        else
        //        {
        //            D = Ch + 10 - 'A';
        //        }
        //        initValue = initValue * m_CodeLen + (uint)D;
        //    }
        //    SetNewSeedL(initValue);
        //}

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

        //private static byte Generator(uint Index)
        //{
        //    switch (Index)
        //    {
        //        case 0:
        //            return Generator0();
        //        case 1:
        //            return Generator1();
        //        case 2:
        //            return Generator2();
        //        case 3:
        //            return Generator3();
        //        case 4:
        //            return Generator4();
        //        case 5:
        //            return Generator5();
        //    }
        //    return 0;
        //}

        //private static byte Generator0()
        //{
        //    double fTmp = 0;
        //    uint I = 0;
        //    uint K = 0;
        //    uint J0 = 0;
        //    uint J1 = 0;
        //    fTmp = DWORD2Double(SeedValue) * 22695477.0 + 37.0;

        //    if (bFillSS)
        //    {
        //        SeedValue = Unchecked(fTmp);
        //    }
        //    else
        //    {
        //        I = Unchecked(fTmp);
        //        J1 = ShiftDWORD24(I) & 127;
        //        K = 0;
        //        while ((SSArray[J1] == I) && (K < 128))
        //        {
        //            K = K + 1;
        //            J1 = (J1 + 1) & 127;
        //        }

        //        if (SSArray[J1] == I)
        //            SSArray[J1] = Unchecked(I + 22695477.0);

        //        J0 = J1 - 23;
        //        if (J0 < 0)
        //            J0 = J0 + 128;
        //        SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
        //        SSArray[J1] = I;
        //    }

        //    return Convert.ToByte(System.Math.Floor(DWORD2Double(SeedValue) * m_Scale1));
        //}

        //private static byte Generator1()
        //{
        //    double fTmp = 0;
        //    uint I = 0;
        //    uint J0 = 0;
        //    uint J1 = 0;
        //    //    fTmp = (0.5 * DWORD2Double(SeedValue) + 1.4142135623731) * 2.23606797749979
        //    fTmp = DWORD2Double(SeedValue) * 22695461.0 + 3.14159265358979;

        //    if (bFillSS)
        //    {
        //        //        SeedValue = Unchecked((fTmp - Int(fTmp)) * 4706870449.79926)
        //        SeedValue = Unchecked(fTmp);
        //    }
        //    else
        //    {
        //        I = Unchecked(fTmp);
        //        J1 = ShiftDWORD24(I) & 127;
        //        J0 = ShiftDWORD24(SeedValue) & 127;
        //        //J1 - 21

        //        SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
        //        SSArray[J0] = I;
        //    }

        //    return Convert.ToByte(System.Math.Floor(DWORD2Double(SeedValue) * m_Scale1));
        //}

        //private static byte Generator2()
        //{
        //    double fTmp = 0;
        //    uint I = 0;
        //    uint J0 = 0;
        //    uint J1 = 0;

        //    //    fTmp = DWORD2Double(SeedValue) * 0.318309886183791 + 3.14159265358979
        //    //    SeedValue = Unchecked((fTmp - Int(fTmp)) * 4896968389.19523)
        //    //    Generator2 = CByte(Int(ShiftDWORD24(SeedValue) * m_RangeScale))
        //    fTmp = DWORD2Double(SeedValue);
        //    fTmp = fTmp * (fTmp + 3.0);
        //    //+ 3.4142135623731
        //    if (bFillSS)
        //    {
        //        SeedValue = Unchecked(fTmp);
        //    }
        //    else
        //    {
        //        I = Unchecked(fTmp);
        //        J1 = ShiftDWORD24(I) & 127;
        //        J0 = ShiftDWORD24(SeedValue) & 127;
        //        SeedValue = (SSArray[J1] & 0xffff0000u) | (ShiftDWORD16(SSArray[J0]));
        //        SSArray[J1] = I;
        //    }

        //    return Convert.ToByte(System.Math.Floor(DWORD2Double(SeedValue) * m_Scale1));
        //}

        //private static byte Generator3()
        //{
        //    double fTmp = 0;
        //    uint I = 0;
        //    uint K = 0;
        //    int J0 = 0;
        //    uint J1 = 0;
        //    fTmp = DWORD2Double(SeedValue) * 22695477.0 + 37.0;

        //    if (bFillSS)
        //    {
        //        SeedValue = Unchecked(fTmp);
        //    }
        //    else
        //    {
        //        I = Unchecked(fTmp);
        //        J1 = ShiftDWORD24(I) & 127;
        //        K = 0;
        //        while ((SSArray[J1] == I) && (K < 128))
        //        {
        //            K = K + 1;
        //            J1 = (J1 + 63) & 127;
        //        }
        //        if (SSArray[J1] == I)
        //            SSArray[J1] = Unchecked(I + 22695477.0);

        //        J0 = (int)J1 - 23;
        //        if (J0 < 0)
        //            J0 = J0 + 128;
        //        SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
        //        SSArray[J1] = I;
        //    }

        //    return Convert.ToByte(System.Math.Floor((SeedValue & 65535) * m_Scale2));
        //}

        //private static byte Generator4()
        //{
        //    double fTmp = 0;
        //    uint I = 0;
        //    uint J0 = 0;
        //    uint J1 = 0;
        //    fTmp = DWORD2Double(SeedValue) * 22695461.0 + 3.14159265358979;

        //    if (bFillSS)
        //    {
        //        SeedValue = Unchecked(fTmp);
        //    }
        //    else
        //    {
        //        I = Unchecked(fTmp);
        //        J1 = ShiftDWORD24(I) & 127;
        //        J0 = ShiftDWORD24(SeedValue) & 127;
        //        //J1 - 21

        //        SeedValue = (SSArray[J0] & 0xffff0000u) | (ShiftDWORD16(SSArray[J1]));
        //        SSArray[J0] = I;
        //    }

        //    return Convert.ToByte(System.Math.Floor((SeedValue & 65535) * m_Scale2));
        //}

        //private static byte Generator5()
        //{
        //    double fTmp = 0;
        //    uint I = 0;
        //    uint J0 = 0;
        //    uint J1 = 0;

        //    fTmp = DWORD2Double(SeedValue);
        //    fTmp = fTmp * (fTmp + 3.0);
        //    if (bFillSS)
        //    {
        //        SeedValue = Unchecked(fTmp);
        //    }
        //    else
        //    {
        //        I = Unchecked(fTmp);
        //        J1 = ShiftDWORD24(I) & 127;
        //        J0 = ShiftDWORD24(SeedValue) & 127;
        //        SeedValue = (SSArray[J1] & 0xffff0000u) | (ShiftDWORD16(SSArray[J0]));
        //        SSArray[J1] = I;
        //    }

        //    return Convert.ToByte(System.Math.Floor((SeedValue & 65535) * m_Scale2));
        //}

		private static void InitModule()
		{
			SeedValue = 0xffffffffu;
			GneratorIndex = m_NGnerators;
			MaxV = 9 + Strings.Asc("A");
			MinV = MaxV - Strings.Asc("0") + 1;
		}

		private static string LstStDtReader(string LST_DT, string key)
		{
			int I = 0;
			int J = 0;

			int[] Tab = null;

			char Ch = '\0';
			string ind = null;
			string Res = null;

			Tab = new int[] {
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			0,
			23,
			21,
			2,
			11,
			3,
			4,
			5,
			16,
			6,
			7,
			8,
			25,
			24,
			17,
			18,
			9,
			12,
			1,
			13,
			15,
			22,
			10,
			20,
			14,
			19
		};
			Res = "";

			for (I = 0; I <= Strings.Len(key) - 1; I++)
			{
				Ch = key[I];
				if (Ch <= '9')
				{
					J = Strings.Asc(Ch) - Strings.Asc("0");
				}
				else
				{
					J = Strings.Asc(Ch) - Strings.Asc("A") + 10;
				}

				ind = Convert.ToString(Tab[J]);

				if (Tab[J] < 10)
					ind = "0" + ind;
				Res = Res + ind;
			}

			return Convert.ToString(Convert.ToDouble(LST_DT) - Convert.ToDouble(Res));
		}

		public static int LstStDtWriter(string modulKey)
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

			string LastStart = null;
			string CRCCode = null;
			string mesNow = null;
			string yrNow = null;
			string LCode = null;
			//		Dim Achar As String
			string key = null;
			string dNow = null;
			string ind = null;
			string Res = null;

			string Str_Renamed = null;
			int I = 0;
			int J = 0;
			char Ch = '\0';
			int[] Tab = null;

			LCode = ARANFunctions.RegRead(Registry.CurrentUser, modulKey, "Acar", "");

			CRCCode = Strings.Mid(LCode, Strings.Len(LCode) - 7, 8);
			// получили CRC код
			LCode = Strings.Mid(LCode, 1, Strings.Len(LCode) - 8);

			LastStart = Strings.Mid(LCode, Strings.Len(LCode) - 9, 10);
			//получили дату последнего запуска
			LCode = Strings.Mid(LCode, 1, Strings.Len(LCode) - 10);

			// проверили его
			if (CRCCode != CRC32.CalcCRC32(LCode))
			{
				return -1;
			}

			Str_Renamed = LCode;

			key = Strings.Mid(LCode, Strings.Len(LCode) - 4, 5);
			//получили ключ
			LCode = Strings.Mid(LCode, 1, Strings.Len(LCode) - 5);

			SetNewSeedS(key);
			LCode = DeCodeString(LCode);

			// сформируем новую дату "последнего запуска"

			Tab = new int[] {
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			0,
			23,
			21,
			2,
			11,
			3,
			4,
			5,
			16,
			6,
			7,
			8,
			25,
			24,
			17,
			18,
			9,
			12,
			1,
			13,
			15,
			22,
			10,
			20,
			14,
			19
		};

			Res = "";
			for (I = 0; I <= Strings.Len(key) - 1; I++)
			{
				Ch = key[I];
				if (Ch <= '9')
				{
					J = Strings.Asc(Ch) - Strings.Asc("0");
				}
				else
				{
					J = Strings.Asc(Ch) - Strings.Asc("A") + 10;
				}

				ind = Convert.ToString(Tab[J]);

				if (Tab[J] < 10)
					ind = "0" + ind;
				Res = Res + ind;
			}

			dNow = Convert.ToString(DateAndTime.Day(DateAndTime.Now));
			mesNow = Convert.ToString(DateAndTime.Month(DateAndTime.Now));
			yrNow = Convert.ToString(DateAndTime.Year(DateAndTime.Now) - 2000);

			if (Strings.Len(dNow) < 2)
				dNow = "0" + dNow;
			if (Strings.Len(mesNow) < 2)
				mesNow = "0" + mesNow;
			if (Strings.Len(yrNow) < 2)
				yrNow = "0" + yrNow;

			LastStart = dNow + mesNow + yrNow;
			LastStart = Convert.ToString(Convert.ToDouble(LastStart) + Convert.ToDouble(Res));

			if (Strings.Len(LastStart) < 10)
				LastStart = "0" + LastStart;

			LCode = Str_Renamed;
			CRCCode = CRC32.CalcCRC32(Str_Renamed);

			LCode = LCode + LastStart + CRCCode;

			return ARANFunctions.RegWrite(Registry.CurrentUser, modulKey, "Acar", LCode);
		}

		public static ARAN.GeometryClasses.Polygon DecodeLCode(string LCode, string ModuleName,SpatialReference geoSP,SpatialReference prjSP)
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
			string X = null;
			string Y = null;
			string R = null;

			string yearKey = null;
			string mesKey = null;
			string dKey = null;

			int mesNow = 0;
			int yrNow = 0;
			int dNow = 0;
			int K = 0;
			int M = 0;
			int I = 0;

			double TL1 = 0;
			double TL2 = 0;
			double TL = 0;
			double Xc = 0;
			double Yc = 0;
			double Rc = 0;
            double XX =0, YY = 0;


            GeometryOperators geomOperator = new GeometryOperators();

            
			Polygon pResult = new Polygon();
			ARAN.GeometryClasses.Point P;

			pResult = new Polygon();

			if (Strings.Len(LCode) <= 0)
				return pResult;

			CRCCode = Strings.Mid(LCode, Strings.Len(LCode) - 7, 8);
			// получили CRC код
			LCode = Strings.Mid(LCode, 1, Strings.Len(LCode) - 8);

			LastStart = Strings.Mid(LCode, Strings.Len(LCode) - 9, 10);
			//получили дату последнего запуска
			LCode = Strings.Mid(LCode, 1, Strings.Len(LCode) - 10);

			if (CRCCode != CRC32.CalcCRC32(LCode))
				return pResult;
			// проверили его

			key = Strings.Mid(LCode, Strings.Len(LCode) - 4, 5);
			//получили ключ
			LCode = Strings.Mid(LCode, 1, Strings.Len(LCode) - 5);

			InitModule();
			SetNewSeedS(key);
			string dLCode = DeCodeString(LCode);
			LCode = dLCode;
			//LCode = "311299980070651141141051180971080260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"
			//LCode = "311299980090681011120971141161171141010260010000008-0732400005500-074410-010970-062230-014050-058290-023250-059230-031220-052000-034930-032790-012530-0326200005860"

			//==================================================================================================
			Tarix = Strings.Mid(LCode, 1, 8);
			// получили срок "годности"
			LCode = Strings.Mid(LCode, 9, Strings.Len(LCode));

			dKey = Strings.Mid(Tarix, 1, 2);
			mesKey = Strings.Mid(Tarix, 3, 2);
			yearKey = Strings.Mid(Tarix, 5, 4);
			TL = Convert.ToInt32(dKey) + Convert.ToInt32(mesKey) * 30.4375 + (Convert.ToInt32(yearKey) - 1899) * 365.25;
			//==================================================================================================
			LastStart = LstStDtReader(LastStart, key);

			dKey = Strings.Mid(LastStart, 1, 2);
			mesKey = Strings.Mid(LastStart, 3, 2);
			yearKey = Strings.Mid(LastStart, 5, 4);
			TL1 = Convert.ToInt32(dKey) + Convert.ToInt32(mesKey) * 30.4375 + (Convert.ToInt32(yearKey) + (2000 - 1899)) * 365.25;

			dNow = DateAndTime.Day(DateAndTime.Now);
			mesNow = DateAndTime.Month(DateAndTime.Now);
			yrNow = DateAndTime.Year(DateAndTime.Now) - 1899;
			TL2 = (dNow + mesNow * 30.4375 + yrNow * 365.25);

			TL1 = (TL1 < TL2 ? TL1 : TL2);
			//==================================================================================================
			// сравним дату последнего запуска LastStart и текущую. если Текущая МЕНЬШЕ даты LastStart - вылетаем
			if (TL < TL1)
				return pResult;
			//==================================================================================================

			PrgNL = Strings.Mid(LCode, 1, 3);
			// получили длину названия модуля
			LCode = Strings.Mid(LCode, 4, Strings.Len(LCode));

			R = Convert.ToString(1);
			PRG_NAME = "";
			tempS = Strings.Mid(LCode, 1, Convert.ToInt32(PrgNL) * 3);
			//получили название модуля в символьном виде
			// теперь сконвертируем его в строку
			for (I = 1; I <= Convert.ToInt32(PrgNL); I += 1)
			{
				PRG_NAME = PRG_NAME + Strings.Chr(Convert.ToInt32(Strings.Mid(tempS, Convert.ToInt32(R), 3)));
				R = Convert.ToString(Convert.ToDouble(R) + 3);
			}

			if (Strings.UCase(PRG_NAME) != Strings.UCase(ModuleName))
				return  pResult;

			LCode = Strings.Mid(LCode, Convert.ToInt32(PrgNL) * 3 + 1, Strings.Len(LCode));

			CountryCode = Strings.Mid(LCode, 1, 3);
			// получили код страны
			LCode = Strings.Mid(LCode, 4, Strings.Len(LCode));
			FigCount = Strings.Mid(LCode, 1, 3);
			// получили число фигур
			LCode = Strings.Mid(LCode, 4, Strings.Len(LCode));


			double fCeloe = 0;
			double fDrobnoe = 0;

			I = 1;
			for (K = 0; K <= Convert.ToInt32(FigCount) - 1; K++)
			{
                Polygon Pol = new Polygon();
                FigCode = Strings.Mid(LCode, 1, 1);
				// получили код фигуры
				I = I + 1;
                
				if (FigCode != "1")
				{
                    Ring ring = new Ring();
                   
                   PointsCount = Strings.Mid(LCode, I, 6);
					// получили число точек для данной фигуры
					I = I + 6;

					for (M = 0; M <= Convert.ToInt32(PointsCount) - 1; M++)
					{
						X = Strings.Mid(LCode, I, 7);
						Y = Strings.Mid(LCode, I + 7, 7);
						I = I + 14;
						//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

						Celoe = Strings.Mid(X, 1, 4);
						Drobnoe = Strings.Mid(X, 5, 3);
						fCeloe = Convert.ToDouble(Celoe);
						fDrobnoe = Convert.ToDouble(Drobnoe);
						Xc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
						// получили координату X

						Celoe = Strings.Mid(Y, 1, 4);
						Drobnoe = Strings.Mid(Y, 5, 3);
						fCeloe = Convert.ToDouble(Celoe);
						fDrobnoe = Convert.ToDouble(Drobnoe);
						Yc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
						// получили координату Y

                        GeographicToProjection(Xc, Yc, ref XX, ref YY);
                        P = new Point(XX, YY);
						//                P.PutCoords Xc, Yc
						//                Set P = ToPrj(P)   'спроецировали точки
						ring.AddPoint(P);
					}
                     Pol.Add(ring);
				}
				else
				{
					X = Strings.Mid(LCode, I, 7);
					Y = Strings.Mid(LCode, I + 7, 7);
					I = I + 14;
					//7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

					R = Strings.Mid(LCode, I, 8);
					I = I + 8;
					//8 позиций R (AAAAAA.BB)

					Celoe = Strings.Mid(X, 1, 4);
					Drobnoe = Strings.Mid(X, 5, 3);
					fCeloe = Convert.ToDouble(Celoe);
					fDrobnoe = Convert.ToDouble(Drobnoe);
					Xc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
					// получили координату X

					Celoe = Strings.Mid(Y, 1, 4);
					Drobnoe = Strings.Mid(Y, 5, 3);
					fCeloe = Convert.ToDouble(Celoe);
					fDrobnoe = Convert.ToDouble(Drobnoe);
					Yc = fCeloe + 0.001 * (fCeloe < 0.0 ? -fDrobnoe : fDrobnoe);
					// получили координату Y

					//MsgBox "x = " && Xc && "    Y = " && Yc

					
                    GeographicToProjection(Xc, Yc,ref XX,ref YY);
                    P= new Point(XX,YY);
						//                P.PutCoords Xc, Yc
						//                Set P = ToPrj(P)   'спроецировали точки
					//            P.PutCoords Xc, Yc
					//            Set P = ToPrj(P)   'спроецировали точки

					Celoe = Strings.Mid(R, 1, 6);
					Drobnoe = Strings.Mid(R, 7, 2);

					Rc = Convert.ToDouble(Celoe) * 1000.0 + Convert.ToDouble(Drobnoe);
					// получили значение радиуса если фигура круг
					Pol.AddRing(ARANFunctions.CreateCirclePrj(P,Rc));  //Functions.CreatePrjCircle(P, Rc);
				}

                //T = Pol as ITopologicalOperator2;
                //T.IsKnownSimple_2 = false;
                //T.Simplify();
                pResult = geomOperator.UnionGeometry(Pol,pResult).AsPolygon;//  T.Union(pResult) as IPolygon;
              
                //T = pResult as ITopologicalOperator2;
                //T.IsKnownSimple_2 = false;
                //T.Simplify();
			}
            ARAN.GeometryClasses.Polygon resultGeo = new Polygon();

            foreach (Ring ring in pResult)
            {
                Ring tmpRing = new Ring();
                foreach (ARAN.GeometryClasses.Point  pt in ring)
                {
                    
                    ProjectionToGeographic(pt.X, pt.Y, ref XX, ref YY);
                    tmpRing.Add(new ARAN.GeometryClasses.Point(XX, YY));
                }
                resultGeo.Add(tmpRing);
                
            }
            return resultGeo;

		}
	}
}
