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
        public Task<string> GetKeyAsync(string key)
        {
            return Task.Factory.StartNew(()=>GetKey(key));
        }
              

        public Task<bool> SetKeyAsync(string key, object value)
        {
            return Task.Factory.StartNew(()=>SetKey(key,value));
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
