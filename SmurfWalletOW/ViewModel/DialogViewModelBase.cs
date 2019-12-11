using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        private string _title;
        private DialogResult _userDialogResult;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        public DialogResult UserDialogResult
        {
            get => _userDialogResult;
            set => Set(ref _userDialogResult, value);
        }

        public void CloseDialogWithResult(Window dialog, DialogResult result)
        {
            this.UserDialogResult = result;
            if (dialog != null)
                dialog.DialogResult = true;
        }

    }
}
