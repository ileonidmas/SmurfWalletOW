using SmurfWalletOW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.ViewModel
{
    public class AccountItemViewModel
    {
        private Account _account;

        public Account Account => _account;


        public AccountItemViewModel(Account account)
        {
            _account = account;
        }

    }
}
