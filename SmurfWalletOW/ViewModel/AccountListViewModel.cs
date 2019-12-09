using GalaSoft.MvvmLight;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.ViewModel
{
    public class AccountListViewModel : ViewModelBase
    {
        private readonly IFileService _fileSerivce;
        private ObservableCollection<AccountItemViewModel> _accountList;

        public ObservableCollection<AccountItemViewModel> AccountList
        {
            get => _accountList;
            set => Set(ref _accountList, value);
        }


        public AccountListViewModel(IFileService fileService)
        {
            _fileSerivce = fileService;

            AccountList = new ObservableCollection<AccountItemViewModel>();
            //temp
            Load();
        }

        private async void Load()
        {
            var list = await _fileSerivce.GetDefaulAccounts();
            foreach(var account in list)
            {
                AccountList.Add(new AccountItemViewModel(account));
            }
        }

        private async void Add()
        {

            var account = new Account("", "");
            await _fileSerivce.AddAccount(account);
            AccountList.Add(new AccountItemViewModel(account));
        }

        private async void Delete(AccountItemViewModel accountViewModel)
        {
            await _fileSerivce.DeleteAccount(accountViewModel.Account);
            AccountList.Remove(accountViewModel);
        }
    }
}
