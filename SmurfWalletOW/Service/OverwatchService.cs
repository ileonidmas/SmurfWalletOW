﻿using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
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
            if(apps.Length == 0)
                return false;
            app = apps[0];
            var wndHandle = app.MainWindowHandle;
            var thread_id = Native.GetWindowThreadProcessId(wndHandle, IntPtr.Zero);
            var dll = Native.LoadLibrary("Shell.dll");
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


            var settingsAreGood = _fileService.SetOverwatchSettingsToWindowedAsync().Result;

            var settings = _fileService.GetSettingsAsync().Result; 
            var wh = StartOverwatch(settings.OverwatchPath);
            var finished = InsertCredentials(wh,account,key,settings,settingsAreGood);
            //maximize after done
            if (settingsAreGood)
                SendKeys.SendWait("%{ENTER}");

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

        private bool InsertCredentials(IntPtr wh,Account account,SecureString key, Settings settings,bool settingsState)
        {
            Native.SetForegroundWindow(wh);


            if (settingsState) { 
            Native.MoveWindow(wh, 0, 0, 720, 436, false);

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
            }
            else
            {
                Thread.Sleep(Convert.ToInt32(Math.Round(settings.LoadingTime * 1000)));
            }
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
