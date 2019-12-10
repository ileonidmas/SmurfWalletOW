using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Model
{
    public class Settings : ModelBase
    {
        private string _overwatchPath;

        public Settings()
        {
        }

        public string OverwatchPath
        {
            get => _overwatchPath;
            set => Set(ref _overwatchPath, value);
        }
    }
}
