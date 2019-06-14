using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class PreFinalPhase : Phase<PreFinalState>
    {
        public override PhaseType PhaseType => PhaseType.PreFinal;
        public override void Init()
        {
            CurrentState = new PreFinalState();
            CurrentState._AlignP_THRMin = 450.0; // RNPAR 4.5.5
            CurrentState._AlignP_THRMax = 1200.0; // RNPAR ?.?.? 
            CurrentState._OCHbyObctacle = 75.0; // RNPAR 2.2.3
            CurrentState._OCHbyAlignment = 0.0; // RNPAR ?.?.?

            CurrentState._GP_RDH = Env.Current.UnitContext.Constants.Pansops[ePANSOPSData.arAbv_Treshold].Value; // RNPAR 4.5.22 - ILS VGSI VPA implement 
            

            CurrentState._AlignP_THR = CurrentState._AlignP_THRMin;
          

            CurrentState._FASRNPval = (double) CurrentState.FASLateralProtectionRule.Calculate(555.6); // RNPAR 4.1.8


            CurrentState._MARNPval = (double)CurrentState.MASLateralProtectionRule.Calculate(1852.0);  // RNPAR 4.1.8
          

            CurrentState._PrelDHval = CurrentState._OCHbyObctacle;
            

            CurrentState._Tmin = Env.Current.DataContext.CurrADHP.ISAtC;

            CommittedState = null;
        }

        public override void ReInit()
        {
            CurrentState.ReCreate();
        }
    }

    struct TWC
    {
        public double height;
        public double speed;
    }
}
