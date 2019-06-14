using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class FileRoutines
	{
		public static void GetShortData(byte[] data, ref uint index, out short Vara)
		{
			Vara = 0;

			for (int i = 0, E = 0; i < 2; i++, E += 8)
				Vara += (short)(data[index + i] << E);

			index += 2;
		}

		public static void GetIntData(byte[] data, ref uint index, out int Vara)
		{
			Vara = 0;

			for (int i = 0, E = 0; i < 4; i++, E += 8)
				Vara += (int)(data[index + i] << E);

			index += 4;
		}

		public static void GetStrData(byte[] data, ref uint index, out string Vara, int size)
		{
			Vara = "";

			for (int i = 0; i < size; i++)
				Vara += (char)data[index++];
		}

		public static void GetPStrData(byte[] data, ref uint index, out string Vara)
		{
			short size;
			GetShortData(data, ref index, out size);

			Vara = "";

			for (int i = 0; i < size; i++)
				Vara += (char)data[index++];
		}

		public static void GetCStrData(byte[] data, ref uint index, out string Vara)
		{
			Vara = "";
			int i = data[index++];

			while (i != 0)
			{
				Vara += (char)i;
				i = data[index++];
			}
		}

		public static void GetDoubleData(byte[] data, ref uint index, out double Vara)
		{
			double mantissa = 0;

			for (int i = 0; i < 8 - 2; i++)
				mantissa = data[index + i] + 0.00390625 * mantissa;

			mantissa = 0.0625 * (((data[index + 8 - 2] & 15) + 16) + 0.00390625 * mantissa);

			int exponent = (int)(0.0625 * (data[index + 8 - 2] & 240));
			exponent = (int)(data[index + 8 - 1] * 16 + exponent);
			index += 8;

			if (mantissa == 1 && exponent == 0)
			{
				Vara = 0.0;
				return;
			}

			int Sign = exponent & 2048;
			exponent = (exponent & 2047) - 1023;

			if (exponent > 0)
				for (int i = 0; i < exponent; i++)
					mantissa = mantissa * 2.0;
			else if (exponent < 0)
				for (int i = -1; i >= exponent; i--)
					mantissa = 0.5 * mantissa;

			if (Sign != 0)
				mantissa = -mantissa;

			Vara = mantissa;
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

		public static ITable OpenTableFromFile(string sFolderName, string sFileName)
		{
			IWorkspaceFactory pFact = new ShapefileWorkspaceFactory();
			IWorkspace pWorkspace = pFact.OpenFromFile(sFolderName, GlobalVars.GetApplicationHWnd());
			return ((IFeatureWorkspace)(pWorkspace)).OpenTable(sFileName);
		}
	}
}
