using SmurfWalletOW.Model;
using SmurfWalletOW.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.Service.Interface
{
    public interface IDialogService
    {
        DialogResult ShowDialogYesNo(string message, Window owner);
        DialogResult ShowDialogAccount(Account account, Window owner);
        DialogResult ShowDialogEncryptionkey(SecureString key,Window owner);
        DialogResult ShowDialogSettings(Settings settings, Window owner);
    }
}
