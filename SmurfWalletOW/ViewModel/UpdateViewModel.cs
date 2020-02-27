using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmurfWalletOW.Enums.DialogResults;

namespace SmurfWalletOW.ViewModel
{


    public class UpdateViewModel : ViewModelBase
    {


        private readonly IUpdateService _updateService;
        private readonly IDialogService _dialogService;


        private RelayCommand<object> _updateCommand;
        bool _newVersionAvaiable;

        public RelayCommand<object> UpdateCommand
        {
            get => _updateCommand;
            set => Set(ref _updateCommand, value);
        }

        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
            get { return _loadCommand; }
            set { _loadCommand = value; }
        }


        public bool NewVersionAvaiable
        {
            get => _newVersionAvaiable;
            set => Set(ref _newVersionAvaiable, value);
        }

        public UpdateViewModel(IUpdateService updateService, IDialogService dialogService)
        {
            _updateService = updateService;
            _dialogService = dialogService;
            _updateCommand = new RelayCommand<object>((parameter) => Update(parameter));
            _loadCommand = new RelayCommand(Load);
        }

        private async void Load()
        {
            NewVersionAvaiable = await _updateService.NewVersionAvaialbeAsync();
        }

        private async void Update(object parameter)
        {
            DialogResult result = _dialogService.ShowDialog(DialogsEnum.DialogYesNo, parameter as Window, "Are you sure you want to update the software right now?");
            if (result == DialogResult.Yes)
            {
                await _updateService.UpdateAsync();
                Application.Current.Shutdown();
            }
        }

    }
}
