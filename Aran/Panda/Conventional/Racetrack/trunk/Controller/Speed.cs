using System.Collections.Generic;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class Speed
	{
		public Speed (LimitingDistance limDist, ModulType selectedModul)
		{
			_limDist = limDist;
			_intervals = new List<SpeedInterval> ();
			CategoryList = new List<string> ();
			_selectedModul = selectedModul;
			_ias = 0;
			_altitude = 0;
			_tas = 0;
			_dT = 15;
			_selectedAircraftCategoryIndex = -1;
			//SetInterval(3050);

			//_height4250 = 4250;
			//if (GlobalParams.Settings.HeightUnit == VerticalDistanceType.Ft)
			//	_height4250 = 4267.2;
			//_height6100 = 6100;
			//if (GlobalParams.Settings.HeightUnit == VerticalDistanceType.Ft)
			//	_height6100 = 6096;
			//_height10350 = 10350;
			//if (GlobalParams.Settings.HeightUnit == VerticalDistanceType.Ft)
			//	_height10350 = 10363.2;
			_heightAccuracy = 0;
			if (GlobalParams.Settings.HeightUnit == VerticalDistanceType.Ft)
				_heightAccuracy = 20;
		}

		#region Add EventHandlers

		public void AddIasIntervalChangedEvent ( IntervalEventHandler onIasChanged )
		{
			_iasIntervalChanged += onIasChanged;
		}

		public void AddTasChangedEvent ( CommonEventHandler tasChanged )
		{
			_tasChanged += tasChanged;
		}

        public void AddCategoryListChangedEvent(CategoryListChangedEventHandler categoryListChanged) 
        {
            _categoryListChanged += categoryListChanged;
        }

		#endregion

		#region Set Values

        public void SetFlightPhase(FlightPhase flightPhase) 
        {
            _flightPhase = flightPhase;
            SetCategoryList();
        }

        private void SetCategoryList()
        {
	        SpeedInterval intervalCatA;
	        SpeedInterval intervalCatC;
	        switch (_flightPhase)
            {
                case FlightPhase.TerminalNormal:
                    if ( _altitude <= (4250 + _heightAccuracy))
                    {
                        CategoryList.Clear();
                        CategoryList.Add("A/B");
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatA = new SpeedInterval
	                    {
		                    Min =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo4250MinNormalTerminal]
				                    [aircraftCategory.acA],
		                    Max =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo4250MaxNormalTerminal]
				                    [aircraftCategory.acA]
	                    };
	                    _intervals.Add(intervalCatA);

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo4250MinNormalTerminal]
				                    [aircraftCategory.acC],
		                    Max =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo4250MaxNormalTerminal]
				                    [aircraftCategory.acC]
	                    };
	                    _intervals.Add(intervalCatC);
                    }
                    else if (_altitude <= (6100 + _heightAccuracy))
                    {
                        CategoryList.Clear();
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo6100MinNormalTerminal]
				                    [aircraftCategory.acC],
		                    Max =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo6100MaxNormalTerminal]
				                    [aircraftCategory.acC]
	                    };
	                    _intervals.Add(intervalCatC);
                    }
                    else if (_altitude <= (10350 + _heightAccuracy))
                    {
                        CategoryList.Clear();
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo10350MinNormalTerminal
			                    ][aircraftCategory.acC],
		                    Max =
			                    GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASUpTo10350MaxNormalTerminal
			                    ][aircraftCategory.acC]
	                    };
	                    _intervals.Add(intervalCatC);
                    }
                    else
                    {
                        CategoryList.Clear();
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min = double.NaN,
		                    Max = double.NaN
	                    };
	                    _intervals.Add(intervalCatC);

                        Ias = CalculateMach() * 0.83;
                    }
                    break;
                case FlightPhase.TerminalTurbulence:
                    if (_altitude <= (4250 + _heightAccuracy))
                    {
                        CategoryList.Clear();
                        CategoryList.Add("A/B");
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatA = new SpeedInterval
	                    {
		                    Min =
			                    GlobalParams.ConstantG.AircraftCategory[
				                    aircraftCategoryData.hldIASUpTo4250MinTurbulenceTerminal][aircraftCategory.acA],
		                    Max =
			                    GlobalParams.ConstantG.AircraftCategory[
				                    aircraftCategoryData.hldIASUpTo4250MaxTurbulenceTerminal][aircraftCategory.acA]
	                    };
	                    _intervals.Add(intervalCatA);

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min =
			                    GlobalParams.ConstantG.AircraftCategory[
				                    aircraftCategoryData.hldIASUpTo4250MinTurbulenceTerminal][aircraftCategory.acC],
		                    Max =
			                    GlobalParams.ConstantG.AircraftCategory[
				                    aircraftCategoryData.hldIASUpTo4250MaxTurbulenceTerminal][aircraftCategory.acC]
	                    };
	                    _intervals.Add(intervalCatC);
                    }
                    else if (_altitude <= (6100 + _heightAccuracy))
                    {
                        CategoryList.Clear();
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min = double.NaN,
		                    Max = double.NaN
	                    };
	                    _intervals.Add(intervalCatC);
                    }
                    else if (_altitude <= (10350 + _heightAccuracy))
                    {
                        CategoryList.Clear();
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min = double.NaN,
		                    Max = double.NaN
	                    };
	                    _intervals.Add(intervalCatC);
                    }
                    else
                    {
                        CategoryList.Clear();
                        CategoryList.Add("C/D/E");

                        _intervals.Clear();

	                    intervalCatC = new SpeedInterval
	                    {
		                    Min = double.NaN,
		                    Max = double.NaN
	                    };
	                    _intervals.Add(intervalCatC);

                        Ias = CalculateMach() * 0.83;
                    }
                    break;
                case FlightPhase.TerminalInitialApproach:

                    CategoryList.Clear();
                    CategoryList.Add("A");
                    CategoryList.Add("B");
                    CategoryList.Add("C");
                    CategoryList.Add("D");
                    CategoryList.Add("E");

                    _intervals.Clear();

		            intervalCatA = new SpeedInterval
		            {
			            Min =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMinInitialApproachTerminal][
					            aircraftCategory.acA],
			            Max =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMaxInitialApproachTerminal][
					            aircraftCategory.acA]
		            };
		            _intervals.Add(intervalCatA);


		            var intervalCatB = new SpeedInterval
		            {
			            Min =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMinInitialApproachTerminal][
					            aircraftCategory.acB],
			            Max =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMaxInitialApproachTerminal][
					            aircraftCategory.acB]
		            };
		            _intervals.Add(intervalCatB);

		            intervalCatC = new SpeedInterval
		            {
			            Min =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMinInitialApproachTerminal][
					            aircraftCategory.acC],
			            Max =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMaxInitialApproachTerminal][
					            aircraftCategory.acC]
		            };
		            _intervals.Add(intervalCatC);

		            var intervalCatD = new SpeedInterval
		            {
			            Min =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMinInitialApproachTerminal][
					            aircraftCategory.acD],
			            Max =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMaxInitialApproachTerminal][
					            aircraftCategory.acD]
		            };
		            _intervals.Add(intervalCatD);


		            var intervalCatE = new SpeedInterval
		            {
			            Min =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMinInitialApproachTerminal][
					            aircraftCategory.acE],
			            Max =
				            GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASMaxInitialApproachTerminal][
					            aircraftCategory.acE]
		            };
		            _intervals.Add(intervalCatE);
                    break;

                case FlightPhase.Enroute:
					if (_altitude <= (10350 + _heightAccuracy))
					{
						CategoryList.Clear ();
						CategoryList.Add ("TEST");
						_intervals.Clear ();

						intervalCatA = new SpeedInterval
						{
							Min = GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASEnroute][aircraftCategory.acA],
							Max = GlobalParams.ConstantG.AircraftCategory[aircraftCategoryData.hldIASEnroute][aircraftCategory.acA]
						};
						_intervals.Add (intervalCatA);
					}
					else
					{
						CategoryList.Clear ();
						CategoryList.Add ("TEST");
						_intervals.Clear ();

						intervalCatC = new SpeedInterval
						{
							Min = double.NaN,
							Max = double.NaN
						};
						_intervals.Add (intervalCatC);

						Ias = CalculateMach () * 0.83;
					}

                    break;
                default:
                    break;
            }
            _categoryListChanged(CategoryList);
        }

		public void SetAircraftCategory ( int index )
		{
             //if ( _selectedAircraftCategoryIndex != index )
			{
				SelectedAircraftCategoryIndex = index;
			}
		}

		public void SetAltitude ( double altitude )
		{
            //SetInterval(altitude);
            _altitude = altitude;
            SetCategoryList();
            if (Ias > 0)
            {
                if (_selectedModul == ModulType.Racetrack)
                {
                    Tas = ARANMath.IASToTAS(Ias, _altitude, _dT);
                }
                else if (_selectedModul == ModulType.Holding)
                {
                    Tas = ARANMath.IASToTASForRnav(Ias, _altitude, _dT);
                }
            }
				
		}

        //private void SetInterval()
        //{
        //    if (_iasIntervalChanged != null && SelectedAircraftCategoryIndex != -1)
        //    {
        //        SpeedInterval argIASInterval = new SpeedInterval();
        //        argIASInterval.Min = GlobalParams.UnitConverter.SpeedToDisplayUnits(_intervals[SelectedAircraftCategoryIndex].Min, eRoundMode.NERAEST);
        //        argIASInterval.Max = GlobalParams.UnitConverter.SpeedToDisplayUnits(_intervals[SelectedAircraftCategoryIndex].Max, eRoundMode.NERAEST);
        //        _iasIntervalChanged(null, argIASInterval);
        //    }
        //}

		public void SetIas ( double ias )
		{
			Ias = GlobalParams.UnitConverter.SpeedToInternalUnits ( ias );
		}

		#endregion

		#region Properties

		public double Height10350 => (10350 + _heightAccuracy);

		//public SpeedInterval this[ int index ]
        //{
        //    get
        //    {
        //        return _intervals[ index ];
        //    }
        //}

		public int SelectedAircraftCategoryIndex
		{
			get
			{
				return _selectedAircraftCategoryIndex;
			}
			private set
			{
				_selectedAircraftCategoryIndex = value;
				if ( _iasIntervalChanged != null && _intervals.Count > 0)
				{
					SpeedInterval argIasInterval = new SpeedInterval
					{
						Min =
							!double.IsNaN(_intervals[SelectedAircraftCategoryIndex].Min)
								? AdaptToStandartSpeedValue(
									GlobalParams.UnitConverter.SpeedToDisplayUnits(_intervals[SelectedAircraftCategoryIndex].Min,
										eRoundMode.NEAREST))
								: double.NaN,
						Max =
							!double.IsNaN(_intervals[SelectedAircraftCategoryIndex].Max)
								? AdaptToStandartSpeedValue(
									GlobalParams.UnitConverter.SpeedToDisplayUnits(_intervals[SelectedAircraftCategoryIndex].Max,
										eRoundMode.NEAREST))
								: double.NaN
					};

					_iasIntervalChanged ( null, argIasInterval );
				}
			}
		}

		public double Ias
		{
			get
			{
				return _ias;
			}

			set
			{
				_ias = value;
                if (_altitude > 0)
                {
                    if (_selectedModul == ModulType.Racetrack)
                    {
                        Tas = ARANMath.IASToTAS(Ias, _altitude, _dT);
                    }
                    else if (_selectedModul == ModulType.Holding)
                    {
                        Tas = ARANMath.IASToTASForRnav(Ias, _altitude, _dT);
                    }
                }
			}
		}

		public double Tas
		{
			get
			{
				return _tas;
			}
			private set
			{
                if (_isTurbulence)
                {

                }
                else 
                {
                    _tas = _altitude > (10350 + _heightAccuracy) ? CalculateMach() : value;
                }
				_limDist.SetTas ( _tas );

				_tasChanged ( null, new CommonEventArg ( GlobalParams.UnitConverter.SpeedToDisplayUnits ( _tas, eRoundMode.NEAREST ) ) );
			}
		}

        public FlightPhase FlighPhase => _flightPhase;

		private bool _isTurbulence;

        public double CalculateMach(double value = 1)
        {
            var h = _altitude;
            if (_altitude > 11000)
                h = 11000;
            var result = System.Math.Sqrt((273.15 + _dT) - h * 6.5 / 1000) * 20.046796 * value;
            return result;
        }

        public bool IsTurbulence
        {
            get { return _isTurbulence; }
            set 
            {
                _isTurbulence = value;
            }
        }
        
		public List<string> CategoryList
		{
			get; }

        public string SelectedCategory => CategoryList[_selectedAircraftCategoryIndex];

		#endregion

        /// <summary>
        /// Returns nearest value that divides 5 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        public double AdaptToStandartSpeedValue(double oldValue)
        {
            double reminder = oldValue % 5;
            int quotient = (int)oldValue / 5;
            if (reminder >= 2.5)
                return (quotient + 1) * 5;
            else
                return quotient * 5;
        }

		internal string GetSpeedIntervalString ()
		{
			return AdaptToStandartSpeedValue (GlobalParams.UnitConverter.SpeedToDisplayUnits (520 * 0.2777777778)) + " " + GlobalParams.UnitConverter.SpeedUnit + " / 0.8 M";
		}

		internal string CalculateMinimumSpeed ()
		{
			double ias = 520;
			double mach = CalculateMach () * 3.6 * 0.8;
			if (ias < mach)
			{
				Ias = ias * 0.2777777778;
				return AdaptToStandartSpeedValue(GlobalParams.UnitConverter.SpeedToDisplayUnits (Ias)) + " " + GlobalParams.UnitConverter.SpeedUnit;
			}
			else
			{
				Ias = mach / 3.6;
				return "0.8 M";
			}
		}

		private IntervalEventHandler _iasIntervalChanged;
        private CategoryListChangedEventHandler _categoryListChanged;
		private CommonEventHandler _tasChanged;

		private readonly LimitingDistance _limDist;
		private double _ias;
		private double _altitude;
		private double _tas;
        private readonly double _dT;
		private readonly List<SpeedInterval> _intervals;
		private int _selectedAircraftCategoryIndex;
        private readonly ModulType _selectedModul;
        private FlightPhase _flightPhase;
		private const double Height4250 = 4250;
		private const double Height6100 = 6100;
		private const double _height10350 = 10350;
		private readonly double _heightAccuracy;
    }
}