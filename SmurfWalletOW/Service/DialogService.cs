
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Factory.Interface;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.View;
using SmurfWalletOW.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.Service
{
    public class DialogService : IDialogService
    {

        private readonly IDialogFactory _dialogFactory;
        public DialogService(IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;
        }

        public DialogResult ShowDialog(DialogsEnum dialog, Window owner,string message)
        {
            var vm = _dialogFactory.Get(dialog, message);
            DialogWindow win = new DialogWindow();
            if (owner != null)
                win.Owner = owner;
            win.DataContext = vm;
            win.ShowDialog();
            DialogResult result = (win.DataContext as DialogViewModelBase).UserDialogResult;
            return result;
        }


    }
}
