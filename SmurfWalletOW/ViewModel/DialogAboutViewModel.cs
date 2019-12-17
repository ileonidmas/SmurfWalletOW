using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public DialogAboutViewModel()
        {
            Title = "About";
            _redirectCommand = new RelayCommand<Uri>((parameter) => Redirect(parameter));
        }

        private void Redirect(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
    }
}
