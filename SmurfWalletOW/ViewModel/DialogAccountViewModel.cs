using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Message;
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

        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
            get { return _loadCommand; }
            set { _loadCommand = value; }
        }

        public DialogAccountViewModel()
        {
            Title = "Account";

            _encryptionService = SimpleIoc.Default.GetInstance<IEncryptionService>();
            _setCommand = new RelayCommand<object[]>((w) => OnSetClicked(w));
            _cancelCommand = new RelayCommand<Window>((w) => OnCancelClicked(w));

            _loadCommand = new RelayCommand(Load);
        }
        private async void Load()
        {
            Account = new Account();
        }


        private void OnSetClicked(object[] parameters)
        {            
            Account.Password = _encryptionService.EncryptString((parameters[0] as PasswordBox).SecurePassword, (parameters[1] as PasswordBox).SecurePassword, Account.ManualEncryption);

            MessengerInstance.Send(new SaveAccountMessage(Account));

            this.CloseDialogWithResult(parameters[2] as Window, DialogResult.Yes);
        }

        private void OnCancelClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}
