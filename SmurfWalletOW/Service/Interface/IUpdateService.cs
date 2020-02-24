using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IUpdateService
    {

        Task<bool> UpdateAsync();
        Task<bool> NewVersionAvaialbeAsync();
        Task<Version> GetCurrentVersionAsync();
    }
}
