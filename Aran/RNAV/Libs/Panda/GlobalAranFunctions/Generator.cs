using System;

namespace ARAN.Common
{
	public static class KeyParameters
	{
		public const int CodeLen = 36;
		//public static const int InputRange = 11;
	}

	public class Generator
	{
		const int SSTableSize = 128;
		const int SSTableMask = SSTableSize - 1;
		const int SSTableBits = 7;

		const int m_NGnerators	= 6;

		const double m_Scale1 = 36.0 / 4294967296.0;
		const double m_Scale2 = 36.0 / 65536.0;

		uint SeedValue;
		int GeneratorIndex;
		Boolean FillSSMode;
		uint[] SSArray;
		private Random r = new Random();

		public Generator()
		{
			SeedValue = 0;
			GeneratorIndex = 0;
		}
		/*
		public string GenerateNewSeed()
		{
			string retsult = "";
			int i = 0;

			while (i < 4)
			{
				int k = r.Next(KeyParameters.CodeLen);
				if (k < 10) retsult += (char)(k + (int)'0');
				else if (k < KeyParameters.CodeLen) retsult += (char)(k + (int)'A' - 10);
				else continue;
				i++;
			}
			return retsult;
		}*/

		public byte Generate()
		{
			switch (GeneratorIndex)
			{
				case 0: return Generate0();
				case 1: return Generate1();
				case 2: return Generate2();
				case 3: return Generate3();
				case 4: return Generate4();
				case 5: return Generate5();
			};
			return 0;
		}

		public void SetNewSeedL(uint initSeed)
		{
			uint i;
			SeedValue = initSeed;
			SSArray = new uint[SSTableSize];

			for (i = 0; i < 128; i++)
			{
				Generate();
				SSArray[i] = (SeedValue & 0xFFFFFFFE) | (i & 1);
			}
			SeedValue = initSeed;
		}

		uint DecodeSeedStr(String initString, out int index)
		{
			int i, l;
			uint d, ch, Result;

			i = initString[0];
			if (i <= '9') i -= '0';
			else i -= 10 - 'A';

			index = i - (KeyParameters.CodeLen >> 1);
			if (index < 0)
				index = index + KeyParameters.CodeLen;
			if (index >= m_NGnerators)
				new Exception("Unsupported format string reached.");

			l = initString.Length;
			Result = 0;

			for (i = 1; i < l; i++)
			{
				ch = (uint)initString[i];

				if (ch <= '9') d = ch - '0';
				else d = ch - 'A' + 10;

				Result = Result * KeyParameters.CodeLen + d;
			}

			return Result;
		}

		public void SetNewSeedS(String initString)
		{
			uint initValue;
			initValue = DecodeSeedStr(initString, out GeneratorIndex);
			SetNewSeedL(initValue);
		}

		private byte Generate0()
		{
			uint i, k, j1;
			int j0;

			i = 22695477 * SeedValue + 37;

			if (FillSSMode)
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

				SeedValue = (SSArray[j0] & 0xFFFF0000) | (SSArray[j1] >> 16);
				SSArray[j1] = i;
			}

			return (byte)(Math.Floor(m_Scale1 * SeedValue));
		}

		private byte Generate1()
		{
			uint i, j0, j1;

			i = 22695461 * SeedValue + 3;

			if (FillSSMode)
				SeedValue = i;
			else
			{
				j1 = (i >> 24) & SSTableMask;
				j0 = (SeedValue >> 24) & SSTableMask;

				SeedValue = (SSArray[j0] & 0xFFFF0000) | (SSArray[j1] >> 16);
				SSArray[j0] = i;
			}

			return (byte)(Math.Floor(m_Scale1 * SeedValue));
		}

		private byte Generate2()
		{
			uint i, j0, j1;

			i = SeedValue * (SeedValue + 3);

			if (FillSSMode)
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

		private byte Generate3()
		{
			uint i, k, j1;
			int j0;

			i = 22695477 * SeedValue + 37;

			if (FillSSMode)
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

		private byte Generate4()
		{
			uint i, j0, j1;

			i = 22695461 * SeedValue + 3;

			if (FillSSMode)
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

		private byte Generate5()
		{
			uint i, j0, j1;

			i = SeedValue * (SeedValue + 3);

			if (FillSSMode)
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
	}
}
