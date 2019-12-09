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
                //SimpleIoc.Default.Register<IFileSerivce, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IFileService, FileService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListViewModel>();
            SimpleIoc.Default.Register<AccountItemViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }
        }

        public AccountListViewModel AccountListViewModel
        {
            get { return SimpleIoc.Default.GetInstance<AccountListViewModel>(); }
        }

        public AccountItemViewModel AccountItemViewModel
        {
            get { return SimpleIoc.Default.GetInstance<AccountItemViewModel>(); }
        }
    }
}
