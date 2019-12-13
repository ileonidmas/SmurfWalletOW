﻿
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Enums;
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

        public DialogResult ShowDialogYesNo(string message, Window owner)
        {            
            DialogViewModelBase vm = new DialogYesNoViewModel(message);
            return ShowDialog(vm, owner);
        }

        public DialogResult ShowDialogAccount(Account account, Window owner)
        {
            DialogAccountViewModel vm = new DialogAccountViewModel(account);
            return ShowDialog(vm, owner);
        }


        public DialogResult ShowDialogEncryptionkey(SecureString key, Window owner)
        {
            DialogEncryptionKeyViewModel vm = new DialogEncryptionKeyViewModel(key);
            return ShowDialog(vm, owner);
        }

        public DialogResult ShowDialogSettings(Settings settings, Window owner)
        {
            DialogSettingsViewModel vm = new DialogSettingsViewModel(settings);
            return ShowDialog(vm, owner);
        }

        public DialogResult ShowDialogAbout(Window owner)
        {
            DialogAboutViewModel vm = new DialogAboutViewModel();
            return ShowDialog(vm, owner);
        }

        private DialogResult ShowDialog(DialogViewModelBase vm, Window owner)
        {
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
