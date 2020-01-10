using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.ViewModel
{
    public class DialogAboutViewModel : DialogViewModelBase
    {
        private RelayCommand<Uri> _redirectCommand;

        public RelayCommand<Uri> RedirectCommand
        {
            get => _redirectCommand;
            set => Set(ref _redirectCommand, value);
        }

        string _appVersion;

        public string AppVersion
        {
            get => _appVersion;
            set => Set(ref _appVersion, value);
        }

        public DialogAboutViewModel()
        {
            Title = "About";
            SetVersion();
            _redirectCommand = new RelayCommand<Uri>((parameter) => Redirect(parameter));
        }

        private void Redirect(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
        private void SetVersion()
        {
            try
            {
                _appVersion ="Version: " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch (Exception)
            {
                _appVersion = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
    }
}
