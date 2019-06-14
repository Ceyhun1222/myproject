using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Aran.Aim.InputForm
{
    public static class Globals
    {
        static Globals ()
        {
            _cursorDict = new Dictionary<string, Cursor> ();
        }

        public static IAranEnvironment Environment { get; set; }

        public static Cursor GetCursor (string name)
        {
            Cursor cursor;
            if (_cursorDict.TryGetValue (name, out cursor))
                return cursor;

            string cursorFileName = Application.LocalUserAppDataPath + "\\" + name + ".cur";

            if (!File.Exists (cursorFileName))
            {
                FileStream fs = new FileStream (cursorFileName, FileMode.Create, FileAccess.Write);

                object obj = Properties.Resources.ResourceManager.GetObject (name);

                var data = (byte []) obj;
                fs.Write (data, 0, data.Length);
                fs.Close ();
            }

            IntPtr imgHandle = Win32.LoadImage (IntPtr.Zero,
                cursorFileName,
                Win32.IMAGE_CURSOR,
                0,
                0,
                Win32.LR_LOADFROMFILE);

            cursor = new Cursor (imgHandle);
            _cursorDict.Add (name, cursor);
            return cursor;
        }

        public static string DD2DMS (double xORy, bool isX, int round)
        {
            double k = xORy;
            double deg, min, sec;
            int sign = Math.Sign (k);

            {
                double n = Math.Abs (Math.Round (Math.Abs (k) * sign, 10));

                deg = (int) n;
                double dn = (n - deg) * 60;
                dn = Math.Round (dn, 8);
                min = (int) dn;
                sec = (dn - min) * 60;
            }

            string degStr = deg.ToString ();
            int strLen = 2;
            string signSymb = "SN";

            if (isX)
            {
                strLen = 3;
                signSymb = "WE";
            }

            sec = Math.Round (sec, round);

            while (degStr.Length < strLen)
                degStr = "0" + degStr;

            return string.Format ("{0}°{1}'{2}\" {3}", degStr, min, sec, signSymb [(sign + 1) >> 1]);
        }

        public static void ShowError (Exception ex)
        {
            MessageBox.Show (ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static Dictionary<string, Cursor> _cursorDict;
    }

    public static class Win32
    {
        [DllImport ("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadImage (IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        public const uint IMAGE_CURSOR = 2;
        public const uint LR_LOADFROMFILE = 0x00000010;
    }
}
