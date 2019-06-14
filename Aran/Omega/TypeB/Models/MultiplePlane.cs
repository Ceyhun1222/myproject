using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Omega.Models
{
    public interface IMultiplePlane
    {
        List<Plane> Planes { get; set; }
    }
}
