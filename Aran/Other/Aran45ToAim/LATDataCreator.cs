using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim;
using Aran.Aim.Objects;
using System.Globalization;

namespace Aran45ToAixm
{
    public class LATDataCreator
    {
        public static IEnumerable<Feature> CreateSampleList ()
        {
            Feature [] featArr = null;

            Feature org = LATDataCreator.CreateOrganisation ();
            yield return org;

            Feature ah = LATDataCreator.CreateAirportHeliport (org.Identifier);
            yield return ah;

            Feature rwy = LATDataCreator.CreateRunway (ah.Identifier);
            yield return rwy;

            Feature [] rwyDirArr = LATDataCreator.CreateRunwayDirectoins (rwy.Identifier);
            for (int i = 0; i < rwyDirArr.Length; i++)
            {
                yield return rwyDirArr [i];

                featArr = LATDataCreator.CreateNavaid (rwyDirArr [i]);
                foreach (var feat in featArr)
                {
                    yield return feat;
                }
            }

            Feature [] rwyCPArr = LATDataCreator.CreateRunwayCentrelinePoints (rwyDirArr [0].Identifier, rwyDirArr [1].Identifier);
            for (int i = 0; i < rwyCPArr.Length; i++)
                yield return rwyCPArr [i];

            Feature [] navArr = LATDataCreator.CreateVORDMENavaid ("RIA", 23.9652, 56.9209, 5, ah.Identifier);
            foreach (var feat in navArr)
                yield return feat;

            navArr = LATDataCreator.CreateVORDMENavaid ("TUK", 23.23997222, 56.9305833333, 7, ah.Identifier);
            foreach (var feat in navArr)
                yield return feat;

            featArr = LATDataCreator.CreateDesignatedPoints ();
            foreach (var feat in featArr)
                yield return feat;
        }

        public static OrganisationAuthority CreateOrganisation ()
        {
            OrganisationAuthority org = new OrganisationAuthority ();
            Fill (org);
            org.Identifier = new Guid ("39db4c6c-32c5-4770-b2d2-7e97af0d47cf");
            org.Name = "LATVIA";
            org.Designator = "EV";
            return org;
        }

        public static AirportHeliport CreateAirportHeliport (Guid orgIdentifier)
        {
            AirportHeliport ah = new AirportHeliport ();
            Fill (ah);
            ah.Identifier = new Guid ("e1424c7e-58a0-48dc-b657-25474f361d17");
            ah.Designator = "EVRA";
            ah.FieldElevation = new ValDistanceVertical (36, UomDistanceVertical.FT);
            ah.MagneticVariation = 7;

            ah.ARP = new ElevatedPoint ();
            ah.ARP.Elevation = ah.FieldElevation;
            ah.ARP.Geo.SetCoords (23.9711, 56.9239, 10.97);


            ah.ServedCity.Add (new City ());
            ah.ServedCity [0].Name = "RIGA";

            ah.ResponsibleOrganisation = new AirportHeliportResponsibilityOrganisation ();
            ah.ResponsibleOrganisation.Role = CodeAuthorityRole.OWN;
            ah.ResponsibleOrganisation.TheOrganisationAuthority = new FeatureRef (orgIdentifier);

            return ah;
        }

        public static Runway CreateRunway (Guid ahIdentifier)
        {
            Runway runway = new Runway ();
            Fill (runway);
            runway.Designator = "18/36";
            runway.AssociatedAirportHeliport = new FeatureRef (ahIdentifier);

            runway.NominalLength = new ValDistance (2, UomDistance.KM);

            return runway;
        }

        public static Feature [] CreateRunwayDirectoins (Guid rwyIdentifier)
        {
            RunwayDirection rwyDir18 = new RunwayDirection ();
            Fill (rwyDir18);
            rwyDir18.Designator = "18";
            rwyDir18.TrueBearing = 185.18;
            rwyDir18.UsedRunway = new FeatureRef (rwyIdentifier);

            RunwayDirection rwyDir36 = new RunwayDirection ();
            Fill (rwyDir36);
            rwyDir36.Designator = "36";
            rwyDir36.TrueBearing = 5.17;
            rwyDir36.UsedRunway = new FeatureRef (rwyIdentifier);

            return new Feature [] { rwyDir18, rwyDir36 };
        }

        public static Feature [] CreateRunwayCentrelinePoints (Guid rwyDir18Identifier, Guid rwyDir36Identifier)
        {
            List<Feature> featList = new List<Feature> ();


            featList.Add (CreateRCP (
                CodeRunwayPointRole.START, 23.9731, 56.9351, 31, rwyDir18Identifier));

            featList.Add (CreateRCP (
                CodeRunwayPointRole.THR, 23.9731, 56.9351, 31, rwyDir18Identifier));

            featList.Add (CreateRCP (
                CodeRunwayPointRole.MID, 23.9712, 56.9237, 36, rwyDir18Identifier));

            featList.Add (CreateRCP (
                CodeRunwayPointRole.END, 23.9684, 56.9064, 36, rwyDir18Identifier));



            featList.Add (CreateRCP (
                CodeRunwayPointRole.END, 23.9731, 56.9351, 31, rwyDir36Identifier));

            featList.Add (CreateRCP (
                CodeRunwayPointRole.MID, 23.9712, 56.9237, 36, rwyDir36Identifier));

            featList.Add (CreateRCP (
                CodeRunwayPointRole.THR, 23.9684, 56.9064, 36, rwyDir36Identifier));

            featList.Add (CreateRCP (
                CodeRunwayPointRole.START, 23.9684, 56.9064, 36, rwyDir36Identifier));

            return featList.ToArray ();
        }

        public static Feature [] CreateNavaid (Feature rwyDir)
        {
            Navaid nav = new Navaid ();
            Fill (nav);
            nav.Type = CodeNavaidService.ILS;

            Feature loc = null;
            Feature gld = null;

            if ((rwyDir as RunwayDirection).Designator == "18")
            {
                nav.Designator = "IRV";
                loc = CreateLocalizerIRV ();
                gld = CreateGlidepathIRV ();
            }
            else
            {
                nav.Designator = "IRP";
                loc = CreateLocalizerIRP ();
                gld = CreateGlidepathIRP ();
            }

            int n = 0;
            nav.NavaidEquipment.Add (new NavaidComponent ());
            nav.NavaidEquipment [n].TheNavaidEquipment = new AbstractNavaidEquipmentRef ();
            nav.NavaidEquipment [n].TheNavaidEquipment.Type = NavaidEquipmentType.Localizer;
            nav.NavaidEquipment [n].TheNavaidEquipment.Identifier = loc.Identifier;
            n = 1;
            nav.NavaidEquipment.Add (new NavaidComponent ());
            nav.NavaidEquipment [n].TheNavaidEquipment = new AbstractNavaidEquipmentRef ();
            nav.NavaidEquipment [n].TheNavaidEquipment.Type = NavaidEquipmentType.Glidepath;
            nav.NavaidEquipment [n].TheNavaidEquipment.Identifier = gld.Identifier;

            nav.RunwayDirection.Add (new FeatureRefObject ());
            nav.RunwayDirection [0].Feature = new FeatureRef (rwyDir.Identifier);

            return new Feature [] { nav, loc, gld };
        }

        public static Feature [] CreateVORDMENavaid (string name, double x, double y, double magVar, Guid airportIdentifier)
        {
            Navaid nav = new Navaid ();
            Fill (nav);

            nav.Type = CodeNavaidService.VOR_DME;
            nav.Designator = name;
            nav.Location = new ElevatedPoint ();
            nav.Location.Geo.SetCoords (x, y);

            NavaidEquipment vor = CreateEquipment (true, name, x, y, magVar);
            NavaidEquipment dme = CreateEquipment (false, name, x, y, magVar);

            int n = 0;
            nav.NavaidEquipment.Add (new NavaidComponent ());
            nav.NavaidEquipment [n].TheNavaidEquipment = new AbstractNavaidEquipmentRef ();
            nav.NavaidEquipment [n].TheNavaidEquipment.Type = NavaidEquipmentType.VOR;
            nav.NavaidEquipment [n].TheNavaidEquipment.Identifier = vor.Identifier;
            n = 1;
            nav.NavaidEquipment.Add (new NavaidComponent ());
            nav.NavaidEquipment [n].TheNavaidEquipment = new AbstractNavaidEquipmentRef ();
            nav.NavaidEquipment [n].TheNavaidEquipment.Type = NavaidEquipmentType.DME;
            nav.NavaidEquipment [n].TheNavaidEquipment.Identifier = dme.Identifier;

            nav.ServedAirport.Add (new FeatureRefObject ());
            nav.ServedAirport [0].Feature = new FeatureRef (airportIdentifier);

            return new Feature [] { nav, vor, dme };
        }

        public static Feature [] CreateDesignatedPoints ()
        {
            List<Feature> list = new List<Feature> ();

            list.Add (CreateDesignatedPoint ("GUDIN", "57°08'38.9\"N", "023°52'24.0\"E"));
            list.Add (CreateDesignatedPoint ("LURIG", "57°12'37.6\"N", "024°18'40.4\"E"));
            list.Add (CreateDesignatedPoint ("LUSOK", "57°13'15.3\"N", "023°57'00.1\"E"));
            list.Add (CreateDesignatedPoint ("REKBI", "56°39'16.7\"N", "023°57'55.0\"E"));

            list.Add (CreateDesignatedPoint ("ELMIX", "57°06'19\"N", "023°23'08\"E"));
            list.Add (CreateDesignatedPoint ("LAPSA", "57°20'13\"N", "022°38'13\"E"));
            list.Add (CreateDesignatedPoint ("TENSI", "57°03'47\"N", "022°18'20\"E"));
            list.Add (CreateDesignatedPoint ("ORVIX", "56°54'09\"N", "022°22'45\"E"));
            list.Add (CreateDesignatedPoint ("ASKOR", "56°22'58\"N", "022°36'46\"E"));
            list.Add (CreateDesignatedPoint ("SOKVA", "57°54'00\"N", "024°17'58\"E"));
            list.Add (CreateDesignatedPoint ("NEMIR", "57°34'14\"N", "023°55'55\"E"));
            list.Add (CreateDesignatedPoint ("IRMAN", "57°20'12\"N", "023°56'39\"E"));
            list.Add (CreateDesignatedPoint ("VANAG", "57°33'01\"N", "024°43'33\"E"));

            return list.ToArray ();
        }


        private static Feature CreateDesignatedPoint (string name, string lat, string lon)
        {
            DesignatedPoint dp = new DesignatedPoint ();
            Fill (dp);

            dp.Designator = name;
            dp.Location = new AixmPoint ();
            FillPoint (dp.Location, lat, lon);
            dp.Type = CodeDesignatedPoint.COORD;

            return dp;
        }

        private static NavaidEquipment CreateEquipment (bool isVor, string name, double x, double y, double magVar)
        {
            NavaidEquipment ne;
            if (isVor)
                ne = new VOR ();
            else
                ne = new DME ();

            Fill (ne);
            ne.Designator = name;
            ne.Location = new ElevatedPoint ();
            ne.Location.Geo.SetCoords (x, y);
            ne.MagneticVariation = magVar;

            return ne;
        }

        private static Localizer CreateLocalizerIRV ()
        {
            Localizer llz = new Localizer ();
            Fill (llz);

            llz.Designator = "IRV";
            llz.Frequency = new ValFrequency (111.1, UomFrequency.HZ);
            llz.TrueBearing = 185.18;
            llz.WidthCourse = 3.16;
            llz.Location = new ElevatedPoint ();
            llz.Location.Geo.SetCoords (23.9675, 56.9012);

            return llz;
        }

        private static Glidepath CreateGlidepathIRV ()
        {
            Glidepath gld = new Glidepath ();
            Fill (gld);
            gld.Designator = "IRV";
            gld.Frequency = new ValFrequency (331.7, UomFrequency.HZ);
            gld.Slope = 3;
            gld.Rdh = new ValDistanceVertical (55, UomDistanceVertical.FT);
            gld.Location = new ElevatedPoint ();
            gld.Location.Geo.SetCoords (23.9706, 56.9323);

            return gld;
        }

        private static Localizer CreateLocalizerIRP ()
        {
            Localizer llz = new Localizer ();
            Fill (llz);

            llz.Designator = "IRP";
            llz.Frequency = new ValFrequency (108.1, UomFrequency.HZ);
            llz.TrueBearing = 5.17;
            llz.WidthCourse = 3.16;
            llz.Location = new ElevatedPoint ();
            llz.Location.Geo.SetCoords (23.9739, 56.9402);

            return llz;
        }

        private static Glidepath CreateGlidepathIRP ()
        {
            Glidepath gld = new Glidepath ();
            Fill (gld);
            gld.Designator = "IRP";
            gld.Frequency = new ValFrequency (334.7, UomFrequency.HZ);
            gld.Slope = 3;
            gld.Rdh = new ValDistanceVertical (55, UomDistanceVertical.FT);
            gld.Location = new ElevatedPoint ();
            gld.Location.Geo.SetCoords (23.9668, 56.9092);

            return gld;
        }

        private static RunwayCentrelinePoint CreateRCP (CodeRunwayPointRole role, double x, double y, double elev, Guid rwyDirIden)
        {
            RunwayCentrelinePoint rcpTHR18 = new RunwayCentrelinePoint ();
            Fill (rcpTHR18);
            rcpTHR18.Role = role;
            rcpTHR18.Location = new ElevatedPoint ();
            rcpTHR18.Location.Elevation = new ValDistanceVertical (elev, UomDistanceVertical.FT);
            rcpTHR18.Location.Geo.SetCoords (x, y, (elev * 0.3048));
            rcpTHR18.OnRunway = new FeatureRef (rwyDirIden);
            return rcpTHR18;
        }

        private static void Fill (Feature feature)
        {
            feature.Identifier = Guid.NewGuid ();

            feature.TimeSlice = new TimeSlice ();
            feature.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
            feature.TimeSlice.SequenceNumber = 1;
            feature.TimeSlice.CorrectionNumber = 0;
            feature.TimeSlice.ValidTime = new TimePeriod (DateTime.Now);
            feature.TimeSlice.FeatureLifetime = new TimePeriod (feature.TimeSlice.ValidTime.BeginPosition);
        }


        private static void FillPoint (AixmPoint aixmPoint, string lat, string lon)
        {
            lat = lat.Replace ("°", "").Replace ("\'", "").Replace ("\"", "");
            lon = lon.Replace ("°", "").Replace ("\'", "").Replace ("\"", "");

            aixmPoint.Geo.X = GetLONGITUDEFromAIXMString (lon);
            aixmPoint.Geo.Y = GetLATITUDEFromAIXMString (lat);
        }

        private static double GetLONGITUDEFromAIXMString (string AIXM_COORD)
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

                if (STORONA_SVETA == "W") SIGN = -1;

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

        private static double GetLATITUDEFromAIXMString (string AIXM_COORD)
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
                if (STORONA_SVETA == "S") SIGN = -1;
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
    }
}
