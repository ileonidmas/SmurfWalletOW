﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Message;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
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
        private RelayCommand<object[]> _updateAccountCommand;
        private RelayCommand<object[]> _playCommand;
        private RelayCommand<object> _addAccountCommand;

        private RelayCommand _loadCommand;
        private ObservableCollection<AccountViewModel> _accountList = new ObservableCollection<AccountViewModel>();


        public ObservableCollection<AccountViewModel> AccountList
        {
            get => _accountList;
            set => Set(ref _accountList, value);
        }
        public RelayCommand LoadCommand
        {
            get => _loadCommand;
            set => Set(ref _loadCommand, value);
        }
        public RelayCommand<object> AddAccountCommand
        {
            get => _addAccountCommand;
            set => Set(ref _addAccountCommand, value);
        }
        public RelayCommand<object[]> PlayCommand
        {
            get => _playCommand;
            set => Set(ref _playCommand, value);
        }
        public RelayCommand<object[]> DeleteAccountCommand
        {
            get => _deleteAccountCommand;
            set => Set(ref _deleteAccountCommand, value);
        }
        public RelayCommand<object[]> UpdateAccountCommand
        {
            get => _updateAccountCommand;
            set => Set(ref _updateAccountCommand, value);
        }

        public AccountListViewModel(IFileService fileService, IDialogService dialogService, IOverwatchService overwatchService)
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _overwatchService = overwatchService;

            _addAccountCommand = new RelayCommand<object>((parameter) => AddAccount(parameter));
            _deleteAccountCommand = new RelayCommand<object[]>((parameters) => DeleteAccount(parameters));
            _updateAccountCommand = new RelayCommand<object[]>((parameters) => UpdateAccount(parameters));
            _playCommand = new RelayCommand<object[]>((parameters) => Play(parameters));
            _loadCommand = new RelayCommand(Load);

            MessengerInstance.Register<SaveAccountMessage>(this, SaveAccount);
            MessengerInstance.Register<SetEncryptionMessage>(this, SetEncryption);
            MessengerInstance.Register<UpdateAccountMessage>(this, Update);
        }

        private async void Load()
        {
            var list = await _fileService.GetAccountsAsync();

            foreach (var account in list)
            {

                var accountVm = SimpleIoc.Default.GetInstance<AccountViewModel>(account.Id);
                accountVm.Account = account;
                accountVm.LoadCommand.Execute("");                
                AccountList.Add(accountVm);
            }
        }

        private void AddAccount(object parameter)
        {
            _dialogService.ShowDialog(DialogsEnum.DialogAccountView,parameter as Window);
        }
        private void UpdateAccount(object[] parameter)
        {
            _dialogService.ShowDialog(DialogsEnum.DialogAccountUpdateView, parameter[1] as Window, (parameter[0] as AccountViewModel).Account.Id);
        }

        private async void SaveAccount(SaveAccountMessage message)
        {
            if (await _fileService.AddAccountAsync(message.Account))
            {
                var accountVm = SimpleIoc.Default.GetInstance<AccountViewModel>(message.Account.Id);
                accountVm.Account = message.Account;
                accountVm.LoadCommand.Execute("");
                AccountList.Add(accountVm);
            }
            else
            {
                //failed to save
            }
        }

        private async void Update(UpdateAccountMessage message)
        {
            if (await _fileService.UpdateAccountAsync(message.Account))
            {
                var index = AccountList.IndexOf(AccountList.Where(x => x.Account.Id == message.Account.Id).First());
                AccountList.RemoveAt(index);
                var accountVm = SimpleIoc.Default.GetInstance<AccountViewModel>(message.Account.Id);
                accountVm.Account = message.Account;
                accountVm.LoadCommand.Execute("");
                AccountList.Insert(index, accountVm);
            }
            else
            {
                //failed to update
            }
        }

        private async void DeleteAccount(object[] parameters)
        {
            var account = parameters[0] as AccountViewModel;
            DialogResult result = _dialogService.ShowDialog(DialogsEnum.DialogYesNo, parameters[1] as Window, "Are you sure you want to delete this entry?");
            if (result == DialogResult.Yes)
            {
                if (await _fileService.DeleteAccountAsync(account.Account))
                {
                    AccountList.Remove(account);
                }
                else
                {
                    //failed to delete
                }
            }
        }

        private async void Play(object[] parameters)
        {
            var account = parameters[0] as AccountViewModel;
            if (account.Account.ManualEncryption)
            {
                _selectedAccount = account.Account;
                DialogResult result = _dialogService.ShowDialog(DialogsEnum.DialogEncryptionKey, parameters[1] as Window);                
            }
            else
            {
                await _overwatchService.StartGameAsync(null, account.Account);
            }

            var settings = await _fileService.GetSettingsAsync();
            if (settings.ExitAfterLogin)
                Application.Current.Shutdown();

        }
        private Account _selectedAccount;

        private async void SetEncryption(SetEncryptionMessage message)
        {
            await _overwatchService.StartGameAsync(message.Key, _selectedAccount);
        }
    }
   
}
