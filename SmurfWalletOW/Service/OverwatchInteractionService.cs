
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SmurfWalletOW.Service
{
    public class OverwatchInteractionService : IOverwatchInteractionService
    {
        public void ClickWindow(IntPtr wh)
        {            
            Native.RECT rect = new Native.RECT();
            if (!Native.GetWindowRect(new HandleRef(null, wh), out rect))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            Rectangle windowSize = rect.ToRectangle();
            int y = 20 ;
            int x = 20;
            Native.PostMessage(wh, Native.WM_LBUTTONDOWN, 0, Native.MakeLParam(x, y));
            Thread.Sleep(100);
            Native.PostMessage(wh, Native.WM_LBUTTONUP, 0, Native.MakeLParam(x, y));
            Thread.Sleep(100);
        }

       

        private IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        public void WaitForLoginScreen(IntPtr wh, double timeout)
        {
            System.Drawing.Color theColor = System.Drawing.Color.FromArgb(255, 209, 209, 212);
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
                if (count > 1000)
                    loggedIn = !loggedIn;
                if (sw.Elapsed > TimeSpan.FromMilliseconds(timeout * 1000))
                    break;
                Thread.Sleep(1);
            } while (!loggedIn);

            sw.Stop();
        }

        public void AltEnter(IntPtr wh)
        {
            Native.PostMessage(wh, Native.WM_KEYDOWN, Native.VK_R_ALT, 0);//tab
            Native.PostMessage(wh, Native.WM_KEYDOWN, Native.VK_ENTER, 0); // enter
            Native.PostMessage(wh, Native.WM_KEYUP, Native.VK_ENTER, 0); // enter
            Native.PostMessage(wh, Native.WM_KEYUP, Native.VK_R_ALT, 0);//tab
        }

        public void EnterKeys(IntPtr wh, string line)
        {
            string txt = Regex.Replace(line, "[+^%~()]", "{$0}");
            byte[] asciiBytes = Encoding.ASCII.GetBytes(line);
            foreach (byte b in asciiBytes)
            {
                Native.PostMessage(wh, Native.WM_CHAR, b, 0);
            }
        }

        public void PressTab(IntPtr wh)
        {
            Native.PostMessage(wh, Native.WM_KEYDOWN, Native.VK_TAB, 0);
            Native.PostMessage(wh, Native.WM_KEYUP, Native.VK_TAB, 0);
        }
        public void PressEnter(IntPtr wh)
        {

            Native.PostMessage(wh, Native.WM_KEYDOWN, Native.VK_ENTER, 0);
            Native.PostMessage(wh, Native.WM_KEYUP, Native.VK_ENTER, 0);
        }

        private Bitmap GetScreenshot(IntPtr hwnd)
        {
            Native.RECT rect = new Native.RECT();

            if (!Native.GetWindowRect(new HandleRef(null, hwnd), out rect))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            Thread.Sleep(500);

            Rectangle windowSize = rect.ToRectangle();
            Bitmap target = new Bitmap(windowSize.Width, windowSize.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.CopyFromScreen(windowSize.X, windowSize.Y, 0, 0, new System.Drawing.Size(windowSize.Width, windowSize.Height));
            }


            return target;
        }

    }
}
