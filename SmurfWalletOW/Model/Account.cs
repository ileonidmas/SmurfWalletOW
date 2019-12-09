using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Model
{
    public class Account : ModelBase
    {
        private string _email;
        private string _password;
        private bool _manualEncription;

        public Account()
        {
            
        }
        public Account(string email,string password)
        {
            _password = password;
            _email = email;
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }


        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public bool ManualEncryption
        {
            get => _manualEncription;
            set => Set(ref _manualEncription, value);
        }
    }
}
