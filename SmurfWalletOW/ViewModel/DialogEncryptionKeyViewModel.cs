using GalaSoft.MvvmLight.Command;
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
        public SecureString Key
        {
            get => _key;
            set => Set(ref _key, value);
        }
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

        public DialogEncryptionKeyViewModel(SecureString key)
        {
            Title = "Encryption key";
            _setCommand = new RelayCommand<object[]>((w) => OnSetClicked(w));
            _cancelCommand = new RelayCommand<Window>((w) => OnCancelClicked(w));
            _key = key;
        }

        private void OnSetClicked(object[] parameters)
        {
            var master = (parameters[0] as PasswordBox).SecurePassword;
            var valuePtr = Marshal.SecureStringToGlobalAllocUnicode(master);

            for (int i = 0; i < master.Length; i++)
            {
               Key.AppendChar(Convert.ToChar(Marshal.ReadInt16(valuePtr, i * 2)));
            }


            this.CloseDialogWithResult(parameters[1] as Window, DialogResult.Yes);
        }

        private void OnCancelClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}
