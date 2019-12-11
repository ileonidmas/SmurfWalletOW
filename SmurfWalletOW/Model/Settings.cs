﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Model
{
    public class Settings : ModelBase
    {
        private string _overwatchPath;
        private int _loadingTime;
        public Settings()
        {
        }

        public string OverwatchPath
        {
            get => _overwatchPath;
            set => Set(ref _overwatchPath, value);
        }

        public int LoadingTime
        {
            get => _loadingTime;
            set => Set(ref _loadingTime, value);
        }
    }
}
