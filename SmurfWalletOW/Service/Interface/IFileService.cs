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
        Task<IEnumerable<Account>> GetDefaulAccounts();

        Task<bool> DeleteAccount(Account account);

        Task<bool> AddAccount(Account account);

    }
}
