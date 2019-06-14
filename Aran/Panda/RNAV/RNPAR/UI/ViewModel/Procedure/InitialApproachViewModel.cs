using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure
{
    class InitialApproachViewModel : StateViewModel
    {
        public InitialApproachViewModel(MainViewModel main) : base(main)
        {
        }

        public InitialApproachViewModel(MainViewModel main, StateViewModel previous) : base(main, previous)
        {
        }

        public override bool CanNext()
        {
            return false;
        }

        public override bool CanPrevious()
        {
            return true;
        }

        protected override void next()
        {
           
        }

        protected override void previous()
        {
            
        }

        protected override void saveReport()
        {
            
        }

        protected override void _destroy()
        {
            
        }
    }
}
