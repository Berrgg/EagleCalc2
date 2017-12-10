using System;
using System.Collections.Generic;
using System.Text;
using EagleCalc.Abstractions;
using EagleCalc.Models;
using EagleCalc.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;

namespace EagleCalc.ViewModels
{
    public class ScanPageViewModel :BaseViewModel
    {
        public ScanPageViewModel(EagleBatch item = null, ProductInfo productInfo = null)
        {
            RefreshBatchListCommand = new Command(async () => await RefreshBatchList());
            DeleteItemCommand = new Command(async () => await DeleteItemAsync());
            AddItemCommand = new Command(async () => await AddItem());

            ProductInfo = productInfo;

            if(item != null)
                IdBatch = item.IdBatch;
            else
                IdBatch = productInfo.ProdCode + "/" + DateTime.Now.ToString("ddMMyyHHmmss");

            Title = IdBatch;
            RefreshBatchListCommand.Execute(null);

            IsBusy = false;
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ICommand RefreshBatchListCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand AddItemCommand { get; }

        public ProductInfo ProductInfo { get; set; }
        private string IdBatch { get; set; }

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

        private string _scanText = string.Empty;
        public string ScanText
        {
            get { return _scanText; }
            set { SetProperty(ref _scanText, value, "ScanText"); }
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
            var batchItems = new List<EagleBatch>(scanList.GroupBy(x => x.IdBatch)
                                                                    .Select(group => new EagleBatch
                                                                    {
                                                                        IdBatch = group.Key,
                                                                        Weight = group.Sum(i => i.Weight),
                                                                        TrayCl = group.Sum(i => i.Weight * i.TrayCl) / group.Sum(i => i.Weight)
                                                                    }));

            if(batchItems.Count > 0)
            {
                PalletWeight = batchItems[0].Weight.ToString("F2");
                PalletCl = batchItems[0].TrayCl.ToString("F2");
            }
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

        async Task AddItem()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                BarCodeSplit barCodeSplit = new BarCodeSplit(ScanText);

                EagleBatch newItem = new EagleBatch
                {
                    IdBatch = IdBatch,
                    CustomerName = ProductInfo.Customer,
                    Line = ProductInfo.ProductionLine,
                    ProductCode = ProductInfo.ProdCode,
                    TrayId = barCodeSplit.TrayId,
                    PluCode = barCodeSplit.PluCode,
                    Weight = barCodeSplit.Weight,
                    TrayCl = barCodeSplit.TrayCl,
                    ProductionDate = DateTime.Today,
                    IsPrinted = false
                };

                await Task.Run(() => ScanList.Add(newItem));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Scan pallet failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                ScanText = string.Empty;
            }
        }
    }
}
