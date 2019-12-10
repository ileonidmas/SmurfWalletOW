using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmurfWalletOW.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly IFileService _fileService;
        private Settings _settings;
        private RelayCommand _loadCommand;
        private RelayCommand _updateOverwatchPathCommand;

        public Settings Settings
        {
            get => _settings;
            set => Set(ref _settings, value);
        }

        public RelayCommand LoadCommand
        {
            get => _loadCommand;
            set => Set(ref _loadCommand, value);
        }
        public RelayCommand UpdateOverwatchCommand
        {
            get => _updateOverwatchPathCommand;
            set => Set(ref _updateOverwatchPathCommand, value);
        }

        public SettingsViewModel(IAppSettingsService appSettigsService, IFileService fileService)
        {
            _appSettingsService = appSettigsService;
            _fileService = fileService;
            _loadCommand = new RelayCommand(Load);
            _updateOverwatchPathCommand = new RelayCommand(UpdateOverwatchPath);


        }

        private async void Load()
        {
            Settings = await _fileService.GetSettingsAsync();
        }

        private async void UpdateOverwatchPath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Exe Files (.exe)|*.exe";

            //Show the dialog to the user
            var result = fileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                string res = fileDialog.FileName;
                Settings.OverwatchPath = res;
                await _fileService.SaveSettingsAsync(Settings);
            }

        }
    }
}
