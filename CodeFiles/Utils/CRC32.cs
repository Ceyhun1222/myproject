﻿using System;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class CRC32
	{
		#region Tables

		private static UInt32[] CRCTable =
		{
			0x00000000, 0x814141AB, 0x83C3C2FD, 0x02828356,
			0x86C6C451, 0x078785FA, 0x050506AC, 0x84444707,
			0x8CCCC909, 0x0D8D88A2, 0x0F0F0BF4, 0x8E4E4A5F,
			0x0A0A0D58, 0x8B4B4CF3, 0x89C9CFA5, 0x08888E0E,
			0x98D8D3B9, 0x19999212, 0x1B1B1144, 0x9A5A50EF,
			0x1E1E17E8, 0x9F5F5643, 0x9DDDD515, 0x1C9C94BE,
			0x14141AB0, 0x95555B1B, 0x97D7D84D, 0x169699E6,
			0x92D2DEE1, 0x13939F4A, 0x11111C1C, 0x90505DB7,
			0xB0F0E6D9, 0x31B1A772, 0x33332424, 0xB272658F,
			0x36362288, 0xB7776323, 0xB5F5E075, 0x34B4A1DE,
			0x3C3C2FD0, 0xBD7D6E7B, 0xBFFFED2D, 0x3EBEAC86,
			0xBAFAEB81, 0x3BBBAA2A, 0x3939297C, 0xB87868D7,
			0x28283560, 0xA96974CB, 0xABEBF79D, 0x2AAAB636,
			0xAEEEF131, 0x2FAFB09A, 0x2D2D33CC, 0xAC6C7267,
			0xA4E4FC69, 0x25A5BDC2, 0x27273E94, 0xA6667F3F,
			0x22223838, 0xA3637993, 0xA1E1FAC5, 0x20A0BB6E,

			0xE0A08C19, 0x61E1CDB2, 0x63634EE4, 0xE2220F4F,
			0x66664848, 0xE72709E3, 0xE5A58AB5, 0x64E4CB1E,
			0x6C6C4510, 0xED2D04BB, 0xEFAF87ED, 0x6EEEC646,
			0xEAAA8141, 0x6BEBC0EA, 0x696943BC, 0xE8280217,
			0x78785FA0, 0xF9391E0B, 0xFBBB9D5D, 0x7AFADCF6,
			0xFEBE9BF1, 0x7FFFDA5A, 0x7D7D590C, 0xFC3C18A7,
			0xF4B496A9, 0x75F5D702, 0x77775454, 0xF63615FF,
			0x727252F8, 0xF3331353, 0xF1B19005, 0x70F0D1AE,
			0x50506AC0, 0xD1112B6B, 0xD393A83D, 0x52D2E996,
			0xD696AE91, 0x57D7EF3A, 0x55556C6C, 0xD4142DC7,
			0xDC9CA3C9, 0x5DDDE262, 0x5F5F6134, 0xDE1E209F,
			0x5A5A6798, 0xDB1B2633, 0xD999A565, 0x58D8E4CE,
			0xC888B979, 0x49C9F8D2, 0x4B4B7B84, 0xCA0A3A2F,
			0x4E4E7D28, 0xCF0F3C83, 0xCD8DBFD5, 0x4CCCFE7E,
			0x44447070, 0xC50531DB, 0xC787B28D, 0x46C6F326,
			0xC282B421, 0x43C3F58A, 0x414176DC, 0xC0003777,

			0x40005999, 0xC1411832, 0xC3C39B64, 0x4282DACF,
			0xC6C69DC8, 0x4787DC63, 0x45055F35, 0xC4441E9E,
			0xCCCC9090, 0x4D8DD13B, 0x4F0F526D, 0xCE4E13C6,
			0x4A0A54C1, 0xCB4B156A, 0xC9C9963C, 0x4888D797,
			0xD8D88A20, 0x5999CB8B, 0x5B1B48DD, 0xDA5A0976,
			0x5E1E4E71, 0xDF5F0FDA, 0xDDDD8C8C, 0x5C9CCD27,
			0x54144329, 0xD5550282, 0xD7D781D4, 0x5696C07F,
			0xD2D28778, 0x5393C6D3, 0x51114585, 0xD050042E,
			0xF0F0BF40, 0x71B1FEEB, 0x73337DBD, 0xF2723C16,
			0x76367B11, 0xF7773ABA, 0xF5F5B9EC, 0x74B4F847,
			0x7C3C7649, 0xFD7D37E2, 0xFFFFB4B4, 0x7EBEF51F,
			0xFAFAB218, 0x7BBBF3B3, 0x793970E5, 0xF878314E,
			0x68286CF9, 0xE9692D52, 0xEBEBAE04, 0x6AAAEFAF,
			0xEEEEA8A8, 0x6FAFE903, 0x6D2D6A55, 0xEC6C2BFE,
			0xE4E4A5F0, 0x65A5E45B, 0x6727670D, 0xE66626A6,
			0x622261A1, 0xE363200A, 0xE1E1A35C, 0x60A0E2F7,

			0xA0A0D580, 0x21E1942B, 0x2363177D, 0xA22256D6,
			0x266611D1, 0xA727507A, 0xA5A5D32C, 0x24E49287,
			0x2C6C1C89, 0xAD2D5D22, 0xAFAFDE74, 0x2EEE9FDF,
			0xAAAAD8D8, 0x2BEB9973, 0x29691A25, 0xA8285B8E,
			0x38780639, 0xB9394792, 0xBBBBC4C4, 0x3AFA856F,
			0xBEBEC268, 0x3FFF83C3, 0x3D7D0095, 0xBC3C413E,
			0xB4B4CF30, 0x35F58E9B, 0x37770DCD, 0xB6364C66,
			0x32720B61, 0xB3334ACA, 0xB1B1C99C, 0x30F08837,
			0x10503359, 0x911172F2, 0x9393F1A4, 0x12D2B00F,
			0x9696F708, 0x17D7B6A3, 0x155535F5, 0x9414745E,
			0x9C9CFA50, 0x1DDDBBFB, 0x1F5F38AD, 0x9E1E7906,
			0x1A5A3E01, 0x9B1B7FAA, 0x9999FCFC, 0x18D8BD57,
			0x8888E0E0, 0x09C9A14B, 0x0B4B221D, 0x8A0A63B6,
			0x0E4E24B1, 0x8F0F651A, 0x8D8DE64C, 0x0CCCA7E7,
			0x044429E9, 0x85056842, 0x8787EB14, 0x06C6AABF,
			0x8282EDB8, 0x03C3AC13, 0x01412F45, 0x80006EEE
		};

		private static char[] CharTab = {	'0', '1', '2', '3', '4', '5', '6', '7',
											'8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

		#endregion

		private static UInt32 CRC32Poly = 0x814141abu;

		public static UInt32 Polynom
		{
			get { return CRC32Poly; }
			set
			{
				CRC32Poly = value;

				for (UInt32 i = 0; i < 256; i++)
				{
					UInt32 lostBit, CRC = i << 24;
					for (UInt32 j = 0; j < 8; j++)
					{
						lostBit = CRC >> 31;
						CRC = CRC << 1;

						if (lostBit == 1)
							CRC = (CRC ^ CRC32Poly) | 1;
					}
					CRCTable[i] = CRC;
				}
			}
		}

		public static string CalcCRC32(string str)
		{
			int i, l = str.Length;

			// Calc CRC value
			UInt32 CRCValue = 0;

			for (i = 0; i < l; i++)
			{
				byte b = (byte)str[i];
				CRCValue = (CRCValue << 8) ^ CRCTable[((CRCValue >> 24) ^ b) & 0xFF];
			}

			// Convert CRC value to string
			string res = "";

			for (i = 0; i < 8; i++)
			{
				res = CharTab[CRCValue & 0xF] + res;
				CRCValue >>= 4;
			}
			return res;
		}

		public static string CalcCRC32(byte[] bytes, int from = 0, int length = -1)
		{
			int l = bytes.Length;

			if (length == -1 || from + length >= l)
				length = l - from;

			int to = from + length;

			// Calc CRC value
            UInt32 crcValue = 0;

			for (int i = from; i < to; i++)
				crcValue = (crcValue << 8) ^ CRCTable[((crcValue >> 24) ^ bytes[i]) & 0xFF];

			// Convert CRC value to string
			return crcValue.ToString("X8");
		}

        public static uint CalcCRC32_2(byte[] bytes, UInt32 crcValue = 0, int from = 0, int length = -1)
        {
            int l = bytes.Length;

            if (length == -1 || from + length >= l)
                length = l - from;

            int to = from + length;

            // Calc CRC value
            //UInt32 crcValue = 0xffffffff;

            for (int i = from; i < to; i++)
                crcValue = (crcValue << 8) ^ CRCTable[((crcValue >> 24) ^ bytes[i]) & 0xFF];

            return crcValue;
        }
	}
}


/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/