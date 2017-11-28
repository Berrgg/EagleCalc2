﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using EagleCalc.Abstractions;
using EagleCalc.Helpers;
using EagleCalc.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace EagleCalc.ViewModels
{
    public class StartPageViewModel : BaseViewModel
    {
        public StartPageViewModel()
        {
            Title = "Start page";

            TakeLineListCommand = new Command(async () => await TakeLineList());
            TakeLineListCommand.Execute(null);
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ICommand TakeLineListCommand { get; }

        public string SelectedLine { get; set; }

        bool isPickerEnable;
        public bool IsPickerEnable
        {
            get { return isPickerEnable; }
            set
            {
                if(isPickerEnable != value)
                {
                    isPickerEnable = value;
                    OnPropertyChanged(nameof(IsPickerEnable));
                }
            }
        }

        ObservableCollection<string> lineNames = new ObservableCollection<string>();
        public ObservableCollection<string> LineNames
        {
            get { return lineNames; }
            set { SetProperty(ref lineNames, value, "LineNames"); }
        }

        async Task TakeLineList()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = CloudService.GetTable<Line>();
                var list = await table.ReadAllItemsAsync();

                foreach (var item in list)
                {
                    LineNames.Add(item.LineName);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Lines not loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
