using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Aran.Temporality.Internal.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct WorkPackageStructure
    {
        public int WorkPackage;
        public bool IsSafe;

        [MarshalAs(UnmanagedType.LPStr, SizeConst = 200)]
        public string Description;
    }
}
