using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Aran.Omega.TypeB.Models
{
    public class Info
    {
        public Info(string name,string value,string unit)
        {
            Name = name;
            Value = value;
            Unit = unit;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
