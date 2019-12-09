using SmurfWalletOW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IFileService
    {
        Task<IEnumerable<Account>> GetDefaultAccountsAsync();

        Task<bool> DeleteAccountAsync(Account account);

        Task<bool> AddAccountAsync(Account account);

    }
}
