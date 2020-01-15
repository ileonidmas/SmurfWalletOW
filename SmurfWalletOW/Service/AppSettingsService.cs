using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmurfWalletOW.Service
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly IEncryptionService _encryptionService;

        public AppSettingsService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public Task<string> GetKeyAsync(string key)
        {
            return Task.Run(()=>GetKey(key));
        }              

        public Task<bool> SetKeyAsync(string key, object value)
        {
            return Task.Run(()=>SetKey(key,value));
        }

        public Task<string> GetDiscordTokenAsync()
        {
            return Task.Run(GetDiscordToken);
        }

        private string GetDiscordToken()
        {
            return _encryptionService.DecryptString("43CH90zIyWDMrOYj5IAD", ConfigurationManager.ConnectionStrings["DiscordToken"].ConnectionString);
        }

        private string GetKey(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
        private bool SetKey(string key, object value)
        {
            ConfigurationManager.AppSettings.Set(key, value.ToString());
            return true;
        }               
    }
}
