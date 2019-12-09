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
        SecureString DecryptString(SecureString key, string cipherText, bool manual);
        string EncryptString(SecureString key, SecureString plainText, bool manual);
    }
}
