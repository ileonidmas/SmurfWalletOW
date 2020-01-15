using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class DialogNotificationViewModel : DialogViewModelBase
    {
        private string _message;
        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        private RelayCommand<Window> _okayCommand = null;
        public RelayCommand<Window> OkayCommand
        {
            get => _okayCommand;
            set => Set(ref _okayCommand, value);
        }

        public DialogNotificationViewModel()
        {
            Title = "Notification";
            _okayCommand = new RelayCommand<Window>((w) => OnOkayClicked(w));
        }



        private void OnOkayClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}

