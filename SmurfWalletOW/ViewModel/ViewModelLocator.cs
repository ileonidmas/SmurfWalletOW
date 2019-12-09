﻿using CommonServiceLocator;
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
                SimpleIoc.Default.Register<IEncryptionService, EncryptionService>();
                SimpleIoc.Default.Register<IDialogService, DialogService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListViewModel>();
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
        public DialogYesNoViewModel DialogYesNoViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogYesNoViewModel>(); }
        }

        public DialogAccountViewModel DialogAccountViewModel
        {
            get { return SimpleIoc.Default.GetInstance<DialogAccountViewModel>(); }
        }
    }
}
