using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OASCalculator
{
	public static class Functions
	{
		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		public static bool GetCoord(string sDeg, string sMin, string sSec, out double val)
		{
			double deg = 0.0, min = 0.0, sec = 0.0;
			long isign, num = 0;
			val = 0.0;
			if (sDeg != null && sDeg != "")
			{
				deg = double.Parse(sDeg);
				num++;
			}

			if (sMin != null && sMin != "")
			{
				min = double.Parse(sMin);
				num++;
			}

			if (sSec != null && sSec != "")
			{
				sec = double.Parse(sSec);
				num++;
			}

			if (num < 1)
				return false;

			isign = Math.Sign(deg);
			deg = Math.Abs(deg);
			min = Math.Abs(min);
			sec = Math.Abs(sec);

			val = isign * (deg + min / 60.0 + sec / 3600.0);
			return true;
		}

		public static string ConvertDegrees(double val, long degMode)//char *Buffer, 
		{
			long iaz, maz, iss;
			double saz;
			char signChr;
			string result = string.Empty;

			if (Math.Abs(val) < 1e-8)
				val = 0;

			if (val < 0.0)
				signChr = '-';
			else
				signChr = ' ';

			val = Math.Abs(val);

			switch (degMode)
			{
				case 0:
					if (Math.Abs(val - 360.0) < 1e-10)
						val = 0.0;
					result = string.Format("{0}{1}°", signChr, val.ToString("000.000000"));
					break;
				case 1:
					saz = val;
					iaz = (long)saz;
					saz = (saz - iaz) * 60.0;

					iss = (long)(saz * 1e8);
					if (iss >= 6000000000)
					{
						saz = 0.0;
						iaz++;
						if (iaz == 360)
							iaz = 0;
					}
					result = string.Format("{0}{1}°{2}'", signChr, iaz, saz.ToString("00.000000"));
					break;
				case 2:
					saz = val;
					iaz = (long)saz;
					saz = (saz - iaz) * 60.0;
					maz = (long)saz;
					saz = (saz - maz) * 60.0;

					//	account for rounding error

					iss = (long)(saz * 1e8);
					if (iss >= 6000000000)
					{
						saz = 0.0;
						maz++;
						if (maz >= 60)
						{
							maz = 0;
							iaz++;
							if (iaz == 360)
								iaz = 0;
						}
					}

					result = string.Format("{0}{1}'{2} {3}\"", signChr, iaz, maz, saz.ToString("00.000000"));
					break;
			}

			return result;
		}

	}
}
