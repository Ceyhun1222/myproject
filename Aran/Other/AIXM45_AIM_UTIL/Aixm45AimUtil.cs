using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using System.Globalization;

namespace AIXM45_AIM_UTIL
{
    public static class Aixm45AimUtil
    {
        private static Dictionary<string, Type> elementTypes = new Dictionary<string, Type>();


        static Aixm45AimUtil()
        {
			elementTypes.Add ("Organisation", typeof (OrganisationReader));
            elementTypes.Add("AD_HP", typeof(ADHPReader));
            elementTypes.Add("RWY", typeof(RunwayReader));
            elementTypes.Add("RwyDirection", typeof(RunwayDirectionReader));
            elementTypes.Add("RwyClinePoint", typeof(RwyClinePointReader));
			elementTypes.Add("RunwayDirectionDeclaredDistance", typeof (RunwayDirectionDeclaredDistance));
            elementTypes.Add("NDB", typeof(NDBReader));
            elementTypes.Add("DME", typeof(DMEReader));
            elementTypes.Add("VOR", typeof(VORReader));
            elementTypes.Add("TACAN", typeof(TACANReader));
            elementTypes.Add("ILZ", typeof(LocalizerReader));
            elementTypes.Add("IGP", typeof(GlidepathReader));
            elementTypes.Add("ILS", typeof(ILSReader));
			
        }

        public static IAIXM45_DATA_READER CreateReader(string MdbFile, string ARAN_Table_Name)
        {
            IWorkspaceFactory Wksps_Fctr = new AccessWorkspaceFactory();
            IWorkspace Wksps = Wksps_Fctr.OpenFromFile(MdbFile, 0);
            IFeatureWorkspace FTR_WKSPS = Wksps as IFeatureWorkspace;
			ITable ARAN_Table = null;

			try
			{
				ARAN_Table= FTR_WKSPS.OpenTable (ARAN_Table_Name);
			}
			catch (Exception)
			{
				return new NullDataReader ();
			}


            Type t = elementTypes[ARAN_Table_Name];
            IAIXM45_DATA_READER result = (IAIXM45_DATA_READER)Activator.CreateInstance(t);
            result.ReadDataFromTable(ARAN_Table_Name, ARAN_Table);
            return result;
        }

        public interface IAIXM45_DATA_READER
        {
            List<ConvertedObj> ListOfObjects { get; set; }
            void ReadDataFromTable(string TblName, ITable ARAN_TABLE);
        }

		public class NullDataReader : IAIXM45_DATA_READER
		{
			public NullDataReader ()
			{
				ListOfObjects = new List<ConvertedObj> ();
			}

			public List<ConvertedObj> ListOfObjects { get; set; }

			public void ReadDataFromTable (string TblName, ITable ARAN_TABLE)
			{
			}
		}

        public static IPoint Create_ESRI_POINT(string Lat, string Lon, string Elev, string ElevUom)
        {

            IPoint pnt = new PointClass();
            pnt.PutCoords(GetLONGITUDEFromAIXMString(Lon), GetLATITUDEFromAIXMString(Lat));
            IZAware zAware = pnt as IZAware;
            zAware.ZAware = true;
            pnt.Z = ConvertValueToMeter(Elev, ElevUom); // сохрагним в Z значение, переведенное в метры

            IMAware mAware = pnt as IMAware;
            mAware.MAware = true;
            pnt.M = ConvertValueToMeter(Elev, "M"); // сохрагним в М значение в тех ЕИ, которые представлены в файле AIXM

            return pnt;

        }

        public static double GetLONGITUDEFromAIXMString_R (string AIXM_COORD)
        {
            // ПЕРЕВОД ДОЛГОТЫ из AIXM в формат DD.MM 

            //				A string of "digits" (plus, optionally, a period) followed by one of the 
            //				"Simple Latin upper case letters" E or W, in the forms DDDMMSS.ssY, DDDMMSSY, DDDMM.mm...Y, DDDMMY, and DDD.dd...Y . 
            //				The Y stands for either E (= East) or W (= West), DDD represents whole degrees, MM whole minutes, and SS whole seconds. 
            //				The period indicates that there are decimal fractions present; whether these are fractions of seconds, minutes, 
            //				or degrees can easily be deduced from the position of the period. The number of digits representing the fractions 
            //				of seconds is 1 = s... <= 4; the relevant number for fractions of minutes and degrees is 1 <= d.../m... <= 8.

            string DD = "";
            string MM = "";
            string SS = "";
            string STORONA_SVETA = "";
            int SIGN = 1;
            double Coord = 0;
            double Gradusy = 0.0;
            double Minuty = 0.0;
            double Sekundy = 0.0;
            string Coordinata = "";

            try
            {

                NumberFormatInfo nfi = new NumberFormatInfo();

                nfi.NumberDecimalSeparator = ".";
                nfi.NumberGroupSeparator = " ";
                nfi.PositiveSign = "+";

                STORONA_SVETA = AIXM_COORD.Substring(AIXM_COORD.Length - 1, 1);
                if (STORONA_SVETA == "W") SIGN = -1;


                if (IsNumeric(STORONA_SVETA))
                {
                    STORONA_SVETA = AIXM_COORD.Substring(0, 1);
                    if (STORONA_SVETA == "S") SIGN = -1;
                    AIXM_COORD = AIXM_COORD.Substring(1, AIXM_COORD.Length - 1);
                }
                else
                    AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);
                //AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);

                int SepPos = AIXM_COORD.LastIndexOf(".");

                if (SepPos > 0) //DDDMMSS.ss...X, DDDMM.mm...X, and DDD.dd...X
                {
                    Coordinata = AIXM_COORD.Substring(0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 3:  //DDD.dd...
                            Coord = Convert.ToDouble(AIXM_COORD, nfi) * SIGN;
                            break;

                        case 5:  //DDDMM.mm... 
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, AIXM_COORD.Length - 3);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 7:  //DDDMMSS.ss... 
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, 2);
                            SS = AIXM_COORD.Substring(5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }
                else //DDDMMSSX and DDDMMX
                {
                    Coordinata = AIXM_COORD;
                    switch (Coordinata.Length)
                    {
                        case 5:  //DDDMM 
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, AIXM_COORD.Length - 3);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 7:  //DDDMMSS
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, 2);
                            SS = AIXM_COORD.Substring(5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                        case 9:  //DDDMMSS.SS
                            DD = AIXM_COORD.Substring(0, 3);
                            MM = AIXM_COORD.Substring(3, 2);
                            SS = AIXM_COORD.Substring(5, AIXM_COORD.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi) / 100;
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }


            }
            catch
            {
               Coord = 0;
            }

            return Coord;

        }

        public static double GetLATITUDEFromAIXMString_R (string AIXM_COORD)
        {
            // ПЕРЕВОД ШИРОТЫ из AIXM в формат DD.MM 

            //A string of "digits" (plus, optionally, a period) followed by one of the "Simple Latin upper case letters" N or S, 
            //in the forms DDMMSS.ss...X, DDMMSSX, DDMM.mm...X, DDMMX, and DD.dd...X. The X stands for either N (= North) or S (= South), 
            //DD represents whole degrees, MM whole minutes, and SS whole seconds. The period indicates that there are decimal 
            //fractions present; whether these are fractions of seconds, minutes, or degrees can easily be deduced from the position 
            //of the period. The number of digits representing the fractions of seconds is 1<= s... <= 4; the relevant number for 
            //fractions of minutes and degrees is 1 <= d.../m... <= 8.

            string DD = "";
            string MM = "";
            string SS = "";
            string STORONA_SVETA = "";
            int SIGN = 1;
            double Coord = 0;
            double Gradusy = 0.0;
            double Minuty = 0.0;
            double Sekundy = 0.0;
            string Coordinata = "";

            try
            {
                NumberFormatInfo nfi = new NumberFormatInfo();

                nfi.NumberDecimalSeparator = ".";
                nfi.NumberGroupSeparator = " ";
                nfi.PositiveSign = "+";

                STORONA_SVETA = AIXM_COORD.Substring(AIXM_COORD.Length - 1, 1);
                if (STORONA_SVETA == "S") SIGN = -1;



                if (IsNumeric(STORONA_SVETA))
                {
                    STORONA_SVETA = AIXM_COORD.Substring(0, 1);
                    if (STORONA_SVETA == "S") SIGN = -1;
                    AIXM_COORD = AIXM_COORD.Substring(1, AIXM_COORD.Length - 1);
                }
                else
                    AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);



                int SepPos = AIXM_COORD.LastIndexOf(".");

                if (SepPos > 0) //DDMMSS.ss...X, DDMM.mm...X, and DD.dd...X
                {
                    Coordinata = AIXM_COORD.Substring(0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 2:  //DD.dd...
                            Coord = Convert.ToDouble(AIXM_COORD, nfi) * SIGN;
                            break;

                        case 4:  //DDMM.mm... 
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, AIXM_COORD.Length - 2);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 6:  //DDMMSS.ss... 
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, 2);
                            SS = AIXM_COORD.Substring(4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }
                else //DDMMSSX and DDMMX
                {
                    Coordinata = AIXM_COORD;
                    switch (Coordinata.Length)
                    {
                        case 4:  //DDMM 
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, AIXM_COORD.Length - 2);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 6:  //DDMMSS
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, 2);
                            SS = AIXM_COORD.Substring(4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                        case 8:  //DDMMSS.ss
                            DD = AIXM_COORD.Substring(0, 2);
                            MM = AIXM_COORD.Substring(2, 2);
                            SS = AIXM_COORD.Substring(4, AIXM_COORD.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi) / 100;
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }


            }
            catch
            {
                Coord = 0;
            }

            return Coord;

        }





		public static double GetLONGITUDEFromAIXMString (string AIXM_COORD)
		{
			// Џ…ђ…‚Ћ„ „Ћ‹ѓЋ’› Ё§ AIXM ў д®а¬ в DD.MM
			// A string of "digits" (plus, optionally, a period) followed by one of the
			// "Simple Latin upper case letters" E or W, in the forms DDDMMSS.ssY, DDDMMSSY, DDDMM.mm...Y, DDDMMY, and DDD.dd...Y . 
			// The Y stands for either E (= East) or W (= West), DDD represents whole degrees, MM whole minutes, and SS whole seconds. 
			// The period indicates that there are decimal fractions present; whether these are fractions of seconds, minutes,
			// or degrees can easily be deduced from the position of the period. The number of digits representing the fractions
			// of seconds is 1 = s... <= 4; the relevant number for fractions of minutes and degrees is 1 <= d.../m... <= 8.
			string DD = "";
			string MM = "";
			string SS = "";
			string STORONA_SVETA = "";
			int SIGN = 1;
			double Coord = 0;
			double Gradusy = 0.0;
			double Minuty = 0.0;
			double Sekundy = 0.0;
			string Coordinata = "";

			try
			{
				NumberFormatInfo nfi = new NumberFormatInfo ();
				nfi.NumberGroupSeparator = " ";
				nfi.PositiveSign = "+";
				STORONA_SVETA = AIXM_COORD.Substring (AIXM_COORD.Length - 1, 1);

				if (STORONA_SVETA == "W")
					SIGN = -1;

				AIXM_COORD = AIXM_COORD.Substring (0, AIXM_COORD.Length - 1);
				int PntPos = AIXM_COORD.LastIndexOf (".");
				int CommPos = AIXM_COORD.LastIndexOf (",");
				int SepPos;

				if (PntPos == -1 && CommPos > -1)
				{
					nfi.NumberDecimalSeparator = ",";
					SepPos = CommPos;
				}
				else
				{
					nfi.NumberDecimalSeparator = ".";
					SepPos = PntPos;
				}

				if (SepPos > 0) //DDDMMSS.ss...X, DDDMM.mm...X, and DDD.dd...X
				{
					Coordinata = AIXM_COORD.Substring (0, SepPos);
					switch (Coordinata.Length)
					{
						case 3: //DDD.dd...
							Coord = Convert.ToDouble (AIXM_COORD, nfi) * SIGN;
							break;
						case 5: //DDDMM.mm... 
							DD = AIXM_COORD.Substring (0, 3);
							MM = AIXM_COORD.Substring (3, AIXM_COORD.Length - 3);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Coord = (Gradusy + (Minuty / 60)) * SIGN;
							break;
						case 7: //DDDMMSS.ss... 
							DD = AIXM_COORD.Substring (0, 3);
							MM = AIXM_COORD.Substring (3, 2);
							SS = AIXM_COORD.Substring (5, AIXM_COORD.Length - 5);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Sekundy = Convert.ToDouble (SS, nfi);
							Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
							break;
					}
				}
				else //DDDMMSSX and DDDMMX
				{
					Coordinata = AIXM_COORD;
					switch (Coordinata.Length)
					{
						case 5: //DDDMM
							DD = AIXM_COORD.Substring (0, 3);
							MM = AIXM_COORD.Substring (3, AIXM_COORD.Length - 3);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Coord = (Gradusy + (Minuty / 60)) * SIGN;
							break;
						case 7: //DDDMMSS
							DD = AIXM_COORD.Substring (0, 3);
							MM = AIXM_COORD.Substring (3, 2);
							SS = AIXM_COORD.Substring (5, AIXM_COORD.Length - 5);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Sekundy = Convert.ToDouble (SS, nfi);
							Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Coord = 0;
				throw ex;
			}
			return Coord;
		}

		public static double GetLATITUDEFromAIXMString (string AIXM_COORD)
		{
			// Џ…ђ…‚Ћ„ €ђЋ’› Ё§ AIXM ў д®а¬ в DD.MM
			//A string of "digits" (plus, optionally, a period) followed by one of the "Simple Latin upper case letters" N or S,
			//in the forms DDMMSS.ss...X, DDMMSSX, DDMM.mm...X, DDMMX, and DD.dd...X. The X stands for either N (= North) or S (= South),
			//DD represents whole degrees, MM whole minutes, and SS whole seconds. The period indicates that there are decimal
			//fractions present; whether these are fractions of seconds, minutes, or degrees can easily be deduced from the position
			//of the period. The number of digits representing the fractions of seconds is 1<= s... <= 4; the relevant number for
			//fractions of minutes and degrees is 1 <= d.../m... <= 8.
			string DD = "";
			string MM = "";
			string SS = "";
			string STORONA_SVETA = "";

			int SIGN = 1;
			double Coord = 0;
			double Gradusy = 0.0;
			double Minuty = 0.0;
			double Sekundy = 0.0;
			string Coordinata = "";
			try
			{
				NumberFormatInfo nfi = new NumberFormatInfo ();

				nfi.NumberGroupSeparator = " ";
				nfi.PositiveSign = "+";
				STORONA_SVETA = AIXM_COORD.Substring (AIXM_COORD.Length - 1, 1);
				if (STORONA_SVETA == "S")
					SIGN = -1;
				AIXM_COORD = AIXM_COORD.Substring (0, AIXM_COORD.Length - 1);
				int PntPos = AIXM_COORD.LastIndexOf (".");
				int CommPos = AIXM_COORD.LastIndexOf (",");
				int SepPos;

				if (PntPos == -1 && CommPos > -1)
				{
					nfi.NumberDecimalSeparator = ",";
					SepPos = CommPos;
				}
				else
				{
					nfi.NumberDecimalSeparator = ".";
					SepPos = PntPos;
				}

				if (SepPos > 0) //DDMMSS.ss...X, DDMM.mm...X, and DD.dd...X
				{
					Coordinata = AIXM_COORD.Substring (0, SepPos);
					switch (Coordinata.Length)
					{
						case 2: //DD.dd...
							Coord = Convert.ToDouble (AIXM_COORD, nfi) * SIGN;
							break;
						case 4: //DDMM.mm... 
							DD = AIXM_COORD.Substring (0, 2);
							MM = AIXM_COORD.Substring (2, AIXM_COORD.Length - 2);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Coord = (Gradusy + (Minuty / 60)) * SIGN;
							break;
						case 6: //DDMMSS.ss... 
							DD = AIXM_COORD.Substring (0, 2);
							MM = AIXM_COORD.Substring (2, 2);
							SS = AIXM_COORD.Substring (4, AIXM_COORD.Length - 4);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Sekundy = Convert.ToDouble (SS, nfi);
							Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
							break;
					}
				}
				else //DDMMSSX and DDMMX
				{
					Coordinata = AIXM_COORD;
					switch (Coordinata.Length)
					{
						case 4: //DDMM
							DD = AIXM_COORD.Substring (0, 2);
							MM = AIXM_COORD.Substring (2, AIXM_COORD.Length - 2);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Coord = (Gradusy + (Minuty / 60)) * SIGN;
							break;
						case 6: //DDMMSS
							DD = AIXM_COORD.Substring (0, 2);
							MM = AIXM_COORD.Substring (2, 2);
							SS = AIXM_COORD.Substring (4, AIXM_COORD.Length - 4);
							Gradusy = Convert.ToDouble (DD, nfi);
							Minuty = Convert.ToDouble (MM, nfi);
							Sekundy = Convert.ToDouble (SS, nfi);
							Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
							break;
					}
				}

			}
			catch (Exception ex)
			{
				Coord = 0;
				throw ex;
			}
			return Coord;
		}





        public static double ConvertValueToMeter(string AIXM_VALUE, string AIXM_UOM)
        {
            double Mvalue = 0;
            double V = 0;

            AIXM_UOM = AIXM_UOM.ToUpper();
            NumberFormatInfo nfi = new NumberFormatInfo();

            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = " ";
            nfi.PositiveSign = "+";

            try
            {
                if (AIXM_VALUE != "") V = Convert.ToDouble(AIXM_VALUE);
                else V = 0;
                switch (AIXM_UOM)
                {
                    case "M":
                        Mvalue = V * 1;
                        break;
                    case "KM":
                        Mvalue = V * 1000;
                        break;
                    case "FT":
                        Mvalue = V * 0.3048;
                        break;
                    case "NM":
                        Mvalue = V * 1852;
                        break;
                    default:
                        Mvalue = 0;
                        break;
                }
            }
            catch (Exception)
            {
            }

            return Mvalue;
        }

        public static bool IsNumeric(string anyString)
        {
            if (anyString == null) {
                anyString = "";
            }
            if (anyString.Length > 0) {
                double dummyOut;
                return Double.TryParse(anyString, NumberStyles.Any,
                    CultureInfo.InvariantCulture.NumberFormat, out dummyOut);
            }
            else {
                return false;
            }
        }
    }

    public class ConvertedObj
    {
        public ConvertedObj()
        {
            this.mid = Guid.NewGuid().ToString();
        }

		public ConvertedObj (string mid, string RelatedMid, Aran.Aim.Features.Feature OBJ)
        {
            this.mid = mid;
            this.RelatedMid = RelatedMid;
            this.Obj = OBJ;
        }

		public string mid { get; set; }

		public string RelatedMid { get; set; }

		public Aran.Aim.Features.Feature Obj { get; set; }

		public System.Object Tag { get; set; }

		public CRCInfo CRCInfo { get; set; }

    }

	public class CRCInfo
	{
		public CRCInfo ()
		{
		}

		public CRCInfo (string lat, string lon)
		{
			Latitude = lat;
			Longitude = lon;
		}

		public string Name { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string Height { get; set; }
		public string Geoid { get; set; }
		public string SourceCRC { get; set; }
		public string NewCRC { get; set; }

		public void CalcNewCRC ()
		{
			var str = string.Empty;

			if (Latitude != null)
				str += Latitude;
			
			if (Longitude != null)
				str +=Longitude;
			
			if (Height != null)
				str += Height;

			if (Geoid != null)
				str += Geoid;

			NewCRC = Aran.PANDA.Common.CRC32.CalcCRC32 (str);
		}

		public bool IsCRCOK
		{
			get { return (SourceCRC == NewCRC); }
		}
	}
}
