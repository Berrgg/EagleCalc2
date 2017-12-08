using System;
using System.Collections.Generic;
using System.Text;
using EagleCalc.Abstractions;
using EagleCalc.Models;
using EagleCalc.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;

namespace EagleCalc.ViewModels
{
    public class ScanPageViewModel :BaseViewModel
    {
        public ScanPageViewModel(EagleBatch item = null, ProductInfo productInfo = null)
        {
            Title = productInfo.ProdCode + "/" + DateTime.Now.ToString("ddMMyyHHmmss");
            ProductInfo = productInfo;

            RefreshBatchListCommand = new Command(async () => await RefreshBatchList());
            DeleteItemCommand = new Command(async () => await DeleteItemAsync());

            IsBusy = false;
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ICommand RefreshBatchListCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public ProductInfo ProductInfo { get; set; }

        EagleBatch currentBatch;
        public EagleBatch CurrentBatch
        {
            get { return currentBatch; }
            set { SetProperty(ref currentBatch, value, "CurrentBatch"); }
        }

        ObservableCollection<EagleBatch> scanList = new ObservableCollection<EagleBatch>();
        public ObservableCollection<EagleBatch> ScanList
        {
            get { return scanList; }
            set
            {
                SetProperty(ref scanList, value, "ScanList");
                if (scanList.Count > 0)
                    CalculateWeightedAverage();
            }
        }

        string palletWeight;
        public string PalletWeight
        {
            get { return palletWeight; }
            set
            {
                if(palletWeight != value)
                {
                    palletWeight = value;
                    OnPropertyChanged(nameof(PalletWeight));
                }
            }
        }

        string palletCl;
        public string PalletCl
        {
            get { return palletCl; }
            set
            {
                if(palletCl != value)
                {
                    palletCl = value;
                    OnPropertyChanged(nameof(PalletCl));
                }
            }
        }

        private void CalculateWeightedAverage()
        {

        }

        async Task RefreshBatchList()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = CloudService.GetTable<EagleBatch>();
                var list = await table.ReadListOfBatches(DateTime.Today, ProductInfo.ProductionLine, ProductInfo.ProdCode);

                ScanList.Clear();

                foreach (var item in list)
                    ScanList.Add(item);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("List not loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task DeleteItemAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if(CurrentBatch != null)
                {
                    var table = CloudService.GetTable<EagleBatch>();
                    await table.DeleteItemAsync(CurrentBatch);
                    MessagingCenter.Send<ScanPageViewModel>(this, "ItemsChanged");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Delete failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
