using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Message;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SmurfWalletOW.ViewModel
{
    public class DialogSettingsViewModel : DialogViewModelBase
    {

        private readonly IFileService _fileService;


        private Settings _settings;
        private RelayCommand _updateOverwatchPathCommand;
        private RelayCommand _updatePtrPathCommand;
        private RelayCommand<Uri> _redirectCommand;
        private RelayCommand<Window> _saveCommand;
        private RelayCommand<Window> _cancelCommand;

        public RelayCommand<Uri> RedirectCommand
        {
            get => _redirectCommand;
            set => Set(ref _redirectCommand, value);
        }

        public Settings Settings
        {
            get => _settings;
            set => Set(ref _settings, value);
        }

        public RelayCommand<Window> SaveCommand
        {
            get { return _saveCommand; }
            set { _saveCommand = value; }
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

        public RelayCommand UpdateOverwatchCommand
        {
            get => _updateOverwatchPathCommand;
            set => Set(ref _updateOverwatchPathCommand, value);
        }
        public RelayCommand UpdatePtrPathCommand
        {
            get => _updatePtrPathCommand;
            set => Set(ref _updatePtrPathCommand, value);
        }

        public DialogSettingsViewModel(IFileService fileService)
        {
            Title = "Settings";
            _fileService = fileService;

            _updateOverwatchPathCommand = new RelayCommand(UpdateOverwatchPath); 
            _updatePtrPathCommand = new RelayCommand(UpdatePtrPath);
            _saveCommand = new RelayCommand<Window>((parameter)=>SaveSettings(parameter));
            _cancelCommand = new RelayCommand<Window>((parameter) => Cancel(parameter)); 
            _loadCommand = new RelayCommand(Load); 
            _redirectCommand = new RelayCommand<Uri>((parameter) => Redirect(parameter));
        }
        private void Redirect(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
        private async void SaveSettings(object parameter)
        {
            await _fileService.SaveSettingsAsync(Settings);
            CloseDialogWithResult(parameter as Window, Enums.DialogResults.DialogResult.Yes);
        }
        private void Cancel(Window parameter)
        {
            CloseDialogWithResult(parameter as Window, Enums.DialogResults.DialogResult.No);
        }

        private async void Load()
        {
            Settings = await _fileService.GetSettingsAsync();
        }


        private void UpdateOverwatchPath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Exe Files (.exe)|*.exe";

            //Show the dialog to the user
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string res = fileDialog.FileName;
                Settings.OverwatchPath = res;                
            }
        }

        private void UpdatePtrPath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Exe Files (.exe)|*.exe";

            //Show the dialog to the user
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string res = fileDialog.FileName;
                Settings.PtrPath = res;
            }
        }
    }
}
