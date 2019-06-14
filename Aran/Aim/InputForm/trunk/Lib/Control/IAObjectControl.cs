using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Objects;

namespace Aran.Aim.InputFormLib
{
    public interface IAObjectControl
    {
        void LoadObject (AObject aObject, AimClassInfo classInfo);
    }
}
