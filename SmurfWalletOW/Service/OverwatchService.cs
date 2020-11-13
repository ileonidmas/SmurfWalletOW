using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.Util;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service
{
    public class OverwatchService : IOverwatchService
    {

        private IntPtr whook;
        private IFileService _fileService;
        private IEncryptionService _encryptionService;
        private IRegionService _regionService;
        private IOverwatchInteractionService _overwatchInteractionService;

        public OverwatchService(IEncryptionService encryptionService, IFileService fileService, IRegionService regionService, IOverwatchInteractionService overwatchInteractionService)
        {
            _encryptionService = encryptionService;
            _fileService = fileService;
            _regionService = regionService;
            _overwatchInteractionService = overwatchInteractionService;
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
            var region = _regionService.GetRegionAsync().Result;
            IntPtr wh = IntPtr.Zero;
            if (region != Enums.Region.PTR)
            {
                wh = StartOverwatch(settings.OverwatchPath);
            }
            else
            {
                wh = StartOverwatch(settings.PtrPath, true);
            }

            var finished = InsertCredentials(wh, account, key, settings);
            return finished;
        }

        private IntPtr StartOverwatch(string path, bool ptr = false)
        {
            app = new Process();
            app.StartInfo.FileName = path;
            if (ptr)
                app.StartInfo.Arguments = "--BNetServer=test.actual.battle.net:1119 --cluster=PTR -uid prometheus_test";


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
            //check if full screen
            var isFullScreen = _fileService.IsOverwatchFullscreenAsync().Result;
            Thread.Sleep(2000);
            if (isFullScreen)
            {
                _overwatchInteractionService.AltEnter(app.MainWindowHandle);
            }
            _overwatchInteractionService.WaitForLoginScreen(app.MainWindowHandle, settings.LoadingTime);
            _overwatchInteractionService.ClickWindow(app.MainWindowHandle);
            
            _overwatchInteractionService.EnterKeys(app.MainWindowHandle, account.Email);
            _overwatchInteractionService.PressTab(app.MainWindowHandle);

            IntPtr valuePtr = IntPtr.Zero;
            var pw = _encryptionService.DecryptString(key, account.Password, account.ManualEncryption);
            valuePtr = Marshal.SecureStringToGlobalAllocUnicode(pw);

            for (int i = 0; i < pw.Length; i++)
            {
                _overwatchInteractionService.EnterKeys(app.MainWindowHandle, Convert.ToChar(Marshal.ReadInt16(valuePtr, i * 2)).ToString());
            }
            Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);

            Thread.Sleep(750);
            _overwatchInteractionService.PressEnter(app.MainWindowHandle);

            if (isFullScreen)
            {
                _overwatchInteractionService.AltEnter(app.MainWindowHandle);
            }

            return true;
        }

    }
}
