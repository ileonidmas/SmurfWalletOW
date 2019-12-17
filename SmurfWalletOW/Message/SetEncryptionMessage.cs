using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Message
{
    public class SetEncryptionMessage
    {
        private SecureString _key;
        public SecureString Key
        {
            get => _key;
            set => _key = value;
        }
        public SetEncryptionMessage(SecureString key)
        {
            _key = key;
        }
    }
}
