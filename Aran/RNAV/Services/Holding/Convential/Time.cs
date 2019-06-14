using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holding.Convential
{
    public class Time
    {
        public void AddTimeChangedEvent ( EventHandlerTime OnTimeChanged )
        {
            _timeChanged += OnTimeChanged;
        }

        public double ValueInSeconds
        {
            get
            {
                return _valueInSeconds;
            }
            set
            {
                if ( _valueInSeconds != value )
                {
                    _valueInSeconds = value;
                    _valueInMinutes = _valueInSeconds / 60;
                    EventArgTime argTime = new EventArgTime ( Math.Round ( _valueInMinutes, 1 ) );
                    _timeChanged ( null, argTime );
                }
            }
        }

        public double ValueInMinutes
        {
            get
            {
                return _valueInMinutes;
            }
        }

        private EventHandlerTime _timeChanged;

        private double _valueInMinutes, _valueInSeconds;
    }
}
