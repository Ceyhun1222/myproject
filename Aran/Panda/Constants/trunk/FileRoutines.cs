
namespace Aran.PANDA.Constants
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class FileRoutines
	{
		public static short GetShortData(byte[] data, ref uint index)
		{
			const uint size = 2;
			short Vara = 0;

			for (int i = 0, j = 0; i < size; i++, j += 8)
				Vara += (short)(data[index + i] << j);

			index += size;
			return Vara;
		}

		public static int GetIntData(byte[] data, ref uint index)
		{
			const uint size = 4;
			int Vara = 0;

			for (int i = 0, j = 0; i < size; i++, j += 8)
				Vara += (short)(data[index + i] << j);

			index += size;
			return Vara;
		}

		public static string GetStrData(byte[] data, ref uint index, int size)
		{
			string Vara = "";

			for (int i = 0; i < size; i++)
				Vara += (char)data[index++];

			return Vara;
		}

		public static string GetPStrData(byte[] data, ref uint index)
		{
			short size = GetShortData(data, ref index);
			string Vara = "";

			for (int i = 0; i < size; i++)
				Vara += (char)data[index++];

			return Vara;
		}

		public static string GetCStrData(byte[] data, ref uint index)
		{
			string Vara = "";
			int i = data[index];

			while (i != 0)
			{
				Vara += (char)i;
				i = data[index++];
			}

			return Vara;
		}

		public static double GetDoubleData(byte[] data, ref uint index)
		{
			double mantissa = 0;
			const uint size = 8;

			for (int i = 0; i < size - 2; i++)
				mantissa = data[index + i] + 0.00390625 * mantissa;

			mantissa = 0.0625 * (((data[index + size - 2] & 15) + 16) + 0.00390625 * mantissa);

			int exponent = (int)(0.0625 * (data[index + size - 2] & 240));
			exponent = (int)(data[index + size - 1] * 16.0 + exponent);
			index += size;

			if (mantissa == 1 && exponent == 0)
				return 0.0;

			int sign = exponent & 2048;
			exponent = (exponent & 2047) - 1023;

			if (exponent > 0)
				for (int i = 0; i < exponent; i++)
					mantissa = mantissa * 2.0;
			else if (exponent < 0)
				for (int i = -1; i >= exponent; i--)
					mantissa = 0.5 * mantissa;

			if (sign != 0)
				mantissa = -mantissa;

			return mantissa;
		}

		public static void GetData(byte[] data, ref uint index, out byte[] Vara, int size)
		{
			if (size > -1)
				Vara = new byte[size];
			else
				Vara = new byte[0];

			for (int i = 0; i < size; i++)
				Vara[i] = data[index++];
		}
	}
}
