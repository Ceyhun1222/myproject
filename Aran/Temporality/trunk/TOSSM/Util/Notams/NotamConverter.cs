using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Aran.Temporality.Common.Entity;

namespace TOSSM.Util.Notams
{
    public class NotamConverter
    {
        private static readonly Dictionary<char, string> Traffics = new Dictionary<char, string>()
        {
            {'I', "IVR"},
            {'V', "VFR"},
            {'K', "NOTAM is a checklist"}
        };

        private static readonly Dictionary<char, string> Scopes = new Dictionary<char, string>()
        {
            {'A', "Aerodrome"},
            {'E', "En-Route"},
            {'W', "Nav warnings"},
            {'K', "NOTAM is a checklist"}
        };



        private static readonly Dictionary<string, string> Categories = new Dictionary<string, string>()
        {
            {"A", "Airspace Organization"},
            {"C", "Communications and Radar"},
            {"F", "Facilities and Services"},
            {"G", "Military"},
            {"I", "Instrument and Microwave Landing"},
            {"L", "Lighting"},
            {"M", "Movement and Landing Area"},
            {"N", "Terminal and En - route Navigation"},
            {"O", "Other Information"},
            {"P", "Air Traffic Procedures"},
            {"R", "Airspace Restrictions"},
            {"S", "Air Traffic and VOLMET Services"},
            {"T", "Hazard"},
            {"W", "Warnings"},
            {"X", "Other"}
        };

        private static readonly Dictionary<string, string> Subjects = new Dictionary<string, string>()
        {
            {"AA", "MINIMUM ALTITUDE"},
            {"AC", "CLASS B, C, D OR E SURFACE AREA (ICAO-CONTROL ZONE)"},
            {"AD", "ADIZ"},
            {"AE", "CONTROL AREA"},
            {"AF", "FLIGHT INFORMATION REGION (FIR)"},
            {"AG", "GENERAL FACILITY"},
            {"AH", "UPPER CONTROL AREA"},
            {"AL", "MINIMUM USABLE FLIGHT LEVEL"},
            {"AN", "AIR NAVIGATION ROUTE"},
            {"AO", "OCEANIC CONTROL ZONE (OCA)"},
            {"AP", "REPORTING POINT"},
            {"AR", "ATS ROUTE"},
            {"AT", "TERMINAL CONTROL AREA (TMA)"},
            {"AU", "UPPER FLIGHT INFORMATION REGION"},
            {"AV", "UPPER ADVISORY AREA"},
            {"AX", "INTERSECTION"},
            {"AZ", "AERODROME TRAFFIC ZONE (ATZ)"},
            {"CA", "AIR/GROUND FACILITY"},
            {"CB", "AUTOMATIC DEPENDENT SURVEILLANCE - BROADCAST"},
            {"CC", "AUTOMATIC DEPENDENT SURVEILLANCE - CONTRACT"},
            {"CD", "CONTROLLER-PILOT DATA LINK COMMUNICATIONS"},
            {"CE", "ENROUTE SURVELLENCE RADAR"},
            {"CG", "GROUND CONTROLLED APPROACH SYSTEM (GCA)"},
            {"CL", "SELECTIVE CALLING SYSTEM"},
            {"CM", "SURFACE MOVEMENT RADAR"},
            {"CP", "PAR"},
            {"CR", "SURVEILLANCE RADAR ELEMENT OF PAR SYSTEM"},
            {"CS", "SECONDARY SURVEILLANCE RADAR"},
            {"CT", "TERMINAL AREA SURVEILLANCE RADAR"},
            {"GJ", "ASR"},
            {"GK", "PRECISION APPROACH LANDING SYSTEM"},
            {"FA", "AERODROME"},
            {"FB", "BRAKING ACTION MEASUREMENT EQUIPMENT"},
            {"FC", "CEILING MEASUREMENT EQUIPMENT"},
            {"FD", "DOCKING SYSTEM"},
            {"FE", "OXYGEN"},
            {"FF", "FIRE FIGHTING AND RESCUE"},
            {"FG", "GROUND MOVEMENT CONTROL"},
            {"FH", "HELICOPTER ALIGHTING AREA/PLATFORM"},
            {"FI", "AIRCRAFT DE-ICING"},
            {"FJ", "OILS"},
            {"FL", "LANDING DIRECTION INDICATOR"},
            {"FM", "METEOROLOGICAL SERVICE"},
            {"FO", "FOG DISPERSAL SYSTEM"},
            {"FP", "HELIPORT"},
            {"FS", "SNOW REMOVAL EQUIPMENT"},
            {"FT", "TRANSMISSOMETER"},
            {"FU", "FUEL AVAILABILITY"},
            {"FW", "WIND DIRECTION INDICATOR"},
            {"FZ", "CUSTOMS"},
            {"GC", "TRANSIENT MAINTENANCE"},
            {"GD", "STARTER UNIT"},
            {"GE", "SOAP"},
            {"GF", "DEMINERALIZED WATER"},
            {"GG", "OXYGEN"},
            {"GH", "OIL"},
            {"GI", "DRAG CHUTES"},
            {"GL", "FACSFAC"},
            {"GS", "NITROGEN"},
            {"GU", "DE-ICE"},
            {"GZ", "BASE OPERATIONS"},
            {"GB", "OPTICAL LANDING SYSTEM"},
            {"GM", "FIRING RANGE"},
            {"GN", "NIGHT VISION GOGGLE (NVG) OPERATIONS"},
            {"GO", "WARNING AREA"},
            {"GP", "ARRESTING GEAR MARKERS (AGM)"},
            {"GQ", "PULSATING/STEADY VISUAL APPROACH SLOPE INDICATOR"},
            {"GR", "DIVERSE DEPARTURE"},
            {"GT", "IFR TAKE-OFF MINIMUMS AND DEPARTURE PROCEDURES"},
            {"GV", "CLEAR ZONE"},
            {"GX", "RUNWAY DISTANCE MARKERS (RDM)"},
            {"GY", "HELO PAD"},
            {"IC", "ILS"},
            {"ID", "DME ASSOCIATED WITH ILS"},
            {"IG", "GLIDE PATH (ILS)"},
            {"II", "INNER MARKER (ILS)"},
            {"IL", "LOCALIZER (ILS)"},
            {"IM", "MIDDLE MARKER (ILS)"},
            {"IN", "LOCALIZER"},
            {"IO", "OUTER MARKER (ILS)"},
            {"IS", "ILS CATEGORY I"},
            {"IT", "ILS CATEGORY II"},
            {"IU", "ILS CATEGORY III"},
            {"IW", "MLS"},
            {"IX", "LOCATOR, OUTER, (ILS)"},
            {"IY", "LOCATOR, MIDDLE (ILS)"},
            {"LA", "APPROACH LIGHTING SYSTEM"},
            {"LB", "AERODROME BEACON"},
            {"LC", "RUNWAY CENTERLINE LIGHTS"},
            {"LD", "LANDING DIRECTION INDICATOR LIGHTS"},
            {"LE", "RUNWAY EDGE LIGHTS"},
            {"LF", "SEQUENCED FLASHING LIGHTS"},
            {"LG", "PILOT-CONTROLLED LIGHTING"},
            {"LH", "HIGH INTENSITY RUNWAY LIGHTS"},
            {"LI", "RUNWAY END IDENTIFIER LIGHTS"},
            {"LJ", "RUNWAY ALIGNMENT INDICATOR LIGHTS"},
            {"LK", "CATEGORY II COMPONENTS OF APPROACH LIGHTING SYSTEM"},
            {"LL", "LOW INTENSITY RUNWAY LIGHTS"},
            {"LM", "MEDIUM INTENSITY RUNWAY LIGHTS"},
            {"LP", "PRECISION APPROACH PATH INDICATOR"},
            {"LR", "ALL LANDING AREA LIGHTING FACILITIES"},
            {"LS", "STOPWAY LIGHTS"},
            {"LT", "THRESHOLD LIGHTS"},
            {"LU", "HELICOPTER APPROACH PATH INDICATOR"},
            {"LV", "VISUAL APRROACH SLOPE INDICATOR"},
            {"LW", "HELIPORT LIGHTING"},
            {"LX", "TAXIWAY CENTER LINE LIGHTS"},
            {"LY", "TAXIWAY EDGE LIGHTS"},
            {"LZ", "RUNWAY TOUCH DOWN ZONE LIGHTS"},
            {"MA", "MOVEMENT AREA"},
            {"MB", "BEARING STRENGTH"},
            {"MC", "CLEARWAY"},
            {"MD", "DECLARED DISTANCES"},
            {"MG", "TAXIING GUIDANCE SYSTEM"},
            {"MH", "RUNWAY ARRESTING GEAR"},
            {"MK", "PARKING AREA"},
            {"MM", "DAYLIGHT MARKINGS"},
            {"MN", "APRON"},
            {"MO", "STOP BAR"},
            {"MP", "AIRCRAFT STANDS"},
            {"MR", "RUNWAY"},
            {"MS", "STOPWAY"},
            {"MT", "THRESHOLD"},
            {"MU", "RUNWAY TURNING BAY"},
            {"MW", "STRIP"},
            {"MX", "TAXIWAY"},
            {"MY", "RAPID EXIT TAXIWAY"},
            {"NA", "ALL RADIO NAVIGATION FACILITIES"},
            {"NB", "NDB"},
            {"NC", "DECCA"},
            {"ND", "DME"},
            {"NF", "FAN MARKER"},
            {"NL", "LOCATOR"},
            {"NM", "VOR/DME"},
            {"NN", "TACAN"},
            {"NT", "VORTAC"},
            {"NV", "VOR"},
            {"NX", "DIRECTION FINDING STATION"},
            {"OA", "AERONAUTICAL INFORMATION SERVICE"},
            {"OB", "OBSTACLE"},
            {"OE", "AIRCRAFT ENTRY REQUIREMENTS"},
            {"OL", "OBSTACLE LIGHTS"},
            {"OR", "RESCUE COORDINATION CENTER"},
            {"PA", "STANDARD INSTRUMENT ARRIVAL (STAR)"},
            {"PB", "STANDARD VFR ARRIVAL"},
            {"PC", "CONTINGENCY PROCEDURES"},
            {"PD", "STANDARD INSTRUMENT DEPARTURE (SID)"},
            {"PE", "STANDARD VFR DEPARTURE"},
            {"PF", "FLOW CONTROL PROCEDURES"},
            {"PH", "HOLDING PROCEDURES"},
            {"PI", "INSTRUMENT APPROACH PROCEDURE"},
            {"PK", "VFR APPROACH PROCEDURE"},
            {"PL", "OBSTACLE CLEARANCE LIMIT"},
            {"PM", "AERODROME OPERATING MINIMA"},
            {"PN", "NOISE OPERATING RESTRICTIONS"},
            {"PO", "OBSTACLE CLEARANCE ALTITUDE"},
            {"PP", "OBSTACLE CLEARANCE HEIGHT"},
            {"PR", "RADIO FAILURE PROCEDURE"},
            {"PT", "TRANSITION ALTITUDE"},
            {"PU", "MISSED APPROACH PROCEDURE"},
            {"PX", "MINIMUM HOLDING ALTITUDE"},
            {"PZ", "ADIZ PROCEDURE"},
            {"RA", "AIRSPACE RESERVATION"},
            {"RD", "DANGER AREA"},
            {"RO", "OVERFLYING OF"},
            {"RP", "PROHIBITED AREA"},
            {"RR", "RESTRICTED AREA"},
            {"RT", "TEMPORARY RESTRICTED AREA"},
            {"RM", "MILITARY OPERATING AREA" },
            {"SA", "AUTOMATIC TERMINAL INFORMATION SERVICE (ATIS)"},
            {"SB", "ATS REPORT OFFICE"},
            {"SC", "AREA CONTROL CENTER"},
            {"SE", "FLIGHT INFORMATION SERVICE"},
            {"SF", "AERODROME FLIGHT INFORMATION SERVICE (AFIS)"},
            {"SL", "FLOW CONTROL CENTER"},
            {"SO", "OCEANIC AREA CONTROL CENTER"},
            {"SP", "APPROACH CONTROL"},
            {"SS", "FLIGHT SERVICE STATION"},
            {"ST", "AERODROME CONTROL TOWER"},
            {"SU", "UPPER AREA CONTROL CENTER"},
            {"SV", "VOLMENT BROADCAST"},
            {"SY", "UPPER ADVISORY SERVICE"},
            {"TT", "MIJI"},
            {"WA", "AIR DISPLAY"},
            {"WB", "AEROBATICS"},
            {"WC", "CAPTIVE BALLOON OR KITE"},
            {"WD", "DEMOLITION OF EXPLOSIVES"},
            {"WE", "EXERCISES"},
            {"WF", "AIR REFUELING"},
            {"WG", "GLIDER FLYING"},
            {"WH", "BLASTING"},
            {"WJ", "BANNER/TARGET TOWING"},
            {"WL", "ASCENT OF FREE BALLOON"},
            {"WM", "MISSLE, GUN OR ROCKET FIRING"},
            {"WP", "PARACHUTE JUMPING EXERCISE"},
            {"WR", "RADIOACTIVE MATERIALS OR TOXIC CHEMICALS"},
            {"WS", "BURNING OR BLOWING GAS"},
            {"WT", "MASS MOVEMENT OF ACFT"},
            {"WU", "UNMANNED AIRCRAFT"},
            {"WV", "FORMATION FLT"},
            {"WW", "SIGNIFICANT VOLCANIC ACTIVITY"},
            {"WY", "AERIAL SURVEY"},
            {"WZ", "MODEL FLYING"}
        };

        private static readonly Dictionary<string, string> Statues = new Dictionary<string, string>()
        {
            {"CH", "CHANGED"},
            {"CS", "INSTALLED"},
            {"TT", "HAZARD"},
            {"XX", ""},
            {"AH", "HOURS OF SERVICE ARE"},
            {"AM", "MILITARY OPERATIONS ONLY"},
            {"AW", "COMPLETELY WITHDRAWN"},
            {"CA", "ACTIVATED"},
            {"CD", "DEACTIVATED"},
            {"LB", "RESERVED FOR AIRCRAFT BASED THEREIN"},
            {"LC", "CLOSED"},
            {"LP", "PROHIBITED TO"},
            {"LV", "CLOSED TO VFR OPERATIONS"},
            {"CL", "REALIGNED"},
            {"LI", "CLOSED TO IFR OPERATIONS"},
            {"CI", "IDENTIFICATION OR RADIO CALL SIGN CHANGED TO"},
            {"HX", "CONCENTRATION OF BIRDS"},
            {"AP", "PRIOR PERMISSION REQUIRED"},
            {"AR", "AVAILABLE, PRIOR PERMISSION REQUIRED"},
            {"LT", "LIMITED TO"},
            {"CM", "DISPLACED"},
            {"CR", "TEMPORARILY REPLACED BY"},
            {"AS", "UNSERVICEABLE"},
            {"CF", "FREQUENCY CHANGED TO"},
            {"LF", "INTERFERENCE FROM"},
            {"LS", "SUBJECT TO INTERRUPTION"},
            {"AO", "OPERATIONAL"},
            {"CT", "ON TEST, DO NOT USE"},
            {"AC", "WITHDRAWN FOR MAINTENANCE"},
            {"AU", "NOT AVAILABLE"},
            {"AF", "FLIGHT CHECKED AND FOUND RELIABLE"},
            {"AK", "RESUMED NORMAL OPERATIONS"},
            {"GH", "UNUSABLE"},
            {"GV", "NOT AUTHORIZED"},
            {"AD", "AVAILABLE FOR DAYLIGHT OPERATIONS"},
            {"AG", "OPERATING BUT GROUND CHECKED ONLY, AWAITING FLIGHT CHECK"},
            {"GA", "NOT COINCIDENTAL WITH ILS/PAR"},
            {"AN", "AVAILABLE FOR NIGHT OPERATIONS"},
            {"GD", "OFFICIAL BUSINESS ONLY"},
            {"GE", "EXPECT LANDING DELAY"},
            {"GF", "EXTENSIVE SERVICE DELAY"},
            {"HG", "GRASS CUTTING IN PROGRESS"},
            {"HW", "WORK IN PROGRESS"},
            {"LH", "UNSERVICEABLE FOR AIRCRAFT HEAVIER THAN"},
            {"LN", "CLOSED TO ALL NIGHT OPERATIONS"},
            {"LR", "AIRCRAFT RESTRICTED TO RUNWAYS AND TAXIWAYS"},
            {"CG", "DOWNGRADED TO"},
            {"GN", "FREQUENCY NOT AVAILABLE"},
            {"GJ", "IN PROGRESS"},
            {"LW", "WILL TAKE PLACE"},
            {"LY", "EFFECTIVE"},
            {"LX", "OPERATING BUT CAUTION ADVISED DUE TO"},
            {"GM", "NOT ILLUMINATED"},
            {"AQ", "COMPLETELY WITHDRAWN"},
            {"LG", "OPERATING WITHOUT IDENTIFICATION"},
            {"GO", "IS WET"},
            {"HA", "BRAKING ACTION IS"},
            {"HB", "BRAKING COEFFICIENT IS"},
            {"HC", "COVERED BY COMPACTED SNOW TO A DEPTH OF"},
            {"HD", "COVERED BY DRY SNOW TO A DEPTH OF"},
            {"HE", "COVERED BY WATER TO A DEPTH OF"},
            {"HF", "TOTALLY FREE OF SNOW AND ICE"},
            {"HI", "COVERED BY ICE"},
            {"HL", "SNOW CLEARANCE COMPLETED"},
            {"HN", "COVERED BY WET SNOW OR SLUSH TO A DEPTH OF"},
            {"HO", "OBSCURED BY SNOW"},
            {"HP", "SNOW CLEARANCE IN PROGRESS"},
            {"HR", "STANDING WATER"},
            {"HS", "SANDING"},
            {"HY", "SNOW BANKS EXIST"},
            {"HZ", "COVERED BY FROZEN RUTS AND RIDGES"},
            {"LL", "USABLE FOR LENGTH OF AND WIDTH OF"},
            {"GI", "UNMONITORED"},
            {"GG", "UNUSABLE BEYOND"},
            {"CE", "ERECTED"},
            {"LD", "UNSAFE"}
        };

        public static string GetName(Notam notam)
        {
            return $"{notam.Series}{notam.Number}/{notam.Year.ToString().Substring(2)}";
        }



        public static string CategoryToHumanReadable(string subject)
        {
            if (Categories.ContainsKey(subject))
                return Categories[subject];
            return subject;
        }

        public static string SubjectToHumanReadable(string subject)
        {
            if (Subjects.ContainsKey(subject))
                return Subjects[subject];
            return subject;
        }

        public static string StatusToHumanReadable(string subject)
        {
            if (Statues.ContainsKey(subject))
                return Statues[subject];
            return subject;
        }

        public static string GetHeader(Notam notam)
        {
            return $"NOTAM {notam.Series}{notam.Number}/{notam.Year.ToString().Substring(2)} {TypeToHumanReadable((NotamType)notam.Type)}";
        }

        private static readonly Dictionary<NotamType, string> Types = new Dictionary<NotamType, string>()
        {
            {NotamType.N, "New"},
            {NotamType.C, "Cancel"},
            {NotamType.R, "Replace"},
        };

        public static string TypeToHumanReadable(NotamType type)
        {
            return Types[type];
        }


        public static string TrafficToHumanReadable(string text)
        {
            Check(text, @"[IVK]{1,3}");
            var chars = text.ToCharArray();
            string result = "";
            var traffics = chars.Select(t => Traffics[t]).ToList();
            traffics.ForEach(t => result= result + " " + t);
            return result.Trim();
        }

        public static string ScopeToHumanReadable(string text)
        {
            Check(text, @"[AEWK]{1,4}");
            var chars = text.ToCharArray();
            string result = "";
            var traffics = chars.Select(t => Scopes[t]).ToList();
            traffics.ForEach(t => result = result + " " + t);
            return result.Trim();
        }

        public static string Code23ToHumanReadable(string text)
        {
            return SubjectToHumanReadable(text);
        }

        public static string Code45ToHumanReadable(string text)
        {
            return StatusToHumanReadable(text);
        }


        private static void Check(string text, string regEx)
        {
            Regex regex = new Regex(regEx);
            var match = regex.Match(text);
            if (!match.Success)
                throw new FormatException("Invalid message format");

        }

        
    }
}
