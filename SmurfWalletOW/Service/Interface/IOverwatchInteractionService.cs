using System;

namespace SmurfWalletOW.Service.Interface
{
    public interface IOverwatchInteractionService
    {
        void AltEnter(IntPtr wh);
        void EnterKeys(IntPtr wh, string line);
        void PressTab(IntPtr wh);
        void PressEnter(IntPtr wh);
        void WaitForLoginScreen(IntPtr wh, double timeout);
    }
}
