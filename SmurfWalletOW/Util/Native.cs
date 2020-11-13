using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SmurfWalletOW.Util
{
    public static class Native
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public const UInt32 SW_HIDE = 0;
        public const UInt32 SW_SHOWNORMAL = 1;
        public const UInt32 SW_NORMAL = 1;
        public const UInt32 SW_SHOWMINIMIZED = 2;
        public const UInt32 SW_SHOWMAXIMIZED = 3;
        public const UInt32 SW_MAXIMIZE = 3;
        public const UInt32 SW_SHOWNOACTIVATE = 4;
        public const UInt32 SW_SHOW = 5;
        public const UInt32 SW_MINIMIZE = 6;
        public const UInt32 SW_SHOWMINNOACTIVE = 7;
        public const UInt32 SW_SHOWNA = 8;
        public const UInt32 SW_RESTORE = 9;

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;

        [DllImport("NotificationDll.dll")]
        public static extern void SetWindowHandle(IntPtr hWnd);

        //from win
        public const int WH_SHELL = 10;

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("User32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public static int MakeLParam(int x, int y) => ((y << 16) | (x & 0xFFFF));

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner

            public Rectangle ToRectangle()
            {
                Rectangle rectangle = new Rectangle(Left, Top, Right - Left, Bottom - Top);
                return rectangle;
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, IntPtr callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("kernel32.dll")]

        public static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableComposition(CompositionAction uCompositionAction);

        [Flags]
        public enum CompositionAction : uint
        {
            /// <summary>
            /// To enable DWM composition
            /// </summary>
            DWM_EC_DISABLECOMPOSITION = 0,
            /// <summary>
            /// To disable composition.
            /// </summary>
            DWM_EC_ENABLECOMPOSITION = 1
        }


        public const int WM_CHAR = 0x0102;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int VK_ENTER = 0x0D;
        public const int VK_TAB = 0x9;
        public const int VK_R_ALT= 0xA5;

        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        [Flags()]
        public enum DeviceContextValues : uint
        {
            /// <summary>DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather
            /// than the client rectangle.</summary>
            Window = 0x00000001,
            /// <summary>DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC
            /// window. Essentially overrides CS_OWNDC and CS_CLASSDC.</summary>
            Cache = 0x00000002,
            /// <summary>DCX_NORESETATTRS: Does not reset the attributes of this DC to the
            /// default attributes when this DC is released.</summary>
            NoResetAttrs = 0x00000004,
            /// <summary>DCX_CLIPCHILDREN: Excludes the visible regions of all child windows
            /// below the window identified by hWnd.</summary>
            ClipChildren = 0x00000008,
            /// <summary>DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows
            /// above the window identified by hWnd.</summary>
            ClipSiblings = 0x00000010,
            /// <summary>DCX_PARENTCLIP: Uses the visible region of the parent window. The
            /// parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is
            /// set to the upper-left corner of the window identified by hWnd.</summary>
            ParentClip = 0x00000020,
            /// <summary>DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded
            /// from the visible region of the returned DC.</summary>
            ExcludeRgn = 0x00000040,
            /// <summary>DCX_INTERSECTRGN: The clipping region identified by hrgnClip is
            /// intersected with the visible region of the returned DC.</summary>
            IntersectRgn = 0x00000080,
            /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
            ExcludeUpdate = 0x00000100,
            /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
            IntersectUpdate = 0x00000200,
            /// <summary>DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate
            /// call in effect that would otherwise exclude this window. Used for drawing during
            /// tracking.</summary>
            LockWindowUpdate = 0x00000400,
            /// <summary>DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to
            /// be completely validated. Using this function with both DCX_INTERSECTUPDATE and
            /// DCX_VALIDATE is identical to using the BeginPaint function.</summary>
            Validate = 0x00200000,
        }

    }
}
