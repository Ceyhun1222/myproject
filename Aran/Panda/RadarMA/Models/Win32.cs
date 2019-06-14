using System;
using System.Runtime.InteropServices;

namespace Aran.Panda.RadarMA.Models
{
    public static class Win32
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        public const uint IMAGE_CURSOR = 2;
        public const uint LR_LOADFROMFILE = 0x00000010;
    }
}
