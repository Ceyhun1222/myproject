using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Converters
{
    public static class ConverterFromSI
    {
        public static double Convert ( object valueType, double value, double defaultValue )
        {
            try
            {
                if ( valueType is UomWeight )
                    return FromWeightKg ( ( UomWeight ) valueType, value );

                if ( valueType is UomTemperature )
                    return FromTemperatureC ( ( UomTemperature ) valueType, value );

                if ( valueType is UomSpeed )
                    return FromSpeedM_Sec ( ( UomSpeed ) valueType, value );

                if ( valueType is UomPressure )
                    return FromPressurePA ( ( UomPressure ) valueType, value );

                if ( valueType is UomLightIntensity )
                    return FromLightIntensityCD ( ( UomLightIntensity ) valueType, value );

                if ( valueType is UomFrequency )
                    return FromFrequencyHZ ( ( UomFrequency ) valueType, value );

                if ( valueType is UomFL )
                    return FromTransitionLevelSM ( ( UomFL ) valueType, value );

                if ( valueType is UomDuration )
                    return FromDurationSEC ( ( UomDuration ) valueType, value );                
                
                if ( valueType is UomDistanceVertical )
                    return FromDistanceVerticalM ( ( UomDistanceVertical ) valueType, value );

                if ( valueType is UomDistance )
                    return FromDistanceM ( ( UomDistance ) valueType, value );

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static double FromWeightKg ( UomWeight uom, double source )
        {
            switch ( uom )
            {
                case UomWeight.KG:
                    return source;
             
                case UomWeight.T:
                    return ( source * 0.001 );

                case UomWeight.LB:
                    return ( source * 2.679209 );
                    
                case UomWeight.TON:
                    return ( source * 0.0011023 );
                
                default:
                    throw new Exception ( "UomWeight is not implemented !" );
            }
        }

        private static double FromTemperatureC ( UomTemperature uom, double source )
        {
            switch ( uom )
            {
                case UomTemperature.C:
                    return source;
             
                case UomTemperature.F:
                    return ( source * 1.8 + 32 );
                
                case UomTemperature.K:
                    return ( source + 273.15 );
                
                default:
                    throw new Exception ( "UomTemperature is not implemented !" );
            }
        }

        private static double FromSpeedM_Sec ( UomSpeed uom, double source )
        {
            switch ( uom )
            {
                case UomSpeed.KM_H:
                    return ( source * 3.6 );
             
                case UomSpeed.KT:
                    return ( source * 1.943844 );

                case UomSpeed.MACH:
                    return ( source * 0.003016955 );

                case UomSpeed.M_MIN:
                    return ( source * 60 );

                case UomSpeed.FT_MIN:
                    return ( source * 196.8504 );

                case UomSpeed.M_SEC:
                    return source;

                case UomSpeed.FT_SEC:
                    return ( source * 3.28084 );

                case UomSpeed.MPH:
                    return ( source * 2.236936 );

                default:
                    throw new Exception ( "UomSpeed is not implemented !" );
            }
        }

        private static double FromPressurePA ( UomPressure uom, double source )
        {
            switch ( uom )
            {
                case UomPressure.PA:
                    return source;
             
                case UomPressure.MPA:
                    return ( source * 0.000001 );
                
                case UomPressure.PSI:
                    return ( source * 0.0001450377 );
                
                case UomPressure.BAR:
                    return ( source * 0.00001 );
                
                case UomPressure.TORR:
                    return ( source * 0.007500617 );
                
                case UomPressure.ATM:
                    return ( source  * 0.000009869 );
                
                case UomPressure.HPA:
                    return ( source * 0.01 );
                
                default:
                    throw new Exception ( "UomPressure is not implemented !" );
            }
        }

        private static double FromLightIntensityCD ( UomLightIntensity uom, double source )
        {
            switch ( uom )
            {
                case UomLightIntensity.CD:
                    return source;
             
                default:
                    throw new Exception ( "UomLightIntensity is not implemented !" );
            }
        }

        private static double FromFrequencyHZ ( UomFrequency uom, double source )
        {
            switch ( uom )
            {
                case UomFrequency.HZ:
                    return source;
             
                case UomFrequency.KHZ:
                    return ( source * 0.001 );

                case UomFrequency.MHZ:
                    return ( source * 0.000001 );

                case UomFrequency.GHZ:
                    return ( source * 0.000000001 );

                default:
                    throw new Exception ( "UomFrequency is not implemented !" );
            }
        }

        private static double FromTransitionLevelSM ( UomFL uom, double source )
        {
            switch ( uom )
            {
                case UomFL.FL:
                    return ( source * 0.32808398 );
             
                case UomFL.SM:
                    return source;
                
                default:
                    throw new Exception ( "UomFL is not implemented !" );
            }
        }

        private static double FromDurationSEC ( UomDuration uom, double source )
        {
            switch ( uom )
            {
                case UomDuration.HR:
                    return ( source * 0.0002777778 );
             
                case UomDuration.MIN:
                    return ( source * 0.01666667 );
                
                case UomDuration.SEC:
                    return source;
                
                default:
                    throw new Exception ( "UomDuration is not implemented !" );
            }
        }

        private static double FromDistanceVerticalM ( UomDistanceVertical uom, double source )
        {
            switch ( uom )
            {
                case UomDistanceVertical.FT:
                    return ( source * 3.28083 );
             
                case UomDistanceVertical.M:
                    return source;
                
                case UomDistanceVertical.FL:
                    return ( source * 0.03280839 );
                
                case UomDistanceVertical.SM:
                    return ( source * 0.1 );
                
                default:
                    throw new Exception ( "UomDistanceVertical is not implemented !" );
            }
        }

        private static double FromDistanceM ( UomDistance uom, double source )
        {
            switch ( uom )
            {
                case UomDistance.NM:
                    return ( source * 0.0005399568 );

                case UomDistance.KM:
                    return ( source * 0.001 );

                case UomDistance.M:
                    return source;

                case UomDistance.FT:
                    return ( source * 3.28083 );

                case UomDistance.MI:
                    return ( source * 0.0006213712 );

                case UomDistance.CM:
                    return ( source * 100 );

                default:
                    throw new Exception ( "UomDistance is not implemented !" );
            }
        }

        private static double FromDepthCM ( UomDepth uom, double source )
        {
            switch ( uom )
            {
                case UomDepth.MM:
                    return ( source * 10 );
             
                case UomDepth.CM:
                    return source;
                
                case UomDepth.IN:
                    return ( source * 0.3937008 );
                
                case UomDepth.FT:
                    return ( source * 0.0328083 );
                
                default:
                    throw new Exception ( "UomDepth is not implemented !" );
            }
        }
    }
}
