using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmurfWalletOW.Service
{
    public class OverwatchService : IOverwatchService
    {

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

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


        private IFileService _fileService;
        private IEncryptionService _encryptionService;
        public OverwatchService(IEncryptionService encryptionService, IFileService fileService)
        {
            _encryptionService = encryptionService;
            _fileService = fileService;
        }

        public Task<bool> StartGameAsync(SecureString key, Account account)
        {
            return Task.Factory.StartNew(() => StartGame(key, account));
        }

        private bool StartGame(SecureString key, Account account)
        {

            var settingsAreSet = _fileService.SetOverwatchSettingsToWindowedAsync().Result;

            var settings = _fileService.GetSettingsAsync().Result; 

            var wh = StartOverwatch(settings.OverwatchPath);

            var finished = InsertCredentials(wh,account,key,settings);

            //maximize after done
            SendKeys.SendWait("%{ENTER}");

            return finished;
        }

        private IntPtr StartOverwatch(string path)
        {
            Process app = new Process();
            app.StartInfo.FileName = path;
            app.Start();
            app.PriorityClass = ProcessPriorityClass.High;


            while (app.MainWindowTitle != "Overwatch")
            {
                Thread.Sleep(10);
                app.Refresh();
            }
            return app.MainWindowHandle;
        }

        private bool InsertCredentials(IntPtr wh,Account account,SecureString key, Settings settings)
        {
            SetForegroundWindow(wh);

            MoveWindow(wh, 0, 0, 720, 436, false);

            Color theColor = Color.FromArgb(255, 209, 209, 212);
            Color pixel1, pixel2;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                var img = GetScreenshot(wh);
                pixel1 = img.GetPixel(319, 300);
                pixel2 = img.GetPixel(400, 300);
                if (sw.Elapsed > TimeSpan.FromMilliseconds(settings.LoadingTime * 1000))
                    break;
            } while (!pixel1.Equals(theColor) || !pixel2.Equals(theColor));


            sw.Stop();

            SendKeys.SendWait(account.Email);
            Thread.Sleep(750);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(750);
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                var pw = _encryptionService.DecryptString(key, account.Password, account.ManualEncryption);
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(pw);

                for (int i = 0; i < pw.Length; i++)
                {
                    SendKeys.SendWait(Convert.ToChar(Marshal.ReadInt16(valuePtr, i * 2)).ToString());
                }
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }

            Thread.Sleep(750);
            SendKeys.SendWait("{ENTER}");
            return true;
        }


        private Bitmap GetScreenshot(IntPtr hwnd)
        {
            RECT rect = new RECT();

            if (!SetForegroundWindow(hwnd))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!GetWindowRect(new HandleRef(null, hwnd), out rect))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            Thread.Sleep(500);

            Rectangle windowSize = rect.ToRectangle();
            Bitmap target = new Bitmap(windowSize.Width, windowSize.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.CopyFromScreen(windowSize.X, windowSize.Y, 0, 0, new Size(windowSize.Width, windowSize.Height));
            }

            return target;
        }

        
    }
}
