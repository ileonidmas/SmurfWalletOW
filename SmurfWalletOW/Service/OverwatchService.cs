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


        private IEncryptionService _encryptionService;
        public OverwatchService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public Task<bool> StartGameAsync(Account account)
        {
            return Task.Factory.StartNew(()=>StartGame(account));
        }

        private bool StartGame(Account account)
        {
            Process app = new Process();
            app.StartInfo.FileName = @"C:\Program Files (x86)\Overwatch\_retail_\Overwatch.exe";
            app.Start();
            app.PriorityClass = ProcessPriorityClass.High;

            Thread.Sleep(10000);
            SendKeys.SendWait(account.Email);
            Thread.Sleep(3000);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(3000);
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                var test = _encryptionService.DecryptString(null, account.Password, account.ManualEncryption);
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(test);
                for (int i = 0; i < test.Length; i++)
                {
                    SendKeys.SendWait(Convert.ToChar(Marshal.ReadInt16(valuePtr, i * 2)).ToString());
                }
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }

            Thread.Sleep(3000);
            SendKeys.SendWait("{TAB}");

            Thread.Sleep(3000);

            SendKeys.SendWait("{ENTER}");
            return true;
        }
    }
}
