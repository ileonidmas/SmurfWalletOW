using SmurfWalletOW.Enums;
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
        DialogResult ShowDialog(DialogsEnum dialog, Window owner, string message = "");
    }
}
