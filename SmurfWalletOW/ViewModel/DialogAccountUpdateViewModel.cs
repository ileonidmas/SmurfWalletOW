using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Message;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class DialogAccountUpdateViewModel : DialogViewModelBase
    {
        private Account _account;
        private string _accountId;

        private RelayCommand<object[]> _setCommand;

        private RelayCommand<Window> _cancelCommand;

        private readonly IEncryptionService _encryptionService;
        private readonly IFileService _fileService;

        public Account Account
        {
            get => _account;
            set => Set(ref _account, value);
        }
        public string AccountId
        {
            get => _accountId;
            set => Set(ref _accountId, value);
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

        public DialogAccountUpdateViewModel(IEncryptionService encryptionService, IFileService fileService)
        {
            Title = "Account";

            _encryptionService = encryptionService;
            _fileService = fileService;
            _setCommand = new RelayCommand<object[]>((w) => OnSetClicked(w));
            _cancelCommand = new RelayCommand<Window>((w) => OnCancelClicked(w));

            _loadCommand = new RelayCommand(Load);
        }
        private async void Load()
        {
            Account = await _fileService.GetAccountAsync(AccountId);
        }


        private void OnSetClicked(object[] parameters)
        {
            Account.Password = _encryptionService.EncryptString((parameters[0] as PasswordBox).SecurePassword, (parameters[1] as PasswordBox).SecurePassword, Account.ManualEncryption);

            MessengerInstance.Send(new UpdateAccountMessage(Account));

            this.CloseDialogWithResult(parameters[2] as Window, DialogResult.Yes);
        }

        private void OnCancelClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}

