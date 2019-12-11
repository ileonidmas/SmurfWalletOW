using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service.Interface
{
    public interface IEncryptionService
    {
        SecureString DecryptString(SecureString masterKey, string password, bool manual);
        string EncryptString(SecureString masterKey, SecureString password, bool manual);
    }
}
