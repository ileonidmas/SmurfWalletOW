using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class DialogAccountViewModel : DialogViewModelBase
    {
        private Account _account;

        private RelayCommand<object[]> _setCommand;

        private RelayCommand<Window> _cancelCommand;

        private IEncryptionService _encryptionService;

        public Account Account
        {
            get => _account;
            set => Set(ref _account, value);
        }
        public RelayCommand<object[]> SetCommand
        {
            get { return _setCommand; }
            set { _setCommand = value; }
        }

        public RelayCommand<Window> CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value; }
        }

        public DialogAccountViewModel(Account account)
        {
            Account = account;
            _encryptionService = SimpleIoc.Default.GetInstance<IEncryptionService>();
            _setCommand = new RelayCommand<object[]>((w) => OnSetClicked(w));
            _cancelCommand = new RelayCommand<Window>((w) => OnCancelClicked(w));
        }


        //parameters[0] - password
        //parameter[1] - key
        //parameter[2] - windowOwner
        private void OnSetClicked(object[] parameters)
        {
            SecureString value = (parameters[1] as PasswordBox).SecurePassword;
            Account.Password = _encryptionService.EncryptString((parameters[1] as PasswordBox).SecurePassword, (parameters[0] as PasswordBox).SecurePassword, Account.ManualEncryption);
            //SecureString test = _encryptionService.DecryptString((parameters[1] as PasswordBox).SecurePassword, Account.Password, Account.ManualEncryption);                      
            this.CloseDialogWithResult(parameters[2] as Window, DialogResult.Yes);
        }

        private void OnCancelClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}
