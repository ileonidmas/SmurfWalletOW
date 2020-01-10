using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SmurfWalletOW.Util
{
    public static class Native
    {
        public const int WH_SHELL = 10;

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);
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
    }
}
