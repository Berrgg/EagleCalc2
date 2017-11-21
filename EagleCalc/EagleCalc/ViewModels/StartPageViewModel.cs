using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using EagleCalc.Abstractions;
using EagleCalc.Helpers;
using EagleCalc.Models;
using Xamarin.Forms;

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
        public List<string> LineList;

        async Task TakeLineList()
        {
            if (IsBusy)
                return;

            try
            {
                var table = await CloudService.GetTableAsync<Line>();
                var list = await table.ReadAllItemsAsync();

                foreach (var item in list)
                {
                    LineList.Add(item.LineName);
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
