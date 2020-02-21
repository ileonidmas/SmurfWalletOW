using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmurfWalletOW.Factory;
using SmurfWalletOW.Factory.Interface;
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
                SimpleIoc.Default.Register<IRegionService, RegionService>();
                SimpleIoc.Default.Register<IFileService, FileService>();
                SimpleIoc.Default.Register<IAppSettingsService, AppSettingsService>();
                SimpleIoc.Default.Register<IEncryptionService, EncryptionService>();
                SimpleIoc.Default.Register<IDialogService, DialogService>();
                SimpleIoc.Default.Register<IOverwatchService, OverwatchService>();
                SimpleIoc.Default.Register<IDiscordService, DiscordService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListViewModel>();
            SimpleIoc.Default.Register<RegionViewModel>();
            SimpleIoc.Default.Register<IDialogFactory, DialogFactory>();

        }

        public MainViewModel MainViewModel
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }
        }

        public AccountListViewModel AccountListViewModel
        {
            get { return SimpleIoc.Default.GetInstance<AccountListViewModel>(); }
        }
        
        public DialogYesNoViewModel DialogYesNoViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogYesNoViewModel>(); }
        }

        public DialogNotificationViewModel DialogNotificationViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogNotificationViewModel>(); }
        }

        public DialogAccountViewModel DialogAccountViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogAccountViewModel>(); }
        }

        public DialogSettingsViewModel DialogSettingsViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogSettingsViewModel>(); }
        }

        public DialogEncryptionKeyViewModel DialogEncryptionKeyViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogEncryptionKeyViewModel>(); }
        }
        public DialogAboutViewModel DialogAboutViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogAboutViewModel>(); }
        }
        public RegionViewModel RegionViewModel
        {
            get { return SimpleIoc.Default.GetInstance<RegionViewModel>(); }
        }

    }
}
