using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.AranEnvironment
{
    public interface ISettingsPlugin
    {
        void Startup(IAranEnvironment aranEnv);
    }
}
