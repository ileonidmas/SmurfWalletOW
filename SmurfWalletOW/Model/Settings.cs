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
        private string _discordUsername;
        private double _loadingTime;
        public Settings()
        {
        }

        public string OverwatchPath
        {
            get => _overwatchPath;
            set => Set(ref _overwatchPath, value);
        }

        public double LoadingTime
        {
            get => _loadingTime;
            set => Set(ref _loadingTime, value);
        }

        public string DiscordUsername
        {
            get => _discordUsername;
            set => Set(ref _discordUsername, value);
        }
    }
}
