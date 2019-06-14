using System;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class LimitingDistance
	{
		public LimitingDistance ( MainController controller, bool hasDsgPnt )
		{
			_controller = controller;
			_nominalDistance = 0;
			_tas = 0;
		    _timeMinimum = 1;
		    _timeMaximum = _timeMinimum;
		}

		#region Add Event Handlers

		internal void AddLimDistIntervalChangedEvent ( IntervalEventHandler onLimDistIntervalChanged )
		{
			_limDistIntervalChanged += onLimDistIntervalChanged;
		}

		internal void AddTimeChangedEvent ( CommonEventHandler onTimeChanged )
		{
			_timeChanged += onTimeChanged;
		}

		internal void AddLimDistChangedEvent ( CommonEventHandler onLimDistChanged )
		{
			_limDistChanged += onLimDistChanged;
		}

		internal void AddLegLengthChangedEvent ( CommonEventHandler onLegLengthChanged )
		{
			_legLengthChanged += onLegLengthChanged;
		}
		#endregion

		#region Methods

		internal void SetNominalDistance ( double nominalDistance )
		{
			_nominalDistance = nominalDistance;
			if ( Radius != 0 && _nominalDistance != 0 )
			{
				bool isOkDist = CalculateMinAndMax ( );
				if(isOkDist &&  _valueInPrj != 0 )
					SetLegLength ( );
			}
		}

	    private double ConvertTimeToSeconds(double timeInMinute)
	    {
	        return timeInMinute * 60;
	    }

		internal void SetEntryDirection ( )
		{
			CalculateMinAndMax ( );
		}

		internal void SetTas ( double tas )
		{
			_tas = tas;
			_radius = CalculateRadius ( _tas );
			if (_controller.ProcType == ProcedureTypeConv.Vordme )
				CalculateMinAndMax ( );
			Radius = _radius;
		}

		internal void SetValue ( double limdist )
		{
			ValueInPrj = GlobalParams.UnitConverter.DistanceToInternalUnits ( limdist );
		}

		internal void SetAltitude ( double altitude )
		{
			_altitude = altitude;
			CalculateValueInGeo ( );
		}

		internal void SetTime ( double timeInMin )
		{
			TimeInMin = timeInMin;//+0.25;
			if ( _tas == 0 )
				return;
			LegLength = TimeInMin * _tas * 60;
			if ( _controller.ProcType == ProcedureTypeConv.Vordme )
			{
				if ( _controller.EntryDirection == EntryDirection.Toward )
					_valueInPrj = Math.Sqrt ( ( _legLength + _nominalDistance ) * ( _legLength + _nominalDistance ) + 4 * _radius * _radius );
				else if ( _controller.EntryDirection == EntryDirection.Away )
					_valueInPrj = Math.Sqrt ( ( _nominalDistance - _legLength ) * ( _nominalDistance - _legLength ) + 4 * _radius * _radius );
				//LegLength = _nominalDistance - Math.Sqrt ( _valueInPrj * _valueInPrj - 4 * Radius * Radius );
				CalculateValueInGeo ( );

				_limDistChanged ( null, new CommonEventArg ( GlobalParams.UnitConverter.DistanceToDisplayUnits ( _valueInPrj, eRoundMode.NEAREST ) ) );
			}
		}

		private void SetLegLength ( )
		{
			if ( _controller.ProcType == ProcedureTypeConv.Vordme )
			{
				if ( _controller.EntryDirection == EntryDirection.Toward )
					LegLength = Math.Sqrt ( _valueInPrj * _valueInPrj - 4 * _radius * _radius ) - _nominalDistance;
				else if (_controller.EntryDirection == EntryDirection.Away )
					LegLength = _nominalDistance - Math.Sqrt ( _valueInPrj * _valueInPrj - 4 * Radius * Radius );
			}
			else
				LegLength = TimeInMin * _tas * 60;
		}

		internal bool CalculateMinAndMax ( )
		{
			if ( _controller.EntryDirection == EntryDirection.None )
				return true;
			if ( _tas != 0 && _nominalDistance != 0 )
			{
				
				/* 60 seconds = 1 minute */
				//
				double time = double.NaN;
				//if ( 60 * _tas < 2 * Radius )
				//{
				//	minLegLength = 2 * Radius;
				//	time = ( minLegLength / _tas ) / 60;
				//}
				//else			    
			    var minLegLength = ConvertTimeToSeconds(_timeMinimum) * _tas;
			    var maxLegLength = ConvertTimeToSeconds(_timeMaximum) * _tas;
    //            if ( _controller.SelectedModul == ModulType.Racetrack )
				//	maxLegLength = 180 * _tas;
				//else
				//{                    
				//	if ( _altitude < _controller.Altitude4Holding )
				//	{
				//		maxLegLength = minLegLength;
				//		time = 1;
				//	}
				//	else
				//		maxLegLength = 90 * _tas;
				//}
				SpeedInterval limDistInterval = new SpeedInterval ( );
				if (_controller.EntryDirection == EntryDirection.Toward )
				{
					Min = Math.Sqrt ( ( _nominalDistance + minLegLength ) * ( _nominalDistance + minLegLength ) + 4 * Radius * Radius );
					Max = Math.Sqrt ( ( _nominalDistance + maxLegLength ) * ( _nominalDistance + maxLegLength ) + 4 * Radius * Radius );
				}
				else
				{
					if ( _nominalDistance > maxLegLength )
					{
						Min = Math.Sqrt ( ( _nominalDistance - maxLegLength ) * ( _nominalDistance - maxLegLength ) + 4 * Radius * Radius );
						Max = Math.Sqrt ( ( _nominalDistance - minLegLength ) * ( _nominalDistance - minLegLength ) + 4 * Radius * Radius );
					}
					else
					{
						//if ( _nominalDistance > minLegLength )
						//{
						//	Min = Math.Sqrt ( ( _nominalDistance - maxLegLength ) * ( _nominalDistance - maxLegLength ) + 4 * Radius * Radius );
						//	Max = Math.Sqrt ( ( _nominalDistance - minLegLength ) * ( _nominalDistance - minLegLength ) + 4 * Radius * Radius );

						//	limDistInterval.Min = GlobalParams.UnitConverter.DistanceToDisplayUnits ( Min, eRoundMode.NERAEST );
						//	limDistInterval.Max = double.NaN;
						//}
						//else
						{
							limDistInterval.Min = double.NaN;
							limDistInterval.Max = double.NaN;
						}
						_limDistIntervalChanged ( null, limDistInterval );
						return false;
					}
				}

				limDistInterval.Min = GlobalParams.UnitConverter.DistanceToDisplayUnits ( Min, eRoundMode.NEAREST );
				limDistInterval.Max = GlobalParams.UnitConverter.DistanceToDisplayUnits ( Max, eRoundMode.NEAREST );
				if ( !double.IsNaN ( time ) )
					_limDistIntervalChanged ( ARANMath.AdvancedRound ( time, 0.5, eRoundMode.CEIL ), limDistInterval );
				else
					_limDistIntervalChanged ( null, limDistInterval );
			}
			return true;
		}

		private double CalculateRadius ( double tas )
		{
			double r = 943.27 / ( tas * 3.6 );
			if ( r > 3 )
				r = 3;
			return ( tas * 3.6 * 1000 ) / ( 62.83 * r );
		}

		private void CalculateValueInGeo ( )
		{
			if ( _valueInPrj > 0 && _altitude > 0 )
				ValueInGeo = Math.Sqrt ( ValueInPrj * ValueInPrj + _altitude * _altitude );
		}

	    internal void SetMaximumTime(double maximum)
	    {
	        if (maximum != _timeMaximum)
	        {
	            _timeMaximum = maximum;
	            CalculateMinAndMax();
	        }
	    }

        #endregion

        #region Properties

        private double Min
		{
			get; set;
		}

		private double Max
		{
			get; set;
		}

		internal double Radius
		{
			get
			{
				return _radius;
			}
			private set
			{
				_radius = value;
				SetLegLength ( );
			}
		}

		internal double ValueInPrj
		{
			get
			{
				return _valueInPrj;
			}

			private set
			{
				_valueInPrj = value;
				CalculateValueInGeo ( );
				SetLegLength ( );
			}
		}

		internal double ValueInGeo
		{
			get;
			private set;
		}

		internal double LegLength
		{
			get
			{
				return _legLength;
			}

			private set
			{
				if ( value == _legLength )
					return;

				_legLength = value;


				_legLengthChanged ( null, new CommonEventArg ( GlobalParams.UnitConverter.DistanceToDisplayUnits ( _legLength, eRoundMode.NEAREST ) ) );

				if ( _controller.ProcType == ProcedureTypeConv.Vordme )
				{
					double time = _legLength / _tas;
					if ( Math.Abs ( TimeInMin * 60 - time ) > Eps )
					{
						TimeInMin = time / 60;

						CommonEventArg argTime = new CommonEventArg ( Math.Round ( TimeInMin, 1 ) );
						_timeChanged ( null, argTime );

					}
				}
			}
		}

		internal double TimeInMin
		{
			get; private set;
		}

		#endregion

		#region Fields

		private IntervalEventHandler _limDistIntervalChanged;
		private CommonEventHandler _timeChanged;
		private CommonEventHandler _legLengthChanged;
		private CommonEventHandler _limDistChanged;

		private const double Eps = 0.01;

		private double _nominalDistance;
		private double _valueInPrj;
		private double _tas;
		private double _legLength;
		private double _radius;
		private double _altitude;
		private readonly MainController _controller;
	    private double _timeMinimum;
	    private double _timeMaximum;
        #endregion
    }
}