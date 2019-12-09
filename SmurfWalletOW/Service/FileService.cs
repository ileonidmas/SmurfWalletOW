using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service
{
    public class FileService : IFileService
    {
        public Task<IEnumerable<Account>> GetDefaultAccountsAsync()
        {
            return Task.Factory.StartNew(GetList);
        }


        public Task<bool> AddAccountAsync(Account account)
        {
            return Task.Factory.StartNew(() => Delete(account));
        }

        public Task<bool> DeleteAccountAsync(Account account)
        {

            return Task.Factory.StartNew(()=> Delete(account));
        }

        private bool Add(Account account)
        {
            return true;
        }

        private bool Delete(Account account)
        {
            return true;
        }

        private IEnumerable<Account> GetList()
        {
            List<Account> list = new List<Account>();
            list.Add(new Account("random", "123456!"));
            list.Add(new Account("random2", "123456!"));
            return list;
        }
    }
}
