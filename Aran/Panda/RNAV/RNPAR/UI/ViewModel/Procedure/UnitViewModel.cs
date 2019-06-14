using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure
{
    internal abstract class UnitViewModel : StateViewModel
    {
        private string _heightUnit;
        private string _heightUnitM;
        private string _distanceUnit;
        private string _distanceUnitNM;
        private string _speedUnit;

        protected UnitViewModel(MainViewModel main) : base(main)
        {
            HeightUnit = UnitConverter.HeightUnit;
            HeightUnitM = UnitConverter.HeightUnitM;
            DistanceUnit = UnitConverter.DistanceUnit;
            SpeedUnit = UnitConverter.SpeedUnit;
        }

        protected UnitViewModel(MainViewModel main, StateViewModel previous) : base(main, previous)
        {
            HeightUnit = UnitConverter.HeightUnit;
            HeightUnitM = UnitConverter.HeightUnitM;
            DistanceUnit = UnitConverter.DistanceUnit;
            SpeedUnit = UnitConverter.SpeedUnit;
        }

        protected UnitConverter UnitConverter => Env.Current.UnitContext.UnitConverter;

        public string HeightUnit
        {
            get => _heightUnit;
            set
            {
                Set(() => HeightUnit, ref _heightUnit, value);
            }
        }

        public string HeightUnitM
        {
            get => _heightUnitM;
            set
            {
                Set(() => HeightUnitM, ref _heightUnitM, value);
            }
        }

        public string DistanceUnit
        {
            get => _distanceUnit;
            set
            {
                Set(() => DistanceUnit, ref _distanceUnit, value);
            }
        }

        public string DistanceUnitNM
        {
            get => _distanceUnitNM;
            set
            {
                Set(() => DistanceUnitNM, ref _distanceUnitNM, value);
            }
        }

        public string SpeedUnit
        {
            get => _speedUnit;
            set
            {
                Set(() => SpeedUnit, ref _speedUnit, value);
            }
        }
    }
}
