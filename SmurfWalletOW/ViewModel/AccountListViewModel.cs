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
        private readonly IFileService _fileSerivce;
        private readonly IDialogService _dialogService;

        private ObservableCollection<AccountItemViewModel> _accountList;
        public ObservableCollection<AccountItemViewModel> AccountList
        {
            get => _accountList;
            set => Set(ref _accountList, value);
        }

        private RelayCommand<object> _openDialogCommand = null;
        public RelayCommand<object> OpenDialogCommand
        {
            get => _openDialogCommand;
            set => Set(ref _openDialogCommand, value);
        }

        public AccountListViewModel(IFileService fileService, IDialogService dialogService)
        {
            _fileSerivce = fileService;
            _dialogService = dialogService;
            _openDialogCommand = new RelayCommand<object>((parameter) =>OpenDialog(parameter));
            AccountList = new ObservableCollection<AccountItemViewModel>();
            //temp
            Load();
        }

        private void OpenDialog(object parameter)
        {
            var account = new Account();
            DialogResult result = _dialogService.ShowDialogAccount(account, parameter as Window);
            MessageBox.Show(result.ToString());
        }

        private async void Load()
        {
            var list = await _fileSerivce.GetDefaultAccountsAsync();
            foreach(var account in list)
            {
                AccountList.Add(new AccountItemViewModel(account));
            }
        }

        private async void Add()
        {

            var account = new Account("", "");
            await _fileSerivce.AddAccountAsync(account);
            AccountList.Add(new AccountItemViewModel(account));
        }

        private async void Delete(AccountItemViewModel accountViewModel)
        {
            await _fileSerivce.DeleteAccountAsync(accountViewModel.Account);
            AccountList.Remove(accountViewModel);
        }
    }
   
}
