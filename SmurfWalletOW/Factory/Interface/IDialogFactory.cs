using GalaSoft.MvvmLight;
using SmurfWalletOW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Factory.Interface
{
    public interface IDialogFactory : IFactory<DialogsEnum>
    {
        new ViewModelBase Get(DialogsEnum dialogs, string message);
    }
}
