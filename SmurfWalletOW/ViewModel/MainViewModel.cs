using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmurfWalletOW.Enums;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Interop;

namespace SmurfWalletOW.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IDialogService _dialogService;
        private readonly IOverwatchService _overwatchService;
        private readonly IDiscordService _discordService;
        private readonly IFileService _fileService;

        private RelayCommand<object> _openSettingsCommand;
        private RelayCommand<object> _openAboutCommand;
        private RelayCommand<object> _hookCommand;
        private RelayCommand<object[]> _closeCommand;

        private bool _hooked = false;
        private bool _enabled = true;

        public RelayCommand<object> OpenSettingsCommand
        {
            get => _openSettingsCommand;
            set => Set(ref _openSettingsCommand, value);
        }

        public RelayCommand<object> OpenAboutCommand
        {
            get => _openAboutCommand;
            set => Set(ref _openAboutCommand, value);
        }

        public RelayCommand<object> HookCommand
        {
            get => _hookCommand;
            set => Set(ref _hookCommand, value);
        }

        public RelayCommand<object[]> CloseCommand
        {
            get => _closeCommand;
            set => Set(ref _closeCommand, value);
        }

        public bool Hooked
        {
            get => _hooked;
            set => Set(ref _hooked, value);
        }

        public bool Enabled
        {
            get => _enabled;
            set => Set(ref _enabled, value);
        }

        public MainViewModel(IDialogService dialogService, IOverwatchService overwatchService, IDiscordService discordService, IFileService fileService)
        {
            _dialogService = dialogService;
            _overwatchService = overwatchService;
            _discordService = discordService;
            _fileService = fileService;

            _openSettingsCommand = new RelayCommand<object>((parameter) => OpenSettings(parameter));
            _openAboutCommand = new RelayCommand<object>((parameter) => OpenAbout(parameter));
            _hookCommand = new RelayCommand<object>((parameter) => Hook(parameter));
            _closeCommand = new RelayCommand<object[]>((parameter) => Close(parameter));

        }


        private void OpenSettings(object window)
        {
            _dialogService.ShowDialog(DialogsEnum.DialogSettings, window as Window);
        }


        private void OpenAbout(object window)
        {
            _dialogService.ShowDialog(DialogsEnum.DialogsAbout, window as Window);
        }

        private async void Hook(object parameter)
        {
            Enabled = false;
            var window = parameter as Window;
            HwndSource source = PresentationSource.FromVisual(window) as HwndSource;
            var settings = await _fileService.GetSettingsAsync();
            if (!_hooked)
            {                
                await _discordService.Start();
                if (await _discordService.DoesUserExists(settings.DiscordUsername))
                {

                    //inject dll to OW
                    if (_overwatchService.Hook())
                    {
                        //hook callback to Main window
                        source.AddHook(WndProc);
                        await _discordService.SendMessageAsync(settings.DiscordUsername, "Notification alert has been set!");
                        Hooked = !_hooked;
                    }
                    else
                    {
                        _dialogService.ShowDialog(DialogsEnum.DialogNotification, window, "Overwatch app was not running!");
                        await _discordService.Stop();
                    }

                }
                else
                {
                    _dialogService.ShowDialog(DialogsEnum.DialogNotification, window, "User is not connected to discord notification server!");
                    await _discordService.Stop();
                }

            }
            else
            {
                source.RemoveHook(WndProc);
                _overwatchService.UnHook();
                if (_discordService.GetSendMessageFlag())
                    await _discordService.SendMessageAsync(settings.DiscordUsername, "You've got a notification in Overwatch!");
                else
                    await _discordService.SendMessageAsync(settings.DiscordUsername, "Notification alert has been removed!");
                await _discordService.Stop();
                Hooked = !_hooked;
            }

            Enabled = true;
        }
                

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0X400 && Hooked)
            {
                _discordService.SetSendMessage(true);
                HookCommand.Execute((Window)HwndSource.FromHwnd(hwnd).RootVisual);
            }
            if (msg == 0X401 && Hooked)
            {
                HookCommand.Execute((Window)HwndSource.FromHwnd(hwnd).RootVisual);
            }

            return IntPtr.Zero;
        }



        private async void Close(object[] parameters)
        {
            Window window = parameters[0] as Window;
            CancelEventArgs cea = parameters[1] as CancelEventArgs;
            
            if (Hooked)
            {
                cea.Cancel = true;
                HwndSource source = PresentationSource.FromVisual(window) as HwndSource;
                source.RemoveHook(WndProc);
                var settings = await _fileService.GetSettingsAsync();
                _overwatchService.UnHook();
                await _discordService.SendMessageAsync(settings.DiscordUsername, "Notification alert has been removed!");
                await _discordService.Stop();
                Hooked = !_hooked;
                window.Close();
            }
        }


    }
}
