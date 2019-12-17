using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IDialogService _dialogService;

        private RelayCommand<object> _openSettingsCommand;
        public RelayCommand<object> OpenSettingsCommand
        {
            get => _openSettingsCommand;
            set => Set(ref _openSettingsCommand, value);
        }

        private RelayCommand<object> _openAboutCommand;
        public RelayCommand<object> OpenAboutCommand
        {
            get => _openAboutCommand;
            set => Set(ref _openAboutCommand, value);
        }

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _openSettingsCommand = new RelayCommand<object>((parameter) => OpenSettings(parameter));
            _openAboutCommand = new RelayCommand<object>((parameter) => OpenAbout(parameter));

        }

        private void OpenSettings(object parameter)
        {
            _dialogService.ShowDialog(DialogsEnum.DialogSettings, parameter as Window);
        }


        private void OpenAbout(object parameter)
        {
            _dialogService.ShowDialog(DialogsEnum.DialogsAbout, parameter as Window);
        }

      
    }
}
