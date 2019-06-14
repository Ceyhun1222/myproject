using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.InputFormLib
{
    public interface IAimFieldControl
    {
        void SetAimField (AimField aimField, AimPropInfo propInfo);
        AimField AimField { get; }
    }
}
