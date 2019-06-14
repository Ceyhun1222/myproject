using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran45ToAixm
{
    internal static class FieldConverter
    {
        public static TEnum? ToSameText<TEnum> (string code) where TEnum : struct
        {
            TEnum res;
            if (Enum.TryParse<TEnum> (code, out res))
                return res;
            return null;
        }

        public static CodeVerticalReference? ToCodeVerticalReference (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "HEI":
                    return CodeVerticalReference.SFC;
                case "ALT":
                    return CodeVerticalReference.MSL;
                case "W84":
                    return CodeVerticalReference.W84;
                case "QFE":
                    return CodeVerticalReference.OTHER_QFE;
                case "QNH":
                    return CodeVerticalReference.OTHER_QNH;
                case "STD":
                    return CodeVerticalReference.STD;
            }
            return null;
        }

        public static CodeDesignatedPoint? ToCodeDesignatedPoint (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "ICAO":
                    return CodeDesignatedPoint.ICAO;
                case "ADHP":
                    return CodeDesignatedPoint.TERMINAL;
            }
            return null;
        }

        public static CodeLevel? ToCodeLevel (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "u":
                    return CodeLevel.UPPER;
                case "l":
                    return CodeLevel.LOWER;
                case "b":
                    return CodeLevel.BOTH;
            }
            return null;
        }

        public static CodeRouteOrigin? ToCodeRouteOrigin (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "i":
                    return CodeRouteOrigin.INTL;
                case "d":
                    return CodeRouteOrigin.DOM;
            }
            return null;
        }

        public static CodeFlightRule? ToCodeFlightRule (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "i":
                    return CodeFlightRule.IFR;
                case "v":
                    return CodeFlightRule.VFR;
                case "iv":
                    return CodeFlightRule.ALL;
            }
            return null;
        }

        public static CodeMilitaryStatus? ToCodeMilitaryStatus (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "c":
                    return CodeMilitaryStatus.CIVIL;
                case "m":
                    return CodeMilitaryStatus.MIL;
                case "b":
                    return CodeMilitaryStatus.ALL;
            }
            return null;
        }

        public static ValDistanceVertical ToValDistanceVertical (object [] values)
        {
            try
            {
                var res = new ValDistanceVertical ();
                res.Value = Convert.ToDouble (values [0]);
                UomDistanceVertical? uom = ToSameText<UomDistanceVertical> (values [1].ToString ());
                if (uom == null)
                    return null;
                res.Uom = uom.Value;
                return res;
            }
            catch
            {
                return null;
            }
        }

        public static ValDistance ToValDistance (object [] values)
        {
            try
            {
                var res = new ValDistance ();
                res.Value = Convert.ToDouble (values [0]);
                UomDistance? uom = ToSameText<UomDistance> (values [1].ToString ());
                if (uom == null)
                    return null;
                res.Uom = uom.Value;
                return res;
            }
            catch
            {
                return null;
            }
        }

        public static CodeATCReporting? ToCodeATCReporting (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "c":
                    return CodeATCReporting.COMPULSORY;
                case "r":
                    return CodeATCReporting.ON_REQUEST;
                case "n":
                    return CodeATCReporting.NO_REPORT;
            }
            return null;
        }

        public static CodeDirection? ToCodeDirection (string code)
        {
            if (string.IsNullOrWhiteSpace (code))
                return null;
            code = code.ToUpper ();
            switch (code)
            {
                case "f":
                    return CodeDirection.FORWARD;
                case "b":
                    return CodeDirection.BACKWARD;
            }
            return null;
        }
    }
}
