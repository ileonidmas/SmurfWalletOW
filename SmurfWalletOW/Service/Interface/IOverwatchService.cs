using SmurfWalletOW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IOverwatchService
    {
        Task<bool> StartGameAsync(SecureString key,Account account);

        void Hook();
        void UnHook();
    }
}
