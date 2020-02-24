using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmurfWalletOW.Service
{
    public class UpdateService : IUpdateService
    {

        public Task<bool> NewVersionAvaialbeAsync()
        {
            return Task.Factory.StartNew(NewVersionAvailable);
        }

        public Task<bool> UpdateAsync()
        {
            return Task.Factory.StartNew(Update);
        }

        public Task<Version> GetCurrentVersionAsync()
        {
            return Task.Factory.StartNew(GetCurrentVersion);
        }

        private Version GetCurrentVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version;
        }


        private bool NewVersionAvailable()
        {
            if (Directory.Exists("Update"))
            {
                Directory.Delete("Update", true);
            }

            DirectoryInfo di = Directory.CreateDirectory("Update");
            using (var client = new WebClient())
            {
                var temp = client.DownloadString("https://raw.githubusercontent.com/ileonidmas/SmurfWalletOW/master/SmurfWalletOW/Properties/AssemblyInfo.cs");
                var versionString = temp.Substring(temp.Length - 11, 7);
                Version newVersion = new Version(versionString);
                var currrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                var val =  newVersion.CompareTo(currrentVersion); // 1 for avaiable

                if (val == 1)
                    return true;
            }
            
            return false;
        }
        
        private bool Update()
        {
            if (Directory.Exists("Update"))
            {
                Directory.Delete("Update", true);
            }

            DirectoryInfo di = Directory.CreateDirectory("Update");
            using (var client = new WebClient())
            {

                client.DownloadFile(new Uri("https://raw.githubusercontent.com/ileonidmas/SmurfWalletOW/master/Installer/SmurfWalletOWInstaller.msi"), "Update\\SmurfWalletOWInstaller.msi");
            }
            Process.Start("Update\\SmurfWalletOWInstaller.msi");

            return true;
        }
    }
}
