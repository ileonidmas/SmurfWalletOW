using Microsoft.Win32;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service
{
    public class RegionService : IRegionService
    {

        public Task<Region> GetRegionAsync()
        {
            return Task.Factory.StartNew(GetRegion);
        }

        public Task<bool> SetRegionAsync(Region region)
        {
            return Task.Factory.StartNew(()=> SetRegion(region));
        }

        public Region GetRegion()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\Battle.net\Launch Options\Pro");
                if (key != null)
                {
                    var region = key.GetValue("Region").ToString();
                    switch (region)
                    {
                        case "US":
                            return Region.US;
                        case "EU":
                            return Region.EU;
                        case "KR":
                            return Region.KR;
                        case "XX":
                            return Region.PTR;
                    }
                }
            }
            catch (Exception ex) 
            {
            }
            return Region.US;
        }

        public bool SetRegion(Region region)
        {
            string regionString;
            if (region != Region.PTR)
                regionString = region.ToString();
            else
                regionString = "XX";
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\Battle.net\Launch Options\Pro", true);
                if (key != null)
                {
                    key.SetValue("REGION", regionString);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
