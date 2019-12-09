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
    public class DialogYesNoViewModel : DialogViewModelBase
    {
        private RelayCommand<Window> yesCommand = null;
        public RelayCommand<Window> YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private RelayCommand<Window> noCommand = null;
        public RelayCommand<Window> NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        public DialogYesNoViewModel(string message) : base(message)
        {
            this.yesCommand = new RelayCommand<Window>((w) => OnYesClicked(w));
            this.noCommand = new RelayCommand<Window>((w) => OnNoClicked(w));
        }

        private void OnYesClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.Yes);
        }

        private void OnNoClicked(Window window)
        {
            this.CloseDialogWithResult(window, DialogResult.No);
        }
    }
}
