using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        private readonly IFileService _fileService;
        private RelayCommand _loadCommand;
        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => Set(ref _settings, value);
        }

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
        public RelayCommand LoadCommand
        {
            get => _loadCommand;
            set => Set(ref _loadCommand, value);
        }
        public MainViewModel(IDialogService dialogService, IFileService fileService)
        {
            _dialogService = dialogService;
            _fileService = fileService;
            _openSettingsCommand = new RelayCommand<object>((parameter) => OpenSettings(parameter));
            _openAboutCommand = new RelayCommand<object>((parameter) => OpenAbout(parameter));
            _loadCommand = new RelayCommand(Load);

        }

        
        private async void OpenSettings(object parameter)
        {
            DialogResult result = _dialogService.ShowDialogSettings(Settings, parameter as Window);
            if (result == DialogResult.Yes)
            {
                await _fileService.SaveSettingsAsync(Settings);
            }
            //load it in case model was changed
            Settings = await _fileService.GetSettingsAsync();
        }

        private async void OpenAbout(object parameter)
        {
            DialogResult result = _dialogService.ShowDialogAbout( parameter as Window);
        }

        private async void Load()
        {
            Settings = await _fileService.GetSettingsAsync();
        }
    }
}
