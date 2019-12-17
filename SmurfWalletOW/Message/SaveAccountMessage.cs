using SmurfWalletOW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Message
{
    public class SaveAccountMessage
    {
        private Account _account;
        public Account Account
        {
            get => _account;
            set => _account = value;
        }
        public SaveAccountMessage(Account account)
        {
            _account = account;
        }
    }
}
