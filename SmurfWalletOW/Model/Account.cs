using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Model
{
    public class Account : ModelBase
    {

        private string _displayName;
        private string _email;
        private string _password;
        private bool _manualEncription;
        private string _id;

        public Account()
        {
            _id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get => _id;
            set => Set(ref _id, value);
        }
        
        public string DisplayName
        {
            get => _displayName;
            set => Set(ref _displayName, value);
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
