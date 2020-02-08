using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace SmurfWalletOW.Service
{
    public class OverwatchService : IOverwatchService
    {

        private IntPtr whook;
        private IFileService _fileService;
        private IEncryptionService _encryptionService;

        public OverwatchService(IEncryptionService encryptionService, IFileService fileService)
        {
            _encryptionService = encryptionService;
            _fileService = fileService; 
           
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetWindowHandle(IntPtr handle);

        public Process app;
        public bool Hook()
        {
            //hook
            var apps = Process.GetProcessesByName("Overwatch");
            if (apps.Length == 0)
                return false;
            app = apps[0];
            var wndHandle = app.MainWindowHandle;
            var thread_id = Native.GetWindowThreadProcessId(wndHandle, IntPtr.Zero);
            var dll = Native.LoadLibrary("NotificationDll.dll");
            var shellCallbackPtr = Native.GetProcAddress(dll, "CallWndProc");
            whook = Native.SetWindowsHookEx(Native.WH_SHELL, shellCallbackPtr, dll, thread_id);
            // set window handle
            var handle = Process.GetCurrentProcess().MainWindowHandle;
            Native.SetWindowHandle(handle);
            return true;

        }

        //remember to unhook on exit
        public void UnHook()
        {
            Native.UnhookWindowsHookEx(whook);
            whook = IntPtr.Zero;
        }
        public Task<bool> StartGameAsync(SecureString key, Account account)
        {           
            return Task.Factory.StartNew(() => StartGame(key, account));
        }

        private bool StartGame(SecureString key, Account account)
        {
            var settings = _fileService.GetSettingsAsync().Result; 
            var wh = StartOverwatch(settings.OverwatchPath);
            var finished = InsertCredentials(wh,account,key,settings);

            return finished;
        }     

        private IntPtr StartOverwatch(string path)
        {
            app = new Process();
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

        private bool InsertCredentials(IntPtr wh, Account account, SecureString key, Settings settings)
        {
            Native.SetForegroundWindow(wh);

            Color theColor = Color.FromArgb(255, 209, 209, 212);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool loggedIn = false;
            do
            {
                var img = GetScreenshot(wh);
                int imgW = img.Width;
                int imgH = img.Height;
                int count = 0;
                for (int z = 0; z < imgH; z++)
                {
                    for (int i = 0; i < imgW; i++)
                    {
                        Color pixelColor = img.GetPixel(i, z);
                        if (pixelColor.Equals(theColor))
                            count++;
                    }
                }
                if (count > 100)
                    loggedIn = !loggedIn;
                if (sw.Elapsed > TimeSpan.FromMilliseconds(settings.LoadingTime * 1000))
                    break;
                Thread.Sleep(1);
            } while (!loggedIn);


            sw.Stop();
            SpecialSendKeys(account.Email);
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
                    SpecialSendKeys(Convert.ToChar(Marshal.ReadInt16(valuePtr, i * 2)).ToString());
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

        private void SpecialSendKeys(string line)
        {
            string txt = Regex.Replace(line, "[+^%~()]", "{$0}");
            SendKeys.SendWait(txt);
        }


        private Bitmap GetScreenshot(IntPtr hwnd)
        {
            Native.RECT rect = new Native.RECT();

            if (!Native.SetForegroundWindow(hwnd))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!Native.GetWindowRect(new HandleRef(null, hwnd), out rect))
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
