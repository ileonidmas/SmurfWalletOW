using Discord;
using Discord.WebSocket;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service
{
    public class DiscordService : IDiscordService
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly DiscordSocketClient _client = new DiscordSocketClient();
        private bool sendMessageFlag = false;
        private bool discordReadyFlag = false;

        public DiscordService(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }


        public void SetSendMessage(bool value)
        {
            sendMessageFlag = value;
        }

        public bool GetSendMessageFlag()
        {
            return sendMessageFlag;
        }

        public async Task Stop()
        {
            _client.Ready -= IsReady;
            await _client.StopAsync();
        }
        public async Task Start()
        {
            discordReadyFlag = false;
            sendMessageFlag = false;
            _client.Ready += IsReady;
            var token = await _appSettingsService.GetDiscordTokenAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Run(IsReadyAsync);
        }

        public async Task<bool> DoesUserExists(string username)
        {
            var exists = await Task.Run(()=>DoesUserExistMethod(username));
            return exists;
        }


        private bool DoesUserExistMethod(string username)
        {
            var split = SplitUsername(username);
            if (split.Length != 2)
                return false;
            var user = _client.GetUser(split[0], split[1]);            
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public async Task SendMessageAsync(string username,string message)
        {
            string[] split = SplitUsername(username);
            if (split.Length != 2)
                return;
            var user = _client.GetUser(split[0], split[1]);
            await user.SendMessageAsync(message);
        }

        
        private void IsReadyAsync()
        {
            while (!discordReadyFlag)
            {
                Thread.Sleep(10);
            }
        }
        private Task IsReady()
        {
            discordReadyFlag = true;
            return Task.CompletedTask;
        }
        private string[] SplitUsername(string username)
        {
           return username.Split('#');
        }
        
    }
}
