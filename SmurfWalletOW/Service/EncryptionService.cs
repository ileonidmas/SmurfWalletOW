using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmurfWalletOW.Service
{
    public class EncryptionService : IEncryptionService
    {
        public string EncryptString(SecureString masterKey, SecureString password, bool manual)
        {
            if (!manual)
            {
                var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
                ManagementObjectCollection mbsList = mbs.Get();
                string id = "";
                foreach (ManagementObject mo in mbsList)
                {
                    id = mo["ProcessorId"].ToString();
                    break;
                }
                var secure = new SecureString();
                foreach (char c in id)
                {
                    secure.AppendChar(c);
                }
                masterKey = secure;
            }

            byte[] iv = new byte[16];
            byte[] array;

            //confirm that length is at least 16, otherwise cut

            if (masterKey.Length < 16)
            {
                while (masterKey.Length != 16)
                {
                    masterKey.AppendChar('0');
                }
            }
            else if (masterKey.Length > 16)
            {
                while (masterKey.Length != 16)
                {
                    masterKey.RemoveAt(masterKey.Length - 1);
                }
            }

            using (SecureStringWrapper keyWrapper = new SecureStringWrapper(masterKey))
            {
                using (SecureStringWrapper passwordWrapper = new SecureStringWrapper(password))
                {

                    byte[] keyBytes = keyWrapper.ToByteArray();
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = keyBytes;
                        aes.IV = iv;
                        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                                {
                                    streamWriter.Write(passwordWrapper.ToCharArray());
                                }
                                array = memoryStream.ToArray();
                            }
                        }
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public SecureString DecryptString(SecureString masterKey, string password, bool manual)
        {
            if (!manual)
            {
                var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
                ManagementObjectCollection mbsList = mbs.Get();
                string id = "";
                foreach (ManagementObject mo in mbsList)
                {
                    id = mo["ProcessorId"].ToString();
                    break;
                }
                var secure = new SecureString();
                foreach (char c in id)
                {
                    secure.AppendChar(c);
                }
                masterKey = secure;
            }

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(password);

            if (masterKey.Length < 16)
            {
                while (masterKey.Length != 16)
                {
                    masterKey.AppendChar('0');
                }
            }
            else if (masterKey.Length > 16)
            {
                while (masterKey.Length != 16)
                {
                    masterKey.RemoveAt(masterKey.Length - 1);
                }
            }
            using (SecureStringWrapper wrapper = new SecureStringWrapper(masterKey))
            {

                byte[] keyBytes = wrapper.ToByteArray();
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                var securePassword = new SecureString();
                                while (streamReader.Peek() >= 0)
                                {
                                    securePassword.AppendChar((char)streamReader.Read());
                                }                               
                                
                                return securePassword;
                            }
                        }
                    }
                }
            }
        }       
    }
}
