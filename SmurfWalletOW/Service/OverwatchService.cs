using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        private IFileService _fileService;
        private IEncryptionService _encryptionService;
        public OverwatchService(IEncryptionService encryptionService, IFileService fileService)
        {
            _encryptionService = encryptionService;
            _fileService = fileService;
        }

        public Task<bool> StartGameAsync(SecureString key, Account account)
        {
            return Task.Factory.StartNew(()=>StartGame(key, account));
        }

        private bool StartGame(SecureString key, Account account)
        {
            Process app = new Process();
            var settings = _fileService.GetSettingsAsync().Result;
            var path = settings.OverwatchPath;
            var loadingTime = settings.LoadingTime * 1000;
            app.StartInfo.FileName = path;
            app.Start();
            app.PriorityClass = ProcessPriorityClass.High;


            while (app.MainWindowTitle != "Overwatch")
            {
                Thread.Sleep(10);
                app.Refresh();
            }

            var wh = app.MainWindowHandle;
            SetForegroundWindow(wh);

            Thread.Sleep(loadingTime);
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
    }
}
