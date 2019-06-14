using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using stdole;
using System.Windows.Forms;

namespace MapEnv
{
    internal static class WindowsAPI
    {
        public const int COLORONCOLOR = 3;
        public const int HORZSIZE = 4;
        public const int VERTSIZE = 6;
        public const int HORZRES = 8;
        public const int VERTRES = 10;
        public const int ASPECTX = 40;
        public const int ASPECTY = 42;
        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MF_STRING = 0x0;

        public const int EM_SETCUEBANNER = 0x1501;

        public enum PictureTypeConstants
        {
            picTypeNone = 0,
            picTypeBitmap = 1,
            picTypeMetafile = 2,
            picTypeIcon = 3,
            picTypeEMetafile = 4
        }
        public struct PICTDESC
        {
            public int cbSizeOfStruct;
            public int picType;
            public IntPtr hPic;
            public IntPtr hpal;
            public int _pad;
        }
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport ("olepro32.dll", EntryPoint = "OleCreatePictureIndirect",
                   PreserveSig = false)]
        public static extern int OleCreatePictureIndirect (
            ref PICTDESC pPictDesc, ref Guid riid, bool fOwn,
            out IPictureDisp ppvObj);

        [DllImport ("gdi32.dll", EntryPoint = "CreateCompatibleDC",
            ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC (IntPtr hDC);

        [DllImport ("gdi32.dll", EntryPoint = "DeleteDC",
            ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC (IntPtr hdc);

        [DllImport ("gdi32.dll", EntryPoint = "SelectObject",
            ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject (
            IntPtr hDC, IntPtr hObject);

        [DllImport ("gdi32.dll", EntryPoint = "DeleteObject",
            ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject (IntPtr hObject);

        [DllImport ("gdi32.dll", EntryPoint = "CreateCompatibleBitmap",
            ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap (
            IntPtr hObject, int width, int height);

        [DllImport ("user32.dll", EntryPoint = "GetDC",
            ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC (IntPtr ptr);

        [DllImport ("user32.dll", EntryPoint = "ReleaseDC",
            ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr ReleaseDC (IntPtr hWnd, IntPtr hDc);

        [DllImport ("gdi32", EntryPoint = "CreateSolidBrush",
            ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateSolidBrush (int crColor);

        [DllImport ("user32", EntryPoint = "FillRect",
            ExactSpelling = true, SetLastError = true)]
        public static extern int FillRect (
            IntPtr hdc, ref RECT lpRect, IntPtr hBrush);

        [DllImport ("GDI32.dll", EntryPoint = "GetDeviceCaps",
            ExactSpelling = true, SetLastError = true)]
        public static extern int GetDeviceCaps (IntPtr hdc, int nIndex);

        [DllImport ("user32", EntryPoint = "GetClientRect",
            ExactSpelling = true, SetLastError = true)]
        public static extern int GetClientRect (
            IntPtr hwnd, ref RECT lpRect);


        [DllImport ("user32.dll")]
        public static extern IntPtr GetSystemMenu (IntPtr hWnd, bool bRevert);
        [DllImport ("user32.dll")]
        public static extern bool InsertMenu (IntPtr hMenu,
            Int32 wPosition, Int32 wFlags, Int32 wIDNewItem,
            string lpNewItem);


        #region Hide TreeNode Checkbox

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageForTreeView(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        
        public static void HideCheckBox(TreeView tvw, TreeNode node)
        {
            var tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessageForTreeView(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

        #endregion

        #region TextBox Plaseholder

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint="SendMessage")]
        private static extern Int32 SendMessageForTextBox(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);
        public static void SetTextBoxPlaseHolder(TextBox textBox, string plaseHolderText)
        {
            SendMessageForTextBox(textBox.Handle, EM_SETCUEBANNER, 0, plaseHolderText);
        }

        #endregion
    }
}
