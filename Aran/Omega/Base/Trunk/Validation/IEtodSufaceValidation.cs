using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Omega.Models;

namespace Aran.Omega.Validation
{
    interface IEtodSufaceValidation
    {
        string GetHtmlReport(DrawingSurface surface);
    }
}
