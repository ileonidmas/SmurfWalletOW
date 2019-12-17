using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public class DialogEncryptionKeyViewModel : DialogViewModelBase
    {
        private SecureString _key;

        private RelayCommand<object[]> _setCommand;

        private RelayCommand<Window> _cancelCommand;
        public RelayCommand<object[]> SetCommand
        {
            get => _setCommand;
            set => Set(ref _setCommand, value);
        }

        public RelayCommand<Window> CancelCommand
        {
            get => _cancelCommand;
            set => Set(ref _cancelCommand, value);
        }

        public DialogEncryptionKeyViewModel()
        {
            Title = "Encryption key";
            _setCommand = new RelayCommand<object[]>((w) => OnSetClicked(w));
            _cancelCommand = new RelayCommand<Window>((w) => OnCancelClicked(w));
        }

        private void OnSetClicked(object[] parameters)
        {
            var master = (parameters[0] as PasswordBox).SecurePassword;
            MessengerInstance.Send(new SetEncryptionMessage(master));
            this.CloseDialogWithResult(parameters[1] as Window, DialogResult.Yes);
        }

        private void OnCancelClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}
