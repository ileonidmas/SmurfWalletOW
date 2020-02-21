using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.ViewModel
{
    public class RegionViewModel : ViewModelBase
    {

        private readonly IRegionService _regionService;
        private RelayCommand _loadCommand;
        private RelayCommand _changeRegionCommand;
        private Region _region;


        public IEnumerable<Region> Regions
        {
            get
            {
                return Enum.GetValues(typeof(Region)).Cast<Region>();
            }
        }

        public Region SelectedRegion
        {
            get => _region;
            set => Set(ref _region, value);
        }

        public RelayCommand LoadCommand
        {
            get => _loadCommand;
            set => Set(ref _loadCommand, value);
        }

        public RelayCommand ChangeRegionCommand
        {
            get => _changeRegionCommand;
            set => Set(ref _changeRegionCommand, value);
        }


        public RegionViewModel(IRegionService regionService)
        {
            _regionService = regionService;          
            _loadCommand = new RelayCommand(Load);
            _changeRegionCommand = new RelayCommand(ChangeRegion);
        }

        private async void Load()
        {
            SelectedRegion = await _regionService.GetRegionAsync();           
        }

        private async void ChangeRegion()
        {
            await _regionService.SetRegionAsync(SelectedRegion);
        }


    }
}
