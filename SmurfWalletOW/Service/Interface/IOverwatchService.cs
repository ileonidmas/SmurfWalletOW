using SmurfWalletOW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IOverwatchService
    {
        Task<bool> StartGameAsync(Account account);
    }
}
