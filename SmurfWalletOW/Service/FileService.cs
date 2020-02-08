using Newtonsoft.Json;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        public Task<bool> UpdateAccountAsync(Account account)
        {
            return Task.Factory.StartNew(() => Update(account));
        }

        public Task<Account> GetAccountAsync(string id)
        {
            return Task.Factory.StartNew(() => Get(id));
        }

        public Task<Settings> GetSettingsAsync()
        {
            return Task.Factory.StartNew(GetSettings);
        }

        public Task<bool> SaveSettingsAsync(Settings settings)
        {
            return Task.Factory.StartNew(() => SaveSettings(settings));
        }

        public Task<bool> IsOverwatchFullscreenAsync()
        {
            return Task.Factory.StartNew(() => IsOverwatchFullscreen());
        }

        //public Task<bool> SetOverwatchSettingsToWindowedAsync()
        //{
        //    return Task.Factory.StartNew(SetOverwatchSettingsToWindowed);
        //}

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

        private bool Update(Account account)
        {
            var list = GetAccountList();
            var index = list.IndexOf(list.Where(x => x.Id == account.Id).First());
            list.RemoveAt(index);
            list.Insert(index, account);
            return SaveAccountList(list);

        }
        private Account Get(string id)
        {
            var list = GetAccountList();
            return list.Where(x => x.Id == id).First();

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

        private bool IsOverwatchFullscreen(){
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + @"Documents\Overwatch\Settings\Settings_v0.ini";
            if (!File.Exists(path))
                return false;
            var owSettings = File.ReadAllLines(path);
            int startIndex = 0, stopIndex = 0;
            bool fsw = false, fswe = false, wm = false, reachedRender = false;
            for (int i = 0; i < owSettings.Length; i++)
            {
                if (!reachedRender)
                {
                    if (owSettings[i] == "[Render.13]")
                        reachedRender = true;
                    continue;
                }

                if (owSettings[i] == "")
                {
                    stopIndex = i;
                    break;
                }
            }

            if (!reachedRender)
                return false;

            if (owSettings[stopIndex - 1] == "WindowMode = \"0\"")
                return true;
            return false;

        }


        private bool SetOverwatchSettingsToWindowed()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + @"Documents\Overwatch\Settings\Settings_v0.ini";
            if (!File.Exists(path))
                return false;
            var owSettings = File.ReadAllLines(path);
            int startIndex = 0, stopIndex = 0;
            bool fsw = false, fswe = false, wm = false, reachedRender = false;
            for (int i = 0; i < owSettings.Length; i++)
            {
                if (!reachedRender)
                {
                    if (owSettings[i] == "[Render.13]")
                        reachedRender = true;
                    continue;
                }

                if (owSettings[i] == "")
                {
                    stopIndex = i;
                    break;
                }                               
            }            

            if (!reachedRender)
                return false;

            if (owSettings[stopIndex - 1].Split(' ')[0] == "WindowMode")
                owSettings[stopIndex - 1] = "WindowMode = \"2\"";
            else
            {
                var list = owSettings.ToList();
                list.Insert(stopIndex, "WindowMode = \"2\"");
                owSettings = list.ToArray();
            }

            File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + @"Documents\Overwatch\Settings\Settings_v0.ini", owSettings);
            return true;
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
