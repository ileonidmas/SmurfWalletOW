using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class AccountListViewModel : ViewModelBase
    {
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IOverwatchService _overwatchService;

        private RelayCommand<object[]> _deleteAccountCommand;
        private RelayCommand<object> _playCommand;
        private RelayCommand<object> _addAccountCommand;
        private ObservableCollection<AccountItemViewModel> _accountList;


        public ObservableCollection<AccountItemViewModel> AccountList
        {
            get => _accountList;
            set => Set(ref _accountList, value);
        }
        public RelayCommand<object> AddAccountCommand
        {
            get => _addAccountCommand;
            set => Set(ref _addAccountCommand, value);
        }
        public RelayCommand<object> PlayCommand
        {
            get => _playCommand;
            set => Set(ref _playCommand, value);
        }
        public RelayCommand<object[]> DeleteAccountCommand
        {
            get => _deleteAccountCommand;
            set => Set(ref _deleteAccountCommand, value);
        }

        public AccountListViewModel(IFileService fileService, IDialogService dialogService, IOverwatchService overwatchService)
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _overwatchService = overwatchService;

            _addAccountCommand = new RelayCommand<object>((parameter) => AddAccount(parameter));
            _deleteAccountCommand = new RelayCommand<object[]>((parameter) => DeleteAccount(parameter));
            _playCommand = new RelayCommand<object>((parameter) => Play(parameter));

            AccountList = new ObservableCollection<AccountItemViewModel>();
            //temp
            Load();
        }



        private async void Load()
        {
            var list = await _fileService.GetDefaultAccountsAsync();
            foreach(var account in list)
            {
                AccountList.Add(new AccountItemViewModel(account));
            }
        }

        private async void AddAccount(object parameter)
        {
            var account = new Account();

            DialogResult result = _dialogService.ShowDialogAccount(account, parameter as Window);
            if (result == DialogResult.Yes)
            {
                if (await _fileService.AddAccountAsync(account)) { 
                    AccountList.Add(new AccountItemViewModel(account));
                }
                else
                {
                    //failed to save
                }
            }

        }

        private async void DeleteAccount(object[] parameters)
        {
            var accountViewModel = parameters[0] as AccountItemViewModel;
            DialogResult result = _dialogService.ShowDialogYesNo("Are you sure you want to delete this entry?", parameters[1] as Window);
            if (result == DialogResult.Yes)
            {
                if (await _fileService.DeleteAccountAsync(accountViewModel.Account))
                {
                    AccountList.Remove(accountViewModel);
                }
                else
                {
                    //failed to delete
                }
            }
           
           
        }

        private async void Play(object parameter)
        {
            await _overwatchService.StartGameAsync((parameter as AccountItemViewModel).Account);
            //SecureString test = _encryptionService.DecryptString((parameters[1] as PasswordBox).SecurePassword, Account.Password, Account.ManualEncryption);               
        }
    }
   
}
