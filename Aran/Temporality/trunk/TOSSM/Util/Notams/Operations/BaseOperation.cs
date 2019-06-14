using System;
using System.Globalization;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using FluentNHibernate.Conventions;
using NHibernate.Util;
using TOSSM.ViewModel.Document.Editor;
using TOSSM.ViewModel.Document.Single.Editable;

namespace TOSSM.Util.Notams.Operations
{
    abstract class BaseOperation: INotamOperation
    {
        protected void Init(NotamFeatureEditorViewModel notam)
        {
            var cfg = DataProvider.PublicationConfiguration;
            if (cfg?.FeatureConfigurations != null && cfg.FeatureConfigurations.ContainsKey((int)notam.FeatureType))
            {
                notam.Configuration = cfg.FeatureConfigurations[(int)notam.FeatureType];
            }
        }

        public abstract NotamFeatureEditorViewModel GetViewModel(Notam notam, int workpackage = 0);
        public abstract void Prepare(NotamFeatureEditorViewModel notamViewModel);
        public abstract void Save(NotamFeatureEditorViewModel notam);
        public abstract FeatureType GetFeatureType(Notam notam, int workpackage = 0);


        protected  void FixValue(EditableSinglePropertyModel activationProperty)
        {
            activationProperty.IsDelta = true;
            activationProperty.UpdateStringValue();
            activationProperty.OnApply();
        }

        protected void GetLimits(string limitString, out ValDistanceVertical limit,
            out CodeVerticalReference? limitReference)
        {

            if (limitString.Equals("GND"))
            {
                limit = new ValDistanceVertical(0, UomDistanceVertical.FT);
                limitReference = CodeVerticalReference.SFC;
            }
            else if (limitString.Equals("SFC"))
            {
                limit = new ValDistanceVertical(0, UomDistanceVertical.FT);
                limitReference = CodeVerticalReference.SFC;
            }
            else if (limitString.Equals("UNL"))
            {
                limit = new ValDistanceVertical(999, UomDistanceVertical.FT);
                limitReference = CodeVerticalReference.STD;
            }
            else if (limitString.Contains("AMSL"))
            {
                var split = limitString.Split(' ');
                var valString = split[0].TrimEnd('M').TrimEnd('T').TrimEnd('F');
                var uomString = split[0].Substring(valString.Length);
                limit = new ValDistanceVertical(Double.Parse(valString),
                    (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), uomString));
                limitReference = CodeVerticalReference.MSL;
            }
            else if (limitString.Contains("AGL"))
            {
                var split = limitString.Split(' ');
                var valString = split[0].TrimEnd('M').TrimEnd('T').TrimEnd('F');
                var uomString = split[0].Substring(valString.Length);
                limit = new ValDistanceVertical(Double.Parse(valString),
                    (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), uomString));
                limitReference = CodeVerticalReference.SFC;
            }
            else
            {
                limit = new ValDistanceVertical(Double.Parse(limitString.Substring(2)), UomDistanceVertical.FL);
                limitReference = CodeVerticalReference.STD;
            }

        }

        protected object GetProperty(NotamFeatureEditorViewModel notamViewModel, string propertyName)
        {
            return notamViewModel.PropertyList.Where(t => t.PropertyName == propertyName).FirstOrNull();
        }

        protected double GetLongitudeFromString(string stringCoord)
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

                STORONA_SVETA = stringCoord.Substring(stringCoord.Length - 1, 1);
                if (STORONA_SVETA == "W") SIGN = -1;


                if (IsNumeric(STORONA_SVETA))
                {
                    STORONA_SVETA = stringCoord.Substring(0, 1);
                    if (STORONA_SVETA == "S") SIGN = -1;
                    stringCoord = stringCoord.Substring(1, stringCoord.Length - 1);
                }
                else
                    stringCoord = stringCoord.Substring(0, stringCoord.Length - 1);
                //AIXM_COORD = AIXM_COORD.Substring(0, AIXM_COORD.Length - 1);

                int SepPos = stringCoord.LastIndexOf(".");

                if (SepPos > 0) //DDDMMSS.ss...X, DDDMM.mm...X, and DDD.dd...X
                {
                    Coordinata = stringCoord.Substring(0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 3:  //DDD.dd...
                            Coord = Convert.ToDouble(stringCoord, nfi) * SIGN;
                            break;

                        case 5:  //DDDMM.mm... 
                            DD = stringCoord.Substring(0, 3);
                            MM = stringCoord.Substring(3, stringCoord.Length - 3);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 7:  //DDDMMSS.ss... 
                            DD = stringCoord.Substring(0, 3);
                            MM = stringCoord.Substring(3, 2);
                            SS = stringCoord.Substring(5, stringCoord.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }
                else //DDDMMSSX and DDDMMX
                {
                    Coordinata = stringCoord;
                    switch (Coordinata.Length)
                    {
                        case 5:  //DDDMM 
                            DD = stringCoord.Substring(0, 3);
                            MM = stringCoord.Substring(3, stringCoord.Length - 3);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 7:  //DDDMMSS
                            DD = stringCoord.Substring(0, 3);
                            MM = stringCoord.Substring(3, 2);
                            SS = stringCoord.Substring(5, stringCoord.Length - 5);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                        case 9:  //DDDMMSS.SS
                            DD = stringCoord.Substring(0, 3);
                            MM = stringCoord.Substring(3, 2);
                            SS = stringCoord.Substring(5, stringCoord.Length - 5);
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

        protected  double GetLatitudeFromString(string coord)
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

                STORONA_SVETA = coord.Substring(coord.Length - 1, 1);
                if (STORONA_SVETA == "S") SIGN = -1;



                if (IsNumeric(STORONA_SVETA))
                {
                    STORONA_SVETA = coord.Substring(0, 1);
                    if (STORONA_SVETA == "S") SIGN = -1;
                    coord = coord.Substring(1, coord.Length - 1);
                }
                else
                    coord = coord.Substring(0, coord.Length - 1);



                int SepPos = coord.LastIndexOf(".");

                if (SepPos > 0) //DDMMSS.ss...X, DDMM.mm...X, and DD.dd...X
                {
                    Coordinata = coord.Substring(0, SepPos);
                    switch (Coordinata.Length)
                    {
                        case 2:  //DD.dd...
                            Coord = Convert.ToDouble(coord, nfi) * SIGN;
                            break;

                        case 4:  //DDMM.mm... 
                            DD = coord.Substring(0, 2);
                            MM = coord.Substring(2, coord.Length - 2);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 6:  //DDMMSS.ss... 
                            DD = coord.Substring(0, 2);
                            MM = coord.Substring(2, 2);
                            SS = coord.Substring(4, coord.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                    }
                }
                else //DDMMSSX and DDMMX
                {
                    Coordinata = coord;
                    switch (Coordinata.Length)
                    {
                        case 4:  //DDMM 
                            DD = coord.Substring(0, 2);
                            MM = coord.Substring(2, coord.Length - 2);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Coord = (Gradusy + (Minuty / 60)) * SIGN;
                            break;

                        case 6:  //DDMMSS
                            DD = coord.Substring(0, 2);
                            MM = coord.Substring(2, 2);
                            SS = coord.Substring(4, coord.Length - 4);
                            Gradusy = Convert.ToDouble(DD, nfi);
                            Minuty = Convert.ToDouble(MM, nfi);
                            Sekundy = Convert.ToDouble(SS, nfi);
                            Coord = (Gradusy + (Minuty / 60) + (Sekundy / 3600)) * SIGN;
                            break;

                        case 8:  //DDMMSS.ss
                            DD = coord.Substring(0, 2);
                            MM = coord.Substring(2, 2);
                            SS = coord.Substring(4, coord.Length - 4);
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

        protected  bool IsNumeric(string anyString)
        {
            if (anyString == null)
            {
                anyString = "";
            }
            if (anyString.Length > 0)
            {
                double dummyOut;
                return Double.TryParse(anyString, NumberStyles.Any,
                    CultureInfo.InvariantCulture.NumberFormat, out dummyOut);
            }
            else
            {
                return false;
            }
        }
    }


}
