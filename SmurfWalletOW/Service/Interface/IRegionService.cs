using SmurfWalletOW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IRegionService
    {
        Task<Region> GetRegionAsync();
        Task<bool> SetRegionAsync(Region region);

    }
}
