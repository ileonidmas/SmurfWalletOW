using Newtonsoft.Json;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service
{
    public class FileService : IFileService
    {
        private readonly IAppSettingsService _appSettingsService;

        public Task<List<Account>> GetAccountsAsync()
        {
            return Task.Factory.StartNew(GetAccountList);
        }
     
        public Task<bool> AddAccountAsync(Account account)
        {
            return Task.Factory.StartNew(() => Add(account));
        }

        public Task<bool> DeleteAccountAsync(Account account)
        {
            return Task.Factory.StartNew(() => Delete(account));
        }

        public Task<Settings> GetSettingsAsync()
        {
            return Task.Factory.StartNew(GetSettings);
        }

        public Task<bool> SaveSettingsAsync(Settings settings)
        {
            return Task.Factory.StartNew(() => SaveSettings(settings));
        }

        public FileService(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

      

        private List<Account> GetAccountList()
        {
            string walletPath = GetApplicationFilesPath() + "\\" + _appSettingsService.GetKeyAsync(AppSettingsKeys.AccountsPath).Result;

            if (!File.Exists(walletPath))
                return new List<Account>();
            var line = File.ReadAllText(walletPath);
            List<Account> list = JsonConvert.DeserializeObject<List<Account>>(line);
            return list;
        }

        private bool SaveAccountList(List<Account> list)
        {
            string walletPath = GetApplicationFilesPath() + "\\" + _appSettingsService.GetKeyAsync(AppSettingsKeys.AccountsPath).Result;
            var output = JsonConvert.SerializeObject(list);
            File.WriteAllText(walletPath, output);
            return true;
        }

        private bool Add(Account account)
        {
            var list = GetAccountList();
            list.Add(account);
            return SaveAccountList(list);
        }

        private bool Delete(Account account)
        {
            var list = GetAccountList();
            list.RemoveAll((a) => a.Id == account.Id);
            return SaveAccountList(list);
        }

        private Settings GetSettings()
        {
            string settingsPath = GetApplicationFilesPath() + "\\" + _appSettingsService.GetKeyAsync(AppSettingsKeys.SettingsPath).Result;
            Settings settings;
            if (!File.Exists(settingsPath))
            {
                settings = CreateDefaultSettings();
                SaveSettings(settings);
                return settings;
            }
            var line = File.ReadAllText(settingsPath);
            settings = JsonConvert.DeserializeObject<Settings>(line);
            return settings;
        }

        private bool SaveSettings(Settings settings)
        {
            string settingsPath = GetApplicationFilesPath() + "\\" + _appSettingsService.GetKeyAsync(AppSettingsKeys.SettingsPath).Result;
            var output = JsonConvert.SerializeObject(settings);
            File.WriteAllText(settingsPath, output);
            return true;
        }

        private Settings CreateDefaultSettings()
        {
            Settings settings = new Settings();
            settings.OverwatchPath = _appSettingsService.GetKeyAsync(AppSettingsKeys.DefaultOverwatchPath).Result;
            settings.LoadingTime = Convert.ToInt32(_appSettingsService.GetKeyAsync(AppSettingsKeys.DefaultLoadingTime).Result);
            return settings;
        }

        private string GetApplicationFilesPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + _appSettingsService.GetKeyAsync(AppSettingsKeys.ApplicationFilesPath).Result;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

    }
}
