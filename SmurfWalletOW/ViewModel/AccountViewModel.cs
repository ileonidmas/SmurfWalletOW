using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {

        private readonly IOverwatchApiService _overwatchApiService;
        private Account _account;
        private int _tankSr;
        private int _dpsSr;
        private int _healSr;
        private RelayCommand _loadCommand;

        public Account Account
        {
            get => _account;
            set => Set(ref _account, value);
        }
        public int TankSr
        {
            get => _tankSr;
            set => Set(ref _tankSr, value);
        }
        public int DpsSr
        {
            get => _dpsSr;
            set => Set(ref _dpsSr, value);
        }
        public int HealSr
        {
            get => _healSr;
            set => Set(ref _healSr, value);
        }

        public RelayCommand LoadCommand
        {
            get => _loadCommand;
            set => Set(ref _loadCommand, value);
        }

        public AccountViewModel(IOverwatchApiService overwatchApiService)
        {
            _overwatchApiService = overwatchApiService;
            _loadCommand = new RelayCommand(Load);
        }

        private async void Load()
        {
            var profile = await _overwatchApiService.GetProfileAsync(Account.Btag);
            if (profile != null)
            {
                TankSr = profile.ratings[0].level;
                DpsSr = profile.ratings[1].level;
                HealSr = profile.ratings[2].level;
            }

        }        
        


    }
}
