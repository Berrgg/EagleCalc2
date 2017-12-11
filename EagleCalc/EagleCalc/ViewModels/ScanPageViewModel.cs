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
            SaveItemCommand = new Command(async () => await SaveItemAsync());
            RemoveLastItemCommand = new Command(async () => await RemoveLastItem());

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
        public ICommand SaveItemCommand { get; }
        public ICommand RemoveLastItemCommand { get; }

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
            set { SetProperty(ref scanList, value, "ScanList"); }
        }

        private string _scanText = string.Empty;
        public string ScanText
        {
            get { return _scanText; }
            set { SetProperty(ref _scanText, value, "ScanText"); }
        }

        private bool buttonCancelEnabled = false;
        public bool ButtonCancelEnabled
        {
            get { return buttonCancelEnabled; }
            set { SetProperty(ref buttonCancelEnabled, value, "ButtonCancelEnabled"); }
        }

        private bool buttonConfirmEnabled = false;
        public bool ButtonConfirmEnabled
        {
            get { return buttonConfirmEnabled; }
            set { SetProperty(ref buttonConfirmEnabled, value, "ButtonConfirmEnabled"); }
        }

        public bool IsScanned { get; set; } = false;

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

                CalculateWeightedAverage();
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
                    CalculateWeightedAverage();
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

                if (IsPalletScanned(ScanList, barCodeSplit.TrayId))
                {
                    await Application.Current.MainPage.DisplayAlert("Scanner", "Pallet already scanned.", "OK");
                    IsScanned = true;
                    ButtonCancelEnabled = false;
                    ButtonConfirmEnabled = true;
                }
                else
                {
                    var newItem = new EagleBatch
                    {
                        IdBatch = IdBatch,
                        ProductCode = ProductInfo.ProdCode,
                        Line = ProductInfo.ProductionLine,
                        TrayId = barCodeSplit.TrayId,
                        PluCode = barCodeSplit.PluCode,
                        Weight = barCodeSplit.Weight,
                        TrayCl = barCodeSplit.TrayCl,
                        ProductionDate = DateTimeOffset.Now.Date,
                        IsPrinted = false
                    };

                    ScanList.Add(newItem);
                    CurrentBatch = newItem;
                    CalculateWeightedAverage();
                    ButtonCancelEnabled = true;
                    ButtonConfirmEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Scan pallet failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task RemoveLastItem()
        {
            try
            {
                if (ScanList.Count > 0)
                    ScanList.RemoveAt(ScanList.Count - 1);

                CalculateWeightedAverage();

                ButtonCancelEnabled = false;
                ButtonConfirmEnabled = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Cancel last scan failed.", ex.Message, "OK");
            }
        }

        async Task SaveItemAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = CloudService.GetTable<EagleBatch>();
                await table.UpsertItemAsync(CurrentBatch);
                MessagingCenter.Send<ScanPageViewModel>(this, "ItemsChanged");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Save into database failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsPalletScanned(ObservableCollection<EagleBatch> batchList, string trayId)
        {
            ObservableCollection<EagleBatch> list = new ObservableCollection<EagleBatch>(batchList.Where(x => x.TrayId == trayId));

            if (list.Count >= 1)
                return true;
            else
                return false;
        }
    }
}
