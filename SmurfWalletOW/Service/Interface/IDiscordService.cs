using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IDiscordService
    {
        Task Start();
        Task Stop();
        Task<bool> DoesUserExists(string username);
        Task SendMessageAsync(string username,string message);
        void SetSendMessage(bool value);
        bool GetSendMessageFlag();
    }
}
