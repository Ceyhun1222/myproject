using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holding.Convential
{
    public class LimitingDistance
    {
        public LimitingDistance ( EntryDirection entryDir, Time time)
        {
            _entryDirection = entryDir;
            _time = time;
        }

        public void AddLimitingDistanceChangedEvent ( EventHandlerLimitingDistance OnLimDistChanged )
        {
            _limitingDistanceChanged += OnLimDistChanged;
        }

        public void SetNominalDistance ( double nominalDistance)
        {
            _nominalDistance = nominalDistance;
            if ( _valueInPrj != 0 && Radius != 0 && _nominalDistance != 0 )
            {
                SetLegLength ();
                CalculateMinAndMax ();
            }
        }

        public void SetEntryDirection ( EntryDirection entryDir )
        {
            _entryDirection = entryDir;
            CalculateMinAndMax ();
        }

        public void SetTAS ( double tas )
        {
            _tas = tas;
            Radius = CalculateRadius ( _tas );
            CalculateMinAndMax ();
        }

        public void SetValue ( double limdist )
        {
            ValueInPrj = Common.DeConvertDistance ( limdist );
        }

        public void SetAltitude ( double altitude )
        {
            _altitude = altitude;
            CalculateValueInGeo ();
        }

        public double LegLength
        {
            get
            {
                return _legLength;
            }
            private set
            {
                _legLength = value;
                _time.ValueInSeconds = _legLength / _tas;
            }
        }

        public double Radius
        {
            get
            {
                return _radius;
            }
            private set
            {
                _radius = value;
                SetLegLength ();
            }
        }

        private void SetLegLength ( )
        {
            if ( _entryDirection == EntryDirection.EntrDir_Toward )
                LegLength = Math.Sqrt ( _valueInPrj*_valueInPrj - 4 * _radius*_radius )-_nominalDistance;
            else if ( _entryDirection == EntryDirection.EntrDir_Away )
                LegLength = _nominalDistance - Math.Sqrt ( _valueInPrj * _valueInPrj - 4 * Radius * Radius );
        }

        public double Min 
        {
            get
            {
                return _min;
            }
            private set
            {
                _min = value;
            }
        }

        public double Max 
        {
            get
            {
                return _max;
            }
            private set
            {
                _max = value;
            }
        }

        public double ValueInPrj 
        {
            get
            {
                return _valueInPrj;
            }

            private set
            {
                _valueInPrj = value;
                CalculateValueInGeo ();
                SetLegLength ();
            }
        }

        public double ValueInGeo 
        {
            get
            {
                return _valueInGeo;
            }
            private set
            {
                _valueInGeo = value;
            }
        }

        private void CalculateMinAndMax ( )
        {
            if ( _entryDirection == EntryDirection.EntrDir_None )
                return;
            if ( _tas != 0 && _nominalDistance != 0 )
            {
                double minLegLength, maxLegLength;
                /* 60 seconds = 1 minute */
                if ( 60 * _tas < 2*Radius )
                {
                    minLegLength = 2 * Radius;
                }
                else
                {
                    minLegLength = 60 * _tas;
                }
                maxLegLength = 180 * _tas;
                if ( _entryDirection == EntryDirection.EntrDir_Toward )
                {
                    Min = Math.Sqrt ( ( _nominalDistance + minLegLength )*( _nominalDistance + minLegLength )+ 4 * Radius*Radius );
                    Max = Math.Sqrt ( ( _nominalDistance + maxLegLength )*( _nominalDistance + maxLegLength )+ 4 * Radius*Radius );
                }
                else 
                {
                    Min = Math.Sqrt ( ( _nominalDistance - maxLegLength ) * ( _nominalDistance - maxLegLength ) + 4 * Radius * Radius );
                    Max = Math.Sqrt ( ( _nominalDistance - minLegLength ) * ( _nominalDistance - minLegLength ) + 4 * Radius * Radius );
                }
                if ( _limitingDistanceChanged != null )
                {
                    EventArgLimitingDistance argLimDist = new EventArgLimitingDistance( Common.ConvertDistance ( Min,roundType.realValue),
                                                                                        Common.ConvertDistance ( Max, roundType.realValue ) );
                    _limitingDistanceChanged ( null, argLimDist );
                }
            }
        }

        private double CalculateRadius ( double tas)
        {
            double R = 943.27 / ( tas*3.6 );
            if ( R > 3 )
                R = 3;
            return ( tas *3.6 *1000 )/ ( 62.83*R );
        }

        private void CalculateValueInGeo ( )
        {
            ValueInGeo = Math.Sqrt ( ValueInPrj * ValueInPrj + _altitude*_altitude );
        }

        private EventHandlerLimitingDistance _limitingDistanceChanged;

        private EntryDirection _entryDirection;
        private double _nominalDistance=0;
        private double _valueInPrj;
        private double _valueInGeo;
        private double _tas =0;
        private double _min, _max;
        private double _legLength;
        private double _radius;
        private double _altitude;
        private Time _time;
    }
}
