using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Factory.Interface;
using SmurfWalletOW.Service.Interface;
using SmurfWalletOW.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Factory
{
    public class DialogFactory : IDialogFactory
    {

        public DialogFactory()
        {

            SimpleIoc.Default.Register<DialogAboutViewModel>();
            SimpleIoc.Default.Register<DialogAccountViewModel>();
            SimpleIoc.Default.Register<DialogEncryptionKeyViewModel>();
            SimpleIoc.Default.Register<DialogSettingsViewModel>();
            SimpleIoc.Default.Register<DialogYesNoViewModel>();
        }
        public ViewModelBase Get(DialogsEnum dialogs)
        {
            switch (dialogs)
            {
                case DialogsEnum.DialogsAbout:
                    return SimpleIoc.Default.GetInstance<DialogAboutViewModel>();
                case DialogsEnum.DialogAccountView:
                    return SimpleIoc.Default.GetInstance<DialogAccountViewModel>();
                case DialogsEnum.DialogEncryptionKey:
                    return SimpleIoc.Default.GetInstance<DialogEncryptionKeyViewModel>();
                case DialogsEnum.DialogSettings:
                    return SimpleIoc.Default.GetInstance<DialogSettingsViewModel>();
                case DialogsEnum.DialogYesNo:
                    return SimpleIoc.Default.GetInstance<DialogYesNoViewModel>();
            }
            return null;
        }     
    }
}
