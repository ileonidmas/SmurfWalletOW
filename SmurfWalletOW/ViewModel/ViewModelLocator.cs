using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Service;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                
            }
            else
            {
                SimpleIoc.Default.Register<IFileService, FileService>();
                SimpleIoc.Default.Register<IAppSettingsService, AppSettingsService>();
                SimpleIoc.Default.Register<IEncryptionService, EncryptionService>();
                SimpleIoc.Default.Register<IDialogService, DialogService>();
                SimpleIoc.Default.Register<IOverwatchService, OverwatchService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListViewModel>();

            SimpleIoc.Default.Register<DialogYesNoViewModel>();
            SimpleIoc.Default.Register<DialogEncryptionKeyViewModel>();
            SimpleIoc.Default.Register<DialogSettingsViewModel>();
            SimpleIoc.Default.Register<DialogAccountViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public AccountListViewModel AccountListViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AccountListViewModel>(); }
        }
        
        public DialogYesNoViewModel DialogYesNoViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DialogYesNoViewModel>(); }
        }

        public DialogAccountViewModel DialogAccountViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DialogAccountViewModel>(); }
        }

        public DialogSettingsViewModel DialogSettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DialogSettingsViewModel>(); }
        }

        public DialogEncryptionKeyViewModel DialogEncryptionKeyViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DialogEncryptionKeyViewModel>(); }
        }
        public DialogAboutViewModel DialogAboutViewModel
        {
            get { return ServiceLocator.Current.GetInstance<DialogAboutViewModel>(); }
        }

    }
}
