using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using ARAN.Contracts.Registry;
using Aran.Interfaces;

namespace SettingsService
{
    internal class Global
    {
        public static IAranEnvironment Env { get; set; }
        public static IPandaAranExtension AranExtension { get; set; }
    }
}
