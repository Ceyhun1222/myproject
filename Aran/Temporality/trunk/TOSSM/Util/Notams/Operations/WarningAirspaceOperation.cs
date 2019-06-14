using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Entity;
using TOSSM.ViewModel.Document.Editor;

namespace TOSSM.Util.Notams.Operations
{
    class WarningAirspaceOperation: TemporaryAirspaceOperation
    {
        public override void Prepare(NotamFeatureEditorViewModel notamViewModel)
        {
            if (notamViewModel.Notam.Type == (int)NotamType.C)
                PrepareForCancelation(notamViewModel);
            else if (notamViewModel.Notam.Code45.Equals("LW"))
                PrepareForActivation(notamViewModel);
            else
                throw new NotImplementedException($"Q-Code 45 {notamViewModel.Notam.Code45} is not supported");
        }

        public override void Save(NotamFeatureEditorViewModel notamViewModel)
        {
            if (notamViewModel.Notam.Type == (int)NotamType.C)
                Cancel(notamViewModel);
            else if (notamViewModel.Notam.Code45.Equals("LW"))
                Activate(notamViewModel);
            else
                throw new NotImplementedException($"Q-Code 45 {notamViewModel.Notam.Code45} is not supported");
        }
    }
}
