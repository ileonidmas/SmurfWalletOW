using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class DialogAboutViewModel : DialogViewModelBase
    {
        private RelayCommand<Uri> _redirectCommand;


        private readonly IUpdateService _updateService;
        private readonly IDialogService _dialogService;

        public RelayCommand<Uri> RedirectCommand
        {
            get => _redirectCommand;
            set => Set(ref _redirectCommand, value);
        }

        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
            get { return _loadCommand; }
            set { _loadCommand = value; }
        }


        Version _appVersion;

        public Version AppVersion
        {
            get => _appVersion;
            set => Set(ref _appVersion, value);
        }

       

        public DialogAboutViewModel(IUpdateService updateService, IDialogService dialogService)
        {
            Title = "About";

            _updateService = updateService;
            _dialogService = dialogService;
            _redirectCommand = new RelayCommand<Uri>((parameter) => Redirect(parameter));
            _loadCommand = new RelayCommand(Load);
        }

        private async void Load()
        {
            AppVersion = await _updateService.GetCurrentVersionAsync();          
        }              

        private void Redirect(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }

    }
}
