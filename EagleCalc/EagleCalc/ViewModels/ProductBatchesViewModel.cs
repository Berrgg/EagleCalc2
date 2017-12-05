using System;
using System.Collections.Generic;
using System.Text;
using EagleCalc.Abstractions;
using EagleCalc.Models;
using EagleCalc.Helpers;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace EagleCalc.ViewModels
{
    public class ProductBatchesViewModel : BaseViewModel
    {
        public ProductBatchesViewModel(ProductInfo productInfo)
        {
            Title = productInfo.ProductionLine;
            ProductInfo = productInfo;

            RefreshBatchListCommand = new Command(async () => await RefreshBatchList());

            MessagingCenter.Subscribe<ScanPageViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await RefreshBatchList();
            });

            RefreshBatchListCommand.Execute(null);

            IsBusy = false;
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ICommand RefreshBatchListCommand { get; }
        public ProductInfo ProductInfo { get; set; }

        ObservableCollection<EagleBatch> batchList = new ObservableCollection<EagleBatch>();
        public ObservableCollection<EagleBatch> BatchList
        {
            get { return batchList; }
            set { SetProperty(ref batchList, value, "BatchList"); }
        }

        EagleBatch selectedItem;
        public EagleBatch SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value, "SelectedItem");
                if(selectedItem != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new Pages.ScanPage(selectedItem));
                    selectedItem = null;
                }
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
                var list = await table.ReadBatchWeightAverage(DateTime.Today, ProductInfo.ProductionLine, ProductInfo.ProdCode);

                BatchList.Clear();

                foreach (var item in list)
                    BatchList.Add(item);

                BatchList = new ObservableCollection<EagleBatch>(BatchList.GroupBy(x => x.IdBatch)
                                                                        .Select(group => new EagleBatch
                                                                        {
                                                                            IdBatch = group.Key,
                                                                            Weight = group.Sum(i => i.Weight),
                                                                            TrayCl = group.Sum(i => i.Weight * i.TrayCl) / group.Sum(i => i.Weight)
                                                                        }));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Batches not loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
